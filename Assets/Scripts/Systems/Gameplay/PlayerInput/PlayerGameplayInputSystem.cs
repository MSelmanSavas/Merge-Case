using MergeCase.Entities;
using MergeCase.Entities.Unity;
using MergeCase.General.Interfaces;
using MergeCase.Systems.Updater;
using UnityEngine;
using UsefulExtensions.Vector3;

namespace MergeCase.Systems.Gameplay
{
    public class PlayerGameplayInputSystem : GameplaySystemBase, IInitializable<SystemUpdateContext<GameplaySystemBase>>, IUpdateable<SystemUpdateContext<GameplaySystemBase>>
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        IWorldToGridIndexConverter _worldToGridIndexConverter;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        IEntityCollection<ItemEntityQueryData> _itemEntities;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        IEntityCollection<GridEntityQueryData> _gridEntities;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        Camera _mainCamera;

        IEntity _selectedEntity;
        Vector2Int _originalIndex;
        Vector3 _originalPosition;

        public bool TryInitialize(SystemUpdateContext<GameplaySystemBase> data)
        {
            if (!data.SystemUpdater.TryGetGameSystemByType(out _worldToGridIndexConverter))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(IWorldToGridIndexConverter)}! Cannot initialize!");
                return false;
            }

            if (!data.SystemUpdater.TryGetGameSystemByType(out _itemEntities))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(IEntityCollection<ItemEntityQueryData>)}! Cannot initialize!");
                return false;
            }

            if (!data.SystemUpdater.TryGetGameSystemByType(out _gridEntities))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(IEntityCollection<GridEntityQueryData>)}! Cannot initialize!");
                return false;
            }

            _mainCamera = Camera.main;

            return true;
        }

        public bool TryDeInitialize(SystemUpdateContext<GameplaySystemBase> data)
        {
            return true;
        }

        public bool TryUpdate(SystemUpdateContext<GameplaySystemBase> data)
        {
            InputToSelectItem();
            MoveItemIfSelected();
            DropItemOnPosition();

            return true;
        }

        void InputToSelectItem()
        {
            if (_selectedEntity != null)
            {
                return;
            }

            if (!Input.GetKeyDown(KeyCode.Mouse0))
            {
                return;
            }

            var screenPos = Input.mousePosition;
            var worldPos = _mainCamera.ScreenToWorldPoint(screenPos);
            var gridIndex = _worldToGridIndexConverter.GetGridIndex(worldPos);

            UnityLogger.LogWithTag($"screenPos : {screenPos}, worldPos : {worldPos}, gridIndex : {gridIndex}");

            if (!_gridEntities.TryGetEntity(new GridEntityQueryData { Index = gridIndex }, out IEntity gridEntity))
            {
                UnityLogger.LogWarningWithTag($"No grid found on index : {gridIndex}");
                return;
            }

            if (!_itemEntities.TryGetEntity(new ItemEntityQueryData { Index = gridIndex }, out IEntity itemEntity))
            {
                UnityLogger.LogWarningWithTag($"No item found on index : {gridIndex}");
                return;
            }

            _originalIndex = gridIndex;

            if (itemEntity.TryGetEntityComponent(out GameObjectComponent gameObjectComponent))
            {
                _originalPosition = gameObjectComponent.GetGameObject().transform.position;
            }

            _selectedEntity = itemEntity;

            UnityLogger.LogWithTag($"Item found on {gridIndex}! Found item : {itemEntity}");
        }

        void MoveItemIfSelected()
        {
            if (!Input.GetKey(KeyCode.Mouse0))
            {
                return;
            }

            if (_selectedEntity == null)
            {
                return;
            }

            if (!_selectedEntity.TryGetEntityComponent(out GameObjectComponent gameObjectComponent))
            {
                return;
            }

            var screenPos = Input.mousePosition;
            var worldPos = _mainCamera.ScreenToWorldPoint(screenPos);

            var entityTransform = gameObjectComponent.GetGameObject().transform;

            entityTransform.position = worldPos.WithZ(entityTransform.position.z);
        }

        void DropItemOnPosition()
        {
            if (!Input.GetKeyUp(KeyCode.Mouse0))
            {
                return;
            }

            if (_selectedEntity == null)
            {
                return;
            }

            if (!_selectedEntity.TryGetEntityComponent(out GameObjectComponent selectedEntityGameObjectComponent))
            {
                return;
            }

            var screenPos = Input.mousePosition;
            var worldPos = _mainCamera.ScreenToWorldPoint(screenPos);
            var gridIndex = _worldToGridIndexConverter.GetGridIndex(worldPos);

            if (!_gridEntities.TryGetEntity(new GridEntityQueryData { Index = gridIndex }, out IEntity gridEntity))
            {
                UnityLogger.LogWarningWithTag($"No grid found on index : {gridIndex}");
                ReturnItemToOriginalIndex();
                ResetInternalValues();
                return;
            }

            if (_itemEntities.TryGetEntity(new ItemEntityQueryData { Index = gridIndex }, out IEntity itemEntity))
            {
                UnityLogger.LogWarningWithTag($"Item is already at index : {gridIndex}! Cannot place selected item there!");
                ReturnItemToOriginalIndex();
                ResetInternalValues();
                return;
            }

            gridEntity.TryGetEntityComponent(out GameObjectComponent gridGameObjectComponent);

            var itemEntityTransform = selectedEntityGameObjectComponent.GetGameObject().transform;
            itemEntityTransform.position = gridGameObjectComponent.GetGameObject().transform.position.WithZ(itemEntityTransform.position.z);


            _selectedEntity.TryGetEntityComponent(out IndexComponent selectedEntityIndexComponent);

            var itemPreviousIndex = selectedEntityIndexComponent.GetIndex();

            _itemEntities.TryRemoveEntity(new ItemEntityQueryData { Index = itemPreviousIndex });
            _itemEntities.TryAddEntity(new ItemEntityQueryData { Index = gridIndex }, _selectedEntity);

            selectedEntityIndexComponent.SetIndex(gridIndex);

            ResetInternalValues();
        }

        void ResetInternalValues()
        {
            _selectedEntity = null;
            _originalIndex = new Vector2Int(-1, -1);
            _originalPosition = new Vector3(-1000, -1000);
        }

        void ReturnItemToOriginalIndex()
        {
            if (!_selectedEntity.TryGetEntityComponent(out GameObjectComponent gameObjectComponent))
            {
                return;
            }

            gameObjectComponent.GetGameObject().transform.position = _originalPosition;
        }
    }
}


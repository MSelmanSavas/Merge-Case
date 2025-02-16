using System.Collections;
using System.Collections.Generic;
using MergeCase.Entities;
using MergeCase.Entities.Components.Unity;
using MergeCase.General.Config;
using MergeCase.General.Config.Gameplay;
using MergeCase.General.Interfaces;
using MergeCase.Systems.Updater;
using UnityEngine;

namespace MergeCase.Systems.Gameplay
{
    public class GameplayItemsSystem : GameplaySystemBase, IInitializable<SystemUpdateContext<GameplaySystemBase>>, IEntityCollection<ItemEntityQueryData>, IWorldToGridIndexConverter<ItemEntityQueryData>, IGridIndexToWorldConverter<ItemEntityQueryData>
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        Dictionary<ItemEntityQueryData, IEntity> _gridEntities = new();

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        GameplayGridsConfigs _gameplayGridConfigs;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        GameplayItemsConfigs _gameplayItemsConfigs;

        Transform _itemsParent;

        public bool TryInitialize(SystemUpdateContext<GameplaySystemBase> data)
        {
            if (!data.DataCollection.TryGet(out ConfigProvider configProvider))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(ConfigProvider)}! Cannot initialize!");
                return false;
            }

            if (!configProvider.TryGet(out _gameplayGridConfigs))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(GameplayGridsConfigs)} as config! Cannot initialize!");
                return false;
            }

            if (!configProvider.TryGet(out _gameplayItemsConfigs))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(GameplayItemsConfigs)} as config! Cannot initialize!");
                return false;
            }

            _itemsParent = new GameObject().transform;
            _itemsParent.gameObject.name = "Items";

            _gridEntities ??= new();
            _gridEntities.Clear();

            return true;
        }


        public bool TryDeInitialize(SystemUpdateContext<GameplaySystemBase> data)
        {
            _gridEntities.Clear();
            GameObject.Destroy(_itemsParent);
            return true;
        }

        public bool TryAddEntity(ItemEntityQueryData entityQueryData, IEntity entity)
        {
            if (_gridEntities.ContainsKey(entityQueryData))
            {
                return false;
            }

            _gridEntities.Add(entityQueryData, entity);

            if (entity.TryGetEntityComponent(out GameObjectComponent gameObjectComponent))
            {
                var transform = gameObjectComponent.GetGameObject().transform;
                transform.SetParent(_itemsParent, worldPositionStays: true);
            }

            return true;
        }

        public bool TryRemoveEntity(ItemEntityQueryData entityQueryData)
        {
            if (!_gridEntities.TryGetValue(entityQueryData, out IEntity entity))
            {
                return false;
            }

            _gridEntities.Remove(entityQueryData);

            if (entity.TryGetEntityComponent(out GameObjectComponent gameObjectComponent))
            {
                var transform = gameObjectComponent.GetGameObject().transform;
                transform.SetParent(null);
            }

            return true;
        }

        public bool TryGetEntity(ItemEntityQueryData entityQueryData, out IEntity entity)
        {
            return _gridEntities.TryGetValue(entityQueryData, out entity);
        }

        public Vector2Int GetGridIndex(Vector3 worldPos)
        {
            worldPos -= _gameplayGridConfigs.StartPositionOffset;

            var gridSize = _gameplayGridConfigs.GridSize;
            var gridSizeHalf = gridSize / 2f;

            worldPos -= (Vector3)gridSizeHalf;

            var xHalf = ((worldPos.x + gridSizeHalf.x) / gridSizeHalf.x);
            var yHalf = ((worldPos.y + gridSizeHalf.y) / gridSizeHalf.y);

            int x = Mathf.RoundToInt((-xHalf + yHalf) * -0.5f);
            int y = Mathf.RoundToInt((xHalf + yHalf) * 0.5f);

            return new Vector2Int(x, y);
        }

        public Vector3 GetWorldPos(Vector2Int gridIndex)
        {
            var gridSize = _gameplayGridConfigs.GridSize;
            var gridSizeHalf = gridSize / 2f;
            var worldPos = Vector3.zero;

            worldPos.x = ((gridIndex.x * gridSizeHalf.x) + (gridIndex.y * gridSizeHalf.x));
            worldPos.y = (-(gridIndex.x * gridSizeHalf.y) + (gridIndex.y * gridSizeHalf.y));

            worldPos += _gameplayGridConfigs.StartPositionOffset;
            worldPos += _gameplayItemsConfigs.PositionOffset;

            return worldPos;
        }
    }
}

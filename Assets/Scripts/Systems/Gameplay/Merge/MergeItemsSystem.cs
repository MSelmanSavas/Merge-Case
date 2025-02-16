using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MergeCase.Entities;
using MergeCase.Entities.Components.Common;
using MergeCase.Entities.Components.Items;
using MergeCase.Entities.Components.Unity;
using MergeCase.General.Config;
using MergeCase.General.Config.Gameplay;
using MergeCase.General.Interfaces;
using MergeCase.Systems.Updater;
using UnityEngine;
using UnityEngine.Pool;

namespace MergeCase.Systems.Gameplay
{
    public class MergeItemsSystem : GameplaySystemBase, IInitializable<SystemUpdateContext<GameplaySystemBase>>, IUpdateable<SystemUpdateContext<GameplaySystemBase>>
    {

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        IEntityCollection<ItemEntityQueryData> _itemEntities;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        MergeItemsConfigs _mergeItemsConfigs;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        GameplayGridsConfigs _gameplayGridsConfigs;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        IGridIndexToWorldConverter<ItemEntityQueryData> _gridToWorldConverter;

        Queue<Vector2Int> _searchQueue = new();
        bool[] _checkedGrids;

        public bool TryInitialize(SystemUpdateContext<GameplaySystemBase> data)
        {
            if (!data.SystemUpdater.TryGetGameSystemByType(out _itemEntities))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(IEntityCollection<ItemEntityQueryData>)}! Cannot initialize!");
                return false;
            }

            if (!data.SystemUpdater.TryGetGameSystemByType(out _gridToWorldConverter))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(IGridIndexToWorldConverter<ItemEntityQueryData>)}! Cannot initialize!");
                return false;
            }

            if (!data.DataCollection.TryGet(out ConfigProvider configProvider))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(ConfigProvider)}! Cannot initialize!");
                return false;
            }

            if (!configProvider.TryGet(out _mergeItemsConfigs))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(MergeItemsConfigs)} as config! Cannot initialize!");
                return false;
            }

            if (!configProvider.TryGet(out _gameplayGridsConfigs))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(GameplayGridsConfigs)} as config! Cannot initialize!");
                return false;
            }

            var gridSize = _gameplayGridsConfigs.TotalGridSize;
            _checkedGrids = new bool[gridSize.x * gridSize.y];

            return true;
        }

        public bool TryDeInitialize(SystemUpdateContext<GameplaySystemBase> data)
        {
            return true;
        }

        public bool TryUpdate(SystemUpdateContext<GameplaySystemBase> data)
        {
            var gridSize = _gameplayGridsConfigs.TotalGridSize;

            List<IEntity> _similarEntities = ListPool<IEntity>.Get();

            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    Vector2Int gridIndex = new Vector2Int(x, y);
                    int gridCheckIndex = x + (y * gridSize.y);

                    if (_checkedGrids[gridCheckIndex])
                    {
                        continue;
                    }

                    if (!_itemEntities.TryGetEntity(new ItemEntityQueryData { Index = gridIndex }, out IEntity entity))
                    {
                        continue;
                    }

                    if (!entity.TryGetEntityComponent(out ItemTypeComponent itemTypeComponent))
                    {
                        continue;
                    }

                    _similarEntities.Clear();
                    FloodFillSearchSimilarItems(gridIndex, itemTypeComponent, gridSize, _similarEntities);

                    if (!_mergeItemsConfigs.TryGetMergeItemData(itemTypeComponent.Type, _similarEntities.Count, out MergeItemData mergeItemData))
                    {
                        continue;
                    }

                    MergeItems(_similarEntities, mergeItemData);
                }
            }

            ListPool<IEntity>.Release(_similarEntities);
            ResetCheckIndices();
            return true;
        }

        void FloodFillSearchSimilarItems(Vector2Int startIndex, ItemTypeComponent itemTypeComponent, Vector2Int gridSize, List<IEntity> similarEntities)
        {
            _searchQueue.Enqueue(startIndex);

            while (_searchQueue.Count > 0)
            {
                Vector2Int checkIndex = _searchQueue.Dequeue();

                if (checkIndex.x < 0 || checkIndex.x >= gridSize.x || checkIndex.y < 0 || checkIndex.y >= gridSize.y)
                {
                    continue;
                }

                int gridCheckIndex = checkIndex.x + (checkIndex.y * gridSize.y);

                if (_checkedGrids[gridCheckIndex])
                {
                    continue;
                }

                if (!_itemEntities.TryGetEntity(new ItemEntityQueryData { Index = checkIndex }, out IEntity entity))
                {
                    continue;
                }

                if (!entity.TryGetEntityComponent(out ItemTypeComponent foundItemTypeComponent))
                {
                    continue;
                }

                if (!itemTypeComponent.IsSame(foundItemTypeComponent))
                {
                    continue;
                }

                similarEntities.Add(entity);
                _checkedGrids[gridCheckIndex] = true;

                foreach (var cardinalVector in DirectionUtils.CardinalVector2Ints)
                {
                    var offsetIndex = checkIndex + cardinalVector;

                    if (offsetIndex.x < 0 || offsetIndex.x >= gridSize.x || offsetIndex.y < 0 || offsetIndex.y >= gridSize.y)
                    {
                        continue;
                    }

                    int offsetCheckIndex = offsetIndex.x + (offsetIndex.y * gridSize.y);

                    if (_checkedGrids[offsetCheckIndex])
                    {
                        continue;
                    }

                    _searchQueue.Enqueue(offsetIndex);
                }
            }
        }

        void ResetCheckIndices()
        {
            for (int i = 0; i < _checkedGrids.Length; i++)
            {
                _checkedGrids[i] = false;
            }
        }

        void MergeItems(List<IEntity> entitiesToMerge, MergeItemData mergeData)
        {
            bool hasSpawnIndexSet = false;
            Vector2Int spawnIndex = Vector2Int.zero;

            bool hasMergePositionSet = false;
            Vector3 mergePosition = Vector3.zero;

            Sequence sequence = DOTween.Sequence();

            foreach (var entityToMerge in entitiesToMerge)
            {
                if (entityToMerge.TryGetEntityComponent(out IndexComponent indexComponent))
                {
                    var entityIndex = indexComponent.GetIndex();
                    _itemEntities.TryRemoveEntity(new ItemEntityQueryData { Index = entityIndex });

                    if (!hasSpawnIndexSet)
                    {
                        spawnIndex = entityIndex;
                        hasSpawnIndexSet = true;
                    }
                }

                if (entityToMerge.TryGetEntityComponent(out GameObjectComponent gameObjectComponent))
                {
                    var transform = gameObjectComponent.GetGameObject().transform;

                    if (!hasMergePositionSet)
                    {
                        mergePosition = transform.position;
                        hasMergePositionSet = true;
                    }

                    sequence.Join(transform.DOMove(mergePosition, 0.5f));
                }
            }

            List<IEntity> entitiesToDestroy = new List<IEntity>(entitiesToMerge);
            sequence.AppendCallback(() =>
            {
                foreach (var entityToDestroy in entitiesToDestroy)
                {
                    if (entityToDestroy.TryGetEntityComponent(out GameObjectComponent gameObjectComponent))
                    {
                        GameObject.Destroy(gameObjectComponent.GetGameObject());
                    }
                }
            });

            sequence.AppendCallback(() =>
            {
                var toBeSpawnedEntity = mergeData.MergedToPrefab;
                var position = _gridToWorldConverter.GetWorldPos(spawnIndex);

                var spawnedObj = GameObject.Instantiate(toBeSpawnedEntity, position, Quaternion.identity);
                var spawnedEntity = spawnedObj.GetComponent<IEntity>();

                if (spawnedEntity.TryGetEntityComponent(out IndexComponent spawnedIndexComponent))
                {
                    spawnedIndexComponent.SetIndex(spawnIndex);
                }

                _itemEntities.TryAddEntity(new ItemEntityQueryData { Index = spawnIndex }, spawnedEntity);
            });
        }
    }
}


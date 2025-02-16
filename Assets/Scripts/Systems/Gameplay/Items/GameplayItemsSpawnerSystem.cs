using System.Collections;
using System.Collections.Generic;
using MergeCase.Entities;
using MergeCase.Entities.Unity;
using MergeCase.General.Config;
using MergeCase.General.Config.Gameplay;
using MergeCase.General.Interfaces;
using MergeCase.Systems.Updater;
using UnityEngine;

namespace MergeCase.Systems.Gameplay
{
    public class GameplayItemsSpawnerSystem : GameplaySystemBase, IInitializable<SystemUpdateContext<GameplaySystemBase>>
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        GameplayItemsConfigs _gameplayItemsConfigs;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        GameplayGridsConfigs _gameplayGridsConfigs;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        IEntityCollection<ItemEntityQueryData> _itemsEntityCollection;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        IEntityCollection<GridEntityQueryData> _gridsEntityCollection;

        public bool TryInitialize(SystemUpdateContext<GameplaySystemBase> data)
        {
            if (!data.SystemUpdater.TryGetGameSystemByType(out _itemsEntityCollection))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(IEntityCollection<GridEntityQueryData>)}! Cannot initialize!");
                return false;
            }

            if (!data.SystemUpdater.TryGetGameSystemByType(out _gridsEntityCollection))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(IEntityCollection<GridEntityQueryData>)}! Cannot initialize!");
                return false;
            }

            if (!data.DataCollection.TryGet(out ConfigProvider configProvider))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(ConfigProvider)}! Cannot initialize!");
                return false;
            }

            if (!configProvider.TryGet(out _gameplayItemsConfigs))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(GameplayItemsConfigs)} as config! Cannot initialize!");
                return false;
            }

            if (!configProvider.TryGet(out _gameplayGridsConfigs))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(GameplayGridsConfigs)} as config! Cannot initialize!");
                return false;
            }

            SpawnItemsFromConfig(_gameplayItemsConfigs, _gameplayGridsConfigs, _itemsEntityCollection, _gridsEntityCollection);
            return true;
        }

        public bool TryDeInitialize(SystemUpdateContext<GameplaySystemBase> data)
        {
            return true;
        }

        void SpawnItemsFromConfig(GameplayItemsConfigs gameplayItemsConfigs, GameplayGridsConfigs gameplayGridsConfigs, IEntityCollection<ItemEntityQueryData> _itemsEntites, IEntityCollection<GridEntityQueryData> gridEntities)
        {
            var basicItemPrefab = gameplayItemsConfigs.BasicItemPrefab;
            var positionOffset = gameplayItemsConfigs.PositionOffset;

            var gridAreaSize = gameplayGridsConfigs.AreaSize;

            for (int y = 0; y < gridAreaSize.y; y++)
            {
                for (int x = 0; x < gridAreaSize.x; x++)
                {
                    var randomfloat = Random.Range(0f, 1f);

                    if (randomfloat >= 0.3f)
                    {
                        continue;
                    }

                    Vector2Int gridIndex = new(x, y);

                    if (!gridEntities.TryGetEntity(new GridEntityQueryData { Index = gridIndex }, out IEntity gridEntity))
                    {
                        continue;
                    }

                    if (!gridEntity.TryGetEntityComponent(out GameObjectComponent gameObjectComponent))
                    {
                        continue;
                    }

                    Vector3 position = gameObjectComponent.GetGameObject().transform.position;

                    position += positionOffset;

                    var spawnedObj = GameObject.Instantiate(basicItemPrefab, position, Quaternion.identity);
                    var entity = spawnedObj.GetComponent<IEntity>();

                    _itemsEntites.TryAddEntity(new ItemEntityQueryData
                    {
                        Index = gridIndex,
                    },
                    entity);
                }
            }
        }
    }
}

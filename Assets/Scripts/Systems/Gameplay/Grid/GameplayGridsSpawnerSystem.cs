using System.Collections;
using System.Collections.Generic;
using MergeCase.Entities;
using MergeCase.General.Config;
using MergeCase.General.Config.Gameplay;
using MergeCase.General.Interfaces;
using MergeCase.Systems.Updater;
using UnityEngine;

namespace MergeCase.Systems.Gameplay
{
    public class GameplayGridsSpawnerSystem : GameplaySystemBase, IInitializable<SystemUpdateContext<GameplaySystemBase>>
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        GameplayAreaConfigs _gameplayGridConfigs;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        IEntityCollection<GridEntityQueryData> _gridEntityCollection;

        public bool TryInitialize(SystemUpdateContext<GameplaySystemBase> data)
        {
            if (!data.SystemUpdater.TryGetGameSystemByType(out _gridEntityCollection))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(IEntityCollection<GridEntityQueryData>)}! Cannot initialize!");
                return false;
            }

            if (!data.DataCollection.TryGet(out ConfigProvider configProvider))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(ConfigProvider)}! Cannot initialize!");
                return false;
            }

            if (!configProvider.TryGet(out _gameplayGridConfigs))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(GameplayAreaConfigs)} as config! Cannot initialize!");
                return false;
            }

            SpawnAreaFromConfig(_gameplayGridConfigs);
            return true;
        }

        public bool TryDeInitialize(SystemUpdateContext<GameplaySystemBase> data)
        {
            return true;
        }

        void SpawnAreaFromConfig(GameplayAreaConfigs gameplayAreaConfigs)
        {
            var areaPrefab = gameplayAreaConfigs.BasicAreaPrefab;
            var areaSize = gameplayAreaConfigs.AreaSize;
            var areaOffset = _gameplayGridConfigs.AreaOffset;
            var positionOfset = _gameplayGridConfigs.PositionOffset;
            var xStepOffset = _gameplayGridConfigs.XStepOffset;
            var yStepOffset = _gameplayGridConfigs.YStepOffset;
            var zStepOffset = _gameplayGridConfigs.ZStepOffset;

            Vector3 startPos = positionOfset;

            for (int y = 0; y < areaSize.y; y++)
            {
                for (int x = 0; x < areaSize.x; x++)
                {
                    Vector2Int entityIndex = new(x, y);

                    var spawnedObj = GameObject.Instantiate(areaPrefab, startPos, Quaternion.identity);
                    var entity = spawnedObj.GetComponent<IEntity>();

                    _gridEntityCollection.TryAddEntity(new GridEntityQueryData
                    {
                        Index = entityIndex,
                    },
                    entity);

                    startPos.x += areaOffset.x;
                }

                startPos.x = positionOfset.x;
                startPos.y += areaOffset.y;

                startPos.x += xStepOffset * (y + 1);
                startPos.y += yStepOffset * y;
                startPos.z += zStepOffset * (y + 1);
            }
        }
    }
}
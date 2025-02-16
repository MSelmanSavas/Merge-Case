using System.Collections;
using System.Collections.Generic;
using MergeCase.Entities;
using MergeCase.Entities.Components.Common;
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
        GameplayGridsConfigs _gameplayGridConfigs;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        IEntityCollection<GridEntityQueryData> _gridEntityCollection;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        Transform _gridParent;

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
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(GameplayGridsConfigs)} as config! Cannot initialize!");
                return false;
            }

            _gridParent = new GameObject().transform;
            _gridParent.gameObject.name = "Grids";

            SpawnAreaFromConfig(_gameplayGridConfigs);
            return true;
        }

        public bool TryDeInitialize(SystemUpdateContext<GameplaySystemBase> data)
        {
            return true;
        }

        void SpawnAreaFromConfig(GameplayGridsConfigs gameplayAreaConfigs)
        {
            var areaPrefab = gameplayAreaConfigs.BasicAreaPrefab;
            var totalGridSize = gameplayAreaConfigs.TotalGridSize;
            var gridSize = _gameplayGridConfigs.GridSize;
            var startPositionOfset = _gameplayGridConfigs.StartPositionOffset;
            var xStepOffset = _gameplayGridConfigs.XStepOffset;
            var yStepOffset = _gameplayGridConfigs.YStepOffset;
            var zStepOffset = _gameplayGridConfigs.ZStepOffset;

            Vector3 startPos = startPositionOfset;

            for (int y = 0; y < totalGridSize.y; y++)
            {
                for (int x = 0; x < totalGridSize.x; x++)
                {
                    Vector2Int gridIndex = new(x, y);

                    var spawnedObj = GameObject.Instantiate(areaPrefab, startPos, Quaternion.identity, _gridParent);
                    var entity = spawnedObj.GetComponent<IEntity>();

                    if (entity.TryGetEntityComponent(out IndexComponent indexComponent))
                    {
                        indexComponent.SetIndex(gridIndex);
                    }

                    _gridEntityCollection.TryAddEntity(new GridEntityQueryData
                    {
                        Index = gridIndex,
                    },
                    entity);

                    startPos.x += (gridSize.x / 2f);
                    startPos.y -= (gridSize.y / 2f);
                }

                startPos = startPositionOfset;

                startPos.x += (gridSize.x / 2f) * (y + 1);
                startPos.y += (gridSize.y / 2f) * (y + 1);

                startPos.x += xStepOffset * (y + 1);
                startPos.y += yStepOffset * (y + 1);
                startPos.z += zStepOffset * (y + 1);
            }
        }
    }
}
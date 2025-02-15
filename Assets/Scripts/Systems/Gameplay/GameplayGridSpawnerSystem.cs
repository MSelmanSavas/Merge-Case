using System.Collections;
using System.Collections.Generic;
using MergeCase.General.Config;
using MergeCase.General.Config.Gameplay;
using MergeCase.General.Interfaces;
using MergeCase.Systems.Updater;
using UnityEngine;

namespace MergeCase.Systems.Gameplay
{
    public class GameplayGridSpawnerSystem : GameplaySystemBase, IInitializable<SystemUpdateContext<GameplaySystemBase>>
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        GameplayAreaConfigs _gameplayAreaConfigs;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        GameplayGridSystem _gameplayAreaSystem;

        public bool TryInitialize(SystemUpdateContext<GameplaySystemBase> data)
        {
            if (!data.SystemUpdater.TryGetGameSystem(out _gameplayAreaSystem))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(GameplayGridSystem)}! Cannot initialize!");
                return false;
            }

            if (!data.DataCollection.TryGet(out ConfigProvider configProvider))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(ConfigProvider)}! Cannot initialize!");
                return false;
            }

            if (!configProvider.TryGet(out _gameplayAreaConfigs))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(GameplayAreaConfigs)} as config! Cannot initialize!");
                return false;
            }

            SpawnAreaFromConfig(_gameplayAreaConfigs);
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
            var areaOffset = _gameplayAreaConfigs.AreaOffset;
            var positionOfset = _gameplayAreaConfigs.PositionOffset;

            Vector2 startPos = positionOfset;

            for (int y = 0; y < areaSize.y; y++)
            {
                for (int x = 0; x < areaSize.x; x++)
                {
                    GameObject.Instantiate(areaPrefab, startPos, Quaternion.identity);

                    startPos.x += areaOffset.x;
                }

                startPos.x = positionOfset.x;
                startPos.y += areaOffset.y;
            }
        }
    }
}
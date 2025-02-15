using System.Collections;
using System.Collections.Generic;
using MergeCase.General.Config;
using MergeCase.General.Config.Gameplay;
using MergeCase.General.Interfaces;
using MergeCase.Systems.Updater;
using UnityEngine;

namespace MergeCase.Systems.Gameplay
{
    public class GameplayAreaSpawnerSystem : GameplaySystemBase, IInitializable<SystemUpdateContext<GameplaySystemBase>>
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        GameplayAreaConfigs _gameplayAreaConfigs;

        public bool TryInitialize(SystemUpdateContext<GameplaySystemBase> data)
        {
            if (!data.DataCollection.TryGet(out ConfigProvider configProvider))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(ConfigProvider)}! Cannot initialize!");
                return false;
            }

            if (!configProvider.TryGet(out _gameplayAreaConfigs))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {_gameplayAreaConfigs.GetType()} as config! Cannot initialize!");
                return false;
            }

            return true;
        }

        public bool TryDeInitialize(SystemUpdateContext<GameplaySystemBase> data)
        {
            return true;
        }
    }
}
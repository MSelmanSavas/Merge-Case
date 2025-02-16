using System.Collections;
using System.Collections.Generic;
using MergeCase.General.Interfaces;
using MergeCase.Systems.Updater;
using UnityEngine;

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
        Camera _mainCamera;

        public bool TryInitialize(SystemUpdateContext<GameplaySystemBase> data)
        {
            if (!data.SystemUpdater.TryGetGameSystemByType(out _worldToGridIndexConverter))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(IWorldToGridIndexConverter)}! Cannot initialize!");
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
            if (!Input.GetKeyDown(KeyCode.Mouse0))
            {
                return true;
            }

            var screenPos = Input.mousePosition;
            var worldPos = _mainCamera.ScreenToWorldPoint(screenPos);
            var gridIndex = _worldToGridIndexConverter.GetGridIndex(worldPos);

            UnityLogger.LogWithTag($"screenPos : {screenPos}, worldPos : {worldPos}, gridIndex : {gridIndex}");

            return true;
        }
    }
}


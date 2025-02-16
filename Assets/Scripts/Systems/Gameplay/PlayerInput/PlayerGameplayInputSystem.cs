using System.Collections;
using System.Collections.Generic;
using MergeCase.General.Interfaces;
using MergeCase.Systems.Updater;
using UnityEngine;

namespace MergeCase.Systems.Gameplay
{
    public class PlayerGameplayInputSystem : GameplaySystemBase, IInitializable<SystemUpdateContext<GameplaySystemBase>>, IUpdateable<SystemUpdateContext<GameplaySystemBase>>
    {

        public bool TryInitialize(SystemUpdateContext<GameplaySystemBase> data)
        {
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

            UnityLogger.LogWithTag($"screenPos : {screenPos}");

            return true;
        }
    }
}


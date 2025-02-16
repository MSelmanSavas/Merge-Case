using System.Collections;
using System.Collections.Generic;
using MergeCase.General.Interfaces;
using UnityEngine;

namespace MergeCase.Systems.Gameplay
{
    public class GameplayMergeObjectsSpawnerSystem : GameplaySystemBase, IInitializable
    {
        public bool TryDeInitialize()
        {
            return true;
        }

        public bool TryInitialize()
        {
            return true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MergeCase.General.Config.Gameplay
{
    [CreateAssetMenu(menuName = "MergeCase/Configs/Create Gameplay Items Configs", fileName = "DefaultGameplayItemsConfigs")]
    public class GameplayItemsConfigs : ScriptableObject
    {
        [field: SerializeField]
        public GameObject BasicItemPrefab { get; private set; }

        [field: SerializeField]
        public Vector3 PositionOffset { get; private set; }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MergeCase.General.Config.Gameplay
{
    [CreateAssetMenu(menuName = "MergeCase/Configs/Create Gameplay Area Configs", fileName = "DefaultGameplayAreaConfigs")]
    public class GameplayAreaConfigs : ScriptableObject
    {
        [field: SerializeField]
        public GameObject BasicAreaPrefab { get; private set; }
    }
}


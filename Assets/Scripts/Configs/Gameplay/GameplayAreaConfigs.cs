using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MergeCase.General.Config.Gameplay
{
    [CreateAssetMenu(menuName = "MergeCase/Configs/Create Gameplay Area Configs", fileName = "DefaultGameplayAreaConfigs")]
    public class GameplayAreaConfigs : ScriptableObject
    {
        [field: SerializeField]
        public Vector2Int AreaSize { get; private set; }

        [field: SerializeField]
        public Vector2 AreaOffset { get; private set; }

        [field: SerializeField]
        public float XStepOffset { get; private set; }

        [field: SerializeField]
        public float YStepOffset { get; private set; }
        [field: SerializeField]
        public float ZStepOffset { get; private set; }

        [field: SerializeField]
        public Vector2 PositionOffset { get; private set; }

        [field: SerializeField]
        public GameObject BasicAreaPrefab { get; private set; }
    }
}


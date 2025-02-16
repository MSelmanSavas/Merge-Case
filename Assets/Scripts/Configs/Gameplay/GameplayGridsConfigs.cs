using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MergeCase.General.Config.Gameplay
{
    [CreateAssetMenu(menuName = "MergeCase/Configs/Create Gameplay Grids Configs", fileName = "DefaultGameplayGridsConfigs")]
    public class GameplayGridsConfigs : ScriptableObject
    {
        [field: SerializeField]
        public Vector2Int TotalGridSize { get; private set; }

        [field: SerializeField]
        public Vector2 GridSize { get; private set; }

        [field: SerializeField]
        public float XStepOffset { get; private set; }

        [field: SerializeField]
        public float YStepOffset { get; private set; }

        [field: SerializeField]
        public float ZStepOffset { get; private set; }

        [field: SerializeField]
        public Vector3 StartPositionOffset { get; private set; }

        [field: SerializeField]
        public GameObject BasicAreaPrefab { get; private set; }
    }
}


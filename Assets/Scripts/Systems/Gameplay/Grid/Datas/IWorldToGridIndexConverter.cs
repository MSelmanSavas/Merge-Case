using UnityEngine;

namespace MergeCase.Systems.Gameplay
{
    public interface IWorldToGridIndexConverter
    {
        public Vector2Int GetGridIndex(Vector3 worldPos);
    }
}
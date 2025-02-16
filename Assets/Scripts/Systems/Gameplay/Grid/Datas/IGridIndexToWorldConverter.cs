using UnityEngine;

namespace MergeCase.Systems.Gameplay
{
    public interface IGridIndexToWorldConverter
    {
        public Vector3 GetWorldPos(Vector2Int gridIndex);
    }
}

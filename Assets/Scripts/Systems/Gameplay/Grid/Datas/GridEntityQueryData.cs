using System;
using UnityEngine;

namespace MergeCase.Systems.Gameplay
{
    public struct GridEntityQueryData : IEquatable<GridEntityQueryData>
    {
        public Vector2Int Index;

        public bool Equals(GridEntityQueryData other)
        {
            return Index.x == other.Index.x && Index.y == other.Index.y;
        }

        public override bool Equals(object obj)
        {
            return obj is GridEntityQueryData loc && Equals(loc);
        }

        public override int GetHashCode()
        {
            return Index.GetHashCode();
        }
    }
}


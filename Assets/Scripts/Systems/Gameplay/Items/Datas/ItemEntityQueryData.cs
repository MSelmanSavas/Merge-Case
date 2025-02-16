using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MergeCase.Systems.Gameplay
{
    public struct ItemEntityQueryData : IEquatable<ItemEntityQueryData>
    {
        public Vector2Int Index;

        public bool Equals(ItemEntityQueryData other)
        {
            return Index.x == other.Index.x && Index.y == other.Index.y;
        }

        public override bool Equals(object obj)
        {
            return obj is ItemEntityQueryData loc && Equals(loc);
        }

        public override int GetHashCode()
        {
            return Index.GetHashCode();
        }
    }
}

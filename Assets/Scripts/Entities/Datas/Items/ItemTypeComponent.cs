using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MergeCase.Entities.Components.Items
{
    public class ItemTypeComponent : IComponent
    {
        [field: SerializeField]
        public ItemType Type { get; private set; }

        public bool IsSame(ItemTypeComponent other)
        {
            return Type == other.Type;
        }
    }
}


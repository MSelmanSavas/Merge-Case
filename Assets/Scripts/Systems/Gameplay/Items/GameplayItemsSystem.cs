using System.Collections;
using System.Collections.Generic;
using MergeCase.Entities;
using UnityEngine;

namespace MergeCase.Systems.Gameplay
{
    public class GameplayItemsSystem : GameplaySystemBase, IEntityCollection<ItemEntityQueryData>
    {
        public bool TryAddEntity(ItemEntityQueryData entityQueryData, IEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public bool TryGetEntity(ItemEntityQueryData entityQueryData, out IEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public bool TryRemoveEntity(ItemEntityQueryData entityQueryData)
        {
            throw new System.NotImplementedException();
        }
    }
}

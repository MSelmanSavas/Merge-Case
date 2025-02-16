using System.Collections;
using System.Collections.Generic;
using MergeCase.Entities;
using UnityEngine;

namespace MergeCase.Systems.Gameplay
{
    public class GameplayItemsSystem : GameplaySystemBase, IEntityCollection<ItemEntityQueryData>
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        Dictionary<ItemEntityQueryData, IEntity> _gridEntities = new();

        public bool TryAddEntity(ItemEntityQueryData entityQueryData, IEntity entity)
        {
            if (_gridEntities.ContainsKey(entityQueryData))
            {
                return false;
            }

            _gridEntities.Add(entityQueryData, entity);
            return true;
        }

        public bool TryRemoveEntity(ItemEntityQueryData entityQueryData)
        {
            if (!_gridEntities.ContainsKey(entityQueryData))
            {
                return false;
            }

            _gridEntities.Remove(entityQueryData);
            return true;
        }

        public bool TryGetEntity(ItemEntityQueryData entityQueryData, out IEntity entity)
        {
            return _gridEntities.TryGetValue(entityQueryData, out entity);
        }
    }
}

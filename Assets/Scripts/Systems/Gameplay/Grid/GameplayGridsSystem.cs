using System.Collections.Generic;
using MergeCase.Entities;
using MergeCase.Systems.Gameplay;

namespace MergeCase.Systems.Gameplay
{
    public class GameplayGridsSystem : GameplaySystemBase, IEntityCollection<GridEntityQueryData>
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        Dictionary<GridEntityQueryData, IEntity> _gridEntities = new();

        public bool TryAddEntity(GridEntityQueryData entityQueryData, IEntity entity)
        {
            if (_gridEntities.ContainsKey(entityQueryData))
            {
                return false;
            }

            _gridEntities.Add(entityQueryData, entity);
            return true;
        }

        public bool TryRemoveEntity(GridEntityQueryData entityQueryData)
        {
            if (!_gridEntities.ContainsKey(entityQueryData))
            {
                return false;
            }

            _gridEntities.Remove(entityQueryData);
            return true;
        }

        public bool TryGetEntity(GridEntityQueryData entityQueryData, out IEntity entity)
        {
            return _gridEntities.TryGetValue(entityQueryData, out entity);
        }
    }
}


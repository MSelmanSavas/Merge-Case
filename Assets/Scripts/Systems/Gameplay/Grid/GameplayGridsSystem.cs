using MergeCase.Entities;
using MergeCase.Systems.Gameplay;

namespace MergeCase.Systems.Gameplay
{
    public class GameplayGridsSystem : GameplaySystemBase, IEntityCollection<GridEntityQueryData>
    {
        public bool TryAddEntity(GridEntityQueryData entityQueryData, IEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public bool TryGetEntity(GridEntityQueryData entityQueryData, out IEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public bool TryRemoveEntity(GridEntityQueryData entityQueryData)
        {
            throw new System.NotImplementedException();
        }
    }
}


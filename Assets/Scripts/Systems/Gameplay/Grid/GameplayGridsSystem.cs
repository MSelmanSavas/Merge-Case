using System.Collections.Generic;
using MergeCase.Entities;
using MergeCase.General.Config;
using MergeCase.General.Config.Gameplay;
using MergeCase.General.Interfaces;
using MergeCase.Systems.Gameplay;
using MergeCase.Systems.Updater;
using UnityEngine;

namespace MergeCase.Systems.Gameplay
{
    public class GameplayGridsSystem : GameplaySystemBase, IInitializable<SystemUpdateContext<GameplaySystemBase>>, IEntityCollection<GridEntityQueryData>, IWorldToGridIndexConverter
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        Dictionary<GridEntityQueryData, IEntity> _gridEntities = new();

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        GameplayGridsConfigs _gameplayGridConfigs;

        public bool TryInitialize(SystemUpdateContext<GameplaySystemBase> data)
        {
            if (!data.DataCollection.TryGet(out ConfigProvider configProvider))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(ConfigProvider)}! Cannot initialize!");
                return false;
            }

            if (!configProvider.TryGet(out _gameplayGridConfigs))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(GameplayGridsConfigs)} as config! Cannot initialize!");
                return false;
            }

            return true;
        }

        public bool TryDeInitialize(SystemUpdateContext<GameplaySystemBase> data)
        {
            return true;
        }

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

        public Vector2Int GetGridIndex(Vector3 worldPos)
        {
            worldPos -= _gameplayGridConfigs.StartPositionOffset;

            var gridSize = _gameplayGridConfigs.GridSize;
            var gridSizeHalf = gridSize / 2f;

            worldPos -= (Vector3)gridSizeHalf;

            var xHalf = ((worldPos.x + gridSizeHalf.x) / gridSizeHalf.x);
            var yHalf = ((worldPos.y + gridSizeHalf.y) / gridSizeHalf.y);

            int y = Mathf.RoundToInt((xHalf + yHalf) * 0.5f);
            int x = Mathf.RoundToInt((-xHalf + yHalf) * -0.5f);

            return new Vector2Int(x, y);
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using MergeCase.Entities;
using MergeCase.General.Interfaces;
using MergeCase.Systems.Updater;
using UnityEngine;

namespace MergeCase.Systems.Gameplay
{
    public class ItemsCleanupSystem : GameplaySystemBase, IInitializable<SystemUpdateContext<GameplaySystemBase>>, IUpdateable
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        IEntityCollection<ItemEntityQueryData> _itemEntities;

        IEnumerator _itemsEnumerator;

        public bool TryInitialize(SystemUpdateContext<GameplaySystemBase> data)
        {
            if (!data.SystemUpdater.TryGetGameSystemByType(out _itemEntities))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(IEntityCollection<ItemEntityQueryData>)}! Cannot initialize!");
                return false;
            }

            _itemsEnumerator = _itemEntities.GetEnumerator();

            return true;
        }

        public bool TryDeInitialize(SystemUpdateContext<GameplaySystemBase> data)
        {
            return true;
        }

        public bool TryUpdate()
        {
            while (_itemsEnumerator.MoveNext())
            {
                if (_itemsEnumerator.Current is not IEntity entity)
                {
                    continue;
                }
            }

            _itemsEnumerator.Reset();
            return true;
        }
    }
}


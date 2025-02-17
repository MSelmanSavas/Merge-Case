using System.Collections;
using System.Collections.Generic;
using MergeCase.Entities;
using MergeCase.Entities.Components.Unity;
using MergeCase.General.Interfaces;
using MergeCase.Systems.Updater;
using UnityEngine;

namespace MergeCase.Systems.Gameplay
{
    public class ItemsCleanupSystem : GameplaySystemBase, IInitializable<SystemUpdateContext<GameplaySystemBase>>, IUpdateable
    {
        ItemsCleanupData _itemsCleanupData;

        public bool TryInitialize(SystemUpdateContext<GameplaySystemBase> data)
        {
            if (!data.DataCollection.TryGet(out _itemsCleanupData))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(ItemsCleanupData)}! Cannot initialize!");
                return false;
            }

            return true;
        }

        public bool TryDeInitialize(SystemUpdateContext<GameplaySystemBase> data)
        {
            return true;
        }

        public bool TryUpdate()
        {
            foreach (var entity in _itemsCleanupData.ItemEntitesToCleanup)
            {
                if (entity.TryGetEntityComponent(out GameObjectComponent gameObjectComponent))
                {
                    GameObject.Destroy(gameObjectComponent.GetGameObject());
                }
            }

            _itemsCleanupData.ItemEntitesToCleanup.Clear();
            return true;
        }
    }
}


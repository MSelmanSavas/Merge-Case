
using System.Collections.Generic;
using DG.Tweening;
using MergeCase.Entities;
using MergeCase.Entities.Components.Common;
using MergeCase.Entities.Components.Unity;
using MergeCase.General.Config.Gameplay;
using MergeCase.Systems.Command.Interfaces;
using MergeCase.Systems.Gameplay;
using UnityEngine;

namespace MergeCase.Systems.Command.Gameplay
{
    public class MergeItemsCommand : ICommand
    {
        public List<IEntity> Entities { get; private set; }
        public MergeItemData MergeItemData { get; private set; }
        public Vector3 MergePosition { get; private set; }
        public Vector2Int MergeToIndex { get; private set; }
        public IEntityCollection<ItemEntityQueryData> ItemEntites { get; private set; }

        ItemsCleanupData _itemsCleanupData;
        bool _isCommandCompleted = false;

        public MergeItemsCommand(IEnumerable<IEntity> entities, MergeItemData mergeItemData, Vector3 mergePosition, Vector2Int mergeToIndex, IEntityCollection<ItemEntityQueryData> itemEntities, ItemsCleanupData itemsCleanupData)
        {
            Entities = new(10);

            foreach (var entity in entities)
            {
                Entities.Add(entity);
            }

            MergeItemData = mergeItemData;
            MergePosition = mergePosition;
            MergeToIndex = mergeToIndex;
            ItemEntites = itemEntities;
            _itemsCleanupData = itemsCleanupData;
            _isCommandCompleted = false;
            CreateDoTweenSequence();
        }

        public bool IsCompleted()
        {
            return _isCommandCompleted;
        }

        public bool TryInitialize()
        {
            foreach (var entity in Entities)
            {
                entity.IsQueryable = false;
            }

            return true;
        }

        public bool TryUpdate()
        {
            return true;
        }

        void CreateDoTweenSequence()
        {
            Sequence sequence = DOTween.Sequence();

            foreach (var entityToMerge in Entities)
            {
                entityToMerge.IsQueryable = false;

                if (entityToMerge.TryGetEntityComponent(out GameObjectComponent gameObjectComponent))
                {
                    var transform = gameObjectComponent.GetGameObject().transform;

                    sequence.Join(transform.DOMove(MergePosition, 0.5f));
                }
            }

            sequence.AppendCallback(() =>
            {
                foreach (var entityToDestroy in Entities)
                {
                    entityToDestroy.IsMarkedToBeRemoved = true;

                    if (entityToDestroy.TryGetEntityComponent(out IndexComponent entityToDestroyIndexComponent))
                    {
                        ItemEntites.TryRemoveEntity(new ItemEntityQueryData { Index = entityToDestroyIndexComponent.GetIndex() });
                    }

                    _itemsCleanupData.ItemEntitesToCleanup.Add(entityToDestroy);
                }
            });

            sequence.AppendCallback(() =>
            {
                var toBeSpawnedEntity = MergeItemData.MergedToPrefab;

                var spawnedObj = GameObject.Instantiate(toBeSpawnedEntity, MergePosition, Quaternion.identity);
                var spawnedEntity = spawnedObj.GetComponent<IEntity>();

                if (spawnedEntity.TryGetEntityComponent(out IndexComponent spawnedIndexComponent))
                {
                    spawnedIndexComponent.SetIndex(MergeToIndex);
                }

                ItemEntites.TryAddEntity(new ItemEntityQueryData { Index = MergeToIndex }, spawnedEntity);
            });

            sequence.AppendCallback(() =>
            {
                _isCommandCompleted = true;
            });
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using MergeCase.Entities.Components.Items;
using UnityEngine;

namespace MergeCase.Systems.Quest.Data
{
    [System.Serializable]
    public class QuestData
    {
        public ItemType QuestType;
        public int CollectAmount;
        public Sprite QuestSprite;

        public QuestData GetCopy()
        {
            return new QuestData
            {
                QuestType = this.QuestType,
                CollectAmount = this.CollectAmount,
                QuestSprite = this.QuestSprite,
            };
        }
    }
}


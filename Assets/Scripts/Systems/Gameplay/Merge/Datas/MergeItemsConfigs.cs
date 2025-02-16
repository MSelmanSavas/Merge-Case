using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MergeCase.Entities.Components.Items;
using UnityEngine;

namespace MergeCase.General.Config.Gameplay
{
    [CreateAssetMenu(menuName = "MergeCase/Configs/Create Merge Items Configs", fileName = "DefaultMergeItemsConfigs")]
    public class MergeItemsConfigs : ScriptableObject
    {
        [field: SerializeField]
        public MergeItemData[] MergeItemDatas { get; private set; }

        public bool TryGetMergeItemData(ItemType itemType, int itemCount, out MergeItemData mergeItemData)
        {
            mergeItemData = MergeItemDatas.FirstOrDefault(x => x.MergeType == itemType && x.RequiredAmount <= itemCount);

            return mergeItemData != null;
        }
    }
}

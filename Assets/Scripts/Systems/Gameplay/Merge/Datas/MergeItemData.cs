using System.Collections;
using System.Collections.Generic;
using MergeCase.Entities.Components.Items;
using UnityEngine;

namespace MergeCase.General.Config.Gameplay
{
    [System.Serializable]
    public class MergeItemData
    {
        public ItemType MergeType;
        public int RequiredAmount;
        public GameObject MergedToPrefab;
    }
}

using System.Collections;
using System.Collections.Generic;
using MergeCase.Systems.Quest.Data;
using UnityEngine;

namespace MergeCase.Systems.Quest.Config
{
    [CreateAssetMenu(menuName = "MergeCase/Configs/Create Quest Configs", fileName = "DefaultQuestConfigs")]
    public class QuestConfigs : ScriptableObject
    {
        [field: SerializeField]
        public List<QuestData> QuestDatas { get; private set; } = new();
    }
}
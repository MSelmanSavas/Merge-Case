using System.Collections;
using System.Collections.Generic;
using MergeCase.Entities.Components.Items;
using MergeCase.General.Config;
using MergeCase.Systems.Quest.Config;
using UnityEngine;

namespace MergeCase.UI.Gameplay
{
    public class QuestUIDisplay : MonoBehaviour
    {
        public List<QuestSingleItemDisplay> QuestItems = new();

        public void Initialize(ConfigProvider configProvider)
        {
            if (!configProvider.TryGet(out QuestConfigs questConfigs))
            {
                UnityLogger.LogErrorWithTag($"No quest config found! Cannot initialize : {this.GetType()}!");
                return;
            }

            int questItemsCount = QuestItems.Count;
            int questCount = questConfigs.QuestDatas.Count;

            foreach (var questItem in QuestItems)
            {
                questItem.gameObject.SetActive(false);
            }

            int iterationCount = Mathf.Min(questItemsCount, questCount);

            for (int i = 0; i < iterationCount; i++)
            {
                var questItem = QuestItems[i];
                questItem.gameObject.SetActive(true);
                questItem.Initialize(questConfigs.QuestDatas[i]);
            }
        }

        public bool TryGetQuestItemSingleDisplayByQuestType(ItemType type, out QuestSingleItemDisplay questSingleItemDisplay)
        {
            questSingleItemDisplay = QuestItems.Find(x => x.QuestData.QuestType == type);

            if (questSingleItemDisplay == null || !questSingleItemDisplay.gameObject.activeInHierarchy)
            {
                return false;
            }

            return true;
        }
    }
}

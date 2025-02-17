using System.Collections;
using System.Collections.Generic;
using MergeCase.Systems.Quest.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MergeCase.UI.Gameplay
{
    public class QuestSingleItemDisplay : MonoBehaviour
    {
        public QuestData QuestData { get; private set; }
        public TextMeshProUGUI RemainingCountText;
        public Image QuestVisual;

        public void Initialize(QuestData questData)
        {
            QuestData = questData.GetCopy();
            RemainingCountText.text = QuestData.CollectAmount.ToString();
            QuestVisual.sprite = QuestData.QuestSprite;
        }

        public Vector3 GetTargetPosition()
        {
            return QuestVisual.transform.position;
        }

        public void UpdateQuest(int collectedCount)
        {
            QuestData.CollectAmount -= collectedCount;
            RemainingCountText.text = QuestData.CollectAmount.ToString();
        }
    }
}

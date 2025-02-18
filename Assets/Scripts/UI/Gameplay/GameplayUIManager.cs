using System.Collections;
using System.Collections.Generic;
using MergeCase.General.Config;
using MergeCase.UI.Popup;
using UnityEngine;

namespace MergeCase.UI.Gameplay
{
    public class GameplayUIManager : MonoBehaviour
    {
        [field: SerializeField]
        public ConfigProvider ConfigProvider { get; private set; }

        [field: SerializeField]
        public QuestUIDisplay QuestUIDisplay { get; private set; }

        [field: SerializeField]
        public GameFinishedPopup GameFinishedPopup { get; private set; }

        void Awake()
        {
            RefBook.Add(this);
        }

        void OnDestroy()
        {
            RefBook.Remove(this);
        }

        void Start()
        {
            QuestUIDisplay.Initialize(ConfigProvider);
        }
    }
}


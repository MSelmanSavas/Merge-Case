using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MergeCase.UI.Gameplay
{
    public class GameplayUIManager : MonoBehaviour
    {
        [field: SerializeField]
        public QuestUIDisplay QuestUIDisplay { get; private set; }

        void Awake()
        {
            RefBook.Add(this);
        }

        void OnDestroy()
        {
            RefBook.Remove(this);
        }
    }
}



using MergeCase.Entities.Components.Items;
using MergeCase.General.Config;
using MergeCase.Systems.Command.Interfaces;
using MergeCase.UI.Gameplay;
using UnityEngine;

namespace MergeCase.Systems.Command.UI
{
    public class UpdateQuestUIDisplayCommand : ICommand
    {
        bool _isCommandCompleted = false;
        QuestSingleItemDisplay _targetDisplay;

        public UpdateQuestUIDisplayCommand(ConfigProvider configProvider, ItemType itemType, Vector3 fromPosition)
        {
            if (!RefBook.TryGet(out GameplayUIManager gameplayUIManager))
            {
                _isCommandCompleted = true;
                return;
            }

            if (!gameplayUIManager.QuestUIDisplay.TryGetQuestItemSingleDisplayByQuestType(itemType, out _targetDisplay))
            {
                _isCommandCompleted = true;
                return;
            }

            _targetDisplay.UpdateQuest(1);
            _isCommandCompleted = true;
        }

        public bool TryInitialize()
        {
            return true;
        }

        public bool TryUpdate()
        {
            return true;
        }

        public bool IsCompleted() => _isCommandCompleted;
    }
}


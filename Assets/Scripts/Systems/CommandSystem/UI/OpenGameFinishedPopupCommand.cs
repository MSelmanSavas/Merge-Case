using System.Collections;
using System.Collections.Generic;
using MergeCase.Systems.Command.Interfaces;
using MergeCase.UI.Gameplay;
using UnityEngine;

namespace MergeCase.Systems.Command.UI
{
    public class OpenGameFinishedPopupCommand : ICommand
    {
        ICommandCollection _commandCollection;
        bool _isCommandCompleted = false;
        List<UpdateQuestUIDisplayCommand> _updateUICommands = new();
        GameplayUIManager _gameplayUIManager;

        public OpenGameFinishedPopupCommand(ICommandCollection commandCollection)
        {
            _commandCollection = commandCollection;

            if (!RefBook.TryGet(out _gameplayUIManager))
            {
                _isCommandCompleted = true;
                return;
            }

            _isCommandCompleted = false;
        }

        public bool IsCompleted() => _isCommandCompleted;

        public bool TryInitialize()
        {
            return true;
        }

        public bool TryUpdate()
        {
            //Means there are ui animation are playing that we need to wait to open finished game popup!
            if (_commandCollection.TryGetAllNoAlloc(_updateUICommands))
            {
                return true;
            }

            _gameplayUIManager.GameFinishedPopup.gameObject.SetActive(true);
            _isCommandCompleted = true;
            return true;
        }
    }
}

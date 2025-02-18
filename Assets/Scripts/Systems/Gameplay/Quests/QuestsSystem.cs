using System.Collections.Generic;
using MergeCase.Entities;
using MergeCase.General.Config;
using MergeCase.General.Interfaces;
using MergeCase.Systems.Command.Gameplay;
using MergeCase.Systems.Command.Interfaces;
using MergeCase.Systems.Command.UI;
using MergeCase.Systems.Gameplay;
using MergeCase.Systems.Quest.Config;
using MergeCase.Systems.Quest.Data;
using MergeCase.Systems.Updater;

namespace MergeCase.Systems.Quest
{
    public class QuestsSystem : GameplaySystemBase, IInitializable<SystemUpdateContext<GameplaySystemBase>>, IUpdateable<SystemUpdateContext<GameplaySystemBase>>
    {
        ICommandCollection _commandCollection;
        ConfigProvider _configProvider;
        QuestConfigs _questConfigs;
        List<QuestData> _questDatas = new();
        List<MergeItemsCommand> _currentlyRunningMergeCommands = new();

        public bool TryInitialize(SystemUpdateContext<GameplaySystemBase> data)
        {
            if (!data.SystemUpdater.TryGetGameSystemByType(out _commandCollection))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(ICommandCollection)}! Cannot initialize!");
                return false;
            }

            if (!data.DataCollection.TryGet(out _configProvider))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(ConfigProvider)}! Cannot initialize!");
                return false;
            }

            if (!_configProvider.TryGet(out _questConfigs))
            {
                UnityLogger.LogErrorWithTag($"{GetType()} could not find {typeof(QuestConfigs)} as config! Cannot initialize!");
                return false;
            }

            foreach (var questData in _questConfigs.QuestDatas)
            {
                _questDatas.Add(questData.GetCopy());
            }

            return true;
        }

        public bool TryDeInitialize(SystemUpdateContext<GameplaySystemBase> data)
        {
            _questDatas.Clear();
            return true;
        }

        public bool TryUpdate(SystemUpdateContext<GameplaySystemBase> data)
        {
            CheckRunningMergeCommands();

            if (CheckAreAllQuestComplete())
            {
                _commandCollection.TryAdd(new OpenGameFinishedPopupCommand(
                    _commandCollection
                ));

                data.GameState.State = GameStateData.GameState.Finished;
                UnityLogger.LogWithTag("Game is finished, all quests are completed!");
            }

            return true;
        }


        void CheckRunningMergeCommands()
        {
            _currentlyRunningMergeCommands.Clear();

            if (!_commandCollection.TryGetAllNoAlloc(_currentlyRunningMergeCommands))
            {
                return;
            }

            foreach (var mergeCommand in _currentlyRunningMergeCommands)
            {
                if (!mergeCommand.IsCompleted())
                {
                    continue;
                }

                var mergeSpawnType = mergeCommand.MergeItemData.MergedToType;

                foreach (var questData in _questDatas)
                {
                    if (questData.CollectAmount <= 0)
                    {
                        continue;
                    }

                    if (mergeSpawnType != questData.QuestType)
                    {
                        continue;
                    }

                    questData.CollectAmount--;
                    UnityLogger.LogWithTag($"Quest is collected! Collected type : {questData.QuestType}. Remaining Count : {questData.CollectAmount}");
                    //Create Quest UI Move command here!
                    _commandCollection.TryAdd(new UpdateQuestUIDisplayCommand(
                        _configProvider,
                        questData,
                        mergeCommand.MergePosition
                    ));
                }
            }
        }

        bool CheckAreAllQuestComplete()
        {
            foreach (var questData in _questDatas)
            {
                if (questData.CollectAmount > 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}


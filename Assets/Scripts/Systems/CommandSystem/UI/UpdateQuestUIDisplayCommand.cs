
using DG.Tweening;
using MergeCase.Entities.Components.Items;
using MergeCase.General.Config;
using MergeCase.Systems.Command.Interfaces;
using MergeCase.Systems.Quest.Config.UI;
using MergeCase.Systems.Quest.Data;
using MergeCase.Systems.Quest.UI;
using MergeCase.UI.Gameplay;
using UnityEngine;

namespace MergeCase.Systems.Command.UI
{
    public class UpdateQuestUIDisplayCommand : ICommand
    {
        bool _isCommandCompleted = false;
        QuestSingleItemDisplay _targetDisplay;
        QuestsDisplayConfigs _configs;
        Vector3 _spawnFromPosition;
        QuestData _questData;

        public UpdateQuestUIDisplayCommand(ConfigProvider configProvider, QuestData questData, Vector3 fromPosition)
        {
            if (!RefBook.TryGet(out GameplayUIManager gameplayUIManager))
            {
                _isCommandCompleted = true;
                return;
            }

            if (!configProvider.TryGet(out QuestsDisplayConfigs questsDisplayConfigs))
            {
                _isCommandCompleted = true;
                return;
            }

            if (!gameplayUIManager.QuestUIDisplay.TryGetQuestItemSingleDisplayByQuestType(questData.QuestType, out _targetDisplay))
            {
                _isCommandCompleted = true;
                return;
            }

            _questData = questData;
            _spawnFromPosition = fromPosition;
            _configs = questsDisplayConfigs;
            CreateDoTweenSequence();
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

        void CreateDoTweenSequence()
        {
            Sequence sequence = DOTween.Sequence();

            var spawnedParticle = GameObject.Instantiate(_configs.CollectParticlePrefab, _spawnFromPosition, Quaternion.identity);

            spawnedParticle.Initialize(_questData.QuestSprite);

            var jumpToPosition = _spawnFromPosition + (Vector3)Random.insideUnitCircle * 1f;

            sequence.Append(spawnedParticle.transform.DOJump(jumpToPosition, 1, 2, _configs.ScaleDuration).SetEase(Ease.Linear));
            sequence.Join(spawnedParticle.transform.DOScale(_configs.ScaleTarget, _configs.ScaleDuration).From(0f).SetEase(Ease.OutBack));
            sequence.AppendInterval(_configs.AfterScaleWaitDuration);
            sequence.Append(spawnedParticle.transform.DOMove(_targetDisplay.GetTargetPosition(), _configs.MoveDuration).SetEase(Ease.InBack));

            sequence.AppendCallback(() =>
            {
                GameObject.Destroy(spawnedParticle.gameObject);
            });

            sequence.AppendCallback(() =>
            {
                _targetDisplay.UpdateQuest(1);
            });

            sequence.AppendCallback(() =>
            {
                _isCommandCompleted = true;
            });
        }
    }
}


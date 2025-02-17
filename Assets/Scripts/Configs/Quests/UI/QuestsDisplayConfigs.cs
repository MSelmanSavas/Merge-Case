using System.Collections;
using System.Collections.Generic;
using MergeCase.Systems.Quest.UI;
using UnityEngine;

namespace MergeCase.Systems.Quest.Config.UI
{
    [CreateAssetMenu(menuName = "MergeCase/Configs/Create Quest Display Configs", fileName = "DefaulQuestsDisplayConfigs")]
    public class QuestsDisplayConfigs : ScriptableObject
    {
        public float ScaleTarget;
        public float ScaleDuration;
        public float AfterScaleWaitDuration;
        public float MoveDuration;
        public QuestsUICollectParticle CollectParticlePrefab;
    }
}

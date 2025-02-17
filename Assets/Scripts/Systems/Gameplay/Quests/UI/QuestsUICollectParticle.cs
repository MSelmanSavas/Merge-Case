using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MergeCase.Systems.Quest.UI
{
    public class QuestsUICollectParticle : MonoBehaviour
    {
        public SpriteRenderer VisualRenderer;

        public void Initialize(Sprite sprite)
        {
            VisualRenderer.sprite = sprite;
        }
    }
}


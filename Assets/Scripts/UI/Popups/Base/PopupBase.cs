using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MergeCase.UI.Popup
{
    public abstract class PopupBase : MonoBehaviour
    {
        public abstract PopupType Type { get; }
        void OnEnable()
        {
            //Used in OnEnable until i can implement a general Popup manager with popup stack logic.
            OnActivate();
        }

        void OnDisable()
        {
            //Used in OnDisable until i can implement a general Popup manager with popup stack logic.
            OnDeactivate();
        }

        public virtual void OnActivate()
        {

        }

        public virtual void OnDeactivate()
        {

        }
    }
}


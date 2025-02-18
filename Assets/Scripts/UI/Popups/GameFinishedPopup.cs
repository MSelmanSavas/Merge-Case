using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MergeCase.UI.Popup
{
    public class GameFinishedPopup : BasePopup
    {
        public override PopupType Type => PopupType.GameFinished;

        [SerializeField]
        Button _restartGameButton;


        public override void OnActivate()
        {
            base.OnActivate();
            _restartGameButton.onClick.AddListener(ReloadLevel);
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();
            _restartGameButton.onClick.RemoveListener(ReloadLevel);
        }

        //Will be using this to reload level until i implement a SceneManagement logic
        //Overkill for a case study though i think
        void ReloadLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MergeCase.Systems.Gameplay
{
    public class GameStateData
    {
        public GameState State;

        public enum GameState
        {
            Loading,
            Playing,
            Finished,
        }
    }
}


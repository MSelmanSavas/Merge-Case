using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayLoader : MonoBehaviour
{
    void Start()
    {
        InitializeGameUpdater();
    }

    private void InitializeGameUpdater()
    {
        GameObject gameUpdater = new GameObject
        {
            name = "GameUpdater",
        };

        GameplaySystemUpdater gameplaySystemUpdater = gameUpdater.AddComponent<GameplaySystemUpdater>();

        //Add Data providers here
        //gameplaySystemUpdater.SystemUpdater.UpdateContext.DataProvider.TryAdd();

        gameplaySystemUpdater.SystemUpdater.TryInitialize();
    }
}

using System.Collections;
using System.Collections.Generic;
using MergeCase.General.Config;
using MergeCase.Systems.Gameplay;
using UnityEngine;

public class GameplayLoader : MonoBehaviour
{
    [SerializeField]
    ConfigProvider _configProvider;

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
        var systemUpdater = gameplaySystemUpdater.SystemUpdater;

        systemUpdater.TryAddGameSystemImmediately(new GameplayGridsSpawnerSystem(), autoInitialize: false);
        systemUpdater.TryAddGameSystemImmediately(new GameplayGridsSystem(), autoInitialize: false);

        systemUpdater.TryAddGameSystemImmediately(new GameplayItemsSpawnerSystem(), autoInitialize: false);
        systemUpdater.TryAddGameSystemImmediately(new GameplayItemsSystem(), autoInitialize: false);

        systemUpdater.UpdateContext.DataCollection.TryAdd(_configProvider);

        systemUpdater.TryInitialize();
    }
}

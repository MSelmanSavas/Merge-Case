using System.Collections;
using System.Collections.Generic;
using MergeCase.General.Config;
using MergeCase.Systems.Command;
using MergeCase.Systems.Gameplay;
using MergeCase.Systems.Quest;
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

        systemUpdater.UpdateContext.DataCollection.TryAdd(_configProvider);
        systemUpdater.UpdateContext.DataCollection.TryAdd(new ItemsCleanupData());

        systemUpdater.TryAddGameSystemImmediately(new GameplayGridsSystem(), autoInitialize: false);
        systemUpdater.TryAddGameSystemImmediately(new GameplayItemsSystem(), autoInitialize: false);

        systemUpdater.TryAddGameSystemImmediately(new GameplayGridsSpawnerSystem(), autoInitialize: false);
        systemUpdater.TryAddGameSystemImmediately(new GameplayItemsSpawnerSystem(), autoInitialize: false);

        systemUpdater.TryAddGameSystemImmediately(new MergeItemsSystem(), autoInitialize: false);
        systemUpdater.TryAddGameSystemImmediately(new PlayerGameplayInputSystem(), autoInitialize: false);

        systemUpdater.TryAddGameSystemImmediately(new QuestsSystem(), autoInitialize: false);
        systemUpdater.TryAddGameSystemImmediately(new CommandSystem(), autoInitialize: false);
        systemUpdater.TryAddGameSystemImmediately(new ItemsCleanupSystem(), autoInitialize: false);

        systemUpdater.TryInitialize();
    }
}

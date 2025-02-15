using System.Collections;
using System.Collections.Generic;
using MergeCase.Systems.Gameplay;
using MergeCase.Systems.Updater;
using UnityEngine;

public class GameplaySystemUpdater : MonoBehaviour
{
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.ShowInInspector]
#endif
    public SystemUpdater<GameplaySystemBase> SystemUpdater { get; private set; }

    void Awake()
    {
        SystemUpdater = new();
    }

    void Update()
    {
        SystemUpdater.TryUpdate();
    }

    void LateUpdate()
    {
        SystemUpdater.TryLateUpdate();
    }
}

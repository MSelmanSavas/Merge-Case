using System.Collections;
using System.Collections.Generic;
using MergeCase.General.Interfaces;
using MergeCase.Systems.Updater;
using UnityEngine;

namespace MergeCase.Systems.Updater
{
    public class SystemUpdateContext<T> where T : SystemBase
    {
        public IDataProvider DataProvider { get; private set; }
        public SystemUpdater<T> SystemUpdater { get; private set; }

        public SystemUpdateContext(IDataProvider dataProvider, SystemUpdater<T> systemUpdater)
        {
            DataProvider = dataProvider;
            SystemUpdater = systemUpdater;
        }
    }
}


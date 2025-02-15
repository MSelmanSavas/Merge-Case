using System.Collections;
using System.Collections.Generic;
using MergeCase.General.Interfaces;
using MergeCase.Systems.Updater;
using UnityEngine;

namespace MergeCase.Systems.Updater
{
    public class SystemUpdateContext<T> where T : SystemBase
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        public IDataCollection DataCollection { get; private set; }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        public SystemUpdater<T> SystemUpdater { get; private set; }

        public SystemUpdateContext(IDataCollection dataCollection, SystemUpdater<T> systemUpdater)
        {
            DataCollection = dataCollection;
            SystemUpdater = systemUpdater;
        }
    }
}


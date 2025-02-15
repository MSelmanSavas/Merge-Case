using System.Collections;
using System.Collections.Generic;
using MergeCase.General.Interfaces;
using UnityEngine;

namespace MergeCase.Systems.Updater
{
    public class SystemUpdateContextDataProvider : IDataProvider
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        Dictionary<System.Type, object> _datas = new();

        public bool TryAdd<T>(T data) where T : class
        {
            if (_datas.ContainsKey(typeof(T)))
            {
                return false;
            }

            _datas.Add(typeof(T), data);
            return true;
        }

        public bool TryGet<T>(out T value) where T : class
        {
            value = default(T);

            if (!_datas.TryGetValue(typeof(T), out var foundData))
            {
                return false;
            }

            value = foundData as T;
            return true;
        }

        public bool TryGetRef<T>(ref T value) where T : class
        {
            if (!_datas.TryGetValue(typeof(T), out var foundData))
            {
                return false;
            }

            value = foundData as T;
            return true;
        }

        public bool TryRemove<T>(T data) where T : class
        {
            if (!_datas.ContainsKey(typeof(T)))
            {
                return false;
            }

            _datas.Remove(typeof(T));
            return true;
        }
    }
}
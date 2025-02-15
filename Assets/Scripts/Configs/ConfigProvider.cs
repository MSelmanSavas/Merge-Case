using System.Collections;
using System.Collections.Generic;
using MergeCase.General.Interfaces;
using UnityEditor;
using UnityEngine;
using UsefulDataTypes;

namespace MergeCase.General.Config
{
    [CreateAssetMenu(menuName = "MergeCase/Configs/Create Config Provider", fileName = "DefaultConfigProvider")]
    public class ConfigProvider : ScriptableObject, IDataProvider
    {

        [SerializeField]
        List<ScriptableObject> _configList = new();

        [System.Serializable]
        class ConfigDictionary : SerializableDictionary<TypeReference, ScriptableObject> { }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ReadOnly]
#endif
        [SerializeField]
        ConfigDictionary _configs = new();

        public bool TryGet<T>(out T value) where T : class
        {
            value = default(T);

            if (!_configs.TryGetValue(typeof(T), out var foundData))
            {
                return false;
            }

            value = foundData as T;
            return true;
        }

        public bool TryGetRef<T>(ref T value) where T : class
        {
            if (!_configs.TryGetValue(typeof(T), out var foundData))
            {
                return false;
            }

            value = foundData as T;
            return true;
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            bool isAnythingChanged = false;

            foreach (var config in _configList)
            {
                if (_configs.TryGetValue(config.GetType(), out var foundConfig))
                {
                    if (foundConfig == config)
                    {
                        continue;
                    }

                    _configs.Remove(config.GetType());
                }

                _configs.Add(config.GetType(), config);
                isAnythingChanged = true;
            }

            if (isAnythingChanged)
            {
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssetIfDirty(this);
            }
        }
#endif
    }
}


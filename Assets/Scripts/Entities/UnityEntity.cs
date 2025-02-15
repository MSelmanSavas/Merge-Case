using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MergeCase.Entities.Unity
{
    public class UnityEntity : MonoBehaviour, IEntity
    {
        TypeReferenceInheritedFrom<IComponent> _entityComponentTypeRefCache = new();

        [SerializeField]
        EntityComponentDictionary _entityComponents = new();

        private void Awake()
        {
            foreach (var entityComponentKV in _entityComponents)
            {
                try
                {
                    if (!entityComponentKV.Value.TryInitialize(this))
                        throw new System.Exception($"Cannot initialize Component type : {entityComponentKV.Key}!");
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Error while initializing Component on {gameObject.name}! Error : {e}");
                }
            }

            AwakeExternal();
        }

        /// <summary>
        /// Any class that inherits this should use this for Awake calls.
        /// Used for preventing accidental overrides on Awake initialization.
        /// </summary>
        protected virtual void AwakeExternal()
        {

        }

        private void OnDisable()
        {
            foreach (var entityComponentKV in _entityComponents)
            {
                try
                {
                    if (!entityComponentKV.Value.TryReset(this))
                        throw new System.Exception($"Cannot initialize Component type : {entityComponentKV.Key}!");
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Error while initializing Component on {gameObject.name}! Error : {e}");
                }
            }

            OnDisableExternal();
        }

        /// <summary>
        /// Any class that inherits this should use this for OnDisable calls.
        /// Used for preventing accidental overrides on Awake initialization.
        /// </summary>
        protected virtual void OnDisableExternal()
        {

        }

        public virtual bool OnLoad() { return true; }
        public virtual bool OnAfterLoad() { return true; }
        public virtual bool OnSpawned() { return true; }

        public bool TryGetEntityComponent<T>(out T component) where T : IComponent, new()
        {
            _entityComponentTypeRefCache.Type = typeof(T);

            if (!_entityComponents.TryGetValue(_entityComponentTypeRefCache, out IComponent baseComponent))
            {
                component = default(T);
                return false;
            }

            component = (T)baseComponent;
            return component != null;
        }

        public bool TryAddEntityComponent<T>(T component) where T : IComponent, new()
        {
            _entityComponentTypeRefCache.Type = typeof(T);

            if (_entityComponents.ContainsKey(typeof(T)))
                return false;

            if (!component.TryInitialize(this))
                return false;

            _entityComponents.Add(typeof(T), component);
            return true;
        }

        public bool TryGetOrAddEntityComponent<T>(out T component) where T : IComponent, new()
        {
            if (TryGetEntityComponent(out component))
                return true;

            component = new T();
            return TryAddEntityComponent(component);
        }

        public bool TryRemoveEntityComponent<T>() where T : IComponent, new()
        {
            _entityComponentTypeRefCache.Type = typeof(T);

            if (!_entityComponents.ContainsKey(_entityComponentTypeRefCache))
                return false;

            return _entityComponents.Remove(_entityComponentTypeRefCache);
        }

        public bool TryRemoveEntityComponent<T>(T component) where T : IComponent, new()
        {
            _entityComponentTypeRefCache.Type = component.GetType();

            if (!_entityComponents.ContainsKey(_entityComponentTypeRefCache))
                return false;

            return _entityComponents.Remove(_entityComponentTypeRefCache);
        }
    }
}


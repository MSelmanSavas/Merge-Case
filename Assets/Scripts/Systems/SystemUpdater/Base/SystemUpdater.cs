using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MergeCase.General.Interfaces;
using UnityEngine;

namespace MergeCase.Systems.Updater
{
    public class SystemUpdater<T> : IInitializable, IUpdateable, ILateUpdateable where T : SystemBase
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        public SystemUpdateContext<T> UpdateContext { get; private set; }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        public bool IsInitialized { get; private set; }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        List<T> _updateGameSystems = new();

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        List<T> _lateUpdateGameSystems = new();

        List<T> _updateSystemsToBeAdded = new();
        List<T> _lateUpdateSystemsToBeAdded = new();

        List<T> _updateSystemsToBeRemoved = new();
        List<T> _lateUpdateSystemsToBeRemoved = new();

        public SystemUpdater()
        {
            UpdateContext = new(new SystemUpdateContextDataCollection(), this);
        }

        public bool TryInitialize()
        {
            IsInitialized = InitializeSystems();
            return true;
        }

        public bool TryDeInitialize()
        {
            return true;
        }

        private bool InitializeSystems()
        {
            foreach (var system in _updateGameSystems)
            {
                if (system is IInitializable<SystemUpdateContext<T>> initializableWithContext)
                {
                    if (!initializableWithContext.TryInitialize(UpdateContext))
                    {
                        UnityLogger.LogErrorWithTag($"Error while trying to initialize system : {system}! Cannot continue initializing update game systems!");
                        return false;
                    }
                }
                else if (system is IInitializable initializable)
                {
                    if (!initializable.TryInitialize())
                    {
                        UnityLogger.LogErrorWithTag($"Error while trying to initialize system : {system}! Cannot continue initializing update game systems!");
                        return false;
                    }
                }
            }

            foreach (var system in _lateUpdateGameSystems)
            {
                if (system is IInitializable<SystemUpdateContext<T>> initializableWithContext)
                {
                    if (!initializableWithContext.TryInitialize(UpdateContext))
                    {
                        UnityLogger.LogErrorWithTag($"Error while trying to initialize late update system : {system}! Cannot continue initializing late update game systems!");
                        return false;
                    }
                }
                else if (system is IInitializable initializable)
                {
                    if (!initializable.TryInitialize())
                    {
                        UnityLogger.LogErrorWithTag($"Error while trying to initialize system : {system}! Cannot continue initializing update game systems!");
                        return false;
                    }
                }
            }

            return true;
        }

        public void DeInitialize()
        {
            DeInitializeSystems();
        }

        private bool DeInitializeSystems()
        {
            foreach (var system in _updateGameSystems)
            {
                if (system is IInitializable<SystemUpdateContext<T>> initializableWithContext)
                {
                    if (initializableWithContext.TryDeInitialize(UpdateContext))
                    {
                        UnityLogger.LogErrorWithTag($"Error while trying to deinitialize system : {system}! Cannot continue deinitializing update game systems!");
                        return false;
                    }
                }
                else if (system is IInitializable initializable)
                {
                    if (initializable.TryDeInitialize())
                    {
                        UnityLogger.LogErrorWithTag($"Error while trying to deinitialize system : {system}! Cannot continue deinitializing update game systems!");
                        return false;
                    }
                }

                _updateGameSystems.Clear();

            }

            foreach (var system in _lateUpdateGameSystems)
            {
                if (system is IInitializable<SystemUpdateContext<T>> initializableWithContext)
                {
                    if (!initializableWithContext.TryDeInitialize(UpdateContext))
                    {
                        UnityLogger.LogErrorWithTag($"Error while trying to deinitialize late update system : {system}! Cannot continue deinitializing late update game systems!");
                        return false;
                    }
                }
                else if (system is IInitializable initializable)
                {
                    if (initializable.TryDeInitialize())
                    {
                        UnityLogger.LogErrorWithTag($"Error while trying to deinitialize system : {system}! Cannot continue deinitializing update game systems!");
                        return false;
                    }
                }
            }

            _lateUpdateGameSystems.Clear();

            _updateSystemsToBeAdded.Clear();
            _lateUpdateSystemsToBeAdded.Clear();

            _updateSystemsToBeRemoved.Clear();
            _lateUpdateSystemsToBeRemoved.Clear();

            return true;
        }


        public bool TryAddGameSystemImmediately(T gameSystem, bool autoInitialize = true)
        {
            if (gameSystem.IsMarkedForRemoval)
                return false;

            if (autoInitialize)
            {
                if (gameSystem is IInitializable<SystemUpdateContext<T>> initializableWithContext)
                {
                    if (!initializableWithContext.TryInitialize(UpdateContext))
                        return false;
                }
                else if (gameSystem is IInitializable initializable)
                {
                    if (!initializable.TryInitialize())
                        return false;
                }
            }

            _updateGameSystems.Add(gameSystem);
            return true;
        }

        public bool TryAddGameSystemByTypeImmediately<T1>(bool autoInitialize = true) where T1 : T
        {
            T gameSystem = Activator.CreateInstance(typeof(T1)) as T;
            return TryAddGameSystemImmediately(gameSystem, autoInitialize);
        }

        public bool TryAddGameSystem(T gameSystem, bool autoInitialize = true)
        {
            if (gameSystem.IsMarkedForRemoval)
                return false;

            if (autoInitialize)
            {
                if (gameSystem is IInitializable<SystemUpdateContext<T>> initializableWithContext)
                {
                    if (!initializableWithContext.TryInitialize(UpdateContext))
                        return false;
                }
                else if (gameSystem is IInitializable initializable)
                {
                    if (!initializable.TryInitialize())
                        return false;
                }
            }

            _updateSystemsToBeAdded.Add(gameSystem);
            return true;
        }

        public bool TryAddGameSystemByType<T1>(bool autoInitialize = true) where T1 : T
        {
            T gameSystem = Activator.CreateInstance(typeof(T)) as T;
            return TryAddGameSystem(gameSystem, autoInitialize);
        }

        public bool TryGetGameSystem<T1>(out T1 gameSystem) where T1 : T
        {
            T foundGameSystem = _updateGameSystems.Where(x => x.GetType() == typeof(T1)).FirstOrDefault();

            if (foundGameSystem == null)
            {
                gameSystem = null;
                return false;
            }

            if (foundGameSystem.IsMarkedForRemoval)
            {
                gameSystem = null;
                return false;
            }

            gameSystem = foundGameSystem as T1;
            return true;
        }

        public bool TryGetGameSystemByType<T1>(out T1 gameSystem) where T1 : class
        {
            T foundGameSystem = _updateGameSystems.Where(x => typeof(T1).IsAssignableFrom(x.GetType())).FirstOrDefault();

            if (foundGameSystem == null)
            {
                gameSystem = null;
                return false;
            }

            if (foundGameSystem.IsMarkedForRemoval)
            {
                gameSystem = null;
                return false;
            }

            gameSystem = foundGameSystem as T1;
            return true;
        }

        public bool TryRemoveGameSystem(T gameSystem)
        {
            if (!_updateGameSystems.Contains(gameSystem))
                return false;

            _updateSystemsToBeRemoved.Add(gameSystem);
            gameSystem.IsMarkedForRemoval = true;
            return true;
        }

        public bool TryRemoveGameSystemByType<T1>() where T1 : T
        {
            T foundGameSystem = _updateGameSystems.Where(x => x.GetType() == typeof(T1)).First();

            if (foundGameSystem == null)
                return false;

            _updateSystemsToBeRemoved.Add(foundGameSystem);
            foundGameSystem.IsMarkedForRemoval = true;
            return true;
        }

        public bool TryRemoveGameSystemByType(Type type)
        {
            T foundGameSystem = _updateGameSystems.Where(x => x.GetType() == type).First();

            if (foundGameSystem == null)
                return false;

            _updateSystemsToBeRemoved.Add(foundGameSystem);
            foundGameSystem.IsMarkedForRemoval = true;
            return true;
        }

        public bool TryAddLateUpdateGameSystemImmediately(T gameSystem, bool autoInitialize = true)
        {
            if (gameSystem.IsMarkedForRemoval)
                return false;

            if (autoInitialize)
            {
                if (gameSystem is IInitializable<SystemUpdateContext<T>> initializableWithContext)
                {
                    if (!initializableWithContext.TryInitialize(UpdateContext))
                        return false;
                }
                else if (gameSystem is IInitializable initializable)
                {
                    if (!initializable.TryInitialize())
                        return false;
                }
            }

            _lateUpdateGameSystems.Add(gameSystem);
            return true;
        }

        public bool TryAddLateUpdateGameSystemByTypeImmediately<T1>(bool autoInitialize = true) where T1 : T
        {
            T gameSystem = Activator.CreateInstance(typeof(T1)) as T;
            return TryAddLateUpdateGameSystemImmediately(gameSystem, autoInitialize);
        }

        public bool TryAddLateUpdateGameSystem(T gameSystem, bool autoInitialize = true)
        {
            if (gameSystem.IsMarkedForRemoval)
                return false;

            if (autoInitialize)
            {
                if (gameSystem is IInitializable<SystemUpdateContext<T>> initializableWithContext)
                {
                    if (!initializableWithContext.TryInitialize(UpdateContext))
                        return false;
                }
                else if (gameSystem is IInitializable initializable)
                {
                    if (!initializable.TryInitialize())
                        return false;
                }
            }

            _lateUpdateSystemsToBeAdded.Add(gameSystem);
            return true;
        }

        public bool TryAddLateUpdateGameSystemByType<T1>(bool autoInitialize = true) where T1 : T
        {
            T gameSystem = Activator.CreateInstance(typeof(T1)) as T;
            return TryAddLateUpdateGameSystem(gameSystem, autoInitialize);
        }

        public bool TryGetLateUpdateGameSystemByType<T1>(out T1 gameSystem) where T1 : T
        {
            T foundGameSystem = _lateUpdateGameSystems.Where(x => x.GetType() == typeof(T1)).First();

            if (foundGameSystem == null)
            {
                gameSystem = null;
                return false;
            }

            if (foundGameSystem.IsMarkedForRemoval)
            {
                gameSystem = null;
                return false;
            }

            gameSystem = foundGameSystem as T1;
            return true;
        }

        public bool TryRemoveLateUpdateGameSystem(T gameSystem)
        {
            if (!_lateUpdateGameSystems.Contains(gameSystem))
                return false;

            _lateUpdateSystemsToBeRemoved.Add(gameSystem);
            gameSystem.IsMarkedForRemoval = true;
            return true;
        }

        public bool TryRemoveLateUpdateGameSystemByType<T1>() where T1 : T
        {
            T foundGameSystem = _lateUpdateGameSystems.Where(x => x.GetType() == typeof(T1)).First();

            if (foundGameSystem == null)
                return false;

            _lateUpdateSystemsToBeRemoved.Add(foundGameSystem);
            foundGameSystem.IsMarkedForRemoval = true;
            return true;
        }

        public bool TryRemoveLateUpdateGameSystemByType(Type type)
        {
            T foundGameSystem = _lateUpdateGameSystems.Where(x => x.GetType() == type).First();

            if (foundGameSystem == null)
                return false;

            _lateUpdateSystemsToBeRemoved.Add(foundGameSystem);
            foundGameSystem.IsMarkedForRemoval = true;
            return true;
        }

        void AddToBeAddedGameSystems()
        {
            foreach (var system in _updateSystemsToBeAdded)
                _updateGameSystems.Add(system);

            foreach (var system in _lateUpdateSystemsToBeAdded)
                _lateUpdateGameSystems.Add(system);

            _updateSystemsToBeAdded.Clear();
            _lateUpdateSystemsToBeAdded.Clear();
        }

        void RemoveToBeRemovedGameSystems()
        {
            foreach (var system in _updateSystemsToBeRemoved)
                _updateGameSystems.Remove(system);

            foreach (var system in _lateUpdateSystemsToBeRemoved)
                _lateUpdateGameSystems.Remove(system);

            _updateSystemsToBeRemoved.Clear();
            _lateUpdateSystemsToBeRemoved.Clear();
        }

        public bool TryUpdate()
        {
            if (!IsInitialized)
                return false;

            foreach (var system in _updateGameSystems)
            {
                if (system.IsMarkedForRemoval)
                    continue;

                if (system is IUpdateable<SystemUpdateContext<T>> updateableWithContext)
                {
                    if (!updateableWithContext.TryUpdate(UpdateContext))
                    {
                        UnityLogger.LogErrorWithTag($"Error while trying to update system : {system}!");
                    }
                }
                if (system is IUpdateable updateable)
                {
                    if (!updateable.TryUpdate())
                    {
                        UnityLogger.LogErrorWithTag($"Error while trying to update system : {system}!");
                    }
                }
            }

            return true;
        }

        public bool TryLateUpdate()
        {
            if (!IsInitialized)
                return false;

            foreach (var system in _lateUpdateGameSystems)
            {
                if (system.IsMarkedForRemoval)
                    continue;

                if (system is ILateUpdateable<SystemUpdateContext<T>> lateUpdateableWithContext)
                {
                    if (!lateUpdateableWithContext.TryLateUpdate(UpdateContext))
                    {
                        UnityLogger.LogErrorWithTag($"Error while trying to LateUpdate system : {system}!");
                    }
                }
                else if (system is ILateUpdateable lateUpdateable)
                {
                    if (!lateUpdateable.TryLateUpdate())
                    {
                        UnityLogger.LogErrorWithTag($"Error while trying to LateUpdate system : {system}!");
                    }
                }
            }

            RemoveToBeRemovedGameSystems();
            AddToBeAddedGameSystems();

            return true;
        }
    }
}

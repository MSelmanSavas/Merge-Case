using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MergeCase.Entities
{
    public interface IEntity
    {
        public bool IsActive { get; set; }
        public bool IsQueryable { get; set; }
        public bool IsMarkedToBeRemoved { get; set; }
        public virtual bool OnLoad() { return true; }
        public virtual bool OnAfterLoad() { return true; }
        public virtual bool OnSpawned() { return true; }
        public bool TryGetEntityComponent<T>(out T Component) where T : IComponent, new();
        public bool TryAddEntityComponent<T>(T Component) where T : IComponent, new();
        public bool TryGetOrAddEntityComponent<T>(out T Component) where T : IComponent, new();
        public bool TryRemoveEntityComponent<T>() where T : IComponent, new();
        public bool TryRemoveEntityComponent<T>(T component) where T : IComponent, new();
    }
}

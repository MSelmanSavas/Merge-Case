using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MergeCase.Entities
{
    public interface IEntityCollection<T> : IEntityProvider<T>
    {
        public bool TryAddEntity(T entityQueryData, IEntity entity);
        public bool TryRemoveEntity(T entityQueryData);
    }

    public interface IEntityCollection<T1, T2> : IEntityProvider<T1, T2> where T2 : IEntity
    {
        public bool TryAddEntity(T1 entityQueryData, T2 entity);
        public bool TryRemoveEntity(T1 entityQueryData);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MergeCase.Entities
{
    public interface IEntityProvider<T>
    {
        public bool TryGetEntity(T entityQueryData, out IEntity entity);
    }

    public interface IEntityProvider<T1, T2> where T2 : IEntity
    {
        public bool TryGetEntity(T1 entityQueryData, out T2 entity);
    }
}


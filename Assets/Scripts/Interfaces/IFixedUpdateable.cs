using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MergeCase.General.Interfaces
{
    public interface IFixedUpdateable
    {
        public bool TryFixedUpdate();
    }

    public interface IFixedUpdateable<T>
    {
        public bool TryFixedUpdate(T data);
    }
}
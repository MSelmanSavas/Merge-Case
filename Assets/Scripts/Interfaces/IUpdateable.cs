using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MergeCase.General.Interfaces
{
    public interface IUpdateable
    {
        public bool TryUpdate();
    }

    public interface IUpdateable<T>
    {
        public bool TryUpdate(T data);
    }
}

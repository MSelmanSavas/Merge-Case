using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MergeCase.General.Interfaces
{
    public interface IDataProvider
    {
        public bool TryGet<T>(out T value) where T : class;
        public bool TryGetRef<T>(ref T value) where T : class;
    }
}


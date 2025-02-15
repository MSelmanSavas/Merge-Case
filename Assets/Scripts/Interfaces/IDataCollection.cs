using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MergeCase.General.Interfaces
{
    public interface IDataCollection : IDataProvider
    {
        public bool TryAdd<T>(T data) where T : class;
        public bool TryRemove<T>(T data) where T : class;
    }
}
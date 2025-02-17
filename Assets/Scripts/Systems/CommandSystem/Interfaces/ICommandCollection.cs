using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MergeCase.Systems.Command.Interfaces
{
    public interface ICommandCollection : ICommandProvider
    {
        public bool TryAdd<T>(T command) where T : ICommand;
        public bool TryRemove<T>(T command) where T : ICommand;
    }
}


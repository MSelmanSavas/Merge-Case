using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MergeCase.Systems.Command.Interfaces
{
    public interface ICommandProvider
    {
        public bool TryGet<T>(out T command) where T : ICommand;
        public bool TryGetAll<T>(out ICollection<T> commands) where T : ICommand;
    }
}

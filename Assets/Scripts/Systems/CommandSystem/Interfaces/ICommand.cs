using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MergeCase.Systems.Command.Interfaces
{
    public interface ICommand
    {
        bool TryInitialize();
        bool TryUpdate();
        bool IsCompleted();
    }

    public interface ICommand<T>
    {
        bool TryInitialize(T data);
        bool TryUpdate(T data);
        bool IsCompleted();
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInitializable
{
    bool TryInitialize();
    bool TryDeInitialize();
}

public interface IInitializable<T>
{
    bool TryInitialize(T data);
    bool TryDeInitialize(T data);
}

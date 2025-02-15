using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILateUpdateable
{
    public bool TryLateUpdate();
}

public interface ILateUpdateable<T>
{
    public bool TryLateUpdate(T data);
}
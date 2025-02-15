using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpdateable
{
    public bool TryUpdate();
}

public interface IUpdateable<T>
{
    public bool TryUpdate(T data);
}

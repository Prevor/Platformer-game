using System;
using UnityEngine;

public interface IPoolable
{
    GameObject GameObject { get; }
    event Action<IPoolable> Destroyed;
    void Reset();
}
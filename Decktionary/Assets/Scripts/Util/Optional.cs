using System;
using UnityEngine;

[Serializable]
public struct Optional<T>
{
    [SerializeField] private bool enabled;
    [SerializeField] private T value;

    public bool Enabled => enabled;
    public T Value => value;

    public Optional(T initialValue, bool enabled)
    {
	   this.enabled = enabled;
	   value = initialValue;
    }
}
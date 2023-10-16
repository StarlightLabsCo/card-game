using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentSingletonBehaviour<T> : SingletonBehaviour<T> where T : Component
{
    protected override void Awake()
    {
        base.Awake();
        if (instance == this)
        {
            if (transform.parent == null)
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }
    }
}
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SingletonBehaviour<T> : SerializedMonoBehaviour where T : Component
{
    private static T _instance;

    public static T instance
    {
	   get
	   {
		  if (_instance == null)
		  {
			 T sceneInstance = FindObjectOfType<T>();
			 // if (sceneInstance == null)
			 // {
			 //     Debug.LogError($"Can't find instance for singleton of type \"{typeof(T).Name}\"");
			 // }
			 _instance = sceneInstance;
		  }

		  return _instance;
	   }
    }

    public static bool HasInstance()
    {
	   return _instance != null || FindObjectOfType<T>() != null;
    }

    protected virtual void Awake()
    {
	   if (instance != this)
	   {
		  Destroy(this.gameObject);
	   }
    }
}

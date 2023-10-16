using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Starlight.UI
{
    /// <summary>
    /// An interface for UI elements that can transition in and out with their own implemented behaviors for doing so
    /// </summary>
    public interface IHasTransition
    {
	   IEnumerator TransitionIn(float duration);
	   IEnumerator TransitionOut(float duration);
    }
}
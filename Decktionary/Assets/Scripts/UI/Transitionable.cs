using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Starlight.UI
{
    /// <summary>
    /// Abstract class for all transitionable classes (e.g. custom button transitions)
    /// </summary>
    public abstract class Transitionable : MonoBehaviour, IHasTransition
    {
	   [SerializeField] bool inByDefault = true;
	   protected RectTransform rectTransform;

	   private void Awake()
	   {
		  rectTransform = transform as RectTransform;
		  //default state
		  StartCoroutine(inByDefault ? TransitionIn(0f) : TransitionOut(0f));
	   }

	   public abstract IEnumerator TransitionIn(float duration);
	   public abstract IEnumerator TransitionOut(float duration);


    }
}
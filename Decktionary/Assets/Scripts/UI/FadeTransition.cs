using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Starlight.UI
{
    public class FadeTransition : Transitionable
    {
	   [SerializeField] CanvasGroup group;
	   [SerializeField] float inFade;
	   [SerializeField] float outFade;

	   public override IEnumerator TransitionIn(float duration)
	   {
		  group.DOFade(inFade, duration).SetEase(Ease.OutSine).SetUpdate(true);
		  yield return new WaitForSecondsRealtime(duration);
		  onTransitionInFinished?.Invoke();
	   }

	   public override IEnumerator TransitionOut(float duration)
	   {
		  group.DOFade(outFade, duration).SetEase(Ease.InSine).SetUpdate(true);
		  yield return new WaitForSecondsRealtime(duration);
		  onTransitionOutFinished?.Invoke();
	   }
    }
}
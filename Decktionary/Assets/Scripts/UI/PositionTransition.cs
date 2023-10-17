using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Starlight.UI
{
    public class PositionTransition : Transitionable
    {
        [SerializeField] Vector2 inPos;
	   [SerializeField] Vector2 outPos;

	   public override IEnumerator TransitionIn(float duration)
        {
            rectTransform.DOAnchorPos(inPos, duration).SetEase(Ease.OutSine).SetUpdate(true);
            yield return new WaitForSecondsRealtime(duration);
            onTransitionInFinished?.Invoke();
	   }

	   public override IEnumerator TransitionOut(float duration)
        {
            rectTransform.DOAnchorPos(outPos, duration).SetEase(Ease.InSine).SetUpdate(true);
            yield return new WaitForSecondsRealtime(duration);
            onTransitionOutFinished?.Invoke();
	   }
    }
}
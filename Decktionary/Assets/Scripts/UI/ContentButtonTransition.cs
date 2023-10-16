using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Starlight.UI
{
    public class ContentButtonTransition : Transitionable
    {
        [SerializeField] GameObject content;
        [SerializeField] Vector2 normalSize;

        Tween sizeTween;

        public override IEnumerator TransitionIn(float duration)
        {
            sizeTween?.Kill();
		  sizeTween = rectTransform.DOSizeDelta(normalSize, duration).SetEase(Ease.OutBack).SetUpdate(true);
            yield return new WaitForSecondsRealtime(duration);
            content.SetActive(true);
	   }

	   public override IEnumerator TransitionOut(float duration)
        {
            content.SetActive(false);
            sizeTween?.Kill();
		  sizeTween = rectTransform.DOSizeDelta(Vector2.zero, duration).SetEase(Ease.InSine).SetUpdate(true);
            yield return new WaitForSecondsRealtime(duration);
	   }
    }
}
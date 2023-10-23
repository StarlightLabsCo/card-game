using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Starlight.UI.BattleUICursor;

namespace Starlight.UI
{
    public class HammerUI : MonoBehaviour
    {
	   [SerializeField] GameObject hammerIcon;
	   [SerializeField] RectTransform hammerArea;
	   [SerializeField] Button hammerButton;
	   [SerializeField] LayoutGroup layoutGroup;
	   [SerializeField] Vector2 closedSize;
	   [SerializeField] Vector2 openSize;
	   [SerializeField] float transitionTime;

	   Tween sizeTween;

	   public void OnCursorChanged(CursorType cursorType, CursorType oldType)
        {
		  //print($"Cursor changed: {oldType} to {cursorType}.");
		  sizeTween?.Kill();

		  if (cursorType == CursorType.Hammer)
		  {
			 hammerIcon.SetActive(false);
			 sizeTween = hammerArea.DOSizeDelta(openSize, transitionTime).SetEase(Ease.OutSine);
			 layoutGroup.enabled = true;
			 hammerButton.enabled = false;
		  } 
		  else if(oldType == CursorType.Hammer)
		  {
			 sizeTween = hammerArea.DOSizeDelta(closedSize, transitionTime).SetEase(Ease.InSine).OnComplete(() => hammerIcon.SetActive(true));
			 layoutGroup.enabled = false;
			 hammerButton.enabled = true;
		  }
	   }
    }
}
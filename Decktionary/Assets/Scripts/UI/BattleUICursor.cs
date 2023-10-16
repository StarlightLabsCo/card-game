using Sirenix.OdinInspector;
using Starlight.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Starlight.UI
{
    public class BattleUICursor : SingletonBehaviour<BattleUICursor>
    {
	   public CursorType CurrentCursorType { get; private set; }


	   [SerializeField] RectTransform cursorAnchor;
	   [SerializeField] Image cursorModeImage;
	   [SerializeField] Sprite normalSprite;
	   [SerializeField] Sprite hammerSprite;

	   [Space]

	   [SerializeField] UnityEvent<CursorType, CursorType> onCursorChanged;

	   public enum CursorType
	   {
		  None,
		  Hammer
	   }

	   public void SwitchCursorTypeInt(int newType)
	   {
		  this.SwitchCursorType((CursorType)newType);
	   }

	   [Button]
	   public void SwitchCursorType(CursorType newType)
	   {
		  CursorType oldType = CurrentCursorType;
		  //each cursor type has their own sprites/behaviors
		  switch (newType)
		  {
			 case CursorType.Hammer:
				cursorModeImage.sprite = hammerSprite;
				break;
			 default:
				cursorModeImage.sprite = normalSprite;
				break;
		  }

		  cursorModeImage.rectTransform.sizeDelta = cursorModeImage.sprite.rect.size;

		  CurrentCursorType = newType;
		  onCursorChanged?.Invoke(CurrentCursorType, oldType);
	   }

	   private void Update()
	   {
		  Cursor.visible = false;
		  cursorAnchor.transform.position = GameCamera.instance.CursorPosWorld;
	   }
    }
}
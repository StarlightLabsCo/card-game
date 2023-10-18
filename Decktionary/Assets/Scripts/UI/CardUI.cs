using Starlight.Managers;
using Starlight.Words;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Starlight.UI
{
    public class CardUI : MonoBehaviour
    {
	   [SerializeField] TMP_Text titleText;
	   [SerializeField] TMP_Text titleTextShadow;
	   [Space]
	   [SerializeField] Image cardIcon;
	   [Space]
	   [SerializeField] TMP_Text descriptionText;

	   [Space]
	   [Header("Dragging")]
	   [SerializeField] float scaleHeight;
	   [SerializeField] float scaleAreaHeight;
	   [SerializeField] Vector2 normalSize;
	   [SerializeField] Vector2 scaledSize;

	   RectTransform rTrans;

	   Transform tmpParent;
	   int tmpSiblingIndex;
	   Vector2 dragOffset;
	   Vector2 dragOffsetScaled;


	   public bool IsBeingDragged { get; private set; } = false;
	   public event Action onPlaced;

	   public CardData Data { get; private set; }

	   public void SetCardData(CardData data)
	   {
		  var title = data.GetPrompt();
		  titleText.text = title;
		  titleTextShadow.text = title;
		  descriptionText.text = data.Description;

		  cardIcon.sprite = data.Icon;

		  Data = data;
	   }

	   private void Awake()
	   {
		  rTrans = transform as RectTransform;
	   }

	   private void Update()
	   {
		  if (!IsBeingDragged) return;
		  float top = scaleHeight + scaleAreaHeight / 2f;
		  float bottom = scaleHeight - scaleAreaHeight / 2f;
		  var scaleBlend = (Mathf.Clamp(rTrans.position.y, bottom, top) - bottom) / scaleAreaHeight;
		  transform.localScale = Vector2.Lerp(normalSize, scaledSize, scaleBlend);
		  dragOffsetScaled = Vector2.Lerp(dragOffset, scaledSize * dragOffset, scaleBlend);
		  transform.position = new Vector3(0f, 0f, transform.position.z) + (Vector3)(dragOffsetScaled + GameCamera.instance.CursorPosWorld);
	   }

	   public void OnStartDrag()
	   {
		  IsBeingDragged = true;
		  tmpParent = transform.parent;
		  tmpSiblingIndex = transform.GetSiblingIndex();
		  transform.SetParent(BattleUICursor.instance.canvas.transform);
		  transform.SetSiblingIndex(transform.parent.childCount - 2);
		  dragOffset = (Vector2)transform.position - GameCamera.instance.CursorPosWorld;
	   }

	   public void OnEndDrag()
	   {
		  IsBeingDragged = false;
		  transform.SetParent(tmpParent);
		  transform.SetSiblingIndex(tmpSiblingIndex);
		  //if on card spot, invoke onPlaced
		  bool placed = false;
		  if(placed)
		  {
			 onPlaced?.Invoke();
		  } else
		  {
			 transform.localScale = normalSize;
		  }
	   }
    }
}
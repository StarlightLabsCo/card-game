using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Starlight.UI
{
    public class CardSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
    {
	   [SerializeField] Image slotImage;
	   [SerializeField] Color normalColor;
	   [SerializeField] Color highlightedColor;

	   public bool IsHighlighted { get; private set; } = false;
	   public CardUI Card { get; private set; }

	   public void OnDrop(PointerEventData eventData)
	   {
		  var dragObj = eventData.pointerDrag;
		  //stop if not a card being dragged
		  if (!dragObj.TryGetComponent(out CardUI card)) return;
		  BattleUICursor.instance.cardMovementArrow.gameObject.SetActive(false);
		  SetHighlighted(false);
		  card.SetSlot(this);
		  SetCard(card);
	   }

	   public void SetCard(CardUI card)
	   {
		  Card = card;

		  //align to parent
		  Card.transform.SetParent(transform);
	   }

	   public void OnPointerEnter(PointerEventData eventData)
	   {
		  var dragged = eventData.pointerDrag;
		  if (!dragged || !dragged.TryGetComponent(out CardUI card) || !card.CanBePlacedOn(this)) return;
		  SetHighlighted(true);

		  if(card.Slot)
		  {
			 BattleUICursor.instance.cardMovementArrow.gameObject.SetActive(true);
			 BattleUICursor.instance.cardMovementArrow.SetPoint(0, (Vector2)transform.position);
		  }
	   }

	   public void OnPointerExit(PointerEventData eventData)
	   {
		  var dragged = eventData.pointerDrag;
		  if (!dragged || !dragged.TryGetComponent(out CardUI card) || !card.CanBePlacedOn(this)) return;
		  SetHighlighted(false);

		  if (card.Slot)
		  {
			 BattleUICursor.instance.cardMovementArrow.gameObject.SetActive(false);
		  }
	   }

	   public void SetHighlighted(bool highlighted)
	   {
		  //maybe add better highlight effects later?

		  IsHighlighted = highlighted;
		  slotImage.color = highlighted ? highlightedColor : normalColor;
	   }
    }
}
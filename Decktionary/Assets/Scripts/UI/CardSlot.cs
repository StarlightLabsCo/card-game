using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Starlight.UI
{
    public class CardSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
    {
	   [SerializeField] CardSlot nextSlot;
	   [SerializeField] CardSlot attackSlot;
	   public CardSlotTeam Team;

	   [SerializeField] Image slotImage;
	   [SerializeField] Color normalColor;
	   [SerializeField] Color highlightedColor;

	   public bool IsHighlighted { get; private set; } = false;
	   public CardUI Card { get; private set; }

	   public const float CARD_MOVE_DURATION = 0.25f;

#if UNITY_EDITOR
	   private void OnDrawGizmosSelected()
	   {
		  if(nextSlot)
		  {
			 Handles.color = Color.green;
			 Handles.Button(transform.position, Quaternion.LookRotation(transform.position.DirVecTo(nextSlot.transform.position)), Vector2.Distance(transform.position, nextSlot.transform.position), 1f, Handles.ArrowHandleCap);
		  }

		  if (attackSlot)
		  {
			 Handles.color = Color.red;
			 Handles.Button(transform.position, Quaternion.LookRotation(transform.position.DirVecTo(attackSlot.transform.position)), Vector2.Distance(transform.position, attackSlot.transform.position), 1f, Handles.ArrowHandleCap);
		  }
	   }
#endif

	   public IEnumerator ExecuteSlotTurn(System.Random seededRandom, int turn)
	   {
		  if (!Card) yield break;
		  if(nextSlot)
		  {
			 yield return Card.TransitionToSlot(nextSlot);
			 yield break;
		  }
		  if(attackSlot)
		  {
			 yield return Card.ExecuteCardTurn(seededRandom, turn, attackSlot);
		  }
	   }

	   public void OnDrop(PointerEventData eventData)
	   {
		  var dragObj = eventData.pointerDrag;
		  //stop if not a card being dragged
		  if (!dragObj.TryGetComponent(out CardUI card) || !card.CanBePlacedOn(this)) return;
		  BattleUICursor.instance.cardMovementArrow.gameObject.SetActive(false);
		  SetHighlighted(false);
		  card.StartCoroutine(card.TransitionToSlot(this));
	   }

	   public void SetCard(CardUI card)
	   {
		  Card = card;
		  if (!Card) return;
		  //align to parent
		  Card.transform.SetParent(transform, true);
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
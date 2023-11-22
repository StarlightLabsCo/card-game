using DG.Tweening;
using Starlight.Managers;
using Starlight.Words;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Starlight.UI
{
	public class CardUI : MonoBehaviour
	{
		[SerializeField] CanvasGroup group;
		[SerializeField] TMP_Text titleText;
		[SerializeField] TMP_Text titleTextShadow;
		[Space]
		[SerializeField] Image cardIcon;
		[Space]
		[SerializeField] TMP_Text descriptionText;
		[Space]
		[SerializeField] TMP_Text healthText;
		[SerializeField] TMP_Text damageText;

		[Space]
		[Header("Dragging")]
		[SerializeField] float fadeAmount;
		[SerializeField] float fadeDuration;
		[SerializeField] float scaleHeight;
		[SerializeField] float scaleAreaHeight;
		[SerializeField] Vector2 normalSize;
		[SerializeField] Vector2 scaledSize;

		RectTransform rTrans;

		Transform tmpParent;
		int tmpSiblingIndex;
		Vector2 dragOffset;
		Vector2 dragOffsetScaled;

		Tween fadeTween;

		public bool IsBeingDragged { get; private set; } = false;

		public CardSlot Slot { get; private set; }
		public CardData Data { get; private set; }


		public void SetSlot(CardSlot slot)
		{
			if (Slot) Slot.SetCard(null);
			this.Slot = slot;
		}

		public void SetCardData(CardData data)
		{
			var title = data.GetPrompt();
			titleText.text = title;
			titleTextShadow.text = title;

			UpdateIcon(data.Icon);
			UpdateDescription(data.Description);
			UpdateHealthText(data.Health);
			UpdateDamageText(data.Damage);

			data.onIconUpdated += UpdateIcon;
			data.onDescriptionUpdated += UpdateDescription;
			data.onHealthUpdated += UpdateHealthText;
			data.onDamageUpdated += UpdateDamageText;

			Data = data;
		}

		void UpdateIcon(Sprite icon)
		{
			cardIcon.sprite = icon;
		}

		void UpdateDescription(string desc)
		{
			descriptionText.text = desc;
		}

		void UpdateHealthText(int health)
		{
			healthText.text = health.ToString();
		}

		void UpdateDamageText(int damage)
		{
			damageText.text = damage.ToString();
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
			if (Slot)
			{
				var arrow = BattleUICursor.instance.cardMovementArrow;
				arrow.SetPointCount(2);
				arrow.SetPoint(1, (Vector2)transform.position);
				return;
			}
			IsBeingDragged = true;
			tmpParent = transform.parent;
			tmpSiblingIndex = transform.GetSiblingIndex();
			transform.SetParent(BattleUICursor.instance.canvas.transform);
			transform.SetSiblingIndex(transform.parent.childCount - 2);
			dragOffset = (Vector2)transform.position - GameCamera.instance.CursorPosWorld;

			//fade
			fadeTween = group.DOFade(fadeAmount, fadeDuration).SetUpdate(true);
			group.blocksRaycasts = false;
		}

		public void OnEndDrag()
		{
			//came from hand
			IsBeingDragged = false;
			//card has been placed if there's a slot
			bool placed = Slot;
			group.blocksRaycasts = true;
			//if on card spot, invoke onPlaced
			if (placed)
			{
				//TODO: Do stuff?
				transform.localScale = scaledSize;
				OnPlaced(Slot);
			}
			else
			{
				transform.SetParent(tmpParent);
				transform.SetSiblingIndex(tmpSiblingIndex);
				transform.localScale = normalSize;

				fadeTween?.Kill();
				group.alpha = 1f;
			}

			//fade
			fadeTween?.Kill();
			group.alpha = 1f;
		}

		public IEnumerator ExecuteCardTurn(CardSlot[,] slotGrid)
		{
			//TODO: Implement card turn actions
			print($"Card turn executed: {Data}");
			yield return null;
		}

		/// <summary>
		/// Called when a card is placed on a slot.
		/// </summary>
		/// <param name="slot">The slot the card has been placed on.</param>
		public void OnPlaced(CardSlot slot)
		{
		}


		/// <summary>
		/// Checks if this card can be placed on a given slot.
		/// </summary>
		/// <param name="slot">The slot the card is being placed on.</param>
		/// <returns>true if can be placed, otherwise false</returns>
		public bool CanBePlacedOn(CardSlot slot)
		{
			return !slot.Card && (!this.Slot || slot.Team == this.Slot.Team);
		}
	}
}
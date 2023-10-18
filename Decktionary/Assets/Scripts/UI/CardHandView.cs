using Starlight.Words;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Starlight.UI
{
    public class CardHandView : MonoBehaviour
    {
        [SerializeField] RectTransform cardParent;
        [SerializeField] CardUI cardPrefab;

	   public void AddCard(CardData card)
        {
            var newCardObj = Instantiate(cardPrefab, cardParent);
            newCardObj.SetCardData(card);
	   }
    }
}
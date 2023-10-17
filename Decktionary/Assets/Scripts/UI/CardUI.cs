using Starlight.Words;
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
    }
}
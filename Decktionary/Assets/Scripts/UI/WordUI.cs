using Starlight.Words;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Starlight.UI
{
    public class WordUI : MonoBehaviour
    {
	   [SerializeField] TMP_Text wordText;
	   public WordData Data { get; private set; }

	   public void SetData(WordData newData) 
	   {
		  wordText.text = newData.word;
		  Data = newData;
	   }

        public void OnClicked()
        {
             WordManager.instance.OnWordClicked(this);
        }

        public void OnHover()
        {
		  WordManager.instance.OnWordHovered(this);
	   }

	   public void OnExitHover()
	   {
		  WordManager.instance.OnWordUnhovered(this);
	   }
    }
}
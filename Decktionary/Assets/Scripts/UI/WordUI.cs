using Starlight.BattleMembers;
using Starlight.Words;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Starlight.UI
{
    public class WordUI : MonoBehaviour
    {
	   [HideInInspector] public PlayerBehaviour playerBehaviour;

	   [SerializeField] TMP_Text wordText;
	   public WordData Data { get; private set; }

	   public void SetData(WordData newData) 
	   {
		  wordText.text = newData.word;
		  Data = newData;
	   }

        public void OnClicked()
        {
		  playerBehaviour.OnWordClicked(this);
        }

        public void OnHover()
        {
		  playerBehaviour.OnWordHovered(this);
	   }

	   public void OnExitHover()
	   {
		  playerBehaviour.OnWordUnhovered(this);
	   }
    }
}
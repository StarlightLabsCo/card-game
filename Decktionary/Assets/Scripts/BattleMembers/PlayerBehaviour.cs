using Sirenix.OdinInspector;
using Starlight.UI;
using Starlight.Words;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Starlight.UI.BattleUICursor;
using static Starlight.Words.WordData;

namespace Starlight.BattleMembers
{
    /// <summary>
    /// The battle member script for the player's actions during a turn.
    /// </summary>
    public class PlayerBehaviour : BattleMember
    {
        [SerializeField] GameObject playerUIBase;

	   [Header("Transitions")]
	   [SerializeField] Transitionable wordMenuTransition;
	   [SerializeField] Transitionable backgroundFadeTransition;
	   [SerializeField] CanvasGroup backgroundGroup;
	   [SerializeField] Button confirmButton;
	   [SerializeField] Transitionable confirmButtonTransition;
	   [SerializeField] float confirmButtonTransitionTime;
	   [SerializeField] Image confirmButtonContent;
	   [SerializeField] Sprite confirmButtonContentSprite;
	   [SerializeField] Sprite cancelButtonContentSprite;

	   [Space]
        [SerializeField] Transform wordInventoryGroup;
	   [SerializeField] Transform hammerGroup;

	   [SerializeField] WordUI wordPrefab;

	   [SerializeField] CanvasGroup tooltipGroup;
	   [SerializeField] TMP_Text tooltipText;

	   [SerializeField] UnityEvent<CardData> onCardCreated;

	   List<WordUI> inventoryWords = new List<WordUI>();
	   List<WordUI> hammerMenuWords = new List<WordUI>();

	   WordUI hoveredWord;

	   public bool IsPlayerTurnOver { get; private set; }

	   [SerializeField] int turnWords = 8;


	   bool CanCombine => hammerMenuWords.Count > 0 && hammerMenuWords.Find(x => x.Data.wordType == WordType.Noun);

	   [Button]
	   public void CompletePlayerTurn()
	   {
		  IsPlayerTurnOver = true;
		  print("Player turn completed!");
	   }

	   public override IEnumerator ExecuteSetupTurn()
	   {
		  IsPlayerTurnOver = false;

		  //generate words
		  PopulateWordInventory(turnWords);

		  playerUIBase.SetActive(true);
		  yield return new WaitUntil(() => IsPlayerTurnOver);
		  playerUIBase.SetActive(false);
	   }

	   public void OnCursorChanged(CursorType cursorType, CursorType oldType)
	   {
		  if (cursorType == CursorType.Hammer)
		  {
			 StartCoroutine(confirmButtonTransition.TransitionIn(confirmButtonTransitionTime));
			 StartCoroutine(wordMenuTransition.TransitionIn(confirmButtonTransitionTime));
			 StartCoroutine(backgroundFadeTransition.TransitionIn(confirmButtonTransitionTime / 2f));
			 backgroundGroup.blocksRaycasts = true;
		  }
		  else if (oldType == CursorType.Hammer)
		  {
			 StartCoroutine(confirmButtonTransition.TransitionOut(confirmButtonTransitionTime));
			 StartCoroutine(wordMenuTransition.TransitionOut(confirmButtonTransitionTime));
			 StartCoroutine(backgroundFadeTransition.TransitionOut(confirmButtonTransitionTime / 2f));
			 backgroundGroup.blocksRaycasts = false;
		  }
		  OnHammerGroupUpdated();
	   }

	   private void OnHammerGroupUpdated()
	   {
		  //can combine if at least one noun
		  confirmButtonContent.sprite = CanCombine ? confirmButtonContentSprite : cancelButtonContentSprite;
	   }

	   public void ClearWordInventory()
	   {
		  for (int i = inventoryWords.Count - 1; i >= 0; i--)
		  {
			 Destroy(inventoryWords[i].gameObject);
		  }

		  inventoryWords.Clear();
	   }

	   /// <summary>
	   /// Clears existing word inventory and generates a list of words for the turn based on the items the player has, weights and categories.
	   /// </summary>
	   [Button]
	   public void PopulateWordInventory(int num)
	   {
		  ClearWordInventory();

		  for (int i = 0; i < num; i++)
		  {
			 WordData wordData = WordManager.RollRandomWordData(WordManager.instance.allWords);
			 var newWord = Instantiate(wordPrefab, wordInventoryGroup);
			 newWord.playerBehaviour = this;
			 newWord.SetData(wordData);
			 inventoryWords.Add(newWord);
		  }
	   }

	   public void OnHammerSideButtonPressed()
	   {
		  if (CanCombine)
		  {
			 ForgeWords();
		  }
		  else
		  {
			 CancelHammer();
		  }
		  //close menu
		  BattleUICursor.instance.SwitchCursorType(CursorType.None);
	   }

	   public void SendToInventory(WordUI word)
	   {
		  hammerMenuWords.Remove(word);
		  word.transform.SetParent(wordInventoryGroup);
		  inventoryWords.Add(word);
	   }

	   public void SendToHammerMenu(WordUI word)
	   {
		  inventoryWords.Remove(word);
		  word.transform.SetParent(hammerGroup);
		  if (word.Data.wordType == WordType.Adjective)
		  {
			 hammerMenuWords.Insert(0, word);
		  }
		  else
		  {
			 hammerMenuWords.Add(word);
		  }
	   }

	   private void CancelHammer()
	   {
		  //send all words back to inventory
		  for (int i = hammerMenuWords.Count - 1; i >= 0; i--)
		  {
			 SendToInventory(hammerMenuWords[i]);
		  }
	   }

	   private void ForgeWords()
	   {
		  //no words, do nothing
		  if (hammerMenuWords.Count == 0) return;

		  CardData newCard = new();

		  int i;
		  foreach (var word in hammerMenuWords)
		  {
			 newCard.AppendWord(word.Data);
		  }

		  //TODO: 
		  //1. disable buttons and functionality
		  //2. wait for AI data
		  // yield return newCard.GenerateAIData();
		  //3. continue

		  for (i = hammerMenuWords.Count - 1; i >= 0; i--)
		  {
			 Destroy(hammerMenuWords[i].gameObject);
			 hammerMenuWords.RemoveAt(i);
		  }

		  onCardCreated?.Invoke(newCard);
	   }

	   public void OnWordClicked(WordUI word)
	   {
		  if (BattleUICursor.instance.CurrentCursorType != CursorType.Hammer) return;

		  if (word.transform.parent == wordInventoryGroup)
		  {
			 SendToHammerMenu(word);
			 if (word.Data.wordType == WordType.Adjective)
			 {
				word.transform.SetSiblingIndex(0);
			 }
		  }
		  else
		  {
			 SendToInventory(word);
		  }
		  OnHammerGroupUpdated();
	   }

	   public void OnWordHovered(WordUI word)
	   {
		  hoveredWord = word;
		  tooltipGroup.alpha = 1f;
		  tooltipText.text = word.Data.description;
	   }

	   public void OnWordUnhovered(WordUI word)
	   {
		  if (hoveredWord == word)
		  {
			 tooltipGroup.alpha = 0f;
		  }
	   }
    }
}
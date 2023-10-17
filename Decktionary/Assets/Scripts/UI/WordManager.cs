using Sirenix.OdinInspector;
using Starlight.Words;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Starlight.UI.BattleUICursor;
using static Starlight.Words.WordData;

namespace Starlight.UI
{
    public class WordManager : SingletonBehaviour<WordManager>
    {
        [Header("Words")]
        [SerializeField] Transform wordInventoryGroup;
	   [SerializeField] Transform hammerGroup;
        [SerializeField] WordUI wordPrefab;

        [Space]

        [Header("Confirm Button")]
        [SerializeField] Button confirmButton;
	   [SerializeField] Transitionable confirmButtonTransition;
	   [SerializeField] float confirmButtonTransitionTime;
        [SerializeField] Image confirmButtonContent;
        [SerializeField] Sprite confirmButtonContentSprite;
        [SerializeField] Sprite cancelButtonContentSprite;

	   [Space]

        [Header("Other")]
	   [SerializeField] CanvasGroup tooltipGroup;
	   [SerializeField] TMP_Text tooltipText;

        [Space]

        [SerializeField] TextAsset startingWordsFile;
	   [SerializeField] WordData[] allWords;

        [Space]

        [SerializeField] AnimationCurve wordWeightOverLength;

        List<WordUI> inventoryWords = new List<WordUI>();
        List<WordUI> hammerMenuWords = new List<WordUI>();
	   WordUI hoveredWord;


        bool CanCombine => hammerMenuWords.Count > 0 && hammerMenuWords.Find(x => x.Data.wordType == WordType.Noun);

	   public float GetWordWeight(int length)
        {
            return wordWeightOverLength.Evaluate(length);
        }

        protected override void Awake()
        {
            base.Awake();
            //parse starting_words.csv
            var allLines = startingWordsFile.text.Split('\n');
            allWords = new WordData[allLines.Length];
		  for(int i = 0; i < allLines.Length; i++)
            {
                var values = allLines[i].Split(',');
                for(int j = 0; j < values.Length; j++)
                {
                    values[j] = values[j].Replace("\r", "");
                }

                WordData newWord = new();

                //TODO: Need to add AI generated descriptions

                newWord.word = values[1];
                Enum.TryParse(values[0], out WordType wType);
                newWord.wordType = wType;
                allWords[i] = newWord;
		  }
	   }

        public WordData RollRandomWordData()
        {
            //calculate total weight
            float totalWeight = 0f;
            foreach(var word in allWords)
            {
			 totalWeight += word.Weight;
		  }
            float val = UnityEngine.Random.value;
            float targetRandWeight = val * totalWeight;
            float run = 0f;
            for(int i = 0; i < allWords.Length; i++)
            {
                run += allWords[i].Weight;
                if(run >= targetRandWeight)
                {
                    return allWords[i];
                }
		  }
            return null;
        }

	   [Button]
        /// <summary>
        /// Generates a list of words for the turn based on the items the player has, weights and categories
        /// </summary>
        public void PopulateWordInventory(int num)
        {
            for(int i = 0; i < num; i++)
            {
                WordData wordData = RollRandomWordData();
                var newWord = Instantiate(wordPrefab, wordInventoryGroup);
                newWord.SetData(wordData);
                inventoryWords.Add(newWord);
            }
        }

        private void OnHammerGroupUpdated()
        {
            //can combine if at least one noun
            confirmButtonContent.sprite = CanCombine ? confirmButtonContentSprite : cancelButtonContentSprite;
	   }

	   public void OnCursorChanged(CursorType cursorType, CursorType oldType)
        {
            if (cursorType == CursorType.Hammer)
            {
                StartCoroutine(confirmButtonTransition.TransitionIn(confirmButtonTransitionTime));
            }
            else if (oldType == CursorType.Hammer)
            {
                StartCoroutine(confirmButtonTransition.TransitionOut(confirmButtonTransitionTime));
            }
            OnHammerGroupUpdated();
	   }

        public void OnHammerSideButtonPressed()
        {
            if(CanCombine)
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
		  hammerMenuWords.Add(word);
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
            foreach(var word in hammerMenuWords)
            {
                newCard.AppendWord(word.Data);
		  }

		  for (i = hammerMenuWords.Count - 1; i >= 0; i--)
            {
                Destroy(hammerMenuWords[i].gameObject);
                hammerMenuWords.RemoveAt(i);
		  }

            //TODO: Do something with new card data created
            print(newCard);
        }

	   public void OnWordClicked(WordUI word)
        {
            if (BattleUICursor.instance.CurrentCursorType != CursorType.Hammer) return;

            if(word.transform.parent == wordInventoryGroup)
            {
                SendToHammerMenu(word);
                if(word.Data.wordType == WordType.Adjective)
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
            if(hoveredWord == word)
            {
			 tooltipGroup.alpha = 0f;
		  }
	   }
    }
}
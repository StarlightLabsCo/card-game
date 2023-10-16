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
        [SerializeField] Transform wordInventoryGroup;
        [SerializeField] Transform hammerGroup;
        [SerializeField] WordUI wordPrefab;
        [SerializeField] Button confirmButton;
	   [SerializeField] Transitionable confirmButtonTransition;
	   [SerializeField] float confirmButtonTransitionTime;

	   [Space]

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
            bool canCombine = hammerMenuWords.Count > 0 && hammerMenuWords.Find(x => x.Data.wordType == WordType.Noun);

            confirmButton.interactable = canCombine;
        }

	   public void OnCursorChanged(CursorType cursorType, CursorType oldType)
        {
            if(cursorType == CursorType.Hammer)
            {
                StartCoroutine(confirmButtonTransition.TransitionIn(confirmButtonTransitionTime));
		  } 
            else if(oldType == CursorType.Hammer)
            {
			 StartCoroutine(confirmButtonTransition.TransitionOut(confirmButtonTransitionTime));
		  }
	   }

        public void ForgeWords()
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
			 word.transform.SetParent(hammerGroup);
                if(word.Data.wordType == WordType.Adjective)
                {
                    word.transform.SetSiblingIndex(0);
                }

                inventoryWords.Remove(word);
                hammerMenuWords.Add(word);
		  }
		  else
            {
			 word.transform.SetParent(wordInventoryGroup);
                hammerMenuWords.Remove(word);
                inventoryWords.Add(word);
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
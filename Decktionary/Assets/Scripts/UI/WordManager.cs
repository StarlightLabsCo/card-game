using Sirenix.OdinInspector;
using Starlight.Words;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Starlight.UI.BattleUICursor;
using static Starlight.Words.WordData;

namespace Starlight.UI
{
    public class WordManager : SingletonBehaviour<WordManager>
    {
        [SerializeField] TextAsset startingWordsFile;
	   [HideInInspector] public WordData[] allWords;

        [Space]

        [SerializeField] AnimationCurve wordWeightOverLength;


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

        public static WordData RollRandomWordData(IList<WordData> words)
        {
            //calculate total weight
            float totalWeight = 0f;
            foreach(var word in words)
            {
			 totalWeight += word.Weight;
		  }
            float val = UnityEngine.Random.value;
            float targetRandWeight = val * totalWeight;
            float run = 0f;
            for(int i = 0; i < words.Count; i++)
            {
                run += words[i].Weight;
                if(run >= targetRandWeight)
                {
                    return words[i];
                }
		  }
            return null;
        }


        public CardData GenerateRandomCard(IList<WordData> words, int wordLength)
        {
            var card = new CardData();
            for(int i = 0; i < wordLength; i++)
            {
                card.AppendWord(RollRandomWordData(words));
            }
            return card;
        }
    }
}
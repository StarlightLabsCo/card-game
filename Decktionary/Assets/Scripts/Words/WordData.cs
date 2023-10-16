using Starlight.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Starlight.Words
{
    [System.Serializable]
    public class WordData
    {
	   public enum WordType
	   {
		  Adjective,
		  Noun
	   }

	   public WordType wordType;
	   public string word;

	   //TODO: replace with AI-generated description instead of just type
	   public string description => wordType.ToString();
	   public float Weight => WordManager.instance.GetWordWeight(word.Length);
    }
}
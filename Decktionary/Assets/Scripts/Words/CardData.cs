using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Starlight.Words
{
    [System.Serializable]
    public class CardData
    {
	   private List<WordData> words;

	   public CardData()
	   {
		  words = new();
	   }

	   public void AppendWord(WordData data)
	   {
		  words.Add(data);
	   }

	   public string GetPrompt()
	   {
		  string promptBuilder = "";
		  foreach (var word in words)
		  {
			 promptBuilder += word.word + " ";
		  }
		  return promptBuilder;
	   }

	   public override string ToString() => GetPrompt();
    }
}
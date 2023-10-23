using System;
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
	   public int WordCount => words.Count;

	   //TODO: needs to be generated by GPT
	   public Sprite Icon { get; private set; }

	   //TODO: needs to be generated by GPT
	   public string Description { get; private set; }

	   //Stats
	   //TODO: needs to be generated by GPT
	   public int Health { get; private set; }
	   public int Damage { get; private set; }

	   private const int WORD_HEALTH_INCREASE = 1;
	   private const int BASE_ATTACK_DAMAGE = 1;

	   public event Action<int> onHealthUpdated;
	   public event Action<int> onDamageUpdated;

	   public CardData()
	   {
		  words = new();
		  //TODO: Generate AI sprite, AI description!

		  //TODO: Replace below stats with GPT decisions
		  Health = 0;
		  Damage = BASE_ATTACK_DAMAGE;
	   }


	   public void AppendWord(WordData data)
	   {
		  words.Add(data);
		  ChangeHealth(WORD_HEALTH_INCREASE);
	   }

	   public void SetHealth(int newHealth)
	   {
		  Health = Mathf.Max(newHealth, 0);
		  onHealthUpdated?.Invoke(Health);
	   }

	   public void ChangeHealth(int change)
	   {
		  SetHealth(Health + change);
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
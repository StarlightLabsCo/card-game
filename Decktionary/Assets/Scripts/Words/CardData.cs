using Starlight.Managers;
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
	   public string Id { get; private set; }

	   public List<WordData> Words;
	   public int WordCount => Words.Count;

	   public Sprite Icon { get; private set; }
	   public string Description { get; private set; }

	   //Stats
	   public int Health { get; private set; }
	   public int Damage { get; private set; }

	   private const int WORD_HEALTH_INCREASE = 1;
	   private const int BASE_ATTACK_DAMAGE = 1;

	   public event Action<Sprite> onIconUpdated;
	   public event Action<string> onDescriptionUpdated;
	   public event Action<int> onHealthUpdated;
	   public event Action onDie;
	   public event Action<int> onDamageUpdated;


	   public CardData(IList<WordData> words)
	   {
		  Id = Guid.NewGuid().ToString();
		  Words = new List<WordData>(words);
		  Words.Sort((x, y) => x.wordType.CompareTo(y.wordType));

		  Health = Words.Count * WORD_HEALTH_INCREASE;
		  Damage = BASE_ATTACK_DAMAGE;

		  CardManager.instance.GenerateCardDetails(this);
	   }


	   // Icon
	   public void SetIcon(Sprite newIcon)
	   {
		  Icon = newIcon;
		  onIconUpdated?.Invoke(Icon);
	   }

	   // Description
	   public void SetDescription(string newDescription)
	   {
		  Description = newDescription;
		  onDescriptionUpdated?.Invoke(Description);
	   }

	   // Health
	   public void SetHealth(int newHealth)
	   {
		  Health = Mathf.Max(newHealth, 0);
		  onHealthUpdated?.Invoke(Health);
		  if(Health == 0)
		  {
			 onDie?.Invoke();
		  }
	   }

	   public void ChangeHealth(int change)
	   {
		  SetHealth(Health + change);
	   }

	   // Damage

	   public void SetDamage(int newDamage)
	   {
		  Damage = Mathf.Max(newDamage, 0);
		  onDamageUpdated?.Invoke(Damage);
	   }

	   public void ChangeDamage(int change)
	   {
		  SetDamage(Damage + change);
	   }

	   public string GetPrompt()
	   {
		  string promptBuilder = "";
		  foreach (var word in Words)
		  {
			 promptBuilder += word.word + " ";
		  }
		  return promptBuilder;
	   }

	   public override string ToString() => GetPrompt();
    }
}
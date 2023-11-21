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
		public List<WordData> Words;
		public int WordCount => Words.Count;

		public Sprite Icon { get; private set; }
		public string Description { get; private set; }

		//Stats
		public int Health { get; private set; }
		public int Damage { get; private set; }

		private const int WORD_HEALTH_INCREASE = 1;
		private const int BASE_ATTACK_DAMAGE = 1;

		public event Action<int> onHealthUpdated;
		public event Action<int> onDamageUpdated;

		public CardData(WordData[] words)
		{
			this.Words = new List<WordData>(words);

			Health = this.Words.Count * WORD_HEALTH_INCREASE;

			WebSocketClient.Instance.GenerateCardDetails(this);
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
			foreach (var word in Words)
			{
				promptBuilder += word.word + " ";
			}
			return promptBuilder;
		}

		public override string ToString() => GetPrompt();
	}
}
using Starlight.Managers;
using Starlight.UI;
using Starlight.Words;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Starlight.BattleMembers
{
    /// <summary>
    /// The battle member script for all enemy encounters.
    /// </summary>
    public class EnemyEncounterBehaviour : BattleMember
    {
	   [SerializeField] List<CardSlot> placementSlots = new List<CardSlot>();
	   [SerializeField] EnemyEncounterData data;
	   [SerializeField] float cardPlacementDelay = 0.5f;

        public override IEnumerator ExecuteSetupTurn(System.Random seededRandom, int turn)
        {
		  int cardAmount = data.GetCardAmount(seededRandom, turn);
		  int wordAmount;
		  int i, j;

		  List<CardData> generatedCards = new();

		  for (i = 0; i < cardAmount; i++)
		  {
			 wordAmount = data.GetWordAmount(seededRandom, turn);
			 List<WordData> words = new(wordAmount);
			 for(j = 0; j < wordAmount; j++)
			 {
				var newWord = WordManager.RollRandomWordData(WordManager.instance.allWords, seededRandom);
				words.Add(newWord);
			 }
			 CardData newCard = new(words);
			 generatedCards.Add(newCard);
		  }

		  List<CardSlot> emptySlots = new();
		  foreach(var slot in placementSlots)
		  {
			 if (!slot.Card) emptySlots.Add(slot);
		  }

		  foreach(var card in generatedCards)
		  {
			 if (emptySlots.Count == 0) break;
			 var chosenSlot = emptySlots[seededRandom.Next(0, emptySlots.Count)];
			 var newCard = CardManager.instance.CreateCard(chosenSlot.transform, card, chosenSlot.transform.position);
			 chosenSlot.SetCard(newCard);
			 newCard.SetSlot(chosenSlot);
			 yield return new WaitForSeconds(cardPlacementDelay);
		  }
		  yield return null;
	   }


	   //OBSOLETE
	   /*/// <summary>
	   /// Gets a float representing the multiplier for different enemy values including card health, amount of cards to put down at once, and amount of words in each card.
	   /// https://www.desmos.com/calculator/jnkh5vplhz
	   /// </summary>
	   public static float GetDifficultyMultiplier(int encounterLevel, int difficultyAccel, int turnCurveLength, int turn)
	   {
		  return (turn <= turnCurveLength ? - Mathf.Pow((turn / turnCurveLength) - 1f, difficultyAccel) + 1f : 1f) * encounterLevel;
	   }*/
    }
}
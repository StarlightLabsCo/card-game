using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Starlight.BattleMembers
{
    [CreateAssetMenu(menuName = "Starlight/Enemy Encounter", fileName = "new_encounter")]
    public class EnemyEncounterData : ScriptableObject
    {
        public AnimationCurve cardsPerTurn;
	   [HorizontalGroup("WordsPerCard")] public AnimationCurve wordsPerCardMin;
	   [HorizontalGroup("WordsPerCard")] public AnimationCurve wordsPerCardMax;

	   /// <summary>
	   /// Gets the amount of cards to be placed given a <paramref name="turn"></paramref> number
	   /// </summary>
	   /// <param name="seededRandom">The encounter seeded random</param>
	   /// <param name="turn">The current turn number</param>
	   /// <returns>The integer amount of cards to be placed this turn</returns>
	   public int GetCardAmount(System.Random seededRandom, int turn)
	   {
		  return Mathf.CeilToInt(cardsPerTurn.Evaluate(turn));
	   }

	   /// <summary>
	   /// Gets the amount of words to be added to each placed card given a <paramref name="turn"></paramref> number
	   /// </summary>
	   /// <param name="seededRandom">The encounter seeded random</param>
	   /// <param name="turn">The current turn number</param>
	   /// <returns>The integer amount of words to be added to the next card</returns>
	   public int GetWordAmount(System.Random seededRandom, int turn)
	   {
		  float min = wordsPerCardMin.Evaluate(turn);
		  float max = wordsPerCardMax.Evaluate(turn);
		  return Mathf.CeilToInt((float)seededRandom.NextDouble() * (max - min) + min);
	   }
    }
}
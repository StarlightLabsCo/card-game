using Starlight.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Starlight.BattleMembers
{
    /// <summary>
    /// The abstract base class of all turn-havers (e.g. player, enemy encounter, boss encounter).
    /// </summary>
    public abstract class BattleMember : MonoBehaviour
    {
	   [SerializeField] List<CardSlot> cardSlots;

	   public abstract IEnumerator ExecuteSetupTurn(System.Random seededRandom, int turn);
	   public IEnumerator ExecuteCardsTurn(System.Random seededRandom, int turn)
	   {
		  foreach (var cardSlot in cardSlots)
		  {
			 yield return cardSlot.ExecuteSlotTurn(seededRandom, turn);
		  }
	   }
    }
}
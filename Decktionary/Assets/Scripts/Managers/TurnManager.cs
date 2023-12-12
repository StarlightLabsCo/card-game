using Sirenix.OdinInspector;
using Starlight.BattleMembers;
using Starlight.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Starlight.Managers
{
    public class TurnManager : SingletonBehaviour<TurnManager>
    {
	   [SerializeField] List<BattleMember> battleMembers = new List<BattleMember>();

        public bool IsPlayerTurnOver { get; private set; } = false;


	   void Start()
        {
		  StartTurns((int)DateTime.Now.Ticks);
        }

	   [Button]
	   public void StartTurns(int seed)
	   {
		  System.Random seededRandom = new(seed);
		  StartCoroutine(TurnCoroutine(seededRandom, 0));
	   }

        IEnumerator TurnCoroutine(System.Random seededRandom, int turn)
        {
		  //setup
		  foreach(var member in battleMembers)
		  {
			 yield return member.ExecuteSetupTurn(seededRandom, turn);
		  }

		  //battle!
		  foreach (var member in battleMembers)
		  {
			 yield return member.ExecuteCardsTurn(seededRandom, turn);
		  }

		  //TODO: Don't start next turn if the player is dead or all enemies are dead!

		  //loop
		  StartCoroutine(TurnCoroutine(seededRandom, turn + 1));
        }

	   /*IEnumerator PlaceEnemyCards()
	   {
		  playerUIBase.SetActive(false);
		  *//*
             * TODO: Implement enemy card placement
             *//*
		  yield return null;
	   }

	   IEnumerator PlacePlayerCards()
	   {
		  //enable UI
		  IsPlayerTurnOver = false;
		  playerUIBase.SetActive(true);

		  //wait until turn over (button press)
		  yield return new WaitUntil(() => IsPlayerTurnOver);
	   }*/

	   /*IEnumerator ExecuteTurnsForType(CardSlotTeam teamType)
	   {
		  //loop through all cards on field (left -> right, top -> bottom)
		  for (int y = 0; y < slotGrid.GetLength(0); y++)
		  {
			 for (int x = 0; x < slotGrid.GetLength(1); x++)
			 {
				var cardSlot = GetCardSlot(x, y);
				//if has a card in the slot, and team is player team, execute the card's turn
				if (cardSlot.Card && cardSlot.Team == teamType)
				{
				    yield return cardSlot.ExecuteSlotTurn(slotGrid);
				}
			 }
		  }
		  yield return null;
	   }

	   IEnumerator ExecutePlayerCardsTurn()
	   {
		  yield return ExecuteTurnsForType(CardSlotTeam.Player);
	   }

	   IEnumerator ExecuteEnemyCardsTurn()
	   {
		  yield return ExecuteTurnsForType(CardSlotTeam.Enemy);
	   }*/
    }
}
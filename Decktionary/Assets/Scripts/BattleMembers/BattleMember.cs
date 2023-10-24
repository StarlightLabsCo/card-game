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
        public abstract IEnumerator ExecuteSetupTurn();
    }
}
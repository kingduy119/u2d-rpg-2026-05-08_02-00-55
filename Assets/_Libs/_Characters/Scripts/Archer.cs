using UnityEngine;
using Weapons;

namespace Characters
{
    public class Archer : Character
    {
        [SerializeField] private ProjectSO _projectSO;

        protected override void InitStates()
        {
            var archerCombat = new ArcherCombatState(this, _projectSO);

            idleState = new IdleState(this);
            moveState = new MovementSate(this);
            combatState = archerCombat;

            _states.Add(archerCombat); // For state can call Tick() in Update 
        }
    }
}
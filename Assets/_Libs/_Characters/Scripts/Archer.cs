using UnityEngine;
using Weapons;

namespace Characters
{
    public class Archer : Character
    {
        [SerializeField] private ProjectSO _projectSO;

        protected override void InitStates()
        {
            idleState = new IdleState(this);
            moveState = new MovementSate(this);
            combatState = new ArcherCombatState(this, _projectSO);

            _states.Add(combatState); // For state can call Tick() in Update 
        }
    }
}
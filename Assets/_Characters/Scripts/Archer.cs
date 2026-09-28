

namespace Characters
{
    public class Archer : Character
    {
        protected override void InitStates()
        {
            idleState = new IdleState(this);
            moveState = new MovementSate(this);
            combatState = new ArcherCombatState(this);

            _states.Add(combatState); // For state can call Tick() in Update 
        }
    }
}
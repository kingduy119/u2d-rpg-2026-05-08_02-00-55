

using UnityEngine;

namespace TDGame
{
    [CreateAssetMenu(fileName = "SpeedAbility", menuName = "Game TD/Abilities/SpeedAbility")]
    public class SpeedAbility : Ability
    {
        [SerializeField] private int _attackSpeed;
        [SerializeField] private int _runSpeed;

        public override void Use(GameObject gameObject = null)
        {
            base.Use(gameObject);
            Debug.Log($"Speed: {_attackSpeed} - {_runSpeed}");
        }

    }
}
using UnityEngine;

namespace TDGame
{
    public abstract class Ability : ScriptableObject
    {
        public string _name;
        public string _description;
        public Sprite _image;

        public virtual void Use(GameObject gameObject = null) { }
        public virtual void Apply(TowerAbility ability) { }
    }
}
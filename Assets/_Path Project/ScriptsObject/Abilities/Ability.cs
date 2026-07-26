using UnityEngine;

namespace TDGame
{
    public abstract class Ability : ScriptableObject
    {
        public string _name;
        [TextArea(3, 10)]
        public string _description;
        public Sprite _image;

        public virtual void Use(GameObject gameObject = null) { }
        public virtual void Apply(GameObject go) { }
        public virtual void Apply(TowerAbility ability) { }
    }
}
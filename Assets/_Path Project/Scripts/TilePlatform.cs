using UnityEngine;
// using UnityEngine.EventSystems;
// using UnityEngine.InputSystem;

namespace TDGame
{
    public class TilePlatform : MonoBehaviour
    {
        [SerializeField] private Color activeColor;
        [SerializeField] private Color unActiveColor;
        [SerializeField] private Collider2D collier;
        [SerializeField] private SpriteRenderer spriteRenderer;

        public bool CanBuild { get; private set; } = true;
        public bool Builded { get; private set; } = false;


        private void OnTriggerEnter2D(Collider2D collision)
        {

            CanBuild = false;
            spriteRenderer.color = unActiveColor;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            // if (Builded) return;

            CanBuild = true;
            spriteRenderer.color = activeColor;
        }



    }
}

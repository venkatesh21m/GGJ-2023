using UnityEngine;

namespace Rudrac.GGJ2023
{
    public class LevelBorder : MonoBehaviour
    {
        public BoxCollider2D Collider2D;
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;

            Scenesmanager.instance.ReloadScene();
        }

        private void OnDrawGizmos()
        {
            if (Collider2D == null) Collider2D = GetComponent<BoxCollider2D>();
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, Vector3.one * Collider2D.size);
        }
    }
}

using UnityEngine;

namespace Rudrac.GGJ2023
{
    public class LevelBorder : MonoBehaviour
    {
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;

            Scenesmanager.instance.ReloadScene();
        }
    }
}

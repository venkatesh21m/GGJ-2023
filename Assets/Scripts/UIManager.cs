using DG.Tweening;
using UnityEngine;

namespace Rudrac.GGJ2023
{
    public class UIManager : MonoBehaviour
    {
        public CanvasGroup Menu;

        public void OnMenuPressedd()
        {
            Scenesmanager.instance.LoadScene(1);
            _ = Menu.DOFade(0, 1.5f).OnComplete(() => Menu.gameObject.SetActive(false));
        }
    }
}

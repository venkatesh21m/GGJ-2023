using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rudrac.GGJ2023
{
    public class Scenesmanager : MonoBehaviour
    {
        public static Scenesmanager instance;
        public CanvasGroup CanvasGroup;
        public Camera BootCamera;
        private void Start() => instance = this;
        int _currentsceneIndex = 0;
        public async void LoadScene(int i)
        {
            _currentsceneIndex = i;
            _ = CanvasGroup.DOFade(1, 1.5f);
            await Task.Delay(1500);
            BootCamera.gameObject.SetActive(true);

            if (SceneManager.sceneCount > 1)
            {
                AsyncOperation ao = SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
                while (!ao.isDone)
                {
                    await Task.Yield();
                }
            }

            if (i != 0)
            {
                AsyncOperation _ao = SceneManager.LoadSceneAsync(i, LoadSceneMode.Additive);
                while (!_ao.isDone)
                {
                    await Task.Yield();
                }
                BootCamera.gameObject.SetActive(false);
            }
            else
            {
                BootCamera.gameObject.SetActive(true);
            }
            _ = CanvasGroup.DOFade(0, 1.5f);
        }

        internal void ReloadScene() => LoadScene(_currentsceneIndex);
    }
}
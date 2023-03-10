using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rudrac.GGJ2023
{
    public class JumpForceChance : MonoBehaviour
    {
        public static event Action ChargingForLaunch;
        public static event Action<bool> Launched;

        public Key ThurstKey = Key.Space;
        public RectTransform MaxIndicator;
        public RectTransform SelectionIndicator;
        public float minTime, maxTime;
        [Space]
        public CanvasGroup GameUI;
        public CanvasGroup JumpForceUI;

        private float _x;
        private Sequence _selectionIndicatorSequence;
        private Player _player;
        private void Start()
        {
            _player = Player.instance;

            ResetChances();
        }

        private void Update()
        {
            if (!_player.Grounded || BoostersManager.BoosterCount == 0)
                return;

            if (Keyboard.current[ThurstKey].wasPressedThisFrame)
            {
                ChargingForLaunch?.Invoke();
                SetUpUI();
                StartMovement();
            }

            if (Keyboard.current[ThurstKey].wasReleasedThisFrame)
            {
                _selectionIndicatorSequence.Kill();
                Launched?.Invoke(GetSuccessState());
                HideUI();
            }
        }

        private bool GetSuccessState() => SelectionIndicator.anchoredPosition.x < _x + 100 && SelectionIndicator.anchoredPosition.x > _x - 100;

        private void StartMovement()
        {
            _selectionIndicatorSequence = DOTween.Sequence();
            _ = _selectionIndicatorSequence.Append(SelectionIndicator.DOAnchorPosX(950, UnityEngine.Random.Range(minTime, maxTime)).SetEase((Ease)UnityEngine.Random.Range(0, 20)));
            _ = _selectionIndicatorSequence.Append(SelectionIndicator.DOAnchorPosX(0, UnityEngine.Random.Range(minTime, maxTime)).SetEase((Ease)UnityEngine.Random.Range(0, 20)));
            _ = _selectionIndicatorSequence.SetLoops(-1);
        }

        private void SetUpUI()
        {
            _x = UnityEngine.Random.Range(0, 800);
            MaxIndicator.anchoredPosition = new Vector2(_x, MaxIndicator.anchoredPosition.y);
            SelectionIndicator.anchoredPosition = new Vector2(0, SelectionIndicator.anchoredPosition.y);

            _ = JumpForceUI.DOFade(1, 0.25f);
            _ = GameUI.DOFade(0, 0.25f);
        }

        private async void HideUI()
        {
            await System.Threading.Tasks.Task.Delay(500);
            _ = JumpForceUI.DOFade(0, 0.25f);
            _ = GameUI.DOFade(1, 0.25f).OnComplete(() => ResetChances());

        }

        private void ResetChances()
        {
            _x = 0;
            SelectionIndicator.anchoredPosition = new Vector2(800, MaxIndicator.anchoredPosition.y);
        }
    }
}
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

        private float X;
        private Sequence SelectionIndicatorSequence;
        private Player Player;
        private void Start() => Player = Player.instance;
        private void Update()
        {
            if (!Player.Grounded)
                return;

            if (Keyboard.current[ThurstKey].wasPressedThisFrame)
            {
                ChargingForLaunch?.Invoke();
                SetUpUI();
                StartMovement();
            }

            if (Keyboard.current[ThurstKey].wasReleasedThisFrame)
            {
                SelectionIndicatorSequence.Kill();
                HideUI();
            }
        }

        private bool GetSuccessState() => SelectionIndicator.anchoredPosition.x < X + 100 && SelectionIndicator.anchoredPosition.x > X - 100;

        private void StartMovement()
        {
            SelectionIndicatorSequence = DOTween.Sequence();
            _ = SelectionIndicatorSequence.Append(SelectionIndicator.DOAnchorPosX(950, UnityEngine.Random.Range(minTime, maxTime)).SetEase((Ease)UnityEngine.Random.Range(0, 20)));
            _ = SelectionIndicatorSequence.Append(SelectionIndicator.DOAnchorPosX(0, UnityEngine.Random.Range(minTime, maxTime)).SetEase((Ease)UnityEngine.Random.Range(0, 20)));
            _ = SelectionIndicatorSequence.SetLoops(-1);
        }

        private void SetUpUI()
        {
            X = UnityEngine.Random.Range(0, 800);
            MaxIndicator.anchoredPosition = new Vector2(X, MaxIndicator.anchoredPosition.y);
            SelectionIndicator.anchoredPosition = new Vector2(0, SelectionIndicator.anchoredPosition.y);

            _ = JumpForceUI.DOFade(1, 0.25f);
            _ = GameUI.DOFade(0, 0.25f);
        }

        private async void HideUI()
        {
            await System.Threading.Tasks.Task.Delay(500);
            _ = JumpForceUI.DOFade(0, 0.25f);
            _ = GameUI.DOFade(1, 0.25f).OnComplete(() =>
            {
                Launched?.Invoke(GetSuccessState());
            });
        }
    }
}
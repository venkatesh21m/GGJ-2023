using DG.Tweening;
using UnityEngine;

public class PlanetMover : MonoBehaviour
{
    Sequence Sequence;
    public float Duration = 30;
    void Start()
    {
        Sequence = DOTween.Sequence();
        float yPos = transform.localPosition.y;
        Sequence.Append(transform.DOMoveY(20, Duration).SetEase(Ease.Linear));
        Sequence.Append(transform.DOMoveY(yPos, Duration).SetEase(Ease.Linear));
        Sequence.SetLoops(-1);
    }

}

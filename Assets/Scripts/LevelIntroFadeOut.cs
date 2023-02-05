using DG.Tweening;
using System.Collections;
using UnityEngine;

public class LevelIntroFadeOut : MonoBehaviour
{
    public float Duration;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(Duration * 2);
        GetComponent<CanvasGroup>().DOFade(0, Duration);
    }


}

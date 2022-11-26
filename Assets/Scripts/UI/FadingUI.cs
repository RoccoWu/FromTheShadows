using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadingUI : MonoBehaviour
{

    [SerializeField] private CanvasGroup canvasGroup;
    private Tween fadeTween;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeIn(float duration)
    {
        Fade(1f, duration, () =>
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        });
    }

    public void FadeOut(float duration)
    {
        Fade(0f, duration, () =>
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        });
    }
    private void Fade(float endValue, float duration, TweenCallback onEnd)
    {
        if(fadeTween != null)
        {
            fadeTween.Kill(false);
        }
        fadeTween = canvasGroup.DOFade(endValue, duration);
        fadeTween.onComplete += onEnd;
    }

    private IEnumerator TestFade()
    {
        yield return new WaitForSeconds(2f);
        FadeOut(1f);
        yield return new WaitForSeconds(2f);
        FadeIn(1f);
    }
}

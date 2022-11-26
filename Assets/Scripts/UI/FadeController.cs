using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeController : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    private Tween fadeTween;
    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.name == "FadeInOut")
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 1;
            FadeInScene();
        }

        else
        {
             gameObject.GetComponent<CanvasGroup>().alpha = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeOut(float duration)
    {
        Fade(1f, duration, () =>
        {
            canvasGroup.interactable = false; //can be true
            canvasGroup.blocksRaycasts = false; //can be true
        });
    }

    public void FadeIn(float duration)
    {
        Fade(0f, duration, () =>
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
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

    public void FadeInScene()
    {
        FadeIn(1f);
    }

    public void FadeOutScene()
    {
        FadeOut(1f);
    }

    public void FadeInTutorial(CanvasGroup cg)
    {
        FadeOut(0.69f);
    }

    public void FadeOutTutorial(CanvasGroup cg)
    {
        FadeIn(0.69f);
    }

}

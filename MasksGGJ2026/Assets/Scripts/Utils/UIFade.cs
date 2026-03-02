using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIFade : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeDuration = 1f;

    private Tween _currentTween;

    private void Awake()
    {
        _canvasGroup.alpha = 0f;
    }

    public void FadeOut(TweenCallback onEnd)
    {
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        _currentTween?.Kill();
        _canvasGroup.alpha = 0;
        _currentTween = _canvasGroup
            .DOFade(1f, _fadeDuration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(onEnd);
    }

    public void FadeIn(TweenCallback onEnd)
    {
        _currentTween?.Kill();
        _canvasGroup.alpha = 1;
        _currentTween = _canvasGroup
            .DOFade(0f, _fadeDuration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(onEnd);
    }
}

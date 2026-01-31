using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpritePaletteObserver : PaletteObserver
{
    public static readonly float TRANSITION_DURATION = 1f;
    [SerializeField] private List<SpriteRenderer> _sprites = new();

    protected override void PaletteUpdated(List<Color> palette)
    {
        foreach (SpriteRenderer sprite in _sprites)
        {
            sprite.DOColor(palette[_paletteIndex], TRANSITION_DURATION).SetEase(Ease.OutQuad);
        }
    }
}

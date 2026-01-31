using System;
using System.Collections.Generic;
using UnityEngine;

public class SpritePaletteObserver : PaletteObserver
{
    [SerializeField] private List<SpriteRenderer> _sprites = new();

    protected override void PaletteUpdated(List<Color> palette)
    {
        foreach (SpriteRenderer sprite in _sprites)
        {
            sprite.color = palette[_paletteIndex];
        }
    }
}

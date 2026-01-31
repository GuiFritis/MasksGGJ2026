using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PaletteObserver : MonoBehaviour
{
    [SerializeField] protected int _paletteIndex;

    void Awake()
    {
        ColorManager.onUpdatePalette += PaletteUpdated;
    }

    void OnDestroy()
    {
        ColorManager.onUpdatePalette -= PaletteUpdated;
    }

    protected abstract void PaletteUpdated(List<Color> palette);
}

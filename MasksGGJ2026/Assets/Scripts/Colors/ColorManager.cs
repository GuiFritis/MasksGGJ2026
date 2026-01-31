using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static System.Action<List<Color>> onUpdatePalette;
    [SerializeField] private List<ColorConfig> _colorConfigs;
    private List<Color> _palette;

    void Start()
    {
        CreateNewPalette();
    }

    public void CreateNewPalette()
    {
        float hue = Random.Range(0, 1f);
        _palette = new();
        foreach (ColorConfig config in _colorConfigs)
        {
            _palette.Add(Color.HSVToRGB(
                CalculateHue(hue, config.hueOffset),
                config.saturation,
                config.brightness
            ));
        }
        onUpdatePalette?.Invoke(_palette);
    }

    private float CalculateHue(float hue, float offset)
    {
        float value = hue + offset;
        if(value > 1)
        {
            value -= 1;
        }
        return value;
    }
}

[System.Serializable]
public struct ColorConfig
{
    [Range(0, 1f)] public float hueOffset;
    [Range(0, 1f)] public float saturation;
    [Range(0, 1f)] public float brightness;
}
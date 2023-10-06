using UnityEngine;
using UnityEngine.UI;

public static class ImageExtensions
{
    public static void Clear(this Image image)
    {
        image.sprite = null;
        image.color = Color.clear;
    }
}
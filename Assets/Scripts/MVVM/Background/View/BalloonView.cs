using UnityEngine.UI;
using UnityEngine;

public class BalloonView : Image
{
    public void Init(Sprite sprite, float scale)
    {
        this.sprite = sprite;

        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

        transform.localScale = Vector3.one * scale;
    }
}
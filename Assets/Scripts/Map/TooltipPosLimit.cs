using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipPosLimit : MonoBehaviour
{
    RectTransform rectTransform;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        Rect rect = rectTransform.rect;

        Vector2 leftBottom = transform.TransformPoint(rect.min);
        Vector2 rightTop = transform.TransformPoint(rect.max);
        Vector2 UIsize = rightTop - leftBottom;

        rightTop = new Vector2(Screen.width, Screen.height) - UIsize;

        float x = Mathf.Clamp(leftBottom.x, 0, rightTop.x);
        float y = Mathf.Clamp(leftBottom.y, 0, rightTop.y);

        Vector2 offSet = (Vector2)transform.position - leftBottom;

        transform.position = new Vector2(x, y) + offSet;
    }
}

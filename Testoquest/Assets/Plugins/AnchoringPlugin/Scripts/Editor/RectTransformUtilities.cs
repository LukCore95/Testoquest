using UnityEngine;

public static class RectTransformUtilities
{
    public static Vector2 GetAbsolutePosition(RectTransform rectTransform)
    {
        return new Vector2(GetAbsoluteX(rectTransform), GetAbsoluteY(rectTransform));
    }

    public static float GetAbsoluteX(RectTransform rectTransform)
    {
        float x;
        if (rectTransform.parent != null)
        {
            RectTransform parent = rectTransform.parent.GetComponent<RectTransform>();

            float anchorsCenter = (rectTransform.anchorMin.x + rectTransform.anchorMax.x) / 2.0f;
            //float deltaAnchor = rectTransform.anchorMax.x - rectTransform.anchorMin.x;
            float pivotDiff = (0.5f - rectTransform.pivot.x) * GetAbsoluteWidth(rectTransform);//(rectTransform.pivot.x - 0.5f) * GetAbsoluteSize(rectTransform).x;

            x = anchorsCenter * GetAbsoluteWidth(parent) + rectTransform.anchoredPosition.x;// - pivotDiff;
        }
        else
        {
            x = 0f;
        }
        return x;
    }

    public static float GetAbsoluteY(RectTransform rectTransform)
    {
        float y;
        if (rectTransform.parent != null)
        {
            RectTransform parent = rectTransform.parent.GetComponent<RectTransform>();

            float anchorsCenter = (rectTransform.anchorMin.y + rectTransform.anchorMax.y) / 2.0f;
            float deltaAnchor = rectTransform.anchorMax.y - rectTransform.anchorMin.y;
            float pivotDiff = ((rectTransform.pivot.y - 0.5f) * GetAbsoluteSize(rectTransform).y) * deltaAnchor;

            y = anchorsCenter * GetAbsoluteHeight(parent) + rectTransform.anchoredPosition.y;// + pivotDiff;
        }
        else
        {
            y = 0f;
        }
        return y;
    }

    public static Vector2 GetAbsoluteSize(RectTransform rectTransform)
    {
        return new Vector2(GetAbsoluteWidth(rectTransform), GetAbsoluteHeight(rectTransform));
    }

    public static Vector2 GetAbsoluteCenter(RectTransform rectTransform)
    {
        Vector2 startPivot = rectTransform.pivot;
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        Vector2 center = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);
        rectTransform.pivot = startPivot;

        return center;
    }

    public static float GetAbsoluteWidth(RectTransform rectTransform)
    {
        float width;
        float scaleX = rectTransform.anchorMax.x - rectTransform.anchorMin.x;
        if (scaleX == 0)
        {
            width = rectTransform.sizeDelta.x;
        }
        else
        {
            if (rectTransform.parent != null)
            {
                RectTransform parent = rectTransform.parent.GetComponent<RectTransform>();
                width = GetAbsoluteWidth(parent) * scaleX + rectTransform.sizeDelta.x;
            }
            else
            {
                width = 0;
            }
        }
        return width;
    }

    public static float GetAbsoluteHeight(RectTransform rectTransform)
    {
        float height;
        float scaleY = rectTransform.anchorMax.y - rectTransform.anchorMin.y;
        if (scaleY == 0)
        {
            height = rectTransform.sizeDelta.y;
        }
        else
        {
            if (rectTransform.parent != null)
            {
                RectTransform parent = rectTransform.parent.GetComponent<RectTransform>();
                height = GetAbsoluteHeight(parent) * scaleY + rectTransform.sizeDelta.y;
            }
            else
            {
                height = 0;
            }
        }
        return height;
    }
}
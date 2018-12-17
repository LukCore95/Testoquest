using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof (RectTransform)), CanEditMultipleObjects]
[EditorExtensionPlain("UnityEditor.RectTransformEditor, UnityEditor")]
public class RectTranformSuperEditor : ExtendedEditor
{
    public const string ButtonText = "Set Up Custom Anchors";

    public bool ShowAnchorSetter
    {
        get
        {
            return EditorPrefs.GetBool("ShowAnchorSetter");
        }
        set
        {
            EditorPrefs.SetBool("ShowAnchorSetter", value);
        }
    }
    public bool HasParent;

    private RectTransform _target;
    private RectTransform[] _targets;

    private Dictionary<string, Texture2D> buttonsIcons = new Dictionary<string, Texture2D>();

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GetData();

        //Show if target parent is not null
        if (HasParent)
        {
            ShowAnchorSetter = EditorGUILayout.Foldout(ShowAnchorSetter, ShowAnchorSetter ? "Hide Custom Anchor Setter" : "Show Custom Anchor Setter");
            			
			if (ShowAnchorSetter)
			{
                EditorGUILayout.Vector2Field("Absolute pos", RectTransformUtilities.GetAbsolutePosition(_target));
                EditorGUILayout.Vector2Field("Absolute size", RectTransformUtilities.GetAbsoluteSize(_target));

                int width = 150;

                GUI.backgroundColor = Color.clear;
                GUILayoutOption[] options = { GUILayout.MaxWidth(100.0f), GUILayout.MinWidth(10.0f) };

                GUILayout.BeginHorizontal(GUILayout.Width(width));
                if (GUILayout.Button(buttonsIcons["Pivot_Button_Top_L_unactive"]))
                {
                    SetCutomAnchorsButton_OnClick(CustomAnchorTypes.LeftTopCorner);
                }
                if (GUILayout.Button(buttonsIcons["Pivot_Button_Top_Center_unactive"]))
                {
                    SetCutomAnchorsButton_OnClick(CustomAnchorTypes.Top);
                }
                if (GUILayout.Button(buttonsIcons["Pivot_Button_Top_R_unactive"]))
                {
                    SetCutomAnchorsButton_OnClick(CustomAnchorTypes.RightTopCorner);
                }
                if (GUILayout.Button(buttonsIcons["Pivot_Button_Streach_Top_unactive"]))
                {
                    SetCutomAnchorsButton_OnClick(CustomAnchorTypes.StretchX_Top);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(GUILayout.Width(width));
                if (GUILayout.Button(buttonsIcons["Pivot_Button_R_Center_unactive"]))
                {
                    SetCutomAnchorsButton_OnClick(CustomAnchorTypes.Left);
                }
                if (GUILayout.Button(buttonsIcons["Pivot_Button_Center_unactive"]))
                {
                    SetCutomAnchorsButton_OnClick(CustomAnchorTypes.Center);
                }
                if (GUILayout.Button(buttonsIcons["Pivot_Button_L_Center_unactive"]))
                {
                    SetCutomAnchorsButton_OnClick(CustomAnchorTypes.Right);
                }
                if (GUILayout.Button(buttonsIcons["Pivot_Button_Streach_Center_Horizontal_unactive"]))
                {
                    SetCutomAnchorsButton_OnClick(CustomAnchorTypes.StretchX);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(GUILayout.Width(width));
                if (GUILayout.Button(buttonsIcons["Pivot_Button_Down_L_unactive"]))
                {
                    SetCutomAnchorsButton_OnClick(CustomAnchorTypes.LeftBottomCorner);
                }
                if (GUILayout.Button(buttonsIcons["Pivot_Button_Down_Center_unactive"]))
                {
                    SetCutomAnchorsButton_OnClick(CustomAnchorTypes.Bottom);
                }
                if (GUILayout.Button(buttonsIcons["Pivot_Button_Down_R_unactive"]))
                {
                    SetCutomAnchorsButton_OnClick(CustomAnchorTypes.RightBottomCorner);
                }
                if (GUILayout.Button(buttonsIcons["Pivot_Button_Streach_Down_unactive"]))
                {
                    SetCutomAnchorsButton_OnClick(CustomAnchorTypes.StretchX_Bottom);
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(GUILayout.Width(width));
                if (GUILayout.Button(buttonsIcons["Pivot_Button_Streach_L_unactive"]))
                {
                    SetCutomAnchorsButton_OnClick(CustomAnchorTypes.StretchY_Left);
                }
                if (GUILayout.Button(buttonsIcons["Pivot_Button_Streach_Center_Vertical_unactive"]))
                {
                    SetCutomAnchorsButton_OnClick(CustomAnchorTypes.StretchY);
                }
                if (GUILayout.Button(buttonsIcons["Pivot_Button_Streach_Center_R_unactive"]))
                {
                    SetCutomAnchorsButton_OnClick(CustomAnchorTypes.StretchY_Right);
                }
                if (GUILayout.Button(buttonsIcons["Pivot_Button_Streach_Center_unactive"]))
                {
                    SetCutomAnchorsButton_OnClick(CustomAnchorTypes.OnCorners);
                }
                GUILayout.EndHorizontal();
            }         
        }
        else    //Show when there is no parent
        {
            GUILayout.Label("No parent with RectTransform", EditorStyles.centeredGreyMiniLabel);
        }
    }

    private void GetData()
    {
        _target = target as RectTransform;
        HasParent = _target.parent != null && _target.parent.GetComponent<RectTransform>() != null;
    }

    protected override void OnEnable()
    {
        base.OnEnable();


        LoadButtonsSprites();
    }

    private void LoadButtonsSprites()
    {
        List<string> paths = new List<string>()
        {
            "Pivot_Button_Center",
            "Pivot_Button_Down_Center",
            "Pivot_Button_Down_L",
            "Pivot_Button_Down_R",
            "Pivot_Button_L_Center",
            "Pivot_Button_R_Center",
            "Pivot_Button_Streach_Center",
            "Pivot_Button_Streach_Center_Horizontal",
            "Pivot_Button_Streach_Center_R",
            "Pivot_Button_Streach_Center_Vertical",
            "Pivot_Button_Streach_Down",
            "Pivot_Button_Streach_L",
            "Pivot_Button_Streach_Top",
            "Pivot_Button_Top_Center",
            "Pivot_Button_Top_L",
            "Pivot_Button_Top_R"
        };

        foreach (var path in paths)
        {
            string active = path + "_active";
            string inactive = path + "_unactive";
            buttonsIcons.Add(active, Resources.Load<Sprite>("Icons/" + active).texture);
            buttonsIcons.Add(inactive, Resources.Load<Sprite>("Icons/" + inactive).texture);
        }
    }

    private void SetUpCustomAnchors(RectTransform rectTransform, CustomAnchorTypes customAnchorType)
    {
        Vector2 pivot = rectTransform.pivot;

        //rectTransform.pivot = new Vector2(0.5f, 0.5f);


        var absoluteSize = RectTransformUtilities.GetAbsoluteSize(rectTransform);
        var absolutePos = RectTransformUtilities.GetAbsolutePosition(rectTransform);
        var parentAbsoluteSize = RectTransformUtilities.GetAbsoluteSize(rectTransform.parent.GetComponent<RectTransform>());
        var offsetForAnchors = new Vector2((0.5f - rectTransform.pivot.x) * rectTransform.sizeDelta.x, (0.5f - rectTransform.pivot.y) * rectTransform.sizeDelta.y);
        var offsetForAnchoredPosition = new Vector2((0.5f - rectTransform.pivot.x) * absoluteSize.x, (0.5f - rectTransform.pivot.y) * absoluteSize.y);

        Debug.Log(offsetForAnchors);
        Debug.Log(offsetForAnchoredPosition);

        absolutePos += offsetForAnchors;
        rectTransform.pivot = new Vector2(0.5f, 0.5f);

        rectTransform.anchorMin = new Vector2();
        rectTransform.anchorMax = new Vector2();

        rectTransform.anchorMin = CustomAnchorMin(absoluteSize, absolutePos, parentAbsoluteSize, rectTransform.pivot, customAnchorType);
        rectTransform.anchorMax = CustomAnchorMax(absoluteSize, absolutePos, parentAbsoluteSize, rectTransform.pivot, customAnchorType);

        rectTransform.anchoredPosition = CustomAnchorPosition(absoluteSize, customAnchorType, offsetForAnchoredPosition);
        rectTransform.sizeDelta = CustomAnchorSize(absoluteSize, customAnchorType);

        rectTransform.pivot = pivot;
    }

    private Vector2 CustomAnchorMin(Vector2 absoluteSize, Vector2 absolutePos, Vector2 parentAbsoluteSize, Vector2 pivot,
                                    CustomAnchorTypes customAnchorType)
    {
        float anchorMinX = 0f;
        float anchorMinY = 0f;
        switch (customAnchorType)
        {
            case CustomAnchorTypes.OnCorners:
                anchorMinX = (absolutePos.x - absoluteSize.x * pivot.x) / parentAbsoluteSize.x;
                anchorMinY = (absolutePos.y - absoluteSize.y * pivot.y) / parentAbsoluteSize.y;
                break;
            case CustomAnchorTypes.StretchX:
                anchorMinX = (absolutePos.x - absoluteSize.x * pivot.x) / parentAbsoluteSize.x;
                anchorMinY = absolutePos.y / parentAbsoluteSize.y;
                break;
            case CustomAnchorTypes.StretchX_Bottom:
                anchorMinX = (absolutePos.x - absoluteSize.x * pivot.x) / parentAbsoluteSize.x;
                anchorMinY = (absolutePos.y - absoluteSize.y * pivot.y) / parentAbsoluteSize.y;
                break;
            case CustomAnchorTypes.StretchX_Top:
                anchorMinX = (absolutePos.x - absoluteSize.x * pivot.x) / parentAbsoluteSize.x;
                anchorMinY = (absolutePos.y + absoluteSize.y * (1.0f - pivot.y)) / parentAbsoluteSize.y;
                break;

            case CustomAnchorTypes.StretchY:
                anchorMinX = absolutePos.x / parentAbsoluteSize.x;
                anchorMinY = (absolutePos.y - absoluteSize.y * pivot.y) / parentAbsoluteSize.y;
                break;
            case CustomAnchorTypes.StretchY_Left:
                anchorMinX = (absolutePos.x - absoluteSize.x * pivot.x) / parentAbsoluteSize.x;
                anchorMinY = (absolutePos.y - absoluteSize.y * pivot.y) / parentAbsoluteSize.y;
                break;
            case CustomAnchorTypes.StretchY_Right:
                anchorMinX = (absolutePos.x + absoluteSize.x * (1.0f - pivot.x)) / parentAbsoluteSize.x;
                anchorMinY = (absolutePos.y - absoluteSize.y * pivot.y) / parentAbsoluteSize.y;
                break;

            case CustomAnchorTypes.LeftTopCorner:
                anchorMinX = (absolutePos.x - absoluteSize.x * pivot.x) / parentAbsoluteSize.x;
                anchorMinY = (absolutePos.y + absoluteSize.y * (1.0f - pivot.y)) / parentAbsoluteSize.y;
                break;
            case CustomAnchorTypes.Top:
                anchorMinX = absolutePos.x / parentAbsoluteSize.x;
                anchorMinY = (absolutePos.y + absoluteSize.y * (1.0f - pivot.y)) / parentAbsoluteSize.y;
                break;
            case CustomAnchorTypes.RightTopCorner:
                anchorMinX = (absolutePos.x + absoluteSize.x * (1.0f - pivot.x)) / parentAbsoluteSize.x;
                anchorMinY = (absolutePos.y + absoluteSize.y * (1.0f - pivot.y)) / parentAbsoluteSize.y;
                break;

            case CustomAnchorTypes.Left:
                anchorMinX = (absolutePos.x - absoluteSize.x * pivot.x) / parentAbsoluteSize.x;
                anchorMinY = absolutePos.y / parentAbsoluteSize.y;
                break;
            case CustomAnchorTypes.Center:
                anchorMinX = absolutePos.x / parentAbsoluteSize.x;
                anchorMinY = absolutePos.y / parentAbsoluteSize.y;
                break;
            case CustomAnchorTypes.Right:
                anchorMinX = (absolutePos.x + absoluteSize.x * (1.0f - pivot.x)) / parentAbsoluteSize.x;
                anchorMinY = absolutePos.y / parentAbsoluteSize.y;
                break;

            case CustomAnchorTypes.LeftBottomCorner:
                anchorMinX = (absolutePos.x - absoluteSize.x * pivot.x) / parentAbsoluteSize.x;
                anchorMinY = (absolutePos.y - absoluteSize.y * pivot.y) / parentAbsoluteSize.y;
                break;
            case CustomAnchorTypes.Bottom:
                anchorMinX = absolutePos.x / parentAbsoluteSize.x;
                anchorMinY = (absolutePos.y - absoluteSize.y * pivot.y) / parentAbsoluteSize.y;
                break;
            case CustomAnchorTypes.RightBottomCorner:
                anchorMinX = (absolutePos.x + absoluteSize.x * (1.0f - pivot.x)) / parentAbsoluteSize.x;
                anchorMinY = (absolutePos.y - absoluteSize.y * pivot.y) / parentAbsoluteSize.y;
                break;   
        }
        return new Vector2(anchorMinX, anchorMinY);

    }

    private Vector2 CustomAnchorMax(Vector2 absoluteSize, Vector2 absolutePos, Vector2 parentAbsoluteSize, Vector2 pivot,
                                    CustomAnchorTypes customAnchorType)
    {
        float anchorMaxX = 0f;
        float anchorMaxY = 0f;
        switch (customAnchorType)
        {
            case CustomAnchorTypes.OnCorners:
                anchorMaxX = (absolutePos.x + absoluteSize.x * (1.0f - pivot.x)) / parentAbsoluteSize.x;
                anchorMaxY = (absolutePos.y + absoluteSize.y * (1.0f - pivot.y)) / parentAbsoluteSize.y;
                break;
            case CustomAnchorTypes.StretchX:
                anchorMaxX = (absolutePos.x + absoluteSize.x * (1.0f - pivot.x)) / parentAbsoluteSize.x;
                anchorMaxY = absolutePos.y / parentAbsoluteSize.y;
                break;
            case CustomAnchorTypes.StretchX_Bottom:
                anchorMaxX = (absolutePos.x + absoluteSize.x * (1.0f - pivot.x)) / parentAbsoluteSize.x;
                anchorMaxY = (absolutePos.y - absoluteSize.y * pivot.y) / parentAbsoluteSize.y;
                break;
            case CustomAnchorTypes.StretchX_Top:
                anchorMaxX = (absolutePos.x + absoluteSize.x * (1.0f - pivot.x)) / parentAbsoluteSize.x;
                anchorMaxY = (absolutePos.y + absoluteSize.y * (1.0f - pivot.y)) / parentAbsoluteSize.y;
                break;
            case CustomAnchorTypes.StretchY:
                anchorMaxX = absolutePos.x / parentAbsoluteSize.x;
                anchorMaxY = (absolutePos.y + absoluteSize.y * (1.0f - pivot.y)) / parentAbsoluteSize.y;
                break;
            case CustomAnchorTypes.StretchY_Left:
                anchorMaxX = (absolutePos.x - absoluteSize.x * pivot.x) / parentAbsoluteSize.x;
                anchorMaxY = (absolutePos.y + absoluteSize.y * (1.0f - pivot.y)) / parentAbsoluteSize.y;
                break;
            case CustomAnchorTypes.StretchY_Right:
                anchorMaxX = (absolutePos.x + absoluteSize.x * (1.0f - pivot.x)) / parentAbsoluteSize.x;
                anchorMaxY = (absolutePos.y + absoluteSize.y * (1.0f - pivot.y)) / parentAbsoluteSize.y;
                break;
            case CustomAnchorTypes.LeftTopCorner:
                anchorMaxX = (absolutePos.x - absoluteSize.x * pivot.x) / parentAbsoluteSize.x;
                anchorMaxY = (absolutePos.y + absoluteSize.y * (1.0f - pivot.y)) / parentAbsoluteSize.y;
                break;
            case CustomAnchorTypes.Top:
                anchorMaxX = absolutePos.x / parentAbsoluteSize.x;
                anchorMaxY = (absolutePos.y + absoluteSize.y * (1.0f - pivot.y)) / parentAbsoluteSize.y;
                break;
            case CustomAnchorTypes.RightTopCorner:
                anchorMaxX = (absolutePos.x + absoluteSize.x * (1.0f - pivot.x)) / parentAbsoluteSize.x;
                anchorMaxY = (absolutePos.y + absoluteSize.y * (1.0f - pivot.y)) / parentAbsoluteSize.y;
                break;

            case CustomAnchorTypes.Left:
                anchorMaxX = (absolutePos.x - absoluteSize.x * pivot.x) / parentAbsoluteSize.x;
                anchorMaxY = absolutePos.y / parentAbsoluteSize.y;
                break;
            case CustomAnchorTypes.Center:
                anchorMaxX = absolutePos.x / parentAbsoluteSize.x;
                anchorMaxY = absolutePos.y / parentAbsoluteSize.y;
                break;
            case CustomAnchorTypes.Right:
                anchorMaxX = (absolutePos.x + absoluteSize.x * (1.0f - pivot.x)) / parentAbsoluteSize.x;
                anchorMaxY = absolutePos.y / parentAbsoluteSize.y;
                break;

            case CustomAnchorTypes.LeftBottomCorner:
                anchorMaxX = (absolutePos.x - absoluteSize.x * pivot.x) / parentAbsoluteSize.x;
                anchorMaxY = (absolutePos.y - absoluteSize.y * pivot.y) / parentAbsoluteSize.y;
                break;

            case CustomAnchorTypes.Bottom:
                anchorMaxX = absolutePos.x / parentAbsoluteSize.x;
                anchorMaxY = (absolutePos.y - absoluteSize.y * pivot.y) / parentAbsoluteSize.y;
                break;
           
            case CustomAnchorTypes.RightBottomCorner:
                anchorMaxX = (absolutePos.x + absoluteSize.x * (1.0f - pivot.x)) / parentAbsoluteSize.x;
                anchorMaxY = (absolutePos.y - absoluteSize.y * pivot.y) / parentAbsoluteSize.y;
                break;   

        }
        return new Vector2(anchorMaxX, anchorMaxY);
    }

    private Vector2 CustomAnchorPosition(Vector2 absoluteSize, CustomAnchorTypes customAnchorType, Vector2 offset)
    {
        switch (customAnchorType)
        {
            case CustomAnchorTypes.OnCorners:
                return new Vector2();
            case CustomAnchorTypes.StretchX:
                return new Vector2(0, -offset.y);
            case CustomAnchorTypes.StretchX_Bottom:
                return new Vector2(0f, absoluteSize.y / 2 - offset.y);
            case CustomAnchorTypes.StretchX_Top:
                return new Vector2(0f, -absoluteSize.y / 2 - offset.y);

            case CustomAnchorTypes.StretchY:
                return new Vector2(-offset.x, 0);
            case CustomAnchorTypes.StretchY_Left:
                return new Vector2(absoluteSize.x / 2 - offset.x, 0);
            case CustomAnchorTypes.StretchY_Right:
                return new Vector2(-absoluteSize.x / 2 - offset.x, 0);

            case CustomAnchorTypes.LeftTopCorner:
                return new Vector2(absoluteSize.x / 2, -absoluteSize.y / 2) - offset;
            case CustomAnchorTypes.Top:
                return new Vector2(0f, -absoluteSize.y / 2) - offset;
            case CustomAnchorTypes.RightTopCorner:
                return new Vector2(-absoluteSize.x / 2, -absoluteSize.y / 2) - offset;

            case CustomAnchorTypes.Left:
                return new Vector2(absoluteSize.x / 2, 0f) - offset;
            case CustomAnchorTypes.Center:
                return new Vector2() - offset;
            case CustomAnchorTypes.Right:
                return new Vector2(-absoluteSize.x / 2, 0f) - offset;

            case CustomAnchorTypes.LeftBottomCorner:
                return new Vector2(absoluteSize.x / 2, absoluteSize.y / 2) - offset;
            case CustomAnchorTypes.Bottom:
                return new Vector2(0f, absoluteSize.y / 2) - offset;
            case CustomAnchorTypes.RightBottomCorner:
                return new Vector2(-absoluteSize.x / 2, absoluteSize.y / 2) - offset;
            default:
                return new Vector2() - offset;
        }
    }

    private Vector2 CustomAnchorSize(Vector2 absoluteSize, CustomAnchorTypes customAnchorType)
    {
        switch (customAnchorType)
        {
            case CustomAnchorTypes.OnCorners:
                return new Vector2();

            case CustomAnchorTypes.StretchX:
                return new Vector2(0f, absoluteSize.y);
            case CustomAnchorTypes.StretchX_Bottom:
                return new Vector2(0f, absoluteSize.y);
            case CustomAnchorTypes.StretchX_Top:
                return new Vector2(0f, absoluteSize.y);

            case CustomAnchorTypes.StretchY:
                return new Vector2(absoluteSize.x, 0f);
            case CustomAnchorTypes.StretchY_Left:
                return new Vector2(absoluteSize.x, 0f);
            case CustomAnchorTypes.StretchY_Right:
                return new Vector2(absoluteSize.x, 0f);

            case CustomAnchorTypes.LeftTopCorner:
                return new Vector2(absoluteSize.x, absoluteSize.y);
            case CustomAnchorTypes.Top:
                return new Vector2(absoluteSize.x, absoluteSize.y);
            case CustomAnchorTypes.RightTopCorner:
                return new Vector2(absoluteSize.x, absoluteSize.y);

            case CustomAnchorTypes.Left:
                return new Vector2(absoluteSize.x, absoluteSize.y);
            case CustomAnchorTypes.Center:
                return new Vector2(absoluteSize.x, absoluteSize.y);
            case CustomAnchorTypes.Right:
                return new Vector2(absoluteSize.x, absoluteSize.y);

            case CustomAnchorTypes.LeftBottomCorner:
                return new Vector2(absoluteSize.x, absoluteSize.y);
            case CustomAnchorTypes.Bottom:
                return new Vector2(absoluteSize.x, absoluteSize.y);
            case CustomAnchorTypes.RightBottomCorner:
                return new Vector2(absoluteSize.x, absoluteSize.y);
            default:
                return new Vector2();
        }
    }

    public void SetCutomAnchorsButton_OnClick(CustomAnchorTypes customAnchorType)
    {
        //Iterate over all targets in this editor
        foreach (var t in targets)
        {
            //Register changes made for specific rect transform
            Undo.RecordObject(t, ButtonText);

            //Set anchor which depends on given parameter customAnchorType
            SetUpCustomAnchors(t as RectTransform, customAnchorType);
        }
    }
}

public enum CustomAnchorTypes
{
    OnCorners,
    StretchX,
    StretchX_Bottom,
    StretchX_Top,
    StretchY,
    StretchY_Left,
    StretchY_Right,
    LeftTopCorner,
    Top,
    RightTopCorner,
    Left,
    Center,
    Right,
    LeftBottomCorner,
    Bottom,
    RightBottomCorner
}
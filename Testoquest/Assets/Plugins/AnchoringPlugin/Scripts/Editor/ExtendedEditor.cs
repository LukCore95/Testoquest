using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Extension for UnityEditor classes by BaalEvan
/// </summary>
public class ExtendedEditor : Editor
{
    protected Editor defaultEditor;

    protected virtual void OnDisable()
    {
        //Call default desctructor for default editor if exists
        if (defaultEditor != null)
        {
            MethodInfo disableMethod = defaultEditor.GetType()
                .GetMethod("OnDisable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (disableMethod != null)
                disableMethod.Invoke(defaultEditor, null);

            DestroyImmediate(defaultEditor);
        }
    }

    protected virtual void OnEnable()
    {
        EditorExtensionAttribute myAttribute =
             (EditorExtensionAttribute)Attribute.GetCustomAttribute(GetType(), typeof(EditorExtensionAttribute));

        EditorExtensionPlainAttribute myPlainAttribute =
            (EditorExtensionPlainAttribute)
                Attribute.GetCustomAttribute(GetType(), typeof(EditorExtensionPlainAttribute));

        if (myAttribute != null)
        {
            defaultEditor = CreateEditor(targets, myAttribute.extendedEditorType);
        }
        else
        {
            if (myPlainAttribute != null)
            {
                defaultEditor = CreateEditor(targets, Type.GetType(myPlainAttribute.extendedEditorType));
            }
        }
    }

    public override void OnInspectorGUI()
    {
        //Draw default editor GUI
        if (defaultEditor != null)
            defaultEditor.OnInspectorGUI();

        //Make space for custom editor fields
        GUILayout.Space(10);
    }

    public void OnSceneGUI()
    {
        //Call default OnSceneGUI method if exists in default editor
        MethodInfo dynMethod = defaultEditor.GetType().GetMethod("OnSceneGUI", BindingFlags.NonPublic | BindingFlags.Instance);

        if (dynMethod != null)
            dynMethod.Invoke(defaultEditor, null);
    }
}

public class EditorExtensionAttribute : Attribute
{
    public Type extendedEditorType;

    public EditorExtensionAttribute(Type type)
    {
        extendedEditorType = type;
    }
}

public class EditorExtensionPlainAttribute : Attribute
{
    public string extendedEditorType;

    public EditorExtensionPlainAttribute(string type)
    {
        extendedEditorType = type;
    }
}
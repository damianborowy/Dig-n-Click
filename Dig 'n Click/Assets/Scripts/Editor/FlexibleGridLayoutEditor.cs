using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FlexibleGridLayout))]
public class FlexibleGridLayoutEditor : Editor
{
    private SerializedProperty _fillPercent;

    private void OnEnable()
    {
        _fillPercent = serializedObject.FindProperty("FillPercent");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_fillPercent);
        serializedObject.ApplyModifiedProperties();
    }
}

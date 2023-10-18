using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(ScriptTest))]
public class ScriptTestEditor : Editor
{
    ScriptTest _myObject = null;

    bool _toggleForceButtons;

    private void OnEnable()
    {
        _myObject = (ScriptTest)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("My label");
        EditorGUILayout.EndHorizontal();
        _myObject.text = EditorGUILayout.TextField("Text Area", _myObject.text);
        serializedObject.ApplyModifiedProperties();
    }
}

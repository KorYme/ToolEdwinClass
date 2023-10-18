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
    SerializedProperty m_googleDriveProperty;

    bool _toggleForceButtons;

    private void OnEnable()
    {
        _myObject = (ScriptTest)target;
        m_googleDriveProperty = serializedObject.FindProperty("GoogleDriveScript");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("My label");
        EditorGUILayout.EndHorizontal();
        _myObject.text = EditorGUILayout.TextField("Text Area", _myObject.text);
        EditorGUILayout.PropertyField(m_googleDriveProperty, new GUIContent("GoogleDriveScript"));
        GoogleDriveTest gd = m_googleDriveProperty.objectReferenceValue as GoogleDriveTest;
        if (GUILayout.Button("Download"))
        {
            gd?.Download();
        }
        if (GUILayout.Button("Upload"))
        {
            gd?.Upload();
        }
        _toggleForceButtons = EditorGUILayout.Foldout(_toggleForceButtons, "Force procedures");
        if (_toggleForceButtons)
        {            
            if (GUILayout.Button("Force Download"))
            {
                gd?.ForceDownload();
            }
            if (GUILayout.Button("Force Upload"))
            {
                gd?.ForceUpload();
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}

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
        if (GUILayout.Button("Download"))
        {
            GoogleDriveTest gd = m_googleDriveProperty.objectReferenceValue as GoogleDriveTest;
            gd?.TestDownload();
        }
        if (GUILayout.Button("Upload"))
        {
            GoogleDriveTest gd = m_googleDriveProperty.objectReferenceValue as GoogleDriveTest;
            gd.TestUpload();
        }
        if (GUILayout.Button("Test"))
        {
            GoogleDriveTest gd = m_googleDriveProperty.objectReferenceValue as GoogleDriveTest;
            gd?.DisplayAllFiles();
        }
        serializedObject.ApplyModifiedProperties();
    }
}

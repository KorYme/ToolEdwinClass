using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.VersionControl;
using UnityEngine;

public class WriterTest : MonoBehaviour
{
    public string UnityFilePath => Path.Combine(Application.dataPath, "Test", "Unity.jpg");
    public void WriteFile(byte[] fileContent) => File.WriteAllBytes(UnityFilePath, fileContent);
    public byte[] ReadFile() => File.ReadAllBytes(UnityFilePath);
}
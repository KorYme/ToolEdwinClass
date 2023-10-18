using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.VersionControl;
using UnityEngine;

public class WwiseSFXFolderModifier : MonoBehaviour
{
    string WwiseSFXFilePath => Path.Combine(Application.dataPath, @"..\", Application.productName + "_WwiseProject", "Originals", "SFX");

    public void WriteFile(byte[] fileContent, string fileName) => File.WriteAllBytes(Path.Combine(WwiseSFXFilePath, fileName), fileContent);
    public byte[] ReadFile(string fileName) => File.ReadAllBytes(Path.Combine(WwiseSFXFilePath, fileName));

    public void ReadFolder()
    {
        Directory.GetFiles(WwiseSFXFilePath).ToList().ForEach(x => print(x));
    }
}
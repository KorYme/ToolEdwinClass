using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.VersionControl;
using UnityEngine;

public class WriterTest : MonoBehaviour
{
    public void WriteFile(byte[] fileContent)
    {
        string path = Path.Combine(Application.dataPath, "Test", "Unity.jpg");

        File.WriteAllBytes(path, fileContent);


        //if (!File.Exists(path))
        //{
        //    // Create a file to write to.
        //    using (StreamWriter sw = File.CreateText(path))
        //    {
        //        sw.WriteLine("Hello");
        //        sw.WriteLine("And");
        //        sw.WriteLine("Welcome");
        //    }
        //}

        // Open the file to read from.
        //using (StreamReader sr = File.OpenText(path))
        //{
        //    string s;
        //    while ((s = sr.ReadLine()) != null)
        //    {
        //        Console.WriteLine(s);
        //    }
        //}
    }
}

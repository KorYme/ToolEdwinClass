using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;
using UnityGoogleDrive;

public class GoogleDriveTest : MonoBehaviour
{
    GoogleDriveFiles.ListRequest request;
    string result = string.Empty;
    string idResult = string.Empty;
    Coroutine _searchCoroutine = null;

    public string googleDriveFilePath = string.Empty;
    public string fileName = string.Empty;
    public WriterTest writer;

    const string GOOGLE_DRIVE_FOLDER = "1A6fu8iMUg3yF1vM0rJ04KBgT-MefGeF0";

    public void TestDownload() => StartCoroutine(DownloadCoroutine());

    IEnumerator DownloadCoroutine()
    {
        yield return _searchCoroutine = StartCoroutine(GetFileByPathRoutine(googleDriveFilePath));
        print(result);
        GoogleDriveFiles.Download(idResult).Send().OnDone += file => writer.WriteFile(file.Content);
    }

    public void TestUpload()
    {
        var file = new UnityGoogleDrive.Data.File(){ Name = fileName, Content = writer.ReadFile(), Parents = new(){ GOOGLE_DRIVE_FOLDER } };
        GoogleDriveFiles.Create(file).Send().OnDone += (x) => print("Send request was a success : " + x.Name);
    }

    public void DestroyOldFile()
    {

    }

    public void Test()
    {
        GoogleDriveFiles.List().Send().OnDone += fileList => fileList.Files.ForEach(x => print(x.Name));
    }

    private IEnumerator GetFileByPathRoutine(string filePath)
    {
        // A folder in Google Drive is actually a file with the MIME type 'application/vnd.google-apps.folder'.
        // Hierarchy relationship is implemented via File's 'Parents' property. To get the actual file using it's path
        // we have to find ID of the file's parent folder, and for this we need IDs of all the folders in the chain.
        // Thus, we need to traverse the entire hierarchy chain using List requests.
        // More info about the Google Drive folders: https://developers.google.com/drive/v3/web/folder.

        var fileName = filePath.Contains("/") ? GetAfter(filePath, "/") : filePath;
        var parentNames = filePath.Contains("/") ? GetBeforeLast(filePath, "/").Split('/') : null;

        // Resolving folder IDs one by one to find ID of the file's parent folder.
        var parentId = "root"; // 'root' is alias ID for the root folder in Google Drive.
        if (parentNames != null)
        {
            for (int i = 0; i < parentNames.Length; i++)
            {
                request = new GoogleDriveFiles.ListRequest();
                request.Fields = new List<string> { "files(id)" };
                request.Q = $"'{parentId}' in parents and name = '{parentNames[i]}' and mimeType = 'application/vnd.google-apps.folder' and trashed = false";

                yield return request.Send();

                if (request.IsError || request.ResponseData.Files == null || request.ResponseData.Files.Count == 0)
                {
                    result = $"Failed to retrieve '{parentNames[i]}' part of '{filePath}' file path.";
                    yield break;
                }

                if (request.ResponseData.Files.Count > 1)
                    Debug.LogWarning($"Multiple '{parentNames[i]}' folders been found.");

                parentId = request.ResponseData.Files[0].Id;
            }
        }

        // Searching the file.
        request = new GoogleDriveFiles.ListRequest();
        request.Fields = new List<string> { "files(id, size, modifiedTime)" };
        request.Q = $"'{parentId}' in parents and name = '{fileName}'";

        yield return request.Send();

        if (request.IsError || request.ResponseData.Files == null || request.ResponseData.Files.Count == 0)
        {
            result = $"Failed to retrieve '{filePath}' file.";
            yield break;
        }

        //if (request.ResponseData.Files.Count > 1)
        //{
        //    Debug.LogWarning($"Multiple (X{request.ResponseData.Files.Count}) '{filePath}' files been found.");
            
        //}

        var file = request.ResponseData.Files[^1];

        result = $"ID: {file.Id}; Size: {file.Size * .001f:0.00}KB;";
        idResult = file.Id;
    }

    private static string GetBeforeLast(string content, string matchString)
    {
        if (content.Contains(matchString))
        {
            var endIndex = content.LastIndexOf(matchString, StringComparison.Ordinal);
            return content.Substring(0, endIndex);
        }
        return null;
    }

    private static string GetAfter(string content, string matchString)
    {
        if (content.Contains(matchString))
        {
            var startIndex = content.LastIndexOf(matchString, StringComparison.Ordinal) + matchString.Length;
            if (content.Length <= startIndex) return string.Empty;
            return content.Substring(startIndex);
        }
        return null;
    }
}

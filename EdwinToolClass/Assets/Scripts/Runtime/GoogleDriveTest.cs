using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityGoogleDrive;

public class GoogleDriveTest : MonoBehaviour
{
    const string GOOGLE_DRIVE_FOLDER_ID = "1A6fu8iMUg3yF1vM0rJ04KBgT-MefGeF0";
    string WwiseSFXFilePath => Path.Combine(Application.dataPath, @"..\", Application.productName + "_WwiseProject", "Originals", "SFX");


    public void Download(bool isForceDownloaded = false)
    {
        List<string> localFiles = Directory.GetFiles(WwiseSFXFilePath).ToList();
        GoogleDriveFiles.ListRequest request = new GoogleDriveFiles.ListRequest() { Q = $"'{GOOGLE_DRIVE_FOLDER_ID}' in parents", Fields = new List<string> { "files(name, id)" } };
        request.Send().OnDone += driveFileList => 
        {
            driveFileList.Files.ForEach(driveFile =>
            {
                if (Path.GetExtension(driveFile.Name) != ".wav") return;
                if (localFiles.FirstOrDefault(localFile => Path.GetFileName(localFile) == driveFile.Name) == default || isForceDownloaded)
                {
                    Debug.Log($"Dowloading {driveFile.Name}");
                    GoogleDriveFiles.Download(driveFile.Id).Send().OnDone += dlFile =>
                    {
                        File.WriteAllBytes(Path.Combine(WwiseSFXFilePath, driveFile.Name), dlFile.Content);
                        Debug.Log($"Download of {driveFile.Name} done, writing the new file in {WwiseSFXFilePath}");
                    };
                }
            });  
        };
        Debug.Log("Download procedure launched");
    }

    public void ForceDownload() => Download(true);


    public void Upload(bool isForceUploaded = false)
    {
        List<string> localFiles = Directory.GetFiles(WwiseSFXFilePath).ToList();
        GoogleDriveFiles.ListRequest request = new GoogleDriveFiles.ListRequest() { Q = $"'{GOOGLE_DRIVE_FOLDER_ID}' in parents and trashed=false", Fields = new List<string> { "files(name, id)" } };
        request.Send().OnDone += driveFileList =>
        {
            localFiles.ForEach(localFile =>
            {
                if (Path.GetExtension(localFile) != ".wav") return;
                string driveFileId = driveFileList.Files.FirstOrDefault(x => Path.GetFileName(localFile) == x.Name)?.Id;
                Debug.Log(driveFileId);
                if (driveFileId == default || isForceUploaded)
                {
                    Debug.Log($"The file {Path.GetFileName(localFile)} is going to be updated on the drive");
                    if (driveFileId != default)
                    {
                        Debug.Log($"Need to delete a file in the Drive");
                        GoogleDriveFiles.DeleteRequest deleteRequest = new(driveFileId);
                        deleteRequest.Send().OnDone += str =>
                        {
                            var file = new UnityGoogleDrive.Data.File() { 
                                Name = Path.GetFileName(localFile), 
                                Content = File.ReadAllBytes(Path.Combine(WwiseSFXFilePath, Path.GetFileName(localFile))), 
                                Parents = new() { GOOGLE_DRIVE_FOLDER_ID } 
                            };
                            GoogleDriveFiles.Create(file).Send().OnDone += createdFile =>
                            {
                                Debug.Log($"Upload of {createdFile.Name} done");
                            };
                        };
                    }
                    else
                    {
                        Debug.Log($"The file {Path.GetFileName(localFile)} is going to be updated on the drive");
                        var file = new UnityGoogleDrive.Data.File()
                        {
                            Name = Path.GetFileName(localFile),
                            Content = File.ReadAllBytes(Path.Combine(WwiseSFXFilePath, Path.GetFileName(localFile))),
                            Parents = new() { GOOGLE_DRIVE_FOLDER_ID }
                        };
                        GoogleDriveFiles.Create(file).Send().OnDone += createdFile =>
                        {
                            Debug.Log($"Upload of {createdFile.Name} done");
                        };
                    }
                }
            });
        };
        Debug.Log("Upload procedure Launched");
    }

    public void ForceUpload() => Upload(true);
}

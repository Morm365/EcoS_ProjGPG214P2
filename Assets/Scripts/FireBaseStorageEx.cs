using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Storage;
using System.IO;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System;

public class FireBaseStorageEx : MonoBehaviour
{

    [SerializeField] List<string> allFilesInBucket = new List<string>();
    [SerializeField] private string destinationFolderPath = Application.streamingAssetsPath;

    private FirebaseStorage storageLocation;
    private StorageReference storageBucket;
    private string storageBucketURL = "gs://ecosystemproj-1435.firebasestorage.app";

    [SerializeField] bool noInternet;

    [SerializeField] List<FileData> storageBuckedFileMetaData = new List<FileData>();  //for each file on the server store its metadata 


    IEnumerator Start()
    {
        if(Application.internetReachability == NetworkReachability.NotReachable || noInternet)
        {

            Debug.Log("NoInternet");

            yield break;
        }


        GetFirebaseInstance();

        GetFirebaseStorageReference();

        if(storageBucket != null)
        {

            yield return StartCoroutine(GetAllFilesInBucket());  //grab all files in bucket
                

        }
            

        if(storageBuckedFileMetaData.Count > 0)
        {

            yield return StartCoroutine(DownloadFiles());

        }

        yield return null;


    }



    void GetFirebaseInstance() 
    {

        storageLocation = FirebaseStorage.DefaultInstance;

        if(storageLocation == null )
        {

            Debug.Log("Storage location not found");

        }


    }

    void GetFirebaseStorageReference()  
    {

        if(storageLocation == null)
        {

            return;

        }

        storageBucket = storageLocation.GetReferenceFromUrl(storageBucketURL);

        if (storageBucket == null)
        {

            Debug.Log("Storage Bucket not found");

        }



    }



    IEnumerator GetAllFilesInBucket()
    {


        for(int i = 0; i < allFilesInBucket.Count; i++ )
        {

            StorageReference fileData = storageBucket.Child(allFilesInBucket[i]);
            
            if(fileData == null)
            {

                Debug.Log("File not found");

                continue;

            }

            Debug.Log("File found" + fileData.Name);

            yield return StartCoroutine(GetFileMetadata(fileData));


        }




        yield return null;

    }




    IEnumerator GetFileMetadata(StorageReference fileToCheck)
    {

        Task<StorageMetadata> fileToCheckMetaData = fileToCheck.GetMetadataAsync();

        //while(fileToCheckMetaData != null && !fileToCheckMetaData.IsCompleted)
        //{

        //    Debug.Log("Getting File Metadata " + fileToCheck.Name);

        //    yield return null;
        //}

        if (fileToCheckMetaData != null && !fileToCheckMetaData.IsCompleted)
        {
            Debug.Log("Getting File Metadata " + fileToCheck.Name);
        }
        while (fileToCheckMetaData != null && !fileToCheckMetaData.IsCompleted)
        {
            yield return null;
        }



        StorageMetadata metadata = fileToCheckMetaData.Result;
        
        if(metadata != null)
        {

            FileData newFile = new FileData();

            newFile.fileName = metadata.Name;
            newFile.dateCreated = metadata.CreationTimeMillis;
            newFile.dateLastModified = metadata.UpdatedTimeMillis;
            newFile.dateCreatedString = metadata.CreationTimeMillis.ToUniversalTime().ToString();
            newFile.dateModifiedString = metadata.UpdatedTimeMillis.ToUniversalTime().ToString();
            newFile.fileSize = metadata.SizeBytes;
            newFile.fileDestination = Path.Combine(destinationFolderPath, metadata.Name);


            storageBuckedFileMetaData.Add(newFile); 

            

        }



        yield return null;

    }


    IEnumerator DownloadFiles()        //all files
    {
        for(int i = 0; i < storageBuckedFileMetaData.Count; i++)
        {

            bool fileExists = File.Exists(storageBuckedFileMetaData[i].fileDestination);

            if(fileExists)  //compare date, delete old and dowload new file
            {

                if(!IsFileUpToDate(new FileInfo(storageBuckedFileMetaData[i].fileDestination), storageBuckedFileMetaData[i]))
                {
                    File.Delete(storageBuckedFileMetaData[i].fileDestination);

                    Debug.Log("Deleting file " + storageBuckedFileMetaData[i].fileName);

                    fileExists = false;

                }


            }

            if(!fileExists)
            {

                yield return DownloadFile(storageBucket.Child(storageBuckedFileMetaData[i].fileName));


            }



            yield return null;
        }



        yield return null;

    }





    private bool IsFileUpToDate(FileInfo localFile, FileData metaData)
    {

        bool isUpToDate = true;

        bool dateIsNewer = false;

        bool fileSizeIsDifferent = false;


        DateTime metaDataTimeUTC = metaData.dateLastModified.ToUniversalTime();
        DateTime localFileTimeUTC = localFile.LastWriteTime.ToUniversalTime();

        metaDataTimeUTC = new DateTime(metaDataTimeUTC.Year, metaDataTimeUTC.Month, metaDataTimeUTC.Day, metaDataTimeUTC.Hour, metaDataTimeUTC.Minute, metaDataTimeUTC.Second);
        localFileTimeUTC = new DateTime(localFileTimeUTC.Year, localFileTimeUTC.Month, localFileTimeUTC.Day, localFileTimeUTC.Hour, localFileTimeUTC.Minute, localFileTimeUTC.Second);

        dateIsNewer = DateTime.Compare(metaDataTimeUTC, localFileTimeUTC) > 0;

        

        fileSizeIsDifferent = localFile.Length != metaData.fileSize;  //compare sizes

        isUpToDate = !(dateIsNewer || fileSizeIsDifferent);

        Debug.Log(isUpToDate);


        return isUpToDate;

    }


    IEnumerator DownloadFile(StorageReference fileToDownload)   //single file
    {

        Task<Uri> uri = fileToDownload.GetDownloadUrlAsync();

        //while(!uri.IsCompleted)
        //{

        //    Debug.Log("Getting URI data " + fileToDownload.Name);

        //    yield return null;

        //}

        if (!uri.IsCompleted)
        {
            Debug.Log("Getting URI data " + fileToDownload.Name);
        }

        while (!uri.IsCompleted)
        {
            yield return null;
        }


        UnityWebRequest www = new UnityWebRequest(uri.Result);

        www.downloadHandler = new DownloadHandlerBuffer();


        yield return www.SendWebRequest(); 

        if(www.result != UnityWebRequest.Result.Success)
        {

            Debug.Log(www.error);

        }
        else
        {

            Debug.Log("Request successful");

            byte[] resultData = www.downloadHandler.data;
            
            while(www.downloadProgress < 1)
            {

                Debug.Log("Downloading " + fileToDownload.Name + (www.downloadProgress * 100) +"%");

                yield return null;
            }


            string destinationPath = Path.Combine(destinationFolderPath, fileToDownload.Name);

            Task writeFile = File.WriteAllBytesAsync(destinationPath, resultData);

            while(!writeFile.IsCompleted)
            {

                Debug.Log("Writing file data, wait " + fileToDownload.Name);

                yield return null;
            }

            Debug.Log("Download completed");


        }




        yield return null;

    }


}

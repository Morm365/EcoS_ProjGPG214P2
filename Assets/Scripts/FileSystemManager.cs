using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;



public class FileSystemManager : MonoBehaviour
{
    




    void Start()
    {

        string assetPath = Application.dataPath;

        string persistentPath = Application.persistentDataPath;

        //streamingAssets

        string streamingAssetsPath = Path.Combine(assetPath, "StreamingAssets");

        if (Directory.Exists(streamingAssetsPath))
        {

            Debug.Log("StreamingAssets Patch: " + streamingAssetsPath);


        }

        else
        {

            Debug.Log("StreamingAssets dont exists ");

            Directory.CreateDirectory(Path.Combine(Application.dataPath, "StreamingAssets"));

            Debug.Log("StreamingAssets Patch: " + streamingAssetsPath);






        }

        //EnemyInfo

        if (File.Exists(Path.Combine(Application.streamingAssetsPath, "EnemyInfo.txt")))
        {

            Debug.Log("EnemyInfo exists ");

        }
        else
        {

            File.CreateText(Path.Combine(Application.streamingAssetsPath, "EnemyInfo.txt"));

            Debug.Log("EnemyInfo created ");



        }















    }


    void Update()
    {
      
        



    }





}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;





public class EnemyData : MonoBehaviour
{

    public Live2 live2;

    private string filePath;



    void Start()
    {

        filePath = Path.Combine(Application.streamingAssetsPath, "EnemyInfo.txt");


    }


    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {

            Save();

        }

        if (Input.GetKeyDown(KeyCode.L))
        {

            Load();

        }



    }


    void Save()
    {

        if (live2 == null) return;

        string data = "health= " + live2.health + "\n" + "hunger= " + live2.hunger + "\n" + "level= " + live2.level + "\n" + "positionX= " + live2.transform.position.x + "\n" + "positionY= " + live2.transform.position.y;

        File.WriteAllText(filePath, data);

        Debug.Log("EnemyData saved");

    }


    void Load()
    {

        if (!File.Exists(filePath))
        {

            Debug.Log("EnemyInfo.txt not found");

            return;

        }


        string[] linesVithData = File.ReadAllLines(filePath); //read each line

        foreach (string line in linesVithData)
        {

            string[] split = line.Split('=');  //split a string into 2 parts using = 

            

            string key = split[0].Trim();

            string value = split[1].Trim();


            float fVal;

            if ((key == "positionX" || key == "positionY") && float.TryParse(value, out fVal))
            {

                Vector3 position = live2.transform.position;

                if (key == "positionX")
                {
                    position.x = fVal;

                }
                else if (key == "positionY")
                {

                    position.y = fVal;

                }

                live2.transform.position = position;

                continue;

            }
                











            int Val;   // transform value into int

            if (!int.TryParse(value, out Val))
            {

                continue;

            }




                if (key == "health")
            {

                live2.health = Val;

            }

            else if (key == "hunger")
            {

                live2.hunger = Val;

            }

            else if (key == "level")
            {

                live2.level = Val;

            }





        }


        Debug.Log("Data Loaded");





    }


}

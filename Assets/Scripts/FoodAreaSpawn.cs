using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodAreaSpawn : MonoBehaviour
{


    public Texture2D foodFormTexture;

    public GameObject objectToSpawn;

    public float spawnDepth = 0f;

    public float spacing = 0.6f;

    public float foodPixelSize = 5f;

    void Start()
    {
        SpawnFoodFromImage();



    }



    

    void SpawnFoodFromImage()
    {

        if (foodFormTexture == null)
        {

            Debug.Log("No Texture");

            return;

        }




        if (foodPixelSize <= 0f || spacing <= 0f)
        {
            Debug.LogError("foodPixelSize and spacing must be > 0");

                return;

        }





            bool[,] occupiedPixels = new bool[foodFormTexture.width, foodFormTexture.height];

        int foodCount = 0;

       // int maxFood = 30;

        for (float y = 0; y< foodFormTexture.height; y += foodPixelSize)
        {

            for(float x = 0; x < foodFormTexture.width; x += foodPixelSize)
            {

                //if (foodCount >= maxFood)
                //{
                //    Debug.Log("Food spawn limit reached");

                //    return;

                //}




                if (CanSpawnFood(foodFormTexture, Mathf.FloorToInt(x), Mathf.FloorToInt(y), occupiedPixels))
                {

                    MarkOccupied(Mathf.FloorToInt(x), Mathf.FloorToInt(y), occupiedPixels);

                    Vector3 spawnPosition = new Vector3(x * spacing, y * spacing, spawnDepth) + transform.position;

                    Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

                    foodCount++;


                    Debug.Log("Food spawned in " + foodCount);

                }



                

            }


                
        }



    }



    bool CanSpawnFood(Texture2D image, int startX, int startY, bool[,] occupied)
    {




        int pixelX = startX + Mathf.FloorToInt(foodPixelSize / 2);


        int pixelY = startY + Mathf.FloorToInt(foodPixelSize / 2);


        if (pixelX >= image.width || pixelY >= image.height)
        {
            return false;

        }
            

        if (occupied[pixelX, pixelY])
        {

            return false;

        }
            

        Color pixelColour = image.GetPixel(pixelX, pixelY);

        return isBlack(pixelColour);


    }


    void MarkOccupied(int startX, int startY, bool[,] occupied)
    {


        int pixelX = startX + Mathf.FloorToInt(foodPixelSize / 2);

        int pixelY = startY + Mathf.FloorToInt(foodPixelSize / 2);


        if (pixelX >= 0 && pixelX < occupied.GetLength(0) && pixelY >= 0 && pixelY < occupied.GetLength(1))
        {

            occupied[pixelX, pixelY] = true;

        }    
            




        //int width = occupied.GetLength(0);

        //int height = occupied.GetLength(1);



        //for (int y = 0; y < Mathf.CeilToInt(foodPixelSize); y++)
        //{

        //    for (int x = 0; y < Mathf.CeilToInt(foodPixelSize); x++)
        //    {
        //        int pixelX = startX + x;
        //        int pixelY = startY + y;


        //       if(pixelX < 0 || pixelX >= width || pixelY < 0 || pixelY >= height)
        //       {

        //            continue;


        //       }

        //        occupied[pixelX, pixelY] = true;

        //    }



        //}

    }


    bool isBlack(Color color)
    {

        // return color.r > 0 && color.g < 1 && color.b < 1;

        return color.grayscale < 0.1f;
    }

    



}

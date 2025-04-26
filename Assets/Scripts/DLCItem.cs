using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;



public class DLCItem : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;


    private string defaultImage = "DefaultImage.png";

    private string dlcImage = "DLCImage.png";




    void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.Log("no spriteRenderer");

            return;
        }

        StartCoroutine(LoadImage());


    }




    IEnumerator LoadImage()
    {


        string pathToLoad = Path.Combine(Application.streamingAssetsPath, defaultImage);


        if (File.Exists(Path.Combine(Application.streamingAssetsPath, dlcImage)))
        {

            pathToLoad = Path.Combine(Application.streamingAssetsPath, dlcImage);

            Debug.Log("DLC dowloaded");
        }

        using (WWW www = new WWW("file:///" + pathToLoad))
        {
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                Texture2D textureIm = www.texture;

                if (textureIm != null)
                {

                    Sprite sprite = Sprite.Create(textureIm, new Rect(0, 0, textureIm.width, textureIm.height), new Vector2(0.5f, 0.5f));

                    spriteRenderer.sprite = sprite;
                }

            }
            else
            {

                Debug.Log("DLC image dowloading failed");

            }



        }



    }



}

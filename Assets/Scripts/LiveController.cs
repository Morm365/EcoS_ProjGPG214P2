using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class LiveController : MonoBehaviour 
{
    public GameObject prefab;

    public Rect spawnArea; 

    public float spawnInterval = 10f;
    
    public int maxObjects = 30;

    private float timer = 0f;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    void Update()
    {

        timer += Time.deltaTime;

        if (timer >= spawnInterval && spawnedObjects.Count < maxObjects)
        {

            timer = 0f;

            Vector2 randomPos = new Vector2(

                Random.Range(spawnArea.xMin, spawnArea.xMax),

                Random.Range(spawnArea.yMin, spawnArea.yMax)

            );

            GameObject obj = Instantiate(prefab, randomPos, Quaternion.identity);

            spawnedObjects.Add(obj);



        }

        


        spawnedObjects.RemoveAll(o => o == null);



    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Vector3 center = new Vector3(spawnArea.center.x, spawnArea.center.y, 0);

        Vector3 size = new Vector3(spawnArea.size.x, spawnArea.size.y, 0);

        Gizmos.DrawWireCube(center, size);

    }



  
}

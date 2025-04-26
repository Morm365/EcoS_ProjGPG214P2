using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SpawnOpti : MonoBehaviour
{

    public GameObject SpawnedObjectTest;


    private Queue<GameObject> pool = new Queue<GameObject>();


    void Start()
    {
       
        InvokeRepeating(nameof(SpawnPool), 0f, 1f);   // call the SpawnFromPool method each second


    }

    void SpawnPool()
    {

        GameObject objSpawn;

        if (pool.Count > 0)           //If there are objects in the pool,  take them from there
        {

            objSpawn = pool.Dequeue();    //take an object from the queue

            objSpawn.SetActive(true);

        }
        else
        {
            objSpawn = Instantiate(SpawnedObjectTest);

        }




        objSpawn.transform.position = Random.insideUnitSphere * 5;

        StartCoroutine(DestroyAfter(objSpawn, 3f));                  //turn off the object and return it to the pool


    }

    System.Collections.IEnumerator DestroyAfter(GameObject objSpawn, float time)
    {
        yield return new WaitForSeconds(time);

        objSpawn.SetActive(false);

        pool.Enqueue(objSpawn);


    }




}

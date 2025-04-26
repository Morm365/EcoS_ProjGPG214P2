using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SpawnNotOpti : MonoBehaviour
{


    public GameObject SpawnedObjectTest;




    void Start()
    {

        InvokeRepeating(nameof(SpawningDestroying), 0f, 1f);


    }


    void SpawningDestroying()
    {

        GameObject objSpawn = Instantiate(SpawnedObjectTest, Random.insideUnitSphere * 5, Quaternion.identity);

        Destroy(objSpawn, 3f);


    } 



}

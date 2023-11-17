using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSpawnTrigger : MonoBehaviour
{
    public IslandSpawnManager spawnManager;
    public bool spawnedIsland = false;
    private Collider terrainCollider;
    // Start is called before the first frame update
    void Start()
    {
        terrainCollider = GetComponent<TerrainCollider>();
        terrainCollider.isTrigger = true;
        print("Trigger? " + terrainCollider.isTrigger);
    }

    void OnTriggerEnter(Collider other)
    {
        print("enter");
        if (!spawnedIsland)
        {
            spawnManager.SpawnObstacle();
            print(other.gameObject.name + " has entered " + gameObject.name);
            spawnedIsland = true;
        }
    }



    //void OnTriggerStay(Collider other)
    //{
    //    print(other.gameObject.name + " is still in " + gameObject.name);
    //}

    //void OnTriggerExit(Collider other)
    //{
    //    print(other.gameObject.name + " has left " + gameObject.name);
    //}
}

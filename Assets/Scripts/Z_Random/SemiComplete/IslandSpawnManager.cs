using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSpawnManager : MonoBehaviour
{
    //public GameObject islandPrefabOLD;
    public GameObject[] IslandSpawnNode;
    public float spawnRangeX = 100f;
    public float minimumSpawnRangeZ = 100f;
    public float maximumSpawnRangeZ = 200f;
    public Terrain islandPrefab;

    private PlayerController playerControllerScript;

    // Start is called before the first frame update
    void Start()
    {        
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        //IslandSpawnNode[0].transform.localPosition = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnObstacle()
    {
        if (!playerControllerScript.IsGameOver)
        {
            //Vector3 spawnPosition = IslandSpawnNode[0].transform.TransformPoint(g); //is this necessary? Can I do this without the node? Alternatives?
            //Vector3 spawnPosition = new Vector3(IslandSpawnNode[0].transform.position.x, transform.position.y, transform.position.z + 10);
            IslandSpawnNode[0].transform.localPosition = generateRandomSpawnPosition();
            Vector3 spawnPosition = IslandSpawnNode[0].transform.localPosition;
            Instantiate(islandPrefab, spawnPosition, islandPrefab.transform.rotation);
            //Destroy(IslandSpawnNode[0]);
        }
    }

    Vector3 generateRandomSpawnPosition()
    {
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        float randomZ = Random.Range(minimumSpawnRangeZ, maximumSpawnRangeZ);
        Vector3 spawnPostion = new Vector3(transform.position.x + randomX, 0, transform.position.z +  randomZ);

        return spawnPostion;
    }
}

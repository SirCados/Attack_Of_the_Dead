using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class IslandManager : MonoBehaviour
{
    //public GameObject Island;
    public ShapeTerrain ShapeTerrainScript;
    public PlayerCharacterController CharacterController;
    public IslandPopulationManager PopulationManager;
    public TerrainLayer[] TerrainLayers;

    private bool _isDoneShaping = false;
    private int _islandCount = 0;
    private bool _isReadyToDestroy = false;
    bool _isSpawnLocked = false;
    float _spawnDelay = 0;


    // Start is called before the first frame update
    void Start()
    {
        SpawnUnvisitedIsland();
    }

    private void Update()
    {
        if (GameObject.FindGameObjectsWithTag("SpawnPoint") != null && GameObject.FindGameObjectsWithTag("SpawnPoint").Length != 0)
        {
            GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
            for (int counter = 0; counter < spawnPoints.Length; counter++)
            {
                print(spawnPoints[counter].name);
            }
        }
    }

    private void LateUpdate()
    {
        if (CharacterController.IsNewIsland)
        {
            PlayerTouchedUnvisitedIsland(CharacterController.NewIslandObject);
            _isReadyToDestroy = true;
        }
        if (_isDoneShaping)
        {
            _isDoneShaping = false;
            GenerateObjects();
        }

        if (_isReadyToDestroy)
        {
            _isReadyToDestroy = false;
            DestroyOldIslands();
        }
        if (_isSpawnLocked)
        {
            _spawnDelay -= Time.deltaTime;
            print(_spawnDelay);
            LockIslandSpawn();
        }
        
    }

    void SpawnUnvisitedIsland()
    {
        LockIslandSpawn();
        _islandCount++;
        //move spawn point, spawn island.
        TerrainData terrainData = new TerrainData();
        PaintTerrain(terrainData);
        terrainData.size = new Vector3(30, 15, 30);
        terrainData.heightmapResolution = 129;
        Vector3 spawnPosition = new Vector3((100 * _islandCount), 0, (100 * _islandCount));
        GameObject freshTerrain = Terrain.CreateTerrainGameObject(terrainData);
        freshTerrain.name = "Island" + _islandCount.ToString(); 
        freshTerrain.tag = "RawIsland";      
        freshTerrain.transform.position = spawnPosition;
        ShapeTerrainScript.MakeIsland(freshTerrain);
        print(Terrain.activeTerrains.Length);
        _isDoneShaping = true;
    }

    void GenerateObjects()
    {
        if (GameObject.FindGameObjectsWithTag("RawIsland") != null && GameObject.FindGameObjectsWithTag("RawIsland").Length != 0)
        {            
            GameObject[] previousIslands = GameObject.FindGameObjectsWithTag("RawIsland");
            for (int counter = 0; counter < previousIslands.Length; counter++)
            {
                PopulationManager.PopulateIsland(previousIslands[counter]);
            }            
        }
    }
    
    void PlayerTouchedUnvisitedIsland(GameObject visitedIsland)
    {
        if (GameObject.FindGameObjectsWithTag("StartArea") != null && GameObject.FindGameObjectsWithTag("StartArea").Length != 0)
        {
            GameObject[] previousIslands = GameObject.FindGameObjectsWithTag("StartArea");
            for (int counter = 0; counter < previousIslands.Length; counter++)
            {
                previousIslands[counter].tag = "Old";
            }
        }

        if (GameObject.FindGameObjectsWithTag("Visited") != null && GameObject.FindGameObjectsWithTag("Visited").Length != 0)
        {
            GameObject[] previousIslands = GameObject.FindGameObjectsWithTag("Visited");
            for (int counter = 0; counter < previousIslands.Length; counter++)
            {
                previousIslands[counter].tag = "Old";
            }
        }

        if (GameObject.FindGameObjectsWithTag("Unvisited") != null && GameObject.FindGameObjectsWithTag("Unvisited").Length != 0)
        {
            GameObject[] previousIslands = GameObject.FindGameObjectsWithTag("Unvisited");
            for (int counter = 0; counter < previousIslands.Length; counter++)
            {
                previousIslands[counter].tag = "Old";
            }
        }
        visitedIsland.tag = "Visited"; //need await here?, or introduce a 4th tag and destroy on last update.
        if (!_isSpawnLocked)
        {
            SpawnUnvisitedIsland();
        }       
    }

    void DestroyOldIslands()
    {
        if (GameObject.FindGameObjectsWithTag("Old") != null && GameObject.FindGameObjectsWithTag("Old").Length != 0)
        {
            GameObject[] previousIslands = GameObject.FindGameObjectsWithTag("Old");
            for (int counter = 0; counter < previousIslands.Length; counter++)
            {
                Destroy(previousIslands[counter]);
            }
        }
    }
    void PaintTerrain(TerrainData terrainData)
    {
        terrainData.terrainLayers = TerrainLayers;
    }

    void LockIslandSpawn()
    {
        if (!_isSpawnLocked)
        {
            _isSpawnLocked = true;
            _spawnDelay = 2;
        }
        if (_isSpawnLocked && _spawnDelay < 0)
        {
            _isSpawnLocked = false;
        }
    }
}

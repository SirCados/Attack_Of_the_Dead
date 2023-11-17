using UnityEngine;

public class IslandPopulationManager : MonoBehaviour
{
    Terrain _terrain;
    int _resolution;
    float _center;
    bool _hasSpawnedWindFont;

    public GameObject[] TerrainObjects;  
    public GameObject WindFont;

    public void PopulateIsland(GameObject terrainToBePopulated)
    {
        _hasSpawnedWindFont = false;
        _terrain = terrainToBePopulated.GetComponent<Terrain>();        
        _resolution = _terrain.terrainData.heightmapResolution;
        _center = (_resolution - 1) / 2;        
        GenerateCircleOfTerrainObjects();
        _terrain.tag = "Unvisited";
    }

    float CheckDistance(float pointX, float pointY)
    {
        float distanceX = Mathf.Pow((pointX - _center), 2);
        float distanceY = Mathf.Pow((pointY - _center), 2);
        float distance = Mathf.Sqrt(distanceX + distanceY);

        return distance;
    }

    void GenerateCircleOfTerrainObjects()
    {
        float distance;
        for (int coordinateToCheckY = 0; coordinateToCheckY < _resolution - 2; coordinateToCheckY++)
        {
            for (int coordinateToCheckX = 0; coordinateToCheckX <= _resolution - 2; coordinateToCheckX++)
            {
                distance = CheckDistance(coordinateToCheckX, coordinateToCheckY) + 10;
                if ((coordinateToCheckX == _resolution / 2 && coordinateToCheckY == _resolution / 2) && !_hasSpawnedWindFont)
                {
                    _hasSpawnedWindFont = true;
                    GenerateTerrainObject(coordinateToCheckX, coordinateToCheckY);
                }
                else if (distance < _center && (Random.Range(1, 50) <= 1))
                {
                    GenerateTerrainObject(coordinateToCheckX, coordinateToCheckY);
                }
            }
        }
    }

    void GenerateTerrainObject(int spawnPositionX, int spawnPositionY)
    {
        Vector3 sampleCoordinates = new Vector3(spawnPositionX + _terrain.transform.position.x, 0, spawnPositionY+ _terrain.transform.position.z);
        //feed terrain into an array? Somehow it is not being concidered here.        
        float cellHeight = _terrain.SampleHeight(sampleCoordinates);
        Vector3 terrainObjectPosition = new Vector3(spawnPositionX, cellHeight - .25f, spawnPositionY);
        GameObject newTerrainObject;
        if (_hasSpawnedWindFont)
        {
            _hasSpawnedWindFont = false;
            newTerrainObject = Instantiate(WindFont, _terrain.transform);
        }
        else
        {
            newTerrainObject = Instantiate(ChooseRandomGameObject());
            newTerrainObject.transform.SetParent(_terrain.transform, true);
        }
       
        newTerrainObject.transform.localPosition = terrainObjectPosition;
    }

    GameObject ChooseRandomGameObject()
    {
        int objectToInstance = Random.Range(0, TerrainObjects.Length);

        if(TerrainObjects[objectToInstance].tag == "EnemySpawnPoint")
        {
            SpawnEnemy randomEnemy = TerrainObjects[objectToInstance].GetComponent<SpawnEnemy>();
            return randomEnemy.GetRandomEnemy();
        }

        return TerrainObjects[objectToInstance];
    }
}

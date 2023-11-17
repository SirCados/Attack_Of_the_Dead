using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeTerrain : MonoBehaviour
{
    private Terrain _terrain;
    private int _resolution;
    private float _center;
    private float _originX = 0f;
    private float _originY = 0f;
    private float _scale;

    /*
     goal of this class is to make a circle with a variable radius
    everything in the circle will be raised
    everything outside of the circle will be lowered to 0
    eventually the circle and the outside of the circle should be smoothed

    Calculate, then if point is in circle, raise it.
     */

    public void MakeIsland(GameObject terrainObject)
    {
        _terrain = terrainObject.GetComponent(typeof(Terrain)) as Terrain;
        _resolution = _terrain.terrainData.heightmapResolution;
        _center = (_resolution - 1) / 2;
        GenerateTerrainData();
        GenerateCircleOfTerrain();
    }

    float CheckDistance(float pointX, float pointY)
    {
        float distanceX = Mathf.Pow((pointX - _center), 2);  
        float distanceY = Mathf.Pow((pointY - _center), 2);
        float distance = Mathf.Sqrt(distanceX + distanceY);

        return distance;
    }

    void GenerateCircleOfTerrain()
    {
        float distance;
        for (int coordinateToRaiseY = 0; coordinateToRaiseY < _resolution - 2; coordinateToRaiseY++)
        {
            for (int coordinateToRaiseX = 0; coordinateToRaiseX <= _resolution - 2; coordinateToRaiseX++)
            {
                distance = CheckDistance(coordinateToRaiseX, coordinateToRaiseY);
                if (distance < _center)
                {
                    float distanceRatio = 1 - (distance/_center);                    
                    RaiseTerrain(coordinateToRaiseX, coordinateToRaiseY, distanceRatio);
                }
                else
                {
                    RemoveTerrain(coordinateToRaiseX, coordinateToRaiseY);
                }
            }
        }
    }

    void GenerateTerrainData()
    {
        //determines starting point of sample from the infinite plane of Perlin Noise
        _originX = Random.Range(0, 1000);
        _originY = Random.Range(0, 1000);
        _scale = Random.Range(3, 8);
    }

    void RaiseTerrain(int coordinateToRaiseX, int coordinateToRaiseY, float distanceRatio)
    {
        float[,] generatedHeightArray;

        float xCoord = ((_originX + coordinateToRaiseX) / _resolution) * _scale;
        float yCoord = ((_originY + coordinateToRaiseY) / _resolution) * _scale;
        float sample = Mathf.PerlinNoise(xCoord, yCoord) * distanceRatio;

        generatedHeightArray = new float[,] { { sample, sample } };
        _terrain.terrainData.SetHeightsDelayLOD(coordinateToRaiseY, coordinateToRaiseX, generatedHeightArray);
    }

    void RemoveTerrain(int coordinateToRaiseX, int coordinateToRaiseY)
    {
        bool[,] holeArray = new bool[,] { { false, false } };
        _terrain.terrainData.SetHolesDelayLOD(coordinateToRaiseY, coordinateToRaiseX, holeArray);
    }
}

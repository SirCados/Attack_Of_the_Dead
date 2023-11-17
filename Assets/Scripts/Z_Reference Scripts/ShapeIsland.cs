using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeIsland : MonoBehaviour
{
    private Terrain terrain;
    private int resolution;
    private float center;
    private float originX = 0f;
    private float originY = 0f;
    private float scale = 5.0F;

    // Start is called before the first frame update
    void Start()
    {
        terrain = Terrain.activeTerrain;
        resolution = terrain.terrainData.heightmapResolution;
        center = (resolution - 1) / 2;
        GeneratePerlinNoiseCoordinates();
        CreateIslandShape();
    }

    void GeneratePerlinNoiseCoordinates()
    {
        //Generates random coordinates on perlin noise plane.
        originX = Random.Range(0, 1000);
        originY = Random.Range(0, 1000);
        //Gets the scale of
        scale = Random.Range(3, 8);
    }

    void CreateIslandShape()
    {
        //Creates an island of terrain. Raising anything within the circle and removing anything outside of the circle.
        float distance;
        for (int coordinateToRaiseY = 0; coordinateToRaiseY < resolution - 1; coordinateToRaiseY++)
        {
            for (int coordinateToRaiseX = 0; coordinateToRaiseX < resolution - 1; coordinateToRaiseX++)
            {
                distance = CheckRadius(coordinateToRaiseX, coordinateToRaiseY);
                if (distance < center)
                {
                    float distanceRatio = 1 - (distance / center);
                    RaiseTerrain(coordinateToRaiseX, coordinateToRaiseY, distanceRatio);
                }
                else
                {
                    RemoveTerrain(coordinateToRaiseX, coordinateToRaiseY);
                }
            }
        }
    }

    float CheckRadius(float pointX, float pointY)
    {
        //Checks distance from center of the terrain to edge of circle terrain
        float distanceX = Mathf.Pow((center - pointX), 2);
        float distanceY = Mathf.Pow((center - pointY), 2);
        float distance = Mathf.Sqrt(distanceX + distanceY);

        return distance;
    }

    void RaiseTerrain(int coordinateToRaiseX, int coordinateToRaiseY, float distanceRatio)
    {
        float[,] generatedHeightArray;

        float xCoord = ((originX + coordinateToRaiseX) / resolution) * scale;
        float yCoord = ((originY + coordinateToRaiseY) / resolution) * scale;
        float sample = Mathf.PerlinNoise(xCoord, yCoord) * distanceRatio;

        generatedHeightArray = new float[,] { { sample, sample } };
        terrain.terrainData.SetHeightsDelayLOD(coordinateToRaiseY, coordinateToRaiseX, generatedHeightArray);
    }

    void RemoveTerrain(int coordinateToRaiseX, int coordinateToRaiseY)
    {
        bool[,] holeArray = new bool[,] { { false, false } };
        terrain.terrainData.SetHolesDelayLOD(coordinateToRaiseY, coordinateToRaiseX, holeArray);
    }
}

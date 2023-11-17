using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveTerrain : MonoBehaviour
{
    private Terrain terrain;
    private int resolution;

    private int terrainX = 0;
    private int terrainY = 0;

    // Start is called before the first frame update
    void Start()
    {
        terrain = Terrain.activeTerrain;
        resolution = terrain.terrainData.heightmapResolution;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("h"))
        {
            PixelByPixel();
        }
    }

    void GenerateCircleOfTerrain()
    {
        float distance;
        for (int coordinateToRaiseY = 0; coordinateToRaiseY < resolution - 1; coordinateToRaiseY++)
        {
            for (int coordinateToRaiseX = 0; coordinateToRaiseX < resolution - 1; coordinateToRaiseX++)
            {
                    MakeHole(coordinateToRaiseX, coordinateToRaiseY);
            }
        }
    }

    void MakeHole(int coordinateToRaiseX, int coordinateToRaiseY)
    {
        bool[,] holeArray = new bool[,] { { false, false } };
        terrain.terrainData.SetHolesDelayLOD(coordinateToRaiseY, coordinateToRaiseX, holeArray);
    }

    void PixelByPixel()
    {
        if (terrainY < resolution)
        {
            if (terrainX < resolution)
            {
                print("Try to make a hole.");
                MakeHole(terrainX, terrainY);
                terrainX++;
            }
            else
            {
                terrainY++;
                terrainX = 0;
            }
        }
    }
}

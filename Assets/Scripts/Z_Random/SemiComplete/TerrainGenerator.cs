using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    private bool isPixelByPixel = false;
    public float scale = 1.0F;

    // The origin of the sampled area in the infinite plane of perlin noise.
    private float originX = 0f;
    private float originY = 0f;

    private Terrain terrain;

    private int resolution;

    private int terrainX = 0;
    private int terrainY = 0;

    private float noiseSample;
    


    // Start is called before the first frame update
    void Start()
    {
        terrain = Terrain.activeTerrain;
        resolution = terrain.terrainData.heightmapResolution;
        print("resolution set to " + resolution);
        SampleSample();  

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("g"))
        {
            GenerateTerrain();
        }
        if (Input.GetKey("p"))
        {
            PixelByPixel();
        }
        if (Input.GetKeyDown("o"))
        {
            GeneratePerlinSampleOrigin();
        }
    }

    void GeneratePerlinSampleOrigin()
    {
        //determines starting point of sample from the infinite plane of Perlin Noise
        //could make these ranges more random.
        originX = Random.Range(0, 1000);
        originY = Random.Range(0, 1000);
        print("with perlin origin (" + originX + ", " + originY + ")");
    }

    void GenerateTerrain()
    {
        isPixelByPixel = false;
        //terrain x and y based on heightmap resolution.
        //ex: heightmap res set to 513x513. Can't go outside these bounds.

        print("Generating");

        for (int coordinateToRaiseY = 0; coordinateToRaiseY < resolution-1; coordinateToRaiseY++)
        {
            for (int coordinateToRaiseX = 0; coordinateToRaiseX < resolution-1; coordinateToRaiseX++)
            {
                RaiseTerrain(coordinateToRaiseX, coordinateToRaiseY);
            }
        }

        terrain.terrainData.SyncHeightmap();
    }

    void SampleSample()
    {
        float xCoord = (originX + 0) / resolution * scale;
        float yCoord = (originY + 0) / resolution * scale;
        print("sample coordinates  (" + xCoord + ", " + yCoord + ")");
        noiseSample = Mathf.PerlinNoise(xCoord, yCoord);
        print("sample noise "+ noiseSample);
    }

    void PixelByPixel()
    {
        isPixelByPixel = true;
        if (terrainY < resolution )
        {
            if(terrainX < resolution)
            {
                RaiseTerrain(terrainX, terrainY);
                terrainX++;
            }
            else
            {
                terrainY++;
                terrainX = 0;
            }
        }
    }

    void RaiseTerrain(int coordinateToRaiseX, int coordinateToRaiseY)
    {
        float[,] generatedHeightArray;

        float xCoord = (originX + coordinateToRaiseX) / resolution * scale;
        float yCoord = (originY + coordinateToRaiseY) /resolution * scale;
        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        if (isPixelByPixel)
        {
            print("sample coordinates: (" + xCoord + ", " + yCoord + ")");
            print("sample noise: " + sample);
        }

        
        generatedHeightArray = new float[,] { { sample, sample } };
        terrain.terrainData.SetHeightsDelayLOD(coordinateToRaiseY, coordinateToRaiseX, generatedHeightArray);
    }

}

/* 
 
        //terrain x and y based on heightmap resolution.
        //ex: heightmap res set to 513x513. Can't go outside these bounds.

each cell represents a "line"
for example, using heightArray{.1,.1} 
Terrain X,Y = (1,1) will generate a raised line across the "cell"
Terrain X,Y = (2,1) will continue the raised line
Terrain X,Y = (1,2) will form a raised square with the line generated at (1,1)
it appears that keeping the values the same yields best results
Do more research into what exactly each value in the 2 dimensional heightArray represents.

So far with this code, the perlin noise has been the same?
The values being entered into Mathf.PerlinNoise are not changing. They are also not a fractional value between 0 and 1.  0 < value < 1

float yCoord = (originY + coordinateToRaiseY) /resolution * scale

it appears that the original calculation from the example is crucial
scale seems to matter a great deal, but why is somewhat of a mystery.


 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNoise : MonoBehaviour
{
    public Renderer rend;

    // Width and height of the texture in pixels.
    public int widthInPixels;
    public int heightInPixels;

    // The origin of the sampled area in the plane.
    public float originX;
    public float originY;

    // The number of cycles of the basic noise pattern that are repeated
    // over the width and height of the texture.
    public float scale = 1.0F;

    public Texture2D noiseTexture;
    private Color[] pixel;

    void Start()
    {
        rend = GetComponent<Renderer>();
        NewTexture();
    }

    void Update()
    {
        if (Input.GetKeyDown("c"))
        {
            GenerateNewOrigin();
            CalculateNoise();
        }
        if (Input.GetKeyDown("a"))
        {
            GenerateNewOrigin();
            AlternateCalculateNoise();
        }

    }

    void NewTexture()
    {
        rend = GetComponent<Renderer>();

        // Set up the texture and a Color array to hold pixels during processing.
        noiseTexture = new Texture2D(widthInPixels, heightInPixels);
        pixel = new Color[noiseTexture.width * noiseTexture.height];
        rend.material.mainTexture = noiseTexture;
    }

    void GenerateNewOrigin()
    {
        //determines starting point of sample from the infinite plane of Perlin Noise
        originX = Random.Range(0, 1000);
        originY = Random.Range(0, 1000);
    }

    void CalculateNoise()
    {
        // For each pixel in the texture...
        //start at the origin(0,0) of the sample from the inifinite plane of Perlin Noise
        float y = 0.0F;

        while (y < noiseTexture.height)
        {
            //start to iterate through the sample, one "column" at a time
            float x = 0.0F;
            while (x < noiseTexture.width)
            {
                //complete the "row" of the sample, then move onto the next "row".
                float xCoord = originX + x / noiseTexture.width * scale;
                float yCoord = originY + y / noiseTexture.height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                pixel[(int)y * noiseTexture.width + (int)x] = new Color(sample, sample, sample);
                x++;
            }
            y++;
        }

        noiseTexture.SetPixels(pixel);
        noiseTexture.Apply();
    }

    void AlternateCalculateNoise()
    {
        // For each pixel in the texture...
        //start at the origin(0,0) of the sample from the inifinite plane of Perlin Noise
        float y = 0.0F;

        while (y < noiseTexture.height)
        {
            //start to iterate through the sample, one "column" at a time
            float x = 0.0F;
            while (x < noiseTexture.width)
            {
                //complete the "row" of the sample, then move onto the next "row".
                float sample = PerlinGenerator(x,y);
                pixel[(int)y * noiseTexture.width + (int)x] = new Color(sample, sample, sample);
                x++;
            }
            y++;
        }

        noiseTexture.SetPixels(pixel);
        noiseTexture.Apply();
    }

    float PerlinGenerator(float coordinateToRaiseX, float coordinateToRaiseY)
    {
        float xCoord = (originX + coordinateToRaiseX) / noiseTexture.width * scale;
        float yCoord = (originY + coordinateToRaiseY) / noiseTexture.height * scale;

        while (xCoord > 1)
        {
            xCoord = xCoord * .1f;
        }
        while (yCoord > 1)
        {
            yCoord = yCoord * .1f;
        }

        float sample = Mathf.PerlinNoise(xCoord, yCoord);

        return sample;
    }

}
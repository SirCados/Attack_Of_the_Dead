using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class squareMilesToPixelsScript : MonoBehaviour
{
    public int TotalSquareMiles;
    public int SquareMilesPerPixel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("g"))
        {
            CalculateInPixels(TotalSquareMiles, SquareMilesPerPixel);
        }
    }

    void CalculateInPixels(int squareMiles, int perPixel)
    {
        int increaseApproximateRange = squareMiles;
        float squareRoot = Mathf.Sqrt(squareMiles/perPixel);

        while (squareRoot % 1 != 0)
        {
            increaseApproximateRange++;
            squareRoot = Mathf.Sqrt(increaseApproximateRange/perPixel);
            print("not perfect square" + squareRoot);
        }
        print("number of square pixels: " + squareRoot);
        print("approximate range: " + increaseApproximateRange);
        

        //int numberOfPixels;

        //return numberOfPixels;, 
    }
}

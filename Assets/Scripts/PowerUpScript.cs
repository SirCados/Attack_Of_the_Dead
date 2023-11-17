using UnityEngine;

public class PowerUpScript : MonoBehaviour
{
    //Most Code is from the Unity Mod the Cube Challenge I did over the Summer
    public MeshRenderer Renderer;
    private Material material;

    //changes the speed of the color change, lower numbers make the change faster
    public float lerpInterval = 1000f;
    public float durationOfLerp = 0f;

    public Color currentColor;
    public Color colorToBecome;
    PlayerStats _playerStats;

    public GameObject Parent;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //probably could make generic object in future to grab damage
            _playerStats = other.gameObject.GetComponent<PlayerStats>();
            RandomPowerUp();
            Destroy(Parent);
        }
    }

    void RandomPowerUp()
    {
        int powerUp = Random.Range(0, 3);

        switch (powerUp)
        {
            case 0:
                _playerStats.GainDamage(1);
                break;
            case 1:
                _playerStats.GainMaxHealth(1);
                break;
            case 2:
                _playerStats.GainSpeed(1);
                break;

        }
    }

    void Start()
    {
        currentColor = generateRandomColor();
        colorToBecome = generateRandomColor();

        material = Renderer.material;
        setMaterialColor();
    }

    void Update()
    {
        //stores current color, which is one pass of the lerp
        currentColor = gradualColorChange();

        if (currentColor == colorToBecome)
        {
            colorToBecome = generateRandomColor();
            durationOfLerp = 0f;
        }

        setMaterialColor();
        rotateCube();
    }

    void setMaterialColor()
    {
        material.color = currentColor;
    }

    Color generateRandomColor()
    {
        //generate random Red, Green, Blue, and Alpha values for a color
        float colorValueR = Random.Range(.1f, 1f);
        float colorValueG = Random.Range(.1f, 1f);
        float colorValueB = Random.Range(.1f, 1f);
        float alphaValue = Random.Range(.1f, 1f);

        Color randomColor = new Color(colorValueR, colorValueG, colorValueB, alphaValue);

        return randomColor;
    }

    Color gradualColorChange()
    {
        //lerp towards the new color from the old color
        Color color = Color.Lerp(material.color, colorToBecome, durationOfLerp);
        //ensures the lerp doesn't take too long towards the end
        durationOfLerp += Time.deltaTime / lerpInterval;
        return color;
    }

    //rotation speed determined by rgb values of old and new color
    void rotateCube()
    {
        float timeMultiplier = Time.deltaTime * 100f;
        float rotateX = calculateRotateSpeedX() * timeMultiplier;
        float rotateY = calculateRotateSpeedY() * timeMultiplier;
        float rotateZ = calculateRotateSpeedZ() * timeMultiplier;
        transform.Rotate(rotateX, rotateY, rotateZ);
    }

    float calculateRotateSpeedX()
    {
        return Mathf.Abs(currentColor.r + colorToBecome.r);
    }

    float calculateRotateSpeedY()
    {
        return Mathf.Abs(currentColor.g + colorToBecome.g);
    }

    float calculateRotateSpeedZ()
    {
        return Mathf.Abs(currentColor.b + colorToBecome.b);
    }
}

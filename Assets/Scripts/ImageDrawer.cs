using UnityEngine;

public class ImageDrawer : MonoBehaviour
{
    public enum NoiseType
    {
        PerlinNoise,
        WorleyMoise
    }

    private Texture2D texture;

    //later change the resolution to match the size of our voxel grid
    const int textureResolution = 256;
    const int gridResolution = 8;

    Vector2[,] perlinPoints;
    Vector2[,] worleyPoints;

    float[,] finalNoiseMap;

    public ImageDrawer.NoiseType noiseType = ImageDrawer.NoiseType.PerlinNoise;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        texture = new Texture2D(textureResolution, textureResolution, TextureFormat.RGBA32, false);
        GetComponent<Renderer>().material.SetTexture("_BaseMap", texture);

        finalNoiseMap = new float[textureResolution, textureResolution];

        switch (noiseType)
        {
            case NoiseType.PerlinNoise:
                PerlinNoise();
                break;
            case NoiseType.WorleyMoise:
                WorleyMoise();
                break;
        }

        texture.Apply();
    }

    //private void OnGUI()
    //{
    //    GUI.DrawTexture(new Rect(0, 0, textureResolution, textureResolution), texture);
    //}

    //used the algorithm from the tutorial https://www.youtube.com/watch?v=9B89kwHvTN4 
    void PerlinNoise() 
    {
        perlinPoints = new Vector2[
            gridResolution + 1,
            gridResolution + 1
        ];

        for (int x = 0; x < gridResolution + 1; x++)
        {
            for (int y = 0; y < gridResolution + 1; y++)
            {

                float pointRotation = Random.Range(0f, 360f);
                float radians = pointRotation * Mathf.Deg2Rad;

                perlinPoints[x, y] = new Vector2(
                    Mathf.Cos(radians),
                    Mathf.Sin(radians)
                );
            }
        }

        for (int x = 0; x < textureResolution; x++)
        {
            for (int y = 0; y < textureResolution; y++)
            {
                float sampleX = (float)x / textureResolution * gridResolution;

                float sampleY = (float)y / textureResolution * gridResolution;

                float noiseValue = CalculatePerlinPoint(
                    sampleX,
                    sampleY
                );

                // this brightness is the final value of the pixel, which is a float between 0 and 1
                // we can use this value to create our voxel
                float brightness = noiseValue * 0.5f + 0.5f;

                finalNoiseMap[x, y] = brightness;

                Color color = new Color(
                    brightness,
                    brightness,
                    brightness
                );

                texture.SetPixel(x, y, color);
            }
        }
    }

    float CalculatePerlinPoint(float sampleX, float sampleY)
    {
        int x0 = Mathf.FloorToInt(sampleX);
        int y0 = Mathf.FloorToInt(sampleY);

        int x1 = x0 + 1;
        int y1 = y0 + 1;

        Vector2 bottomLeftGradient = perlinPoints[x0, y0];
        Vector2 bottomRightGradient = perlinPoints[x1, y0];
        Vector2 topLeftGradient = perlinPoints[x0, y1];
        Vector2 topRightGradient = perlinPoints[x1, y1];

        Vector2 point = new Vector2(sampleX, sampleY);

        Vector2 bottomLeftToPoint = point - new Vector2(x0, y0);
        Vector2 bottomRightToPoint = point - new Vector2(x1, y0);
        Vector2 topLeftToPoint = point - new Vector2(x0, y1);
        Vector2 topRightToPoint = point - new Vector2(x1, y1);

        float bottomLeftValue = Vector2.Dot(bottomLeftGradient, bottomLeftToPoint);
        float bottomRightValue = Vector2.Dot(bottomRightGradient, bottomRightToPoint);
        float topLeftValue = Vector2.Dot(topLeftGradient, topLeftToPoint);
        float topRightValue = Vector2.Dot(topRightGradient, topRightToPoint);

        float percentX = Fade(sampleX - x0);
        float percentY = Fade(sampleY - y0);

        float bottomValue = Mathf.Lerp(
            bottomLeftValue,
            bottomRightValue,
            percentX
        );

        float topValue = Mathf.Lerp(
            topLeftValue,
            topRightValue,
            percentX
        );

        float finalValue = Mathf.Lerp(
            bottomValue,
            topValue,
            percentY
        );

        return finalValue;
    }

    float Fade(float t)
    {
        return t * t * t * (t * (t * 6f - 15f) + 10f);
    }

    void WorleyMoise() {
        //worleyPoints = new Vector2[
        //    gridResolution + 1,
        //    gridResolution + 1
        //];

        //for (int x = 0; x < gridResolution + 1; x++)
        //{
        //    for (int y = 0; y < gridResolution + 1; y++)
        //    {

        //        float randomX = Random.Range(0f, 1f);
        //        float randomY = Random.Range(0f, 1f);

        //        worleyPoints[x, y] = new Vector2(
        //            x + randomX,
        //            y + randomY
        //        );
        //    }
        //}
    }

    // this map store the final noise values for each pixel, which we can use to create our voxel
    public float[,] GetNoiseMap()
    {
        return finalNoiseMap;
    }
}

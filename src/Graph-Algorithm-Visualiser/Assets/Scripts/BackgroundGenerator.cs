using UnityEngine;

// This script generates a square background with side length 2^(specified size), and textures it with a chequerboard pattern where each square represents a partition
// A partition is sized such that 16 circles of the specified radius can be packed exactly into it
public class BackgroundGenerator : MonoBehaviour
{

    [SerializeField] int sizeExponent = 6;
    [SerializeField] int radiusExponent = 1;

    private int previousSizeExponent = 0;
    private int previousRadiusExponent = 0;
    
    void Start()
    {
        UpdateBackground();
    }

    void Update() {
        // Update the background if either the background size or circle radius has changed
        if (sizeExponent != previousSizeExponent || radiusExponent != previousRadiusExponent) {
            UpdateBackground();
        }
    }

    private void UpdateBackground() {
        // Transform the game object, generate the texture and apply it to the renderer component
        gameObject.transform.localScale = new Vector3(Mathf.Pow(2, sizeExponent), 1, Mathf.Pow(2, sizeExponent));

        Texture2D texture = GenerateTexture();

        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null) {
            renderer.material.mainTexture = texture;
        }

        // Update the previous values
        previousSizeExponent = sizeExponent;
        previousRadiusExponent = radiusExponent;
    }

    private Texture2D GenerateTexture() {
        int size = (int)Mathf.Pow(2, sizeExponent);
        int radius = (int)Mathf.Pow(2, radiusExponent);

        // A square which can hold 16 circles has side length 8 times the radius, since the circles are packed 4x4
        int partitionSize = 8 * radius;

        Texture2D texture = new Texture2D(size, size);

        // Change the filter and wrap modes to avoid a blurred texture
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        Color colour1 = new Color(195 / 255f, 160 / 255f, 130 / 255f, 1);
        Color colour2 = new Color(242 / 255f, 225 / 255f, 195 / 255f, 1);

        for (int y = 0; y < size; y++) {
            for (int x = 0; x < size; x++) {
                // Determine the chequerboard colour based on square position
                bool isColour1 = ((x / partitionSize) + (y / partitionSize)) % 2 == 0;
                texture.SetPixel(x, y, isColour1 ? colour1 : colour2);
            }
        }

        texture.Apply();
        return texture;
    }
}

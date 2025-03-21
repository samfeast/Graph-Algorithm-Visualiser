using UnityEngine;

// This script generates a square background with side length 2^(specified size), and textures it with a chequerboard pattern where each square represents a partition
// A partition is sized such that 16 circles of the specified radius can be packed exactly into it
public class BackgroundGenerator : MonoBehaviour
{
    // Enums for the board size, node size, and node spacing
    private enum BoardSize {
        Tiny = 128,
        Small = 256,
        Medium = 512,
        Large = 1024,
        ExtraLarge = 2048
    }
    private enum NodeSize {
        Tiny = 1,
        Small = 2,
        Medium = 4,
        Large = 8,
        ExtraLarge = 16
    }

    private enum NodeSpacing {
        None = 0,
        Small = 1,
        Large = 3
    }

    // Configurable fields
    [SerializeField] private BoardSize boardSize = BoardSize.Medium;

    [SerializeField] private NodeSize nodeSize = NodeSize.Medium;
    [SerializeField] private NodeSpacing nodeSpacing = NodeSpacing.Small;

    private BoardSize prevBoardSize;
    private NodeSize prevNodeSize;
    private NodeSpacing prevNodeSpacing;

    // Background game object (a plane)
    private GameObject background;

    void Start()
    {
        // Create the background plane and rotate is as required
        background = GameObject.CreatePrimitive(PrimitiveType.Plane);
        background.transform.Rotate(new Vector3(-90f, 0, 0));
        UpdateBackground();
    }

    void Update() {
        // Update the background if the configuration has changed
        if (boardSize != prevBoardSize || nodeSize != prevNodeSize || nodeSpacing != prevNodeSpacing) {
            UpdateBackground();
        }
    }

    private void UpdateBackground() {
        // Transform the game object, generate the texture and apply it to the renderer component
        background.transform.localScale = new Vector3((int)boardSize, 1, (int)boardSize);

        Texture2D texture = GenerateTexture();

        Renderer renderer = background.GetComponent<Renderer>();

        if (renderer != null) {
            renderer.material.mainTexture = texture;
        }

        // Update the previous values
        prevBoardSize = boardSize;

        prevNodeSize = nodeSize;
        prevNodeSpacing = nodeSpacing;

    }

    private Texture2D GenerateTexture() {

        int totalRadius = (int)nodeSize * (1 + (int)nodeSpacing);

        // A square which can hold 16 circles has side length 8 times the radius, since the circles are packed 4x4
        int partitionSize = 8 * totalRadius;

        Texture2D texture = new Texture2D((int)boardSize, (int)boardSize);

        // Change the filter and wrap modes to avoid a blurred texture
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        Color colour1 = new Color(195 / 255f, 160 / 255f, 130 / 255f, 1);
        Color colour2 = new Color(242 / 255f, 225 / 255f, 195 / 255f, 1);

        for (int y = 0; y < (int)boardSize; y++) {
            for (int x = 0; x < (int)boardSize; x++) {
                // Determine the chequerboard colour based on square position
                bool isColour1 = ((x / partitionSize) + (y / partitionSize)) % 2 == 0;
                texture.SetPixel(x, y, isColour1 ? colour1 : colour2);
            }
        }

        texture.Apply();
        return texture;
    }
}

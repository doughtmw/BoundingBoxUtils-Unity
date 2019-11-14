using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBoundingBoxes : MonoBehaviour
{
    public Vector2 textureSize;

    [System.Serializable]
    public struct BoundingBoxXYWH
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;
        public string label;
        public Color color;
    }

    public List<BoundingBoxXYWH> boundingBoxes;
    public GameObject textBoxPrefab;

    private Material _material;
    private Texture2D _texture;

    // Start is called before the first frame update
    void Start()
    {
        // Get material component from attached game object via the mesh renderer.
        _material = this.gameObject.GetComponent<MeshRenderer>().material;

        // Create a new texture instance with same size as the canvas.
        _texture = new Texture2D((int)textureSize.x, (int)textureSize.y);

        // Set the texture to transparent (with helper method)
        _texture = Texture2DExtension.TransparentTexture(_texture);

        // Draw bounding boxes at specified coordinates.
        foreach (var box in boundingBoxes)
        {
            DrawBoundingBoxesOnCanvas(box);
        }

        // Apply and set main material texture;
        _texture.Apply();
        _material.mainTexture = _texture;

        ScreenCapture.CaptureScreenshot("Assets/catBoundingBox.jpg");
    }

    private void DrawBoundingBoxesOnCanvas(BoundingBoxXYWH box)
    {
        // Check the bounds of bounding box
        // Give buffer for line drawing to prevent wrap around
        int x1 = box.X > 0.0f ? (int)box.X : 3;
        int y1 = box.Y > 0.0f ? (int)box.Y : 3;
        int x2 = (box.Width + x1) > textureSize.x ? (int)(textureSize.x) - 3 : (int)(box.Width + x1);
        int y2 = (box.Height + y1) > textureSize.y ? (int)(textureSize.y) - 3 : (int)(box.Height + y1);

        Debug.LogFormat("x1: {0}, y1: {1}, x2: {2}, y2: {3}", x1, y1, x2, y2);

        // Plot on texture
        var topLeft = new Vector2(x1, y1);
        var bottomRight = new Vector2(x2, y2);
        _texture = Texture2DExtension.Box(
            _texture,
            topLeft,
            bottomRight,
            box.color);

        // Create a new 3D text object at position and
        // set the label string.Canvas is scaled to x = -0.5, 5
        // and y = -0.5, 0.5.
        var xText = ((topLeft.x / textureSize.x) - 0.5f) + 0.01f;
        var yText = 0.5f - (1.0f - (topLeft.y / textureSize.y));
        var thisBoundingBox = Instantiate(
            textBoxPrefab,
            Vector3.zero,
            Quaternion.identity,
            this.gameObject.transform) as GameObject;

        thisBoundingBox.transform.localPosition = new Vector3(xText, yText, 0f);

       // Set the label of the bounding box.
       thisBoundingBox.GetComponent<TextMesh>().text = box.label;
       thisBoundingBox.GetComponent<TextMesh>().color = box.color;

        // Move the gameobject to become visible
        this.gameObject.transform.localPosition += new Vector3(0, 0, -0.002f);
    }
}

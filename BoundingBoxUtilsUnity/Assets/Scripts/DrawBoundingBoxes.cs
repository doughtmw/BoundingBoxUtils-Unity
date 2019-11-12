using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBoundingBoxes : MonoBehaviour
{
    [System.Serializable]
    public struct BoundingBox
    {
        public Vector2 topLeft;
        public Vector2 bottomRight;
        public string label;
        public Color color;
    }

    public List<BoundingBox> boundingBoxes;
    public GameObject textBoxPrefab;

    private Material _material;
    private Texture2D _texture;

    // Start is called before the first frame update
    void Start()
    {
        // Get material component from attached game object via the mesh renderer.
        _material = this.gameObject.GetComponent<MeshRenderer>().material;

        // Create a new texture instance with same size as the canvas.
        _texture = new Texture2D(1268, 720);

        // Set the texture to transparent (with helper method)
        _texture = Texture2DExtension.TransparentTexture(_texture);

        // Draw bounding boxes at specified coordinates.
        foreach (var boundingBox in boundingBoxes)
        {
            _texture = Texture2DExtension.Box(
                _texture,
                boundingBox.topLeft,
                boundingBox.bottomRight,
                boundingBox.color);

            // Create a new 3D text object at position and 
            // set the label string. Canvas is scaled to x = -0.5, 5
            // and y = -0.5, 0.5.
            var xText = ((boundingBox.bottomRight.x / 1268.0f) - 0.5f) - 0.18f;
            var yText = (0.5f - (1.0f - (boundingBox.bottomRight.y / 720.0f))) - 0.1f;
            var thisBoundingBox = Instantiate(
                textBoxPrefab,
                Vector3.zero,
                Quaternion.identity,
                this.gameObject.transform) as GameObject;

            thisBoundingBox.transform.localPosition = new Vector3(xText, yText, 0f);

            // Set the label of the bounding box.
            thisBoundingBox.GetComponent<TextMesh>().text = boundingBox.label;
            thisBoundingBox.GetComponent<TextMesh>().color = boundingBox.color;
        }


        // Apply and set main material texture;
        _texture.Apply();
        _material.mainTexture = _texture;

        ScreenCapture.CaptureScreenshot("Assets/catBoundingBox.jpg");
    }
}

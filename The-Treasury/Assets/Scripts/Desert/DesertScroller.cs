using UnityEngine;

public class DesertScroller : MonoBehaviour
{
    private MeshRenderer desertMeshRenderer;
    private Vector2 textureOffset;

    [SerializeField] private float scrollSpeed = 1f;

    void Start()
    {
        desertMeshRenderer = GetComponent<MeshRenderer>();
        textureOffset = desertMeshRenderer.material.mainTextureOffset;
    }

    // Update is called once per frame
    void Update()
    {
        textureOffset.x += scrollSpeed * Time.deltaTime;
        desertMeshRenderer.material.mainTextureOffset = textureOffset;
    }
}

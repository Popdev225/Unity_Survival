using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public float scrollSpeed = 0.1f;  
    private Vector2 offset;           
    private Material backgroundMaterial;  

    void Start()
    {
        backgroundMaterial = GetComponent<Renderer>().material;
        offset = new Vector2(0, scrollSpeed);
    }

    void Update()
    {
        backgroundMaterial.mainTextureOffset += offset * Time.deltaTime;
    }
}

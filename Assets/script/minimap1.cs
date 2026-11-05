using UnityEngine;
using UnityEngine.UI;

public class minimap1 : MonoBehaviour
{
    public Camera cam;
    private RenderTexture rt;
    public RawImage img;

    private void Start()
    {
        rt = new RenderTexture(Screen.width, Screen.height, 0);
        cam.targetTexture = rt;
        img.texture = rt;
    }
}





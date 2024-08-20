using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCaptureController : MonoBehaviour
{
    private static ScreenCaptureController instance;

    private Camera camera;
    private bool takeScreenshot;
    int num = 0;

    private void Awake()
    {
        instance = this;
        camera = gameObject.GetComponent<Camera>();
    }

    private void OnPostRender()
    {
        if (takeScreenshot)
        {
            takeScreenshot = false;
            RenderTexture renderTexture = camera.targetTexture;
            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);

            byte[] byteArray = renderResult.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.dataPath + "/CameraScreenshot" + num + ".png", byteArray);
            RenderTexture.ReleaseTemporary(renderTexture);
            camera.targetTexture = null;
            Debug.Log("asdfasfd");
            num++;
        }
    }

    private void TakeScreenshot(int w, int h)
    {
        camera.targetTexture = RenderTexture.GetTemporary(w, h, 16);
        takeScreenshot = true;
    }

    public static void StaticTakeScreenshot(int w, int h)
    {
        instance.TakeScreenshot(w, h);
    }
}

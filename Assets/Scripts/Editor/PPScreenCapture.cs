using UnityEngine;
using UnityEditor;

public class PPScreenCapture : EditorWindow
{
    [MenuItem("Tools/Take Screenshot.../Normal Size %j")]
    static void ScreenshotOne()
    {
        TakeScreenshot(1);
    }
    [MenuItem("Tools/Take Screenshot.../Double Size %k")]
    static void ScreenshotTwo()
    {
        TakeScreenshot(2);
    }
    [MenuItem("Tools/Take Screenshot.../Quadruple Size %l")]
    static void ScreenshotThree()
    {
        TakeScreenshot(4);
    }
    [MenuItem("Tools/Take Screenshot.../With Alpha")]
    static void ScreenshotFour()
    {
        SaveScreenshotToFile();
    }

    static void TakeScreenshot(int quality)
    {
        if (Application.isPlaying)
        {
            string cTime = System.DateTime.Now.ToString("hh.mm.ss");
            string cDate = System.DateTime.Now.ToString("yyyy-MM-dd");

            ScreenCapture.CaptureScreenshot("Screenshot " + cDate + " at " + cTime + ".png", quality);
            Debug.Log("Screenshot taken " + cDate + " at " + cTime + ", Size: " + Screen.width * quality + "x" + Screen.height * quality);
        }
        else
        {
            Debug.Log("Screenshot not taken");
        }
    }

    static Texture2D Screenshot()
    {

        int resWidth = Camera.main.pixelWidth;
        int resHeight = Camera.main.pixelHeight;
        Camera camera = Camera.main;
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 32);
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.ARGB32, false);
        camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenShot.Apply();
        camera.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);
        return screenShot;
    }

    static Texture2D SaveScreenshotToFile()
    {
        string cTime = System.DateTime.Now.ToString("hh.mm.ss");
        string cDate = System.DateTime.Now.ToString("yyyy-MM-dd");
        string fileName = "Screenshot " + cDate + " at " + cTime + ".png";

        Texture2D screenShot = Screenshot();
        byte[] bytes = screenShot.EncodeToPNG();
        System.IO.File.WriteAllBytes(fileName, bytes);
        Debug.Log("Screenshot taken " + cDate + " at " + cTime + ", Size: " + Screen.width + "x" + Screen.height);
        return screenShot;
    }
}
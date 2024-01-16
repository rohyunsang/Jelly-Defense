
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor.Profiling.Memory.Experimental;
using System.Drawing;
/*
public enum Grade
{
    Normal,
    Uncommon,
    Rare,
    Legend,
}

public eneum Size
{
    POT64,
    POT128,
    POT256,
    POT512,
    POT1024,
}
*/

public class Capture : MonoBehaviour
{
    public Camera cam;
    public RenderTexture rt;
    public Image bg;

  //  public Grade grade;
  //  public Size size;

    private void Start()
    {
        cam = Camera.main;
       // SettingColor();
      //  SettingSize();  
    }
    
    public void Create()
    {
        StartCoroutine(CaptureImage());
    }
    IEnumerator CaptureImage()
    {
        yield return null;

        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false, true);
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);

        yield return null;  

        var data = tex.EncodeToPNG();
        string name = "Tumbnail";
        string extention = ".png";
        string path =  Application.persistentDataPath + "/Tumbnail/";

        Debug.Log(path); //저장 위치

        if (!Directory.Exists(path)) Directory.CreateDirectory(path); //경로를 확인해서

        File.WriteAllBytes(path + name + extention, data); //이름, 확장자

        yield return null;
    }
    /*
    void SettingColor()//배경색 넣기
    {
        switch (grade)
        {
            case Grade.Normal;
                cam.backgroundColor = Color.white;
                bgr.Color = Color.white;
                break;
            case Grade.Uncommon;
                cam.backgroundColor = Color.green;
                bgr.Color = Color.green;
                break;
            case Grade.Rare;
                cam.backgroundColor = Color.blue;
                bgr.Color = Color.blue;
                break;
            case Grade.Legend;
                cam.backgroundColor = Color.yellow;
                bgr.Color = Color.yellow;
                break;
        }
    }

    void SettingSize()
    {
        switch (size)
        {
            case Size.POT64;
                rt.width = 64;
                rt.height = 64; 
                break;  
            case Size.POT128;
                rt.width = 128;
                rt.height = 128; 
                break;  
            case Size.POT256;
                rt.width = 256;
                rt.height = 256; 
                break;  
            case Size.POT512;
                rt.width = 512;
                rt.height = 512;
                break;  
            case Size.POT1024;
                rt.width = 1024;
                rt.height = 1024; 
                break;  
            case Size.POT64;
                rt.width = 64;
                rt.height = 64; 
                break;  
        }
    }
    public void AllCreate()
    {
        StartCoroutine(AllCaptureImage());
    }
    IEnumerator AllCaptureImage()
    {
        while (nowCnt < object.Length)
        {

            var nowObj = Instantiate(object[nowCnt].gameObject);

            yield return null;  

            Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false, true);
            RenderTexture.active = rt;
            tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);

            yield return null;

            var data = tex.EncodeToPNG();
            string name = $"Tumbnaill {object[nowCnt].gameObject.name}";
            string extention = ".png";
            string path = Application.persistentDataPath + "/Tumbnail/";

            Debug.Log(path);

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            File.WriteAllBytes(patth + name + extention, data);

            yield return null;

            DestroyImmediate(nowObj);
            nowCnt++; ;

            yield return null;

    }*/
}
/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public enum Grade
{
    Normal,
    UnCommon,
    Rare,
    Legend,
}

public enum Size // Changed 'eneum' to 'enum'
{
    POT64,
    POT128,
    POT256,
    POT512,
    POT1024,
}

public class Capture : MonoBehaviour
{
    public Camera cam;
    public RenderTexture rt;
    public Image bg;

    public Grade grade;
    public Size size;

    private void Start()
    {
        cam = Camera.main;
        SettingColor();
        SettingSize();
    }

    public void Create()
    {
        StartCoroutine(CaptureImage());
    }
    IEnumerator CaptureImage()
    {
        yield return null;

        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false, true);
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);

        yield return null;

        var data = tex.EncodeToPNG();
        string name = "Thumbnail"; // Corrected spelling of 'Thumbnail'
        string extension = ".png"; // Corrected spelling of 'extension'
        string path = Application.persistentDataPath + "/Thumbnail/";

        Debug.Log(path); //저장 위치

        if (!Directory.Exists(path)) Directory.CreateDirectory(path); //경로를 확인해서

        File.WriteAllBytes(path + name + extension, data); //이름, 확장자

        yield return null;
    }

    void SettingColor()//배경색 넣기
    {
        switch (grade)
        {
            case Grade.Normal:
                cam.backgroundColor = Color.white;
                bg.color = Color.white; // Corrected 'bgr' to 'bg'
                break;
            case Grade.Uncommon:
                cam.backgroundColor = Color.green;
                bg.color = Color.green; // Corrected 'bgr' to 'bg'
                break;
            case Grade.Rare:
                cam.backgroundColor = Color.blue;
                bg.color = Color.blue; // Corrected 'bgr' to 'bg'
                break;
            case Grade.Legend:
                cam.backgroundColor = Color.yellow;
                bg.color = Color.yellow; // Corrected 'bgr' to 'bg'
                break;
        }
    }

    void SettingSize()
    {
        switch (size)
        {
            case Size.POT64:
                rt.width = 64;
                rt.height = 64;
                break;
            // Removed duplicate POT64 case
            // Other cases are fine
            case Size.POT128:
                rt.width = 128;
                rt.height = 128;
                break;
            case Size.POT256:
                rt.width = 256;
                rt.height = 256;
                break;
            case Size.POT512:
                rt.width = 512;
                rt.height = 512;
                break;
            case Size.POT1024:
                rt.width = 1024;
                rt.height = 1024;
                break;
        }
    }
    public void AllCreate()
    {
        StartCoroutine(AllCaptureImage());
    }
    IEnumerator AllCaptureImage()
    {
        while (nowCnt < object.Length)
        {

            var nowObj = Instantiate(object[nowCnt].gameObject);

            yield return null;

            Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false, true);
            RenderTexture.active = rt;
            tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);

            yield return null;

            var data = tex.EncodeToPNG();
            string name = $"Tumbnaill {object[nowCnt].gameObject.name}";
            string extention = ".png";
            string path = Application.persistentDataPath + "/Tumbnail/";

            Debug.Log(path);

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            File.WriteAllBytes(path + name + extention, data);

            yield return null;

            DestroyImmediate(nowObj);
            nowCnt++; ;

            yield return null;

        }
    }
*/

#endif
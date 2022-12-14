using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Utility : MonoBehaviour
{ 

    public static string API_KEY = "sk-xxxxxxxxx";
    public static string ORGANIZATION_KEY = "org-xxxxxxxxx";

    public static readonly string resolution_256 = "256x256"; // Possible Resolution 256x256, 512x512, or 1024x1024.
    public static readonly string resolution_512 = "512x512";
    public static readonly string resolution_1024 = "1024x1024";

    public static readonly string maskTextureName = "_mergedTex.png";

    public static Texture2D CreateBigTranmsparentTexture(int height = 1024 , int width = 1024)
    {
        Texture2D newTexture = new Texture2D(height, width);

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                newTexture.SetPixel(i, j, Color.clear);
            }
        }
        newTexture.Apply();
        return newTexture;
    }

    public static Texture2D CreateMaskImage(Texture2D bigTexture, Texture2D otherTexture)
    {
        for (int i = 0; i < otherTexture.height; i++)
        {
            for (int j = 0; j < otherTexture.width; j++)
            {
                bigTexture.SetPixel((bigTexture.width / 4 + i), (bigTexture.height / 4 + j), otherTexture.GetPixel(i, j));
            }
        }
        bigTexture.Apply();
        return bigTexture;
    }

    

	public static string WriteImageOnDisk(Texture2D texture, string fileName)
	{
		byte[] textureBytes = texture.EncodeToPNG();
		string path = GetBasePath() + fileName;
		File.WriteAllBytes(path, textureBytes);
		Debug.Log("File Written On Disk! " + path);
		return path;
	}

    public static Texture2D GetTextureFromFileName(string fileName)
    {
        string path = GetBasePath() + "/" + fileName;
        return GetTextureFromPath(path);
    }

    public static Texture2D GetTextureFromPath(string filePath)
    {
        var rawData = System.IO.File.ReadAllBytes(filePath);
        Texture2D tex = new Texture2D(2, 2); // Create an empty Texture; size doesn't matter (she said)
        tex.LoadImage(rawData);
        return tex;
    }

    public static Texture2D Resize(Texture2D texture, int width, int height)
    {
        return TextureScale.Bilinear(texture, width, height);
    }

    public static string GetImageName(int i)
    {
        return Time.time +  "_item_" + i + "_.png";
    }

    public static string GetBasePath()
    {
        return Application.dataPath + "/OpenAI Generated Assets/";// "Assets/OpenAiImages/";
    }

    //public static string Texture2DToBase64(Texture2D texture)
    //{
    //    byte[] imageData = texture.EncodeToPNG();
    //    return Convert.ToBase64String(imageData);
    //}

    //public static Texture2D Base64ToTexture2D(string encodedData)
    //{
    //    byte[] imageData = Convert.FromBase64String(encodedData);

    //    int width, height;
    //    GetImageSize(imageData, out width, out height);

    //    Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false, true);
    //    texture.hideFlags = HideFlags.HideAndDontSave;
    //    texture.filterMode = FilterMode.Point;
    //    texture.LoadImage(imageData);

    //    return texture;
    //}
    //private static void GetImageSize(byte[] imageData, out int width, out int height)
    //{
    //    width = ReadInt(imageData, 3 + 15);
    //    height = ReadInt(imageData, 3 + 15 + 2 + 2);
    //}
    //private static int ReadInt(byte[] imageData, int offset)
    //{
    //    return (imageData[offset] << 8) | imageData[offset + 1];
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Crosstales.FB;
using UnityEditor;
using System.IO;

public class Import360 : MonoBehaviour
{

   // public Texture2D importedSkybox;
    public Material modulableSkyboxMat;
  //  private string directory;

    //// Use this for initialization
    //void Start()
    //{
    //  //  directory = "";
    //   // modulableSkyboxMat.shader = Shader.Find("SkyboxPlus/Cubemap");
    //}

    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.A))
    //    {
    //        OnClickChangeEnvironment();
    //    }
    //}

    //private IEnumerator LoadImages()
    //{
    //    yield return null;
    //    //Download Link
    //   // directory = FileBrowser.OpenSingleFile("Select a 360 TEXTURE file", System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "jpg");
    // //   Debug.Log(directory);
    //    if (1==1)
    //    {
    //       // WWW www = new WWW(directory);

    //        //Wait for the download to complete
    //        //yield return www;
    //        //importedSkybox = www.texture;

    //        //Create path in the asset folders:
    //        string path = "Assets/Resources/SVLevels/av26.jpg";

    //        //Load Image to modify in the ressource folder
    //        Texture2D tex2d = Resources.Load<Texture2D>("SVLevels/av26");

    //        //Uptake byte data from downloaded www image
    //        // byte[] imData = importedSkybox.EncodeToJPG();
    //        Debug.Log(Application.persistentDataPath);
    //        Texture2D _tex = Utility.GetTextureFromPath(Application.persistentDataPath +"/" + "917_item_0_.png");
    //        byte[] imData = _tex.EncodeToJPG();
    //        File.WriteAllBytes(Application.dataPath + "/Resources/SVLevels/av26.jpg", imData);

    //        //Change the texture to cubemap:
    //        TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(path);
    //        if (tex2d != null && tex2d.dimension != UnityEngine.Rendering.TextureDimension.Cube)
    //        {
    //            importer.textureShape = TextureImporterShape.TextureCube;
    //            importer.SaveAndReimport();
    //        }



    //        // yield return new WaitForSecondsRealtime(1); //INCREASE OR DECREASE THIS TIMING; Depends on the ability of the computer to rapidly execute the previous tasks

    //        //Reference the Skybox material with the newly made cubemap texture ! IT WILL BE A CUBEMAP TO LOAD AGAIN!
    //        Cubemap finalSkybox = Resources.Load<Cubemap>("SVLevels/av26");

    //        modulableSkyboxMat.mainTexture = finalSkybox;
    //        RenderSettings.skybox = modulableSkyboxMat;
    //    }

    //}


    //public void OnClickChangeEnvironment()
    //{
    //    Cubemap cube2d = Resources.Load<Cubemap>("SVLevels/av26");

    //    string path = "Assets/Resources/SVLevels/av26.jpg";
    //    TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(path);
    //    if (cube2d != null && cube2d.dimension == UnityEngine.Rendering.TextureDimension.Cube)
    //    {
    //        importer.textureShape = TextureImporterShape.Texture2D;
    //        importer.SaveAndReimport();
    //    }

    //    StartCoroutine("LoadImages");
    //}

    public void SetSkyBoxMaterial(string texturePath)
    {
        Cubemap cube2d = Resources.Load<Cubemap>("SVLevels/av26");

        string path = "Assets/Resources/SVLevels/av26.jpg";
        TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(path);
        if (cube2d != null && cube2d.dimension == UnityEngine.Rendering.TextureDimension.Cube)
        {
            importer.textureShape = TextureImporterShape.Texture2D;
            importer.SaveAndReimport();
        }


        StartCoroutine(SetSkyBoxMaterialIEnum(texturePath));
    }

    public IEnumerator SetSkyBoxMaterialIEnum(string texturePath)
    {
        //Create path in the asset folders:
        string resourcePath = "Assets/Resources/SVLevels/av26.jpg";
        Texture2D tex2d = Resources.Load<Texture2D>("SVLevels/av26");

    
       // Debug.Log(Application.persistentDataPath);
        Texture2D _tex = Utility.GetTextureFromPath(texturePath);
        byte[] imData = _tex.EncodeToJPG();
        File.WriteAllBytes(Application.dataPath + "/Resources/SVLevels/av26.jpg", imData);

        //Change the texture to cubemap:
        TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(resourcePath);
        if (tex2d != null && tex2d.dimension != UnityEngine.Rendering.TextureDimension.Cube)
        {
            importer.textureShape = TextureImporterShape.TextureCube;
            importer.SaveAndReimport();
        }

        yield return new WaitForSecondsRealtime(1); //INCREASE OR DECREASE THIS TIMING; Depends on the ability of the computer to rapidly execute the previous tasks

        //Reference the Skybox material with the newly made cubemap texture ! IT WILL BE A CUBEMAP TO LOAD AGAIN!
        Cubemap finalSkybox = Resources.Load<Cubemap>("SVLevels/av26");

        modulableSkyboxMat.mainTexture = finalSkybox;
        RenderSettings.skybox = modulableSkyboxMat;
    }

}
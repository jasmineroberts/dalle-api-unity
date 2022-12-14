using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using REST_API_HANDLER;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;



public class InpaintingDemo : MonoBehaviour
{
    public GameObject loadingPanel;
    public TMP_InputField inputText;
    // public RawImage rawImage;
    public bool willChangeSkybox = true;
    public GameObject flatOutput;
    public Material flatOutputMat;

    public Import360 skyboxImporter;

   // public List<GameObject> previewObjs;

    private string IMAGE_GENERTION_API_URL = "https://api.openai.com/v1/images/generations";
    private string EDIT_IMAGE_API_URL = "https://api.openai.com/v1/images/edits";

    int outPaintNum = 0;
    float delayTime = 0.5f;


    public void GenerateBtnPressed()
    {
        loadingPanel.SetActive(true);


        //SetFlatOutput(null);

        //for (int i = 0; i < previewObjs.Count; i++)
        //{
        //    previewObjs[i].GetComponent<Renderer>().material.mainTexture = null;
        //}

        if (outPaintNum <= 0)
        {
            GenerateFirstImage();
        }
        else
        {
            GenerateOutPaintImage();
        }
    }

    public void ResetButtonClicked()
    {
        outPaintNum = 0;
        inputText.interactable = true;
    }

    private void GenerateFirstImage()
    {
        GenerateImage(inputText.text,Utility.resolution_512, (_pathList)=>
        { 
            //Debug.Log("Path List Count " + _pathList.Count);
            if (_pathList.Count > 0)
            {
                outPaintNum += 1;
                string path = _pathList[0];
                StartCoroutine(MakeMergedTexture(path,false));
                inputText.interactable = false;

                StartCoroutine(AfterLoadingAction(path));
            }
            loadingPanel.SetActive(false);
        });
    }

    private void GenerateOutPaintImage()
    {
        string maskImage = Utility.GetBasePath() + Utility.maskTextureName;
        EditImage(maskImage, inputText.text, Utility.resolution_1024, (_pathList) => {

            if (_pathList.Count > 0)
            {
                outPaintNum += 1;
                string path = _pathList[0];
               
                StartCoroutine(MakeMergedTexture(path, true));
                StartCoroutine(AfterLoadingAction(path));
                //skyboxImporter.SetSkyBoxMaterial(path);
                //StartCoroutine( MakeMergedTexture(path, true));
            }

            loadingPanel.SetActive(false);

        });
    }


    private IEnumerator AfterLoadingAction(string path)
    {
        Texture2D _originalTex = Utility.GetTextureFromPath(path);
        if (willChangeSkybox == false)
        {
            SetFlatOutput(_originalTex);
            yield return new WaitForSeconds(delayTime);
        }
        else
        {
            Texture2D _resizedTex = Utility.Resize(_originalTex, _originalTex.width, _originalTex.height / 2);
            yield return new WaitForSeconds(delayTime);
            string panaromaImgPath = Utility.WriteImageOnDisk(_resizedTex, "panaroma_" + Time.time + "_" + outPaintNum + "_" + ".png");
            yield return new WaitForSeconds(delayTime);
            skyboxImporter.SetSkyBoxMaterial(panaromaImgPath);
        }

      
    
      //  rawImage.texture = _originalTex;


        //for (int i = 0; i < previewObjs.Count; i++)
        //{
        //    previewObjs[i].GetComponent<Renderer>().material.mainTexture = _originalTex;
        //}

    }

    private void SetFlatOutput(Texture2D _tex)
    {
        if (flatOutput != null)
        {

            flatOutput.GetComponent<Renderer>().material.mainTexture = _tex;
        }
    }

    private IEnumerator MakeMergedTexture(string path, bool shouldDoResize)
    {
        //Texture2D _tex = Utility.GetTextureFromPath(path);
        //yield return new WaitForSeconds(0.5f);

        //Texture2D _oTex = Utility.Resize(_tex, _tex.width, _tex.height / 2);
        //yield return new WaitForSeconds(0.5f);
       
        //Utility.WriteImageOnDisk(_oTex, "ratio_" + outPaintNum + "_" + ".png");
        //yield return new WaitForSeconds(1);

        Debug.Log("U>> Out Num " + outPaintNum);
        if (shouldDoResize)
        {
            Debug.Log("U>> Start Resizing num " + outPaintNum);
            Texture2D _originalTex = Utility.GetTextureFromPath(path);
            yield return new WaitForSeconds(delayTime);
            Texture2D _resizedTex = Utility.Resize(_originalTex, _originalTex.width / 2, _originalTex.width / 2);
            yield return new WaitForSeconds(delayTime);
            path = Utility.WriteImageOnDisk(_resizedTex, "resiized_" + outPaintNum + "_" + ".png");
            yield return new WaitForSeconds(delayTime);
        }



        Texture2D _transparentTex = Utility.CreateBigTranmsparentTexture(1024, 1024);
        yield return new WaitForSeconds(delayTime);
        Texture2D _mergedTexture = Utility.CreateMaskImage(_transparentTex, Utility.GetTextureFromPath(path));
        yield return new WaitForSeconds(delayTime);
        string Url = Utility.WriteImageOnDisk(_mergedTexture, Utility.maskTextureName);
        yield return new WaitForSeconds(delayTime);

    }

    private void GenerateImage(string description, string resolution, Action<List<string>> completationAction)
    {

        GenerateImageRequestModel reqModel = new GenerateImageRequestModel(description, 1 , resolution);
        ApiCall.instance.PostRequest<GenerateImageResponseModel>(IMAGE_GENERTION_API_URL, reqModel.ToCustomHeader(), null, reqModel.ToBody(), (result =>
        {
            loadTexture(result.data, completationAction);
        }), (error =>
        {
            ErrorResponseModel entity = JsonUtility.FromJson<ErrorResponseModel>(error);
            completationAction.Invoke(new List<string>());
        }));

    }


    public void EditImage(string imagePath,string description, string resolution, Action<List<string>> completationAction)
    {

        EditImageRequestModel reqModel = new EditImageRequestModel(imagePath,description, 1, resolution);
        ApiCall.instance.PostRequest<GenerateImageResponseModel>(EDIT_IMAGE_API_URL, reqModel.ToCustomHeader(), reqModel.ToFormData(), null , (result =>
        {
            loadTexture(result.data, completationAction);
        }), (error =>
        {
            ErrorResponseModel entity = JsonUtility.FromJson<ErrorResponseModel>(error);
            completationAction.Invoke(new List<string>());
           // resultText.enabled = true;
           // resultText.text = entity.error.message;
        }));

    }



    async void loadTexture(List<UrlClass> urls, Action<List<string>> completationAction)
    {
        List<string> filePathList = new List<string>();
        for (int i = 0; i < urls.Count; i++)
        {
            Texture2D _texture = await GetRemoteTexture(urls[i].url);
            string savePath = Utility.WriteImageOnDisk(_texture,Utility.GetImageName(outPaintNum));
            filePathList.Add(savePath);
        }
        completationAction.Invoke(filePathList);
    }

    public static async Task<Texture2D> GetRemoteTexture(string url)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            var asyncOp = www.SendWebRequest();

            while (asyncOp.isDone == false)
                await Task.Delay(1000 / 30);//30 hertz

            // read results:
            if (www.isNetworkError || www.isHttpError)
            {
                return null;
            }
            else
            {
                return DownloadHandlerTexture.GetContent(www);
            }
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using REST_API_HANDLER;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System;
using System.IO;

public class OpenAiImageExample : MonoBehaviour
{
	public GameObject loadingpanel;
	public TMP_InputField inputText;
	public TMP_Text resultText;
	public List<GameObject> previewObjs;

	private string IMAGE_GENERTION_API_URL = "https://api.openai.com/v1/images/generations";

	public void SearchButtonClicked()
    {
		resultText.text = "";
		resultText.enabled = false;
		loadingpanel.SetActive(true);

		for (int i = 0; i < previewObjs.Count; i++)
		{
			previewObjs[i].GetComponent<Renderer>().material.mainTexture = null;
		}

		string description = inputText.text;
		string resolution = "256x256"; // Possible Resolution 256x256, 512x512, or 1024x1024.

		GenerateImage(description, resolution, () => {
			loadingpanel.SetActive(false);
		});
		
	}

	public void GenerateImage(string description, string resolution, Action completationAction)
	{

		GenerateImageRequestModel reqModel = new GenerateImageRequestModel(description, 3 ,resolution);
		ApiCall.instance.PostRequest<GenerateImageResponseModel>(IMAGE_GENERTION_API_URL, reqModel.ToCustomHeader(), null, reqModel.ToBody(), (result =>
		{
			loadTexture(result.data, completationAction);
			resultText.enabled = true;
		}), (error =>
		{
			ErrorResponseModel entity = JsonUtility.FromJson<ErrorResponseModel>(error);
			completationAction.Invoke();
			resultText.enabled = true;
			resultText.text = entity.error.message;
		}));

	}




	async void loadTexture(List<UrlClass> urls, Action completationAction)
    {
		for (int i = 0; i < urls.Count; i++)
        {
			Texture2D _texture = await GetRemoteTexture(urls[i].url);
			previewObjs[i].GetComponent<Renderer>().material.mainTexture = _texture;
			Utility.WriteImageOnDisk(_texture, System.DateTime.Now.Millisecond + "_createImg_" + i + "_.jpg");
		}

		completationAction.Invoke();
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

	private void WriteImageOnDisk(Texture2D texture, string fileName)
	{
		byte[] textureBytes = texture.EncodeToPNG();
		string path = Application.persistentDataPath + fileName;
		File.WriteAllBytes(path, textureBytes);
		Debug.Log("File Written On Disk! "  + path );
	}
}

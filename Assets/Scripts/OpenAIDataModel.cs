using System;
using System.Collections;
using System.Collections.Generic;
using REST_API_HANDLER;
using UnityEngine;

public class OpenAIDataModel 
{
    
}

[Serializable]
public class GenerateImageRequestModel
{
    public string prompt;
    public int n;
    public string size;

    string API_KEY = Utility.API_KEY;//"sk-xxxxxx";
    string ORGANISATION_KEY = Utility.ORGANIZATION_KEY;//"org-xxxxxx";

    public GenerateImageRequestModel(string _prompt, int _n, string _size)
    {
        prompt = _prompt;
        n = _n;
        size = _size;
    }

    public string ToBody()
    {
        var jsonString = JsonUtility.ToJson(this);
        Debug.Log("STR >> " + jsonString);
        return jsonString;
    }

    //public WWWForm ToFormData()
    //{
    //    WWWForm formData = new WWWForm();
    //    formData.AddField("title", title);
    //    return formData;
    //}

    public Dictionary<string, string> ToCustomHeader()
    {

        string auth = "Bearer " + API_KEY;
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-type", "application/json; charset=UTF-8");
        headers.Add("Authorization", auth);
        headers.Add("OpenAI-Organization", ORGANISATION_KEY);

        return headers;
       // return ApiConfig.GetHeaders();
    }
}

[Serializable]
public class GenerateImageResponseModel
{
    public int created;
    public List<UrlClass> data;
}

[Serializable]
public class UrlClass
{
    public string url;
}

[Serializable]
public class ErrorResponseModel
{
    public ErrorClass error;
}

[Serializable]
public class ErrorClass
{
    public string message;
    public string type;
}

[Serializable]
public class EditImageRequestModel
{
    public string image;
    public string mask;
    public string prompt;
    public int n;
    public string size;
    public string response_format;

    string API_KEY = Utility.API_KEY; //"sk-xxxxxx";
    string ORGANISATION_KEY = Utility.ORGANIZATION_KEY;//"org-xxxxx";

    public EditImageRequestModel(string _image, string _prompt, int _n, string _size)
    {
        image = _image;
        prompt = _prompt;
        n = _n;
        size = _size;
        response_format = "url";
    }

    public string ToBody()
    {
        var jsonString = JsonUtility.ToJson(this);
        Debug.Log("STR >> " + jsonString);
        return jsonString;
    }

    public WWWForm ToFormData()
    {
         ToBody();

        byte[] fileBytes = System.IO.File.ReadAllBytes(image);

        Debug.Log("Bytes " + fileBytes.Length);

        WWWForm formData = new WWWForm();
        formData.AddBinaryData("image", fileBytes);
        //formData.AddField("image", image);
        formData.AddField("prompt", prompt);
        formData.AddField("n", n);
        formData.AddField("size", size);
        formData.AddField("response_format", response_format);
        return formData;
    }

    public Dictionary<string, string> ToCustomHeader()
    {

        string auth = "Bearer " + API_KEY;
        Dictionary<string, string> headers = new Dictionary<string, string>();
       // headers.Add("Content-type", "multipart/form-data");
        headers.Add("Authorization", auth);
        headers.Add("OpenAI-Organization", ORGANISATION_KEY);

        return headers;
        // return ApiConfig.GetHeaders();
    }
}

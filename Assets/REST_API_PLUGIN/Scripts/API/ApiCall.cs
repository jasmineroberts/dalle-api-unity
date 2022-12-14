using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace REST_API_HANDLER {


    public class ApiCall : MonoBehaviour
    {
        public static ApiCall instance;
        private HttpClient client = new HttpClient();

        public static string CAN_NOT_DECODE_JSON = "CAN_NOT_DECODE_JSON";

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }



        public void GetRequest<RESULT>(string url, Dictionary<string, string> headers, Action<RESULT> success, Action<string> error, string jsonPrefix = null)
        {
            client.GetAsync(url, jsonPrefix, headers, (result) =>
            {
                if (result.success)
                {
                    try
                    {
                        RESULT entity = JsonUtility.FromJson<RESULT>(result.resultText);
                        success.Invoke(entity);
                    }
                    catch (Exception e)
                    {
                        error.Invoke(e.Message);
                    }
                }
                else
                {
                    error.Invoke(result.error);
                }
            });
        }


        public void PostRequest<RESULT>(string url, Dictionary<string, string> headers, WWWForm form, string jsonParams, Action<RESULT> success, Action<string> error, string jsonPrefix = null)
        {
            client.PostAsync(url, jsonPrefix, jsonParams, headers, form, (result) =>

            {
                if (result.success)
                {
                    try
                    {
                        RESULT entity = JsonUtility.FromJson<RESULT>(result.resultText);
                        success.Invoke(entity);
                    }
                    catch (Exception e)
                    {
                        error.Invoke(e.Message);
                    }
                }
                else
                {
                    error.Invoke(result.error);
                }
            });
        }

        public void PutRequest<RESULT>(string url, Dictionary<string, string> headers, WWWForm form, string jsonParams, Action<RESULT> success, Action<string> error, string jsonPrefix = null)
        {

            client.PutAsync(url, jsonPrefix, jsonParams, headers, form, (result) =>
            {
                if (result.success)
                {
                    try
                    {
                        RESULT entity = JsonUtility.FromJson<RESULT>(result.resultText);
                        success.Invoke(entity);
                    }
                    catch (Exception e)
                    {
                        error.Invoke(e.Message);
                    }
                }
                else
                {
                    error.Invoke(result.error);
                }
            });
        }

        public void DeleteRequest<RESULT>(string url, Dictionary<string, string> headers, Action<RESULT> success, Action<string> error, string jsonPrefix = null)
        {

            client.DeleteAsync(url, jsonPrefix, headers, (result) =>
            {
                if (result.success)
                {
                    try
                    {
                        RESULT entity = JsonUtility.FromJson<RESULT>(result.resultText);
                        success.Invoke(entity);
                    }
                    catch (Exception e)
                    {
                        error.Invoke(CAN_NOT_DECODE_JSON);
                    }
                }
                else
                {
                    error.Invoke(result.error);
                }
            });
        }
    }

}



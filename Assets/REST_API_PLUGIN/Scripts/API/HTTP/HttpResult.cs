using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace REST_API_HANDLER
{
    public class HttpResult
    {
        public readonly Dictionary<string, string> headers = null;
        public readonly string url = default;
        public readonly string resultText = null;
        public readonly string error = default;
        public readonly bool success = default;

        public HttpResult(string rror)
        {
            success = false;
            error = rror;
        }

        public HttpResult(Dictionary<string, string> headers, string url, string text)
        {
            this.headers = headers;
            this.url = url;
            success = true;
            this.resultText = text;
        }
    }
}


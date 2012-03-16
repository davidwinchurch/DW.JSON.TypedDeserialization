using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;

namespace Rest
{
    public class RestHelper
    {
        public static string GetResponse(Uri url, string data, string method, string contentType, NameValueCollection headers = null, int timeout = 60000, bool gzip = true, NetworkCredential credential = null)
        {
            var request = CreateRequest(url, data, method, contentType, headers, timeout, gzip, credential);

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                //Get the response stream
                var responseStream = response.GetResponseStream();
                //Check for a compressed response stream and instantiate correct decompressor
                if (response.ContentEncoding.ToLower().Contains("gzip"))
                    responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                else if (response.ContentEncoding.ToLower().Contains("deflate"))
                    responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);

                //Create response stream reader
                var responseStreamReader = new StreamReader(responseStream, Encoding.Default);

                //Read full response and return
                return responseStreamReader.ReadToEnd();
            }
        }

        public static string GetResponse(HttpWebResponse response)
        {
            using (var reader = response.GetResponseStream())
            {
                if (reader != null)
                {
                    var responseStreamReader = new StreamReader(reader, Encoding.Default);
                    return responseStreamReader.ReadToEnd();
                }
            }
            return null;
        }

        private static HttpWebRequest CreateRequest(Uri url, string data, string method, string contentType, NameValueCollection headers, int timeout, bool gzip, NetworkCredential credential = null)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = method.ToUpper();
            request.ContentType = contentType;
            var noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            request.CachePolicy = noCachePolicy;
            request.Timeout = timeout;
            request.Credentials = credential;

            if (headers != null)
                request.Headers.Add(headers);

            if (gzip)
                request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");

            if (data != null)
            {
                var encoding = new UTF8Encoding();
                var byteData = encoding.GetBytes(data);
                request.ContentLength = byteData.Length;

                using (var postStream = request.GetRequestStream())
                {
                    postStream.Write(byteData, 0, byteData.Length);
                }
            }
            else if (request.Method != "GET")
            {
                //Specify a zero content length to prevent 411s
                request.ContentLength = 0;
            }

            return request;
        }

        public static string GetJsonResponse(Uri uri, string data, string method, string contentType = "application/json", NameValueCollection headers = null, int timeout = 60000, bool gzip = true)
        {
            return GetResponse(uri, data, method, contentType, headers, timeout, gzip);
        }
    }
}

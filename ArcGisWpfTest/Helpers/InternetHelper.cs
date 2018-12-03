using System;
using System.Net;

namespace ArcGisWpfTest.Helpers
{
    public class InternetHelper
    {
        /// <summary>
        /// Почему то этот метод не всегда работает
        /// </summary>
        /// <returns></returns>
        public static bool CheckForInternetConnection()
        {
            try
            {
                var request = (HttpWebRequest) WebRequest.Create("http://mail.ru");
                request.Timeout = 5000;
                request.Credentials = CredentialCache.DefaultNetworkCredentials;

                var response = (HttpWebResponse) request.GetResponse();

                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Этот метод хорошо работает
        /// </summary>
        /// <returns></returns>
        public static bool CheckForInternetConnection2()
        {
            try
            {
                using (var client = new MyWebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private class MyWebClient : WebClient
        {
            protected override WebRequest GetWebRequest(Uri uri)
            {
                WebRequest w = base.GetWebRequest(uri);
                w.Timeout = 1500;
                return w;
            }
        }
    }
}
using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace ArcGisWpfTest.Helpers
{
    public class InternetHelper
    {
        public static bool CheckForInternetConnectionHttp()
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://www.google.com");
                req.Timeout = 1500;
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool CheckForInternetConnection()
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
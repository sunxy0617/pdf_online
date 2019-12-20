using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace showPdf.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ViewResult Index(string url)
        {
            byte[] data = downloadData(url);
            ViewBag.data = data;
            return View();
        }

        private byte[] downloadData(string url)
        {
            if (string.IsNullOrWhiteSpace(url)||!url.Contains("://"))
            {
                return null;
            }
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream stream = response.GetResponseStream();

                int index = 0;
                int dataLength = (int)response.ContentLength;


                int tempLength = 65536;//16k
                byte[] temp = new byte[tempLength];
                byte[] data = new byte[dataLength];
                int streamReadLength;

                do
                {
                    streamReadLength = stream.Read(temp, 0, tempLength);
                    for (int i = 0; i < streamReadLength; i++)
                    {
                        data[index++] = temp[i];
                    }

                }
                while (streamReadLength > 0);
                temp = null;
                stream.Close();
                response.Close();

                return data;
            }
            catch
            {
                return null;
            }
        }
    }
}
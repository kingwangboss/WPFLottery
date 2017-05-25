//文件名称（File Name）                 HttpTool.cs
//作者(Author)                          yjq
//日期（Create Date）                   2017.5.1
//修改记录(Revision History)
//    R1:
//        修改作者:
//        修改日期:
//        修改原因:
//    R2:
//        修改作者:
//        修改日期:
//        修改原因:
//**************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace Network
{
    /// <summary>
    /// 网络请求工具类
    /// </summary>
    public class HttpTool
    {
        IHttpCallback httpCallback = null;


        /// <summary>
        /// 发送post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="dic">请求的参数</param>
        /// <param name="reqEncode">请求的编码</param>
        /// <param name="resEncode">响应的编码</param>
        /// <returns></returns>
        public static string PostRequest(string url, IDictionary<string, string> dic, string reqEncode, string resEncode)
        {
            StringBuilder strb = new StringBuilder();
            foreach (string key in dic.Keys)
            {
                strb.AppendFormat("{0}={1}&", key, dic[key]);
            }
            String queryString = strb.ToString();
            queryString = queryString.EndsWith("&") ? queryString.Remove(queryString.LastIndexOf('&')) : queryString;
            byte[] data = Encoding.GetEncoding(reqEncode.ToUpper()).GetBytes(queryString); ;

            WebClient webClient = new WebClient();
            try
            {
                //采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
                webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                //得到返回字符流  
                byte[] responseData = webClient.UploadData(url, "POST", data);
                //解码  
                String responseString = Encoding.GetEncoding(resEncode.ToUpper()).GetString(responseData);
                return responseString;
            }
            catch (WebException ex)
            {
                Stream stream = ex.Response.GetResponseStream();
                string m = ex.Response.Headers.ToString();
                byte[] buf = new byte[256];
                stream.Read(buf, 0, 256);
                stream.Close();
                int count = 0;
                foreach (Byte b in buf)
                {
                    if (b > 0)
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }
                return ex.Message + "\r\n\r\n" + Encoding.GetEncoding(resEncode.ToUpper()).GetString(buf, 0, count);
            }
        }

        /// <summary>
        /// 发送Get请求
        /// </summary>
        /// <param name="requestUrl">请求地址</param>
        /// <param name="reqEncode">请求编码</param>
        /// <param name="resEncode">响应编码</param>
        /// <returns></returns>
        public static string GetRequest(string requestUrl, string reqEncode, string resEncode)
        {
            String url = requestUrl.Substring(0, requestUrl.LastIndexOf('?'));
            String queryString = requestUrl.Substring(requestUrl.LastIndexOf('?') + 1); ;
            byte[] data = Encoding.GetEncoding(reqEncode.ToUpper()).GetBytes(queryString); ;
            WebClient webClient = new WebClient();
            try
            {
                //得到返回字符流  
                byte[] responseData = webClient.UploadData(url, "GET", data);
                //解码  
                String responseString = Encoding.GetEncoding(resEncode.ToUpper()).GetString(responseData);
                return responseString;
            }
            catch (WebException ex)
            {
                Stream stream = ex.Response.GetResponseStream();
                string m = ex.Response.Headers.ToString();
                byte[] buf = new byte[256];
                stream.Read(buf, 0, 256);
                stream.Close();
                int count = 0;
                foreach (Byte b in buf)
                {
                    if (b > 0)
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }
                return ex.Message + "\r\n\r\n" + Encoding.GetEncoding(resEncode.ToUpper()).GetString(buf, 0, count);
            }
        }

        #region WebRequestd的异步处理
        public void AsyncGetRequestByWebClient(string url,IHttpCallback httpCallback)
        {
            this.httpCallback = httpCallback;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.BeginGetResponse(new AsyncCallback(ReadCallback), request);
        }
        private void ReadCallback(IAsyncResult asynchronousResult)
        {
            var request = (HttpWebRequest)asynchronousResult.AsyncState;
            var response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
            {
                var resultString = streamReader.ReadToEnd();
            }
            httpCallback.run();
        }


        #endregion

        #region WebClient的异步处理
        public static void AsyncGetWithWebClient(string url)
        {
            var webClient = new WebClient();

            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            webClient.DownloadStringAsync(new Uri(url));
        }

        private static void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            //Console.WriteLine(e.Cancelled);
            Console.WriteLine(e.Error != null ? "WebClient异步GET发生错误！" : e.Result);
        }

        #endregion


        #region WebClient的OpenReadAsync测试

        public static void TestGetWebResponseAsync(string url)
        {
            var webClient = new WebClient();
            webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(webClient_OpenReadCompleted);
            webClient.OpenReadAsync(new Uri(url));
        }

        private static void webClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                var streamReader = new StreamReader(e.Result);
                var result = streamReader.ReadToEnd();
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine("执行WebClient的OpenReadAsync出错：" + e.Error);
            }
        }

        #endregion
    }



}

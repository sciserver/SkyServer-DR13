using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SkyServer.Tools.Search
{
    public class ResponseREST
    {
       
        public static void writeOutput(String result, string fmt, HttpResponse response)
        {
            if (response.IsClientConnected)
            {
                switch (fmt.ToLower())
                {
                    case ("csv"):
                        writeCSV(result, response);
                        break;
                    //case ("xml"):
                    //    writeXML(result, response);
                    //    break;
                    case ("json"):
                        writeJSON(result, response);
                        break;
                    //case ("votable"):
                    //    writeVOTable(result, response);
                    //    break;
                    //case ("fits"):
                    //    writeFITS(result, response);
                    //    break;
                    default:
                        break;
                }
            }
        }

        public static void writeCSV(String result, HttpResponse response)
        {
            try
            {
                response.ContentType = "text/plain";
            }
            catch { }
            response.Write(result);
        }

        public static void writeJSON(String result, HttpResponse response)
        {
            try
            {
                response.ContentType = "text/plain";
            }
            catch { }
            response.Write(result);
        }

        /// <summary>
        /// This is to run query using new RESTful services
        /// </summary>
        public static void runQuery(String serviceUrl,String requestString, HttpResponse resp)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(serviceUrl);
            
            string requestUri = client.BaseAddress + "?" + requestString;
            string queryResult = "";
            HttpResponseMessage response = client.GetAsync(requestUri).Result;
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
                queryResult = response.Content.ReadAsStringAsync().Result;
            else
                throw new ApplicationException("Query did not return results successfully, check input and try again later.");

            writeOutput(queryResult, "json", resp);
        }


        public static void postQuery(String serviceUrl,String requestString, HttpResponse resp, String token, string uploaded)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(serviceUrl);

            StringContent content = new StringContent(uploaded);
            if (!(token == null || token == String.Empty))
            {
                content.Headers.Add("X-Auth-Token", token);
            }
            //content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            
            string url = serviceUrl+"?"+ requestString;
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            String queryResult = "";
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
                //return response;
                queryResult = response.Content.ReadAsStringAsync().Result;
            else
                throw new ApplicationException("Query did not return results successfully, check input and try again later.");
            writeOutput(queryResult, "json", resp);
        }

    }
}
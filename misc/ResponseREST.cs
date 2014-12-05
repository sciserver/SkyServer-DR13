using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Specialized;
using System.IO;

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
      private void runQuery(String serviceUrl, String requestString, HttpResponse resp, string uploaded)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(serviceUrl);
            
            string requestUri = client.BaseAddress + "?" + requestString;
            string queryResult = "";
            HttpResponseMessage response = null;

            if (uploaded == null || uploaded.Equals(""))
            {
                response = client.GetAsync(requestUri).Result;
            }
            else {
                
                string token = "";
                /// Once the authenticated skyserver is ready, we ca update the code to retrieve token
                var appRequest = HttpContext.Current.Request;
                HttpCookie cookie = appRequest.Cookies["Keystone"];
                if (cookie != null)
                    if (cookie["token"] != null || !cookie["token"].Equals(""))
                        token = cookie["token"];

                StringContent content = new StringContent(uploaded);
                if (!(token == null || token == String.Empty))
                {
                    content.Headers.Add("X-Auth-Token", token);
                }
                response = client.PostAsync(requestUri, content).Result;
            }

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
            //response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
                //return response;
                queryResult = response.Content.ReadAsStringAsync().Result;
            else
                throw new ApplicationException("Query did not return results successfully, check input and try again later.");
            writeOutput(queryResult, "json", resp);
        }


        /// <summary>
        /// This method is used to pass all the requests and run rest web service
        /// </summary>
        public void ProcessRequestREST(HttpRequest Request, HttpResponse Response)
        {
            NameValueCollection inputForm = Request.Form;
            if(inputForm.Count==0)
             inputForm = Request.QueryString;
            String requestString = "";
            foreach (string key in inputForm.Keys)
            {
                requestString += key + "=" + inputForm[key] + "&";
            }

            String searchTool = inputForm["searchtool"];

            string requestUrl = "";
            bool temp = false;
            string radecText = "";
            Globals globals = new Globals();

            switch(searchTool){
                case "SQL": requestUrl = globals.SQLSearchWS; break;
                case "Radial": requestUrl = globals.RadialSearchWS; break;
                case "Rectangular": requestUrl = globals.RectangularSearchWS; break;
                case "Imaging":
                    switch (inputForm["positionType"])
                    {
                        case "cone": requestUrl = globals.ConeImaging; break;
                        case "rectangular": requestUrl = globals.RectangularImaging; break;
                        case "none": requestUrl = globals.NoPositionImaging; break;
                        case "proximity": temp = true; requestUrl = globals.ProximityImaging; break;
                        default: throw new Exception("No proper positionType selcted."); break;
                    }
                    break;
                case "Spectro":
                    switch (inputForm["positionType"])
                    {
                        case "cone": requestUrl = globals.ConeSpectroWS; break;
                        case "rectangular": requestUrl = globals.RectangularSpectroWS; break;
                        case "none": requestUrl = globals.NoPositionSpectroWS; break;
                        case "proximity": temp = true; requestUrl = globals.ProximitySpectroWS; break;
                        default: throw new Exception("No proper positionType selcted."); break;
                    }
                    break;
                case "IRSpectra":
                    switch (inputForm["positionType"])
                    {
                        case "cone": requestUrl = globals.ConeIRWS; break;
                        case "conelb": requestUrl = globals.GalacticIRWS; break;
                        case "none": requestUrl = globals.NoPositionIRWS; break;
                        //case "proximity": temp = true; requestUrl = globals.ProximitySpectroWS; break;
                        default: throw new Exception("No proper positionType selcted."); break;
                    }
                    break;
                default: throw new Exception("Select proper tool");

            }
            
            if (temp)
            {
                radecText = inputForm["radecTextArea"];
                if (radecText == null || radecText.Equals(""))
                {
                    if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                    {
                        radecText = (new StreamReader(Request.Files[0].InputStream)).ReadToEnd();
                    }
                }                
            }
            
            runQuery(requestUrl, requestString, Response,radecText);
        }


        public void ProcessRequest() { 

        }
    }
}
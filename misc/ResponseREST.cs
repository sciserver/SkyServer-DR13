using System.Collections.Specialized;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using net.ivoa.VOTable;
using System.Net.Http;
using System.Net.Http.Headers;

using System.Threading.Tasks;
using System.Text;
using System.IO;

using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;

namespace SkyServer.Tools.Search
{
    public class ResponseREST
    {

        HttpResponse httpResponse;
        HttpCookie cookie;
        string token = "";
        public void ProcessRequest()
        {

        }
        /// <summary>
        /// This method is used to pass all the requests and run rest web service
        /// </summary>
        public void ProcessRequestREST(HttpRequest Request, HttpResponse Response)
        {
            httpResponse = Response;
            
            cookie = Request.Cookies["Keystone"];
            if (cookie != null)
                if (cookie["token"] != null || !cookie["token"].Equals(""))
                    token = cookie["token"];

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

            runQuery(requestUrl, requestString, radecText, inputForm["format"]);
        }
        

        private void runQuery(String serviceUrl, String requestString, string uploaded, string returnType)
        {
            /// Once the authenticated skyserver is ready, we can update the code to retrieve token          
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(serviceUrl);                        
            string requestUri = client.BaseAddress + "?" + requestString;

            string queryResult = "";
            HttpResponseMessage respMessage = null;
            StringContent content = null;
            if (uploaded == null || uploaded.Equals(""))
                content = new StringContent("");
            else 
                content = new StringContent(uploaded);
             
            if (!(token == null || token == String.Empty))
            {
                content.Headers.Add("X-Auth-Token", token);
                respMessage = client.PostAsync(requestUri, content).Result;
            }else{
                respMessage = client.GetAsync(requestUri).Result;
            }

            respMessage.EnsureSuccessStatusCode();
            if (respMessage.IsSuccessStatusCode)
                queryResult = respMessage.Content.ReadAsStringAsync().Result;
                
            else
                throw new ApplicationException("Query did not return results successfully, check input and try again later.");

           

            setContentType(returnType);
            if (returnType.ToLower().Equals("html"))
                httpResponse.Write(JsonToHtml(queryResult));
            else
                httpResponse.Write(queryResult);
            
        }

        public void setContentType(string format) { 
            format  = format.ToLower();
            switch(format){
                case "csv"    :  httpResponse.ContentType ="text/plain"; break;
                case "xml"    :  httpResponse.ContentType = "application/xml"; break;
                case "votable":  httpResponse.ContentType = "application/xml"; break;
                case "json"   :  httpResponse.ContentType = "application/json"; break;
                case "fits"   :  httpResponse.ContentType = "application/octet-stream";
                                 httpResponse.AddHeader("Content-Disposition", "attachment;filename=\"result.fits\"");
                                 break;
                case "html"   :  httpResponse.ContentType = ""; break;
                default       :  httpResponse.ContentType = "text/plain"; break;
            }

            //return format;
        }

        public string JsonToHtml(string json) {

            int cnt = 0;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<html><head>\n");
            sb.AppendFormat("<title>SDSS Query Results</title>\n");
            sb.AppendFormat("</head><body bgcolor=white>\n");
            //JObject o = JObject.Parse(json);
            JArray jarr = JArray.Parse(json);
            sb.AppendFormat("<table border='1'>");
            foreach (JObject o in jarr.Children<JObject>())
            {
                var temp = o["Rows"];
                JArray arr = (JArray)o["Rows"];
                foreach (JObject jb in arr.Children<JObject>())
                {
                    IList<string> keys = jb.Properties().Select(p => p.Name).ToList();
                    if (cnt == 0)
                    {
                        sb.AppendFormat("<tr align=center>");
                        for (int i = 0; i < keys.Count; i++)
                            sb.AppendFormat("<td><font size=-1>{0}</font></td>", keys[i]);
                        sb.AppendFormat("</tr>");
                    }
                    sb.AppendFormat("<tr align=center>");
                    for (int i = 0; i < keys.Count; i++)
                        sb.AppendFormat("<td><font size=-1>{0}</font></td>", jb[keys[i]]);
                    sb.AppendFormat("</tr>");
                    cnt++;
                }
               
            }
            sb.AppendFormat("</table></body></html>");
            return sb.ToString();
        } 

    }
}
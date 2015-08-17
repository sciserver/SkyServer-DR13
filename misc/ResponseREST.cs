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
using System.Text.RegularExpressions;
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
        string requestUrl = "";
        String requestString = "";
        public void ProcessRequest()
        {

        }
        public ResponseREST() { }
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

            foreach (string key in inputForm.Keys)
            {
                    requestString += key + "=" + Uri.EscapeDataString(inputForm[key]) + "&";
            }


            String searchTool = inputForm["searchtool"];

            
            bool temp = false;
            string radecText = "";
            Globals globals = new Globals();

            switch(searchTool){
                case "SearchForm": requestUrl = globals.SQLSearchWS; break;
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
                        default: throw new Exception("No proper positionType selected."); break;
                    }
                    break;
                case "Spectro":
                    switch (inputForm["positionType"])
                    {
                        case "cone": requestUrl = globals.ConeSpectroWS; break;
                        case "rectangular": requestUrl = globals.RectangularSpectroWS; break;
                        case "none": requestUrl = globals.NoPositionSpectroWS; break;
                        case "proximity": temp = true; requestUrl = globals.ProximitySpectroWS; break;
                        default: throw new Exception("No proper positionType selected."); break;
                    }
                    break;
                case "IRSpectra":
                    switch (inputForm["positionType"])
                    {
                        case "cone": requestUrl = globals.ConeIRWS; break;
                        case "conelb": requestUrl = globals.GalacticIRWS; break;
                        case "none": requestUrl = globals.NoPositionIRWS; break;
                        //case "proximity": temp = true; requestUrl = globals.ProximitySpectroWS; break;
                        default: throw new Exception("No proper positionType selected."); break;
                    }
                    break;
                default: throw new Exception("Select proper tool");

            }
            
            if (temp)
            {
                /*
                radecText = inputForm["radecTextArea"];
                if (radecText == null || radecText.Equals(""))
                {
                    if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                    {
                        radecText = (new StreamReader(Request.Files[0].InputStream)).ReadToEnd();
                    }
                } 
                 */
                if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                {
                    radecText = (new StreamReader(Request.Files[0].InputStream)).ReadToEnd();
                }
                else
                {
                    try
                    {
                        radecText = inputForm["radecTextArea"];
                        if(radecText.Length == 0)
                            throw new ApplicationException("ERROR: No (RA,DEC) list specified for Proximity search.");
                    }
                    catch
                    {
                        throw new ApplicationException("ERROR: Neither upload file nor list specified for Proximity search.");
                    }

                }

            }

            runQuery(requestUrl, requestString, radecText, inputForm["format"]);
        }
        

        private void runQuery(String serviceUrl, String requestString, string uploaded, string returnType)
        {
            Globals globals = new Globals();
            /// Once the authenticated skyserver is ready, we can update the code to retrieve token          
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(serviceUrl);
            client.Timeout = new TimeSpan(0, 0, 0, globals.TimeoutSkyserverWS);// default is 100000ms
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
                //respMessage = client.GetAsync(requestUri).Result;
                respMessage = client.PostAsync(requestUri, content).Result;
            }

            //respMessage.EnsureSuccessStatusCode();
            if (respMessage.IsSuccessStatusCode)
                queryResult = respMessage.Content.ReadAsStringAsync().Result;
            else
            {
                string ErrorMessage = respMessage.Content.ReadAsStringAsync().Result;
                queryResult = getErrorMessageHTMLresult(ErrorMessage);
                returnType = "html";
                //throw new ApplicationException("Query did not return results successfully, check input and try again later.");
            }

            setContentType(returnType);
            httpResponse.Write(queryResult);
            //if (returnType.ToLower().Equals("html"))
            //    //httpResponse.Write(JsonToHtml(queryResult));
            //    httpResponse.Write(queryResult);
            //else
            //    httpResponse.Write(queryResult);
            
        }

        public void setContentType(string format) { 
            format  = format.ToLower();
            switch(format){
                case "csv"    :  httpResponse.ContentType = "text/plain"; break;
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


        public string getErrorMessageHTMLresult(string ErrorMessage)
        {
            string message = "";
            string message2 = "";
            string[] Expressions = new string[] { "\"Error Message\":\"(.*?)\"", "\"Error Message\": \"(.*?)\"", "\"Error Message\" :\"(.*?)\"", "\"Error Message\" : \"(.*?)\"", "\"Message\":\"(.*?)\"", "\"Message\" :\"(.*?)\"" , "\"Message\": \"(.*?)\"", "\"Message\" : \"(.*?)\""};
            foreach (string expresion in Expressions)
            {
                Regex regex = new Regex(expresion);
                var v = regex.Match(ErrorMessage);
                message = v.Groups[1].ToString();
                if (message != "")
                    break;
            }
            if (message == "")
            {
                message = "Query did not return results successfully, check input and try again later.";

                string errmessage = ErrorMessage.ToLower();
                if (errmessage.Contains("<html>"))
                {
                    message2 = errmessage;
                    message += "<br>MORE INFO FROM RESPONSE:<br><br>";
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<html><head>\n");
            sb.AppendFormat("<title>Skyserver Error</title>\n");
            sb.AppendFormat("</head><body bgcolor=white>\n");
            sb.AppendFormat("<h2>An error occured</h2>");
            sb.AppendFormat("<H3 BGCOLOR=pink><font color=red><br>" + message + "</font></H3>");
            sb.AppendFormat("</BODY></HTML>\n");
            sb.AppendFormat(message2);
            return sb.ToString();
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


        //***************************************************************************************************************************************************************
        // this code can be redesigned and thought after April 15 2015


        public DataSet RunCasjobs(string command, string taskname)
        {
            // throw new IndexOutOfRangeException("There is an invalid argument");

            try
            {

                var request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(ConfigurationManager.AppSettings["casjobsRESTapi"]);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/x-dataset";

                if (!token.Equals("") && token != null)
                    request.Headers.Add("X-Auth-Token", token);

                StreamWriter streamWriter = new StreamWriter(request.GetRequestStream());
                StringWriter sw = new StringWriter();
                JsonWriter jsonWriter = new JsonTextWriter(sw);
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName("Query");
                jsonWriter.WriteValue(command);
                jsonWriter.WritePropertyName("TaskName");
                jsonWriter.WriteValue(taskname);
                //jsonWriter.WritePropertyName("ReturnDataSet");
                //jsonWriter.WriteValue(true);
                jsonWriter.WriteEndObject();
                jsonWriter.Close();
                streamWriter.Write(sw.ToString());
                streamWriter.Close();

                DataSet ds = null;
                using (System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse())
                {
                    BinaryFormatter fmt = new BinaryFormatter();
                    ds = (DataSet)fmt.Deserialize(response.GetResponseStream());
                }
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("There is an error running this Query.\n Query:" + command + " ");

            }



        }
        //</summary>
        //<param name="command"></param>
        //<returns></returns>
        public DataSet RunCasjobs(string command)
        {
            // throw new IndexOutOfRangeException("There is an invalid argument");

            try
            {

                var request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(ConfigurationManager.AppSettings["casjobsRESTapi"]);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/x-dataset";

                if (!token.Equals("") && token != null)
                    request.Headers.Add("X-Auth-Token", token);

                StreamWriter streamWriter = new StreamWriter(request.GetRequestStream());
                StringWriter sw = new StringWriter();
                JsonWriter jsonWriter = new JsonTextWriter(sw);
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName("Query");
                jsonWriter.WriteValue(command);
                //jsonWriter.WritePropertyName("ReturnDataSet");
                //jsonWriter.WriteValue(true);
                jsonWriter.WriteEndObject();
                jsonWriter.Close();
                streamWriter.Write(sw.ToString());
                streamWriter.Close();

                DataSet ds = null;
                using (System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse())
                {
                    BinaryFormatter fmt = new BinaryFormatter();
                    ds = (DataSet)fmt.Deserialize(response.GetResponseStream());
                }
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("There is an error running this Query.\n Query:" + command + " ");

            }
        }

        /// <summary>
        /// this is a hack  for some of QS_parameter functions
        /// </summary>

        public void showImgParams(string type, HttpResponse response)
        {

            string cmd = "SELECT [name] FROM DBColumns WHERE tableName='PhotoObjAll'";

            response.Write("<td class='q' width='100'>");
            DataSet ds = RunCasjobs(cmd, "SkyServer:showImgParams");
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {   
                if (!reader.HasRows)
                {
                    response.Write("<b>No columns found for table PhotoObjAll</b>\n");
                }
                else
                {
                    response.Write("\t<SELECT name=\"imgparams\" multiple=\"multiple+\" size=\"3\">\n");
                    if (type == "spec")
                    {
                        response.Write("\t\t<OPTION value=\"none\" selected>none</OPTION>\n");
                        response.Write("\t\t<OPTION value=\"minimal\">minimal</OPTION>\n");
                    }
                    else
                        response.Write("\t\t<OPTION value=\"minimal\" selected>minimal</OPTION>\n");
                    response.Write("\t\t<OPTION value=\"typical\">typical</OPTION>\n");
                    response.Write("\t\t<OPTION value=\"radec\">radec</OPTION>\n");
                    response.Write("\t\t<OPTION value=\"model_mags\">model_mags</OPTION>\n");
                    response.Write("\t\t<OPTION value=\"model_magerrs\">model_magerrs</OPTION>\n");
                    response.Write("\t\t<OPTION value=\"psf_mags\">psf_mags</OPTION>\n");
                    response.Write("\t\t<OPTION value=\"psf_magerrs\">psf_magerrs</OPTION>\n");
                    response.Write("\t\t<OPTION value=\"petro_mags\">petro_mags</OPTION>\n");
                    response.Write("\t\t<OPTION value=\"petro_magerrs\">petro_magerrs</OPTION>\n");
                    response.Write("\t\t<OPTION value=\"model_colors\">model_colors</OPTION>\n");
                    response.Write("\t\t<OPTION value=\"ugModelColor\">ugModelColor</OPTION>\n");
                    response.Write("\t\t<OPTION value=\"grModelColor\">grModelColor</OPTION>\n");
                    response.Write("\t\t<OPTION value=\"riModelColor\">riModelColor</OPTION>\n");
                    response.Write("\t\t<OPTION value=\"izModelColor\">izModelColor</OPTION>\n");
                    response.Write("\t\t<OPTION value=\"SDSSname\">SDSSname</OPTION>\n");
                    response.Write("\t\t<OPTION value=\"blankImg\"> </OPTION>\n");
                    while (reader.Read())
                    {
                        response.Write("\t\t<OPTION value=\"" + Utilities.getSqlString(reader.GetValue(0)) + "\">" + Utilities.getSqlString(reader.GetValue(0)) + "\n");
                    }
                    response.Write("\t</OPTION></SELECT>\n");
                }
                response.Write("</td>\n");
            } // using dattable reader

        }

        public  void showSpecParams( string type, HttpResponse response)
        {
            
             string cmd = "SELECT [name] FROM DBColumns WHERE tableName='SpecObjAll'";
             DataSet ds = RunCasjobs(cmd,"SkyServer:showSpecParams");
             using (DataTableReader reader = ds.Tables[0].CreateDataReader())
             {
                response.Write("<td class='q' width='100'>");
                if (!reader.HasRows)
                {
                    response.Write("<b>No columns found for table SpecObjAll</b>\n");
                }
                else
                {
                    response.Write("\t<SELECT name=\"specparams\" multiple=\"multiple\" size=\"3\">\n");
                    if (type == "spec")
                        response.Write("\t\t<OPTION value=\"minimal\" selected>minimal</OPTION>\n");
                    else
                    {
                        response.Write("\t\t<OPTION value=\"none\" selected>none</OPTION>\n");
                        response.Write("\t\t<OPTION value=\"minimal\">minimal</OPTION>\n");
                    }
                    response.Write("\t\t<OPTION value=\"typical\">typical</OPTION>\n");
                    response.Write("\t\t<OPTION value=\"radec\">radec</OPTION>\n");
                    response.Write("\t\t<OPTION value=\"blankSpec\"> </OPTION>\n");
                    while (reader.Read())
                    {
                        response.Write("\t\t<OPTION value=\"" + Utilities.getSqlString(reader.GetValue(0)) + "\">" + Utilities.getSqlString(reader.GetValue(0)) + "\n");

                    }
                    response.Write("\t</OPTION></SELECT>\n");
                }
                response.Write("</td>\n");
             } // using DataReader
            
        }

        public  void showIRSpecParams( string type, HttpResponse response)
        {            
            //string cmd = "SELECT [name] FROM DBColumns WHERE tableName='apogeeStar'";
            string cmd = "SELECT CASE WHEN [name] = 'file' then '['+[name]+']' else [name] END FROM DBColumns WHERE tableName='apogeeStar'";
            DataSet ds = RunCasjobs(cmd,"SkyServer:showIRSpecParams");
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
               
            {
                response.Write("<td class='q' colspan='2' align='center'>");
                if (!reader.HasRows)
                {
                    response.Write("<b>No columns found for table apogeeStar</b>\n");
                }
                else
                {
                    response.Write("\t<SELECT style=\"width:200px;\" name=\"irspecparams\" multiple=\"multiple\" size=\"6\">\n");
                    if (type == "irspec")
                        response.Write("\t\t<OPTION value=\"minimal\">Minimal</OPTION>\n");
                    else
                    {
                        response.Write("\t\t<OPTION value=\"none\" selected>none</OPTION>\n");
                        response.Write("\t\t<OPTION value=\"minimal\">minimal</OPTION>\n");
                    }
                    response.Write("\t\t<OPTION value=\"typical\" selected>Typical</OPTION>\n");
                    response.Write("\t\t<OPTION value=\"blankSpec\"> </OPTION>\n");
                    while (reader.Read())
                    {
                        response.Write("\t\t<OPTION value=\"" + Utilities.getSqlString(reader.GetValue(0)) + "\">" + Utilities.getSqlString(reader.GetValue(0)) + "\n");

                    }
                    response.Write("\t\t<OPTION value=\"blankSpec\"> </OPTION>\n");
                    response.Write("\t\t<OPTION value=\"twomassj\">2MASS J</OPTION>\n");
                    response.Write("\t\t<OPTION value=\"twomassh\">2MASS H</OPTION>\n");
                    response.Write("\t\t<OPTION value=\"twomassk\">2MASS K_s</OPTION>\n");
                    response.Write("\t</OPTION></SELECT>\n");
                }
                response.Write("</td>\n");
            } // using DataReader           
        }       
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Specialized;
using System.IO;
using System.Data.SqlClient;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;

namespace SkyServer.Tools.Explore
{
    // use casjobs to run
    public class RunQuery
    {
        //private String connectionString;
        private Globals global = new Globals();
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
            else
            {                
                string token = "";
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
        }


        private NameValueCollection runDb(string query)
        {
            NameValueCollection nvc = new NameValueCollection();

            using (SqlConnection oConn = new SqlConnection(global.ConnectionString))
            {
                oConn.Open();
                using (SqlCommand oCmd = oConn.CreateCommand())
                {
                    oCmd.CommandText = query;
                    using (SqlDataReader reader = oCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.HasRows)
                            {
                                //string[] names = new string[reader.FieldCount];
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    nvc.Add(reader.GetName(i), reader.GetValue(i).ToString());
                                }
                            }

                        }
                    }
                }
            }
            return nvc;
        }

        string[] injection = new string[] { "--", ";", "/*", "*/", "'", "\"" };

        string requestUri = "http://dev.sciserver.org/CasJobs/RestApi/contexts/dr10/query";

        ///**
        // * This is working but casjobs stopped working.
        // **/ 
        //public DataSet RunCasjobs(string command) 
        //{
        //    var request = (HttpWebRequest)WebRequest.Create(requestUri);
        //    request.Method = "POST";
        //    request.ContentType = "application/json";
        //    StreamWriter streamWriter = new StreamWriter(request.GetRequestStream());
        //    StringWriter sw = new StringWriter();
        //    JsonWriter jsonWriter = new JsonTextWriter(sw);
        //    jsonWriter.WriteStartObject();
        //    jsonWriter.WritePropertyName("Query");
        //    jsonWriter.WriteValue(command);
        //    jsonWriter.WritePropertyName("ReturnDataSet");
        //    jsonWriter.WriteValue(true);
        //    jsonWriter.WriteEndObject();
        //    jsonWriter.Close();
        //    streamWriter.Write(sw.ToString());
        //    streamWriter.Close();

        //    DataSet ds = null;
        //    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        //    {
        //        BinaryFormatter fmt = new BinaryFormatter();
        //        ds = (DataSet)fmt.Deserialize(response.GetResponseStream());
        //    }
        //    return ds;
        //}

        public DataSet RunCasjobs(string command)
        {
            DataSet ds = new DataSet();
            using (SqlConnection oConn = new SqlConnection(global.ConnectionString))
            {
                oConn.Open();
                using (SqlCommand oCmd = oConn.CreateCommand())
                {
                    oCmd.CommandText = command;
                    using (SqlDataAdapter da = new SqlDataAdapter(command, global.ConnectionString)) {
                        da.Fill(ds);
                    }
                }
            }
            return ds;
        }     
    }
}
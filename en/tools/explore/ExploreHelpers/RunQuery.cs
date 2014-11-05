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

        private Globals global = new Globals();

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

        ////***this was added to use the namevalue pair options
        //// Just kept for the reference.

        //private NameValueCollection runDb(string query)
        //{
        //    NameValueCollection nvc = new NameValueCollection();

        //    using (SqlConnection oConn = new SqlConnection(global.ConnectionString))
        //    {
        //        oConn.Open();
        //        using (SqlCommand oCmd = oConn.CreateCommand())
        //        {
        //            oCmd.CommandText = query;
        //            using (SqlDataReader reader = oCmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    if (reader.HasRows)
        //                    {
        //                        //string[] names = new string[reader.FieldCount];
        //                        for (int i = 0; i < reader.FieldCount; i++)
        //                        {
        //                            nvc.Add(reader.GetName(i), reader.GetValue(i).ToString());
        //                        }
        //                    }

        //                }
        //            }
        //        }
        //    }
        //    return nvc;
        //}

    }
}
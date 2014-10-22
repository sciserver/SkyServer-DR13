using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Specialized;
using System.IO;
using System.Data.SqlClient;

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
                /// Once the authenticated skyserver is ready, we ca update the code to retrieve token
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
    }
}
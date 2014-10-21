using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;

namespace SkyServer.Tools.Explore
{
    public class ApogeeInfoREST : ApogeeInfo
    {
        string[] injection = new string[] { "--", ";", "/*", "*/", "'", "\"" };

        public ApogeeInfoREST(string requestUri, double ra, double dec, double radius)
        {
            string command = ApogeeInfo.BASE_QUERY + FIND_NEAREST;
            command = command.Replace("@radius", radius.ToString());
            command = command.Replace("@ra", ra.ToString());
            command = command.Replace("@dec", dec.ToString());


            try { ReadInfo(requestUri, command); }
            catch (Exception ex) { throw new Exception("ApogeeInfo ERROR (ra: " + ra + ", dec: " + dec + ", radius: " + radius + ")", ex); }

            ReadVisits(requestUri);
        }

        public ApogeeInfoREST(string requestUri, string id)
        {
            string command;
            if (id.StartsWith("apogee")) { command = BASE_QUERY + FIND_APSTAR_ID; }
            else { command = BASE_QUERY + FIND_APOGEE_ID; }

            foreach (string s in injection) 
            {
                if (id.IndexOf(s) >= 0)
                {
                    throw new Exception("Invalid APOGEE ID: "+id);
                }
            }

            command = command.Replace("@id", "'"+id+"'");

            try { ReadInfo(requestUri, command); }
            catch (Exception ex) { throw new Exception("ApogeeInfo ERROR (id: " + id + ")", ex); }

            ReadVisits(requestUri);
        }

        public ApogeeInfoREST(string requestUri, long plate, long mjd, long fiberid)
        {
            string command = BASE_QUERY + FIND_PLFIB;
            command = command.Replace("@plate", plate.ToString());
            command = command.Replace("@mjd", mjd.ToString());
            command = command.Replace("@fiberid", fiberid.ToString());

            try { ReadInfo(requestUri, command); }
            catch (Exception ex) { throw new Exception("ApogeeInfo ERROR (plate: " + plate + ", mjd: " + mjd + ", fiberid: " + fiberid + ")", ex); }

            ReadVisits(requestUri);
        }

        private void ReadInfo(string requestUri, string command) 
        {
            var request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.Method = "POST";
            request.ContentType = "application/json";
            StreamWriter streamWriter = new StreamWriter(request.GetRequestStream());
            StringWriter sw = new StringWriter();
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("Query");
            jsonWriter.WriteValue(command);
            jsonWriter.WritePropertyName("ReturnDataSet");
            jsonWriter.WriteValue(true);
            jsonWriter.WriteEndObject();
            jsonWriter.Close();
            streamWriter.Write(sw.ToString());
            streamWriter.Close();

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                BinaryFormatter fmt = new BinaryFormatter();
                DataSet ds = (DataSet)fmt.Deserialize(response.GetResponseStream());

                using (DataTableReader reader = ds.Tables[0].CreateDataReader())
                {
                    ReadInfoFromDbReader(reader);
                }
            }
        }

        private void ReadVisits(string requestUri)
        {
            string command = ApogeeVisit.BASE_QUERY;
            foreach (string s in injection)
            {
                if (apogee_id.IndexOf(s) >= 0)
                {
                    throw new Exception("Invalid APOGEE ID: " + apogee_id);
                }
            }
            command = command.Replace("@id", "'" + apogee_id + "'");

            var request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.Method = "POST";
            request.ContentType = "application/json";
            StreamWriter streamWriter = new StreamWriter(request.GetRequestStream());
            StringWriter sw = new StringWriter();
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("Query");
            jsonWriter.WriteValue(command);
            jsonWriter.WritePropertyName("ReturnDataSet");
            jsonWriter.WriteValue(true);
            jsonWriter.WriteEndObject();
            jsonWriter.Close();
            streamWriter.Write(sw.ToString());
            streamWriter.Close();

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                BinaryFormatter fmt = new BinaryFormatter();
                DataSet ds = (DataSet)fmt.Deserialize(response.GetResponseStream());

                using (DataTableReader reader = ds.Tables[0].CreateDataReader())
                {
                    ReadVisitsFromDbReader(reader);
                }
            }
        }
    }
}
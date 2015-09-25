using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using SkyServer.Tools.Search;
using System.Data;

namespace SkyServer.Get
{
    /// <summary>
    /// Summary description for FrameById
    /// </summary>
    public class FrameById : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "image/jpeg";
            Globals globals = (Globals)context.Application[Globals.PROPERTY_NAME];
            long id = long.Parse(context.Request.QueryString["id"]);
            int zoom = int.Parse(context.Request.QueryString["zoom"]);

            string cmd = "SELECT run,camCol,field,zoom FROM Frame WHERE fieldID="+id.ToString()+" AND zoom="+zoom.ToString();
            ResponseREST runQuery = new ResponseREST();
            string ClientIP = runQuery.GetClientIP(); 
            DataSet ds = runQuery.RunDatabaseSearch(cmd, globals.ContentDataset, ClientIP, "Skyserver.Explore.FrameById.getRCFZ");
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (!reader.HasRows)
                {
                    context.Response.Redirect("noimage2.jpg");
                }
                else
                {
                    reader.Read();
                    string run = reader.GetValue(0).ToString();
                    string col = reader.GetValue(1).ToString();
                    string fld = reader.GetValue(2).ToString();
                    string zz = reader.GetValue(3).ToString();
                    context.Response.Redirect(globals.WSGetCodecUrl + "?R=" + run + "&C=" + col + "&F=" + fld + "&Z=" + zz);
                }
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
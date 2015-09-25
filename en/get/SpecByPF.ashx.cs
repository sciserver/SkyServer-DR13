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
    /// Summary description for SpecByPF
    /// </summary>
    public class SpecByPF : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "image/gif";
            Globals globals = (Globals)context.Application[Globals.PROPERTY_NAME];

            long plateid = long.Parse(context.Request.QueryString["P"]);
            short fiberid = short.Parse(context.Request.QueryString["F"]);

            string cmd = "SELECT img FROM SpecObjAll WHERE plateID=" + plateid.ToString() + " AND fiberID=" + fiberid.ToString();

            ResponseREST runQuery = new ResponseREST();
            string ClientIP = runQuery.GetClientIP();
            DataSet ds = runQuery.RunDatabaseSearch(cmd, globals.ContentDataset, ClientIP, "Skyserver.Explore.SpecByPF.getImg");
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (!reader.HasRows)
                {
                    context.Response.Redirect("noimage2.gif");
                }
                else
                {
                    reader.Read();
                    context.Response.BinaryWrite((byte[])reader.GetValue(0));
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
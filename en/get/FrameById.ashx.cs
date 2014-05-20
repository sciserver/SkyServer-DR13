using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

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

            string cmd = "SELECT run,camCol,field,zoom FROM Frame WHERE fieldID=@id AND zoom=@zoom";

            using (SqlConnection oConn = new SqlConnection(globals.ConnectionString))
            {
                oConn.Open();
                using (SqlCommand oCmd = oConn.CreateCommand())
                {
                    oCmd.CommandText = cmd;
                    oCmd.Parameters.AddWithValue("@id", id);
                    oCmd.Parameters.AddWithValue("@zoom", zoom);

                    using (SqlDataReader reader = oCmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {

                            context.Response.Redirect("noimage2.jpg");

                        }
                        else
                        {
                            reader.Read();
                            string run = reader.GetSqlValue(0).ToString();
                            string col = reader.GetSqlValue(1).ToString();
                            string fld = reader.GetSqlValue(2).ToString();
                            string zz = reader.GetSqlValue(3).ToString();
                            context.Response.Redirect(globals.WSGetCodecUrl + "?R=" + run + "&C=" + col + "&F=" + fld + "&Z=" + zz);
                        }
                    }
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
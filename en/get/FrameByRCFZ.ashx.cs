using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace SkyServer.Get
{
    /// <summary>
    /// Summary description for FrameByRCFZ
    /// </summary>
    public class FrameByRCFZ : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "image/jpeg";
            Globals globals = (Globals)context.Application[Globals.PROPERTY_NAME];

            int run = int.Parse(context.Request.QueryString["R"]);
            int col = int.Parse(context.Request.QueryString["C"]);
            int fld = int.Parse(context.Request.QueryString["F"]);
            int zz = int.Parse(context.Request.QueryString["Z"]);

            //	build the SQL query string

            string cmd = "SELECT run,camCol,field,zoom FROM Frame WHERE zoom=@zz AND run=@run AND camCol=@col AND field=@fld";

            using (SqlConnection oConn = new SqlConnection(globals.ConnectionString))
            {
                oConn.Open();
                using (SqlCommand oCmd = oConn.CreateCommand())
                {
                    oCmd.CommandText = cmd;
                    oCmd.Parameters.AddWithValue("@zz", zz);
                    oCmd.Parameters.AddWithValue("@run", run);
                    oCmd.Parameters.AddWithValue("@col", col);
                    oCmd.Parameters.AddWithValue("@fld", fld);

                    using (SqlDataReader reader = oCmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {

                            context.Response.Redirect("noimage2.jpg");

                        }
                        else
                        {
                            reader.Read();
                            run = reader.GetInt32(0);
                            col = reader.GetInt32(1);
                            fld = reader.GetInt32(2);
                            zz = reader.GetInt32(3);
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
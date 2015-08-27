using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Data.SqlClient;
using System.Data;
using SkyServer.Tools.Search;

namespace SkyServer.Get
{
    /// <summary>
    /// Summary description for SpecById
    /// </summary>
    public class SpecById : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "image/gif";

            long specid = long.Parse(context.Request.QueryString["ID"]);
            string cmd = "SELECT img FROM SpecObjAll WHERE specobjID="+specid;

            Globals globals = (Globals)context.Application[Globals.PROPERTY_NAME];
            ResponseREST rs = new ResponseREST();
        
            //using (SqlConnection oConn = new SqlConnection(globals.ConnectionString))
            //{
                //oConn.Open();
                //using (SqlCommand oCmd = oConn.CreateCommand())
                //{
                    //oCmd.CommandText = cmd;
                    //oCmd.Parameters.AddWithValue("@specid", specid);

                    //using (SqlDataReader reader = oCmd.ExecuteReader())
                    DataSet ds = rs.RunCasjobs(cmd,"SkyServer:SpecById");
                    using (DataTableReader reader = ds.CreateDataReader(ds.Tables[0]))  
                    {
                        // start writing the image
                        // if there is no image in the database, redirect the URL to a
                        // predefined blank image of the same size

                        if (!reader.HasRows)
                        {
                            context.Response.Redirect("noimage2.gif");
                        }
                        else
                        {
                            reader.Read();
                            context.Response.BinaryWrite((byte[]) reader.GetValue(0));//reader.GetSqlBytes(0).Buffer);
                        }
                    }
                //}
            //}
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
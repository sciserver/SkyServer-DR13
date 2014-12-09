using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace SkyServer.Tools.Search
{
    public partial class X_Radial : System.Web.UI.Page
    {
        protected string url;
        protected Globals globals;

        string windows_name;
        string server_name;
        string remote_addr;

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.AppendHeader("Access-Control-Allow-Origin", "*");

            url = ResolveClientUrl("~/en");
            globals = (Globals)Application[Globals.PROPERTY_NAME];

            windows_name = System.Environment.MachineName;
            server_name = Request.ServerVariables["SERVER_NAME"];
            remote_addr = Request.ServerVariables["REMOTE_ADDR"];
        }

        protected void ProcessRequestREST()
        {
            Response.Buffer = true;
            ResponseREST rs = new ResponseREST();
            //rs.ProcessRequestREST(Request, Response, "Radial");
        }
        protected void ProcessRequest()
        {
            try
            {
                DataSet dataSet = new DataSet();
                RadialSearchRequest searchRequest;

                try
                {
                    searchRequest = new RadialSearchRequest(url, Request);
                }
                catch (Exception ex)
                {
                    Response.Write("<H1><FONT color=blue>Please fix errors before continuing.</Font></H1><P>");
                    Response.Write(ex.Message + "<P>");
                    return;
                }

                using (SqlConnection oConn = new SqlConnection(globals.ConnectionString))
                {
                    oConn.Open();
                    if (searchRequest.FP != "none")
                    {
                        using (SqlCommand oCmd = oConn.CreateCommand())
                        {
                            oCmd.CommandText = "EXEC spExecuteSQL @sql, 100000, @server_name, @windows_name, @remote_addr, @access";
                            oCmd.Parameters.AddWithValue("@sql", searchRequest.ImageQuery);
                            oCmd.Parameters.AddWithValue("@server_name", server_name);
                            oCmd.Parameters.AddWithValue("@windows_name", windows_name);
                            oCmd.Parameters.AddWithValue("@remote_addr", remote_addr);
                            oCmd.Parameters.AddWithValue("@access", globals.Access);

                            using (SqlDataReader reader = oCmd.ExecuteReader())
                            {
                                if (searchRequest.Format == "html")
                                {
                                    if (!reader.HasRows)
                                    {
                                        Response.Write("<span style=\"font-size:large;color:red;\">Sorry, the area you requested (RA=" + searchRequest.RA + ", dec=" + searchRequest.Dec + ", radius=" + searchRequest.Radius + ") is outside the SDSS " + globals.Release + " survey area.</span>");
                                        Response.Flush();
                                        return;
                                    }
                                    else
                                    {
                                        if (searchRequest.FP == "only")
                                        {
                                            Response.Write("<span style=\"font-size:large;color:green;\">The area you requested (RA=" + searchRequest.RA + ", dec=" + searchRequest.Dec + ", radius=" + searchRequest.Radius + ") overlaps with the SDSS " + globals.Release + " survey area.</span>");
                                            Response.Flush();
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    else if (searchRequest.FP != "only")
                    {
                        string execImage = "EXEC spExecuteSQL '" + searchRequest.ImageQuery + "', 100000,'" + server_name + "','" + windows_name + "','" + remote_addr + "','" + globals.Access + "'";
                        string execIR = "EXEC spExecuteSQL '" + searchRequest.IRQuery + "', 100000,'" + server_name + "','" + windows_name + "','" + remote_addr + "','" + globals.Access + "'";

                        if (searchRequest.Format == "html")
                        {
                            Response.Write("<h1>Imaging</h1>\n");
                            ResponseAux.writeOutput(oConn, execImage, searchRequest.ImageQuery, searchRequest.Format, Response, globals, globals.FormTimeout);
                            Response.Write("\n<h1>Infrared Spectra</h1>\n");
                            ResponseAux.writeOutput(oConn, execIR, searchRequest.IRQuery, searchRequest.Format, Response, globals, globals.FormTimeout);
                        }
                        else
                        {
                            DataTable table1 = ResponseAux.getDataTable(oConn, execImage, globals.FormTimeout);
                            dataSet.Tables.Add(table1);
                            DataTable table2 = ResponseAux.getDataTable(oConn, execIR, globals.FormTimeout);
                            dataSet.Tables.Add(table2);
                            ResponseAux.writeOutput(dataSet, searchRequest.Format, Response);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ResponseAux.writeError(ex, Response);
            }
        }
    }
}
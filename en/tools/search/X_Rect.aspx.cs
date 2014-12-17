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
    public partial class X_Rect : System.Web.UI.Page
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

        protected void ProcessRequest()
        {
            try
            {
                DataSet dataSet = new DataSet();
                RectSearchRequest searchRequest;
                try
                {
                    searchRequest = new RectSearchRequest(url, Request);
                }
                catch (Exception ex)
                {
                    Response.Write("<H2><FONT color=red>Please fix errors before continuing.</Font></H2><P>");
                    Response.Write(ex.Message + "<P>");
                    return;
                }

                using (SqlConnection oConn = new SqlConnection(globals.ConnectionString))
                {
                    oConn.Open();

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
            catch (Exception ex)
            {
                ResponseAux.writeError(ex, Response);
            }
        }
    }
}
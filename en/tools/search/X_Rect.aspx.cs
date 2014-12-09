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

        private string check_u;
        private string check_g;
        private string check_r;
        private string check_i;
        private string check_z;

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
            //RectSearchRequest searchRequest;
            try
            {
                //int limit = int.Parse(Request["limit"] ?? "0");
                //string format = Request.Form["format"] ?? "html";
                //format = format.ToLower();
                //string whichway = Request.Form["whichway"];

                //string check_u = Request.Form["check_u"] ?? "false";
                //string check_g = Request.Form["check_g"] ?? "false";
                //string check_r = Request.Form["check_r"] ?? "false";
                //string check_i = Request.Form["check_i"] ?? "false";
                //string check_z = Request.Form["check_z"] ?? "false";
                
                ////string syntax = Request.Form["syntax"];
                //string requestString = "min_ra=" + Request["min_ra"] + "&min_dec=" + Request["min_dec"];
                //       requestString += "&max_ra=" + Request.Form["max_ra"] + "&max_dec=" + Request["max_dec"];

                //if (check_u == "u") requestString += "&uband=" + Request.Form["min_u"] + "," + Request.Form["max_u"];
                //if (check_g == "g") requestString += "&gband=" + Request.Form["min_g"] + "," + Request.Form["max_g"];
                //if (check_r == "r") requestString += "&rband=" + Request.Form["min_r"] + "," + Request.Form["max_r"];
                //if (check_i == "i") requestString += "&iband=" + Request.Form["min_i"] + "," + Request.Form["max_i"];
                //if (check_z == "z") requestString += "&zband=" + Request.Form["min_z"] + "," + Request.Form["max_z"];

                //requestString += "&searchtype=" + whichway + "&returntype=" + format + "&limit=" + limit;

                //string requestString = Request.QueryString.ToString();
                //string rectangularSear = globals.RectangularSearchWS;
                //ResponseREST.runQuery(rectangularSear, requestString, Response);

                ResponseREST rs = new ResponseREST();
                //rs.ProcessRequestREST(Request, Response,"Rectangular");
            }
            catch (Exception ex)
            {
                Response.Write("<H2><FONT color=red>Please fix errors before continuing.</Font></H2><P>");
                Response.Write(ex.Message + "<P>");
                return;
            }
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
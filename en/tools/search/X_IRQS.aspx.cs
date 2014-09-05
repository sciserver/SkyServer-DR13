using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Specialized;
namespace SkyServer.Tools.Search
{
    public partial class X_IRQS : System.Web.UI.Page
    {
        protected string url;
        protected Globals globals;

        string windows_name;
        string server_name;
        string remote_addr;

        protected void Page_Load(object sender, EventArgs e)
        {
            url = ResolveClientUrl("~/en");
            globals = (Globals)Application[Globals.PROPERTY_NAME];

            windows_name = System.Environment.MachineName;
            server_name = Request.ServerVariables["SERVER_NAME"];
            remote_addr = Request.ServerVariables["REMOTE_ADDR"];
        }

        protected void ProcessRequestREST() {
            NameValueCollection inputForm = Request.Form;
            String requestString = "";
            foreach (string key in inputForm.Keys) {
                
                requestString += key + "=" + inputForm[key]+"&";
            }

            string requestUrl = "";
            
            switch(inputForm["positionType"]){
                case "cone": requestUrl = globals.ConeIRWS; break;
                case "conelb": requestUrl = globals.GalacticIRWS; break;
                case "none": requestUrl = globals.NoPositionIRWS; break;
                default: throw new Exception("No proper positionType selcted."); break;
            }
            //string requestUrl = "http://apus.pha.jhu.edu/SkyserverWS/DR10/IRSpectraQuery/GalacticIR";
            ResponseREST.runQuery(requestUrl,requestString,Response); 
        }

        protected void ProcessRequest()
        {


            // check for QueryString if TotalBytes==0 (GET method)
            if ("GET".Equals(Request.HttpMethod))
            { }
            // otherwise get the Forms (POST method)
            else
            { }

            if (Request["format"] == "html")
            {
                Response.Write("<html><body>\n");
                Response.Write("<h2>Executing query ... </h2>\n");
            }

            using (SqlConnection oConn = new SqlConnection(globals.ConnectionString))
            {
                oConn.Open();
                string cmd;
                
                   //Jordan debugging lines, plz remove
                  // Response.Write("<h1><font color='green'>" + Request.Form.Count + "</font></h1>");
                /*  string s;

                  for (int i = 0; i < Request.Form.Count; i++)
                  {
                      s = Request.Form[i].ToString();
                      Response.Write("<h1><font color='blue'>i = " +i+ " , Key: " + Request.Form.Keys[i] + " , Value: " + s + "</font></h1>");
                  }
                    /*
                  if (String.Equals(Request.Form["positionType"].ToString(), "conelb"))
                  {
                      Response.Write("<h1><font color='green'>" + Request.Form["raCenter"] + "</font></h1>");
                      //Request.Form["raCenter"] = "0";
                      Response.Write("<h1><font color='orange'>" + Request.Form["raCenter"] + "</font></h1>");
                  }
                 */
                // end of debugging lines
  
                cmd = QueryFuncs.buildQuery(oConn, "irspec", globals, Request);
              //  Response.Write("<h1><font color='red'>" + cmd + "</font></h1>");  // moar debugging linez from Jordan
                
                string[] cmdSplit = cmd.Split(' ');
                /*
                if (dbg == 1)
                    showLine("cmd start = " + cmdSplit[0] + "\n"); */

                if (cmdSplit[0] == "ERROR:")
                    Response.Write("<h3><font color=red>Error:</font> " + cmd + "</h3>\n");
                else
                {
                    string c = cmd;
                    string cmd_echo = cmd;

                    c = c.Replace("'", "''");
                    cmd = "EXEC spExecuteSQL '" + c + "  ', " + 50000 + ",'" + server_name + "','" + windows_name + "','" + remote_addr + "','" + globals.Access + "'";


                    //oConn.CommandTimeout = formTimeout;
                    //oCmd.CommandTimeout = formTimeout;
                    // Server.ScriptTimeout = formTimeout;
                    /*
                    if (dbg==1) 
                        showLine(cmd+"\n");
                    */
                    /*
                    if (Request["format"] == "html")
                        ResponseAux.writeHTML(oConn, cmd, cmd_echo, 1, Response, globals, globals.FormTimeout);
                    else if (Request["format"] == "xml") ResponseAux.writeXML(oConn, cmd, cmd_echo, Response, globals, globals.FormTimeout);
                    else if (Request["format"] == "csv") ResponseAux.writeCSV(oConn, cmd, Response, globals, globals.FormTimeout);
                     */
                    string format = Request["format"];

                    if (format == "html")
                    {
                        ResponseAux.writeOutput(oConn, cmd, c, format, Response, globals, globals.FormTimeout);
                    }
                    else
                    {
                        try
                        {
                            DataTable table = ResponseAux.getDataTable(oConn, cmd, globals.FormTimeout);
                            DataSet dataSet = new DataSet();
                            dataSet.Tables.Add(table);
                            ResponseAux.writeOutput(dataSet, format, Response);
                        }
                        catch (Exception ex)
                        {
                            ResponseAux.writeError(ex, Response);
                        }
                    }
                }

            }
            if (Request["format"] == "html")
                Response.Write("</body></html>\n");
        }
    } 
}
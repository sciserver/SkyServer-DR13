using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Data;
using System.Net.Http;
//using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace SkyServer.Tools.Search
{
    public partial class X_SQL : System.Web.UI.Page
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

            //	read in query     
            string c = Request.Form["cmd"];
            string format = Request.Form["format"];
            string syntax = Request.Form["syntax"];

            if (c == null)
            { c = Request.QueryString["cmd"]; }

            if (format == null)
            { format = Request.QueryString["format"]; }

            if (syntax == null)
            { syntax = Request.QueryString["syntax"]; }

            string c2 = Regex.Replace(c, @"\/\*(.*\n)*\*\/", "");	// remove all multi-line comments
            c2 = Regex.Replace(c2, @"^[ \t\f\v]*--.*\r\n", "", RegexOptions.Multiline);		// remove all isolated single-line comments
            c2 = Regex.Replace(c2, @"--[^\r^\n]*", "");				// remove all embedded single-line comments
            c2 = Regex.Replace(c2, @"[ \t\f\v]+", " ");				// replace multiple whitespace with single space
            c2 = Regex.Replace(c2, @"^[ \t\f\v]*\r\n", "", RegexOptions.Multiline);			// remove empty lines
            c = c2;								// make a copy of massaged query
            c2 = c2.Replace("'", "''");		// 'c' is query version that's printed on output page
            // 'c2' is the version that is sent to DB server


            string requestString = "query=" + c2 + "&format=" + format; 

            ResponseREST.runQuery(globals.SQLSearchWS,requestString,Response);            
                       
        }

        protected void ProcessRequest()
        {
            Response.Buffer = true;

            //	read in query     
            string c = Request.Form["cmd"];
            string format = Request.Form["format"];
            string syntax = Request.Form["syntax"];

            if (c == null)
            { c = Request.QueryString["cmd"]; }

            if (format == null)
            { format = Request.QueryString["format"]; }

            if (syntax == null)
            { syntax = Request.QueryString["syntax"]; }


            string c2 = Regex.Replace(c, @"\/\*(.*\n)*\*\/", "");	// remove all multi-line comments
            c2 = Regex.Replace(c2, @"^[ \t\f\v]*--.*\r\n", "",RegexOptions.Multiline);		// remove all isolated single-line comments
            c2 = Regex.Replace(c2, @"--[^\r^\n]*", "");				// remove all embedded single-line comments
            c2 = Regex.Replace(c2, @"[ \t\f\v]+", " ");				// replace multiple whitespace with single space
            c2 = Regex.Replace(c2, @"^[ \t\f\v]*\r\n", "",RegexOptions.Multiline);			// remove empty lines
            c = c2;								// make a copy of massaged query
            c2 = c2.Replace("'", "''");		// 'c' is query version that's printed on output page
            // 'c2' is the version that is sent to DB server

            string cmd;
            using (SqlConnection oConn = new SqlConnection(globals.ConnectionString))
            {
                oConn.Open();
                if (syntax == "Syntax")
                {
                    // if syntax check only is desired, add line numbers to query
                    string[] clines = c.Split('\n');
                    c = "<i>Line#</i>\n";
                    for (int i = 0; i < clines.Length; i++)
                    {
                        if ((i < (clines.Length - 1)) || (clines[i].Length > 0))
                        {
                            if ((i + 1) < 10)
                                c += "<i>" + (i + 1) + ".</i>   " + clines[i];
                            else if ((i + 1) < 100)
                                c += "<i>" + (i + 1) + ".</i>  " + clines[i];
                            else
                                c += "<i>" + (i + 1) + ".</i> " + clines[i];
                        }
                    }

                    // prefix with "set parseonly on" to invoke SQL syntax check
                    cmd = "EXEC spExecuteSQL '" + "set parseonly on " + c2 + "  ', " + 500000 + ",'" + server_name + "','" + windows_name + "','" + remote_addr + "','" + globals.Access + "'";
                    ResponseAux.writeSyntaxMessage(oConn, cmd, c, Response);		// ignore output format, call special output function for syntax
                }
                else
                {
                    cmd = "EXEC spExecuteSQL '" + c2 + "  ', " + 500000 + ",'" + server_name + "','" + windows_name + "','" + remote_addr + "','" + globals.Access + "'";

                    if (format == "html")
                    {
                        ResponseAux.writeOutput(oConn, cmd, c, format, Response, globals, globals.SqlTimeout);
                    }
                    else
                    {
                        try
                        {
                            DataTable table = ResponseAux.getDataTable(oConn, cmd, globals.SqlTimeout);
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
            } // using SqlConnection
        }

    }
}
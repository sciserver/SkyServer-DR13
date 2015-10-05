using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using SkyServer.Tools.Search;
using System.Data;

namespace SkyServer.Tools.GetImg
{
    public partial class Fields : System.Web.UI.Page
    {
        protected string getimgurl;
        protected string url;

        protected string field0;
        protected Globals globals;
        ToolsMaster master;
        ResponseREST runQuery;
        string ClientIP = "";



        protected void Page_Load(object sender, EventArgs e)
        {
            runQuery = new ResponseREST();
            ClientIP = runQuery.GetClientIP();

            globals = (Globals)Application[Globals.PROPERTY_NAME];
            master = (ToolsMaster)Page.Master;
            master.gselect = 4.1;

            url = ResolveClientUrl("~/en");
            getimgurl = url + "/get/FrameByRCFZ.ashx";
        }

        protected void writeFields()
        {
            string cmd = "SELECT run,startField,endField ";
            cmd += " FROM Run ORDER BY run";
            DataSet ds = runQuery.RunDatabaseSearch(cmd, globals.ContentDataset, ClientIP, "Skyserver.getimg.Fields.getRun_startField_endField");
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {

                string v = "";
                int lastRun = -1, run;
                int i;
                while (reader.Read())
                {
                    run = reader.GetInt16(0);
                    if (lastRun > -1)
                    {
                        if (run != lastRun)
                        {
                            ResponseAux.writeRow(lastRun, v, Response);
                            v = "";
                        }
                        else
                        {
                            v += ";";
                        }
                    }
                    for (i = 1; i < reader.FieldCount; i++)
                    {
                        v += reader.GetValue(i).ToString();
                        if (i != reader.FieldCount - 1)
                            v += ",";
                    }
                    lastRun = run;
                }
                ResponseAux.writeRow(lastRun, v, Response);

            }
        }

        protected void writeOptions()
        {
            string cmd = "SELECT run,stripe,startfield FROM Run ORDER BY run";
            DataSet ds = runQuery.RunDatabaseSearch(cmd, globals.ContentDataset, ClientIP, "Skyserver.getimg.Fields.getRun_stripe_startField");
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {

                int i = 0;
                while (reader.Read())
                {
                    if (i > 0)
                        Response.Write("<option VALUE=\"" + reader.GetValue(0).ToString() + "\">" + reader.GetValue(0).ToString() + "</option>");
                    else
                    {
                        field0 = reader.GetValue(2).ToString();
                        Response.Write("<option VALUE=\"" + reader.GetValue(0).ToString() + "\" selected>" + reader.GetValue(0).ToString() + "</option>");
                    }
                    i++;
                }
            } // using SqlDataReader
        }
    }
}
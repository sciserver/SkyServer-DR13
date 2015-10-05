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
    public partial class Spectra : System.Web.UI.Page
    {
        protected string url;
        protected string getimgurl;
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
            master.gselect = 4.3;

            url = ResolveClientUrl("~/en");
            getimgurl = url + "/get/SpecByPF.ashx";
        }

        protected void writeOptions()
        {
                string cmd = "SELECT CAST(plateID as VARCHAR(64)), plate, mjd ";
                cmd += " from PlateX order by plateID";
                DataSet ds = runQuery.RunDatabaseSearch(cmd, globals.ContentDataset, ClientIP, "Skyserver.getimg.Spectra.getPlateID");
                using (DataTableReader reader = ds.Tables[0].CreateDataReader())
                {
                    string v = "";

                        while (reader.Read())
                        {
                            v = "<OPTION VALUE='" + reader.GetValue(0).ToString() + "'>";
                            v += reader.GetValue(1).ToString() + "/" + reader.GetValue(2).ToString() + "</OPTION>\n";
                            Response.Write(v);
                           
                        }
                } // using SqlDataReader
        }
    } 
}
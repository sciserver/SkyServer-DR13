using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Globalization;
using SkyServer;
using System.Data;

namespace SkyServer.Tools.Explore
{
    public partial class FitsSpec : System.Web.UI.Page
    {
        protected string[] hrefsSpec;
        long? specObjId = null;

        protected Globals globals;

        protected RunQuery runQuery;
        protected string ClientIP = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            runQuery = new RunQuery();
            ClientIP = runQuery.GetClientIP();

            globals = (Globals)Application[Globals.PROPERTY_NAME];

            string sid = Request.QueryString["sid"];
            specObjId = Utilities.ParseId(sid);
            hrefsSpec = getFits(specObjId);
        }

        string[] getFits(long? specObjId) {
	        string[] result = null;
            string cmd = "select dbo.fGetUrlFitsSpectrum(@specObjId)";
            cmd = cmd.Replace("@specObjId", specObjId == null ? "" : specObjId.ToString());

            DataSet ds = runQuery.RunDatabaseSearch(cmd, globals.ContentDataset, ClientIP, "Skyserver.Explore.FitsSpec.getUrlFitsSpectrum");
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (reader.HasRows)
                {
                    result = new string[reader.FieldCount];
                }
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                        result[i] = reader.GetValue(i).ToString();
                }
            }
	        return result;
        }
    }  
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Globalization;
using SkyServer;

namespace SkyServer.Tools.Explore
{
    public partial class FitsSpec : System.Web.UI.Page
    {
        protected string[] hrefsSpec;
        long? specObjId = null;

        protected Globals globals;

        protected void Page_Load(object sender, EventArgs e)
        {
            globals = (Globals)Application[Globals.PROPERTY_NAME];

            string sid = Request.QueryString["sid"];
            specObjId = Utilities.ParseId(sid);

            using (SqlConnection oConn = new SqlConnection(globals.ConnectionString))
            {
                oConn.Open();
                hrefsSpec = getFits(oConn, specObjId);
            }
        }

        string[] getFits(SqlConnection oConn, long? specObjId) {
	        string[] result = null;
            using (SqlCommand oCmd = oConn.CreateCommand())
            {
                oCmd.CommandText = "select dbo.fGetUrlFitsSpectrum(@specObjId)";
                oCmd.Parameters.AddWithValue("@specObjId", specObjId);
                using (SqlDataReader reader = oCmd.ExecuteReader())
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
            }
	        return result;
        }
    }  
}

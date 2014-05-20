using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using SkyServer;

namespace SkyServer.Tools.Explore
{
    public partial class FitsImg : System.Web.UI.Page
    {
        protected Globals globals;
        protected string[] hrefsCf;
        protected string fieldId;

        protected void Page_Load(object sender, EventArgs e)
        {
            globals = (Globals)Application[Globals.PROPERTY_NAME];
            long? fieldId = Utilities.ParseId(Request.QueryString["fieldId"]);
            if (fieldId.HasValue)
            {
                using (SqlConnection oConn = new SqlConnection(globals.ConnectionString))
                {
                    oConn.Open();
                    hrefsCf = getCFrame(oConn, fieldId.Value);
                }
            }
        }

        private string[] getCFrame(SqlConnection oConn, long fieldId)
        {
            string[] result = null;
            using (SqlCommand oCmd = oConn.CreateCommand())
            {
                string cmd = "select";
                cmd += " dbo.fGetUrlFitsCFrame(" + fieldId + ",'u'),";
                cmd += " dbo.fGetUrlFitsCFrame(" + fieldId + ",'g'),";
                cmd += " dbo.fGetUrlFitsCFrame(" + fieldId + ",'r'),";
                cmd += " dbo.fGetUrlFitsCFrame(" + fieldId + ",'i'),";
                cmd += " dbo.fGetUrlFitsCFrame(" + fieldId + ",'z'),";

                cmd += " dbo.fGetUrlFitsBin(" + fieldId + ",'u'),";
                cmd += " dbo.fGetUrlFitsBin(" + fieldId + ",'g'),";
                cmd += " dbo.fGetUrlFitsBin(" + fieldId + ",'r'),";
                cmd += " dbo.fGetUrlFitsBin(" + fieldId + ",'i'),";
                cmd += " dbo.fGetUrlFitsBin(" + fieldId + ",'z'),";

                cmd += " dbo.fGetUrlFitsMask(" + fieldId + ",'u'),";
                cmd += " dbo.fGetUrlFitsMask(" + fieldId + ",'g'),";
                cmd += " dbo.fGetUrlFitsMask(" + fieldId + ",'r'),";
                cmd += " dbo.fGetUrlFitsMask(" + fieldId + ",'i'),";
                cmd += " dbo.fGetUrlFitsMask(" + fieldId + ",'z'),";

                cmd += " dbo.fGetUrlFitsAtlas(" + fieldId + "),";
                cmd += " dbo.fGetUrlFitsField(" + fieldId + ")";

                oCmd.CommandText = cmd;
                using (SqlDataReader reader = oCmd.ExecuteReader())
                {

                    if (reader.HasRows)
                    {
                        result = new string[reader.FieldCount];
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                                result[i] = reader.GetValue(i).ToString();
                        }
                    }
                } // using SqlDataReader

            } // using SqlCommand
            return result;
        }
    
    }
}
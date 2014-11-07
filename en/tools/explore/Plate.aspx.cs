using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using SkyServer;
using System.Data;
using System.Data.SqlClient;

namespace SkyServer.Tools.Explore
{
    public partial class Plate : System.Web.UI.Page
    {
        protected long? plateId = null;
        protected Globals globals;
        protected ObjectExplorer master;

        protected void Page_Load(object sender, EventArgs e)
        {
            globals = (Globals)Application[Globals.PROPERTY_NAME];
            master = (ObjectExplorer)Page.Master;
            
            string s = Request.QueryString["plateId"];
            plateId = Utilities.ParseId(s);          
        }

        public void showFTable()
        {

            DataSet ds = master.runQuery.RunCasjobs(master.exploreQuery.PlateShow);
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {

                string u = "<a class='content' target='_top' href='obj.aspx?sid=";
                string sid;

                int col = 0;
                int row = 0;
                string c = "st";
                Response.Write("<table>\n");
                Response.Write("<tr>");
                while (reader.Read())
                {
                    sid = u + RunQuery.checkNullorParse(reader.GetValue(0)) + "'>";
                    string v = "[" + reader.GetValue(1).ToString() + "]&nbsp;";
                    v += reader.GetValue(2).ToString() + " z=" + reader.GetValue(3).ToString();
                    Response.Write("<td nowrap class='" + c + "'>" + sid + v + "</a></td>\n");
                    if (++col > 3)
                    {
                        col = 0;
                        row++;
                        Response.Write("</tr>\n<tr>\n");
                        c = ("st".Equals(c) ? "sb" : "st");
                    }
                }
                Response.Write("<td></td></tr>\n</table>\n");
            } // using SqlDataReader
        }
    }
}
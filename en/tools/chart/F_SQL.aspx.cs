using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using SkyServer;

namespace SkyServer.Tools.Chart
{
    public partial class F_SQL : System.Web.UI.Page
    {
        protected string result;
        protected int count = 0;

        Globals globals;

        protected void Page_Load(object sender, EventArgs e)
        {
            globals = (Globals)Application[Globals.PROPERTY_NAME];

            string c		= Request["cmd"];
	        string url	    = Request["url"];
	        //string format  = "form";

            //c = c.Replace("'","''");

            string windows_name = System.Environment.MachineName;

	        string server_name = Request.ServerVariables["SERVER_NAME"];
	        string remote_addr = Request.ServerVariables["REMOTE_ADDR"];

	        string cmd = "EXEC spExecuteSQL @c, @row_limit, @server_name, @windows_name, @remote_addr, @access";

            string res = "";

            try
            {
                using (SqlConnection oConn = new SqlConnection(globals.ConnectionString))
                {
                    oConn.Open();
                    using (SqlCommand oCmd = oConn.CreateCommand())
                    {
                        oCmd.CommandText = cmd;
                        oCmd.Parameters.AddWithValue("@c", c);
                        oCmd.Parameters.AddWithValue("@row_limit", globals.RowLimit);
                        oCmd.Parameters.AddWithValue("@server_name", server_name);
                        oCmd.Parameters.AddWithValue("@windows_name", windows_name);
                        oCmd.Parameters.AddWithValue("@remote_addr", remote_addr);
                        oCmd.Parameters.AddWithValue("@access", globals.Access);

                        using (SqlDataReader reader = oCmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {

                                for (int index = 0; index < (reader.FieldCount); index++)
                                {
                                    res += (reader.GetName(index));
                                    res += ((index != reader.FieldCount - 1) ? "," : "\n");
                                }
                                while (reader.Read())
                                {
                                    for (int index = 0; index < (reader.FieldCount); index++)
                                    {
                                        res += reader.GetSqlValue(index).ToString();
                                        res += ((index != (reader.FieldCount - 1)) ? "," : "\n");
                                    }
                                    count++;
                                }
                            }
                        } // using SqlDataReader
                    } // using SqlCommand
                } // using SqlConnection
            }
            catch (Exception ex)
            {
		        Response.Write("ERROR\n\nSQL returned the following error message:\n" + ex.Message +"\n");
		        Response.Write("Your SQL command was:\n" + cmd+"\n");
 		        //format = "error";
            }

            result = res;
        }
    }
}
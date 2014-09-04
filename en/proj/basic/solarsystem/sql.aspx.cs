using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;

namespace SkyServer.Proj.Solarsystem
{
    public partial class sqlsearch : System.Web.UI.Page
    {
        protected string url;

        protected Globals globals;

        protected void Page_Load(object sender, EventArgs e)
        {
            globals = (Globals)Application[Globals.PROPERTY_NAME];
            cmd.Text = "select top 10 * from photoobj";
        }

        protected void process_request(object sender, EventArgs e)
        {
            Response.Write("<h1><font color='red'>" + cmd.Text + "</font></h1>");
            SqlDataSource1.ConnectionString = globals.ConnectionString;
            SqlDataSource1.SelectCommand = cmd.Text;
        }

    }
}
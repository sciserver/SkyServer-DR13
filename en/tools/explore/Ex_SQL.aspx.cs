using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SkyServer;

namespace SkyServer.Tools.Explore
{
    public partial class Ex_SQL : System.Web.UI.Page
    {
        protected string cmd;
        protected string tab;
        protected string name;
        protected string url;

        protected Globals globals;
        protected ObjectExplorer master;

        protected void Page_Load(object sender, EventArgs e)
        {
            globals = (Globals)Application[Globals.PROPERTY_NAME];
            master = (ObjectExplorer)Page.Master;

            url = master.getURL();

            string c = Request.QueryString["cmd"];
            name = Request.QueryString["name"];
            tab = Request.QueryString["tab"];
            //string format = "html";

            c = c.Replace("'", "''");

            string windows_name = System.Environment.MachineName;
            var server_name = Request.ServerVariables["SERVER_NAME"];
            var remote_addr = Request.ServerVariables["REMOTE_ADDR"];

            if ("collab".Equals(globals.Access))
                cmd = "EXEC spExecuteSQL '" + c + "  ', 500000,'" + server_name + "','" + windows_name + "','" + remote_addr + "','" + globals.Access + "'";
            else
                cmd = "EXEC spExecuteSQL '" + c + "  ', 100000,'" + server_name + "','" + windows_name + "','" + remote_addr + "','" + globals.Access + "'";
        }
    }
}
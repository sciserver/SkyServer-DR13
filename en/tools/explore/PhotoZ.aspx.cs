using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SkyServer;

namespace SkyServer.Tools.Explore
{
    public partial class PhotoZ : System.Web.UI.Page
    {
        protected string cmd1;
        protected string cmd2;
        protected Globals globals;
        protected ObjectExplorer master;
        protected string url;
        protected void Page_Load(object sender, EventArgs e)
        {
            globals = (Globals)Application[Globals.PROPERTY_NAME];
            master = (ObjectExplorer)Page.Master;
            url = master.getURL();
            
            string objid = Request.QueryString["id"];
            string c1 = "select * from Photoz where objid=" + objid;
            string c2 = "select * from Photoz2 where objid=" + objid;

            c1 = c1.Replace("'", "''");
            c2 = c2.Replace("'", "''");

            string windows_name = System.Environment.MachineName;
            string server_name = Request.ServerVariables["SERVER_NAME"];
            string remote_addr = Request.ServerVariables["REMOTE_ADDR"];
            

            if (globals.Access == "collab")
            {
                cmd1 = "EXEC spExecuteSQL '" + c1 + "  ', 500000,'" + server_name + "','" + windows_name + "','" + remote_addr + "','" + globals.Access + "'";
                cmd2 = "EXEC spExecuteSQL '" + c2 + "  ', 500000,'" + server_name + "','" + windows_name + "','" + remote_addr + "','" + globals.Access + "'";
            }
            else
            {
                cmd1 = "EXEC spExecuteSQL '" + c1 + "  ', 100000,'" + server_name + "','" + windows_name + "','" + remote_addr + "','" + globals.Access + "'";
                cmd2 = "EXEC spExecuteSQL '" + c2 + "  ', 100000,'" + server_name + "','" + windows_name + "','" + remote_addr + "','" + globals.Access + "'";
            }
        }
    }
}
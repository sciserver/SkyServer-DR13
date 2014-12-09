using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SkyServer;
using System.Data;

namespace SkyServer.Tools.Explore
{
    public partial class Neighbors : System.Web.UI.Page
    {
        protected Globals globals;
        protected ObjectExplorer master;
        protected string objId;

        protected RunQuery runQuery;
        protected DataSet ds_neighbor1, ds_neighbor2;

        protected void Page_Load(object sender, EventArgs e)
        {
            globals = (Globals)Application[Globals.PROPERTY_NAME];
            master = (ObjectExplorer)Page.Master;

            objId = Request.QueryString["id"];
            runQuery = new RunQuery();

            executeQuery();
        }

        private void executeQuery() {
            string cmd = ExplorerQueries.neighbors1.Replace("@objId", objId);
            ds_neighbor1 = runQuery.RunCasjobs(cmd);

            cmd = ExplorerQueries.neighbors2.Replace("@objId", objId);
            ds_neighbor2 = runQuery.RunCasjobs(cmd);

        }
    }
}
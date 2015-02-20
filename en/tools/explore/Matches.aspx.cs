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
    public partial class Matches : System.Web.UI.Page
    {
        protected string objId;

        protected Globals globals;
        protected ObjectExplorer master;

        protected RunQuery runQuery;
        protected DataSet ds_match1, ds_match2;

        protected void Page_Load(object sender, EventArgs e)
        {
            globals = (Globals)Application[Globals.PROPERTY_NAME];
            objId = Request.QueryString["id"];
            master = (ObjectExplorer)Page.Master;
            runQuery = new RunQuery();
            executeQueries();
        }

        private void executeQueries() {
            string cmd = ExplorerQueries.matches1.Replace("@objId", objId);
            ds_match1 = runQuery.RunCasjobs(cmd);

            cmd = ExplorerQueries.matches2.Replace("@objId", objId);
            ds_match2 = runQuery.RunCasjobs(cmd);
        }
    }
}
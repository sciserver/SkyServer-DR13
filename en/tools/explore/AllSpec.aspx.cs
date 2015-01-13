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
    public partial class AllSpec : System.Web.UI.Page
    {
        protected Globals globals;
        protected ObjectExplorer master;
        protected string objId;        
        protected RunQuery runQuery ;
        protected DataSet ds_spec1, ds_spec2;
        protected void Page_Load(object sender, EventArgs e)
        {
            globals = (Globals)Application[Globals.PROPERTY_NAME];
            objId = Request.QueryString["id"];
            master = (ObjectExplorer)Page.Master;            
            runQuery = new RunQuery();
            executeQuery();
        }

        private void executeQuery() {
            string cmd = ExplorerQueries.AllSpec1.Replace("@objId", objId);
            ds_spec1 =runQuery.RunCasjobs(cmd);

            cmd = ExplorerQueries.AllSpec2.Replace("@objId", objId);
            ds_spec2 = runQuery.RunCasjobs(cmd);
        }
    }
}
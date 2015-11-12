using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SkyServer;
using System.Data;
using SkyServer.Tools.Search;

namespace SkyServer.Tools.Explore
{
    public partial class AllSpec : System.Web.UI.Page
    {
        protected Globals globals;
        protected ObjectExplorer master;
        protected string objId;        
        protected RunQuery runQuery ;
        protected string ClientIP = "";
        protected DataSet ds_spec1, ds_spec2;
        protected void Page_Load(object sender, EventArgs e)
        {
            globals = (Globals)Application[Globals.PROPERTY_NAME];
            objId = Request.QueryString["id"];
            master = (ObjectExplorer)Page.Master;            
            runQuery = new RunQuery();
            ClientIP = runQuery.GetClientIP();
            executeQuery();
        }

        private void executeQuery() {
            string cmd = ExplorerQueries.AllSpec1.Replace("@objId", objId);
            //ds_spec1 =runQuery.RunCasjobs(cmd,"Explore: All Spec");
            //ds_spec1 = runQuery.RunDatabaseSearch(cmd, globals.ContentDataset, ClientIP, "Skyserver.Explore.Allspec.AllSpec1");

            cmd = ExplorerQueries.AllSpec2.Replace("@objId", objId);
            //ds_spec2 = runQuery.RunCasjobs(cmd,"Explore: All Spec");
            //ds_spec2 = runQuery.RunDatabaseSearch(cmd, globals.ContentDataset, ClientIP, "Skyserver.Explore.Allspec.AllSpec2");

            ResponseREST rs = new ResponseREST();

            DataSet AllSpecTables = new DataSet();

            if (Session["AllSpec"] != null)
                AllSpecTables = (DataSet)Session["AllSpec"];
            else
            {
                string URIparams = "?id=" + objId + "&query=AllSpec&TaskName=Skyserver.Explore.Allspec.AllSpec";
                AllSpecTables = rs.GetObjectInfoFromWebService(globals.ExploreWS, URIparams);
                Session["AllSpec"] = AllSpecTables;
            }
            ds_spec1 = new DataSet();
            ds_spec2 = new DataSet();
            ds_spec1.Merge(AllSpecTables.Tables["AllSpec1"]);
            ds_spec2.Merge(AllSpecTables.Tables["AllSpec2"]);

        }
    }
}
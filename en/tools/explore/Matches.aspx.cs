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
    public partial class Matches : System.Web.UI.Page
    {
        protected string objId;

        protected Globals globals;
        protected ObjectExplorer master;

        protected RunQuery runQuery;
        protected ResponseREST rs;
        protected DataSet ds_match1, ds_match2;

        protected void Page_Load(object sender, EventArgs e)
        {
            globals = (Globals)Application[Globals.PROPERTY_NAME];
            objId = Request.QueryString["id"];
            master = (ObjectExplorer)Page.Master;
            runQuery = new RunQuery();
            rs = new ResponseREST();
            executeQueries();
        }

        private void executeQueries() {

            DataSet MatchesTables = new DataSet();
            if (Session["Matches"] != null)
                MatchesTables = (DataSet)Session["Matches"];
            else
            {
                string URIparams = "?id=" + objId + "&query=Matches&TaskName=Skyserver.Explore.Matches.Matches";
                MatchesTables = rs.GetObjectInfoFromWebService(globals.ExploreWS, URIparams);
                Session["Matches"] = MatchesTables;
            }
            ds_match1 = new DataSet();
            ds_match2 = new DataSet();
            ds_match1.Merge(MatchesTables.Tables["matches1"]);
            ds_match2.Merge(MatchesTables.Tables["matches2"]);
            
            //string cmd = ExplorerQueries.matches1.Replace("@objId", objId);
            //ds_match1 = runQuery.RunCasjobs(cmd, "Explore: Matches");
            //string ClientIP = runQuery.GetClientIP();
            //ds_match1 = runQuery.RunDatabaseSearch(cmd, globals.ContentDataset, ClientIP, "Skyserver.Explore.Matches.matches1");
            //cmd = ExplorerQueries.matches2.Replace("@objId", objId);
            //ds_match2 = runQuery.RunCasjobs(cmd, "Explore: Matches");
            //ds_match2 = runQuery.RunDatabaseSearch(cmd, globals.ContentDataset, ClientIP, "Skyserver.Explore.Matches.matches2");
        }
    }
}
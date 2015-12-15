using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SkyServer.Tools.Search;
using System.Data;

namespace SkyServer.en.tools.UserHistory
{

    public partial class GetHistory : System.Web.UI.Page
    {
        protected Globals globals;

        private HttpCookie cookie;
        private string token = "";

        protected string ClientIP = "";
        protected string task = "";

        public ResponseREST rs = null;

        DataSet ResultSet;


        protected void Page_Load(object sender, EventArgs e)
        {

            if (1==1)//(Session["ResultSet"] == null)
            {
                rs = new ResponseREST();
                ClientIP = rs.GetClientIP();
                globals = (Globals)Application[Globals.PROPERTY_NAME];

                string requestURI = globals.UserHistoryWS;

                string AllParameters = rs.GetURIparameters(HttpContext.Current.Request);
                AllParameters += "&TaskName=Skyserver.tools.GetHistory&CustomMessageType=1&DoShowAllHistory=false&format=html";
                
                //ResultSet = rs.GetObjectInfoFromWebService(requestURI, AllParameters);

                //rs.ProcessRequestREST(Request, Response);//  .runQuery(requestURI, AllParameters, "", "html");
                //Session["ResultSet"] = ResultSet;
            }
            else
            {
                //ResultSet = (DataSet)Session["ResultSet"];
            }

            //Response.Write(rs.getTableHTMLresult(ResultSet));


            //ResultSet.Tables[0]









        }
    }
}
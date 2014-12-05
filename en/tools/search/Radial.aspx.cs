using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SkyServer.Tools.Search
{
    public partial class Radial : System.Web.UI.Page
    {

        //protected string whichway = "equitorial";
        // the values below have been replaced by (visualRa, visualDec)

//        protected double searchRA1 = 197.61;		// radial RA and first rect RA
//	    protected double searchDec1 = 18.44;		// radial dec and first rect dec
	    protected double searchRad = 3.0;		// radial search radius
//	    protected double searchRA2 = 15.65;		// second rect RA
//      protected double searchDec2 = 0.6 ;		// second rect dec

        protected string url;
        protected string userguide;
        protected Globals globals;
        ToolsMaster master;
        protected void Page_Load(object sender, EventArgs e)
        {
            globals = (Globals)Application[Globals.PROPERTY_NAME];
            master = (ToolsMaster)Page.Master;
            master.gselect = 5.1;

            url = ResolveClientUrl("~/en");
            userguide = url + "/help/docs/QS_UserGuide.aspx";
        }

        protected void test(object sender, EventArgs e)
        {
            Response.Redirect("X_Results.aspx");
        }
    }
}
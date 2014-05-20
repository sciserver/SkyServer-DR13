using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SkyServer
{
    public partial class Home : System.Web.UI.Page
    {
        protected int logoleftpos;
        protected string siteTitle;
        
        protected Globals globals;

        protected void Page_Load(object sender, EventArgs e)
        {
            globals = (Globals)Application[Globals.PROPERTY_NAME];

            if (globals.SiteName != globals.Release)
                siteTitle = globals.Release + " " + globals.SiteName;
            else
                siteTitle = globals.SiteName;

            if (globals.Access == "astro" || globals.Access == "collab")
            {
                logoleftpos = 730;
            }
            else
            {
                logoleftpos = 690;
            }
        }
    }
}
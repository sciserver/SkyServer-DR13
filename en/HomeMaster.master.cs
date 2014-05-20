using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace SkyServer
{
    public partial class HomeMaster : System.Web.UI.MasterPage
    {
        public string imgpath;

        protected string url;
        protected string contactUrl;
        protected string siteTitle;
        protected Globals globals;

        protected void Page_Load(object sender, EventArgs e)
        {
            globals = (Globals)Application[Globals.PROPERTY_NAME];

            url = ResolveClientUrl("~/en/");
            imgpath = ResolveClientUrl("~/en/images/");

            contactUrl = globals.ContactUrl + "&url=" + Server.UrlEncode("http://" + Request.ServerVariables["SERVER_NAME"] + Request.ServerVariables["SCRIPT_NAME"] + Request.ServerVariables["QUERY_STRING"]);

            if (globals.SiteName != globals.Release)
            {
                siteTitle = globals.Release + " " + globals.SiteName;
            }
            else
            {
                siteTitle = globals.SiteName;
            }

            siteTitle += ((globals.SiteIcon != null) ? "&nbsp;<img src=\"../" + url + "images/"+globals.SiteIcon + "\"/>" : "");
        }
    }
}
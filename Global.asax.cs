using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using SkyServer.Tools.Explore;

namespace SkyServer
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            Globals globals = new Globals();
            Application.Add(Globals.PROPERTY_NAME, globals);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            /// This can be modified to make universal portal

            HttpContext con = HttpContext.Current;
            string pageR = con.Request.Url.ToString();

            //If Explore Tool throws an error it goes to generic error page
            if(pageR.Contains("/explore/"))
                Server.Transfer("ExploreError.aspx");
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}
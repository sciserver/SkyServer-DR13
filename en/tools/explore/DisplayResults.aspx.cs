using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SkyServer;

namespace SkyServer.Tools.Explore
{
    public partial class DisplayResults : System.Web.UI.Page
    {
        protected string cmd = null;
        protected string name = null;
        protected string url = null;
        protected ObjectExplorer master;

        protected void Page_Load(object sender, EventArgs e)
        {
            master = (ObjectExplorer)Page.Master;
            cmd = Request.QueryString["cmd"];
            name = Request.QueryString["name"];
            url = Request.QueryString["url"];
        }
    }
}

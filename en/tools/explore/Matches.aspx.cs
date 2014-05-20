using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SkyServer;

namespace SkyServer.Tools.Explore
{
    public partial class Matches : System.Web.UI.Page
    {
        protected string objId;

        protected Globals globals;
        protected ObjectExplorer master;

        protected void Page_Load(object sender, EventArgs e)
        {
            globals = (Globals)Application[Globals.PROPERTY_NAME];
            objId = Request.QueryString["id"];
            master = (ObjectExplorer)Page.Master;
        }
    }
}
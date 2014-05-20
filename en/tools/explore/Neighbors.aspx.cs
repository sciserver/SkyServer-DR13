using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SkyServer;

namespace SkyServer.Tools.Explore
{
    public partial class Neighbors : System.Web.UI.Page
    {
        protected Globals globals;
        protected ObjectExplorer master;
        protected string objId;

        protected void Page_Load(object sender, EventArgs e)
        {
            globals = (Globals)Application[Globals.PROPERTY_NAME];
            master = (ObjectExplorer)Page.Master;

            objId = Request.QueryString["id"];
        }
    }
}
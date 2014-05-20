using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using SkyServer;

namespace SkyServer.Tools.Explore
{
    public partial class Plate : System.Web.UI.Page
    {
        protected long? plateId = null;
        protected Globals globals;
        protected ObjectExplorer master;

        protected void Page_Load(object sender, EventArgs e)
        {
            globals = (Globals)Application[Globals.PROPERTY_NAME];
            master = (ObjectExplorer)Page.Master;
            
            string s = Request.QueryString["plateId"];
            plateId = Utilities.ParseId(s);
            /*
            if (s != null && !"".Equals(s))
            {
                try
                {
                    if (s.StartsWith("0x"))
                        plateId = long.Parse(s.Substring(2), NumberStyles.AllowHexSpecifier);
                    else
                        plateId = long.Parse(s);
                }
                catch (Exception ex) { }
            }
            */
        }
    }
}
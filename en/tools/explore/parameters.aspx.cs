using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Globalization;
using SkyServer;

namespace SkyServer.Tools.Explore
{
    public partial class Parameters : System.Web.UI.Page
    {
        protected Globals globals;
        protected long? id = null;
        protected long? specId = null;

        //protected string link;
        protected string idstring;

        protected ObjectExplorer master;

        protected void Page_Load(object sender, EventArgs e)
        {
            globals = (Globals)Application[Globals.PROPERTY_NAME];
            master = (ObjectExplorer)Page.Master;


            string qId = HttpUtility.UrlEncode(Request.QueryString["id"]);
            string qSpecId = Request.QueryString["spec"];

            try
            {
                if (qId != null && !"".Equals(qId))
                {
                    if (qId.StartsWith("apogee"))
                    {
                        //idstring = HttpUtility.UrlEncode(qId);
                        idstring = qId;
                    }
                    else
                    {
                        // code changed by Jordan on 2013-3-28 to allow either decimal or hex input
                        id = Utilities.ParseId(qId);
                        //if (qId.StartsWith("0x")) id = Int64.Parse(qId.Substring(2), NumberStyles.AllowHexSpecifier);
                        //else id = Int64.Parse(qId);
                    }
                }
                if (qSpecId != null && !"".Equals(qSpecId))
                {
                    // code changed by Jordan on 2013-3-28 to allow either decimal or hex input
                    specId = Utilities.ParseId(qSpecId);
                    //if (qSpecId.StartsWith("0x")) specId = Int64.Parse(qSpecId.Substring(2), NumberStyles.AllowHexSpecifier);
                    //else specId = Int64.Parse(qSpecId);
                }

                //Response.Write("<h1><font color='red'>id = " + id + "</font></h1>");
//                Response.Write("<h1><font color='red'>specId = " + specId + "</font></h1>");

            }
            catch (Exception ex)
            {
                // Could not parse, so leave null
            }

        }
    }
}
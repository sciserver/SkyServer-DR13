using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SkyServer.Tools.Chart
{
    public partial class SqlToForm : System.Web.UI.Page
    {
        //protected string url;
        protected string suburl;
        protected string prturl;
        protected string query;
        protected string caller;

        ListBase master;
        protected void Page_Load(object sender, EventArgs e)
        {
            master = (ListBase)Page.Master;
            //url = master.getURL();

            suburl = "list.aspx";
            prturl = "printlist.aspx";
            query = "f_sql.aspx";
            caller = ResolveUrl("list.aspx");
        }
    }
}
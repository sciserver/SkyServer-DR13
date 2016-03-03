using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SkyServer.Exceptions;

namespace SkyServer
{
    public partial class ErrorPage : System.Web.UI.Page
    {
        public string bugreportJson = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                bugreportJson = Request.Headers.GetValues("bugreport")[0];
            }
            catch { }
        }
    }
}
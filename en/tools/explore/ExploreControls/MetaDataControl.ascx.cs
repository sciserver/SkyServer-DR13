using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SkyServer;
using System.Data;

namespace SkyServer.Tools.Explore
{
    public partial class MetaDataControl : System.Web.UI.UserControl
    {
        protected Globals globals;

        protected double ra ;
        protected double dec ;

        protected long? specObjId = null;
        protected int? clean = null;
        protected int? mode = null;
        protected string otype = null;
        protected string survey;
        protected int? imageMJD = null;

        protected ObjectExplorer master;
      
        protected void Page_Load(object sender, EventArgs e)
        {
            globals = (Globals)Application[Globals.PROPERTY_NAME];
            master = (ObjectExplorer)Page.Master;
            runQuery();
      
        }

        private void runQuery()
        {
            DataSet ds = master.runQuery.RunCasjobs(master.exploreQuery.getObjParamaters(master.objId));
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        ra = reader.GetDouble(0);
                        dec = reader.GetDouble(1);

                        if (!reader.IsDBNull(2))
                            specObjId = reader.GetInt64(2);

                        clean = reader.GetInt32(3);

                        if (!reader.IsDBNull(4))
                            survey = reader.GetString(4);

                        mode = reader.GetInt32(5);

                        otype = reader.GetString(6);

                        imageMJD = reader.GetInt32(7);
                    }
                }
            }
        }
    }
}
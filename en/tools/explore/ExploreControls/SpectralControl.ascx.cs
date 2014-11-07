using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace SkyServer.Tools.Explore
{
    public partial class SpectralControl : System.Web.UI.UserControl
    {
        protected Globals globals;
        protected ObjectExplorer master;

        protected string objId;
        protected string specId;     

        protected long plate;
        protected int mjd;
        protected int fiberid;
        protected string instrument;
        protected string objclass;
        protected string redshift_z;
        protected string redshift_err;
        protected string redshift_flags;
        protected string survey;
        protected string programname;
        protected int primary;
        protected int other_spec;
        protected string sourcetype;
        protected string vel_disp;
        protected string veldisp_err;

        protected long? specObjId;

        protected void Page_Load(object sender, EventArgs e)
        {
            globals = (Globals)Application[Globals.PROPERTY_NAME];
            master = (ObjectExplorer)Page.Master;
            try
            {
                objId = Request.QueryString["id"];
                specId = Request.QueryString["spec"];
                specObjId = Utilities.ParseId(specId);
            }
            catch(Exception exp){
                specId = null;
            }
            if(specId != null && !specId.Equals(""))
            runQuery();
        }

        private void runQuery() {
            DataSet ds = master.runQuery.RunCasjobs(master.exploreQuery.getSpectroQuery(specId,objId));
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        plate = Convert.ToInt32(reader.GetValue(0));
                        mjd = reader.GetInt32(1);
                        fiberid = reader.GetInt16(2);
                        instrument = reader.GetString(3);
                        objclass = reader.GetString(4);
                        redshift_z = reader.GetString(5);
                        redshift_err = reader.GetString(6);
                        redshift_flags = reader.GetString(7);
                        survey = reader.GetString(8);
                        programname = reader.GetString(9);
                        primary = reader.GetInt16(10);
                        other_spec = reader.GetInt32(11);
                        sourcetype = reader.GetString(12);
                        vel_disp = reader.GetString(13);
                        veldisp_err = reader.GetString(14);
                    }
                }
            }
        }
    }
}
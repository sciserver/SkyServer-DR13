using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Globalization;
using SkyServer;
using System.Data;

namespace SkyServer.Tools.Explore
{
    public partial class Summary : System.Web.UI.Page
    {
        protected const string ZERO_ID = "0x0000000000000000";

        protected Globals globals;

        protected double? ra = null;
        protected double? dec = null;

        protected double L = 0;
        protected double B = 0; // galactic coordinates, which we will convert into from ra, dec

        protected double? ApogeeRa = null;
        protected double? ApogeeDec = null;

        protected double? glon = null;
        protected double? glat = null;

        protected long? specObjId = null;
        protected string apStar;
        protected string starName;
        protected long? apLocationID;
        protected long? commissioningFlag;

        protected string plateId = null;
        protected string otype = null;

        protected int? clean = null;
        protected int? mode = null;

        protected string survey;

        protected int? imageMJD = null;

        protected long origSpecId;
        protected bool otherSpec = false;

        protected long? id = null;
        protected long? specId;

        protected string link;
        protected string apid;

        protected string cmd = null;
        protected string name = null;
        protected string url = null;

        protected ObjectExplorer master;
        protected ExplorerQueries exploreQuery;
        protected RunQuery runQuery;

        protected void Page_Load(object sender, EventArgs e)
        {
            globals = (Globals)Application[Globals.PROPERTY_NAME];
            master = (ObjectExplorer)Page.Master;
            
            //exploreQuery = new ExplorerQueries();
            //runQuery = new RunQuery();

            string qId = HttpUtility.UrlEncode(Request.QueryString["id"]);
            string qSpecId = Request.QueryString["spec"];
            string qApogeeId = HttpUtility.UrlEncode(Request.QueryString["apid"]);

            
            id = Utilities.ParseId(qId);
            specId = Utilities.ParseId(qSpecId);
            apid = ("".Equals(qApogeeId)) ? null : qApogeeId;

            //cmd  = Request.QueryString["cmd"];
            //name = Request.QueryString["name"];
            //url  = Request.QueryString["url"];
            
            //if (id.HasValue) getObjPmts(id, specId);
            //if (!String.IsNullOrEmpty(apid)) getAPOGEEparams( apid);
            //double? newra = ra;
            //double? newdec = dec;
            //L = Utilities.ra2glon(newra ?? 0, newdec ?? 0);
            //B = Utilities.dec2glat(newra ?? 0, newdec ?? 0);
            
            //// the value of 0.2 passes into the scale, so the navi tool will give 0.2 arcmin scale
            //link = "javascript:showNavi(" + ra + "," + dec + "," + 0.2 + ");";
        }
        ////getObjParamaters
        //public void getObjPmts( long? id, long? specId) 
        //{
        //    DataSet ds = runQuery.RunCasjobs(exploreQuery.getObjParamaters(id.ToString()));
        //    using (DataTableReader reader = ds.Tables[0].CreateDataReader())
        //    {
        //        if (reader.Read())
        //            {
        //                    ra = reader.GetDouble(0);
        //                    dec = reader.GetDouble(1);

        //                      if (!reader.IsDBNull(2))
        //                            specObjId = reader.GetInt64(2);

        //                      clean = reader.GetInt32(3);

        //                      if (!reader.IsDBNull(4))
        //                          survey = reader.GetString(4);

        //                      mode = reader.GetInt32(5);

        //                      otype = reader.GetString(6);

        //                      imageMJD = reader.GetInt32(7);
        //                }
        //           else { this.id = null; }
                    
        //        }
                
        //        if (specId.HasValue)
        //        {
        //            if ((specObjId.HasValue) && (specObjId != 0)) 
        //                origSpecId = specObjId.Value;
        //            ds = runQuery.RunCasjobs(exploreQuery.getSpec(specId));
        //            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
        //            {                 
        //                if (reader.Read())
        //                {
        //                    specObjId = reader.GetInt64(0);
        //                    survey = reader.GetString(1);
        //                }
        //            }
        //            if (specObjId != origSpecId)
        //                otherSpec = true;	// there are other spectra of this photoobj
        //        }
            
        //}

        //protected void getAPOGEEparams(string apogeeid)
        //{
        //     DataSet ds = runQuery.RunCasjobs(exploreQuery.getApogee(apogeeid));
        //     using (DataTableReader reader = ds.Tables[0].CreateDataReader())
        //     {                 
        //         if (reader.Read())
        //         {
        //                apStar = reader.GetString(0);
        //                ApogeeRa = reader.GetDouble(1);
        //                ApogeeDec = reader.GetDouble(2);
        //                starName = reader.GetString(3);
        //                glon = reader.GetDouble(4);
        //                glat = reader.GetDouble(5);
        //                apLocationID = reader.GetInt64(6);
        //                commissioningFlag = reader.GetInt64(7);
        //         }             
        //     }
        //}
    }
}
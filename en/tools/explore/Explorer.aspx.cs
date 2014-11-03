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
    public partial class Explorer1 : System.Web.UI.Page
    {
        //protected const string ZERO_ID = "0x0000000000000000";

        protected Globals globals;

        //protected double? ra = null;
        //protected double? dec = null;

        //protected double L = 0;
        //protected double B = 0; // galactic coordinates, which we will convert into from ra, dec

        //protected double? ApogeeRa = null;
        //protected double? ApogeeDec = null;

        //protected double? glon = null;
        //protected double? glat = null;

        //protected long? specObjId = null;
        //protected string apStar;
        //protected string starName;
        //protected long? apLocationID;
        //protected long? commissioningFlag;

        //protected string plateId = null;
        //protected string otype = null;

        //protected int? clean = null;
        //protected int? mode = null;

        //protected string survey;

        //protected int? imageMJD = null;

        //protected long origSpecId;
        //protected bool otherSpec = false;

        protected long? id = null;
        protected long? specId = null;

        //protected string link;
        protected string apid;


        protected ObjectExplorer master;

        //trying the to remove Ex_SQL
        protected string cmd  = null;
        protected string name = null;
        protected string url  = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            globals = (Globals)Application[Globals.PROPERTY_NAME];
            master = (ObjectExplorer)Page.Master;

            string qId = HttpUtility.UrlEncode(Request.QueryString["id"]);
            string qSpecId = Request.QueryString["spec"];
            string qApogeeId = HttpUtility.UrlEncode(Request.QueryString["apid"]);

            id = Utilities.ParseId(qId);
            specId = Utilities.ParseId(qSpecId);
            apid = ("".Equals(qApogeeId)) ? null : qApogeeId;

            //These parameters are used to run the  Left Panel SQL queries
            cmd = Request.QueryString["cmd"];
            name = Request.QueryString["name"];
            url = Request.QueryString["url"];

            //End of SQL Queries to run
            /*
            try
            {
                if (qId != null && !"".Equals(qId))
                {
                    // code changed by Jordan on 2013-3-28 to allow either decimal or hex input
                    if (qId.StartsWith("0x")) id = Int64.Parse(qId.Substring(2), NumberStyles.AllowHexSpecifier);
                    else id = Int64.Parse(qId);
                }
                if (qSpecId != null && !"".Equals(qSpecId))
                {
                    // code changed by Jordan on 2013-3-28 to allow either decimal or hex input
                    if (qSpecId.StartsWith("0x")) specId = Int64.Parse(qSpecId.Substring(2), NumberStyles.AllowHexSpecifier);
                    else specId = Int64.Parse(qSpecId);
                }
                if (qApogeeId != null && !"".Equals(qApogeeId))
                {
                    apid = qApogeeId;
                }
                 
            }
            catch (Exception ex)
            {
                // Could not parse, so leave null
            }
            */

            //using (SqlConnection oConn = new SqlConnection(globals.ConnectionString))
            //{
            //    oConn.Open();
            //    if (id.HasValue) getObjPmts(oConn, id, specId);
            //    if (!String.IsNullOrEmpty(apid)) getAPOGEEparams(oConn, apid);
            //    double? newra = ra;
            //    double? newdec = dec;
               // L = Utilities.ra2glon(newra ?? 0, newdec ?? 0);
               // B = Utilities.dec2glat(newra ?? 0, newdec ?? 0);
            //}

            //// the value of 0.2 passes into the scale, so the navi tool will give 0.2 arcmin scale
            //link = "javascript:showNavi(" + ra + "," + dec + "," + 0.2 + ");";
        }

        //public void getObjPmts(SqlConnection oConn, long? id, long? specId)
        //{
        //    using (SqlCommand oCmd = oConn.CreateCommand())
        //    {
        //        if (id.HasValue)
        //        {
        //            oCmd.CommandText =
        //                "select p.ra, p.dec, s.specObjId," +
        //                " p.clean, s.survey, cast(p.mode as int), dbo.fPhotoTypeN(p.type) as otype, p.mjd" +
        //                " from PhotoObjAll p LEFT OUTER JOIN SpecObjAll s ON s.bestobjid=p.objid AND s.scienceprimary=1" +
        //                " where p.objId=@id";
        //            oCmd.Parameters.AddWithValue("@id", id);

        //            using (SqlDataReader reader = oCmd.ExecuteReader())
        //            {

        //                if (reader.Read())
        //                {
        //                    ra = reader.GetDouble(0);
        //                    dec = reader.GetDouble(1);

        //                    if (!reader.IsDBNull(2))
        //                        specObjId = reader.GetInt64(2);

        //                    clean = reader.GetInt32(3);

        //                    if (!reader.IsDBNull(4))
        //                        survey = reader.GetString(4);

        //                    mode = reader.GetInt32(5);

        //                    otype = reader.GetString(6);

        //                    imageMJD = reader.GetInt32(7);
        //                }
        //                else { this.id = null; }
        //            }
        //        }

        //        //if ("".Equals(specObjId)) specObjId = "0";

        //        if (specId.HasValue)
        //        {
        //            if ((specObjId.HasValue) && (specObjId != 0))
        //                origSpecId = specObjId.Value;

        //            oCmd.Parameters.Clear();
        //            oCmd.CommandText =
        //                "select specobjid,survey from specobjall where specobjid=@specId";
        //            oCmd.Parameters.AddWithValue("@specId", specId);
        //            using (SqlDataReader reader = oCmd.ExecuteReader())
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
        //    }
        //}
    }
}
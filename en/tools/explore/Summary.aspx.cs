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

            cmd  = Request.QueryString["cmd"];
            name = Request.QueryString["name"];
            url  = Request.QueryString["url"];
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
            //    L = Utilities.ra2glon(newra ?? 0, newdec ?? 0);
            //    B = Utilities.dec2glat(newra ?? 0, newdec ?? 0);
            //}

            // the value of 0.2 passes into the scale, so the navi tool will give 0.2 arcmin scale
            link = "javascript:showNavi(" + ra + "," + dec + "," + 0.2 + ");";
        }

        public void getObjPmts(SqlConnection oConn, long? id, long? specId) 
        {
            using (SqlCommand oCmd = oConn.CreateCommand())
            {
                if (id.HasValue) {
                    oCmd.CommandText = 
                        "select p.ra, p.dec, s.specObjId," +
                        " p.clean, s.survey, cast(p.mode as int), dbo.fPhotoTypeN(p.type) as otype, p.mjd" +
                        " from PhotoObjAll p LEFT OUTER JOIN SpecObjAll s ON s.bestobjid=p.objid AND s.scienceprimary=1" + 
                        " where p.objId=@id";
                    oCmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = oCmd.ExecuteReader())
                    {

                        if (reader.Read())
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
                        else { this.id = null; }

                    }
                }
                
                //if ("".Equals(specObjId)) specObjId = "0";

                if (specId.HasValue)
                {
                    if ((specObjId.HasValue) && (specObjId != 0)) 
                        origSpecId = specObjId.Value;

                    oCmd.Parameters.Clear();
                    oCmd.CommandText =
                        "select specobjid,survey from specobjall where specobjid=@specId";
                    oCmd.Parameters.AddWithValue("@specId",specId);
                    using (SqlDataReader reader = oCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            specObjId = reader.GetInt64(0);
                            survey = reader.GetString(1);
                        }
                    }
                    if (specObjId != origSpecId)
                        otherSpec = true;	// there are other spectra of this photoobj
                }
            }
        }

        protected void getAPOGEEparams(SqlConnection oConn, string apogeeid)
        {
            using (SqlCommand oCmd = oConn.CreateCommand())
            {
                if (!String.IsNullOrEmpty(apogeeid))
                {
                    oCmd.CommandText =
                        "select apstar_id, ra, dec, apogee_id, glon, glat,location_id,commiss" +
                        " from apogeeStar" +
                        " where apstar_id=@id";
                    oCmd.Parameters.AddWithValue("@id", apogeeid);
                }

                using (SqlDataReader reader = oCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        apStar = reader.GetString(0);
                        ApogeeRa = reader.GetDouble(1);
                        ApogeeDec = reader.GetDouble(2);
                        starName = reader.GetString(3);
                        glon = reader.GetDouble(4);
                        glat = reader.GetDouble(5);
                        apLocationID = reader.GetInt64(6);
                        commissioningFlag = reader.GetInt64(7);
                    }
                }
            }
        }


        protected void showSasLink(SqlConnection oConn, string cmd) 
        {
            using (SqlCommand oCmd = oConn.CreateCommand())
            {
                oCmd.CommandText = cmd;

                using (SqlDataReader reader = oCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {

                        short plate = reader.GetInt16(0);
                        int mjd = reader.GetInt32(1);
                        short fiber = reader.GetInt16(2);

                        string spectrumlink = globals.DasUrl + "spectrumDetail?plateid=" + plate + "&mjd=" + mjd + "&fiber=" + fiber;

                        Response.Write("<p><b>\n");
                        Response.Write("<a class='content' href='");
                        Response.Write(spectrumlink);
                        Response.Write("' target='_blank'>Interactive spectrum");
                        Response.Write("<img src='../../images/new_window_black.png' alt=' (new window)' />");
                        Response.Write("</a></b></p>\n");
                    }
                }
            }
        }

        protected void showSasLinkApogee()
        {
            string doWeNeedCinApLinks = "";
            if (commissioningFlag == 1) doWeNeedCinApLinks = "C";


            string spectrumlink = globals.ApogeeSpectrumLink+"?locid=" + apLocationID + "&commiss="+commissioningFlag+"&apogeeid="+starName;
            string fitslink = globals.ApogeeFitsLink + apLocationID + "/apStar" + doWeNeedCinApLinks + "-s3-" + HttpUtility.UrlEncode(starName) + ".fits";

            Response.Write("<tr>\n");

                 Response.Write("<td class='irspeclink'>\n");
                    Response.Write("<a class='content' href='");
                    Response.Write(spectrumlink);
                    Response.Write("' target='_blank'>Interactive spectrum");
                    Response.Write("<img src='../../images/new_window_black.png' alt=' (new window)' />");
                    Response.Write("</a>\n");
                Response.Write("</td>\n");

                Response.Write("<td class='irspeclink' align='right'>\n");
                Response.Write("<a class='content' href='");
                Response.Write(fitslink);
                Response.Write("' target='_blank'>Download FITS");
                Response.Write("<img src='../../images/new_window_black.png' alt=' (new window)' />");
                Response.Write("</a>\n");
                Response.Write("</td>\n");

            Response.Write("</tr>\n");
        }


        public void showVisits(SqlConnection oConn, string cmd, int w, string tableName)
        {
            using (SqlCommand oCmd = oConn.CreateCommand())
            {
                oCmd.Parameters.Clear();
                oCmd.CommandText = cmd;

                string[] names = new string[0];

                Response.Write("<table cellpadding=2 cellspacing=2 border=0");
                if (w > 0)
                    Response.Write(" width=" + w);
                Response.Write(">\n");

                Response.Write("<tr>");

                using (SqlDataReader reader = oCmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        names = new string[reader.FieldCount];
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            names[i] = reader.GetName(i);
                            // names[0] = visit_id
                            // names[1] = plate
                            // names[2] = mjd
                            // names[3] = fiberid
                        }
                    }
                }

                for (int i = 0; i < names.Length; i++)
                {
                    string unit = master.getUnit(oConn, tableName, names[i]);

                    Response.Write("<td align='middle' class='h'>");

                    // this section works with setGlossaryKey(name,value) to print out linked glossary symbols for parameters that have glossary entries  -JR 6/6/07
                    //		key = setGlossaryKey( oRs.fields(i).name, oRs.fields(i).value );
                    //		link = "";
                    //		if( key != "" )
                    // Check whether there is a glossary key. Display a link to the glossary only if there is a key.
                    //			link = "../../help/docs/glossary.aspx?key="+key;
                    //		Response.Write("<span style='font-weight:bold'><a href='"+link+"' ");
                    Response.Write("<span ");
                    if (unit != "")
                        Response.Write("ONMOUSEOVER=\"this.T_ABOVE=true;this.T_WIDTH='100';return escape('<i>unit</i>=" + unit + "')\" ");
                    //		Response.Write( "target='help'>"+oRs.fields(i).name+"</a></span>" );
                    Response.Write(">" + names[i] + "</span>");
                    //		else {
                    //			Response.Write(oRs.fields(i).name);
                    //		}
                    Response.Write("</td>");

                }
                Response.Write("</tr>\n");

                using (SqlDataReader reader = oCmd.ExecuteReader())
                {

                    char c = 't';
                    string val;

                    while (reader.Read())
                    {
                        /* use for testing until DR10 spec tool is ready
                        string plate = "2292";
                        string mjd = "53713";
                        string fiberid = "342";
                        */
                        string plate = reader.GetValue(1).ToString();
                        string mjd = reader.GetValue(2).ToString();
                        string fiberid = reader.GetValue(3).ToString();

                        string spectrumlink = globals.ApogeeSpectrumLink + "?plateid=" + plate + "&mjd=" + mjd + "&fiber=" + fiberid;

                        Response.Write("<tr>");
                        // special case for visit_id to print the link
                        Response.Write("<td nowrap align='middle' class='" + c + "'>");
                        Response.Write("<a href='" + spectrumlink + "' class='content' target='_blank'>" + reader.GetValue(0).ToString() + " <img src='../../images/new_window_black.png' alt=' (new window)' /></a>");
                        Response.Write("</td>");
                        
                        // starting at i=1 instead  - use the special case above for visit_id (names[0])
                        for (int i = 1; i < (reader.FieldCount); i++)
                        {
                            val = reader.GetValue(i).ToString();
                            Response.Write("<td nowrap align='middle' class='" + c + "'>" + (val == "" ? "&nbsp;" : val) + "</td>");
                        }
                        Response.Write("</tr>\n");
                        c = (c == 't' ? 'b' : 't');

                    }
                    Response.Write("</table>\n");
                }
            }
        }



    }
}
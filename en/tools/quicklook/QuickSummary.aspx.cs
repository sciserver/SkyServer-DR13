using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Globalization;

namespace SkyServer.Tools.QuickLook
{
    public partial class QuickSummary : System.Web.UI.Page
    {
        protected const string ZERO_ID = "0x0000000000000000";

        protected long? objId;
        protected long? specObjId;
        
        protected long? specId;
        protected long? id = 1237654386268439267;
        protected double? ra;
        protected double? dec;

        protected string name;
        protected string otype;
        protected string naviLink;

        protected long? origSpecId;
        protected bool otherSpec = false;

        protected Globals globals;

        protected void Page_Load(object sender, EventArgs e)
        {
            globals = (Globals)Application[Globals.PROPERTY_NAME];
            

            string qId = Request.QueryString["id"];
            string qSpecId = Request.QueryString["spec"];

            try
            {
                id = Utilities.ParseId(qId);
                specId = Utilities.ParseId(qSpecId);

                /*
                if (qId != null && !"".Equals(qId))
                {
                    if (qId.StartsWith("0x"))
                        id = long.Parse(qId.Substring(2), NumberStyles.AllowHexSpecifier);
                    else
                        id = long.Parse(qId);
                }

                if (qSpecId != null && !"".Equals(qSpecId))
                {
                    if (qSpecId.StartsWith("0x"))
                        specId = long.Parse(qSpecId.Substring(2), NumberStyles.AllowHexSpecifier);
                    else
                        specId = long.Parse(qSpecId);
                }
                */

                using (SqlConnection oConn = new SqlConnection(globals.ConnectionString)) {
                    oConn.Open();
                    getObjPmts(oConn, id, specId);
                }

                name = Functions.SDSSname(ra ?? 0, dec ?? 0);
                naviLink = "javascript:showNavi(" + ra + "," + dec + ");";
            }

            catch (Exception ex)
            {
                // Could not parse, so leave null
            }
        }

        protected void getObjPmts(SqlConnection oConn, long? id, long? specId) {
            string cmd = "select ra, dec, fieldId, specObjId,";
            cmd += " objId, dbo.fPhotoTypeN(type) as otype, rowc, colc";
	        cmd		+= " from PhotoObjAll with (nolock) where objId=@id";

            using (SqlCommand oCmd = oConn.CreateCommand())
            {
                oCmd.CommandText = cmd;
                oCmd.Parameters.AddWithValue("@id", id);
                using (SqlDataReader reader = oCmd.ExecuteReader())
                {
                    
                    if (reader.Read())
                    {
                        ra = reader.GetDouble(0);
                        dec = reader.GetDouble(1);
                        //fieldId = getValue(oRs, 2);
                        specObjId = reader.GetInt64(3);
                        objId = reader.GetInt64(4);
                        otype = reader.GetString(5);
                        //rowc = getValue(oRs, 6);
                        //colc = getValue(oRs, 7);
                    }
                } // using SqlDataReader
                    if (specId.HasValue)
                    {
                        if ((specObjId != null) && (specObjId != 0))
                            origSpecId = specObjId;

                        oCmd.Parameters.Clear();
                        oCmd.CommandText =
                            "select specobjid as specobjid from specobjall where specobjid=@specId";
                        oCmd.Parameters.AddWithValue("@specId", specId);
                        using (SqlDataReader reader = oCmd.ExecuteReader())
                        {
                            if (reader.Read())
                                specObjId = reader.GetInt64(0);
                        }
                        if (specObjId != origSpecId)
                            otherSpec = true;	// there are other spectra of this photoobj
                    }
               
            } // using SqlCommand
        }

        protected void showMagsTable(SqlConnection oConn, string cmd, int width)
        {
            /*
                This function displays *only* magnitudes and errors. It works with the 'Quick Look' interface.
                Only magnitudes and errors will ever be sent to this function.
                The function includes specialized formatting to work with the magnitudes and errors.
	
                    -Jordan Raddick, 9/11/07
            */
            using (SqlCommand oCmd = oConn.CreateCommand())
            {
                oCmd.CommandText = cmd;
                using (SqlDataReader reader = oCmd.ExecuteReader())
                {

                    string u = "<a target='_top' href='obj.aspx?sid=";


                    Response.Write("<table width='" + width + "' cellpadding=2 cellspacing=2 border=0>");
                    Response.Write("<tr>");
                    Response.Write("<td align='middle' class='h' width='50%'>" + reader.GetName(0) + "</td>");	  // this line writes *only* the filter name

                    //	Response.Write("</tr>\n");

                    char c = 't';
                    while (reader.Read())
                    {
                        //		Response.Write("<tr>");
                        Response.Write("<td nowrap align='middle' class='t' width='50%'>");
                        for (int i = 0; i < (reader.FieldCount); i++)
                        {
                            string v = reader.GetSqlValue(i).ToString();

                            v = (v == "" ? "&nbsp;" : v);
                            //			if (i==0) Response.Write(u+id+"'>"+id+"</a>");
                            //			else Response.Write(v+"</td>");
                            Response.Write(v);
                            if (i == 0) Response.Write(" &plusmn; ");
                        }
                        Response.Write("</td>\n");
                        c = (c == 't' ? 'b' : 't');

                    }

                    Response.Write("</tr>\n");
                    Response.Write("</table>\n");
                } // using SqlDataReader
            } // using SqlCommand
        }

        protected bool checkFlags(SqlConnection oConn, string cmd, string objecttype)
        {
            /*
                This function checks the photo flags for an object to see if it has bad photometry.
                It returns TRUE if photometry is bad.
                It returns FALSE if photometry is good.
	
                All this information comes from the "Clean Photometry" query of the Sample SQL queries.
	
                    -Jordan Raddick, 9/17/07
            */

            // First, load up an array with flags that indicate photometry is bad.
            List<string> BadFlags = new List<string>();
            BadFlags.Add("NOPROFILE");
            BadFlags.Add("PEAKCENTER");
            BadFlags.Add("NOTCHECKED");
            BadFlags.Add("PSF_FLUX_INTERP");
            BadFlags.Add("SATURATED");
            BadFlags.Add("BAD_COUNTS_ERROR");
            BadFlags.Add("DEBLEND_NOPEAK");
            BadFlags.Add("INTERP_CENTER");
            BadFlags.Add("COSMIC_RAY");
            if (objecttype == "STAR")
            {
                BadFlags.Add("EDGE");
            }

            using (SqlCommand oCmd = oConn.CreateCommand())
            {

                oCmd.CommandText = cmd;
                using (SqlDataReader reader = oCmd.ExecuteReader())
                {
                    reader.Read();
                    string flags = reader.GetSqlValue(0).ToString();
                    //psfmagerr_g = getValue(oRs, 1);
                    //	Response.Write(flags+"<br />");

                    // First, iterate through all the bad flags - if any of them comes up true, photometry is bad.		
                    for (int i = 0; i < BadFlags.Count; i++)
                    {
                        //		Response.Write(BadFlags[i]+": "+flags.search(BadFlags[i])+"  ");
                        if (flags.Contains(BadFlags[i])) return true;
                    }

                    // Now check psfmagerr. If it's large (greater than 0.2), photometry is bad.
                    //	Response.Write("psfmagerr_g: "+psfmagerr_g);
                    //	if (psfmagerr_g >= 0.2) return true;

                    // Lastly, check to see if BINNED1 is false. If it is, then we know this has bad photometry.
                    //	Response.Write("BINNED1: "+flags.search(/BINNED1/));
                    if (!flags.Contains("BINNED1"))
                    {
                        return true;
                    }
                } // using SqlDataReader
            } // using SqlCommand
            return false;

        }
        protected void showSpecData(SqlConnection oConn, string cmd, int width)
        {
            /*
                This function displays *only* the spectral data required by the 'Quick Look' interface.
                Only class, Redshift (z), and zConf will ever be sent to this function.
                The function includes specialized formatting to work with those data.
	
                    -Jordan Raddick, 9/17/07
            */
            using (SqlCommand oCmd = oConn.CreateCommand())
            {
                oCmd.CommandText = cmd;

                using (SqlDataReader reader = oCmd.ExecuteReader())
                {

                    string u = "<a target='_top' href='obj.aspx?sid=";


                    if (!reader.HasRows)
                    {
                        Response.Write("No objects have been found<BR><P>");
                    }
                    else
                    {
                        reader.Read();

                        //Response.Write("<div id=\""+div+"\">\n");
                        //write the spectral classification

                        short plate = reader.GetInt16(2);
                        int mjd = reader.GetInt32(3);
                        short fiberid = reader.GetInt16(4);


                        var spectrumlink = globals.DasUrlBase + "spectrumDetail?plateid=" + plate + "&mjd=" + mjd + "&fiber=" + fiberid;

                        Response.Write("<h3><a href='" + spectrumlink + "' target='_blank' class='content'>Interactive spectrum<img src='../../images/new_window_black.png' alt=' (new window)' /></a></h3>");
                        Response.Write("<p><b>" + reader.GetName(0) + ":</b> " + reader.GetName(0) + "</p>");
                        Response.Write("<table cellpadding=2 cellspacing=2 border=0 width=" + width + ">");

                        //write the redshift
                        char c = 'b';
                        Response.Write("<b>&nbsp;Redshift Data:</b><br />");
                        Response.Write("<tr align=left><td width='50%' valign=top class='h'>");
                        Response.Write(reader.GetName(1));
                        Response.Write("</td><td width='50%' align='right' valign=top class='" + c + "'>");
                        string z = reader.GetSqlValue(1).ToString();
                        Response.Write(("".Equals(z) ? "&nbsp;" : z));
                        Response.Write("</td></tr>\n");
                        c = (c == 't' ? 'b' : 't');
                        Response.Write("</td>");
                        Response.Write("</tr>\n");
                        Response.Write("</table>\n");

                        //if zConf > 0.35, print a caution message

                        var csvlink = globals.DasUrlBase + "csvSpectrum?plateid=" + plate + "&mjd=" + mjd + "&fiber=" + fiberid + "&reduction2d=v5_7_0";

                        Response.Write("<a href='" + csvlink + "' target='_blank' class='content'><h3>Get spectrum as CSV</a>");

                        //Response.Write("</div>\n");
                    }
                } // using SqlDataReader

            } // using SqlCommand;
        }
    }
}
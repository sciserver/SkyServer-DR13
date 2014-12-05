using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Globalization;
using SkyServer.Tools.Explore;
using System.Data;

namespace SkyServer.Tools.QuickLook
{
    public partial class QuickSummary : System.Web.UI.Page
    {
        protected const string ZERO_ID = "0x0000000000000000";

        //protected long? objId;
        //protected long? specObjId;
        
        protected long? specId;
        //protected long? id = 1237654386268439267;
        
        protected double? ra;
        protected double? dec;

        protected string name;
        protected string otype;
        protected string naviLink;

        protected long? origSpecId;
        protected bool otherSpec = false;

        protected Globals globals;

        //const string ZERO_ID = "0x0000000000000000";

        protected long? id = null;
        protected long? sid = null;
        protected double? qra = null;
        protected double? qdec = null;

        protected int? mjd = null;
        protected short? plate = null;
        protected short? fiber = null;
        //double? ra = null;
        //double? dec = null;

        protected string objId = null;
        protected string fieldId = null;
        protected string specObjId = null;
        protected string plateId = null;

        protected short? run = null;
        protected short? rerun = null;
        protected int camcol = 99;
        protected short? field = null;

        //Globals globals;
        QuickQueries qQueries = null;
        RunQuery runQuery = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            globals = (Globals)Application[Globals.PROPERTY_NAME];

            qQueries = new QuickQueries();
            runQuery = new RunQuery();

            //string qId = Request.QueryString["id"];
            //string qSpecId = Request.QueryString["spec"];
            if (Request.QueryString.Keys.Count == 0)
                id = 1237650797291962534;

            foreach (string key in Request.QueryString.Keys)
            {
                if (key == "id")
                {
                    string s = Request.QueryString["id"];
                    if(s != null || !s.Equals(""))
                     id = Utilities.ParseId(s);                    
                }
                if (key == "sid")
                {
                    string s = Request.QueryString["sid"];
                    if (s != null || !s.Equals(""))
                        sid = Utilities.ParseId(s);                   
                }
                if (key == "ra") qra = double.Parse(Request.QueryString["ra"]); // need to parse J2000
                if (key == "dec") qdec = double.Parse(Request.QueryString["dec"]); // need to parse J2000
                if (key == "plate") plate = short.Parse(Request.QueryString["plate"]);
                if (key == "mjd") mjd = int.Parse(Request.QueryString["mjd"]);
                if (key == "fiber") fiber = short.Parse(Request.QueryString["fiber"]);
            }

            try
            {
                getObjPmts();
                //getObjPmts(oConn, id, specId);
                name = Functions.SDSSname(ra ?? 0, dec ?? 0);
                naviLink = "javascript:showNavi(" + ra + "," + dec + ");";
            }
            catch (Exception ex)
            {
                // Could not parse, so leave null
            }
        }

        private void getObjPmts()
        {
            if (fiber.HasValue && plate.HasValue) pmtsFromPlfib(plate, mjd, fiber);
            else if (qra.HasValue && qdec.HasValue) pmtsFromEq( qra, qdec);
            else if (sid.HasValue) pmtsFromSpec( sid);
            else if (id.HasValue && !sid.HasValue) pmtsFromPhoto( id);
        }

        //select p.ra, p.dec, cast(p.fieldId as binary(8)) as fieldId, cast(s.specObjId as binary(8)) as specObjId,
        //                                     cast(p.objId as binary(8)) as objId, cast(s.plateId as binary(8)) as plateId, s.mjd, s.fiberId, q.plate
        //                                     from SpecObjAll s JOIN PhotoTag p ON s.bestobjid=p.objid JOIN PlateX q ON s.plateId=q.plateId
        //                                     where s.mjd = @mjd and s.fiberId = @fiber and q.plate = @plate";
        private void pmtsFromPlfib( short? plate, int? mjd, short? fiber)
        {
            string cmd = QuickQueries.pmtsFromPlfib;
                   cmd = cmd.Replace("@mjd", mjd.ToString());
                   cmd = cmd.Replace("@fiber", fiber.ToString());
                   cmd = cmd.Replace("@plate", plate.ToString());

            DataSet ds = runQuery.RunCasjobs(cmd);
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (reader.Read())
                {
                    ra = reader["ra"] is DBNull ? -999.9 : (double)reader["ra"];
                    dec = reader["dec"] is DBNull ? -999.9 : (double)reader["dec"];
                    fieldId = reader["fieldId"] is DBNull ? "" : Functions.BytesToHex((byte[])reader["fieldId"]);
                    specObjId = reader["specObjId"] is DBNull ? "" : Functions.BytesToHex((byte[])reader["specObjId"]);
                    objId = reader["objId"] is DBNull ? "" : Functions.BytesToHex((byte[])reader["objId"]);
                    plateId = reader["plateId"] is DBNull ? "" : Functions.BytesToHex((byte[])reader["plateId"]);                    
                    //specObjId = Functions.BytesToHex(reader.GetSqlBytes(3).Buffer);
                    //objId = Functions.BytesToHex(reader.GetSqlBytes(4).Buffer);
                    //plateId = Functions.BytesToHex(reader.GetSqlBytes(5).Buffer);
                    mjd = reader["mjd"] is DBNull ? -9999 : (int)reader["mjd"];
                    fiber =  (short)reader["fiber"];
                    plate =  (short)reader["plate"];
                }
            }
        }

        //string cmd = "select ra, dec, fieldId, specObjId,";
        //    cmd += " objId, dbo.fPhotoTypeN(type) as otype, rowc, colc";
        //    cmd		+= " from PhotoObjAll with (nolock) where objId=@id";
        protected void getObjPmts(long? id, long? specId) {

            string cmd = QuickQueries.getObjPmts;
                   cmd = cmd.Replace("@id", id.ToString());

            DataSet ds = runQuery.RunCasjobs(cmd);
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (reader.Read())
                {
                    ra = reader["ra"] is DBNull ? -999.9 : (double)reader["ra"];
                    dec = reader["dec"] is DBNull ? -999.9 : (double)reader["dec"];                    
                    specId = reader["specObjId"] is DBNull ? 9999 : (long)reader["specObjId"];
                    id = reader["objId"] is DBNull ? 9999 : (long)reader["objId"];
                    otype = reader["otype"] is DBNull ? "" : (string)reader["otype"];                    
                }
            }
            
            if (specId.HasValue)
            {
                if ((specObjId != null) && (specId != 0)) origSpecId = specId;
                    cmd = QuickQueries.getSpecPmts;
                    cmd = cmd.Replace("@specId", specId.ToString());
                ds = runQuery.RunCasjobs(cmd);
                using (DataTableReader reader = ds.Tables[0].CreateDataReader())
                {
                    if (reader.Read()) specObjId = (string)reader["specobjid"];
                }
                if (specId != origSpecId)
                    otherSpec = true;	// there are other spectra of this photoobj
            }               
           
        }

        protected void showMagsTable(string cmd, int width)
        {
            DataSet ds = runQuery.RunCasjobs(cmd);
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                string u = "<a target='_top' href='obj.aspx?sid=";
                Response.Write("<table width='" + width + "' cellpadding=2 cellspacing=2 border=0>");
                Response.Write("<tr>");
                Response.Write("<td align='middle' class='h' width='50%'>" + reader.GetName(0) + "</td>");	  // this line writes *only* the filter name

                char c = 't';
                while (reader.Read())
                {
                    Response.Write("<td nowrap align='middle' class='' width='50%'>");
                    for (int i = 0; i < (reader.FieldCount); i++)
                    {
                       string v = reader[i].ToString();
                       v = (v == "" ? "&nbsp;" : v);
                       Response.Write(v);
                       if (i == 0) Response.Write(" &plusmn; ");
                    }
                    Response.Write("</td>\n");
                    //c = (c == 't' ? 'b' : 't');
                 }

                Response.Write("</tr>\n");
                Response.Write("</table>\n");                
            } // using DataReader
        }

        protected bool checkFlags(string cmd, string objecttype)
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

            DataSet ds = runQuery.RunCasjobs(cmd);
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                    reader.Read();
                    string flags = reader[0].ToString();
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
            } 
            return false;
        }

        protected void showSpecData(string cmd, int width)
        {
            /*
                This function displays *only* the spectral data required by the 'Quick Look' interface.
                Only specClass, Redshift (z), and zConf will ever be sent to this function.
                The function includes specialized formatting to work with those data.
	
                    -Jordan Raddick, 9/17/07
            */
           DataSet ds = runQuery.RunCasjobs(cmd);
           using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {

                    string u = "<a target='_top' href='obj.aspx?sid=";


                    if (!reader.HasRows)
                    {
                        Response.Write("No objects have been found<BR><P>");
                    }
                    else
                    {
                        reader.Read();

                        short plate = (short)reader["plate"];
                        int mjd = (int)reader["mjd"];
                        short fiberid = (short)reader["fiberId"];

                        var spectrumlink = "http://dr9.sdss3.org/spectrumDetail?plateid=" + plate + "&mjd=" + mjd + "&fiber=" + fiberid;

                        Response.Write("<h3><a href='" + spectrumlink + "' target='_blank' class='content'>Interactive spectrum<img src='../../images/new_window_black.png' alt=' (new window)' /></a></h3>");
                        Response.Write("<p><b>" + reader.GetName(0) + ":</b> " + reader.GetName(0) + "</p>");
                        Response.Write("<table cellpadding=2 cellspacing=2 border=0 width=" + width + ">");

                        //write the redshift
                        char c = 'b';
                        Response.Write("<b>&nbsp;Redshift Data:</b><br />");
                        Response.Write("<tr align=left><td width='50%' valign=top class='h'>");
                        Response.Write(reader.GetName(1));
                        Response.Write("</td><td width='50%' align='right' valign=top class='" + c + "'>");
                        string z = reader[1].ToString();
                        Response.Write(("".Equals(z) ? "&nbsp;" : z));
                        Response.Write("</td></tr>\n");
                        c = (c == 't' ? 'b' : 't');
                        Response.Write("</td>");
                        Response.Write("</tr>\n");
                        Response.Write("</table>\n");
                        
                        var csvlink = "http://dr9.sdss3.org/dr9-cgi/getSpectra/csv?plateid=" + plate + "&mjd=" + mjd + "&fiber=" + fiberid;
                        
                        Response.Write("<a href='" + csvlink + "' target='_blank' class='content'><h3>Get spectrum as CSV</a>");

                }
            } 
        }

        private void pmtsFromEq( double? qra, double? qdec)
        {
            string cmd = QuickQueries.pmtsFromEq;
            cmd = cmd.Replace("@qra", qra.ToString());
            cmd = cmd.Replace("@qdec", qdec.ToString());
            cmd = cmd.Replace("@eqSearchRadius", globals.EqSearchRadius.ToString());
            DataSet ds = runQuery.RunCasjobs(cmd);
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {   
                    if (reader.Read())
                    {
                        ra = reader.GetDouble(0);
                        dec = reader.GetDouble(1);

                        run = reader.GetInt16(2);
                        rerun = reader.GetInt16(3);
                        camcol = reader.GetByte(4);
                        field = reader.GetInt16(5);

                        fieldId = reader["fieldId"] is DBNull ? "" : Functions.BytesToHex((byte[])reader["fieldId"]);
                        specObjId = reader["specObjId"] is DBNull ? "" : Functions.BytesToHex((byte[])reader["specObjId"]);
                        objId = reader["objId"] is DBNull ? "" : Functions.BytesToHex((byte[])reader["objId"]);
                    }
            }

                // get the plateId from the specObj, if it exists

                if (specObjId != null && !ZERO_ID.Equals(specObjId))
                {
                    cmd = QuickQueries.pmtsFromeq2;
                    cmd = cmd.Replace("@specObjId", Int64.Parse(specObjId.Substring(2), NumberStyles.AllowHexSpecifier).ToString());
                    
                    ds = runQuery.RunCasjobs(cmd);
                    using (DataTableReader reader = ds.Tables[0].CreateDataReader())
                    {   
                        if (reader.Read())
                        {
                            plateId = Functions.BytesToHex((byte[])reader[0]);
                            mjd = (int)reader[1];
                            fiber = (short)reader[2];
                            plate = (short)reader[3];
                        }
                    } 
              }
             
        }


        private void pmtsFromSpec(long? sid)
        {
             string cmd = QuickQueries.pmtsFromSpec;
                    cmd = cmd.Replace("@sid", sid.ToString()); 
             DataSet ds = runQuery.RunCasjobs(cmd);
             using (DataTableReader reader = ds.Tables[0].CreateDataReader())
             {   
                    if (reader.Read())
                    {
                        ra = reader["ra"] is DBNull ? -999.9 : (double)reader["ra"];
                        dec = reader["dec"] is DBNull ? -999.9 : (double)reader["dec"];                        
                        fieldId = reader["fieldId"] is DBNull ? "" : Functions.BytesToHex((byte[])reader["fieldId"]);
                        specObjId = reader["specObjId"] is DBNull ? "" : Functions.BytesToHex((byte[])reader["specObjId"]);
                        objId = reader["objId"] is DBNull ? "" : Functions.BytesToHex((byte[])reader["objId"]);
                        plateId = reader["plateId"] is DBNull ? "" : Functions.BytesToHex((byte[])reader["plateId"]);
                        mjd = reader["mjd"] is DBNull ? -9999 : (int)reader["mjd"];
                        fiber = (short)reader["fiber"];
                        plate = (short)reader["plate"];
                    }
             }             
        }


        private void pmtsFromPhoto( long? id)
        {
            string cmd = QuickQueries.pmtsFromPhoto;            
                   cmd = cmd.Replace("@id", id.ToString());

            DataSet ds = runQuery.RunCasjobs(cmd);
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {                   
                if (reader.Read())
                {
                    ra = reader["ra"] is DBNull ? -999.9 : (double)reader["ra"];
                    dec = reader["dec"] is DBNull ? -999.9 : (double)reader["dec"];
                    run = reader["run"] is DBNull ? -999 : (short?)reader["run"];
                    rerun = reader["rerun"] is DBNull ? -999 : (short?)reader["rerun"];
                    camcol = reader["camcol"] is DBNull ? -999 : (byte)reader["camcol"];
                    field = reader["field"] is DBNull ? -999 : (short?)reader["field"];
                    fieldId = reader["fieldId"] is DBNull ? "" : Functions.BytesToHex((byte[])reader["fieldId"]);
                    specObjId = reader["specObjId"] is DBNull ? "" : Functions.BytesToHex((byte[])reader["specObjId"]);
                    specId = Utilities.ParseId(specObjId);
                    objId = reader["objId"] is DBNull ? "" : Functions.BytesToHex((byte[])reader["objId"]);
                }
            }

            // get the plateId and fiberId from the specObj, if it exists

            if (specObjId != null && !ZERO_ID.Equals(specObjId))
            {
                cmd = QuickQueries.pmtsFromPhoto2;        
                cmd = cmd.Replace("@specObjId", long.Parse(specObjId.Substring(2), NumberStyles.AllowHexSpecifier).ToString());

                ds = runQuery.RunCasjobs(cmd);
                using (DataTableReader reader = ds.Tables[0].CreateDataReader())
                {
                    if (reader.Read())
                    {
                        plateId = reader["plateId"] is DBNull ? "" : Functions.BytesToHex((byte[])reader["plateId"]);
                        mjd = reader["mjd"] is DBNull ? -9999 : (int)reader["mjd"];
                        //fiber = (short)reader["fiber"];
                        plate = (short)reader["plate"];
                    }
                } 
            }           
        }
    }
}
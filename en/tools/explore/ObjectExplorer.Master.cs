using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Globalization;
using SkyServer;
using SkyServer.Tools.Explore;
using System.Collections.Specialized;

namespace SkyServer.Tools.Explore
{
    public partial class ObjectExplorer : System.Web.UI.MasterPage
    {
        protected const string ZERO_ID = "0x0000000000000000";

        protected HRefs hrefs = new HRefs();

        protected long? id = null;
        protected string apid;
        protected long? specId = null;

        protected int tabwidth = 128;
        
        protected string url;
        protected string enUrl;

        double? ra;
        double? dec;
        string objId;
        string fieldId;
        string specObjId;
        string plateId;
        int? mjdNum;
        short? fiberId;
        int? run;
        short? rerun;
        short? camcol;
        short? field;
        protected int? plate = null;

        protected Globals globals;


        protected void Page_Load(object sender, EventArgs e)
        {
            
            globals = (Globals)Application[Globals.PROPERTY_NAME];
            url = getURL();
            enUrl = getEnURL();

            string qId = HttpUtility.UrlEncode(Request.QueryString["id"]);
            string qSpecId = Request.QueryString["spec"];
            string qApogeeId = HttpUtility.UrlEncode(Request.QueryString["apid"]);

            id = Utilities.ParseId(qId);
            specId = Utilities.ParseId(qSpecId);
            apid = ("".Equals(qApogeeId))?null:qApogeeId;

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
                if (qApogeeId != null && !"".Equals(qSpecId))
                {
                    apid = qApogeeId;
                }
            }
            catch (Exception)
            {
                // Could not parse, so leave null
            }
            */
                using (SqlConnection oConn = new SqlConnection(globals.ConnectionString))
                {
                    oConn.Open();
                    if (id.HasValue) pmtsFromPhoto(oConn, id, specId);
                }

                // id is the decimal representation; objId is the hex representation.

//                if (id.HasValue) hrefs.Summary = "summary.aspx?id=" + id + "&spec=" + specId;
//                if (!String.IsNullOrEmpty(apid)) hrefs.Summary = "summary.aspx?apid=" + apid;
                hrefs.Summary = "summary.aspx?id=" + id + "&spec=" + specId + "&apid=" + apid;
                hrefs.PhotoObj = "ex_sql.aspx?cmd=select+*+from+PhotoObjAll+where+objId=" + objId + "&name=PhotoObj&tab=V&id="+id + "&spec=" + specId + "&apid=" + apid;
                hrefs.PhotoTag = "ex_sql.aspx?cmd=select+*+from+PhotoTag+where+objId=" + objId + "&name=PhotoTag&tab=V&id=" + id + "&spec=" + specId + "&apid=" + apid;
                hrefs.Matches = "matches.aspx?id=" + id + "&spec=" + specId + "&apid=" + apid;
                hrefs.Field = "ex_sql.aspx?cmd=select+*+from+Field+where+fieldId=" + fieldId + "&name=Field&tab=V&id=" + id + "&spec=" + specId + "&apid=" + apid;
                if (globals.ReleaseNumber >= 8)
                    hrefs.Galaxyzoo = "galaxyzoo.aspx?id=" + id + "&spec=" + specId + "&apid=" + apid;
                if (globals.ReleaseNumber > 4)
                {
                    hrefs.PhotoZ = "photoz.aspx?id=" + id + "&spec=" + specId + "&apid=" + apid;
                    hrefs.PhotozRF = "photozRF.aspx?id=" + id + "&spec=" + specId + "&apid=" + apid;
                }
                hrefs.Frame = "ex_sql.aspx?cmd=select+*+from+Frame+where+fieldId=" + fieldId + "&name=Frame&tab=V&id=" + id + "&spec=" + specId + "&apid=" + apid;
                hrefs.Neighbors = "neighbors.aspx?id=" + id + "&spec=" + specId + "&apid=" + apid;
                hrefs.Chart = "javascript:gotochart(" + ra + "," + dec + ");";
                hrefs.Navigate = "javascript:gotonavi(" + ra + "," + dec + ");";
                hrefs.SaveBook = "javascript:saveBook(\"" + objId + "\");";
                hrefs.ShowBook = "javascript:showNotes();";
                //if (globals.DBType == "STRIPE")
                if (globals.Database.StartsWith("STRIPE"))
                {
                    if (run == 106)
                        run = 100006;
                    if (run == 206)
                        run = 200006;
                }

                // if( dasUrlBase.length > 1 )
                //	hrefs.FITS = dasUrlBase+"fields.html?search=runcamcolfield&run="+run+"&camcol="+camcol+"&field="+field+"&submit=Submit";
                //	getImgFitsUrl( oCmd, fieldid );
                // else
                hrefs.FITS = "fitsimg.aspx?id=" + id + "&fieldId=" + fieldId + "&spec=" + specId + "&apid=" + apid;
                //hrefs.FITS = globals.DasUrl + "fields/raDec?ra=" + ra + "&dec=" + dec + "&submit=Submit";

                hrefs.NED = "http://nedwww.ipac.caltech.edu/cgi-bin/nph-objsearch?search_type=Near+Position+Search"
                                + "&in_csys=Equatorial&in_equinox=J2000.0&obj_sort=Distance+to+search+center"
                                + "&lon=" + (ra.HasValue?(Math.Round((double)ra, 7).ToString()+"d"):"") + "&lat=" + (dec.HasValue?(Math.Round((double)dec, 7).ToString()+"d"):"") + "&radius=1.0";

                string hmsRA;
                hmsRA = Functions.hmsPad(ra ?? 0).Replace(" ", "+");

                string dmsDec;
                if (dec >= 0)
                    dmsDec = Functions.dmsPad(dec ?? 0).Replace("+", "%2B");
                else
                    dmsDec = Functions.dmsPad(dec ?? 0);

                dmsDec = dmsDec.Replace(" ", "+");
                hrefs.SIMBAD = "http://simbad.u-strasbg.fr/sim-id.pl?protocol=html&Ident=" + hmsRA + "+" + dmsDec + "&NbIdent=1"
                                    + "&Radius=1.0&Radius.unit=arcmin&CooFrame=FK5&CooEpoch=2000&CooEqui=2000"
                                    + "&output.max=all&o.catall=on&output.mesdisp=N&Bibyear1=1983&Bibyear2=2005"
                                    + "&Frame1=FK5&Frame2=FK4&Frame3=G&Equi1=2000.0&Equi2=1950.0&Equi3=2000.0"
                                    + "&Epoch1=2000.0&Epoch2=1950.0&Epoch3=2000.0";
                hrefs.ADS = "http://adsabs.harvard.edu/cgi-bin/nph-abs_connect?db_key=AST&sim_query=YES&aut_xct=NO&aut_logic=OR"
                                    + "&obj_logic=OR&author=&object=" + hmsRA + "+" + dmsDec + "&start_mon=&start_year=&end_mon="
                                    + "&end_year=&ttl_logic=OR&title=&txt_logic=OR&text=&nr_to_return=100&start_nr=1"
                                    + "&start_entry_day=&start_entry_mon=&start_entry_year=&min_score=&jou_pick=ALL"
                                    + "&ref_stems=&data_and=ALL&group_and=ALL&sort=SCORE&aut_syn=YES&ttl_syn=YES"
                                    + "&txt_syn=YES&aut_wt=1.0&obj_wt=1.0&ttl_wt=0.3&txt_wt=3.0&aut_wgt=YES&obj_wgt=YES"
                                    + "&ttl_wgt=YES&txt_wgt=YES&ttl_sco=YES&txt_sco=YES&version=1";
                hrefs.Print = "framePrint();";
                hrefs.AllSpec = "allSpec.aspx?id=" + id + "&spec=" + specId + "&apid=" + apid;

//                if (specObjId != null && !ZERO_ID.Equals(specObjId))
                if (specId != null)
                {
                    hrefs.SpecObj = "ex_sql.aspx?cmd=select+*+from+SpecObjAll+where+specObjId=" + specId + "&name=SpecObj&tab=V&id=" + id + "&spec=" + specId + "&apid=" + apid;
                    hrefs.sppLines = "ex_sql.aspx?cmd=select+*+from+sppLines+where+specObjId=" + specId + "&name=sppLines&tab=V&id=" + id + "&spec=" + specId + "&apid=" + apid;
                    hrefs.sppParams = "ex_sql.aspx?cmd=select+*+from+sppParams+where+specObjId=" + specId + "&name=sppParams&tab=V&id=" + id + "&spec=" + specId + "&apid=" + apid;
                    hrefs.galSpecLine = "ex_sql.aspx?cmd=select+*+from+galSpecLine+where+specObjId=" + specId + "&name=galSpecLine&tab=V&id=" + id + "&spec=" + specId + "&apid=" + apid;
                    hrefs.galSpecIndx = "ex_sql.aspx?cmd=select+*+from+galSpecIndx+where+specObjId=" + specId + "&name=galSpecIndx&tab=V&id=" + id + "&spec=" + specId + "&apid=" + apid;
                    hrefs.galSpecInfo = "ex_sql.aspx?cmd=select+*+from+galSpecInfo+where+specObjId=" + specId + "&name=galSpecInfo&tab=V&id=" + id + "&spec=" + specId + "&apid=" + apid;
                    hrefs.Spectrum = "../../get/SpecById.ashx?ID=" + specId;
                    hrefs.Plate = "plate.aspx?id=" + id + "&plateId=" + plateId + "&spec=" + specId + "&apid=" + apid;
                    //  if( dasUrlBase.length > 1 )
                    //      hrefs.SpecFITS	= dasUrlBase+"getSpectra.html?plateid="+plate+"&mjd="+mjdNum+"&fiber="+fiberId+"&submit_form=Submit";
                    //	getSpecFitsUrl( oCmd, specObjId );
                    //  else
                    hrefs.SpecFITS = "fitsspec.aspx?&sid=" + specObjId + "&id=" + id + "&spec=" + specId + "&apid=" + apid;
                    
                    if (globals.ReleaseNumber >= 8)
                    {
                        hrefs.theParameters = "parameters.aspx?id=" + id + "&spec=" + specId + "&apid=" + apid;
                        hrefs.stellarMassStarformingPort = "ex_sql.aspx?cmd=select+*+from+stellarMassStarformingPort+where+specObjId=" + specId + "&name=stellarMassStarFormingPort&tab=V&id=" + id + "&spec=" + specId + "&apid=" + apid;
                        hrefs.stellarMassPassivePort = "ex_sql.aspx?cmd=select+*+from+stellarMassPassivePort+where+specObjId=" + specId + "&name=stellarMassPassivePort&tab=V&id=" + id + "&spec=" + specId + "&apid=" + apid;
                        hrefs.emissionLinesPort = "ex_sql.aspx?cmd=select+*+from+emissionLinesPort+where+specObjId=" + specId + "&name=emissionlinesPort&tab=V&id=" + id + "&spec=" + specId + "&apid=" + apid;
                        hrefs.stellarMassPCAWiscBC03 = "ex_sql.aspx?cmd=select+*+from+stellarMassPCAWiscBC03+where+specObjId=" + specId + "&name=stellarMassPCAWiscBC03&tab=V&id=" + id + "&spec=" + specId + "&apid=" + apid;
                        hrefs.stellarMassPCAWiscM11 = "ex_sql.aspx?cmd=select+*+from+stellarMassPCAWiscM11+where+specObjId=" + specId + "&name=stellarMassPCAWiscM11&tab=V&id=" + id + "&spec=" + specId + "&apid=" + apid;
                    }
                    if (globals.ReleaseNumber >= 10)
                    {
                        hrefs.stellarMassFSPSGranEarlyDust = "ex_sql.aspx?cmd=select+*+from+stellarMassFSPSGranEarlyDust+where+specObjId=" + specId + "&name=stellarMassFSPSGranEarlyDust&tab=V&id=" + id + "&spec=" + specId + "&apid=" + apid;
                        hrefs.stellarMassFSPSGranEarlyNoDust = "ex_sql.aspx?cmd=select+*+from+stellarMassFSPSGranEarlyNoDust+where+specObjId=" + specId + "&name=stellarMassFSPSGranEarlyNoDust&tab=V&id=" + id + "&spec=" + specId + "&apid=" + apid;
                        hrefs.stellarMassFSPSGranEarlyDust = "ex_sql.aspx?cmd=select+*+from+stellarMassFSPSGranWideDust+where+specObjId=" + specId + "&name=stellarMassFSPSGranWideDust&tab=V&id=" + id + "&spec=" + specId + "&apid=" + apid;
                        hrefs.stellarMassFSPSGranWideNoDust = "ex_sql.aspx?cmd=select+*+from+stellarMassFSPSGranWideDust+where+specObjId=" + specId + "&name=stellarMassFSPSGranWideNoDust&tab=V&id=" + id + "&spec=" + specId + "&apid=" + apid;
                    }
                }
                else
                {
                    hrefs.SpecObj = "none.htm";
                    hrefs.sppLines = "none.htm";
                    hrefs.sppParams = "none.htm";
                    hrefs.galSpecLine = "none.htm";
                    hrefs.galSpecIndx = "none.htm";
                    hrefs.galSpecInfo = "none.htm";
                    hrefs.emissionLinesPort = "none.htm";
                    hrefs.Spectrum = "none.htm";
                    hrefs.Plate = "none.htm";
                    hrefs.SpecFITS = "none.htm";
                    hrefs.theParameters = "none.htm";
                    hrefs.stellarMassStarformingPort = "none.htm";
                    hrefs.stellarMassPassivePort = "none.htm";
                    hrefs.stellarMassPCAWiscBC03 = "none.htm";
                    hrefs.stellarMassPCAWiscM11 = "none.htm";
                    hrefs.stellarMassFSPSGranEarlyDust = "none.htm";
                    hrefs.stellarMassFSPSGranEarlyNoDust = "none.htm";
                    hrefs.stellarMassFSPSGranEarlyDust = "none.htm";
                    hrefs.stellarMassFSPSGranWideNoDust = "none.htm";
                }

                if (!String.IsNullOrEmpty(apid))
                {
                    hrefs.apogeeStar = "ex_sql.aspx?cmd=select+*+from+apogeeStar+where+apstar_id=" + HttpUtility.UrlEncode("'" + apid + "'") + "&name=apogeeStar&tab=V&id=" + id + "&spec=" + specId + "&apid=" + apid;
                    //                    hrefs.apogeeVisit = "ex_sql.aspx?cmd=select+v.*+from+apogeeVisit+v+join+apogeeStar+s+on+v.star=s.star+where+s.apstar_id=" + HttpUtility.UrlEncode("'" + apid + "'") + "&name=apogeeVisit&tab=V&id=" + HttpUtility.UrlEncode("'" + apid + "'");
                    //                    hrefs.apogeePlate = "ex_sql.aspx?cmd=select+*+from+apogeePlate+where+apstar_id=" + HttpUtility.UrlEncode("'" + apid + "'") + "&name=apogeePlate&tab=V&id=" + HttpUtility.UrlEncode("'" + apid + "'");
                    hrefs.aspcapStar = "ex_sql.aspx?cmd=select+*+from+aspcapStar+where+apstar_id=" + HttpUtility.UrlEncode("'" + apid + "'") + "&name=aspcapStar&tab=V&id=" + id + "&spec=" + specId + "&apid=" + apid;
                }
        }



        public void pmtsFromPhoto(SqlConnection oConn, long? id, long? specId) 
        {
            using (SqlCommand oCmd = oConn.CreateCommand())
            {
                oCmd.CommandText =
                    " select p.ra, p.dec, p.run, p.rerun, p.camcol, p.field," +
                    " cast(p.fieldId as binary(8)) as fieldId," +
                    " cast(s.specobjid as binary(8)) as specObjId," +
                    " cast(p.objId as binary(8)) as objId " +
                    " from PhotoTag p " +
                    " left outer join SpecObjAll s ON s.bestobjid=p.objid " +
                    " where p.objId=dbo.fObjId(@id) ";

                oCmd.Parameters.AddWithValue("@id", id);
                using (SqlDataReader reader = oCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ra = reader.GetDouble(0);
                        dec = reader.GetDouble(1);
                        run = reader.GetInt16(2);
                        rerun = reader.GetInt16(3);
                        camcol = reader.GetByte(4);
                        field = reader.GetInt16(5);
                        fieldId = Functions.BytesToHex(reader.GetSqlBytes(6).Buffer);
                        specObjId = Functions.BytesToHex(reader.GetSqlBytes(7).Buffer);
                        objId = Functions.BytesToHex(reader.GetSqlBytes(8).Buffer);
                    }
                }
                //Response.Write("<h1><font color='purple'>" + objId + "</font></h1>");

                // get the plateId and fiberId from the specObj, if it exists

//                if (specObjId != null && !ZERO_ID.Equals(specObjId))
                if (specId == null)
                {
                    specId = Utilities.ParseId(specObjId);
                    //if (specObjId.StartsWith("0x")) specId = Int64.Parse(specObjId.Substring(2), NumberStyles.AllowHexSpecifier);
                    //else specId = Int64.Parse(specObjId);
                } 

                if (specId != null)
                {
                    oCmd.Parameters.Clear();
                    oCmd.CommandText = 
                        " select cast(s.plateId as binary(8)) as plateId, s.mjd, s.fiberId, q.plate" +
                        " from SpecObjAll s JOIN PlateX q ON s.plateId=q.plateId where specObjId=@specId"; //specObjId";
//                    oCmd.Parameters.AddWithValue("@specObjId", long.Parse(specObjId.Substring(2), NumberStyles.AllowHexSpecifier));
                    oCmd.Parameters.AddWithValue("@specId", specId);
                    using (SqlDataReader reader = oCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                                plateId = Functions.BytesToHex(reader.GetSqlBytes(0).Buffer);
                                mjdNum = reader.GetInt32(1);
                                fiberId = reader.GetInt16(2);
                                plate = reader.GetInt16(3);
                        }
                    }
                }
            }
        }

        public string getURL()
        {
            string host = Request.ServerVariables["SERVER_NAME"];
            string path = Request.ServerVariables["SCRIPT_NAME"];

            string root = "http://" + host;
            string s = path;
            string[] q = s.Split('/');

            string lang = "";
            for (int i = 0; i < q.Length; i++)
            {
                if (i > 0) root += "/";
                root += q[i];
                lang = q[i];
                if (lang == "en" || lang == "de" || lang == "jp"
                  || lang == "hu" || lang == "sp" || lang == "ce" || lang == "pt" || lang == "zh" || lang == "uk" || lang == "ru")
                {
                    //depth = q.length - i - 2;
                    return root;
                }
            }
            return root;
        }

        public string getEnURL()
        {
            string host = Request.ServerVariables["SERVER_NAME"];
            string path = Request.ServerVariables["SCRIPT_NAME"];

            string root = "http://" + host;
            string s = path;
            string[] q = s.Split('/');

            string lang = "";
            for (int i = 0; i < q.Length; i++)
            {
                if (i > 0) root += "/";
                lang = q[i];
                if (lang == "en" || lang == "de" || lang == "jp"
                  || lang == "hu" || lang == "sp" || lang == "ce" || lang == "pt" || lang == "zh" || lang == "uk" || lang == "ru")
                {
                    //depth = q.Length - i - 2;
                    root += "en";
                    return root;
                }
                else
                {
                    root += q[i];
                }
            }
            return root;
        }

        // ***** Functions *****

        public void showNTable(SqlConnection oConn, string cmd) {
            using (SqlCommand oCmd = oConn.CreateCommand())
            {
                oCmd.CommandText = cmd;
                using (SqlDataReader reader = oCmd.ExecuteReader())
                {
                    string u = "<a class='content' target='_top' href='obj.aspx?id=";
                    string id, v;
                    char c;

                    Response.Write("<table cellpadding=2 cellspacing=2 border=0>");

                    if (reader.HasRows)
                    {
                        Response.Write("<tr>");
                        for (int i = 0; i < reader.FieldCount; i++)
                            Response.Write("<td align='middle' class='h'>" + reader.GetName(i) + "</td>");
                        Response.Write("</tr>\n");
                    }

                    c = 't';
                    while (reader.Read())
                    {
                        Response.Write("<tr>");
                        id = reader.GetValue(0).ToString();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            v = reader.GetValue(i).ToString();
                            v = (v == "" ? "&nbsp;" : v);
                            Response.Write("<td nowrap align='middle' class='" + c + "'>");
                            if (i == 0) Response.Write(u + id + "'>" + id + "</a></td>");
                            else Response.Write(v + "</td>");
                        }
                        Response.Write("</tr>\n");
                        c = (c == 't' ? 'b' : 't');
                    }
                    Response.Write("</table>\n");
                } // using SqlDataReader
            } // using SqlCommand
        }

        public void showSTable(SqlConnection oConn, string cmd) 
        {
            using (SqlCommand oCmd = oConn.CreateCommand())
            {
                oCmd.CommandText = cmd;

                string u = "<a class='content' target='_top' href='obj.aspx?sid=";
                string id, v;
                char c;

                Response.Write("<table cellpadding=2 cellspacing=2 border=0>");

                Response.Write("<tr>");
                using (SqlDataReader reader = oCmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                            Response.Write("<td align='middle' class='h'>" + reader.GetName(i) + "</td>");
                    }
                    Response.Write("</tr>\n");

                    c = 't';

                    while (reader.Read())
                    {
                        Response.Write("<tr>");
                        id = reader.GetValue(0).ToString();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            v = reader.GetValue(i).ToString();
                            v = (v == "" ? "&nbsp;" : v);
                            Response.Write("<td nowrap align='middle' class='" + c + "'>");
                            if (i == 0) Response.Write(u + id + "'>" + id + "</a></td>");
                            else Response.Write(v + "</td>");
                        }
                        Response.Write("</tr>\n");
                        c = (c == 't' ? 'b' : 't');
                    }
                    Response.Write("</table>\n");
                } //using SqlDataReader
            } // using SqlCommand
        }

        public void showFTable(SqlConnection oConn, long? plateId) {
            using (SqlCommand oCmd = oConn.CreateCommand()) {
	            string cmd = " select cast(specObjID as binary(8)) as specObjId," +
	                " fiberId, class as name, str(z,5,3) as z" +
	                " from SpecObjAll where plateID=@plateId order by fiberId";

	            oCmd.CommandText = cmd;
                oCmd.Parameters.AddWithValue("@plateId", plateId);

                using (SqlDataReader reader = oCmd.ExecuteReader()) {

	                string u = "<a class='content' target='_top' href='obj.aspx?sid=";
	                string sid;
	
	                int col=0;
	                int row=0;
	                string c = "st";
	                Response.Write("<table>\n");
	                Response.Write("<tr>");
	                while(reader.Read()) {
		                sid   = u+Functions.BytesToHex(reader.GetSqlBytes(0).Buffer)+"'>";
		                string v = "["+reader.GetValue(1).ToString()+"]&nbsp;";
		                v += reader.GetValue(2).ToString()+" z="+reader.GetValue(3).ToString();
		                Response.Write("<td nowrap class='"+c+"'>"+sid+v+"</a></td>\n");
		                if (++col>3) {
			                col = 0;
			                row++;
			                Response.Write("</tr>\n<tr>\n");
			                c = ("st".Equals(c)?"sb":"st");
		                }
	                }
	                Response.Write("<td></td></tr>\n</table>\n");
                } // using SqlDataReader
            } // using SqlCommand
        }

        public string getUnit(SqlConnection oConn, string tableName, string name)
        {
            using (SqlCommand oCmd = oConn.CreateCommand())
            {
                oCmd.CommandText = "select unit from DBcolumns where tablename=@tablename and [name]=@name";
                oCmd.Parameters.AddWithValue("@tablename", tableName);
                oCmd.Parameters.AddWithValue("@name", name);

                using (SqlDataReader reader = oCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetString(0);
                    }
                }
            }

            return "";
        }

       
        /// <summary>
        /// Added new HTable with namevalue pair options
        /// </summary>
        /// <param name="namevalues"></param>
        /// <param name="w"></param>
        public void showHTable(NameValueCollection namevalues, int w)
        {
            char c = 't'; string unit = "test";
            
            Response.Write("<table cellpadding=2 cellspacing=2 border=0");
            
            if (w > 0)
                Response.Write(" width=" + w);
            Response.Write(">\n");
            
                Response.Write("<tr>");

                foreach (String k in namevalues.AllKeys)
                {
                    Response.Write("<td align='middle' class='h'>");
                    Response.Write("<span ");
                    if (unit != "")
                        Response.Write("ONMOUSEOVER=\"this.T_ABOVE=true;this.T_WIDTH='100';return escape('<i>unit</i>=" + unit + "')\" ");
                    Response.Write("></span>");
                    Response.Write(k+"</td>");                                      
                }
                Response.Write("</tr>");
                
                Response.Write("<tr>");

                foreach (String k in namevalues.AllKeys)
                {
                    Response.Write("<td nowrap align='middle' class='" + c + "'>");
                    Response.Write(namevalues[k]);
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
            
            Response.Write("</table>");
        }
        
        /// <summary>
        /// Vertical aligned table  With name value pair
        /// </summary>
        /// <param name="namevalues"></param>
        /// <param name="w"></param>
        public void showVTable(NameValueCollection namevalues, int w)
        {
            char c = 't'; string unit = "test";
            Response.Write("<table cellpadding=2 cellspacing=2 border=0");
            if (w > 0)
                Response.Write(" width=" + w);
            Response.Write(">\n");
            foreach (String k in namevalues.AllKeys)
            {
                Response.Write("<tr align='left' >");
                Response.Write("<td  valign='top' class='h'>");
                Response.Write("<span ");
                if (unit != "")
                    Response.Write("ONMOUSEOVER=\"this.T_ABOVE=true;this.T_WIDTH='100';return escape('<i>unit</i>=" + unit + "')\" ");
                Response.Write("></span>");
                Response.Write(k + "</td>");
            
                Response.Write("<td valign='top' class='" + c + "'>");
                Response.Write(namevalues[k]);
                Response.Write("</td>");
                Response.Write("</tr>");
            }
            Response.Write("</table>");
        }
    }
}
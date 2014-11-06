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
using System.Data;

namespace SkyServer.Tools.Explore
{
    public partial class ObjectExplorer : System.Web.UI.MasterPage
    {
        protected const string ZERO_ID = "0x0000000000000000";

        protected HRefs hrefs = new HRefs();

        protected long? id = null;
        public string apid;
        protected long? specId = null;

        protected int tabwidth = 128;
        
        protected string url;
        protected string enUrl;

        double? ra;
        double? dec;
        public string objId;
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
        public ExplorerQueries exploreQuery;
        public RunQuery runQuery;

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

            objId = qId;

            using (SqlConnection oConn = new SqlConnection(globals.ConnectionString))
            {
                    oConn.Open();
                    if (id.HasValue) pmtsFromPhoto(oConn, id, specId);
            }
                        
            ///This is new added 
            exploreQuery = new ExplorerQueries(id.ToString(), specId.ToString(), apid, fieldId, plateId, fiberId.ToString());  
            runQuery = new RunQuery();
            // common query to explorer
            string allId ="id="+id + "&spec=" + specId + "&apid=" + apid;
            string explore = "DisplayResults.aspx?" + allId + "&cmd=";
            ///string explore = "Explorer.aspx?cmd=";
            
            // id is the decimal representation; objId is the hex representation.
            hrefs.Summary = "summary.aspx?"+allId;
            hrefs.PhotoObj = explore + exploreQuery.PhotoObjQuery + "&name=PhotoObj";
            hrefs.PhotoTag = explore + exploreQuery.PhotoTagQuery + "&name=PhotoTag";
            hrefs.Field    = explore + exploreQuery.FieldQuery + "&name=Field";
            hrefs.Frame    = explore + exploreQuery.FrameQuery + "&name=Frame";
            

            if (globals.ReleaseNumber >= 8)
                    hrefs.Galaxyzoo = "galaxyzoo.aspx?"+allId;

            if (globals.ReleaseNumber > 4)
            {
                hrefs.PhotoZ =  explore+exploreQuery.PhotoZ+"&name=photoZ";
                hrefs.PhotozRF = explore + exploreQuery.PhotozRF + "&name=photozRF";
            }

            hrefs.Matches = "matches.aspx?"+allId;
            hrefs.Neighbors = "neighbors.aspx?"+allId;
            hrefs.Chart = "javascript:gotochart(" + ra + "," + dec + ");";
            hrefs.Navigate = "javascript:gotonavi(" + ra + "," + dec + ");";
            hrefs.SaveBook = "javascript:saveBook(\"" + objId + "\");";
            hrefs.ShowBook = "javascript:showNotes();";
            
            if (globals.Database.StartsWith("STRIPE"))
            {
                    if (run == 106)  run = 100006;
                    if (run == 206)  run = 200006;
            }

            hrefs.FITS = "fitsimg.aspx?id=" + id + "&fieldId=" + fieldId + "&spec=" + specId + "&apid=" + apid;
            
            hrefs.NED = "http://nedwww.ipac.caltech.edu/cgi-bin/nph-objsearch?search_type=Near+Position+Search"
                      + "&in_csys=Equatorial&in_equinox=J2000.0&obj_sort=Distance+to+search+center"
                      + "&lon=" + (ra.HasValue?(Math.Round((double)ra, 7).ToString()+"d"):"") 
                      + "&lat=" + (dec.HasValue?(Math.Round((double)dec, 7).ToString()+"d"):"") + "&radius=1.0";

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
            hrefs.AllSpec = "allSpec.aspx?"+allId;

            if (specId != null)
            {
                hrefs.SpecObj     = explore + exploreQuery.SpecObjQuery      + "&name=SpecObj";
                hrefs.sppLines    = explore + exploreQuery.sppLinesQuery     + "&name=sppLines";
                hrefs.sppParams   = explore + exploreQuery.sppParamsQuery    + "&name=sppParams";
                hrefs.galSpecLine = explore + exploreQuery.galSpecLineQuery  + "&name=galSpecLine";
                hrefs.galSpecIndx = explore + exploreQuery.galSpecIndexQuery + "&name=galSpecIndx";
                hrefs.galSpecInfo = explore + exploreQuery.galSpecInfoQuery  + "&name=galSpecInfo";

                hrefs.Plate       = "plate.aspx?" +allId + "&name=Plate&plateId=" + plateId;

                hrefs.Spectrum = "../../get/SpecById.ashx?ID=" + specId;

                hrefs.SpecFITS = "fitsspec.aspx?&sid=" + specObjId + "&id=" + id + "&spec=" + specId + "&apid=" + apid;
                    
                if (globals.ReleaseNumber >= 8)
                {
                    hrefs.theParameters = "parameters.aspx?"+allId;
                    hrefs.stellarMassStarformingPort = explore + exploreQuery.stellarMassStarformingPortQuery+ "&name=stellarMassStarFormingPort";
                    hrefs.stellarMassPassivePort     = explore + exploreQuery.stellarMassPassivePortQuery+ "&name=stellarMassPassivePort";
                    hrefs.emissionLinesPort          = explore + exploreQuery.emissionLinesPortQuery + "&name=emissionlinesPort";
                    hrefs.stellarMassPCAWiscBC03     = explore + exploreQuery.stellarMassPCAWiscBC03Query+"&name=stellarMassPCAWiscBC03";
                    hrefs.stellarMassPCAWiscM11      = explore + exploreQuery.stellarMassPCAWiscM11Query+ "&name=stellarMassPCAWiscM11";
                }
                if (globals.ReleaseNumber >= 10)
                {
                    hrefs.stellarMassFSPSGranEarlyDust   = explore + exploreQuery.stellarMassFSPSGranEarlyDust + "&name=stellarMassFSPSGranEarlyDust";
                    hrefs.stellarMassFSPSGranEarlyNoDust = explore + exploreQuery.stellarMassFSPSGranEarlyNoDust+"&name=stellarMassFSPSGranEarlyNoDust";
                    hrefs.stellarMassFSPSGranWideDust    = explore + exploreQuery.stellarMassFSPSGranWideDust + "&name=stellarMassFSPSGranWideDust";
                    hrefs.stellarMassFSPSGranWideNoDust  = explore + exploreQuery.stellarMassFSPSGranWideNoDust + "&name=stellarMassFSPSGranWideNoDust";
                 }
             }            
             if (!String.IsNullOrEmpty(apid))
             {
                 hrefs.apogeeStar = explore + exploreQuery.apogeeStar + "&name=apogeeStar";
                 hrefs.aspcapStar = explore + exploreQuery.aspcapStar + "&name=aspcapStar";
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
           
                if (specId == null)
                {
                    specId = Utilities.ParseId(specObjId);
                } 

                if (specId != null)
                {
                    oCmd.Parameters.Clear();
                    oCmd.CommandText = 
                        " select cast(s.plateId as binary(8)) as plateId, s.mjd, s.fiberId, q.plate" +
                        " from SpecObjAll s JOIN PlateX q ON s.plateId=q.plateId where specObjId=@specId"; //specObjId";
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
        /// Vertical aligned table  With DataSet
        /// </summary>
        /// <param name="namevalues"></param>
        /// <param name="w"></param>
        public void showVTable(DataSet ds, int w)
        {
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                char c = 't'; string unit = "test";
                Response.Write("<table cellpadding=2 cellspacing=2 border=0");
                if (w > 0)
                    Response.Write(" width=" + w);
                Response.Write(">\n");
                if (reader.Read())
                {
                    for(int k=0; k <reader.FieldCount; k++)
                    {
                        Response.Write("<tr align='left' >");
                        Response.Write("<td  valign='top' class='h'>");
                        Response.Write("<span ");
                        if (unit != "")
                            Response.Write("ONMOUSEOVER=\"this.T_ABOVE=true;this.T_WIDTH='100';return escape('<i>unit</i>=" + unit + "')\" ");
                        Response.Write("></span>");
                        Response.Write(reader.GetName(k) + "</td>");

                        Response.Write("<td valign='top' class='" + c + "'>");
                        Response.Write(reader.GetValue(k));
                        Response.Write("</td>");
                        Response.Write("</tr>");
                    }
                }
                Response.Write("</table>");
            }
        }

        /// <summary>
        /// Added new HTable with namevalue pair options
        /// </summary>
        /// <param name="namevalues"></param>
        /// <param name="w"></param>
        public void showHTable(DataSet ds, int w, string target)
        {
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                char c = 't'; string unit = "test";

                Response.Write("<table cellpadding=2 cellspacing=2 border=0");

                if (w > 0)
                    Response.Write(" width=" + w);
                Response.Write(">\n");

                Response.Write("<tr>");

                if (reader.HasRows)
                {
                   for (int k = 0; k < reader.FieldCount; k++)
                   {
                            Response.Write("<td align='middle' class='h'>");
                            Response.Write("<span ");
                            if (unit != "")
                                Response.Write("ONMOUSEOVER=\"this.T_ABOVE=true;this.T_WIDTH='100';return escape('<i>unit</i>=" + unit + "')\" ");
                            Response.Write("></span>");
                            Response.Write(reader.GetName(k) + "</td>");
                   }
                   Response.Write("</tr>");
                    
                } 
                while(reader.Read()){
                    Response.Write("<tr>");

                    for (int k = 0; k < reader.FieldCount; k++)
                    {
                        Response.Write("<td nowrap align='middle' class='" + c + "'>");

                        // think something else if possible for this
                        if (target.Equals("AllSpectra") && k == 0)
                        {
                            string u = "<a class='content' target='_top' href='obj.aspx?sid=";
                            Response.Write(u + reader.GetValue(k) + "'>" + reader.GetValue(k) + "</a></td>");
                            
                        }
                        else if(target.Equals("Neighbors") && k==0)
                        {
                            string u = "<a class='content' target='_top' href='obj.aspx?id=";
                            Response.Write(u + reader.GetValue(k) + "'>" + reader.GetValue(k) + "</a></td>");
                        }
                        
                        else
                        {
                            Response.Write(reader.GetValue(k));
                        }
                        Response.Write("</td>");
                    }
                }

                Response.Write("</tr>");

                Response.Write("</table>");
            }
        }
        

    }
}
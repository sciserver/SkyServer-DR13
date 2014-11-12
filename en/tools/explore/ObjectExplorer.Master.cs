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
        
        protected Globals globals;
        public ExplorerQueries exploreQuery;
        public RunQuery runQuery;
        
        protected HRefs hrefs = new HRefs();

        protected long? id = null;
        public string apid;
        protected long? specId = null;

        protected int tabwidth = 128;        
       
        protected string enUrl;
        
      

        protected double? ra = null;
        protected double? dec = null;

        protected double L = 0;
        protected double B = 0; // galactic coordinates, which we will convert into from ra, dec

        protected double? ApogeeRa = null;
        protected double? ApogeeDec = null;

        protected double? glon = null;
        protected double? glat = null;

        public string specObjId = null;
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
     
        protected string link;
  
        protected string cmd = null;
        protected string name = null;
        protected string url = null;

        string sidstring = null;
        long? sid = null;


        double? qra = null;
        double? qdec = null;

        int? mjd = null;
        short? plate = null;
        short? fiber = null;

        protected short? fiberId = null;
        protected int? mjdNum = null;
        public string objId = null;
        string fieldId = null;
        string idstring = null; // if it's an APOGEE object, we need to store the value as a string instead.

        int? run = null;
        int? rerun = null;
        short? camcol = null;
        short? field = null;


        
        protected void Page_Load(object sender, EventArgs e)
        {
            
            globals = (Globals)Application[Globals.PROPERTY_NAME];
            runQuery = new RunQuery();
            exploreQuery = new ExplorerQueries();
            

            if (Request.QueryString.Keys.Count == 0)
            {
                id = globals.ExploreDefault;
            }

            foreach (string key in Request.QueryString.Keys)
            {
                if (key == "id")
                {
                    string s = Request.QueryString["id"];
                    id = Utilities.ParseId(s);
                    objId = id.ToString();
                }
                if (key == "sid")
                {
                    string s = Request.QueryString["sid"];
                    sidstring = (string.Equals(s, "")) ? s : Utilities.ParseId(s).ToString();
                    specId = Utilities.ParseId(sidstring);
                }
                if (key == "spec")
                {
                    string s = Request.QueryString["spec"];
                    sidstring = (string.Equals(s, "")) ? s : Utilities.ParseId(s).ToString();
                    specId = Utilities.ParseId(sidstring);
                }
                if (key == "apid")
                {
                    string s = HttpUtility.UrlEncode( Request.QueryString["apid"] );
                    try
                    {
                        if (s != null & !"".Equals(s))
                        {
                            apid = s;
                        }
                    }
                    catch (Exception) { }
                }
                if (key == "ra") qra = Utilities.parseRA(Request.QueryString["ra"]); // need to parse J2000
                if (key == "dec") qdec = Utilities.parseDec(Request.QueryString["dec"]); // need to parse J2000
                if (key == "plate") plate = short.Parse(Request.QueryString["plate"]);
                if (key == "mjd") mjd = int.Parse(Request.QueryString["mjd"]);
                if (key == "fiber") fiber = short.Parse(Request.QueryString["fiber"]);
            }

            //This is imp function to get all different ids.
            getObjPmts();

            //if these are updated before
            if (id == null && objId != null) id = Utilities.ParseId(objId);
            if(specId == null && specObjId != null)specId = Utilities.ParseId(specObjId);            

            url = getURL();
            enUrl = getEnURL();
        
            
            ///This is new addition (will try to remove this by making queries static)            
            exploreQuery = new ExplorerQueries(id, specId, apid, fieldId, plateId, fiberId.ToString());  
           
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
            {
                hrefs.Galaxyzoo = "galaxyzoo.aspx?" + allId;
            }            

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
                    hrefs.stellarMassStarformingPort = explore + exploreQuery.stellarMassStarformingPortQuery + "&name=stellarMassStarFormingPort";
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

        //public void pmtsFromPhoto( long? id, long? specId) 
        //{
        //    exploreQuery = new ExplorerQueries();
        //    DataSet ds = runQuery.RunCasjobs(exploreQuery.getpmtsFromPhoto(id));
        //    using (DataTableReader reader = ds.Tables[0].CreateDataReader())
        //    {
        //         if (reader.Read())
        //         {
        //            ra = reader.GetDouble(0);
        //            dec = reader.GetDouble(1);
        //            run = reader.GetInt16(2);
        //            rerun = reader.GetInt16(3);
        //            camcol = reader.GetByte(4);
        //            field = reader.GetInt16(5);
        //            fieldId = RunQuery.checkNullorParse(reader.GetValue(6));
        //            specObjId = RunQuery.checkNullorParse(reader.GetValue(7));
        //            objId = RunQuery.checkNullorParse(reader.GetValue(8));
        //         }               
        //    }
           
        //    if (specId == null)
        //    {
        //        specId = Utilities.ParseId(specObjId);
        //    } 
            
        //    if (specId != null)
        //    {
        //        ds = runQuery.RunCasjobs(exploreQuery.getPlateFiberFromSpecObj(specObjId.ToString()));
        //        using (DataTableReader reader = ds.Tables[0].CreateDataReader())
        //        {
        //             if (reader.Read())
        //             {
        //                 plateId = RunQuery.checkNullorParse(reader.GetValue(0));
        //                 mjdNum = reader.GetInt32(1);
        //                 fiberId = reader.GetInt16(2);
        //                 plate = reader.GetInt16(3);
        //             }
        //         }
        //     }            
        //}

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

  

        public string getUnit(string tableName, string name)
        {            
            DataSet ds = runQuery.RunCasjobs(exploreQuery.getUnit(tableName,name));
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (reader.Read())
                    {
                        return reader.GetString(0);
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


        private void getObjPmts()
        {
            if (fiber.HasValue && plate.HasValue) ObjIDFromPlfib(plate, mjd, fiber);
            else if (qra.HasValue && qdec.HasValue) pmtsFromEq(qra, qdec);
            else if (sid.HasValue || !String.IsNullOrEmpty(sidstring)) pmtsFromSpec(sidstring);
            else if (id.HasValue && !sid.HasValue) pmtsFromPhoto(id);
            else if (!String.IsNullOrEmpty(apid)) parseApogeeID(apid);
        }

        private void ObjIDFromPlfib(short? plate, int? mjd, short? fiber)
        {
            DataSet ds = runQuery.RunCasjobs(exploreQuery.getObjIDFromPlfib(plate, mjd, fiber));
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (reader.Read())
                {
                    objId = reader["objId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["objId"]);
                    specObjId = reader["specObjId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["specObjId"]);
                }
            } // using DataReader

            // if we couldn't find that plate/mjd/fiber, maybe it's an APOGEE object
            if (String.IsNullOrEmpty(objId))
            {
                ds = runQuery.RunCasjobs(exploreQuery.getAPOGEEId_PlateFiberMjd(plate, mjd, fiber));
                using (DataTableReader reader = ds.Tables[0].CreateDataReader())
                {
                    if (reader.Read())
                    {
                        apid = reader.GetString(0);
                    }
                } // using DataReader                
            }

        }

        private void apogeeFromEq(double? qra, double? qdec)
        {
            DataSet ds = runQuery.RunCasjobs(exploreQuery.getApogeeFromEq(qra, qdec, 0.5 / 60));
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (reader.Read())
                {
                    apid = reader.GetString(0);
                }
            }
        }

        private void photoFromEq(double? qra, double? qdec)
        {
            DataSet ds = runQuery.RunCasjobs(exploreQuery.getPhotoFromEq(qra, qdec, globals.EqSearchRadius));
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (reader.Read())
                {
                    objId = reader["objId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["objId"]);
                    specObjId = reader["specObjId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["specObjId"]);
                }
            }
        }

        private void pmtsFromEq(double? qra, double? qdec)
        {
            DataSet ds = runQuery.RunCasjobs(exploreQuery.getpmtsFromEq(qra, qdec, globals.EqSearchRadius));
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (reader.Read())
                {
                    objId = reader["objId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["objId"]);
                    specObjId = reader["specObjId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["specObjId"]);
                }
            }
            if (objId != null && !objId.Equals("") )
            {
                // This is required to get the primary specObjId (with sciprimary=1). PhotoTag.specObjId is not necessarily primary...
                pmtsFromPhoto(Utilities.ParseId(objId));
                apogeeFromEq(qra, qdec);
            }
        }


        private void pmtsFromSpec(string sid)
        {
            long? sidnumber = 0;
            try
            {
                pmtsFromSpecWithApogeeID(sidstring);
                if (apid != null && apid != string.Empty)
                {
                    photoFromEq(ra, dec);
                }
            }
            catch (Exception e)
            {
            }

            try
            {
                sidnumber = Convert.ToInt64(sidstring);
                pmtsFromSpecWithSpecobjID(sidnumber);
                if (specObjId != null && specObjId != ZERO_ID)
                {
                    apogeeFromEq(ra, dec);
                }
            }
            catch
            {
            }
        }

        private void pmtsFromSpecWithApogeeID(string sid)
        {
            string whatdoiget = null;
            if (sid.StartsWith("apogee")) { whatdoiget = "apstar_id"; } else { whatdoiget = "apogee_id"; }

            DataSet ds = runQuery.RunCasjobs(exploreQuery.getpmtsFromSpecWithApogeeId(whatdoiget, sid));
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (reader.Read())
                {
                    apid = reader.GetString(0);
                    ra = reader.GetDouble(1);
                    dec = reader.GetDouble(2);
                }
            } // using DataReader        
        }
      
        private void pmtsFromSpecWithSpecobjID(long? sid)
        {
            DataSet ds = runQuery.RunCasjobs(exploreQuery.getpmtsFromSpecWithSpecobjID(sid));
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (reader.Read())
                {
                    ra = (double)reader["ra"];
                    dec = (double)reader["dec"];
                    fieldId = reader["fieldId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["fieldId"]);
                    objId = reader["objId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["objId"]);
                    specObjId = reader["specObjId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["specObjId"]);
                    plateId = reader["plateId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["plateId"]);
                    mjd = (int)reader["mjd"];
                    fiber = (short)reader["fiberId"];
                    plate = (short)reader["plate"];
                }
            } // using DataReader

        }

    
        private void pmtsFromPhoto(long? id)
        {
            DataSet ds = runQuery.RunCasjobs(exploreQuery.getpmtsFromPhoto(id));
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (reader.Read())
                {
                    ra = (double)reader["ra"];
                    dec = (double)reader["dec"];
                    run = (short)reader["run"];
                    rerun = (short)reader["rerun"];
                    camcol = (byte)reader["camcol"];
                    field = (short)reader["field"];

                    fieldId = reader["fieldId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["fieldId"]);
                    objId = reader["objId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["objId"]);
                    specObjId = reader["specObjId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["specObjId"]);
                    
                }
            }

            // get the plateId and fiberId from the specObj, if it exists
            if (specObjId != null && !ZERO_ID.Equals(specObjId))
            {
                ds = runQuery.RunCasjobs(exploreQuery.getPlateFiberFromSpecObj(specObjId));
                using (DataTableReader reader = ds.Tables[0].CreateDataReader())
                {
                    if (reader.Read())
                    {
                        plateId = reader["plateId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["plateId"]);
                      
                        mjd = reader.GetInt32(1);
                        fiber = reader.GetInt16(2);
                        plate = reader.GetInt16(3);
                    }
                } // using DataReader
            }

            try
            {
                apogeeFromEq(ra, dec);
            }
            catch { }
        }

        private void parseApogeeID(string idstring)
        {
            // ds = runQuery.RunCasjobs(exploreQuery.getParseApogeeId(idstring));
            //using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            //{
            //    if (reader.Read())
            //    {
            //        apid = reader.GetString(0);
            //    }
            //}
            long amjd = 0;
            long afiber =0;
            long aplate =0;
            DataSet ds = runQuery.RunCasjobs(exploreQuery.getPlateFromApogee(apid));
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (reader.Read())
                {
                    amjd = (long)reader["mjd"];
                    afiber = (long)reader["fiberId"];
                    aplate = (long)reader["plate"];
                }
            }

            ds = runQuery.RunCasjobs(exploreQuery.getObjIDFromPlfib((short)aplate, (int)amjd, (short)afiber));
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (reader.Read())
                {
                    objId = reader["objId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["objId"]);
                    specObjId = reader["specObjId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["specObjId"]);
                }
            }

        }

        
    }
}
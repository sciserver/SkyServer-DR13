﻿using System;
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
        protected HRefs hrefs = new HRefs();
        protected string enUrl;
        protected string url = null;
        protected int tabwidth = 128;        

        public ExplorerQueries exploreQuery;       

        public long? id = null;        
        public long? specId = null;
        public string apid;
        public string objId = null;
        public string specObjId = null;
                
        public double? ra = null;
        public double? dec = null;

        public string plateId = null;
        public string fieldId = null;
        public short? fiberId = null;
        public int? mjd;
        public short? plate;

        public int? run ;
        public short? rerun;
        public short? camcol;
        public short? field ;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
            globals = (Globals)Application[Globals.PROPERTY_NAME];
            
            exploreQuery = new ExplorerQueries();           
            //Get Values from Session Object
            getSessionIds();

            url = getURL();
            enUrl = getEnURL();
           
            // common query to explorer
            string allId ="id="+id + "&spec=" + specId + "&apid=" + apid+"&field="+fieldId;

            
            // id is the decimal representation; objId is the hex representation.
            hrefs.Summary  = "summary.aspx?"+allId;
            hrefs.PhotoObj = "DisplayResults.aspx?name=PhotoObj&"+allId;
            hrefs.PhotoTag = "DisplayResults.aspx?name=PhotoTag&" + allId;
            hrefs.Field    = "DisplayResults.aspx?name=Field&" + allId;
            hrefs.Frame    = "DisplayResults.aspx?name=Frame&" + allId;


            if (globals.ReleaseNumber >= 8) 
            {
                hrefs.Galaxyzoo = "galaxyzoo.aspx?" + allId;
            }            

            if (globals.ReleaseNumber > 4)
            {
                hrefs.PhotoZ = "DisplayResults.aspx?&name=photoZ&" + allId;
               // hrefs.PhotozRF = "DisplayResults.aspx?&name=photozRF&" + allId;
            }

            hrefs.Matches = "matches.aspx?"+allId;
            hrefs.Neighbors = "neighbors.aspx?"+allId;
            hrefs.Chart    = "javascript:gotochart(" + ra + "," + dec + ");";
            hrefs.Navigate = "javascript:gotonavi(" + ra + "," + dec + ");";
            hrefs.SaveBook = "javascript:saveBook(\"" + objId + "\");";
            hrefs.ShowBook = "javascript:showNotes();";
            
            if (globals.Database.StartsWith("STRIPE"))
            {
                    if (run == 106)  run = 100006;
                    if (run == 206)  run = 200006;
            }

            hrefs.FITS = "fitsimg.aspx?"+allId;//id=" + id + "&fieldId=" + fieldId + "&spec=" + specId + "&apid=" + apid;
            
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
                hrefs.SpecObj = "DisplayResults.aspx?name=SpecObj&" + allId;
                hrefs.sppLines = "DisplayResults.aspx?name=sppLines&" + allId;
                hrefs.sppParams = "DisplayResults.aspx?name=sppParams&" + allId;
                hrefs.galSpecLine = "DisplayResults.aspx?name=galSpecLine&" + allId;
                hrefs.galSpecIndx = "DisplayResults.aspx?name=galSpecIndx&" + allId;
                hrefs.galSpecInfo = "DisplayResults.aspx?name=galSpecInfo&" + allId;

                hrefs.Plate = "plate.aspx?&name=Plate&plateId=" + plateId;

                hrefs.Spectrum = "../../get/SpecById.ashx?ID=" + specId;

                hrefs.SpecFITS = "fitsspec.aspx?&sid=" + specObjId + "&id=" + id + "&spec=" + specId + "&apid=" + apid;
                    
                if (globals.ReleaseNumber >= 8)
                {
                    hrefs.theParameters = "parameters.aspx?"+allId;
                    hrefs.stellarMassStarformingPort = "DisplayResults.aspx?name=stellarMassStarFormingPort&" + allId;
                    hrefs.stellarMassPassivePort = "DisplayResults.aspx?name=stellarMassPassivePort&" + allId;
                    hrefs.emissionLinesPort = "DisplayResults.aspx?name=emissionlinesPort&" + allId;
                    hrefs.stellarMassPCAWiscBC03 = "DisplayResults.aspx?name=stellarMassPCAWiscBC03&" + allId;
                    hrefs.stellarMassPCAWiscM11 = "DisplayResults.aspx?name=stellarMassPCAWiscM11&" + allId;
                }
                if (globals.ReleaseNumber >= 10)
                {
                    hrefs.stellarMassFSPSGranEarlyDust = "DisplayResults.aspx?name=stellarMassFSPSGranEarlyDust&" + allId;
                    hrefs.stellarMassFSPSGranEarlyNoDust = "DisplayResults.aspx?name=stellarMassFSPSGranEarlyNoDust&" + allId;
                    hrefs.stellarMassFSPSGranWideDust = "DisplayResults.aspx?name=stellarMassFSPSGranWideDust&" + allId;
                    hrefs.stellarMassFSPSGranWideNoDust = "DisplayResults.aspx?name=stellarMassFSPSGranWideNoDust&" + allId;
                 }
             }            
             if (apid != null && !String.IsNullOrEmpty(apid))
             {
                 hrefs.apogeeStar = "DisplayResults.aspx?name=apogeeStar&" + allId;
                 hrefs.aspcapStar = "DisplayResults.aspx?name=aspcapStar&" + allId;
             }
        }

        private void getSessionIds() {

            ObjectInfo o =(ObjectInfo)Session["objectInfo"];
            objId = o.objId;
            specObjId = o.specObjId;
            apid = o.apid;

            ra = o.ra;
            dec = o.dec;

            plateId =o.plateId;
            fieldId = o.fieldId;
            fiberId = o.fiberId;
            mjd = o.mjd;
            plate = o.plate;

            run = o.run;
            rerun = o.rerun;
            camcol = o.camcol;
            field = o.field;

            id = o.id;
            specId = o.specId;
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
                if ( lang == "en" || lang == "de" || lang == "jp"
                  || lang == "hu" || lang == "sp" || lang == "ce" || lang == "pt" 
                  || lang == "zh" || lang == "uk" || lang == "ru")
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

  

        //public string getUnit(string tableName, string name)
        //{            
        //    DataSet ds = runQuery.RunCasjobs(exploreQuery.getUnit(tableName,name));
        //    using (DataTableReader reader = ds.Tables[0].CreateDataReader())
        //    {
        //        if (reader.Read())
        //            {
        //                return reader.GetString(0);
        //            }
        //    }            

        //    return "";
        //}

       
      

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
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        for (int k = 0; k < reader.FieldCount; k++)
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
                }
                else {
                    Response.Write("<tr> <td class='nodatafound'>No data found for this object </td></tr>");
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


                    while (reader.Read())
                    {
                        Response.Write("<tr>");

                        for (int k = 0; k < reader.FieldCount; k++)
                        {
                            Response.Write("<td nowrap align='middle' class='" + c + "'>");

                            // think something else if possible for this
                            if (target.Equals("AllSpectra") && k == 0)
                            {
                                string u = "<a class='content' target='_top' href='Summary.aspx?sid=";
                                Response.Write(u + reader.GetValue(k) + "'>" + reader.GetValue(k) + "</a></td>");
                            }

                            else if (target.Equals("Neighbors") && k == 0)
                            {
                                string u = "<a class='content' target='_top' href='Summary.aspx?id=";
                                Response.Write(u + reader.GetValue(k) + "'>" + reader.GetValue(k) + "</a></td>");
                            }

                            else
                            {
                                Response.Write(reader.GetValue(k));
                            }
                            Response.Write("</td>");
                        }
                    }
                }
                else {
                    Response.Write(" <td class='nodatafound'>No data found for this object </td>");
                }

                Response.Write("</tr>");

                Response.Write("</table>");
            }
        }
    }
}
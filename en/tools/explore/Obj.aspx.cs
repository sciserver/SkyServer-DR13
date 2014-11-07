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
    public partial class Obj : System.Web.UI.Page
    {
        const string ZERO_ID = "0x0000000000000000";

        long? id = null;
        string sidstring = null;
        long? sid = null;
        string apid = null;

        double? qra = null;
        double? qdec = null;

        int? mjd = null;
        short? plate = null;
        short? fiber = null;
        double? ra = null;
        double? dec = null;

        string objId = null;
        string fieldId = null;
        string specObjId = null;
        string plateId = null;
        string idstring = null; // if it's an APOGEE object, we need to store the value as a string instead.

        short? run = null;
        short? rerun = null;
        short? camcol = null;
        short? field = null;

        Globals globals;
        ExplorerQueries exploreQueries;
        RunQuery runQuery;

        protected void Page_Load(object sender, EventArgs e)
        {
              globals = (Globals)Application[Globals.PROPERTY_NAME];
              exploreQueries = new ExplorerQueries();
              runQuery = new RunQuery();
              if (Request.QueryString.Keys.Count == 0) {
                  id = globals.ExploreDefault;
              }

              foreach (string key in Request.QueryString.Keys)
              {
                    if (key == "id")
                    {
                        string s = Request.QueryString["id"];
                        id = Utilities.ParseId(s);
                    }
                    if (key == "sid")
                    {
                        string s = Request.QueryString["sid"];
                        sidstring = (string.Equals(s,"")) ? s : Utilities.ParseId(s).ToString();                       
                    }
                    if (key == "apid")
                    {
                        string s = Request.QueryString["apid"];
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
                
               getObjPmts();

               Response.Redirect("Summary.aspx?id=" + ((objId == null) ? "" : objId)
                                  + "&spec=" + ((specObjId == null) ? "" : specObjId)
                                  + "&apid=" + ((apid == null) ? "" : apid));           
        }

        private void getObjPmts()
        {
            if (fiber.HasValue && plate.HasValue) ObjIDFromPlfib( plate, mjd, fiber);
            else if (qra.HasValue && qdec.HasValue) pmtsFromEq( qra, qdec);
            else if (sid.HasValue || !String.IsNullOrEmpty(sidstring)) pmtsFromSpec( sidstring);
            else if (id.HasValue && !sid.HasValue) pmtsFromPhoto( id);
            else if (!String.IsNullOrEmpty(idstring)) parseApogeeID( idstring);
        }

        private void ObjIDFromPlfib( short? plate, int? mjd, short? fiber)
        {            
            DataSet ds = runQuery.RunCasjobs(exploreQueries.getObjIDFromPlfib(plate, mjd, fiber));
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                    if (reader.Read())
                    {
                        objId = RunQuery.checkNullorParse(reader.GetValue(0));
                        specObjId = RunQuery.checkNullorParse(reader.GetValue(1));
                    }
            } // using DataReader

            // if we couldn't find that plate/mjd/fiber, maybe it's an APOGEE object
            if (String.IsNullOrEmpty(objId)) {
                ds = runQuery.RunCasjobs(exploreQueries.getAPOGEEId_PlateFiberMjd(plate, mjd, fiber));
                using (DataTableReader reader = ds.Tables[0].CreateDataReader()){
                        if (reader.Read())
                        {
                            apid = reader.GetString(0);
                        }
                } // using DataReader                
            }
        }

        private void apogeeFromEq(double? qra, double? qdec)
        {
            DataSet ds = runQuery.RunCasjobs(exploreQueries.getApogeeFromEq(qra,qdec,0.5/60));
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
            DataSet ds = runQuery.RunCasjobs(exploreQueries.getPhotoFromEq(qra,qdec,globals.EqSearchRadius));
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (reader.Read())
                {
                    objId = RunQuery.checkNullorParse(reader.GetValue(0));
                    specObjId = RunQuery.checkNullorParse(reader.GetValue(1));
                }               
            }
        }

        private void pmtsFromEq( double? qra, double? qdec)
        {
            DataSet ds = runQuery.RunCasjobs(exploreQueries.getpmtsFromEq(qra, qdec, globals.EqSearchRadius));
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (reader.Read())
                {
                    objId = RunQuery.checkNullorParse(reader.GetValue(0));
                    specObjId = RunQuery.checkNullorParse(reader.GetValue(1));
                }
            }
                        
            // This is required to get the primary specObjId (with sciprimary=1). PhotoTag.specObjId is not necessarily primary...
            pmtsFromPhoto( Utilities.ParseId(objId));
            apogeeFromEq(qra,qdec);               
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
                pmtsFromSpecWithSpecobjID( sidnumber);
                if (specObjId != null && specObjId != ZERO_ID)
                {
                    apogeeFromEq( ra, dec);
                }
            }
            catch
            {   
            } 
        }

        private void pmtsFromSpecWithApogeeID(string sid)
        {
            string whatdoiget = null;
                if (sid.StartsWith("apogee")) { whatdoiget = "apstar_id"; } else { whatdoiget="apogee_id"; }

                DataSet ds = runQuery.RunCasjobs(exploreQueries.getpmtsFromSpecWithApogeeId(whatdoiget,sid));
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

        private void pmtsFromSpecWithSpecobjID( long? sid)
        {
           DataSet ds = runQuery.RunCasjobs(exploreQueries.getpmtsFromSpecWithSpecobjID(sid));
           using (DataTableReader reader = ds.Tables[0].CreateDataReader())
           {
               if (reader.Read())
               {
                        ra = reader.GetDouble(0);
                        dec = reader.GetDouble(1);
                        fieldId = RunQuery.checkNullorParse(reader.GetValue(6));
                        specObjId = RunQuery.checkNullorParse(reader.GetValue(7));
                        objId = RunQuery.checkNullorParse(reader.GetValue(8));
                        plateId = RunQuery.checkNullorParse(reader.GetValue(5));
                        mjd = reader.GetInt32(6);
                        fiber = reader.GetInt16(7);
                        plate = reader.GetInt16(8);
               }
            } // using DataReader
            
        }

        private void pmtsFromPhoto( long? id)
        {
           DataSet ds = runQuery.RunCasjobs(exploreQueries.getpmtsFromPhoto(id));
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

                        fieldId = RunQuery.checkNullorParse(reader.GetValue(6));
                        specObjId =RunQuery.checkNullorParse(reader.GetValue(7));
                        objId = RunQuery.checkNullorParse(reader.GetValue(8));
                    }
           }
            
           // get the plateId and fiberId from the specObj, if it exists
           if (specObjId != null && !ZERO_ID.Equals(specObjId))
           {
              ds = runQuery.RunCasjobs(exploreQueries.getPlateFiberFromSpecObj(specObjId));
              using (DataTableReader reader = ds.Tables[0].CreateDataReader())
              {
                  if (reader.Read())
                  {
                      plateId = RunQuery.checkNullorParse(reader.GetValue(0));
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
        
        private void parseApogeeID( string idstring)
        {
           DataSet ds = runQuery.RunCasjobs(exploreQueries.getParseApogeeId(idstring));
           using (DataTableReader reader = ds.Tables[0].CreateDataReader())
           {
              if (reader.Read())
              {
                  apid = reader.GetString(0);
              }
           }           
        }

        //private string RunQuery.checkNullorParse(object value) {
        //    string stringValue="";
        //    if (value.Equals(DBNull.Value)) return null;
        //    else
        //    stringValue = Functions.BytesToHex((byte[])value);
        //    return stringValue;
        //}
    }
}
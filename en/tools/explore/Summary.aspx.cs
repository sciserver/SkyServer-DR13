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
        protected ObjectExplorer master;
        
        public RunQuery runQuery;
        public ObjectInfo objectInfo = new ObjectInfo();

        //protected HRefs hrefs = new HRefs();

         long? id = null;
         string apid;
         long? specId = null;
         string sidstring = null;
         double? qra = null;
         double? qdec = null;

        int? mjd = null;
        short? plate = null;
        short? fiber = null;

      
        protected void Page_Load(object sender, EventArgs e)
        {
           
            runQuery = new RunQuery();
            globals = (Globals)Application[Globals.PROPERTY_NAME];
            master = (ObjectExplorer)Page.Master;
            Session["objectInfo"] = objectInfo;

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
                }
                if (key == "sid")
                {
                    string s = Request.QueryString["sid"].Trim().ToUpper();
                    if (s.StartsWith("2M")) sidstring = s;
                    else
                    sidstring = (string.Equals(s, "")) ? s : Utilities.ParseId(s).ToString();                   
                }
                if (key == "spec")
                {
                    string s = Request.QueryString["spec"];
                    sidstring = (string.Equals(s, "")) ? s : Utilities.ParseId(s).ToString();                 
                }
                if (key == "apid")
                {
                    string s = HttpUtility.UrlEncode(Request.QueryString["apid"]);                    
                    if (s != null & !"".Equals(s))
                    {
                            apid = s;
                    }                    
                }
                if (key == "ra") qra = Utilities.parseRA(Request.QueryString["ra"]); // need to parse J2000
                if (key == "dec") qdec = Utilities.parseDec(Request.QueryString["dec"]); // need to parse J2000
                if (key == "plate") plate = short.Parse(Request.QueryString["plate"]);
                if (key == "mjd") mjd = int.Parse(Request.QueryString["mjd"]);
                if (key == "fiber") fiber = short.Parse(Request.QueryString["fiber"]);
            }
           
            //This is imp function to get all different ids.
            getObjPmts();

            //parseId and store ObjectInfo in session
            parseIds();
            
        }

        private void parseIds() {
            if (objectInfo.objId != null && !objectInfo.objId.Equals(""))
                objectInfo.id = Utilities.ParseId(objectInfo.objId);

            if (objectInfo.specObjId != null && !objectInfo.specObjId.Equals(""))
                objectInfo.specId = Utilities.ParseId(objectInfo.specObjId);

            Session["objectInfo"] = objectInfo;

        }

        private void getObjPmts()
        {
            if (fiber.HasValue && plate.HasValue) ObjIDFromPlfib(plate, mjd, fiber);
            else if (qra.HasValue && qdec.HasValue) pmtsFromEq(qra, qdec);
            else if (specId.HasValue || !String.IsNullOrEmpty(sidstring)) pmtsFromSpec(sidstring);
            else if (id.HasValue && !specId.HasValue) pmtsFromPhoto(id);
            else if (!String.IsNullOrEmpty(apid)) parseApogeeID(apid);
        }

        private void ObjIDFromPlfib(short? plate, int? mjd, short? fiber)
        {
            string cmd = ExplorerQueries.getObjIDFromPlatefiberMjd;
            cmd = cmd.Replace("@mjd", mjd.ToString());
            cmd = cmd.Replace("@plate", plate.ToString());
            cmd = cmd.Replace("@fiberId", fiber.ToString());

            DataSet ds = runQuery.RunCasjobs(cmd);
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (reader.Read())
                {
                   objectInfo.objId = reader["objId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["objId"]);
                   objectInfo.specObjId = reader["specObjId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["specObjId"]);
                   objectInfo.ra = (double)reader["ra"];
                   objectInfo.dec = (double)reader["dec"];
                }
            } // using DataTableReader


            cmd = ExplorerQueries.getApogeeFromEq;
            cmd = cmd.Replace("@qra", objectInfo.ra.ToString());
            cmd = cmd.Replace("@qdec", objectInfo.dec.ToString());
            cmd = cmd.Replace("@searchRadius", (0.5 / 60).ToString());
            // if we couldn't find that plate/mjd/fiber, maybe it's an APOGEE object
            if (!String.IsNullOrEmpty(objectInfo.objId))
            {
                ds = runQuery.RunCasjobs(cmd);
                using (DataTableReader reader = ds.Tables[0].CreateDataReader())
                {
                    if (reader.Read())
                    {
                        objectInfo.apid = (string)reader["apstar_id"];
                    }
                } // using DataTableReader                
            }

        }

        private void apogeeFromEq(double? qra, double? qdec)
        {

            string cmd = ExplorerQueries.getApogeeFromEq;
            cmd = cmd.Replace("@qra", qra.ToString());
            cmd = cmd.Replace("@qdec", qdec.ToString());
            cmd = cmd.Replace("@searchRadius", (0.5 / 60).ToString());

            DataSet ds = runQuery.RunCasjobs(cmd);
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (reader.Read())
                {
                    objectInfo.apid = (string)reader["apstar_id"];
                }
            }
        }

        private void photoFromEq(double? qra, double? qdec)
        {
            string cmd = ExplorerQueries.getPhotoFromEq;
            cmd = cmd.Replace("@qra", qra.ToString());
            cmd = cmd.Replace("@qdec", qdec.ToString());
            cmd = cmd.Replace("@searchRadius", (0.5 / 60).ToString());
            DataSet ds = runQuery.RunCasjobs(cmd);
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (reader.Read())
                {
                   objectInfo.objId = reader["objId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["objId"]);
                  objectInfo.specObjId = reader["specObjId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["specObjId"]);
                }
            }
        }

        private void pmtsFromEq(double? qra, double? qdec)
        {
            string cmd = ExplorerQueries.getpmtsFromEq;
            cmd = cmd.Replace("@qra", qra.ToString());
            cmd = cmd.Replace("@qdec", qdec.ToString());
            cmd = cmd.Replace("@searchRadius", (0.5 / 60).ToString());

            DataSet ds = runQuery.RunCasjobs(cmd);
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (reader.Read())
                {
                   objectInfo.objId = reader["objId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["objId"]);
                   objectInfo.specObjId = reader["specObjId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["specObjId"]);
                }
            }
            if (objectInfo.objId != null && !objectInfo.objId.Equals(""))
            {
                // This is required to get the primary specObjId (with sciprimary=1). PhotoTag.specObjId is not necessarily primary...
                pmtsFromPhoto(Utilities.ParseId(objectInfo.objId));
                apogeeFromEq(qra, qdec);
            }
        }


        private void pmtsFromSpec(string sid)
        {
            long? sidnumber = 0;
            try
            {
                pmtsFromSpecWithApogeeID(sidstring);
                if (objectInfo.apid != null && objectInfo.apid != string.Empty)
                {
                    photoFromEq(objectInfo.ra, objectInfo.dec);
                }
            }
            catch (Exception e) { }

            try
            {
                sidnumber = Convert.ToInt64(sidstring);
                pmtsFromSpecWithSpecobjID(sidnumber);
                if (objectInfo.specObjId != null && objectInfo.specObjId != ZERO_ID)
                {
                    apogeeFromEq(objectInfo.ra, objectInfo.dec);
                }
            }
            catch (Exception e) { }
        }

        private void pmtsFromSpecWithApogeeID(string sid)
        {
            string whatdoiget = null;
            if (sid.StartsWith("apogee")) { whatdoiget = "apstar_id"; } else { whatdoiget = "apogee_id"; }

            string cmd = ExplorerQueries.getpmtsFromSpecWithApogeeId;
            cmd = cmd.Replace("@whatdoiget",whatdoiget);
            cmd = cmd.Replace("@sid","'"+sid+"'");

            DataSet ds = runQuery.RunCasjobs(cmd);
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (reader.Read())
                {
                   objectInfo.apid = reader.GetString(0);
                   objectInfo.ra = reader.GetDouble(1);
                   objectInfo.dec = reader.GetDouble(2);
                }
            } // using DataReader        
        }

        private void pmtsFromSpecWithSpecobjID(long? sid)
        {
            string cmd = ExplorerQueries.getpmtsFromSpecWithSpecobjID;
            cmd = cmd.Replace("@sid", sid.ToString());
            DataSet ds = runQuery.RunCasjobs(cmd);
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (reader.Read())
                {
                   objectInfo.ra = (double)reader["ra"];
                   objectInfo.dec = (double)reader["dec"];
                   objectInfo.fieldId = reader["fieldId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["fieldId"]);
                   objectInfo.objId = reader["objId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["objId"]);
                   objectInfo.specObjId = reader["specObjId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["specObjId"]);
                   objectInfo.plateId = reader["plateId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["plateId"]);
                   objectInfo.mjd = (int)reader["mjd"];
                   objectInfo.fiberId = (short)reader["fiberId"];
                   objectInfo.plate = (short)reader["plate"];
                }
            } // using DataReader

        }


        private void pmtsFromPhoto(long? id)
        {
            string cmd = ExplorerQueries.getpmtsFromPhoto;
            cmd = cmd.Replace("@objid", id.ToString());

            DataSet ds = runQuery.RunCasjobs(cmd);
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (reader.Read())
                {
                   objectInfo.ra = (double)reader["ra"];
                   objectInfo.dec = (double)reader["dec"];
                   objectInfo.run = (short)reader["run"];
                   objectInfo.rerun = (short)reader["rerun"];
                   objectInfo.camcol = (byte)reader["camcol"];
                   objectInfo.field = (short)reader["field"];
                   objectInfo.fieldId = reader["fieldId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["fieldId"]);
                   objectInfo.objId = reader["objId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["objId"]);
                   objectInfo.specObjId = reader["specObjId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["specObjId"]);

                }
            }

            // get the plateId and fiberId from the specObj, if it exists
            if (objectInfo.specObjId != null && !ZERO_ID.Equals(objectInfo.specObjId))
            {
                long specId = long.Parse(objectInfo.specObjId.Substring(2), NumberStyles.AllowHexSpecifier);
                cmd = ExplorerQueries.getPlateFiberFromSpecObj;
                cmd = cmd.Replace("@specId", specId.ToString());
                
                ds = runQuery.RunCasjobs(cmd);
                using (DataTableReader reader = ds.Tables[0].CreateDataReader())
                {
                    if (reader.Read())
                    {
                       objectInfo.plateId = reader["plateId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["plateId"]);
                       objectInfo.mjd = (int)reader["mjd"];
                       objectInfo.fiberId = (short) reader["fiberId"];
                       objectInfo.plate = (short)reader["plate"];
                    }
                } // using DataReader
            }

            try
            {
                apogeeFromEq(objectInfo.ra, objectInfo.dec);
            }
            catch { }
        }

        private void parseApogeeID(string idstring)
        {
            double qra =0, qdec=0;
            objectInfo.apid = apid;
            string cmd = "";
            apid = apid.ToLower();
            if(apid.Contains("apogee"))
             cmd = ExplorerQueries.getApogee;
            else
             cmd = ExplorerQueries.getApogee2;

            cmd = cmd.Replace("@apogeeId",apid);
            DataSet ds = runQuery.RunCasjobs(cmd);
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (reader.Read())
                {
                    qra = (double)reader["ra"];
                    qdec =(double)reader["dec"];
                    
                }
            }
            cmd = ExplorerQueries.getpmtsFromEq;
            cmd = cmd.Replace("@qra", qra.ToString());
            cmd = cmd.Replace("@qdec", qdec.ToString());
            cmd = cmd.Replace("@searchRadius", (0.5/60).ToString());

            ds = runQuery.RunCasjobs(cmd);
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (reader.Read())
                {
                   objectInfo.objId = reader["objId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["objId"]);
                   objectInfo.specObjId = reader["specObjId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["specObjId"]);                    
                }
            }
        }
    }
}
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


namespace SkyServer.Tools.QuickLook
{
    public partial class Summary : System.Web.UI.Page
    {
        protected const string ZERO_ID = "0x0000000000000000";
        protected Globals globals;
        protected ObjectQuickLook master;
        
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
        private HttpCookie cookie;
        private string token = "";

     
        protected void Page_Load(object sender, EventArgs e)
        {
           
            
            globals = (Globals)Application[Globals.PROPERTY_NAME];
            master = (ObjectQuickLook)Page.Master;
            Session["QuickLookObjectInfo"] = objectInfo;
          
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

            ///Check for authenticated token
            cookie = Request.Cookies["Keystone"];
            if (cookie != null)
            {
                if (cookie["token"] != null || !cookie["token"].Equals(""))
                {
                    token = cookie["token"];

                }
            }
            runQuery = new RunQuery(token);
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

            Session["QuickLookObjectInfo"] = objectInfo;

        }

        private void getObjPmts()
        {
            if (fiber.HasValue && plate.HasValue) setFromPlateMjdFiber(plate, mjd, fiber);
            else if (qra.HasValue && qdec.HasValue) setFromEq(qra, qdec);
            else if (specId.HasValue || !String.IsNullOrEmpty(sidstring)) setFromSpecObjID(sidstring);
            else if (id.HasValue && !specId.HasValue) setFromObjID(id);
            else if (!String.IsNullOrEmpty(apid)) parseApogeeID(apid);
        }


        private void setObjectInfo(string cmd)
        {
            DataSet ds = runQuery.RunCasjobs(cmd, "QuickLook: Summary");
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (reader.Read())
                {
                    objectInfo.objId = reader["objId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["objId"]);
                    objectInfo.specObjId = reader["specObjId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["specObjId"]);
                    objectInfo.ra = (double?)reader["ra"];
                    objectInfo.dec = (double?)reader["dec"];

                    objectInfo.mjd = (int?)reader["mjd"];
                    objectInfo.plate = (short?)reader["plate"];
                    objectInfo.plateId = reader["plateId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["plateId"]);
                    objectInfo.fiberId = (short?)reader["fiberId"];
                    objectInfo.fieldId = reader["fieldId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["fieldId"]);

                    objectInfo.run = (short?)reader["run"];
                    objectInfo.rerun = (short?)reader["rerun"];
                    objectInfo.camcol = (byte?)reader["camcol"];
                    objectInfo.field = (short?)reader["field"];

                    objectInfo.u = (string)reader["u"];
                    objectInfo.g = (string)reader["g"];
                    objectInfo.r = (string)reader["r"];
                    objectInfo.i = (string)reader["i"];
                    objectInfo.z = (string)reader["z"];
                    objectInfo.err_u = (string)reader["err_u"];
                    objectInfo.err_g = (string)reader["err_g"];
                    objectInfo.err_r = (string)reader["err_r"];
                    objectInfo.err_i = (string)reader["err_i"];
                    objectInfo.err_z = (string)reader["err_z"];
                    objectInfo.otype = (string)reader["otype"];
                    objectInfo.spectralClass = (string)reader["spectralClass"];
                    objectInfo.flags = (string)reader["flags"];
                    objectInfo.redshift = (float?)reader["redshift"];
                }
            } // using DataTableReader
        }

        private void setFromPlateMjdFiber(short? plate, int? mjd, short? fiber)
        {
            string cmd = QuickLookQueries.getParamsFromPlateFiberMjd;
            cmd = cmd.Replace("@mjd", mjd.ToString());
            cmd = cmd.Replace("@plate", plate.ToString());
            cmd = cmd.Replace("@fiberId", fiber.ToString());
            setObjectInfo(cmd);
        }

        private void setFromEq(double? qra, double? qdec)
        {

            string cmd = QuickLookQueries.getObjIDFromEq;
            cmd = cmd.Replace("@qra", qra.ToString());
            cmd = cmd.Replace("@qdec", qdec.ToString());
            cmd = cmd.Replace("@searchRadius", (0.5 / 60).ToString());
            DataSet ds = runQuery.RunCasjobs(cmd, "QuickLook: Summary");
            string ObjID2 = null;
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (reader.Read())
                {
                    ObjID2 = reader["objId"].ToString();
                }
            }
            if (ObjID2 != null && !ObjID2.Equals(""))
            {
                cmd = QuickLookQueries.getParamsFromObjID;
                cmd = cmd.Replace("@objid", ObjID2);
                setObjectInfo(cmd);
            }
        }


        private void setFromSpecObjID(string sid)
        {
            string cmd = QuickLookQueries.getParamsFromSpecObjID;
            cmd = cmd.Replace("@sid", sid);
            setObjectInfo(cmd);
        }


        private void setFromObjID(long? id)
        {
            string cmd = QuickLookQueries.getParamsFromObjID;
            cmd = cmd.Replace("@objid", id.ToString());
            setObjectInfo(cmd);
        }


        private void parseApogeeID(string idstring)
        {
            double qra = 0, qdec = 0;
            objectInfo.apid = apid;
            string cmd = "";
            apid = apid.ToLower();
            if (apid.Contains("apogee"))
                cmd = QuickLookQueries.getApogee;
            else
                cmd = QuickLookQueries.getApogee2;

            cmd = cmd.Replace("@apogeeId", apid);
            DataSet ds = runQuery.RunCasjobs(cmd, "QuickLook: Summary");
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (reader.Read())
                {
                    qra = (double)reader["ra"];
                    qdec = (double)reader["dec"];

                }
            }
            cmd = QuickLookQueries.getObjIDFromEq;
            cmd = cmd.Replace("@qra", qra.ToString());
            cmd = cmd.Replace("@qdec", qdec.ToString());
            cmd = cmd.Replace("@searchRadius", (0.5 / 60).ToString());
            setObjectInfo(cmd);
        }

        



    }









}
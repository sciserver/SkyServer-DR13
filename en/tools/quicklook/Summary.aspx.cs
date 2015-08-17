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

        protected Int16? run = null;
        protected Int16? rerun = null;
        protected byte? camcol = null;
        protected Int16? field = null;
        protected Int16? obj = null;


     
        protected void Page_Load(object sender, EventArgs e)
        {
           
            
            globals = (Globals)Application[Globals.PROPERTY_NAME];
            master = (ObjectQuickLook)Page.Master;
            Session["QuickLookObjectInfo"] = objectInfo;
          
            if (Request.QueryString.Keys.Count == 0)
            {
                id = globals.ExploreDefault;
            }

            try
            {
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

                    if (key == "run")
                    {
                        try { string s = Request.QueryString["run"]; run = string.Equals(s, "") ? run : Convert.ToInt16(s); }
                        catch { }
                    }
                    if (key == "rerun")
                    {
                        try { string s = Request.QueryString["rerun"]; rerun = string.Equals(s, "") ? rerun : Convert.ToInt16(s); }
                        catch { }
                    }
                    if (key == "camcol")
                    {
                        try { string s = Request.QueryString["camcol"]; camcol = string.Equals(s, "") ? camcol : Convert.ToByte(s); }
                        catch { }
                    }
                    if (key == "field")
                    {
                        try { string s = Request.QueryString["field"]; field = string.Equals(s, "") ? field : Convert.ToInt16(s); }
                        catch { }
                    }
                    if (key == "obj")
                    {
                        try { string s = Request.QueryString["obj"]; obj = string.Equals(s, "") ? obj : Convert.ToInt16(s); }
                        catch { }
                    }

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
            catch { }

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
            else if (!id.HasValue && !specId.HasValue && (run.HasValue || rerun.HasValue || camcol.HasValue || field.HasValue || obj.HasValue)) setFrom5PartSDSS(run, rerun, camcol, field, obj);
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
                    objectInfo.ra = reader["ra"] is DBNull ? null : (double?)reader["ra"];
                    objectInfo.dec = reader["dec"] is DBNull ? null : (double?)reader["dec"];

                    objectInfo.mjd = reader["mjd"] is DBNull ? null : (int?)reader["mjd"];
                    objectInfo.plate = reader["plate"] is DBNull ? null : (short?)reader["plate"]; 
                    objectInfo.plateId = reader["plateId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["plateId"]);
                    objectInfo.fiberId = reader["fiberid"] is DBNull ? null : (short?)reader["fiberid"];
                    objectInfo.fieldId = reader["fieldId"] is DBNull ? null : Functions.BytesToHex((byte[])reader["fieldId"]);

                    objectInfo.run = reader["run"] is DBNull ? null : (short?)reader["run"];
                    objectInfo.rerun = reader["rerun"] is DBNull ? null : (short?)reader["rerun"];
                    objectInfo.camcol = reader["camcol"] is DBNull ? null : (byte?)reader["camcol"];
                    objectInfo.field = reader["field"] is DBNull ? null : (short?)reader["field"];

                    objectInfo.u = reader["u"] is DBNull ? "" : (string)reader["u"];
                    objectInfo.g = reader["g"] is DBNull ? "" : (string)reader["g"];
                    objectInfo.r = reader["r"] is DBNull ? "" : (string)reader["r"];
                    objectInfo.i = reader["i"] is DBNull ? "" : (string)reader["i"];
                    objectInfo.z = reader["z"] is DBNull ? "" : (string)reader["z"];
                    objectInfo.err_u = reader["err_u"] is DBNull ? "" : (string)reader["err_u"];
                    objectInfo.err_g = reader["err_g"] is DBNull ? "" : (string)reader["err_g"];
                    objectInfo.err_r = reader["err_r"] is DBNull ? "" : (string)reader["err_r"];
                    objectInfo.err_i = reader["err_i"] is DBNull ? "" : (string)reader["err_i"];
                    objectInfo.err_z = reader["err_z"] is DBNull ? "" : (string)reader["err_z"];
                    objectInfo.otype = reader["otype"] is DBNull ? "" : (string)reader["otype"];
                    objectInfo.spectralClass = reader["spectralClass"] is DBNull ? "" : (string)reader["spectralClass"];
                    objectInfo.flags = reader["flags"] is DBNull ? "" : (string)reader["flags"];
                    objectInfo.redshift = reader["redshift"] is DBNull ? null : (float?)reader["redshift"];  
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
            cmd = cmd.Replace("@searchRadius", (globals.EqSearchRadius).ToString());
            //cmd = cmd.Replace("@searchRadius", (0.5 / 60).ToString());
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


        private void setFrom5PartSDSS(Int16? Run, Int16? Rerun, byte? Camcol, Int16? Field, Int16? Obj)
        {
            string cmd = QuickLookQueries.getpmtsFrom5PartSDSS;
            cmd = cmd.Replace("@run", Run == null ? "null" : Run.ToString());
            cmd = cmd.Replace("@rerun", Rerun == null ? "null" : Rerun.ToString());
            cmd = cmd.Replace("@camcol", Camcol == null ? "null" : Camcol.ToString());
            cmd = cmd.Replace("@field", Field == null ? "null" : Field.ToString());
            cmd = cmd.Replace("@obj", Obj == null ? "null" : Obj.ToString());
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
            cmd = cmd.Replace("@searchRadius", (globals.EqSearchRadius).ToString());
            //cmd = cmd.Replace("@searchRadius", (0.5 / 60).ToString());
            setObjectInfo(cmd);
        }

        



    }









}
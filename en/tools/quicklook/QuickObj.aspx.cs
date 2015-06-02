using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Globalization;
using SkyServer;

namespace SkyServer.Tools.QuickLook
{
    public partial class QuickObj : System.Web.UI.Page
    {
        const string ZERO_ID    = "0x0000000000000000";

        long? id = null;
        long? sid = null;
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

        short? run = null;
        short? rerun = null;
        short? camcol = null;
        short? field = null;

        Globals globals;

        protected void Page_Load(object sender, EventArgs e)
        {
            globals = (Globals)Application[Globals.PROPERTY_NAME];

            using (SqlConnection oConn = new SqlConnection(globals.ConnectionString))
            {
                oConn.Open();
                using (SqlCommand oCmd = oConn.CreateCommand())
                {
                    oCmd.CommandText =
                        "SELECT TOP 1 p.objID from PhotoObj p " +
                        "JOIN SpecObj s ON s.bestObjID=p.objID " +
                        "JOIN USNO u ON u.objID = p.objID " +
                        "JOIN FIRST f ON f.objid=p.objid " +
                        "WHERE p.type = 3 AND u.propermotion > 0 AND f.peak > 0";
                    using (SqlDataReader reader = oCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            id = reader.GetInt64(0);
                        }
                    }
                } // using SqlCommand

                foreach (string key in Request.QueryString.Keys)
                {
                    if (key == "id")
                    {
                        string s = Request.QueryString["id"];
                        id = Utilities.ParseId(s);
                        /*
                        try
                        {
                            if (s != null & !"".Equals(s))
                            {
                                if (s.StartsWith("0x"))
                                    id = long.Parse(s.Substring(2), NumberStyles.AllowHexSpecifier);
                                else
                                    id = long.Parse(s);
                            }  
                        }
                        catch (Exception ex) 
                        {
                            id = null;
                        }
                        */
                    }
                    if (key == "sid")
                    {
                        string s = Request.QueryString["sid"];
                        sid = Utilities.ParseId(s);
                        /*
                        try
                        {
                            if (s != null & !"".Equals(sid))
                            {
                                if (s.StartsWith("0x"))
                                    sid = long.Parse(s.Substring(2), NumberStyles.AllowHexSpecifier);
                                else
                                    sid = long.Parse(s);
                            }
                        }
                        catch (Exception ex) { }
                        */ 
                    }
                    if (key == "ra") qra = double.Parse(Request.QueryString["ra"]); // need to parse J2000
                    if (key == "dec") qdec = double.Parse(Request.QueryString["dec"]); // need to parse J2000
                    if (key == "plate") plate = short.Parse(Request.QueryString["plate"]);
                    if (key == "mjd") mjd = int.Parse(Request.QueryString["mjd"]);
                    if (key == "fiber") fiber = short.Parse(Request.QueryString["fiber"]);
                }

                getObjPmts(oConn);

                Response.Redirect("quicksummary.aspx?id=" + ((objId == null) ? "" : objId)
                    + "&spec=" + ((specObjId == null) ? "" : specObjId));

            } // using SqlConnection
        }

        private void getObjPmts(SqlConnection oConn)
        {
            if (fiber.HasValue && plate.HasValue) pmtsFromPlfib(oConn, plate, mjd, fiber);
            else if (qra.HasValue && qdec.HasValue) pmtsFromEq(oConn, qra, qdec);
            else if (sid.HasValue) pmtsFromSpec(oConn, sid);
            else if (id.HasValue && !sid.HasValue) pmtsFromPhoto(oConn, id);
        }

        private void pmtsFromPlfib(SqlConnection oConn, short? plate, int? mjd, short? fiber)
        {
            using (SqlCommand oCmd = oConn.CreateCommand())
            {
                oCmd.CommandText =
                    " select p.ra, p.dec," +
                    " cast(p.fieldId as binary(8)) as fieldId," +
                    " cast(s.specObjId as binary(8)) as specObjId," +
                    " cast(p.objId as binary(8)) as objId," +
                    " cast(s.plateId as binary(8)) as plateId, s.mjd, s.fiberId, q.plate" +
                    " from SpecObjAll s JOIN PhotoTag p ON s.bestobjid=p.objid JOIN PlateX q ON s.plateId=q.plateId" +
                    " where s.mjd = @mjd" +
                    " and s.fiberId = @fiber" +
                    " and q.plate = @plate";

                oCmd.Parameters.AddWithValue("@mjd", mjd);
                oCmd.Parameters.AddWithValue("@fiber", fiber);
                oCmd.Parameters.AddWithValue("@plate", plate);

                using (SqlDataReader reader = oCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ra = reader.GetDouble(0);
                        dec = reader.GetDouble(1);
                        fieldId = Functions.BytesToHex(reader.GetSqlBytes(2).Buffer);
                        specObjId = Functions.BytesToHex(reader.GetSqlBytes(3).Buffer);
                        objId = Functions.BytesToHex(reader.GetSqlBytes(4).Buffer);
                        plateId = Functions.BytesToHex(reader.GetSqlBytes(5).Buffer);
                        mjd = reader.GetInt32(6);
                        fiber = reader.GetInt16(7);
                        plate = reader.GetInt16(8);
                    }
                } // using SqlDataReader
            } // using SqlCommand
        }

        private void pmtsFromEq(SqlConnection oConn, double? qra, double? qdec)
        {
            using (SqlCommand oCmd = oConn.CreateCommand())
            {
                oCmd.CommandText =
                    " select top 1 p.ra, p.dec, p.run, p.rerun, p.camcol, p.field," +
                    " cast(p.fieldId as binary(8)) as fieldId," +
                    " cast(p.specObjId as binary(8)) as specObjId," +
                    " cast(p.objId as binary(8)) as objId" +
                    " from PhotoTag p, dbo.fGetNearbyObjAllEq(@qra,@qdec,@eqSearchRadius) n" +
                    " where p.objId=n.objId order by n.mode asc, n.distance asc";
                oCmd.Parameters.AddWithValue("@qra", qra);
                oCmd.Parameters.AddWithValue("@qdec", qdec);
                oCmd.Parameters.AddWithValue("@eqSearchRadius", globals.EqSearchRadius);

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

                // get the plateId from the specObj, if it exists

                if (specObjId != null && !ZERO_ID.Equals(specObjId))
                {
                    oCmd.Parameters.Clear();
                    oCmd.CommandText =
                        " select cast(s.plateId as binary(8)) as plateId, s.mjd, s.fiberId, q.plate" +
                        " from SpecObjAll s JOIN PlateX q ON s.plateId=q.plateId where specObjId=@specObjId";

                    oCmd.Parameters.AddWithValue("@specObjId", Int64.Parse(specObjId.Substring(2), NumberStyles.AllowHexSpecifier));

                    using (SqlDataReader reader = oCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            plateId = Functions.BytesToHex(reader.GetSqlBytes(0).Buffer);
                            mjd = reader.GetInt32(1);
                            fiber = reader.GetInt16(2);
                            plate = reader.GetInt16(3);
                        }
                    } // using SqlDataReader
                }
            } // using SqlCommand
        }

        private void pmtsFromSpec(SqlConnection oConn, long? sid)
        {
            using (SqlCommand oCmd = oConn.CreateCommand())
            {
                oCmd.CommandText =
                    " select p.ra, p.dec," +
                    " cast(p.fieldId as binary(8)) as fieldId," +
                    " cast(s.specObjId as binary(8)) as specObjId," +
                    " cast(p.objId as binary(8)) as objId," +
                    " cast(s.plateId as binary(8)) as plateId, s.mjd, s.fiberId, q.plate" +
                    " from SpecObjAll s JOIN PhotoTag p ON s.bestobjId=p.objid JOIN PlateX q ON s.plateId=q.plateId" +
                    " where s.specObjId=@sid";

                oCmd.Parameters.AddWithValue("@sid", sid);

                using (SqlDataReader reader = oCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ra = reader.GetDouble(0);
                        dec = reader.GetDouble(1);
                        fieldId = Functions.BytesToHex(reader.GetSqlBytes(2).Buffer);
                        specObjId = Functions.BytesToHex(reader.GetSqlBytes(3).Buffer);
                        objId = Functions.BytesToHex(reader.GetSqlBytes(4).Buffer);
                        plateId = Functions.BytesToHex(reader.GetSqlBytes(5).Buffer);
                        mjd = reader.GetInt32(6);
                        fiber = reader.GetInt16(7);
                        plate = reader.GetInt16(8);
                    }
                } // using SqlDataReader
            } //using SqlCommand
        }

        private void pmtsFromPhoto(SqlConnection oConn, long? id)
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
                    " where p.objId=dbo.fObjId(@id)";

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

                // get the plateId and fiberId from the specObj, if it exists

                if (specObjId != null && !ZERO_ID.Equals(specObjId))
                {
                    oCmd.Parameters.Clear();
                    oCmd.CommandText =
                        " select cast(s.plateId as binary(8)) as plateId, s.mjd, s.fiberId, q.plate" +
                        " from SpecObjAll s JOIN PlateX q ON s.plateId=q.plateId where specObjId=@specObjId";
                    oCmd.Parameters.AddWithValue("@specObjId", long.Parse(specObjId.Substring(2), NumberStyles.AllowHexSpecifier));

                    using (SqlDataReader reader = oCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            plateId = Functions.BytesToHex(reader.GetSqlBytes(0).Buffer);
                            mjd = reader.GetInt32(1);
                            fiber = reader.GetInt16(2);
                            plate = reader.GetInt16(3);
                        }
                    } // using SqlDataReader
                }
            } // using SqlCommand
        }

    }
}
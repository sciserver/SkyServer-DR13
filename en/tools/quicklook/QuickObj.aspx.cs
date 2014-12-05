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
        protected void Page_Load(object sender, EventArgs e)
        {
            string queryString = Request.QueryString.ToString();
            Response.Redirect("QuickSummary.aspx?" + queryString);        
        }

        //private void pmtsFromPlfib(SqlConnection oConn, short? plate, int? mjd, short? fiber)
        //{
        //    using (SqlCommand oCmd = oConn.CreateCommand())
        //    {
        //        oCmd.CommandText =
        //            " select p.ra, p.dec," +
        //            " cast(p.fieldId as binary(8)) as fieldId," +
        //            " cast(s.specObjId as binary(8)) as specObjId," +
        //            " cast(p.objId as binary(8)) as objId," +
        //            " cast(s.plateId as binary(8)) as plateId, s.mjd, s.fiberId, q.plate" +
        //            " from SpecObjAll s JOIN PhotoTag p ON s.bestobjid=p.objid JOIN PlateX q ON s.plateId=q.plateId" +
        //            " where s.mjd = @mjd" +
        //            " and s.fiberId = @fiber" +
        //            " and q.plate = @plate";

        //        oCmd.Parameters.AddWithValue("@mjd", mjd);
        //        oCmd.Parameters.AddWithValue("@fiber", fiber);
        //        oCmd.Parameters.AddWithValue("@plate", plate);

        //        using (SqlDataReader reader = oCmd.ExecuteReader())
        //        {
        //            if (reader.Read())
        //            {
        //                ra = reader.GetDouble(0);
        //                dec = reader.GetDouble(1);
        //                fieldId = Functions.BytesToHex(reader.GetSqlBytes(2).Buffer);
        //                specObjId = Functions.BytesToHex(reader.GetSqlBytes(3).Buffer);
        //                objId = Functions.BytesToHex(reader.GetSqlBytes(4).Buffer);
        //                plateId = Functions.BytesToHex(reader.GetSqlBytes(5).Buffer);
        //                mjd = reader.GetInt32(6);
        //                fiber = reader.GetInt16(7);
        //                plate = reader.GetInt16(8);
        //            }
        //        } // using SqlDataReader
        //    } // using SqlCommand
        //}        

    }
}
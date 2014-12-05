using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Globalization;

namespace SkyServer.Tools.QuickLook
{
    public partial class QuickLook : System.Web.UI.MasterPage
    {
        protected const string ZERO_ID = "0x0000000000000000";

        double? ra;
        double? dec;
        int? mjd;
        short? fiber;
        protected int? plate;


        string objId;
        string specObjId;

        protected int tabwidth = 128;
        protected long? id = 1237654386268439267;
        protected Globals globals;
        protected HRefs hrefs;
        protected string enUrl;

        protected string url;

        protected void Page_Load(object sender, EventArgs e)
        {
            globals = (Globals)Application[Globals.PROPERTY_NAME];
            hrefs = new HRefs();
            enUrl = getEnURL();
            url = ResolveClientUrl("~/en");
            string s = Request.QueryString["id"];
            if (s != null & !"".Equals(s))
            id = Utilities.ParseId(s);
            
            //using (SqlConnection oConn = new SqlConnection(globals.ConnectionString))
            //{
            //    oConn.Open();
            //    if (id.HasValue) pmtsFromPhoto(oConn, id);
            //}

            hrefs.Quicklook = "quickobj.aspx?id=" + id;
            hrefs.Explore = "../explore/obj.aspx?id=" + id;
            hrefs.FindingChart = "../chart/chart.aspx?ra=" + ra + "&dec=" + dec;

            if (plate.HasValue && mjd.HasValue && fiber.HasValue)
            {
                hrefs.Quicklook = "quickobj.aspx?plate=" + plate + "&mjd=" + mjd + "&fiber=" + fiber;
                hrefs.Explore = "../explore/obj.aspx?plate=" + plate + "&mjd=" + mjd + "&fiber=" + fiber;
            }
            else
            {
                if (ra.HasValue && dec.HasValue)
                {
                    hrefs.Quicklook = "quickobj.aspx?ra=" + ra + "&dec=" + dec;
                    hrefs.Explore = "../explore/obj.aspx?ra=" + ra + "&dec=" + dec;
                }
                else
                {
                    if (specObjId != null && !ZERO_ID.Equals(specObjId))
                    {
                        hrefs.Quicklook = "quickobj.aspx?sid=" + specObjId;
                        hrefs.Explore = "../explore/obj.aspx?sid=" + specObjId;
                    }
                }
            }

            hrefs.Search = "search.aspx?id=" + objId;
            hrefs.Eq = "javascript:setEq(\"" + ra + "\",\"" + dec + "\");";
            hrefs.SDSS = "javascript:setSDSS(\"" + objId + "\");";
            hrefs.Id = "javascript:setId(\"" + objId + "\");";
            hrefs.Plfib = "javascript:setPlfib(\"" + specObjId + "\");";

            hrefs.SaveBook = "javascript:saveBook(\"" + objId + "\");";
            hrefs.ShowBook = "javascript:showNotes();";

            hrefs.Tutorial = "quicklook.swf";
            hrefs.Examples = "help.aspx#examples";

            hrefs.Print = "framePrint();";
            hrefs.Help = "help.aspx"; 	
        }

        public void pmtsFromPhoto(SqlConnection oConn, long? id)
        {
            using (SqlCommand oCmd = oConn.CreateCommand())
            {
                oCmd.CommandText =
                    " select p.ra, p.dec," +
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
                        specObjId = Utilities.BytesToHex(reader.GetSqlBytes(2).Buffer);
                        objId = Utilities.BytesToHex(reader.GetSqlBytes(3).Buffer);
                    }
                }
                // get the plateId and fiberId from the specObj, if it exists

                if (specObjId != null && !ZERO_ID.Equals(specObjId))
                {
                    oCmd.Parameters.Clear();
                    oCmd.CommandText =
                        " select s.mjd, s.fiberId, q.plate" +
                        " from SpecObjAll s JOIN PlateX q ON s.plateId=q.plateId where specObjId=@specObjId";
                    oCmd.Parameters.AddWithValue("@specObjId", long.Parse(specObjId.Substring(2), NumberStyles.AllowHexSpecifier));
                    using (SqlDataReader reader = oCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            mjd = reader.GetInt32(0);
                            fiber = reader.GetInt16(1);
                            plate = reader.GetInt16(2);
                        }
                    }
                }
            } // using SqlCommand
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
    }
}
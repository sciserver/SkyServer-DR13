using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

namespace SkyServer.Tools.Explore
{
    public partial class ApogeeTest : System.Web.UI.Page
    {
        Globals globals;
        protected ApogeeInfo apogeeInfo;
        protected string apogeeSpecThumbnail;
        
        protected string apogeeSpecImage;
        protected string spectrumLink;
        protected string fitsLink;

        protected void Page_Load(object sender, EventArgs e)
        {
            string requestUri = ConfigurationManager.AppSettings["TEST_CasJobsRequestUri"];

            globals = (Globals)Application[Globals.PROPERTY_NAME];
            using (SqlConnection connection = new SqlConnection(globals.ConnectionString))
            {
                connection.Open();

                if (Request["id"] != null) 
                {
                    //apogeeInfo = new ApogeeInfoSQL(connection, Request["id"]);
                    apogeeInfo = new ApogeeInfoREST(requestUri, Request["id"]);
                }
                else if (Request["plate"] != null && Request["mjd"] != null && Request["fiberid"] != null)
                {
                    long plate = long.Parse(Request["plate"]);
                    long mjd = long.Parse(Request["mjd"]);
                    long fiberid = long.Parse(Request["fiberid"]);

                    //apogeeInfo = new ApogeeInfoSQL(connection, plate, mjd, fiberid);
                    apogeeInfo = new ApogeeInfoREST(requestUri, plate, mjd, fiberid);
                }
                else if (Request["ra"] != null && Request["dec"] != null)
                {
                    double radius = (Request["radius"] == null) ? ApogeeInfo.DEFAULT_RADIUS : double.Parse(Request["radius"]);
                    double ra = double.Parse(Request["ra"]);
                    double dec = double.Parse(Request["dec"]);

                    //apogeeInfo = new ApogeeInfoSQL(connection, ra, dec, radius);
                    apogeeInfo = new ApogeeInfoREST(requestUri, ra, dec, radius);
                }
                else throw new Exception("Required parameters missing: id | (ra, dec, [radius]) | (plate, mjd, fiberid)");
            }

            /* Generate links */
            if (apogeeInfo != null) 
            {
                string specApogeeLink = globals.ApogeeSpectrumLink + "?apogee_id=" + apogeeInfo.apogee_id;
                string doWeNeedC = (apogeeInfo.commiss == 1) ? "C" : "";

                apogeeSpecImage = globals.ApogeeFitsLink + apogeeInfo.location_id + "/plots/apStar" + doWeNeedC + "-s3-" + HttpUtility.UrlEncode(apogeeInfo.apogee_id) + ".gif";
                spectrumLink = globals.ApogeeSpectrumLink + "?locid=" + apogeeInfo.location_id + "&commiss=" + apogeeInfo.commiss + "&apogeeid=" + apogeeInfo.apogee_id;
                fitsLink = globals.ApogeeFitsLink + apogeeInfo.location_id + "/apStar" + doWeNeedC + "-s3-" + HttpUtility.UrlEncode(apogeeInfo.apogee_id) + ".fits";
            }
        }

    }
}
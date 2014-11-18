using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace SkyServer.Tools.Explore
{
    public partial class ApogeeControl : System.Web.UI.UserControl
    {
        protected Globals globals;
        protected ObjectExplorer master;

        public const double DEFAULT_RADIUS = 0.5 / 60;

        /* TABLE apogeeStar */
        public double ra;
        public double dec;
        public string apstar_id;
        public string apogee_id;
        public double glon;
        public double glat;
        public long location_id;
        public long commiss;
        public float vhelio_avg;          // Avg v<sub>helio</sub> (km/s)
        public float vscatter;            // Scatter in v<sub>helio</sub> (km/s)

        /* TABLE aspcapStar */
        public float teff;                // Best-fit temperature (K)
        public float teff_err;            // Temp error
        public float logg;                // Surface Gravity log<sub>10</sub>(g)
        public float logg_err;            // log(g) error
        public float metals;              // Metallicity [Fe/H]
        public float metals_err;          // Metal error
        public float alphafe;             // [&alpha;/Fe]
        public float alphafe_err;         // [&alpha;/Fe] error

        /* TABLE apogeeObject */
        public float j;                   // 2MASS j
        public float h;                   // 2MASS h
        public float k;                   // 2MASS k
        public float j_err;
        public float h_err;
        public float k_err;
        public float? mag_4_5;            // 4.5 micron magnitude
        public float? mag_4_5_err;        // 4.5 micron magnitude error
        public string src_4_5;            // 4.5 micron magnitude source

        /* Flags */
        public string apogeeTarget1N;      // APOGEE target flags 1
        public string apogeeTarget2N;      // APOGEE target flags 2
        public string apogeeStarFlagN;     // Star flags
        public string apogeeAspcapFlagN;   // Processing flags (ASPCAP)


        /* TABLE apogeeVisit */
        public string visit_id;
        public long plate;
        public long mjd;
        public long fiberid;
        public string dateobs;
        public float vrel;

        public const string FIND_NEAREST = @"join (select top 1 apstar_id from dbo.fGetNearestApogeeStarEq(@ra, @dec, @radius)) s on a.apstar_id = s.apstar_id";
        public const string FIND_APSTAR_ID = @"where a.apstar_id = @id";
        public const string FIND_APOGEE_ID = @"where a.apogee_id = @id";
        public const string FIND_PLFIB = @"join apogeeVisit v on a.apogee_id = v.apogee_id
        where 
        v.plate = @plate and 
        v.mjd = @mjd and 
        v.fiberID = @fiberid";

        protected string apogeeSpecThumbnail;

        protected string apogeeSpecImage;
        protected string spectrumLink;
        protected string fitsLink;

        string[] injection = new string[] { "--", ";", "/*", "*/", "'", "\"" };
        string command;

        protected bool isData = false;

        protected RunQuery runQuery;
      
        /* Visits */
        public List<ApogeeVisit> visits = new List<ApogeeVisit>();

        protected void Page_Load(object sender, EventArgs e)
        {
            globals = (Globals)Application[Globals.PROPERTY_NAME];
            master = (ObjectExplorer)Page.Master;
            runQuery = new RunQuery();
            if (master.apid != null && !master.apid.Equals(""))
            {
                apogeeID(master.apid);
                ReadInfoFromDbReader();
                ReadApogeeLinks();
                ReadVisitsFromDbReader();
            }
        } 

        protected void ReadInfoFromDbReader()  
        {
            DataSet ds = runQuery.RunCasjobs(command);
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
             //BASE_QUERY + FIND_NEAREST;
                if (reader.Read()) // Only one row expected
                {
                    ra = (double)reader["ra"];
                    dec = (double)reader["dec"];
                    apstar_id = (string)reader["apstar_id"];
                    apogee_id = (string)reader["apogee_id"];
                    glon = (double)reader["glon"];
                    glat = (double)reader["glat"];
                    location_id = (long)reader["location_id"];
                    commiss = (long)reader["commiss"];
                    vhelio_avg = (float)reader["vhelio_avg"];
                    vscatter = (float)reader["vscatter"];

                    teff = (float)reader["teff"];
                    teff_err = (float)reader["teff_err"];
                    logg = (float)reader["logg"];
                    logg_err = (float)reader["logg_err"];
                    metals = (float)reader["metals"];
                    metals_err = (float)reader["metals_err"];
                    alphafe = (float)reader["alphafe"];
                    alphafe_err = (float)reader["alphafe_err"];

                    j = (float)reader["j"];
                    h = (float)reader["h"];
                    k = (float)reader["k"];
                    j_err = (float)reader["j_err"];
                    h_err = (float)reader["h_err"];
                    k_err = (float)reader["k_err"];
                    mag_4_5 = reader["mag_4_5"] is DBNull ? null : (float?)reader["mag_4_5"];
                    mag_4_5_err = reader["mag_4_5_err"] is DBNull ? null : (float?)reader["mag_4_5_err"];
                    src_4_5 = (string)reader["src_4_5"];

                    apogeeTarget1N = (string)reader["apogeeTarget1N"];
                    apogeeTarget2N = (string)reader["apogeeTarget2N"];
                    apogeeStarFlagN = (string)reader["apogeeStarFlagN"];
                    apogeeAspcapFlagN = (string)reader["apogeeAspcapFlagN"];

                    isData = true;
                }
                else {
                    isData = false;
                    //throw new Exception("APOGEE data not found"); 
                }
            }        
        }

        private void ReadApogeeLinks() {
            string specApogeeLink = globals.ApogeeSpectrumLink + "?apogee_id=" + apogee_id;
            string doWeNeedC = (commiss == 1) ? "C" : "";

            apogeeSpecImage = globals.ApogeeFitsLink + location_id + "/plots/apStar" + doWeNeedC + "-s3-" + HttpUtility.UrlEncode(apogee_id) + ".gif";
            spectrumLink = globals.ApogeeSpectrumLink + "?locid=" + location_id + "&commiss=" + commiss + "&apogeeid=" + apogee_id;
            fitsLink = globals.ApogeeFitsLink + location_id + "/apStar" + doWeNeedC + "-s3-" + HttpUtility.UrlEncode(apogee_id) + ".fits";
        }

        protected void ReadVisitsFromDbReader()
        {

            string command = ExplorerQueries.APOGEEVISITS_BASE_QUERY;
            foreach (string s in injection)
            {  
                if (apogee_id != null && apogee_id.IndexOf(s) >= 0)
                {
                    throw new Exception("Invalid APOGEE ID: " + apogee_id);
                }
            }
            command = command.Replace("@id", "'" + apogee_id + "'");

            DataSet ds = runQuery.RunCasjobs(command);
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                while (reader.Read()) // Multiple rows expected
                {
                    ApogeeVisit v = new ApogeeVisit();
                    v.visit_id = (string)reader["visit_id"];
                    v.plate = (long)reader["plate"];
                    v.mjd = (long)reader["mjd"];
                    v.fiberid = (long)reader["fiberid"];
                    v.dateobs = (string)reader["dateobs"];
                    v.vrel = (float)reader["vrel"];
                    visits.Add(v);
                }
            }
        }
        
        public void apogeeRaDec( double ra, double dec, double radius)
        {
            command = ExplorerQueries.APOGEE_BASE_QUERY + FIND_NEAREST;
            command = command.Replace("@radius", radius.ToString());
            command = command.Replace("@ra", ra.ToString());
            command = command.Replace("@dec", dec.ToString());           
        }

        public void apogeeID( string id)
        {

            if (id.StartsWith("apogee")) { command = ExplorerQueries.APOGEE_BASE_QUERY + FIND_APSTAR_ID; }
            else { command = ExplorerQueries.APOGEE_BASE_QUERY + FIND_APOGEE_ID; }

            foreach (string s in injection)
            {
                if (id.IndexOf(s) >= 0)
                {
                    throw new Exception("Invalid APOGEE ID: " + id);
                }
            }
            command = command.Replace("@id", "'"+id+"'");            
        }


        public void apogeePlate(long plate, long mjd, long fiberid)
        {
            command = ExplorerQueries.APOGEE_BASE_QUERY + FIND_PLFIB;
            command = command.Replace("@plate", plate.ToString());
            command = command.Replace("@mjd", mjd.ToString());
            command = command.Replace("@fiberid", fiberid.ToString());            
        }
      
    }
}
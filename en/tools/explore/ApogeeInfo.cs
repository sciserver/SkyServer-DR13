using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data.Common;

namespace SkyServer.Tools.Explore
{
    public abstract class ApogeeInfo
    {
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

        /* Queries */
        public const string BASE_QUERY = @"select
  a.ra, 
  a.dec,
  a.apstar_id, 
  a.apogee_id, 
  a.glon, 
  a.glat, 
  a.location_id,
  a.commiss,
  a.vhelio_avg, 
  a.vscatter, 

  b.teff, 
  b.teff_err,
  b.logg, 
  b.logg_err,
  b.metals, 
  b.metals_err,  
  b.alphafe, 
  b.alphafe_err,

  c.j,
  c.h,
  c.k,
  c.j_err,
  c.h_err,
  c.k_err,
  case c.src_4_5 
    when 'none' then NULL 
    when 'WISE' then c.wise_4_5 
    when 'IRAC' then c.irac_4_5 
    end 
    as mag_4_5,
  case c.src_4_5 
    when 'none' then NULL 
    when 'WISE' then c.wise_4_5_err 
    when 'IRAC' then c.irac_4_5_err 
    end 
    as mag_4_5_err,
  c.src_4_5,

  dbo.fApogeeTarget1N(a.apogee_target1) as apogeeTarget1N,
  dbo.fApogeeTarget2N(a.apogee_target2) as apogeeTarget2N,
  dbo.fApogeeStarFlagN(a.starflag) as apogeeStarFlagN,
  dbo.fApogeeAspcapFlagN(aspcapflag) as apogeeAspcapFlagN

from apogeeStar a
join aspcapStar b on a.apstar_id = b.apstar_id
join apogeeObject c on a.apogee_id = c.apogee_id
";

        public const string FIND_NEAREST = @"join (select top 1 apstar_id from dbo.fGetNearestApogeeStarEq(@ra, @dec, @radius)) s on a.apstar_id = s.apstar_id";
        public const string FIND_APSTAR_ID = @"where a.apstar_id = @id";
        public const string FIND_APOGEE_ID = @"where a.apogee_id = @id";
        public const string FIND_PLFIB = @"join apogeeVisit v on a.apogee_id = v.apogee_id
where 
  v.plate = @plate and 
  v.mjd = @mjd and 
  v.fiberID = @fiberid";

        /* Visits */
        public List<ApogeeVisit> visits = new List<ApogeeVisit>();

        protected ApogeeInfo() { }

        protected void ReadInfoFromDbReader(DbDataReader reader)  
        {
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
            }
            else throw new Exception("APOGEE data not found");
        }

        protected void ReadVisitsFromDbReader(DbDataReader reader)
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
}
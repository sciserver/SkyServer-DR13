using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace SkyServer.Tools.Explore
{
    public class ApogeeInfo
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

        private ApogeeInfo() { }

        public ApogeeInfo(SqlConnection connection, double ra, double dec, double radius)
        {
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = BASE_QUERY + FIND_NEAREST;
                command.Parameters.AddWithValue("ra", ra);
                command.Parameters.AddWithValue("dec", dec);
                command.Parameters.AddWithValue("radius", radius);

                try { ReadInfo(command); }
                catch (Exception ex) { throw new Exception("ApogeeInfo ERROR (ra: " + ra + ", dec: " + dec + ", radius: " + radius + ")", ex); }
            }

            ReadVisits(connection);
        }

        public ApogeeInfo(SqlConnection connection, string id)
        {
            using (SqlCommand command = connection.CreateCommand())
            {
                if (id.StartsWith("apogee")) { command.CommandText = BASE_QUERY + FIND_APSTAR_ID; }
                else { command.CommandText = BASE_QUERY + FIND_APOGEE_ID; }

                command.Parameters.AddWithValue("id", id);

                try { ReadInfo(command); }
                catch (Exception ex) { throw new Exception("ApogeeInfo ERROR (id: " + id + ")", ex); }
            }

            ReadVisits(connection);
        }

        public ApogeeInfo(SqlConnection connection, long plate, long mjd, long fiberid)
        {
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = BASE_QUERY + FIND_PLFIB;
                command.Parameters.AddWithValue("plate", plate);
                command.Parameters.AddWithValue("mjd", mjd);
                command.Parameters.AddWithValue("fiberid", fiberid);

                try { ReadInfo(command); }
                catch (Exception ex) { throw new Exception("ApogeeInfo ERROR (plate: " + plate + ", mjd: " + mjd + ", fiberid: " + fiberid + ")", ex); }
            }

            ReadVisits(connection);
        }

        private void ReadInfo(SqlCommand command) 
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read()) // Only one row expected
                {
                    ra = reader.GetDouble(0);
                    dec = reader.GetDouble(1);
                    apstar_id = reader.GetString(2);
                    apogee_id = reader.GetString(3);
                    glon = reader.GetDouble(4);
                    glat = reader.GetDouble(5);
                    location_id = reader.GetInt64(6);
                    commiss = reader.GetInt64(7);
                    vhelio_avg = reader.GetFloat(8);
                    vscatter = reader.GetFloat(9);

                    teff = reader.GetFloat(10);
                    teff_err = reader.GetFloat(11);
                    logg = reader.GetFloat(12);
                    logg_err = reader.GetFloat(13);
                    metals = reader.GetFloat(14);
                    metals_err = reader.GetFloat(15);
                    alphafe = reader.GetFloat(16);
                    alphafe_err = reader.GetFloat(17);

                    j = reader.GetFloat(18);
                    h = reader.GetFloat(19);
                    k = reader.GetFloat(20);
                    j_err = reader.GetFloat(21);
                    h_err = reader.GetFloat(22);
                    k_err = reader.GetFloat(23);
                    mag_4_5 = (reader.IsDBNull(24) ? null : (float?)reader.GetFloat(24));
                    mag_4_5_err = (reader.IsDBNull(25) ? null : (float?)reader.GetFloat(25));        
                    src_4_5 = reader.GetString(26);

                    apogeeTarget1N = reader.GetString(27);
                    apogeeTarget2N = reader.GetString(28);
                    apogeeStarFlagN = reader.GetString(29);
                    apogeeAspcapFlagN = reader.GetString(30);
                }
                else throw new Exception("APOGEE data not found");
            }
        }

        private void ReadVisits(SqlConnection connection)
        {
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = ApogeeVisit.BASE_QUERY;
                command.Parameters.AddWithValue("id", apogee_id);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read()) // Multiple rows expected
                    {
                        ApogeeVisit v = new ApogeeVisit();

                        v.visit_id = reader.GetString(0);
                        v.plate = reader.GetInt64(1);
                        v.mjd = reader.GetInt64(2);
                        v.fiberid = reader.GetInt64(3);
                        v.dateobs = reader.GetString(4);
                        v.vrel = reader.GetFloat(5);

                        visits.Add(v);
                    }
                }
            }
        }

    }
}
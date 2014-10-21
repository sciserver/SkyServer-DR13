using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace SkyServer.Tools.Explore
{
    public class ApogeeInfoSQL : ApogeeInfo
    {
        public ApogeeInfoSQL(SqlConnection connection, double ra, double dec, double radius)
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

        public ApogeeInfoSQL(SqlConnection connection, string id)
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

        public ApogeeInfoSQL(SqlConnection connection, long plate, long mjd, long fiberid)
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
                ReadInfoFromDbReader(reader);
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
                    ReadVisitsFromDbReader(reader);
                }
            }
        }
    }
}
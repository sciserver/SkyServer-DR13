using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Collections.Specialized;

namespace SkyServer.Tools.Explore
{
    public class SpectralParameters
    {

        public SpectralParameters() { }

        public SpectralParameters(long? objid, long? specid, string connectionstring)
        {
            this.objid = objid.ToString();
            this.specid = specid.ToString();
            this.connectionstring = connectionstring;
        }

        public string getSpectroQuery(string specid, string objid)
        {
            string iQuery = " select s.plate,s.mjd,fiberid ,s.instrument as 'Spectrograph' ,class, str(z,7,3) as 'Redshift (z)', str(zerr,10,5) as 'Redshift error' ";
            iQuery += " , dbo.fSpecZWarningN(zWarning) as 'Redshift flags'";
            iQuery += " ,s.survey, s.programname, s.scienceprimary as 'primary', (x.nspec-1) as 'Other spec' ";
            iQuery += " ,s.sourcetype, str(velDisp,6,2) as 'Velocity dispersion (km/s)', str(velDispErr,7,3) as 'veldisp_error' ";
            iQuery += " ,case s.survey ";
            iQuery += " WHEN 'sdss' THEN (select(dbo.fPrimtargetN(s.legacy_target1)+' '+dbo.fPrimTargetN(s.legacy_target2)+' '+dbo.fSpecialTarget1N(s.special_target1)))";
            iQuery += " WHEN 'boss' THEN (select str(boss_target1)+','+str(ancillary_target1)+','+str(ancillary_target2))";
            iQuery += " WHEN 'segue1' THEN (select dbo.fSEGUE1target1N(segue1_target1)+','+dbo.fSEGUE1target2N(segue1_target2)) ";
            iQuery += " WHEN 'segue2' THEN (select dbo.fSEGUE2target1N(segue2_target1)+','+ dbo.fSEGUE2target2N(segue2_target2) ) ";
            //iQuery += " --WHEN 'apogee' THEN (select apogee_target1,apogee_target2 ) ";
            iQuery += " ELSE ' No Data ' ";
            iQuery += " END ";
            iQuery += " as targeting_flags ";
            iQuery += " from  PlateX p ,SpecObjAll s ";
            iQuery += " join (select bestobjid, count(*) as nspec from specobjall where bestobjid=" + objid + " group by bestobjid) x on s.bestobjid=x.bestobjid ";
            iQuery += " where p.plateId=s.plateId and  s.specObjId=" + specid;

            return iQuery;
        }

        private string objid;
        private string specid;
        private string connectionstring;

        public int plate;
        public int mjd;
        public int fiberid;
        public string instrument;
        public string objclass;
        public string redshift_z;
        public string redshift_err;
        public string redshift_flags;
        public string survey;
        public string programname;
        public string primary;
        public string other_spec;
        public string sourcetype;
        public string vel_disp;
        public string veldisp_err;

        public NameValueCollection runDb() {
            NameValueCollection nvc = new NameValueCollection();
            using (SqlConnection oConn = new SqlConnection(connectionstring))
            {
                oConn.Open();
                using (SqlCommand oCmd = oConn.CreateCommand())
                {
                    oCmd.CommandText = getSpectroQuery(specid,objid);
                    using (SqlDataReader reader = oCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.HasRows)
                            {
                                //plate = reader.GetInt16(0);
                                //mjd = reader.GetInt32(1);
                                //fiberid = reader.GetInt16(2);
                                //instrument = reader.GetValue(3).ToString();
                                //objclass = reader.GetValue(4).ToString();
                                //redshift_z = reader.GetValue(5).ToString();
                                //redshift_err = reader.GetValue(6).ToString();
                                //redshift_flags = reader.GetValue(7).ToString();
                                //survey = reader.GetValue(8).ToString();
                                //programname = reader.GetValue(9).ToString();
                                //primary = reader.GetValue(10).ToString();
                                //other_spec = reader.GetValue(11).ToString();
                                //sourcetype = reader.GetValue(12).ToString();
                                //vel_disp = reader.GetValue(13).ToString();
                                //veldisp_err = reader.GetValue(14).ToString();
                                //string[] names = new string[reader.FieldCount];
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    nvc.Add(reader.GetName(i), reader.GetValue(i).ToString());
                                }
                            }
                        }
                    }
                }
            }
            return nvc;
        }
    }
}


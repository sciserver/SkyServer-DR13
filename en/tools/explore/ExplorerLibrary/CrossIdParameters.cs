using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Data.SqlClient;
namespace SkyServer.Tools.Explore
{
    public class CrossIdParameters
    {
        private string objId = "";
        private string wiseLink = "";
        private string connectionString = "";

        public CrossIdParameters() { }

        public CrossIdParameters(String connectionString, String objid, String wiselink) {
            this.objId = objid;
            this.wiseLink = wiselink;
            this.connectionString = connectionString;
        }

        public CrossIdParameters(String objid, String wiselink) {
            this.objId = objid;
            this.wiseLink = wiselink;
            
        }
       
        public NameValueCollection runQuery(string whichQuery) {
            
            NameValueCollection nvc = new NameValueCollection();
            String query = "";
            switch (whichQuery) {
                case "USNO" : query = " select 'USNO' as Catalog, str(10*propermotion,6,2)+' &plusmn; '+str(sqrt(power(muraerr,2)+power(mudecerr,2)),8,3) as 'Proper motion (mas/yr)', str(angle,6,3) as 'PM angle (deg E)' from USNO where objId=";  break;
                case "FIRST": query = " select 'FIRST' as Catalog, str(peak,8,2)+' &plusmn; '+str(rms,8,2) as 'Peak flux (mJy)', major as 'Major axis (arcsec)', minor as 'Minor axis (arcsec)' from First where objId="; break;
                case "ROSAT": query = " select 'ROSAT' as Catalog, cps, hr1, hr2, ext from ROSAT where objId="; break;
                case "RC3"  : query = " select 'RC3' as Catalog, hubble as 'Hubble type', str(m21,5,2)+' &plusmn; '+str(m21err,6,3) as '21 cm magnitude', hi as 'Neutral Hydrogen Index' from RC3 where objId="; break;
                case "WISE":  query = " select 'WISE' as Catalog,w.w1mag,w.w2mag,w.w3mag,w.w4mag,'<a href=''" + wiseLink + "''>Link</a>' as 'Full WISE data' from WISE_xmatch x join WISE_allsky w on x.wise_cntr=w.cntr where x.sdss_objid="; break;
                case "2MASS": query = " select '2MASS' as Catalog, str(j,7,3) as 'J', h as 'H', k as 'K_s', phQual from TwoMASS where objId="; break;
                default: break;
            }
            query += objId;
            nvc = runDb(query);

            return nvc;
        }

        private NameValueCollection runDb(string query) {
            NameValueCollection nvc = new NameValueCollection();
                            
            using (SqlConnection oConn = new SqlConnection(connectionString))
            {
                oConn.Open();
                using (SqlCommand oCmd = oConn.CreateCommand())
                {
                    oCmd.CommandText = query;
                    using (SqlDataReader reader = oCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.HasRows)
                            {
                                //string[] names = new string[reader.FieldCount];
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    nvc.Add(reader.GetName(i),reader.GetValue(i).ToString());
                                }
                            }
                            
                        }
                    }
                }
            }
            return nvc;
        }

        private NameValueCollection runCasjobs() { 
            NameValueCollection nvc = new NameValueCollection();
            return nvc;
        }
        
    }
}
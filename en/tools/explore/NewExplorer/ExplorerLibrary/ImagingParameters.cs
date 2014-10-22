using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Collections.Specialized;
namespace SkyServer.en.tools.explore.NewExplorer.ExplorerLibrary
{
    public class ImagingParameters
    {
        //--- phototag
        public string flag; //0
        public double ra = 0;//1
        public double dec = 0;//2
        public int run;//3
        public int rerun;//4
        public int camcol;//5
        public long field;//6
        public string fieldId;//7
        public string objId;//8
        
        //--- PhotoObjAll
        public int clean;//14
        public string otype;//15
        public double u;//16
        public double g;//17
        public double r;//18
        public double i;//19
        public double z;//20
        public double err_u;//21
        public double err_g;//22
        public double err_r;//23
        public double err_i;//24
        public double err_z;//25
        
        //--- PhotoObj
        public string mode;//26
        public int mjdNum;//27
        public int otherObs;//28
        public long parentId;//29
        public int nchild;//30
        public string extinction_r;//31
        public string petrorad_r;//32
        
        //--- PhotoZ, photoZRF
        public string photoZ_KD;//33
        public string photoZ_RF;//34
        public string galaxyZoo_Morph;//35

        /// <summary>
        /// ExplorerQueries  hold all the queries
        /// </summary>
        protected ExplorerQueries exQuery;
        public string connectionString = "";       

        public ImagingParameters(){      
            
            
        }

        public string iQuery = "";

        public NameValueCollection magnitudes = new NameValueCollection();
        public NameValueCollection magErrors  = new NameValueCollection();
        public NameValueCollection restPhoto1 = new NameValueCollection();
        public NameValueCollection restPhoto2 = new NameValueCollection();

        public NameValueCollection runQueryDB()
        {
            NameValueCollection nvc = new NameValueCollection();
            using (SqlConnection oConn = new SqlConnection(connectionString))
            {
                oConn.Open();
                using (SqlCommand oCmd = oConn.CreateCommand())
                {
                    oCmd.CommandText = iQuery;
                    using (SqlDataReader reader = oCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.HasRows)
                            {
                                flag = reader.GetString(0);
                                ra = reader.GetDouble(1);
                                dec = reader.GetDouble(2);
                                run = reader.GetInt16(3);
                                rerun = reader.GetInt16(4);
                                camcol = reader.GetByte(5);
                                field = reader.GetInt16(6);
                                //fieldId = Functions.BytesToHex(reader.GetSqlBytes(7).Buffer);                            
                                //objId = Functions.BytesToHex(reader.GetSqlBytes(8).Buffer);
                                //photoObjall
                                clean = reader.GetInt32(9);
                                otype = reader.GetString(10);
                                //magnitudes
                                u = Convert.ToDouble(reader.GetValue(11)); 
                                    magnitudes.Add(reader.GetName(11), reader.GetValue(11).ToString());
                                g = Convert.ToDouble(reader.GetValue(12)); 
                                    magnitudes.Add(reader.GetName(12), reader.GetValue(12).ToString());
                                r = Convert.ToDouble(reader.GetValue(13)); 
                                    magnitudes.Add(reader.GetName(13), reader.GetValue(13).ToString());
                                i = Convert.ToDouble(reader.GetValue(14));
                                    magnitudes.Add(reader.GetName(14), reader.GetValue(14).ToString());
                                z = Convert.ToDouble(reader.GetValue(15)); 
                                    magnitudes.Add(reader.GetName(15), reader.GetValue(15).ToString());
                                //mag errors
                                err_u = Convert.ToDouble(reader.GetValue(16)); 
                                    magErrors.Add(reader.GetName(16), reader.GetValue(16).ToString());
                                err_g = Convert.ToDouble(reader.GetValue(17)); 
                                    magErrors.Add(reader.GetName(17), reader.GetValue(17).ToString());
                                err_r = Convert.ToDouble(reader.GetValue(18)); 
                                    magErrors.Add(reader.GetName(18), reader.GetValue(18).ToString());
                                err_i = Convert.ToDouble(reader.GetValue(19)); 
                                    magErrors.Add(reader.GetName(19), reader.GetValue(19).ToString());
                                err_z = Convert.ToDouble(reader.GetValue(20)); 
                                    magErrors.Add(reader.GetName(20), reader.GetValue(20).ToString());
                                ////--- PhotoObj
                                mode = reader.GetString(21);
                                mjdNum = reader.GetInt32(22);
                                    restPhoto1.Add(reader.GetName(22), reader.GetValue(22).ToString());
                                    restPhoto1.Add(reader.GetName(21), reader.GetValue(21).ToString());
                                int otherObs = reader.GetInt32(23);
                                    restPhoto1.Add(reader.GetName(23), reader.GetValue(23).ToString());
                                parentId = reader.GetInt64(24);
                                    restPhoto1.Add(reader.GetName(24), reader.GetValue(24).ToString());
                                nchild = reader.GetInt16(25);
                                    restPhoto1.Add(reader.GetName(25), reader.GetValue(25).ToString());
                                extinction_r = reader.GetValue(26).ToString();
                                    restPhoto1.Add(reader.GetName(26), reader.GetValue(26).ToString());
                                petrorad_r = reader.GetValue(27).ToString();
                                    restPhoto1.Add(reader.GetName(27), reader.GetValue(27).ToString());
                                ////--- PhotoZ, photoZRF
                                photoZ_KD = reader.GetValue(28).ToString();
                                    restPhoto2.Add(reader.GetName(28), reader.GetValue(28).ToString());
                                photoZ_RF = reader.GetValue(29).ToString();
                                    restPhoto2.Add(reader.GetName(29), reader.GetValue(29).ToString());    
                                galaxyZoo_Morph = reader.GetValue(30).ToString();
                                    restPhoto2.Add(reader.GetName(30), reader.GetValue(30).ToString());
                                /// put all in nv pair
                                for (int j = 0; j < reader.FieldCount; j++)
                                {
                                    nvc.Add(reader.GetName(j), reader.GetValue(j).ToString());
                                }
                            }
                        }        
                    }
                }
            }
            return nvc;
        }

        public void runQueryCasjobs() { 
        
        }

        public void  getImagingQuery(string objid, string sdssurl)
        {

            string flagsLink = sdssurl + "algorithms/photo_flags_recommend.php";

            iQuery = " select";
            //--- phototag
            iQuery += " dbo.fPhotoFlagsN(pt.flags) as '<a href=" + flagsLink + ">Flags <img src=./images/offsite_black.png /></a>',pt.ra, pt.dec, pt.run, pt.rerun, pt.camcol, pt.field, ";
            iQuery += " cast(pt.fieldId as binary(8)) as fieldId, cast(pt.objId as binary(8)) as objId, ";
            //--PhotoObjall
            iQuery += " pa.clean,  dbo.fPhotoTypeN(pa.type) as otype, ";
            iQuery += " str(pa.u,7,2) as u, str(pa.g,7,2) as g, str(pa.r,7,2) as r, str(pa.i,7,2) as i, str(pa.z,7,2)as z, ";
            iQuery += " str(pa.err_u,7,2) as err_u, str(pa.err_g,7,2) as err_g, str(pa.err_r,7,2) as err_r, str(pa.err_i,7,2) as err_i, str(pa.err_z,7,2) as err_z, ";
            //-- photoObj
            iQuery += " dbo.fPhotoModeN(po.mode) as mode,po.mjd as 'Image MJD',  (po.nDetect-1) as 'Other observations', po.parentID, po.nChild, str(po.extinction_r,7,2) as extinction_r,";
            iQuery += " str(po.petroRad_r,9,2)+' &plusmn; '+str(po.petroRadErr_r,10,3) as 'PetroRad_r (arcmin)',";
            //--- photz,photozRF,zoospec 
            iQuery += " (str(phz.z,7,3)+' &plusmn; '+str(phz.zerr,8,4))as 'photoZ (KD-tree method)', (str(phzrf.z,7,3)+' &plusmn; '+str(phzrf.zerr,8,4)) as 'photoZ (RF method)', ";
            iQuery += " case (1*zz.spiral+10*zz.elliptical+100*zz.uncertain) when 1 then 'Spiral' when 10 then 'Elliptical' when 100 then 'Uncertain' else '-' end as 'Galaxy Zoo 1 morphology' ";
            //all joins
            iQuery += " from PhotoTag pt  ";
            iQuery += " left outer join PhotoObj po on po.objid = pt.objid";
            iQuery += " left outer join Photoz phz on po.objid=phz.objid ";
            iQuery += " left outer join PhotozRF phzrf on po.objid=phzrf.objid ";
            iQuery += " left outer join zooSpec zz on po.objid=zz.objid ";
            iQuery += " left outer join field f on f.fieldID=pt.fieldID ";
            iQuery += " left outer join photoobjall pa with (nolock)on  pa.objid = po.objid ";
            iQuery += " where pt.objId= " + objid;
        }
    }
}
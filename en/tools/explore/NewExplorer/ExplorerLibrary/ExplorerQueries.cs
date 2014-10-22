using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkyServer.en.tools.explore.NewExplorer.ExplorerLibrary
{
    public class ExplorerQueries
    {

        public String getImagingQuery(string objid, string sdssurl)
        {
            string flagsLink = sdssurl + "algorithms/photo_flags_recommend.php";
            String iQuery = "";
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
            return iQuery;
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

        public String runQuery(string whichQuery, string objId)
        {   
            String query = "";
            String wiseLink = "";
            switch (whichQuery)
            {
                case "USNO": query = " select 'USNO' as Catalog, str(10*propermotion,6,2)+' &plusmn; '+str(sqrt(power(muraerr,2)+power(mudecerr,2)),8,3) as 'Proper motion (mas/yr)', str(angle,6,3) as 'PM angle (deg E)' from USNO where objId="; break;
                case "FIRST": query = " select 'FIRST' as Catalog, str(peak,8,2)+' &plusmn; '+str(rms,8,2) as 'Peak flux (mJy)', major as 'Major axis (arcsec)', minor as 'Minor axis (arcsec)' from First where objId="; break;
                case "ROSAT": query = " select 'ROSAT' as Catalog, cps, hr1, hr2, ext from ROSAT where objId="; break;
                case "RC3": query = " select 'RC3' as Catalog, hubble as 'Hubble type', str(m21,5,2)+' &plusmn; '+str(m21err,6,3) as '21 cm magnitude', hi as 'Neutral Hydrogen Index' from RC3 where objId="; break;
                case "WISE": query = " select 'WISE' as Catalog,w.w1mag,w.w2mag,w.w3mag,w.w4mag,'<a href=''" + wiseLink + "''>Link</a>' as 'Full WISE data' from WISE_xmatch x join WISE_allsky w on x.wise_cntr=w.cntr where x.sdss_objid="; break;
                case "2MASS": query = " select '2MASS' as Catalog, str(j,7,3) as 'J', h as 'H', k as 'K_s', phQual from TwoMASS where objId="; break;
                default: break;
            }
            query += objId;           

            return query;
        }
    }
}


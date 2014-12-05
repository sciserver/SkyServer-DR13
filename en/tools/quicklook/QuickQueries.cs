using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkyServer.Tools.QuickLook
{
    public class QuickQueries
    {

        public static string quick1 = @"SELECT TOP 1 p.objID from PhotoObj p JOIN SpecObj s ON s.bestObjID=p.objID 
                                      JOIN USNO u ON u.objID = p.objID JOIN FIRST f ON f.objid=p.objid 
                                      WHERE p.type = 3 AND u.propermotion > 0 AND f.peak > 0";

        public static string pmtsFromPlfib = @" select p.ra, p.dec, cast(p.fieldId as binary(8)) as fieldId, cast(s.specObjId as binary(8)) as specObjId,
                                             cast(p.objId as binary(8)) as objId, cast(s.plateId as binary(8)) as plateId, s.mjd, s.fiberId, q.plate
                                             from SpecObjAll s JOIN PhotoTag p ON s.bestobjid=p.objid JOIN PlateX q ON s.plateId=q.plateId
                                             where s.mjd = @mjd and s.fiberId = @fiber and q.plate = @plate";

        public static string pmtsFromEq = @" select top 1 p.ra, p.dec, p.run, p.rerun, p.camcol, p.field,
                                          cast(p.fieldId as binary(8)) as fieldId,
                                          cast(p.specObjId as binary(8)) as specObjId,
                                          cast(p.objId as binary(8)) as objId
                                          from PhotoTag p, dbo.fGetNearbyObjAllEq(@qra,@qdec,@eqSearchRadius) n
                                          where p.objId=n.objId order by n.mode asc, n.distance asc";

        public static string getObjPmts = @"select ra, dec, fieldId, specObjId, objId, dbo.fPhotoTypeN(type) as otype, rowc, colc
                                          from PhotoObjAll with (nolock) where objId=@id";

        public static string getSpecPmts = "select specobjid as specobjid from specobjall where specobjid=@specId";

        public static string pmtsFromeq2 = @" select cast(s.plateId as binary(8)) as plateId, s.mjd, s.fiberId, q.plate
                                           from SpecObjAll s JOIN PlateX q ON s.plateId=q.plateId where specObjId=@specObjId";

        public static string pmtsFromSpec = @" select p.ra, p.dec,
                                            cast(p.fieldId as binary(8)) as fieldId,
                                            cast(s.specObjId as binary(8)) as specObjId,
                                            cast(p.objId as binary(8)) as objId,
                                            cast(s.plateId as binary(8)) as plateId, s.mjd, s.fiberId, q.plate
                                            from SpecObjAll s JOIN PhotoTag p ON s.bestobjId=p.objid JOIN PlateX q ON s.plateId=q.plateId
                                            where s.specObjId=@sid";

        public static string pmtsFromPhoto = @" select p.ra, p.dec, p.run, p.rerun, p.camcol, p.field,
                                             cast(p.fieldId as binary(8)) as fieldId,
                                             cast(s.specobjid as binary(8)) as specObjId,
                                             cast(p.objId as binary(8)) as objId 
                                             from PhotoTag p 
                                             left outer join SpecObjAll s ON s.bestobjid=p.objid 
                                             where p.objId=dbo.fObjId(@id)";

        public static string pmtsFromPhoto2 = @" select cast(s.plateId as binary(8)) as plateId, s.mjd, s.fiberId, q.plate
                                              from SpecObjAll s JOIN PlateX q ON s.plateId=q.plateId where specObjId=@specObjId";

        public static string uPhoto = @" select str(u,7,2) as 'Ultraviolet (u): ', str(err_u,7,2) as ' '
                                      from PhotoObjAll with (nolock) where objID=@id";     
                    
        public static string gPhoto = @" select str(g,7,2) as 'Green (g): ', str(err_g,7,2) as ' '
                                      from PhotoObjAll with (nolock) where objID=@id";       	                
   	
   	    public static string rPhoto = @" select str(r,7,2) as 'Red (r): ', str(err_r,7,2) as ' '
                                      from PhotoObjAll with (nolock) where objID=@id";       	                
   	
        public static string iPhoto = @" select str(i,7,2) as 'Infrared - 7600 &Aring; (i): ', str(err_i,7,2) as ' '
                                     from PhotoObjAll with (nolock) where objID=@id";    

        public static string zPhoto = @" select str(z,7,2) as 'Infrared - 9100 &Aring; (z): ', str(err_z,7,2) as ' '
	                                 from PhotoObjAll with (nolock) where objID=@id";    
   	                
        public static string flagsPhoto = @"select dbo.fPhotoFlagsN(flags) as flags, psfMagErr_g
                                           from PhotoObjAll with (nolock) where objID=@id";

        public static string specClass = @" select class as 'Spectral classification (Class)', z as 'Redshift (z):', 
                                    plate,mjd,fiberid from SpecObjAll s where s.specObjId=@specObjId";
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SkyServer;
using System.Data;

namespace SkyServer.Tools.Explore
{
    public partial class DisplayResults : System.Web.UI.Page
    {
        protected string cmd = null;
        protected string name = null;
        protected string url = null;
        protected ObjectExplorer master;
        protected RunQuery runQuery = new RunQuery();
        protected DataSet ds;
        ObjectInfo o;
        protected void Page_Load(object sender, EventArgs e)
        {
            master = (ObjectExplorer)Page.Master;
            cmd  = Request.QueryString["cmd"];
            name = Request.QueryString["name"];
            url  = Request.QueryString["url"];
            o = (ObjectInfo)Session["objectInfo"];
            if(cmd == null || cmd.Equals(""))
            getQuery();

            executeQuery();
        }

        private void executeQuery() {

            try {
                ds = runQuery.RunCasjobs(cmd);  
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }

        }

        private void getQuery() {

            switch (name) {
                case "PhotoObj": 
                        cmd = ExplorerQueries.PhotoObjQuery.Replace("@objId", o.objId); break;
                case "PhotoTag":
                        cmd = ExplorerQueries.PhotoTagQuery.Replace("@objId", o.id.ToString()); break;
                case "photoZ":
                        cmd = ExplorerQueries.PhotoZ.Replace("@objId", o.objId); break;
                case "photozRF":
                        cmd = ExplorerQueries.PhotozRF.Replace("@objId", o.objId); break;

                case "Field":
                        cmd = ExplorerQueries.FieldQuery.Replace("@fieldId", o.fieldId); break;
                case "Frame":
                        cmd = ExplorerQueries.FrameQuery.Replace("@fieldId", o.fieldId); break;
                
                case "SpecObj":
                        cmd = ExplorerQueries.SpecObjQuery.Replace("@specId", o.specId.ToString()); break;
                case "sppLines":
                        cmd = ExplorerQueries.sppLinesQuery.Replace("@specId", o.specId.ToString()); break;
                case "sppParams":
                        cmd = ExplorerQueries.sppParamsQuery.Replace("@specId", o.specId.ToString()); break;
                case "galSpecLine":
                        cmd = ExplorerQueries.galSpecLineQuery.Replace("@specId", o.specId.ToString()); break;
                case "galSpecIndx":
                        cmd = ExplorerQueries.galSpecIndexQuery.Replace("@specId", o.specId.ToString()); break;
                case "galSpecInfo":
                        cmd = ExplorerQueries.galSpecInfoQuery.Replace("@specId", o.specId.ToString()); break;
                case "stellarMassStarFormingPort":
                        cmd = ExplorerQueries.stellarMassStarformingPortQuery.Replace("@specId", o.specId.ToString()); break;  
                case "stellarMassPassivePort":
                        cmd = ExplorerQueries.stellarMassPassivePortQuery.Replace("@specId", o.specId.ToString()); break;
                case "emissionlinesPort":
                        cmd = ExplorerQueries.emissionLinesPortQuery.Replace("@specId", o.specId.ToString()); break;
                case "stellarMassPCAWiscBC03":
                        cmd = ExplorerQueries.stellarMassPCAWiscBC03Query.Replace("@specId", o.specId.ToString()); break;
                case "stellarMassPCAWiscM11":
                        cmd = ExplorerQueries.stellarMassPCAWiscM11Query.Replace("@specId", o.specId.ToString()); break;
                case "stellarMassFSPSGranEarlyDust":
                        cmd = ExplorerQueries.stellarMassFSPSGranEarlyDust.Replace("@specId", o.specId.ToString()); break;
                case "stellarMassFSPSGranEarlyNoDust":
                        cmd = ExplorerQueries.stellarMassFSPSGranEarlyNoDust.Replace("@specId", o.specId.ToString()); break;
                case "stellarMassFSPSGranWideDust":
                        cmd = ExplorerQueries.stellarMassFSPSGranWideDust.Replace("@specId", o.specId.ToString()); break;
                case "stellarMassFSPSGranWideNoDust":
                        cmd = ExplorerQueries.stellarMassFSPSGranWideNoDust.Replace("@specId", o.specId.ToString()); break;
                
                case "apogeeStar":
                        cmd= ExplorerQueries.apogeeStar.Replace("@apid", o.apid.ToString()); break;
                case "aspcapStar":
                        cmd= ExplorerQueries.aspcapStar.Replace("@apid", o.apid.ToString()); break;

                default: cmd = ""; break;


            }
        }
    }
}

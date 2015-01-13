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
        protected string objId = null;
        protected string specId = null;
        protected string apid = null;
        protected string fieldId = null;


        protected ObjectExplorer master;
        protected RunQuery runQuery = new RunQuery();
        protected DataSet ds;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            master = (ObjectExplorer)Page.Master;
            

            foreach (string key in Request.QueryString.Keys)
            {
                if(key == "apid")
                    apid= HttpUtility.UrlEncode(Request.QueryString["apid"]);
                objId = Request.QueryString["id"];
                specId = Request.QueryString["spec"];
                
                fieldId = Request.QueryString["field"];

                cmd = Request.QueryString["cmd"];
                name = Request.QueryString["name"];
                url = Request.QueryString["url"];
            }
           
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
                        cmd = ExplorerQueries.PhotoObjQuery.Replace("@objId", objId); break;
                case "PhotoTag":
                        cmd = ExplorerQueries.PhotoTagQuery.Replace("@objId", objId); break;
                case "photoZ":
                        cmd = ExplorerQueries.PhotoZ.Replace("@objId", objId); break;
                case "photozRF":
                        cmd = ExplorerQueries.PhotozRF.Replace("@objId", objId); break;

                case "Field":
                        cmd = ExplorerQueries.FieldQuery.Replace("@fieldId", fieldId); break;
                case "Frame":
                        cmd = ExplorerQueries.FrameQuery.Replace("@fieldId", fieldId); break;
                
                case "SpecObj":
                        cmd = ExplorerQueries.SpecObjQuery.Replace("@specId", specId); break;
                case "sppLines":
                        cmd = ExplorerQueries.sppLinesQuery.Replace("@specId", specId); break;
                case "sppParams":
                        cmd = ExplorerQueries.sppParamsQuery.Replace("@specId", specId); break;
                case "galSpecLine":
                        cmd = ExplorerQueries.galSpecLineQuery.Replace("@specId", specId); break;
                case "galSpecIndx":
                        cmd = ExplorerQueries.galSpecIndexQuery.Replace("@specId", specId); break;
                case "galSpecInfo":
                        cmd = ExplorerQueries.galSpecInfoQuery.Replace("@specId", specId); break;
                case "stellarMassStarFormingPort":
                        cmd = ExplorerQueries.stellarMassStarformingPortQuery.Replace("@specId", specId); break;  
                case "stellarMassPassivePort":
                        cmd = ExplorerQueries.stellarMassPassivePortQuery.Replace("@specId", specId); break;
                case "emissionlinesPort":
                        cmd = ExplorerQueries.emissionLinesPortQuery.Replace("@specId", specId); break;
                case "stellarMassPCAWiscBC03":
                        cmd = ExplorerQueries.stellarMassPCAWiscBC03Query.Replace("@specId",specId); break;
                case "stellarMassPCAWiscM11":
                        cmd = ExplorerQueries.stellarMassPCAWiscM11Query.Replace("@specId", specId); break;
                case "stellarMassFSPSGranEarlyDust":
                        cmd = ExplorerQueries.stellarMassFSPSGranEarlyDust.Replace("@specId", specId); break;
                case "stellarMassFSPSGranEarlyNoDust":
                        cmd = ExplorerQueries.stellarMassFSPSGranEarlyNoDust.Replace("@specId", specId); break;
                case "stellarMassFSPSGranWideDust":
                        cmd = ExplorerQueries.stellarMassFSPSGranWideDust.Replace("@specId", specId); break;
                case "stellarMassFSPSGranWideNoDust":
                        cmd = ExplorerQueries.stellarMassFSPSGranWideNoDust.Replace("@specId", specId); break;
                
                case "apogeeStar":
                        cmd= ExplorerQueries.apogeeStar.Replace("@apid", apid); break;
                case "aspcapStar":
                        cmd= ExplorerQueries.aspcapStar.Replace("@apid", apid); break;

                default: cmd = ""; break;


            }
        }
    }
}

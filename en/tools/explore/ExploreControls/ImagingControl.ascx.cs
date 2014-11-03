using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace SkyServer.Tools.Explore
{
    public partial class ImagingControl : System.Web.UI.UserControl
    {        
        protected Globals globals;
        protected ObjectExplorer master;        

        //--- phototag
        protected string flag; //0
        protected double ra = 0;//1
        protected double dec = 0;//2
        protected int run;//3
        protected int rerun;//4
        protected int camcol;//5
        protected long field;//6
        protected string fieldId;//7
        protected string objId;//8

        //--- PhotoObjAll
        public int clean;//14
        protected string otype;//15
        protected double u;//16
        protected double g;//17
        protected double r;//18
        protected double i;//19
        protected double z;//20
        protected double err_u;//21
        protected double err_g;//22
        protected double err_r;//23
        protected double err_i;//24
        protected double err_z;//25

        //--- PhotoObj
        protected string mode;//26
        protected int mjdNum;//27
        protected int otherObs;//28
        protected long parentId;//29
        protected int nchild;//30
        protected string extinction_r;//31
        protected string petrorad_r;//32

        //--- PhotoZ, photoZRF
        protected string photoZ_KD;//33
        protected string photoZ_RF;//34
        protected string galaxyZoo_Morph;//35

        protected string sdssUrl;

        public void Page_Load(object sender, EventArgs e)
        {
            globals = (Globals)Application[Globals.PROPERTY_NAME];            
            master  = (ObjectExplorer)Page.Master;
            objId   = Request.QueryString["id"];
            sdssUrl = globals.SdssUrl;
            runQuery();
        }

        private void runQuery()
        {
            DataSet ds = master.runQuery.RunCasjobs(master.exploreQuery.getImagingQuery(objId, sdssUrl));
            using (DataTableReader reader = ds.Tables[0].CreateDataReader())
            {
                if (reader.Read())
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

                        g = Convert.ToDouble(reader.GetValue(12));

                        r = Convert.ToDouble(reader.GetValue(13));

                        i = Convert.ToDouble(reader.GetValue(14));

                        z = Convert.ToDouble(reader.GetValue(15));


                        ////mag errors
                        err_u = Convert.ToDouble(reader.GetValue(16));

                        err_g = Convert.ToDouble(reader.GetValue(17));

                        err_r = Convert.ToDouble(reader.GetValue(18));

                        err_i = Convert.ToDouble(reader.GetValue(19));

                        err_z = Convert.ToDouble(reader.GetValue(20));


                        ////--- PhotoObj
                        mode = reader.GetString(21);

                        mjdNum = reader.GetInt32(22);

                        int otherObs = reader.GetInt32(23);

                        parentId = reader.GetInt64(24);

                        nchild = reader.GetInt16(25);

                        extinction_r = reader.GetValue(26).ToString();

                        petrorad_r = reader.GetValue(27).ToString();

                        ////--- PhotoZ, photoZRF
                        photoZ_KD = reader.GetValue(28).ToString();

                        photoZ_RF = reader.GetValue(29).ToString();

                        galaxyZoo_Morph = reader.GetValue(30).ToString();
                    }
                }
            }
        }
    }
}
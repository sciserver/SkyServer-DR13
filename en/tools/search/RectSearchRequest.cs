using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkyServer.Tools.Search
{
    public class RectSearchRequest
    {
        private int limit;
        private string url;
        private string format;

        private double min_ra;
        private double min_dec;
        private double max_ra;
        private double max_dec;

        private string check_u;
        private string check_g;
        private string check_r;
        private string check_i;
        private string check_z;

        private double max_u;
        private double min_u;
        private double max_g;
        private double min_g;
        private double max_r;
        private double min_r;
        private double max_i;
        private double min_i;
        private double max_z;
        private double min_z;

        private string imageQuery;
        private string irQuery;

        public string ImageQuery { get { return imageQuery; } }
        public string IRQuery { get { return irQuery; } }
        public string Format { get { return format; } }

        public RectSearchRequest(string url, HttpRequest request) 
        {
            this.url = url;

            limit = int.Parse(request["limit"] ?? "0");
            format = request["format"] ?? "html";
            format = format.ToLower();

            SetRectArea(request);

            check_u = request["check_u"] ?? "false";
            check_g = request["check_g"] ?? "false";
            check_r = request["check_r"] ?? "false";
            check_i = request["check_i"] ?? "false";
            check_z = request["check_z"] ?? "false";

            max_u = double.Parse(request["max_u"] ?? "20");
            min_u = double.Parse(request["min_u"] ?? "0");
            max_g = double.Parse(request["max_g"] ?? "20");
            min_g = double.Parse(request["min_g"] ?? "0");
            max_r = double.Parse(request["max_r"] ?? "20");
            min_r = double.Parse(request["min_r"] ?? "0");
            max_i = double.Parse(request["max_i"] ?? "20");
            min_i = double.Parse(request["min_i"] ?? "0");
            max_z = double.Parse(request["max_z"] ?? "20");
            min_z = double.Parse(request["min_z"] ?? "0");

            Utilities.ValueCheckOrFail("min_ra", min_ra, 0, 360);
            Utilities.ValueCheckOrFail("max_ra", max_ra, 0, 360);
            Utilities.RangeCheckOrFail("dec", min_dec, max_dec, -90, 90);

            double delta_ra = max_ra - min_ra;
            double delta_dec = max_dec - min_dec;

            Utilities.ValueCheckOrFail("max_ra-min_ra", delta_ra, 0, 0.2);
            Utilities.ValueCheckOrFail("max_dec-min_dec", delta_dec, 0, 0.2);

            if (check_u == "u") Utilities.RangeCheckOrFail("u", min_u, max_u, 0, 35);
            if (check_g == "g") Utilities.RangeCheckOrFail("g", min_g, max_g, 0, 35);
            if (check_r == "r") Utilities.RangeCheckOrFail("r", min_r, max_r, 0, 35);
            if (check_i == "i") Utilities.RangeCheckOrFail("i", min_i, max_i, 0, 35);
            if (check_z == "z") Utilities.RangeCheckOrFail("z", min_z, max_z, 0, 35);

            SetImageQuery();
            SetIRQuery();
        }

        private void SetRectArea(HttpRequest request)
        {
            double min_ra = Utilities.parseRA(request["min_ra"]);
            double max_ra = Utilities.parseRA(request["max_ra"]);
            double min_dec = Utilities.parseDec(request["min_dec"]);
            double max_dec = Utilities.parseDec(request["max_dec"]);

            string whichway = request["whichway"];

            if (whichway == "galactic")
            {
                // If the user requested galactic coordinates, the "ra" and "dec" they entered is actually B and L, respectively. Convert.

                double new_min_ra = Utilities.glon2ra(min_ra, min_dec);
                double new_max_ra = Utilities.glon2ra(max_ra, max_dec);
                double new_min_dec = Utilities.glat2dec(min_ra, min_dec);
                double new_max_dec = Utilities.glat2dec(max_ra, max_dec);

                if (new_min_ra > new_max_ra)
                {
                    min_ra = new_max_ra;
                    max_ra = new_min_ra;
                }
                else
                {
                    min_ra = new_min_ra;
                    max_ra = new_max_ra;
                }

                if (new_min_dec > new_max_dec)
                {
                    min_dec = new_max_dec;
                    max_dec = new_min_dec;
                }
                else
                {
                    min_dec = new_min_dec;
                    max_dec = new_max_dec;
                }
            }

            this.min_ra = min_ra;
            this.min_dec = min_dec;
            this.max_ra = max_ra;
            this.max_dec = max_dec;
        }

        private void SetImageQuery()
        {
            string sql;

            sql = "SELECT ";
            sql += (limit <= 0) ? "" : " TOP " + limit;

            if (format == "html")
            {
                sql += " ''<a target=INFO href=" + url + "/tools/explore/obj.aspx?id='' + cast(p.objId as varchar(20)) + ''>'' + cast(p.objId as varchar(20)) + ''</a>'' as objID,\n";
            }
            else
            {
                sql += " p.objid,\n";
            }
            sql += "   p.run, p.rerun, p.camcol, p.field, p.obj,\n";
            sql += "   p.type, p.ra, p.dec, p.u,p.g, p.r, p.i, p.z,\n";
            sql += "   p.Err_u, p.Err_g, p.Err_r, p.Err_i, p.Err_z\n";
            sql += "   FROM fGetObjFromRect(" + min_ra + "," + max_ra + "," + min_dec + "," + max_dec + ") n,";
            sql += "   PhotoPrimary p\n";
            sql += "   WHERE n.objID=p.objID ";

            int ccount = 1;

            if (check_u == "u")
                sql += Utilities.ccut("u", ccount++, min_u, max_u);
            if (check_g == "g")
                sql += Utilities.ccut("g", ccount++, min_g, max_g);
            if (check_r == "r")
                sql += Utilities.ccut("r", ccount++, min_r, max_r);
            if (check_i == "i")
                sql += Utilities.ccut("i", ccount++, min_i, max_i);
            if (check_z == "z")
                sql += Utilities.ccut("z", ccount++, min_z, max_z);

            this.imageQuery = sql;
        }

        private void SetIRQuery()
        {
            string sql;

            sql = "SELECT ";
            sql += (limit <= 0) ? "" : " TOP " + limit;

            if (format == "html")
            {
                sql += " ''<a target=INFO href=" + url + "/tools/explore/summary.aspx?apid='' + cast(p.apstar_id as varchar(40)) + ''>'' + cast(p.apstar_id as varchar(40)) + ''</a>'' as apstar_id,\n";
            }
            else
            {
                sql += " p.apstar_id, \n";
            }
            sql += "   p.apogee_id,p.ra, p.dec, p.glon, p.glat,\n";
            sql += "   p.vhelio_avg,p.vscatter,\n";
            sql += "   a.teff,a.logg,a.metals\n";
            sql += "   FROM apogeeStar p\n";
            sql += "   JOIN aspcapStar a on a.apstar_id = p.apstar_id\n";
            sql += "   WHERE ra BETWEEN " + min_ra + " AND " + max_ra + "\n";
            sql += "   AND dec BETWEEN " + min_dec + " AND " + max_dec + "\n";

            /*
            int ccount = 1;
            
            if (check_u == "u")
                sql += Utilities.ccut("u", ccount++, min_u, max_u);
            if (check_g == "g")
                sql += Utilities.ccut("g", ccount++, min_g, max_g);
            if (check_r == "r")
                sql += Utilities.ccut("r", ccount++, min_r, max_r);
            if (check_i == "i")
                sql += Utilities.ccut("i", ccount++, min_i, max_i);
            if (check_z == "z")
                sql += Utilities.ccut("z", ccount++, min_z, max_z);
            */
            this.irQuery = sql;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.IO;

namespace SkyServer.Tools.CrossId
{
    public partial class X_CrossId : System.Web.UI.Page
    {
        string[] ubody;
        string utype;
        string uvalue;

        string reSignedFloat = @"^((((\+|-)?\d+(\.\d*)?)|((\+|-)?(\d*\.)?\d+))([eE](\+|-){1}\d+)?)$";
        //string reObjID = @"\d{15,18}";
        //string reGator = @"^\\\s+";
        string reSplit = @"(\,|\s+)";
        //string reName = "\"(\\w+)\"";
        //string reColon = @"\w+: ";
        //string reBlank = @"^\s*";
        string reSelect = @"select";
        string rePobjid = @"p\.objid\s*,";
        int firstCol = 0;
        List<string> colNames = new List<string>();

        protected string url;
        protected Globals globals;

        string windows_name;
        string server_name;
        string remote_addr;

        int rowLimit = 500000;

        protected void Page_Load(object sender, EventArgs e)
        {
            url = ResolveClientUrl("~/en");
            globals = (Globals)Application[Globals.PROPERTY_NAME];

            windows_name = System.Environment.MachineName;
            server_name = Request.ServerVariables["SERVER_NAME"];
            remote_addr = Request.ServerVariables["REMOTE_ADDR"];

            string table;
            if (Request.Files.Count > 0 && Request.Files[0].ContentLength != 0)
            {
                table = (new StreamReader(Request.Files[0].InputStream)).ReadToEnd();
            }
            else
            {
                table = Request["paste"];
            }
            ubody = table.Split(new string[] {"\n"},StringSplitOptions.RemoveEmptyEntries);
            firstCol = int.Parse(Request["firstcol"]);
        }

        protected void ProcessRequest()
        {
            try
            {
                string searchType;

                getUploadFormat(Request["paste"]);

                if ("photo".Equals(Request["searchType"]))
                {
                    if ("ra-dec".Equals(Request["photoUpType"]))
                        searchType = "photo RA and dec";
                    else
                        searchType = "SDSS ID";
                }
                else if ("spectro".Equals(Request["searchType"]))
                {
                    if ("ra-dec".Equals(Request["spectroUpType"]))
                        searchType = "spectro RA and dec";
                    else
                        searchType = "plate, MJD and fiberID";
                }
                else //if ("apogee".Equals(Request["searchType"]))
                {
                    searchType = "APOGEE RA and dec";
                }

                if ("html".Equals(Request["format"]))
                {
                    Response.Write("<html><body>\n");
                    Response.Write("<h2>Uploading " + searchType + " list for cross-identification</h2>\n");
                }

                using (SqlConnection oConn = new SqlConnection(globals.ConnectionString))
                {
                    oConn.Open();

                    string cmd = UploadCmd();
                    sendSQL(oConn, cmd);

                    updateBatch(oConn);

                    if ("photo".Equals(Request["searchType"]))
                    {
                        if ("ra-dec".Equals(Request["photoUpType"]))
                        {
                            if ("allPrim".Equals(Request["photoScope"]) || "allObj".Equals(Request["photoScope"]))
                                getNearby(oConn, "");
                            else // if(f.photoScope.value=="nearPrim" || f.photoScope.value=="nearObj") 
                                getNearest(oConn, "");
                        }
                        else
                            getObjID(oConn);
                    }
                    else if ("spectro".Equals(Request["searchType"]))
                    {
                        if ("ra-dec".Equals(Request["spectroUpType"]))
                        {
                            if ("allPrim".Equals(Request["spectroScope"]) || "allObj".Equals(Request["spectroScope"]))
                                getNearby(oConn, "Spec");
                            else // if(f.spectroScope.value=="nearPrim" || f.spectroScope.value=="nearObj") 
                                getNearest(oConn, "Spec");
                        }
                        else
                            getPmf(oConn);
                    }
                    else //if ("apogee".Equals(Request["searchType"]))
                    {
                        getNearestApogee(oConn);
                    }
                }
            }
            catch (Exception ex)
            {
                ResponseAux.writeError(ex, Response);
                return;
            }

            if ("html".Equals(Request["format"]))
                Response.Write("</body></html>\n");
        }

        private void updateBatch(SqlConnection oConn)
        {
            string[] names = Regex.Split(uvalue, reSplit, RegexOptions.ExplicitCapture).Where(str => !str.Equals(String.Empty)).ToArray();
            using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM #upload", oConn)) 
            {
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                builder.QuotePrefix = "[";
                builder.QuoteSuffix = "]";
                
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                for (int i=0; i<ubody.Length; i++) 
                {
                    insertLine(ds,i,names,ubody[i]);
                }
                adapter.Update(ds);
            }
        }

        private void insertLine(DataSet ds, int row, string[] n, string line) {
            string[] v = Regex.Split(line, reSplit, RegexOptions.ExplicitCapture).Where(str => !str.Equals(String.Empty)).ToArray();
            DataRow dr = ds.Tables[0].NewRow();
	        dr["up_id"] = row;

            string[] values = (string[])v.Clone();
            string[] names = (string[])n.Clone();

            if ((Request["searchType"] == "apogee") && (Request["apogeeUpType"] == "l-b")) 
            {
                int il = -1;
                int ib = -1;
                for (int i = 0; i < names.Length; i++)
                {
                    if (names[i] == "l") { il = i; names[i] = "ra"; }
                    if (names[i] == "b") { ib = i; names[i] = "dec"; }
                }
                double glon = double.Parse(values[ib]);
                double glat = double.Parse(values[il]);
                values[il] = Utilities.glon2ra(glat, glon).ToString("F5", CultureInfo.InvariantCulture);
                values[ib] = Utilities.glat2dec(glat, glon).ToString("F5", CultureInfo.InvariantCulture);
            }

            for (int i = 0; i < names.Length; i++)
            {
                object o = Convert.ChangeType(values[i], dr.Table.Columns["up_" + names[i]].DataType);
                dr["up_" + names[i]] = o;
            }
	        //if( dbg==1 ) showLine("  --"+line+" =>#upload");

            ds.Tables[0].Rows.Add(dr);
        }

        private string getUploadFormat(string u)
        {
            //string[] body = u.Split('\n');
            string s = ubody[0];
            uvalue = "";

            // check for Gator format
            if (Regex.IsMatch(s, @"^\\\ \w+"))
            {
                // need to scroll forward to the beginning of data
                while (Regex.IsMatch(ubody[0], @"^\\")) ubody = ubody.Skip(1).ToArray();
                if (Regex.IsMatch(ubody[0], @"^\s*\|") && "".Equals(uvalue))
                {
                    uvalue = ubody[0].Replace("|", " ");
                    ubody = ubody.Skip(2).ToArray();
                }
                utype = "G";
                return "G";
            }

            // is it a comma/whitespace?
            string[] s2 = s.Split('#');			// allow for # as first char for column headers line
            if (s2.Length > 1)
                s = s2[1];
            string[] c = Regex.Split(s, reSplit, RegexOptions.ExplicitCapture).Where(str => !str.Equals(String.Empty)).ToArray();
            //colNames.AddRange(c);
            int n = c.Length;
            int fc = 0;
            fc = int.Parse(Request["firstcol"]);
            if (fc >= n)
                return Error("first data column number " + firstCol + " is larger than number of columns");

            int ndata = n - fc, nvalid;
            if (Request["searchType"] == "photo")
            {
                if (Request["photoUpType"] == "ra-dec")
                    nvalid = 2;
                else
                    nvalid = 5;
            }
            else if (Request["searchType"] == "spectro")
            {
                if (Request["spectroUpType"] == "ra-dec")
                    nvalid = 2;
                else
                    nvalid = 3;
            }
            else //if (Request["searchType"] == "apogee")
            {
                nvalid = 2;
            }

            if (ndata != nvalid)
                return Error("problem with upload file/text:<br> " + ndata + " data items in first line: '" + s + "'<br>Is the number of preceding columns correct?");

            // we have ndata values, are they numbers or text
            uvalue = "";

            //if( dbg == 1 ) showLine( "checking upload type\n" );

            if (Request["searchType"] == "photo")
            {
                if (Request["photoUpType"] == "run-rerun")
                {
                    // if the first line has objID (so no heading line)
                    if (Regex.IsMatch(c[n - 5], @"\d+"))
                    {
                        for (int i = 0; i < fc; i++)
                            uvalue += "col" + i + " ";
                        uvalue += "run rerun camcol field obj";
                        colNames.AddRange(Regex.Split(uvalue, reSplit, RegexOptions.ExplicitCapture).Where(str => !str.Equals(String.Empty)).ToArray());
                        utype = "N" + n;
                        return "N" + n;
                    }
                    else if (Regex.IsMatch(c[n - 5], @"^run$", RegexOptions.IgnoreCase) && Regex.IsMatch(c[n - 4], @"^rerun$", RegexOptions.IgnoreCase) &&
                         Regex.IsMatch(c[n - 3], @"^camcol$", RegexOptions.IgnoreCase) && Regex.IsMatch(c[n - 2], @"^field$", RegexOptions.IgnoreCase) &&
                         Regex.IsMatch(c[n - 1], @"^obj$", RegexOptions.IgnoreCase))
                    {
                        foreach (string i in c)
                            uvalue += " " + i;
                        colNames.AddRange(Regex.Split(uvalue, reSplit, RegexOptions.ExplicitCapture).Where(str => !str.Equals(String.Empty)).ToArray());
                        ubody = ubody.Skip(1).ToArray();
                        utype = "H" + n;
                        return "H" + n;
                    }
                    else
                        return Error("problem with upload file/text: incorrect header names on first line '" + s + "'");
                }
                else
                {
                    // if the first line has floats (ra/dec with no heading line)
                    if (Regex.IsMatch(c[n - 2], reSignedFloat))
                    {
                        for (int i = 0; i < fc; i++)
                            uvalue += "col" + i + " ";
                        uvalue += "ra dec";
                        colNames.AddRange(Regex.Split(uvalue, reSplit, RegexOptions.ExplicitCapture).Where(str => !str.Equals(String.Empty)).ToArray());
                        utype = "N" + n;
                        return "N" + n;
                    }
                    else if (Regex.IsMatch(c[n - 2], @"^ra$", RegexOptions.IgnoreCase) && Regex.IsMatch(c[n - 1], @"^dec$", RegexOptions.IgnoreCase))
                    {
                        foreach (string i in c)
                            uvalue += " " + i;
                        colNames.AddRange(Regex.Split(uvalue, reSplit, RegexOptions.ExplicitCapture).Where(str => !str.Equals(String.Empty)).ToArray());
                        ubody = ubody.Skip(1).ToArray();
                        utype = "H" + n;
                        return "H" + n;
                    }
                    else
                        return Error("problem with upload file/text: incorrect header names on first line '" + s + "'");
                }
            }
            else if (Request["searchType"] == "spectro")
            {
                //if( dbg == 1 ) showLine( "checking spectro fields\n" );
                if (Request["spectroUpType"] == "plate-mjd-fiber")
                {
                    // if the first line has plate (so no heading line)
                    if (Regex.IsMatch(c[n - 3], @"\d+"))
                    {
                        for (int i = 0; i < fc; i++)
                            uvalue += "col" + i + " ";
                        uvalue += "plate mjd fiber";
                        colNames.AddRange(Regex.Split(uvalue, reSplit, RegexOptions.ExplicitCapture).Where(str => !str.Equals(String.Empty)).ToArray());
                        utype = "N" + n;
                        return "N" + n;
                    }
                    else if (Regex.IsMatch(c[n - 3], @"^plate$", RegexOptions.IgnoreCase) && Regex.IsMatch(c[n - 2], @"^mjd$", RegexOptions.IgnoreCase) &&
                         Regex.IsMatch(c[n - 1], @"^fiber$", RegexOptions.IgnoreCase))
                    {
                        //if( dbg == 1 ) showLine( "plate-mjd-fiber list\n" );
                        foreach (string i in c)
                            uvalue += " " + i;
                        colNames.AddRange(Regex.Split(uvalue, reSplit, RegexOptions.ExplicitCapture).Where(str => !str.Equals(String.Empty)).ToArray());
                        ubody = ubody.Skip(1).ToArray();
                        utype = "H" + n;
                        return "H" + n;
                    }
                    else
                        return Error("problem with upload file/text: incorrect header names on first line '" + s + "'");
                }
                else
                {
                    // if the first line has floats (ra/dec with no heading line)
                    if (Regex.IsMatch(c[n - 2], reSignedFloat))
                    {
                        for (int i = 0; i < fc; i++)
                            uvalue += "col" + i + " ";
                        uvalue += "ra dec";
                        colNames.AddRange(Regex.Split(uvalue, reSplit, RegexOptions.ExplicitCapture).Where(str => !str.Equals(String.Empty)).ToArray());
                        utype = "N" + n;
                        return "N" + n;
                    }
                    else if (Regex.IsMatch(c[n - 2], @"^ra$", RegexOptions.IgnoreCase) && Regex.IsMatch(c[n - 1], @"^dec$", RegexOptions.IgnoreCase))
                    {
                        foreach (string i in c)
                            uvalue += " " + i;
                        colNames.AddRange(Regex.Split(uvalue, reSplit, RegexOptions.ExplicitCapture).Where(str => !str.Equals(String.Empty)).ToArray());
                        ubody = ubody.Skip(1).ToArray();
                        utype = "H" + n;
                        return "H" + n;
                    }
                    else
                        return Error("problem with upload file/text: incorrect header names on first line '" + s + "'");
                }
            }
            else //if (Request["searchType"] == "apogee")
            {
                if (Request["apogeeUpType"] == "ra-dec")
                {
                    if (Regex.IsMatch(c[n - 2], reSignedFloat))
                    {
                        for (int i = 0; i < fc; i++)
                            uvalue += "col" + i + " ";
                        uvalue += "ra dec";
                        colNames.AddRange(Regex.Split(uvalue, reSplit, RegexOptions.ExplicitCapture).Where(str => !str.Equals(String.Empty)).ToArray());
                        utype = "N" + n;
                        return "N" + n;
                    }
                    else if (Regex.IsMatch(c[n - 2], @"^ra$", RegexOptions.IgnoreCase) && Regex.IsMatch(c[n - 1], @"^dec$", RegexOptions.IgnoreCase))
                    {
                        foreach (string i in c)
                            uvalue += " " + i;
                        colNames.AddRange(Regex.Split(uvalue, reSplit, RegexOptions.ExplicitCapture).Where(str => !str.Equals(String.Empty)).ToArray());
                        ubody = ubody.Skip(1).ToArray();
                        utype = "H" + n;
                        return "H" + n;
                    }
                    else
                        return Error("problem with upload file/text: incorrect header names on first line '" + s + "'");
                }
                else //if (Request["apogeeUpType"] == "l-b")
                {
                    if (Regex.IsMatch(c[n - 2], reSignedFloat))
                    {
                        for (int i = 0; i < fc; i++)
                            uvalue += "col" + i + " ";
                        uvalue += "l b";
                        colNames.AddRange(Regex.Split(uvalue, reSplit, RegexOptions.ExplicitCapture).Where(str => !str.Equals(String.Empty)).ToArray());
                        utype = "N" + n;
                        return "N" + n;
                    }
                    else if (Regex.IsMatch(c[n - 2], @"^l$", RegexOptions.IgnoreCase) && Regex.IsMatch(c[n - 1], @"^b$", RegexOptions.IgnoreCase))
                    {
                        foreach (string i in c)
                            uvalue += " " + i;
                        colNames.AddRange(Regex.Split(uvalue, reSplit, RegexOptions.ExplicitCapture).Where(str => !str.Equals(String.Empty)).ToArray());
                        ubody = ubody.Skip(1).ToArray();
                        utype = "H" + n;
                        return "H" + n;
                    }
                    else
                        return Error("problem with upload file/text: incorrect header names on first line '" + s + "'");
                }

            }
        }

        protected new string Error(string msg)
        {
            Response.Write("<h2>Error: " + msg + "</h2>\n");
            return null;
        }

        private void getNearestApogee(SqlConnection oConn) {
            DataSet dataSet = new DataSet();
            string radius = Request["radius"];
            sendSQL(oConn, "create table #x (up_id int, apstar_id varchar(50))");
            string cmd = "INSERT INTO #x SELECT up_id, (SELECT apstar_id FROM fGetNearestApogeeStarEq(up_ra,up_dec," + radius + ")) as apstar_id \n     ";
            cmd += "FROM #upload WHERE (SELECT apstar_id FROM fGetNearestApogeeStarEq(up_ra,up_dec," + radius + ")) IS NOT NULL";
            sendSQL(oConn, cmd);

            var c = UserCommand(Request["uquery"]);

            cmd = "EXEC spExecuteSQL '" + c + "  ', " + rowLimit + ",'" + server_name + "','" + windows_name + "','" + remote_addr + "','" + globals.Access + "'";

            if (firstCol > 0)
            {
                var fields = "SELECT ";
                for (int i = 0; i < firstCol; i++)
                    fields += "u.up_" + colNames[i] + " as [" + colNames[i] + "],";
                cmd = Regex.Replace(cmd, reSelect, fields, RegexOptions.IgnoreCase);
            }

            //ResponseAux.writeOutput(oConn, cmd, c, Request["format"], Response, globals, globals.CrossidTimeout);
            string format = Request["format"];
            if (format == "html")
            {
                ResponseAux.writeOutput(oConn, cmd, c, format, Response, globals, globals.CrossidTimeout);
            }
            else
            {
                DataTable table = ResponseAux.getDataTable(oConn, cmd, globals.CrossidTimeout);
                dataSet.Tables.Add(table);
                ResponseAux.writeOutput(dataSet, format, Response);
            }
        }

        private void getNearest(SqlConnection oConn, string spec)
        {
            DataSet dataSet = new DataSet();
            string radius = Request["radius"];

            //if (dbg == 1) showLine("<h4>Get nearest object within " + radius + " arcmins</h4>");

            sendSQL(oConn, "create table #x (up_id int," + spec + "objID bigint)");

            //if (dbg == 1) showLine("created table #x");
            string fun;
            if ((spec == "" && Request["photoScope"] == "nearObj") || (spec == "Spec" && Request["spectroScope"] == "nearObj"))
                fun = " dbo.fGetNearest" + spec + "ObjIdAllEq";
            else
                fun = " dbo.fGetNearest" + spec + "ObjIdEq";
            fun += "(up_ra,up_dec," + radius + ") ";
            string cmd = "INSERT INTO #x SELECT up_id," + fun + "as " + spec + "objId \n     ";
            cmd += "FROM #upload WHERE" + fun + "IS NOT NULL";
            sendSQL(oConn, cmd);
            //if (dbg == 1) showLine("inserted into #x");

            var c = UserCommand(Request["uquery"]);

            cmd = "EXEC spExecuteSQL '" + c + "  ', " + rowLimit + ",'" + server_name + "','" + windows_name + "','" + remote_addr + "','" + globals.Access + "'";
            if (spec == "" && !Regex.IsMatch(cmd, rePobjid, RegexOptions.IgnoreCase))
                cmd = Regex.Replace(cmd, reSelect, "SELECT x.objID,", RegexOptions.IgnoreCase);
            if (firstCol > 0)
            {
                var fields = "SELECT ";
                for (int i = 0; i < firstCol; i++)
                    fields += "u.up_" + colNames[i] + " as [" + colNames[i] + "],";
                cmd = Regex.Replace(cmd, reSelect, fields, RegexOptions.IgnoreCase);
            }

            if (spec == "" && Request["format"] == "html")
                cmd = Regex.Replace(cmd, rePobjid, "''<a target=INFO href=" + url + "/tools/explore/obj.aspx?id='' + cast(x.objId as varchar(20)) + ''>''+ cast(x.objId as varchar(20)) + ''</a>'' as objID,",RegexOptions.IgnoreCase);
            //if (dbg == 1) showLine(c);
            //ResponseAux.writeOutput(oConn, cmd, c, Request["format"], Response, globals, globals.CrossidTimeout);
            string format = Request["format"];
            if (format == "html")
            {
                ResponseAux.writeOutput(oConn, cmd, c, format, Response, globals, globals.CrossidTimeout);
            }
            else
            {
                DataTable table = ResponseAux.getDataTable(oConn, cmd, globals.CrossidTimeout);
                dataSet.Tables.Add(table);
                ResponseAux.writeOutput(dataSet, format, Response);
            }
        }

        private void getObjID(SqlConnection oConn)
        {
            DataSet dataSet = new DataSet();
            //if (dbg == 1) showLine("<h4>Get all objects with given objIDs (run,rerun,camcol,field,obj)</h4>");

            sendSQL(oConn, "create table #x (up_id int,objID bigint)");
            string cmd = "INSERT INTO #x SELECT up_id, p.objID \n     ";
            int n = colNames.Count;
            cmd += "FROM #upload, PhotoTag p WHERE p.objID = dbo.fObjidFromSDSS(" + globals.SkyVersion + ",up_" + colNames[n - 5];
            cmd += ",up_" + colNames[n - 4] + ",up_" + colNames[n - 3] + ",up_" + colNames[n - 2];
            cmd += ",up_" + colNames[n - 1] + ")";
            //if (dbg == 1) showLine(cmd);
            sendSQL(oConn, cmd);

            var c = UserCommand(Request["uquery"]);

            cmd = "EXEC spExecuteSQL '" + c + "  ', " + rowLimit + ",'" + server_name + "','" + windows_name + "','" + remote_addr + "','" + globals.Access + "'";
            if (!Regex.IsMatch(cmd, rePobjid, RegexOptions.IgnoreCase))
                cmd = Regex.Replace(cmd, reSelect, "SELECT x.objID,", RegexOptions.IgnoreCase);
            var fields = "SELECT ";
            for (int i = 0; i < n; i++)
                fields += "u.up_" + colNames[i] + " as " + colNames[i] + ",";
            cmd = Regex.Replace(cmd, reSelect, fields, RegexOptions.IgnoreCase);
            if (Request["format"] == "html")
                cmd = Regex.Replace(cmd, rePobjid, "''<a target=INFO href=" + url + "/tools/explore/obj.aspx?id='' + cast(x.objId as varchar(20)) + ''>''+ cast(x.objId as varchar(20)) + ''</a>'' as objID,", RegexOptions.IgnoreCase);
            //if (dbg == 1) showLine(c);
            //ResponseAux.writeOutput(oConn, cmd, c, Request["format"], Response, globals, globals.CrossidTimeout);
            string format = Request["format"];
            if (format == "html")
            {
                ResponseAux.writeOutput(oConn, cmd, c, format, Response, globals, globals.CrossidTimeout);
            }
            else
            {
                DataTable table = ResponseAux.getDataTable(oConn, cmd, globals.CrossidTimeout);
                dataSet.Tables.Add(table);
                ResponseAux.writeOutput(dataSet, format, Response);
            }
        }

        private void getPmf(SqlConnection oConn)
        {
            DataSet dataSet = new DataSet();

            string c = UserCommand(Request["uquery"]);


            var cmd = "EXEC spExecuteSQL '" + c + "  ', " + rowLimit + ",'" + server_name + "','" + windows_name + "','" + remote_addr + "','" + globals.Access + "'";

            //if (dbg == 1) showLine(c);
            //ResponseAux.writeOutput(oConn, cmd, c, Request["format"], Response, globals, globals.CrossidTimeout);
            string format = Request["format"];
            if (format == "html")
            {
                ResponseAux.writeOutput(oConn, cmd, c, format, Response, globals, globals.CrossidTimeout);
            }
            else
            {
                DataTable table = ResponseAux.getDataTable(oConn, cmd, globals.CrossidTimeout);
                dataSet.Tables.Add(table);
                ResponseAux.writeOutput(dataSet, format, Response);
            }
        }

        private void sendSQL(SqlConnection oConn, string cmd)
        {
            using (SqlCommand oCmd = oConn.CreateCommand())
            {
                oCmd.CommandTimeout = globals.CrossidTimeout;
                oCmd.CommandText = cmd;
                oCmd.ExecuteNonQuery();
            }
        }

        private string UploadCmd()
        {
            string[] names = Regex.Split(uvalue, reSplit, RegexOptions.ExplicitCapture).Where(str => !str.Equals(String.Empty)).ToArray();
            var cmd = "CREATE TABLE #upload ( up_id int";
            foreach (string i in names)
            {
                string name = i;
                if (i == "l") name = "ra";
                if (i == "b") name = "dec";
                cmd += ", up_" + name + " ";
                switch (name)
                {
                    case "objID":
                        cmd += "bigint";
                        break;
                    case "ra":
                    case "dec":
                        cmd += "float";
                        break;
                    case "plate":
                    case "mjd":
                    case "fiber":
                    case "fiberid":
                        cmd += "int";
                        break;
                    default:
                        cmd += "varchar(32)";
                        break;
                }
            }
            cmd += " )";
            return cmd;
        }

        private void getNearby(SqlConnection oConn, string spec)
        {
            DataSet dataSet = new DataSet();

            string radius = Request["radius"];

            if ((spec == "" && Request["photoScope"] == "allPrim") || (spec == "Spec" && Request["spectroScope"] == "allPrim"))
            {
                //if (dbg==1) showLine("<h4>Get all nearby primary objects within "+radius+" arcmins</h4>");
                sendSQL(oConn, "create table #x (up_id int," + spec + "objID bigint)");
                sendSQL(oConn, "insert into #x EXEC dbo.spGet" + spec + "NeighborsPrim " + radius);
            }
            else
            {
                //if (dbg==1) showLine("<h4>Get all nearby objects within "+radius+" arcmins</h4>");
                sendSQL(oConn, "create table #x (up_id int," + spec + "objID bigint)");
                sendSQL(oConn, "insert into #x EXEC dbo.spGet" + spec + "NeighborsAll " + radius);
            }

            string c = UserCommand(Request["uquery"]);

            var cmd = "EXEC spExecuteSQL '" + c + "  ', " + rowLimit + ",'" + server_name + "','" + windows_name + "','" + remote_addr + "','" + globals.Access + "'";
            if (spec == "" && !Regex.IsMatch(cmd, rePobjid, RegexOptions.IgnoreCase))
                cmd = Regex.Replace(cmd, reSelect, "SELECT x.objID,", RegexOptions.IgnoreCase);
            if (firstCol > 0)
            {
                var fields = "SELECT ";
                for (int i = 0; i < firstCol; i++)
                    fields += "u.up_" + colNames[i] + " as " + colNames[i] + ",";
                cmd = Regex.Replace(cmd, reSelect, fields, RegexOptions.IgnoreCase);
            }
            if (spec == "" && Request["format"] == "html")
                cmd = Regex.Replace(cmd, rePobjid, "''<a target=INFO href=" + url + "/tools/explore/obj.aspx?id='' + cast(x.objId as varchar(20)) + ''>''+ cast(x.objId as varchar(20)) + ''</a>'' as objID,", RegexOptions.IgnoreCase);
            //if (dbg==1) showLine(c);
            //ResponseAux.writeOutput(oConn, cmd, c, Request["format"], Response, globals, globals.CrossidTimeout);
            string format = Request["format"];
            if (format == "html")
            {
                ResponseAux.writeOutput(oConn, cmd, c, format, Response, globals, globals.CrossidTimeout);
            }
            else
            {
                DataTable table = ResponseAux.getDataTable(oConn, cmd, globals.CrossidTimeout);
                dataSet.Tables.Add(table);
                ResponseAux.writeOutput(dataSet, format, Response);
            }
        }

        private string UserCommand(string uquery)
        {
  	        var c = uquery;
	        /*
            if (ubody.Length>1)
		        foreach (string j in ubody)
			        c += j+"\n";
            */
	        c = c.Replace("'","''");
	        return c;
        }
    }
}
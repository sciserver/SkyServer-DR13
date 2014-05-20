<%@ Page Title="" Language="C#" MasterPageFile="ObjectExplorer.Master" AutoEventWireup="true" CodeBehind="parameters.aspx.cs" Inherits="SkyServer.Tools.Explore.Parameters" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="SkyServer.Tools.Explore" %>
<%@ Import Namespace="SkyServer" %>

<asp:Content ID="Head1" ContentPlaceHolderID="OEHead" runat="server">
    <!--<script src="../../../formatting.js" type="text/javascript" language="javascript"></script>-->
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="OEContent" runat="server">
    <% string cmd; %>
    <!--<div class="content" style="display:none">-->
    <div class="content">
          <table class="content">
        <tr>
            <td colspan="2">
                <%
                    
                    using (SqlConnection oConn = new SqlConnection(globals.ConnectionString))
                    {
                        oConn.Open();

                        Response.Write("<h1>Parameters from Spectroscopic Fitting</h1>");

                        Response.Write("<p>For most objects, SDSS processing pipelines find estimates of the physical parameters most likely to have produced the observed spectrum. Many different estimates are available - the right one to use will depend on the problem at hand.</p>");

                        Response.Write("<p>Click on the name of each set of estimates for more information on the pipeline that produced those estimates (opens in a new window). Most objects will have either estimated stellar parameters or galaxy properties, but a few objects will have both.");
                                                
                        Response.Write("<h2>Stellar Parameters</h2>");

                        Response.Write("<h3><a href='" + globals.SdssUrl + "spectro/sspp.php' target='_blank'>SEGUE Stellar Parameters Pipeline (SSPP) <img src='../../images/new_window_black.png' alt=' (new window)' /></a></h3>");
                        
                        cmd = "select targetstring as 'Targeting criteria', flag as 'SEGUE flags',spectypehammer as 'HAMMER spectral type', SPECTYPESUBCLASS as 'Spectral subclass'";
                        cmd += " from sppParams where specObjId=" + specId;
                        master.showHTable(oConn, cmd, 600, "SpecObjAll");

                        cmd = " select str(elodiervfinal,7,2) as 'Radial velocity (km/s)', str(elodiervfinalerr,8,3) as 'RV error', str(teffadop,6,0) as 'Effective temp (K)', str(teffadopunc,6,1) as 'Teff error'";
                        cmd += " from sppParams where specObjId=" + specId;
                        master.showHTable(oConn, cmd, 600, "SpecObjAll");

                        cmd = " select str(fehadop,7,2) as '[Fe/H] (dex)', str(fehadopunc,8,3) as '[Fe/H] error', str(loggadop,7,2) as 'log<sub>10</sub>(g) (dex)', str(loggadopunc,8,3) as 'log<sub>10</sub>(g) error'";
                        cmd += " from sppParams where specObjId=" + specId;
                        master.showHTable(oConn, cmd, 600, "SpecObjAll");

                        Response.Write("<h3><a href='" + globals.SdssUrl + "irspec/aspcap.php' target='_blank'>APOGEE Stellar Parameters and Chemical Abundances Pipeline (ASPCAP) <img src='../../images/new_window_black.png' alt=' (new window)' /></a></h3>");

                        Response.Write("<p>&nbsp;</p>");
                        
                        Response.Write("<h2><a href='"+globals.SdssUrl+"spectro/galaxy.php' target='_blank'>Galaxy Property Estimates <img src='../../images/new_window_black.png' alt=' (new window)' /></a></h2>");

                        Response.Write("<h3><a href='" + globals.SdssUrl + "spectro/galaxy_portsmouth.php#fitting' target='_blank'>Model Fitting: Portsmouth group (Spectro-photometric matching) <img src='../../images/new_window_black.png' alt=' (new window)' /></a></h3>");
                        
                        Response.Write("<h4>Star Forming Model</h4>");
                         
                        cmd = "select logMass as 'Best-fit log<sub>10</sub>(stellar mass)',";
                        cmd += " minLogMass as '1-&sigma; min', maxLogMass as '1-&sigma; max'";
                        cmd += " from stellarMassStarformingPort where specObjId=" + specId;

                        //Response.Write("<h1><font color='green'>" + cmd + "</font></h1>");

                        master.showHTable(oConn, cmd, 600, "SpecObjAll");

                        cmd = "select age as 'Best-fit age (Gyr)', minAge as '1-&sigma; min', maxAge as '1-&sigma; max'";
                        cmd += " from stellarMassStarformingPort where specObjId=" + specId;
                        master.showHTable(oConn, cmd, 600, "SpecObjAll");

                        cmd = "select SFR as 'Best-fit SFR (M<sub>&#9737;</sub> / yr)', minSFR as '1-&sigma; min', maxSFR as '1-&sigma; max'";
                        cmd += " from stellarMassStarformingPort where specObjId=" + specId;
                        master.showHTable(oConn, cmd, 600, "SpecObjAll");

                        Response.Write("<h4>Passive Model</h4>");

                        cmd = "select logMass as 'Best-fit log<sub>10</sub>(stellar mass)',";
                        cmd += " minLogMass as '1-&sigma; min', maxLogMass as '1-&sigma; max'";
                        cmd += " from stellarMassPassivePort where specObjId=" + specId;
                        master.showHTable(oConn, cmd, 600, "SpecObjAll");

                        cmd = "select age as 'Best-fit age (Gyr)', minAge as '1-&sigma; min', maxAge as '1-&sigma; max'";
                        cmd += " from stellarMassPassivePort where specObjId=" + specId;
                        master.showHTable(oConn, cmd, 600, "SpecObjAll");

                        cmd = "select SFR as 'Best-fit SFR (M<sub>&#9737;</sub> / yr)', minSFR as '1-&sigma; min', maxSFR as '1-&sigma; max'";
                        cmd += " from stellarMassPassivePort where specObjId=" + specId;
                        master.showHTable(oConn, cmd, 600, "SpecObjAll");

                        Response.Write("<h3><a href='" + globals.SdssUrl + "spectro/galaxy_portsmouth.php#kinematics' target='_blank'>Stellar Kinematics: Portsmouth group (Emission Line Fluxes) <img src='../../images/new_window_black.png' alt=' (new window)' /></a></h3>");

                        cmd = "select velstars as 'Stellar velocity (km/s)',";
                        cmd += " sigmaStars as 'Stellar velocity disperson (km/s)', sigmaStarsErr as 'Velocity dispersion error'";
                        cmd += " from emissionLinesPort where specObjId=" + specId;
                        master.showHTable(oConn, cmd, 600, "SpecObjAll");

                        cmd = "select chisq as 'Chi-squared fit of template'";
                        cmd += " from emissionLinesPort where specObjId=" + specId;
                        master.showVTable(oConn,cmd,600);                        
                        
                        cmd = "select bpt as 'BPT classification'";
                        cmd += " from emissionLinesPort where specObjId=" + specId;
                        master.showVTable(oConn, cmd, 600); 
                                                                                  
                        Response.Write("<h3><a href='" + globals.SdssUrl + "spectro/galaxy_wisconsin.php' target='_blank'>Wisconsin group (Principal Component Analysis) <img src='../../images/new_window_black.png' alt=' (new window)' /></a></h3>");
                        
                        Response.Write("<h4>Assuming model spectra from Bruzal &amp; Charlot (2003)</h4>");

                        cmd = "select str(mstellar_median,7,2) as 'Best-fit log<sub>10</sub>(stellar mass)', str(mstellar_err,8,3) as 'Error', str(vdisp_median,8,2) as 'Median veldisp (km/s)', str(vdisp_err,9,3) as 'Error'";
                        cmd += " from stellarMassPCAWiscBC03 where specObjId=" + specId;
                        master.showHTable(oConn, cmd, 600, "SpecObjAll");

                        Response.Write("<h4>Assuming model spectra from Maraston &amp; Str&ouml;mback (2011)</h4>");

                        cmd = "select str(mstellar_median,7,2) as 'Best-fit log<sub>10</sub>(stellar mass)', str(mstellar_err,8,3) as 'Error', str(vdisp_median,8,2) as 'Median veldisp (km/s)', str(vdisp_err,9,3) as 'Error'";
                        cmd += " from stellarMassPCAWiscM11 where specObjId=" + specId;
                        master.showHTable(oConn, cmd, 600, "SpecObjAll");

                        Response.Write("<h3><a href='" + globals.SdssUrl + "spectro/galaxy_granada.php' target='_blank'>Granada group (Flexible Stellar Population Synthesis) <img src='../../images/new_window_black.png' alt=' (new window)' /></a></h3>");

                        Response.Write("<h4>Early Galaxy Formation, Dust Attenuation</h4>");

                        cmd = "select str(logmass,7,2) as 'Best-fit log<sub>10</sub>(stellar mass)', str(logmass_err,8,3) as 'Error', str(age,8,2) as 'Age (Gyr)', str(ssfr,8,2) as 'SSFR', str(metallicity,8,2) as 'metallicity'";
                        cmd += " from stellarmassFSPSGranEarlyDust where specObjId=" + specId;
                        master.showHTable(oConn, cmd, 600, "SpecObjAll");

                        Response.Write("<h4>Early Galaxy Formation, No Dust Attenuation</h4>");

                        cmd = "select str(logmass,7,2) as 'Best-fit log<sub>10</sub>(stellar mass)', str(logmass_err,8,3) as 'Error', str(age,8,2) as 'Age (Gyr)', str(ssfr,8,2) as 'SSFR', str(metallicity,8,2) as 'metallicity'";
                        cmd += " from stellarmassFSPSGranEarlyNoDust where specObjId=" + specId;
                        master.showHTable(oConn, cmd, 600, "SpecObjAll");

                        Response.Write("<h4>Wide Range of Formation Times, Dust Attenuation</h4>");

                        cmd = "select str(logmass,7,2) as 'Best-fit log<sub>10</sub>(stellar mass)', str(logmass_err,8,3) as 'Error', str(age,8,2) as 'Age (Gyr)', str(ssfr,8,2) as 'SSFR', str(metallicity,8,2) as 'metallicity'";
                        cmd += " from stellarmassFSPSGranWideDust where specObjId=" + specId;
                        master.showHTable(oConn, cmd, 600, "SpecObjAll");

                        Response.Write("<h4>Wide Range of Formation Times, Dust Attenuation</h4>");

                        cmd = "select str(logmass,7,2) as 'Best-fit log<sub>10</sub>(stellar mass)', str(logmass_err,8,3) as 'Error', str(age,8,2) as 'Age (Gyr)', str(ssfr,8,2) as 'SSFR', str(metallicity,8,2) as 'metallicity'";
                        cmd += " from stellarmassFSPSGranWideNoDust where specObjId=" + specId;
                        master.showHTable(oConn, cmd, 600, "SpecObjAll");


                        /*
                        cmd = "select warning as 'Warnings'";
                        cmd += " from stellarMassPCAWisc where specObjId=" + specObjId;
                        master.showVTable(oConn, cmd, 500);
                        */
                    }
                    %>
            </td>
            </tr>    
    </table>
</div>
</asp:Content>

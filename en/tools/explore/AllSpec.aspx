<%@ Page Title="" Language="C#" MasterPageFile="ObjectExplorer.Master" AutoEventWireup="true" CodeBehind="AllSpec.aspx.cs" Inherits="SkyServer.Tools.Explore.AllSpec" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<asp:Content ID="Content2" ContentPlaceHolderID="OEContent" runat="server">

<div id="neighbors" class="content">
<table class="content">
    <table class="content">
    <tr><td>
    <h2>Other spectra</h2>
    <p class="content">The SDSS often takes repeat spectra of the same object. Each time that 
    astronomical object is spectroscopically 
    observed with any of the spectrographs connected to the SDSS telescope, it is assigned a 
    separate specObjID. We then select the best spectrum and assign it as <em>sciencePrimary</em>.</p>

    <p class="content">Information about each spectroscopic observation is stored in the 
    <a href="../../help/browser/browser.aspx?cmd=description+SpecObjAll+U" class='content'>specObjAll</a> table. 
    The <a href="../../help/browser/browser.aspx?cmd=description+SpecObj+U" class='content'>specObj</a> view 
    contains only data for spectra assigned as sciencePrimary.</p>

    <p class="content">The table below shows all spectra that were measured for this object.</p>
        
       <h3>All Spectra of this Object</h3>

<%     
    
    using (SqlConnection oConn = new SqlConnection(globals.ConnectionString))
    {
        oConn.Open();

        string cmd = "select s.specObjId, s.plate as plate, s.mjd as MJD, s.fiberID as fiber, str(t.ra,10,5) as ra, str(t.dec,10,5) as dec, ";
        cmd += "  str(s.ra,10,5) as specRa, str(s.dec,10,5) as specDec, ";
        cmd += " s.sciencePrimary, str(dbo.fDistanceArcMinEq(t.ra,t.dec,s.ra,s.dec),10,8) as distanceArcMin, ";
        cmd += " s.class as specClass";
        cmd += " from SpecObjAll s, photoobjall t where t.objid=" + objId + " and s.bestobjid=t.objid ";
        //cmd += " order by scienceprimary desc, distanceArcMin asc";
        cmd += " order by scienceprimary desc, plate, MJD, fiber"; 
        master.showSTable(oConn, cmd);
    }
%>

       <h3>Flux-Matched Spectra of this Object</h3>

<%     
    
    using (SqlConnection oConn = new SqlConnection(globals.ConnectionString))
    {
        oConn.Open();

        string cmd = "select s.specObjId, s.plate as plate, s.mjd as MJD, s.fiberID as fiber, str(t.ra,10,5) as ra, str(t.dec,10,5) as dec, ";
        cmd += "  str(s.ra,10,5) as specRa, str(s.dec,10,5) as specDec, ";
        cmd += " s.sciencePrimary, str(dbo.fDistanceArcMinEq(t.ra,t.dec,s.ra,s.dec),10,8) as distanceArcMin, ";
        cmd += " s.class as specClass";
        cmd += " from SpecObjAll s, photoobjall t where t.objid=" + objId + " and s.fluxobjid=t.objid ";
        //cmd += " order by scienceprimary desc, distanceArcMin asc";
        cmd += " order by scienceprimary desc, plate, MJD, fiber ";   
        master.showSTable(oConn, cmd);
    }
%>
</td></tr>
</table>
</div>
</asp:Content>

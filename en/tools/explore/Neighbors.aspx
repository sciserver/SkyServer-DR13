<%@ Page Title="" Language="C#" MasterPageFile="ObjectExplorer.Master" AutoEventWireup="true" CodeBehind="Neighbors.aspx.cs" Inherits="SkyServer.Tools.Explore.Neighbors" %>
<%@ Import Namespace="System.Data.SqlClient" %>


<asp:Content ID="Content2" ContentPlaceHolderID="OEContent" runat="server">
<div id="neighbors" class="content">
<table class="content">
    <tr><td>
    <h2>Nearby objects (neighbors)</h2>

    <p class="content">Many interesting astronomical questions start with asking, "what is near this 
    object"? To enable these kinds of questions, the list below shows all SDSS objects within 0.5 
    arcminutes of the selected object.</p>

    <p class="content">Finding nearby objects is computationally intensive, so we have precomputed 
    a 0.5-arcminute neighbors search for each object in the photoObjAll table. These precomputed 
    neighbor searches are stored in the  
    <a href="../../help/browser/browser.aspx?cmd=description+neighbors+U" class='content'>Neighbors</a> 
    table.</p>

    </td></tr>
    <tr><td>

    <h3>This Object</h3>
    <%   
    using (SqlConnection oConn = new SqlConnection(globals.ConnectionString))
    {
        oConn.Open();


        string cmd = "select dbo.fIAUFromEq(p.ra,p.dec) as 'IAU name', p.objid, p.thingid";
        cmd += " from photoobjall p where p.objid=" + objId;

        master.showHTable(oConn, cmd, 200, "PhotoObj");
        
    %>

    <h3>Neighboring objects within 0.5 arcminutes</h3>

    <%
        cmd = "select n.neighborObjId as objId,str(t.ra,10,5) as ra, str(t.dec,10,5) as dec, ";
        cmd += " str(n.distance,5,3) as 'distance (arcmin)',";
        cmd += " dbo.fPhotoTypeN(n.neighborType) as type, neighborMode as mode,";
        cmd += " dbo.fPhotoModeN(n.neighborMode) as '(mode description)'";
        cmd += " from Neighbors n, photoobjall t where n.neighborObjid=t.objid ";
        cmd += " and n.objId=" + objId + " order by n.distance asc ";
        master.showNTable(oConn, cmd);
    }
%>
</td></tr></table>
</div>
</asp:Content>

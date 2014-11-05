<%@ Page Title="" Language="C#" MasterPageFile="ObjectExplorer.Master" AutoEventWireup="true" CodeBehind="galaxyzoo.aspx.cs" Inherits="SkyServer.Tools.Explore.GalaxyZoo" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Data" %>

<asp:Content ID="Content2" ContentPlaceHolderID="OEContent" runat="server">

    <div id="neighbors" class="content">
    
    <table class="content">
    <tr><td>
    <h1>Galaxy Zoo</h1>

    <p class="content">SDSS photometric data also includes morphological classifications of galaxies from the Galaxy Zoo project. More than 200,000 
        online volunteer citizen scientists. <a href="http://zoo1.galaxyzoo.org" class="content">Galaxy Zoo 1 
        <img src="../../images/offsite_black.png" alt="offiste" /></a> data consists of simple morphological type classification 
        (spiral vs. elliptical) for 893,212 galaxies. Galaxy Zoo 1 data are described in 
        <a href="http://adsabs.harvard.edu/abs/2011MNRAS.410..166L" class="content">Lintott et al. 2011 
        <img src="../../images/offsite_black.png" alt="(offsite)" /></a>, and details of the project are available 
        in <a href="http://adsabs.harvard.edu/abs/2008MNRAS.389.1179L" class="content">
        Linott et al. 2008 <img src="../../images/offsite_black.png" alt="(offsite)" /></a>.
    </p>

    <p class="content">New in SDSS Data Release 10 are more detailed classifications of internal structure of the brightest 25% of the Main Galaxy 
        Sample from <a href="http://zoo2.galaxyzoo.org" class="content">Galaxy Zoo 2 <img src="../../images/offsite_black.png" alt="(offiste)" /></a>. 
        Full details of the processes used to combine the votes (from a median of ~40 people per galaxy) and construct debiased classification 
        likelihoods are described in Willett et al. (in press at <i>MNRAS</i>).
    </p>

    <p class="content">If this object has matches in any of the Galaxy Zoo tables, the most commonly accessed columns in those tables are 
        displayed below. For full data, click on the table names. To return to this page, click "Galaxy Zoo" in the left-hand column.
    </p>

    </td></tr>
    <tr><td>
        
    <h2>Galaxy Zoo 1</h2>
        <%
            string explore = "DisplayResults.aspx?id=" + objId + "&cmd=";
         %>
        <h3><a href="<%=explore+master.exploreQuery.zooSpec%>&name=zooSpec&id=<%=objId%>" class="content">zooSpec</a></h3>

    <%  
        DataSet ds = null;

        ds = master.runQuery.RunCasjobs(master.exploreQuery.zooSpec1);
        master.showHTable(ds, 600, "PhotoObj");

        ds = master.runQuery.RunCasjobs(master.exploreQuery.zooSpec2);
        master.showHTable(ds, 600, "PhotoObj"); 
    %>

        <h3><a href="<%=explore+master.exploreQuery.zooNoSpec%>&name=zooNoSpec&id=<%=objId%>" class="content">zooNoSpec</a></h3>
        
     <%    
         ds= master.runQuery.RunCasjobs(master.exploreQuery.zooNoSpec);
         master.showHTable(ds, 600, "PhotoObj");
     %>

        <h3><a href="<%=explore+master.exploreQuery.zooConfidence%>&name=zooConfidence" class="content">zooConfidence</a></h3>
        
     <%
         ds = master.runQuery.RunCasjobs(master.exploreQuery.zooConfidence2);
         master.showHTable(ds, 600, "PhotoObj");                   
     %>

        <h3><a href="<%=explore+master.exploreQuery.zooMirrorBias%>&name=zooMirrorBias" class="content">zooMirrorBias</a></h3>
        
     <%
         ds = master.runQuery.RunCasjobs(master.exploreQuery.zooMirrorBias2);  
         master.showHTable(ds, 600, "PhotoObj");     
     %>

        <h3><a href="<%=explore+master.exploreQuery.zooMonochromeBias%>&name=zooMonochromeBias" class="content">zooMonochromeBias</a></h3>
        
     <%
         ds=master.runQuery.RunCasjobs(master.exploreQuery.zooMonochromeBias2);
         master.showHTable(ds, 600, "PhotoObj");
            
        // show the Galaxy Zoo 2 data only if this is DR10
        if (globals.ReleaseNumber >= 10) 
        {
     %>

     <h2>Galaxy Zoo 2</h2>

        <h3><a href="<%=explore+master.exploreQuery.zoo2MainSpecz%>&name=zoo2MainSpecz" class="content">zoo2MainSpecz</a></h3>
        
     <%
         ds = master.runQuery.RunCasjobs(master.exploreQuery.zoo2MainSpecz2);
         master.showHTable(ds, 600, "PhotoObj");
     %>

        <h3><a href="<%=explore+master.exploreQuery.zoo2MainPhotoz%>&name=zoo2MainPhotoz" class="content">zoo2MainPhotoz</a></h3>
        
     <%
         ds = master.runQuery.RunCasjobs(master.exploreQuery.zoo2MainPhotoz2);
         master.showHTable(ds, 600, "PhotoObj");
     %>

        <h3><a href="<%=explore+master.exploreQuery.zoo2Stripe82Normal%>&name=zoo2Stripe82Normal" class="content">zoo2Stripe82Normal</a></h3>
        
     <%
         ds = master.runQuery.RunCasjobs(master.exploreQuery.zoo2Stripe82Normal2);
         master.showHTable(ds, 600, "PhotoObj");
     %>

        <h3><a href="<%=explore+master.exploreQuery.zoo2Stripe82Coadd1%>&name=zoo2Stripe82Coadd1" class="content">zoo2Stripe82Coadd1</a></h3>
        
     <%
         ds = master.runQuery.RunCasjobs(master.exploreQuery.zoo2Stripe82Coadd1_2);
         master.showHTable(ds, 600, "PhotoObj");
     %>

        <h3><a href="<%=explore+master.exploreQuery.zoo2Stripe82Coadd2%>&name=zoo2Stripe82Coadd2" class="content">zoo2Stripe82Coadd2</a></h3>
        
     <%
         ds = master.runQuery.RunCasjobs(master.exploreQuery.zoo2Stripe82Coadd2_2);
         master.showHTable(ds, 600, "PhotoObj");
     
            }  // end of if statement where we control whether Zoo 2 data get displayed
     %>

    </td></tr></table>
    </div>
</asp:Content>

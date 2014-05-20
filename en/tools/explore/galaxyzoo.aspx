<%@ Page Title="" Language="C#" MasterPageFile="ObjectExplorer.Master" AutoEventWireup="true" CodeBehind="galaxyzoo.aspx.cs" Inherits="SkyServer.Tools.Explore.GalaxyZoo" %>
<%@ Import Namespace="System.Data.SqlClient" %>

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

        <h3><a href="ex_sql.aspx?cmd=select+*+from+zooSpec+where+objid=<%=objId%>&name=zooSpec&tab=V&id=<%=objId%>" class="content">zooSpec</a></h3>

    <%  
        using (SqlConnection oConn = new SqlConnection(globals.ConnectionString))
        {
            oConn.Open();

            string cmd = "select objid,nvote as 'Votes',str(p_el_debiased,5,3) 'Elliptical proabability (debiased)',";
            cmd += " str(p_cs_debiased,5,3) 'Spiral probability (debiased)'";
            cmd += " from zooSpec where objid=" + objId;

            master.showHTable(oConn, cmd, 600, "PhotoObj");

            cmd = "select str(p_cw,5,3) as 'Clockwise spiral probability', str(p_acw,5,3) as 'Anticlockwise spiral probability',";
            cmd += " str(p_edge,5,3) as 'Edge-on spiral probablity', str(p_mg,5,3) as 'Merger system probability'";
            cmd += " from zooSpec where objid=" + objId;

            master.showHTable(oConn, cmd, 600, "PhotoObj");            
        
            %>

        <h3><a href="ex_sql.aspx?cmd=select+*+from+zooNoSpec+where+objid=<%=objId%>&name=zooNoSpec&tab=V&id=<%=objId%>" class="content">zooNoSpec</a></h3>
        
        <%
            cmd = "select objid,nvote,p_el,p_cs";
            cmd += " from zooNoSpec where objid=" + objId;

            master.showHTable(oConn, cmd, 600, "PhotoObj");
            
        
     %>

        <h3><a href="ex_sql.aspx?cmd=select+*+from+zooConfidence+where+objid=<%=objId%>&name=zooConfidence&tab=V&id=<%=objId%>" class="content">zooConfidence</a></h3>
        
        <%
            cmd = "select objid,f_unclass_clean,f_misclass_clean";
            cmd += " from zooConfidence where objid=" + objId;

            master.showHTable(oConn, cmd, 600, "PhotoObj");
            
        
     %>

        <h3><a href="ex_sql.aspx?cmd=select+*+from+zooMirrorBias+where+objid=<%=objId%>&name=zooMirrorBias&tab=V&id=<%=objId%>" class="content">zooMirrorBias</a></h3>
        
        <%
            cmd = "select objid,nvote_mr1,nvote_mr2";
            cmd += " from zooMirrorBias where objid=" + objId;

            master.showHTable(oConn, cmd, 600, "PhotoObj");
            
        
     %>

        <h3><a href="ex_sql.aspx?cmd=select+*+from+zooMonochromeBias+where+objid=<%=objId%>&name=zooMonochromeBias&tab=V&id=<%=objId%>" class="content">zooMonochromeBias</a></h3>
        
        <%
            cmd = "select objid,nvote_mon";
            cmd += " from zooMonochromeBias where objid=" + objId;

            master.showHTable(oConn, cmd, 600, "PhotoObj");
            
            // show the Galaxy Zoo 2 data only if this is DR10
            
            if (globals.ReleaseNumber >= 10) 
            {
     %>

     <h2>Galaxy Zoo 2</h2>

        <h3><a href="ex_sql.aspx?cmd=select+*+from+zoo2MainSpecz+where+dr8objid=<%=objId%>&name=zoo2MainSpecz&tab=V&id=<%=objId%>" class="content">zoo2MainSpecz</a></h3>
        
        <%
            cmd = "select dr8objid, total_classifications, total_votes";
            cmd += " from zoo2MainSpecz where dr8objid=" + objId;

            master.showHTable(oConn, cmd, 600, "PhotoObj");
     %>

        <h3><a href="ex_sql.aspx?cmd=select+*+from+zoo2MainPhotoz+where+dr8objid=<%=objId%>&name=zoo2MainPhotoz&tab=V&id=<%=objId%>" class="content">zoo2MainPhotoz</a></h3>
        
        <%
            cmd = "select dr8objid, total_classifications, total_votes";
            cmd += " from zoo2MainPhotoz where dr8objid=" + objId;

            master.showHTable(oConn, cmd, 600, "PhotoObj");
     %>

        <h3><a href="ex_sql.aspx?cmd=select+*+from+zoo2Stripe82Normal+where+dr8objid=<%=objId%>&name=zoo2Stripe82Normal&tab=V&id=<%=objId%>" class="content">zoo2Stripe82Normal</a></h3>
        
        <%
            cmd = "select dr8objid, total_classifications, total_votes";
            cmd += " from zoo2Stripe82Normal where dr8objid=" + objId;

            master.showHTable(oConn, cmd, 600, "PhotoObj");
     %>

        <h3><a href="ex_sql.aspx?cmd=select+*+from+zoo2Stripe82Coadd1+where+dr8objid=<%=objId%>&name=zoo2Stripe82Coadd1&tab=V&id=<%=objId%>" class="content">zoo2Stripe82Coadd1</a></h3>
        
        <%
            cmd = "select dr8objid, total_classifications, total_votes";
            cmd += " from zoo2Stripe82Coadd1 where dr8objid=" + objId;

            master.showHTable(oConn, cmd, 600, "PhotoObj");
     %>

        <h3><a href="ex_sql.aspx?cmd=select+*+from+zoo2Stripe82Coadd2+where+dr8objid=<%=objId%>&name=zoo2Stripe82Coadd2&tab=V&id=<%=objId%>" class="content">zoo2Stripe82Coadd2</a></h3>
        
        <%
            cmd = "select dr8objid, total_classifications, total_votes";
            cmd += " from zoo2Stripe82Coadd2 where dr8objid=" + objId;

            master.showHTable(oConn, cmd, 600, "PhotoObj");
     
            }  // end of if statement where we control whether Zoo 2 data get displayed
        } // end of sqlConnection  %>

    </td></tr></table>
    </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="QuickLook.Master" AutoEventWireup="true" CodeBehind="QuickSummary.aspx.cs" Inherits="SkyServer.Tools.QuickLook.QuickSummary" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        table.content td  { font-size:9pt; font-family: arial,helvetica,sans-serif; }
        a.content { color: blue; text-decoration: underline; }
        a.content:hover	{ color: blue; }
        .h		{ background-color: #b0c4de;}
        .b		{ background-color: #eeeeee;}
        .t		{ background-color: #e0e0e0;}	
    </style>
    <script type="text/javascript">

        function showNavi(ra, dec) {
            var url = "../chart/navi.aspx?ra=" + ra + "&dec=" + dec;
            var w = window.open(url, "NAVIGATE");
            w.focus();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table border='0' width='640' class="content">
	<tr>	
	<td colspan=2 align='center'>
        <h2><font color=#000086>Summary data for:&nbsp;&nbsp;<%= name %> </font></h2>
        <h2>Position Data (How do I find it?)</h2>
        <table width='640' border='0' cellpadding='3' cellspacing='3'>
        <tr>
            <td width='33%' class='h' align=center>
                Object ID&nbsp;&nbsp;(objID):
            </td>
            <td width='33%' class='h' align=center>
                Right ascension&nbsp;&nbsp;(ra):
            </td>
            <td width='33%' class='h' align=center>
                Declination&nbsp;&nbsp;(dec):
            </td>
        </tr>
        <tr>
            <td align='center' width='33%' class='t'><%= objId %></td>
            <td align='center' width='33%' class='t'><%= ra %></td>
            <td align='center' width='33%' class='t'><%= dec %></td>
        </tr>
        </table>
        <table width='640' border='0'>
            <tr>
                <td align='center' valign='top' width='50%'>
                    <h2>Image Data</b></font><br><font size=+0><b>(What does it look like?)</h2>
                </td>
                <td align='center' valign='top' width='50%'>
                    <h2>Spectrum Data</b></font><br /><font size='+0'><b>(What does its spectrum look like?)</h2>
                </td>
            </tr>
            <tr>
                <td align='center' valign='top' width='50%'>
                    <p><b>Preview image (click to go to Navigate tool)</b></p>
                    <a href='<%= naviLink %>'>
                    <img alt='An image of this object' align='center' src='<%= (globals.WSGetJpegUrl + "?ra="+ra+"&dec="+dec+"&scale=0.2&width=200&height=200&opt=G") %>' border='0' width='200' height='200'/>
                    </a>
                </td>
                <td align='center' valign='top' width='50%'>
                <% if (specObjId != null && !ZERO_ID.Equals(specObjId)) {%>
                        	
    		        <p><b>Preview spectrum (click for a larger version)</b></p>  	
	   		        <a href='../../get/SpecById.ashx?id=<%= specObjId %>'>
			            <img src='../../get/SpecById.ashx?id=<%= specObjId %>' width='256' height='205' border='0' align='center' />
                    </a>
                <% } else { %>
                    <p align='left'><b>The SDSS has not measured a spectrum for this object.</b><br />  	  	
       			    See its <a href='../explore/obj.aspx?id=<%= id %>' target='_blank'>Explore</a> entry if you think it should have a spectrum.</p>
                <% } %>
                </td>
            </tr>
            <tr>
            <td align='center' valign='top' width='50%'>
            <p><b>Object Type (type):</b><%= otype %></p>
            <p><b>&nbsp;Magnitudes:</b>
            <table><tr><td>
            <% 
                using (SqlConnection oConn = new SqlConnection(globals.ConnectionString)) {
                    oConn.Open();
                    
                    string cmd;
                    cmd = " select";
                    cmd += " str(u,7,2) as 'Ultraviolet (u): ', str(err_u,7,2) as ' '";
	                cmd += " from PhotoObjAll with (nolock) where objID="+id;     
                    
                    showMagsTable(oConn,cmd,300);
                    
                    cmd = " select";
                    cmd += " str(g,7,2) as 'Green (g): ', str(err_g,7,2) as ' '";
	                cmd += " from PhotoObjAll with (nolock) where objID="+id;    

   	                showMagsTable(oConn,cmd,300);
   	
   	                cmd = " select";
                    cmd += " str(r,7,2) as 'Red (r): ', str(err_r,7,2) as ' '";
	                cmd += " from PhotoObjAll with (nolock) where objID="+id;    

   	                showMagsTable(oConn,cmd,300);   	
   	
                    cmd = " select";
                    cmd += " str(i,7,2) as 'Infrared - 7600 &Aring; (i): ', str(err_i,7,2) as ' '";
	                cmd += " from PhotoObjAll with (nolock) where objID="+id;    

   	                showMagsTable(oConn,cmd,300);   	
   	
   	
                    cmd = " select";
                    cmd += " str(z,7,2) as 'Infrared - 9100 &Aring; (z): ', str(err_z,7,2) as ' '";
	                cmd += " from PhotoObjAll with (nolock) where objID="+id;    

   	                showMagsTable(oConn,cmd,300);

                    // check the flags to see if the photometry is reliable, give the user a warning if 
                    // photometry is not reliable. This uses the checkFlags(oCmd,cmd,otype) function, which is 
                    // in ex-functions.js.
    
                    cmd = "select dbo.fPhotoFlagsN(flags) as flags, psfMagErr_g";
                    cmd += " from PhotoObjAll with (nolock) where objID="+id;

                    bool badphotometry = checkFlags(oConn,cmd,otype);
            %>

            <%  if (badphotometry) { %>
            <p><b>Caution:</b> Magnitudes and other data for this object may be unreliable. See the <b>flags</b> in the <a href='../explore/obj.aspx?id=<%= objId %>+"&spec=<%= specObjId %>' target='explore'>Explore</a> tool summary page for more information.
            <% } %>

            </table></td><td align='center' valign='top' width='50%'>

            <%
                    if (specObjId != null && !ZERO_ID.Equals(specObjId)) {
                        cmd = " select class as 'Spectral classification (Class)',";
                        cmd += " z as 'Redshift (z):', ";
                        cmd += " plate,mjd,fiberid";
                        cmd += " from SpecObjAll s";
		                cmd += " where s.specObjId="+specObjId;
		
   		                showSpecData(oConn,cmd,250);
	                }
                } // using SqlConnection
            %>
            </td></tr>
            </table>
        </td>
        </tr>
        </table>
</asp:Content>

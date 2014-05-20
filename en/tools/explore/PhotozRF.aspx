<%@ Page Title="" Language="C#" MasterPageFile="ObjectExplorer.Master" AutoEventWireup="true" CodeBehind="PhotozRF.aspx.cs" Inherits="SkyServer.Tools.Explore.PhotozRF" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<asp:Content ID="Content2" ContentPlaceHolderID="OEContent" runat="server">
<div class="content">
<table class="content">
<tr valign="top">
<td>
<h2 style="font-size:18pt;font-family: times;"><a class="content" href="../../help/browser/description.aspx?n=PhotozRF&t=U" target="_top">PhotozRF</a></h2>
<p>
<%
    using (SqlConnection oConn = new SqlConnection(globals.ConnectionString))
    {
        oConn.Open();
        master.showVTable(oConn, cmd1, 200);
    }
%>
</td>
<td>
<!--<h2 style="font-size:18pt;font-family: times;"><a href="<%=url%>/help/docs/tabledesc.aspx?key=photoz2" target="_top">PhotoZ2</a></h2>-->
<p>
<%
//	showVTable(oCmd,cmd2,200);
%>
</td>
</tr>
</table>
</div>
</asp:Content>

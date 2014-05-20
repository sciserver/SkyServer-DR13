<%@ Page Title="" Language="C#" MasterPageFile="ObjectExplorer.Master" AutoEventWireup="true" CodeBehind="PhotoZ.aspx.cs" Inherits="SkyServer.Tools.Explore.PhotoZ" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<asp:Content ID="Content1" ContentPlaceHolderID="OEHead" runat="server">
    <style type="text/css">
        div.content       
            { background-color: #ffffff; }
        table.content td  
            { font-size:9pt; font-family: arial,helvetica,sans-serif; }
        .h		            
            { background-color: #b0c4de; }
        .b		            
            { background-color: #eeeeee; }
        .t		            
            { background-color: #e0e0e0; }
        a.content		    
            { color: blue; text-decoration: underline; }
        a.content:hover	
            { color: blue; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OEContent" runat="server">
<div class="content">
<table class="content">
<tr valign="top">
<td>
<h2 style="font-size:18pt;font-family: times;"><a class="content" href="../../help/browser/browser.aspx?cmd=description+PhotoZ+U" target="_top">PhotoZ</a></h2>
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

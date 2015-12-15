<%@ Page Language="C#" MasterPageFile="../ToolsMaster.master" AutoEventWireup="true" CodeBehind="UserHistory.aspx.cs" Inherits="SkyServer.en.tools.UserHistory.GetHistory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ToolsContent" runat="server">
<style>
	#waiting {position:absolute;top:500;left:180;}
	td,p	{font-size:9pt; color:#000000}
	.tiny	{font-size:8pt; color:#000000}
	.med	{font-size:9pt; color:#000000}
	.qtitle {font-size:10pt;color:#ffffff;font-weight:bold;}
	.q	{font-size:9pt; background-color:#dddddd}
	.frame	{background-color:#bbbbbb;}
	a		{color:#000088;font-weight:normal;}
	.pos	{width:66px}
	.mag	{width:56px}
	.name	{width:200px}
	.txt	{width:260px}
</style>

<div id="title">My Query History</div>

<div id="transp">
<%--<%
// For public site, pop up results in a new window.
if( globals.Access == "public" ) {
%>
    <form method ="post" target="search" name="sql" action="x_sql.aspx">
<%
} else {
%>
    <form method ="post" name="sql" action="x_sql.aspx">
<%
}
%>--%>

    <%
// For public site, pop up results in a new window.
if( globals.Access == "public" ) {
%>
    <form method ="get" target="search" name="sql" action="x_results.aspx">
<%
} else {
%>
    <form method ="get" name="sql" action="x_results.aspx">
<%
}
%>


<input type="hidden" name="TaskName" id="TaskName" value="Skyserver.tools.UserHistory"/>
<input type="hidden" name="CustomMessageType" id="CustomMessageType" value="1"/>
<input type="hidden" name="DoShowAllHistory" id="DoShowAllHistory1" value="false"/>
<input type="hidden" name="format" id="format" value="html"/>
<input type="hidden" name="searchtool" id="searchtool" value="UserHistory"/>

<input id=submit type=submit value=Submit onmouseover="return escape('Press this to submit the query to the server.');">        

</form>
</div>

</asp:Content>



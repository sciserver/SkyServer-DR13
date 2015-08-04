<%@ Page Title="" Language="C#" MasterPageFile="../ToolsMaster.master" AutoEventWireup="true" CodeBehind="Rect.aspx.cs" Inherits="SkyServer.Tools.Search.Rect" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ToolsContent" runat="server">
<style type="text/css">
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

<div id="title">Rectangular Search</div>


<div id="transp">
<%--<%
// Pop up results in a new window for public site only.
if( globals.Access == "public" ) {
%>
    <form method="get" target="search" action="x_rect.aspx" id="rect" name="rect">
<%
} else {
%>
    <form method="get" action="x_rect.aspx" id="rect" name="rect">
<%
}
%>--%>

    <%
// Pop up results in a new window for public site only.
if( globals.Access == "public" ) {
%>
    <form method="get" target="search" action="x_results.aspx" id="rect" name="rect">
<%
} else {
%>
    <form method="get" action="x_results.aspx" id="Form1" name="rect">
<%
}
%>

<script type="text/javascript">
    window.onload = function page_load() {
        document.getElementById('labelRaOrL').innerHTML = "RA"; // give RA/Dec option by default
        document.getElementById('labelDecOrB').innerHTML = "Dec"; // give RA/Dec option by default
        document.getElementById('min_ra').value = "258.2";
        document.getElementById('max_ra').value = "258.3";
        document.getElementById('min_dec').value = "64";
        document.getElementById('max_dec').value = "64.1";
    }

    function changeSearchType(newtype) {
        //        alert("in fcn");
        //alert(newtype);
        if (newtype == 'galactic') {
            document.getElementById('labelRaOrL').innerHTML = "Galactic Longitude (<i>l</i>)";
            document.getElementById('labelDecOrB').innerHTML = "Galactic Latitude (<i>b</i>)";
            document.getElementById('min_ra').value = "89.98";
            document.getElementById('max_ra').value = "90.03";
            document.getElementById('min_dec').value = "-7.03";
            document.getElementById('max_dec').value = "-6.98";
            document.getElementById('galactic').checked = "checked";
        }
        else {
            document.getElementById('labelRaOrL').innerHTML = "RA";
            document.getElementById('labelDecOrB').innerHTML = "Dec";
            document.getElementById('min_ra').value = "258.2";
            document.getElementById('max_ra').value = "258.3";
            document.getElementById('min_dec').value = "64";
            document.getElementById('max_dec').value = "64.1";
            document.getElementById('equitorial').checked = "checked";

        }

    }

    function goToWindow() {
        var w = window.open("", 'search');
        w.focus();

    }
</script>
<input type="hidden" name="searchtool" id="searchtool" value="Rectangular" />
<table BORDER=0 WIDTH="600" bgcolor=#aaaaaa>
	<tr><td class="frame">
	NOTE: To be fair to other users, queries run from SkyServer search
	tools are restricted in how long they can run and how much output they
	return, by <b>timeouts</b> and <b>row limits</b>.  Please see the <a
	href="<%=url%>/help/docs/limits.aspx">Query Limits help page.</a>
<%
	if( globals.ReleaseNumber > 1 ) {
		Response.Write("To run a query that is not restricted by a timeout or number of rows returned,");
		Response.Write(" please use the <a href=\""+globals.CasJobs+"\">CasJobs batch query service.</a>");
	}
%>
	</td></tr>
	<tr VALIGN=top><td ALIGN=middle>
		<table BORDER=2 WIDTH="100%" BGCOLOR="#aaaaaa"  cellpadding=1 cellspacing=1>
			<tr ALIGN=middle VALIGN=center>
                <td width="30%" class="qtitle">Type of search</td>
                <td WIDTH="70%" class="q" colspan="2">
                    <input type="radio" name="whichway" value="equitorial" onclick="javascript: changeSearchType('equitorial');" checked />Equitorial ( RA / Dec )&nbsp;&nbsp;&nbsp;&nbsp;
                    <input type="radio" name="whichway" value="galactic" onclick="javascript: changeSearchType('galactic');" />Galactic (<i>l</i> and <i>b</i>)&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
			</tr>
			<tr ALIGN=middle VALIGN=center>
				<td><input TYPE="FLOAT" id="min_ra" NAME="min_ra" SIZE="20" value="258.2"></td>
 				<td width="40%" class="qtitle"><asp:Label ID="labelRaOrL"></asp:Label></td>
				<td><input TYPE="FLOAT" id="max_ra" name="max_ra" SIZE="20" value="258.3"></td></tr>
			<tr ALIGN=middle VALIGN=center>
				<td><input TYPE="FLOAT" id="min_dec" NAME="min_dec" SIZE="20" value="64"></td>
 				<td width="40%" class="qtitle"><asp:Label ID="labelDecOrB"></asp:Label></td>
				<td><input TYPE="FLOAT" id="max_dec" NAME="max_dec" SIZE="20" value="64.1"></td></tr>
		</table>
	</td></tr>

	<tr VALIGN=top><td ALIGN=middle>
<table BORDER=2 WIDTH="100%" BGCOLOR="#aaaaaa" cellpadding=1 cellspacing=1>
 <tr ALIGN=middle VALIGN=center>
  <td class="qtitle">&nbsp;</td>
  <td class="qtitle">Min</td>
  <td class="qtitle">&nbsp;</td>
  <td class="qtitle">Max</td>
  </tr>
 <tr ALIGN=middle VALIGN=center>
  <td class="qtitle"><input id=query_u name=check_u type=checkbox class="box" value=u></td>
  <td class="q"><input TYPE="FLOAT" NAME="min_u" VALUE="0" SIZE="5"></td>
  <td class="qtitle">u</td>
  <td class="q"><input TYPE="FLOAT" NAME="max_u" VALUE="20" SIZE="5"></td>
  </tr>
 <tr ALIGN=middle VALIGN=center>
  <td class="qtitle"><input id=check_g name=check_g type=checkbox class="box" value=g></td>
  <td class="q"><input TYPE="FLOAT" NAME="min_g" VALUE="0" SIZE="5"></td>
  <td class="qtitle">g</td>
  <td class="q"><input TYPE="FLOAT" NAME="max_g" VALUE="20" SIZE="5"></td>
  </tr>
 <tr ALIGN=middle VALIGN=center>
  <td class="qtitle"><input id=check_r name=check_r type=checkbox class="box" value=r></td>
  <td class="q"><input TYPE="FLOAT" NAME="min_r" VALUE="0" SIZE="5"></td>
  <td class="qtitle">r</td>
  <td class="q"><input TYPE="FLOAT" NAME="max_r" VALUE="20" SIZE="5"></td>
  </tr>
 <tr ALIGN=middle VALIGN=center>
  <td class="qtitle"><input id=check_i name=check_i type=checkbox class="box" value=i></td>
  <td class="q"><input TYPE="FLOAT" NAME="min_i" VALUE="0" SIZE="5"></td>
  <td class="qtitle">i</td>
  <td class="q"><input TYPE="FLOAT" NAME="max_i" VALUE="20" SIZE="5"></td>
  </tr>
 <tr ALIGN=middle VALIGN=center>
  <td class="qtitle"><input id=check_z name=check_z type=checkbox class="box" value=z></td>
  <td class="q"><input TYPE="FLOAT" NAME="min_z" VALUE="0" SIZE="5"></td>
  <td class="qtitle">z</td>
  <td class="q"><input TYPE="FLOAT" NAME="max_z" VALUE="20" SIZE="5"></td>
  </tr>
</table>
	</td></tr>

	<tr><td>
	<table BORDER=0 WIDTH="100%" >
  	<tr class='q'>
			<td ALIGN=left width="20%">Output Format</td>
			<td nowrap width="13%" ALIGN=middle><input name=format value="html" type=radio class="box" CHECKED> HTML</td>
			<td nowrap width="13%" ALIGN=middle><input name=format value="xml"  type=radio class="box">XML</td>
			<td nowrap width="13%" ALIGN=middle><input name=format value="csv"  type=radio class="box">CSV</td>
      <td nowrap width="13%" ALIGN=middle><input name=format value="json"  type=radio class="box">JSON</td>
      <td nowrap width="13%" ALIGN=middle><input name=format value="votable"  type=radio class="box">VOTable</td>
      <td nowrap width="13%" ALIGN=middle><input name=format value="fits"  type=radio class="box">FITS</td>
		</tr>
		<tr>
			<td align="left"><input id=submit type="submit" value="Submit Request"></td>
			<td colspan="5">Limit number of output rows (0 for max) to <input type=text name="limit" size=5 value="10"></td>
			<td align="right"><input id=reset  type="reset" value="Reset Form"></td>
		</tr>
  </table>
	</td></tr>

	<tr VALIGN=top><td>
	Enter the lower and upper limits for both
	<em>ra</em> and <em>dec</em> either in degrees or in h:m:s, d:m:s notation. 
	The lower limit on dec should always be less than the upper limit. The range
	in either coordinates should be less than 0.2 degrees, to contrain the number
	of objects returned, for the time being. 	If the lower
	limit on ra is less than the upper limit, it will be interpreted as a wrap
	around ra=0. E.g. 345 &lt;ra&lt;15 means to search between 23h and 1h.
	Check the magnitudes you would like to constrain in your query. If you
	prefer not to use specific attributes,leave those rows unchecked.
	(If you do not insert constraints and select all entries, you will receive 
	many records!)
	</td></tr>
</table>
</form>
</div>
</asp:Content>

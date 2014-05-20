<%@ Page Title="" Language="C#" MasterPageFile="../../HelpMaster.master" AutoEventWireup="true" CodeBehind="MoreThanOnce2.aspx.cs" Inherits="SkyServer.Help.Cooking.MoreThanOnce2" %>
<%@ Import Namespace="SkyServer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HelpContent" runat="server">
<div id="title">Multiple Observations</div>


<div id="transp">
<table width="600" border="0" cellspacing="3" cellpadding="5">

<tr><td>

<p><a href="default.aspx">Back to Stars index</a></p>

<h2>Search with SQL (<a href="../../../tools/search/sql.aspx" target="tools">open in new window</a>)</h2>

<hr />

<ol>
  <li><p>Go to the <a href="../../../tools/search/sql.aspx" target="tools">SQL Search</a> tool (link 
  opens in new window). To get there from the astronomers' main page, look under <i>Search Tools</i>. 
  To get there from the public main page, look under <i>SkyServer Tools</i>, then <i>Search</i>. Or, you 
  can practice the query in the query box at the bottom of this page.</p></li>
  
  <li><p>In the main window, type the following query (note that lines preceded by -- are comments):</p>
  
  <table width="100%" class="code">
    <tr>
      <td>SELECT top 100 h.objid, m.objid2		-- return object IDs of both observations</td>
    </tr>
    <tr>
      <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-- for 100 objects</td>
    </tr>
    <tr>
      <td>FROM matchhead h, match m&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-- from "match" tables</td>
    </tr>
    <tr>
      <td></td>
    </tr>
    <tr>
      <td>WHERE h.objid = m.objid1&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-- observations match between tables</td>
    </tr>
    <tr>
      <td>&nbsp;&nbsp;&nbsp;&nbsp;AND h.matchcount > 1&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-- more than one observation</td>
    </tr>
    <tr>
      <td>&nbsp;&nbsp;&nbsp;&nbsp;AND m.type1 = 6&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-- observation 1 is of a star</td>
    </tr>
    <tr>
      <td>&nbsp;&nbsp;&nbsp;&nbsp;AND m.type2 = 6&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-- observation 2 is of a star</td>
    </tr>
  </table>

    <p>The query returns the object ID of each observation of the star, for 100 stars.</p>
    </li>
    
    <li><p>To learn more about each observation of the star, you will need to go to another tool. Save the object 
           ID of each observation; you will need it to find the observations later. One way to save the 
           object IDs is to request the query results as <i>CSV</i>, then save the resulting CSV file to your machine.</p></li>


  
</ol>

    <p>Use the query box below to practice running the query shown above. You can modify the query to return other data if 
    you want. Click <b>Submit</b> to run the query or <b>Reset</b> to reset the query box.</p>


	<%	
        string query = "SELECT top 100 h.objid, m.objid2\n";
        query += "-- return object IDs of both observations\n";
        query += "\n";
        query += "FROM matchhead h, match m  -- from \"match\" tables\n";
        query += "\n";
        query += "WHERE h.objid = m.objid1 -- observations match between tables\n";
        query += "AND h.matchcount > 1 	 -- more than one observation\n";
        query += "AND m.type1 = 6      	 -- observation 1 is of a star\n";
        query += "AND m.type2 = 6		 -- observation 2 is of a star";

	   	ResponseAux.sqlform(0,query,url,Response); 	%>

<hr />

<p><a href="morethanonce3.aspx">Next: Explore the observations of each star</a></p>

</td>
</tr>

<tr>
  <td></td>
</tr>
<tr>
      <td>&nbsp;<a href="morethanonce.aspx"><img align="left" src="../../../proj/images/previous.jpg" alt="Go to the previous page" /></a>
      <a href="morethanonce3.aspx"><img align="right" src="../../../proj/images/next.jpg" alt="Go to the next page" /></a></td>
</tr>

<tr>
  <td>
    <p><a href="../">Back to Cooking with Sloan main page</a><br />
    <a href="../../">Back to Help main page</a><br />
    <a href="<%=globals.ContactUrl+"&url="+Server.UrlEncode("http://"+Request.ServerVariables["SERVER_NAME"]+Request.ServerVariables["SCRIPT_NAME"]+Request.ServerVariables["QUERY_STRING"])%>">Contact Help Desk </a></p>
  </td>
</tr>

</table>
</div>
</asp:Content>

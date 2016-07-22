<%@ Page Title="" Language="C#" MasterPageFile="SearchHowtoMaster.master" AutoEventWireup="true" CodeBehind="OrderBy.aspx.cs" Inherits="SkyServer.Help.Howto.OrderBy" %>
<%@ Import Namespace="SkyServer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="SearchHowtoContent" runat="server">
<script>
function popup(filename,target,w,h) {
    var link = filename;
    var w = window.open(link,target,'width='+w+',height='+h+',resizeable,scrollbars');
	w.focus();
}
</script>
<div id="title">SQL Tutorial</div>
<div id="transp">
  <table WIDTH="600" border="0" cellspacing="3" cellpadding="3">
    <tr>
      <td>
      <p></p>
      <a name="groupby"></a>
      <h1>The <i>Order By</i> Command</h1>
      
      <p>Sometimes, you might want to ask questions containing the 
      words "the most" and "the least." For example, you might want to know on which 
      dates specific SDSS spectroscopic plates were observed. One easy way to 
      answer this question would be to retrieve those plates ordered by the 
      <a href="http://en.wikipedia.org/wiki/Julian_day#Alternatives">Modified 
      Julian date (MJD)</a> on which they were observed, in order from the first 
      date to the last date. SQL's <em>ORDER BY</em> command will sort records in 
      ascending order according to a specified column.</p>
      
      <p>The "plateX" table contains data on SDSS plates (spectroscopic observations of 
      one area of sky). Plates are organized by two numbers: the plate number (<i>plate</i>) 
      and the observation date (<i>mjd</i>). Sometimes multiple plates are observed in one 
      night, and sometimes a single plate is spread out over several nights, so both numbers 
      are necessary.</p>
      
      <p>To look at a few plates - in this case, just the ones numbered 275 or less - 
      in order of observation date - use the ORDER BY command. The syntax of 
      ORDER BY is simple: just type "order by" followed by the name of the data column 
      by which you want to sort. If you want the data sorted 
      in descending order, from most to least, add "desc" to the end of the 
      command.</p>
      
      <p>To sort plates by observation date, then, use the query 
      below:</p>

      <table border="1" cellpadding="7" cellspacing="0" bgcolor="#552255">      
        <tr>
          <td>
            <pre>
select mjd,plate
from plateX
where plate <= 275
order by mjd
            </pre>
          </td>
        </tr>
      </table>
            
	  <p>The order by command also works with the group by command. Use them 
	  both together when you want to sort groups of records by a trait you 
	  chose to return. Put the group by command after the where block, 
	  followed by the order by command. Make sure that the column you list 
	  in the order by command is also in the group by command OR contains an 
	  aggregate function.</p>
	  
      
      <h1>Try It!</h1>
      
      <p>Modify the query in the textbox below to order by plate number instead of MJD. Which of the plates 
      was observed over more than one night?
      &nbsp;&nbsp;&nbsp;<a href=javascript:popup('answers_orderby.aspx','sidebar',300,400)>Answer</a></p>
      
      <p>See the <a href="<%=url%>/tools/getimg/plate.aspx">Plate Browser</a> for a list of the plates 
      available in Data Release <%=globals.ReleaseNumber%>.</p>
      
    

    </td></tr>
    <%  
    string query = "select mjd,plate\n";
    query += "from plateX\n";
    query += "where plate <= 275\n";
    query += "order by mjd";

    ResponseAux.sqlform(0,query,url,Response);
    %>
    
    <tr>
      <td><a href="groupby.aspx">
      <img align="left" src="../../../proj/images/previous.jpg"></a> <a href="practice4.aspx">
      <img align="right" src="../../../proj/images/next.jpg"></a></td>
    </tr>
  </table>
</div>
</asp:Content>

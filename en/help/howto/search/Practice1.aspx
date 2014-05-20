<%@ Page Title="" Language="C#" MasterPageFile="SearchHowtoMaster.master" AutoEventWireup="true" CodeBehind="Practice1.aspx.cs" Inherits="SkyServer.Help.Howto.Practice1" %>
<%@ Import Namespace="SkyServer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="SearchHowtoContent" runat="server">
<script>
    function popup(filename, target, w, h) {
        var link = filename;
        var w = window.open(link, target, 'width=' + w + ',height=' + h + ',resizeable,scrollbars');
        w.focus();
    }
</script>
<div id="title">SQL Tutorial</div>
<div id="transp">
  <table WIDTH="600" border="0" cellspacing="3" cellpadding="3">
    <tr>
      <td>
      <p></p>
      <h1>Writing Simple Queries</h1>
      <p>Now it's your turn to write some SQL queries! The query window 
      below contains the sample query from the last page. Modify it to 
      answer the questions in Practice 1 and Practice 2 below.</p>
	  <table class=exercise width=600 cellpadding=5 cellspacing=0>   
        <tr>
          <td width="100%">
          <p><strong>Practice 1.</strong> What objects has the SDSS seen 
          in a smaller area of the sky near <br>ra = 140.5, dec = 20.5 (the same 
          area you searched in the previous query)?</p> 
          <p>Modify the previous query so it will return ra and dec of 
          objects where the ra is between 140.25 and 140.75 and the 
          dec is between 20.25 and 20.75. About how many objects did the query 
          return? Does this seem to make sense, given the smaller area you searched 
          here?</p>
          </td>
        </tr>
      </table>
      <p></p>
	  <table class=exercise width=600 cellpadding=5 cellspacing=0>   
      <tr>
        <td>
          <p><strong>Practice 2.</strong> Which of the objects you found in 
          Practice 1 are galaxies?</p>
          <p>Open the
          <a target="tools" href="../../browser/browser.aspx">
          Schema Browser</a>. Click on views and select SpecObj. 
          You will see a list of all the columns in the SpecObj 
          table, along with short descriptions of what they mean and 
	  if applpicable, what values the column may have. For instance, 
          if you scroll down to the "class" column, its description says 
          that it may have one of the following values: 'STAR',
	  'GALAXY', or 'QSO'.  
	  Modify your query so that it returns the ra, dec, and best 
	  object ID for galaxies (and only galaxies) whose ra is 
          between 140.25 and 140.75 and whose dec is between 20.25 and 20.75.
        </td>
      </tr>
    </table>
    <a name="answers"></a>
    <p><a href="#answers" onclick="javascript:popup('answers1.html','popup',350,500);">Answers</a></p>
      <p></p>
 
    
 	</td></tr>

	<%
		string query = "select ra,dec\n";
		query +=	"from specObj\n";
		query +=	"where ra BETWEEN 140 and 141 AND\n";
		query +=	"dec BETWEEN 20 and 21\n";
	    ResponseAux.sqlform(0,query,url,Response); 
	%>

    <tr>
      <td><a href="default.aspx"><img align="left" src="../../../proj/images/previous.jpg"></a>
      <a href="commonsearch.aspx"><img align="right" src="../../../proj/images/next.jpg"></a></td>
    </tr>
  </table>
</div>
</asp:Content>

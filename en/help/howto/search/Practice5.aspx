<%@ Page Title="" Language="C#" MasterPageFile="SearchHowtoMaster.master" AutoEventWireup="true" CodeBehind="Practice5.aspx.cs" Inherits="SkyServer.Help.Howto.Practice5" %>
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
<div id="transp">  <table WIDTH="600" border="0" cellspacing="3" cellpadding="3">
    <tr>
      <td>
           <p></p>
      <h1>Practice Using Functions</h1>
      
	  <p>Write queries to answer the astronomy questions in Practice 10 and 11. 
	  Use the <A href="../../browser/browser.aspx" target=tools>Schema 
	  Browser</a> when you need it.</p>
      
	  <table class=exercise width=600 cellpadding=5 cellspacing=0>
        <tr>
          <td width="100%">
          <p><strong>Practice 10.</strong> In the field 5112-6-119, what percentage 
          of all objects detected by the SDSS are too close to the edge 
          of their fields to be trusted?</p>
          </p>
          <p><b>Hint:</b> Use two searches, one with a flag and one 
          without. Search run=5112, camcol=6, field=119. </p>
          </td>
        </tr>
      </table>
      <p></p>
	  <table class=exercise width=600 cellpadding=5 cellspacing=0>
	    <tr>
	      <td>      
          <p><strong>Practice 11.</strong> Choose a galaxy cluster from SkyServer's 
          <a href="../../../tools/places/" target="tools">Famous Places</a> tool. 
          Write a query to select galaxies in the cluster, and only galaxies in 
          the cluster.</p>
          <p><b>Hint:</b> After you pick a cluster, use the <a href="../../../tools/chart/navi.aspx" target="tools">
          Navigation Tool</a> to examine the cluster. Guess which galaxies 
          belong to the cluster - you should be able to tell just by looking. 
          Click on 5-10 cluster galaxies and save them in your online notebook. 
          Open the notebook, and look for features that the cluster galaxies 
          have in common. Guess the center position and radius of 
          the galaxies. Then, write a query that uses what you have learned 
          to search for cluster galaxies.</p>                  
          </td>
        </tr>
      </table>
      
      <p></p>
            
      <p></p>
      <a name="answers"></a>
      <p><a href="#answers" onclick="javascript:popup('answers5.html','sidebar',500,500)">
      Answers</a></p>
      
      <h1>An Afterthought: Procedures and Constants</h1>
      
      <p>In addition to tables, views, and functions, the Schema Browser contains 
      entries for procedures and constants. <em>Procedures</em>, like functions, are 
      mini-programs that the database can execute. Unlike functions, they 
      do not output a single value; rather, they manipulate the inputs passed 
      in to them. Procedures are called with the syntax</p>
      
      <p>dbo.spProcedurename(inputs)</p>
      
      <p>Nearly all of the procedures in SkyServer are used by the web 
      server or tools - you will hardly ever use them in your queries. If 
      you ever need them, the <a href="../../browser/browser.aspx" target="tools">
      Schema Browser</a> will tell you what they do and how to use them.</p>
      
      <p><em>Constants</em> are, well, constant. They can be displayed and 
      searched just like tables can. See the <a href="../../browser/browser.aspx" target="tools">
      Schema Browser</a> for a complete description of SkyServer's constants.</p>
      
      <p>You're done! Click Next for the conclusion.</p>
      

</td></tr>
<%  ResponseAux.sqlform(0,"",url,Response); %>

    <tr>
      <td><a href="functions.aspx"><img align="left" src="../../../proj/images/previous.jpg"></a>
      <a href="conclusion.aspx"><img align="right" src="../../../proj/images/next.jpg"></a></td>
    </tr>
  </table>
</div>
</asp:Content>

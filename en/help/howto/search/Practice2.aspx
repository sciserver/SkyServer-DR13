<%@ Page Title="" Language="C#" MasterPageFile="SearchHowtoMaster.master" AutoEventWireup="true" CodeBehind="Practice2.aspx.cs" Inherits="SkyServer.Help.Howto.Practice2" %>
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
      <h1>More Practice Queries</h1>
      
      <p>Write queries to answer the astronomy questions in Practice 3 and 4. 
      Use the <a target="tools" href="../../browser/browser.aspx">Schema 
      Browser</a> when you need it.</p>
      
	  <table class=exercise width=600 cellpadding=5 cellspacing=0>
        <tr>
          <td width="100%">
          <p><strong>Practice 3. </strong>What are the reddest galaxies in the 
          area of sky near ra = 141?</p>
          <p>Write a query to search for galaxies between ra = 140.9
          and ra = 141.1 brighter than <br>g = 18.0 
          for which u - g &gt; 2.2. Retrieve the Object ID, ra, 
          dec, and the five final magnitudes.</p>
          </td>
        </tr>
      </table>
      
      <p></p>
      
      <table class=exercise width=600 cellpadding=5 cellspacing=0>
        <tr>
          <td>
          <p><strong>Practice 4.</strong> What are the highest-redshift 
          quasars in the SDSS database?</p>
          <p>Write a query to search for quasars for which we have 
          obtained spectra (search the specObj table) with  
          redshifts greater than 4.5 and good measurements (zWarning = 0). 
          Retrieve each quasar's Photo ID, ra, dec, and redshift.</p>
          <p>How many results do you get?</p></td>       
          
        </tr>
      </table>
          
      <a name="answers"></a>
      <p><a href="#answers" onclick="javascript:popup('answers2.html','sidebar',400,600)">Answers</a></p>
      <p></p>
      

	</td></tr>

	<%  ResponseAux.sqlform(0,"",url,Response); %>
   
    <tr>
      <td><a href="query2.aspx">
      <img align="left" src="../../../proj/images/previous.jpg"></a> <a href="query3.aspx">
      <img align="right" src="../../../proj/images/next.jpg"></a></td>
    </tr>
  </table>
</div>
</asp:Content>

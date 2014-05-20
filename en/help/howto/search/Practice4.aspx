<%@ Page Title="" Language="C#" MasterPageFile="SearchHowtoMaster.master" AutoEventWireup="true" CodeBehind="Practice4.aspx.cs" Inherits="SkyServer.Help.Howto.Practice4" %>
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
      <h1>Practice Using Aggregate Functions, Group By, and Order By</h1>
      
	  <p>Write queries to answer the astronomy questions in Practice 7, 8, and 9. 
	  Use the <A href="../../browser/browser.aspx" target=tools>Schema 
	  Browser</a> when you need it.</p>
      
	  <table class=exercise width=600 cellpadding=5 cellspacing=0>
        <tr>
          <td width="100%">
          <p><strong>Practice 7.</strong> What are the northernmost and southernmost 
	  objects with spectra measured by the SDSS?</p>         
          </td>
        </tr>
      </table>
      <p></p>
	  <table class=exercise width=600 cellpadding=5 cellspacing=0>
	    <tr>
	      <td>      
          <p><strong>Practice 8.</strong> What is the redshift of the nearest galaxy 
          whose spectrum was measured by the SDSS with high confidence (zWarning=0)? 
          Use the equation</p>
          <p>cz = H<sub>0</sub>d</p>
          <p>to find the distance to the galaxy in light-years. The speed of 
          light, c, is 1 light-year per year. The constant H<sub>0</sub>, called 
          the Hubble constant, is about 70 km/s/Mpc, or <br>7.11 x 10<sup>-11</sup> / yr.</p>
          <p>Compare the distance you found to the distances to the Andromeda 
          Galaxy (2 million light-years) and the Whirlpool Galaxy (37 million 
          light-years). Does the distance you found seem reasonable?</p></td>
                  
          </td>
        </tr>
      </table>
      
      <p></p>
      
	  <table class=exercise width=600 cellpadding=5 cellspacing=0>
	    <tr>
	      <td>      
          <p><strong>Practice 9.</strong> What field has galaxies with 
          the highest average redshifts in run=5112, camcol=1?</p>
          <p>Be sure you are searching fields (as run-camcol-field) for 
          galaxies. Also look at how many spectrally measured galaxies 
          are in the field - make sure you don't pick a field with only one or two 
          galaxies! Also, note that this query will probably take a long time to 
          execute.</p></td>
                  
          </td>
        </tr>
      </table>
      
      <p></p>
      <a name="answers"></a>
      <p>Each practice question can be answered in different ways, but 
      here are some <a href="#answers" onclick="javascript:popup('answers4.html','sidebar',500,500)">
      Sample Answers</a>.</p>
      
      <p>Notice that these practice questions have gotten more difficult, and 
      are more like the questions that astronomers really ask in their 
      research. You're getting better in your searches!</p>


</td></tr>
<%  ResponseAux.sqlform(0,"",url,Response); %>

    <tr>
      <td><a href="orderby.aspx"><img align="left" src="../../../proj/images/previous.jpg"></a>
      <a href="views.aspx"><img align="right" src="../../../proj/images/next.jpg"></a></td>
    </tr>
  </table>
</div>
</asp:Content>

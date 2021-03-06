﻿<%@ Page Title="" Language="C#" MasterPageFile="ColorMaster.master" AutoEventWireup="true" CodeBehind="Explore.aspx.cs" Inherits="SkyServer.Proj.Advanced.Color.Explore" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ColorContent" runat="server">
<div id="transp">
<table WIDTH=600 border=0 cellspacing="3" cellpadding="3">
<tr><td>

<h1>Colors of Stars in the SDSS</h1>

<p>First, take a look at some of the stars in the SDSS database. The next 
two Explore exercises will let you examine stars with SkyServer. You will see the 
colors for yourself, and you will try to discover patterns that could explain 
why stars come in different colors.</p>

<a name="ex1"></a>
      <table class=exercise width=600 cellpadding=5 cellspacing=0>
        <tr>
          <td width="100%">
          <p><strong>Explore 1</strong>. Look through SkyServer's database 
          and find several stars whose colors appear different. Find some blue 
          stars, some red stars, some yellow stars, and some white stars.</p>
          <p>To search through the stars, you will use SkyServer's Navigation tool. 
          Click the link below to launch the tool. The tool will open in a new window, 
          which will show a part of the sky. Use the Zoom buttons (the magnifying glasses 
          and blue rectangles) to zoom in our out. Use the NWSE buttons to move around 
          to different parts of the sky.</p>
          <p>Click on a star to see its data. The data will appear in a box on the right side of 
          the window - "ra" and "dec" give the star's position, "type" tells you whether 
          it is a star or a galaxy, and u,g,r,i, and z are the star's "magnitudes." You'll 
          learn more about magnitudes later; for now, you'll search for patterns in the 
          magnitudes.</p>
          <p>When you find a star you are interested in, record its data in this 
          <a href="workbooks/explore1.xls">workbook</a>. Record its position (RA and Dec), color, 
          and its five magnitudes (u,g,r,i,z). Record data for 10-15 stars. You will use 
          these data in the next activity.</p>
          <p><a target="tools" 
          href="../../../tools/chart/navi.aspx">Launch the Navigation Tool</a>
          <p><a href="workbooks/explore1.xls">Download Explore 1 workbook</a><br />
          The workbook is an Excel 
          spreadsheet. If you do not have Excel, you can open it with <a href="http://spreadsheets.google.com" target="offsite">
          Google Spreadsheets</a> or <a href="http://www.openoffice.org" target="offsite">Open Office</a>.</p>
          </td>
        </tr>
      </table>

<p></p>

<table class=exercise width=600 cellpadding=5 cellspacing=0><tr>
   <td><p><strong>Explore 2.</strong> Now, see if you can discover a pattern in the colors.</p>
      
    <p>Look at the data you have saved in your workbook. The directions here tell you how to 
    analyze the data in Excel; to analyze the data in other graphing programs, you would follow 
    similar steps. Click on the first row of data, then use the tab or right arrow key to move the highlighted 
    box to the column to the right of your data. Type the color of the first star and 
    hit enter. Repeat this process to type in the colors of all the stars you saw.</p>
    
    <p>Now, click on one of the cells in your spreadsheet and select Sort from the 
    Data menu. Sort by u in ascending order. Do you see the colors group into any 
    patterns? Repeat the sort by g, r, i, and z. Do you see any patterns now?</p>
    
    <p>Next, create another column to the right of the colors. Label this column u-g. 
    Click on the u-g column for the row for the first star. Type an equal (=) sign. 
    Click on the box with the first star's u value. Type a minus (-) sign. Click on the 
    box with the first star's g value. Press enter. Then, click the small square in the 
    lower right corner of the cell you just entered and drag down to the last row of data. Excel will 
    automatically repeat the subtraction for the other stars.</p>
    
    <p>Repeat this procedure to get columns for g-r, r-i, and i-z. Now, sort the data by 
    u-g, g-r, r-i, and i-z. What patterns do you see now? What column of data gives you 
    the clearest pattern?</p>
    
    </td>
</tr></table>

</td></tr>

<tr><td><p></p></td></tr>
<tr><td><p></p></td></tr>

<tr><td><a href="default.aspx" ><IMG src="../../images/previous.jpg" align=left></a>
<a href="definition.aspx"><IMG src="../../images/next.jpg" align=right></a></td></tr>

</table>
<P>

</div>
</asp:Content>

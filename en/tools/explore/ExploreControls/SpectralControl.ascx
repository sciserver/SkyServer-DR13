<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpectralControl.ascx.cs" Inherits="SkyServer.Tools.Explore.SpectralControl" %>
<%if(specId !=null && !specId.Equals("")){  %>
 <div id="spectro">

   <h3>Optical Spectra</h3>
      
    <div class="infobox">
        <table width="100%">
            <tr>
                <td width="60%">
                  <p><b> SpecObjID = <%=specObjId%></b></p>                
                </td>
                <td width="40%">
                    <% string spectrumlink = globals.DasUrl + "spectrumDetail?plateid=" + plate + "&mjd=" + mjd + "&fiber=" + fiberid; %>
                    <p><b>
                      <a class='content' href="<%=spectrumlink%>"  target='_blank'>
                          Interactive spectrum<img src='../../images/new_window_black.png' alt=' (new window)' />
                       </a>
                    </b></p>
                </td>
            </tr>
        </table>
   </div>
   <table class="content">
        <tr>
            <%             
                    string instrumentLink = globals.SdssUrlBase + "instruments";            
            %>
	   	      <td>             
		            <a href="../../get/SpecById.ashx?id=<%= specObjId %>">
		                <img alt="" src="../../get/SpecById.ashx?id=<%= specObjId %>" width="316" height="253" border="0" align="left" />
		            </a>
              </td>
              <td>                     
                 <table cellpadding=2 cellspacing=2 border=0 width=300>
                   <tr align='left' >
                       <td  valign='top' class='h'><span>plate</span></td>
                       <td valign='top' class='t'>2618</td>
                   </tr>
                  <tr align='left' >
                      <td  valign='top' class='h'><span>mjd</span></td>
                      <td valign='top' class='t'>54506</td>
                  </tr>
                  <tr align='left' >
                      <td  valign='top' class='h'><span>fiberid</span></td>
                      <td valign='top' class='t'>310</td>
                  </tr>
                   <tr align='left' >
                       <td  valign='top' class='h'><span>Spectrograph</span></td>
                       <td valign='top' class='t'>SDSS</td>
                   </tr>
                   <tr align='left' >
                       <td  valign='top' class='h'><span>class</span></td>
                       <td valign='top' class='t'>GALAXY</td>
                   </tr>
                   <tr align='left' >
                       <td  valign='top' class='h'><span>Redshift (z)</span></td>
                       <td valign='top' class='t'>  0.012</td>
                   </tr>
                   <tr align='left' >
                       <td  valign='top' class='h'><span>Redshift error</span></td>
                       <td valign='top' class='t'>   0.00001</td>
                   </tr>
                   <tr align='left' >
                       <td  valign='top' class='h'><span>Redshift flags</span></td>
                       <td valign='top' class='t'>OK</td>
                   </tr>
                   <tr align='left' >
                       <td  valign='top' class='h'><span>survey</span></td>
                       <td valign='top' class='t'>sdss</td>
                   </tr>
                   <tr align='left' >
                       <td  valign='top' class='h'><span>programname</span></td>
                       <td valign='top' class='t'>legacy</td>
                   </tr>
                   <tr align='left' >
                       <td  valign='top' class='h'><span>primary</span></td>
                       <td valign='top' class='t'>1</td>
                   </tr>
                   <tr align='left' >
                       <td  valign='top' class='h'><span>Other spec</span></td>
                       <td valign='top' class='t'>0</td>
                   </tr>
                   <tr align='left' >
                       <td  valign='top' class='h'><span>sourcetype</span></td>
                       <td valign='top' class='t'>GALAXY</td>
                   </tr>
                   <tr align='left' >
                       <td  valign='top' class='h'><span>Velocity dispersion (km/s)</span></td>
                       <td valign='top' class='t'> 69.11</td>
                   </tr>
                   <tr align='left' >
                       <td  valign='top' class='h'><span>veldisp_error</span></td>
                       <td valign='top' class='t'>  6.176</td>
                   </tr>
                   <tr align='left' >
                       <td  valign='top' class='h'><span>targeting_flags</span></td>
                       <td valign='top' class='t'>GALAXY GALAXY_RED   </td>
                   </tr>
                 </table>
	   	      </td>            
        </tr>   
    </table> 
</div>  <!-- end of spectro div -->
<%} %>

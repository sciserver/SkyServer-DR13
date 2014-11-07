<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImagingControl.ascx.cs"  Inherits="SkyServer.Tools.Explore.ImagingControl" %>
<%@ Import Namespace="SkyServer.Tools.Explore" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SkyServer" %>
<%if(objId != null && !objId.Equals("")) {%>
<div id="imaging">
<h3>Imaging</h3>            

<%  if (clean == 0)  { %>
            <div class="warning">
                <table><tr><td>
                    <b>WARNING:</b> 
                    This object's photometry may be unreliable. See the photometric 
                    <em>flags</em> below.
                </td></tr></table>
            </div>

<% } %>

<table>
    <tr>
        <td>
            <table cellpadding=2 cellspacing=2 border=0 width=420>
                <tr align='left' >
                    <td  valign='top' class='h'><span ONMOUSEOVER="this.T_ABOVE=true;this.T_WIDTH='100';return escape('<i>unit</i>=test')" ></span>Flags</td>
                    <td valign='top' class='t'><a href='<%=flagsLink%>'>Flags <img src=../../images/offsite_black.png /><%=flag %></a></td>
                </tr>
            </table>
         </td>
     </tr>
</table>

<table>
    <tr>
        <td style="vertical-align:top">
             <% string link = "javascript:showNavi(" + ra + "," + dec + "," + 0.2 + ");"; %>
             <a href="<%= link%>"><img alt="" src="<%= globals.WSGetJpegUrl + "?ra=" + ra + "&dec=" + dec + "&scale=0.2&width=200&height=200&opt=G"%>" border="0" width="200" height="200" /></a>
         </td>
        <td >
            <table cellpadding=2 cellspacing=2 border=0 width=420>
                <tr>
                    <td align='middle' class='h'><span ONMOUSEOVER="this.T_ABOVE=true;this.T_WIDTH='100';" ></span></td></tr>
                <tr><td nowrap align='middle' class='t'><b>Magnitudes</b></td></tr>
            </table>
            <table cellpadding=2 cellspacing=2 border=0 width=420>
                <tr><td align='middle' class='h'><span ONMOUSEOVER="this.T_ABOVE=true;this.T_WIDTH='100';return escape('<i>unit</i>=<%=getUnit("PhotoObj","u") %>')" ></span>u</td>
                    <td align='middle' class='h'><span ONMOUSEOVER="this.T_ABOVE=true;this.T_WIDTH='100';return escape('<i>unit</i>=<%=getUnit("PhotoObj","g") %>')" ></span>g</td>
                    <td align='middle' class='h'><span ONMOUSEOVER="this.T_ABOVE=true;this.T_WIDTH='100';return escape('<i>unit</i>=<%=getUnit("PhotoObj","r") %>')" ></span>r</td>
                    <td align='middle' class='h'><span ONMOUSEOVER="this.T_ABOVE=true;this.T_WIDTH='100';return escape('<i>unit</i>=<%=getUnit("PhotoObj","i") %>')" ></span>i</td>
                    <td align='middle' class='h'><span ONMOUSEOVER="this.T_ABOVE=true;this.T_WIDTH='100';return escape('<i>unit</i>=<%=getUnit("PhotoObj","z") %>')" ></span>z</td></tr>
                <tr><td nowrap align='middle' class='t'> <%=Math.Round(u,2) %></td>
                    <td nowrap align='middle' class='t'> <%=Math.Round(g,2) %></td>
                    <td nowrap align='middle' class='t'> <%=Math.Round(r,2) %></td>
                    <td nowrap align='middle' class='t'> <%=Math.Round(i,2) %></td>
                    <td nowrap align='middle' class='t'> <%=Math.Round(z,2)%></td></tr>
            </table>
            <table cellpadding=2 cellspacing=2 border=0 width=420>
                <tr><td align='middle' class='h'><span ONMOUSEOVER="this.T_ABOVE=true;this.T_WIDTH='100';" ></span></td></tr>
                <tr><td nowrap align='middle' class='t'><b>Magnitude uncertainties</b></td></tr>
            </table><table cellpadding=2 cellspacing=2 border=0 width=420>
                <tr><td align='middle' class='h'><span ONMOUSEOVER="this.T_ABOVE=true;this.T_WIDTH='100';return escape('<i>unit</i>=<%=getUnit("PhotoObj","err_u") %>')" ></span>err_u</td>
                    <td align='middle' class='h'><span ONMOUSEOVER="this.T_ABOVE=true;this.T_WIDTH='100';return escape('<i>unit</i>=<%=getUnit("PhotoObj","err_g") %>')" ></span>err_g</td>
                    <td align='middle' class='h'><span ONMOUSEOVER="this.T_ABOVE=true;this.T_WIDTH='100';return escape('<i>unit</i>=<%=getUnit("PhotoObj","err_r") %>')" ></span>err_r</td>
                    <td align='middle' class='h'><span ONMOUSEOVER="this.T_ABOVE=true;this.T_WIDTH='100';return escape('<i>unit</i>=<%=getUnit("PhotoObj","err_i") %>')" ></span>err_i</td>
                    <td align='middle' class='h'><span ONMOUSEOVER="this.T_ABOVE=true;this.T_WIDTH='100';return escape('<i>unit</i>=<%=getUnit("PhotoObj","err_z") %>')" ></span>err_z</td>
                </tr>
                <tr><td nowrap align='middle' class='t'> <%=Math.Round(err_u,2) %></td>
                    <td nowrap align='middle' class='t'> <%=Math.Round(err_g,2) %></td>
                    <td nowrap align='middle' class='t'> <%=Math.Round(err_r,2) %></td>
                    <td nowrap align='middle' class='t'> <%=Math.Round(err_i,2) %></td>
                    <td nowrap align='middle' class='t'> <%=Math.Round(err_z,2) %></td></tr>
              </table>       
          </td>
        </tr>
        <tr>
          <td colspan="2">
            <table cellpadding=2 cellspacing=2 border=0 width=625>
               <tr><td align='middle' class='h'><span ONMOUSEOVER="this.T_ABOVE=true;this.T_WIDTH='1000';return escape('<i>unit</i>=test')" ></span>Image MJD</td>
                   <td align='middle' class='h'><span ONMOUSEOVER="this.T_ABOVE=true;this.T_WIDTH='100';return escape('<i>unit</i>=test')" ></span>mode</td>
                   <td align='middle' class='h'><span ONMOUSEOVER="this.T_ABOVE=true;this.T_WIDTH='100';return escape('<i>unit</i>=test')" ></span>Other observations</td>
                   <td align='middle' class='h'><span ONMOUSEOVER="this.T_ABOVE=true;this.T_WIDTH='100';return escape('<i>unit</i>=test')" ></span>parentID</td>
                   <td align='middle' class='h'><span ONMOUSEOVER="this.T_ABOVE=true;this.T_WIDTH='100';return escape('<i>unit</i>=test')" ></span>nChild</td>
                   <td align='middle' class='h'><span ONMOUSEOVER="this.T_ABOVE=true;this.T_WIDTH='100';return escape('<i>unit</i>=test')" ></span>extinction_r</td>
                   <td align='middle' class='h'><span ONMOUSEOVER="this.T_ABOVE=true;this.T_WIDTH='100';return escape('<i>unit</i>=test')" ></span>PetroRad_r (arcmin)</td>
               </tr>
               <tr><td nowrap align='middle' class='t'><%=mjdNum %></td>
                   <td nowrap align='middle' class='t'><%=mode %></td>
                   <td nowrap align='middle' class='t'><%=otherObs %></td>
                   <td nowrap align='middle' class='t'><%=parentId %></td>
                   <td nowrap align='middle' class='t'><%=nchild %></td>
                   <td nowrap align='middle' class='t'><%=extinction_r %></td>
                   <td nowrap align='middle' class='t'><%=petrorad_r %></td>
               </tr>
            </table>
            <table cellpadding=2 cellspacing=2 border=0 width=625>
                <tr>
                    <td align='middle' class='h'><span ONMOUSEOVER="this.T_ABOVE=true;this.T_WIDTH='100';return escape('<i>unit</i>=test')" ></span>photoZ (KD-tree method)</td>
                    <td align='middle' class='h'><span ONMOUSEOVER="this.T_ABOVE=true;this.T_WIDTH='100';return escape('<i>unit</i>=test')" ></span>photoZ (RF method)</td>
                    <td align='middle' class='h'><span ONMOUSEOVER="this.T_ABOVE=true;this.T_WIDTH='100';return escape('<i>unit</i>=test')" ></span>Galaxy Zoo 1 morphology</td>
                </tr>
                <tr>
                    <td nowrap align='middle' class='t'><%=photoZ_KD %></td>
                    <td nowrap align='middle' class='t'><%=photoZ_RF %></td>
                    <td nowrap align='middle' class='t'><%=galaxyZoo_Morph %></td>
                </tr>
            </table>                            
            </td>
        </tr>
    </table>
    </div>   
  <%--  <!-- end of imaging div  -->--%><%} %>
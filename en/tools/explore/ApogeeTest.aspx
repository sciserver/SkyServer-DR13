<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApogeeTest.aspx.cs" Inherits="SkyServer.Tools.Explore.ApogeeTest" %>
<%@ Import Namespace="SkyServer" %>
<%@ Import Namespace="SkyServer.Tools.Explore" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <link href="../../tools.css" rel="stylesheet" type="text/css" />
  <title></title>
  <script type="text/javascript">
    function showLink(thelink) {
      document.getElementById(thelink).style.display = "inline";
    }
    function hideLink(thelink) {
      document.getElementById(thelink).style.display = "none";
    }

    function showSection(thediv) {
      document.getElementById(thediv).className += "shown ";
      document.getElementById(thediv).className =
              document.getElementById(thediv).className.replace
                    (/(?:^|\s)hidden(?!\S)/g, '')
      /* code wrapped for readability - above is all one statement */
    }
    function hideSection(thediv) {
      document.getElementById(thediv).className += "hidden ";
      document.getElementById(thediv).className =
              document.getElementById(thediv).className.replace
                  (/(?:^|\s)shown(?!\S)/g, '')
      /* code wrapped for readability - above is all one statement */
    }
  </script>
</head>
<body>
  <form id="form1" runat="server">
    <div id="content" class="content">
      <div id="irspec">
        <h3>Infrared Spectra
          <span class="target">Targeted star: <%=apogeeInfo.apogee_id %></span>
        </h3>

        <table width="800">
          <tr>
            <td class="h">Instrument</td>
            <td class="t"><b>APOGEE</b></td>
          </tr>
          <tr>
            <td class="h">APOGEE ID</td>
            <td class="t"><%=apogeeInfo.apstar_id %></td>
          </tr>
        </table>

        <table width="800">
          <tr>
            <td align="center" class="h" colspan="2" width="50%">Galactic Coordinates</td>
            <td align="center" class="h" colspan="2" width="50%">RA,dec</td>
          </tr>
          <tr>
            <td align="center" class="h">Longitude (L)</td>
            <td align="center" class="h">Latitude (B)</td>
            <td align="center" class="h">Decimal</td>
            <td align="center" class="h">Sexagesimal</td>
          </tr>
          <tr>
            <td align="center" class="t"><%=apogeeInfo.glon.ToString("F5") %></td>                        
            <td align="center" class="t"><%=apogeeInfo.glat.ToString("F5") %></td>                     
            <td align="center" class="t"><%=apogeeInfo.ra.ToString("F5") %>, <%=apogeeInfo.dec.ToString("F5") %></td>
            <td align="center" class="t"><span class="large"><%=Functions.hmsC(apogeeInfo.ra) + ", " + Functions.dmsC(apogeeInfo.dec) %></span></td>
          </tr>
        </table>
            
        <table>
          <tr>
            <td colspan="2">
              <a href="<%=apogeeSpecImage %>"><img src="<%=apogeeSpecImage %>" width="780" height="195" alt="APOGEE infrared spectrum of <%=apogeeInfo.apogee_id %>" /></a>
            </td>
          </tr>
          <tr>
            <td class="irspeclink">
              <a class="content" href="<%=spectrumLink %>" target="_blank">Interactive spectrum<img src="../../images/new_window_black.png" alt=" (new window)" /></a>
            </td>
            <td class="irspeclink" align="right">
              <a class="content" href="<%=fitsLink %>" target="_blank">Download FITS<img src="../../images/new_window_black.png" alt=" (new window)" /></a>
            </td>
          </tr>
        </table>
                    
        <h3>Targeting Information</h3>
             
        <table cellpadding="2" cellspacing="2" border="0" width="800">
          <tr>
            <td align="middle" class="h"><span>2MASS j</span></td>
            <td align="middle" class="h"><span>2MASS h</span></td>
            <td align="middle" class="h"><span>2MASS k</span></td>
            <td align="middle" class="h"><span>j_err</span></td>
            <td align="middle" class="h"><span>h_err</span></td>
            <td align="middle" class="h"><span>k_err</span></td>
          </tr>
          <tr>
            <td nowrap align="middle" class="t"><%=apogeeInfo.j %></td>
            <td nowrap align="middle" class="t"><%=apogeeInfo.h %></td>
            <td nowrap align="middle" class="t"><%=apogeeInfo.k %></td>
            <td nowrap align="middle" class="t"><%=apogeeInfo.j_err %></td>
            <td nowrap align="middle" class="t"><%=apogeeInfo.h_err %></td>
            <td nowrap align="middle" class="t"><%=apogeeInfo.k_err %></td>
          </tr>
        </table>

        <table cellpadding="2" cellspacing="2" border="0" width="800">
          <tr>
            <td align="middle" class="h"><span>4.5 micron magnitude</span></td>
            <td align="middle" class="h"><span>4.5 micron magnitude error</span></td>
            <td align="middle" class="h"><span>4.5 micron magnitude source</span></td>
          </tr>
          <tr>
            <td nowrap align="middle" class="t"><%=apogeeInfo.mag_4_5 %></td>
            <td nowrap align="middle" class="t"><%=apogeeInfo.mag_4_5_err %></td>
            <td nowrap align="middle" class="t"><%=apogeeInfo.src_4_5 %></td>
          </tr>
        </table>

        <table cellpadding="2" cellspacing="2" width="800">
          <tr align="left">
            <td valign="top" class="h">APOGEE target flags 1</td>
            <td valign="top" class="b"><%=apogeeInfo.apogeeTarget1N %></td>
          </tr>
        </table>

        <table cellpadding="2" cellspacing="2" width="800">
          <tr align="left">
            <td valign="top" class="h">APOGEE target flags 2</td>
            <td valign="top" class="b"><%=apogeeInfo.apogeeTarget2N %></td>
          </tr>
        </table>

        <h3>Stellar Parameters</h3>
                
        <table cellpadding="2" cellspacing="2" border="0" width="800">
          <tr>
            <td align="middle" class="h"><span>Avg v<sub>helio</sub> (km/s)</span></td>
            <td align="middle" class="h"><span>Scatter in v<sub>helio</sub> (km/s)</span></td>
            <td align="middle" class="h"><span>Best-fit temperature (K)</span></td>
            <td align="middle" class="h"><span>Temp error</span></td>
          </tr>
          <tr>
            <td nowrap align="middle" class="t"><%=apogeeInfo.vhelio_avg %></td>
            <td nowrap align="middle" class="t"><%=apogeeInfo.vscatter %></td>
            <td nowrap align="middle" class="t"><%=apogeeInfo.teff.ToString("F0") %></td>
            <td nowrap align="middle" class="t"><%=apogeeInfo.teff_err.ToString("F1") %></td>
          </tr>
        </table>
                
        <table cellpadding="2" cellspacing="2" border="0" width="800">  
          <tr>
            <td align="middle" class="h"><span>Surface Gravity log<sub>10</sub>(g)</span></td>
            <td align="middle" class="h"><span>log(g) error</span></td>
            <td align="middle" class="h"><span>Metallicity [Fe/H]</span></td>
            <td align="middle" class="h"><span>Metal error</span></td>
            <td align="middle" class="h"><span>[&alpha;/Fe]</span></td>
            <td align="middle" class="h"><span>[&alpha;/Fe] error</span></td>
          </tr>
          <tr>
            <td nowrap align="middle" class="t"><%=apogeeInfo.logg.ToString("F2") %></td>
            <td nowrap align="middle" class="t"><%=apogeeInfo.logg_err.ToString("F3") %></td>
            <td nowrap align="middle" class="t"><%=apogeeInfo.metals.ToString("F1") %></td>
            <td nowrap align="middle" class="t"><%=apogeeInfo.metals_err.ToString("F3") %></td>
            <td nowrap align="middle" class="t"><%=apogeeInfo.alphafe.ToString("F2") %></td>
            <td nowrap align="middle" class="t"><%=apogeeInfo.alphafe_err.ToString("F3") %></td>
          </tr>
        </table>
                
        <table cellpadding="2" cellspacing="2" width="800">
          <tr align="left">
            <td valign="top" class="h">Star flags</td>
            <td valign="top" class="b"><%=apogeeInfo.apogeeStarFlagN %></td>
          </tr>
        </table>

        <table cellpadding="2" cellspacing="2" width="800">
          <tr align="left">
            <td valign="top" class="h">Processing flags (ASPCAP)</td>
            <td valign="top" class="b"><%=apogeeInfo.apogeeAspcapFlagN %></td>
          </tr>
        </table>

        <h3 class="sectionlabel">
          Visits (click to see visit spectrum)
          <a id="visits_are_shown" href="javascript:showSection('visits');javascript:showLink('visits_are_hidden');javascript:hideLink('visits_are_shown');" class="showinglink">
            Show
          </a>
          <a id="visits_are_hidden" href="javascript:hideSection('visits');javascript:showLink('visits_are_shown');javascript:hideLink('visits_are_hidden');" class="hidinglink">
            Hide
          </a>
        </h3>

        <div id="visits">
          <table cellpadding="2" cellspacing="2" border="0" width="800">
            <tr>
              <td align="middle" class="h"><span>visit_id</span></td>
              <td align="middle" class="h"><span>plate</span></td>
              <td align="middle" class="h"><span>mjd</span></td>
              <td align="middle" class="h"><span>fiberid</span></td>
              <td align="middle" class="h"><span>date</span></td>
              <td align="middle" class="h"><span>time (UTC)</span></td>
              <td align="middle" class="h"><span>vrel</span></td>
            </tr>

            <% string cellClass = "t";
               foreach (ApogeeVisit v in apogeeInfo.visits)
               { %>
            <tr>
              <td nowrap align="middle" class="<%=cellClass %>">
                <a href="http://dr10.sdss3.org/irSpectrumDetail?plateid=<%=v.plate %>&mjd=<%=v.mjd %>&fiber=<%=v.fiberid %>" class="content" target="_blank">
                  <%= v.visit_id %>&nbsp;<img src="../../images/new_window_black.png" alt=" (new window)" />
                </a>
              </td>
              <td nowrap align="middle" class="<%=cellClass %>"><%=v.plate %></td>
              <td nowrap align="middle" class="<%=cellClass %>"><%=v.mjd %></td>
              <td nowrap align="middle" class="<%=cellClass %>"><%=v.fiberid %></td>
              <td nowrap align="middle" class="<%=cellClass %>"><%=v.dateobs.Substring(0,10) %></td>
              <td nowrap align="middle" class="<%=cellClass %>"><%=v.dateobs.Substring(11,12) %></td>
              <td nowrap align="middle" class="<%=cellClass %>"><%=v.vrel %></td>
            </tr>
            <% cellClass = (cellClass == "t") ? "b" : "t"; // Alternating row colors 
               } %>
          </table>                          
        </div>  <!-- end of visits div -->
      </div>  <!-- end of irspec div -->
    </div>  <!-- end of content div -->
  </form>
</body>
</html>

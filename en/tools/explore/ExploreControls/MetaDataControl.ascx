<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MetaDataControl.ascx.cs" Inherits="SkyServer.Tools.Explore.MetaDataControl" %>
<%@ Import Namespace="SkyServer" %>
<%if(master.objId != null && !master.objId.Equals("")) {%>
 <div id="metadata">
    <table class="content">
        <tr>
            <td colspan="2">
                <h1 id="othernames"><%= Functions.SDSSname(ra, dec)%></h1>
                <h2 id="sdssname">&nbsp;<input type="button" onclick="findOtherNames(ra, dec)" value="Look up common name" /></h2>
            </td>
        </tr>
        <tr>
            <td>
                <table width="620">
                    <tr>
                        <td align="center" class="h" colspan="2">Type</td>
                        <td align="center" class="h" colspan="2">SDSS Object ID</td>
                    </tr>
                    <tr>
                        <td class="t" align="center" colspan="2"><b><%=otype%></b></td>
                        <td class="t" align="center" colspan="2"><%=master.id%></td>
                    </tr>
                    <tr>
                        <td class="h" align="center" colspan="2">RA, Dec</td>
                        <td class="h" align="center" colspan="2">Galactic Coordinates (<i>l</i>, <i>b</i>)</td>
                    </tr>
                    <tr>
                        <td align="center" class="h">Decimal</td>
                        <td align="center" class="h">Sexagesimal</td>
                        <td align="center" class="h"><i>l</i></td>
                        <td align="center" class="h"><i>b</i></td>
                    </tr>
                    <tr>
                        <td align="center" class="t">
                            <script type="text/javascript" language="javascript">
                                var thera = new Number(<%=ra%>);
                                var thedec = new Number(<%=dec%>);
                                document.write(thera.toFixed(5) + ', ' + thedec.toFixed(5));
                            </script>
                        </td>
                        <td align="center" class="t">
                            <span class="large"><%= Functions.hmsC(ra) + ", " + Functions.dmsC(dec)%></span>
                        </td>
                        <td align="center" class="t">
                            <script type="text/javascript" language="javascript">
                                var L = new Number(<%=Utilities.ra2glon(ra, dec)%>);
                                document.write(L.toFixed(5));
                            </script>
                        </td>
                        <td align="center" class="t">
                            <script type="text/javascript" language="javascript">
                                var B = new Number(<%=Utilities.dec2glat(ra, dec)%>);
                                document.write(B.toFixed(5));
                            </script>
                        </td>
                    </tr>
                 </table>
              </td>
        </tr>
    </table>
       </div> <!-- end of metadata div -->

<%}else{%>
             <table cellpadding=2 cellspacing=2 border=0 width=625>
                  <tr><td class='nodatafound'>Object is out of SDSS footprint</td></tr>
            </table>
<%}%>
<%@ Page Language="C#"  MasterPageFile="ObjectExplorer.Master" AutoEventWireup="true" CodeBehind="Explorer.aspx.cs" Inherits="SkyServer.Tools.Explore.Explorer1" %>

<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="SkyServer" %>
<%@ Import Namespace="SkyServer.Tools.Explore"%>

<asp:Content ID="Head1" ContentPlaceHolderID="OEHead" runat="server">  
    <script type="text/javascript" src="./javascript/explore.js"></script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="OEContent" runat="server">
    
    <div class="content">
    <%       
        ImagingParameters im = new ImagingParameters();
        im.connectionString = globals.ConnectionString;
        im.getImagingQuery(id.ToString(), globals.SdssUrl);
        NameValueCollection nvImaging = im.runQueryDB();

        //reusable nv to use for small collections
        NameValueCollection nvTemp = new NameValueCollection();
        if (id.HasValue)
        {
    %>
    
    <div id="metadata">
    <table class="content">
        <tr>
            <td colspan="2">
                <h1 id="othernames"><%= Functions.SDSSname(im.ra, im.dec)%></h1>
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
                        <td class="t" align="center" colspan="2"><b><%=im.otype%></b></td>
                        <td class="t" align="center" colspan="2"><%=id%></td>
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
                                var thera = new Number(<%=im.ra%>);
                                var thedec = new Number(<%=im.dec%>);
                                document.write(thera.toFixed(5) + ', ' + thedec.toFixed(5));
                            </script>
                        </td>
                        <td align="center" class="t">
                            <span class="large"><%= Functions.hmsC(im.ra) + ", " + Functions.dmsC(im.dec)%></span>
                        </td>
                        <td align="center" class="t">
                            <script type="text/javascript" language="javascript">
                                var L = new Number(<%=Utilities.ra2glon(im.ra, im.dec)%>);                              
                                document.write(L.toFixed(5));
                            </script>
                        </td>
                        <td align="center" class="t">
                            <script type="text/javascript" language="javascript">
                                var B = new Number(<%=Utilities.dec2glat(im.ra, im.dec)%>);
                                document.write(B.toFixed(5));
                            </script>
                        </td>
                    </tr>
                 </table>
                </td>
        </tr>
    </table>
       </div> <!-- end of metadata div -->

        <div id="imaging">

        <h3>Imaging</h3>
            <% 
            // if clean=0, print a message that says the photometry may have issues
            if (im.clean == 0)
            { %>
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
                <%                       
                    nvTemp.Add("Flags", im.flag);
                    master.showVTable(nvTemp, 420);                                    
                %>
            </td>
        </tr>
    </table>

    <table>
        <tr>
            <td style="vertical-align:top">
                <% string link = "javascript:showNavi(" + im.ra + "," + im.dec + "," + 0.2 + ");"; %>
                <a href="<%= link%>"><img alt="" src="<%= globals.WSGetJpegUrl + "?ra=" + im.ra + "&dec=" + im.dec + "&scale=0.2&width=200&height=200&opt=G"%>" border="0" width="200" height="200" /></a>
            </td>
            <td >
                <%
                   nvTemp = new NameValueCollection();
                   nvTemp.Add("", "<b>Magnitudes</b>");
                   master.showHTable(nvTemp, 420);
                    
                   master.showHTable(im.magnitudes,420);

                   nvTemp = new NameValueCollection();
                   nvTemp.Add("", "<b>Magnitude uncertainties</b>");
                   master.showHTable(nvTemp, 420);

                   master.showHTable(im.magErrors, 420);                   
                %>       

          </td>
        </tr>
        <tr>
            <td colspan="2">
            <%
                   master.showHTable(im.restPhoto1, 625);
                   
                   master.showHTable(im.restPhoto2, 625);
               }
             %>                            
            </td>
        </tr>
    </table>
    </div>  <!-- end of imaging div  -->
    <h3 class="sectionlabel" id="crossidtop">Cross-identifications
        <a id="crossid_is_shown" href="javascript:showSection('crossid');javascript:showLink('crossid_is_hidden');javascript:hideLink('crossid_is_shown');" class="showinglink">
            Show
        </a>
        <a id="crossid_is_hidden" href="javascript:hideSection('crossid');javascript:showLink('crossid_is_shown');javascript:hideLink('crossid_is_hidden');" class="hidinglink">
            Hide
        </a>
  </h3>
       
  <div id="crossid">
    <table class="content">
        <tr>
            <td>     
                <% 
                    string wiselink = "ex_sql.aspx?cmd=select+*+from+wise_xmatch+x+join+wise_allsky+a+on+x.wise_cntr=a.cntr+where+x.sdss_objid=" + id + "&name=WISE_allsky&tab=V&id=" + id;
                    
                    CrossIdParameters cross = new CrossIdParameters(globals.ConnectionString, id.ToString(), wiselink);

                    String[] crossCatalogs = new String[] { "USNO", "FIRST", "ROSAT", "RC3", "2MASS", "WISE" };
                    
                    NameValueCollection nvc = null;
                    
                    for (int i = 0; i < crossCatalogs.Length; i++) {
                        nvc = cross.runQuery(crossCatalogs[i]);
                        master.showHTable(nvc, 620);
                    }                   
                 %>
            </td>
             </tr>
        </table>
   </div>  <!-- end of crossid div --> 

   <div id="spectro">

   <h3>Optical Spectra</h3>
       <% 
           long? specObjId = specId;
           SpectralParameters sp = new SpectralParameters(id, specId, globals.ConnectionString);
           NameValueCollection nvSpectral = sp.runDb();
        %>
    <div class="infobox">
        <table width="100%">
            <tr>
                <td width="60%">
                <% if (specId.HasValue && specId != 0){ 
                 %>
                      <p><b> SpecObjID<%= (!specObjId.HasValue || specObjId == 0) ? "not found" : ("= " + specObjId)%></b></p>
                <% 
                }else{ 
                %>
                      <p><b><%= (!specObjId.HasValue || specObjId == 0 ?
                                "No scienceprimary SpecObj linked to this PhotoObj<dd><font size=-1>(Click on \"All Spectra\" link if you think this object has a spectrum)</font>" :
                                "SpecObjID = " + specObjId)%> 
                      </b></p>                       
                <% } %>
                </td>
                <td width="40%">
                <%                    
                    //string spectrumlink = globals.DasUrl + "spectrumDetail?plateid=" + sp.plate + "&mjd=" + sp.mjd + "&fiber=" + sp.fiberid;
                    string spectrumlink = globals.DasUrl + "spectrumDetail?plateid=" + nvSpectral["plate"] + "&mjd=" + nvSpectral["mjd"] + "&fiber=" + nvSpectral["fiberid"];
                    Response.Write("<p><b>\n");
                    Response.Write("<a class='content' href='");
                    Response.Write(spectrumlink);
                    Response.Write("' target='_blank'>Interactive spectrum");
                    Response.Write("<img src='../../images/new_window_black.png' alt=' (new window)' />");
                    Response.Write("</a></b></p>\n");
                %>
                </td>
            </tr>
        </table>
   </div>
   <table class="content">
        <tr>
            <%
              if (specObjId.HasValue && specObjId != 0)
               {
                    string instrumentLink = globals.SdssUrlBase + "instruments";            
            %>
	   	      <td>             
		            <a href="../../get/SpecById.ashx?id=<%= specObjId %>">
		                <img alt="" src="../../get/SpecById.ashx?id=<%= specObjId %>" width="316" height="253" border="0" align="left" />
		            </a>
              </td>
              <td>                     
                    <%                      
                        master.showVTable(nvSpectral, 300);                     
                    %>            
	   	       </td>
            <%
                } 
            %>
        </tr>   
    </table> 
</div>  <!-- end of spectro div -->
</div> <!-- end of overall page div -->
</asp:Content>


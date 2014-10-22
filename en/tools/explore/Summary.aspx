<%@ Page Title="" Language="C#" MasterPageFile="ObjectExplorer.Master" AutoEventWireup="true" CodeBehind="Summary.aspx.cs" Inherits="SkyServer.Tools.Explore.Summary" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="SkyServer.Tools.Explore" %>
<%@ Import Namespace="SkyServer" %>

<asp:Content ID="Head1" ContentPlaceHolderID="OEHead" runat="server">

<!--<script src="../../../formatting.js" type="text/javascript" language="javascript"></script>-->

    <script type="text/javascript">

        function showChart(ra, dec) {
            var url = "../chart/chart.aspx?ra=" + ra + "&dec=" + dec;
            var w = window.open(url, "_top");
            w.focus();
        }

        function showNavi(ra, dec, scale) {
            var url = "../chart/navi.aspx?ra=" + ra + "&dec=" + dec + "&scale=" + scale;
            var w = window.open(url, "_blank");
            w.focus();
        }

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

        function morePrecise(short, long) {
            var theShortOne = document.getElementById(short);
            var theLongOne = document.getElementById(long);
            theShortOne.style.display = 'none';
            theLongOne.style.display = 'block';
        }

        function lessPrecise(short, long) {
            var theShortOne = document.getElementById(short);
            var theLongOne = document.getElementById(long);
            theShortOne.style.display = 'block';
            theLongOne.style.display = 'none';
        }

        function findOtherNames(thera,thedec) {
            var theothernames = document.getElementById("othernames");
            var sdssnamedisplay = document.getElementById("sdssname");
            $.ajax({
                type: "GET",
                url: "../Resolver.ashx?ra=" + thera + "&dec=" + thedec,
                success: function (response) {
                    if (response.indexOf("Error:") == 0) {
                        //alert(response);
                        sdssnamedisplay.innerHTML = "No common name found";
                    }
                    else {
                        var s = response.split('\n');
                        theothernames.innerHTML = s[0].substring(6);
                        sdssnamedisplay.innerHTML = "<%= Functions.SDSSname(ra ?? 0, dec ?? 0)%>";
                    }
                },
                error: function () {
                    alert("Error: Could not resolve coordinates.");
                }
            });
        }

    </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="OEContent" runat="server">
    <% string cmd; %>
    <!--<div class="content" style="display:none">-->
    <div class="content">
    <%
        if (id.HasValue)
        {
    %>
    
    <div id="metadata">
    <table class="content">
        <tr>
            <td colspan="2">
                <h1 id="othernames"><%= Functions.SDSSname(ra ?? 0, dec ?? 0)%></h1>
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
                               var thera = new Number(<%=ra %>);
                               var thedec = new Number(<%=dec %>);
                               document.write(thera.toFixed(5) + ', ' + thedec.toFixed(5));
                            </script>
                        </td>
                        <td align="center" class="t">
                            <span class="large"><%= Functions.hmsC(ra ?? 0) + ", " + Functions.dmsC(dec ?? 0)%></span>
                        </td>
                        <td align="center" class="t">
                            <script type="text/javascript" language="javascript">
                                var L = new Number(<%=L%>);
                                <%
                                //Response.Write("<h1><font color='red'>RA, dec = " + ra + " , " + dec + "</font></h1>");
                                //Response.Write("<h1><font color='blue'>L, B = " + L + " , " + B + "</font></h1>");
                                %>
                               document.write(L.toFixed(5));
                            </script>
                        </td>
                        <td align="center" class="t">
                            <script type="text/javascript" language="javascript">
                                var B = new Number(<%=B%>);
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
                if (clean == 0)
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
<!--        <tr>
            <td>
                <span class='t'><i>Move mouse over a column name to get its units</i>.</span>
            </td>
        </tr>-->
        <tr>
            <td>
                <%                       
            string flagsLink = globals.SdssUrl + "algorithms/photo_flags_recommend.php";

            using (SqlConnection oConn = new SqlConnection(globals.ConnectionString))
            {
                oConn.Open();

                cmd = "select dbo.fPhotoFlagsN(p.flags) as";
                cmd += " '<a href=" + flagsLink + ">Flags <img src=../../images/offsite_black.png /></a>'";
                cmd += " from PhotoTag p";
                cmd += " join field f on f.fieldID=p.fieldID";
                cmd += " where p.objID=" + id;
                master.showVTable(oConn, cmd, 620);                
                
                %>
            </td>
        </tr>
    </table>

    <table>
        <tr>
            <td style="vertical-align:top">
                <a href="<%= link %>"><img alt="" src="<%= globals.WSGetJpegUrl+"?ra="+ra+"&dec="+dec+"&scale=0.2&width=200&height=200&opt=G"%>" border="0" width="200" height="200" /></a>
            </td>
            <td>
                <%          

                cmd = " select '<b>Magnitudes</b>'";
                master.showHTable(oConn, cmd, 420, "PhotoObjAll");
                                          
                cmd = " select";
                cmd += " str(u,7,2) as u, str(g,7,2) as g,";
                cmd += " str(r,7,2) as r, str(i,7,2) as i,";
                cmd += " str(z,7,2)as z";
                cmd += " from PhotoObjAll with (nolock) where objID=" + id;
                master.showHTable(oConn, cmd, 420, "PhotoObjAll");

                cmd = " select '<b>Magnitude uncertainties</b>'";
                master.showHTable(oConn, cmd, 420, "PhotoObjAll");
                
                cmd = " select";
                cmd += " str(err_u,7,2) as err_u, str(err_g,7,2) as err_g, str(err_r,7,2) as err_r, str(err_i,7,2) as err_i,";
                cmd += " str(err_z,7,2) as err_z";
                cmd += " from PhotoObjAll with (nolock) where objID=" + id;
                master.showHTable(oConn, cmd, 420, "PhotoObjAll");
              
/*                cmd = " select run, rerun, camcol, field, obj, str(rowc,6,1) as rowc,";
                cmd += " str(colc,6,1) as colc ";
                cmd += " from PhotoObjAll with (nolock) where objID=" + id;
                master.showHTable(oConn, cmd, 420, "PhotoObjAll");
*/
            
            %>       

          </td>
        </tr>
        <tr>
            <td colspan="2">
            
            <%
                cmd = "select mjd as 'Image MJD', dbo.fPhotoModeN(mode) as mode, (nDetect-1) as 'Other observations',";
                cmd += " parentID, nChild, str(extinction_r,7,2) as extinction_r,";
                cmd += " str(petroRad_r,9,2)+' &plusmn; '+str(petroRadErr_r,10,3) as 'PetroRad_r (arcmin)'";
                cmd += " from photoObj";
                cmd += " where objID=" + id;

                master.showHTable(oConn, cmd, 625, "PhotoObj");    
                
                if (globals.ReleaseNumber >= 8)
                {
                    cmd = "select";
                    cmd += " (str(phz.z,7,3)+' &plusmn; '+str(phz.zerr,8,4))as 'photoZ (KD-tree method)',";
                    cmd += " (str(phzrf.z,7,3)+' &plusmn; '+str(phzrf.zerr,8,4)) as 'photoZ (RF method)',";                   
                    cmd += " case (1*zz.spiral+10*zz.elliptical+100*zz.uncertain)";
                    cmd += " when 1 then 'Spiral' when 10 then 'Elliptical' when 100 then 'Uncertain' else '-' end";
                    cmd += " as 'Galaxy Zoo 1 morphology'";
                    cmd += " from photoObj p";
                    cmd += " left outer join Photoz phz on p.objid=phz.objid";
                    cmd += " left outer join PhotozRF phzrf on p.objid=phzrf.objid";
                    cmd += " left outer join zooSpec zz on p.objid=zz.objid";
                    cmd += " where p.objid = " + id;

                    master.showHTable(oConn, cmd, 625, "PhotoObj");
                }
                else if (globals.ReleaseNumber >= 5)
                {
                    cmd = "select";
                    cmd += " (str(phz.z,7,3)+' &plusmn; '+str(phz.zerr,8,4))as 'photoZ (KD-tree method)', (str(phzrf.z,7,3)+' &plusmn; '+str(phzrf.zerr,8,4)) as 'photoZ (RF method)'";
                    cmd += " from Photoz phz";
                    cmd += " join PhotozRF phzrf on phz.objid=phzrf.objid";
                    cmd += " where phz.objid = " + id;

                    master.showHTable(oConn, cmd, 420, "PhotoObj");
                }
                else { } // if DR4 or earlier, skip this part
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
                        
                cmd = " select 'USNO' as Catalog, str(10*propermotion,6,2)+' &plusmn; '+str(sqrt(power(muraerr,2)+power(mudecerr,2)),8,3) as 'Proper motion (mas/yr)',";
                cmd += " str(angle,6,3) as 'PM angle (deg E)' from USNO where objId=" + id;
                master.showHTable(oConn, cmd, 620, "USNO");

                cmd = " select 'FIRST' as Catalog,";
                cmd += " str(peak,8,2)+' &plusmn; '+str(rms,8,2) as 'Peak flux (mJy)',";
                cmd += " major as 'Major axis (arcsec)', minor as 'Minor axis (arcsec)'";
                cmd += " from First where objId=" + id;
                master.showHTable(oConn, cmd, 620, "FIRST");
                
                cmd = " select 'ROSAT' as Catalog, cps, hr1, hr2, ext from ROSAT where objId=" + id;
                master.showHTable(oConn, cmd, 620, "ROSAT");
                
                cmd = " select 'RC3' as Catalog, hubble as 'Hubble type', str(m21,5,2)+' &plusmn; '+str(m21err,6,3) as '21 cm magnitude', ";
                cmd += " hi as 'Neutral Hydrogen Index'";//name1, name2, name3 from RC3 where objId=" + id;
                cmd += " from RC3 where objId=" + id;
                master.showHTable(oConn, cmd, 620, "RC3");              

                cmd = " select '2MASS' as Catalog, str(j,7,3) as 'J', h as 'H', k as 'K_s', phQual from TwoMASS where objId=" + id;
                master.showHTable(oConn, cmd, 620, "2MASS");

                string wiselink = "ex_sql.aspx?cmd=select+*+from+wise_xmatch+x+join+wise_allsky+a+on+x.wise_cntr=a.cntr+where+x.sdss_objid=" + id + "&name=WISE_allsky&tab=V&id=" + id;
                
                cmd = "select 'WISE' as Catalog,w.w1mag,w.w2mag,w.w3mag,w.w4mag,";
                cmd += "'<a href=''"+wiselink+"''>Link</a>' as 'Full WISE data'";
                cmd += " from WISE_xmatch x";
                cmd += " join WISE_allsky w on x.wise_cntr=w.cntr";
                cmd += " where x.sdss_objid=" + id;
                master.showHTable(oConn, cmd, 620, "WISE");
                     %>
                </td>
             </tr>
        </table>
    </div>  <!-- end of crossid div -->

  
  <div id="spectro">

   <h3>Optical Spectra</h3>

    <div class="infobox">
        <table width="100%">
            <tr>
                <td width="60%">
                <% 
                if (specId.HasValue && specId != 0)
                { 
                %>
                        <p><b> SpecObjID<%= (!specObjId.HasValue || specObjId == 0) ? "not found" : ("= " + specObjId)%> 
                        </b></p>
                <% 
                }
                else
                { 
                %>
                        <p><b><%= (!specObjId.HasValue || specObjId == 0 ?
                                "No scienceprimary SpecObj linked to this PhotoObj<dd><font size=-1>(Click on \"All Spectra\" link if you think this object has a spectrum)</font>" :
                                "SpecObjID = " + specObjId)
                                %> 
                        </b></p>                       
                <% } %>

                </td>

                <td width="40%">
                <%
                    
                if (specObjId.HasValue && specObjId != 0)
                {
                    cmd = " select plate,mjd,fiberid ";
                    cmd += " from SpecObjAll where specObjId=" + specObjId;
                    showSasLink(oConn, cmd);
                } 
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
            %>
	   	            <td>             
		            <a href="../../get/SpecById.ashx?id=<%= specObjId %>">
		            <img alt="" src="../../get/SpecById.ashx?id=<%= specObjId %>" width="316" height="253" border="0" align="left" /></a>
                    </td>

                    <td>
            <%
                
                    string instrumentLink = globals.SdssUrlBase + "instruments";

                    cmd = "select instrument as 'Spectrograph'"; //'<a href=''" + instrumentLink + "'' target=''_blank''>Spectrograph <img src=''../../images/info.gif'' alt='' (offsite)'' /></a>'";
                    cmd += " from specObjAll s where specObjID=" + specObjId;
                    master.showVTable(oConn, cmd, 300);
                      
                    cmd = "select class, str(z,7,3) as 'Redshift (z)', str(zerr,10,5) as 'Redshift error'";
                    cmd += " from specobjall";
                    cmd += " where specobjid = " + specObjId;
                    master.showHTable(oConn, cmd, 300, "SpecObjAll");

                    cmd = "select dbo.fSpecZWarningN(zWarning) as 'Redshift flags'";
                    cmd += " from specObjAll s where specObjID=" + specObjId;
                    master.showVTable(oConn, cmd, 300);

                    cmd = " select p.plate, p.mjd, s.fiberId";
                    cmd += " from SpecObjAll s, PlateX p where p.plateId=s.plateId and s.specObjId=" + specObjId;
                    master.showHTable(oConn, cmd, 300, "SpecObjAll");

                    cmd = "select s.survey, s.programname, s.scienceprimary as 'primary', (x.nspec-1) as 'Other spec'";
                    cmd += " from SpecObjAll s";
                    cmd += " join (select bestobjid, count(*) as nspec from specobjall where bestobjid=" + id + " group by bestobjid) x on s.bestobjid=x.bestobjid";
                    cmd += " where s.specObjId=" + specObjId;
                    master.showHTable(oConn, cmd, 300, "SpecObjAll");

                    if (String.Equals(survey, "segue1") | String.Equals(survey, "segue2")) cmd = "select subclass,";
                    else cmd = "select sourcetype,";
                    cmd += " str(velDisp,6,2) as 'Velocity dispersion (km/s)', str(velDispErr,7,3) as 'veldisp_error'";
                    cmd += " from SpecObjAll where specObjId=" + specObjId;
                    master.showHTable(oConn, cmd, 300, "SpecObjAll");

                    cmd = "select ''";
                    if (String.Equals(survey, "sdss")) cmd = "select (dbo.fPrimtargetN(legacy_target1)+' '+dbo.fPrimTargetN(legacy_target2)+' '+dbo.fSpecialTarget1N(special_target1)) as targeting_flags";
                    if (String.Equals(survey, "boss")) cmd = "select boss_target1, ancillary_target1, ancillary_target2";
                    if (String.Equals(survey, "segue1")) cmd = "select dbo.fSEGUE1target1N(segue1_target1) as segue1_targeting_flags1, dbo.fSEGUE1target2N(segue1_target2) as segue1_targeting_flags2";
                    if (String.Equals(survey, "segue2")) cmd = "select dbo.fSEGUE2target1N(segue2_target1) as segue2_targeting_flags1, dbo.fSEGUE2target2N(segue2_target2) as segue2_targeting_flags2";
                    if (String.Equals(survey, "apogee")) cmd = "select apogee_target1,apogee_target2";
                    cmd += " from specObjAll where specObjID=" + specObjId;

                    master.showHTable(oConn, cmd, 300, "SpecObjAll");
                           
            %>
	   	            </td>
                </tr>               
	   	    <%          
                
                  oConn.Close();
                }
            %>
 
    </table> 
</div>  <!-- end of spectro div -->




        <%            
            } // using SqlConnection
        }      // if id has value   
      else      // if id does not have value
      {
            // before we say we couldn't find this object, check whether there is an APOGEE identifier passed in.
            if (String.IsNullOrEmpty(apid)) {
                Response.Write("<h3>The object corresponding to the id specified does not exist in the database.<br> Please try another object.</h3>");
            }
       }%>




        <%
            // if there is an APOGEE object
            if (!String.IsNullOrEmpty(apid))
            {
                string specApogeeLink = globals.ApogeeSpectrumLink + "?apogee_id=" + apid;
                
                string doWeNeedC = "";
                if (commissioningFlag==1) doWeNeedC="C";

                string SpecThumbnailLink = globals.ApogeeFitsLink + apLocationID + "/plots/apStar" + doWeNeedC + "-s3-" + HttpUtility.UrlEncode(starName) + ".gif";
                string apogeeSpecThumbnail = "<a href='" + SpecThumbnailLink + "'><img src='" + SpecThumbnailLink + "' width='780' height='195' alt='APOGEE infrared spectrum of " + starName + "' /></a>";
                %>

        <div id="irspec">

            <h3>Infrared Spectra
                <span class="target">Targeted star: <%=starName%></span>
            </h3>

                <table width="800">
                    <tr>
                        <td class="h">Instrument</td>
                        <td class="t"><b>APOGEE</b></td>
                    </tr>
                    <tr>
                        <td class="h">APOGEE ID</td>
                        <td class="t"><%=apid %></td>
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
                        <td align="center" class="t">
                                <script type="text/javascript" language="javascript">
                                    var ra = new Number(<%=glon %>);
                                    document.write(ra.toFixed(5));
                                </script>
                        </td>                        
                        <td align="center" class="t">
                                <script type="text/javascript" language="javascript">
                                    var dec = new Number(<%=glat %>);
                                    document.write(dec.toFixed(5));
                                </script>
                        </td>                        

                        <td align="center" class="t">
                                <script type="text/javascript" language="javascript">
                                    var ara = new Number(<%=ApogeeRa %>);
                                    var adec = new Number(<%=ApogeeDec %>);
                                    document.write(ara.toFixed(5)+', '+adec.toFixed(5));
                                </script>
                        </td>
                        <td align="center" class="t"><span class="large"><%= Functions.hmsC(ApogeeRa ?? 0) + ", " + Functions.dmsC(ApogeeDec ?? 0)%></span></td>
                    </tr>
                    </table>
            
                    <table>
                        <tr>
                            <td colspan="2"><%=apogeeSpecThumbnail%></td>
                        </tr>
                            <% showSasLinkApogee(); %>
                    </table>
                    
                    <h3>Targeting Information</h3>
             
             <table>
                <tr>
                    <td>
                <%


                using (SqlConnection oConn = new SqlConnection(globals.ConnectionString))
                {
                    oConn.Open();

                    cmd = "select j as '2MASS j',h as '2MASS h',k as '2MASS k',j_err,h_err,k_err";
                    cmd += " from apogeeObject where apogee_id=\'" + starName + "\'";
                    master.showHTable(oConn, cmd, 800, "SpecObjAll");

                    cmd = "select case src_4_5 when 'none' then 'none' when 'WISE' then CONVERT(varchar,wise_4_5) when 'IRAC' then CONVERT(varchar,irac_4_5) end as '4.5 micron magnitude',";
                    cmd += " case src_4_5 when 'none' then '-' when 'WISE' then CONVERT(varchar,wise_4_5_err) when 'IRAC' then CONVERT(varchar,irac_4_5_err) end as '4.5 micron magnitude error',";
                    cmd += " src_4_5 as '4.5 micron magnitude source'";
                    cmd += " from apogeeObject where apogee_id=\'" + starName + "\'";
                    master.showHTable(oConn, cmd, 800, "SpecObjAll");

                    cmd = "select dbo.fApogeeTarget1N(apogee_target1) as 'APOGEE target flags 1'";
                    cmd += " from apogeeStar";
                    cmd += " where apstar_id=\'" + apid + "\'";
                    master.showVTable(oConn, cmd, 800);

                    cmd = "select dbo.fApogeeTarget2N(apogee_target2) as 'APOGEE target flags 2'";
                    cmd += " from apogeeStar";
                    cmd += " where apstar_id=\'" + apid + "\'";
                    master.showVTable(oConn, cmd, 800);                                   
                    
            %>       

                  </td>
            </tr>    
        </table>

                <%

                    Response.Write("<h3>Stellar Parameters</h3>");

                    cmd = "select a.vhelio_avg as 'Avg v<sub>helio</sub> (km/s)', a.vscatter as 'Scatter in v<sub>helio</sub> (km/s)', ";
                    cmd += " str(q.teff,4,0) as 'Best-fit temperature (K)', str(q.teff_err,6,1) as 'Temp error'";
                    cmd += " from apogeeStar a ";
                    cmd += " join aspcapStar q on a.apstar_id = q.apstar_id";
                    cmd += " where a.apstar_id=\'" + apid + "\'";
                    master.showHTable(oConn, cmd, 800, "SpecObjAll");

                    cmd = "select str(logg,4,2) as 'Surface Gravity log<sub>10</sub>(g)', str(logg_err,7,3) as 'log(g) error',";
                    cmd += " str(metals,6,1) as 'Metallicity [Fe/H]', str(metals_err,7,3) as 'Metal error', ";
                    cmd += " str(alphafe,6,2) as '[&alpha;/Fe]', str(alphafe_err,7,3) as '[&alpha;/Fe] error'";
                    cmd += " from aspcapStar where apstar_id=\'" + apid + "\'";
                    master.showHTable(oConn, cmd, 800, "SpecObjAll");
                    
                    cmd = "select dbo.fApogeeStarFlagN(starflag) as 'Star flags'";
                    cmd += " from apogeeStar where apstar_id=\'" + apid + "\'";
                    master.showVTable(oConn, cmd, 800);                              
                    
                    cmd = "select dbo.fApogeeAspcapFlagN(aspcapflag) as 'Processing flags (ASPCAP)'";
                    cmd += " from aspcapStar where apstar_id=\'" + apid + "\'";
                    master.showVTable(oConn, cmd, 800);          
                              

                    %>

                    <h3 class="sectionlabel">Visits (click to see visit spectrum)
                            <a id="visits_are_shown" href="javascript:showSection('visits');javascript:showLink('visits_are_hidden');javascript:hideLink('visits_are_shown');" class="showinglink">
                                Show
                            </a>
                            <a id="visits_are_hidden" href="javascript:hideSection('visits');javascript:showLink('visits_are_shown');javascript:hideLink('visits_are_hidden');" class="hidinglink">
                                Hide
                            </a>
                    </h3>

                    <div id="visits">
                    <%

                    //string visitlink = globals.DasUrl + "spectrumDetail?plateid=" + plate + "&mjd=" + mjd + "&fiber=" + fiber;
                 
                    cmd = "select visit_id, plate, mjd, fiberid,";
                    cmd += " substring(dateobs,1,10) as date, substring(dateobs,12,12) as 'time (UTC)', vrel";
                    cmd += " from apogeeVisit";
                    cmd += " where apogee_id=\'" + starName + "\'";
                    cmd += " order by dateobs";
                    showVisits(oConn, cmd, 800, "SpecObjAll");

                    oConn.Close();
            }
        }
    %>
                                
                    </div>  <!-- end of visits div -->
       </div>  <!-- end of irspec div -->
   </div> <!-- end of overall page div -->
</asp:Content>

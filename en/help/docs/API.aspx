<%@ Page Title="" Language="C#" MasterPageFile="../HelpMaster.master" AutoEventWireup="true" CodeBehind="API.aspx.cs" Inherits="SkyServer.Help.Docs.API" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HelpContent" runat="server">
<div id="title">API: Programmer's Interface</div>
<div id="transp">
<table width="600px" cellspacing="3" cellpadding="5">
	<tr><td>
	<p>Many of the tools on this site may be called programatically or may be included in your own
	website or tutorial pages. 
	A URL may be constructed for the ASP pages passing parameters in the query string.
	Examples are provided below.
	</p>
	<ul>
	<table>
	<tr><td class="b">Topic</td><td class="b">Description</td> </tr>
	<tr><td class="h"><a href="#getframe">Get Frame </a></td><td class="h"> Access frame pictures</td> </tr>
	<tr><td class="h"><a href="#getspec"> Get Spectra  </a></td><td class="h"> Access spectra graphs and data</td> </tr>
	<tr><td class="h"><a href="#getssky">Get Scrolling Sky Image  </a></td><td class="h">Animated images of the sky passing the telescope </td> </tr>
	<tr><td class="h"><a href="#spectrum">Full Spectral Access  </a></td><td class="h">Web Service access to details of spectra </td> </tr>
	<tr><td class="h"><a href="#search">Searching  </a></td><td class="h"> Radial,Rectangular,SQL database searches</td> </tr>
	<tr><td class="h"><a href="#cutout">Image Cutout  </a></td><td class="h">Built custom color images at given location</td> </tr>
	<tr><td class="h"><a href="#chart">Finding Chart </a></td><td class="h">Call Finding Chart at a particular location </td> </tr>
	<tr><td class="h"><a href="#navi">Navigate </a></td><td class="h">Pull up navigator with particular location </td> </tr>
	<tr><td class="h"><a href="#imglist">ImgList  </a></td><td class="h">Display thumbnails for uploaded object list</td> </tr>
	<tr><td class="h"><a href="#explore"> Explore </a></td><td class="h">Interactive exploration of object properties </td> </tr>
<!--
	<tr><td class="h"><a href="#density">Density Plots  </a></td><td class="h"> Reguest color coded density plots </td> </tr>
-->
	<tr><td class="h"><a href="#cross"> Cross-id </a></td><td class="h">Upload your own objects for cross-identification </td> </tr>
	</table>
	</ul>

	<h1 class='m'>Image access</h1>


	<a name="getframe"></a>
	<h3> Get Frame </h3>
	<pre><a target="api" class='sml' href="<%=url%>/get/frameByRCFZ.ashx?R=1140&C=1&F=120&Z=12"><%=url%>/get/frameByRCFZ.ashx?R=1140&C=1&F=120&Z=12 </a></pre>
	where R is the Run, C is the CamCol and Z is the Zoom level (Z can be either 12, 25, 50 or 0 (for 100%)), or 
	<pre><a target="api"  class='sml' href="<%=url%>/get/frameById.ashx?id=1237650369404207104&zoom=12"> <%=url%>/get/frameById.ashx?id=1237650369404207104&zoom=12 </a></pre>
	where id is the fieldId.
	<p>

	<hr>
	<a name="getspec"></a>
	<h3>Get Spectra </h3>
	<pre><a target="api" class='sml'  href="<%=url%>/get/specByPF.ashx?P=299489402567026688&F=25"><%=url%>/get/specByPF.ashx?P=299489402567026688&F=25 </a> </pre>
	where P is the Plate and F is the Fiber, or
	<pre><a target="api" class='sml'  href="<%=url%>/get/specById.ashx?ID=301889361622886400"><%=url%>/get/specById.ashx?ID=301889361622886400 </a></pre>
	where ID is the SpecObjId.
	<p>

	<hr>
	<a name="getssky"></a>
	<h3> Get Scrolling Sky image</h3>
	<pre><a target="api" class='sml'  href="<%=url%>/tools/scroll/camcol.aspx?R=1140&C=1&Z=25"><%=url%>/tools/scroll/camcol.aspx?R=1140&C=1&Z=25 </a></pre>
	Where R is the Run number , C is the CamCol and Z is the Zoom
	level.  Z can be either 12, 25, 50 or 0 (for 100%).
	
	<p>
	<hr>
	<a name="spectrum"></a>
	<h3>Full Spectral Access</h3>
	There is a comprehensive web site to access the details of spectra from the SDSS
	and other surveys (2dF). You can also upload your own spectra. <p>
	<a target="api"  class='sml' href="<%=globals.WSSpecHome%>"> <%=globals.WSSpecHome%></a>  <br>
	<p>
	This site also has a <a href="<%=globals.WSFilterHome%>">Filter Profile service</a> and a SOAP interface 
	to the spectrum service described in its <a target="api" href="<%=globals.WSSpecHome%>/docs/doc.aspx?p=00_index.htm">documentation page </a>.
	<p>


	<a name="search"></a>
	<h1 class='m'>Searching </h1>
	

	<h4>Radial</h4>
	<pre><a target="api"  class='sml' href="<%=url%>/tools/search/x_radial.aspx?ra=180&dec=-0.2&radius=3&format=csv&limit=20"><%=url%>/tools/search/x_radial.aspx?
ra=180&dec=-0.2&radius=3&format=csv&limit=20</a> </pre>
	where ra is Right Ascension in degrees, dec is Declination in degrees, radius is search radius in arcminutes,
	format is the format to return, which may be csv, html, or xml,
	limit is the number of rows to return. If you want all rows, put entries=all instead.
	<p>
	Additional parameters may be passed for restricting its range. For each band (x one of r,i,u,g,z)
	<i>check_x,min_x and max_x </i>may be specified. Hence adding to the above we may restrict the result to rows with 
	u between 20 and 22 as follows:
	<pre><a target="api"  class='sml' href="<%=url%>/tools/search/x_radial.aspx?ra=180&dec=-0.2&radius=3&format=xml&limit=20&check_u=u&min_u=20&max_u=22"><%=url%>/tools/search/x_radial.aspx?
ra=180&dec=-0.2&radius=3&format=xml&limit=20&check_u=u&min_u=20&max_u=22</a> </pre>
	<p>The same parameters can be also sent using POST. The names forms are the same as the GET parameter names.	<p>

	<hr>
	<h4>Rectangular</h4>
	<pre><a target="api" class='sml'  href="<%=url%>/tools/search/x_rect.aspx?min_ra=180.1&max_ra=180.2&min_dec=-0.1&max_dec=0.01&format=xml&limit=20"><%=url%>/tools/search/x_rect.aspx?
min_ra=180.1&max_ra=180.2&min_dec=-0.1&max_dec=0.01&format=xml&limit=20</a> </pre>
	where min_ra,min_dec in degrees form one corner, max_ra,max_dec form the other corner,
	format is the format to return which may be csv, html, or xml,
	limit is the number of rows to return. If you want all rows, put entries=all instead.
	<p>The same parameters can be also sent using POST. The names forms are the same as the GET parameter names.	<p>
	
	<hr>
	<h4>SQL</h4>
	<pre><a target="api"  class='sml' href="<%=url%>/tools/search/x_sql.aspx?format=xml&cmd=select top 10 * from galaxy"><%=url%>/tools/search/x_sql.aspx?
format=xml&cmd=select top 10 * from galaxy </a> </pre>
	Where cmd is the sql command to execute and format is the format to return which may be csv, html, or xml.
	The command should be preferably URL-encoded, like
	<ul>
	 <pre>&amp;cmd=SELECT+TOP+10+*+FROM+Galaxy+WHERE+r%3C17</pre> 
	</ul>
	<p>The same parameters can be also sent using POST. The POST parameter values can be in plain text, they do not
	need to be encoded. The names forms are the same as the GET parameter names.	<p>

<!--
	<hr>
	<a name="casaccess"></a>
	<h4>CasAccess </h4>
	CASAccess provides a <em>Web Service</em> interface for querying SDSS Data. Example clients and information may be found at 
	<br><a href="http://skyservice.pha.jhu.edu/develop/vo/casclient.aspx">http://skyservice.pha.jhu.edu/develop/vo/casclient.aspx</a>
	An example query through http GET would be like 
	<pre><a target="api" class='sml'  href="<=wsCasAccess%>/GetVoTable?sqlcmd=select+top+10+ra%2Cdec%2Cr%2Ci%2Cu%2Cg%2Cz+from+galaxy"><=wsCasAccess%>/GetVoTable?
sqlcmd=select+top+10+ra%2Cdec%2Cr%2Ci%2Cu%2Cg%2Cz+from+galaxy</a></pre>
-->

	<h1 class='m'>Visualization Tools</h1>

	<a name="cutout"></a>
	<h3>Image Cutout </h3>
	This is a <em>Web Service</em> that returns a JPEG image centered on <i>(ra,dec)</i>, of size <i>(height x width)</i>
	where the image is scaled to an arbitrary scale <i>(scale)</i>. 
	Various drawing options can be specified <i>(opt)</i>.
	The image cutout may be called using SOAP or through a HTTP interface, using either GET or POST.
	It can be also included in a page through the URL in an IMG tag.
	<p>
	For information on writing a Java SOAP client see <a href="http://skyservice.pha.jhu.edu/develop/vo/imgcutoutclient.aspx"> ImgCutOut DIME page</a>.
	<br>The <a target="api"  class='sml' href = "../../tools/chart/chart.aspx">ImgCutout page </a>itself contains information on building url's but here is an example also:
	<pre><a target="api"  class='sml' href="<%=globals.WSGetJpegUrl%>?ra=114.82991&dec=10.91817&width=512&height=512&opt=OG"><%=globals.WSGetJpegUrl%>
ra=224.5941&dec=-1.09&width=512&height=512&opt=OG </a></pre>
	where ra is Right Ascension in degrees, dec is declination in degrees,<br>
	scale is the scale of the image in arcsec per pixel, 0.4 is default<br> 
	opt is a string of charectors for overlays on the image codes are : 
	<ul>
	<table>
	<tr> <td class='b'> Code</td><td class='b'> Effect on image</td></tr>
	<%
		string[] p = new string[]{"G","L","P","S","T","O","B","F","M","Q","I"};
		string[] f = new string[]{"Grid","Label","PhotoObjs","SpecObjs",
			"TargetObjs","Outline","BoundingBox","Fields","Masks",
			"Plates","InvertImage"};
		for (int i=0; i<p.Length;i++) {
			Response.Write("<tr><td class='h'>"+p[i]+"</td>");
			Response.Write("<td class='h'>"+f[i]+"</td></tr>");
		}

	%> 
	</table>
	</ul>

	<hr>
	<a name="chart"></a>
	<h3>Finding Chart </h3>
	The page, built on top of the Cutout service, creates a Finding Chart, centered
	on the specified location at a specified scale. Properly formatted finding charts 
	can be sent to the printer. The printed image is always inverted for better looking
	prints.
	<p>You may jump directly to a particular location with the navigation tool using:
	<pre><a target="api"  class='sml' href="<%=url%>/tools/chart/chart.aspx?ra=114.82991&dec=10.91817&scale=0.3&opt=GO"><%=url%>/tools/chart/chart.aspx?ra=184.9511&dec=-0.8754&opt=GO</a></pre>
	where ra is Right Ascension in degrees, dec is Declination in degrees,<br>
	scale is the scale of the image in arcsec per pixel, 0.4 is default<br> 
	opt is a string of characters, as listed above for the cutout service.
	<p>
	
	<hr>
	<a name="navi"></a>
	<h3>Navigate</h3>
	This page, built from the Cutout service provides easy navigation in the vicinity 
	of a given point on the sky. One can move around by clicking on the frame of the image,
	or on individual objects. A short list of the basic properties and a magnified thumbnail \
	image of the object nearest to the selected point is then displayed. 
	<p>You may jump directly to a particular location with the navigation tool using:
	<pre><a target="api"  class='sml' href="<%=url%>/tools/chart/navi.aspx?ra=184.9511&dec=-0.8754&scale=0.3&opt=GO"><%=url%>/tools/chart/navi.aspx?ra=184.9511&dec=-0.8754&opt=GO</a></pre>
	where ra is Right Ascension in degrees, dec is declination in degrees,<br>
	scale is the scale of the image in arcsec per pixel, 0.4 is default<br> 
	opt is a string of characters, as seen above.
	<p>
	
	<hr>
	<a name="imglist"></a>
   	<h3>ImgList</h3>
   	This page, built from the Cutout service, can generate thumbnails of SDSS images based upon a
		user defined list of object positions. The images are linked to the
		other tools. The list can also be specified with a query. The page can be called via HTTP POST. <br>
	You will need a form such as the following :
	<pre cls="sml">&lt;form action="<%=url%>/tools/chart/showlist.aspx"
method="post">
&lt;TEXTAREA name="paste">
    ra     dec  
159.815 -0.655
161.051  0.152
161.739  0.893
164.090 -0.889
&lt;/TEXTAREA>
&lt;input type="submit">
&lt;/form>
</pre>
Basically you must post to the server a field called "paste" which contains the list of thumbnails you require.
	<p>
	
	
	<hr>
	<a name="explore"></a>
	<h3>Explore</h3>
			The Explore Tool contains a summary of an object, and interactive 
		links to further detailed properties, like its spectrum or neighbors, etc.

	<pre><a target="api"  class='sml' href="<%=url%>/tools/explore/obj.aspx?id=1237654386268439267"><%=url%>/tools/explore/obj.aspx?id=1237654386268439267</a></pre>
	where id is the objId, or
	<pre><a target="api" class='sml'  href="<%=url%>/tools/explore/obj.aspx?ra=180.6&dec=0.2"><%=url%>/tools/explore/obj.aspx?ra=180.6&dec=0.2</a></pre>
	where ra is Right Ascension in degrees, dec is declination in degrees, or
	<pre><a target="api" class='sml'  href="<%=url%>/tools/explore/obj.aspx?sid=299492151346096128"><%=url%>/tools/explore/obj.aspx?sid=299492151346096128</a></pre>
	where sid is the SpecObjId, or 
	<pre><a target="api" class='sml'  href="<%=url%>/tools/explore/obj.aspx?plate=266&fiberid=100"><%=url%>/tools/explore/obj.aspx?plate=266&fiberid=100</a></pre>
	where plate is the plate and fiber is the fiberid.

<!--
	<hr>
	<a name="density"></a>
	<h3> Density plots</h3>
	You may request colour coded density plots for queries using this SOAP webservice. See <br>
	<a target="api"  class='sml' href="<=Density%>"><=Density%></a> 
-->	

	<h1 class='m'>Cross-identification</h1>
	
	<a name="cross"></a>
	<p>You can search for photometric (image) and/or spectroscopic
	objects using the same cross-id form.  You may use POST or GET
	methods to this URL - the basic form is as follows.</p>

<h3>The GET method (example for default crossid page):	</h3>

	<pre><a target="api" class='sml' href="<%=url%>/tools/crossid/x_crossid.aspx?searchType=photo&photoScope=nearPrim&photoUpType=ra-dec&radius=0.5&firstcol=1&paste=++name++ra+++++++dec%0D%0A++A1+++195.2++++2.5%0D%0A++A2+++194.5++++2.6%0D%0A++A3+++193.6++++2.8%0D%0A++A4+++-90.1++++-90.051%0D%0A&upload=&uquery=SELECT+%0D%0A+++p.objID%2C+p.ra%2C+p.dec%2C+p.run%2C+p.rerun%2C+p.camcol%2C+p.field%2C%0D%0A+++dbo.fPhotoTypeN(p.type)+as+type%2C%0D%0A+++p.modelMag_u%2C+p.modelMag_g%2C+p.modelMag_r%2C+p.modelMag_i%2C+p.modelMag_z+%0D%0AFROM+%23x+x%2C+%23upload+u%2C+PhotoTag+p%0D%0AWHERE+u.up_id+%3D+x.up_id+and+x.objID%3Dp.objID+%0D%0AORDER+BY+x.up_id%0D%0A%09%09&format=html">http://sdssorgdev/dev/en/tools/crossid/x_crossid.aspx?searchType=photo&photoScope=nearPrim&photoUpType=ra-dec&radius=0.5&firstcol=1&paste=++name++ra+++++++dec%0D%0A++A1+++195.2++++2.5%0D%0A++A2+++194.5++++2.6%0D%0A++A3+++193.6++++2.8%0D%0A++A4+++-90.1++++-90.051%0D%0A&upload=&uquery=SELECT+%0D%0A+++p.objID%2C+p.ra%2C+p.dec%2C+p.run%2C+p.rerun%2C+p.camcol%2C+p.field%2C%0D%0A+++dbo.fPhotoTypeN(p.type)+as+type%2C%0D%0A+++p.modelMag_u%2C+p.modelMag_g%2C+p.modelMag_r%2C+p.modelMag_i%2C+p.modelMag_z+%0D%0AFROM+%23x+x%2C+%23upload+u%2C+PhotoTag+p%0D%0AWHERE+u.up_id+%3D+x.up_id+and+x.objID%3Dp.objID+%0D%0AORDER+BY+x.up_id%0D%0A%09%09&format=html</a>
	</pre>


<h3>The POST method:	</h3>

	<pre class="sml">
&lt;FORM METHOD="post" ENCTYPE="multipart/form-data" ACTION="<%=url%>/tools/crossid/x_crossid.aspx" id="crossid" name="crossid"&gt;
&lt;input id="searchType1" name="searchType" value="photo" type=radio class="box" CHECKED&gt;
&lt;input id="photoScope1" name="photoScope" value="nearPrim" type=radio class="box" CHECKED&gt;Nearest Primary Object&lt;/input&gt;
&lt;input id="photoScope2" name="photoScope" value="nearObj" type=radio class="box"&gt;Nearest Object&lt;/input&gt;
&lt;input id="photoScope3" name="photoScope" value="allPrim" type=radio class="box"&gt;All Nearby Primary Objects&lt;/input&gt;
&lt;input id="photoScope4" name="photoScope" value="allObj" type=radio class="box"&gt;All Nearby Objects&lt;/input&gt;
&lt;input id="photoUpType1" name="photoUpType" value="ra-dec" type=radio class="box"  CHECKED&gt;RA, dec&lt;/input&gt;
&lt;input id="photoUpType2" name="photoUpType" value="run-rerun" type=radio class="box"&gt;run-rerun-camcol-field-obj&lt;/input&gt;
&lt;input id="joinSpec" name="joinSpec" type="checkbox"&gt;Spectra&lt;/input&gt;
&lt;input id="searchType1" name="searchType" value="spectro" type="radio" class="box"&gt;
&lt;input id="spectroScope1" name="spectroScope" value="nearPrim" type=radio class="box" CHECKED DISABLED&gt;Nearest Primary Spectrum&lt;/input&gt;
&lt;input id="spectroScope2" name="spectroScope" value="nearObj" type=radio class="box" DISABLED&gt;Nearest Spectrum&lt;/input&gt;
&lt;input id="spectroScope3" name="spectroScope" value="allPrim" type=radio class="box" DISABLED&gt;All Nearby Primary Spectra&lt;/input&gt;
&lt;input id="spectroScope4" name="spectroScope" value="allObj" type=radio class="box" DISABLED&gt;All Nearby Spectra&lt;/input&gt;
&lt;input id="spectroUpType1" name="spectroUpType" value="ra-dec" type=radio class="box" DISABLED CHECKED&gt;RA, dec&lt;/input&gt;
&lt;input id="spectroUpType2" name="spectroUpType" value="plate-mjd-fiber" type=radio class="box" DISABLED&gt;plate-MJD-fiberID&lt;/input&gt;
&lt;input id="joinPhoto" name="joinPhoto" type="checkbox" DISABLED&gt;Images&lt;/input
&lt;input id=radius name=radius VALUE="0.5" SIZE="3"&gt;
&lt;input id=firstcol name=firstcol VALUE="1" SIZE="1"&gt;
&lt;TEXTAREA cols='40' name='paste' id='paste' rows='4' wrap='virtual' class='med'&gt;&lt;/TEXTAREA&gt;
&lt;INPUT TYPE="File" NAME="upload" size=32 &gt;
&lt;TEXTAREA cols='85' name='uquery' rows='6' wrap='virtual' class='med'&gt;&lt;/TEXTAREA&gt; 
&lt;input id=submit type=submit value=Submit class="button"&gt;
&lt;input name=format value="html" type=radio class="box" CHECKED&gt;
&lt;input name=format value="xml"  type=radio class="box"&gt;
&lt;input name=format value="csv"  type=radio class="box"&gt;
&lt;input TYPE="reset" VALUE="Reset " class="button" id="reset" name="reset"&gt;
</pre>

	</td></tr>
</table>
</div>
</asp:Content>

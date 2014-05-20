<%@ Page Title="" Language="C#" MasterPageFile="SearchHowtoMaster.master" AutoEventWireup="true" CodeBehind="Query3.aspx.cs" Inherits="SkyServer.Help.Howto.Query3" %>
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
          
      <h1>Querying from multiple tables: Joining</h1>
      <p>Take another look at the <a target="tools" href="../../browser/browser.aspx">
      Schema Browser</a>. Data on the same objects are often stored in different 
      tables. For example, a galaxy's magnitudes are stored in the photoObj 
      table, its redshift in the specObj table, and the plate used to observe 
      its spectrum in the plateX table.</p>
      <p>Quite often, you will want to compare information from many 
      different tables. For example, making a Hubble Diagram to show the expansion 
      of the universe requires some measure of brightness, such as magnitude, 
      and redshift. Magnitude is in the photoObj table; redshift is in the 
      specObj table. How do you search both tables?</p>
      <p>You can list more than one table in the from block, for example 
      "from photoObj, specObj." But be careful - you have to be sure you are looking at the 
      same object in both tables! To make sure you are searching the same 
      objects, you must use a <em>join</em>.</p>    
      <p>For example, look at the following query:</p>
      
      <table border="1" cellpadding="7" cellspacing="0" bgcolor="#552255">            
      <tr>
      <td>
      <pre>
select
    plateX.plate, plateX.mjd, 
    specObj.fiberID, 
    PhotoObj.modelMag_u, PhotoObj.modelMag_g, 
    PhotoObj.modelMag_r, PhotoObj.modelMag_i, 
    PhotoObj.modelMag_z,
    PhotoObj.ra, PhotoObj.dec,
    specObj.z, PhotoObj.ObjID
from
    photoObj, specObj, plateX
where
    specObj.bestObjid = PhotoObj.ObjID 
    and plateX.plateID = specObj.plateID 
    and class = 'qso' 
    and specObj.zWarning = 0 
    and specobj.z between 0.3 and 0.4
      </pre>
      
      </td>
      </tr>
      </table>
      <a name="queryend"></a>

      <p>To join two tables, look for common entries - when the same column 
      appears in both tables. If you match the same column in the 
      two different tables, then only records with the same data value in that 
      column will be returned. Since you are ensuring that records contain 
      the same data for this column, can be sure that you are looking at 
      data for the same object in both tables. In the example above, 
      what statement joins the photoObj and specObj tables?&nbsp;&nbsp;
      <a href="#queryend" onclick="javascript:popup('answers_join.aspx','sidebar',400,300)">
      Answer</a></p>

	  <p>To find columns to use in joining tables, look in the 
	  <a target="tools" href="../../browser/browser.aspx">Schema Browser</a>. 
	  Find places where the same column name appears in more than one table. 
	  You can use that column in the where block to join the tables.</p>
           
      <p>Notice that when you are querying multiple tables at once, you must specify the 
      table from which each piece of data is retrieved using the format 
      "table.column". For example, plateX.plateID retrieves the value in the 
      column plateID from the table plate. You must declare all tables you 
      are searching in the from block, separating them with commas.</p>
      
      <p>Naming tables in the select and where blocks is important because sometimes 
      columns in different tables have the same name, yet contain different data. 
      For example, the PhotoObj table contains the right ascension and declination 
      of the object in the "ra" and "dec" columns. The PlateX table also has ra 
      and dec columns, but these are the ra and dec of the telescope while it 
      is observing a single plate, not the ra and dec of an individual object 
      on the plate. So photoObj.ra and plate.ra mean totally different things!</p>

	  <p>To save typing, you can also declare "nicknames" for tables in the 
	  from block. By saying "from photoObj p, specObj s", you can use the 
	  nicknames p and s in the rest of your query - typing p.ra or s.zWarning. 
	  Whether you want to use nicknames or full names for tables is up to you.</p>

      <p>In addition to the join statements, the query above includes two 
      other commands in the where block. The command specObj.class = 'qso'
      tells the query to return quasars only, and the command 
      specObj.zWarning = 0 tells the query to return only objects for 
      which we have well-measured 
      redshifts. Again, you can add more restrictions to your query in terms 
      of colors, magnitudes, ra and dec or any number of parameters.</p>
      
      <h1>Try It!</h1>   
      
      <p>Click Submit below to run the query. If you would like, try rewriting the 
      query using one-letter nicknames for all the tables.</p>
      
      <p>Click Next to get some more practice using joins.</p>
      
	</td></tr>

<%  
	string query="select\n";
	query+="plateX.plate, plateX.mjd, specObj.fiberID, PhotoObj.modelMag_u, PhotoObj.modelMag_g, \n";
	query+="PhotoObj.modelMag_r, PhotoObj.modelMag_i, PhotoObj.modelMag_z,\n"; 
	query+="PhotoObj.ra, PhotoObj.dec,\n"; 
	query+="specObj.z, PhotoObj.ObjID\n";
	query+="from\n";
	query+="PhotoObj, specObj, plateX\n";
	query+="where\n";
	query+="specObj.bestObjid = PhotoObj.ObjID AND plateX.plateID =\n"; 
	query+="specObj.plateID AND specObj.class = 'qso' AND \n"; 
	query+="specObj.zWarning = 0 and specobj.z between 0.3 and 0.4\n";
	ResponseAux.sqlform(0,query,url,Response); 
%>

    <tr>
      <td><a href="practice2.aspx">
      <img align="left" src="../../../proj/images/previous.jpg"></a> <a href="practice3.aspx">
      <img align="right" src="../../../proj/images/next.jpg"></a></td>
    </tr>
  </table>
</div>
</asp:Content>

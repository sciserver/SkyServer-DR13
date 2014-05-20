<%@ Page Language="C#" %>
<html>
<head>
<link href="../../../skyserver.css" rel="stylesheet" type="text/css" />
<title>More Information - Object Upload</title>
</head>

<script type="text/javascript">
    function launch(link) {
        window.opener.location.href = link;
        window.opener.focus();
        window.close();
    }

    function done() {
        window.close();
    }

</script>

<body>

  <table WIDTH="400" border="0" cellspacing="3" cellpadding="3">
    <tr>
      <td>
      <h1>Object Upload</h1>

      <p><A href="../crossid/" target="tools">Launch the Object Upload tools</A></p>

	  <p>The Object Upload tools (cross-ID and Speclist) lets you search the 
	  SDSS database to find objects 
	  that match objects in your own list. If you have a list of stars or 
	  galaxies from another survey, you can use this tool to find the same 
	  objects in SDSS data. You can then use the Object Explorer to see what 
	  the SDSS has learned about your objects. Cross-ID finds SDSS data based 
	  on position, and Speclist finds SDSS photometric data based on SDSS 
	  plate/fiber number.</p>
	  
	  <p>The cross-ID tool works by searching the SDSS database for objects 
	  near the positions you specify in your list. You can enter your list 
	  in the large window, or you can upload the list as a text file. 
	  The list can have any of four formats, each of which is described on 
	  the Cross-ID page. You can have the tool return only the object 
	  nearest the position you specify (which will probably 
	  be the object you are interested in), or all objects within a 
	  specified radius. You can tell the tool what attributes to return for 
	  each SDSS object by selecting from the drop-down menu. You can have the 
	  tool return the results as an HTML table, as an XML (eXtended 
	  Markup Language) file, or as a CSV (Comma Separated Value) file.</p>

	  <p>Speclist works just like cross-ID, except that you should enter SDSS 
	  plate/MJD/fiber numbers instead of ra/dec. You can also enter a query in 
	  the query window that searches the SDSS data based based on your uploaded 
	  information. You can use the query window to specify what columns you 
	  want the Speclist tool to return.</p>

      <p><A href='javascript:done()'>Done</A></p>     
	</td>
    </tr>

  </table>
</body>
</html>
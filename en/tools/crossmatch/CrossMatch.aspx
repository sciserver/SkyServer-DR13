<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CrossMatch.aspx.cs" Inherits="SkyServer.en.tools.crossmatch.CrossMatch" %>
<!DOCTYPE html>
<html lang="en">
  <head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="icon" href="../../favicon.ico">

    <title>Cross Match Client</title>

    <!-- Bootstrap core CSS -->
    <link href="css/bootstrap.min.css" rel="stylesheet">
    <!-- Bootstrap theme -->
    <link href="css/bootstrap-theme.min.css" rel="stylesheet">    
      <script src="js/crossmatch.js"></script>
      <script src="http://code.jquery.com/jquery-1.11.2.min.js"></script>
      <script src="http://code.jquery.com/jquery-1.11.2.js"></script>
    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
      <!--Cross match specific-->
      <link href="css/crossmatch.css" rel="stylesheet">    

      <link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/highlight.js/8.4/styles/default.min.css">
       <script src="//cdnjs.cloudflare.com/ajax/libs/highlight.js/8.4/highlight.min.js"></script>

  </head>

  <body role="document" onload="init()">
      <script>hljs.initHighlightingOnLoad();</script>
      <%string skyqueryurl = ConfigurationManager.AppSettings["skyqueryUrl"]; %>
      <script type="text/javascript">var skyqueryUrl = "<%=skyqueryurl%>";</script>
      <%@ Register TagPrefix="login" TagName="loginParam" Src="..\..\Loginfloat.ascx"  %>
  <login:loginParam ID="loginParams" runat="server"/>
  
    <!-- Fixed navbar -->
    <!--style="background-image: url(images/new_titlebar.jpg)">-->
      <!--<div class="container" > 
        
          <a class="header-brand" href="#"></a> 
          <div class="row">
            <div class="col-lg-3 col-lg-offset-3">
                <button type="button" class="btn btn-lg btn-default"  id="Button1">Login</button>
            </div>
           </div>                  
    
      </div>-->
    
   <div class="container-fluid">
        <div class="row">
           <!-- <a class="header-brand" href="#"></a> -->
            <div class="col-lg-3 col-lg-offset-9">
           <%--<button type="button" class="btn btn-lg btn-default"  id="Button2">Login</button>--%>
            </div>
         </div>         
    </div>

    <div class="container theme-showcase" role="main">

      <!-- Main jumbotron for a primary marketing message or call to action -->
      <div class="tophead">

        <h3>Cross Match Client</h3>
        <p>
            This tool uses Skyquery web services to run cross match. More about SkyQuery is available 
            <a href="<%=skyqueryurl%>">here</a> 
        </p>
      </div>
      
      <div class="page-header">
        <h4>Browse tables and columns available for crossmatch</h4>
      </div>    

    <div class="row">
        <div class="col-sm-3"><label>DataSets</label>
          <div class="mylist-group" id="ListDataSets"></div>
        </div><!-- /.col-sm-4 -->
        <div class="col-sm-1">
            <span class="glyphicon glyphicon-arrow-right" ></span>
        </div>
        <div class="col-sm-3">Tables
          <div class="mylist-group" id="ListTables"></div>
        </div><!-- /.col-sm-4 -->
        <div class="col-sm-1">
            <span class="glyphicon glyphicon-arrow-right" aria-hidden="true"></span>
        </div>
        <div class="col-sm-4">Columns
          <div class="mylist-group" id="ListColumns"></div>
        </div><!-- /.col-sm-4 -->
    </div>

    <div class="page-header">
        <h4>Type your Query or Select From Samples</h4>
         <div class="dropdown">
          <label class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false">Sample Queries <span class="caret"></span></label>
            <ul class="dropdown-menu scrollable-menu" role="menu" id="samples" aria-labelledby="dropdownMenu">
                <li><a tabindex="0" href="#" id="simple1">Simple1</a></li>
                <li><a tabindex="1" href="#" id="simple2">Simple2</a></li>
                <li><a tabindex="2" href="#" id="crossmatch1">CrossMatch1</a></li>
                <li><a tabindex="3" href="#" id="crossmatch2">CrossMatch2</a></li>
            </ul>                          
       </div>                
    </div>
                            
       <div class="query-text-area">
        <pre> <%--<code class="SQL"> --%>
        <textarea class="form-control" id="query" rows="7">
SELECT s.objid, s.ra, s.dec, g.objid, g.ra, g.dec, x.ra, x.dec 
INTO twowayxmatch 
FROM XMATCH 
    (MUST EXIST IN SDSSDR7:PhotoObjAll AS s WITH(POINT(s.ra, s.dec), ERROR(0.1, 0.1, 0.1)), 
     MUST EXIST IN GALEX:PhotoObjAll AS g WITH(POINT(g.ra, g.dec), ERROR(0.2, 0.2, 0.2)), 
     LIMIT BAYESFACTOR TO 1e3) AS x 
WHERE s.ra BETWEEN 0 AND 5 AND s.dec BETWEEN 0 AND 5 AND g.ra BETWEEN 0 AND 5 AND g.dec BETWEEN 0 AND 5 </textarea>
            <%--</code>--%>
        </pre>
      </div>

      <p>  
       <button type="button" class="btn btn-lg btn-default" id="submit">Submit</button>
       <button type="button" class="btn btn-lg btn-primary" id="quick">Quick</button>
       <button type="button" class="btn btn-lg btn-info" id="info">Info</button>       
      </p>

      <div class="page-header">
        <h4>Jobs List</h4>  
        <span><input id="quickList" type="checkbox" aria-label="..."/> Quick</span>
        <button type="button" class="glyphicon glyphicon-refresh" id="refresh"></button>
          
      </div>
      <div class="row">
        <div class="col-md-7">
          <table class="table" id="jobslist">
            <thead>
              <tr>
                <th>canCancel</th>
                <th>dateCreated</th>
                <th>dateStarted</th>
                <th>dateFinished</th>    
                <th>guid</th>                
                <th>queue</th>                
                <th>status</th>                
                <th>download</th>
              </tr>
            </thead>
            <tbody class="jobList" id="bjobsList">
              
            </tbody>
          </table>
        </div>
        
      </div>


    

    
    <!-- Bootstrap core JavaScript
    ================================================== -->
    <!-- Placed at the end of the document so the pages load faster -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <!--<script src="../../assets/js/docs.min.js"></script>
    <!-- IE10 viewport hack for Surface/desktop Windows 8 bug -->
    <!--<script src="../../assets/js/ie10-viewport-bug-workaround.js"></script>-->
  </body>
</html>


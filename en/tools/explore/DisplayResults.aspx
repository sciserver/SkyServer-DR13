<%@ Page Title="" Language="C#" MasterPageFile="ObjectExplorer.Master" AutoEventWireup="true" CodeBehind="DisplayResults.aspx.cs" Inherits="SkyServer.Tools.Explore.DisplayResults" %>
<%@ Import Namespace="SkyServer" %>
<%@ Import Namespace="SkyServer.Tools.Explore" %>

<asp:Content ID="Head1" ContentPlaceHolderID="OEHead" runat="server">
    <script type="text/javascript" src="./javascript/explore.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="OEContent" runat="server">
   <div class="content">
   <div id="QueryResults">
    <p />
     <h2><a href="<%=url%>/help/browser/browser.aspx?cmd=description+<%=name%>+U" target="_top" class="content"><%=name%></a></h2>
    <p />   
    <%
        master.showVTable(ds, 300);   // put the option for Plate Objects   
    %>
    </div>
 </div>    
</asp:Content>
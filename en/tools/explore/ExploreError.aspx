<%@ Page Title="" Language="C#" MasterPageFile="ObjectExplorer.Master" AutoEventWireup="true" CodeBehind="ExploreError.aspx.cs" Inherits="SkyServer.Tools.Explore.ExploreError" %>
<!DOCTYPE html>

<asp:Content ID="Head1" ContentPlaceHolderID="OEHead" runat="server">
    <script type="text/javascript" src="./javascript/explore.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OEContent" runat="server">
 <div class="content">
     <table cellpadding=2 cellspacing=2 border=0 width=625>
        <tr><td class='nodatafound'>There is an error running ExploreTool for given Object.</td></tr>
         <tr><td class='nodatafound'>Error Details: <br/> <%=Session["error"] %></td></tr>
     </table>           
 </div>    
</asp:Content>

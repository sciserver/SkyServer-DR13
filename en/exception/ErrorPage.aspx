<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="../HomeMaster.master" CodeBehind ="ErrorPage.aspx.cs" Inherits="SkyServer.ErrorPage" %>
<%@ Import Namespace="SkyServer.Exceptions" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <div class="content">
     <h2>An error has occurred in Skyserver</h2>
    <p>
      Please inform <%=ConfigurationManager.AppSettings["helpdesk"].ToString()%>, or <br />
    </p>
<br> 
         <form method ="POST" target="_blank" name="bugreportform" action="<%=ConfigurationManager.AppSettings["BugReportURL"]%>" >
         <input type="hidden" name="bugreport" id="bugreport" value="<%=bugreportJson%>"/>
         <input id="submit" type="submit" value="Click to Report Error">
         </form>


 </div>    
</asp:Content>

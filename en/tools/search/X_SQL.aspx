<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="X_SQL.aspx.cs" Inherits="SkyServer.Tools.Search.X_SQL" %>
<%--<% ProcessRequest(); %>--%>
<%  
   ProcessRequestREST();
   SkyServer.Tools.Search.ResponseREST rs = new SkyServer.Tools.Search.ResponseREST();
   //rs.ProcessRequestREST(this.Request, this.Response, "SQL");
   %>

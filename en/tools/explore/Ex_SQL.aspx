<%@ Page Title="" Language="C#" MasterPageFile="ObjectExplorer.Master" AutoEventWireup="true" CodeBehind="Ex_SQL.aspx.cs" Inherits="SkyServer.Tools.Explore.Ex_SQL" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<asp:Content ID="Content2" ContentPlaceHolderID="OEContent" runat="server">
    <div class="content">
        <table class="content">
            <tr>
                <td>
                    <p />
                    <h2><a href="<%=url%>/help/browser/browser.aspx?cmd=description+<%=name%>+U" target="_top" class="content"><%=name%></a></h2>
                    <p />
                    <%
                        using (SqlConnection oConn = new SqlConnection(globals.ConnectionString))
                        {
                            oConn.Open();
                            if (tab == "V")
                                master.showVTable(oConn, cmd, 200);
                                //Response.Write(cmd);
                            else
                                master.showHTable(oConn, cmd, 600, name);
                        }
                    %>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

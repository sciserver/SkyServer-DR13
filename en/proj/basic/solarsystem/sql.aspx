<%@ Page Language="C#" AutoEventWireup="true" Inherits="SkyServer.Proj.Solarsystem.sqlsearch" MasterPageFile="~/en/HomeMaster.master" Title="SQL Test" CodeBehind="sql.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<form id="form1" runat="server">
<div id="transp">
<table WIDTH=600 border=0 cellspacing="3" cellpadding="3">
<tr><td>

<p>Enter a valid SQL query in this box:</p>


    <asp:TextBox ID="cmd" runat="server"></asp:TextBox>

    <p></p>
    <asp:Button OnClick="process_request" Text="Submit" runat="server" />
    <!--<input id=submit type=submit value=Submit onmouseover="return escape('Press this to submit the query to the server.');" onclick="process_request" />-->
    <p></p>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server">  
    </asp:SqlDataSource>

    <p>RESULTS:</p>
    <asp:GridView ID="GridView1" DataSourceID="SqlDataSource1" CellPadding="3" CellSpacing="3" gridlines="Both" runat="server">
    </asp:GridView>

</table>

<P></P>

</div>
</form>
</asp:Content>
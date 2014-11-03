<%@ Page Title="" Language="C#" MasterPageFile="ObjectExplorer.Master" AutoEventWireup="true" CodeBehind="Summary.aspx.cs" Inherits="SkyServer.Tools.Explore.Summary" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="SkyServer.Tools.Explore" %>
<%@ Import Namespace="SkyServer" %>


<asp:Content ID="Head1" ContentPlaceHolderID="OEHead" runat="server">
    <script type="text/javascript" src="./javascript/explore.js"></script>
</asp:Content>

<%--This is for Metadata --%>
<%@ Register TagPrefix="metadata" TagName="MetadataParameters" Src="./ExploreControls/MetaDataControl.ascx"  %>
<asp:Content ContentPlaceHolderID="OEContent0" runat="server">
    <metadata:MetadataParameters runat="server"/>
</asp:Content>

<%--This section for Imaging Parameters--%>
<%@ Register TagPrefix="imaging" TagName="ImagingParameters" Src="./ExploreControls/ImagingControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="OEContent1" runat="server">
    <div id="imaging"> <imaging:ImagingParameters runat="server" /></div>    
</asp:Content>

<%--This section for CrossId table Results--%>
<%@ Register TagPrefix="crossid" TagName="CrossId" Src="./ExploreControls/CrossIdControls.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="OEContent2" runat="server">
   <crossid:CrossId runat="server" />  
</asp:Content>

<%--This section for Spectral Parameters--%>

<%@ Register TagPrefix="spectral" TagName="SpectralControl" Src="./ExploreControls/SpectralControl.ascx" %>

<asp:Content ID="Content3" ContentPlaceHolderID="OEContent3" runat="server">
     <spectral:SpectralControl runat="server" />
</asp:Content>

<%--This Section for APOGEE parameters--%>

<%@ Register TagPrefix="apogee" TagName="ApogeeControl" Src="./ExploreControls/ApogeeControl.ascx" %>

<asp:Content ID="Content4" ContentPlaceHolderID="OEContent4" runat="server">
     <apogee:ApogeeControl runat="server" />
</asp:Content>


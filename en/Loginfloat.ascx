<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Loginfloat.ascx.cs" Inherits="SkyServer.en.Loginfloat" %>
<%
    string  imgpath = ResolveClientUrl("~/en/images/");
%>
 <script type="text/javascript">
     function keystone_login(logout) {
         if (logout) {
             window.location.href = "<%=ResolveClientUrl("~/en/HandleKeystone.aspx")%>";
         }
         else {
             //window.location.href = "<%= ConfigurationManager.AppSettings["Keystone.Login"]+Request.Url.AbsoluteUri%>";
             window.location.href = "<%= ConfigurationManager.AppSettings["Keystone.Login"]+ConfigurationManager.AppSettings["skyserverBase"]+Request.Url.AbsolutePath%>";
             //alert(window.location.href);
             }
         }
    </script>
  <div  style="height: 90px; width: 200px;   position:absolute;    right:0;    top:0;    background-color: rgba(0,33,66,0.5);	  border-radius:5px;">	
     <div>
		<img src="<%= imgpath %>sciserverlogo.png"  style="max-width: 95%;margin: 2%;" />
	</div>
	<div style="margin: 0;background-color:transparent;">

		<asp:Div style="float:left; padding:5px 15px;font-family:sans-serif;color:#eee" id="sc1s3rv3r_uname" >
            <%= Session["userid"] == null ? "Not logged in": Session["userid"] %>
		</asp:Div>
		<asp:Div style="float:right; background-color:#eee;margin-right:15px;border-radius:5px;border:solid 1px #444;padding:5px;"><asp:a style="font-weight:bold;font-family:sans-serif;color:#003366"  id="sc1s3rv3r_login" href="#" onclick="keystone_login(<%= Session["userid"] == null ? "false": "true" %>);return false;">
            <%= Session["userid"] == null ? "Login": "Logout" %>
	    </asp:a></asp:Div>
	</div>
</div>
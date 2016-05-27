<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MangaControl.ascx.cs" Inherits="SkyServer.Tools.Explore.MangaControl" %>
<%@ Import Namespace="SkyServer" %>
<%@ Import Namespace="SkyServer.Tools.Explore" %>
<%@ Import Namespace="System.Data" %>
<% if (HasData)   { %>




<div id="manga">
    
  
        <h3>MaNGA observation(s)</h3>




    <!-- data.sdss.org/sas/dr13/manga/spectro/redux/v1_5_4/7443/stack/images/12701.png -->


    <% for(int RowIndex = 0; RowIndex < dt.Rows.Count; RowIndex++){  %>

         <table width=625>
          <tr>

            <% if(RowIndex == 0){ %>
                  <td width="70" align="center"> 
                        <a target="_blank" href="<%=globals.MangaUrlBase + dt.Rows[RowIndex]["versdrp3"].ToString() + "/" + dt.Rows[RowIndex]["plate"].ToString() + "/stack/images/" + dt.Rows[RowIndex]["ifudsgn"].ToString() + ".png"%>"><img src="<%=globals.MangaUrlBase + dt.Rows[RowIndex]["versdrp3"].ToString() + "/" + dt.Rows[RowIndex]["plate"].ToString() + "/stack/images/" + dt.Rows[RowIndex]["ifudsgn"].ToString() + ".png"%>" width="70" height="70" alt="Manga Image" /></a> 
                  </td>
            <%} else { %>
                  <td width="70" align="center">  
                        <a id="ImageText<%=RowIndex%>" href="javascript:showLink('Image<%=RowIndex%>');hideLink('ImageText<%=RowIndex%>')" >
                              Show Image
                        </a>
                        <a target="_blank" id="Image<%=RowIndex%>" style="display: none" href="<%=globals.MangaUrlBase + dt.Rows[RowIndex]["versdrp3"].ToString() + "/" + dt.Rows[RowIndex]["plate"].ToString() + "/stack/images/" + dt.Rows[RowIndex]["ifudsgn"].ToString() + ".png"%>"><img src="<%=globals.MangaUrlBase + dt.Rows[RowIndex]["versdrp3"].ToString() + "/" + dt.Rows[RowIndex]["plate"].ToString() + "/stack/images/" + dt.Rows[RowIndex]["ifudsgn"].ToString() + ".png"%>" width="70" height="70" alt="Manga Image" /> </a>
                  </td>
            <%} %>

            <td>
                <table class="content" cellpadding=2 cellspacing=2 border=0 width=520 >   

                    <tr>  
                        <td align="center" class="h">plateIFU</td>
                        <td align="center" class="h">mangaid</td>
                        <td align="center" class="h">objra</td>
                        <td align="center" class="h">objdec</td>
                        <td align="center" class="h">ifura</td>
                        <td align="center" class="h">ifudec</td>

                    </tr>
                    <tr>
                        <td align="center" class="t"><%=dt.Rows[RowIndex]["plateIFU"].ToString() %></td>
                        <td align="center" class="t" ><%=dt.Rows[RowIndex]["mangaid"].ToString() %></td>
                        <td align="center" class="t" ><%=dt.Rows[RowIndex]["objra"].ToString() %></td>
                        <td align="center" class="t" ><%=dt.Rows[RowIndex]["objdec"].ToString() %></td>
                        <td align="center" class="t" ><%=dt.Rows[RowIndex]["ifura"].ToString() %></td>
                        <td align="center" class="t" ><%=dt.Rows[RowIndex]["ifudec"].ToString() %></td>

                    </tr>
                </table>
                <table class="content" cellpadding=2 cellspacing=2 border=0 width=520>   
                    <tr>  
                        <td align="center" class="h">drp3qual</td>
                        <td align="center" class="h">bluesn2</td>
                        <td align="center" class="h">redsn2</td>
                        <td align="center" class="h">mjdmax</td>
                        <td align="center" class="h">mngtarg1</td>
                        <td align="center" class="h">mngtarg2</td>
                        <td align="center" class="h">mngtarg3</td>
                    </tr>
                    <tr>
                        <td align="center" class="t" ><%=dt.Rows[RowIndex]["drp3qual"].ToString() %></td>
                        <td align="center" class="t" ><%=dt.Rows[RowIndex]["bluesn2"].ToString() %></td>
                        <td align="center" class="t" ><%=dt.Rows[RowIndex]["redsn2"].ToString() %></td>
                        <td align="center" class="t" ><%=dt.Rows[RowIndex]["mjdmax"].ToString() %></td>
                        <td align="center" class="t" ><%=dt.Rows[RowIndex]["mngtarg1"].ToString() %></td>
                        <td align="center" class="t" ><%=dt.Rows[RowIndex]["mngtarg2"].ToString() %></td>
                        <td align="center" class="t" ><%=dt.Rows[RowIndex]["mngtarg3"].ToString() %></td>
                    </tr>
                </table>
             </td>
          </tr>
             
        </table>
        <hr width="625" align="left">
    <%} %>



    <!--%  master.showHTable( ds, 400, "");   %-->

</div>  <!-- end of manga div -->
<%} %>
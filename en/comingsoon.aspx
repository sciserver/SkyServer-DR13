<%@ Page Title="" Language="C#" MasterPageFile="HomeMaster.master" AutoEventWireup="true" CodeBehind="comingsoon.aspx.cs" Inherits="SkyServer.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #logos  {position:absolute;top:0px;left:0px;visibility:visible}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="migrationMenu">
    <h2>SciServer Update</h2>
    <ul>

        <li><a href="#sciserver-login">SciServer Accounts</a></li>
        <li><a href="#scidrive">File-based data: SciDrive</a></li>
        <li><a href="#casjobs-scratch">Shared storage: MyScratch</a></li>
        <li><a href="#skyserver-integration">Integration with CasJobs</a></li>
        <li><a href="#skyquery">Cross-Matching: SkyQuery</a></li>
        <li><a href="#release">Release Date</a></li>
        <li><a href="#roadmap">Future Plans</a></li>
        <li><a href="#questions">Contact Us</a></li>
    </ul>
</div>
<div class="migrationNotice">
        <h1>SciServer Update Information <span class="new-label">Coming Soon!</span></h1>
    
        <div class="figure" style="float:right;">
            <img src="images/sciserver_icon_bkgd_blk_150px.png" alt="sciserver logo" class="imgnoborder" />
            <p class="figure-caption">The SciServer logo</p>
        </div>
        

        <p>Big changes are coming! SkyServer will soon become part of the new
            <a href="http://www.sciserver.org" target="_blank">SciServer</a> collaborative science framework. We 
            hope this new framework will open up even more possibilities for improving your science and 
            your teaching.</p>

        <p>SciServer will provide an integrated environment for working with big data. This new framework 
            will continue to support the Sloan Digital Sky Survey, while adding many new tools for managing 
            and exploring scientific data. These new tools include a drag-and-drop interface for data management, 
            and a new online environment to run analysis scripts in several languages with big data inputs 
            and outputs. SciServer will also offer access to large datasets in other science domains, starting 
            with ecology, fluid dynamics, and genomics, and with more to follow. With these new features, SciServer 
            will expand the SDSS's new approach to research and education to all of science.</p>
                        
        <p>As these powerful new tools come online, the core experience of SkyServer and CasJobs will not change. 
            You will still be able to run queries, explore results, upload tables, and share information as you 
            always have. SkyServer will continue to allow anonymous user freeform SQL queries without logging in. 
            All tools and datasets, both existing and new, will continue to be offered free of charge to everyone.</p>

        <p>This page describes the new features coming to SkyServer over the upcoming months. Scroll down or use 
            the list of links in the sidebar to explore this page. <a href="<%=globals.CasJobs%>/migrationinfo.aspx">A similar page on 
            CasJobs</a> describes the changes coming there.
        </p>
            
        <h2 id="sciserver-login">SciServer Accounts <span class="new-label">New Feature!</span></h2>

        <p>The first big change you will notice here is that a new login widget will soon appear in the top right 
            corner of each page. Clicking it will take you to the new SciServer login portal, where you can create 
            a free account for the entire SciServer system. You can use that account both here and on the powerful 
            <a href="<%=globals.CasJobs%>">CasJobs</a> site to see all your work on both sites.</p>
            
        <p>With your new SciServer login, you can save the results of your SkyServer queries and come back to them 
            later. You can also access your SkyServer queries in CasJobs, and vice versa, giving you a new set of 
            powerful tools for understanding science data.
        </p>

        <p>Creating a SciServer account will work slightly differently depending on whether or not you have an 
            existing CasJobs account. Both sets of instructions are below.</p>
            

        <h3 id="upgrade-casjobs">Migrating your existing CasJobs account</h3>

        <p>If you are an existing CasJobs user, i.e. you have an active account on the CasJobs site along with your own 
            database (MyDB), you will be able to migrate your CasJobs account to SciServer using the Migrate CasJobs 
            Account button on the SciServer Login Portal. Here is a screenshot of the Login Portal showing the options 
            available:</p>

        <div class="figure"><img src="images/migrateaccount.png" alt="Login to the SciServer Portal" />
            <p class="figure-caption">Migrate your CasJobs Account on the SciServer Single Sign-on Portal</p>
        </div>

        <p>Once you have created your new SciServer account &mdash; using the same username and password as your 
            original CasJobs account &mdash; your new old and new accounts will be automatically linked. If you have 
            forgotten you original CasJobs username and/or password, email the SDSS helpdesk to recover them.</p>
            
        <p>Once you have linked your accounts, your MyDB and all the data it contains will be available under the 
            new account. You should stop logging in through CasJobs, and log in only with the SciServer login portal. 
            We will soon drop support for the old CasJobs logins, so it is important to migrate your account into 
            the SciServer system soon.
        </p>            
            

        <h3 id="create-account">Creating New SciServer Account</h3>

        <p>If you do not currently have a CasJobs account (and a MyDB), you will need to create a new SciServer 
            single sign-on login account using the Register New Account button:</p>

        <div class="figure"><img src="images/registerportal.png" alt="Register on SciServer portal" />
            <p class="figure-caption">Register for a New SciServer Single Sign-On Account</p>
        </div>

            <p>Once you create an account and log in, you will be automatically redirected back to the site where 
                you initiated the login session (CASJobs, SkyServer, or the Login Portal homepage), which will 
                look the same as before except for the SciServer login widget at the top right. The Widget will 
                show your current login name as confirmation that you have logged in. There will also be a 
                "Logout" button there so that you may log out of the SciServer system.</p>

        <h3 id="login">Logging Into SkyServer</h3>

        <p>After you create your login, the login widget will continue to appear in the top right corner of 
            SkyServer. It will always show whether or not you are logged in. The images below show the widget 
            both ways. If you are not logged in, just click "Login" to go to the login portal and enter your 
            information. Once you are logged in, you will be redirected to SkyServer.</p>

        <div class="figure"><img src="images/notloggedin.png" alt="login widget with login button" />
            <p class="figure-caption">You are not logged in</p>
        </div>

        <div class="figure"><img src="images/loggedin.png" alt="login widget with username" />
                <p class="figure-caption">You are logged in</p>
        </div>
        <p>If you have an existing CasJobs account, you will need to migrate your account in to the new system 
            by following the instructions below. Once you do so, your entire MyDB personal database and jobs 
            history will be available in both CasJobs and SkyServer. We will eventually drop support for the old 
            CasJobs logins, so it is important to migrate your old account soon after creating a new one.</p>
    

            <p class="to-top"><a href="#">Back to Top</a></p>

            <h2 id="scidrive">SciDrive - a new tool for file-based data access <span class="new-label">New Feature!</span></h2> 
    
            <p>CasJobs offers storage and easy access to database tables, but not everything can be stored in a 
                database. For those times when your science requires flat file access, we are providing SciDrive. 
                SciDrive is a web application that will let you drag-and-drop to upload or download files, and will 
                automatically extract any metadata it can from those files. If the files contain tabular data, 
                the data will automatically be available in your CasJobs MyDB &mdash; and all the contents of your 
                MyDB will be available for easy download via SciDrive. SciDrive will make it easier than ever to 
                manage your research from beginning to end.</p>

        <div class="figure"><img src="images/scidrive.jpg" alt="SciDrive" />
            <p class="figure-caption">SciDrive offers drag-and-drop file management with automatic metadata extraction</p>
        </div>

            <p class="to-top"><a href="#">Back to Top</a></p>
            <h2 id="casjobs-scratch">CasJobs - Scratch space for Large query results <span class="new-label">New Feature!</span></h2>
    
            <p>Have you ever started a query, then come back later to find it failed because you had filled up all 
                the space in your MyDB? No more! The new CasJobs will include "MyScratch" space shared among all 
                users that will be large enough to hold results for nearly any query you can imagine. MyScratch 
                space is shared and temporary, but your results are guaranteed to stay for 7 days, during which 
                time you can further explore the data, and transfer any subsets you need into your MyDB.</p>

        <div class="figure"><img src="images/myscratch1.jpg" alt="CasJobs MyScratch" />
            <p class="figure-caption">MyScratch in CasJobs is shared temporary storage, allowing nearly any query to 
                return full results
            </p>
        </div>

            <p class="to-top"><a href="#">Back to Top</a></p>

  
            <h2 id="skyserver-integration">SkyServer - Better integration with CasJobs <span class="new-label">New Feature!</span></h2>

        <div class="figure" style="float:right;"><img src="images/mydb1.jpg" alt="CasJobs MyDB" />
            <p class="figure-caption">The CasJobs MyDB logo</p>
        </div>

    
            <p>CasJobs grew out of the SkyServer website, which offers quick and easy access to all SDSS data. 
                SkyServer does not allow long asynchronous queries like CasJobs does, but it includes many 
                additional features, such as a greatly expanded help section and educational projects you can use 
                with your students. It will be easier than ever to use both sites in tandem. In our upgraded 
                system, you will have the option of logging in to SkyServer as well as CasJobs. While you will not 
                be required to log in in order to use SkyServer, if you do then all queries you run on SkyServer 
                will show up in your CasJobs history, and all query results will be available in your MyDB.</p>

            <p class="to-top"><a href="#">Back to Top</a></p>

            <h2 id="skyquery">SkyQuery - Astronomy Cross-Matching <span class="new-label">New Feature!</span></h2>

            <p>SkyQuery is a system that has been developed over the past 5 years to provide a sophisticated and 
                scalable approach to running cross-match queries across distributed databases. The core capability is 
                centered around the SDSS datasets, but it also allows cross-matches between SDSS data and a user's own 
                datasets. SkyQuery is fully integrated with both CasJobs MyDBs and SciDrive.</p>

        <div class="figure"><img src="images/skyquery1.jpg" alt="SkyQuery" />
            <p class="figure-caption">SkyQuery enables easy cross-matching of objects in multiple surveys</p>
        </div>

            <p class="to-top"><a href="#">Back to Top</a></p>

            <h2 id="release">Release Date</h2>

            <p>We haven't yet picked a specific release date for SciServer, but it will be available later on 
                in Fall 2015. All these tools are provided free of charge, and always will be. We will also offer 
                programmatic interfaces to our new SciServer resources; contact us for more information. We will provide 
                regular updates as we have them, in particular to answer common questions in a FAQ page.</p>
                
            <p class="to-top"><a href="#">Back to Top</a></p>

            <h2 id="roadmap">Roadmap for Additional New Features</h2>

            <p>We are also hard at work on creating long-term enhancements and new capabilities for SciServer, to 
            be included in future releases. The list below describes two exciting new features coming in the future.</p>

            <h3 id="additional-datasets" style="clear:both;">Additional Scientific Datasets</h3>

            <p>We have been developing SciServer not just with Astronomy, but also with Turbulence and Cosmology researchers. 
                SciServer will also start to bring these large-scale simulation datasets from these fields online over the 
                next six months, and provide them with query and processing capabilities similar to those we 
                offer for SDSS data, but customized to meet the needs of those fields.</p>

            <h3 id="scripting">Scripting and Compute capability</h3>
    
            <p>SciServer tools offer many flexible ways to find scientific data, but most data processing must 
                still be done locally. Future relesaes of SciServer will offer the ability to post-process 
                large datasets so that intermediate steps occur online, with fast processing and enough storage 
                for all types of analysis. Final results of your post-processing will be stored in your MyDB and 
                SciDrive for easy access.</p>
                
            <p>This new capability builds on the MyScratch feature for large intermediate queries, and adds in a "compute" 
                capability that allows users to run Python or Matlab scripts to generate additional datasets that will 
                be automatically integrated with MyDB and SciDrive. We are very excited to bring this new feature online 
                in late 2015, as it will greatly extend the services that SciServer provides.</p>

            <p>The two key features of the new compute capability are that you can write your own programs to be 
                executed in a fast and safe online environment, and that these programs can work with datasets far 
                larger than previously possible.</p>

            <p>This feature has been demonstrated to the National Science Foundation using using iPython Notebooks 
                with SciServer's Turbulence and Cosmology Simulation datasets, and has also been used as part of a 
                data processing workshop. An example of the problems that will be enabled can be seen in a use 
                case from Turbulence Simulation: filtering a large dataset. The figure below shows a schematic 
                diagram of how the filtering can be done, along with the Python code to perform the filtering. 
                The code is shown in a SciScript notebook that can be executed online.
            </p>

    <div class="figure" style="float:left;">
            <img src="images/turbulence_usecase_3_300px.png" alt="diagram of use case with descriptive boxes and connecting lines" />
            <p class="figure-caption">A schematic of data filtering</p>
    </div>
    <div class="figure" style="float:right;">
            <img src="images/turbulence_usecase_3b_300px.png" alt="Python code shown in an online SciScipt notebook" />
            <p class="figure-caption">SciScript Python code to filter data</p>
    </div>

            <h2 id="questions" style="clear:both;">Questions?</h2>

            <p>We are excited to bring these new resources online over the coming months. We will add more 
                information about SciServer as well, including a frequently asked questions page. If you have questions, 
                please 
                <a href="<%=globals.ContactUrl+"&url="+Server.UrlEncode("http://"+Request.ServerVariables["SERVER_NAME"]+Request.ServerVariables["SCRIPT_NAME"]+Request.ServerVariables["QUERY_STRING"])%>">
                    Contact Us</a>!</p>


            <p class="to-top"><a href="#">Back to Top</a></p>
        </div>


</asp:Content>

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

        <li><a href="#sciserver-login">SciServer Login</a></li>
        <li><a href="#scidrive">SciDrive - a new tool for file-based data access</a></li>
        <li><a href="#casjobs-scratch">CasJobs - Scratch space for Large query results</a></li>
        <li><a href="#skyserver-integration">SkyServer Integration with CasJobs</a></li>
        <li><a href="#skyquery">SkyQuery - Astronomy Cross-Matching</a></li>
        <li><a href="#release">Release Date</a></li>
        <li><a href="#roadmap">Roadmap for Additional New Features</a></li>
    </ul>
</div>
<div class="migrationNotice">
        <h1>SciServer Update Information <span class="new-label">Coming Soon!</span></h1>

        <p>Thank you for using the Sloan Digital Sky Survey’s SkyServer and CasJobs systems. We strive to bring you 
                simple yet powerful tools to enable new research techniques and strategies for your science. We are 
                excited to announce the upcoming release of a new system called SciServer, which we hope will open up 
                broad new capabilities for your science. SciServer will provide a central integrated environment 
                for all the current Astronomy systems, as well as supporting additional scientific datasets in the 
                future.</p>

        <p>The core experience of CasJobs and SkyServer will not change &mdash; you will still be able to run queries, 
            explore results, upload tables, and share data as you always have. But SciServer will add a number of 
            exciting new features that we hope will allow you to do your science more easily than ever before.</p>
            
        <h2 id="sciserver-login">SciServer Login <span class="new-label">New Feature!</span></h2>

        <p>This will be the most prominent change that you will notice. Soon, a new Login widget will appear in the 
            top right corner of the SkyServer site. Clicking it will take you to the new SciServer login portal, 
            where you can create a free account for the entire SciServer system. You can use this account both on 
            SkyServer and on the much more powerful <a href="http://skyserver.sdss.org/casjobs/">CasJobs</a> site. 
            If you already have a CasJobs account, you can upgrade it to a SciServer account. After you finish 
            creating your account, you will be automatically returned to SkyServer.</p>

        <p>With your new SciServer login, you can save the results of your SkyServer queries and come back to them 
            later. You can also access your SkyServer queries in CasJobs, and vice versa, giving you a new set of 
            powerful tools for understanding SDSS data.
        </p>

        <p>After you create your login, the login widget will continue to appear in the top right corner of 
            SkyServer. It will always show whether or not you are logged in. The images below show the widget 
            both ways. If you are not logged in, just click "Login" to go to the login portal and enter your 
            information. Once you are logged in, you will be redirected to SkyServer.</p>

        <p>There will be a SciServer login widget in the top right-hand corner of the SkyServer and CasJobs websites 
            that will always show you your SciServer login status and allow you to log in and out. Here are screenshots 
            of the SciServer login widget showing logged in and out states:</p>

        <div class="figure"><img src="images/notloggedin.png" alt="login widget with login button" />
            <p class="figure-caption">User is not logged in.</p>
        </div>

        <div class="figure"><img src="images/loggedin.png" alt="login widget with username" />
                <div class="figure-caption">User is logged in.</div>
        </div>

        <h3 id="upgrade-casjobs">Upgrading your existing CasJobs account</h3>

        <p>For current users of the CasJobs site (http://skyserver.sdss.org/casjobs/), migrating your CasJobs account to 
            SciServer will be straightforward and easy. If you do not already have a CasJobs account, you can just 
            create a new SciServer account to access all the features of CasJobs. See the instructions below to learn 
            how to create a new SciServer account.</p>

        <p>If you are an existing CasJobs user, i.e. you have an active account on the CasJobs site along with your own 
            database (MyDB), you will be able to migrate your CasJobs account to SciServer using the Migrate CasJobs 
            Account button on the SciServer Login Portal. Here is a screenshot of the Login Portal showing the options 
            available.</p>

        <div class="figure"><img src="images/migrateaccount.png" alt="Login to the SciServer Portal" />
            <p class="figure-caption">Migrate your CasJobs Account on the SciServer Single Sign-on Portal</p>
        </div>

        <p>Once you have created your new SciServer account &mdash; using the same username and password as your 
            original CasJobs account &mdash; your new old and new accounts will be automatically linked. If you have 
            forgotten you original CasJobs username and/or password, email the helpdesk to recover them.</p>

        <p>Once you have linked your accounts, your MyDB and all the data it contains will be available under the 
            new account. You should stop logging in through CasJobs, and log in only with the SciServer login portal.</p>
    
        <h3 id="create-account">Creating New SciServer Account</h3>

        <p>If you do not currently have a CasJobs account (and a MyDB), you will need to create a new SciServer 
            single sign-on login account using the Register New Account button:</p>

        <div class="figure"><img src="images/registerportal.png" alt="Register on SciServer portal" />
            <p class="figure-caption">Register for a New SciServer Single Sign-On Account</p>
        </div>

            <p>Once you log in, you will be automatically redirected back to the site where you initiated the login 
                (CASJobs, SkyServer, or the Login Portal page itself), which will look the same as before except for 
                the SciServer login widget at the top right. The Widget will show your current login name as 
                confirmation that you have logged in. There will also be a "Logout" button there so that you may 
                log out of the SciServer system.</p>

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
            <p class="figure-caption">SciDrive</p>
        </div>

            <p class="to-top"><a href="#">Back to Top</a></p>
            <h2 id="casjobs-scratch">CasJobs - Scratch space for Large query results <span class="new-label">New Feature!</span></h2>
    
            <p>Have you ever started a query, then come back later to find it failed because you had filled up all 
                the space in your MyDB? No more! The new CasJobs will include "MyScratch" space shared among all 
                users that will be large enough to hold results for nearly any query you can imagine. MyScratch 
                space is shared and temporary, but your results are guaranteed to stay for 7 days, during which 
                time you can further explore the data, and transfer any subsets you need into your MyDB.</p>

        <div class="figure"><img src="images/myscratch1.jpg" alt="CasJobs MyScratch" />
            <p class="figure-caption">CasJobs MyScratch</p>
        </div>

            <p class="to-top"><a href="#">Back to Top</a></p>

            <h2 id="skyserver-integration">SkyServer - Better integration with CasJobs <span class="new-label">New Feature!</span></h2>
    
            <p>CasJobs grew out of the SkyServer website, which offers quick and easy access to all SDSS data. 
                SkyServer does not allow long asynchronous queries like CasJobs does, but it includes many 
                additional features, such as a greatly expanded help section and educational projects you can use 
                with your students. It will be easier than ever to use both sites in tandem. In our upgraded 
                system, you will have the option of logging in to SkyServer as well as CasJobs. While you will not 
                be required to log in in order to use SkyServer, if you do then all queries you run on SkyServer 
                will show up in your CasJobs history, and all query results will be available in your MyDB.</p>

        <div class="figure"><img src="images/mydb1.jpg" alt="CasJobs MyDB" />
            <p class="figure-caption">CasJobs MyDB</p>
        </div>

            <p class="to-top"><a href="#">Back to Top</a></p>

            <h2 id="skyquery">SkyQuery - Astronomy Cross-Matching <span class="new-label">New Feature!</span></h2>

            <p>SkyQuery is a system that has been developed over the past 5 years to provide a sophisticated and 
                scalable approach to running cross-match queries across distributed databases. The core capability is 
                centered around the SDSS datasets, but it also allows cross-matches between SDSS data and a user's own 
                datasets. SkyQuery is fully integrated with both CASJobs MyDBs and SciDrive!</p>

        <div class="figure"><img src="images/skyquery1.jpg" alt="SkyQuery" />
            <p class="figure-caption">SkyQuery</p>
        </div>

            <p class="to-top"><a href="#">Back to Top</a></p>

            <h2 id="release">Release Date</h2>

            <p>We haven't yet picked a specific release date for SciServer, but it will be available later on 
                in Fall 2015. All these tools are provided free of charge, and always will be. We will also offer 
                programmatic interfaces to our new SciServer resources; contact us for more information. We will provide 
                regular updates as we have them, in particular to clarify any questions people may have through an 
                FAQ page.</p>


            <p class="to-top"><a href="#">Back to Top</a></p>

            <h2 id="roadmap">Roadmap for Additional New Features</h2>

            <h3 id="scripting">Scripting and Compute capability</h3>
    
            <p>Along with the update of all the existing tools, and the 
                integrated release of SciDrive and SkyQuery, the SciServer team have also been busy developing a 
                new capability for post processing large datasets, that firstly builds on the MyScratch feature for 
                large intermediate queries, and adds in a "compute" capability that allows users to run Python or 
                Matlab scripts to generate additional datasets, and to integrate back to MyDB and SciDrive for the 
                final results. This feature has been demonstrated to the NSF using Turbulence and Cosmology 
                Simulation datasets, and has also been used as part of a workshop for teaching about data processing. 
                We are very excited to bring this online later in 2015 as it will greatly extend the services that 
                SciServer provides.</p>

            <h3 id="additional-datasets">Additional Scientific Datasets</h3>

            <p>We have been developing SciServer not just with Astronomy, but also with Turbulence and Cosmology groups. 
                SciServer will also start to bring these data sets online over the next six months, and provide them 
                with query and processing capabilities for very large data sets.</p>

            <p align="center">Please <a href="<%=globals.ContactUrl+"&url="+Server.UrlEncode("http://"+Request.ServerVariables["SERVER_NAME"]+Request.ServerVariables["SCRIPT_NAME"]+Request.ServerVariables["QUERY_STRING"])%>">Contact Us</a> for more information.</p>

            <p class="to-top"><a href="#">Back to Top</a></p>
        </div>


</asp:Content>

//var skyqueryUrl = 'http://scitest01.pha.jhu.edu/skyquery/';
var xAuth = "";
var selectedQueue = "long";
var sampleQueries = [
    "Select top 10 ra,dec from  SDSSDR7:Field",
    "Select top 10 ra,dec from  SDSSDR7:Field",
    "SELECT s.objid, s.ra, s.dec, g.objid, g.ra, g.dec, x.ra, x.dec INTO twowayxmatch FROM SDSSDR7:PhotoObjAll AS s WITH(POINT(s.ra, s.dec), ERROR(0.1, 0.1, 0.1))     CROSS JOIN GALEX:PhotoObjAll AS g WITH(POINT(g.ra, g.dec), ERROR(0.2, 0.2, 0.2)) XMATCH BAYESFACTOR x MUST EXIST s MUST EXIST g HAVING LIMIT 1e3     WHERE s.ra BETWEEN 0 AND 5 AND s.dec BETWEEN 0 AND 5  AND g.ra BETWEEN 0 AND 5 AND g.dec BETWEEN 0 AND 5",
    "SELECT s.objid, s.ra, s.dec, g.objid, g.ra, g.dec, x.ra, x.dec INTO twowayxmatch FROM SDSSDR7:PhotoObjAll AS s WITH(POINT(s.ra, s.dec), ERROR(0.1, 0.1, 0.1))     CROSS JOIN GALEX:PhotoObjAll AS g WITH(POINT(g.ra, g.dec), ERROR(0.2, 0.2, 0.2)) XMATCH BAYESFACTOR x MUST EXIST s MUST EXIST g HAVING LIMIT 1e3     WHERE s.ra BETWEEN 0 AND 5 AND s.dec BETWEEN 0 AND 5  AND g.ra BETWEEN 0 AND 5 AND g.dec BETWEEN 0 AND 5",
];


function init() {

    readCookie("Keystone");

    //callServices();
    
}


function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    //console.log(ca);
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) {
            var temp = c.substring(nameEQ.length, c.length);
            if (temp.indexOf("token") > -1)
                xAuth = temp.replace("token=", "");
            console.log(xAuth);
            return c.substring(nameEQ.length, c.length);
        }
    }
    return null;
}


function callServices() {


    var datasetsUrl = skyqueryUrl + "Api/V1/Schema.svc/datasets";
    var tablesUrl = "";

    //update database dropdown
    skyQueryConnect(datasetsUrl, xAuth, "GET", "datasets");    

    //update tables dropdown menu on selecting database
    $(document).on('click', '#ListDataSets a', function () {
        //console.log("Selected Option:" + $(this).text());
        tablesUrl = datasetsUrl + "/" + $(this).text() + "/tables";
        skyQueryConnect(tablesUrl, xAuth, "GET", "tablelist");
    });

    //update columns dropdown menu on table select
    $(document).on('click', '#ListTables a', function () {
        //console.log("Selected table Option:" + $(this).text());
        var columnsurl = tablesUrl + "/" + $(this).text() + "/columns";
        skyQueryConnect(columnsurl, xAuth, "GET", "columnlist");
    });
    callJobs('long');
    callSubmit();

    $('#quickList').change(function () {
        if ($(this).is(":checked")) {
            selectedQueue = "quick";
            callJobs('quick');
        } else {
            selectedQueue = "long";
            callJobs('long');
        }
        //$('#quickList').val($(this).is(':checked'));
    });

    /// select sample queries
    $(document).on('click', '#samples a', function () {
        var index = $(this).attr("tabindex");
        console.log("Selected sample Option:" + index);//$(this).find(":selected").index());
        $("textarea#query").val(sampleQueries[index]);
    });
}

function callJobs(whichqueue) {
    //load Jobs 
    var jobsurl = skyqueryUrl + "Api/V1/Jobs.svc/queues/" + whichqueue + "/jobs";
    console.log(jobsurl);
    skyQueryConnect(jobsurl, xAuth, "GET", "jobs");
}

function callSubmit() {
    
    
    //submitJob
    $(document).on('click', "#submit", function () {                
        submitJob("long");
    });

    //runquick query
    $(document).on('click', "#quick", function () {        
        submitJob("quick");
    });

    // to open more information about skyquery apis
    $(document).on('click', "#info", function () {
        window.location = skyqueryUrl+"/Api/Default.aspx";
    });

    // to open more information about skyquery apis
    $(document).on('click', "#refresh", function () {
        console.log("Test");
        callJobs(selectedQueue);
    });
    
}


function submitJob(whichqueue) {
   
    var jobsurl = skyqueryUrl + "Api/V1/Jobs.svc/queues/" + whichqueue + "/jobs";
    var obj = new Object();
    obj.query = $("#query").val();

    var queryObj = new Object();
    queryObj.queryJob = obj;

    var jsonString = JSON.stringify(queryObj);

    skyQueryConnect(jobsurl, xAuth, "POST",  whichqueue, jsonString);
}
// Update datasets 
function updateDropDown(response) {

    var temp = $.parseJSON(JSON.stringify(response));
    
    var mydbs = $('#ListDataSets');
    $('#ListDataSets').empty();
    $.each(temp, function (val, text) {
        $.each(text, function (val1, text1) {
    
            mydbs.append($("<a href=\"#\" class=\"list-group-item\">" + text1.name + "</a>").val(val1));
        });
    });
}

// update tables according to database selection
function updatetables(response) {
    //alert(JSON.stringify(response));
    var temp = $.parseJSON(JSON.stringify(response));
    
    var listTables = $('#ListTables');
    $('#ListTables').empty();
    $.each(temp, function (val, text) {
        $.each(text, function (val1, text1) {
    
            listTables.append($("<a href=\"#\" class=\"list-group-item\">" + text1.name + "</a>").val(val1));
        });
    });
}

function updateColumns(response) {
    //alert(JSON.stringify(response));
    var temp = $.parseJSON(JSON.stringify(response));    
    var listColumns = $('#ListColumns');
    $('#ListColumns').empty();
    $.each(temp, function (val, text) {
        $.each(text, function (val1, text1) {    
            listColumns.append($("<a href=\"#\" class=\"list-group-item\">" + text1.name + "</a>").val(val1));
        });
    });
}

/// This is to populate jobslist
function updateJobs(response) {
    //alert(JSON.stringify(response));
    var temp = $.parseJSON(JSON.stringify(response));
    var mySelect = $('#bjobsList');
    $('#bjobsList').empty();
    var tablerow = "";
    $.each(temp, function (val, text) {       
        $.each(text, function (val1, text1) {            
            tablerow = "<tr>";
            $.each(text1, function (val2, text2) {
                //$.each(text2, function (val3, text3) {                
                //    if (val3 != "query" && val3 != "name" && val3 != "comments" && val3 != "error") {
                //        var cls = "label label-default";
                //        if (val3 == "status") cls = getLabelCls(text3);
                //        tablerow += "<td><span class=\""+cls+"\">" + text3 + "</span></td>";
                //        //console.log(cls);//(val2 + "::" + val3 + "::" + text3);
                //    }                    
                //});
                var cls = "label label-default";
                tablerow += "<td><span class=\""+cls+"\">" + text2.canCancel + "</span></td>";
                tablerow += "<td><span class=\"" + cls + "\">" + text2.dateCreated + "</span></td>";
                tablerow += "<td><span class=\"" + cls + "\">" + text2.dateStarted + "</span></td>";
                tablerow += "<td><span class=\"" + cls + "\">" + text2.dateFinished + "</span></td>";
                tablerow += "<td><span class=\""+cls+"\">" +"<a tabindex=\"-1\" href=\"#\" onclick=\"updateJobDetails('" + text2.guid + "')\">" + text2.guid + "</a></span></td>";
                
                tablerow += "<td><span class=\"" + cls + "\">" + text2.queue + "</span></td>";
                tablerow += "<td><span class=\"" + getLabelCls(text2.status) + "\">" + text2.status + "</span></td>";
                var dbtb = "";
                //if (text2.output.indexOf(":") >= 0)
                if (typeof text2.output != 'undefined')
                     dbtb = text2.output.split(":");
                var link = skyqueryUrl + "Api/V1/Data.svc/" + dbtb[0] + "/" + dbtb[1];
                var dnlink = "<a href=" + link + "  download><span class=\"" + getLabelCls(text2.status) + "\"> " + text2.output + "</span></a>";
                tablerow += "<td>"+dnlink+"</td>";
            });
            tablerow += "</tr>";
            mySelect.append(tablerow);           
        });
    });
}
function getLabelCls(status) {
    var cls = "";
    switch(status){
        case "completed": cls = "label label-success"; break;
        case "failed": cls = "label label-danger"; break;
        case "canceled": cls = "label label-warning"; break;
        case "waiting": cls = "label label-primary"; break;
        case "executing": cls = "label label-default"; break;
    }
    return cls;
}

function updateJobDetails(id) {
    alert(id);
}

function skyQueryConnect(url, authToken, method, caller, senddata) {

    $.ajax({
        url: url,
        dataType: "json",
        data : senddata,
        headers: { "Content-Type": "application/json", "Accept": "application/json", "X-Auth-Token": authToken },
        type: method,
        success: function (response) {
            
            if (caller == "datasets") {               
                updateDropDown(response);
            }
            else if (caller == "tablelist") {
                updatetables(response);
            }
            else if (caller == "columnlist") {
                updateColumns(response);
            }
            else if (caller == "jobs") {
                updateJobs(response);
            }
            else if (caller == "long" || caller == "quick") {               
                selectedQueue = caller;

                if (caller == "quick") { $('#quickList').prop('checked', true); callJobs(caller); }
                else
                    callJobs(caller);
            }
        },
        error: function (data, errorThrown) {
            alert(errorThrown);
        }
    });
}

function testFunction() {

    $.ajax({
        url: "http://scitest02.pha.jhu.edu/skyquery/Api/V1/Schema.svc/datasets",        
         
        dataType: "json",
       
        headers: { "Content-Type": "application/json", "Accept": "application/json", "X-Auth-Token": "61d7e192cc2e45c3aaa10541607b2e3c" },
        type: 'GET',
       
        success: function (response) {
            alert(JSON.stringify(response));
            //console.log(response);
        },
        error: function (data, errorThrown) {
            alert(errorThrown);
        }
    });
}




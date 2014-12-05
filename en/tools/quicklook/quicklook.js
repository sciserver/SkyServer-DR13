function gotochart(ra, dec) {
    var s = "../chart/chart.aspx?ra=" + ra + "&dec=" + dec;
    var w = window.open(s, "NAVIGATE");
    w.focus();
}
function gotonavi(ra, dec) {
    var s = "../chart/navi.aspx?ra=" + ra + "&dec=" + dec;
    var w = window.open(s, "NAVIGATE");
    w.focus();
}

function setSDSS(id) {
    var s = "setSDSS.aspx?id=" + id;
    var w = window.open(s, "POPUP", "width=240,height=220");
    w.focus();
}
function setEq(ra, dec) {
    var s = "setEq.aspx?ra=" + ra + "&dec=" + dec;
    var w = window.open(s, "POPUP", "width=240,height=220");
    w.focus();
}
function setId(id) {
    var s = "setId.aspx?id=" + id;
    var w = window.open(s, "POPUP", "width=240,height=220");
    w.focus();
}
function setPlfib(sid) {
    var s = "setPlfib.aspx?sid=" + sid;
    var w = window.open(s, "POPUP", "width=240,height=220");
    w.focus();
}
function loadQuickLook(id) {

    parent.OMAIN.document.location = "quicklook.aspx?id=" + id;
}
function showNotes() {
    var s = "../chart/book.aspx";
    var w = window.open(s, "POPUP");
    w.focus();
}
function framePrint() {
    window.print();
    /*
    var pr = (window.print) ? 1 : 0;
    var da = (document.all) ? 1 : 0;
    var mac = (navigator.userAgent.indexOf("Mac") != -1);
    if (pr && da) {		// IE5
        parent.OMAIN.focus();
        window.print();
    } else if (pr) {	// NS4
        parent.OMAIN.print();
    } else {
        alert("Sorry, your browser does not support this feature");
    }
    */
}
function saveBook(id) {
    /*
    var url = "../chart/book.aspx?add=" + id;
    top.frames.HIDDEN.location.href = url;
    toggleElement('check', 1);
    */
    var url = "../chart/book.aspx?add=" + id;
    var frame = document.getElementById("test");
    frame.src = url;
}

function getid(id) {
    var link = "quickobj.aspx?id=" + id;
    return link;
}

function toggleElement(divID, state) // 1 visible, 0 hidden
{
    // I got this function from the Internet:
    // http://www.geocities.com/technofundo/tech/js/showhide.html
    // Jordan Raddick, 09/20/07, Baltimore
    //
    // "divID" is the div that needs to be visible or hidden. Should be passed in as a string.
    // "state" is whether it is visible or hidden: integer, 1 means visible, 0 means hidden
    //

    if (document.layers)	   //NN4+
    {
        document.layers[divID].visibility = state ? "show" : "hide";
    }
    else if (document.getElementById)	  //gecko(NN6) + IE 5+
    {
        var obj = document.getElementById(divID);
        obj.style.visibility = state ? "visible" : "hidden";
    }
    else if (document.all)	// IE 4
    {
        document.all[szDivID].style.visibility = state ? "visible" : "hidden";
    }
}
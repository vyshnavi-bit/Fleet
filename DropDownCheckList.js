/************************************************************************
 * DropDownCheckList.js
 * 
 *  Client-side javascript to support the ASP.NET DropDownCheckList 
 *  server control.
 *
 *  written by Mike Ellison, 20-September-2005
 *
 ************************************************************************
 *  Copyright (c) 2005 Mike Ellison.  All rights reserved.
 *  
 *  Redistribution and use in source and binary forms, with or without 
 *  modification, are permitted provided that the following conditions are met:
 *  
 *    * Redistributions of source code must retain the above copyright notice, 
 *      this list of conditions, the following disclaimer, and the following
 *      acknowledgements. 
 *  	
 *    * Redistributions in binary form must reproduce the above copyright notice, 
 *      this list of conditions and the following disclaimer in the documentation 
 *  	and/or other materials provided with the distribution.
 *  	
 *    * The name of the author may not be used to endorse or promote products 
 *      derived from this software without specific prior written permission. 
 *  
 *  THIS SOFTWARE IS PROVIDED BY THE AUTHOR "AS IS" AND ANY EXPRESS OR IMPLIED 
 *  WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF 
 *  MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
 *  IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT, 
 *  INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
 *  NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
 *  DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY 
 *  OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
 *  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, 
 *  EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 *  Acknowledgements:
 *      - The "shim" technique used to support Internet Explorer 6.x
 *        is courtesy Joe King of Coalesys, Inc., 
 *        http://dotnetjunkies.com/WebLog/jking/archive/2003/07/21/488.aspx
 *
 *      - Javascript functions findPosX and findPosY for locating objects absolutely
 *        are courtesy Peter-Paul Koch, http://www.quirksmode.org
 *  
 ************************************************************************/

/************************************************************************
 * Utility Functions (global to the page)
 ************************************************************************/
 var DDCL_DROPDOWNMODE_INLINE        = 0;
 var DDCL_DROPDOWNMODE_ONTOP         = 1;
 var DDCL_DROPDOWNMODE_ONTOPWITHSHIM = 2;
 var DDCL_DISPLAYTEXTLIST_LABELS     = 0;
 var DDCL_DISPLAYTEXTLIST_VALUES     = 1;

 var VehicleType;
 var Zones;
 var statuses;
 var Totalstring;
 var initializedata=false;


 ddcl_Objects = new Array();
 function DDCL_GetObject(id)
 {
   for (var i=0; i<ddcl_Objects.length; i++)
   {
     // alert(id.substring(ddcl_Objects[i].length) + ", " + ddcl_Objects[i].id);
     if (id.substring(0,ddcl_Objects[i].id.length) == ddcl_Objects[i].id) 
     {
        // alert('gotit');
        return ddcl_Objects[i];
     }
   }
   return null;
 }
 
 // -----------------------------------------------------
 function DDCL_findPosX(obj)
{
    /***
    Original script by Peter-Paul Koch, http://www.quirksmode.org
    ***/
	var curleft = 0;
	if (obj.offsetParent)
	{
		while (obj.offsetParent)
		{
			curleft += obj.offsetLeft
			obj = obj.offsetParent;
		}
	}
	else if (obj.x)
		curleft += obj.x;
	return curleft;
}

function DDCL_findPosY(obj)
{
    /***
    Original script by Peter-Paul Koch, http://www.quirksmode.org
    ***/
	var curtop = 0;
	if (obj.offsetParent)
	{
		while (obj.offsetParent)
		{
			curtop += obj.offsetTop
			obj = obj.offsetParent;
		}
	}
	else if (obj.y)
		curtop += obj.y;
	return curtop;
}
 // -----------------------------------------------------
 
 
 function DDCL_HandleCheckboxClickById(id)
 {
    var obj = DDCL_GetObject(id);
    obj.HandleCheckboxClick();
 }

 // called as the onclick event handler for a check box
 var labelall = false;
 var allcheched = false;
 var checkedlabel;
 function DDCL_HandleCheckboxClick(e) {
     if (!e) var e = window.event;
     if (e) {
         var checkedvalue = e.target.labels[0].innerHTML;
         checkedlabel = checkedvalue;
         if (checkedvalue == "All Vehicle Types" || checkedvalue == "All Plants" || checkedvalue == "All Vehicles") {
             if (e.target.checked) {
                 labelall = true;
                 allcheched = true;
             }
             else {
                 labelall = true;
                 allcheched = false;
             }
         }
         else {
             labelall = false;
             allcheched = false;
         }
         var elem = this;
         if (elem) {
             var obj = DDCL_GetObject(elem.id);
             if (obj) {
                 obj.HandleCheckboxClick();
                 obj.inCheckboxDiv = true;
             }
         }
     }
 }

 // called as the onclick event handler for a check box from outsite
 var firsttext;
 function DDCL_CheckboxClick(selectedtext) {
     firsttext = selectedtext;
     var checkedvalue = selectedtext;
     if (checkedvalue == "All Vehicle Types" || checkedvalue == "All Plants" || checkedvalue == "All Vehicles") {
         if (e.target.checked) {
             labelall = true;
             allcheched = true;
         }
         else {
             labelall = true;
             allcheched = false;
         }
     }
     else {
         labelall = false;
         allcheched = false;
     }
     var obj = DDCL_GetObject("ctl00_head_ContentPlaceHolder1_chblZones_0");
     if (obj) {
         obj.HandleCheckboxClick();
         obj.inCheckboxDiv = true;
     }
 }
 
 // called as the onclick event handler for a display box
 function DDCL_HandleDisplayBoxClick(e)
 {
    if (!e) var e = window.event;
    if (e) {
        var elem = this;
        if (elem) {
           var obj = DDCL_GetObject(elem.id);
           if (obj)
           {
              obj.HandleDisplayBoxClick(); 
              obj.inCheckboxDiv = true;
           }
        }
    }    
 }

 
 // called as the onclick event handler for a checkbox div
 function DDCL_HandleCheckboxDivClick(e) {
     if (!e) var e = window.event;
     if (e) {
         var obj = DDCL_GetObject(this.id);
         if (obj)
             obj.inCheckboxDiv = true;
     }
 }
  
 function DDCL_HandleDocumentClick(id)
 {   
   var obj = DDCL_GetObject(id);
   if (obj)
   {
    if (obj.inCheckboxDiv == true)
        obj.inCheckboxDiv = false;
    else
        obj.CloseCheckList(); 
   }
 }
 

/************************************************************************
 * Object definition
 ************************************************************************/
 // constructor; takes a required id as a parameter; this will be the
 // same as the ClientID of the ASP.NET control
 function DDCL_DropDownCheckList(id, textWhenNone, separator, truncateString, dropDownMode, allowExpand, displayList)
 {
     // remember the id, displayMode, and text to display when no
    // boxes are checked
    this.id = id;
    this.textWhenNone = textWhenNone;
    this.separator = separator;
    this.truncateString = truncateString;
    this.dropDownMode = dropDownMode;
    this.allowExpand = allowExpand;
    this.displayList = displayList;
    
    // get the elements representing the display box div and
    // checklist div; these will be referenced by object methods
    this.divCheckboxes  = document.getElementById(id + "_checkboxes");
    this.divBoundingBox = document.getElementById(id + "_boundingbox");
    this.divText        = document.getElementById(id + "_text");
    this.divDisplayBox  = document.getElementById(id + "_displaybox");
    this.img            = document.getElementById(id + "_img");
    this.shim           = document.getElementById(id + "_shim");
    
    this.boundingBoxWidth = this.divBoundingBox.offsetWidth;
    
    this.divCheckboxes.style.display = "none";
    
    // wire click events for the display box div and the checkboxes div
    this.divDisplayBox.onclick = DDCL_HandleDisplayBoxClick;
    this.divCheckboxes.onclick = DDCL_HandleCheckboxDivClick;
    
    // wire click events for the checkboxes
    var e = this.divCheckboxes.getElementsByTagName("input");
    for (var i=0; i<e.length; i++)
    {
      if (e[i].type == "checkbox")
      {
        e[i].onclick = DDCL_HandleCheckboxClick;
      }
    }
           
    // if the browser supports bubbling events, install a default click
    // handler for the document too, that will close the checkboxes div
    // if there is a click outside it
    if (document.attachEvent)
    {
        document.attachEvent('onclick'
                              , function() { eval("DDCL_HandleDocumentClick('" + id + "');") }
                             );
    }
    else if (document.addEventListener)
    {
        document.addEventListener('click'
                              , function() { eval("DDCL_HandleDocumentClick('" + id + "');") }
                              , false);  
    }    
        
    
    // initial display of checked items, or the textWhenNone string
    this.DisplayCheckedItems();
 
    // add this object to our reference array on the page so we can get it 
    // back later
    ddcl_Objects.push(this);

 }


 // get a label given a checkbox element
 function DDCL_DropDownCheckList_GetLabelForCheckbox(elem) {

     var e = this.divCheckboxes.getElementsByTagName("label");
     for (var i = 0; i < e.length; i++) {
         if (e[i].htmlFor == elem.id) {
             for (var j = 0; j < e[i].childNodes.length; j++) {
                 if (e[i].childNodes[j].nodeType == 3) //text type
                 {
                     return e[i].childNodes[j].nodeValue;
                 }
             }
         }
     }

     // still here?  no <label> for this checkbox then
     return null;
 }

 
 // refresh the checked items listing in the display box
 var allvehicles = false;
 function DDCL_DropDownCheckList_DisplayCheckedItems() {
     deletelocationOverlays();
     var sLabel = "";
     var sCurrent = "";
     var sFull = "";
     var sBefore = "";
     var sCompText = "";
     var bEllipsisAdded = false;

     // get all checkboxes in the checklist
     var e = this.divCheckboxes.getElementsByTagName("input");
     // clear the display text
     this.divText.innerHTML = "";
     // clear the title (tooltip) attribute
     this.divDisplayBox.title = "";

     if (labelall == true && allcheched == true) {
         for (var k = 0; k < e.length; k++) {
             e[k].checked = true;
         }
     }
     else if (labelall == true && allcheched == false) {
         for (var k = 0; k < e.length; k++) {
             e[k].checked = false;
         }
     }
     if (firsttext == "ALL PLANTS") {
//         for (var k = 0; k < e.length; k++) {
//             e[k].checked = true;
//         }
     }
     else {
         for (var k = 0; k < e.length; k++) {
             if (e[k].labels[0].innerHTML == firsttext) {
                 e[k].checked = true;
             }
         }
     }
     firsttext = "";
     if (checkedlabel == "Stopped Vehicles" || checkedlabel == "Running Vehicles" || checkedlabel == "Inside POI Vehicles" || checkedlabel == "Outside POI Vehicles" || checkedlabel == "MainPower Off Vehicles" || checkedlabel == "Delay in Update Vehicles") {
         for (var k = 0; k < e.length; k++) {
             if (e[k].labels[0].innerHTML == checkedlabel) {
             }
             else {
                 e[k].checked = false;
             }
         }
     }
     // loop through all checkboxes in the checklist to see 
     // which ones are checked;
     for (var i = 0; i < e.length; i++) {
         if (e[i].type == "checkbox" && e[i].checked) {
             // if the checkbox is checked, get its associated label text
             if (this.displayList == DDCL_DISPLAYTEXTLIST_LABELS)
             // get the label for the checkbox
                 sLabel = this.GetLabelForCheckbox(e[i]);
             else
             // get the value for the checkbox
                 sLabel = e[i].value;

             // add the list separator if necessary
             if (sCurrent != "") {
                 sCurrent += this.separator;
                 sFull += this.separator;
             }

             sFull += sLabel;
             sCurrent += sLabel;


             if (bEllipsisAdded == false) {
                 // add this one to the text box, then test for the
                 // width against the display box
                 if (this.boundingBoxWidth == 0) {
                     this.boundingBoxWidth = 110;
                 }
                 this.divText.innerHTML = "<nobr>" + sCurrent + "</nobr>";
                 if (this.divText.offsetWidth > this.boundingBoxWidth && this.allowExpand == false) {
                     // too big; shrink by what we can and add the ellipsis (or other trunacte string)
                     while (this.divText.offsetWidth > this.boundingBoxWidth && sCurrent.length > 0) {
                         sCurrent = sCurrent.substr(0, sCurrent.length - 1);
                         this.divText.innerHTML = "<nobr>" + sCurrent + this.truncateString + "</nobr>";
                     }

                     // and indicate the ellipsis (or other truncate text) has been added
                     bEllipsisAdded = true;
                 }
             }
         }
     }



     // finally, if there are no contents, display the textWhenNone message
     if (this.divText.innerHTML == "") {
         if (this.textWhenNone == "") {
             this.divText.innerHTML = "&nbsp;";
         }
         else {
             this.divText.innerHTML = this.textWhenNone;
         }
     }
     var selectedgroup = sFull;
     var Vehiclestring = new Array();
     Vehiclestring = this.id.split('_');
     if (Vehiclestring[3] == 'chblVehicleTypes') {
         VehicleType = selectedgroup;
     }
     if (Vehiclestring[3] == 'chblZones') {
         Zones = selectedgroup;
     }
     if (Vehiclestring[3] == 'chblvelstatus') {
         statuses = selectedgroup;
     }
     if (Vehiclestring[3] == 'chbllocations') {

     }
     if (VehicleType == "" || typeof VehicleType === "undefined") {
         VehicleType = "All Vehicle Types";
     }
     if (Zones == "" || typeof Zones === "undefined") {
         Zones = "All Plants";
     }
     if (statuses == "" || typeof statuses === "undefined") {
         statuses = "All Vehicles";
     }
     Totalstring = VehicleType + '#' + Zones+ '#' + statuses;
     filvehdiv(Totalstring);

     // if we added the ellipsis, set the title attribute to the full string
     // (which will display as a tooltip in most browsers)
     if (bEllipsisAdded)
         this.divDisplayBox.title = sFull;
     else
         this.divDisplayBox.title = "";
 }

 // handle a click in a participating checkbox
 function DDCL_DropDownCheckList_HandleCheckboxClick()
 {
    // refresh the checked items display box
    this.DisplayCheckedItems();
 }
 
 // handle a click in the display box
 function DDCL_DropDownCheckList_HandleDisplayBoxClick()
 {    
    // toggle the display of the checklist
    this.ToggleCheckList();
 }

 // toggle the display of the checklist boxes
 function DDCL_DropDownCheckList_ToggleCheckList()
 {
    if (this.divCheckboxes.style.display != "none")
        this.CloseCheckList();
    else
        this.OpenCheckList();                    
 }
 
 // explicitly close the checklist
 function DDCL_DropDownCheckList_CloseCheckList()
 {
    // hide the checkboxes div
    this.divCheckboxes.style.display = "none";
    
    // if we're using a shim, hide it as well
    if (this.dropDownMode == DDCL_DROPDOWNMODE_ONTOPWITHSHIM) {
        this.shim.style.width = "10px";
        this.shim.style.height = "10px";
        this.shim.style.display = "none";
    }
 }
 
 // explicitly open the checklist
 function DDCL_DropDownCheckList_OpenCheckList()
 {
    // open the checkboxlist; first, position it below the displaybox

    // determine the position based on the dropDownMode
    if (this.dropDownMode == DDCL_DROPDOWNMODE_INLINE)
    {
        // inline mode; we're already setup as we need to be
        this.divCheckboxes.style.display = "block";
    }
    else
    {
        // on top modes; position the box
        this.divCheckboxes.style.left = DDCL_findPosX(this.divDisplayBox);
        this.divCheckboxes.style.top  = DDCL_findPosY(this.divDisplayBox) + this.divDisplayBox.offsetHeight;
        
        this.divCheckboxes.style.display = "block";
       // if we want the shim, apply that now
        if (this.dropDownMode == DDCL_DROPDOWNMODE_ONTOPWITHSHIM)
        {
            this.shim.style.width   = this.divCheckboxes.offsetWidth;
            this.shim.style.height  = this.divCheckboxes.offsetHeight;
            this.shim.style.top     = this.divCheckboxes.style.top;
            this.shim.style.left    = this.divCheckboxes.style.left;
            this.shim.style.zIndex = this.divCheckboxes.style.zIndex - 1;
            this.shim.style.width = "10px";
            this.shim.style.height = "10px";
//            this.shim.style.zIndex = 99999;
            this.shim.style.display = "block";                
        }
    }
}

var vehiclesdata;
function filvehdiv(selectedgrp) {
    for (var i = checkedvehicles.length - 1; i >= 0; i--) {
        checkedvehicles.splice(i, 1);
    }

    if (VehicleType == "" || typeof VehicleType === "undefined") {
        VehicleType = "All Vehicle Types";
    }
    if (Zones == "" || typeof Zones === "undefined") {
        Zones = "All Plants";
    }
    if (statuses == "" || typeof statuses === "undefined") {
        statuses = "All Vehicles";
    }
    Totalstring = VehicleType + '#' + Zones + '#' + statuses;
//    if (Totalstring != "All Vehicle Types#All Plants") {
        initializedata = true;
        var data = { 'op': 'InitilizeGroups', 'filterstring': selectedgrp };
        var s = function (msg) {
            if (msg) {
                Groupsfilling(msg);
            }
            else {
            }
        };
        var e = function (x, h, e) {
        };
        callHandler(data, s, e);

        function Groupsfilling(groupsdata) {
            var vehiclenos = new Array();
            var insidevehiclenos = new Array();
          
            for (var vehicleid in groupsdata) {
                if (statuses.indexOf("All Vehicles") != -1) {
                    vehiclenos.push({ vehicleno: groupsdata[vehicleid].vehicleno, vehiclemodeltype: groupsdata[vehicleid].vehiclemodeltype, Routename: groupsdata[vehicleid].Routename });
                }
                else if (statuses == "Stopped Vehicles") {
                    for (var veh in livedata) {
                        if (groupsdata[vehicleid].vehicleno == livedata[veh].vehiclenum && livedata[veh].Speed == 0) {
                            vehiclenos.push({ vehicleno: groupsdata[vehicleid].vehicleno, vehiclemodeltype: groupsdata[vehicleid].vehiclemodeltype, Routename: groupsdata[vehicleid].Routename });
                        }
                    }
                }
                else if (statuses == "Running Vehicles") {
                    for (var veh in livedata) {
                        if (groupsdata[vehicleid].vehicleno == livedata[veh].vehiclenum && livedata[veh].Speed > 0) {
                            vehiclenos.push({ vehicleno: groupsdata[vehicleid].vehicleno, vehiclemodeltype: groupsdata[vehicleid].vehiclemodeltype, Routename: groupsdata[vehicleid].Routename });
                        }
                    }
                }
                else if (statuses == "Inside POI Vehicles") {
                    for (var veh in livedata) {
                        if (groupsdata[vehicleid].vehicleno == livedata[veh].vehiclenum) {
                            var Latitude = livedata[veh].latitude;
                            var Longitude = livedata[veh].longitude;
                            for (var cont = 0; cont < Locationsdata.length; cont++) {
                                var targetLoc = new google.maps.LatLng(Latitude, Longitude);
                                var center = new google.maps.LatLng(Locationsdata[cont].latitude, Locationsdata[cont].longitude);
                                var radius = Locationsdata[cont].radius;
                                var isinside = pointInCircle(targetLoc, radius, center);
                                function pointInCircle(point, radius, center) {
                                    return (google.maps.geometry.spherical.computeDistanceBetween(point, center) <= radius)
                                }
                                if (isinside) {
                                    vehiclenos.push({ vehicleno: groupsdata[vehicleid].vehicleno, vehiclemodeltype: groupsdata[vehicleid].vehiclemodeltype, Routename: groupsdata[vehicleid].Routename });
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (statuses == "Outside POI Vehicles") {
                    for (var veh in livedata) {
                        if (groupsdata[vehicleid].vehicleno == livedata[veh].vehiclenum) {
                            var Latitude = livedata[veh].latitude;
                            var Longitude = livedata[veh].longitude;
                            for (var cont = 0; cont < Locationsdata.length; cont++) {
                                var targetLoc = new google.maps.LatLng(Latitude, Longitude);
                                var center = new google.maps.LatLng(Locationsdata[cont].latitude, Locationsdata[cont].longitude);
                                var radius = Locationsdata[cont].radius;
                                var isinside = pointInCircle(targetLoc, radius, center);
                                function pointInCircle(point, radius, center) {
                                    return (google.maps.geometry.spherical.computeDistanceBetween(point, center) <= radius)
                                }
                                if (isinside) {
                                    insidevehiclenos.push({ vehicleno: groupsdata[vehicleid].vehicleno, vehiclemodeltype: groupsdata[vehicleid].vehiclemodeltype, Routename: groupsdata[vehicleid].Routename });
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (statuses == "MainPower Off Vehicles") {
                    for (var veh in livedata) {
                        if (groupsdata[vehicleid].vehicleno == livedata[veh].vehiclenum && livedata[veh].mainpower == "OFF") {
                            vehiclenos.push({ vehicleno: groupsdata[vehicleid].vehicleno, vehiclemodeltype: groupsdata[vehicleid].vehiclemodeltype, Routename: groupsdata[vehicleid].Routename });
                        }
                    }
                }
                else if (statuses == "Delay in Update Vehicles") {
                    for (var veh in livedata) {
                        var updatedata = livedata[veh].Datetime;
                        var date = updatedata.split(" ")[0];
                        var time = updatedata.split(" ")[1];
                        var datevalues = new Array();
                        var timevalues = new Array();
                        datevalues = date.split('/');
                        timevalues = time.split(':');
                        var updatetime = new Date(datevalues[2], datevalues[1] - 1, datevalues[0], timevalues[0], timevalues[1], timevalues[2]);
                        var todaydate = new Date();
                        var _MS_PER_DAY = 86400000;
                        var _MS_PER_aaa = 3600000;
                        var _MS_PER_sss = 60000;
                        var _MS_PER_ddd = 1000;
                        var days = Math.floor((todaydate - updatetime) / _MS_PER_DAY);
                        var hours = Math.floor((todaydate - updatetime) / _MS_PER_aaa);
                        if (hours > 24) {
                            hours = hours % 24;
                        }
                        var min = Math.floor((todaydate - updatetime) / _MS_PER_sss);
                        if (min > 60) {
                            min = min % 60;
                        }
                        var sec = Math.floor((todaydate - updatetime) / _MS_PER_ddd);
                        if (sec > 60) {
                            sec = sec % 60;
                        }
                        if (groupsdata[vehicleid].vehicleno == livedata[veh].vehiclenum && (min > 30 || hours >= 1 || days >= 1)) {
                            vehiclenos.push({ vehicleno: groupsdata[vehicleid].vehicleno, vehiclemodeltype: groupsdata[vehicleid].vehiclemodeltype, Routename: groupsdata[vehicleid].Routename });
                        }
                    }
                }
            }
            if (statuses == "Outside POI Vehicles") {
                for (var vehicleid in groupsdata) {
                    var existed = false;
                    for (var veh in insidevehiclenos) {
                        if (groupsdata[vehicleid].vehicleno == insidevehiclenos[veh].vehicleno) {
                            existed = true;
                            break;
                        }
                    }
                    if (!existed) {
                        existed = false;
                        vehiclenos.push({ vehicleno: groupsdata[vehicleid].vehicleno, vehiclemodeltype: groupsdata[vehicleid].vehiclemodeltype, Routename: groupsdata[vehicleid].Routename });
                    }
                }
            }
            $('#divassainedvehs').css('display', 'block');
            $.getScript("js/JTemplate.js", function (data, textStatus, jqxhr) {
                $('#divassainedvehs').setTemplateURL('reports1.htm');
                $('#divassainedvehs').processTemplate(vehiclenos);
//                document.getElementById('lblvehscount').innerHTML = vehiclenos.length;
            });
            //
            $('#divAllvehiclesassign').css('display', 'block');
            $.getScript("js/JTemplate.js", function (data, textStatus, jqxhr) {
                $('#divAllvehiclesassign').setTemplateURL('RouteAssignInfo.htm');
                $('#divAllvehiclesassign').processTemplate(vehiclenos);
                // liveupdate();
                document.getElementById('lblvehscount').innerHTML = vehiclenos.length;
            });
            //
            $('#divAllvehicles').css('display', 'block');
            $.getScript("js/JTemplate.js", function (data, textStatus, jqxhr) {
                $('#divAllvehicles').setTemplateURL('liveview1.htm');
                $('#divAllvehicles').processTemplate(vehiclenos);
               // liveupdate();
                document.getElementById('lblvehscount').innerHTML = vehiclenos.length;
            });
        }
//    }
//    else {
//        if (initializedata) {
//            if (vehiclesdata == 0 || typeof vehiclesdata === "undefined") {
//                var data = { 'op': 'InitilizeVehicles' };
//                var s = function (msg) {
//                    if (msg) {
//                        //                        vehiclessfilling(msg);
//                        alert("ddl");
//                    }
//                    else {
//                    }
//                };
//                var e = function (x, h, e) {
//                    // $('#BookingDetails').html(x);
//                };
//                callHandler(data, s, e);

//                function vehiclessfilling(data) {
//                    vehiclesdata = data;

//                    var vehiclenos = new Array();
//                    for (var vehicleid in vehiclesdata) {
//                        vehiclenos.push({ vehicleno: vehiclesdata[vehicleid].vehicleno, vehiclemodeltype: vehiclesdata[vehicleid].vehiclemodeltype, Routename: vehiclesdata[vehicleid].Routename });
//                        //                        vehiclenos.push({ vehicleno: vehiclesdata[vehicleid].vehicleno });
//                    }
//                    $('#divassainedvehs').css('display', 'block');
//                    $.getScript("js/JTemplate.js", function (data, textStatus, jqxhr) {
//                        $('#divassainedvehs').setTemplateURL('reports1.htm');
//                        $('#divassainedvehs').processTemplate(vehiclenos);
//                        document.getElementById('lblvehscount').innerHTML = vehiclenos.length;
//                    });
//                    $('#divAllvehicles').css('display', 'block');
//                    $.getScript("js/JTemplate.js", function (data, textStatus, jqxhr) {
//                        $('#divAllvehicles').setTemplateURL('liveview.htm');
//                        $('#divAllvehicles').processTemplate(vehiclenos);
//                        liveupdate();
//                        document.getElementById('lblvehscount').innerHTML = vehiclenos.length;
//                    });
//                }
//            }
//            else {
//                var vehiclenos = new Array();
//                for (var vehicleid in vehiclesdata) {
//                    vehiclenos.push({ vehicleno: vehiclesdata[vehicleid].vehicleno, vehiclemodeltype: vehiclesdata[vehicleid].vehiclemodeltype, Routename: vehiclesdata[vehicleid].Routename });
//                    //                    vehiclenos.push({ vehicleno: vehiclesdata[vehicleid].vehicleno });
//                }
//                $('#divassainedvehs').css('display', 'block');
//                $.getScript("js/JTemplate.js", function (data, textStatus, jqxhr) {
//                    $('#divassainedvehs').setTemplateURL('reports1.htm');
//                    $('#divassainedvehs').processTemplate(vehiclenos);
//                    document.getElementById('lblvehscount').innerHTML = vehiclenos.length;
//                });
//                $('#divAllvehicles').css('display', 'block');
//                $.getScript("js/JTemplate.js", function (data, textStatus, jqxhr) {
//                    $('#divAllvehicles').setTemplateURL('liveview.htm');
//                    $('#divAllvehicles').processTemplate(vehiclenos);
//                    liveupdate();
//                    document.getElementById('lblvehscount').innerHTML = vehiclenos.length;
//                });
//            }
//        }
//    }
}




//function intialize() {
//    var data = { 'op': 'InitilizeGroups' };
//    var s = function (msg) {
//        if (msg) {
//            Groupsfilling(msg);
//        }
//        else {
//        }
//    };
//    var e = function (x, h, e) {
//    };
//    callHandler(data, s, e);
//}

//var groupsdata;
//function Groupsfilling(data) {
//    groupsdata = data;
//}


     function callHandler(d, s, e) {
         $.ajax({
             url: 'Bus.axd',
             data: d,
             type: 'GET',
             dataType: "json",
             contentType: "application/json; charset=utf-8",
             async: true,
             cache: true,
             success: s,
             error: e
         });
     }
 
 // wire methods as prototypes
 DDCL_DropDownCheckList.prototype.DisplayCheckedItems     = DDCL_DropDownCheckList_DisplayCheckedItems;
 DDCL_DropDownCheckList.prototype.GetLabelForCheckbox     = DDCL_DropDownCheckList_GetLabelForCheckbox;
 DDCL_DropDownCheckList.prototype.HandleCheckboxClick     = DDCL_DropDownCheckList_HandleCheckboxClick;
 DDCL_DropDownCheckList.prototype.HandleDisplayBoxClick   = DDCL_DropDownCheckList_HandleDisplayBoxClick;
 DDCL_DropDownCheckList.prototype.ToggleCheckList         = DDCL_DropDownCheckList_ToggleCheckList;
 DDCL_DropDownCheckList.prototype.CloseCheckList          = DDCL_DropDownCheckList_CloseCheckList;
 DDCL_DropDownCheckList.prototype.OpenCheckList           = DDCL_DropDownCheckList_OpenCheckList;

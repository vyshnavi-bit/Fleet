<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="Trip_assign.aspx.cs" Inherits="Trip_assign" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/utility.js" type="text/javascript"></script>
    <link href="autocomplete/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            getallveh_nos();
            get_driverand_helper();
            getroutes();
            get_BillingOwnersList();
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1; //January is 0!
            var yyyy = today.getFullYear();
            if (dd < 10) {
                dd = '0' + dd
            }
            if (mm < 10) {
                mm = '0' + mm
            }
            var hrs = today.getHours();
            var mnts = today.getMinutes();
            $('#txt_tripstrtdate').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
            fill_tripsheet_no();
        });
        

        //GET Dairy owners 
        function get_BillingOwnersList() {
            var OwnerType = 'Dairy'
            var data = { 'op': 'get_BillingOwnersList', 'OwnerType': OwnerType };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fill_BillingOwnersList(msg);
                    }
                    else {
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function fill_BillingOwnersList(msg) {
            var data = document.getElementById('slct_BillingOwners_List');
            var length = data.options.length;
            document.getElementById('slct_BillingOwners_List').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select BillingOwners";
            opt.value = "Select BillingOwners";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].Sno != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].ID;
                    option.value = msg[i].Sno;
                    data.appendChild(option);
                }
            }
        }


        var AccountNameDetails = [];
        function getroutes() {
            var data = { 'op': 'getroutes' };
            var s = function (msg) {
                if (msg) {
                    AccountNameDetails = msg;
                    var AccountNameList = [];
                    for (var i = 0; i < msg.length; i++) {
                        var routename = msg[i].routename;
                        AccountNameList.push(routename);
                    }
                    $('#txt_routename').autocomplete({
                        source: AccountNameList,
                        change: RouteNamechange,
                        autoFocus: true
                    });
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }


        function RouteNamechange() {
            var routename = document.getElementById('txt_routename').value;
            for (var i = 0; i < AccountNameDetails.length; i++) {
                if (routename == AccountNameDetails[i].routename) {
                    document.getElementById('txt_route').value = AccountNameDetails[i].sno;
                }
            }
        }
        var veh_data = [];
        function getallveh_nos() {
            var data = { 'op': 'get_all_veh_master_data_notrips' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        veh_data = [];
                        fillvehmasterdata(msg);
//                        fillvehnodata(msg);
                        veh_data = msg;
                    }
                    else {
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillvehmasterdata(msg) {
            var data = document.getElementById('slct_vehicle_no');
            var length = data.options.length;
            document.getElementById('slct_vehicle_no').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Vehicle No";
            opt.value = "Select Vehicle No";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].registration_no != null) {
                    if (msg[i].VehType == "Puff" || msg[i].VehType == "Tanker" || msg[i].VehType == "Truck") {
                        var option = document.createElement('option');
                        option.innerHTML = msg[i].registration_no;
                        option.value = msg[i].vm_sno;
                        data.appendChild(option);
                    }
                }
            }
        }

        function get_driverand_helper() {
            var data = { 'op': 'get_driver_and_helper' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filldrive_helper(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        var employeedata = [];
        function filldrive_helper(msg) {
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].emp_type == "Driver") {
                    employeedata = msg;
                    var availableTags = [];
                    for (i = 0; i < msg.length; i++) {
                        availableTags.push(msg[i].employname);
                    }
                    $("#txt_driver").autocomplete({
                        source: function (req, responseFn) {
                            var re = $.ui.autocomplete.escapeRegex(req.term);
                            var matcher = new RegExp("^" + re, "i");
                            var a = $.grep(availableTags, function (item, index) {
                                return matcher.test(item);
                            });
                            responseFn(a);
                        },
                        change: driverchange,
                        autoFocus: true
                    });
                }
            }
        }
        function driverchange() {
            var employname = document.getElementById('txt_driver').value;
            for (var i = 0; i < employeedata.length; i++) {
                if (employname == employeedata[i].employname) {
                    document.getElementById('txt_driver_name').value = employeedata[i].emp_sno;
                }
            }
        }
        function btnstart_tripsheetclick() {
            var vehicle_no = document.getElementById('slct_vehicle_no').value;
            var driver = document.getElementById('txt_driver_name').value;
            var routename = document.getElementById('txt_routename').value;
            var route = document.getElementById('txt_route').value;
            var startreading = document.getElementById('txt_startreading').value;
            var hourreading = document.getElementById('txt_hourreading').value;
            var qty = document.getElementById('txt_qty').value;
            var BillingOwnerID = document.getElementById('slct_BillingOwners_List').value;

            if (vehicle_no == "" || vehicle_no == "Select Vehicle No") {
                alert("Please select vehicle");
                return;
            }
            if (route == "") {
                alert("Enter Route Name");
                return;
            }
            if (driver == "" || driver == "0") {
                alert("Please select driver");
                return;
            }
            if (startreading == "") {
                alert("Please enter vehicle start reading");
                return;
            }
            if (qty == "") {
                alert("Please enter qty");
                return;
            }

            if (BillingOwnerID == "Select Billing Owner") {
                alert("Please Select Billing Owner otherwise Defaultmode Zero...");
                BillingOwnerID = 0;
                return;
            }

            var data = { 'op': 'btnstart_tripsheetclick', 'vehicle_no': vehicle_no, 'driver': driver, 'RouteID': routename, 'VehicleStartReading': startreading, 'qty': qty, 'hourreading': hourreading, 'BillingOwnerID': BillingOwnerID };
            var s = function (msg) {
                if (msg) {
                    btnRefreshTripSheetClick();
                    alert(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function btnRefreshTripSheetClick() {
            document.getElementById('slct_vehicle_no').selectedIndex = 0;
            document.getElementById('txt_driver').value = "";
            document.getElementById('txt_routename').value = "";
            document.getElementById('txt_route').value = "";
            document.getElementById('txt_startreading').value = "";
            document.getElementById('txt_hourreading').value = "";
            document.getElementById('slct_BillingOwners_List').selectedIndex = 0;
        }

        ////
        //Trip End Form.........
        ////////
        function fill_tripsheet_no() {
            var data = { 'op': 'fill_tripsheet_no' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fill_trip_sheet_no(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fill_trip_sheet_no(msg) {
            assignedtrips = msg;
            var data2 = document.getElementById('slct_ass_trip_end');
            var length2 = data2.options.length;
            document.getElementById('slct_ass_trip_end').options.length = null;
            var opt2 = document.createElement('option');
            opt2.innerHTML = "Select Trip";
            opt2.value = "Select Trip";
            opt2.setAttribute("selected", "selected");
            opt2.setAttribute("disabled", "disabled");
            opt2.setAttribute("class", "dispalynone");
            data2.appendChild(opt2);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].sno != null) {
                    var option2 = document.createElement('option');
                    option2.innerHTML = msg[i].tripsheetno;
                    option2.value = msg[i].sno;
                    data2.appendChild(option2);
                }
            }
        }
        function end_tripsheet_change() {
            document.getElementById('lbl_startdate').innerHTML = "";
            document.getElementById('lbl_vehno').innerHTML = "";
            document.getElementById('lbl_route').innerHTML = "";
            document.getElementById('lbl_Driver').innerHTML = "";
            document.getElementById('lbl_startodometer').innerHTML = "0";
            document.getElementById('lbl_tripkms').innerHTML = "0";
            document.getElementById('txt_gpskms').value = "0";
            document.getElementById('txt_endodometerrdng').value = "0";
            document.getElementById('txt_endhourmtrrdng').value = "0";
            var TripDate = ""; 
            var tripsno = document.getElementById('slct_ass_trip_end').value;
            if (tripsno == "Select Trip") {
                return;
            }
            var data = { 'op': 'gettripalldetails', 'tripsno': tripsno, 'TripDate': TripDate };
            var s = function (msg) {
                if (msg) {
                    document.getElementById('lbl_startdate').innerHTML = msg.Tripdate;
                    document.getElementById('lbl_vehno').innerHTML = msg.Vehicleno;
                    document.getElementById('lbl_route').innerHTML = msg.RouteName;
                    document.getElementById('lbl_Driver').innerHTML = msg.Drivername;
                    document.getElementById('lbl_startodometer').innerHTML = msg.StrartReading;
                    var gpskms = msg.gpskms;
                    gpskms = parseFloat(gpskms).toFixed(2);
                    document.getElementById('txt_gpskms').value = gpskms;
                    var data = "<table style='margin: 10px;color: #777777 !important;font-weight: bold;font-family: sans-serif;'>";
                }
                else {
                }
            }
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function totalCalculation() {
            var srartodordng = document.getElementById('lbl_startodometer').innerHTML;
            var endodordng = document.getElementById('txt_endodometerrdng').value;
            var tripkms = 0;
            if (srartodordng != "" && endodordng != "") {
                srartodordng = parseFloat(srartodordng)
                endodordng = parseFloat(endodordng)
                tripkms = (endodordng - srartodordng).toFixed(2);
            }
            document.getElementById('lbl_tripkms').innerHTML = tripkms;
            tripkms = parseFloat(tripkms);
            var mileage = 0;
            var gpskms = document.getElementById('txt_gpskms').value;
            gpskms = parseFloat(gpskms);
        }
        function BtnSaveEndTripSheetClick() {
            var endodordng = document.getElementById('txt_endodometerrdng').value;
            var gpskms = document.getElementById('txt_gpskms').value;
            var TripDate = "";
            if (endodordng == "") {
                alert("Please Enter Odometer Reading");
                return;
            }
            var startodometer = document.getElementById('lbl_startodometer').innerHTML;
            if (endodordng <= startodometer) {
                alert("Please Enter Ending Odometer Greater than Starting Odometer");
                return false;
            }
            var fuelprice = "0"; //  document.getElementById('txt_fuelprice').value;
            //            if (fuelprice == "") {
            //                alert("Please Enter Diesel Rate");
            //                return;
            //            }
            var tripsno = document.getElementById('slct_ass_trip_end').value;
            var endhourmtrrdng = document.getElementById('txt_endhourmtrrdng').value;
            var data = { 'op': 'TripendSaveClick', 'fuelprice': fuelprice, 'TripDate': TripDate, 'endodordng': endodordng, 'gpskms': gpskms, 'tripsno': tripsno, 'endhourmtrrdng': endhourmtrrdng };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Trip ended successfully") {
                    }
                    alert(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <div style="width: 100%; height: 100%;">
        <div id="second_div" style="padding: 20px;">
            <div role="tabpanel">
                <ul class="nav nav-tabs" role="tablist">
                    <li role="presentation" class="active"><a href="#TripAssign" aria-controls="TripAssign"
                        role="tab" data-toggle="tab">Trip Assign</a></li>
                    <li role="presentation"><a href="#TripEnd" aria-controls="TripEnd" role="tab" data-toggle="tab">
                        Trip End</a></li>
                </ul>
                <div class="tab-content">
                    <div role="tabpanel" class="tab-pane active" id="TripAssign">
                        <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Trip Assign Details
                </h3>
            </div>
            <div class="box-body">
                <table align="center">
                    <tr>
                        <td>
                            <label>
                                Vehicle Number</label>
                            <select id="slct_vehicle_no" class="form-control" >
                                <option selected disabled value="Select Vehicle No">Select Vehicle No</option>
                            </select>
                        </td>
                        <td style="width: 5px;">
                        </td>
                        <td>
                            <label>
                                Driver Name</label>
                            <input id="txt_driver_name" class="form-control" type="hidden" placeholder="Select Driver Name" />
                            <input id="txt_driver" class="form-control" type="text" placeholder="Select Driver Name" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Route Name</label>
                            <input id="txt_routename" class="form-control" type="text" placeholder="Select Route Name" />
                                <input id="txt_route" type="hidden" style="height: 28px; opacity: 1.0; width: 150px;" />
                        </td>
                        <td style="width: 5px;">
                        </td>
                        <td>
                            <label>
                                Vehicle Start Reading</label>
                            <input id="txt_startreading" class="form-control" type="text" placeholder="Enter Vehicle Start Reading" />
                        </td>
                    </tr>
                   
                    <tr>
                     <td>
                            <label>
                                Qty</label>
                            <input id="txt_qty" class="form-control" type="text" placeholder="Enter Qty" />
                        </td>
                         <td style="width:5px;"></td>
                          <td>
                            <label>Hour Reading</label>
                             <input id="txt_hourreading" class="form-control" type="text" placeholder="Enter Hour Reading" />
                          </td>
                    </tr>
                    <tr>
                        <td>
                           <label>Billing Owner</label>
                            <select id="slct_BillingOwners_List" class="form-control">
                               <option selected disabled value="Select Billing Owner">Select Billing Owner</option>
                            </select>                            
                        </td>
                         <td style="width:5px;"></td>
                        <td></td>
                    </tr>
                    <tr>

                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input id="btn_Starttrip" type="button" value="Start Trip" class="btn btn-primary"
                                onclick="btnstart_tripsheetclick();" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </section>
                    </div>
                    <div role="tabpanel" class="tab-pane" id="TripEnd">
                        <section class="content">
                            <div class="box box-info">
                                <div class="box-header with-border">
                                    <h3 class="box-title">
                                        <i style="padding-right: 5px;" class="fa fa-cog"></i>Trip End
                                    </h3>
                                </div>
                                <div class="box-body">
                                    <div style="padding: 20px; text-align: center;">
                                    <table>
                                        <tr>
                                            <td>
                                                <label>
                                                    Assigned Trip Sheets</label>
                                                <select id="slct_ass_trip_end" class="form-control" >
                                                    <option selected disabled value="Select Trip">Select Trip</option>
                                                </select>
                                            </td>
                                            <td style="height: 20px; width: 20px;">
                                            </td>
                                            <td >
                             <input type="button"   id="btn_gen" class="btn btn-primary"  name="Generate" value='Generate' onclick="end_tripsheet_change();"/>
                                    </td>
                                        </tr>
                                     
                                        </table>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td>
                                                    <div align="center">
                                                        <table style='margin: 10px; color: #777777 !important; font-weight: bold; font-family: sans-serif;'>
                                                            <tr>
                                                                <td style="color: #080A89">
                                                                    End Odometer Reading
                                                                </td>
                                                                <td>
                                                                    <input id="txt_endodometerrdng" type="text" class="form-control" placeholder="EndOdometer" onkeyup="totalCalculation()" />
                                                                </td>
                                                            </tr>
                                                              <tr>
                                                                <td style="color: #080A89">
                                                                    End Hour Meter Reading
                                                                </td>
                                                                <td>
                                                                    <input id="txt_endhourmtrrdng" type="text" class="form-control" placeholder="End HourReading"
                                                                        onkeyup="totalCalculation()" />
                                                                </td>
                                                            <tr>
                                                                <td style="height: 20px;">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="color: #080A89">
                                                                    Trip KMS
                                                                </td>
                                                                <td style="color: Red; font-size: 25px;">
                                                                    <span id="lbl_tripkms">0</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 20px;">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="color: #080A89">
                                                                    GPS KMS
                                                                </td>
                                                                <td style="color: Red; font-size: 25px;">
                                                                    <input id="txt_gpskms" type="text" value="0" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 20px;">
                                                                </td>
                                                                <td>
                                              <label id="lbl_costperltr" style="display: none;"></label>
                                                    <br />
                                            </td>
                                                            </tr>
                                                            
                                                        </table>
                                                    </div>
                                                </td>
                                                <td width="400px">
                                                    <div style="border: solid 1px #d5d5d5;">
                                                        <div class="divstyle">
                                                            <span style="text-align: center; font-size: 20px; color: orange;">Trip Details</span>
                                                        </div>
                                                        <div id="div_tripdetails">
                                                            <table style='margin: 10px; color: #777777 !important; font-weight: bold; font-family: sans-serif;'>
                                                                <tr>
                                                                    <td style="color: #080A89; width: 150px;">
                                                                        Start Date
                                                                    </td>
                                                                    <td>
                                                                        <span id="lbl_startdate"></span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="color: #080A89; width: 150px;">
                                                                        Vehicle Number
                                                                    </td>
                                                                    <td>
                                                                        <span id="lbl_vehno"></span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="color: #080A89; width: 150px;">
                                                                        Route
                                                                    </td>
                                                                    <td>
                                                                        <span id="lbl_route"></span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="color: #080A89; width: 150px;">
                                                                        Driver Name
                                                                    </td>
                                                                    <td>
                                                                        <span id="lbl_Driver"></span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="color: #080A89; width: 150px;">
                                                                       Starting Odometer
                                                                    </td>
                                                                    <td>
                                                                        <span id="lbl_startodometer">0</span>
                                                                    </td>

                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <div align="left">
                                            <input id="btn_endtrip" type="button" class="btn btn-primary" value="End Trip" onclick="BtnSaveEndTripSheetClick();" />
                                           
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </section>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

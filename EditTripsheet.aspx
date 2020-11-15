<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="EditTripsheet.aspx.cs" Inherits="TripsheetEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/utility.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            getallveh_nos();
            get_driverand_helper();

        });

        var veh_data = [];
        function getallveh_nos() {
            var data = { 'op': 'get_all_vehhilcles' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        veh_data = [];
                        fillvehmasterdata(msg);
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
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].registration_no;
                    option.value = msg[i].vm_sno;
                    data.appendChild(option);
                }
            }
        }
        function vehonchenage() {
            var veh_sno = document.getElementById('slct_vehicle_no').value;
            for (var i = 0; i < veh_data.length; i++) {
                if (veh_data[i].vm_sno == veh_sno) {
                    document.getElementById('lbl_capacity').innerHTML = veh_data[i].Capacity;
                    document.getElementById('txt_vehtype').value = veh_data[i].VehType;
                    document.getElementById('lbl_fuelcapacity').innerHTML = veh_data[i].v_ty_fuel_capacity;
                    return;
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
        function filldrive_helper(msg) {
            var data = document.getElementById('txt_driver_name');
            var length = data.options.length;
            document.getElementById('txt_driver_name').options.length = null;
            var data2 = document.getElementById('txt_helper_name');
            var length2 = data2.options.length;
            document.getElementById('txt_helper_name').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Driver";
            opt.value = "Select Driver";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            var opt2 = document.createElement('option');
            opt2.innerHTML = "Select Helper";
            opt2.value = "Select Helper";
            opt2.setAttribute("selected", "selected");
            opt2.setAttribute("disabled", "disabled");
            opt2.setAttribute("class", "dispalynone");
            data2.appendChild(opt2);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].emp_sno != null && msg[i].emp_type == "Driver") {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].employname;
                    option.value = msg[i].emp_sno;
                    data.appendChild(option);
                }
                if (msg[i].emp_sno != null && msg[i].emp_type == "Helper") {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].employname;
                    option.value = msg[i].emp_sno;
                    data2.appendChild(option);
                }
            }
        }
        function GetTripSheetValues() {
            var TripRefno = document.getElementById('txt_Tripid').value;
            if (TripRefno == "") {
                alert("Please enter tripsheet no");
                return false;
            }
            var data = { 'op': 'GetEditTripSheetValues', 'TripRefno': TripRefno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        if (msg == "No Tripsheet found") {
                            $('#divEditTrip').css('display', 'none');
                            alert(msg);
                            return false;
                        }
                        else {
                            $('#divEditTrip').css('display', 'block');
                            document.getElementById('slct_vehicle_no').value = msg[0].VehicleSno;
                            document.getElementById('txt_helper_name').value = msg[0].HelperID;
                            document.getElementById('txt_routename').value = msg[0].RouteID;
                            document.getElementById('txt_driver_name').value = msg[0].DriverID;
                            document.getElementById('txt_startreading').value = msg[0].VehStartReading;
                            document.getElementById('txt_hourreading').value = msg[0].HrReading;
                            document.getElementById('txt_loadtype').value = msg[0].LoadType;
                            document.getElementById('txt_qty').value = msg[0].Qty;
                            document.getElementById('txt_endodometerrdng').value = msg[0].EndOdometerReading;
                            document.getElementById('txt_endfuelrdng').value = msg[0].Fuelfilled;
                            document.getElementById('txt_pumpreading').value = msg[0].PumpReading;
                            document.getElementById('txt_token').value = msg[0].Token;
                            document.getElementById('txt_tripstrtdate').value = msg[0].tripdate;
                            document.getElementById('txt_enddate').value = msg[0].enddate;
                            document.getElementById('txt_reffuel').value = msg[0].refrigerationfuel;
                            document.getElementById('txt_endhr').value = msg[0].endhrreading;
                        }
                    }
                    else {
                        $('#divEditTrip').css('display', 'none');
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function btnEditTripSheetSaveClick() {
            var vehicleNo = document.getElementById('slct_vehicle_no').value;
            var RouteID = document.getElementById('txt_routename').value;
            var driver = document.getElementById('txt_driver_name').value;
            var helper = document.getElementById('txt_helper_name').value;
            var VehicleStartReading = document.getElementById('txt_startreading').value;
            var HrReading = document.getElementById('txt_hourreading').value;
            var load = document.getElementById('txt_loadtype').value;
            var Qty = document.getElementById('txt_qty').value;
            var txtendodometerrdng = document.getElementById('txt_endodometerrdng').value;
            var txtendfuelrdng = document.getElementById('txt_endfuelrdng').value;
            var txtpumpreading = document.getElementById('txt_pumpreading').value;
            var txttoken = document.getElementById('txt_token').value;
            var txtTripid = document.getElementById('txt_Tripid').value;
            var startdate = document.getElementById('txt_tripstrtdate').value;
            var enddate = document.getElementById('txt_enddate').value;
            var refrigerationfuel = document.getElementById('txt_reffuel').value;
            var Endhrreading = document.getElementById('txt_endhr').value;
            var data = { 'op': 'btnEditTripSheetSaveClick', 'txtTripid': txtTripid, 'startdate': startdate, 'enddate': enddate, 'vehicleNo': vehicleNo, 'RouteID': RouteID, 'driver': driver, 'helper': helper, 'VehicleStartReading': VehicleStartReading, 'HrReading': HrReading, 'load': load, 'Qty': Qty, 'txtendodometerrdng': txtendodometerrdng, 'txtendfuelrdng': txtendfuelrdng, 'txtpumpreading': txtpumpreading, 'txttoken': txttoken, 'refrigerationfuel': refrigerationfuel, 'Endhrreading': Endhrreading };
            var s = function (msg) {
                if (msg) {
                    getallveh_nos();
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
            document.getElementById('txt_helper_name').selectedIndex = 0;
            document.getElementById('txt_routename').value = "";
            document.getElementById('txt_driver_name').selectedIndex = 0;
            document.getElementById('txt_startreading').value = "";
            document.getElementById('txt_hourreading').value = "";
            document.getElementById('txt_loadtype').value = "";
            document.getElementById('txt_qty').value = "";
            document.getElementById('txt_endodometerrdng').value = "";
            document.getElementById('txt_endfuelrdng').value = "";
            document.getElementById('txt_pumpreading').value = "";
            document.getElementById('txt_token').value = "";
        }
        //        function findSize() {
        //            var fileInput = document.getElementById("myFile");
        //            try {
        //                alert(fileInput.files[0].size); // Size returned in bytes.
        //            } catch (e) {
        //                var objFSO = new ActiveXObject("Scripting.FileSystemObject");
        //                var e = objFSO.getFile(fileInput.value);
        //                var fileSize = e.size;
        //                alert(fileSize);
        //            }
        //        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>Edit Tripsheet<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Edit Tripsheet</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Trip Details
                </h3>
            </div>
            <div class="box-body">
                <div id="second_div" style="padding: 20px;">
                    <div role="tabpanel" class="tab-pane" id="TripLogs">
                        <div class="row">
                            <div>
                                <table align="center">
                                    <tr>
                                        <td>
                                            <label>
                                                Trip Ref No</label>
                                        </td>
                                        <td>
                                            <input type="text" id="txt_Tripid" maxlength="45" class="form-control" placeholder="Enter Trip RefNo" />
                                        </td>
                                        <td style="width: 5px;"></td>
                                        <td>
                                            <input id="Button1" type="button" class="btn btn-primary" value="Get Trip Details"
                                                onclick="GetTripSheetValues()" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div class="tab-content" style="display: none;" id="divEditTrip">
                        <div role="tabpanel" class="tab-pane active" id="TripStart">
                            <table align="center">
                                <tr>
                                    <td>
                                        <label>
                                            Start Date</label>
                                        <input id="txt_tripstrtdate" class="form-control" type="datetime-local" />
                                    </td>
                                    <td style="width: 5px;"></td>
                                    <td>
                                        <label>
                                            Vehicle Number</label>
                                        <select id="slct_vehicle_no" class="form-control" onchange="vehonchenage()">
                                            <option selected disabled value="Select Vehicle No">Select Vehicle No</option>
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Vehicle Type</label>
                                        <input id="txt_vehtype" disabled class="form-control" type="text" placeholder="Vehicle Type" />
                                    </td>
                                    <td style="width: 5px;"></td>
                                    <td>
                                        <label>
                                            Capacity(Ltr)</label>
                                        <label id="lbl_capacity" class="form-control">
                                        </label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Fuel Capacity(Ltr)</label>
                                        <label id="lbl_fuelcapacity" class="form-control">
                                        </label>
                                    </td>
                                    <td style="width: 5px;"></td>
                                    <td>
                                        <label>
                                            Driver Name</label>
                                        <select id="txt_driver_name" class="form-control" style="min-width: 200px;">
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Helper Name</label>
                                        <select id="txt_helper_name" class="form-control" style="min-width: 200px;">
                                        </select>
                                    </td>
                                    <td style="width: 5px;"></td>
                                    <td>
                                        <label>
                                            Route Name</label>
                                        <input id="txt_routename" class="form-control" type="text" placeholder="Enter Route Name" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Vehicle Start Reading</label>
                                        <input id="txt_startreading" class="form-control" type="text" placeholder="Enter Vehicle Start Reading" />
                                    </td>
                                    <td style="width: 5px;"></td>
                                    <td>
                                        <label>
                                            Hour Start Reading</label>
                                        <input id="txt_hourreading" class="form-control" type="text" placeholder="Enter Hour Reading" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Load Type</label>
                                        <input id="txt_loadtype" class="form-control" type="text" placeholder="Enter Load Type" />
                                    </td>
                                    <td style="width: 5px;"></td>
                                    <td>
                                        <label>
                                            Qty</label>
                                        <input id="txt_qty" class="form-control" type="text" placeholder="Enter Qty" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            End Date</label>
                                        <input id="txt_enddate" class="form-control" type="datetime-local" />
                                    </td>
                                    <td style="width: 5px;"></td>
                                    <td>
                                        <label>
                                            End Odometer Reading</label>
                                        <input id="txt_endodometerrdng" type="text" class="form-control" placeholder="End Odometer" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Fuel Filled</label>
                                        <input id="txt_endfuelrdng" type="text" class="form-control" placeholder="Fuel Filled" />
                                    </td>
                                    <td style="width: 5px;"></td>
                                    <td>
                                        <label>
                                            Pump Reading</label>
                                        <input id="txt_pumpreading" class="form-control" type="text" placeholder="Enter Pump Reading" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Token</label>
                                        <input id="txt_token" class="form-control" type="text" placeholder="Enter Token" />
                                    </td>
                                    <td style="width: 5px;"></td>
                                    <td>
                                        <label>
                                            Refrigeration Fuel</label>
                                        <input id="txt_reffuel" class="form-control" type="text" placeholder="Enter Refrigeration Fuel" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Hour End Reading</label>
                                        <input id="txt_endhr" class="form-control" type="text" placeholder="Enter Refrigeration Fuel" />
                                    </td>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <input id="btn_Starttrip" type="button" value="EDIT TRIP SHEET" class="btn btn-primary"
                                            onclick="btnEditTripSheetSaveClick();" />
                                    </td>
                                    <td style="width: 5px;"></td>
                                    <td>
                                        <input id="btn_Print" type="button" value="PRINT" class="btn btn-danger" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>

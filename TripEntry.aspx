<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="TripEntry.aspx.cs" Inherits="TripEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/utility.js" type="text/javascript"></script>
     <link href="autocomplete/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            retrive_locations();
            getallveh_nos();
            get_driverand_helper();
            get_jobcards();
            get_dieselcost();
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
        function get_dieselcost() {
            var data = { 'op': 'get_diesel_cost' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        document.getElementById("lbl_costperltr").innerHTML = msg[0].costperltr;
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
        var options = "";
        function retrive_locations() {
            var data = { 'op': 'retrive_all_location' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        options = "";
                        for (var i = 0; i < msg.length; i++) {
                            if (msg[i].Location_name != null) {
                                options += "<option value=" + msg[i].sno + ">" + msg[i].Location_name + "</option>";
                            }
                        }
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
        var nme = 0;
        function add_location_log() {
            var rowcount = $('#tbl_trip_locations tbody tr').length;
            if (rowcount > 0) {
                var lastrow = $('#tbl_trip_locations tbody tr:last');
                var from_locof_last = $(lastrow).find('[name=From_location] :selected').text();
                var KMS = $(lastrow).find('[name=kms]').val();
                var checkbox = "NO";
                if ($(lastrow).find('.Own').is(":checked")) {
                    checkbox = "OWN";
                }
                if ($(lastrow).find('.Hired').is(":checked")) {
                    checkbox = "HIRED";
                }
                if (checkbox == "NO") {
                    $(lastrow).find('[name=fuel]').val("");
                }
                var firstrow = $('#tbl_trip_locations tbody tr:first');
                var from_locof_first_val = $(firstrow).find('[name=From_location] :selected').val();

                if ($(lastrow).find('[name=datetime_log]').val() == "") {
                    alert("Eneter Proper trip Datetime");
                    $(lastrow).find('[name=datetime_log]').focus();
                    return;
                }
                if (from_locof_last != "Location" && KMS != "") {
                    //$(lastrow).find('[name=From_location]').attr('disabled', 'disabled');
                    //$(lastrow).find('[name=To_location]').attr('disabled', 'disabled');
                    $("#tbl_trip_locations").append('<tr><td><input style="max-width: 185px;font-size:12px;padding: 0px 5px;height:30px;" type="datetime-local" class="form-control" name="datetime_log" /></td>' +
                '<td data-title="From"><select class="form-control" name="From_location" style="width: 100px;font-size:12px;padding: 0px 5px;height:30px;" ><option selected disabled value="Location">Location</option>' + options + '</select></td>' +
                '<td data-title="Odometer"><input class="form-control" type="text" placeholder="OdoMeter" name="OdoMeter" style="width:90px;font-size:12px;padding: 0px 5px;height:30px;" onblur="cal_odomtr(this)"/></td>' +
                '<td data-title="KMS"><input class="form-control" type="text" placeholder="KMS" name="kms" value="0" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="Cost"><input id="Text1" class="form-control" type="text" placeholder="Charge" name="charge" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="Load"><input name="txt_capload" class="form-control" type="text" placeholder="In Ltrs" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/><input name="txt_capload_kgs" class="form-control" type="text" placeholder="In Kgs" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="UnLoad"><input name="txt_capunload" class="form-control" type="text" placeholder="In Ltrs" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/><input name="txt_capunload_kgs" class="form-control" type="text" placeholder="In Kgs" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="Fuel"><label><input name="' + nme + '" type="radio" class="Own"/>Own</label><label><input name="' + nme + '" type="radio" class="Hired"/>Hired</label><input class="form-control" name="fuel" type="text" placeholder="Fuel" style="width:90px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="acfuel"><input class="form-control" name="acfuel" type="text" placeholder="acfuel" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="Expences"><input class="form-control" name="expences" type="text" placeholder="Expences" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="TollGate"><input class="form-control" name="tollgate" type="text" placeholder="TollGate" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="Remarks"><textarea class="form-control" name="remarks" placeholder="Remarks" style="width:70px;font-size:12px;padding: 0px 5px;" cols="20" rows="3"></textarea></td></tr>');
//                    only_no_trips();
                }
                else {
                    alert("Please Select Proper Location and KMS");
                    $(lastrow).find('[name=From_location]').focus();
                }
                var lastrow_after = $('#tbl_trip_locations tbody tr:last');
                var to_locof_last_val_after = $(lastrow).find('[name=From_location] :selected').val();
            }
            else {
                $("#tbl_trip_locations").append('<tr><td><input style="max-width: 185px;font-size:12px;padding: 0px 5px;height:30px;" type="datetime-local" class="form-control" name="datetime_log" /></td>' +
                '<td data-title="From"><select class="form-control" name="From_location" style="width: 100px;font-size:12px;padding: 0px 5px;height:30px;" ><option selected disabled value="Location">Location</option>' + options + '</select></td>' +
                '<td data-title="Odometer"><input class="form-control" type="text" placeholder="OdoMeter" name="OdoMeter" style="width:90px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="KMS"><input class="form-control" type="text" placeholder="KMS" name="kms" value="0" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="Cost"><input id="Text1" class="form-control" type="text" placeholder="Charge" name="charge" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="Load"><input name="txt_capload" class="form-control" type="text" placeholder="In Ltrs" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/><input name="txt_capload_kgs" class="form-control" type="text" placeholder="In Kgs" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="UnLoad"><input name="txt_capunload" class="form-control" type="text" placeholder="In Ltrs" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/><input name="txt_capunload_kgs" class="form-control" type="text" placeholder="In Kgs" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="Fuel"><label><input name="' + nme + '" type="radio" class="Own"/>Own</label><label><input name="' + nme + '" type="radio" class="Hired"/>Hired</label><input class="form-control" name="fuel" type="text" placeholder="Fuel" style="width:90px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="acfuel"><input class="form-control" name="acfuel" type="text" placeholder="acfuel" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="Expences"><input class="form-control" name="expences" type="text" placeholder="Expences" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="TollGate"><input class="form-control" name="tollgate" type="text" placeholder="TollGate" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="Remarks"><textarea class="form-control" name="remarks" placeholder="Remarks" style="width:70px;font-size:12px;padding: 0px 5px;" cols="20" rows="3"></textarea></td></tr>');
                nme++;
//                only_no_trips();
            }
        }
        function cal_odomtr(thisid) {
            var thisodo = $(thisid).val();
            var prevodo = $(thisid).parent().parent().prev().children().find('[name=OdoMeter]').val();
            var kms = parseInt(thisodo) - parseInt(prevodo);
            $(thisid).parent().parent().find('[name=kms]').val(kms);
        }
        //Function for only no
        $(document).ready(function () {
            //$("[name=charge]").keydown(function (event) {
            $("[name=charge],[name=txt_capload],[name=txt_capunload],[name=fuel],[name=expences],[name=tollgate],[name=kms],[name=OdoMeter],[name=txt_capload_kgs],[name=txt_capunload_kgs]").keydown(function (event) {
                // Allow: backspace, delete, tab, escape, and enter
                if (event.keyCode == 46 || event.keyCode == 110 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 || event.keyCode == 190 ||
                // Allow: Ctrl+A
            (event.keyCode == 65 && event.ctrlKey === true) ||
                // Allow: home, end, left, right
            (event.keyCode >= 35 && event.keyCode <= 39)) {
                    // let it happen, don't do anything
                    return;
                }
                else {
                    // Ensure that it is a number and stop the keypress
                    if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                        event.preventDefault();
                    }
                }
            });
        });

        function getkms(thisid) {
            var val_tolocation = $(thisid).val();
            var fromlocation_val = $(thisid).parent().parent().prev().find('select[name*="From_location"] :selected').val();
            var data = { 'op': 'get_kms_from_to', 'fromlocation_val': fromlocation_val, 'val_tolocation': val_tolocation };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        $(thisid).parent().parent().find('.kms').html(msg[0].Distance);
                    }
                    else {
                        document.getElementById('lbl_popfromloc').innerHTML = fromlocation_val;
                        document.getElementById('lbl_poptoloc').innerHTML = val_tolocation;
                        document.getElementById('fieldpopup').style.display = 'block';
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function insert_distances() {
            var fromloc = document.getElementById('lbl_popfromloc').innerHTML;
            var toloc = document.getElementById('lbl_poptoloc').innerHTML;
            var distance = document.getElementById('txt_updatedistance').value;
            var Data = { 'op': 'insert_Distances_fromtrip', 'fromloc': fromloc, 'toloc': toloc, 'distance': distance };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        alert(msg);
                        var lastrow = $('#tbl_trip_locations tbody tr:last');
                        $(lastrow).find('.kms').text(distance);
                        document.getElementById('fieldpopup').style.display = 'none';
                        document.getElementById('lbl_popfromloc').innerHTML = "";
                        document.getElementById('lbl_poptoloc').innerHTML = "";
                        document.getElementById('txt_updatedistance').value = "";
                    }
                }
            }
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(Data, s, e);
        }

        var veh_module_config_Data = [];
        function getallveh_nos_frommoduleConfig() {
            var data = { 'op': 'getallveh_nos_frommoduleConfig' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        veh_module_config_Data = [];
                        //fillvehmasterdata(msg);
                        veh_module_config_Data = msg;
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

        var veh_data = [];
        function getallveh_nos() {
            var data = { 'op': 'get_all_veh_master_data_notrips' };
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
        $(function () {
            //FillRegions();
        });
        var Regions = "";
        function get_jobcards() {
            var minimaster = "JobCards";
            var data = { 'op': 'get_Mini_Master_data', 'minimaster': minimaster };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        for (var i = 0; i < msg.length; i++) {
                            if (msg[i].mm_name != null && msg[i].mm_status != "0" && msg[i].mm_type == "JobCards") {
                                Regions += msg[i].mm_name + ",";
                            }
                        }
                        Regions = Regions.substring(0, Regions.length - 1);
                        FillRegions();
                    }
                    else {
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
        function FillRegions() {
            document.getElementById('divWork').innerHTML = "";
            var Region = Regions.split(',');
            var data = "<table style='margin: 12px 25px 12px 25px;'>";
            for (var i = 0; i <= Region.length; i++) {
                if (typeof (Region[i]) != "undefined") {
                    data += "<tr><td><input type='checkbox' name='checkbox' value='checkbox' onchange='ckb_onchange(this);' id = " + i + " class = 'chkclass'/><span for=" + i + ">" + Region[i] + "</span></td><td><input type='text' value='' class = 'txtClass'/></td></tr>";
                }
            }
            data += "</table>";
            $('#divWork').append(data);
        }
        function btnTripSheetSaveClick() {
            var TripDate = document.getElementById('txt_tripstrtdate').value;
            var vehicleNo = document.getElementById('slct_vehicle_no').value;
            var RouteID = document.getElementById('txt_routename').value;
            var driver = document.getElementById('txt_driver_name').value;
            var helper = document.getElementById('txt_helper_name').value;
            var VehicleStartReading = document.getElementById('txt_startreading').value;
            var HrReading = document.getElementById('txt_hourreading').value;
            var FuelTank = "";
            var load = document.getElementById('txt_loadtype').value;
            var Qty = document.getElementById('txt_qty').value;
            var tripstartfrom = document.getElementById('txt_startFrom').value;
            var route = document.getElementById('txt_route').value;
            //var txttoken = document.getElementById('txt_token').value;
            var BillingOwnerID = document.getElementById('slct_BillingOwners_List').value;
            var Dsalary = document.getElementById("txt_Dsalary").value;

            if (vehicleNo == "Select Vehicle No") {
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
            if (VehicleStartReading == "") {
                alert("Please enter vehicle start reading");
                return;
            }
            if (load == "") {
                alert("Please enter load type");
                return;
            }
            if (Qty == "") {
                alert("Please enter quantity");
                return;
            }
            if (helper == "Select Helper") {
                helper = 0;
            }
            if (BillingOwnerID == "Select Billing Owner") {
                alert("Please Select Billing Owner otherwise Defaultmode Zero...");
                BillingOwnerID = 0;
                return;
            }
            if (Dsalary == "") {
                alert("Please enter driver salary");
                Dsalary = 0;
                return;
            }
            var data = { 'op': 'btnTripSheetSaveClick', 'TripDate': TripDate, 'vehicleNo': vehicleNo, 'RouteID': RouteID, 'driver': driver, 'helper': helper, 'VehicleStartReading': VehicleStartReading, 'HrReading': HrReading, 'FuelTank': FuelTank, 'load': load, 'Qty': Qty, 'tripstartfrom': tripstartfrom, 'BillingOwnerID': BillingOwnerID, 'Dsalary': Dsalary };
            var s = function (msg) {
                if (msg) {
                    getallveh_nos();
                    btnRefreshTripSheetClick();
                    fill_tripsheet_no();
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
            document.getElementById('slct_vehicle_no').selectedIndex = 0;
            document.getElementById('txt_helper_name').selectedIndex = 0;
            document.getElementById('txt_routename').selectedIndex = "";
            document.getElementById('txt_driver_name').value = "";
            document.getElementById('txt_driver').value = "";
            document.getElementById('txt_startreading').value = "";
            document.getElementById('txt_hourreading').value = "";
            //document.getElementById('txt_fuel').value = "";
            document.getElementById('txt_loadtype').value = "";
            document.getElementById('txt_qty').value = "";
            document.getElementById('txt_startFrom').value = "";
            document.getElementById('txt_vehtype').value = "";
            document.getElementById('lbl_capacity').innerHTML = "";
            document.getElementById('txt_route').value = "";
            document.getElementById('lbl_fuelcapacity').innerHTML = "";
            document.getElementById('txt_rateperkm').value = "";
            document.getElementById('slct_BillingOwners_List').selectedIndex = 0; 
            document.getElementById('txt_Dsalary').value = "";
        }
        /////////////////////////...................JobCards................////////////////////////////////
        function tripsheet_change() {
            var ckdvlsdiv = document.getElementById('divWork').childNodes;
            for (var i = 0, row; row = ckdvlsdiv[0].rows[i]; i++) {
                if (row.cells[0].childNodes[0].type == 'checkbox') {
                    row.cells[0].childNodes[0].checked = false;
                    row.cells[0].childNodes[0].title = "";
                    row.cells[1].childNodes[0].value = "";
                }
            }

            var Jobcards = 'N';
            var tripsno = document.getElementById('cmb_Tripsheets').value;
            for (i = 0; i < assignedtrips.length; i++) {
                if (assignedtrips[i].tripsno == tripsno) {
                    document.getElementById('txt_vehno').value = assignedtrips[i].vehicleno;
                    Jobcards = assignedtrips[i].Jobcards;
                    break;
                }
                else {
                    document.getElementById('txt_vehno').value = "";
                }
            }
            if (Jobcards == 'Y') {
                var data = { 'op': 'gettripjobcards', 'tripsno': tripsno };
                var s = function (msg) {
                    if (msg) {
                        for (var i = 0, row; row = ckdvlsdiv[0].rows[i]; i++) {
                            if (row.cells[0].childNodes[0].type == 'checkbox') {
                                var labelval = row.cells[0].childNodes[1].innerHTML;
                                var txtval = row.cells[1].childNodes[0].value;
                                for (var jc = 0; jc < msg.length; jc++) {
                                    if (labelval == msg[jc].jobcard) {
                                        row.cells[0].childNodes[0].checked = true;
                                        row.cells[0].childNodes[0].title = msg[jc].jobcardstatus;
                                        row.cells[1].childNodes[0].value = msg[jc].jobcarddetails;
                                        break;
                                    }
                                }
                            }
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
        }
        function BtnSaveJobcardsClick() {
            var tripsno = document.getElementById('cmb_Tripsheets').value;
            var jobcarddate = ""; //  document.getElementById('txt_datetime').value;
            if (tripsno == "Select Trip") {
                alert("Please select trip to assign job cards");
                return false;
            }
            var ckdvlsdiv = document.getElementById('divWork').childNodes;
            var checkedjobcards = [];
            for (var i = 0, row; row = ckdvlsdiv[0].rows[i]; i++) {
                if (row.cells[0].childNodes[0].type == 'checkbox' && row.cells[0].childNodes[0].checked == true) {
                    var labelval = row.cells[0].childNodes[1].innerHTML;
                    var txtval = row.cells[1].childNodes[0].value;
                    checkedjobcards.push({ 'jobtype': labelval, 'jobdetails': txtval });
                }
            }
            if (checkedjobcards.length == 0) {
                alert("Please select job cards");
                return false;
            }
            var data = { 'op': 'jobcardsaveclick', 'jobcarddate': jobcarddate, 'tripsno': tripsno, 'checkedjobcards': checkedjobcards };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    //                    FillTripsheets();
                    btnRefreshJobcardsClick();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            CallHandlerUsingJson(data, s, e);
        }
        function btnRefreshJobcardsClick() {
            document.getElementById('cmb_Tripsheets').selectedIndex = 0;
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
            $('#txt_datetime').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
            document.getElementById('txt_vehno').value = "";
            var ckdvlsdiv = document.getElementById('divWork').childNodes;
            var ckdvlsdiv = document.getElementById('divWork').childNodes;
            for (var i = 0, row; row = ckdvlsdiv[0].rows[i]; i++) {
                if (row.cells[0].childNodes[0].type == 'checkbox') {
                    row.cells[0].childNodes[0].checked = false;
                    row.cells[0].childNodes[0].title = "";
                    row.cells[1].childNodes[0].value = "";
                }
            }
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
            $('#txt_datetime').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
        }

        function BtnSaveEndTripSheetClick() {
            var endodordng = document.getElementById('txt_endodometerrdng').value;
            var endfuelrdng = document.getElementById('txt_endfuelrdng').value;
            var endhourmtrrdng = document.getElementById('txt_endhourmtrrdng').value;
            var gpskms = document.getElementById('txt_gpskms').value;
            var tripsno = document.getElementById('slct_ass_trip_end').value;
            var mileage = document.getElementById('lbl_tripmileage').innerHTML;
            var TripDate = ""; // document.getElementById('txt_datetime').value;
            var fuelprice = document.getElementById('txt_fuelprice').value;
            var txtpumpreading = document.getElementById('txt_pumpreading').value;
            var txttoken = document.getElementById('txt_token').value;
            var total_expences = document.getElementById("spn_tripexpences").innerHTML;
            var perlitercost = document.getElementById("lbl_costperltr").innerHTML;
            var txtrefrigeration = document.getElementById('txt_refrigeration').value;
            var ddlfuelstatus = document.getElementById('ddlfuel').value;
            if (endodordng == "") {
                alert("Please Enter Odometer Reading");
                return;
            }
            if (endfuelrdng == "") {
                alert("Please Enter Fuel Value");
                return;
            }
            if (fuelprice == "") {
                alert("Please Enter Diesel rate");
                return;
            }
            if (endhourmtrrdng == "") {
                alert("Please enter Hour Meter Reading");
                return;
            }
            if (tripsno == "Select Trip") {
                alert("Please select Trip");
                return;
            }
            var data = { 'op': 'btnTripendSaveClick','fuelprice':fuelprice, 'TripDate': TripDate, 'tripsno': tripsno, 'endodordng': endodordng, 'endfuelrdng': endfuelrdng, 'endhourmtrrdng': endhourmtrrdng, 'gpskms': gpskms, 'mileage': mileage, 'txtpumpreading': txtpumpreading, 'txttoken': txttoken, 'total_expences': total_expences, 'perlitercost': perlitercost, 'txtrefrigeration': txtrefrigeration, 'ddlfuelstatus': ddlfuelstatus };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Trip ended successfully") {
                        btnEndTripSheetRefreshClick();
                        fill_tripsheet_no();
                        //                        FillTripsheets();
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
        function ckb_onchange(id) {
            if (id.title == "C") {
                id.checked = true;
                alert("This jobcard already completed");
            }
        }
        function btnEndTripSheetRefreshClick() {
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1; //January is 0!
            var yyyy = today.getFullYear();
            if (dd < 10) {
                dd = '0' + dd;
            }
            if (mm < 10) {
                mm = '0' + mm;
            }
            var hrs = today.getHours();
            var mnts = today.getMinutes();
            $('#txt_datetime').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
            document.getElementById('div_jobcards').innerHTML = null;
            document.getElementById('lbl_startdate').innerHTML = "";
            document.getElementById('lbl_vehno').innerHTML = "";
            document.getElementById('lbl_route').innerHTML = "";
            document.getElementById('lbl_Driver').innerHTML = "";
            document.getElementById('lbl_startodometer').innerHTML = "0";
            document.getElementById('lbl_startfuel').innerHTML = "0";
            document.getElementById('lbl_tripkms').innerHTML = "0";
            document.getElementById('txt_gpskms').value = "0";
            document.getElementById('lbl_fuelconsumption').innerHTML = "0";
            document.getElementById('lbl_tripmileage').innerHTML = "0";
            document.getElementById('lbl_diffbtengpsandmanualkms').innerHTML = "0";
            document.getElementById('txt_endodometerrdng').value = "0";
            document.getElementById('txt_endfuelrdng').value = "0";
            document.getElementById('txt_endhourmtrrdng').value = "0";
            document.getElementById('lbl_betweenfuel').innerHTML = "0";
            document.getElementById('txt_vehtype').value = "";
            document.getElementById('lbl_capacity').innerHTML = "";
            document.getElementById('lbl_fuelcapacity').innerHTML = "";
            document.getElementById('txt_rateperkm').value = "";
            document.getElementById('cmb_Tripsheets').selectedIndex = 0;
            document.getElementById('txt_pumpreading').value = "";
            document.getElementById('txt_token').value = "";
            document.getElementById('slct_ass_trip_end').value = "Select Trip";
            document.getElementById('spn_tripexpences').innerHTML = "0";
        }
        var assignedtrips;
        function jobcard_tripsheet_change() {
            var ckdvlsdiv = document.getElementById('divWork').childNodes;
            for (var i = 0, row; row = ckdvlsdiv[0].rows[i]; i++) {
                if (row.cells[0].childNodes[0].type == 'checkbox') {
                    row.cells[0].childNodes[0].checked = false;
                    row.cells[0].childNodes[0].title = "";
                    row.cells[1].childNodes[0].value = "";
                }
            }
            var Jobcards = 'N';
            var tripsno = document.getElementById('cmb_Tripsheets').value;
            for (i = 0; i < assignedtrips.length; i++) {
                if (assignedtrips[i].sno == tripsno) {
                    document.getElementById('txt_vehno').value = assignedtrips[i].registration_no;
                    Jobcards = assignedtrips[i].Jobcards;
                    break;
                }
                else {
                    document.getElementById('txt_vehno').value = "";
                }
            }
            if (Jobcards == 'Y') {
                var data = { 'op': 'gettripjobcards', 'tripsno': tripsno };
                var s = function (msg) {
                    if (msg) {
                        for (var i = 0, row; row = ckdvlsdiv[0].rows[i]; i++) {
                            if (row.cells[0].childNodes[0].type == 'checkbox') {
                                var labelval = row.cells[0].childNodes[1].innerHTML;
                                var txtval = row.cells[1].childNodes[0].value;
                                for (var jc = 0; jc < msg.length; jc++) {
                                    if (labelval == msg[jc].jobcard) {
                                        row.cells[0].childNodes[0].checked = true;
                                        row.cells[0].childNodes[0].title = msg[jc].jobcardstatus;
                                        row.cells[1].childNodes[0].value = msg[jc].jobcarddetails;
                                        break;
                                    }
                                }
                            }
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
        }
        function end_tripsheet_change() {
            document.getElementById('div_jobcards').innerHTML = null;
            document.getElementById('lbl_startdate').innerHTML = "";
            document.getElementById('lbl_vehno').innerHTML = "";
            document.getElementById('lbl_route').innerHTML = "";
            document.getElementById('lbl_Driver').innerHTML = "";
            document.getElementById('lbl_startodometer').innerHTML = "0";
            document.getElementById('lbl_startfuel').innerHTML = "0";
            document.getElementById('lbl_tripkms').innerHTML = "0";
            document.getElementById('txt_gpskms').value = "0";
            document.getElementById('lbl_fuelconsumption').innerHTML = "0";
            document.getElementById('lbl_tripmileage').innerHTML = "0";
            document.getElementById('lbl_diffbtengpsandmanualkms').innerHTML = "0";
            document.getElementById('txt_endodometerrdng').value = "0";
            document.getElementById('txt_endfuelrdng').value = "0";
            document.getElementById('txt_endhourmtrrdng').value = "0";
            var TripDate = ""; //  document.getElementById('txt_datetime').value;
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
                    document.getElementById('lbl_startfuel').innerHTML = msg.StrartFuel;
                    if (msg.logfuel == "") {
                        document.getElementById('lbl_betweenfuel').innerHTML = 0;
                    }
                    else {
                        document.getElementById('lbl_betweenfuel').innerHTML = msg.logfuel;
                    }
                    if (msg.logexpences == "") {
                        document.getElementById('lbl_logexpences').innerHTML = 0;
                    }
                    else {
                        document.getElementById('lbl_logexpences').innerHTML = msg.logexpences;
                    }
                    var gpskms = msg.gpskms;
                    gpskms = parseFloat(gpskms).toFixed(2);
                    document.getElementById('txt_gpskms').value = gpskms;
                    var data = "<table style='margin: 10px;color: #777777 !important;font-weight: bold;font-family: sans-serif;'>";
                    for (var i = 0; i < msg.jobcards.length; i++) {
                        var status = "";
                        var jobcardsts = msg.jobcards[i].status;
                        if (jobcardsts == "A") {
                            status = "Assigned";
                        }
                        else {
                            status = "Completed";
                        }
                        data += "<tr><td style='color: #080A89'><span>" + msg.jobcards[i].jobcardname + "</span></td><td style='width:20px;'></td><td><span>" + status + "</span></td></tr>";
                    }
                    data += "</table>";
                    $('#div_jobcards').append(data);
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
            var srartfuelrdng = document.getElementById('lbl_startfuel').innerHTML;
            var betweenfuel = document.getElementById('lbl_betweenfuel').innerHTML;
            var endodordng = document.getElementById('txt_endodometerrdng').value;
            var endfuelrdng = document.getElementById('txt_endfuelrdng').value;
            if (endfuelrdng == "") {
                endfuelrdng = "0";
            }
            var tripkms = 0;
            var fuelconsumption = 0;
            if (srartodordng != "" && endodordng != "") {
                srartodordng = parseFloat(srartodordng)
                endodordng = parseFloat(endodordng)
                tripkms = (endodordng - srartodordng).toFixed(2);
            }
            fuelconsumption = parseFloat(endfuelrdng) + parseFloat(betweenfuel);
            document.getElementById('lbl_tripkms').innerHTML = tripkms;
            document.getElementById('lbl_fuelconsumption').innerHTML = fuelconsumption;
            tripkms = parseFloat(tripkms);
            fuelconsumption = parseFloat(fuelconsumption);
            var mileage = 0;
            if (tripkms != 0 && fuelconsumption != 0) {
                mileage = (tripkms / fuelconsumption);
            }
            document.getElementById('lbl_tripmileage').innerHTML = parseFloat(mileage).toFixed(2);
            var gpskms = document.getElementById('txt_gpskms').value;
            gpskms = parseFloat(gpskms);
            var diffbtwngpsandmanual = tripkms - gpskms;
            diffbtwngpsandmanual = parseFloat(diffbtwngpsandmanual).toFixed(2);
            document.getElementById('lbl_diffbtengpsandmanualkms').innerHTML = diffbtwngpsandmanual;
            var costperltr = document.getElementById("lbl_costperltr").innerHTML;
            if (costperltr == "") {
                costperltr = "0";
            }
            var logexpences = document.getElementById("lbl_logexpences").innerHTML;
            var totalexpences = 0;
            if (endfuelrdng != "") {
                totalexpences = (parseFloat(costperltr) * parseFloat(endfuelrdng)) + parseFloat(logexpences);
            }
            if (totalexpences == "NaN") {
                totalexpences = 0;
            }
            document.getElementById("spn_tripexpences").innerHTML = totalexpences.toFixed(2);
        }

        function CallHandlerUsingJson(d, s, e) {
            d = JSON.stringify(d);
            d = d.replace(/&/g, '\uFF06');
            d = d.replace(/#/g, '\uFF03');
            d = d.replace(/\+/g, '\uFF0B');
            d = d.replace(/\=/g, '\uFF1D');
            $.ajax({
                type: "GET",
                url: "FleetManagementhandler.axd?json=",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: d,
                async: true,
                cache: true,
                success: s,
                error: e
            });
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
            var data2 = document.getElementById('txt_helper_name');
            var length2 = data2.options.length;
            document.getElementById('txt_helper_name').options.length = null;
            var opt2 = document.createElement('option');
            opt2.innerHTML = "Select Helper";
            opt2.value = "Select Helper";
            opt2.setAttribute("selected", "selected");
            opt2.setAttribute("disabled", "disabled");
            opt2.setAttribute("class", "dispalynone");
            data2.appendChild(opt2);
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
        //Function for only no
        $(document).ready(function () {
            $("#txt_hourreading,#txt_rateperkm,#txt_qty,#txt_startreading,#txt_updatedistance,#txt_pumpreading,#txt_endodometerrdng,#txt_endhourmtrrdng,#txt_endfuelrdng").keydown(function (event) {
                // Allow: backspace, delete, tab, escape, and enter
                if (event.keyCode == 46 || event.keyCode == 110 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 || event.keyCode == 190 ||
                // Allow: Ctrl+A
            (event.keyCode == 65 && event.ctrlKey === true) ||
                // Allow: home, end, left, right
            (event.keyCode >= 35 && event.keyCode <= 39)) {
                    // let it happen, don't do anything
                    return;
                }
                else {
                    // Ensure that it is a number and stop the keypress
                    if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                        event.preventDefault();
                    }
                }
            });
        });
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
            var data = document.getElementById('cmb_Tripsheets');
            var length = data.options.length;
            document.getElementById('cmb_Tripsheets').options.length = null;
            var data2 = document.getElementById('slct_ass_trip_end');
            var length2 = data2.options.length;
            document.getElementById('slct_ass_trip_end').options.length = null;
            var data3 = document.getElementById('slct_triplogs_tripid');
            var length3 = data3.options.length;
            document.getElementById('slct_triplogs_tripid').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Trip";
            opt.value = "Select Trip";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            var opt2 = document.createElement('option');
            opt2.innerHTML = "Select Trip";
            opt2.value = "Select Trip";
            opt2.setAttribute("selected", "selected");
            opt2.setAttribute("disabled", "disabled");
            opt2.setAttribute("class", "dispalynone");
            data2.appendChild(opt2);
            var opt3 = document.createElement('option');
            opt3.innerHTML = "Select Trip";
            opt3.value = "Select Trip";
            opt3.setAttribute("selected", "selected");
            opt3.setAttribute("disabled", "disabled");
            opt3.setAttribute("class", "dispalynone");
            data3.appendChild(opt3);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].sno != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].tripsheetno;
                    option.value = msg[i].sno;
                    data.appendChild(option);
                    var option2 = document.createElement('option');
                    option2.innerHTML = msg[i].tripsheetno;
                    option2.value = msg[i].sno;
                    data2.appendChild(option2);
                    var option3 = document.createElement('option');
                    option3.innerHTML = msg[i].tripsheetno;
                    option3.value = msg[i].sno;
                    data3.appendChild(option3);
                }
            }
        }
        function for_saving_trip_logs() {
            var tripid = document.getElementById('slct_triplogs_tripid').value;
            var tripbtnval = document.getElementById('btn_save_triplogs').value;
            var triplogs_array = [];
            $('#tbl_trip_locations> tbody > tr').each(function () {
                var log_datetime = $(this).find('[name="datetime_log"]').val();
                var log_tolocation = $(this).find('select[name*="To_location"] :selected').val();
                var log_fromlocation = $(this).find('select[name*="From_location"] :selected').val();
                var log_kms = $(this).find('[name="kms"]').val();
                var log_charge = $(this).find('[name="charge"]').val();
                var log_capload = $(this).find('[name="txt_capload"]').val();
                var log_capunload = $(this).find('[name="txt_capunload"]').val();
                var log_capload_kgs = $(this).find('[name="txt_capload_kgs"]').val();
                var log_capunload_kgs = $(this).find('[name="txt_capunload_kgs"]').val();
                var log_fuel = $(this).find('[name="fuel"]').val();
                var log_expences = $(this).find('[name="expences"]').val();
                var log_acfuel = $(this).find('[name="acfuel"]').val();
                var log_tollgate = $(this).find('[name="tollgate"]').val();
                var log_OdoMeter = $(this).find('[name="OdoMeter"]').val();
                var log_remarks = $(this).find('[name="remarks"]').val();
                var rowindex = $(this).index();
                var rank = (parseInt(rowindex) + 1).toString();
                var checkbox = "NO";
                if ($(this).find('.Own').is(":checked")) {
                    checkbox = "OWN";
                }
                if ($(this).find('.Hired').is(":checked")) {
                    checkbox = "HIRED";
                }
                if (log_datetime != "") {
                    triplogs_array.push({ 'log_datetime': log_datetime,'log_acfuel':log_acfuel, 'log_tolocation': log_tolocation, 'log_fromlocation': log_fromlocation, 'log_kms': log_kms,
                        'log_charge': log_charge, 'log_capload': log_capload, 'log_capunload': log_capunload, 'log_fuel': log_fuel, 'log_expences': log_expences,
                        'rank': rank, 'log_tollgate': log_tollgate, 'log_fueltype': checkbox, 'log_OdoMeter': log_OdoMeter, 'log_capload_kgs': log_capload_kgs, 'log_capunload_kgs': log_capunload_kgs, 'log_remarks': log_remarks
                    });
                }
            });
            var data = { 'op': 'TripLog_save_start' };
            var s = function (msg) {
                if (msg) {
                    for (var i = 0; i < triplogs_array.length; i++) {
                        var Data = { 'op': 'TripLog_save_RowData', 'row_detail': triplogs_array[i], 'end': 'N' };
                        if (i == triplogs_array.length - 1) {
                            Data = { 'op': 'TripLog_save_RowData', 'row_detail': triplogs_array[i], 'end': 'Y' };
                        }
                        var s = function (msg) {
                            if (msg == 'Y') {
                                var Data = { 'op': 'save_edit_TripLog', 'tripid': tripid, 'tripbtnval': tripbtnval
                                };
                                var s = function (msg) {
                                    if (msg) {
                                        alert(msg);
                                    }
                                }
                                var e = function (x, h, e) {
                                };
                                callHandler(Data, s, e);
                            }
                        }
                        var e = function (x, h, e) {
                        };
                        CallHandlerUsingJson(Data, s, e);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        }
        function closePopup() {
            document.getElementById('fieldpopup').style.display = 'none';
        }
        function openPopup() {
            document.getElementById('fieldpopup').style.display = 'block';
        }
        function for_resettriplogs() {
            var table = document.getElementById("tbl_trip_locations");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            document.getElementById("slct_triplogs_tripid").value = "Select Trip";
            document.getElementById('btn_save_triplogs').disabled = false;
        }

        function check_triplog_save() {
            var tripid = document.getElementById("slct_triplogs_tripid").value;
            var Data = { 'op': 'check_triplog_save', 'tripid': tripid
            };
            var s = function (msg) {
                if (msg) {
                    if (msg == "YES") {
                        alert("Trip Logs Already Existed For this TripID");
                        document.getElementById('btn_save_triplogs').disabled = true;
                    }
                    else {
                        document.getElementById('btn_save_triplogs').disabled = false;
                    }
                }
            }
            var e = function (x, h, e) {
            };
            callHandler(Data, s, e);
        }
        var start_odometer = 0;
        function get_trip_startingodometer() {
            var tripid = document.getElementById("slct_triplogs_tripid").value;
            var Data = { 'op': 'get_trip_startingodometer', 'tripid': tripid
            };
            var s = function (msg) {
                if (msg) {
                    start_odometer = msg;
                }
            }
            var e = function (x, h, e) {
            };
            callHandler(Data, s, e);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 100%; height: 100%;">
        <div id="second_div" style="padding: 20px;">
            <div role="tabpanel">
                <!-- Nav tabs -->
                <ul class="nav nav-tabs" role="tablist">
                    <li role="presentation" class="active"><a href="#TripStart" aria-controls="TripStart"
                        role="tab" data-toggle="tab">Trip Start</a></li>
                    <li role="presentation"><a href="#TripLogs" aria-controls="TripLogs" role="tab" data-toggle="tab">
                        Trip Logs</a></li>
                    <li role="presentation"><a href="#IssueJobcard" aria-controls="IssueJobcard" role="tab"
                        data-toggle="tab">Issue Jobcard</a></li>
                    <li role="presentation"><a href="#TripEnd" aria-controls="TripEnd" role="tab" data-toggle="tab">
                        Trip End</a></li>
                </ul>
                <!-- Tab panes -->
                <div class="tab-content">
                    <div role="tabpanel" class="tab-pane active" id="TripStart">
                        <section class="content">
                            <div class="box box-info">
                                <div class="box-header with-border">
                                    <h3 class="box-title">
                                        <i style="padding-right: 5px;" class="fa fa-cog"></i>Trip Assign
                                    </h3>
                                </div>
                                <div class="box-body">
                                        <table align="center">
                                            <tr>
                                                <td>
                                                    <label>
                                                        Start Date</label>
                                                    <input id="txt_tripstrtdate" class="form-control" type="datetime-local" />
                                                </td>
                                               <td style="width:5px;"></td>

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
                                               <td  style="width:5px;"></td>
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
                                               <td style="width:5px;"></td>
                                                <td>
                                                    <label>
                                                        Driver Name</label>
                                                   <%-- <select id="txt_driver_name" class="form-control" style="min-width: 200px;">
                                                    </select>--%>
                                                    <input id="txt_driver_name" class="form-control" type="hidden" placeholder="Select Route Name" />
                                                    <input id="txt_driver" class="form-control" type="text" placeholder="Select Route Name" />

                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label>
                                                        Helper Name</label>
                                                    <select id="txt_helper_name" class="form-control" style="min-width: 200px;">
                                                    </select>
                                                </td>
                                               <td style="width:5px;"></td>
                                                <td>
                                                    <label>
                                                        Route Name</label>
                                                    <input id="txt_routename" class="form-control" type="text" placeholder="Select Route Name" />
                                <input id="txt_route" type="hidden" style="height: 28px; opacity: 1.0; width: 150px;" />

                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label>
                                                        Vehicle Start Reading</label>
                                                    <input id="txt_startreading" class="form-control" type="text" placeholder="Enter Vehicle Start Reading" />
                                                </td>
                                               <td style="width:5px;"></td>
                                                <td>
                                                    <label>
                                                        Hour Start Reading</label>
                                                    <input id="txt_hourreading" class="form-control" type="text" placeholder="Enter Hour Reading" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label>
                                                        RS/KM</label>
                                                    <input id="txt_rateperkm" class="form-control" type="text" placeholder="Enter RS/KM" />
                                                </td>
                                               <td style="width:5px;"></td>
                                                <td>
                                                    <label>
                                                        Load Type</label>
                                                    <input id="txt_loadtype" class="form-control" type="text" placeholder="Enter Load Type" />
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
                                                    <label>
                                                        Start From</label>
                                                    <input id="txt_startFrom" class="form-control" type="text" placeholder="Enter Start From" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label>
                                                        Remarks</label>
                                                    <textarea rows="3" cols="45" id="txt_remarks" class="form-control" maxlength="200"
                                                        placeholder="Enter Remarks"></textarea>
                                                  
                                                </td>
                                               <td style="width:5px;"></td>
                                                <td>
                                                    <label>Billing Owner</label>
                                                    <select id="slct_BillingOwners_List" class="form-control">
                                                        <option selected disabled value="Select Billing Owner">Select Billing Owner</option>                                                         
                                                    </select>
                                                </td>
                                               
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label>Salary</label>
                                                    <input type="text"  id="txt_Dsalary" class="form-control" placeholder="Enter Driver Salary"/>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                            <td></td>
                                               <td style="width:5px;"></td>
                                            <td>
                                              <label id="lbl_costperltr" style="display: none;"></label>
                                                    <br />
                                            </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input id="btn_Starttrip" type="button" value="START TRIP" class="btn btn-primary"
                                                        onclick="btnTripSheetSaveClick();" />
                                                </td>
                                               <td style="width:5px;"></td>
                                                <td>
                                                    <input id="btn_Print" type="button" value="PRINT" class="btn btn-danger" />
                                                </td>
                                            </tr>
                                        </table>
                                </div>
                            </div>
                        </section>
                    </div>
                    <div role="tabpanel" class="tab-pane" id="TripLogs">
                        <section class="content">
                            <div class="box box-info">
                                <div class="box-header with-border">
                                    <h3 class="box-title">
                                        <i style="padding-right: 5px;" class="fa fa-cog"></i>Trip Logs
                                    </h3>
                                </div>
                                <div class="box-body">
                                    <div style="padding: 20px; text-align: center;">
                                        <table >
                                            <tr>
                                                <td>
                                                    <label>
                                                        Assigned Trips</label>
                                                    <select id="slct_triplogs_tripid" class="form-control" style="min-width: 200px;"
                                                        onchange="check_triplog_save()">
                                                        <option selected disabled value="Select
    Trip">Select Trip</option>
                                                    </select>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <div style="overflow-x: scroll;">
                                            <table id="tbl_trip_locations" class="table table-bordered table-hover dataTable no-footer"
                                                role="grid" aria-describedby="example2_info" style="font-size: 12px;">
                                                <thead>
                                                    <tr>
                                                        <th scope="col">
                                                            Date
                                                        </th>
                                                        <th scope="col">
                                                            Location
                                                        </th>
                                                        <%--<th scope="col"> To </th>--%>
                                                        <th scope="col">
                                                            OdoMeter
                                                        </th>
                                                        <th scope="col">
                                                            KMS
                                                        </th>
                                                        <th scope="col">
                                                            Cost
                                                        </th>
                                                        <th scope="col">
                                                            Load
                                                        </th>
                                                        <th scope="col">
                                                            UnLoad
                                                        </th>
                                                        <th scope="col">
                                                            Diesel Type
                                                        </th>
                                                        <th scope="col">
                                                            AC Fuel
                                                        </th>
                                                        <th scope="col">
                                                            Expences
                                                        </th>
                                                        <th scope="col">
                                                            TollGate
                                                        </th>
                                                        <th scope="col">
                                                            Remarks
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                </tbody>
                                            </table>
                                        </div>
                                        <div>
                                        <br />
                                        </div>
                                        <div>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <input id="btn_addlocation" type="button" class="btn btn-default" value="ADD Another Location"
                                                            onclick="add_location_log()" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <table align="center">
                                                <tr>
                                                    <td>
                                                        <input id="btn_save_triplogs" type="button" class="btn btn-primary" value="Save
    Trip Logs" onclick="for_saving_trip_logs()" />
                                                    </td>
                                                    <td>
                                                        <input id="btn_resettriplogs" type="button" class="btn btn-danger" value="Reset Trip Logs"
                                                            onclick="for_resettriplogs()" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </section>
                    </div>
                    <div role="tabpanel" class="tab-pane" id="IssueJobcard">
                        <section class="content">
                            <div class="box box-info">
                                <div class="box-header with-border">
                                    <h3 class="box-title">
                                        <i style="padding-right: 5px;" class="fa fa-cog"></i>Job Cards
                                    </h3>
                                </div>
                                <div class="box-body">
                                    <div style="padding: 20px; text-align: center;">
                                        <table>
                                            <tr>
                                                <td>
                                                    <label>
                                                        Assigned Trip Sheets</label>
                                                    <select id="cmb_Tripsheets" class="form-control" onchange="jobcard_tripsheet_change();">
                                                        <option selected disabled value="Select Trip">Select Trip</option>
                                                    </select>
                                                </td>
                                                <td style="width:6px;"></td>
                                                <td>
                                                    <label>
                                                        Vehicle Number</label>
                                                    <input id="txt_vehno" disabled class="form-control" type="text" />
                                                </td>
                                            </tr>
                                        </table>
                                        <div class="row">
                                            <%-- <div class="form-group"> <label>
    Date</label> <input id="txt_datetime" class="form-control" type="date"/> </div>--%>
                                            <div class="form-group">
                                            </div>
                                        </div>
                                        <div align="center">
                                            <table>
                                                <tr>
                                                    <td colspan="2" align="center" style="text-align: left;">
                                                        <div id="divWork" style="float: left; border-radius: 7px 7px 7px 7px; padding: 3% 0% 3% 0%;
                                                            border: 2px solid grey; font-weight: bold;">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div>
                                            <input type="button" id="BtnSave" value="Save Job Card" onclick="BtnSaveJobcardsClick();"
                                                class="btn btn-primary" />&nbsp&nbsp
                                            <input type="button" id="btnreset" value="Reset Job Card" onclick="btnRefreshJobcardsClick();"
                                                class="btn btn-danger" />
                                        </div>
                                    </div>
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
                                                <select id="slct_ass_trip_end" class="form-control" onchange="end_tripsheet_change();">
                                                    <option selected disabled value="Select Trip">Select Trip</option>
                                                </select>
                                            </td>
                                        </tr>
                                        </table>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td>
                                                    <div align="center">
                                                        <table style='margin: 10px; color: #777777 !important; font-weight: bold; font-family: sans-serif;'>
                                                            <%--
    <tr> <td style="color: #080A89"> Trip End Date </td> <td style="color: #080A89;
    font-size: 20px;"> <input id="txt_datetime" type="datetime-local" class="txtsize"
    /> </td> </tr>--%>
                                                            <tr>
                                                                <td style="color: #080A89">
                                                                    End Odometer Reading
                                                                </td>
                                                                <td>
                                                                    <input id="txt_endodometerrdng" type="text" class="form-control" placeholder="End
    Odometer" onkeyup="totalCalculation()" />
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
                                                            </tr>
                                                            <tr>
                                                                <td style="color: #080A89">
                                                                    Fuel Filled
                                                                </td>
                                                                <td>
                                                                    <input id="txt_endfuelrdng" type="text" class="form-control" placeholder="Fuel Filled"
                                                                        onkeyup="totalCalculation()" />
                                                                </td>
                                                                <td>
                                                                    <select id="ddlfuel" class="form-control">
                                                                        <option>Now</option>
                                                                        <option>Next</option>
                                                                    </select>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="color: #080A89">
                                                                    Pump Reading
                                                                </td>
                                                                <td>
                                                                    <input id="txt_pumpreading" class="form-control" type="text" placeholder="Enter Pump Reading" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="color: #080A89">
                                                                    Token
                                                                </td>
                                                                <td>
                                                                    <input id="txt_token" class="form-control" type="text" placeholder="Enter Token" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="color: #080A89">
                                                                    Refrigeration Fuel Tank
                                                                </td>
                                                                <td>
                                                                    <input id="txt_refrigeration" class="form-control" type="text" placeholder="Enter Refrigeration Fuel Tank" />
                                                                </td>
                                                            </tr>
                                                             <tr>
                                                                <td style="color: #080A89">
                                                                    Fuel Price
                                                                </td>
                                                                <td>
                                                                    <input id="txt_fuelprice" class="form-control" type="text" placeholder="Enter Fuel Price" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="color: #080A89">
                                                                    Total Trip Expences
                                                                </td>
                                                                <td style="color: Red; font-size: 25px;">
                                                                    <label id="lbl_logexpences" style="display: none;">
                                                                    </label>
                                                                    <span id="spn_tripexpences">0</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 20px;">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="color: #080A89">
                                                                    Fuel Consumption
                                                                </td>
                                                                <td style="color: Red; font-size: 25px;">
                                                                    <span id="lbl_fuelconsumption">0</span>
                                                                </td>
                                                            </tr>
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
                                                            </tr>
                                                            <tr>
                                                                <td style="color: #080A89">
                                                                    Trip Mileage
                                                                </td>
                                                                <td style="color: Red; font-size: 25px;">
                                                                    <span id="lbl_tripmileage">0</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 20px;">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="color: #080A89">
                                                                    Difference between GPS and Manual KMS
                                                                </td>
                                                                <td style="color: Red; font-size: 25px;">
                                                                    <span id="lbl_diffbtengpsandmanualkms">0</span>
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
                                                                        Odometer Reading
                                                                    </td>
                                                                    <td>
                                                                        <span id="lbl_startodometer">0</span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="color: #080A89; width: 150px;">
                                                                        Fuel at start (Ltrs)
                                                                    </td>
                                                                    <td>
                                                                        <span id="lbl_startfuel">0</span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="color: #080A89; width: 150px;">
                                                                        Fuel Filled in Trip(Ltrs)
                                                                    </td>
                                                                    <td>
                                                                        <span id="lbl_betweenfuel">0</span>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div class="divstyle">
                                                            <span style="text-align: center; font-size: 20px; color: orange;">Job Cards</span>
                                                        </div>
                                                        <div id="div_jobcards">
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <div align="left">
                                            <input id="btn_endtrip" type="button" class="btn btn-primary" value="End Trip" onclick="BtnSaveEndTripSheetClick();" />
                                            <input id="btn_endtrip_reset" type="button" class="btn btn-danger" value="Reset"
                                                onclick="btnEndTripSheetRefreshClick();" />
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
    <div id="fieldpopup" class="pickupclass" style="text-align: center; height: 100%;
        width: 100%; position: absolute; display: none; left: 0%; top: 0%; z-index: 99999;
        background: rgba(192, 192, 192, 0.7);">
        <div id="fieldslct" style="position: absolute; top: 35%; background-color: White;
            left: 25%; right: 15%; width: 50%; -webkit-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
            -moz-box-shadow: 1px 1px
    10px rgba(50, 50, 50, 0.65); box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65); border-radius: 10px 10px 10px 10px;">
            <br />
            <br />
            <div style="width: 100%; height: 100%;">
                <div align="center">
                    Distance Not Available Between these locations<br />
                    <br />
                    <div class="row" style="margin-left: 29%;">
                        <div class="form-group">
                            <label>
                                Enter Distance</label>
                            <input id="txt_updatedistance" class="form-control" type="text" />
                        </div>
                        <label style="display: none;" id="lbl_popfromloc">
                        </label>
                        <label style="display: none;" id="lbl_poptoloc">
                        </label>
                    </div>
                </div>
                <br />
                <button type="button" class="btn
    btn-primary" onclick="insert_distances();">
                    Submit changes
                </button>
                <br />
            </div>
            <div id="divclose" style="width: 25px; top: 0%; right: 0%; position: absolute; z-index: 99999;
                cursor: pointer;">
                <img src="images/PopClose.png" alt="close" onclick="closePopup();" />
            </div>
        </div>
    </div>
</asp:Content>

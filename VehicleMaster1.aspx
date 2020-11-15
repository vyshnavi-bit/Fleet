<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="VehicleMaster1.aspx.cs" Inherits="VehicleMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="autocomplete/jquery-ui.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
            .dispalynone
            {
            display: none;
            }
            .bpmouseover
            {
            height: 155px;
            width: 200px;
            display: none;
            position: absolute;
            z-index: 99999;
            padding: 10px 5px 5px 15px;
            background-color: #fffffc;
            border: 1px solid #c0c0c0;
            border-radius: 3px 3px 3px 3px;
            box-shadow: 3px 3px 3px rgba(0,0,0,0.3);
            font-family: Verdana;
            font-size: 12px;
            opacity: 1.0;
            }

            .row + .row > *
            {
            padding: 0px 0 0 40px;
            }
    .img-circle {
    border-radius: 50%;
}
.img-thumbnail {
    display: inline-block;
    max-width: 100%;
    height: auto;
    padding: 4px;
    line-height: 1.42857143;
    background-color: #fff;
    border: 1px solid #ddd;
    border-radius: 4px;
    -webkit-transition: all .2s ease-in-out;
    -o-transition: all .2s ease-in-out;
    transition: all .2s ease-in-out;
}
.photo-edit-icon-admin {
    background: none repeat scroll 0 0 #fff;
    border-radius: 50%;
    padding: 6px 10px;
    </style>
    <script type="text/javascript">
        function callHandler(d, s, e) {
            $.ajax({
                url: 'FleetManagementHandler.axd',
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
        function CallPrint(strid) {
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
        function CallHandlerUsingJson(d, s, e) {
            d = JSON.stringify(d);
            d = d.replace(/&/g, '\uFF06');
            d = d.replace(/#/g, '\uFF03');
            d = d.replace(/\+/g, '\uFF0B');
            d = d.replace(/\=/g, '\uFF1D');
            $.ajax({
                type: "GET",
                url: "FleetManagementHandler.axd?json=",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: d,
                async: true,
                cache: true,
                success: s,
                error: e
            });
        }

        //------------>Prevent Backspace<--------------------//
        $(document).unbind('keydown').bind('keydown', function (event) {
            var doPrevent = false;
            if (event.keyCode === 8) {
                var d = event.srcElement || event.target;
                if ((d.tagName.toUpperCase() === 'INPUT' && (d.type.toUpperCase() === 'TEXT' || d.type.toUpperCase() === 'PASSWORD'))
            || d.tagName.toUpperCase() === 'TEXTAREA') {
                    doPrevent = d.readOnly || d.disabled;
                } else {
                    doPrevent = true;
                }
            }
            if (doPrevent) {
                event.preventDefault();
            }
        });

        var options_val = "";
        var axil_name_check = "";
        $(function () {
            get_tyresize();
            getinsurancecompany();
            retrivedata();
            get_tyresdata();
            getBatterydetails();
            $('.hiddenrow').hide();
            $('#div_axilautofill').hide();
            only_no();
            getall_axilnames();
            $('#add_partnumber').click(function () {
                $('.hiddenrow').hide();
                $('#vehmaster_fillform').css('display', 'block');
                $('#vehmaster_showlogs').css('display', 'none');
                $('#div_vehmaster_table').css('display', 'none');
                $('.hiddenrow').hide();
                hiding_error();
                clearall();
                $('#odometer_div').show();
                $('#slct_vehtype').attr('disabled', false);
                $('#tyre_ass_done').hide();
                $('#tyre_assign').show();
                document.getElementById('tyre_check').checked = false;
                document.getElementById('slct_axilmaster').disabled = true;
                document.getElementById('slct_axilmaster').value = "Select Axil Master";
                document.getElementById('txt_odometer').disabled = true;
                document.getElementById('txt_odometer').value = "";
            });
            $('#close_vehmaster').click(function () {
                $('.hiddenrow').hide();
                $('#vehmaster_fillform').css('display', 'none');
                $('#vehmaster_showlogs').css('display', 'block');
                $('#div_vehmaster_table').css('display', 'block');
                $('.hiddenrow').hide();
                hiding_error();
                clearall();
            });
            $('#close_documents').click(function () {
                $('.hiddenrow').hide();
                $('#vehmaster_fillform').css('display', 'none');
                $('#vehmaster_showlogs').css('display', 'block');
                $('#div_vehmaster_table').css('display', 'block');
                $('.hiddenrow').hide();
                hiding_error();
                clearall();
            });
        });

        //--------->Getting vehtypes<--------------//
        function getBatterydetails() {
            var FormName = "VendorNames";
            var data = { 'op': 'get_Battery_details', 'FormName': FormName };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillBatterydetails(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillBatterydetails(vehtypemsg) {
            var insurancecompany = document.getElementById('ddlBatteryno');
            var length = insurancecompany.options.length;
            document.getElementById('ddlBatteryno').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Battery No1";
            opt.value = "Select Battery No1";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            insurancecompany.appendChild(opt);
            for (var i = 0; i < vehtypemsg.length; i++) {
                if (vehtypemsg[i].bat_sno != null && vehtypemsg[i].bat_sno != "0") {
                    var option = document.createElement('option');
                    option.innerHTML = vehtypemsg[i].bat_sno;
                    option.value = vehtypemsg[i].Sno;
                    insurancecompany.appendChild(option);
                }
            }

            var ddlBatteryno2 = document.getElementById('ddlBatteryno2');
            var length = ddlBatteryno2.options.length;
            document.getElementById('ddlBatteryno2').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Battery No2";
            opt.value = "Select Battery No2";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            ddlBatteryno2.appendChild(opt);
            for (var i = 0; i < vehtypemsg.length; i++) {
                if (vehtypemsg[i].bat_sno != null && vehtypemsg[i].bat_sno != "0") {
                    var option = document.createElement('option');
                    option.innerHTML = vehtypemsg[i].bat_sno;
                    option.value = vehtypemsg[i].Sno;
                    ddlBatteryno2.appendChild(option);
                }
            }
        }
        //--------->Getting vehtypes<--------------//
        function getinsurancecompany() {
            var data = { 'op': 'get_insurancecompany' };
            var s = function (msg) {
                if (msg) {
                    fillinsurancecompany(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillinsurancecompany(vehtypemsg) {
            var insurancecompany = document.getElementById('slct_insurancecompany');
            var length = insurancecompany.options.length;
            document.getElementById('slct_insurancecompany').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Insurence Company";
            opt.value = "Select Insurence Company";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            insurancecompany.appendChild(opt);
            for (var i = 0; i < vehtypemsg.length; i++) {
                if (vehtypemsg[i].InsuranceName != null && vehtypemsg[i].InsuranceName != "0") {
                    var option = document.createElement('option');
                    option.innerHTML = vehtypemsg[i].InsuranceName;
                    option.value = vehtypemsg[i].InsSno;
                    insurancecompany.appendChild(option);
                }
            }
        }
        function get_tyresize() {
            var minimaster = "VehicleMake,VehicleType";
            var data = { 'op': 'get_Mini_Master_data', 'minimaster': minimaster };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        tyresizedata(msg);
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
        function tyresizedata(msg) {
            var vehmake = document.getElementById('slct_vehiclemake');
            var length = vehmake.options.length;
            document.getElementById('slct_vehiclemake').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Vehicle Make";
            opt.value = "Select Vehicle Make";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            vehmake.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].mm_name != null && msg[i].mm_status != "0" && msg[i].mm_type == "VehicleMake") {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].mm_name;
                    option.value = msg[i].sno;
                    vehmake.appendChild(option);
                }
            }
            var vehtype = document.getElementById('slct_vehtype');
            var length = vehtype.options.length;
            document.getElementById('slct_vehtype').options.length = null;
            var opt2 = document.createElement('option');
            opt2.innerHTML = "Select Vehicle Type";
            opt2.value = "Select Vehicle Type";
            opt2.setAttribute("selected", "selected");
            opt2.setAttribute("disabled", "disabled");
            opt2.setAttribute("class", "dispalynone");
            vehtype.appendChild(opt2);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].mm_name != null && msg[i].mm_status != "0" && msg[i].mm_type == "VehicleType") {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].mm_name;
                    option.value = msg[i].sno;
                    vehtype.appendChild(option);
                }
            }
        }
        //---------> End OF Getting Part Groups<--------------//
        function hiding_error() {
            $("#lbl_reg_error_msg").hide();
            $("#lbl_vehtype_error_msg").hide();
            $("#lbl_vehmake_error_msg").hide();
            $("#lbl_category_error_msg").hide();
            $("#lbl_odometer_error_msg").hide();
            $("#lbl_capacity_error_msg").hide();
            $("#lbl_vehmake_error_msg").hide();
            $("#lbl_fuelcapacity_error_msg").hide();
            $("#lbl_error_axilmstr").hide();
        }
        function save_vehmaster_click() {
            var regno = document.getElementById('txt_regno').value;
            var vehtype = document.getElementById('slct_vehtype').value;
            var doorno = document.getElementById('txt_doorno').value;
            var sno = document.getElementById('txt_sno').value;
            var btnval = document.getElementById('save_vehmaster').value;
            var status = document.getElementById('cmb_status').value;
            var Actualmileage = document.getElementById('txt_actualmileage').value;
            var Capacity = document.getElementById('txt_capacity').value;
            var model = document.getElementById('txt_model').value;
            var engineno = document.getElementById('txt_engineno').value;
            var chasiss = document.getElementById('txt_chasiss').value;
            var owner = document.getElementById('txt_owner').value;
            var address = document.getElementById('txt_address').value;
            var rcno = document.getElementById('txt_rcno').value;
            var rcexp_date = document.getElementById('txt_rcexp_date').value;
            var pollution = document.getElementById('txt_pollution').value;
            var pollexp_date = document.getElementById('txt_pollexp_date').value;
            var insurence = document.getElementById('txt_insurence').value;
            var insexp_date = document.getElementById('txt_insexp_date').value;
            var fitness = document.getElementById('txt_fitness').value;
            var fitexp_date = document.getElementById('txt_fitexp_date').value;
            var roadtax = document.getElementById('txt_roadtax').value;
            var roadtaxexp_date = document.getElementById('txt_roadtaxexp_date').value;
            var vehicle_make = document.getElementById('slct_vehiclemake').value;
            var fuel_capacity = document.getElementById('txt_fuelcapacity').value;
            var slctinsurancecompany = document.getElementById('slct_insurancecompany').value;
            var permitstatename = document.getElementById('txt_permitstatename').value;
            var permit_state_expdate = document.getElementById('txt_permit_state_expdate').value;
            var stateroadtax = document.getElementById('txt_stateroadtax').value;
            var stateroadtaxexp_date = document.getElementById('txt_stateroadtaxexp_date').value;
            var Batteryno = document.getElementById('ddlBatteryno').value;
            var Batteryno2 = document.getElementById('ddlBatteryno2').value;
            var flag = false;
            var axilmaster = "";
            var Odometer = "";
            if (document.getElementById('tyre_check').checked == true) {
                axilmaster = document.getElementById('slct_axilmaster').value;
                Odometer = document.getElementById('txt_odometer').value;
                if (Odometer == "") {
                    $("#lbl_odometer_error_msg").show();
                    flag = true;
                }
                if (axilmaster == "Select Axil Master") {
                    $("#lbl_error_axilmstr").show();
                    flag = true;
                }
            }
            if (regno == "") {
                $("#lbl_reg_error_msg").show();
                flag = true;
            }
            if (vehtype == "Select Vehicle Type") {
                $("#lbl_vehtype_error_msg").show();
                flag = true;
            }
            if (Capacity == "") {
                $("#lbl_capacity_error_msg").show();
                flag = true;
            }
            if (slctinsurancecompany == "") {
                $("#lbl_insurancecompany").show();
                flag = true;
            }
            if (vehicle_make == "Select Vehicle Make") {
                $("#lbl_vehmake_error_msg").show();
                flag = true;
            }
            if (fuel_capacity == "") {
                $("#lbl_fuelcapacity_error_msg").show();
                flag = true;
            }
            if (flag) {
                return;
            }
            var leftalert = "";
            var rightalert = "";
            var stepalert = "";
            // if (btnval == "Save") {
            var axil_table = [];
            if (document.getElementById('tyre_check').checked == true) {
                $('#tbl_axils> tbody > tr:even').each(function () {
                    var innertable_array = [];
                    var axilname = $(this).find(".axilname").html();
                    var noof_tyres = $(this).find(".tyreno").val();
                    var axlesno = $(this).find(".axlesno").html();
                    var next_row = $(this).next();
                    var tble = $(next_row).find("[name='tyre_table']");
                    if (axilname != "") {
                        $(next_row).find('.responsive-table > tbody > tr').each(function () {
                            $(this).find('.right').each(function (i, obj) {
                                var right_tyresno = $(this).find('[name="tyresno"]').val();
                                var right_tyre_position_sno = $(this).find('.right_tyre_position_sno').html();
                                if (right_tyresno == null || right_tyresno == "Select Tyre") {
                                    leftalert = "Please Select All the Right Side Tyres Properly";
                                }
                                innertable_array.push({ 'tyre_sno': right_tyresno, 'Side': "R", 'tyre_position_sno': right_tyre_position_sno });
                            });
                            $(this).find('.left').each(function (i, obj) {
                                var left_tyresno = $(this).find('[name="tyresno"]').val();
                                var left_tyre_position_sno = $(this).find('.left_tyre_position_sno').html();

                                if (left_tyresno == null || left_tyresno == "Select Tyre") {
                                    rightalert = "Please Select All the Left Side Tyres Properly";
                                }
                                innertable_array.push({ 'tyre_sno': left_tyresno, 'Side': "L", 'tyre_position_sno': left_tyre_position_sno });
                            });
                        });
                        var step_tyre_name = $(next_row).find('[name=step_tyre_name]').val();
                        var step_tyresno = $(next_row).find('[name=tyresno]').val();
                        var step_tyre_position_sno = $(next_row).find('.step_tyre_position_sno').html();
                        if (step_tyre_name == "Stephanie") {
                            //                            if (step_tyresno == null || step_tyresno == "Select Tyre") {
                            //                                stepalert = "Please Select Stephanie Tyre Properly";
                            //                            }
                            innertable_array.push({ 'tyre_sno': step_tyresno, 'Side': "S", 'tyre_position_sno': step_tyre_position_sno });
                        }
                        axil_table.push({ 'axilname': axilname, 'noof_tyres': noof_tyres, 'axlesno': axlesno, 'innertable_array': innertable_array });
                    }
                });
                if (leftalert != "") {
                    alert(leftalert);
                    return;
                }
                if (rightalert != "") {
                    alert(rightalert);
                    return;
                }
                if (stepalert != "") {
                    alert(stepalert);
                    return;
                }
                var data = { 'op': 'Axils_save_start' };
                var s = function (msg) {
                    if (msg) {
                        for (var i = 0; i < axil_table.length; i++) {
                            var Data = { 'op': 'Axils_save_RowData', 'row_detail': axil_table[i], 'end': 'N' };
                            if (i == axil_table.length - 1) {
                                Data = { 'op': 'Axils_save_RowData', 'row_detail': axil_table[i], 'end': 'Y' };
                            }
                            var s = function (msg) {
                                if (msg == 'Y') {
                                    var Data = { 'op': 'save_vehicle_master', 'regno': regno, 'doorno': doorno, 'vehtype': vehtype, 'sno': sno, 'btnval': btnval, 'status': status, 'Capacity': Capacity,
                                        'model': model, 'engineno': engineno, 'chasiss': chasiss, 'owner': owner, 'address': address, 'rcno': rcno, 'rcexp_date': rcexp_date, 'pollution': pollution,
                                        'pollexp_date': pollexp_date, 'insurence': insurence, 'insexp_date': insexp_date, 'fitness': fitness, 'fitexp_date': fitexp_date, 'roadtax': roadtax,
                                        'roadtaxexp_date': roadtaxexp_date, 'permitstatename': permitstatename, 'permit_state_expdate': permit_state_expdate, 'stateroadtax': stateroadtax, 'stateroadtaxexp_date': stateroadtaxexp_date, 'vehicle_make': vehicle_make, 'fuel_capacity': fuel_capacity, 'axilmaster': axilmaster, 'InsuranceSno': slctinsurancecompany, 'Odometer': Odometer, 'Actualmileage': Actualmileage, 'Batteryno': Batteryno, 'Batteryno2': Batteryno2, 'axil_name_check': axil_name_check
                                    };
                                    var s = function (msg) {
                                        if (msg) {
                                            if (msg.length > 0) {
                                                if (msg == "OK") {
                                                    alert("New Vehicle Master Successfully Added");
                                                    $('#vehmaster_fillform').css('display', 'none');
                                                    $('#vehmaster_showlogs').css('display', 'block');
                                                    $('#div_vehmaster_table').css('display', 'block');
                                                    $('#save_vehmaster').val("Save");
                                                    clearall();
                                                    retrivedata();
                                                    hiding_error();
                                                    get_tyresdata();

                                                }
                                                else if (msg == "UPDATE") {
                                                    alert("Vehicle Master Successfully Updated");
                                                    $('#vehmaster_fillform').css('display', 'none');
                                                    $('#vehmaster_showlogs').css('display', 'block');
                                                    $('#div_vehmaster_table').css('display', 'block');
                                                    $('#save_vehmaster').val("Save");
                                                    retrivedata();
                                                    clearall();
                                                    hiding_error();
                                                    get_tyresdata();
                                                }
                                                else {
                                                    alert(msg);
                                                }
                                            }
                                            else {

                                            }
                                        }
                                    }
                                    var e = function (x, h, e) {
                                    };
                                    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
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
            else {
                var Data = { 'op': 'save_vehicle_master', 'regno': regno, 'doorno': doorno, 'vehtype': vehtype, 'sno': sno, 'btnval': btnval, 'status': status, 'Capacity': Capacity,
                    'model': model, 'engineno': engineno, 'chasiss': chasiss, 'owner': owner, 'address': address, 'rcno': rcno, 'rcexp_date': rcexp_date, 'pollution': pollution,
                    'pollexp_date': pollexp_date, 'insurence': insurence, 'insexp_date': insexp_date, 'fitness': fitness, 'fitexp_date': fitexp_date, 'roadtax': roadtax,
                    'roadtaxexp_date': roadtaxexp_date, 'permitstatename': permitstatename, 'permit_state_expdate': permit_state_expdate, 'stateroadtax': stateroadtax, 'stateroadtaxexp_date': stateroadtaxexp_date, 'vehicle_make': vehicle_make, 'fuel_capacity': fuel_capacity, 'axilmaster': axilmaster, 'InsuranceSno': slctinsurancecompany, 'Odometer': Odometer, 'Actualmileage': Actualmileage, 'Batteryno': Batteryno,'Batteryno2':Batteryno2, 'axil_name_check': axil_name_check
                };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            if (msg == "OK") {
                                alert("New Vehicle Master Successfully Added");
                                $('#vehmaster_fillform').css('display', 'none');
                                $('#vehmaster_showlogs').css('display', 'block');
                                $('#div_vehmaster_table').css('display', 'block');
//                                $('#save_vehmaster').val("Save");
                                clearall();
                                retrivedata();
                                hiding_error();

                            }
                            else if (msg == "UPDATE") {
                                alert("Vehicle Master Successfully Updated");
                                $('#vehmaster_fillform').css('display', 'none');
                                $('#vehmaster_showlogs').css('display', 'block');
                                $('#div_vehmaster_table').css('display', 'block');
//                                $('#save_vehmaster').val("Save");
                                retrivedata();
                                clearall();
                                hiding_error();
                            }
                            else {
                                alert(msg);
                            }
                        }
                        else {
                        }
                    }
                }
                var e = function (x, h, e) {
                };
                $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                callHandler(Data, s, e);
            }
        }
        var VehicleDetails = [];
        function retrivedata() {
            var table = document.getElementById("tbl_vehmaster");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var data = { 'op': 'get_all_veh_master_data' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filldata(msg);
                        fillVehicledetails(msg);
                        VehicleDetails = msg;
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
        var vehicleList = [];
        function fillVehicledetails(msg) {
            for (var i = 0; i < msg.length; i++) {
                var registration_no = msg[i].registration_no;
                vehicleList.push(registration_no);
            }
            $('#txt_Vehicle1').autocomplete({
                source: vehicleList,
                change: vehiclenochange,
                autoFocus: true
            });
        }
        function vehiclenochange() {
            var results = VehicleDetails;
            var vehicleno = document.getElementById("txt_Vehicle1").value;
            var table = document.getElementById("tbl_vehmaster");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var k = 0;
            var colorue = ["#d1ece3", "#fff6e9", "#d7e0e0", "#ffe7e5", "Bisque"];
            for (var i = 0; i < results.length; i++) {
                if (vehicleno == results[i].registration_no) {
                    if (results[i].VehType != null) {
                        var VehType = results[i].VehType;
                        var registration_no = results[i].registration_no;
                        var door_no = results[i].door_no;
                        var capcity = results[i].Capacity;
                        var statuscode = results[i].status;
                        var vehmake = results[i].VehMake;
                        var fuelcapacity = results[i].v_ty_fuel_capacity;
                        var axil_name = results[i].axils_refno;
                        var axilmstr_name = results[i].axilmaster_name;
                        var act_mileage = results[i].act_mileage;
                        var insurancesno = results[i].insurancesno;
                        var permitstatename = results[i].permitstatename;
                        var permit_state_expdate = results[i].permit_state_expdate;
                        var stateroadtax = results[i].stateroadtax;
                        var stateroadtaxexp_date = results[i].stateroadtaxexp_date;
                        var batteryno = results[i].batteryno;
                        var batteryno2 = results[i].batteryno2;
                        var ftp = results[i].ftplocation;
                        var imageloc = results[i].imagename;
                        var rndmnum = Math.floor((Math.random() * 10) + 1);
                        img_url = ftp + imageloc + '?v=' + rndmnum;
                        var status = "";
                        if (statuscode == "True") {
                            status = "Enabled";
                        }
                        else {
                            status = "Disabled";
                        }
                        var vm_sno = results[i].vm_sno;
                        var tablerowcnt = document.getElementById("tbl_vehmaster").rows.length;
                        $('#tbl_vehmaster').append('<tr style="background-color:' + colorue[k] + '"><th scope="row">' + registration_no + '</th><td data-title="Vehicle Type">' + VehType + '</td><td data-title="Door No" >' + door_no + '</td><td data-title="Status" >' + status + '</td><td data-title="sno" style="display:none;">' + vm_sno + '</td><td data-title="Capacity">' + capcity + '</td><td data-title="Vehicle Make">' + vehmake + '</td><td data-title="Fuel Capacity">' + fuelcapacity + '</td><td data-title="AxilName_sno" style="display: none;">' + axil_name + '</td><td data-title="AxilName" >' + axilmstr_name + '</td><td data-title="act_mileage" style="display:none;">' + act_mileage + '</td><td data-title="insurancesno" style="display:none;">' + insurancesno + '</td><td data-title="permitstatename" style="display:none;">' + permitstatename + '</td><td data-title="permit_state_expdate" style="display:none;">' + permit_state_expdate + '</td><td data-title="stateroadtax" style="display:none;">' + stateroadtax + '</td><td data-title="stateroadtaxexp_date" style="display:none;">' + stateroadtaxexp_date + '</td><td data-title="stateroadtaxexp_date" style="display:none;">' + batteryno + '</td><td data-title="stateroadtaxexp_date" style="display:none;">' + batteryno2 + '</td><td data-title="stateroadtaxexp_date" style="display:none;">' + img_url + '</td><td><input type="button" class="btn btn-primary" name="Update" value ="View" onclick="updateclick(this);"/></td></tr>');
                        k = k + 1;
                        if (k == 4) {
                            k = 0;
                        }
                    }
                }
            }
        }
        var img_url;
        function filldata(results) {
            var k = 0;
            var colorue = ["#d1ece3", "#fff6e9", "#d7e0e0", "#ffe7e5", "Bisque"];
            var table = document.getElementById("tbl_vehmaster");
            for (var i = 0; i < results.length; i++) {
                if (results[i].VehType != null) {
                    var VehType = results[i].VehType;
                    var registration_no = results[i].registration_no;
                    var door_no = results[i].door_no;
                    var capcity = results[i].Capacity;
                    var statuscode = results[i].status;
                    var vehmake = results[i].VehMake;
                    var fuelcapacity = results[i].v_ty_fuel_capacity;
                    var axil_name = results[i].axils_refno;
                    var axilmstr_name = results[i].axilmaster_name;
                    var act_mileage = results[i].act_mileage;
                    var insurancesno = results[i].insurancesno;
                    var permitstatename = results[i].permitstatename;
                    var permit_state_expdate = results[i].permit_state_expdate;
                    var stateroadtax = results[i].stateroadtax;
                    var stateroadtaxexp_date = results[i].stateroadtaxexp_date;
                    var batteryno = results[i].batteryno;
                    var batteryno2 = results[i].batteryno2;
                    var ftp = results[i].ftplocation;
                    var imageloc = results[i].imagename;
                    var rndmnum = Math.floor((Math.random() * 10) + 1);
                    img_url = ftp + imageloc + '?v=' + rndmnum;
                    var status = "";
                    if (statuscode == "True") {
                        status = "Enabled";
                    }
                    else {
                        status = "Disabled";
                    }
                    var vm_sno = results[i].vm_sno;
                    var tablerowcnt = document.getElementById("tbl_vehmaster").rows.length;
                    $('#tbl_vehmaster').append('<tr style="background-color:' + colorue[k] + '"><th scope="row">' + registration_no + '</th><td data-title="Vehicle Type">' + VehType + '</td><td data-title="Door No" >' + door_no + '</td><td data-title="Status" >' + status + '</td><td data-title="sno" style="display:none;">' + vm_sno + '</td><td data-title="Capacity">' + capcity + '</td><td data-title="Vehicle Make">' + vehmake + '</td><td data-title="Fuel Capacity">' + fuelcapacity + '</td><td data-title="AxilName_sno" style="display: none;">' + axil_name + '</td><td data-title="AxilName" >' + axilmstr_name + '</td><td data-title="act_mileage" style="display:none;">' + act_mileage + '</td><td data-title="insurancesno" style="display:none;">' + insurancesno + '</td><td data-title="permitstatename" style="display:none;">' + permitstatename + '</td><td data-title="permit_state_expdate" style="display:none;">' + permit_state_expdate + '</td><td data-title="stateroadtax" style="display:none;">' + stateroadtax + '</td><td data-title="stateroadtaxexp_date" style="display:none;">' + stateroadtaxexp_date + '</td><td data-title="stateroadtaxexp_date" style="display:none;">' + batteryno + '</td><td data-title="stateroadtaxexp_date" style="display:none;">' + batteryno2 + '</td><td data-title="stateroadtaxexp_date" style="display:none;">' + img_url + '</td><td><input type="button" class="btn btn-primary" name="Update" value ="View" onclick="updateclick(this);"/></td></tr>');
                    k = k + 1;
                    if (k == 4) {
                        k = 0;
                    }
                }
            }
        }
        var axilname_sno = "";
        var Veh_sno = "";
        var vehiclesno = "";
        var reg_no = "";
        function updateclick(thisid) {
            axilname_sno = "";
            var row = $(thisid).parents('tr');
            Veh_sno = row[0].cells[4].innerHTML;
            var registration_no = row[0].cells[0].innerHTML;
            vehiclesno = Veh_sno;
            reg_no = row[0].cells[0].innerHTML;
            var VehType = row[0].cells[1].innerHTML;
            var door_no = row[0].cells[2].innerHTML;
            var statuscode = row[0].cells[3].innerHTML;
            var vm_sno = row[0].cells[4].innerHTML;
            var capacity = row[0].cells[5].innerHTML;
            var vehmake = row[0].cells[6].innerHTML;
            var fuelcapacity = row[0].cells[7].innerHTML;
            var axilname = row[0].cells[8].innerHTML;
            axilname_sno = row[0].cells[8].innerHTML;
            var act_mileage = row[0].cells[10].innerHTML;
            var insurancesno = row[0].cells[11].innerHTML;
            var batteryno = row[0].cells[16].innerHTML;
            var batteryno2 = row[0].cells[17].innerHTML;
            var imageurl = row[0].cells[18].innerHTML;
            var status = "";
            if (statuscode == "Enabled") {
                status = "True";
            }
            else {
                status = "False";
            }
            document.getElementById('lbl_topempname').innerHTML = row[0].cells[0].innerHTML; ;
            document.getElementById('lbl_topemployeeid').innerHTML = row[0].cells[1].innerHTML;
            document.getElementById('lbl_topempemailid').innerHTML = row[0].cells[6].innerHTML;
            document.getElementById('lbl_topempmobno').innerHTML = row[0].cells[5].innerHTML;
            $("select#slct_vehtype option").each(function () { this.selected = (this.text == VehType); });
            $("select#slct_vehiclemake option").each(function () { this.selected = (this.text == vehmake); });
            document.getElementById('txt_regno').value = registration_no;
            document.getElementById('txt_sno').value = vm_sno;
            document.getElementById('txt_doorno').value = door_no;
            document.getElementById('cmb_status').value = status;
            document.getElementById('txt_capacity').value = capacity;
            document.getElementById('txt_fuelcapacity').value = fuelcapacity;
            document.getElementById('txt_actualmileage').value = act_mileage;
            document.getElementById('slct_insurancecompany').value = insurancesno;
            document.getElementById('ddlBatteryno').value = batteryno;
            document.getElementById('ddlBatteryno2').value = batteryno2;
            document.getElementById('txt_permitstatename').value = row[0].cells[12].innerHTML;
            document.getElementById('txt_permit_state_expdate').value = row[0].cells[13].innerHTML;
            document.getElementById('txt_stateroadtax').value = row[0].cells[14].innerHTML;
            document.getElementById('txt_stateroadtaxexp_date').value = row[0].cells[15].innerHTML;
            //$('#odometer_div').hide();
            getremaining_veh_data();
            $('#vehmaster_fillform').css('display', 'block');
            $('#vehmaster_showlogs').css('display', 'none');
            $('#div_vehmaster_table').css('display', 'none');
//            $('#save_vehmaster').val("Modify");
            $('.hiddenrow').show();
            if (axilname != "") {
                axil_name_check = "Done";
                $('#tyre_ass_done').show();
                $('#tyre_assign').hide();
                get_tyrenames();
            }
            else {
                axil_name_check = "NotDone";
                $('#tyre_ass_done').hide();
                $('#tyre_assign').show();
                document.getElementById('tyre_check').checked = false;
                document.getElementById('slct_axilmaster').disabled = true;
                document.getElementById('slct_axilmaster').value = "Select Axil Master";
                document.getElementById('txt_odometer').disabled = true;
                document.getElementById('txt_odometer').value = "";
                getall_axilnames();
            }

            if (imageurl != "") {
                $('#main_img').attr('src', imageurl).width(155).height(200);
            }
            else {
                $('#main_img').attr('src', 'Images/Employeeimg.jpg').width(200).height(200);
            }
            getvehicle_Uploaded_Documents(Veh_sno);
            change_Personal();
        }
        var TyreInformation = [];
        function get_tyrenames() {
            $('#div_axilautofill').show();
            var vehmstr_sno = document.getElementById('txt_sno').value;
            var data = { 'op': 'get_filled_data_tyres_vehmstr', 'sno': axilname_sno, 'vehmstr_sno': vehmstr_sno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        TyreInformation = msg;
                        var table = document.getElementById("tbl_axils");
                        for (var i = table.rows.length - 1; i > 0; i--) {
                            table.deleteRow(i);
                        }
                        var main_results = "";
                        var right = "";
                        var left = "";
                        document.getElementById("lblvehicle").innerHTML = document.getElementById('txt_regno').value;
                        for (var i = 0; i < msg.length; i++) {
                            //$("#tbl_axils").append('<tr><th scope="row" style="border: 1px solid #1d96b2;">' + (i + 1) + '</th><td style="border: 1px solid #1d96b2;" data-title="Axil Name"><input type="text" placeholder="Axle Name" value="' + msg[i].AxileName + '" class="axilname"/></td><td style="border: 1px solid #1d96b2;" data-title="No.of Tyres"><input step="2" min="0" type="number" onblur="onchange_row(this)" value="' + msg[i].nooftyresperaxle + '" class="tyreno"/></td><td style="display:none;">' + msg[i].veh_typ_axel_sno + '</td></tr><tr class="hide_row" style="display:none;"><th colspan="4" scope="row"><div class="sub_td" style="float:right;"></div></th></tr>');
                            right = "";
                            left = "";
                            if (msg[i].AxileName != "Stephanie") {
                                main_results += '<tr><th scope="row" style="border: 1px solid #1d96b2;">' + (i + 1) + '</th><td style="border: 1px solid #1d96b2;" data-title="Axil Name"><label>' + msg[i].AxileName + '</label></td><td style="border: 1px solid #1d96b2;" data-title="No.of Tyres"><label>' + msg[i].nooftyresperaxle + ' </label></td><td style="display:none;"><label class="axlesno">' + msg[i].veh_typ_axel_sno + '</label></td></tr><tr class="hide_row"><th colspan="4" scope="row"><div class="sub_td">';
                                // main_results += '<tr><th scope="row" style="border: 1px solid #1d96b2;">' + (i + 1) + '</th><td style="border: 1px solid #1d96b2;" data-title="Axil Name"><input type="text" placeholder="Axle Name" value="' + msg[i].AxileName + '" class="axilname"/></td><td style="border: 1px solid #1d96b2;" data-title="No.of Tyres"><input step="2" min="0" type="number" onblur="onchange_row(this)" value="' + msg[i].nooftyresperaxle + '" class="tyreno"/></td><td style="display:none;"><label class="axlesno">' + msg[i].veh_typ_axel_sno + '</label></td></tr><tr class="hide_row"><th colspan="4" scope="row"><div class="sub_td" style="float:right;">';
                                main_results += '<table id="tbl_' + msg[i].AxileName + '" name="tyre_table"  class="responsive-table"><thead><tr><th scope="col" align="center">Left</th><th scope="col" align="center">Right</th></tr></thead><tbody>';
                                left += '<td data-title="Left Tyre">';
                                right += '<td data-title="Right Tyre" style="text-align: right;">';
                                for (var j = 0; j < msg[i].tyredata.length; j++) {
                                    if (msg[i].tyredata[j].side == "L") {
                                        var SVDSno = msg[i].tyredata[j].SVDSNo;
                                        left += '<div class="right" style="float:left;"><div class="tyreimage" onmouseover=tyreclick(this) onmouseout=bpmouseout()  id="' + SVDSno + '"><img src="images/tyre.jpg"  /></div><label style="font-size: 12px;">' + msg[i].tyredata[j].tyre_name + '</label></br><label >' + msg[i].tyredata[j].grove + '</label></br><label style="font-size: 12px;">' + msg[i].tyredata[j].SVDSNo + '</label></br><label name="tyresno" class="label label-default" style="font-size: 12px;">' + msg[i].tyredata[j].Tyre_Name + '</label><label class="right_tyre_position_sno" style="display:none;">' + msg[i].tyredata[j].tyre_position_sno + '</label></div>';

                                    }
                                    if (msg[i].tyredata[j].side == "R") {
                                        var SVDSno = msg[i].tyredata[j].SVDSNo;
                                        right += '<div class="left" style="float:right;"><div class="tyreimage" onmouseover=tyreclick(this) onmouseout=bpmouseout() id="' + SVDSno + '"><img src="images/tyre.jpg"  /></div><label style="font-size: 12px;">' + msg[i].tyredata[j].tyre_name + '</label></br><label >' + msg[i].tyredata[j].grove + '</label></br><label style="font-size: 12px;">' + msg[i].tyredata[j].SVDSNo + '</label></br><label name="tyresno" class="label label-default" style="font-size: 12px;">' + msg[i].tyredata[j].Tyre_Name + '</label><label class="left_tyre_position_sno" style="display:none;" >' + msg[i].tyredata[j].tyre_position_sno + '</label></div>';
                                    }
                                }
                                right += '</td>';
                                left += '</td>';
                                main_results += '<tr>' + left + '' + right + '</tr>';
                                main_results += '</tbody></table>';
                                main_results += '</div></th></tr>';
                            }
                            if (msg[i].AxileName == "Stephanie") {
                                main_results += '<tr><th scope="row" style="border: 1px solid #1d96b2;">' + (i + 1) + '</th><td style="border: 1px solid #1d96b2;" data-title="Axil Name"><input type="text" placeholder="Axle Name" value="Stephanie" disabled class="axilname"/></td><td style="border: 1px solid #1d96b2;" data-title="No.of Tyres"><input step="1" value="1" disabled min="0" type="number" class="tyreno"/></td><td style="display:none;"><label class="axlesno">' + msg[i].veh_typ_axel_sno + '</label></td></tr><tr><th colspan="4" scope="row"><div class="sub_td" style="float:right;">';
                                for (var n = 0; n < msg[i].tyredata.length; n++) {
                                    if (msg[i].tyredata[n].side == "S") {
                                        var SVDSno = msg[i].tyredata[n].SVDSNo;
                                        main_results += '<div class="stephanie" style="float:left;"><div class="tyreimage" onmouseover=tyreclick(this) onmouseout=bpmouseout() id="' + SVDSno + '"><img src="images/tyre.jpg" /></div><label style="font-size: 12px;">' + msg[i].tyredata[n].tyre_name + '</label></br><label >' + msg[i].tyredata[n].grove + '</label></br><label style="font-size: 12px;">' + msg[i].tyredata[n].SVDSNo + '</label></br><label name="tyresno" class="label label-default" style="font-size: 12px;">' + msg[i].tyredata[n].Tyre_Name + '</label><label class="left_tyre_position_sno" style="display:none;" >' + msg[i].tyredata[n].tyre_position_sno + '</label></div></br>';
                                    }
                                }
                                main_results += '</div></th></tr>';
                            }
                        }
                        $("#tbl_axils").append(main_results);
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
        function bpmouseout() {
            $("#displaydiv").css("display", "none"); $("#displaydiv").html("");
        }
        function tyreclick(ID) {
            var valu = $('#displaydiv');
            $('#displaydiv').css("display", "block");
            var Tyreno = ID.id;
            var pos = $(ID).offset();
            var content = "";
            for (var i = 0; i < TyreInformation.length; i++) {
                for (var n = 0; n < TyreInformation[i].tyredata.length; n++) {
                    if (TyreInformation[i].tyredata[n].SVDSNo == Tyreno) {
                        content += "SVDSNo : " + TyreInformation[i].tyredata[n].SVDSNo + "<br/>" + "Position : " + TyreInformation[i].tyredata[n].tyre_name + "<br/>" + "Brand : " + TyreInformation[i].tyredata[n].Brand + "<br/>" + "Grove : " + TyreInformation[i].tyredata[n].grove + "<br/>" + "TyreNo : " + TyreInformation[i].tyredata[n].Tyre_Name + "<br/>" + "Tyre Size : " + TyreInformation[i].tyredata[n].tyresize;
                    }
                }
            }
            $("#displaydiv").html(content);

            var top = $(document).scrollTop();
            var tothei = $(document).height();
            var xx = window.screen.availHeight;
            var aa = pos.top;
            var zz = aa - 50;
            if ((top + xx) >= (pos.top + $('#displaydiv').height() + (-300) + ($('#displaydiv').height() * 0.5))) {
                $('#displaydiv').css("top", zz).css("left", pos.left - 300);
            }
            else {
                if ((pos.top - $('#displaydiv').height()) < 0) {
                    $('#displaydiv').css("top", "0").css("left", pos.left - 300);
                }
                else {
                    $('#displaydiv').css("top", pos.top - $('#displaydiv').height() - 300).css("left", pos.left - 300);
                }
            }
        }
        function getremaining_veh_data() {
            var vm_sno = document.getElementById('txt_sno').value;
            var data = { 'op': 'getremaining_veh_data', 'vm_sno': vm_sno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        document.getElementById('txt_model').value = msg[0].vm_model;
                        document.getElementById('txt_engineno').value = msg[0].vm_engine;
                        document.getElementById('txt_chasiss').value = msg[0].vm_chasiss;
                        document.getElementById('txt_owner').value = msg[0].vm_owner;
                        document.getElementById('txt_address').value = msg[0].vm_owneraddr;
                        $("select#txt_rcno option").each(function () { this.selected = (this.text == msg[0].vm_rcno); });
                        document.getElementById('txt_rcexp_date').value = msg[0].vm_rcexpdate;
                        document.getElementById('txt_pollution').value = msg[0].vm_pollution;
                        document.getElementById('txt_pollexp_date').value = msg[0].vm_poll_exp_date;
                        document.getElementById('txt_insurence').value = msg[0].vm_insurance;
                        document.getElementById('txt_insexp_date').value = msg[0].vm_isurence_exp_date;
                        document.getElementById('txt_fitness').value = msg[0].vm_fitness;
                        document.getElementById('txt_fitexp_date').value = msg[0].vm_fit_exp_date;
                        document.getElementById('txt_roadtax').value = msg[0].vm_roadtax;
                        document.getElementById('txt_roadtaxexp_date').value = msg[0].vm_roadtax_exp_date;
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
        function clearall() {
            document.getElementById('txt_regno').value = "";
            document.getElementById('slct_vehtype').selectedIndex = 0;
            document.getElementById('slct_vehiclemake').selectedIndex = 0;
            document.getElementById('txt_capacity').value = "";
            document.getElementById('txt_doorno').value = "";
            document.getElementById('txt_sno').value = "";
            document.getElementById('txt_actualmileage').value = "";
//            document.getElementById('save_vehmaster').value = "Save";
            document.getElementById('cmb_status').selectedIndex = 0;
            $('#div_axilautofill').hide();
            var table = document.getElementById("tbl_axils");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            document.getElementById('txt_odometer').value = "";
            document.getElementById('txt_model').value = "";
            document.getElementById('txt_engineno').value = "";
            document.getElementById('txt_chasiss').value = "";
            document.getElementById('txt_owner').value = "";
            document.getElementById('txt_address').value = "";
            document.getElementById('txt_rcno').value = "Local";
            document.getElementById('txt_rcexp_date').value = "";
            document.getElementById('txt_pollution').value = "";
            document.getElementById('txt_pollexp_date').value = "";
            document.getElementById('txt_insurence').value = "";
            document.getElementById('txt_insexp_date').value = "";
            document.getElementById('txt_fitness').value = "";
            document.getElementById('txt_fitexp_date').value = "";
            document.getElementById('txt_roadtax').value = "";
            document.getElementById('txt_roadtaxexp_date').value = "";
            document.getElementById('txt_fuelcapacity').value = "";
            document.getElementById('tyre_check').checked = false;
            document.getElementById('slct_axilmaster').disabled = true;
            document.getElementById('slct_axilmaster').value = "Select Axil Master";
            document.getElementById('txt_odometer').disabled = true;
            document.getElementById('txt_odometer').value = "";
            document.getElementById('ddlBatteryno').selectedIndex = 0;
            document.getElementById('ddlBatteryno2').selectedIndex = 0;
            $('#main_img').attr('src', 'Images/Employeeimg.jpg').width(155).height(200);
            document.getElementById('lbl_topempname').innerHTML = "";
            document.getElementById('lbl_topemployeeid').innerHTML = "";
            document.getElementById('lbl_topempemailid').innerHTML = "";
            document.getElementById('lbl_topempmobno').innerHTML = "";

            $('#tyre_ass_done').hide();
        }
        var tyres_data = [];
        function get_tyresdata() {
            var data = { 'op': 'get_only_tyre_data' };
            var s = function (msg) {
                if (msg) {
                    options_val = "<option value='Select Tyre' selected disabled>Select Tyre</option>";
                    if (msg.length > 0) {
                        tyres_data = [];
                        for (var i = 0; i < msg.length; i++) {
                            if (msg[i].tyre_sno != null && msg[i].status != "0") {
                                options_val += "<option value='" + msg[i].sno + "'>" + msg[i].tyre_sno + "</option>";
                                tyres_data.push({ label: msg[i].tyre_sno, id: msg[i].sno });
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
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function vehtype_onchange() {
            $('#div_axilautofill').show();
            var sno = document.getElementById('slct_axilmaster').value;
            var data = { 'op': 'get_all_data_Axils', 'sno': sno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        var table = document.getElementById("tbl_axils");
                        for (var i = table.rows.length - 1; i > 0; i--) {
                            table.deleteRow(i);
                        }
                        var main_results = "";
                        var right = "";
                        var left = "";
                        for (var i = 0; i < msg.length; i++) {
                            //$("#tbl_axils").append('<tr><th scope="row" style="border: 1px solid #1d96b2;">' + (i + 1) + '</th><td style="border: 1px solid #1d96b2;" data-title="Axil Name"><input type="text" placeholder="Axle Name" value="' + msg[i].AxileName + '" class="axilname"/></td><td style="border: 1px solid #1d96b2;" data-title="No.of Tyres"><input step="2" min="0" type="number" onblur="onchange_row(this)" value="' + msg[i].nooftyresperaxle + '" class="tyreno"/></td><td style="display:none;">' + msg[i].veh_typ_axel_sno + '</td></tr><tr class="hide_row" style="display:none;"><th colspan="4" scope="row"><div class="sub_td" style="float:right;"></div></th></tr>');
                            right = "";
                            left = "";
                            if (msg[i].AxileName != "Stephanie") {
                                main_results += '<tr><th scope="row" style="border: 1px solid #1d96b2;">' + (i + 1) + '</th><td style="border: 1px solid #1d96b2;" data-title="Axil Name"><label>' + msg[i].AxileName + '</label></td><td style="border: 1px solid #1d96b2;" data-title="No.of Tyres"><label>' + msg[i].nooftyresperaxle + ' </label></td><td style="display:none;"><label class="axlesno">' + msg[i].veh_typ_axel_sno + '</label></td></tr><tr class="hide_row"><th colspan="4" scope="row"><div class="sub_td" style="float:right;">';
                                // main_results += '<tr><th scope="row" style="border: 1px solid #1d96b2;">' + (i + 1) + '</th><td style="border: 1px solid #1d96b2;" data-title="Axil Name"><input type="text" placeholder="Axle Name" value="' + msg[i].AxileName + '" class="axilname"/></td><td style="border: 1px solid #1d96b2;" data-title="No.of Tyres"><input step="2" min="0" type="number" onblur="onchange_row(this)" value="' + msg[i].nooftyresperaxle + '" class="tyreno"/></td><td style="display:none;"><label class="axlesno">' + msg[i].veh_typ_axel_sno + '</label></td></tr><tr class="hide_row"><th colspan="4" scope="row"><div class="sub_td" style="float:right;">';
                                main_results += '<table id="tbl_' + msg[i].AxileName + '" name="tyre_table"  class="responsive-table"><thead><tr><th scope="col">Right</th><th scope="col">Left</th></tr></thead><tbody>';
                                right += '<td data-title="Right Tyre">';
                                left += '<td data-title="Left Tyre">';
                                for (var j = 0; j < msg[i].tyredata.length; j++) {
                                    if (msg[i].tyredata[j].side == "R") {
                                        right += '<div class="right"><label>' + msg[i].tyredata[j].tyre_name + '</label></br><select name="tyresno" class="form-control">' + options_val + '</select><label class="right_tyre_position_sno" style="display:none;">' + msg[i].tyredata[j].tyre_position_sno + '</label></div></br>';
                                    }
                                    if (msg[i].tyredata[j].side == "L") {
                                        left += '<div class="left"><label>' + msg[i].tyredata[j].tyre_name + '</label></br><select name="tyresno" class="form-control">' + options_val + '</select><label class="left_tyre_position_sno" style="display:none;">' + msg[i].tyredata[j].tyre_position_sno + '</label></div></br>';
                                    }
                                }
                                right += '</td>';
                                left += '</td>';
                                main_results += '<tr>' + right + '' + left + '</tr>';
                                main_results += '</tbody></table>';
                                main_results += '</div></th></tr>';
                            }
                            if (msg[i].AxileName == "Stephanie") {
                                main_results += '<tr><th scope="row" style="border: 1px solid #1d96b2;">' + (i + 1) + '</th><td style="border: 1px solid #1d96b2;" data-title="Axil Name"><label>' + msg[i].AxileName + '</label></td><td style="border: 1px solid #1d96b2;" data-title="No.of Tyres"><label>' + msg[i].nooftyresperaxle + ' </label></td><td style="display:none;"><label class="axlesno">' + msg[i].veh_typ_axel_sno + '</label></td></tr><tr class="hide_row"><th colspan="4" scope="row"><div class="sub_td" style="float:right;">';
                                for (var n = 0; n < msg[i].tyredata.length; n++) {
                                    if (msg[i].tyredata[n].side == "S") {
                                        main_results += '<div class="stephanie"><input type="text" name="step_tyre_name" value="Stephanie" disabled class="form-control" placeholder="Step Tyre Name" /></br><label>' + msg[i].tyredata[n].tyresize + '</label></br><select name="tyresno" class="form-control">' + options_val + '</select><label class="step_tyre_position_sno" style="display:none;">' + msg[i].tyredata[n].tyre_position_sno + '</label></div></br>';
                                    }
                                }
                                main_results += '</div></th></tr>';
                            }
                        }
                        $("#tbl_axils").append(main_results);
                        hiden();
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
        function hiden() {
            $('select[name*="tyresno"]').change(function () {
                // start by setting everything to enabled
                $('select[name*="tyresno"] option').attr('disabled', false);
                // loop each select and set the selected value to disabled in all other selects
                $('select[name*="tyresno"]').each(function () {
                    var $this = $(this);
                    $('select[name*="tyresno"]').not($this).find('option').each(function () {
                        if ($(this).attr('value') == $this.val())
                            $(this).attr('disabled', true);
                    });
                });
            });
        }
        //Function for only no
        function only_no() {
            //$("#txt_fuelcapacity,#txt_axilno,.tyreno").keydown(function (event) {
            $("#txt_capacity,#txt_fuelcapacity").keydown(function (event) {
                // Allow: backspace, delete, tab, escape, and enter
                if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 || event.keyCode == 190 ||
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
        }
        function getall_axilnames() {
            var data = { 'op': 'get_all_axil_names' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        var vehtype = document.getElementById('slct_axilmaster');
                        var length = vehtype.options.length;
                        document.getElementById('slct_axilmaster').options.length = null;
                        var opt2 = document.createElement('option');
                        opt2.innerHTML = "Select Axil Master";
                        opt2.value = "Select Axil Master";
                        opt2.setAttribute("selected", "selected");
                        opt2.setAttribute("disabled", "disabled");
                        opt2.setAttribute("class", "dispalynone");
                        vehtype.appendChild(opt2);
                        for (var i = 0; i < msg.length; i++) {
                            if (msg[i].axilmaster_name != null) {
                                var option = document.createElement('option');
                                option.innerHTML = msg[i].axilmaster_name;
                                option.value = msg[i].sno;
                                vehtype.appendChild(option);
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
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function getall_axilnames_type() {
            var vehiclemake = document.getElementById('slct_vehiclemake').value;
            if (vehiclemake != "Select Vehicle Make") {
                getall_axilnames();
            }
            else {
            }
        }
        function checkbox_change() {
            if (document.getElementById('tyre_check').checked == true) {
                document.getElementById('slct_axilmaster').disabled = false;
                document.getElementById('txt_odometer').disabled = false;
            }
            else {
                document.getElementById('slct_axilmaster').disabled = true;
                document.getElementById('slct_axilmaster').value = "Select Axil Master";
                document.getElementById('txt_odometer').disabled = true;
                document.getElementById('txt_odometer').value = "";
            }
        }
        //        function GetDocuments_click() {
        //            $('#addexampopup').css('display', 'block');
        //            $('#hover').css('display', 'block');
        //            document.getElementById("rc_photo").src = "EmployeeImageHandler.ashx?sno=" + Veh_sno + " RCCopy";
        //        }

        ///////////////////////

        function change_Documents() {
            $("li").removeClass("active");
            $("li").addClass("");
            $("#id_tab_documents").removeClass("");
            $("#id_tab_documents").addClass("active");
            $("#div_basic_details").css("display", "none");
            $("#btn_modify").css("display", "none");
            $("#div_Documents").css("display", "block");
        }
        function change_Personal() {
            $("li").removeClass("active");
            $("li").addClass("");
            $("#id_tab_documents").removeClass("");
            $("#id_tab_documents").addClass("active");
            $("#div_basic_details").css("display", "block");
            $("#btn_modify").css("display", "block");
            $("#div_Documents").css("display", "none");
        }
        //        function btn_cancelexampopup_click() {
        //            $('#addexampopup').css('display', 'none');
        //            $('#hover').css('display', 'none');
        //        }
        //-------------> allow only required extention
        function hasExtension(fileName, exts) {
            return (new RegExp('(' + exts.join('|').replace(/\./g, '\\.') + ')$')).test(fileName);
        }

        function readURL(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('#main_img,#img_1').attr('src', e.target.result).width(155).height(200);
                    //                    $('#img_1').css('display', 'inline');
                };
                reader.readAsDataURL(input.files[0]);
            }
        }

        function getFile() {
            document.getElementById("file").click();
        }
        //----------------> convert base 64 to file
        function dataURItoBlob(dataURI) {
            // convert base64/URLEncoded data component to raw binary data held in a string
            var byteString;
            if (dataURI.split(',')[0].indexOf('base64') >= 0)
                byteString = atob(dataURI.split(',')[1]);
            else
                byteString = unescape(dataURI.split(',')[1]);
            // separate out the mime component
            var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];
            // write the bytes of the string to a typed array
            var ia = new Uint8Array(byteString.length);
            for (var i = 0; i < byteString.length; i++) {
                ia[i] = byteString.charCodeAt(i);
            }
            return new Blob([ia], { type: 'image/jpeg' });
        }
        function upload_profile_pic() {
            var dataURL = document.getElementById('main_img').src;
            var div_text = $('#yourBtn').text().trim();
            var blob = dataURItoBlob(dataURL);
            //            var empsno = 1;
            var Data = new FormData();
            Data.append("op", "Vehicle_pic_files_upload");
            Data.append("vehiclesno", vehiclesno);
            Data.append("Veh_sno", Veh_sno);

            Data.append("canvasImage", blob);
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    document.getElementById('btn_upload_profilepic').disabled = true;
                }
                else {
                    document.location = "Default.aspx";
                }
            };
            var e = function (x, h, e) {
            };
            callHandler_nojson_post(Data, s, e);
        }
        function callHandler_nojson_post(d, s, e) {
            $.ajax({
                url: 'FleetManagementHandler.axd',
                type: "POST",
                // dataType: "json",
                contentType: false,
                processData: false,
                data: d,
                success: s,
                error: e
            });
        }
        function getFile_doc() {
            document.getElementById("FileUpload1").click();
        }
        function readURL_doc(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.readAsDataURL(input.files[0]);
                document.getElementById("FileUpload_div").innerHTML = input.files[0].name;
            }
        }
        function upload_Vehicle_Document_Info() {
            var documentid = document.getElementById('ddl_documenttype').value;
            var documentname = document.getElementById('ddl_documenttype').selectedOptions[0].innerText;
            if (documentid == null || documentid == "" || documentid == "Select Document Type") {
                document.getElementById("ddl_documenttype").focus();
                alert("Please select Document Type");
                return false;
            }
            var documentExists = 0;
            $('#tbl_documents tr').each(function () {
                var selectedrow = $(this);
                var document_manager_id = selectedrow[0].cells[0].innerHTML;
                if (document_manager_id == documentid) {
                    alert(documentname + "  Already Exist For This Employee");
                    documentExists = 1;
                    return false;
                }

            });
            if (documentExists == 1) {
                return false;
            }
            var Data = new FormData();
            Data.append("op", "save_Vehicle_Document_Info");
            Data.append("vehiclesno", vehiclesno);
            Data.append("registration_no", reg_no);
            Data.append("documentname", documentname);
            Data.append("documentid", documentid);
            var fileUpload = $("#FileUpload1").get(0);
            var files = fileUpload.files;
            for (var i = 0; i < files.length; i++) {
                Data.append(files[i].name, files[i]);
            }

            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    getvehicle_Uploaded_Documents(vehiclesno);
                }
            };
            var e = function (x, h, e) {
                alert(e.toString());
            };
            callHandler_nojson_post(Data, s, e);
        }
        function getvehicle_Uploaded_Documents(Vehiclesno) {
            var data = { 'op': 'getvehicle_Uploaded_Documents', 'Vehiclesno': Vehiclesno };
            var s = function (msg) {
                if (msg) {
                    fillemployee_Uploaded_Documents(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillemployee_Uploaded_Documents(msg) {

            var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer">';
            results += '<thead><tr><th scope="col">Sno</th><th scope="col" style="text-align:center">Document Name</th><th scope="col">Photo</th><th scope="col">Download</th></tr></thead></tbody>';
            var k = 1;
            for (var i = 0; i < msg.length; i++) {
                results += '<tr><td>' + k++ + '</td>';
                var path = img_url = msg[i].ftplocation + msg[i].photo;
                var documentid = msg[i].documentid;
                var documentname = "";
                if (documentid == "1") {
                    documentname = "RCCopy";
                }
                if (documentid == "2") {
                    documentname = "Insurance";
                }
                if (documentid == "3") {
                    documentname = "APPermitt";
                }
                if (documentid == "4") {
                    documentname = "TNPermitt";
                }
                if (documentid == "5") {
                    documentname = "APRoadTax";
                }
                if (documentid == "6") {
                    documentname = "TNRoadTax";
                }
                if (documentid == "7") {
                    documentname = "FC";
                }
                if (documentid == "8") {
                    documentname = "Pollution";
                }
                if (documentid == "9") {
                    documentname = "Bilateraltax";
                }
                if (documentid == "10") {
                    documentname = "Authorisation";
                }
                if (documentid == "11") {
                    documentname = "NationalPermitt";
                }
                if (documentid == "12") {
                    documentname = "TSRoadTax";
                }
                if (documentid == "13") {
                    documentname = "TSPermitt";
                }
                
                results += '<th scope="row" class="1" style="text-align:center;">' + documentname + '</th>';
                results += '<td data-title="Code" class="2">' + msg[i].Status + '</td>';
                if (documentname == "Insurance") {
                    results += '<td data-title="brandstatus" class="2"><iframe src=' + path + ' style="width:400px; height:400px;" frameborder="0"></iframe></td>';
                }
                else {
                    results += '<td data-title="brandstatus" class="2"><img src=' + path + '  style="cursor:pointer;height:400px;width:400px;border-radius: 5px;"/></td>';
                }
                results += '<th scope="row" class="1" ><a  target="_blank" href=' + path + '><i class="fa fa-download" aria-hidden="true"></i> Download</a></th>';
                //results += '<td style="display:none" class="4">' + msg[i].Reason + '</td>';
                results += '</tr>';
            }
            results += '</table></div>';
            $("#div_documents_table").html(results);
        }
      
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Vehicle Master<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Vehicle Master</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Vehicle Master Details
                </h3>
            </div>
            <div class="box-body">
                <div id="vehmaster_showlogs" style="text-align: center;">
                    <table>
                        <tr>
                            <td>
                                <input id="txt_Vehicle1" type="text" style="height: 28px; opacity: 1.0; width: 180px;"
                                    class="form-control" placeholder="Search vehicle Number" />
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <i class="fa fa-search" aria-hidden="true">Search</i>
                            </td>
                            <td style="width: 500px">
                            </td>
                      <%--      <td>
                                <input id="add_partnumber" type="button" class="btn btn-primary" name="submit" value='Add Vehicle Master' />
                            </td>--%>
                        </tr>
                    </table>
                </div>
                <div id='vehmaster_fillform' style="display: none;">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="well panel panel-default" style="padding: 0px;">
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-sm-4" style="width: 100%;">
                                            <div class="row">
                                                <div class="col-xs-12 col-sm-3 text-center">
                                                    <div class="pictureArea1">
                                                        <img class="center-block img-circle img-thumbnail img-responsive profile-img" id="main_img"
                                                            src="Images/Employeeimg.jpg" alt="your image" style="width: 155px; height: 200px;
                                                            border-radius: 50%;" />
                                                        <%--<img id="prw_img" class="center-block img-circle img-thumbnail img-responsive profile-img" src="Images/Employeeimg.jpg" alt="your image" style="width: 150px; height: 150px;">--%>
                                                        <div class="photo-edit-admin">
                                                            <a onclick="getFile();" class="photo-edit-icon-admin" href="/employee/emp-master/emp-photo?eid=3"
                                                                title="Change Vehicle Picture" data-toggle="modal" data-target="#photoup"><i class="fa fa-pencil">
                                                                </i></a>
                                                        </div>
                                                        <div id="yourBtn" class="img_btn" onclick="getFile();" style="margin-top: 5px; display: none;">
                                                            Click to Choose Image
                                                        </div>
                                                        <div style="height: 0px; width: 0px; overflow: hidden;">
                                                            <input id="file" type="file" name="files[]" onchange="readURL(this);">
                                                        </div>
                                                        <div>
                                                            <input type="button" id="btn_upload_profilepic" class="btn btn-primary" onclick="upload_profile_pic();"
                                                                style="margin-top: 5px;" value="Upload Vehicle Pic">
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-xs-12 col-sm-9">
                                                    <h2 class="text-primary">
                                                        <b><span class="fa fa-truck"></span>
                                                            <label id="lbl_topempname">
                                                            </label>
                                                        </b>
                                                    </h2>
                                                    <p>
                                                        <strong>Vehicle Type : <span style="color: Red;">*</span></strong>
                                                        <label style="padding-left: 20px; font-weight: 700;" id="lbl_topemployeeid">
                                                        </label>
                                                    </p>
                                                    <p>
                                                        <strong>Vehicle Make : <span style="color: Red;">*</span></strong>
                                                        <label id="lbl_topempemailid">
                                                        </label>
                                                    </p>
                                                    <p>
                                                        <strong>Capacity :<span style="color: Red;">*</span> </strong>
                                                        <label id="lbl_topempmobno">
                                                        </label>
                                                    </p>
                                                </div>
                                                <!--/col-->
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div>
                        <ul class="nav nav-tabs">
                            <li id="id_tab_Personal" class="active"><a data-toggle="tab" href="#" onclick="change_Personal()">
                                <i class="fa fa-street-view"></i>&nbsp;&nbsp;Basic Details</a></li>
                            <li id="id_tab_documents" class=""><a data-toggle="tab" href="#" onclick="change_Documents()">
                                <i class="fa fa-file-text"></i>&nbsp;&nbsp;Vehicle Documents</a></li>
                        </ul>
                    </div>
                    <div style="text-align: center;">
                        <div id="div_basic_details" style="display: block;">
                            <table align="center" style="width: 90%;">
                                <tr>
                                    <td>
                                        <label>
                                            Reg No<span style="color: red;">*</span></label>
                                        <input id="txt_regno" type="text" class="form-control" name="vendorcode" placeholder="Registration Number"><label
                                            id="lbl_reg_error_msg" class="errormessage">* Please Enter Reg Name</label>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <label>
                                            Door No</label>
                                        <input id="txt_doorno" class="form-control" type="text" name="vendorcode" placeholder="Door Number">
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <label>
                                            Vehicle Type<span style="color: red;">*</span></label>
                                        <select id="slct_vehtype" class="form-control" style="min-width: 195px;">
                                            <option value="Select Vehicle Type" selected disabled>Select Vehicle Type</option>
                                        </select><label id="lbl_vehtype_error_msg" class="errormessage">* Please Selet Vehicle
                                            Type</label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Vehicle Make<span style="color: red;">*</span></label>
                                        <select id="slct_vehiclemake" class="form-control" style="min-width: 195px;">
                                            <option value="Select Vehicle Make" selected disabled>Select Vehicle Make</option>
                                        </select><label id="lbl_vehmake_error_msg" class="errormessage">* Please Selet Vehicle
                                            Make</label>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <label>
                                            Capacity(Ltrs)<span style="color: red;">*</span></label>
                                        <input id="txt_capacity" class="form-control" type="text" name="vendorcode" placeholder="Capacity" />
                                        <label id="lbl_capacity_error_msg" class="errormessage">
                                            * Please Enter Vehicle Capacity
                                        </label>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <label>
                                            Fuel Capacity(Ltrs)<span style="color: red;">*</span></label>
                                        <input id="txt_fuelcapacity" class="form-control" type="text" placeholder="Fuel Capacity" />
                                        <label id="lbl_fuelcapacity_error_msg" class="errormessage">
                                            * Please Enter Fuel Capacity
                                        </label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Status</label>
                                        <select id="cmb_status" class="form-control" style="min-width: 195px;">
                                            <option value="True">Enabled</option>
                                            <option value="False">Disabled</option>
                                        </select>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <label>
                                            Model</label>
                                        <input id="txt_model" type="text" class="form-control" name="vendorcode" placeholder="Model">
                                    </td>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <label>
                                            Engine No</label>
                                        <input id="txt_engineno" type="text" class="form-control" name="vendorcode" placeholder="Engine No" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Chassis No</label>
                                        <input id="txt_chasiss" type="text" class="form-control" name="vendorcode" placeholder="Chassis No" />
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <label>
                                            Owner</label>
                                        <%-- <input id="txt_owner" type="text" class="form-control" name="vendorcode" placeholder="Owner" />--%>
                                        <select id="txt_owner" class="form-control" style="min-width: 195px;">
                                            <option>SVDS</option>
                                            <option>SVF</option>
                                            <option>SLT</option>
                                            <option>RSNREDDY</option>
                                        </select>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <div class="form-group" style="display: none;">
                                            <input type="text" id="txt_sno" maxlength="45" name="vendorcode" placeholder="sno">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Address</label>
                                        <input id="txt_address" type="text" class="form-control" name="vendorcode" placeholder="Address" />
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <label>
                                            Permit type</label>
                                        <select id="txt_rcno" class="form-control" style="min-width: 195px;">
                                            <option value="Local">Local</option>
                                            <option value="National">National</option>
                                        </select>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <label>
                                            Permit Expire Date</label>
                                        <input id="txt_rcexp_date" type="date" class="form-control" name="vendorcode" placeholder="RC Expire Date"
                                            style="min-width: 195px;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            State Name</label>
                                        <input id="txt_permitstatename" type="text" class="form-control" name="vendorcode"
                                            placeholder="State Name" />
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <label>
                                            State Permit Expire Date</label>
                                        <input id="txt_permit_state_expdate" type="date" class="form-control" name="vendorcode"
                                            placeholder="RC Permit State Expire Date" style="min-width: 195px;" />
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <label>
                                        </label>
                                        <%-- <input id='Button1' type="button" class="btn btn-primary" name="submit" value='Show Documents'
                                        onclick="GetDocuments_click()" />--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Pollution Number</label>
                                        <input id="txt_pollution" type="text" class="form-control" name="vendorcode" placeholder="Pollution Number" />
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <label>
                                            Pollution Expire Date</label>
                                        <input id="txt_pollexp_date" type="date" class="form-control" name="vendorcode" placeholder="Pollution Expire Date"
                                            style="min-width: 195px;" />
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <label>
                                            Insurence Company</label>
                                        <select id="slct_insurancecompany" class="form-control" style="min-width: 195px;">
                                            <option value="Select Vehicle Type" selected disabled>Select Insurence Company</option>
                                        </select><label id="lbl_insurancecompany" class="errormessage">* Please Select Insurence
                                            Company Name</label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Insurence</label>
                                        <input id="txt_insurence" type="text" class="form-control" name="vendorcode" placeholder="Insurence" />
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <label>
                                            Insurence Expire Date</label>
                                        <input id="txt_insexp_date" type="date" class="form-control" name="vendorcode" placeholder="Insurence Expire Date"
                                            style="min-width: 195px;" />
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <label>
                                            Fittness</label>
                                        <input id="txt_fitness" type="text" class="form-control" name="vendorcode" placeholder="Fittness" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Fitness Expire Date</label>
                                        <input id="txt_fitexp_date" type="date" class="form-control" name="vendorcode" placeholder="Fittness Expire Date"
                                            style="min-width: 195px;" />
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <label>
                                            Actual Mileage</label>
                                        <input id="txt_actualmileage" type="text" class="form-control" name="vendorcode"
                                            placeholder="Actual Mileage" />
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <label>
                                            RoadTax</label>
                                        <input id="txt_roadtax" type="text" class="form-control" name="vendorcode" placeholder="Road Tax" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            RoadTax Expire Date</label>
                                        <input id="txt_roadtaxexp_date" type="date" class="form-control" name="vendorcode"
                                            placeholder="RoadTax Expire Date" style="min-width: 195px;" />
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <label>
                                            State RoadTax</label>
                                        <input id="txt_stateroadtax" type="text" class="form-control" name="vendorcode" placeholder="State Road Tax" />
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <label>
                                            State RoadTax Expire Date</label>
                                        <input id="txt_stateroadtaxexp_date" type="date" class="form-control" name="vendorcode"
                                            placeholder="tate RoadTax Expire Date" style="min-width: 195px;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Battery No1</label>
                                        <select id="ddlBatteryno" class="form-control" style="min-width: 195px;">
                                            <option value="Select Battery No1" selected disabled>Select Battery No1</option>
                                        </select><label id="Label1" class="errormessage">* Please Select Battery No1</label>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <label>
                                            Battery No2</label>
                                        <select id="ddlBatteryno2" class="form-control" style="min-width: 195px;">
                                            <option value="Select Battery No2" selected disabled>Select Battery No2</option>
                                        </select><label id="Label2" class="errormessage">* Please Select Battery No2</label>
                                    </td>
                                </tr>
                            </table>
                            <div align="center" id="tyre_assign" style="display: none;">
                                <div>
                                    <div class="form-group">
                                        <label>
                                            <input type="checkbox" id="tyre_check" onchange="checkbox_change()" style="height: 30px;
                                                width: 30px;">Check Here to Assign Tyres For First Time</label>
                                    </div>
                                </div>
                                <table align="center">
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <label>
                                                Axil Master Name <span style="color: red;">*</span></label>
                                            <select id="slct_axilmaster" class="form-control" style="min-width: 195px;" onchange="vehtype_onchange()"
                                                disabled>
                                                <option selected disabled value="Select Axil Master">Select Axil Master</option>
                                            </select>
                                            <label id="lbl_error_axilmstr" class="errormessage">
                                                * Please Select Axil Master
                                            </label>
                                        </td>
                                        <td style="width: 5px;">
                                        </td>
                                        <td>
                                            <label>
                                                Odometer <span style="color: red;">*</span></label>
                                            <input id="txt_odometer" class="form-control" type="text" name="vendorcode" disabled
                                                placeholder="Odometer" />
                                            <label id="lbl_odometer_error_msg" class="errormessage">
                                                * Please Enter Odometer Reading
                                            </label>
                                        </td>
                                        <td style="width: 5px;">
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="tyre_ass_done" style="display: none;">
                                <label>
                                    Tyres Assign Alredy Done For This Vehicle</label>
                            </div>
                            <div id="div_axilautofill" align="center">
                                <span style="color: #0252AA; font-size: 18px; font-weight: bold;">SRI VYSHNAVI DAIRY
                                    SPECIALITIES (P) LTD </span>
                                <br />
                                Vehicle No <span id="lblvehicle" style="color: red; font-size: 16px; font-weight: bold;">
                                </span>
                                <table id="tbl_axils" class="responsive-table" style="width: 65%;">
                                    <thead>
                                        <tr>
                                            <th scope="col">
                                                Axil
                                            </th>
                                            <th scope="col">
                                                Axil Name
                                            </th>
                                            <th scope="col">
                                                No Of Tyres
                                            </th>
                                            <th scope="col" style="display: none;">
                                                Axil Sno
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>
                            </div>
                            <br />
                            <div>
                              <%--  <input id='save_vehmaster' type="button" class="btn btn-success" name="submit" value='Save'
                                    onclick="save_vehmaster_click()" />--%>
                                <input id='close_vehmaster' type="button" class="btn btn-danger" name="Close" value='Close' />
                                <input id='btnPrint' type="button" class="btn btn-primary" name="Close" value='Print'
                                    onclick="javascript:CallPrint('div_axilautofill');" />
                            </div>
                        </div>
                        <div id="div_Documents" class="box box-danger" style="display: none;">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Documents Upload</h3>
                            </div>
                            <div class="box-body">
                                <div class="row">
                                    <div>
                                        <br>
                                        <div class="box-body">
                                            <div class="row">
                                                <div class="col-sm-4">
                                                    <label class="control-label">
                                                        Document Type</label>
                                                    <select id="ddl_documenttype" class="form-control">
                                                        <option>Select Document Type</option>
                                                        <option value="1">RCCopy</option>
                                                        <option value="2">Insurance</option>
                                                        <option value="3">APPermitt</option>
                                                        <option value="4">TNPermitt</option>
                                                        <option value="5">APRoadTax</option>
                                                        <option value="6">TNRoadTax</option>
                                                        <option value="7">FC</option>
                                                        <option value="8">Pollution</option>
                                                        <option value="9">Bilateraltax</option>
                                                        <option value="10">Authorisation</option>
                                                        <option value="11">NationalPermitt</option>
                                                        <option value="12">TSRoadTax</option>
                                                        <option value="13">TSPermitt</option>
                                                    </select>
                                                </div>
                                                <div class="col-sm-4">
                                                    <table class="table table-bordered table-striped">
                                                        <tbody>
                                                            <tr>
                                                                <td>
                                                                    <div id="FileUpload_div" class="img_btn" onclick="getFile_doc()" style="height: 50px;
                                                                        width: 100%">
                                                                        Choose Document To Upload
                                                                    </div>
                                                                    <div style="height: 0px; width: 0px; overflow: hidden;">
                                                                        <input id="FileUpload1" type="file" name="files[]" onchange="readURL_doc(this);">
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                                <div class="col-sm-4">
                                                    <input id="btn_upload_document" type="button" class="btn btn-primary" name="submit"
                                                        value="UPLOAD" onclick="upload_Vehicle_Document_Info();" style="width: 120px;
                                                        margin-top: 25px;">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="box-body">
                                            <div id="div_documents_table">
                                            </div>
                                        </div>
                                        <br />
                                        <div>
                                            <input id='close_documents' type="button" class="btn btn-danger" name="Close" value='Close' />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <div id="div_vehmaster_table">
                    <table id="tbl_vehmaster" class="table table-bordered table-striped">
                        <thead>
                            <tr>
                                <th scope="col">
                                    Reg No
                                </th>
                                <th scope="col">
                                    Vehicle Type
                                </th>
                                <th scope="col">
                                    Door No
                                </th>
                                <th scope="col">
                                    Status
                                </th>
                                <th scope="col" style="display: none;">
                                    sno
                                </th>
                                <th scope="col">
                                    Capacity
                                </th>
                                <th scope="col">
                                    Vehicle Make
                                </th>
                                <th scope="col">
                                    Fuel Capacity
                                </th>
                                <th scope="col" style="display: none;">
                                    AxilName_sno
                                </th>
                                <th scope="col">
                                    AxilName
                                </th>
                                <th scope="col">
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
                <div id="displaydiv" class="bpmouseover">
                </div>
                <div id="hover" style="display: none;">
                </div>
                <%--<div id="addexampopup" class="popup" style="display: none;">
                    <div class="popupclose" onclick="btn_cancelexampopup_click()">
                        X</div>
                    <div class="ui-dialog-titlebar ui-widget-header ui-corner-all ui-helper-clearfix ui-draggable-handle"
                        style="height: 50px; border-radius: 5px 5px 0px 0px; padding: 8px 0px 0px 15px;
                        text-align: left; font-family: 'ProximaNovaRegular'; letter-spacing: 0px; font-weight: normal;
                        text-shadow: 0px 0px 1px #304316;">
                        <span id="title_popup" class="ui-dialog-title" style="font-size: 21px; text-align: center;">
                            RC BOOK</span>
                    </div>
                    <div style="padding: 10px;">
                        <img id="rc_photo" src="" alt="RC BOOK" width="390px" height="390px" />
                    </div>
                </div>--%>
            </div>
        </div>
    </section>
</asp:Content>

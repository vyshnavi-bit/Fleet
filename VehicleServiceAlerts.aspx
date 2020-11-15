<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="VehicleServiceAlerts.aspx.cs" Inherits="VehicleServviceAlerts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            getallveh_nos();
            $('#add_partnumber').click(function () {
                $('.hiddenrow').hide();
                $('#vehser_fillform').css('display', 'block');
                $('#div_vendordata').hide();
                $('#vehservice_showlogs').css('display', 'none');
            });
            $('#close_vehmaster').click(function () {
                $('.hiddenrow').hide();
                $('#vehser_fillform').css('display', 'none');
                $('#vehservice_showlogs').css('display', 'block');
                $('#div_vendordata').show();
            });
            only_no();
            get_vehicleServicealerts_details();
        });
        function addvehicleservicealerts() {
            $('.hiddenrow').hide();
            $('#vehser_fillform').css('display', 'block');
            $('#div_vendordata').hide();
            $('#vehservice_showlogs').css('display', 'none');
            clearServicesalerts();
        }
        function closevehiclealertdetails() {
            $('.hiddenrow').hide();
            $('#vehser_fillform').css('display', 'none');
            $('#vehservice_showlogs').css('display', 'block');
            $('#div_vendordata').show();
            clearServicesalerts();
        }
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
        function save_Veh_Servicing_Save_click() {
            var veh_sno = document.getElementById('slct_vehicle_no').value;
            var flag = false;
            if (veh_sno == "" || veh_sno == "Select Vehicle No") {
                $("#lbl_VehicleNo").show();
                $("#slct_vehicle_no").focus();
                flag = true;
            }
            var txt_eoc = document.getElementById('txt_eoc').value;
            if (txt_eoc == "") {
                $("#lbl_eoc").show();
                $("#txt_eoc").focus();
                flag = true;
            }
            var txt_goc = document.getElementById('txt_goc').value;
            if (txt_goc == "") {
                $("#lbl_goc").show();
                $("#txt_goc").focus();
                flag = true;
            }
            var txt_ofc = document.getElementById('txt_ofc').value;
            if (txt_ofc == "") {
                $("#lbl_ofc").show();
                $("#txt_ofc").focus();
                flag = true;
            }
            var txt_afc = document.getElementById('txt_afc').value;
            if (txt_afc == "") {
                $("#lbl_afc").show();
                flag = true;
            }
            var txt_brakefluid = document.getElementById('txt_brakefluid').value;
            if (txt_brakefluid == "") {
                $("#lbl_brakefluid").show();
                $("#txt_brakefluid").focus();
                flag = true;
            }
            var txt_powersteeringfluid = document.getElementById('txt_powersteeringfluid').value;
            if (txt_powersteeringfluid == "") {
                $("#lbl_powersteeringfluid").show();
                $("#txt_powersteeringfluid").focus();
                flag = true;
            }
            var txt_transmissionfluid = document.getElementById('txt_transmissionfluid').value;
            if (txt_transmissionfluid == "") {
                $("#lbl_transmissionfluid").show();
                $("#txt_transmissionfluid").focus();
                flag = true;
            }
            var txt_washerfluid = document.getElementById('txt_washerfluid').value;
            if (txt_washerfluid == "") {
                $("#lbl_washerfluid").show();
                $("#txt_washerfluid").focus();
                flag = true;
            }
            var txt_checkbrakes = document.getElementById('txt_checkbrakes').value;
            if (txt_checkbrakes == "") {
                $("#lbl_checkbrakes").show();
                $("#txt_checkbrakes").focus();
                flag = true;
            }
            var txt_checkleaks = document.getElementById('txt_checkleaks').value;
            if (txt_checkleaks == "") {
                $("#lbl_checkleaks").show();
                $("#txt_checkleaks").focus();
                flag = true;
            }
            var txt_allbelts = document.getElementById('txt_allbelts').value;
            if (txt_allbelts == "") {
                $("#lbl_allbelts").show();
                $("#txt_allbelts").focus();
                flag = true;
            }
            var txt_lubricatechassis = document.getElementById('txt_lubricatechassis').value;
            if (txt_lubricatechassis == "") {
                $("#lbl_lubricatechassis").show();
                $("#txt_lubricatechassis").focus();
                flag = true;
            }
            var txt_airchecking = document.getElementById('txt_airchecking').value;
            if (txt_airchecking == "") {
                $("#lbl_airchecking").show();
                $("#txt_airchecking").focus();
                flag = true;
            }
            var txt_tyreinterchanging = document.getElementById('txt_tyreinterchanging').value;
            if (txt_tyreinterchanging == "") {
                $("#lbl_tyreinterchanging").show();
                $("#txt_tyreinterchanging").focus();
                flag = true;
            }
            if (flag) {
                return;
            }
            var btnval = document.getElementById('btn_save').innerHTML;
            var sno = document.getElementById('lbl_sno').innerHTML;
            var data = { 'op': 'save_Veh_Servicing_Save_click', 'vehsno': veh_sno, 'eoc': txt_eoc, 'goc': txt_goc, 'ofc': txt_ofc, 'afc': txt_afc, 'brakefluid': txt_brakefluid, 'powersteeringfluid': txt_powersteeringfluid, 'transmissionfluid': txt_transmissionfluid, 'washerfluid': txt_washerfluid, 'checkbrakes': txt_checkbrakes, 'checkleaks': txt_checkleaks, 'allbelts': txt_allbelts, 'lubricatechassis': txt_lubricatechassis,'airchecking':txt_airchecking,'tyreinterchanging':txt_tyreinterchanging, 'btnval': btnval, 'sno': sno };
            var s = function (msg) {
                if (msg) {
                    getallveh_nos();
                    clearServicesalerts();
                    alert(msg);
                    $('.hiddenrow').hide();
                    $('#vehser_fillform').css('display', 'none');
                    $('#vehservice_showlogs').css('display', 'block');
                    $('#div_vendordata').show();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function clearServicesalerts() {
            document.getElementById('slct_vehicle_no').selectedIndex = 0;
            document.getElementById('txt_eoc').value = "";
            document.getElementById('txt_goc').value = "";
            document.getElementById('txt_ofc').value = "";
            document.getElementById('txt_afc').value = "";
            document.getElementById('txt_brakefluid').value = "";
            document.getElementById('txt_powersteeringfluid').value = "";
            document.getElementById('txt_transmissionfluid').value = "";
            document.getElementById('txt_washerfluid').value = "";
            document.getElementById('txt_checkbrakes').value = "";
            document.getElementById('txt_checkleaks').value = "";
            document.getElementById('txt_allbelts').value = "";
            document.getElementById('txt_lubricatechassis').value = "";
            document.getElementById('txt_vehtype').value = "";
            document.getElementById('lbl_capacity').innerHTML = "";
            document.getElementById('lbl_fuelcapacity').innerHTML = "";
            document.getElementById('btn_save').innerHTML = "Save";
            document.getElementById('txt_airchecking').value = "";
            document.getElementById('txt_tyreinterchanging').value = "";
            $('#lbl_VehicleNo').hide();
            $('#lbl_eoc').hide();
            $('#lbl_goc').hide();
            $('#lbl_ofc').hide();
            $('#lbl_afc').hide();
            $('#lbl_brakefluid').hide();
            $('#lbl_powersteeringfluid').hide();
            $('#lbl_transmissionfluid').hide();
            $('#lbl_washerfluid').hide();
            $('#lbl_checkbrakes').hide();
            $('#lbl_checkleaks').hide();
            $('#lbl_allbelts').hide();
            $('#lbl_lubricatechassis').hide();
            $('#lbl_airchecking').hide();
            $('#lbl_tyreinterchanging').hide();
        }
        //Function for only no
        function only_no() {
            //$("#txt_fuelcapacity,#txt_axilno,.tyreno").keydown(function (event) {
            $("#txt_eoc,#txt_goc,#txt_ofc,#txt_afc,#txt_brakefluid,#txt_powersteeringfluid,#txt_transmissionfluid,#txt_washerfluid,#txt_checkbrakes,#txt_checkleaks,#txt_allbelts,#txt_lubricatechassis").keydown(function (event) {
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
        function get_vehicleServicealerts_details() {
            var data = { 'op': 'get_vehicleServicealerts_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillServicingdetails(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillServicingdetails(msg) {
            var k = 0;
            var colorue = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            var results = '<div style="overflow:auto;"><table  class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;"></th><th scope="col" style="font-weight: bold;">VehicleNumber</th><th scope="col" style="font-weight: bold;">EOC</th><th scope="col" style="font-weight: bold;">GOC</th><th scope="col" style="font-weight: bold;">AFC</th><th scope="col" style="font-weight: bold;">OFC</th><th scope="col" style="font-weight: bold;">Brake Fluid</th><th scope="col" style="font-weight: bold;">Steering Fluid</th><th scope="col" style="font-weight: bold;">Transmission Fluid</th><th scope="col" style="font-weight: bold;">Washer Fluid</th><th scope="col" style="font-weight: bold;">Brakes & Bearings</th><th scope="col" style="font-weight: bold;">Check Leaks</th><th scope="col" style="font-weight: bold;">All Belts</th><th scope="col" style="font-weight: bold;">Lubricate Chassis</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + colorue[k] + '">';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getme(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td>';
                //results += '<td scope="row" class="1">' + msg[i].vehicleno + '</td>';
                results += '<td   class="1"><i class="fa fa-truck" aria-hidden="true"></i>&nbsp;<span id="1">' + msg[i].vehicleno + '</span></td>';
                results += '<td data-title="EOC" class="2">' + msg[i].EOC + '</td>';
                results += '<td data-title="GOC" class="3">' + msg[i].GOC + '</td>';
                results += '<td data-title="AFC" class="4">' + msg[i].AFC + '</td>';
                results += '<td data-title="OFC" class="5">' + msg[i].OFC + '</td>';
                results += '<td data-title="brakefluid" class="6">' + msg[i].brakefluid + '</td>';
                results += '<td data-title="powersteeringfluid" class="7">' + msg[i].powersteeringfluid + '</td>';
                results += '<td data-title="transmissionfluid" class="8">' + msg[i].transmissionfluid + '</td>';
                results += '<td data-title="washerfluid" class="9">' + msg[i].washerfluid + '</td>';
                results += '<td data-title="checkbrakes" class="10">' + msg[i].checkbrakes + '</td>';
                results += '<td data-title="checkleaks" class="11">' + msg[i].checkleaks + '</td>';
                results += '<td data-title="allbelts" class="12">' + msg[i].allbelts + '</td>';
                results += '<td data-title="lubricatechassis" class="13">' + msg[i].lubricatechassis + '</td>';
                results += '<td style="display:none" class="14">' + msg[i].vehsno + '</td>';
                results += '<td style="display:none" class="sno">' + msg[i].Sno + '</td></tr>';
                k = k + 1;
                if (k == 4) {
                    k = 0;
                }
            }
            results += '</table></div>';
            $("#div_vendordata").html(results);
        }
        function getme(thisid) {
            var sno = $(thisid).parent().parent().children('.sno').html();
            var vehsno = $(thisid).parent().parent().children('.14').html();
            var VehicleNo = $(thisid).parent().parent().find('#1').html();
            VehicleNo = replaceHtmlEntites(VehicleNo);
            var EOC = $(thisid).parent().parent().children('.2').html();
            EOC = replaceHtmlEntites(EOC);
            var GOC = $(thisid).parent().parent().children('.3').html();
            var AFC = $(thisid).parent().parent().children('.4').html();
            var OFC = $(thisid).parent().parent().children('.5').html();
            var brakefluid = $(thisid).parent().parent().children('.6').html();
            var powersteeringfluid = $(thisid).parent().parent().children('.7').html();
            var transmissionfluid = $(thisid).parent().parent().children('.8').html();
            var washerfluid = $(thisid).parent().parent().children('.9').html();
            var checkbrakes = $(thisid).parent().parent().children('.10').html();
            var checkleaks = $(thisid).parent().parent().children('.11').html();
            var allbelts = $(thisid).parent().parent().children('.12').html();
            var lubricatechassis = $(thisid).parent().parent().children('.13').html();

            document.getElementById('slct_vehicle_no').value = vehsno;
            document.getElementById('txt_eoc').value = EOC;
            document.getElementById('txt_goc').value = GOC;
            document.getElementById('txt_ofc').value = AFC;
            document.getElementById('txt_afc').value = AFC;
            document.getElementById('txt_brakefluid').value = brakefluid;
            document.getElementById('txt_powersteeringfluid').value = powersteeringfluid;
            document.getElementById('txt_transmissionfluid').value = transmissionfluid;
            document.getElementById('txt_washerfluid').value = washerfluid;
            document.getElementById('txt_checkbrakes').value = checkbrakes;
            document.getElementById('txt_checkleaks').value = checkleaks;
            document.getElementById('txt_allbelts').value = allbelts;
            document.getElementById('txt_lubricatechassis').value = lubricatechassis;
            document.getElementById('lbl_sno').innerHTML = sno;
            document.getElementById('btn_save').innerHTML = "Modify";
            $("#div_vendordata").hide();
            $("#vehser_fillform").show();
            $('#vehservice_showlogs').hide();
            vehonchenage();
        }
        var replaceHtmlEntites = (function () {
            var translate_re = /&(nbsp|amp|quot|lt|gt);/g;
            var translate = {
                "nbsp": " ",
                "amp": "&",
                "quot": "\"",
                "lt": "<",
                "gt": ">"
            };
            return function (s) {
                return (s.replace(translate_re, function (match, entity) {
                    return translate[entity];
                }));
            }
        })();
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Vehicle Service Alerts<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Vehicle Service Alerts</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Vehicle Service Alerts
                </h3>
            </div>
            <div class="box-body">
                <div id="vehservice_showlogs" style="width: 10px;padding-left: 880px;">
                    <%--<input id="add_partnumber" type="button" class="btn btn-primary" name="submit" value='Add Vehicle Service Alerts' />--%>
                    <div class="input-group" >
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addvehicleservicealerts()"></span><span id="add_partnumber" onclick="addvehicleservicealerts()">Add Vehicle Service</span>
                          </div>
                          </div>
                </div>
                <div id="div_vendordata" style="background: #ffffff">
                </div>
                <div id='vehser_fillform' style="padding: 20px; display: none;">
                    <table align="center">
                        <tr>
                            <td>
                                <label>
                                    Vehicle Number</label>
                                <select id="slct_vehicle_no" class="form-control" onchange="vehonchenage()">
                                    <option selected disabled value="Select Vehicle No">Select Vehicle No</option>
                                </select>
                                <label id="lbl_VehicleNo" class="errormessage">
                                    * Please Select Vehicle No
                                </label>
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td>
                                <label>
                                    Vehicle Type</label>
                                <input id="txt_vehtype" disabled class="form-control" type="text" placeholder="Vehicle Type" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Capacity(Ltr)</label>
                                <label id="lbl_capacity" class="form-control">
                                </label>
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td>
                                <label>
                                    Fuel Capacity(Ltr)</label>
                                <label id="lbl_fuelcapacity" class="form-control">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Engine Oil Change (Kms)<span style="color: red;">*</span></label>
                                <input id="txt_eoc" class="form-control" type="number" name="vendorcode" placeholder="Engine Oil Change (Kms)" />
                                <label id="lbl_eoc" class="errormessage">
                                    * Please Enter Engine Oil Change
                                </label>
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td>
                                <label>
                                    Gear Oil Change (Kms)<span style="color: red;">*</span></label>
                                <input id="txt_goc" class="form-control" type="text" name="vendorcode" placeholder="Gear Oil Change (Kms)" />
                                <label id="lbl_goc" class="errormessage">
                                    * Please Enter Gear Oil Change
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Oil Filter Change (Kms)<span style="color: red;">*</span></label>
                                <input id="txt_ofc" class="form-control" type="text" name="vendorcode" placeholder="Oil Filter (Kms)" />
                                <label id="lbl_ofc" class="errormessage">
                                    * Please Enter Oil Filter
                                </label>
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td>
                                <label>
                                    Air Filter (Kms)<span style="color: red;">*</span></label>
                                <input id="txt_afc" class="form-control" type="text" name="vendorcode" placeholder="Air Filter (Kms)" />
                                <label id="lbl_afc" class="errormessage">
                                    * Please Enter Air Filter
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Brake Fluid<span style="color: red;">*</span></label>
                                <input id="txt_brakefluid" class="form-control" type="text" name="vendorcode" placeholder="Brake Fluid" />
                                <label id="lbl_brakefluid" class="errormessage">
                                    * Please Enter Brake Fluid
                                </label>
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td>
                                <label>
                                    Power Steering Fluid<span style="color: red;">*</span></label>
                                <input id="txt_powersteeringfluid" class="form-control" type="text" name="vendorcode"
                                    placeholder="Power Steering Fluid" />
                                <label id="lbl_powersteeringfluid" class="errormessage">
                                    * Please Enter Power Steering Fluid
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Transmission Fluid<span style="color: red;">*</span></label>
                                <input id="txt_transmissionfluid" class="form-control" type="text" name="vendorcode"
                                    placeholder="Transmission Fluid" />
                                <label id="lbl_transmissionfluid" class="errormessage">
                                    * Please Enter Transmission Fluid
                                </label>
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td>
                                <label>
                                    Washer Fluid<span style="color: red;">*</span></label>
                                <input id="txt_washerfluid" class="form-control" type="text" name="vendorcode" placeholder="Washer Fluid" />
                                <label id="lbl_washerfluid" class="errormessage">
                                    * Please Enter Washer Fluid
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Check brakes and wheel bearings(Kms)<span style="color: red;">*</span></label>
                                <input id="txt_checkbrakes" class="form-control" type="text" name="vendorcode" placeholder="Check brakes and wheel bearings" />
                                <label id="lbl_checkbrakes" class="errormessage">
                                    * Please Enter Check brakes and wheel bearings
                                </label>
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td>
                                <label>
                                    Check for leaks <span style="color: red;">*</span></label>
                                <input id="txt_checkleaks" class="form-control" type="text" name="vendorcode" placeholder="Check for leaks " />
                                <label id="lbl_checkleaks" class="errormessage">
                                    * Please Enter Check for leaks
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Check all belts and hoses<span style="color: red;">*</span></label>
                                <input id="txt_allbelts" class="form-control" type="text" name="vendorcode" placeholder="Check all belts and hoses" />
                                <label id="lbl_allbelts" class="errormessage">
                                    * Please Enter Check all belts and hoses
                                </label>
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td>
                                <label>
                                    Lubricate chassis<span style="color: red;">*</span></label>
                                <input id="txt_lubricatechassis" class="form-control" type="text" name="vendorcode"
                                    placeholder="Lubricate chassis" />
                                <label id="lbl_lubricatechassis" class="errormessage">
                                    * Please Enter Lubricate chassis
                                </label>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                <label>
                                    AirChecking<span style="color: red;">*</span></label>
                                <input id="txt_airchecking" class="form-control" type="text" name="vendorcode" placeholder="Air Checking" />
                                <label id="lbl_airchecking" class="errormessage">
                                    * Please Enter Air Checking
                                </label>
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td>
                                <label>
                                    Tyre InterChanging<span style="color: red;">*</span></label>
                                <input id="txt_tyreinterchanging" class="form-control" type="text" name="vendorcode"
                                    placeholder="Tyre InterChanging" />
                                <label id="lbl_tyreinterchanging" class="errormessage">
                                    * Please Enter Tyre InterChanging
                                </label>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr hidden>
                            <td>
                                <label id="lbl_sno">
                                </label>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div align="center">
                        <%--<input id='btn_save' type="button" class="btn btn-primary" name="submit" value='Save'
                            onclick="save_Veh_Servicing_Save_click()" />
                        <input id='close_vehmaster' type="button" class="btn btn-danger" name="Close" value='Close' />--%>
                        <table>
                        <tr>
                        <td>
                        </td>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="btn_save1" onclick="save_branchname()"></span><span id="btn_save" onclick="save_Veh_Servicing_Save_click()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='close_vehmaster1' onclick="closevehiclealertdetails()"></span><span id='close_vehmaster' onclick="closevehiclealertdetails()">Close</span>
                            </div>
                            </div>
                            </td>
                            </tr>
                            </table>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>

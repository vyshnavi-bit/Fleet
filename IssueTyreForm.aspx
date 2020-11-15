<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="IssueTyreForm.aspx.cs" Inherits="IssueTyreForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<link href="opcss/bootextract.css" rel="stylesheet" type="text/css" />
  <link rel="stylesheet" type="text/css" href="dist/css/bootstrap.css" />--%>
    <script src="js/utility.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#dtp_issueddate').datepicker();
            $('#dtp_issueddate').datepicker("option", "dateFormat", "dd/mm/yy");
            $('#dtp_fitteddate').datepicker();
            $('#dtp_fitteddate').datepicker("option", "dateFormat", "dd/mm/yy");
            get_vehicles_data();
            get_removedtyres_data();
            for_removing_hideerrors();
        });
        $(document).ready(function () {
            $("#txt_Odometerreading,#txt_fittedodometer").keydown(function (event) {
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
        function get_vehicles_data() {
            var data = { 'op': 'getalldataforissuetyre' };
            var s = function (msg) {
                if (msg) {
                    fillcombos(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function get_removedtyres_data() {
            var data = { 'op': 'getalldataforissuetyre_tyres' };
            var s = function (msg) {
                if (msg) {
                    fillcombos2(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        var gettyres = [];

        function fillcombos(msg) {
            var data = msg;
            var getvehicles = data["vehicles"];
            var vehicles = document.getElementById('Seslct_vehiclenumber');
            var length = vehicles.options.length;
            document.getElementById('Seslct_vehiclenumber').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Vehicle";
            opt.value = "Select Vehicle";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            vehicles.appendChild(opt);
            for (i = 0; i < getvehicles.length; i++) {
                var option = document.createElement('option');
                option.innerHTML = getvehicles[i].vehiclenum;
                option.value = getvehicles[i].vehiclesno;
                vehicles.appendChild(option);
            }
        }

        function fillcombos2(msg) {
            gettyres = [];
            var data = msg;
            gettyres = data["tyres"];
            var tyres = document.getElementById('slct_issuedtyresno');
            var length = tyres.options.length;
            document.getElementById('slct_issuedtyresno').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Tyre";
            opt.value = "Select Tyre";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            tyres.appendChild(opt);
            for (i = 0; i < gettyres.length; i++) {
                var option = document.createElement('option');
                option.innerHTML = gettyres[i].tyre_sno;
                option.value = gettyres[i].sno;
                tyres.appendChild(option);
            }
        }
        function get_axils() {
            var vehicle_sno = document.getElementById('Seslct_vehiclenumber').value;
            var data = { 'op': 'get_axils_for_vehicles', 'vehicle_sno': vehicle_sno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillaxilcombo(msg);
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
        function fillaxilcombo(vehmakemsg) {
            var vehtyp = document.getElementById('Seslct_axlenumber');
            var length = vehtyp.options.length;
            document.getElementById('Seslct_axlenumber').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Axil";
            opt.value = "Select Axil";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            vehtyp.appendChild(opt);
            for (var i = 0; i < vehmakemsg.length; i++) {
                if (vehmakemsg[i].sno != null) {
                    var option = document.createElement('option');
                    option.innerHTML = vehmakemsg[i].axlename;
                    option.value = vehmakemsg[i].sno;
                    vehtyp.appendChild(option);
                }
            }
        }

        function axle_onchange() {
            var vehiclenumber = document.getElementById('Seslct_vehiclenumber').value;
            var axlenumber = document.getElementById('Seslct_axlenumber').value;
            if (vehiclenumber == "Select Vehicle" || axlenumber == "Select Axle") {
                return;
            }
            var data = { 'op': 'get_issued_tyres', 'vehiclenumber': vehiclenumber, 'axlenumber': axlenumber };
            var s = function (msg) {
                if (msg) {
                    fillissuedtyre(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillissuedtyre(msg) {
            var vehtyp = document.getElementById('slct_tyreposition');
            var length = vehtyp.options.length;
            document.getElementById('slct_tyreposition').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Tyre Position";
            opt.value = "Select Tyre Position";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            vehtyp.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].sno != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].tyrename;
                    option.value = msg[i].sno;
                    vehtyp.appendChild(option);
                }
            }
        }
        var remove_tyredata = [];
        function filltyres_sno_withposition() {
            remove_tyredata = [];
            var tyreposition = document.getElementById('slct_tyreposition').value;
            var vehiclenumber = document.getElementById('Seslct_vehiclenumber').value;
            var axlenumber = document.getElementById('Seslct_axlenumber').value;
            if (tyreposition == "Select Tyre Position") {
                return;
            }
            var data = { 'op': 'get_tyre_using_position', 'tyreposition': tyreposition, 'vehiclenumber': vehiclenumber, 'axlenumber': axlenumber };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        remove_tyredata = msg;
                        fillissure_tyre_combo(msg);
                        document.getElementById('txt_prevodordng').value = "";
                        for_removing_clearall();
                        for_fitted_clearall();
                        get_axil_odometer();
                    }
                    else {
                        document.getElementById('txt_prevodordng').value = "";
                        alert("There is No Tyre for this Position");
                        var vehtyp = document.getElementById('slct_removedtyresno');
                        var length = vehtyp.options.length;
                        document.getElementById('slct_removedtyresno').options.length = null;
                        var opt = document.createElement('option');
                        opt.innerHTML = "Select Tyre";
                        opt.value = "Select Tyre";
                        opt.setAttribute("selected", "selected");
                        opt.setAttribute("disabled", "disabled");
                        opt.setAttribute("class", "dispalynone");
                        vehtyp.appendChild(opt);
                        for_removing_clearall();
                        for_fitted_clearall();
                        get_axil_odometer();

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
        function get_axil_odometer() {
            var tyreposition = document.getElementById('slct_tyreposition').value;
            var vehiclenumber = document.getElementById('Seslct_vehiclenumber').value;
            var axlenumber = document.getElementById('Seslct_axlenumber').value;
            if (tyreposition == "Select Tyre Position") {
                return;
            }
            var data = { 'op': 'get_axil_odometer_using_position', 'tyreposition': tyreposition, 'vehiclenumber': vehiclenumber, 'axlenumber': axlenumber };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        document.getElementById('txt_fittedodometer').value = msg[0].Odometer;
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

        function fillissure_tyre_combo(msg) {
            var vehtyp = document.getElementById('slct_removedtyresno');
            var length = vehtyp.options.length;
            document.getElementById('slct_removedtyresno').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Tyre";
            opt.value = "Select Tyre";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            vehtyp.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].Sno != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].tyre_sno;
                    option.value = msg[i].Sno;
                    vehtyp.appendChild(option);
                }
            }
        }
        function removetyre_onchange() {
            var remvtyre = document.getElementById('slct_removedtyresno').value;
            for (var i = 0; i < remove_tyredata.length; i++) {
                if (remove_tyredata[i].Sno == remvtyre) {
                    var presodo = remove_tyredata[i].Odometer;
                    var preskms = remove_tyredata[i].current_KMS;
                    var diff = parseInt(presodo) - parseInt(preskms);
                    document.getElementById('txt_prevodordng').value = diff;
                    if (remove_tyredata[i].current_KMS == "") {
                        document.getElementById('lbl_prevkms').innerHTML = "0";
                    }
                    else {
                        document.getElementById('lbl_prevkms').innerHTML = remove_tyredata[i].current_KMS;
                    }
                    break;
                }
            }
        }
        function for_tyre_removing_save() {
            var issueddate = document.getElementById('dtp_issueddate').value;
            var removedtyresno = document.getElementById('slct_removedtyresno').value;
            var removedtyretype = document.getElementById('Seslct_removedtyretype').value;
            var Odometerreading = document.getElementById('txt_Odometerreading').value;
            var axlenumber = document.getElementById('Seslct_axlenumber').value;
            var tyreposition = document.getElementById('slct_tyreposition').value;
            var d = document.getElementById('Seslct_axlenumber');
            var axilname = d.options[d.selectedIndex].text;
            var vehiclenumber = document.getElementById('Seslct_vehiclenumber').value;
            var remarks = document.getElementById('txt_remarks').value;
            //var kms = document.getElementById('lbl_diffkms').innerHTML;
            var totalkms = $('#lbl_diffkms').text();
            var kms = $('#lbl_kmstrvldthisvhl').text();
            var flag = false;
            if (issueddate == "") {
                $("#lbl_issueddate_error_msg").show();
                flag = true;
            }
            if (removedtyresno == "Select Tyre") {
                $("#lbl_removedtyresno_error_msg").show();
                flag = true;
            }
            if (removedtyretype == "Select Type") {
                $("#lbl_removedtyretype_error_msg").show();
                flag = true;
            }
            if (Odometerreading == "") {
                $("#lbl_odoreading_error_msg").show();
                flag = true;
            }
            if (axlenumber == "Select Axil") {
                $("#lbl_axlenumber_error_msg").show();
                flag = true;
            }
            if (vehiclenumber == "Select Vehicle") {
                $("#lbl_vehiclenumber_error_msg").show();
                flag = true;
            }
            if (tyreposition == "Select Tyre Position") {
                $("#lbl_tyrepos_error_msg").show();
                flag = true;
            }
            if (kms < 0) {
                alert("Please give valid odometers to get travelled kms");
                flag = true;
            }
            if (flag) {
                return;
            }
            var data = { 'op': 'for_save_edit_removedtyre', 'issueddate': issueddate, 'removedtyresno': removedtyresno, 'removedtyretype': removedtyretype, 'Odometerreading': Odometerreading,
                'axlenumber': axlenumber, 'tyreposition': tyreposition, 'vehiclenumber': vehiclenumber, 'remarks': remarks, 'kms': kms, 'totalkms': totalkms, 'axilname': axilname
            };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        if (msg == "OK") {
                            alert("Tyre Removed Successfully");
                            for_removing_clearall();
                            var presentodo = document.getElementById('txt_Odometerreading').value;
                            document.getElementById('txt_fittedodometer').value = presentodo;
                            var issueddate = document.getElementById('dtp_issueddate').value;
                            document.getElementById('dtp_fitteddate').value = issueddate;
                            filltyres_sno_withposition();
                            get_removedtyres_data();
                        }
                        else {
                            alert(msg);
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
        function for_removing_clearall() {
            document.getElementById('dtp_issueddate').value = "";
            document.getElementById('slct_removedtyresno').value = "Select Tyre";
            document.getElementById('Seslct_removedtyretype').value = "Select Type";
            document.getElementById('txt_Odometerreading').value = "";
            document.getElementById('txt_remarks').value = "";
            document.getElementById('txt_prevodordng').value = "";
            document.getElementById('lbl_diffkms').innerHTML = "";
            document.getElementById('lbl_prevkms').innerHTML = "";
            document.getElementById('lbl_kmstrvldthisvhl').innerHTML = "";
            for_removing_hideerrors();
        }

        function for_removing_hideerrors() {
            $("#lbl_issueddate_error_msg").hide();
            $("#lbl_removedtyresno_error_msg").hide();
            $("#lbl_odoreading_error_msg").hide();
            $("#lbl_Vendor_error_msg").hide();
            $("#lbl_axlenumber_error_msg").hide();
            $("#lbl_vehiclenumber_error_msg").hide();
            $("#lbl_tyrepos_error_msg").hide();
            $("#lbl_fittingdate_error_msg").hide();
            $("#lbl_fittingtyresno_error_msg").hide();
            $("#lbl_fittedtyretype_error_msg").hide();
            $("#lbl_fitted_odoreading_error_msg").hide();
        }
        function for_tyre_fitting_save() {
            var fitteddate = document.getElementById('dtp_fitteddate').value;
            var issuedtyresno = document.getElementById('slct_issuedtyresno').value;
            var issuedtyretype = document.getElementById('Seslct_issuedtyretype').value;
            var fittedodometer = document.getElementById('txt_fittedodometer').value;
            var axlenumber = document.getElementById('Seslct_axlenumber').value;
            var d = document.getElementById('Seslct_axlenumber');
            var axilname = d.options[d.selectedIndex].text;
            var tyreposition = document.getElementById('slct_tyreposition').value;
            var vehiclenumber = document.getElementById('Seslct_vehiclenumber').value;
            var fitremarks = document.getElementById('txt_fitremarks').value;
            var fitkms = $('#lbl_fitkms').text();
            var flag = false;
            if (fitteddate == "") {
                $("#lbl_fittingdate_error_msg").show();
                flag = true;
            }
            if (issuedtyresno == "Select Tyre") {
                $("#lbl_fittingtyresno_error_msg").show();
                flag = true;
            }
            if (issuedtyretype == "Select Type") {
                $("#lbl_fittedtyretype_error_msg").show();
                flag = true;
            }
            if (fittedodometer == "") {
                $("#lbl_fitted_odoreading_error_msg").show();
                flag = true;
            }
            if (axlenumber == "Select Axil") {
                $("#lbl_axlenumber_error_msg").show();
                flag = true;
            }
            if (vehiclenumber == "Select Vehicle") {
                $("#lbl_vehiclenumber_error_msg").show();
                flag = true;
            }
            if (tyreposition == "Select Tyre Position") {
                $("#lbl_tyrepos_error_msg").show();
                flag = true;
            }
            if (fitkms < 0) {
                alert("Please give valid odometers to get travelled kms");
                flag = true;
            }
            var remvdtyresno = document.getElementById('slct_removedtyresno').length;
            if (remvdtyresno > 1) {
                alert("There is already a tyre fitted for this Position");
                flag = true;
            }
            if (flag) {
                return;
            }
            var data = { 'op': 'for_save_edit_fittedtyre', 'fitteddate': fitteddate, 'issuedtyresno': issuedtyresno, 'issuedtyretype': issuedtyretype, 'fittedodometer': fittedodometer,
                'axlenumber': axlenumber, 'tyreposition': tyreposition, 'vehiclenumber': vehiclenumber, 'fitremarks': fitremarks, 'fitkms': fitkms, 'axilname': axilname
            };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        if (msg == "OK") {
                            alert("Tyre Issued Successfully");
                            //forclearall();
                            //errormags();
                            //getalldataforissuetyre();
                            for_fitted_clearall();
                            filltyres_sno_withposition();
                            get_removedtyres_data();
                        }
                        else {
                            alert(msg);
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
        function for_fitted_clearall() {
            document.getElementById('dtp_fitteddate').value = "";
            document.getElementById('slct_issuedtyresno').value = "Select Tyre";
            document.getElementById('Seslct_issuedtyretype').value = "Select Type";
            document.getElementById('txt_fittedodometer').value = "";
            document.getElementById('txt_fitremarks').value = "";
            document.getElementById('lbl_fitkms').innerHTML = "";
            for_removing_hideerrors();
        }
        function for_getting_kms_forfitting() {
            var issuedtyresno = document.getElementById('slct_issuedtyresno').value;
            var removedtyredno = document.getElementById('slct_removedtyresno').options.length;
            if (removedtyredno > 1) {
                alert("Already Tyre Fitted For this Position \n Please Select Another Position");
                document.getElementById('slct_issuedtyresno').value = "Select Tyre";
                return;
            }
            for (var i = 0; i < gettyres.length; i++) {
                if (gettyres[i].sno == issuedtyresno) {
                    if (gettyres[i].current_KMS == "") {
                        document.getElementById('lbl_fitkms').innerHTML = "0";
                    }
                    else {
                        document.getElementById('lbl_fitkms').innerHTML = gettyres[i].current_KMS;
                    }
                    break;
                }
            }
        }
        function for_diaplay_removetyre_kms() {
            if (document.getElementById('slct_removedtyresno').value == "Select Tyre") {
                alert("Please Select Removing Tyre First");
                slct_removedtyresno.focus();
                return;
            }
            var prevodo = document.getElementById('txt_prevodordng').value;
            var presentodo = document.getElementById('txt_Odometerreading').value;
            if (prevodo == "") {
                prevodo = "0";
            }
            if (presentodo == "") {
                presentodo = "0";
            }
            if (parseInt(presentodo) < parseInt(prevodo)) {
                alert("Present Odometer Must be greater than Previous Odometer");
                txt_Odometerreading.focus();
                return;
            }
            var kms = parseInt(presentodo) - parseInt(prevodo);
            var prevkms = document.getElementById('lbl_prevkms').innerHTML;
            var d = document.getElementById("Seslct_axlenumber");
            var ifstep = d.options[d.selectedIndex].text;
            if (ifstep == "Stephanie") {
                document.getElementById('lbl_kmstrvldthisvhl').innerHTML = 0;
                document.getElementById('lbl_diffkms').innerHTML = parseInt(prevkms);
            }
            else {
                document.getElementById('lbl_kmstrvldthisvhl').innerHTML = kms;
                var totalkms = parseInt(prevkms);
                document.getElementById('lbl_diffkms').innerHTML = totalkms;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
           Issue Tyre<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Issue Tyre</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Issue Tyre Details
                </h3>
            </div>
            <div class="box-body">
                <div style="padding: 20px;" id="second_div">
                    <div id="sameall_div">
                        <table align="center">
                            <tr>
                                <td>
                                    <label>
                                        Vehicle Number<span style="color: red;">*</span></label>
                                    <select id="Seslct_vehiclenumber" onchange="get_axils()" class="form-control" style="min-width: 150px;">
                                        <option selected disabled value="Select Vehicle">Select Vehicle</option>
                                    </select><label id="lbl_vehiclenumber_error_msg" class="errormessage">* Please Select
                                        Vehicle Number</label>
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    <label>
                                        Axil Number<span style="color: red;">*</span></label>
                                    <select id="Seslct_axlenumber" onchange="axle_onchange();" class="form-control" style="min-width: 150px;">
                                        <option selected disabled value="Select Axil">Select Axil</option>
                                    </select><label id="lbl_axlenumber_error_msg" class="errormessage">* Please Select Axle
                                        Number</label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Tyre Position<span style="color: red;">*</span></label>
                                    <select id="slct_tyreposition" onchange="filltyres_sno_withposition()" class="form-control"
                                        style="min-width: 150px;">
                                        <option selected disabled value="Select Tyre Position">Select Tyre Position</option>
                                    </select>
                                    <label id="lbl_tyrepos_error_msg" class="errormessage">
                                        * Please Select Tyre Position</label>
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                            <td>
                            <br />
                            </td>
                            </tr>
                        </table>
                    </div>
                    <div id="div_universal" class="panel panel-primary">
                        <div class="panel-heading" style="padding: 5px 15px;">
                            <h3 class="panel-title">
                                Tyre Removing</h3>
                        </div>
                        <div class="panel-body">
                            <table align="center">
                                <tr>
                                    <td>
                                        <label>
                                            Issued Date<span style="color: red;">*</span></label>
                                        <input type="text" id="dtp_issueddate" name="Date" placeholder="Date" class="form-control" />
                                        <label id="lbl_issueddate_error_msg" class="errormessage">
                                            * Please Select Issued Date</label>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <label>
                                            Removed Tyre Sno<span style="color: red;">*</span></label>
                                        <select id="slct_removedtyresno" class="form-control" style="min-width: 150px;" onchange="removetyre_onchange();">
                                            <option selected disabled value="Select Tyre">Select Tyre</option>
                                        </select><label id="lbl_removedtyresno_error_msg" class="errormessage">* Please Select
                                            Removed Tyre SerialNo</label>
                                      
                                    </td>
                                      <td style="width: 5px;">
                                    </td>
                                    <td>
                                    <label>
                                            Prev Odometer<span style="color: red;">*</span></label>
                                      <input id="txt_prevodordng" value="" class="form-control" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Removed Tyre Type<span style="color: red;">*</span></label>
                                        <select id="Seslct_removedtyretype" class="form-control" style="min-width: 150px;">
                                            <option value="Select Type" selected disabled>Select Type</option>
                                            <option value="Puncture">Puncture</option>
                                            <option value="Spare">Spare</option>
                                            <option value="Scrap">Scrap</option>
                                            <option value="Rebutton">Rebutton</option>
                                            <option value="Stephanie">Stephanie</option>
                                        </select><label id="lbl_removedtyretype_error_msg" class="errormessage">* Please Select
                                            Removed Tyre Type</label>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <label>
                                            Odometer Reading<span style="color: red;">*</span></label>
                                        <input type="text" id="txt_Odometerreading" maxlength="45" name="OdometerReading"
                                            class="form-control" placeholder="Odometer Reading" onblur="for_diaplay_removetyre_kms()"><label
                                                id="lbl_odoreading_error_msg" class="errormessage">* Please Enter Odometer Reading</label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Remarks</label>
                                        <input type="text" class="form-control" id="txt_remarks" maxlength="45" name="remarks"
                                            placeholder="Enter Remarks">
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <label>
                                            Total KMS Travelled</label>
                                        <label id="lbl_prevkms" style="display: none;">
                                        </label>
                                        <label class="form-control" id="lbl_diffkms" style="color: rgb(253, 5, 5); font-size: 19px;
                                            font-weight: bold;">
                                        </label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            KMS Travelled For This Vehicle</label>
                                        <label class="form-control" id="lbl_kmstrvldthisvhl" style="color: rgb(253, 5, 5);
                                            font-size: 19px; font-weight: bold;">
                                        </label>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <input id='save_removetyre' type="button" class="btn btn-primary" name="submit" value='Save Removing'
                                            onclick="for_tyre_removing_save()" />
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <input id='clear_removetyre' type="button" class="btn btn-danger" name="Close" value='Clear'
                                            onclick="for_removing_clearall()" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div id="div1" class="panel panel-primary">
                        <div class="panel-heading" style="padding: 5px 15px;">
                            <h3 class="panel-title">
                                Tyre Fitting</h3>
                        </div>
                        <div class="panel-body">
                            <table align="center">
                                <tr>
                                    <td>
                                        <label>
                                            Fitting Date<span style="color: red;">*</span></label>
                                        <input type="text" id="dtp_fitteddate" name="Date" placeholder="Date" class="form-control" />
                                        <label id="lbl_fittingdate_error_msg" class="errormessage">
                                            * Please Select Issued Date</label>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <label>
                                            Fitted Tyre Sno<span style="color: red;">*</span></label>
                                        <select id="slct_issuedtyresno" class="form-control" style="min-width: 150px;" onchange="for_getting_kms_forfitting()">
                                        </select><label id="lbl_fittingtyresno_error_msg" class="errormessage">* Please Select
                                            Fitted Tyre SerialNo</label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Fitted Tyre Type<span style="color: red;">*</span></label>
                                        <select id="Seslct_issuedtyretype" class="form-control" style="min-width: 150px;">
                                            <option value="Select Type" selected disabled>Select Type</option>
                                            <option value="Puncture">Puncture</option>
                                            <option value="Spare">Spare</option>
                                            <option value="Scrap">Scrap</option>
                                            <option value="Rebutton">Rebutton</option>
                                            <option value="Stephanie">Stephanie</option>
                                            <option value="Stephanie">Position Change</option>
                                            <option value="NewTyre">New Tyre</option>
                                        </select><label id="lbl_fittedtyretype_error_msg" class="errormessage">* Please Select
                                            Fitted Tyre Type</label>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <label>
                                            Odometer Reading<span style="color: red;">*</span></label>
                                        <input type="text" id="txt_fittedodometer" maxlength="45" name="OdometerReading"
                                            class="form-control" placeholder="Enter Odometer Reading"><label id="lbl_fitted_odoreading_error_msg"
                                                class="errormessage">* Please Enter Odometer Reading</label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Remarks</label>
                                        <input type="text" class="form-control" id="txt_fitremarks" maxlength="45" name="remarks"
                                            placeholder="Enter Remarks">
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <label>
                                            Total KMS Travelled</label>
                                        <label id="lbl_fitkms" class="form-control" style="color: rgb(253, 5, 5); font-size: 19px;
                                            font-weight: bold;">
                                        </label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <input id='save_fitting' type="button" class="btn btn-primary" name="submit" value='Save Fitting'
                                            onclick="for_tyre_fitting_save()" />
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <input id='clear_fitting' type="button" class="btn btn-danger" name="Close" value='Clear'
                                            onclick="for_fitted_clearall()" />
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

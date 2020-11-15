<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="AxilMaster.aspx.cs" Inherits="VehicleTypeMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <link href="opcss/bootextract.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="dist/css/bootstrap.css" />--%>
    <script src="js/utility.js" type="text/javascript"></script>
    <style type="text/css">
        div.sub_td:before
        {
            float: right;
        }
        div.sub_td:after
        {
            float: none;
        }
    </style>
    <script type="text/javascript">
        var options_val = "";
        $(function () {
            $("#staus_div").hide();
            $("#second_div").hide();
            retrive_vehtypes();
            get_tyresize();
            $('#add_axiltype').click(function (e) {
                e.preventDefault();
                $("#second_div").show();
                $("#first_div").hide();
                //$("#btn_save").val("Save");
                document.getElementById('btn_save').innerHTML = "Save";
                forclearall();
            });
            $('#btn_close').click(function (e) {
                e.preventDefault();
                $("#second_div").hide();
                $("#first_div").show();
                forclearall();
            });
        });
        function addnewaxil() {
            event.preventDefault();
            $("#second_div").show();
            $("#first_div").hide();
            //$("#btn_save").val("Save");
            document.getElementById('btn_save').innerHTML = "Save";
            forclearall();
        }
        function closeaxilmaster() {
            event.preventDefault();
            $("#second_div").hide();
            $("#first_div").show();
            forclearall();
        }
        //Function for only no
        $(document).ready(function () {
            $("#txt_axilno,.tyreno").keydown(function (event) {
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

        function axilonchange() {
            var noofaxils = document.getElementById('txt_axilno').value;
            var table = document.getElementById("tbl_axils");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            for (var i = 1; i <= noofaxils; i++) {
                $("#tbl_axils").append('<tr><th scope="row" style="border: 1px solid #1d96b2;">' + i + '</th><td style="border: 1px solid #1d96b2;" data-title="Axil Name"><input type="text" placeholder="Axle Name" class="axilname"/></td><td style="border: 1px solid #1d96b2;" data-title="No.of Tyres"><input step="2" min="0" type="number" onblur="onchange_row(this)" value="0" class="tyreno"/></td><td style="display:none;"><label class="axlesno"></label></td></tr><tr class="hide_row" style="display:none;"><th colspan="4" scope="row"><div class="sub_td" style="float:right;"></div></th></tr>');
            }
            $("#tbl_axils").append('<tr><th scope="row" style="border: 1px solid #1d96b2;">' + i + '</th><td style="border: 1px solid #1d96b2;" data-title="Axil Name"><input type="text" placeholder="Axle Name" value="Stephanie" disabled class="axilname"/></td><td style="border: 1px solid #1d96b2;" data-title="No.of Tyres"><input step="1" value="1" disabled min="0" type="number" class="tyreno"/></td><td style="display:none;"><label class="axlesno"></label></td></tr><tr><th colspan="4" scope="row"><div class="sub_td" style="float:right;">' +
            '<div class="stephanie"><input type="text" name="step_tyre_name" value="Stephanie" disabled class="form-control" placeholder="Step Tyre Name" /><select type="text" name="step_tyre_size" class="form-control">' + options_val + '</select><label class="step_tyre_position_sno" style="display:none;"></label></div></br>' +
            '</div></th></tr>');

            only_no();
        }

        function onchange_row(thisid) {
            var axilname = $(thisid).parent().parent().find(".axilname").val();
            if (axilname == "") {
                alert("Please Fill Axil Name");
                $(thisid).val("0");
                return;
            }
            var nooftyres = $(thisid).val();
            var half_tyres = parseInt(nooftyres) / 2;
            var next_row = $(thisid).parent().parent().next();
            if ($(next_row).css("display") != "none") {
                var confi = confirm("Tyres Data Will be lost for this axile./n Do you want to Continue?");
                if (confi) {
                    if (nooftyres % 2 == 0 && nooftyres != 0) {
                        $(next_row).show();
                        var results = '<table id="tbl_' + axilname + '" name="tyre_table"   class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info"><thead><tr><th scope="col">Right</th><th scope="col">Left</th></th></tr></thead><tbody>';

                        var right = '<td data-title="Right Tyre">';
                        var left = '<td data-title="Left Tyre">';
                        for (var i = 1; i <= half_tyres; i++) {
                            right += '<div class="right"><input type="text" name="right_tyre_name" class="form-control" placeholder="Right Tyre Name" /><select type="text" name="right_tyre_size" class="form-control"></select><label class="right_tyre_position_sno" style="display:none;"></label></div></br>';
                            left += '<div class="left"><input type="text" placeholder="Left Tyre Name" name="left_tyre_name" class="form-control"/><select type="text" name="left_tyre_size" class="form-control"></select><label class="left_tyre_position_sno" style="display:none;"></label></div></br>';
                        }
                        right += '</td>';
                        left += '</td>';
                        results += '<tr>' + right + '' + left + '</tr></tbody></table>';
                        $(next_row).find(".sub_td").html(results);
                        $(next_row).find('[name="right_tyre_size"]').append(options_val);
                        $(next_row).find('[name="left_tyre_size"]').append(options_val);
                    }
                    else {
                        alert("Enter multiples of two only /n examples:2,4,6,8.....");
                        $(next_row).hide();
                        $(next_row).find(".sub_td").html("");
                        $(thisid).val("0");
                    }
                }
            }
            else {
                if (nooftyres % 2 == 0 && nooftyres != 0) {
                    $(next_row).show();
                    var results = '<table id="tbl_' + axilname + '" name="tyre_table"   class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info"><thead><tr><th scope="col">Right</th><th scope="col">Left</th></th></tr></thead><tbody>';

                    var right = '<td data-title="Right Tyre">';
                    var left = '<td data-title="Left Tyre">';
                    for (var i = 1; i <= half_tyres; i++) {
                        right += '<div class="right"><input type="text" name="right_tyre_name" class="form-control" placeholder="Right Tyre Name" /><select type="text" name="right_tyre_size" class="form-control"></select></div></br>';
                        left += '<div class="left"><input type="text" placeholder="Left Tyre Name" name="left_tyre_name" class="form-control"/><select type="text" name="left_tyre_size" class="form-control"></select></div></br>';
                    }
                    right += '</td>';
                    left += '</td>';
                    results += '<tr>' + right + '' + left + '</tr></tbody></table>';
                    $(next_row).find(".sub_td").html(results);
                    $(next_row).find('[name="right_tyre_size"]').append(options_val);
                    $(next_row).find('[name="left_tyre_size"]').append(options_val);
                }
                else {
                    alert("Enter multiples of two only \n examples:2,4,6,8.....");
                    $(next_row).hide();
                    $(next_row).find(".sub_td").html("");
                    $(thisid).val("0");
                }
            }
        }
        function get_tyresize() {
            var minimaster = "TyreSize,VehicleMake,VehicleType";
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
            var vehmake = document.getElementById('txt_vehmake');
            var length = vehmake.options.length;
            document.getElementById('txt_vehmake').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Vehicle Make";
            opt.value = "Select Vehicle Make";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            vehmake.appendChild(opt);
            options_val = "<option value='Tyre Size' selected disabled>Tyre Size</option>";
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].mm_name != null && msg[i].mm_status != "0" && msg[i].mm_type == "TyreSize") {
                    options_val += "<option value='" + msg[i].sno + "'>" + msg[i].mm_name + "</option>";
                }
                if (msg[i].mm_name != null && msg[i].mm_status != "0" && msg[i].mm_type == "VehicleMake") {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].mm_name;
                    option.value = msg[i].sno;
                    vehmake.appendChild(option);
                }
            }

            var vehtype = document.getElementById('txt_vehname');
            var length = vehtype.options.length;
            document.getElementById('txt_vehname').options.length = null;
            var opt2 = document.createElement('option');
            opt2.innerHTML = "Select VehicleType";
            opt2.value = "Select VehicleType";
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
        function hide_error_msg() {
            $("#lbl_error_axilmstr").hide();
            $("#lbl_error_vehtype").hide();
            $("#lbl_error_vehmake").hide();
            $("#lbl_error_axils").hide();
        }       

        function for_Main_Saving1() {
            var vehtype_name = document.getElementById('txt_vehname').value;
            var d = document.getElementById('txt_vehmake');
            var vehmake_sno = d.options[d.selectedIndex].value;
            var no_axils = document.getElementById('txt_axilno').value;
            var btnval = document.getElementById('btn_save').innerHTML;
            var sno = document.getElementById('lbl_sno').innerHTML;
            var status = document.getElementById('cmb_status').value;
            var axil_mstr_name = document.getElementById('txt_axilmstr_name').value;
            var flag = false;
            if (axil_mstr_name == "") {
                $("#lbl_error_axilmstr").show();
                $('#txt_axilmstr_name').focus();
                flag = true;
            }
            if (vehtype_name == "Select VehicleType") {
                $("#lbl_error_vehtype").show();
                $('#txt_vehname').focus();
                flag = true;
            }
            if (vehmake_sno == "Select Vehicle Make") {
                $("#lbl_error_vehmake").show();
                flag = true;
            }
            if (no_axils == "") {
                $("#lbl_error_axils").show();
                $('#txt_axilno').focus();
                flag = true;
            }
            if (flag) {
                return;
            }
            var axil_table = [];
            $('#tbl_axils> tbody > tr:even').each(function () {
                var innertable_array = [];
                var axilname = $(this).find(".axilname").val();
                var noof_tyres = $(this).find(".tyreno").val();
                var axlesno = $(this).find(".axlesno").html();
                var ranking = $(this).find('th').text();
                var next_row = $(this).next();
                var tble = $(next_row).find("[name='tyre_table']");
                if (axilname != "") {
                    $(next_row).find('#"tbl_' + axilname + '" > tbody > tr').each(function () {
                        $(this).find('.right').each(function (i, obj) {
                            var right_tyre_name = $(this).find('[name="right_tyre_name"]').val();
                            var right_tyre_size_sno = $(this).find('[name="right_tyre_size"]').val();
                            var right_tyre_position_sno = $(this).find('.right_tyre_position_sno').html();
                            innertable_array.push({ 'tyre_name': right_tyre_name, 'tyre_size_sno': right_tyre_size_sno, 'Side': "R", 'tyre_position_sno': right_tyre_position_sno });
                        });
                        $(this).find('.left').each(function (i, obj) {
                            var left_tyre_name = $(this).find('[name="left_tyre_name"]').val();
                            var left_tyre_size_sno = $(this).find('[name="left_tyre_size"]').val();
                            var left_tyre_position_sno = $(this).find('.left_tyre_position_sno').html();
                            innertable_array.push({ 'tyre_name': left_tyre_name, 'tyre_size_sno': left_tyre_size_sno, 'Side': "L", 'tyre_position_sno': left_tyre_position_sno });
                        });

                    });
                    var step_tyre_name = $(next_row).find('[name=step_tyre_name]').val();
                    var step_tyre_size = $(next_row).find('[name=step_tyre_size]').val();
                    var step_tyre_position_sno = $(next_row).find('.step_tyre_position_sno').html();
                    if (step_tyre_name == "Stephanie") {
                        innertable_array.push({ 'tyre_name': step_tyre_name, 'tyre_size_sno': step_tyre_size, 'Side': "S", 'tyre_position_sno': step_tyre_position_sno });
                    }
                    axil_table.push({ 'axilname': axilname, 'noof_tyres': noof_tyres, 'axlesno': axlesno, 'innertable_array': innertable_array, 'ranking': ranking });
                }
            });
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
                                var Data = { 'op': 'save_edit_Axils', 'vehtype_name': vehtype_name, 'vehmake_sno': vehmake_sno, 'no_axils': no_axils, 'btnval': btnval, 'sno': sno, 'status': status, 'axil_mstr_name': axil_mstr_name };
                                var s = function (msg) {
                                    if (msg) {
                                        alert(msg);
                                        retrive_vehtypes();
                                        $("#second_div").hide();
                                        $("#first_div").show();
                                        //$("#btn_save").val("Save");
                                        document.getElementById('btn_save').innerHTML == "Save";
                                        forclearall();
                                        hide_error_msg();
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


        var Axil_Tyres_Name = [];
        function AxilTyresName(side, val, Fc) {
            if (side == "R") {
                if (val == 2) {
                    Axil_Tyres_Name[0] = "R-" + Fc + "-IN";
                    Axil_Tyres_Name[1] = "R-" + Fc + "-OUT";

                }
                else if (val == 3) {
                    Axil_Tyres_Name[0] = "R-" + Fc + "-IN";
                    Axil_Tyres_Name[1] = "R-" + Fc + "-MID";
                    Axil_Tyres_Name[2] = "R-" + Fc + "-OUT";
                }
                else if (val == 4) {
                    Axil_Tyres_Name[0] = "R-" + Fc + "-IN";
                    Axil_Tyres_Name[1] = "R-" + Fc + "-MID1";
                    Axil_Tyres_Name[2] = "R-" + Fc + "-MID2";
                    Axil_Tyres_Name[3] = "R-" + Fc + "-OUT";
                }
                else {
                    alert('Value Incorrect');
                }

            }
            else if (side == "L") {
                if (val == 2) {
                    Axil_Tyres_Name[0] = "L-" + Fc + "-IN";
                    Axil_Tyres_Name[1] = "L-" + Fc + "-OUT";
                }
                else if (val == 3) {
                    Axil_Tyres_Name[0] = "L-" + Fc + "-IN";
                    Axil_Tyres_Name[1] = "L-" + Fc + "-MID";
                    Axil_Tyres_Name[2] = "L-" + Fc + "-OUT";
                }
                else if (val == 4) {
                    Axil_Tyres_Name[0] = "L-" + Fc + "-IN";
                    Axil_Tyres_Name[1] = "L-" + Fc + "-MID1";
                    Axil_Tyres_Name[2] = "L-" + Fc + "-MID2";
                    Axil_Tyres_Name[3] = "L-" + Fc + "-OUT";
                }
                else {
                    alert('Value Incorrect');
                }

            }
            else {
                alert('Value Incorrect');
            }

        }


        function for_Main_Saving() {

            var vehtype_name = document.getElementById('txt_vehname').value;
            var d = document.getElementById('txt_vehmake');
            var vehmake_sno = d.options[d.selectedIndex].value;
            var no_axils = document.getElementById('txt_axilno').value;
            var btnval = document.getElementById('btn_save').innerHTML;
            var sno = document.getElementById('lbl_sno').innerHTML;
            var status = document.getElementById('cmb_status').value;
            var axil_mstr_name = document.getElementById('txt_axilmstr_name').value;
            var flag = false;
            if (axil_mstr_name == "") {
                $('#lbl_error_axilmstr').show();
                $('txt_axilmstr_name').focus();
                flag = true;
            }
            if (vehtype_name == "Select VehicleType") {
                $("#lbl_error_vehtype").show();
                $('#txt_vehname').focus();
                flag = true;
            }
            if (vehmake_sno == "Select Vehicle Make") {
                $("#lbl_error_vehmake").show();
                flag = true;
            }
            if (no_axils == "") {
                $("#lbl_error_axils").show();
                $('#txt_axilno').focus();
                flag = true;
            }
            if (flag) {
                return;
            }
            var axil_table = [];
            for (var k = 0; k < no_axils.length; k++) {
                $('#tbl_axils> tbody > tr:even').each(function () {
                    var innertable_array = [];
                    var axilname = $(this).find(".axilname").val();
                    var noof_tyres = $(this).find(".tyreno").val();
                    var axlesno = $(this).find(".axlesno").html();
                    var ranking = $(this).find('th').text();
                    var next_row = $(this).next();
                    var ArrAname = [];
                    var Aname = axilname;
                    Aname = Aname.charAt(0);
                   // alert(Aname);
                    // var tble = $(next_row).find("[name='tyre_table']");
                    if (axilname != "") {
                        
                        $(next_row).find('#tbl_' + axilname + ' > tbody > tr').each(function () {

                            $(next_row).find('.right').each(function (i, obj) {
                                var RLop = parseInt(noof_tyres) / 2;
                                if (RLop == "1") {
                                    var RTN = axilname + ' ' + "RIGHT";
                                    var right_tyre_name = RTN;
                                    var right_tyre_size_sno = $(this).find('[name="right_tyre_size"]').val();
                                    var right_tyre_position_sno = $(this).find('.right_tyre_position_sno').html();
                                    innertable_array.push({ 'tyre_name': right_tyre_name, 'tyre_size_sno': right_tyre_size_sno, 'Side': "R", 'tyre_position_sno': right_tyre_position_sno });
                                }
                                else {
                                    AxilTyresName('R', RLop, Aname);
                                   // for (var i = 0; i < RLop; i++) {
                                        var right_tyre_name = Axil_Tyres_Name[i];
                                        var right_tyre_size_sno = $(this).find('[name="right_tyre_size"]').val();
                                        var right_tyre_position_sno = $(this).find('.right_tyre_position_sno').html();
                                        innertable_array.push({ 'tyre_name': right_tyre_name, 'tyre_size_sno': right_tyre_size_sno, 'Side': "R", 'tyre_position_sno': right_tyre_position_sno });
                                   // }
                                }

                            });
                            $(next_row).find('.left').each(function (i, obj) {
                                var LLop = parseInt(noof_tyres) / 2;
                                if (LLop == "1") {
                                    var LTN = axilname + ' ' + "LEFT";
                                    var left_tyre_name = LTN;
                                    var left_tyre_size_sno = $(this).find('[name="left_tyre_size"]').val();
                                    var left_tyre_position_sno = $(this).find('.left_tyre_position_sno').html();
                                    innertable_array.push({ 'tyre_name': left_tyre_name, 'tyre_size_sno': left_tyre_size_sno, 'Side': "L", 'tyre_position_sno': left_tyre_position_sno });
                                }
                                else {
                                    AxilTyresName('L', LLop, Aname);
                                   // for (var i = 0; i < LLop; i++) {
                                        var left_tyre_name = Axil_Tyres_Name[i];
                                        var left_tyre_size_sno = $(this).find('[name="left_tyre_size"]').val();
                                        var left_tyre_position_sno = $(this).find('.left_tyre_position_sno').html();
                                        innertable_array.push({ 'tyre_name': left_tyre_name, 'tyre_size_sno': left_tyre_size_sno, 'Side': "L", 'tyre_position_sno': left_tyre_position_sno });
                                  //  }
                                }

                            });

                        });
                        var step_tyre_name = $(next_row).find('[name=step_tyre_name]').val();
                        var step_tyre_size = $(next_row).find('[name=step_tyre_size]').val();
                        var step_tyre_position_sno = $(next_row).find('.step_tyre_position_sno').html();
                        if (step_tyre_name == "Stephanie") {
                            innertable_array.push({ 'tyre_name': step_tyre_name, 'tyre_size_sno': step_tyre_size, 'Side': "S", 'tyre_position_sno': step_tyre_position_sno });
                        }
                        axil_table.push({ 'axilname': axilname, 'noof_tyres': noof_tyres, 'axlesno': axlesno, 'innertable_array': innertable_array, 'ranking': ranking });
                    }

                });
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
                                var Data = { 'op': 'save_edit_Axils', 'vehtype_name': vehtype_name, 'vehmake_sno': vehmake_sno, 'no_axils': no_axils, 'btnval': btnval, 'sno': sno, 'status': status, 'axil_mstr_name': axil_mstr_name };
                                var s = function (msg) {
                                    if (msg) {
                                        alert(msg);
                                        retrive_vehtypes();
                                        $("#second_div").hide();
                                        $("#first_div").show();
                                        //$("#btn_save").val("Save");
                                        document.getElementById('btn_save').innerHTML == "Save";
                                        forclearall();
                                        hide_error_msg();
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

        function retrive_vehtypes() {
            var k = 0;
            var colorue = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            var table = document.getElementById("tbl_vehtype");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var data = { 'op': 'get_only_axilmaster' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        for (var i = 0; i < msg.length; i++) {
                            var status = "";
                            if (msg[i].v_ty_status == "1") {
                                status = "Enabled";
                            }
                            else {
                                status = "Disabled";
                            }
                            $("#tbl_vehtype").append('<tr style="background-color:' + colorue[k] + '">' +
                            //'<td scope="row">' + msg[i].axilmaster_name + '</td>' +
                            '<td style="font-weight: 600;"><i class="fa  fa-life-bouy" aria-hidden="true"></i>&nbsp;<span id="0">' + msg[i].axilmaster_name + '</span></td>' + 
                            '<td data-title="Vehicle Type">' + msg[i].v_ty_name + '</td>' +
                            '<td data-title="Vehicle Make">' + msg[i].v_ty_make + '</td>' +
                            '<td  data-title="No.of Axles">' + msg[i].no_of_axles + '</td>' +
                            '<td  data-title="Status">' + status + '</td>' +
                            '<td style="display:none;">' + msg[i].sno + '</td>' +
                            //'<td><input type="button" class="btn btn-primary" name="Update" value ="Modify" onclick="updateclick(this);"/></td></tr>');
                            '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls" name="Update" value ="Modify"   onclick="updateclick(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>');
                            k = k + 1;
                            if (k == 4) {
                                k = 0;
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

        function updateclick(thisid) {
            var row = $(thisid).parents('tr');
            var axil_master_name = $(thisid).parent().parent().find('#0').text();
            var vehicle_type = row[0].cells[1].innerHTML;
            var make = row[0].cells[2].innerHTML;
            var noof_axils = row[0].cells[3].innerHTML;
            var sno = row[0].cells[5].innerHTML;
            var statuscode = row[0].cells[4].innerHTML;
            var status = "";
            if (statuscode == "Enabled") {
                status = "1";
            }
            else {
                status = "0";
            }
            $("select#txt_vehname option").each(function () { this.selected = (this.text == vehicle_type); });
            $("select#txt_vehmake option").each(function () { this.selected = (this.text == make); });
            document.getElementById("txt_axilmstr_name").value = axil_master_name;
            document.getElementById("txt_axilno").value = noof_axils;
            document.getElementById("lbl_sno").innerHTML = sno;
            document.getElementById("cmb_status").value = status;
            $("#staus_div").show();
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
                                main_results += '<tr><th scope="row" style="border: 1px solid #1d96b2;">' + (i + 1) + '</th><td style="border: 1px solid #1d96b2;" data-title="Axil Name"><input type="text" placeholder="Axle Name" value="' + msg[i].AxileName + '" class="axilname"/></td><td style="border: 1px solid #1d96b2;" data-title="No.of Tyres"><input step="2" min="0" type="number" disabled onblur="onchange_row(this)" value="' + msg[i].nooftyresperaxle + '" class="tyreno"/></td><td style="display:none;"><label class="axlesno">' + msg[i].veh_typ_axel_sno + '</label></td></tr><tr class="hide_row"><th colspan="4" scope="row"><div class="sub_td" >';
                                main_results += '<table id="tbl_' + msg[i].AxileName + '" name="tyre_table"   class="table table-bordered table-hover dataTable no-footer"  role="grid" aria-describedby="example2_info"><thead><tr><th scope="col">Right</th><th scope="col">Left</th></tr></thead><tbody>';
                                right += '<td data-title="Right Tyre">';
                                left += '<td data-title="Left Tyre">';
                                for (var j = 0; j < msg[i].tyredata.length; j++) {

                                    if (msg[i].tyredata[j].side == "R") {
                                        right += '<div class="right" style="float:left;width: 35%;margin: 0% 2% 0% 0%;"><div class="tyreimage"><img src="images/tyre.jpg" /></div></br><input type="text" name="right_tyre_name" class="form-control" value="' + msg[i].tyredata[j].tyre_name + '" placeholder="Right Tyre Name" /><select name="right_tyre_size" class="form-control">' + options_val + '</select><label class="right_tyre_position_sno" style="display:none;">' + msg[i].tyredata[j].tyre_position_sno + '</label></div>';

                                    }
                                    if (msg[i].tyredata[j].side == "L") {
                                        left += '<div class="left" style="float:left;width: 35%;margin: 0% 2% 0% 0%;"><div class="tyreimage"><img src="images/tyre.jpg" /></div></br><input type="text" placeholder="Left Tyre Name" name="left_tyre_name" value="' + msg[i].tyredata[j].tyre_name + '" class="form-control"/><select name="left_tyre_size" class="form-control">' + options_val + '</select><label class="left_tyre_position_sno" style="display:none;">' + msg[i].tyredata[j].tyre_position_sno + '</label></div>';
                                    }
                                }
                                right += '</td>';
                                left += '</td>';
                                main_results += '<tr>' + right + '' + left + '</tr>';
                                main_results += '</tbody></table>';
                                main_results += '</div></th></tr>';
                            }
                            if (msg[i].AxileName == "Stephanie") {
                                main_results += '<tr><th scope="row" style="border: 1px solid #1d96b2;">' + (i + 1) + '</th><td style="border: 1px solid #1d96b2;" data-title="Axil Name"><input type="text" placeholder="Axle Name" value="Stephanie" disabled class="axilname"/></td><td style="border: 1px solid #1d96b2;" data-title="No.of Tyres"><input step="1" value="1" disabled min="0" type="number" class="tyreno"/></td><td style="display:none;"><label class="axlesno">' + msg[i].veh_typ_axel_sno + '</label></td></tr><tr><th colspan="4" scope="row"><div class="sub_td" style="float:right;">';
                                for (var n = 0; n < msg[i].tyredata.length; n++) {
                                    if (msg[i].tyredata[n].side == "S") {
                                        main_results += '<div class="stephanie"><div class="tyreimage"><img src="images/tyre.jpg" /></div></br><input type="text" name="step_tyre_name" value="Stephanie" disabled class="form-control" placeholder="Step Tyre Name" /><select type="text" name="step_tyre_size" class="form-control">' + options_val + '</select><label class="step_tyre_position_sno" style="display:none;">' + msg[i].tyredata[n].tyre_position_sno + '</label></div></br>';
                                    }
                                }
                                main_results += '</div></th></tr>';
                            }
                        }
                        $("#tbl_axils").append(main_results);
                        for (var k = 0; k < msg.length; k++) {
                            $('#tbl_axils> tbody > tr:even').each(function () {
                                var axilname = $(this).find(".axilname").val();
                                var noof_tyres = $(this).find(".tyreno").val();
                                var next_row = $(this).next();
                                if (msg[k].AxileName == axilname) {
                                    for (var l = 0; l < msg[k].tyredata.length; l++) {
                                        //var next_row = $(this).next();
                                    $(next_row).find('#tbl_' + axilname + ' > tbody > tr').each(function () {
                                            if (msg[k].tyredata[l].side == "R") {
                                                $(next_row).find('.right').each(function (i, obj) {
                                                    var right_tyre_name = $(this).find('[name="right_tyre_name"]').val();
                                                    if (right_tyre_name == msg[k].tyredata[l].tyre_name) {
                                                        $(this).find('[name="right_tyre_size"]').val(msg[k].tyredata[l].tyre_size);
                                                        return false;
                                                    }
                                                });
                                            }
                                            if (msg[k].tyredata[l].side == "L") {
                                                $(next_row).find('.left').each(function (i, obj) {
                                                    var left_tyre_name = $(this).find('[name="left_tyre_name"]').val();
                                                    if (left_tyre_name == msg[k].tyredata[l].tyre_name) {
                                                        $(this).find('[name="left_tyre_size"]').val(msg[k].tyredata[l].tyre_size);
                                                        return false;
                                                    }
                                                });
                                            }

                                        });
                                        if (msg[k].tyredata[l].side == "S") {
                                            var step_tyre_name = $(next_row).find('[name="step_tyre_name"]').val();
                                            if (step_tyre_name == msg[k].tyredata[l].tyre_name) {
                                                $(next_row).find('[name="step_tyre_size"]').val(msg[k].tyredata[l].tyre_size);
                                                return false;
                                            }
                                        }
                                    }
                                    return false;
                                }
                            });
                        }
                        document.getElementById('txt_axilno').disabled = true;
                        $("#second_div").show();
                        $("#first_div").hide();
                        //$("#btn_save").val("Modify");
                        document.getElementById('btn_save').innerHTML = "Modify";
                    }
                    else {
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
            $("#second_div").show();
            $("#first_div").hide();
            //$("#btn_save").val("Modify");
            document.getElementById('btn_save').innerHTML = "Modify";
        }

        function forclearall() {
            document.getElementById("txt_axilmstr_name").value = "";
            document.getElementById("txt_vehname").value = "Select VehicleType";
            document.getElementById("txt_vehmake").value = "Select Vehicle Make";
            document.getElementById("txt_axilno").value = "";
            document.getElementById("lbl_sno").innerHTML = "";
            document.getElementById("cmb_status").value = "1";
            document.getElementById('txt_axilno').disabled = false;
            var table = document.getElementById("tbl_axils");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            $("#staus_div").hide();
            hide_error_msg();
            document.getElementById("btn_save").innerHTML = "Save";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form role="form">
    <section class="content-header">
        <h1>
            Axil Master<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Axil Master</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Axil Master Details
                </h3>
            </div>
            <div class="box-body">
                <div style="width: 100%; height: 100%;">
                    <div id="first_div">
                        <div id="div_vehtype" style="text-align: center;">
                            <%--<input id="add_axiltype" type="button" class="btn btn-primary" name="submit" value='Add New Axil Master' />--%>
                            <div class="input-group" style="width: 100px;padding-left: 960px;">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addnewaxil()"></span><span id="add_axiltype" onclick="addnewaxil()">Add Axil</span>
                          </div>
                          </div>
                    <br />
                        </div>
                        <div id="div_vehtype_table" style="background: #fff;">
                            <table id="tbl_vehtype"  class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">
                                <thead>
                                    <tr style="background:#5aa4d0; color: white; font-weight: bold;">
                                        <th scope="col">
                                            Axil Master Name
                                        </th>
                                        <th scope="col">
                                            Vehicle Type
                                        </th>
                                        <th scope="col">
                                            Vehicle Make
                                        </th>
                                        <th scope="col">
                                            No Of Axils
                                        </th>
                                        <th scope="col">
                                            Status
                                        </th>
                                        <th scope="col" style="display: none;">
                                            sno
                                        </th>
                                        <th scope="col">
                                        </th>
                                    </tr>
                                </thead>    
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div id="second_div">
                        <div style="padding: 20px; text-align: center;">
                        <table align="center">
                        <tr>
                        <td>
                         <label>
                                        Axil Master Name <span style="color: red;">*</span></label>
                                    <input type="text" class="form-control" id="txt_axilmstr_name" placeholder="Axil Master Name">
                                    <label id="lbl_error_axilmstr" class="errormessage">
                                        Please Enter Axil Master Name</label>
                        </td>
                        <td style="width:5px;"></td>
                        <td>
                        <label>
                                        Vehicle Type Name <span style="color: red;">*</span></label>
                                    <select id="txt_vehname" class="form-control" style="min-width: 200px;">
                                        <option disabled selected value="Select VehicleType">Select VehicleType</option>
                                    </select>
                                    <label id="lbl_error_vehtype" class="errormessage">
                                        Please select VehicleType</label>
                        </td>
                            <tr>
                                <td>
                                 <label>
                                        Vehicle Make <span style="color: red;">*</span></label>
                                    <select id="txt_vehmake" class="form-control">
                                        <option disabled selected value="Select Vehicle Make">Select Vehicle Make</option>
                                    </select>
                                    <label id="lbl_error_vehmake" class="errormessage">
                                        Please select Vehicle Make</label>
                                </td>
                                <td style="width:5px;"></td>
                                <td>
                                 <label>
                                        No Of Axils <span style="color: red;">*</span></label>
                                    <input id="txt_axilno" min="0" class="form-control" type="number" onblur="axilonchange()" />
                                    <label id="lbl_error_axils" class="errormessage">
                                        Please Enter No of Axils</label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                 <label>
                                        Status</label>
                                    <select id="cmb_status" class="form-control">
                                        <option value="1">Enable</option>
                                        <option value="0">Disable</option>
                                    </select>
                                    <label id="lbl_sno" type="date" style="display: none;">
                                    </label>
                                </td>
                                <td style="width: 5px;">
                                </td>
                            </tr>
                        </table>
                            <div id="div_axilautofill" align="center">
                                <table id="tbl_axils"  class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info" style="width: 75%;">
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
                            <div align="center">
                                <%--<input id="btn_save" type="button" class="btn btn-primary" value="Save" onclick="for_Main_Saving()" />
                                <input id="btn_close" type="button" class="btn btn-danger" value="Close" />--%>
                                <table>
                                <tr>
                                <td>
                                </td>
                                <td>
                                    <div class="input-group">
                                        <div class="input-group-addon">
                                        <span class="glyphicon glyphicon-ok" id="btn_saves" onclick="for_Main_Saving()"></span><span id="btn_save" onclick="for_Main_Saving()">Save</span>
                                  </div>
                                  </div>
                                    </td>
                                    <td style="width:10px;"></td>
                                    <td>
                                     <div class="input-group">
                                        <div class="input-group-close">
                                        <span class="glyphicon glyphicon-remove" id='btn_closes' onclick="closeaxilmaster()"></span><span id='btn_close' onclick="closeaxilmaster()">Close</span>
                                  </div>
                                  </div>
                                    </td>
                                    </tr>
                                    </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    </form>
</asp:Content>

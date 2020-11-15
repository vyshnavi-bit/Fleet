<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="TyreTransfer.aspx.cs" Inherits="TyreTransfer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<script src="js/utility.js" type="text/javascript"></script>

<script type="text/javascript">
    $(function () {
        getdepartment();
        get_tyres();
        only_no();
        get_branches();
    });
        function only_no() {
            $("#txt_noof_tyres").keydown(function (event) {
                // Allow: backspace, delete, tab, escape, and enter
                if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 || event.keyCode == 190 || event.keyCode == 110 ||
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

        var TyreBrand = "";
        var TyreSize = "";
        var TyreType = "";
        var VehicleMake = "";

        function getdepartment() {
            var minimaster = "TyreBrand,TyreSize,TyreType,VehicleMake";
            var data = { 'op': 'get_Mini_Master_data', 'minimaster': minimaster };
            var s = function (msg) {
                if (msg) {
                    TyreBrand = "";
                    TyreSize = "";
                    TyreType = "";
                    VehicleMake = "";
                    if (msg.length > 0) {
                        for (var i = 0; i < msg.length; i++) {
                            if (msg[i].mm_name != null && msg[i].mm_status != "0" && msg[i].mm_type == "TyreBrand") {
                                TyreBrand += "<option value=" + msg[i].sno + ">" + msg[i].mm_name + "</option>";
                            }
                            if (msg[i].mm_name != null && msg[i].msg != "0" && msg[i].mm_type == "TyreSize") {
                                TyreSize += "<option value=" + msg[i].sno + ">" + msg[i].mm_name + "</option>";
                            }
                            if (msg[i].mm_name != null && msg[i].mm_status != "0" && msg[i].mm_type == "TyreType") {
                                TyreType += "<option value=" + msg[i].sno + ">" + msg[i].mm_name + "</option>";
                            }
                            if (msg[i].mm_name != null && msg[i].mm_status != "0" && msg[i].mm_type == "VehicleMake") {
                                VehicleMake += "<option value=" + msg[i].sno + ">" + msg[i].mm_name + "</option>";
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

        var get_tyres_data = "";
        var tyres_data = [];
        function get_tyres() {
            var data = { 'op': 'get_tyres_new' };
            var s = function (msg) {
                if (msg) {
                    get_tyres = "";

                    if (msg.length > 0) {
                        tyres_data = msg;
                        for (var i = 0; i < msg.length; i++) {
                            if (msg[i].tyre_sno != null) {
                                get_tyres_data += "<option>" + msg[i].tyre_sno + "</option>";
                            }
                        }
                        document.getElementById('tyres_list').innerHTML = get_tyres_data;
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

        function generate_rows() {
            var no = document.getElementById('txt_noof_tyres').value;

            for (var i = 1; i <= no; i++) {
                $("#tyres_table").append('<tr><td>' + i + '</td><td><input name="tyre_sno" list="tyres_list" onchange="selectfromthis(this)" style="width:150px;" class="form-control" type="text" placeholder="Tyre_sno" /></td>' +
            '<td><select name="Brand" class="form-control" style="width:100px;" disabled>' + TyreBrand + '</select></td>' +
            '<td><select name="Type_of_Tyre" class="form-control" style="width:100px;" disabled>' + TyreType + '</select></td>' +
            '<td><select name="Tube_Type" class="form-control" style="width:100px;" disabled><option value="TubeLess">TubeLess</option><option value="Tube">Tube</option></select></td>' +
            '<td><select name="Size" class="form-control"style="width:100px;" disabled>' + TyreSize + '</select></td>' +
            '<td><input name="Remarks" class="form-control" type="text" placeholder="Remarks" style="width:150px;"/></td></tr>');
            }
            $("#morerows_div").show();
            document.getElementById('txt_noof_tyres').disabled = true;
            if (no == "") {
                document.getElementById('txt_noof_tyres').disabled = false;
            }
            only_no();
        }
        function addmore() {
            document.getElementById('txt_noof_tyres').disabled = true;
            var no = document.getElementById('txt_noof_tyres').value;
            if (no == "") {
                no = "0";
            }
            $("#tyres_table").append('<tr><td>' + (parseInt(no) + 1) + '</td><td><input name="tyre_sno" list="tyres_list" onchange="selectfromthis(this)" style="width:150px;" class="form-control" type="text" placeholder="Tyre_sno" /></td>' +
            '<td><select name="Brand" class="form-control" style="width:100px;">' + TyreBrand + '</select></td>' +
            '<td><select name="Type_of_Tyre" class="form-control" style="width:100px;">' + TyreType + '</select></td>' +
            '<td><select name="Tube_Type" class="form-control" style="width:100px;"><option value="TubeLess">TubeLess</option><option value="Tube">Tube</option></select></td>' +
            '<td><select name="Size" class="form-control"style="width:100px;">' + TyreSize + '</select></td>' +
            '<td><input name="Remarks" class="form-control" type="text" placeholder="Remarks" style="width:150px;"/></td></tr>');
            document.getElementById('txt_noof_tyres').value = parseInt(no) + 1;
            only_no();
        }
        function selectfromthis(thisid) {
            var tyrename = $(thisid).val();
            var x = document.getElementById('tyres_list');
            var isthere = false;
            for (var i = 0; i < x.options.length; i++) {
                if (tyrename == x.options[i].value) {
                    isthere = true;
                    break;
                }
            }
            for (var i = 0; i < tyres_data.length; i++) {
                if (tyres_data[i].tyre_sno == tyrename) {
                    $(thisid).parent().parent().children().find("[name=Brand] option[value='" + tyres_data[i].brand + "']").attr("selected", "selected");
                    $(thisid).parent().parent().children().find("[name=Type_of_Tyre] option[value='" + tyres_data[i].type_of_tyre + "']").attr("selected", "selected");
                    $(thisid).parent().parent().children().find("[name=Tube_Type] option[value='" + tyres_data[i].tyre_tube + "']").attr("selected", "selected");
                    $(thisid).parent().parent().children().find("[name=Size] option[value='" + tyres_data[i].size + "']").attr("selected", "selected");
                }
            }

            if (!isthere) {
                alert("Please Select From Given Tyres Only");
                $(thisid).focus();
                $(thisid).val("");
                return;
            }
        }

        function get_branches() {
            var data = { 'op': 'get_all_BranchName_data' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillbranchdata(msg);
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
        function fillbranchdata(msg) {
            var partgroup = document.getElementById('slct_branchname');
            var length = partgroup.options.length;
            document.getElementById('slct_branchname').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Branch";
            opt.value = "Select Branch";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            partgroup.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].branchname != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].branchname;
                    option.value = msg[i].brnch_sno;
                    partgroup.appendChild(option);
                }
            }

        }




        function save_tyre_click() {

            var branchname = document.getElementById('slct_branchname').value;
            var date = document.getElementById('txt_date').value;
            var remarks = document.getElementById('txt_remarks').value;
            var sendby = document.getElementById('txt_sendby').value;

            var tyres = [];


            $('#tyres_table> tbody > tr').each(function () {
                var tyresno = $(this).find('[name=tyre_sno]').val();
                var Brand = $(this).find('[name=Brand] :selected').val();
                var Type_of_Tyre = $(this).find('[name=Type_of_Tyre] :selected').val();
                var Tube_Type = $(this).find('[name=Tube_Type] :selected').val();
                var Size = $(this).find('[name=Size] :selected').val();
                var Remarks = $(this).find('[name=Remarks]').val();
                tyres.push({ 'tyresno': tyresno, 'Brand': Brand, 'Type_of_Tyre': Type_of_Tyre, 'Tube_Type': Tube_Type, 'Size': Size, 'Remarks': Remarks });
            });

            var flag = false;

            if (branchname == "Select Branch") {
                $("#lbl_error_branch").show();
                flag = true;
            }
            if (date == "") {
                $("#lbl_errror_date").show();
                flag = true;
            }
            if (sendby == "") {
                $("#lbl_error_sendby").show();
                flag = true;
            }
            if (flag) {
                return;
            }

            var data = { 'op': 'Tyres_save_start' };
            var s = function (msg) {
                if (msg) {
                    for (var i = 0; i < tyres.length; i++) {
                        var Data = { 'op': 'Tyres_save_RowData', 'row_detail': tyres[i], 'end': 'N' };
                        if (i == tyres.length - 1) {
                            Data = { 'op': 'Tyres_save_RowData', 'row_detail': tyres[i], 'end': 'Y' };
                        }
                        var s = function (msg) {
                            if (msg == 'Y') {

                                var Data = { 'op': 'save_edit_TyresTransfer', 'branchname': branchname, 'date': date, 'remarks': remarks, 'sendby': sendby };
                                var s = function (msg) {
                                    if (msg) {
                                        alert(msg);
                                        hide_error_logs();
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

        function hide_error_logs() {
            $("#lbl_error_branch").hide();
            $("#lbl_errror_date").hide();
            $("#lbl_error_sendby").hide();
        }


        function resetall() {
            document.getElementById('slct_branchname').value = "Select Branch";
            document.getElementById('txt_date').value = "";
            document.getElementById('txt_remarks').value = "";
            document.getElementById('txt_sendby').value = "";
            document.getElementById('txt_noof_tyres').value = "";
            document.getElementById('save_tyre').value = "Save";
            document.getElementById('txt_noof_tyres').disabled = false;
            var table = document.getElementById("tyres_table");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            $("#morerows_div").hide();
            hide_error_logs();

        }



        </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div id='vehmaster_fillform' style=" padding: 20px;">
        <div class="row">
            <div class="form-group">
          <datalist id="tyres_list"></datalist>
                <label>
                    Branch<span style="color: red;">*</span></label>
                <select id="slct_branchname" class="form-control" style="min-width: 195px;">
                    <option selected disabled value="Select Branch">Select Branch</option>
                </select><label id="lbl_error_branch" class="errormessage">* Please Selet Branch Name</label>
            </div>
            <div class="form-group">
                <label>
                    Date<span style="color: red;">*</span></label>
                <input id="txt_date" class="form-control" type="date">
                <label id="lbl_errror_date" class="errormessage">* Please Selet Transferring Date</label>
            </div>
            <div class="form-group">
                <label>
                    Remarks<span style="color: red;">*</span></label>
                <textarea id="txt_remarks" class="form-control" placeholder="Remarks" cols="30" rows="2"></textarea>
            </div>
            <div class="form-group">
                <label>
                   No of Tyres</label>
                <input id="txt_noof_tyres" class="form-control" type="text" name="vendorcode" placeholder="No of Tyres" onblur="generate_rows()"/>
            </div>
            </div>
            <div class="row">
            <div class="form-group">
                <label>
                    Send By<span style="color: red;">*</span></label>
                <input id="txt_sendby" class="form-control" type="text" placeholder="Send By Authority" />
                   <label id="lbl_error_sendby" class="errormessage">* Please Enter Authoritator Name</label>
            </div>
            </div>
        <div class="table-responsive">
            <table class="table table-condensed" id="tyres_table">
            <thead>
            <tr>
            <th>#</th>
            <th>Tyre_Sno</th>
            <th>Brand</th>
            <th>Type_of_Tyre</th>
            <th>Tube_Type</th>
            <th>Size</th>
            <th>Remarks</th>
            </tr>
            </thead>
            <tbody>
            </tbody>
            </table>

            <div align="right" id="morerows_div" style="display:none;">
               <input id='btn_addmore' type="button" class="btn btn-primary" value ='Add More Tyres' onclick="addmore()"/>
            </div>

        </div>

        <div align="center">

        <input id='save_tyre' type="button" class="btn btn-primary" value ='Save' onclick="save_tyre_click()"/>
         <input id='close_tyre' type="button" class="btn btn-primary" value ='Reset' onclick="resetall()"/>
        
        </div>

    </div>

</asp:Content>

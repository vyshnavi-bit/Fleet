<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="TyreRethread.aspx.cs" Inherits="TyreRetread" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/utility.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            get_vendor_details();
            getdepartment();
            get_tyres();
            only_no();
        });
        function only_no() {
            $("[name=KMS],#txt_noof_tyres").keydown(function (event) {
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
        function get_vendor_details() {
            var FormName = "VendorNames";
            var data = { 'op': 'get_vendor_details', 'FormName': FormName };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillvendordetails(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillvendordetails(vendormsg) {
            var vendoravailableTags = [];
            var vendoravailableTags2 = [];
            var partgroup = document.getElementById('slct_servicingat');
            var length = partgroup.options.length;
            document.getElementById('slct_servicingat').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Servicer";
            opt.value = "Select Servicer";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            partgroup.appendChild(opt);
            for (var i = 0; i < vendormsg.length; i++) {
                if (vendormsg[i].vendorname != null && vendormsg[i].vendor_type == "Service" || vendormsg[i].vendor_type == "Both") {
                    var option = document.createElement('option');
                    option.innerHTML = vendormsg[i].vendorname;
                    option.value = vendormsg[i].sno;
                    partgroup.appendChild(option);
                }
            }
        }

        var TyreBrand = "";
        var no_of_ty = 0;
        function getdepartment() {
            var minimaster = "TyreBrand";
            var data = { 'op': 'get_Mini_Master_data', 'minimaster': minimaster };
            var s = function (msg) {
                if (msg) {
                    TyreBrand = "";
                    no_of_ty = 0;
                    if (msg.length > 0) {
                        for (var i = 0; i < msg.length; i++) {
                            if (msg[i].mm_name != null && msg[i].mm_status != "0" && msg[i].mm_type == "TyreBrand") {
                                TyreBrand += "<option value=" + msg[i].sno + ">" + msg[i].mm_name + "</option>";
                                no_of_ty++;
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

        var get_tyres_data = [];
        var tyres_data = [];
        function get_tyres() {
            var data = { 'op': 'get_tyres_new' };
            var s = function (msg) {
                if (msg) {
                    tyres_data = [];
                    if (msg.length > 0) {
                        get_tyres_data = msg;
                        for (var i = 0; i < msg.length; i++) {
                            if (msg[i].tyre_sno != null) {
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
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }


        function generate_rows() {
            var no = document.getElementById('txt_noof_tyres').value;

            for (var i = 1; i <= no; i++) {
                $("#tyres_table").append('<tr><td>' + i + '</td>' +
             '<td><input type="text" name="tyre_sno" style="width:200px;" class="form-control"  placeholder="Type Tyre No" /></td>' +
             '<td style="display:none;"><input type="text" disabled name="tyreno_sno" style="width:100px;" class="form-control" /></td>' +
            '<td><select name="Brand" class="form-control" style="width:100px;">' + TyreBrand + '</select></td>' +
            '<td><input name="KMS" class="form-control" type="text" placeholder="KMS" style="width:100px;"/></td></tr>');
            }
            $("#morerows_div").show();
            document.getElementById('txt_noof_tyres').disabled = true;
            if (no == "") {
                document.getElementById('txt_noof_tyres').disabled = false;
            }
            only_no();
            hiden();
            $("[name=tyre_sno]").autocomplete({
                source: function (req, responseFn) {
                    var re = $.ui.autocomplete.escapeRegex(req.term);
                    var matcher = new RegExp("^" + re, "i");
                    var a = $.grep(tyres_data, function (item, index) {
                        return matcher.test(item.label);
                    });
                    responseFn(a);
                },
                autoFocus: true,
                select: function (event, ui) {
                    $(this).parent().parent().children().find('[name=tyreno_sno]').val(ui.item.id);
                },
                change: function (event, ui) { selectfromthis(this); }
            });
        }

        function addmore() {
            document.getElementById('txt_noof_tyres').disabled = true;
            var no = document.getElementById('txt_noof_tyres').value;
            if (no == "") {
                no = "0";
            }

            $("#tyres_table").append('<tr><td>' + (parseInt(no) + 1) + '</td>' +
           '<td><input type="text" name="tyre_sno" style="width:200px;" class="form-control" placeholder="Type Tyre No"/></td>' +
           '<td style="display:none;"><input type="text"   disabled name="tyreno_sno" style="width:100px;" class="form-control" /></td>' +
           '<td><select name="Brand" class="form-control" style="width:100px;">' + TyreBrand + '</select></td>' +
           '<td><input name="KMS" class="form-control" type="text" placeholder="KMS" style="width:100px;"/></td></tr>');
            document.getElementById('txt_noof_tyres').value = parseInt(no) + 1;
            only_no();
            hiden();
            $("[name=tyre_sno]").autocomplete({
                source: function (req, responseFn) {
                    var re = $.ui.autocomplete.escapeRegex(req.term);
                    var matcher = new RegExp("^" + re, "i");
                    var a = $.grep(tyres_data, function (item, index) {
                        return matcher.test(item.label);
                    });
                    responseFn(a);
                },
                autoFocus: true,
                select: function (event, ui) {
                    $(this).parent().parent().children().find('[name=tyreno_sno]').val(ui.item.id);
                },
                change: function (event, ui) { selectfromthis(this); }
            });
        }


        function selectfromthis(thisid) {
            var tyrename = $(thisid).val();
            var isthere = false;
            for (var i = 0; i < get_tyres_data.length; i++) {
                if (tyrename == get_tyres_data[i].tyre_sno) {
                    isthere = true;
                    $(thisid).parent().parent().children().find("[name=Brand] option[value='" + get_tyres_data[i].brand + "']").attr("selected", "selected");
                    $(thisid).parent().parent().children().find("[name=KMS]").val(get_tyres_data[i].current_KMS);
                    break;
                }
            }
            if (!isthere) {
                alert("Please Select From Existed Tyres Only");
                $(thisid).focus();
                $(thisid).val("");
                $(thisid).parent().parent().children().find("[name=tyreno_sno]").val("");
                return;
            }
        }

        function save_tyre_click() {

            var senddate = document.getElementById('txt_senddate').value;
            var servicingat = document.getElementById('slct_servicingat').value;
            var excepteddate = document.getElementById('txt_excepteddate').value;
            var remarks = document.getElementById('txt_remarks').value;
            var sendby = document.getElementById('txt_sendby').value;
            var nooftyres = document.getElementById('txt_noof_tyres').value;

            var tyres = [];

            $('#tyres_table> tbody > tr').each(function () {
                var tyresno = $(this).find('[name=tyreno_sno]').val();
                var Brand = $(this).find('[name=Brand] :selected').val();
                var KMS = $(this).find('[name=KMS]').val();
                tyres.push({ 'tyresno': tyresno, 'Brand': Brand, 'KMS': KMS });
            });

            var flag = false;

            if (senddate == "") {
                $("#lbl_error_senddate").show();
                flag = true;
            }
            if (servicingat == "Select Servicer") {
                $("#lbl_error_service").show();
                flag = true;
            }
            if (excepteddate == "") {
                $("#lbl_expdate_error").show();
                flag = true;
            }
            if (sendby == "") {
                $("#lbl_error_sendby").show();
                flag = true;
            }
            if (nooftyres == "") {
                $("#Label1").show();
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

                                var Data = { 'op': 'save_edit_TyresRethread', 'senddate': senddate, 'servicingat': servicingat, 'excepteddate': excepteddate, 'remarks': remarks, 'sendby': sendby, 'nooftyres': nooftyres };
                                var s = function (msg) {
                                    if (msg) {
                                        alert(msg);
                                        hide_error_logs();
                                        hiden();
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
        function hiden() {
            $('select[name*="tyre_sno"]').change(function () {
                // start by setting everything to enabled
                $('select[name*="tyre_sno"] option').attr('disabled', false);

                // loop each select and set the selected value to disabled in all other selects
                $('select[name*="tyre_sno"]').each(function () {
                    var $this = $(this);
                    $('select[name*="tyre_sno"]').not($this).find('option').each(function () {
                        if ($(this).attr('value') == $this.val())
                            $(this).attr('disabled', true);
                    });
                });
            });
        }
        function hide_error_logs() {
            $("#lbl_error_senddate").hide();
            $("#lbl_error_service").hide();
            $("#lbl_expdate_error").hide();
            $("#lbl_error_sendby").hide();
            $("#Label1").hide();

        }


        function resetall() {
            document.getElementById('txt_senddate').value = "";
            document.getElementById('slct_servicingat').value = "Select Servicer";
            document.getElementById('txt_excepteddate').value = "";
            document.getElementById('txt_remarks').value = "";
            document.getElementById('txt_noof_tyres').value = "";
            document.getElementById('save_tyre').value = "Save";
            document.getElementById('txt_noof_tyres').disabled = false;
            var table = document.getElementById("tyres_table");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            $("#morerows_div").hide();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Tyre Rethread<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Tyre Rethread</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Tyre Rethread Details
                </h3>
            </div>
            <div class="box-body">
                <div id='vehmaster_fillform' style="padding: 20px;">
                <table align="center">
                    <tr>
                        <td>
                          <datalist id="tyres_list">
                        </datalist>
                        <div class="form-group">
                            <label>
                                Send Date<span style="color: red;">*</span></label>
                            <input id="txt_senddate" type="date" class="form-control" name="vendorcode" style="min-width: 195px;"><label
                                id="lbl_error_senddate" class="errormessage">* Please Enter Send Date</label>
                        </td>
                        <td style="width: 5px;">
                        </td>
                        <td>
                            <label>
                                Servicing At<span style="color: red;">*</span></label>
                            <select id="slct_servicingat" class="form-control" style="min-width: 195px;">
                                <option selected disabled value="Select Servicer">Select Servicer</option>
                            </select><label id="lbl_error_service" class="errormessage">* Servicing At</label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                         <label>
                                Excepted Date<span style="color: red;">*</span></label>
                            <input id="txt_excepteddate" type="date" class="form-control" name="vendorcode" style="min-width: 195px;">
                            <label id="lbl_expdate_error" class="errormessage">
                                * Please Enter Excepted Date</label>
                        </td>
                        <td style="width: 5px;">
                        </td>
                        <td>
                          <label>
                                Remarks</label>
                            <textarea id="txt_remarks" class="form-control" placeholder="Remarks" cols="30" rows="2"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        <label>
                                No of Tyres<span style="color: red;">*</span></label>
                            <input id="txt_noof_tyres" class="form-control" type="text" name="vendorcode" placeholder="No of Tyres"
                                onblur="generate_rows()" />
                            <label id="Label1" class="errormessage">
                                * Please Enter NoOfTyres</label>
                        </td>
                        <td style="width: 5px;">
                        </td>
                        <td>
                          <label>
                                Send By<span style="color: red;">*</span></label>
                            <input id="txt_sendby" class="form-control" type="text" placeholder="Send By Authority" />
                            <label id="lbl_error_sendby" class="errormessage">
                                * Please Enter Authoritator Name</label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td style="width: 5px;">
                        </td>
                        <td>
                        <br />
                        </td>
                    </tr>
                </table>
                    <div class="table-responsive">
                        <table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info" id="tyres_table">
                            <thead>
                                <tr>
                                    <th>
                                        #
                                    </th>
                                    <th>
                                        Tyre_Sno
                                    </th>
                                    <th>
                                        Make/Pattern
                                    </th>
                                    <th>
                                        KMS Run
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                        <div align="right" id="morerows_div" style="display: none;">
                            <input id='btn_addmore' type="button" class="btn btn-default" value='Add More Tyres'
                                onclick="addmore()" />
                        </div>
                    </div>
                    <div align="center">
                        <input id='save_tyre' type="button" class="btn btn-primary" value='Save' onclick="save_tyre_click()" />
                        <input id='close_tyre' type="button" class="btn btn-danger" value='Reset' onclick="resetall()" />
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>

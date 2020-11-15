<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="AddTyre.aspx.cs" Inherits="NewTyre" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/utility.js" type="text/javascript"></script>
    <style type="text/css">
        .container
        {
            max-width: 100%;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            //            getallvehtypes();
            getdepartment();
            get_vendor_details();
            document.getElementById('txt_noof_tyres').disabled = false;
            $("#morerows_div").hide();
        });
        $(document).ready(function () {
            $("#txt_totalcost,#txt_tax,#txt_discount,#txt_othrexpences,#txt_noof_tyres,[name=Cost],[name=grove],[name=min_grove],[name=max_grove]").keydown(function (event) {
                // Allow: backspace, delete, tab, escape, and enter
                if (event.keyCode == 46 || event.keyCode == 110 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 || event.keyCode == 190 || event.keyCode == 110 ||
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
        function generate_rows() {
            var no = document.getElementById('txt_noof_tyres').value;
            for (var i = 1; i <= no; i++) {
                $("#tyres_table").append('<tr><td>' + i + '</td><td><input name="tyre_sno" style="width:200px;" class="form-control" type="text" placeholder="Tyre_sno" /></td>' +
            '<td><input name="SVDSNO" class="form-control" type="text" placeholder="SVDS No" style="width:100px;"/></td>' +
            '<td><select name="Brand" class="form-control" style="width:100px;">' + TyreBrand + '</select></td>' +
            '<td><select name="Type_of_Tyre" class="form-control" style="width:100px;">' + TyreType + '</select></td>' +
            '<td><select name="Tube_Type" class="form-control" style="width:100px;"><option value="TubeLess">TubeLess</option><option value="Tube">Tube</option></select></td>' +
            '<td><select name="Size" class="form-control"style="width:100px;">' + TyreSize + '</select></td>' +
            '<td><input name="Cost" class="form-control" type="text" placeholder="Cost" style="width:100px;"/></td>' +
            '<td><input name="min_grove" class="form-control" type="text" placeholder="Min Grove"  style="width:80px;"/></td>' +
            '<td><input name="max_grove" class="form-control" type="text" placeholder="Max Grove" style="width:80px;"/></td>' +
            '<td><input name="grove" class="form-control" type="text" placeholder="Present Grove" style="width:80px;"/></td>' +
            '<td><select name="FrntrRear" class="form-control"style="width:100px;"><option value="Rear">Rear</option><option value="Front">Front</option></select></td></tr>');
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
            $("#tyres_table").append('<tr><td>' + (parseInt(no) + 1) + '</td><td><input name="tyre_sno" style="width:200px;" class="form-control" type="text" placeholder="Tyre_sno" /></td>' +
            '<td><input name="SVDSNO" class="form-control" type="text" placeholder="SVDS No" style="width:100px;"/></td>' +
            '<td><select name="Brand" class="form-control" style="width:100px;">' + TyreBrand + '</select></td>' +
            '<td><select name="Type_of_Tyre" class="form-control" style="width:100px;">' + TyreType + '</select></td>' +
            '<td><select name="Tube_Type" class="form-control" style="width:100px;"><option value="TubeLess">TubeLess</option><option value="Tube">Tube</option></select></td>' +
            '<td><select name="Size" class="form-control"style="width:100px;">' + TyreSize + '</select></td>' +
            '<td><input name="Cost" class="form-control" type="text" placeholder="Cost" style="width:100px;"/></td>' +
            '<td><input name="min_grove" class="form-control" type="text" placeholder="Min Grove"  style="width:80px;"/></td>' +
            '<td><input name="max_grove" class="form-control" type="text" placeholder="Max Grove" style="width:80px;"/></td>' +
            '<td><input name="grove" class="form-control" type="text" placeholder="Present Grove" style="width:80px;"/></td>' +
            '<td><select name="FrntrRear" class="form-control"style="width:100px;"><option value="Rear">Rear</option><option value="Front">Front</option></select></td></tr>');
            document.getElementById('txt_noof_tyres').value = parseInt(no) + 1;
            only_no();
        }

        function get_vendor_details() {
            var FormName = "Tyres";
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
            var partgroup = document.getElementById('slct_vendor');
            var length = partgroup.options.length;
            document.getElementById('slct_vendor').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Vendor";
            opt.value = "Select Vendor";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            partgroup.appendChild(opt);
            for (var i = 0; i < vendormsg.length; i++) {
                if (vendormsg[i].vendorname != null) {
                    var option = document.createElement('option');
                    option.innerHTML = vendormsg[i].vendorname;
                    option.value = vendormsg[i].sno;
                    partgroup.appendChild(option);
                }
            }
        }
        var TyreBrand = "";
        var TyreSize = "";
        var TyreType = "";
        var VehicleMake = "";
        var VehicleType = "";
        function getdepartment() {
            var minimaster = "TyreBrand,TyreSize,TyreType,VehicleMake,VehicleType";
            var data = { 'op': 'get_Mini_Master_data', 'minimaster': minimaster };
            var s = function (msg) {
                if (msg) {
                    TyreBrand = "";
                    TyreSize = "";
                    TyreType = "";
                    VehicleMake = "";
                    VehicleType = "";
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
                            if (msg[i].mm_name != null && msg[i].mm_status != "0" && msg[i].mm_type == "VehicleType") {
                                VehicleType += "<option value=" + msg[i].sno + ">" + msg[i].mm_name + "</option>";
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
        function tax_onchange() {
            var totalcost = document.getElementById('txt_totalcost').value;
            var tax = document.getElementById('txt_tax').value;
            var discount = document.getElementById('txt_discount').value;
            var othrexpences = document.getElementById('txt_othrexpences').value;
            if (totalcost == "") {
                totalcost = 0;
            }
            if (tax == "") {
                tax = 0;
            }
            if (discount == "") {
                discount = 0;
            }
            if (othrexpences == "") {
                othrexpences = 0;
            }
            var totpay = parseFloat(totalcost) + parseFloat(tax) + parseFloat(othrexpences);
            totpay = totpay - parseFloat(discount);
            document.getElementById('txt_grandtotal').value = totpay;
            document.getElementById('txt_payablecost').value = parseInt(totpay);
        }

        function grandttl_onchange() {
            var grand_ttl = document.getElementById('txt_grandtotal').value;
            document.getElementById('txt_payablecost').value = parseInt(grand_ttl);
        }
        function save_tyre_click() {
            var purchasedate = document.getElementById('txt_purchasedate').value;
            var invoiceno = document.getElementById('txt_invoiceno').value;
            var payment = document.getElementById('slct_payment').value;
            var vendor = document.getElementById('slct_vendor').value;
            var totalcost = document.getElementById('txt_totalcost').value;
            var tax = document.getElementById('txt_tax').value;
            var discount = document.getElementById('txt_discount').value;
            var grandtotal = document.getElementById('txt_grandtotal').value;
            var payablecost = document.getElementById('txt_payablecost').value;
            var noof_tyres = document.getElementById('txt_noof_tyres').value;
            var btnval = document.getElementById('save_tyre').value;
            var othrexpences = document.getElementById('txt_othrexpences').value;
            var tyres = [];
            $('#tyres_table> tbody > tr').each(function () {
                var tyresno = $(this).find('[name=tyre_sno]').val();
                var Brand = $(this).find('[name=Brand] :selected').val();
                var Type_of_Tyre = $(this).find('[name=Type_of_Tyre] :selected').val();
                var Tube_Type = $(this).find('[name=Tube_Type] :selected').val();
                var Size = $(this).find('[name=Size] :selected').val();
                var Cost = $(this).find('[name=Cost]').val();
                var FrntrRear = $(this).find('[name=FrntrRear] :selected').val();
                var SVDSNO = $(this).find('[name=SVDSNO]').val();
                var grove = $(this).find('[name=grove]').val();
                var min_grove = $(this).find('[name=min_grove]').val();
                var max_grove = $(this).find('[name=max_grove]').val();
                if (tyresno != "") {
                    tyres.push({ 'tyresno': tyresno, 'Brand': Brand, 'Type_of_Tyre': Type_of_Tyre, 'Tube_Type': Tube_Type, 'Size': Size, 'Cost': Cost, 'FrntrRear': FrntrRear, 'SVDSNO': SVDSNO, 'grove': grove, 'min_grove': min_grove, 'max_grove': max_grove });
                }
            });
            var flag = false;
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

                                var Data = { 'op': 'save_edit_Tyres', 'purchasedate': purchasedate, 'invoiceno': invoiceno, 'payment': payment, 'vendor': vendor, 'totalcost': totalcost,
                                    'tax': tax, 'discount': discount, 'grandtotal': grandtotal, 'payablecost': payablecost, 'noof_tyres': noof_tyres, 'btnval': btnval, 'othrexpences': othrexpences
                                };
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
            $("#lbl_error_purchasedate").hide();
            $("#lbl_vendor_error").hide();
            $("#lbl_error_ttlcost").hide();
        }
        function resetall() {
            document.getElementById('txt_purchasedate').value = "";
            document.getElementById('txt_invoiceno').value = "";
            document.getElementById('slct_payment').value = "cash";
            document.getElementById('slct_vendor').value = "Select Vendor";
            document.getElementById('txt_totalcost').value = "";
            document.getElementById('txt_tax').value = "";
            document.getElementById('txt_discount').value = "";
            document.getElementById('txt_grandtotal').value = "";
            document.getElementById('txt_payablecost').value = "";
            document.getElementById('txt_noof_tyres').value = "";
            document.getElementById('save_tyre').value = "Save";
            document.getElementById('txt_othrexpences').value = "";
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
            Add Tyre<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Add Tyre</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Add Tyre Details
                </h3>
            </div>
            <div class="box-body">
                <div id='vehmaster_fillform' style="padding: 20px;">
                    <table align="center">
                        <tr>
                            <td>
                                <label>
                                    Purchase Date</label>
                                <input id="txt_purchasedate" type="date" class="form-control" name="vendorcode" style="min-width: 195px;" /><label
                                    id="lbl_error_purchasedate" class="errormessage">* Please Enter Purchase Date</label>
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <label>
                                    Invoice No</label>
                                <input id="txt_invoiceno" class="form-control" type="text" name="vendorcode" placeholder="Invoice Number">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Payment Type</label>
                                <select id="slct_payment" class="form-control" style="min-width: 195px;">
                                    <option value="cash">cash</option>
                                    <option value="cash">credit</option>
                                </select><label id="lbl_error_payment" class="errormessage">* Please Selet Payment Type</label>
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <label>
                                    Purchase From</label>
                                <select id="slct_vendor" class="form-control" style="min-width: 195px;">
                                    <option value="Select Vendor">Select Vendor</option>
                                </select><label id="lbl_vendor_error" class="errormessage">* Please Selet Vendor</label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Total Cost</label>
                                <input id="txt_totalcost" class="form-control" type="text" name="vendorcode" placeholder="Total Cost"
                                    onkeyup="tax_onchange()" />
                                <label id="lbl_error_ttlcost" class="errormessage">
                                    * Please Enter Total Cost
                                </label>
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <label>
                                    Tax</label>
                                <input id="txt_tax" class="form-control" type="text" name="vendorcode" placeholder="Tax"
                                    onkeyup="tax_onchange()" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Discount</label>
                                <input id="txt_discount" class="form-control" type="text" name="vendorcode" placeholder="Discount"
                                    onkeyup="tax_onchange()" />
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <label>
                                    Other Expences</label>
                                <input id="txt_othrexpences" class="form-control" type="text" name="vendorcode" placeholder="Other Expences"
                                    onkeyup="tax_onchange()" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Grand Total</label>
                                <input id="txt_grandtotal" class="form-control" disabled type="text" name="vendorcode"
                                    placeholder="Grand Total" onchange="grandttl_onchange()" />
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <label>
                                    Payable Cost</label>
                                <input id="txt_payablecost" disabled class="form-control" type="text" name="vendorcode"
                                    placeholder="Payable Cost" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    No of Tyres</label>
                                <input id="txt_noof_tyres" class="form-control" type="text" name="vendorcode" placeholder="No of Tyres"
                                    onblur="generate_rows()" />
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <div class="form-group" style="display: none;">
                                    <input type="text" id="txt_sno" maxlength="45" name="vendorcode" placeholder="sno">
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="table-responsive">
                        <table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info"
                            id="tyres_table">
                            <thead>
                                <tr>
                                    <th>
                                        #
                                    </th>
                                    <th>
                                        Tyre_Sno
                                    </th>
                                    <th>
                                        SVDS No
                                    </th>
                                    <th>
                                        Brand
                                    </th>
                                    <th>
                                        Type_of_Tyre
                                    </th>
                                    <th>
                                        Tube_Type
                                    </th>
                                    <th>
                                        Size
                                    </th>
                                    <th>
                                        Cost
                                    </th>
                                    <th>
                                        Min Grove
                                    </th>
                                    <th>
                                        Max Grove
                                    </th>
                                    <th>
                                        Present Grove
                                    </th>
                                    <th>
                                        Front/Rear
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

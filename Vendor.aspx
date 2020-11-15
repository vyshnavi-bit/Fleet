<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="Vendor.aspx.cs" Inherits="Vendor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%-- <link href="css/formstable.css" rel="stylesheet" type="text/css" />
    <link href="css/custom.css" rel="stylesheet" type="text/css" />--%>
    <script type="text/javascript">
        $(function () {
            $('#btn_addvendor').click(function () {
                $('#fillform').css('display', 'block');
                $('#showlogs').css('display', 'none');
                $('#div_vendordata').hide();
                forclearall();
            });
            $('#btn_close').click(function () {
                $('#fillform').css('display', 'none');
                $('#showlogs').css('display', 'block');
                $('#div_vendordata').show();
                forclearall();
            });
            get_vendor_details();
            get_state_details();
        });
        function get_state_details() {
            var data = { 'op': 'get_state_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillstates(msg);
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
        function fillstates(msg) {
            var ddlstate = document.getElementById('slct_state');
            var length = ddlstate.options.length;
            ddlstate.options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "select";
            ddlstate.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].statename != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].statename;
                    opt.value = msg[i].sno;
                    ddlstate.appendChild(opt);
                }
            }
        }
        function addvedordetails() {
            $('#fillform').css('display', 'block');
            $('#showlogs').css('display', 'none');
            $('#div_vendordata').hide();
            forclearall();
        }
        function closevendordetails() {
            $('#fillform').css('display', 'none');
            $('#showlogs').css('display', 'block');
            $('#div_vendordata').show();
            forclearall();
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


        //Function for only no
        $(document).ready(function () {
            $("#txt_phoneno,#txt_servtax").keydown(function (event) {
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
        });

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

        function validateEmail(email) {
            var reg = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/
            if (reg.test(email)) {
                return true;
            }
            else {
                return false;
            }
        }

        function for_save_edit_vendor() {
            var vendorcode = document.getElementById('txt_vendorcode').value;
            var vendorname = document.getElementById('txt_vendorname').value;
            var addr = document.getElementById('txt_address').value;
            var email = document.getElementById('txt_email').value;
            var phoneno = document.getElementById('txt_phoneno').value;
            var tan = document.getElementById('txt_tan').value;
            var vat = document.getElementById('txt_vat').value;
            var cst = document.getElementById('txt_cst').value;
            var servtax = document.getElementById('txt_servtax').value;
            var btnval = document.getElementById('btn_save').value;
            var sno = document.getElementById('lbl_sno').innerHTML;
            var vendor_type = document.getElementById('cmb_vendortype').value;
            var tinno = document.getElementById('txt_tinno').value;
            var panno = document.getElementById('txt_panno').value;
            var gst = document.getElementById('txt_gst').value;
            var btnval = document.getElementById('btn_save').innerHTML;
            var state = document.getElementById('slct_state').value;
            var flag = false;
            if (vendorcode == "") {
                $("#lbl_vencode_error_msg").show();
                $("#txt_vendorcode").focus();
                flag = true;
            }

            if (vendorname == "") {
                $("#lbl_vennme_error_msg").show();
                $("#txt_vendorname").focus();
                flag = true;
            }
            if (!validateEmail(email)) {
                $("#lbl_email_error_msg").show();
                $("#txt_email").focus();
                flag = true;
            }
            if (phoneno == "") {
                alert("Please enter phoneno");
                return false;
            }
            if (addr == "") {
                alert("Please enter address");
                return false;
            }
            if (state == "" || state == "select") {
                alert("Please select state");
                $("#slct_state").focus();
                return false;
            }
            if (flag) {
                return;
            }
            var data = { 'op': 'for_save_edit_vendor', 'tinno': tinno, 'panno': panno, 'vendorcode': vendorcode, 'vendorname': vendorname, 'addr': addr, 'email': email, 'phoneno': phoneno, 'tan': tan,
                'vat': vat, 'state': state, 'btnval': btnval, 'cst': cst, 'btnval': btnval, 'servtax': servtax, 'sno': sno, 'vendor_type': vendor_type, 'gst': gst
            };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        if (msg == "OK") {
                            alert("New Vendor Successfully Created");
                            forclearall();
                            get_vendor_details();
                            $('#div_vendordata').show();
                            $('#fillform').css('display', 'none');
                            $('#showlogs').css('display', 'block');
                        }
                        else if (msg == "UPDATE") {
                            alert(vendorname + "  Vendor Successfully Modified");
                            forclearall();
                            get_vendor_details();
                            $('#div_vendordata').show();
                            $('#fillform').css('display', 'none');
                            $('#showlogs').css('display', 'block');
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
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function forclearall() {
            document.getElementById('txt_vendorcode').value = "";
            document.getElementById('txt_vendorname').value = "";
            document.getElementById('txt_address').value = "";
            document.getElementById('txt_email').value = "";
            document.getElementById('txt_phoneno').value = "";
            document.getElementById('txt_tan').value = "";
            document.getElementById('txt_vat').value = "";
            document.getElementById('txt_cst').value = "";
            document.getElementById('txt_servtax').value = "";
            document.getElementById('txt_gst').value = "";
            document.getElementById('slct_state').selectedIndex = 0;
            document.getElementById('cmb_vendortype').selectedIndex = 0;
            document.getElementById('btn_save').innerHTML = "Save";
            $("#lbl_vencode_error_msg").hide();
            $("#lbl_vennme_error_msg").hide();
            $("#lbl_email_error_msg").hide();
        }
        function get_vendor_details() {
            var FormName = "VendorNames";
            var data = { 'op': 'get_vendor_details', 'FormName': FormName };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filldetails(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function filldetails(msg) {
            var k = 0;
            var colorue = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            var results = '<div style="overflow:auto;"><table  class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;"></th><th scope="col" style="font-weight: bold;">Vendor Name</th><th scope="col" style="font-weight: bold;">VendorCode</th><th scope="col" style="font-weight: bold;">Email</th><th scope="col" style="font-weight: bold;">PhoneNumber</th><th style="font-weight: bold;">VendorType</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                //results += '<tr style="background-color:' + colorue[k] + '"><td><input id="btn_poplate" type="button"  onclick="getme(this)" name="submit" class="btn btn-primary" value="Choose" /></td>';
                results += '<tr style="background-color:' + colorue[k] + '"><td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="getme(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td>';
                //results += '<td scope="row" class="1" style="font-weight: 600;">' + msg[i].vendorname + '</td>';
                results += '<td   class="1" style="font-weight: 600;"><i class="" aria-hidden="true"></i>&nbsp;<span id="1">' + msg[i].vendorname + '</span></td>';
                results += '<td data-title="Vendor Code" class="2">' + msg[i].vendor_code + '</td>';
                results += '<td data-title="Address" class="3" style="display:none">' + msg[i].vendor_address + '</td>';
                //results += '<td data-title="Email" class="4">' + msg[i].vendor_email + '</td>';
                results += '<td   class="1"><i class="fa fa-envelope-o" aria-hidden="true"></i>&nbsp;<span id="4">' + msg[i].vendor_email + '</span></td>';
                //results += '<td data-title="Phone number" class="5">' + msg[i].vendor_mob + '</td>';
                results += '<td class="1"><i class="fa fa-phone" aria-hidden="true"></i>&nbsp;<span id="5">' + msg[i].vendor_mob + '</span></td>';
                results += '<td data-title="TAN" class="6"style="display:none" >' + msg[i].vendor_tin + '</td>';
                results += '<td data-title="VAT" class="7" style="display:none">' + msg[i].vendor_vat + '</td>';
                results += '<td data-title="CST" class="8" style="display:none">' + msg[i].vendor_cst + '</td>';
                results += '<td data-title="Service Tax(%)" class="9" style="display:none">' + msg[i].vendor_stax + '</td>';
                results += '<td data-title="Vendor Type" class="10">' + msg[i].vendor_type + '</td>';
                results += '<td style="display:none" class="sno">' + msg[i].sno + '</td>';
                results += '<td style="display:none" class="12">' + msg[i].stateid + '</td>';
                results += '<td style="display:none" class="13">' + msg[i].gstin + '</td></tr>';
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
            var vendorcode = $(thisid).parent().parent().children('.2').html();
            vendorcode = replaceHtmlEntites(vendorcode);
            var vendorname = $(thisid).parent().parent().find('#1').html();
            vendorname = replaceHtmlEntites(vendorname);
            var addr = $(thisid).parent().parent().children('.3').html();
            addr = replaceHtmlEntites(addr);
            var email = $(thisid).parent().parent().find('#4').html();
            var phoneno = $(thisid).parent().parent().find('#5').html();
            var tan = $(thisid).parent().parent().children('.6').html();
            var vat = $(thisid).parent().parent().children('.7').html();
            var cst = $(thisid).parent().parent().children('.8').html();
            var servtax = $(thisid).parent().parent().children('.9').html();
            var vendor_type = $(thisid).parent().parent().children('.10').html();
            var stateid = $(thisid).parent().parent().children('.12').html();
            var gstin = $(thisid).parent().parent().children('.13').html();

            document.getElementById('txt_vendorcode').value = vendorcode;
            document.getElementById('txt_vendorname').value = vendorname;
            document.getElementById('txt_address').value = addr;
            document.getElementById('txt_email').value = email;
            document.getElementById('txt_phoneno').value = phoneno;
            document.getElementById('txt_tan').value = tan;
            document.getElementById('txt_vat').value = vat;
            document.getElementById('txt_cst').value = cst;
            document.getElementById('txt_servtax').value = servtax;
            document.getElementById('cmb_vendortype').value = vendor_type;
            document.getElementById('slct_state').value = stateid;
            document.getElementById('txt_gst').value = gstin;
            document.getElementById('lbl_sno').innerHTML = sno;
            document.getElementById('btn_save').innerHTML = "Modify";
            $("#div_vendordata").hide();
            $("#fillform").show();
            $('#showlogs').hide();
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
    <style type="text/css">
        th
        {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Vendor Master<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Vendor Master</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Vendor Master Details
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs" align="center">
                    <%--<input id="btn_addvendor" type="button" name="submit" value='Add Vendor' class="btn btn-primary" />--%>
                    <div class="input-group" style="width: 100px;padding-left: 960px;">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addvedordetails()"></span><span id="btn_addvendor" onclick="addvedordetails()">Add Vendor</span>
                          </div>
                          </div>
                </div>
                <div id="div_vendordata">
                </div>
                <div id='fillform' style="text-align: -webkit-center;display: none;" >
                    <div> 
                        <table>
                            <tr>
                                <th colspan="2" align="center">
                                </th>
                            </tr>
                            <tr>
                                <td style="height:40px;">
                                   <label> Code </label><span style="color: red;">*</span>
                                    <input type="text" maxlength="45" id="txt_vendorcode" class="form-control" name="vendorcode"
                                        placeholder="Enter Vendor Code" /><label id="lbl_vencode_error_msg" class="errormessage">*
                                            Please Enter Vendor Code</label>
                                </td>
                           <td style="width: 5px;">
                             </td>
                                <td style="height:40px;">
                                  <label>  Name </label> <span style="color: red;">*</span>
                                    <input type="text" maxlength="45" id="txt_vendorname" class="form-control" name="vendorcode"
                                        placeholder="Enter Vendor Name" /><label id="lbl_vennme_error_msg" class="errormessage">*
                                            Please Enter Vendor Name</label>
                                </td>
                            </tr>
                            <tr>
                                <td style="height:40px;">
                                  <label>  Type </label>
                                    <select id="cmb_vendortype" class="form-control">
                                        <option value="Both">Both</option>
                                        <option value="Sales">Sales</option>
                                        <option value="Service">Service</option>
                                        <option value="Insurance">Insurance</option>
                                        <option value="Dairy">Dairy</option>
                                        <option value="Fuel">Fuel</option>
                                    </select>
                                </td>
                           <td style="width: 5px;">
                             </td>
                                <td style="height:40px;">
                                  <label>  Address </label>
                                    <input type="text"  name="vendorcode" id="txt_address" class="form-control" placeholder="Enter Address"></input>
                                </td>
                            </tr>
                            <tr>
                                <td style="height:40px;">
                                  <label>  Email </label><span style="color: red;">*</span>
                                    <input type="text" name="vendorcode" id="txt_email" class="form-control" maxlength="45"
                                        placeholder="Enter Email ID" /><label id="lbl_email_error_msg" class="errormessage">*
                                            Please Enter Proper Email ID</label>
                                </td>
                           <td style="width: 5px;">
                             </td>
                                <td style="height:40px;">
                                   <label> Phone Number </label> <span style="color: red;">*</span>
                                    <input type="text" name="vendorcode" id="txt_phoneno" class="form-control" maxlength="10"
                                        placeholder="Enter Phone Number" />
                                </td>
                            </tr>
                              <tr>
                                <td style="height:40px;">
                                 <label>   Tin No </label>
                                    <input type="text" name="vendorcode" id="txt_tinno" class="form-control" placeholder="Enter Tin No" />
                                </td>
                            <td style="width: 5px;">
                             </td>
                                <td style="height:40px;">
                                 <label>   Pan No </label>
                                    <input type="text" name="vendorcode" id="txt_panno" class="form-control" placeholder="Enter Pan No" />
                                </td>
                            </tr>
                            <tr>
                                <td style="height:40px;">
                                 <label>   Tan No </label>
                                    <input type="text" name="vendorcode" id="txt_tan" class="form-control" placeholder="Enter Tan No" />
                                </td>
                          <td style="width: 5px;">
                             </td>
                                <td style="height:40px;">
                                  <label>     Vat </label>
                                    <input type="text" name="vendorcode" id="txt_vat" class="form-control" placeholder="Enter VAT" />
                                </td>
                            </tr>
                            <tr>
                                <td style="height:40px;">
                                   <label>    CST </label>
                                    <input type="text" name="vendorcode" id="txt_cst" class="form-control" placeholder="Enter CST No" />
                                </td>
                           <td style="width: 5px;">
                             </td>
                                <td style="height:40px;">
                                 <label>      Service Tax (%)</label>
                                    <input type="text" name="vendorcode" id="txt_servtax" class="form-control" placeholder="Enter Service Tax In Percentile (%)" />
                                </td>
                            </tr>
                             <tr>
                                <td style="height:40px;">
                                   <label>    GST </label>
                                    <input type="text" name="gst" id="txt_gst" class="form-control" placeholder="Enter GST No" />
                                </td>
                                  <td style="width: 5px;">
                             </td>
                                <td>
                                <label>
                                    State Name:</label>
                                <select id="slct_state" class="form-control">
                                </select>
                                </td>
                            </tr>
                            <tr hidden>
                                <td>
                                    <label id="lbl_sno">
                                    </label>
                                </td>
                            </tr>
                        </table>
                        <div style="padding-left: 8px;padding-top: 10px;">
                                <table>
                                <tr>
                                <td>
                                </td>
                                <td>
                                    <div class="input-group">
                                        <div class="input-group-addon">
                                        <span class="glyphicon glyphicon-ok" id="btn_save1" onclick="for_save_edit_vendor()"></span><span id="btn_save" onclick="for_save_edit_vendor()">Save</span>
                                  </div>
                                  </div>
                                    </td>
                                    <td style="width:10px;"></td>
                                    <td>
                                     <div class="input-group">
                                        <div class="input-group-close">
                                        <span class="glyphicon glyphicon-remove" id='btn_close1' onclick="closevendordetails()"></span><span id='btn_close' onclick="closevendordetails()">Close</span>
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
    </section>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="Batterymaster.aspx.cs" Inherits="Batterymaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/utility.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#btn_addbattery').click(function () {
                $('#fillform').css('display', 'block');
                $('#showlogs').css('display', 'none');
                $('#div_batterydata').hide();
                clearall();
            });
            $('#btn_close').click(function () {
                $('#fillform').css('display', 'none');
                $('#showlogs').css('display', 'block');
                $('#div_batterydata').show();
            });
            get_Battery_details();
            get_BatteryMake();
        });
        function addbatterydetails() {
            $('#fillform').css('display', 'block');
            $('#showlogs').css('display', 'none');
            $('#div_batterydata').hide();
            clearall();
        }
        function closebatterymater() {
            $('#fillform').css('display', 'none');
            $('#showlogs').css('display', 'block');
            $('#div_batterydata').show();
        }
        function get_Battery_details() {
            var FormName = "VendorNames";
            var data = { 'op': 'get_Battery_details', 'FormName': FormName };
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
            var results = '<div class="divcontainer" style="overflow:auto;"><table  class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;"></th><th scope="col" style="font-weight: bold;">Battery Make</th><th scope="col" style="font-weight: bold;">Battery Number</th><th scope="col" style="font-weight: bold;">Purchase Date</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + colorue[k] + '">';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getme(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td>';
                results += '<td scope="row" class="1" style="font-weight: 700;">' + msg[i].BatteryMake + '</td>';
                //results += '<td data-title="Vendor Code" class="2">' + msg[i].bat_sno + '</td>';
                results += '<td class="1"><i class="glyphicon glyphicon-barcode" aria-hidden="true"></i>&nbsp;<span id="2">' + msg[i].bat_sno + '</span></td>';
                //results += '<td data-title="Address" class="3">' + msg[i].purchasedate + '</td>';
                results += '<td class="1"><i class="fa  fa-calendar-times-o" aria-hidden="true"></i>&nbsp;<span id="3">' + msg[i].purchasedate + '</span></td>';
                results += '<td style="display:none" class="sno">' + msg[i].Sno + '</td>';
                results += '<td style="display:none" class="4">' + msg[i].MakeSno + '</td></tr>';
                k = k + 1;
                if (k == 4) {
                    k = 0;
                }
            }
            results += '</table></div>';
            $("#div_batterydata").html(results);
        }
        function getme(thisid) {
            var sno = $(thisid).parent().parent().children('.sno').html();
            var BatteryMake = $(thisid).parent().parent().children('.4').html();
            var batsno = $(thisid).parent().parent().find('#2').html();
            batsno = replaceHtmlEntites(batsno);
            var purchasedate = $(thisid).parent().parent().find('#3').html();
            purchasedate = replaceHtmlEntites(purchasedate);
            document.getElementById('ddl_batterymake').value = BatteryMake;
            document.getElementById('txt_batterynumber').value = batsno;
            document.getElementById('txt_purchasedate').value = purchasedate;
            document.getElementById('lbl_sno').innerHTML = sno;
            document.getElementById('btn_save').innerHTML = "Modify";
            $("#div_batterydata").hide();
            $("#fillform").show();
            $("#showlogs").hide();
        }
        function get_BatteryMake() {
            var minimaster = "BatteryMake";
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
            var vehmake = document.getElementById('ddl_batterymake');
            var length = vehmake.options.length;
            document.getElementById('ddl_batterymake').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Battery Make";
            opt.value = "Select Battery Make";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            vehmake.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].mm_name != null && msg[i].mm_status != "0" && msg[i].mm_type == "BatteryMake") {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].mm_name;
                    option.value = msg[i].sno;
                    vehmake.appendChild(option);
                }
            }
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
        function btnsavebatterymasterClick() {
            var ddlbatterymake = document.getElementById('ddl_batterymake').value;
            if (ddlbatterymake == "" || ddlbatterymake == "Select Battery Make") {
                alert("Please select battery make");
                $('#ddl_batterymake').focus();
                return false;
            }
            var txtbatterynumber = document.getElementById('txt_batterynumber').value;
            if (txtbatterynumber == "") {
                alert("Please enter battery no");
                $('#txt_batterynumber').focus();
                return false;
            }
            var txtpurchasedate = document.getElementById('txt_purchasedate').value;
            if (txtpurchasedate == "") {
                alert("Please select purchase date");
                $('#txt_purchasedate').focus();
                return false;
            }
            var btnval = document.getElementById('btn_save').innerHTML;
            var sno = document.getElementById('lbl_sno').innerHTML;
            var data = { 'op': 'btnsavebatterymasterClick', 'batterymake': ddlbatterymake, 'batterynumber': txtbatterynumber, 'purchasedate': txtpurchasedate, 'btnval': btnval, 'sno': sno };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    clearall();
                    get_Battery_details();
                    $('#div_batterydata').show();
                    $('#fillform').css('display', 'none');
                    $('#showlogs').css('display', 'block');
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
            document.getElementById('ddl_batterymake').selectedIndex = 0;
            document.getElementById('txt_batterynumber').value = "";
            document.getElementById('txt_purchasedate').value = "";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Battery Master<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Battery Master</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Battery Details
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs" align="center">
                    <%--<input id="btn_addbattery" type="button" name="submit" value='Add Battery' class="btn btn-primary" />--%>
                      <div class="input-group" style="width: 100px;padding-left: 960px;">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addbatterydetails()"></span><span id="btn_addbattery" onclick="addbatterydetails()">Add Battery</span>
                          </div>
                          </div>
                </div>
                <div id="div_batterydata" style="background: #ffffff">
                </div>
                <div id='fillform' style="display: none;">
                    <div style="padding: 20px;">
                        <div>
                            <table align="center">
                                <tr>
                                    <td style="height:40px;"> 
                                      <label>  Battery Make </label><span style="color: red;">*</span>
                                    </td>
                                    <td >
                                        <select id="ddl_batterymake" class="form-control" style="min-width: 200px;">
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height:40px;">
                                     <label>   Battery Number  </label><span style="color: red;">*</span>
                                    </td>
                                    <td>
                                        <input id="txt_batterynumber" class="form-control" type="text" placeholder="Battery Number" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height:40px;">
                                   <label>     Purchase Date  </label><span style="color: red;">*</span>
                                    </td>
                                    <td>
                                        <input id="txt_purchasedate" type="date" class="form-control" name="vendorcode" placeholder="Fittness Purchase Date"
                                            style="min-width: 195px;" />
                                    </td>
                                </tr>
                                <tr hidden>
                                    <td>
                                        <label id="lbl_sno">
                                        </label>
                                    </td>
                                </tr>
                                <tr>
                                    <%--<td>
                                    </td>
                                    <td style="height:40px;">
                                        <input id="btn_save" type="button" value="Save" class="btn btn-primary" onclick="btnsavebatterymasterClick();" />
                                        <input id='btn_close' type="button" class="btn btn-danger" name="Close" value='Close' />
                                    </td>--%>
                                </tr>
                            </table>
                           <table>
                                <tr>
                                <td style="padding-left: 420px;padding-top: 56px;">
                                </td>
                                <td>
                                    <div class="input-group">
                                        <div class="input-group-addon">
                                        <span class="glyphicon glyphicon-ok" id="btn_save1" onclick="btnsavebatterymasterClick()"></span><span id="btn_save" onclick="btnsavebatterymasterClick()">Save</span>
                                  </div>
                                  </div>
                                    </td>
                                    <td style="width:10px;"></td>
                                    <td>
                                     <div class="input-group">
                                        <div class="input-group-close">
                                        <span class="glyphicon glyphicon-remove" id='btn_close1' onclick="closebatterymater()"></span><span id='btn_close' onclick="closebatterymater()">Close</span>
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

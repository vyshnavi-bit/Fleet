<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="VehicleExpenduture.aspx.cs" Inherits="VehicleExpenduture" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/utility.js" type="text/javascript"></script>
    <%--<link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />--%>
    <script src="Js/JTemplate.js?v=3003" type="text/javascript"></script>
    <link href="responsivegridsystem/css/2cols.css" rel="stylesheet" type="text/css" />
    <link href="responsivegridsystem/css/col.css" rel="stylesheet" type="text/css" />
  <%--  <link href="responsivegridsystem/css/html5reset.css" rel="stylesheet" type="text/css" />--%>
    <style type="text/css">
        .inputstable td
        {
            width: 150px;
            padding: 5px 5px 5px 20px;
        }
        h1
        {
            font-size: 2.2em;
            padding: 0 0 .5em 0;
        }
        h2
        {
            font-size: 1.5em;
        }
        .header
        {
            padding: 1em 0;
        }
        .col
        {
            border: 1px solid #d5d5d5;
            text-align: center;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            getallveh_nos();
            get_headdata();
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
            $('#txt_tripstrtdate').val(yyyy + '-' + mm + '-' + dd);
        });
        function get_headdata() {
            var data = { 'op': 'get_head_master_List' };
            var s = function (msg) {
                if (msg) {
                    BindHeads(msg);
                }
                else {
                    document.location = "Default.aspx";
                }
            }
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        }
        function BindHeads(msg) {
            var data = document.getElementById('ddlHeads');
            var length = data.options.length;
            document.getElementById('ddlHeads').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select head of account";
            opt.value = "Select head of account";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].desc != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].desc;
                    option.value = msg[i].refno;
                    data.appendChild(option);
                }
            }
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
        var Cashform = [];
        function BtnCashToAddClick() {
            var Head = document.getElementById("ddlHeads");
            var HeadSno = Head.options[Head.selectedIndex].value;
            var HeadOfAccount = Head.options[Head.selectedIndex].text;
            if (HeadOfAccount == "select" || HeadOfAccount == "") {
                alert("Select Account Name");
                return false;
            }
            var Checkexist = false;
            $('.AccountClass').each(function (i, obj) {
                var IName = $(this).text();
                if (IName == HeadOfAccount) {
                    alert("Account Already Added");
                    Checkexist = true;
                }
            });
            if (Checkexist == true) {
                return;
            }
            var Amount = document.getElementById("txtCashAmount").value;
            if (Amount == "") {
                alert("Enter Amount");
                return false;
            }
            Cashform.push({ HeadSno: HeadSno, HeadOfAccount: HeadOfAccount, Amount: Amount });
            $('#divHeadAcount').setTemplateURL('CashForm.htm');
            $('#divHeadAcount').processTemplate(Cashform);
            var TotRate = 0.0;
            $('.AmountClass').each(function (i, obj) {
                if ($(this).text() == "") {
                }
                else {
                    TotRate += parseFloat($(this).text());
                }
            });
            TotRate = parseFloat(TotRate).toFixed(2);
            document.getElementById("txtAMount").innerHTML = TotRate;
            document.getElementById("txtCashAmount").value = "";
        }
        function RowDeletingClick(Account) {
            Cashform = [];
            var HeadOfAccount = "";
            var HeadSno = "";
            var Account = $(Account).closest("tr").find("#txtAccount").text();
            var Amount = "";
            var rows = $("#tableCashFormdetails tr:gt(0)");
            $(rows).each(function (i, obj) {
                if ($(this).find('#txtamount').text() != "") {
                    HeadOfAccount = $(this).find('#txtAccount').text();
                    HeadSno = $(this).find('#HeadSno').val();
                    Amount = $(this).find('#txtamount').text();
                    if (Account == HeadOfAccount) {
                    }
                    else {
                        Cashform.push({ HeadSno: HeadSno, HeadOfAccount: HeadOfAccount, Amount: Amount });
                    }
                }
            });
            $('#divHeadAcount').setTemplateURL('CashForm.htm');
            $('#divHeadAcount').processTemplate(Cashform);
            var TotRate = 0.0;
            $('.AmountClass').each(function (i, obj) {
                if ($(this).text() == "") {
                }
                else {
                    TotRate += parseFloat($(this).text());
                }
            });
            TotRate = parseFloat(TotRate).toFixed(2);
            document.getElementById("txtAMount").innerHTML = TotRate;
        }
        function BtnRaiseVehicleExpendatureClick() {
            var doe = document.getElementById("txt_tripstrtdate").value;
            var vehicleno = document.getElementById("slct_vehicle_no").value;
            var name = document.getElementById("txtname").value;
            var Amount = document.getElementById("txtAMount").innerHTML;
            var Incharge = document.getElementById("txtincharge").value;
            var Remarks = document.getElementById("txtRemarks").value;
            if (doe == "") {
                alert("Select Date");
                $('#txt_tripstrtdate').focus();
                return false;
            }
            if (vehicleno == "" || vehicleno == "Select Vehicle No") {
                alert("Select vehicle no");
                $('#slct_vehicle_no').focus();
                return false;
            }
            if (name == "") {
                alert("Enter Name");
                $('#txtname').focus();
                return false;
            }
            if (Amount == "") {
                alert("Please Enter Amount");
                $('#txtAMount').focus();
                return false;
            }
            if (Incharge == "") {
                alert("Enter incharge name");
                $('#txtincharge').focus();
                return false;
            }
            if (Remarks == "") {
                alert("Enter Remarks");
                $('#txtRemarks').focus();
                return false;
            }
            var btnSave = document.getElementById("btnSave").innerHTML;
            var rows = $("#tableCashFormdetails tr:gt(0)");
            var Cashdetails = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txtProductQty').val() != "") {
                    Cashdetails.push({ headsno: $(this).find('#hdnHeadSno').val(), Account: $(this).find('#txtAccount').text(), amount: $(this).find('#txtamount').text() });
                }
            });
            var data = { 'op': 'BtnRaiseVehicleExpendature_saveclick', 'Cashdetails': Cashdetails, 'doe': doe, 'vehicleno': vehicleno, 'Amount': Amount, 'name': name, 'Incharge': Incharge, 'btnSave': btnSave, 'Remarks': Remarks, 'refno': refno };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    document.getElementById("slct_vehicle_no").selectedIndex = 0;
                    document.getElementById("divHeadAcount").selectedIndex = 0;
                    document.getElementById("txtname").value = "";
                    document.getElementById("txtAMount").innerHTML = "";
                    document.getElementById("txtincharge").value = "";
                    document.getElementById("txtRemarks").value = "";
                    refno = 0;
                    document.getElementById("btnSave").innerHTML = "Raise";
                    Cashform = [];
                    $('#divHeadAcount').setTemplateURL('CashForm.htm');
                    $('#divHeadAcount').processTemplate(Cashform);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            CallHandlerUsingJson(data, s, e);
        }
        function divRaiseMaintainceClick() {
            $('#divRaiseMaintaince').css('display', 'block');
            $('#divViewMaintaince').css('display', 'none');
        }
        function divViewMaintainceClick() {
            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (day < 10) day = "0" + day;
            today = year + "-" + month + "-" + day;
            $('#txtFromDate').val(today);
            $('#txtToDate').val(today);
            $('#divRaiseMaintaince').css('display', 'none');
            $('#divViewMaintaince').css('display', 'block');
        }
        function BtnGenerateClick() {
            var k = 0;
            var colorue = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            var FromDate = document.getElementById("txtFromDate").value;
            var ToDate = document.getElementById("txtToDate").value;
            var data = { 'op': 'GetMaintenance_list', 'FromDate': FromDate, 'ToDate': ToDate };
            var s = function (msg) {
                if (msg) {
                    var table = document.getElementById("tbl_feecategorylist");
                    for (var i = table.rows.length - 1; i > 0; i--) {
                        table.deleteRow(i);
                    }
                    for (var i = 0; i < msg.length; i++) {
                        var tablerowcnt = document.getElementById("tbl_feecategorylist").rows.length;
                        var status;
                        if (msg[i].status == "1") {
                            status = "Enable";
                        }
                        else {
                            status = "Disable";
                        }
                        $('#tbl_feecategorylist').append('<tr style="background-color:' + colorue[k] + '">' +
                        '<td data-title="categorysno" >' + msg[i].sno + '</td>' +
                        '<th scope="Category Name" >' + msg[i].maintace_id + '</th>' +
                        //'<td data-title="IsTransport" class="2">' + msg[i].vehicleno + '</td>' +
                        '<td ><i class="fa fa-truck" aria-hidden="true"></i>&nbsp;<span id="2">' + msg[i].vehicleno + '</span></td>' +
                        //'<td data-title="IsTransport">' + msg[i].amount + '</td>' +
                        '<td ><i class="fa fa-rupee" aria-hidden="true"></i>&nbsp;<span id="3">' + msg[i].amount + '</span></td>' +
                        //'<td data-title="IsTransport">' + msg[i].name + '</td>' +
                        '<td ><i class="fa fa-user" aria-hidden="true"></i>&nbsp;<span id="4">' + msg[i].name + '</span></td>' + 
                        '<td data-title="IsTransport">' + msg[i].incharge + '</td>' +
                        '<td data-title="Status" style="display:none;">' + msg[i].maintace_code + '</td>' +
                        '<td data-title="Status" style="display:none;">' + msg[i].veh_sno + '</td>' +
                        '<td data-title="Status" style="display:none;">' + msg[i].remarks + '</td>' +
                        //'<td><input type="button" class="btn btn-primary"  value ="Edit" onclick="Editclick(this);"/></td>' +
                        '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls" name="Update" value ="Modify"  onclick="Editclick(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td>' +
                        //'<td><input type="button" class="btn btn-primary" name="Update" value ="Print" onclick="Printclick(this);"/></td></tr>');
                        '<td><button type="button" title="Click Here To Print!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls" name="Print"  onclick="Printclick(this)"><span class="fa  fa-print" style="top: 0px !important;"></span></button></td></tr>');
                        k = k + 1;
                        if (k == 4) {
                            k = 0;
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
        var refno = 0;

        function Editclick(rowid) {
            var selectedrow = $(rowid).closest('tr');
            ///var row = $(thisid).parents('tr');
            var Maintenance_sno = selectedrow[0].cells[1].innerHTML;
            var vehicleno = $(rowid).parent().parent().find('#2').html();
            var amount = $(rowid).parent().parent().find('#3').html();
            var name = $(rowid).parent().parent().find('#4').html();
            var incharge = selectedrow[0].cells[5].innerHTML;
            var maintace_code = selectedrow[0].cells[6].innerHTML;
            var veh_sno = selectedrow[0].cells[7].innerHTML;
            var remarks = selectedrow[0].cells[8].innerHTML;
            refno = maintace_code;
            document.getElementById('slct_vehicle_no').value = veh_sno;
            document.getElementById('txtAMount').value = amount;
            document.getElementById('txtname').value = name;
            document.getElementById('txtincharge').value = incharge;
            document.getElementById('txtRemarks').value = remarks;
            var data = { 'op': 'GetSubPaybleValues', 'Maintenance_sno': Maintenance_sno };
            var s = function (msg) {
                if (msg) {
                    $('#divHeadAcount').setTemplateURL('CashForm.htm');
                    $('#divHeadAcount').processTemplate(msg);
                    var TotRate = 0.0;
                    $('.AmountClass').each(function (i, obj) {
                        if ($(this).text() == "") {
                        }
                        else {
                            TotRate += parseFloat($(this).text());
                        }
                    });
                    TotRate = parseFloat(TotRate).toFixed(2);
                    document.getElementById("txtAMount").innerHTML = TotRate;
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
            document.getElementById('btnSave').innerHTML = "Modify";
            $('#divRaiseMaintaince').css('display', 'block');
            $('#divViewMaintaince').css('display', 'none');
        }
        function Printclick(rowid) {
            var selectedrow = $(rowid).closest('tr');
            var MaintenanceID = selectedrow[0].cells[1].innerHTML;
            var data = { 'op': 'btnMaintenancePrintClick', 'MaintenanceID': MaintenanceID };
            var s = function (msg) {
                if (msg) {

                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
            window.location = "PrintMaintenance.aspx";
        }
        function closevehicleexpendature() {
            document.getElementById("slct_vehicle_no").selectedIndex = 0;
            document.getElementById("divHeadAcount").selectedIndex = 0;
            document.getElementById("txtname").value = "";
            document.getElementById("txtAMount").innerHTML = "";
            document.getElementById("txtincharge").value = "";
            document.getElementById("txtRemarks").value = "";
            refno = 0;
            document.getElementById("btnSave").innerHTML = "Raise";
            Cashform = [];
            $('#divHeadAcount').setTemplateURL('CashForm.htm');
            $('#divHeadAcount').processTemplate(Cashform);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <section class="content-header">
        <h1>
            Vehicle Expenditure<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Vehicle Expenditure</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Vehicle Expenditure Details
                </h3>
            </div>
            <div class="box-body">
    <div style="width: 100%; float: left;">
        <a id="ancRaise" onclick="divRaiseMaintainceClick();" style="width: 30%; font-size: 24px;
            text-decoration: underline;">RaiseMaintenance</a> <a id="ancView" onclick="divViewMaintainceClick();"
                style="padding-left: 32%; font-size: 24px; text-decoration: underline;">View Maintenance</a>
    </div>
    <div id="divRaiseMaintaince">
        <br />
        <table align="center">
        <tr>
        <td>
        <label>  Date </label>
        </td>
        <td>
             <input id="txt_tripstrtdate" class="form-control" type="date" />
        </td>
        </tr>
            <tr>
                <td>
                 <label>   Vehicle No</label>
                </td>
                <td style="height:40px;">
                    <select id="slct_vehicle_no" class="form-control">
                    </select>
                </td>
            </tr>
            <tr>
                <td>
                  <label>  Name </label>
                </td>
                <td style="height:40px;">
                    <input type="text" id="txtname" class="form-control" maxlength="45" placeholder="Enter Name" />
                </td>
            </tr>
            <tr>
                <td>
                  <label>  Head Of Account </label>
                </td>
                <td style="height:40px;">
                    <select id="ddlHeads" class="form-control">
                    </select>
                </td>
                <td style="height:40px;" >
                    <input type="number" id="txtCashAmount" class="form-control" placeholder="Enter Amount" />
                </td>
                <td>
                    <%--<input type="button" id="Button3" value="Add" onclick="BtnCashToAddClick();" class="btn btn-primary"/>--%>
                    <input type="button" id="Button3" style="width: 30px;height: 30px;padding: 0px 0; border-radius: 15px;text-align: center;font-size: 21px; font-weight:bold; line-height: 1.428571429;" value="+" class="btn btn-primary"  onclick="BtnCashToAddClick();" />
                </td>
            </tr>
            <tr>
                <td rowspan="1" colspan="2">
                    <div id="divHeadAcount">
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                 <label>   Amount </label>
                </td>
                <td style="height:40px;">
                    <span id="txtAMount" class="form-control"></span>
                </td>
            </tr>
            <tr>
                <td>
                 <label>   Incharge </label>
                </td>
                <td style="height:40px;">
                    <input type="text" id="txtincharge" class="form-control" maxlength="45" placeholder="Enter Incharge Name" />
                </td>
            </tr>
            <tr>
                <td>
                 <label>   Remarks </label>
                </td>
                <td style="height:40px;">
                    <textarea rows="5" cols="45" id="txtRemarks" class="form-control" maxlength="2000"
                        placeholder="Enter Remarks"></textarea>
                </td>
            </tr>
        </table>
        <table>
        <tr>
            <td style="height: 50px;padding-left: 395px;">
                <%-- <input type="button" id="btnSave" value="Raise" onclick="BtnRaiseVehicleExpendatureClick();"
                    class="btn btn-primary"  />--%>
                <div class="input-group">
                    <div class="input-group-addon">
                    <span class="glyphicon glyphicon-ok" id="btnSave1" onclick="BtnRaiseVehicleExpendatureClick()"></span><span id="btnSave" onclick="BtnRaiseVehicleExpendatureClick()">Raise</span>
                </div>
                </div>
                </td>
                <td style="width:10px;"></td>
                <td>
                    <div class="input-group">
                    <div class="input-group-close">
                    <span class="glyphicon glyphicon-remove" id='close_vehexp1' onclick="closevehicleexpendature()"></span><span id='close_vehexp' onclick="closevehicleexpendature()">Clear</span>
                </div>
                </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divViewMaintaince" style="display:none;">
        <div>
            <table >
                <tr>
                <td  style="padding-left: 145px;padding-top: 7px;">
                </td>
                    <td >
                       <label> From Date :  </label> 
                    </td>
                    <td>
                        <input type="date" id="txtFromDate" class="form-control" />
                    </td>
                    <td>
                     <label>   To Date : </label>
                    </td>
                    <td>
                        <input type="date" id="txtToDate" class="form-control" />
                    </td>
                    <td>
                        <%--<input type="button" id="btnGeneretae" value="Generate" onclick="BtnGenerateClick();"
                            class="btn btn-primary"/>--%>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="fa  fa-refresh" onclick="BtnGenerateClick()"></span><span onclick="BtnGenerateClick()">Generate</span>
                          </div>
                          </div>
                    </td>
                   
                </tr>
            </table>
            <table id="tbl_feecategorylist" class="table table-bordered table-hover dataTable no-footer"
                        role="grid" aria-describedby="example2_info">
                <thead>
                    <tr style="background:#5aa4d0; color: white; font-weight: bold;">
                        <th scope="col">
                            Sno
                        </th>
                        <th scope="col">
                            Maintenance ID
                        </th>
                        <th scope="col">
                            Vehicle No
                        </th>
                        <th scope="col">
                            Amount
                        </th>
                        <th scope="col">
                            Name
                        </th>
                        <th scope="col">
                            Incharge
                        </th>
                         <th scope="col">
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
     </div>
    </div>
    </section>
</asp:Content>

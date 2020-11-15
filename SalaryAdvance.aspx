<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="SalaryAdvance.aspx.cs" Inherits="SalaryAdvance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
 <script src="js/utility.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        get_driverand_helper();
    });
   
    var EmpData = [];
    function get_driverand_helper() {
        var data = { 'op': 'get_driver_and_helper' };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    EmpData = msg;
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

    function ddltypechange(ID) {
        var Type = ID.value;
        var msg = EmpData;
        var data = document.getElementById('ddl_empname');
        var length = data.options.length;
        document.getElementById('ddl_empname').options.length = null;
        if (Type == "Driver") {
            var opt = document.createElement('option');
            opt.innerHTML = "Select Driver";
            opt.value = "Select Driver";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].emp_sno != null && msg[i].emp_type == "Driver") {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].employname;
                    option.value = msg[i].emp_sno;
                    data.appendChild(option);
                }
            }
        }
        if (Type == "Helper") {
            var opt2 = document.createElement('option');
            opt2.innerHTML = "Select Helper";
            opt2.value = "Select Helper";
            opt2.setAttribute("selected", "selected");
            opt2.setAttribute("disabled", "disabled");
            opt2.setAttribute("class", "dispalynone");
            data2.appendChild(opt2);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].emp_sno != null && msg[i].emp_type == "Helper") {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].employname;
                    option.value = msg[i].emp_sno;
                    data2.appendChild(option);
                }
            }
        }
    }
    function btnSalaryadvanceClick() {
        var ddltype = document.getElementById('ddl_type').value;
        if (ddltype == "Select Employee Type" || ddltype == "") {
            alert("Please Select Employee Type");
            return false;
        }
        var ddlempname = document.getElementById('ddl_empname').value;
        if (ddlempname == "") {
            alert("Please Select Employee Name");
            return false;
        }
        var txtamount = document.getElementById('txt_amount').value;
        if (txtamount == "") {
            alert("Please Enter Amount");
            return false;
        }
        var ddlpaymenttype = document.getElementById('ddl_paymenttype').value;
        var txtpaidby = document.getElementById('txt_paidby').value;
        if (txtpaidby == "") {
            alert("Please Enter Paid By");
            return false;
        }
        var txtremarks = document.getElementById('txt_remarks').value;
        if (txtremarks == "") {
            alert("Please Enter Remarks");
            return false;
        }
        var Data = { 'op': 'btnSalaryadvanceClick', 'EmpSno': ddlempname, 'Amount': txtamount, 'Paymenttype': ddlpaymenttype, 'Remarks': txtremarks, 'Paidby': txtpaidby };
        var s = function (msg) {
            if (msg) {
                alert(msg);
                document.getElementById('ddl_type').selectedIndex = 0;
                document.getElementById('txt_amount').value = "";
                document.getElementById('txt_paidby').value = "";
                document.getElementById('ddl_empname').selectedIndex = 0;
                document.getElementById('ddl_paymenttype').selectedIndex = 0;
                document.getElementById('txt_remarks').value = "";
            }
        }
        var e = function (x, h, e) {
        }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        callHandler(Data, s, e);
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Salary Advance<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Salary Advance</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Salary Advance Details
                </h3>
            </div>
            <div class="box-body">
        <table align="center">
            <tr>
                <td>
                    Employee Type
                </td>
                <td style="height:40px;">
                    <select id="ddl_type" class="form-control" style="min-width: 200px;" onchange="ddltypechange(this)">
                        <option value="Driver">Select Employee Type</option>
                        <option value="Driver">Driver</option>
                        <option value="Helper">Helper</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>
                    Employee Name
                </td>
                <td style="height:40px;">
                    <select id="ddl_empname" class="form-control" style="min-width: 200px;">
                    </select>
                </td>
            </tr>
            <tr>
                <td>
                    Payment Type
                </td>
                <td style="height:40px;">
                    <select id="ddl_paymenttype" class="form-control" style="min-width: 200px;">
                        <option value="Cash">Cash</option>
                        <option value="Cheque">Cheque</option>
                        <option value="Bank Transfer">Bank Transfer</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>
                    Amount
                </td>
                <td style="height:40px;">
                    <input id="txt_amount" class="form-control" type="text" placeholder="Enter Amount" />
                </td>
            </tr>
            <tr>
                <td>
                    Paid By
                </td>
                <td style="height:40px;">
                    <input id="txt_paidby" class="form-control" type="text" placeholder="Enter  Paid By" />
                </td>
            </tr>
            <tr>
                <td>
                    Remarks
                </td>
                <td style="height:40px;">
                    <textarea rows="3" cols="45" id="txt_remarks" class="form-control" maxlength="200"
                        placeholder="Enter Remarks"></textarea>
                </td>
            </tr>
            <tr>
            <td>
            <br />
            </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <input id="btn_Starttrip" type="button" value="Save" class="btn btn-primary" onclick="btnSalaryadvanceClick();" />
                </td>
            </tr>
        </table>
    </div>
    </div>
    </section>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="LoanEntry.aspx.cs" Inherits="LoanEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/utility.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            getallveh_nos();
            getall_TermLoanDetails();
        });
        function getall_TermLoanDetails() {
            var data = { 'op': 'getall_TermLoanDetails' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillTermLoanDetails(msg);
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
        function fillTermLoanDetails(msg) {
            var k = 0;
            var colorue = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            var results = '<div ><table  class="table table-bordered table-hover dataTable no-footer" aria-describedby="example2_info">';
            results += '<thead><tr style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;"></th><th scope="col" style="text-align:center" style="font-weight: bold;">VehicleNumber</th><th scope="col" style="font-weight: bold;">MFGDate</th><th scope="col" style="font-weight: bold;">TermLoanDate</th><th scope="col" style="font-weight: bold;">Type</th><th scope="col" style="font-weight: bold;">LoanAmount</th><th scope="col" style="font-weight: bold;">InstalmentAmount</th><th scope="col" style="font-weight: bold;">TermLoanNumber</th><th scope="col" style="font-weight: bold;">InstalmentDate</th><th scope="col" style="font-weight: bold;">IntersetPer</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + colorue[k] + '"><td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="getme(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td>';
                //results += '<th scope="row" class="1" style="text-align:center;">' + msg[i].VehicleNo + '</th>';
                results += '<td   class="1" style="font-weight: 700;"><i class="fa  fa-truck" aria-hidden="true"></i>&nbsp;<span id="1">' + msg[i].VehicleNo + '</span></td>';
                //results += '<td data-title="Code" class="2">' + msg[i].mfgdate + '</td>';
                results += '<td   class="2"><i class="fa fa-calendar " aria-hidden="true"></i><span id="2">' + msg[i].mfgdate + '</span></td>';
                //results += '<td style="display:none" class="4">' + msg[i].Reason + '</td>';
                //results += '<td  class="3">' + msg[i].termloandate + '</td>';
                results += '<td   class="3"><i class="fa fa-calendar-check-o " aria-hidden="true"></i>&nbsp;<span id="3">' + msg[i].mfgdate + '</span></td>';
                results += '<td  class="4">' + msg[i].type + '</td>';
                //results += '<td  class="5">' + msg[i].loanamount + '</td>';
                results += '<td   class="5"><i class="fa fa-rupee " aria-hidden="true"></i>&nbsp;<span id="5">' + msg[i].loanamount + '</span></td>';
                //results += '<td  class="6">' + msg[i].instalamount + '</td>';
                results += '<td   class="6"><i class="fa fa-rupee " aria-hidden="true"></i>&nbsp;<span id="6">' + msg[i].instalamount + '</span></td>';
                results += '<td  class="7">' + msg[i].termloanno + '</td>';
                //results += '<td  class="8">' + msg[i].instaldate + '</td>';
                results += '<td   class="8"><i class="fa fa-calendar-times-o" aria-hidden="true"></i>&nbsp;<span id="8">' + msg[i].instaldate + '</span></td>';
                results += '<td  class="9">' + msg[i].interest_per + '</td>';
                results += '<td style="display:none" class="15">' + msg[i].ledgername + '</td>';
                results += '<td style="display:none" class="10">' + msg[i].sno + '</td>';
                results += '<td style="display:none" class="11">' + msg[i].vm_sno + '</td>';
                results += '<td style="display:none" class="12">' + msg[i].totalinstall + '</td>';
                results += '<td style="display:none" class="13">' + msg[i].bankname + '</td>';
                results += '<td style="display:none" class="14">' + msg[i].com_install + '</td>';
                results += '<td style="display:none" class="16">' + msg[i].ledgercode + '</td>';
                results += '<td></td></tr>';
                k = k + 1;
                if (k == 4) {
                    k = 0;
                }
            }
            results += '</table></div>';
            $("#div_Termloandata").html(results);
        }
        var refno = 0;
        function getme(thisid) {
            var VehicleNo = $(thisid).parent().parent().find('#1').html();
            var mfgdate = $(thisid).parent().parent().find('#2').html();
            var termloandate = $(thisid).parent().parent().find('#3').html();
            var type = $(thisid).parent().parent().children('.4').html();
            var loanamount = $(thisid).parent().parent().find('#5').html();
            var instalamount = $(thisid).parent().parent().find('#6').html();
            var termloanno = $(thisid).parent().parent().children('.7').html();
            var instaldate = $(thisid).parent().parent().find('#8').html();
            var interest_per = $(thisid).parent().parent().children('.9').html();
            refno = $(thisid).parent().parent().children('.10').html();
            var vm_sno = $(thisid).parent().parent().children('.11').html();
            var totalinstall = $(thisid).parent().parent().children('.12').html();
            var bankname = $(thisid).parent().parent().children('.13').html();
            var com_install = $(thisid).parent().parent().children('.14').html();
            var ledgername = $(thisid).parent().parent().children('.15').html();
            var ledgercode = $(thisid).parent().parent().children('.16').html();
            document.getElementById('slct_vehicle_no').value = vm_sno;
            document.getElementById('txt_mfgdate').value = mfgdate
            document.getElementById('txt_termloanno').value = termloanno;
            document.getElementById('txt_termloanamount').value = loanamount;
            document.getElementById('txt_monthlyinstal').value = instalamount;
            document.getElementById('slct_type').value = type;
            document.getElementById('txt_bankname').value = bankname;
            document.getElementById('txt_paidinstal').value = com_install;
            document.getElementById('txt_totalinstal').value = totalinstall;
            document.getElementById('txt_instalmentdate').value = instaldate;
            document.getElementById('txt_Interestper').value = interest_per;
            document.getElementById('txt_ledgername').value = ledgername;
            document.getElementById('txt_ledgercode').value = ledgercode;
            document.getElementById('txt_termLoandate').value = termloandate;
            document.getElementById('btn_LoanEntry').innerHTML = "Modify";
            $('#div_Termloandata').hide();
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

        function btnVehicleTermLoanEntrySaveClick() {
            var ddlvehicleno = document.getElementById('slct_vehicle_no').value;
            var txtmfgdate = document.getElementById('txt_mfgdate').value;
            var txttermloanno = document.getElementById('txt_termloanno').value;
            var txttermloanamount = document.getElementById('txt_termloanamount').value;
            var txtmonthlyinstal = document.getElementById('txt_monthlyinstal').value;
            var txtinstalmentdate = document.getElementById('txt_instalmentdate').value;
            var txttotalinstal = document.getElementById('txt_totalinstal').value;
            var txtpaidinstal = document.getElementById('txt_paidinstal').value;
            var txtbankname = document.getElementById('txt_bankname').value;
            var txtremarks = document.getElementById('txt_remarks').value;
            var slcttype = document.getElementById('slct_type').value;
            var txt_Interestper = document.getElementById('txt_Interestper').value;
            var btn_save = document.getElementById('btn_LoanEntry').innerHTML;
            var txt_ledgername = document.getElementById('txt_ledgername').value;
            var ledgercode = document.getElementById('txt_ledgercode').value;
            var txt_termLoandate = document.getElementById('txt_termLoandate').value;
            if (ddlvehicleno == "Select Vehicle No" || ddlvehicleno == "") {
                alert("Please Select Vehicle No");
                $('#slct_vehicle_no').focus();
                return false;
            }
            if (txtmfgdate == "") {
                alert("Please Select mfg date");
                $('#txt_mfgdate').focus();
                return false;
            }
            if (txttermloanno == "") {
                alert("Please Term loan no");
                $('#txt_termloanno').focus();
                return false;
            }
            if (txttermloanamount == "") {
                alert("Please Enter term loan amount");
                $('#txt_termloanamount').focus();
                return false;
            }
            if (txtmonthlyinstal == "") {
                alert("Please Enter monthly instal");
                $('#txt_monthlyinstal').focus();
                return false;
            }
            if (txtinstalmentdate == "") {
                alert("Please Enter instal date");
                $('#txt_instalmentdate').focus();
                return false;
            }
            if (txttotalinstal == "") {
                alert("Please Enter total instal");
                $('#txt_totalinstal').focus();
                return false;
            }
            if (txtpaidinstal == "") {
                alert("Please Enter paid instal");
                $('#txt_paidinstal').focus();
                return false;
            }
            if (txtbankname == "") {
                alert("Please Enter bank name");
                $('#txt_bankname').focus();
                return false;
            }
            if (txt_Interestper == "") {
                alert("Please Enter Interest percentage");
                $('#txt_Interestper').focus();
                return false;
            }
            if (txt_ledgername == "") {
                alert("Please Enter ledgername");
                $('#txt_ledgername').focus();
                return false;
            }
            if (txt_termLoandate == "") {
                alert("Please Enter TermLoandate");
                $('#txt_termLoandate').focus();
                return false;
            }
            var Data = { 'op': 'btnVehicleTermLoanEntrySaveClick', 'ledgercode': ledgercode, 'ledgername': txt_ledgername, 'termLoandate': txt_termLoandate, 'refno': refno, 'btn_save': btn_save, 'Interestper': txt_Interestper, 'vehicleno': ddlvehicleno, 'mfgdate': txtmfgdate, 'termloanno': txttermloanno, 'termloanamount': txttermloanamount, 'monthlyinstal': txtmonthlyinstal, 'instalmentdate': txtinstalmentdate, 'totalinstal': txttotalinstal, 'paidinstal': txtpaidinstal, 'bankname': txtbankname, 'remarks': txtremarks, 'Type': slcttype };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    getall_TermLoanDetails();
                    forclearall();
                }
            }
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(Data, s, e);
        }
        function forclearall() {
            document.getElementById('slct_vehicle_no').selectedIndex = 0;
            document.getElementById('slct_type').selectedIndex = 0;
            document.getElementById('txt_mfgdate').value = "";
            document.getElementById('txt_termloanno').value = "";
            document.getElementById('txt_termloanamount').value = "";
            document.getElementById('txt_monthlyinstal').value = "";
            document.getElementById('txt_instalmentdate').value = "";
            document.getElementById('txt_totalinstal').value = "";
            document.getElementById('txt_paidinstal').value = "";
            document.getElementById('txt_bankname').value = "";
            document.getElementById('txt_remarks').value = "";
            document.getElementById('txt_Interestper').value = "";
            document.getElementById('txt_ledgername').value = "";
            document.getElementById('txt_ledgercode').value = "";
            document.getElementById('txt_termLoandate').value = "";
            document.getElementById('btn_LoanEntry').innerHTML = "Save";
            $('#div_Termloandata').show();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Vehicle Term Loans<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Vehicle Term Loans</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Vehicle Term Loan Details
                </h3>
            </div>
            <div class="box-body">
                <div style="padding: 20px; text-align: center;">
                    <table align="center">
                        <tr>
                            <td>
                                <label>
                                    Vehicle Number <span style="color: red;">*</span></label>
                                <select id="slct_vehicle_no" class="form-control">
                                </select>
                                <label id="lbl_error_selectveh" class="errormessage">
                                    * Please select Vehicle</label>
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <label>
                                    Type <span style="color: red;">*</span></label>
                                <select id="slct_type" class="form-control">
                                    <option>Chasis</option>
                                    <option>Body</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Term Loan Number <span style="color: red;">*</span></label>
                                <input id="txt_termloanno" class="form-control" type="text" placeholder="Term Loan Number">
                                <label id="lbl_termloanno" class="errormessage">
                                    * Please Enter Term Loan No</label>
                            </td>
                             <td style="width: 5px;">
                            </td>
                            <td>
                                <label>
                                    Term Loan Amount <span style="color: red;">*</span></label>
                                <input id="txt_termloanamount" class="form-control" type="text" placeholder="Term Loan Amount">
                                <label id="lbl_termloanamount" class="errormessage">
                                    * Please Enter Term Loan Amount</label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Monthly Instalment <span style="color: red;">*</span></label>
                                <input id="txt_monthlyinstal" class="form-control" type="text" placeholder=" Monthly Instalment">
                                <label id="lbl_monthlyinstal" class="errormessage">
                                    * Please Enter Monthly Instal</label>
                            </td>
                             <td style="width: 5px;">
                            </td>
                            <td>
                                <label>
                                    Instalment Date <span style="color: red;">*</span></label>
                                <input id="txt_instalmentdate" class="form-control" type="date">
                                <label id="lbl_instalmentdate" class="errormessage">
                                    * Please select Instalment Date
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Total Instalment <span style="color: red;">*</span></label>
                                <input id="txt_totalinstal" class="form-control" type="text" placeholder="Total Instalment">
                                <label id="lbl_totalinstal" class="errormessage">
                                    * Please Enter Total Instal</label>
                            </td>
                             <td style="width: 5px;">
                            </td>
                            <td>
                                <label>
                                    Paid Instalment <span style="color: red;">*</span></label>
                                <input id="txt_paidinstal" class="form-control" type="text" placeholder="Paid Instalment">
                                <label id="lbl_paidinstal" class="errormessage">
                                    * Please Enter Total Instal</label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Bank Name <span style="color: red;">*</span></label>
                                <input id="txt_bankname" class="form-control" type="text" placeholder="Bank Name">
                                <label id="lbl_bankname" class="errormessage">
                                    * Please Enter Total Instal</label>
                            </td>
                             <td style="width: 5px;">
                            </td>
                            <td>
                                <label>
                                    MFG Date <span style="color: red;">*</span></label>
                                <input id="txt_mfgdate" class="form-control" type="date">
                                <label id="lbl_mfgdate" class="errormessage">
                                    * Please select Mfg Date</label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Remarks <span style="color: red;">*</span></label>
                                <input id="txt_remarks" class="form-control" type="text" placeholder="Remarks">
                                <label id="Label1" class="errormessage">
                                    * Please Enter Total Instal</label>
                            </td>
                             <td style="width: 5px;">
                            </td>
                            <td>
                                <label>
                                    Interest % <span style="color: red;">*</span></label>
                                <input id="txt_Interestper" class="form-control" type="text" placeholder="Interest %">
                                <label id="Label2" class="errormessage">
                                    * Please Enter Interest %</label>
                            </td>
                        </tr>
                        <tr>
                         <td>
                                <label>
                                    Ledger Code <span style="color: red;">*</span></label>
                                <input id="txt_ledgercode" class="form-control" type="text" placeholder="Ledger Code">
                            </td>
                             <td style="width: 5px;">
                            </td>
                         <td>
                                <label>
                                    Ledger Name <span style="color: red;">*</span></label>
                                <input id="txt_ledgername" class="form-control" type="text" placeholder="Ledger Name">
                            </td>
                        </tr>
                        <tr>
                           
                             <td>
                                <label>
                                    Term Loan Date <span style="color: red;">*</span></label>
                                <input id="txt_termLoandate" class="form-control" type="date">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr>
                           <%-- <td>
                                <input id="btn_LoanEntry" type="button" value="Save" class="btn btn-primary" onclick="btnVehicleTermLoanEntrySaveClick();" />
                            </td>--%>
                            
                        </tr>
                    </table>
                    <table>
                    <tr>
                    <td style="padding-left: 446px;">
                        <div class="input-group">
                            <div class="input-group-addon">
                            <span class="glyphicon glyphicon-ok" id="btn_LoanEntry1" onclick="btnVehicleTermLoanEntrySaveClick()"></span><span id="btn_LoanEntry" onclick="btnVehicleTermLoanEntrySaveClick()">Save</span>
                        </div>
                        </div>
                        </td>
                        <td style="width:10px;"></td>
                        <td>
                            <div class="input-group">
                            <div class="input-group-close">
                            <span class="glyphicon glyphicon-remove" id='btn_clear1' onclick="forclearall()"></span><span id='btn_clear' onclick="forclearall()">Clear</span>
                        </div>
                        </div>
                        </td>
                    </tr>
                    </table>
                </div>
              <div>
                <div id="div_Termloandata" style="overflow:auto;height:500px" >
                </div>
            </div>
            </div>
        </div>
    </section>
</asp:Content>

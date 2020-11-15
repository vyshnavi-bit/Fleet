<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" 
  CodeFile="termloantransactionentry.aspx.cs" Inherits="termloantransactionentry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script src="js/utility.js" type="text/javascript"></script>
 <script type="text/javascript">
     $(function () {
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
         $('#txt_date').val(yyyy + '-' + mm + '-' + dd);
     });
     function get_termloan_transaction_details() {
         var date = document.getElementById('txt_date').value;
         var data = { 'op': 'get_termloan_transaction_details','date':date };
         var s = function (msg) {
             if (msg) {
                 if (msg.length > 0) {
                     $("#termtrans_fillform").show();
                     var results = '<div    style="overflow:auto;"><table id="table_termloans_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                     results += '<thead><tr><th scope="col">Ledger Name</th><th scope="col">Vehicle no</th><th scope="col">Amount</th><th scope="col">Remarks</th></tr></thead></tbody>';
                     for (var i = 0; i < msg.length; i++) {
                         results += '<tr>';
                         results += '<td scope="row" class="1" style="font-weight: 700;">' + msg[i].ledgername + '</td>';
                         results += '<td scope="row" class="2" style="font-weight: 700;">' + msg[i].vehiclename + '</td>';
                         results += '<td style="display:none"  scope="row" class="3" style="font-weight: 700;">' + msg[i].whcode + '</td>';
                         results += '<td style="display:none"  scope="row" class="5" style="font-weight: 700;">' + msg[i].ledgercode + '</td>';
                         results += '<td  style="display:none" ><input id="txt_vehiclesno" class="form-control" value="' + msg[i].vehiclesno + '" type="text" name="vehiclesno" ></td>';
                         results += '<td><input id="txt_amount" class="form-control" value="' + msg[i].amount + '" type="text"  name="amount"></td>';
                         results += '<td scope="row" class="4" style="font-weight: 700;">' + msg[i].remarks + '</td>';
                         results += '</tr>';
                     }
                     results += '</table></div>';
                     $("#termtrans_fillform").html(results);
                 }
                 else {
                 }
             }
             else{ 
             }
         };
         var e = function (x, h, e) {
         };
         $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
         callHandler(data, s, e);
     }
     function save_termloans_details_click() {
         var date = document.getElementById('txt_date').value;
         var btnval = document.getElementById('btn_save').value;
         if (date == "") {
             alert("Select date");
             return false;
         }
         var termloansdetails = [];
         $('#table_termloans_details> tbody > tr').each(function () {
             var ledgername = $(this).children('.1').html();
            var vehiclename = $(this).children('.2').html();
            var whcode = $(this).children('.3').html();
            var remarks = $(this).children('.4').html();
            var ledgercode = $(this).children('.5').html();
             var vehiclesno = $(this).find('[name="vehiclesno"]').val();
             var amount = $(this).find('[name="amount"]').val();
             if (amount == "") {
             }
             else {
                 termloansdetails.push({ 'vehiclesno': vehiclesno, 'amount': amount, 'ledgername': ledgername, 'vehiclename': vehiclename, 'whcode': whcode, 'remarks': remarks, 'ledgercode': ledgercode
                 });
             }
         });
         if (termloansdetails.length == "0") {
             alert("Please enter Ammount");
             return false;
         }
         var data = { 'op': 'termlons_save_start' };
         var s = function (msg) {
             if (msg) {
                 for (var i = 0; i < termloansdetails.length; i++) {
                     var Data = { 'op': 'termloans_save_RowData', 'row_detail': termloansdetails[i], 'end': 'N' };
                     if (i == termloansdetails.length - 1) {
                         Data = { 'op': 'termloans_save_RowData', 'row_detail': termloansdetails[i], 'end': 'Y' };
                     }
                     var s = function (msg) {
                         if (msg == 'Y') {
                             var Data = { 'op': 'termloans_save_edit_data', 'date': date, 'btnval': btnval
                             };
                             var s = function (msg) {
                                 if (msg) {
                                     alert(msg);
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

 </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <section class="content-header">
        <h1>
            Term Loan Transactions<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Term Loan Transactions</a></li>
        </ol>
    </section>
    <section class="content">
     <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Term Loan Transaction Details
                </h3>
            </div>
            <div class="box-body">
                <div>
                    <table align="center">
                     <tr>
                            <td style="height: 40px;">
                                <label>
                                  Date</label>
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td>
                                <input id="txt_date" class="form-control" type="date" name="date"  />
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td>
                               <input id="txt_generatebutton" class="btn btn-primary" type="button" name="button" value="Generate" onclick="get_termloan_transaction_details();" />
                            </td>
                    </tr>
                    </table>
                </div>
            </div>
            <div class="box-body" style="font-size:12px;">
                <div id='termtrans_fillform'>
                </div>
                <div >
                    <table align="center">
                     <tr>
                            <td>
                               <input id="btn_save" class="btn btn-primary" type="button" name="button" value="Save" onclick="save_termloans_details_click();" />
                            </td>
                    </tr>
                    </table>
                </div>
            </div>
      </div>
    </section>
</asp:Content>


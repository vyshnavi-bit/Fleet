<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="SAP_DB_Query.aspx.cs" Inherits="SAP_DB_Query" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">
    $(function () {
    });
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
    function update_sales_data() {
        var module = document.getElementById('slct_module').value;
        var data = { 'op': 'update_sales_data', 'module': module };
        var s = function (msg) {
            if (msg) {
                if (msg == "Updated Successfully") {
                    alert(msg);
                    return false;
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
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i> SAP DB
                </h3>
            </div>
            <div class="box-body" style="font-size:12px">
                <div style="padding: 20px;">
                    <div id="sap_div">
                        <table style="width: 100%;">
                            <tr>
                            <td style="width: 10%;">
                            <label>
                                       Module<span style="color: red;">*</span></label>
                            </td>
                                <td style="width: 25%;"> 
                                    <select id="slct_module" class="form-control" >
                                        <option selected disabled value="Select Module">Select Module</option>
                                        <option  value="EMROIGN">Production</option>
                                        <option value="EMRORDR">Sales Order</option>
                                        <option value="EMROWTR">Stock Transfer</option>
                                        <option  value="EMROINV">Sales Invoice</option>
                                        <option value="EMRORIN">Credit Memo</option>
                                        <option  value="EMROIGE">Goods Issue</option>
                                        <option  value="EMROJDT">Journal Voucher</option>
                                        <option  value="EMROPDN">MRN/GRN</option>
                                        <option  value="EMROPCH">Purchase Invoice</option>
                                        <option  value="EMROPCHS">Stores Purchase</option>
                                        <option  value="EMROJDT">Journal Voucher</option>
                                        <option  value="EMROJDTP">Journal Payments</option>
                                        <option  value="EMRORCT">Receipts</option>
                                        <option  value="EMROVPM">Payments</option>
                                        <option value="EMROPOR">Purchase Order</option>
                                         </td>
                                <td style="width: 5px;">
                                </td>
                                 <td>
                                        <input id='btn_sapupdate' type="button" class="btn btn-primary" name="submit" value='Update'
                                            onclick="update_sales_data()" />
                                    </td>
                            </tr>
                        </table>
                    </div>
                    <div id="div_sapreport"></div>
                        </div>
                    </div>
                </div>
    </section>
</asp:Content>


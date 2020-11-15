<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="SAPReport.aspx.cs" Inherits="SAPReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
        function Generate_data() {
            document.getElementById('txt_pendingrows').innerHTML = "";
            var module = document.getElementById('slct_mdl').value;
            var data = { 'op': 'generate_sales_data', 'module': module };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillinvoicedetails(msg);
                        $("#div_sapreport").show();
                    }
                    else {
                        $("#div_sapreport").hide();
                        alert("No data were found:");
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
        function fillinvoicedetails(msg) {
            var k = 1;
            var module = document.getElementById('slct_mdl').value;
            document.getElementById('txt_pendingrows').innerHTML = msg.length;
            if (module == "EMROIGN") {
                var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr><th scope="col">Sno</th><th scope="col">Date</th><th scope="col">ItemCode</th><th scope="col">Description</th><th scope="col">WhsCode</th><th scope="col">Quantity</th><th scope="col">Price</th><th scope="col">B1upload</th><th scope="col">Processed</th></tr></thead></tbody>';
                for (var i = 0; i < msg.length; i++) {
                    results += '<tr><td data-title="transactiondate" class="1">' + k++ + '</td>';
                    results += '<td data-title="svdsno" class="3" >' + msg[i].taxdate + '</td>';
                    results += '<td data-title="kms" class="1">' + msg[i].itemcode + '</td>';
                    results += '<td data-title="kms" class="2">' + msg[i].desc + '</td>';
                    results += '<td data-title="kms" class="3">' + msg[i].whscode + '</td>';
                    results += '<td data-title="kms" class="4">' + msg[i].qty + '</td>';
                    results += '<td data-title="kms" class="5">' + msg[i].price + '</td>';
                    results += '<td data-title="kms" class="7">' + msg[i].b1upload + '</td>';
                    results += '<td data-title="kms" class="8">' + msg[i].processed + '</td></tr>';
                }
                results += '</table></div>';
                $("#div_sapreport").html(results);
            }
            if (module == "EMRORDR") {
                var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr><th scope="col">Sno</th><th scope="col">Date</th><th scope="col">CardCode</th><th scope="col">CardName</th><th scope="col">ReferenceNo</th><th scope="col">ItemCode</th><th scope="col">Description</th><th scope="col">WhsCode</th><th scope="col">Quantity</th><th scope="col">Price</th><th scope="col">B1Upload</th><th scope="col">Processed</th></tr></thead></tbody>';
                for (var i = 0; i < msg.length; i++) {
                    results += '<tr><td data-title="transactiondate" class="1">' + k++ + '</td>';
                    results += '<td data-title="svdsno" class="3" >' + msg[i].taxdate + '</td>';
                    results += '<td data-title="svdsno" class="3" >' + msg[i].cardcode + '</td>';
                    results += '<td data-title="regno" class="4" >' + msg[i].cardname + '</td>';
                    results += '<td data-title="cost" class="9">' + msg[i].refno + '</td>';
                    results += '<td data-title="kms" class="10">' + msg[i].itemcode + '</td>';
                    results += '<td data-title="kms" class="11">' + msg[i].desc + '</td>';
                    results += '<td data-title="kms" class="12">' + msg[i].whscode + '</td>';
                    results += '<td data-title="kms" class="13">' + msg[i].qty + '</td>';
                    results += '<td data-title="kms" class="14">' + msg[i].price + '</td>';
                    results += '<td data-title="kms" class="15">' + msg[i].b1upload + '</td>';
                    results += '<td data-title="kms" class="15">' + msg[i].processed + '</td></tr>';
                }
                results += '</table></div>';
                $("#div_sapreport").html(results);
            }
            if (module == "EMROWTR") {
                var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr><th scope="col">Sno</th><th scope="col">Date</th><th scope="col">FromWareHouse</th><th scope="col">ToWareHouse</th><th scope="col">ReferenceNo</th><th scope="col">ItemCode</th><th scope="col">Description</th><th scope="col">Quantity</th><th scope="col">Price</th><th scope="col">B1Upload</th><th scope="col">Processed</th></tr></thead></tbody>';
                for (var i = 0; i < msg.length; i++) {
                    results += '<tr><td data-title="transactiondate" class="1">' + k++ + '</td>';
                    results += '<td data-title="svdsno" class="3" >' + msg[i].taxdate + '</td>';
                    results += '<td data-title="regno" class="4" >' + msg[i].fromwarehouse + '</td>';
                    results += '<td data-title="regno" class="4" >' + msg[i].towarehouse + '</td>';
                    results += '<td data-title="cost" class="9">' + msg[i].refno + '</td>';
                    results += '<td data-title="kms" class="10">' + msg[i].itemcode + '</td>';
                    results += '<td data-title="kms" class="11">' + msg[i].desc + '</td>';
                    results += '<td data-title="kms" class="13">' + msg[i].qty + '</td>';
                    results += '<td data-title="kms" class="14">' + msg[i].price + '</td>';
                    results += '<td data-title="kms" class="15">' + msg[i].b1upload + '</td>';
                    results += '<td data-title="kms" class="15">' + msg[i].processed + '</td></tr>';
                }
                results += '</table></div>';
                $("#div_sapreport").html(results);
            }
            if (module == "EMROINV") {
                var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr><th scope="col">Sno</th><th scope="col">Date</th><th scope="col">CardCode</th><th scope="col">CardName</th><th scope="col">ReferenceNo</th><th scope="col">ItemCode</th><th scope="col">Description</th><th scope="col">WhsCode</th><th scope="col">Quantity</th><th scope="col">Price</th><th scope="col">B1Upload</th><th scope="col">Processed</th></tr></thead></tbody>';
                for (var i = 0; i < msg.length; i++) {
                    results += '<tr><td data-title="transactiondate" class="1">' + k++ + '</td>';
                    results += '<td data-title="svdsno" class="3" >' + msg[i].taxdate + '</td>';
                    results += '<td data-title="svdsno" class="3" >' + msg[i].cardcode + '</td>';
                    results += '<td data-title="regno" class="4" >' + msg[i].cardname + '</td>';
                    results += '<td data-title="cost" class="9">' + msg[i].refno + '</td>';
                    results += '<td data-title="kms" class="10">' + msg[i].itemcode + '</td>';
                    results += '<td data-title="kms" class="11">' + msg[i].desc + '</td>';
                    results += '<td data-title="kms" class="12">' + msg[i].whscode + '</td>';
                    results += '<td data-title="kms" class="13">' + msg[i].qty + '</td>';
                    results += '<td data-title="kms" class="14">' + msg[i].price + '</td>';
                    results += '<td data-title="kms" class="15">' + msg[i].b1upload + '</td>';
                    results += '<td data-title="kms" class="15">' + msg[i].processed + '</td></tr>';
                }
                results += '</table></div>';
                $("#div_sapreport").html(results);
            }
            if (module == "EMRORIN") {
                var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr><th scope="col">Sno</th><th scope="col">Date</th><th scope="col">CardCode</th><th scope="col">CardName</th><th scope="col">ReferenceNo</th><th scope="col">Description</th><th scope="col">WhsCode</th><th scope="col">Price</th><th scope="col">B1Upload</th><th scope="col">Processed</th></tr></thead></tbody>';
                for (var i = 0; i < msg.length; i++) {
                    results += '<tr><td data-title="transactiondate" class="1">' + k++ + '</td>';
                    results += '<td data-title="svdsno" class="3" >' + msg[i].taxdate + '</td>';
                    results += '<td data-title="svdsno" class="3" >' + msg[i].cardcode + '</td>';
                    results += '<td data-title="regno" class="4" >' + msg[i].cardname + '</td>';
                    results += '<td data-title="cost" class="9">' + msg[i].refno + '</td>';
                    results += '<td data-title="kms" class="11">' + msg[i].desc + '</td>';
                    results += '<td data-title="kms" class="12">' + msg[i].whscode + '</td>';
                    results += '<td data-title="kms" class="14">' + msg[i].price + '</td>';
                    results += '<td data-title="kms" class="15">' + msg[i].b1upload + '</td>';
                    results += '<td data-title="kms" class="15">' + msg[i].processed + '</td></tr>';
                }
                results += '</table></div>';
                $("#div_sapreport").html(results);
            }
            if (module == "EMROIGE") {
                var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr><th scope="col">Sno</th><th scope="col">Date</th><th scope="col">WhsCode</th><th scope="col">ReferenceNo</th><th scope="col">Item Code</th><th scope="col">Description</th><th scope="col">Qty</th><th scope="col">Price</th><th scope="col">B1Upload</th><th scope="col">Processed</th></tr></thead></tbody>';
                for (var i = 0; i < msg.length; i++) {
                    results += '<tr><td data-title="transactiondate" class="1">' + k++ + '</td>';
                    results += '<td data-title="svdsno" class="3" >' + msg[i].taxdate + '</td>';
                    results += '<td data-title="kms" class="12">' + msg[i].whscode + '</td>';
                    results += '<td data-title="cost" class="9">' + msg[i].refno + '</td>';
                    results += '<td data-title="cost" class="9">' + msg[i].itemcode + '</td>';
                    results += '<td data-title="kms" class="11">' + msg[i].desc + '</td>';
                    results += '<td data-title="kms" class="11">' + msg[i].qty + '</td>';
                    results += '<td data-title="kms" class="14">' + msg[i].price + '</td>';
                    results += '<td data-title="kms" class="15">' + msg[i].b1upload + '</td>';
                    results += '<td data-title="kms" class="15">' + msg[i].processed + '</td></tr>';
                }
                results += '</table></div>';
                $("#div_sapreport").html(results);
            }
            if (module == "EMROJDT") {
                var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr><th scope="col">Sno</th><th scope="col">Date</th><th scope="col">WhsCode</th><th scope="col">ReferenceNo</th><th scope="col">Item Code</th><th scope="col">Description</th><th scope="col">Debit</th><th scope="col">Credit</th><th scope="col">B1Upload</th><th scope="col">Processed</th></tr></thead></tbody>';
                for (var i = 0; i < msg.length; i++) {
                    results += '<tr><td data-title="transactiondate" class="1">' + k++ + '</td>';
                    results += '<td data-title="svdsno" class="3" >' + msg[i].taxdate + '</td>';
                    results += '<td data-title="kms" class="12">' + msg[i].whscode + '</td>';
                    results += '<td data-title="cost" class="9">' + msg[i].refno + '</td>';
                    results += '<td data-title="cost" class="9">' + msg[i].itemcode + '</td>';
                    results += '<td data-title="kms" class="11">' + msg[i].desc + '</td>';
                    results += '<td data-title="kms" class="11">' + msg[i].debit + '</td>';
                    results += '<td data-title="kms" class="14">' + msg[i].credit + '</td>';
                    results += '<td data-title="kms" class="15">' + msg[i].b1upload + '</td>';
                    results += '<td data-title="kms" class="15">' + msg[i].processed + '</td></tr>';
                }
                results += '</table></div>';
                $("#div_sapreport").html(results);
            }
            if (module == "EMROJDTP") {
                var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr><th scope="col">Sno</th><th scope="col">Date</th><th scope="col">WhsCode</th><th scope="col">ReferenceNo</th><th scope="col">Item Code</th><th scope="col">Description</th><th scope="col">Debit</th><th scope="col">Credit</th><th scope="col">B1Upload</th><th scope="col">Processed</th></tr></thead></tbody>';
                for (var i = 0; i < msg.length; i++) {
                    results += '<tr><td data-title="transactiondate" class="1">' + k++ + '</td>';
                    results += '<td data-title="svdsno" class="3" >' + msg[i].taxdate + '</td>';
                    results += '<td data-title="kms" class="12">' + msg[i].whscode + '</td>';
                    results += '<td data-title="cost" class="9">' + msg[i].refno + '</td>';
                    results += '<td data-title="cost" class="9">' + msg[i].itemcode + '</td>';
                    results += '<td data-title="kms" class="11">' + msg[i].desc + '</td>';
                    results += '<td data-title="kms" class="11">' + msg[i].debit + '</td>';
                    results += '<td data-title="kms" class="14">' + msg[i].credit + '</td>';
                    results += '<td data-title="kms" class="15">' + msg[i].b1upload + '</td>';
                    results += '<td data-title="kms" class="15">' + msg[i].processed + '</td></tr>';
                }
                results += '</table></div>';
                $("#div_sapreport").html(results);
            }
            if (module == "EMROPDN") {
                var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr><th scope="col">Sno</th><th scope="col">Date</th><th scope="col">CardCode</th><th scope="col">CardName</th><th scope="col">ReferenceNo</th><th scope="col">ItemCode</th><th scope="col">Description</th><th scope="col">WhsCode</th><th scope="col">Quantity</th><th scope="col">Price</th><th scope="col">B1Upload</th><th scope="col">Processed</th></tr></thead></tbody>';
                for (var i = 0; i < msg.length; i++) {
                    results += '<tr><td data-title="transactiondate" class="1">' + k++ + '</td>';
                    results += '<td data-title="svdsno" class="3" >' + msg[i].taxdate + '</td>';
                    results += '<td data-title="svdsno" class="3" >' + msg[i].cardcode + '</td>';
                    results += '<td data-title="regno" class="4" >' + msg[i].cardname + '</td>';
                    results += '<td data-title="cost" class="9">' + msg[i].refno + '</td>';
                    results += '<td data-title="kms" class="10">' + msg[i].itemcode + '</td>';
                    results += '<td data-title="kms" class="11">' + msg[i].desc + '</td>';
                    results += '<td data-title="kms" class="12">' + msg[i].whscode + '</td>';
                    results += '<td data-title="kms" class="13">' + msg[i].qty + '</td>';
                    results += '<td data-title="kms" class="14">' + msg[i].price + '</td>';
                    results += '<td data-title="kms" class="15">' + msg[i].b1upload + '</td>';
                    results += '<td data-title="kms" class="15">' + msg[i].processed + '</td></tr>';
                }
                results += '</table></div>';
                $("#div_sapreport").html(results);
            }
            if (module == "EMROPCH") {
                document.getElementById('txt_pendingrows').innerHTML = msg.length + "  --->It Showing TOP 1500 rows";
                var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr><th scope="col">Sno</th><th scope="col">Date</th><th scope="col">CardCode</th><th scope="col">CardName</th><th scope="col">ReferenceNo</th><th scope="col">ItemCode</th><th scope="col">Description</th><th scope="col">WhsCode</th><th scope="col">Quantity</th><th scope="col">Price</th><th scope="col">B1Upload</th><th scope="col">Processed</th></tr></thead></tbody>';
                for (var i = 0; i < msg.length; i++) {
                    results += '<tr><td data-title="transactiondate" class="1">' + k++ + '</td>';
                    results += '<td data-title="svdsno" class="3" >' + msg[i].taxdate + '</td>';
                    results += '<td data-title="svdsno" class="3" >' + msg[i].cardcode + '</td>';
                    results += '<td data-title="regno" class="4" >' + msg[i].cardname + '</td>';
                    results += '<td data-title="cost" class="9">' + msg[i].refno + '</td>';
                    results += '<td data-title="kms" class="10">' + msg[i].itemcode + '</td>';
                    results += '<td data-title="kms" class="11">' + msg[i].desc + '</td>';
                    results += '<td data-title="kms" class="12">' + msg[i].whscode + '</td>';
                    results += '<td data-title="kms" class="13">' + msg[i].qty + '</td>';
                    results += '<td data-title="kms" class="14">' + msg[i].price + '</td>';
                    results += '<td data-title="kms" class="15">' + msg[i].b1upload + '</td>';
                    results += '<td data-title="kms" class="15">' + msg[i].processed + '</td></tr>';
                }
                results += '</table></div>';
                $("#div_sapreport").html(results);
            }
            if (module == "EMROPCHS") {
                var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr><th scope="col">Sno</th><th scope="col">Date</th><th scope="col">CardCode</th><th scope="col">CardName</th><th scope="col">ReferenceNo</th><th scope="col">ItemCode</th><th scope="col">Description</th><th scope="col">WhsCode</th><th scope="col">Quantity</th><th scope="col">Price</th><th scope="col">B1Upload</th><th scope="col">Processed</th></tr></thead></tbody>';
                for (var i = 0; i < msg.length; i++) {
                    results += '<tr><td data-title="transactiondate" class="1">' + k++ + '</td>';
                    results += '<td data-title="svdsno" class="3" >' + msg[i].taxdate + '</td>';
                    results += '<td data-title="svdsno" class="3" >' + msg[i].cardcode + '</td>';
                    results += '<td data-title="regno" class="4" >' + msg[i].cardname + '</td>';
                    results += '<td data-title="cost" class="9">' + msg[i].refno + '</td>';
                    results += '<td data-title="kms" class="10">' + msg[i].itemcode + '</td>';
                    results += '<td data-title="kms" class="11">' + msg[i].desc + '</td>';
                    results += '<td data-title="kms" class="12">' + msg[i].whscode + '</td>';
                    results += '<td data-title="kms" class="13">' + msg[i].qty + '</td>';
                    results += '<td data-title="kms" class="14">' + msg[i].price + '</td>';
                    results += '<td data-title="kms" class="15">' + msg[i].b1upload + '</td>';
                    results += '<td data-title="kms" class="15">' + msg[i].processed + '</td></tr>';
                }
                results += '</table></div>';
                $("#div_sapreport").html(results);
            }
            if (module == "EMROPOR") {
                var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr><th scope="col">Sno</th><th scope="col">Date</th><th scope="col">CardCode</th><th scope="col">CardName</th><th scope="col">ReferenceNo</th><th scope="col">ItemCode</th><th scope="col">Description</th><th scope="col">WhsCode</th><th scope="col">Quantity</th><th scope="col">Price</th><th scope="col">B1Upload</th><th scope="col">Processed</th></tr></thead></tbody>';
                for (var i = 0; i < msg.length; i++) {
                    results += '<tr><td data-title="transactiondate" class="1">' + k++ + '</td>';
                    results += '<td data-title="svdsno" class="3" >' + msg[i].taxdate + '</td>';
                    results += '<td data-title="svdsno" class="3" >' + msg[i].cardcode + '</td>';
                    results += '<td data-title="regno" class="4" >' + msg[i].cardname + '</td>';
                    results += '<td data-title="cost" class="9">' + msg[i].refno + '</td>';
                    results += '<td data-title="kms" class="10">' + msg[i].itemcode + '</td>';
                    results += '<td data-title="kms" class="11">' + msg[i].desc + '</td>';
                    results += '<td data-title="kms" class="12">' + msg[i].whscode + '</td>';
                    results += '<td data-title="kms" class="13">' + msg[i].qty + '</td>';
                    results += '<td data-title="kms" class="14">' + msg[i].price + '</td>';
                    results += '<td data-title="kms" class="15">' + msg[i].b1upload + '</td>';
                    results += '<td data-title="kms" class="15">' + msg[i].processed + '</td></tr>';
                }
                results += '</table></div>';
                $("#div_sapreport").html(results);
            }
            if (module == "EMROVPM") {
                var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr><th scope="col">Sno</th><th scope="col">Date</th><th scope="col">CardCode</th><th scope="col">ReferenceNo</th><th scope="col">Amount</th><th scope="col">B1Upload</th><th scope="col">Processed</th></tr></thead></tbody>';
                for (var i = 0; i < msg.length; i++) {
                    results += '<tr><td data-title="transactiondate" class="1">' + k++ + '</td>';
                    results += '<td data-title="svdsno" class="3" >' + msg[i].taxdate + '</td>';
                    results += '<td data-title="svdsno" class="3" >' + msg[i].cardcode + '</td>';
                    results += '<td data-title="cost" class="9">' + msg[i].refno + '</td>';
                    results += '<td data-title="kms" class="10">' + msg[i].amount + '</td>';
                    results += '<td data-title="kms" class="15">' + msg[i].b1upload + '</td>';
                    results += '<td data-title="kms" class="15">' + msg[i].processed + '</td></tr>';
                }
                results += '</table></div>';
                $("#div_sapreport").html(results);
            }
            if (module == "EMRORCT") {
                var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr><th scope="col">Sno</th><th scope="col">Date</th><th scope="col">ReferenceNo</th><th scope="col">ItemCode</th><th scope="col">Price</th><th scope="col">Remarks</th><th scope="col">B1Upload</th><th scope="col">Processed</th></tr></thead></tbody>';
                for (var i = 0; i < msg.length; i++) {
                    results += '<tr><td data-title="transactiondate" class="1">' + k++ + '</td>';
                    results += '<td data-title="cost" class="9">' + msg[i].taxdate + '</td>';
                    results += '<td data-title="cost" class="9">' + msg[i].refno + '</td>';
                    results += '<td data-title="kms" class="15">' + msg[i].itemcode + '</td>';
                    results += '<td data-title="kms" class="15">' + msg[i].price + '</td>';
                    results += '<td data-title="kms" class="15">' + msg[i].remarks + '</td>';
                    results += '<td data-title="kms" class="15">' + msg[i].b1upload + '</td>';
                    results += '<td data-title="kms" class="15">' + msg[i].processed + '</td></tr>';
                }
                results += '</table></div>';
                $("#div_sapreport").html(results);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i> SAP Report
                </h3>
            </div>
            <div class="box-body" style="font-size:12px">
                <div style="padding: 20px;">
                    <div id="tyre_div">
                        <table style="width: 100%;">
                            <tr>
                            <td style="width: 10%;">
                            <label>
                                       Module<span style="color: red;">*</span></label>
                            </td>
                                <td style="width: 25%;"> 
                                    <select id="slct_mdl" class="form-control" >
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
                                        <option  value="EMROJDTP">Journal Payments</option>
                                        <option  value="EMRORCT">Receipts</option>
                                        <option  value="EMROVPM">Payments</option>
                                        <option value="EMROPOR">Purchase Order</option>
                                         </td>
                                <td style="width: 5px;">
                                </td>
                                 <td>
                                        <input id='btn_sapgenerate' type="button" class="btn btn-primary" name="submit" value='Generate'
                                            onclick="Generate_data()" />
                                    </td>
                                    <td style="float:right;">
                                      <label style="font-size:18px;font-weight:bold">
                                       Total Pending Rows:</label>
                                       <span id="txt_pendingrows" style="color: red;font-size:26px;font-weight:bold"></span>
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

<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="TyreSummaryReport.aspx.cs" Inherits="TyreSummaryReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <link href="autocomplete/jquery-ui.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    $(function () {
        get_tyres_number();
        get_tyres_report();
        $('#div_tyremaster_table').css('display', 'none');
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
    var tyrearr = [];
    function get_tyres_report()
        {
            var data = { 'op': 'get_tyresum_report'};
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filltyredetails(msg);
                        tyrearr = msg;
                    }
                }
                else {
                }
            };
            var e = function (x, h, e)
            {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function filltyredetails(msg)
        {
            var k = 1;
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">Sno</th><th scope="col">SVDSNo</th><th scope="col">TyreNo</th><th scope="col">VehicleNo</th><th scope="col">TyrePosition</th><th scope="col">Size</th><th scope="col">Brand</th><th scope="col">Totalkms</th><th scope="col">Cost</th><th scope="col">FittingType</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr><td data-title="transactiondate" class="1">' + k++ + '</td>';
                results += '<td data-title="tyreno" class="3">' + msg[i].svdsno + '</td>';
                results += '<td data-title="svdsno" class="4" >' + msg[i].tyreno + '</td>';
                results += '<td data-title="regno" class="5" >' + msg[i].regno + '</td>';
                results += '<td data-title="cost" class="11">' + msg[i].tyreposition + '</td>';
                results += '<td data-title="size" class="6" >' + msg[i].size + '</td>';
                results += '<td data-title="brand" class="7" >' + msg[i].brand + '</td>';
                results += '<td data-title="kms" class="8">' + msg[i].kms + '</td>';
                results += '<td data-title="cost" class="9">' + msg[i].cost + '</td>';
                results += '<td data-title="kms" class="10">' + msg[i].fittingtype + '</td>'; 
                results += '<td><input id="btn_poplate" type="button"  data-toggle="modal" data-target="#myModal" onclick="getkmdetails(this)" name="submit" class="btn btn-primary" value="View"/></td></tr>';
               // results += '<td data-title="Plus"><span><img src="images/plus.png" onclick="getkmdetails()" data-target="#myModal" style="cursor:pointer"/></span></td></tr>';
            }
            results += '</table></div>';
            $("#tyresum").html(results);
        }
        function getkmdetails(thisid) {
            var vytyreno = $(thisid).parent().parent().children('.3').html();
            var data = { 'op': 'get_tyre_report', 'vytyreno': vytyreno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filltyrehisdetails(msg);
                        $('#divtyrekms').css('display','block');
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
        function filltyrehisdetails(msg) {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">Transaction Date</th><th scope="col">Vehicle No</th><th scope="col">TyrePosition</th><th scope="col">TyreNo</th><th scope="col">KMS</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr><td data-title="transactiondate" class="2">' + msg[i].transdate + '</td>';
                results += '<td data-title="regno" class="33">' + msg[i].regno + '</td>';
                 results += '<td data-title="position" class="4" >' + msg[i].tyreposition + '</td>';
                 results += '<td data-title="vtype" class="5" >' + msg[i].tyreno + '</td>';
                //results += '<td data-title="totalkms" class="6" >' + msg[i].currentkms + '</td>';
                results += '<td data-title="kms" class="8">' + msg[i].kms + '</td></tr>';
            }
            results += '</table></div>';
            $("#divtyrekms").html(results);
        }

        function get_tyres_number() {
            var data = { 'op': 'get_tyres_number' };
            var s = function (msg) {
                if (msg) {
                    filltyreno(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        var tyreList = [];
        function filltyreno(msg) {
            for (var i = 0; i < msg.length; i++) {
                var tyreno = msg[i].svdsno;
                tyreList.push(tyreno);
            }
            $('#txt_tyre').autocomplete({
                source: tyreList,
                change: tyrenochange,
                autoFocus: true
            });
        }
        function tyrenochange() {
            var msg = tyrearr;  
            var tyreno = document.getElementById("txt_tyre").value;
            var k = 0;
            var colorue = ["#d1ece3", "#fff6e9", "#d7e0e0", "#ffe7e5", "Bisque"];
            if (tyreno == "") {
                var k = 1;
                var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr><th scope="col">Sno</th><th scope="col">SVDSNo</th><th scope="col">TyreNo</th><th scope="col">VehicleNo</th><th scope="col">TyrePosition</th><th scope="col">Size</th><th scope="col">Brand</th><th scope="col">Totalkms</th><th scope="col">Cost</th><th scope="col">FittingType</th></tr></thead></tbody>';
                for (var i = 0; i < msg.length; i++) {
                    results += '<tr><td data-title="transactiondate" class="1">' + k++ + '</td>';
                    results += '<td data-title="tyreno" class="3">' + msg[i].svdsno + '</td>';
                    results += '<td data-title="svdsno" class="4" >' + msg[i].tyreno + '</td>';
                    results += '<td data-title="regno" class="5" >' + msg[i].regno + '</td>';
                    results += '<td data-title="cost" class="11">' + msg[i].tyreposition + '</td>';
                    results += '<td data-title="size" class="6" >' + msg[i].size + '</td>';
                    results += '<td data-title="brand" class="7" >' + msg[i].brand + '</td>';
                    results += '<td data-title="kms" class="8">' + msg[i].kms + '</td>';
                    results += '<td data-title="cost" class="9">' + msg[i].cost + '</td>';
                    results += '<td data-title="kms" class="10">' + msg[i].fittingtype + '</td>';
                    results += '<td><input id="btn_poplate" type="button"  data-toggle="modal" data-target="#myModal" onclick="getkmdetails(this)" name="submit" class="btn btn-primary" value="View"/></td></tr>';
                    // results += '<td data-title="Plus"><span><img src="images/plus.png" onclick="getkmdetails()" data-target="#myModal" style="cursor:pointer"/></span></td></tr>';
                }
                results += '</table></div>';
                $("#tyresum").html(results);
            }
            else {

                var k = 1;
                var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr><th scope="col">Sno</th><th scope="col">SVDSNo</th><th scope="col">TyreNo</th><th scope="col">VehicleNo</th><th scope="col">TyrePosition</th><th scope="col">Size</th><th scope="col">Brand</th><th scope="col">Totalkms</th><th scope="col">Cost</th><th scope="col">FittingType</th></tr></thead></tbody>';
                for (var i = 0; i < msg.length; i++) {
                    var svdsno = msg[i].svdsno;
                    if (tyreno == svdsno.trim()) {
                        results += '<tr><td data-title="transactiondate" class="1">' + k++ + '</td>';
                        results += '<td data-title="tyreno" class="3">' + msg[i].svdsno + '</td>';
                        results += '<td data-title="svdsno" class="4" >' + msg[i].tyreno + '</td>';
                        results += '<td data-title="regno" class="5" >' + msg[i].regno + '</td>';
                        results += '<td data-title="cost" class="11">' + msg[i].tyreposition + '</td>';
                        results += '<td data-title="size" class="6" >' + msg[i].size + '</td>';
                        results += '<td data-title="brand" class="7" >' + msg[i].brand + '</td>';
                        results += '<td data-title="kms" class="8">' + msg[i].kms + '</td>';
                        results += '<td data-title="cost" class="9">' + msg[i].cost + '</td>';
                        results += '<td data-title="kms" class="10">' + msg[i].fittingtype + '</td>';
                        results += '<td><input id="btn_poplate" type="button"  data-toggle="modal" data-target="#myModal" onclick="getkmdetails(this)" name="submit" class="btn btn-primary" value="View"/></td></tr>';
                        // results += '<td data-title="Plus"><span><img src="images/plus.png" onclick="getkmdetails()" data-target="#myModal" style="cursor:pointer"/></span></td></tr>';
                    }
                }
                results += '</table></div>';
                $("#tyresum").html(results);
            }
        }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<section class="content-header">
        <h1>
          Tyre Summary<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Reports</a></li>
            <li><a href="#"> Tyre Summary</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i> Tyre Summary Report
                </h3>
            </div>
            <div class="box-body">
            <div id="vehmaster_showlogs" style="text-align: center;">
                    <table>
                        <tr>
                            <td>
                                <input id="txt_tyre" type="text" style="height: 28px; opacity: 1.0; width: 180px;"
                                    class="form-control" placeholder="Search Tyre" />
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <i class="fa fa-search" aria-hidden="true">Search</i>
                            </td>
                        </tr>
                    </table>
                </div>
                    <div id="tyresum" style="overflow:auto;height:500px"></div>
                     <div id="div_tyremaster_table">
                </div>
                        </div>
                        <div id="myModal" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">
                            &times;</button>
                        <h4 class="modal-title">
                            <b>Tyres History</b></h4>
                    </div>
                    <div class="modal-body">
                       <table align="center">
                       <tr>
                            <td colspan="2">
                                <div id="divtyrekms">
                                </div>
                            </td>
                        </tr>
                        <tr hidden>
                            <td>
                                <label id="lbl_sno"></label>
                            </td>
                        </tr>
                    </table>
                  <%--  <table align="center">
                        <tr>
                            <td colspan="2" align="center" style="height: 40px;">
                                <input id="btn_approve" type="button" class="btn btn-success" name="submit" value='Approve'
                                    onclick="save_approvalbill_click()" />
                                <input id='btn_reject' type="button" class="btn btn-danger" name="Close" value='Reject'
                                    onclick="save_rejectbill_click()" />
                            </td>
                        </tr>
                    </table>--%>
                    </div>
                    <div class="modal-footer" align="center">
                        <button id="btnaclose" type="button" class="btn btn-danger" data-dismiss="modal">
                            Close</button>
                    </div>
                </div>
            </div>
        </div>
                </div>
    </section>
</asp:Content>

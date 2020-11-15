<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="DashBoard_alerts.aspx.cs" Inherits="DashBoard_alerts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="autocomplete/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="js/JTemplate.js" type="text/javascript"></script>
    <script src="js/utility.js" type="text/javascript"></script>
    <link href="bootstrap/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap/RouteWiseTimeLine.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        $(function () {
            GetAssignTripSheets();
        });
        function GetAssignTripSheets() {
            var data = { 'op': 'GetVehicleAlertDeatails' };
            var s = function (msg) {
                if (msg) {
                    if (msg == "No data found") {
                    }
                    else {
                        $('#divFillScreen').removeTemplate();
                        $('#divFillScreen').setTemplateURL('VehicleDetails2.htm');
                        $('#divFillScreen').processTemplate(msg);
                        blinkFont();
                        fillVehicledetails(msg);
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
        var vehicleList = [];
        function fillVehicledetails(msg) {
            for (var i = 0; i < msg.length; i++) {
                var vehicleno = msg[i].vehicleno;
                vehicleList.push(vehicleno);
            }
            $('#txt_VehicleNo').autocomplete({
                source: vehicleList,
                change: vehiclenochange,
                autoFocus: true
            });
        }
        function vehiclenochange() {
        }
        function blinkFont() {
            $('.odometer').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "            " || qtyclass == "") {
                    $(this).parent().css('background', 'green');
                    $(this).parent().css('color', 'white');
                }
            });
            $('.vehiclemake').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "            " || qtyclass == "") {
                    $(this).parent().css('background', 'green');
                    $(this).parent().css('color', 'white');
                }
            });
            $('.vehiclemodel').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "            " || qtyclass == "") {
                    $(this).parent().css('background', 'green');
                    $(this).parent().css('color', 'white');
                }
            });
            $('.vehicletype').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "            " || qtyclass == "") {
                    $(this).parent().css('background', 'green');
                    $(this).parent().css('color', 'white');
                }
            });
            $('.Capacity').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "            " || qtyclass == "") {
                    $(this).parent().css('background', 'green');
                    $(this).parent().css('color', 'white');
                }
            });
            $('.FuelCapacity').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "            " || qtyclass == "") {
                    $(this).parent().css('background', 'green');
                    $(this).parent().css('color', 'white');
                }
            });
            $('.EngineNo').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "            " || qtyclass == "") {
                    $(this).parent().css('background', 'green');
                    $(this).parent().css('color', 'white');
                }
            });
            $('.ChasisNo').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "            " || qtyclass == "") {
                    $(this).parent().css('background', 'green');
                    $(this).parent().css('color', 'white');
                }
            });
            $('.Ins_exp').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "            ") {
                    $(this).parent().css('background', 'green');
                    $(this).parent().css('color', 'white');
                }
            });
            $('.Pol_exp').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "            ") {
                    $(this).parent().css('background', 'green');
                    $(this).parent().css('color', 'white');
                    $(this).parent().css('fontsize ', '25px');
                }
            });
            $('.fit_exp').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "            ") {
                    $(this).parent().css('background', 'green');
                    $(this).parent().css('color', 'white');
                    $(this).parent().css('fontsize ', '25px');
                }
            });
            $('.roadtax_exp').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "            ") {
                    $(this).parent().css('background', 'green');
                    $(this).parent().css('color', 'white');
                    $(this).parent().css('fontsize ', '25px');
                }
            });
            $('.permit_exp').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "            ") {
                    $(this).parent().css('background', 'green');
                    $(this).parent().css('color', 'white');
                    $(this).parent().css('fontsize ', '25px');
                }
            });
            setTimeout("setblinkFont()", 100)
        }
        function setblinkFont() {
            $('.odometer').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "            " || qtyclass == "") {
                    $(this).parent().css('background', 'blue');
                    $(this).parent().css('color', 'white ');
                }
            });
            $('.EngineNo').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "            " || qtyclass == "") {
                    $(this).parent().css('background', 'blue');
                    $(this).parent().css('color', 'white ');
                }
            });
            $('.ChasisNo').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "            " || qtyclass == "") {
                    $(this).parent().css('background', 'blue');
                    $(this).parent().css('color', 'white ');
                }
            });
            $('.vehiclemake').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "            " || qtyclass == "") {
                    $(this).parent().css('background', 'blue');
                    $(this).parent().css('color', 'white ');
                }
            });
            $('.vehiclemodel').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "            " || qtyclass == "") {
                    $(this).parent().css('background', 'blue');
                    $(this).parent().css('color', 'white ');
                }
            });
            $('.vehicletype').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "            " || qtyclass == "") {
                    $(this).parent().css('background', 'blue');
                    $(this).parent().css('color', 'white ');
                }
            });
            $('.Capacity').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "            " || qtyclass == "") {
                    $(this).parent().css('background', 'blue');
                    $(this).parent().css('color', 'white ');
                }
            });
            $('.FuelCapacity').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "            " || qtyclass == "") {
                    $(this).parent().css('background', 'blue');
                    $(this).parent().css('color', 'white ');
                }
            });
            $('.Ins_exp').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "            ") {
                    $(this).parent().css('background', 'blue');
                    $(this).parent().css('color', 'white ');
                }
            });
            $('.Pol_exp').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "            ") {
                    $(this).parent().css('background', 'blue');
                    $(this).parent().css('color', 'white ');
                }
            });
            $('.fit_exp').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "            ") {
                    $(this).parent().css('background', 'blue');
                    $(this).parent().css('color', 'white ');
                }
            });
            $('.roadtax_exp').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "            ") {
                    $(this).parent().css('background', 'blue');
                    $(this).parent().css('color', 'white ');
                }
            });
            $('.permit_exp').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "            ") {
                    $(this).parent().css('background', 'blue');
                    $(this).parent().css('color', 'white ');
                }
            });
            setTimeout("blinkFont()", 100)
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Vehicle Details
                </h3>
            </div>
            <div class="box-body">
                <table align="center">
                    <tr>
                        <td>
                            <input id="txt_VehicleNo" type="text" style="height: 28px; opacity: 1.0; width: 180px;"
                                class="form-control" placeholder="Search Vehicle Number" />
                        </td>
                        <td style="width: 5px;">
                        </td>
                        <td>
                            <i class="fa fa-search" aria-hidden="true">Search</i>
                        </td>
                    </tr>
                </table>
                <div id="divFillScreen" style="height: 8000px;">
                </div>
            </div>
        </div>
    </section>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="RouteWisechart.aspx.cs" Inherits="RouteWisechart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="Kendo/jquery.min.js" type="text/javascript"></script>
    <script src="Kendo/kendo.all.min.js" type="text/javascript"></script>
    <link href="Kendo/kendo.common.min.css" rel="stylesheet" type="text/css" />
    <link href="Kendo/kendo.default.min.css" rel="stylesheet" type="text/css" />
    <script src="js/utility.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ddlTypeChange() {
            var ChartType = document.getElementById('ddlChartType').value;
            if (ChartType == "Vehicle Wise") {
                getallveh_nos();
            }
            if (ChartType == "Driver Wise") {
                get_driverand_helper();
            }
            if (ChartType == "Vehicle Make Wise") {
                get_vehiclemake();
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
        function get_driverand_helper() {
            var data = { 'op': 'get_driver_and_helper' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filldrive_helper(msg);
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
        function filldrive_helper(msg) {
            var data = document.getElementById('slct_vehicle_no');
            var length = data.options.length;
            document.getElementById('slct_vehicle_no').options.length = null;
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
        function get_vehiclemake() {
            var data = { 'op': 'get_vehiclemake' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        bindvehiclemake(msg);
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
        function bindvehiclemake(msg) {
            var data = document.getElementById('slct_vehicle_no');
            var length = data.options.length;
            document.getElementById('slct_vehicle_no').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Make";
            opt.value = "Select Make";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].make != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].make;
                    option.value = msg[i].sno;
                    data.appendChild(option);
                }
            }
        }
        function GetVehicleWisePerformanceclick() {
            var vehicleno = "";
            var ChartType = document.getElementById('ddlChartType').value;
            if (ChartType == "Vehicle Wise") {
                vehicleno = document.getElementById('slct_vehicle_no').value;
                if (vehicleno == "" || vehicleno == "Select Vehicle No") {
                    alert("Please Select Vehicle No");
                    return false;
                }
            }
            if (ChartType == "Driver Wise") {
                vehicleno = document.getElementById('slct_vehicle_no').value;
                if (vehicleno == "" || vehicleno == "Select Driver") {
                    alert("Please Select Driver Name");
                    return false;
                }
            }
            if (ChartType == "Vehicle Make Wise") {
                vehicleno = document.getElementById('slct_vehicle_no').value;
                if (vehicleno == "" || vehicleno == "Select Make") {
                    alert("Please Select Make");
                    return false;
                }
            }
            var FromDate = document.getElementById('txtFromDate').value;
            if (FromDate == "") {
                alert("Please Select From Date");
                return false;
            }
            var Todate = document.getElementById('txtTodate').value;
            if (Todate == "") {
                alert("Please Select To date");
                return false;
            }
            $('#divHide').css('display', 'block');
            var vehicleperformancechart = "Routewisechart";
            var data = { 'op': 'GetVehicleWisePerformanceclick', 'vehicleno': vehicleno, 'FromDate': FromDate, 'Todate': Todate, 'ChartType': ChartType, 'FormName': vehicleperformancechart };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Time Out Expired") {
                        alert(msg);
                        return false;
                    }
                    else {
                        LineChartforVehicleMileage(msg);
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
        function LineChartforVehicleMileage(databind) {
            var datainXSeries = 0;
            var datainYSeries = 0;
            var newXarray = [];
            var newYarray = [];
            var sel = document.getElementById("slct_vehicle_no").value;
            var VehicleNo = "";
            var ChartType = document.getElementById('ddlChartType').value;
            if (ChartType == "Vehicle Wise") {
                for (l = 0; l < veh_data.length; l++) {
                    if (veh_data[l].vm_sno == sel) {
                        VehicleNo = veh_data[l].registration_no + "-" + veh_data[l].VehMake + "-" + veh_data[l].VehType + "-" + veh_data[l].Capacity;
                    }
                }
            }
            if (ChartType == "Driver Wise") {
                var driver = document.getElementById("slct_vehicle_no");
                VehicleNo = driver.options[driver.selectedIndex].text;
            }
            if (ChartType == "Vehicle Make Wise") {
                var driver = document.getElementById("slct_vehicle_no");
                VehicleNo = driver.options[driver.selectedIndex].text;
            }
            for (var k = 0; k < databind.length; k++) {
                var BranchName = [];
                var IndentDate = databind[k].TripDate;
                var UnitQty = databind[k].AvgLtr;
                var DeliveryQty = databind[k].Mileageltr;
                var Status = databind[k].Status;
                newXarray = IndentDate.split(',');
                for (var i = 0; i < DeliveryQty.length; i++) {
                    newYarray.push({ 'data': DeliveryQty[i].split(','), 'name': Status[i] });
                }
            }
            $("#divChart").kendoChart({
                title: {
                    position: "left",
                    text: document.getElementById('ddlChartType').value + "  Mileage " + VehicleNo,
                    color: "#006600",
                    font: "bold italic 20px Arial,Helvetica,sans-serif"
                },
                legend: {
                    visible: false
                },

                seriesDefaults: {
                    type: "line",
                    style: "smooth",
                    width: 90
                },
                series: newYarray,
                valueAxis: {
                    line: {
                        visible: false
                    },
                    minorGridLines: {
                        visible: true
                    }
                },
                categoryAxis: {
                    categories: newXarray,
                    majorGridLines: {
                        visible: false
                    },
                    labels: {
                        rotation: 65
                    }
                },
                seriesColors: ["#FF00FF", "#0041C2", "#800517"],
                tooltip: {
                    visible: true,
                    template: "#= series.name #: #= value #"
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <section class="content-header">
        <h1>
            RouteWise Chart<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Trip Reports</a></li>
            <li><a href="#">RouteWise Chart</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>RouteWise Chart Details
                </h3>
            </div>
            <div class="box-body">
    <div style="width: 100%;" align="center">
        <table>
            <tr>
                <td>
                    <span>Chart Type</span>
                </td>
                <td >
                    <select id="ddlChartType" class="form-control" onchange="ddlTypeChange(this);">
                        <option>Select Type</option>
                        <option>Vehicle Wise</option>
                        <option>Driver Wise</option>
                        <option>Vehicle Make Wise</option>
                    </select>
                </td>
                <td style="width: 6px;">
                </td>
                <td>
                    <span>Vehicle No</span>
                </td>
                <td>
                    <select id="slct_vehicle_no" class="form-control">
                    </select>
                </td>
                <td style="width: 6px;">
                </td>
                <td>
                    <span>From Date</span>
                </td>
                <td >
                    <input type="date" class="form-control" id="txtFromDate" />
                </td>
                <td style="width: 6px;">
                </td>
                <td>
                    <span>To Date</span>
                </td>
                <td >
                    <input type="date" class="form-control" id="txtTodate" />
                </td>
                <td style="width: 6px;">
                </td>
                <td>
                    <input type="button" id="submit" value="Generate" class="btn btn-primary" onclick="GetVehicleWisePerformanceclick()" />
                </td>
            </tr>
        </table>
    </div>
    <div id="divChart">
    </div>
    </div>
    </div>
    </section>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="Rpt_VehicleSummary.aspx.cs" Inherits="Rpt_VehicleSummary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/utility.js" type="text/javascript"></script>
    <link href="autocomplete/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {

            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (day < 10) day = "0" + day;
            today = year + "-" + month + "-" + day;

            $('#txt_fromdate').val(today);
            $('#txt_todate').val(today);
            $('#txt_fromdate1').val(today);
            $('#txt_todate1').val(today);

            $("#div_Daily").css("display", "block");
            loadvehicletype();
        });

        function show_Daily() {
            $("#div_Daily").css("display", "block");
            $("#div_Consolidated").css("display", "none");
            $("#div_Monthlydistance").css("display", "none");
            $("#div_Dieselfuel").css("display", "none");
        }
        function show_Consolidated() {
            $("#div_Daily").css("display", "none");
            $("#div_Consolidated").css("display", "block");
            $("#div_Monthlydistance").css("display", "none");
            $("#div_Dieselfuel").css("display", "none");
        }
        function show_Monthly() {
            $("#div_Daily").css("display", "none");
            $("#div_Consolidated").css("display", "none");
            $("#div_Monthlydistance").css("display", "block");
            $("#div_Dieselfuel").css("display", "none");
        }

        function show_DieselFuel() {
            $("#div_Daily").css("display", "none");
            $("#div_Consolidated").css("display", "none");
            $("#div_Monthlydistance").css("display", "none");
            $("#div_Dieselfuel").css("display", "block");
        }


        // Daily Data
        function getDailyinfo() {
            var fromdate = document.getElementById('txt_fromdate').value;
            var todate = document.getElementById('txt_todate').value;
            var vehicletype = document.getElementById('selectvehicletype').value;

            var data = { 'op': 'getDailyinfo', 'fromdate': fromdate, 'todate': todate, 'Type': vehicletype };
            var s = function (msg) {
                if (msg) {
                    fillDailyinfo(msg);
                }
                else {
                    alert("please check the Data...");
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        var AllDates = [];
        var Allerioddata = [];
        var AllVehicleno = [];
        var status;
        var TotalKm;
        var TotalDiesel;

        function fillDailyinfo(data) {
            var ks = 0;
            var colorue = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            $("#Daily_griddata").html('');
            AllDates = [];
            Allerioddata = [];
            AllVehicleno = [];

            AllDates = data[0].ListBetweendate;
            Allerioddata = data[0].Listperiodfuel;
            var k = 0;
            var results = '<div class="divcontainer" style="overflow: auto;"><table id="tblbiologs" style="font-size: smaller;" class="responsive-table">';
            results += '<thead><tr>';
            results += '<th scope="col" style="text-align:center;border: 1px solid #d0cdcd;"><i class="fa fa-car" aria-hidden="true"></i> VehicleNo/Date</th>';
            for (var i = 0; i < AllDates.length; i++) {
                results += '<th scope="col" style="border: 1px solid #d0cdcd;" id="txtDate"> ' + AllDates[i].Betweendate + '</th>';
            }
            results += '<th scope="col" style="border: 1px solid #d0cdcd;" id="txtDate">TotalKm</th>';
            results += '</tr></thead></tbody>';
            for (var i = 0; i < Allerioddata.length; i++) {
                results += '<tr style="background-color:' + colorue[ks] + '">';
                var vehicleno = Allerioddata[i].periodvhileno
                if (AllVehicleno.indexOf(vehicleno) == -1) {
                    TotalKm = 0;
                    results += '<td data-title="brandstatus" class="1" style="border: 1px solid #d0cdcd;width: 5px;">' + Allerioddata[i].periodvhileno + '</td>';
                    results += '<td style="display:none;"  data-title="brandstatus" class="1">' + Allerioddata[i].periodvhileno + '</td>';
                    AllVehicleno.push(vehicleno);
                    for (var j = 0; j < AllDates.length; j++) {
                        status = 0;
                        for (var k = 0; k < Allerioddata.length; k++) {
                            if (AllDates[j].Betweendate == Allerioddata[k].periodBetweendate && vehicleno == Allerioddata[k].periodvhileno) {
                                if (Allerioddata[k].periodvhileno != "") {
                                    var st = Allerioddata[k].periodkm;
                                    results += '<td id="' + st + '" data-title="brandstatus" class="1" style="border: 1px solid #d0cdcd;width: 5px;">' + Allerioddata[k].periodkm + '</td>';
                                    status = 1;
                                    TotalKm = parseFloat(TotalKm) + parseFloat(st);
                                }
                            }
                        }
                        if (status == 0) {
                            results += '<td id="' + status + '" data-title="brandstatus" class="1" style="border: 1px solid #d0cdcd;width: 5px;">' + status + '</td>';
                        }
                    }
                    results += '<td id="' + status + '" data-title="brandstatus" class="1" style="border: 1px solid #d0cdcd;width: 5px;">' + TotalKm + '</td>';
                    results += '</tr>';
                    ks = ks + 1;
                    if (ks == 4) {
                        ks = 0;
                    }
                }
            }

            results += '</table></div>';
            $("#Daily_griddata").html(results);
        }
        // Daily Data
        // Diesel Fuel Data
        function getDieselFuelinfo() {
            var fromdate = document.getElementById('txt_fromdate1').value;
            var todate = document.getElementById('txt_todate1').value;
            var vehicletype=document.getElementById('selectvehicletype').value;
            var data = { 'op': 'getDailyinfo', 'fromdate': fromdate, 'todate': todate, 'Type': vehicletype };
            var s = function (msg) {
                if (msg) {
                    fillDieselFuelinfo(msg);
                }
                else {
                    alert("please check the Data...");
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function fillDieselFuelinfo(data) {
            var ks = 0;
            var colorue = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            $("#Daily_griddata").html('');
            AllDates = [];
            Allerioddata = [];
            AllVehicleno = [];

            AllDates = data[0].ListBetweendate;
            Allerioddata = data[0].Listperiodfuel;
            var k = 0;
            var results = '<div class="divcontainer" style="overflow: auto;"><table id="tblbiologs" style="font-size: smaller;" class="responsive-table">';
            results += '<thead><tr>';
            results += '<th scope="col" style="text-align:center;border: 1px solid #d0cdcd;"><i class="fa fa-car" aria-hidden="true"></i> VehicleNo</th>';
            results += '<th scope="col" style="text-align:center; border: 1px solid #d0cdcd;" id="txtDate"> Day</th>';
            for (var i = 0; i < AllDates.length; i++) {
                results += '<th scope="col" style="border: 1px solid #d0cdcd;" id="txtDate"> ' + AllDates[i].Betweendate + '</th>';
            }
            results += '<th scope="col" style="border: 1px solid #d0cdcd;" id="txtDate">TotalKm</th>';
            results += '</tr></thead></tbody>';
            for (var i = 0; i < Allerioddata.length; i++) {
                results += '<tr style="background-color:' + colorue[ks] + '">';
                var vehicleno = Allerioddata[i].periodvhileno
                if (AllVehicleno.indexOf(vehicleno) == -1) {
                    TotalKm = 0;
                    TotalDiesel = 0;
                    results += '<td rowspan="2" data-title="brandstatus" class="1" style="border: 1px solid #d0cdcd;width: 5px;">' + Allerioddata[i].periodvhileno + '</td>';
                    results += '<td  id="txtDate" data-title="brandstatus" class="1" style="border: 1px solid #d0cdcd;width: 5px;">Dist</td>';
                    results += '<td style="display:none;"  data-title="brandstatus" class="1">' + Allerioddata[i].periodvhileno + '</td>';
                    AllVehicleno.push(vehicleno);
                    for (var j = 0; j < AllDates.length; j++) {
                        status = 0;
                        for (var k = 0; k < Allerioddata.length; k++) {
                            if (AllDates[j].Betweendate == Allerioddata[k].periodBetweendate && vehicleno == Allerioddata[k].periodvhileno) {
                                if (Allerioddata[k].periodvhileno != "") {
                                    var st = Allerioddata[k].periodkm;
                                    results += '<td id="' + st + '" data-title="brandstatus" class="1" style="border: 1px solid #d0cdcd;width: 5px;">' + Allerioddata[k].periodkm + '</td>';
                                    status = 1;
                                    TotalKm = parseFloat(TotalKm) + parseFloat(st);
                                }
                            }
                        }
                        if (status == 0) {
                            results += '<td id="' + status + '" data-title="brandstatus" class="1" style="border: 1px solid #d0cdcd;width: 5px;">' + status + '</td>';
                        }
                    }

                    results += '<td id="' + status + '" data-title="brandstatus" class="1" style="border: 1px solid #d0cdcd;width: 5px;color: red;">' + TotalKm + '</td>';
                    results += '</tr>';
                    //
                    results += '<tr style="background-color:' + colorue[ks] + '">';
                    results += '<td  id="txtDate" data-title="brandstatus" class="1" style="border: 1px solid #d0cdcd;width: 5px;">Fuel</td>';
                    for (var j = 0; j < AllDates.length; j++) {
                        status = 0;
                        for (var k = 0; k < Allerioddata.length; k++) {
                            if (AllDates[j].Betweendate == Allerioddata[k].periodBetweendate && vehicleno == Allerioddata[k].periodvhileno) {
                                if (Allerioddata[k].periodvhileno != "") {
                                    var st = Allerioddata[k].periodfuel;
                                    results += '<td id="' + st + '" data-title="brandstatus" class="1" style="border: 1px solid #d0cdcd;width: 5px;">' + Allerioddata[k].periodfuel + '</td>';
                                    status = 1;
                                    TotalDiesel = parseFloat(TotalDiesel) + parseFloat(st);
                                }
                            }
                        }
                        if (status == 0) {
                            results += '<td id="' + status + '" data-title="brandstatus" class="1" style="border: 1px solid #d0cdcd;width: 5px;">' + status + '</td>';
                        }
                    }

                    results += '<td id="' + status + '" data-title="brandstatus" class="1" style="border: 1px solid #d0cdcd;width: 5px;color: red;">' + TotalDiesel.toFixed(2) + '</td>';
                    results += '</tr>';
                    //



                    ks = ks + 1;
                    if (ks == 4) {
                        ks = 0;
                    }
                }
            }

            results += '</table></div>';
            $("#DieselFuel_griddata").html(results);
        }
        // Diesel Fuel Data

        function loadvehicletype() {
            var select = document.getElementById("selectvehicletype");
            var options = ["ALL", "All Puffs", "All Tankers"];
            for (var i = 0; i < options.length; i++) {
                var opt = options[i];
                var el = document.createElement("option");
                el.textContent = opt;
                el.value = opt;
                select.appendChild(el);
            }
        }

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



    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 100%; height: 100%;">
        <div id="second_div" style="padding: 20px;">
            <div role="tabpanel">
                <!-- Nav tabs -->
                <ul class="nav nav-tabs" role="tablist">
                    <li role="presentation" class="active"><a href="#" aria-controls="Daily" role="tab"
                        data-toggle="tab" onclick="show_Daily()">Daily</a></li>
                    <li role="presentation"><a href="#" aria-controls="Consolidated" role="tab" data-toggle="tab"
                        onclick="show_Consolidated()">Consolidated</a></li>
                    <li role="presentation"><a href="#" aria-controls="Monthlydistance" role="tab" data-toggle="tab"
                        onclick="show_Monthly()">Monthly Distance</a></li>
                    <li role="presentation"><a href="#" aria-controls="Dieselfuel" role="tab" data-toggle="tab"
                        onclick="show_DieselFuel()">Diesel Fuel</a></li>
                </ul>
                <!-- Tab panes -->
                <div class="tab-content">
                    <div role="tabpanel" class="tab-pane active" id="div_Daily">
                        <section class="content">
                            <div class="box box-info">
                                <div class="box-header with-border">
                                    <h3 class="box-title">
                                        <i style="padding-right: 5px;" class="fa fa-cog"></i>Daily Report
                                    </h3>
                                </div>
                                <div class="box-body">
                                  <table id="mytable" align="center">
                    <tr>
                        <td style="width: 5px;">
                        </td>                       
                        <td style="height: 40px;">
                          <label class="control-label">
                            From Date <span style="color: red;">*</span>
                            </label>
                        </td>
                        <td>
                            <input type="date" class="form-control" id="txt_fromdate" class="form-control" />
                        </td>
                        <td style="width: 5px;">
                        </td>
                        <td style="height: 40px;">
                          <label class="control-label">
                            TO Date <span style="color: red;">*</span>
                            </label>
                        </td>
                        <td>
                            <input type="date" class="form-control" id="txt_todate" class="form-control" />
                        </td>
                        <td style="width: 5px;">
                        </td>
                        <td>
                            <input id="btn_DailyData" type="button" class="btn btn-primary" name="submit" value="Get DailyData"
                                onclick="getDailyinfo();" style="width: 100px;">
                        </td>
                    </tr>
                </table>
                                       <div id="Daily_griddata"></div>
                                </div>
                            </div>
                        </section>
                    </div>
                    <div role="tabpanel" class="tab-pane active" id="div_Consolidated" style="display: none;">
                        <section class="content">
                            <div class="box box-info">
                                <div class="box-header with-border">
                                    <h3 class="box-title">
                                        <i style="padding-right: 5px;" class="fa fa-cog"></i>Consolidated Report
                                    </h3>
                                </div>
                                <div class="box-body">
                                 </div>
                            </div>
                        </section>
                        <div>
                        </div>
                    </div>
                    <div role="tabpanel" class="tab-pane active" id="div_Monthlydistance" style="display: none;">
                        <section class="content">
                            <div class="box box-info">
                                <div class="box-header with-border">
                                    <h3 class="box-title">
                                        <i style="padding-right: 5px;" class="fa fa-cog"></i>Monthly Distance Report
                                    </h3>
                                </div>
                                <div class="box-body">
                                 </div>
                            </div>
                        </section>
                        <div>
                        </div>
                    </div>
                    <div role="tabpanel" class="tab-pane active" id="div_Dieselfuel" style="display: none;">
                        <section class="content">
                            <div class="box box-info">
                                <div class="box-header with-border">
                                    <h3 class="box-title">
                                        <i style="padding-right: 5px;" class="fa fa-cog"></i>Diesel Fuel Report
                                    </h3>
                                </div>
                                <div class="box-body">
                                <table id="Table1" align="center">
                    <tr>
                     <td style="width: 5px;">
                        </td>
                        <td style="height: 40px;">
                          <label class="control-label">
                            Select Type <span style="color: red;">*</span>
                            </label>
                        </td>
                        <td>
                        <select id="selectvehicletype">
                        <option>Select vehicle Type</option>
                        </select>
                        </td>
                        <td style="width: 5px;">
                        </td>                       
                        <td style="height: 40px;">
                          <label class="control-label">
                            From Date <span style="color: red;">*</span>
                            </label>
                        </td>
                        <td>
                            <input type="date" class="form-control" id="txt_fromdate1" class="form-control" />
                        </td>
                        <td style="width: 5px;">
                        </td>
                        <td style="height: 40px;">
                          <label class="control-label">
                            TO Date <span style="color: red;">*</span>
                            </label>
                        </td>
                        <td>
                            <input type="date" class="form-control" id="txt_todate1" class="form-control" />
                        </td>
                        <td style="width: 5px;">
                        </td>
                        <td>
                            <input id="Button1" type="button" class="btn btn-primary" name="submit" value="Get DieselData"
                                onclick="getDieselFuelinfo();" style="width: 100px;">
                        </td>
                    </tr>
                </table>
                                       <div id="DieselFuel_griddata"></div>
                                 </div>
                            </div>
                        </section>
                        <div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

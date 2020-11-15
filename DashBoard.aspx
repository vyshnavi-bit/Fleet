<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="DashBoard.aspx.cs" Inherits="DashBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/JTemplate.js" type="text/javascript"></script>
    <script src="js/utility.js" type="text/javascript"></script>
    <%-- <link href="bootstrap/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap/RouteWiseTimeLine.css" rel="stylesheet" type="text/css" />--%>
    <script type="text/javascript">
        $(function () {
            GetVehicleExpDeatails();
            GetVehicleMileageDeatails();
            GetAvailableVehicleDeatails();
            GetRunningVehicleDeatails();
            GetIdleVehicleDeatails();
        });
        var k = 0;
        var colorue = ["#b3ffe6", "AntiqueWhite", "#daffff", "MistyRose", "Bisque"];
        function GetIdleVehicleDeatails() {
            var table = document.getElementById("tbl_idle_veh_list");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var data = { 'op': 'GetIdle_Vehicle_Deatails' };
            var s = function (msg) {
                if (msg) {
                    var j = 1;
                    for (var i = 0; i < msg.length; i++) {
                        var tablerowcnt = document.getElementById("tbl_idle_veh_list").rows.length;
                        $('#tbl_idle_veh_list').append('<tr style="background-color:' + colorue[k] + '"><td data-title="categorysno">' + j + '</td><th scope="Category Name">' + msg[i].vehicleno + '</th><th scope="Category Name" ><span class="badge bg-yellow">' + msg[i].type + '</span></th><td data-title="IsTransport" ><span class="badge bg-red">' + msg[i].make + '</span></td></tr>');
                        k = k + 1;
                        if (k == 4) {
                            k = 0;
                        }
                        j++;
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
        function GetRunningVehicleDeatails() {
            var table = document.getElementById("tbl_running_veh_list");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var data = { 'op': 'Getrunning_Vehicle_Deatails' };
            var s = function (msg) {
                if (msg) {
                    var j = 1;
                    for (var i = 0; i < msg.length; i++) {
                        var tablerowcnt = document.getElementById("tbl_running_veh_list").rows.length;
                        $('#tbl_running_veh_list').append('<tr style="background-color:' + colorue[k] + '"><td data-title="categorysno">' + j + '</td><th scope="Category Name">' + msg[i].vehicleno + '</th><th scope="Category Name" ><span class="badge bg-green">' + msg[i].type + '</span></th><td data-title="IsTransport" ><span class="badge bg-light-blue">' + msg[i].route + '</span></td></tr>');
                        j++;
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
        function GetAvailableVehicleDeatails() {
            var table = document.getElementById("tbl_available_veh_list");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var data = { 'op': 'Getavailable_Vehicle_Deatails' };
            var s = function (msg) {
                if (msg) {
                    var j = 1;
                    for (var i = 0; i < msg.length; i++) {
                        var tablerowcnt = document.getElementById("tbl_available_veh_list").rows.length;
                        $('#tbl_available_veh_list').append('<tr style="background-color:' + colorue[k] + '"><td data-title="categorysno">' + j + '</td><th scope="Category Name">' + msg[i].vehicleno + '</th><th scope="Category Name" ><span class="badge bg-green">' + msg[i].type + '</span></th><td data-title="IsTransport" ><span class="badge bg-light-blue">' + msg[i].make + '</span></td></tr>');
                        j++;
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
        function GetVehicleExpDeatails() {
            var table = document.getElementById("tbl_veh_exp_list");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var data = { 'op': 'GetVehicleExpDeatails' };
            var s = function (msg) {
                if (msg) {
                    var j = 1;
                    for (var i = 0; i < msg.length; i++) {
                        var tablerowcnt = document.getElementById("tbl_veh_exp_list").rows.length;
                        var InsExpcolor = msg[i].InsExpcolor;
                        var name = "";
                        if (InsExpcolor == "Red") {
                            name = "Ins Exp date";
                            $('#tbl_veh_exp_list').append('<tr style="background-color:' + colorue[k] + '"><td data-title="categorysno">' + j + '</td><th scope="Category Name">' + msg[i].vehicleno + '</th><th scope="Category Name">' + name + '</th><td data-title="IsTransport" ><span class="badge bg-red">' + msg[i].InsExpDate + '</span></td></tr>');
                        }
                        if (InsExpcolor == "orange") {
                            name = "Ins Exp date";
                            $('#tbl_veh_exp_list').append('<tr style="background-color:' + colorue[k] + '"><td data-title="categorysno">' + j + '</td><th scope="Category Name">' + msg[i].vehicleno + '</th><th scope="Category Name">' + name + '</th><td data-title="IsTransport" ><span class="badge bg-yellow">' + msg[i].InsExpDate + '</span></td></tr>');
                        }
                        var FitExpcolor = msg[i].FitExpcolor;
                        if (FitExpcolor == "Red") {
                            name = "Fitness Exp date";
                            $('#tbl_veh_exp_list').append('<tr style="background-color:' + colorue[k] + '"><td data-title="categorysno">' + j + '</td><th scope="Category Name">' + msg[i].vehicleno + '</th><th scope="Category Name">' + name + '</th><td data-title="IsTransport" ><span class="badge bg-red">' + msg[i].FitExpDate + '</span></td></tr>');
                        }
                        if (FitExpcolor == "orange") {
                            name = "Fitness Exp date";
                            $('#tbl_veh_exp_list').append('<tr style="background-color:' + colorue[k] + '"><td data-title="categorysno">' + j + '</td><th scope="Category Name">' + msg[i].vehicleno + '</th><th scope="Category Name">' + name + '</th><td data-title="IsTransport" ><span class="badge bg-yellow">' + msg[i].FitExpDate + '</span></td></tr>');
                        }
                        var RCExpcolor = msg[i].RCExpcolor;
                        if (RCExpcolor == "Red") {
                            name = "Permitt Exp date";
                            $('#tbl_veh_exp_list').append('<tr style="background-color:' + colorue[k] + '"><td data-title="categorysno">' + j + '</td><th scope="Category Name">' + msg[i].vehicleno + '</th><th scope="Category Name">' + name + '</th><td data-title="IsTransport" ><span class="badge bg-red">' + msg[i].RCExpDate + '</span></td></tr>');
                        }
                        if (RCExpcolor == "orange") {
                            name = "Permitt Exp date";
                            $('#tbl_veh_exp_list').append('<tr style="background-color:' + colorue[k] + '"><td data-title="categorysno">' + j + '</td><th scope="Category Name">' + msg[i].vehicleno + '</th><th scope="Category Name">' + name + '</th><td data-title="IsTransport" ><span class="badge bg-yellow">' + msg[i].RCExpDate + '</span></td></tr>');
                        }

                        var RoadTaxExpColor = msg[i].RoadTaxExpcolor;
                        if (RoadTaxExpColor == "Red") {
                            name = "RoadTax Exp date";
                            $('#tbl_veh_exp_list').append('<tr style="background-color:' + colorue[k] + '"><td data-title="categorysno">' + j + '</td><th scope="Category Name">' + msg[i].vehicleno + '</th><th scope="Category Name">' + name + '</th><td data-title="IsTransport" ><span class="badge bg-red">' + msg[i].RoadTaxExpDate + '</span></td></tr>');
                        }
                        if (RoadTaxExpColor == "orange") {
                            name = "RoadTax Exp date";
                            $('#tbl_veh_exp_list').append('<tr style="background-color:' + colorue[k] + '"><td data-title="categorysno">' + j + '</td><th scope="Category Name">' + msg[i].vehicleno + '</th><th scope="Category Name">' + name + '</th><td data-title="IsTransport" ><span class="badge bg-yellow">' + msg[i].RoadTaxExpDate + '</span></td></tr>');
                        }
                        j++;
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
        function GetVehicleMileageDeatails() {
            var table = document.getElementById("tbl_veh_mileage_list");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var data = { 'op': 'GetVehicle_MileageDeatails' };
            var s = function (msg) {
                if (msg) {
                    var j = 1;
                    for (var i = 0; i < msg.length; i++) {
                        var tablerowcnt = document.getElementById("tbl_veh_mileage_list").rows.length;
                        $('#tbl_veh_mileage_list').append('<tr style="background-color:' + colorue[k] + '"><td data-title="categorysno">' + j + '</td><th scope="Category Name">' + msg[i].vehicleno + '</th><th scope="Category Name">' + msg[i].routename + '</th><td data-title="IsTransport" ><span class="badge bg-red">' + msg[i].mileage + '</span></td></tr>');
                        j++;
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            DashBoard <small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <%--   <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Charts</a></li>
            <li class="active">ChartJS</li>--%>
        </ol>
    </section>
    <!-- Main content -->
    <section class="content">
        <div class="row">
            <div class="col-md-6">
                <!-- AREA CHART -->
                <div class="box box-primary">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                          <i style="padding-right: 5px;" class="fa fa-outdent"></i>    Vehicle Expiry Details</h3>
                       <div class="box-tools pull-right">
                        <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
 
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="box-body no-padding" style="height:300px;overflow-y: scroll;">
                            <table class="table" id="tbl_veh_exp_list">
                                <tr>
                                    <th style="width: 10px">
                                        #
                                    </th>
                                    <th>
                                        Vehciel No
                                    </th>
                                    <th>
                                        Expiry Type
                                    </th>
                                    <th style="width: 40px">
                                        Date
                                    </th>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="box box-danger">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                           <i style="padding-right: 5px;" class="fa fa-list-ol"></i>   Idle vehicles
                        </h3>
                        <div class="box-tools pull-right">
                        <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
 
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="box-body no-padding" style="height:300px;overflow-y: scroll;">
                            <table class="table" id="tbl_idle_veh_list">
                                <tr>
                                    <th style="width: 10px">
                                        #
                                    </th>
                                    <th>
                                        Vehicle No
                                    </th>
                                    <th>
                                        Type
                                    </th>
                                    <th style="width: 40px">
                                        Make
                                    </th>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <!-- LINE CHART -->
                <div class="box box-info">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                             <i style="padding-right: 5px;" class="fa fa-list-alt"></i> Mileage Details</h3>
                        <div class="box-tools pull-right">
                        <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
 
                        </div>
                    </div>
                    <div class="box-body">
                        <!-- /.box-header -->
                        <div class="box-body no-padding" style="height:300px;overflow-y: scroll;">
                            <table class="table" id="tbl_veh_mileage_list">
                                <tr>
                                    <th style="width: 10px">
                                        #
                                    </th>
                                    <th>
                                        Vehicle No
                                    </th>
                                    <th>
                                        Route Name
                                    </th>
                                    <th style="width: 40px">
                                        Mileage
                                    </th>
                                </tr>
                            </table>
                        </div>
                        <!-- /.box-body -->
                        <!-- /.box -->
                    </div>
                </div>
                <div class="box box-success">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                           <i style="padding-right: 5px;" class="fa fa-dropbox"></i>  Running Vehicles</h3>
                           <div class="box-tools pull-right">
                        <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
 
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="box-body no-padding" style="height:300px;overflow-y: scroll;">
                            <table class="table" id="tbl_running_veh_list">
                                <tr>
                                    <th style="width: 10px">
                                        #
                                    </th>
                                    <th>
                                        Vehicle No
                                    </th>
                                    <th>
                                       Type
                                    </th>
                                    <th>
                                        Route Name
                                    </th>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
             <div class="col-md-6">
              <div class="box box-danger">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i style="padding-right: 5px;" class="fa fa-bars"></i>  Available vehicles
                        </h3>
                        <div class="box-tools pull-right">
                        <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
 
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="box-body no-padding" style="height:300px;overflow-y: scroll;">
                            <table class="table" id="tbl_available_veh_list">
                                <tr>
                                    <th style="width: 10px">
                                        #
                                    </th>
                                    <th>
                                        Vehicle No
                                    </th>
                                    <th>
                                        Type
                                    </th>
                                    <th style="width: 40px">
                                        Make
                                    </th>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>

             </div>
        </div>
    </section>
</asp:Content>

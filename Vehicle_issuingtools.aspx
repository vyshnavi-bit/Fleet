<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="Vehicle_issuingtools.aspx.cs" Inherits="Vehicle_issuingtools" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/utility.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            getallveh_nos();
            get_tools();
            get_make();
            get_driverand_helper();
        });
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
        function get_tools() {
            var minimaster = "VehicleTools";
            var data = { 'op': 'get_Mini_Master_data', 'minimaster': minimaster };
            var s = function (msg) {
                if (msg) {
                    var slct_tools = document.getElementById('slct_tools');
                    var length = slct_tools.options.length;
                    document.getElementById('slct_tools').options.length = null;
                    var opt = document.createElement('option');
                    opt.innerHTML = "Select Tools";
                    opt.value = "Select Tools";
                    opt.setAttribute("selected", "selected");
                    opt.setAttribute("disabled", "disabled");
                    opt.setAttribute("class", "dispalynone");
                    slct_tools.appendChild(opt);
                    for (var i = 0; i < msg.length; i++) {
                        if (msg[i].sno != null) {
                            var option = document.createElement('option');
                            option.innerHTML = msg[i].mm_name;
                            option.value = msg[i].sno;
                            slct_tools.appendChild(option);
                        }
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function get_make() {
            var minimaster = "VehicleMake";
            var data = { 'op': 'get_Mini_Master_data', 'minimaster': minimaster };
            var s = function (msg) {
                if (msg) {
                    var slct_make = document.getElementById('slct_make');
                    var length = slct_make.options.length;
                    document.getElementById('slct_make').options.length = null;
                    var opt = document.createElement('option');
                    opt.innerHTML = "Select Make";
                    opt.value = "Select Make";
                    opt.setAttribute("selected", "selected");
                    opt.setAttribute("disabled", "disabled");
                    opt.setAttribute("class", "dispalynone");
                    slct_make.appendChild(opt);
                    for (var i = 0; i < msg.length; i++) {
                        if (msg[i].sno != null) {
                            var option = document.createElement('option');
                            option.innerHTML = msg[i].mm_name;
                            option.value = msg[i].sno;
                            slct_make.appendChild(option);
                        }
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function get_driverand_helper() {
            var data = { 'op': 'get_driver_and_helper' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        var data = document.getElementById('slct_drivername');
                        var length = data.options.length;
                        document.getElementById('slct_drivername').options.length = null;
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
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function save_Vehcleissuetools_click() {
            var slct_vehicle_no = document.getElementById('slct_vehicle_no').value;
            var slct_tools = document.getElementById('slct_tools').value;
            var slct_make = document.getElementById('slct_make').value;
            var slct_drivername = document.getElementById('slct_drivername').value;
            var txt_capacity = document.getElementById('txt_capacity').value;
            var txt_cost = document.getElementById('txt_cost').value;
            var cmb_alerttype = document.getElementById('cmb_alerttype').value;
            if (slct_vehicle_no == "" || slct_vehicle_no == "Select Vehicle No") {
                alert("Please select vehicle no");
                $('#slct_vehicle_no').focus();
                return false;
            }
            if (slct_tools == "" || slct_tools == "Select Tools") {
                alert("Please select tools");
                $('#slct_tools').focus();
                return false;
            }
            if (slct_make == "" || slct_make == "Select Make") {
                alert("Please select make");
                $('#slct_make').focus();
                return false;
            }
            if (slct_drivername == "" || slct_drivername == "Select Select Driver") {
                alert("Please select driver");
                $('#slct_drivername').focus();
                return false;
            }
            if (txt_capacity == "") {
                alert("Please enter capacity");
                $('#txt_capacity').focus();
                return false;
            }
            if (cmb_alerttype == "") {
                alert("Please select type");
                $('#cmb_alerttype').focus();
                return false;
            }
            var Data = { 'op': 'btnVehicle_tools_issue_return_saveclick', 'vehicleno': slct_vehicle_no, 'tools': slct_tools, 'make': slct_make,
                'driverid': slct_drivername, 'cost': txt_cost, 'capacity': txt_capacity, 'type': cmb_alerttype
            };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    document.getElementById('slct_vehicle_no').selectedIndex = 0;
                    document.getElementById('slct_tools').selectedIndex = 0;
                    document.getElementById('slct_make').selectedIndex = 0;
                    document.getElementById('slct_drivername').selectedIndex = 0;
                    document.getElementById('txt_capacity').value = "";
                    document.getElementById('txt_cost').value = "";
                    document.getElementById('cmb_alerttype').selectedIndex = 0;
                }
            }
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(Data, s, e);
        }
        function closeissuetools() {
            document.getElementById('slct_vehicle_no').selectedIndex = 0;
            document.getElementById('slct_tools').selectedIndex = 0;
            document.getElementById('slct_make').selectedIndex = 0;
            document.getElementById('slct_drivername').selectedIndex = 0;
            document.getElementById('txt_capacity').value = "";
            document.getElementById('txt_cost').value = "";
            document.getElementById('cmb_alerttype').selectedIndex = 0;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Tools Issue and Return<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Tools Issue and Return</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Vehicle Tools Issue and Return
                    Details
                </h3>
            </div>
            <div class="box-body">
                <table align="center">
                    <tr>
                        <td>
                            <label>
                                Vehicle Number <span style="color: red;">*</span></label>
                        </td>
                        <td style="height: 40px;">
                            <select id="slct_vehicle_no" class="form-control">
                            </select>
                            <label id="lbl_error_selectveh" class="errormessage">
                                * Please select Vehicle</label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-right: 10px; font-weight: 700;">
                            Tools
                        </td>
                        <td style="height: 40px;">
                            <select id="slct_tools" class="form-control">
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-right: 10px; font-weight: 700;">
                            Make
                        </td>
                        <td style="height: 40px;">
                            <select id="slct_make" class="form-control">
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-right: 10px; font-weight: 700;">
                            Driver Name
                        </td>
                        <td style="height: 40px;">
                            <select id="slct_drivername" class="form-control">
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-right: 10px; font-weight: 700;">
                            Capacity <span style="color: red;">*</span>
                        </td> 
                        <td style="height: 40px;">
                            <input type="text" id="txt_capacity" maxlength="45" class="form-control" name="vendorcode"
                                placeholder="Capacity">
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-right: 10px; font-weight: 700;">
                            Cost
                        </td>
                        <td style="height: 40px;">
                            <input type="text" id="txt_cost" maxlength="45" class="form-control" name="vendorcode"
                                placeholder="Cost">
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-right: 10px; font-weight: 700;">
                            Type
                        </td>
                        <td style="height: 40px;">
                            <select id="cmb_alerttype" class="form-control">
                                <option>Issued</option>
                                <option>Return</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <%--<tr>
                        <td colspan="2" align="center">
                            <input id='save_branch' type="button" name="submit" value='Save' onclick="save_Vehcleissuetools_click()"
                                class="btn btn-primary" />
                            <input id='close_branch' type="button" name="Close" value='Close' class="btn btn-danger" />
                        </td>
                    </tr>--%>
                </table>
                <table>
                <tr>
                <td style="padding-left: 424px;">
                    </td>
                    <td>
                        <div class="input-group">
                            <div class="input-group-addon">
                            <span class="glyphicon glyphicon-ok" id="save_issuetools1" onclick="save_Vehcleissuetools_click()"></span><span id="save_issuetools" onclick="save_Vehcleissuetools_click()">Save</span>
                        </div>
                        </div>
                        </td>
                        <td style="width:10px;"></td>
                        <td>
                            <div class="input-group">
                            <div class="input-group-close">
                            <span class="glyphicon glyphicon-remove" id='close_issuetools1' onclick="closeissuetools()"></span><span id='close_issuetools' onclick="closeissuetools()">Clear</span>
                        </div>
                        </div>
                        </td>
                </tr>
                </table>
            </div>
        </div>
    </section>
</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Routes.aspx.cs" MasterPageFile="~/Operations.master"
    Inherits="Routes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%-- <link href="css/formstable.css" rel="stylesheet" type="text/css" />
    <link href="css/custom.css" rel="stylesheet" type="text/css" />--%>
    <script type="text/javascript">
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
        function CallHandlerUsingJson(d, s, e) {
            d = JSON.stringify(d);
            d = d.replace(/&/g, '\uFF06');
            d = d.replace(/#/g, '\uFF03');
            d = d.replace(/\+/g, '\uFF0B');
            d = d.replace(/\=/g, '\uFF1D');
            $.ajax({
                type: "GET",
                url: "FleetManagementHandler.axd?json=",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: d,
                async: true,
                cache: true,
                success: s,
                error: e
            });
        }
        function retriveroutesdata() {
            var table = document.getElementById("tbl_routes");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var data = { 'op': 'getroutes' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillroutes(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
    </script>
    <script type="text/javascript">
        $(function () {
            retriveroutesdata();
            $('#addroute').click(function () {
                $('#routefillform').css('display', 'block');
                $('#routeshowlogs').css('display', 'none');
                $('#div_routetable').css('display', 'none');
                document.getElementById('txt_routename').value = "";
                document.getElementById('txt_mobileno').value = "";
                document.getElementById('saveroute').innerHTML = "Save";
            });
            $('#closeroute').click(function () {
                $('#routefillform').css('display', 'none');
                $('#routeshowlogs').css('display', 'block');
                $('#div_routetable').css('display', 'block');
                document.getElementById('txt_routename').value = "";
                document.getElementById('txt_ledgername').value = "";
                document.getElementById('txt_mobileno').value = "";
                document.getElementById('saveroute').innerHTML = "Save";
            });
        });
        function addroutes() {
            $('#routefillform').css('display', 'block');
            $('#routeshowlogs').css('display', 'none');
            $('#div_routetable').css('display', 'none');
            document.getElementById('txt_routename').value = "";
            document.getElementById('txt_mobileno').value = "";
            document.getElementById('saveroute').innerHTML = "Save";
        }
        function closeroutes() {
            $('#routefillform').css('display', 'none');
            $('#routeshowlogs').css('display', 'block');
            $('#div_routetable').css('display', 'block');
            document.getElementById('txt_routename').value = "";
            document.getElementById('txt_ledgername').value = "";
            document.getElementById('txt_mobileno').value = "";
            document.getElementById('saveroute').innerHTML = "Save";
        }
        function save_roots() {
            var routename = document.getElementById('txt_routename').value;
            var ledgername = document.getElementById('txt_ledgername').value;
            var mobileno = document.getElementById('txt_mobileno').value;
            var g = document.getElementById('cmb_routestatus');
            var status = g.options[g.selectedIndex].value;
            if (routename == "") {
                alert("Please enter Route Name");
            }
            var btnsts = document.getElementById('saveroute').innerHTML;
            var data = { 'op': 'save_route', 'routename': routename, 'ledgername': ledgername, 'status': status, 'btnval': btnsts, 'updatesno': updatesno, 'mobileno': mobileno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        if (typeof msg == "string") {
                            alert(msg);
                            document.getElementById('txt_routename').value = "";
                            retriveroutesdata();
                            //$('#saveroute').val("Save");
                            document.getElementById('saveroute').innerHTML = "Save";
                            updatesno = "";
                            $('#routefillform').css('display', 'none');
                            $('#routeshowlogs').css('display', 'block');
                            $('#div_routetable').css('display', 'block');
                        }
                    }
                }
                else {
                    alert("Error! Please try again");
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            CallHandlerUsingJson(data, s, e);
        }
        function fillroutes(results) {
            var k = 0;
            var colorue = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            var table = document.getElementById("tbl_routes");
            for (var i = 0; i < results.length; i++) {
                if (results[i].routename != null) {
                    var routename = results[i].routename;
                    var statuscode = results[i].status;
                    var ledgername = results[i].ledgername;
                    var mobileno = results[i].mobileno;
                    var status = "";
                    if (statuscode == "1") {
                        status = "Enabled";
                    }
                    else {
                        status = "Disabled";
                    }
                    var sno = results[i].sno;
                    var tablerowcnt = document.getElementById("tbl_routes").rows.length;
                    $('#tbl_routes').append('<tr style="background-color:' + colorue[k] + '">' +
                    //'<td scope="row">' + routename + '</td>' +
                    '<td style="font-weight: 600;"><i class="glyphicon glyphicon-map-marker" aria-hidden="true"></i>&nbsp;<span id="0">' + routename + '</span></td>' +
                    //'<td scope="row">' + ledgername + '</td>' +
                    '<td ><i class="" aria-hidden="true"></i>&nbsp;<span id="2">' + ledgername + '</span></td>' +
                    '<td data-title="Status">' + status + '</td>' +
                    '<td data-title="sno" style="display:none;">' + sno + '</td>' +
                    '<td data-title="mobileno">' + mobileno + '</td>' +
                    //'<td><input type="button" class="btn btn-primary" name="Update" value ="Update" onclick="updateclick(this);"/></td></tr>');
                    '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls" name="Update" value ="Modify" onclick="updateclick(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>');
                    k = k + 1;
                    if (k == 4) {
                        k = 0;
                    }
                }
            }
        }
        var updatesno = "";
        function updateclick(thisid) {
            var row = $(thisid).parents('tr');
            var sno = row[0].cells[3].innerHTML;
            updatesno = sno;
            var routename = $(thisid).parent().parent().find('#0').text();
            var ledgername = $(thisid).parent().parent().find('#2').text();
            var mobileno = row[0].cells[4].innerHTML;
            var statuscode = row[0].cells[2].innerHTML;
            var status = "";
            if (statuscode == "Enabled") {
                status = "1";
            }
            else {
                status = "0";
            }
            document.getElementById('txt_routename').value = routename;
            document.getElementById('txt_ledgername').value = ledgername;
            document.getElementById('txt_mobileno').value = mobileno;
            document.getElementById('cmb_routestatus').value = status;
            $('#routefillform').css('display', 'block');
            $('#routeshowlogs').css('display', 'none');
            $('#div_routetable').css('display', 'none');
            //$('#saveroute').val("Modify");
            document.getElementById('saveroute').innerHTML = "Modify";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>Route Master<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Master</a></li>
            <li><a href="#">Route Master</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Route Master Details
                </h3>
            </div>
            <div class="box-body">
                <div id="routeshowlogs" style="text-align: center;">
                    <div class="input-group" style="width: 100px; padding-left: 960px;">
                        <div class="input-group-addon">
                            <span class="glyphicon glyphicon-plus-sign" onclick="addroutes()"></span><span id="addroute" onclick="addroutes()">Add Route</span>
                        </div>
                    </div>

                </div>
                <div id='routefillform' style="display: none;" class='CSSTableGenerator'>
                    <table align="center">
                        <tr>
                            <td>
                                <label>Route Name </label>
                                <span style="color: red;">*</span>
                            </td>
                            <td style="height: 40px;">
                                <input id="txt_routename" type="text" name="vendorcode" class="form-control" placeholder="Route Name">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>Ledger Name </label>
                                <span style="color: red;">*</span>
                            </td>
                            <td style="height: 40px;">
                                <input id="txt_ledgername" type="text" name="vendorcode" class="form-control" placeholder="Ledger Name">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>Mobile Number </label>
                                <span style="color: red;">*</span>
                            </td>
                            <td style="height: 40px;">
                                <input id="txt_mobileno" type="number" name="vendorcode" class="form-control" placeholder="Enter Mobile Number">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label > Status </label>
                                <span style="color: red;">*</span>
                            </td>
                            <td style="height: 40px;">
                                <select id="cmb_routestatus" class="form-control">
                                    <option value="1">Enabled</option>
                                    <option value="0">Disabled</option>
                                </select>
                            </td>
                        </tr>
                     
                    </table>
                    <table>
                        <tr>
                            <td>
                                <td style="padding-left: 435px;"></td>
                                <td>
                                    <div class="input-group">
                                        <div class="input-group-addon">
                                            <span class="glyphicon glyphicon-ok" id="saveroutes" onclick="save_roots()"></span><span id="saveroute" onclick="save_roots()">Save</span>
                                        </div>
                                    </div>
                                </td>
                                <td style="width: 10px;"></td>
                                <td>
                                    <div class="input-group">
                                        <div class="input-group-close">
                                            <span class="glyphicon glyphicon-remove" id='closeroutes' onclick="closeroutes()"></span><span id='closeroute' onclick="closeroutes()">Close</span>
                                        </div>
                                    </div>
                                </td>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="background: #ffffff">
                    <div id="div_routetable" class='divcontainer'>
                        <table id="tbl_routes" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">
                            <thead>
                                <tr style="background: #5aa4d0; color: white; font-weight: bold;">
                                    <th scope="col">Route Name
                                    </th>
                                    <th scope="col">Ledger Name
                                    </th>
                                    <th scope="col">Status
                                    </th>
                                    <th scope="col">Mobile Number
                                    </th>
                                    <th scope="col" style="display: none;">sno
                                    </th>
                                    <th scope="col"></th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>

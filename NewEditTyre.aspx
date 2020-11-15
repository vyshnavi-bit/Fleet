<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="NewEditTyre.aspx.cs" Inherits="NewEditTyre" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/utility.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            get_vehicles_data();
        });

        function get_vehicles_data() {
            var data = { 'op': 'getalldataforissuetyre' };
            var s = function (msg) {
                if (msg) {
                    fillcombos(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function fillcombos(msg) {
            var data = msg;
            var getvehicles = data["vehicles"];
            var vehicles = document.getElementById('Seslct_vehiclenumber');
            var length = vehicles.options.length;
            document.getElementById('Seslct_vehiclenumber').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Vehicle";
            opt.value = "Select Vehicle";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            vehicles.appendChild(opt);
            for (i = 0; i < getvehicles.length; i++) {
                var option = document.createElement('option');
                option.innerHTML = getvehicles[i].vehiclenum;
                option.value = getvehicles[i].vehiclesno;
                vehicles.appendChild(option);
            }
        }

        function fitting_change() {
            var fittingtype = document.getElementById("slct_tyrertype").value;
            if (fittingtype == "Fitted") {
                $("#veh_div").show();
            }
            else if (fittingtype == "NotFitted") {
                $("#veh_div").hide();
                getalltyres();
            }
        }
        function getalltyres() {
            var vehicleno = document.getElementById("Seslct_vehiclenumber").value;
            var fittingtype = document.getElementById("slct_tyrertype").value;
            var table = document.getElementById("tyres_table");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var data = { 'op': 'get_remaining_tyres_data', 'vehicleno': vehicleno, 'fittingtype': fittingtype };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        var main_string = "";
                        for (var i = 0; i < msg.length; i++) {
                            if (msg[i].sno != null) {
                                if (msg[i].SVDSNO == "" || msg[i].SVDSNO == null || msg[i].min_grove == "" || msg[i].min_grove == null || msg[i].max_grove == "" || msg[i].max_grove == null || msg[i].grove == "" || msg[i].grove == null) {
                                    $("#tyres_table").append('<tr><td>' + (i + 1) + '</td><td><label>' + msg[i].tyresno + '</label></td>' +
                                                         '<td><label>' + msg[i].Brand + '</label></td>' +
                                                         '<td><label>' + msg[i].Type_of_Tyre + '</label></td>' +
                                                         '<td><label>' + msg[i].Tube_Type + '</label></td>' +
                                                         '<td><label>' + msg[i].Size + '</label></td>' +
                                                         '<td><input name="SVDSNO" class="form-control" type="text" placeholder="SVDS No" value="' + msg[i].SVDSNO + '" style="width:100px;"/></td>' +
                                                         '<td><input name="min_grove" class="form-control" type="text" placeholder="Min Grove" value="' + msg[i].min_grove + '" style="width:100px;"/></td>' +
                                                         '<td><input name="max_grove" class="form-control" type="text" placeholder="Max Grove" value="' + msg[i].max_grove + '" style="width:100px;"/></td>' +
                                                         '<td><input name="grove" class="form-control" type="text" placeholder="Present Grove" value="' + msg[i].grove + '" style="width:100px;"/></td>' +
                                                         '<td><label name="sub_sno">' + msg[i].sno + '</label></tr>');
                                }
                            }
                        }
                    }
                    only_no();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        $(document).ready(function () {
            $("[name=Cost],[name=min_grove],[name=max_grove],[name=grove]").keydown(function (event) {
                // Allow: backspace, delete, tab, escape, and enter
                if (event.keyCode == 46 || event.keyCode == 110 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 || event.keyCode == 190 || event.keyCode == 110 ||
                // Allow: Ctrl+A
            (event.keyCode == 65 && event.ctrlKey === true) ||
                // Allow: home, end, left, right
            (event.keyCode >= 35 && event.keyCode <= 39)) {
                    // let it happen, don't do anything
                    return;
                }
                else {
                    // Ensure that it is a number and stop the keypress
                    if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                        event.preventDefault();
                    }
                }
            });
        });
        function save_tyre_click() {
            var tyres = [];
            $('#tyres_table> tbody > tr').each(function () {
                var SVDSNO = $(this).find('[name=SVDSNO]').val();
                var min_grove = $(this).find('[name=min_grove]').val();
                var max_grove = $(this).find('[name=max_grove]').val();
                var grove = $(this).find('[name=grove]').val();
                var sno = $(this).find('[name=sub_sno]').html();
                if (sno != "") {
                    tyres.push({ 'SVDSNO': SVDSNO, 'min_grove': min_grove, 'max_grove': max_grove, 'sno': sno, 'grove': grove });
                }
            });
            var data = { 'op': 'Tyres_save_start' };
            var s = function (msg) {
                if (msg) {
                    for (var i = 0; i < tyres.length; i++) {
                        var Data = { 'op': 'Tyres_save_RowData', 'row_detail': tyres[i], 'end': 'N' };
                        if (i == tyres.length - 1) {
                            Data = { 'op': 'Tyres_save_RowData', 'row_detail': tyres[i], 'end': 'Y' };
                        }
                        var s = function (msg) {
                            if (msg == 'Y') {

                                var Data = { 'op': 'save_edit_Tyres_new' };
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
        function resetall() {
            document.getElementById("Seslct_vehiclenumber").value = "Select Vehicle";
            document.getElementById("slct_tyrertype").value = "Select Fitting Type";
            var table = document.getElementById("tyres_table");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            $("#veh_div").hide();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id='vehmaster_fillform' style=" padding: 20px;">
        <div class="row">
            <div class="form-group">
                <label>
                    Tyre Type</label>
                <select id="slct_tyrertype" class="form-control" style="min-width: 195px;" onchange="fitting_change()">
                    <option value="Select Fitting Type" selected disabled>Select Fitting Type</option>
                    <option value="Fitted">Fitted</option>
                    <option value="NotFitted">NotFitted</option>
                </select>
            </div>
            <div class="form-group" id="veh_div" style="display: none;">
                <label>
                    Vehicle Number<%--<span style="color: red;">*</span>--%></label>
                <select id="Seslct_vehiclenumber" class="form-control" style="min-width: 195px;"
                    onchange="getalltyres()">
                </select>
            </div>
        </div>
        <div class="table-responsive">
            <table class="table table-condensed" id="tyres_table">
                <thead>
                    <tr>
                        <th>
                            #
                        </th>
                        <th>
                            Tyre_Sno
                        </th>
                        <th>
                            Brand
                        </th>
                        <th>
                            Type_of_Tyre
                        </th>
                        <th>
                            Tube_Type
                        </th>
                        <th>
                            Size
                        </th>
                        <th>
                            SVDS No
                        </th>
                        <th>
                            Min Grove
                        </th>
                        <th>
                            Max Grove
                        </th>
                        <th>
                            Present Grove
                        </th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
        </div>
        <div align="center">
            <input id='save_tyre' type="button" class="btn btn-primary" value='Edit' onclick="save_tyre_click()" />
            <input id='close_tyre' type="button" class="btn btn-primary" value='Reset' onclick="resetall()" />
        </div>
    </div>
</asp:Content>

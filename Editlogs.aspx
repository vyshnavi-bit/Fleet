<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="Editlogs.aspx.cs" Inherits="EditTriplogs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/utility.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            retrive_locations();
        });
        var options = "";
        function retrive_locations() {
            var data = { 'op': 'retrive_all_location' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        options = "";
                        for (var i = 0; i < msg.length; i++) {
                            if (msg[i].Location_name != null) {
                                options += "<option value=" + msg[i].sno + ">" + msg[i].Location_name + "</option>";
                            }
                        }
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
        var triplogdsdata = [];
        function get_triplogs_data() {
            var tripid = document.getElementById('txt_Tripid').value;
            if (tripid == "") {
                alert("Enter Tripsheetno");
                return false;
            }
            var data = { 'op': 'get_Edit_triplogs_data', 'tripid': tripid };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        triplogdsdata = [];
                        triplogdsdata = msg;
                        gen_tripdata(msg);
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

        var nme = 0;
        function gen_tripdata(msg) {
            var table = document.getElementById("tbl_trip_locations");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].doe != "" || msg[i].doe != null) {

                    $("#tbl_trip_locations").append('<tr><td><input style="max-width: 185px;font-size:12px;padding: 0px 5px;height:30px;" type="datetime-local" class="form-control" name="datetime_log" /></td>' +
                '<td data-title="From"><select class="form-control" name="From_location" style="width: 100px;font-size:12px;padding: 0px 5px;height:30px;" ><option selected disabled value="Location">Location</option>' + options + '</select></td>' +
                '<td data-title="Odometer"><input class="form-control" type="text" placeholder="OdoMeter" name="OdoMeter" style="width:70px;font-size:12px;padding: 0px 5px;height:30px;" onblur="cal_odomtr(this)"/></td>' +
                '<td data-title="KMS"><input class="form-control" type="text" placeholder="KMS" name="kms" value="0" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;" /></td>' +
                '<td data-title="Cost"><input id="Text1" class="form-control" type="text" placeholder="Charge" name="charge" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="Load"><input name="txt_capload" class="form-control" type="text" placeholder="In Ltrs" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/><input name="txt_capload_kgs" class="form-control" type="text" placeholder="In Kgs" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="UnLoad"><input name="txt_capunload" class="form-control" type="text" placeholder="In Ltrs" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/><input name="txt_capunload_kgs" class="form-control" type="text" placeholder="In Kgs" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="Fuel"><label><input name="' + nme + '" type="radio" class="Own"/>Own</label><label><input name="' + nme + '" type="radio" class="Hired"/>Hired</label><input class="form-control" name="fuel" type="text" placeholder="Fuel" style="width:90px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="acfuel"><input class="form-control" name="acfuel" type="text" placeholder="acfuel" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="Expences"><input class="form-control" name="expences" type="text" placeholder="Expences" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="TollGate"><input class="form-control" name="tollgate" type="text" placeholder="TollGate" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="Remarks"><textarea class="form-control" name="remarks" placeholder="Remarks" style="width:70px;font-size:12px;padding: 0px 5px;" cols="20" rows="3"></textarea></td>' +
                '<td data-title="Plus"><span><img src="images/plus.png" onclick="addrow(this)" style="cursor:pointer"/></span></td>' +
                '<td data-title="Minus"><span><img src="images/minus.png" onclick="removerow(this)" style="cursor:pointer"/></span></td></tr>');
                    nme++;
                }
            }
            fill_tripdata();
        }



        function addrow(thisid) {
            nme = 0;
            var row = $(thisid).parents('tr');
            var rowindex = $(thisid).index();
            nme = rowindex + 1;
            $(row).after('<tr><td><input style="max-width: 185px;font-size:12px;padding: 0px 5px;height:30px;" type="datetime-local" class="form-control" name="datetime_log" /></td>' +
                '<td data-title="From"><select class="form-control" name="From_location" style="width: 100px;font-size:12px;padding: 0px 5px;height:30px;" ><option selected disabled value="Location">Location</option>' + options + '</select></td>' +
                '<td data-title="Odometer"><input class="form-control" type="text" placeholder="OdoMeter" name="OdoMeter" style="width:70px;font-size:12px;padding: 0px 5px;height:30px;" onblur="cal_odomtr(this)"/></td>' +
                '<td data-title="KMS"><input class="form-control" type="text" placeholder="KMS" name="kms" value="0" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="Cost"><input id="Text1" class="form-control" type="text" placeholder="Charge" name="charge" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="Load"><input name="txt_capload" class="form-control" type="text" placeholder="In Ltrs" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/><input name="txt_capload_kgs" class="form-control" type="text" placeholder="In Kgs" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="UnLoad"><input name="txt_capunload" class="form-control" type="text" placeholder="In Ltrs" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/><input name="txt_capunload_kgs" class="form-control" type="text" placeholder="In Kgs" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="Fuel"><label><input name="' + nme + '" type="radio" class="Own"/>Own</label><label><input name="' + nme + '" type="radio" class="Hired"/>Hired</label><input class="form-control" name="fuel" type="text" placeholder="Fuel" style="width:90px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="acfuel"><input class="form-control" name="acfuel" type="text" placeholder="acfuel" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="Expences"><input class="form-control" name="expences" type="text" placeholder="Expences" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="TollGate"><input class="form-control" name="tollgate" type="text" placeholder="TollGate" style="width:50px;font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="Remarks"><textarea class="form-control" name="remarks" placeholder="Remarks" style="width:70px;font-size:12px;padding: 0px 5px;" cols="20" rows="3"></textarea></td>' +
                '<td data-title="Plus"><span><img src="images/plus.png" onclick="addrow(this)" style="cursor:pointer"/></span></td>' +
                '<td data-title="Minus"><span><img src="images/minus.png" onclick="removerow(this)" style="cursor:pointer"/></span></td></tr>');


        }
        function removerow(thisid) {
            $(thisid).parents('tr').remove();
        }


        function fill_tripdata() {
            var msginddex = 0;
            $('#tbl_trip_locations> tbody > tr').each(function () {

                $(this).find('[name="datetime_log"]').val(triplogdsdata[msginddex].doe);
                $(this).find('[name="From_location"] option[value=' + triplogdsdata[msginddex].place + ']').attr("selected", "selected");

                //$('select[name="From_location"]').each(function () { this.selected = (this.value == triplogdsdata[msginddex].place); });


                $(this).find('[name="kms"]').val(triplogdsdata[msginddex].km);
                $(this).find('[name="charge"]').val(triplogdsdata[msginddex].charge);
                $(this).find('[name="txt_capload"]').val(triplogdsdata[msginddex].load_cap);
                $(this).find('[name="txt_capunload"]').val(triplogdsdata[msginddex].unload_cap);

                $(this).find('[name="txt_capload_kgs"]').val(triplogdsdata[msginddex].load_cap_kgs);
                $(this).find('[name="txt_capunload_kgs"]').val(triplogdsdata[msginddex].unload_cap_kgs);

                $(this).find('[name="fuel"]').val(triplogdsdata[msginddex].fuel);
                $(this).find('[name="acfuel"]').val(triplogdsdata[msginddex].acfuel);
                $(this).find('[name="expences"]').val(triplogdsdata[msginddex].expamount);
                $(this).find('[name="tollgate"]').val(triplogdsdata[msginddex].tollgateamnt);
                $(this).find('[name="OdoMeter"]').val(triplogdsdata[msginddex].odometer);

                $(this).find('[name="remarks"]').val(triplogdsdata[msginddex].remarks);

                //                var rowindex = $(this).index();
                //                var rank = (parseInt(rowindex) + 1).toString();
                if (triplogdsdata[msginddex].fuel_type == "OWN") {
                    $(this).find('.Own').attr('checked', 'checked');
                }
                if (triplogdsdata[msginddex].fuel_type == "HIRED") {
                    $(this).find('.Hired').attr('checked', 'checked');
                }
                msginddex++;

            });
        }


        function for_resettriplogs() {
            var table = document.getElementById("tbl_trip_locations");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            document.getElementById("slct_triplogs_tripid").value = "Select Trip";
            document.getElementById('btn_save_triplogs').disabled = false;

        }


        function cal_odomtr(thisid) {
            var thisodo = $(thisid).val();
            var prevodo = $(thisid).parent().parent().prev().children().find('[name=OdoMeter]').val();
            var kms = parseInt(thisodo) - parseInt(prevodo);
            $(thisid).parent().parent().find('[name=kms]').val(kms);
        }

        //Function for only no
        $(document).ready(function () {
            //$("[name=charge]").keydown(function (event) {
            $("[name=charge],[name=txt_capload],[name=txt_capunload],[name=fuel],[name=expences],[name=tollgate],[name=kms],[name=OdoMeter],[name=txt_capload_kgs],[name=txt_capunload_kgs]").keydown(function (event) {
                // Allow: backspace, delete, tab, escape, and enter
                if (event.keyCode == 46 || event.keyCode == 110 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 || event.keyCode == 190 ||
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

        function for_saving_trip_logs() {
            var tripid = document.getElementById('txt_Tripid').value;
            var tripbtnval = document.getElementById('btn_save_triplogs').value;
            var triplogs_array = [];
            var flag = false;
            $('#tbl_trip_locations> tbody > tr').each(function () {
                var rowindex = $(this).index();
                var rank = (parseInt(rowindex) + 1).toString();
                var log_datetime = $(this).find('[name="datetime_log"]').val();
                if (log_datetime == "") {
                    alert("Please Enter Proper Date Time for " + rank + " Log");
                    flag = true;
                    return false;

                }
                var log_tolocation = $(this).find('select[name*="To_location"] :selected').val();
                var log_fromlocation = $(this).find('select[name*="From_location"] :selected').val();
                if (log_fromlocation == "Location") {
                    alert("Please Select Proper location at " + rank + " Log");
                    flag = true;

                    return false;
                }
                var log_kms = $(this).find('[name="kms"]').val();
                var log_charge = $(this).find('[name="charge"]').val();
                var log_capload = $(this).find('[name="txt_capload"]').val();
                var log_capunload = $(this).find('[name="txt_capunload"]').val();

                var log_capload_kgs = $(this).find('[name="txt_capload_kgs"]').val();
                var log_capunload_kgs = $(this).find('[name="txt_capunload_kgs"]').val();

                var log_fuel = $(this).find('[name="fuel"]').val();
                var log_acfuel = $(this).find('[name="acfuel"]').val();
                var log_expences = $(this).find('[name="expences"]').val();
                var log_tollgate = $(this).find('[name="tollgate"]').val();
                var log_OdoMeter = $(this).find('[name="OdoMeter"]').val();

                var log_remarks = $(this).find('[name="remarks"]').val();


                var checkbox = "NO";
                if ($(this).find('.Own').is(":checked")) {
                    checkbox = "OWN";
                }
                if ($(this).find('.Hired').is(":checked")) {
                    checkbox = "HIRED";
                }

                if (log_datetime != "") {
                    triplogs_array.push({
                        'log_datetime': log_datetime, 'log_acfuel': log_acfuel, 'log_tolocation': log_tolocation, 'log_fromlocation': log_fromlocation, 'log_kms': log_kms,
                        'log_charge': log_charge, 'log_capload': log_capload, 'log_capunload': log_capunload, 'log_fuel': log_fuel, 'log_expences': log_expences,
                        'rank': rank, 'log_tollgate': log_tollgate, 'log_fueltype': checkbox, 'log_OdoMeter': log_OdoMeter, 'log_capload_kgs': log_capload_kgs, 'log_capunload_kgs': log_capunload_kgs
                    });
                }

            });
            if (flag) {
                return;
            }

            var Data = {
                'op': 'Triplogs_edit_Click', 'triplogs_array': triplogs_array, 'tripid': tripid, 'tripbtnval': tripbtnval
            };
            var s = function (msg) {
                if (msg) {
                    alert(msg);

                }
            }
            var e = function (x, h, e) {
            };
            CallHandlerUsingJson_POST(Data, s, e);
        }
        function CallHandlerUsingJson_POST(d, s, e) {
            d = JSON.stringify(d);
            //    d = d.replace(/&/g, '\uFF06');
            //    d = d.replace(/#/g, '\uFF03');
            //    d = d.replace(/\+/g, '\uFF0B');
            //    d = d.replace(/\=/g, '\uFF1D');
            d = encodeURIComponent(d);
            $.ajax({
                type: "POST",
                url: "FleetManagementhandler.axd?json=",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: d,
                async: true,
                cache: true,
                success: s,
                error: e
            });
        }

        function CallHandlerUsingJson(d, s, e) {
            d = JSON.stringify(d);
            //    d = d.replace(/&/g, '\uFF06');
            //    d = d.replace(/#/g, '\uFF03');
            //    d = d.replace(/\+/g, '\uFF0B');
            //    d = d.replace(/\=/g, '\uFF1D');
            d = encodeURIComponent(d);
            $.ajax({
                type: "GET",
                url: "FleetManagementhandler.axd?json=",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: d,
                async: true,
                cache: true,
                success: s,
                error: e
            });
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>Edit Trip Logs<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Edit Trip Logs</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Trip Logs Details
                </h3>
            </div>
            <div class="box-body">
                <div role="tabpanel" class="tab-pane" id="TripLogs">
                    <div class="row">
                        <div>
                            <table>
                                <tr>
                                    <td>
                                        <label>
                                            Trip Ref No</label>
                                    </td>
                                    <td>
                                        <input type="text" id="txt_Tripid" maxlength="45" class="form-control" placeholder="Enter Trip Ref No">
                                    </td>
                                    <td style="width: 5px;"></td>
                                    <td>
                                        <input id="Button1" type="button" class="btn btn-primary" value="Get Trip Details" onclick="get_triplogs_data()" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <br />
                    <div style="overflow-x: scroll;">
                        <table id="tbl_trip_locations" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">
                            <thead>
                                <tr>
                                    <th scope="col">Date
                                    </th>
                                    <th scope="col">Location
                                    </th>
                                    <th scope="col">OdoMeter
                                    </th>
                                    <th scope="col">KMS
                                    </th>
                                    <th scope="col">Cost
                                    </th>
                                    <th scope="col">Load
                                    </th>
                                    <th scope="col">UnLoad
                                    </th>
                                    <th scope="col">Diesel Type
                                    </th>
                                    <th scope="col">AC Fuel
                                    </th>
                                    <th scope="col">Expences
                                    </th>
                                    <th scope="col">TollGate
                                    </th>
                                    <th scope="col">Remarks
                                    </th>
                                    <th></th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                    <div>
                        <div align="right">
                            <input id="btn_save_triplogs" type="button" class="btn btn-primary" value="EDIT Trip Logs"
                                onclick="for_saving_trip_logs()" />
                        </div>
                        <div align="left">
                            <input id="btn_resettriplogs" type="button" class="btn btn-danger" value="Reset Trip Logs"
                                onclick="for_resettriplogs()" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>

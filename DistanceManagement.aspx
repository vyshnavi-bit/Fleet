<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="DistanceManagement.aspx.cs" Inherits="DistanceManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<%--<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <link href="opcss/bootextract.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="dist/css/bootstrap.css" />--%>
    <script src="js/utility.js" type="text/javascript"></script>

    <style type="text/css">
    
    div.sub_td:before
    {
        float:right;
    }
    div.sub_td:after
    {
        float:none;
    }
    
    </style>

    <script type="text/javascript">

        $(function () {
            retrive_locations();
        });
        function retrive_locations() {
            var data = { 'op': 'retrive_all_location' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filllocations(msg);
                    }
                    else {
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

        function filllocations(msg) {
            var department = document.getElementById('slct_locations');
            var length = department.options.length;
            document.getElementById('slct_locations').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Location";
            opt.value = "Select Location";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            department.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].Location_name != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].Location_name;
                    option.value = msg[i].sno;
                    department.appendChild(option);
                }
            }
        }

        function generate_locations() {
            var d = document.getElementById('slct_locations');
            var location_val = d.options[d.selectedIndex].value;
            var location_name = d.options[d.selectedIndex].text;
            var data = { 'op': 'generate_locations', 'location_val': location_val };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        var table = document.getElementById("distance_tbl");
                        for (var i = table.rows.length - 1; i > 0; i--) {
                            table.deleteRow(i);
                        }
                        for (var i = 0; i < msg.length; i++) {
                            if (msg[i].Location_name != null && msg[i].Location_name != "") {
                                $("#distance_tbl").append('<tr><td>' + (i + 1) + '</td><td>' + location_name + '</td><td>' + msg[i].Location_name + '</td><td style="display:none;">' + msg[i].sno + '</td><td><input type="text" class="form-control" name="KMS" placeholder="KMS"/></td></tr>');
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
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function for_Main_Saving() {
            var d = document.getElementById('slct_locations');
            var location_val = d.options[d.selectedIndex].value;
            var location_name = d.options[d.selectedIndex].text;
            var btn_value = document.getElementById('btn_save').value;

            var distance_ary = [];
            $('#distance_tbl> tbody > tr').each(function () {
                if ($(this).find("[name=KMS]").val() != "") {
                    distance_ary.push({ "To_location": $(this).find("td:eq(3)").text(), "Distance": $(this).find("[name=KMS]").val() });
                }
            });
            var data = { 'op': 'Distances_save_start' };
            var s = function (msg) {
                if (msg) {
                    for (var i = 0; i < distance_ary.length; i++) {
                        var Data = { 'op': 'Distances_save_RowData', 'row_detail': distance_ary[i], 'end': 'N' };
                        if (i == distance_ary.length - 1) {
                            Data = { 'op': 'Distances_save_RowData', 'row_detail': distance_ary[i], 'end': 'Y' };
                        }
                        var s = function (msg) {
                            if (msg == 'Y') {

                                var Data = { 'op': 'save_edit_Distances', 'location_val': location_val, 'btn_value': btn_value };
                                var s = function (msg) {
                                    if (msg) {
                                        alert(msg);
                                    }
                                }
                                var e = function (x, h, e) {
                                };
                                $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                                callHandler(Data, s, e);
                            }
                        }
                        var e = function (x, h, e) {
                        };
                        $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                        CallHandlerUsingJson(Data, s, e);
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div style="width: 100%; height: 100%;">
        <div id="second_div" style="padding: 20px;">
            <div class="row">
                <div class="form-group">
                    <label>
                        Select Location</label>
                    <select id="slct_locations" class="form-control" onchange="generate_locations()">
                      <option selected disabled value="Select Location">Select Location</option>
                    </select>
                </div>
            </div>


            <div>
            
                <table class="table table-condensed" id="distance_tbl">
                    <thead>
                        <tr>
                            <th>
                                #S.No
                            </th>
                            <th>
                                #From Location
                            </th>
                            <th>
                                #To Location
                            </th>
                            <th style="display:none;">
                            </th>
                            <th>
                                #Distance
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            
            </div>

            <div>
            
                    <input id="btn_save" type="button" class="btn btn-primary" value="Save Distances" onclick="for_Main_Saving()"/>
            
            </div>

        </div>
    </div>

</asp:Content>


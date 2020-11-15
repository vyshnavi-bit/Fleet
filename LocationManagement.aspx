<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="LocationManagement.aspx.cs" Inherits="LocationManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <link href="opcss/bootextract.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="dist/css/bootstrap.css" />--%>
    <script src="js/utility.js" type="text/javascript"></script>
    <style type="text/css">
        div.sub_td:before
        {
            float: right;
        }
        div.sub_td:after
        {
            float: none;
        }
        .row + .row > *
        {
            padding: 0px 0 0 40px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            retrive_locations();
            clearall();
        });
        function for_Main_Saving() {
            var locationname = document.getElementById('txt_location_name').value;
            if (locationname == "") {
                alert("Please Enter Location");
                $('#txt_location_name').focus();
                return false;
            }
            var btnval = document.getElementById('btn_save').innerHTML;
            var sno = document.getElementById('lbl_sno').innerHTML;
            var data = { 'op': 'edit_save_location', 'locationname': locationname, 'btnval': btnval, 'sno': sno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        alert(msg);
                        retrive_locations();
                        clearall();
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
        function retrive_locations() {
            var k = 0;
            var colorue = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            var data = { 'op': 'retrive_all_location' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        var table = document.getElementById("tbl_locations");
                        for (var i = table.rows.length - 1; i > 0; i--) {
                            table.deleteRow(i);
                        }
                        for (var i = 0; i < msg.length; i++) {
                            if (msg[i].Location_name != null && msg[i].Location_name != "") {
                                $("#tbl_locations").append('<tr style="background-color:' + colorue[k] + '">' +
                                '<td scope="row">' + (i + 1) + '</td>' +
                                //'<td data-title="Location Name">' + msg[i].Location_name + '</td>' +
                                '<td style="font-weight: 600;"><i class="glyphicon glyphicon-map-marker" aria-hidden="true"></i>&nbsp;<span id="0">' + msg[i].Location_name + '</span></td>' +   
                                '<td style="display:none;">' + msg[i].sno + '</td>' +
                                '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls" name="Update" value ="Modify"  onclick="updateclick(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>');
                                k = k + 1;
                                if (k == 4) {
                                    k = 0;
                                }
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
        function updateclick(thisid) {
            var row = $(thisid).parents('tr');
            var location_name = $(thisid).parent().parent().find('#0').text();
            var sno = row[0].cells[2].innerHTML;
            document.getElementById('txt_location_name').value = location_name;
            document.getElementById('btn_save').innerHTML = "Modify";
            document.getElementById('lbl_sno').innerHTML = sno;
        }
        function clearall() {
            document.getElementById('txt_location_name').value = "";
            document.getElementById('btn_save').innerHTML = "Save";
            document.getElementById('lbl_sno').innerHTML = "";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <section class="content-header">
        <h1>
         Add Location <small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Add Location</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Location Details
                </h3>
            </div>
            <div class="box-body">
    <div style="width: 100%; height: 100%;">
        <div id="second_div" style="padding: 20px;">
            <table align="center">
                <tr>
                    <td >
                       <td> <label>
                            Location Name<span style="color:Red;">*</span></label>
                            <td>
                            <td>
                        <input id="txt_location_name" class="form-control" type="text" placeholder="Enter Location Name" />
                        </td>
                        <label id="lbl_sno" style="display: none;">
                        </label>
                    </td>
                </tr>
                <tr>
                <td>
                <br />
                </td>
                </tr>
                <tr>
                    <td>
                        <%--<input id="btn_save" type="button" class="btn btn-primary" value="Save" onclick="for_Main_Saving()" />
                        <input id="btn_clear" type="button" class="btn btn-danger" value="Clear" onclick="clearall()" />--%>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                <td style="padding-left: 420px;">
                </td>
                <td>
                    <div class="input-group">
                        <div class="input-group-addon">
                        <span class="glyphicon glyphicon-ok" id="btn_save1" onclick="for_Main_Saving()"></span><span id="btn_save" onclick="for_Main_Saving()">Save</span>
                    </div>
                    </div>
                    </td>
                    <td style="width:10px;"></td>
                    <td>
                        <div class="input-group">
                        <div class="input-group-close">
                        <span class="glyphicon glyphicon-remove" id='btn_clear1' onclick="clearall()"></span><span id='btn_clear' onclick="clearall()">Clear</span>
                    </div>
                    </div>
                    </td>
                    </tr>
                    </table>
        </div>
        <div>
            <table id="tbl_locations"  class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">
                <thead>
                    <tr style="background:#5aa4d0; color: white; font-weight: bold;">
                        <th scope="col">
                            Sno
                        </th>
                        <th scope="col">
                            Location Name
                        </th>
                        <th scope="col" style="display: none;">
                            sno
                        </th>
                        <th scope="col">
                        </th>
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

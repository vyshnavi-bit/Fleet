<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="EditTyreForm2.aspx.cs" Inherits="EditTyreForm2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/utility.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            getalltyres();
        });

        function getalltyres() {
            var fittingtype = "complete";
            var table = document.getElementById("tyres_table");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var data = { 'op': 'get_remaining_tyres_data', 'fittingtype': fittingtype };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        var main_string = "";
                        var num = 0;
                        for (var i = 0; i < msg.length; i++) {
                            if (msg[i].sno != null) {
                                num++;
                                $("#tyres_table").append('<tr><td>' + num + '</td><td><input name="tyreno" class="form-control" type="text" placeholder="SVDS No" value="' + msg[i].tyresno + '" style="width:130px;"/></td>' +
                                '<td><label>' + msg[i].Brand + '</label></td>' +
                                '<td><label>' + msg[i].Type_of_Tyre + '</label></td>' +
                                '<td style="display:none;"><label>' + msg[i].Tube_Type + '</label></td>' +
                                '<td style="display:none;"><label>' + msg[i].Size + '</label></td>' +
                                '<td><input name="SVDSNO" class="form-control" type="text" placeholder="SVDS No" value="' + msg[i].SVDSNO + '" style="width:100px;"/></td>' +
                                '<td style="display:none;"><input name="min_grove" class="form-control" type="text" placeholder="Min Grove" value="' + msg[i].min_grove + '" /></td>' +
                                '<td style="display:none;"><input name="max_grove" class="form-control" type="text" placeholder="Max Grove" value="' + msg[i].max_grove + '" /></td>' +
                                '<td><input name="grove" class="form-control" type="text" placeholder="Present Grove" value="' + msg[i].grove + '" /></td>' +
                                '<td><input name="currentkms" class="form-control" type="text" placeholder="Current Kms" value="' + msg[i].currentkms + '" style="width:100px;"/></td>' +
                                '<td><input name="flag" class="form-control" type="text" placeholder="Flag" value="' + msg[i].flag + '" /></td>' +
                                '<td style="display:none;"><label name="sub_sno">' + msg[i].sno + '</label>' +
                                '<td><input name="savetyre" type="button" class="btn btn-primary" value="Modify" onclick="save_tyre_click(this)"/></td></tr>');

                            }
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
        function save_tyre_click(thisid) {
            var row = $(thisid).parents('tr');
            var tyreno = $(row).children().find("[name=tyreno]").val();
            var SVDSNO = $(row).children().find("[name=SVDSNO]").val();
            var min_grove = $(row).children().find("[name=min_grove]").val();
            var max_grove = $(row).children().find("[name=max_grove]").val();
            var grove = $(row).children().find("[name=grove]").val();
            var sno = $(row).children().find("[name=sub_sno]").html();
            var currentkms = $(row).children().find("[name=currentkms]").val();
            var flag = $(row).children().find("[name=flag]").val();
            var Data = { 'op': 'save_edit_Tyres_new', 'tyreno': tyreno, 'SVDSNO': SVDSNO, 'min_grove': min_grove, 'max_grove': max_grove, 'grove': grove, 'sno': sno, 'currentkms': currentkms, 'flag': flag };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    getalltyres();
                }
            }
            var e = function (x, h, e) {
            };
            callHandler(Data, s, e);
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

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Edit Tyre<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Edit Tyre</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Edit Tyre Details
                </h3>
            </div>
            <div class="box-body" style="font-size:12px;">
                <div id='vehmaster_fillform'>
                    <div class="table-responsive">
                        <table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info"
                            id="tyres_table">
                            <thead>
                                <tr>
                                    <th>
                                        #
                                    </th>
                                    <th>
                                        Tyre Sno
                                    </th>
                                    <th>
                                        Brand
                                    </th>
                                    <th>
                                        Tyre Type
                                    </th>
                                    <th style="display: none;">
                                        Tube_Type
                                    </th>
                                    <th style="display: none;">
                                        Size
                                    </th>
                                    <th>
                                        SVDS No
                                    </th>
                                    <th style="display:none;">
                                        Min Grove
                                    </th>
                                    <th style="display:none;">
                                        Max Grove
                                    </th>
                                    <th>
                                        Fitting Type
                                    </th>
                                      <th>
                                        Current Kms
                                    </th>
                                       <th>
                                        Flag
                                    </th>
                                    <th style="display:none;">
                                        Sno
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

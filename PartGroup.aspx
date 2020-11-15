<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="PartGroup.aspx.cs" Inherits="PartGroup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<link href="css/formstable.css" rel="stylesheet" type="text/css" />
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

        //------------>Prevent Backspace<--------------------//
        $(document).unbind('keydown').bind('keydown', function (event) {
            var doPrevent = false;
            if (event.keyCode === 8) {
                var d = event.srcElement || event.target;
                if ((d.tagName.toUpperCase() === 'INPUT' && (d.type.toUpperCase() === 'TEXT' || d.type.toUpperCase() === 'PASSWORD'))
            || d.tagName.toUpperCase() === 'TEXTAREA') {
                    doPrevent = d.readOnly || d.disabled;
                } else {
                    doPrevent = true;
                }
            }
            if (doPrevent) {
                event.preventDefault();
            }
        });

        $(function () {
            retriveallpartgroups();
            $(".hiddenrow").hide();
            $('#add_partgroup').click(function () {
                $('#partfillform').css('display', 'block');
                $('#group_showlogs').css('display', 'none');
                $('#div_prtgrptable').css('display', 'none');
                clearalldata();
                $(".hiddenrow").hide();
            });

            $('#close_prtgrp').click(function () {
                $('#partfillform').css('display', 'none');
                $('#group_showlogs').css('display', 'block');
                $('#div_prtgrptable').css('display', 'block');
            });

            $('#save_prtgrp').click(function () {
                var partgroup = document.getElementById('txt_partname').value;
                var partgrpdesc = document.getElementById('txt_groupdesc').value;
                var status = document.getElementById('cmb_routestatus').value;
                var sno = document.getElementById('txt_sno').value;
                var btnval = document.getElementById('save_prtgrp').value;
                var flag = false;
                if (partgroup == "") {
                    $("#lbl_part_Group_error_msg").show();
                    flag = true;
                }
                if (flag) {
                    return;
                }
                var data = { 'op': 'for_save_edit_PArtGroup', 'partgroup': partgroup, 'partgrpdesc': partgrpdesc, 'status': status, 'sno': sno, 'btnval': btnval };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            if (msg == "OK") {
                                alert("New Part Group Successfully Created");
                                clearalldata();
                                retriveallpartgroups();
                                $('#partfillform').css('display', 'none');
                                $('#group_showlogs').css('display', 'block');
                                $('#div_prtgrptable').css('display', 'block');
                                $('#save_prtgrp').val("Save");
                            }
                            else if (msg == "UPDATE") {
                                alert("PartGroup Successfully Modified");
                                clearalldata();
                                retriveallpartgroups();
                                $('#partfillform').css('display', 'none');
                                $('#group_showlogs').css('display', 'block');
                                $('#div_prtgrptable').css('display', 'block');
                                $('#save_prtgrp').val("Save");
                            }
                            else {
                                alert(msg);
                            }
                        }
                    }
                    else {
                    }
                };
                var e = function (x, h, e) {
                }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                callHandler(data, s, e);
            });
        });
        function clearalldata() {
            document.getElementById('txt_partname').value = "";
            document.getElementById('txt_groupdesc').value = "";
            document.getElementById('cmb_routestatus').value = "1";
            document.getElementById('txt_sno').value = "";
            document.getElementById('save_prtgrp').value = "Save";
            $("#lbl_part_Group_error_msg").hide();
        }
        function retriveallpartgroups() {
            var table = document.getElementById("tbl_partgroup");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var data = { 'op': 'get_all_partgroups' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filldata(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function filldata(results) {
            var table = document.getElementById("tbl_partgroup");
            for (var i = 0; i < results.length; i++) {
                if (results[i].pg_name != null) {
                    var pg_name = results[i].pg_name;
                    var pg_desc = results[i].pg_desc;
                    var statuscode = results[i].status;
                    var short_name = results[i].short_name;
                    var status = "";
                    if (statuscode == "1") {
                        status = "Enabled";
                    }
                    else {
                        status = "Disabled";
                    }
                    var sno = results[i].sno;
                    var tablerowcnt = document.getElementById("tbl_partgroup").rows.length;
                    $('#tbl_partgroup').append('<tr><th scope="row">' + pg_name + '</th><td data-title="Description">' + pg_desc + '</td><td data-title="Status" >' + status + '</td><td data-title="sno" style="display:none;">' + sno + '</td><td><input type="button" name="Update" value ="Modify" onclick="updateclick(this);"/></td></tr>');
                }
            }
        }
        function updateclick(thisid) {
            var row = $(thisid).parents('tr');
            var sno = row[0].cells[3].innerHTML;
            var pg_name = row[0].cells[0].innerHTML;
            var pg_desc = row[0].cells[1].innerHTML;
            var statuscode = row[0].cells[2].innerHTML;
            var status = "";
            if (statuscode == "Enabled") {
                status = "1";
            }
            else {
                status = "0";
            }
            $(".hiddenrow").show();
            document.getElementById('txt_partname').value = pg_name;
            document.getElementById('txt_groupdesc').value = pg_desc;
            document.getElementById('cmb_routestatus').value = status;
            document.getElementById('txt_sno').value = sno;
            $('#partfillform').css('display', 'block');
            $('#group_showlogs').css('display', 'none');
            $('#div_prtgrptable').css('display', 'none');
            $('#save_prtgrp').val("Modify");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="group_showlogs" style="text-align: center;">
        <input id="add_partgroup" type="button" name="submit" value='Add Part Group' />
    </div>
    <div id='partfillform' style="display: none;" class='CSSTableGenerator'>
        <section>
            <table cellpadding="1px">
                <tr>
                    <th colspan="2" align="center">
                        <h3>
                            Add PartGroup</h3>
                    </th>
                </tr>
                <tr>
                    <td>
                        Part Group Name<span style="color: red;">*</span>
                    </td>
                    <td>
                        <input id="txt_partname" type="text" name="vendorcode" placeholder="Part Group Name"><label
                            id="lbl_part_Group_error_msg" class="errormessage">* Please Enter Part Group Name</label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Part Group Description
                    </td>
                    <td>
                        <input type="text" id="txt_groupdesc" maxlength="45" name="vendorcode" placeholder="Description">
                    </td>
                </tr>
                <tr style="display: none;" class="hiddenrow">
                    <td>
                        Status
                    </td>
                    <td>
                        <select id="cmb_routestatus" class="allinputs">
                            <option value="1">Enabled</option>
                            <option value="0">Disabled</option>
                        </select>
                    </td>
                </tr>
                <tr style="display: none;">
                    <td>
                        Sno
                    </td>
                    <td>
                        <input type="text" id="txt_sno" maxlength="45" name="vendorcode" placeholder="sno">
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <input id='save_prtgrp' type="button" name="submit" value='Save' />
                        <input id='close_prtgrp' type="button" name="Close" value='Close' />
                    </td>
                </tr>
            </table>
        </section>
    </div>
    <div >
        <div id="div_prtgrptable" class='divcontainer'>
            <table id="tbl_partgroup" class="responsive-table">
                <thead>
                    <tr>
                        <th scope="col">
                            PartGroup Name
                        </th>
                        <th scope="col">
                            Description
                        </th>
                        <th scope="col">
                            Status
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
</asp:Content>

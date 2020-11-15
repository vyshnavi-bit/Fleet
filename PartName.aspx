<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="PartName.aspx.cs" Inherits="PartName" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<link href="css/formstable.css" rel="stylesheet" type="text/css" />
    <link href="css/custom.css" rel="stylesheet" type="text/css" />--%>
    <style type="text/css">
        .dispalynone
        {
            display: none;
        }
    </style>
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
            getallpartgroups();
            retrivedata();
            $('#add_partnme').click(function () {
                $('#partnme_fillform').css('display', 'block');
                $('#prtnme_showlogs').css('display', 'none');
                $('#div_prtnametable').css('display', 'none');
                $("#lbl_partgrp_error_msg").hide();
                $("#lbl_part_Name_error_msg").hide();
                clearall();
            });

            $('#close_prtname').click(function () {
                $('#partnme_fillform').css('display', 'none');
                $('#prtnme_showlogs').css('display', 'block');
                $('#div_prtnametable').css('display', 'block');
                $("#lbl_partgrp_error_msg").hide();
                $("#lbl_part_Name_error_msg").hide();
            });
        });
        function getallpartgroups() {
            var minimaster = "PartGroup";
            var data = { 'op': 'get_Mini_Master_data', 'minimaster': minimaster };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillpartgroupdata(msg);
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
        function fillpartgroupdata(msg) {
            var partgroup = document.getElementById('slct_partgroup');
            var length = partgroup.options.length;
            document.getElementById('slct_partgroup').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Part Group";
            opt.value = "Select Part Group";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            partgroup.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].mm_name != null && msg[i].mm_status != "0") {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].mm_name;
                    option.value = msg[i].sno;
                    partgroup.appendChild(option);
                }
            }
        }
        function savepartname() {
            var partgroup_sno = document.getElementById('slct_partgroup').value;
            var partname = document.getElementById('txt_partname').value;
            var partdesc = document.getElementById('txt_desc').value;
            var sno = document.getElementById('txt_sno').value;
            var btnval = document.getElementById('save_prtname').value;
            var flag = false;
            if (partgroup_sno == "Select Part Group") {
                $("#lbl_partgrp_error_msg").show();
                flag = true;
            }
            if (partname == "") {
                $("#lbl_part_Name_error_msg").show();
                flag = true;
            }

            if (flag) {
                return;
            }
            var data = { 'op': 'save_partname', 'partgroup_sno': partgroup_sno, 'partname': partname, 'partdesc': partdesc, 'sno': sno
            , 'btnval': btnval
            };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        if (msg == "OK") {
                            alert("New Part Name Successfully Added");
                            $('#partnme_fillform').css('display', 'none');
                            $('#prtnme_showlogs').css('display', 'block');
                            $('#div_prtnametable').css('display', 'block');
                            $('#save_prtname').val("Save");
                            clearall();
                            retrivedata();
                            $("#lbl_partgrp_error_msg").hide();
                            $("#lbl_part_Name_error_msg").hide();
                        }
                        else if (msg == "UPDATE") {
                            alert("Part Name Successfully Updated");
                            $('#partnme_fillform').css('display', 'none');
                            $('#prtnme_showlogs').css('display', 'block');
                            $('#div_prtnametable').css('display', 'block');
                            $('#save_prtname').val("Save");
                            retrivedata();
                            clearall();
                            $("#lbl_partgrp_error_msg").hide();
                            $("#lbl_part_Name_error_msg").hide();
                        }
                        else {
                            alert(msg);
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
        function retrivedata() {
            var table = document.getElementById("tbl_partname");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var data = { 'op': 'get_all_partname_data' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillpartnamedata(msg);
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
        function fillpartnamedata(results) {
            var table = document.getElementById("tbl_partname");
            for (var i = 0; i < results.length; i++) {
                if (results[i].pn_name != null) {
                    var pn_name = results[i].pn_name;
                    var mm_name = results[i].mm_name;
                    var pn_desc = results[i].pn_desc;
                    var sno = results[i].sno;
                    var tablerowcnt = document.getElementById("tbl_partname").rows.length;
                    $('#tbl_partname').append('<tr><th scope="row">' + pn_name + '</th><td data-title="Part Group">' + mm_name + '</td><td data-title="Description" >' + pn_desc + '</td><td data-title="sno" style="display:none;">' + sno + '</td><td><input type="button" class="btn btn-primary" name="Update" value ="Modify" onclick="updateclick(this);"/></td></tr>');
                }
            }
        }

        function updateclick(thisid) {
            var row = $(thisid).parents('tr');
            var sno = row[0].cells[3].innerHTML;
            var pn_name = row[0].cells[0].innerHTML;
            var mm_name = row[0].cells[1].innerHTML;
            var pn_desc = row[0].cells[2].innerHTML;
            document.getElementById('txt_partname').value = pn_name;
            $("select#slct_partgroup option").each(function () { this.selected = (this.text == mm_name); });
            document.getElementById('txt_desc').value = pn_desc;
            document.getElementById('txt_sno').value = sno;
            $('#partnme_fillform').css('display', 'block');
            $('#prtnme_showlogs').css('display', 'none');
            $('#div_prtnametable').css('display', 'none');
            $('#save_prtname').val("Modify");
        }
        function clearall() {
            document.getElementById('slct_partgroup').value = "Select Part Group";
            document.getElementById('txt_partname').value = "";
            document.getElementById('txt_desc').value = "";
            document.getElementById('txt_sno').value = "";
            document.getElementById('save_prtname').value = "Save";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="prtnme_showlogs" style="text-align: center;">
        <input id="add_partnme" type="button" class="btn btn-primary" name="submit" value='Add Part Name' />
    </div>
    <div id='partnme_fillform' style="display: none;" class='CSSTableGenerator'>
        <section>
            <table cellpadding="1px">
                <tr>
                    <th colspan="2" align="center">
                        <h3>
                            Add PartName</h3>
                    </th>
                </tr>
                <tr>
                    <td>
                        Part Group<span style="color: red;">*</span>
                    </td>
                    <td>
                        <select id="slct_partgroup">
                        </select>
                        <label id="lbl_partgrp_error_msg" class="errormessage">
                            * Please Select Part Group</label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Part Name<span style="color: red;">*</span>
                    </td>
                    <td>
                        <input id="txt_partname" type="text" name="vendorcode" placeholder="Part Name"><label
                            id="lbl_part_Name_error_msg" class="errormessage">* Please Enter Part Name</label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Description
                    </td>
                    <td>
                        <input type="text" id="txt_desc" maxlength="45" name="vendorcode" placeholder="Description">
                    </td>
                </tr>
                <%-- <tr style="display:none;" class="hiddenrow"> <td>Status</td><td>
                <select id="cmb_routestatus" class="allinputs">
                <option value="1">Enabled</option>
                <option value="0">Disabled</option>
                </select>
                </td></tr>--%>
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
                        <input id='save_prtname' type="button" class="btn btn-primary" name="submit" value='Save'
                            onclick="savepartname()" />
                        <input id='close_prtname' type="button" class="btn btn-primary" name="Close" value='Close' />
                    </td>
                </tr>
            </table>
        </section>
    </div>
    <div >
        <div id="div_prtnametable" class='divcontainer'>
            <table id="tbl_partname" class="responsive-table">
                <thead>
                    <tr>
                        <th scope="col">
                            Part Name
                        </th>
                        <th scope="col">
                            Part Group
                        </th>
                        <th scope="col">
                            Description
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

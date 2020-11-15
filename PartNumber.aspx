<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="PartNumber.aspx.cs" Inherits="PartNumber" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
            getallRakenames();
            retrivedata();
            $('.hiddenrow').hide();
            $('#add_partnumber').click(function () {
                $('.hiddenrow').hide();
                $('#partnme_fillform').css('display', 'block');
                $('#prtnumber_showlogs').css('display', 'none');
                $('#div_prt_num_table').css('display', 'none');
                $("#lbl_partgrp_error_msg").hide();
                $("#lbl_part_Name_error_msg").hide();
                $("#lbl_part_Number_error_msg").hide();
                $("#lbl_rake_error_msg").hide();
                $("#lbl_availstock_error_msg").hide();
                $("#lbl_unitcost_error_msg").hide();
                $("#lbl_MinStock_error_msg").hide();
                clearall();
            });

            $('#close_prtumber').click(function () {
                $('.hiddenrow').hide();
                $('#partnme_fillform').css('display', 'none');
                $('#prtnumber_showlogs').css('display', 'block');
                $('#div_prt_num_table').css('display', 'block');
                $("#lbl_partgrp_error_msg").hide();
                $("#lbl_part_Name_error_msg").hide();
                $("#lbl_part_Number_error_msg").hide();
                $("#lbl_rake_error_msg").hide();
                $("#lbl_unitcost_error_msg").hide();
                $("#lbl_MinStock_error_msg").hide();
            });
        });

        //--------->Getting Part Groups<--------------//
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
        function fillpartgroupdata(partgroupmsg) {
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
            for (var i = 0; i < partgroupmsg.length; i++) {
                if (partgroupmsg[i].mm_name != null && partgroupmsg[i].mm_status != "0") {
                    var option = document.createElement('option');
                    option.innerHTML = partgroupmsg[i].mm_name;
                    option.value = partgroupmsg[i].sno;
                    partgroup.appendChild(option);
                }
            }
        }
        //---------> End OF Getting Part Groups<--------------//
        //--------->Getting Part Names<--------------//
        function getallpartnames() {
            var partgroup_sno = document.getElementById('slct_partgroup').value;
            var data = { 'op': 'get_Part_NAme_data', 'partgroup_sno': partgroup_sno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillpartname(msg);
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
        function fillpartname(partnamemsg) {
            var partname = document.getElementById('slct_partname');
            var length = partname.options.length;
            document.getElementById('slct_partname').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Part Name";
            opt.value = "Select Part Name";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            partname.appendChild(opt);
            for (var i = 0; i < partnamemsg.length; i++) {
                if (partnamemsg[i].pn_name != null) {
                    var option = document.createElement('option');
                    option.innerHTML = partnamemsg[i].pn_name;
                    option.value = partnamemsg[i].sno;
                    partname.appendChild(option);
                }
            }
        }
        //--------->Getting Rake Names<--------------//
        function getallRakenames() {
            var minimaster = "RakeInfo";
            var data = { 'op': 'get_Mini_Master_data', 'minimaster': minimaster };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillRakedata(msg);
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
        function fillRakedata(rakedatamsg) {
            var rakename = document.getElementById('slct_rake');
            var length = rakename.options.length;
            document.getElementById('slct_rake').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Rake";
            opt.value = "Select Rake";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            rakename.appendChild(opt);
            for (var i = 0; i < rakedatamsg.length; i++) {
                if (rakedatamsg[i].mm_name != null && rakedatamsg[i].mm_status != "0") {
                    var option = document.createElement('option');
                    option.innerHTML = rakedatamsg[i].mm_name;
                    option.value = rakedatamsg[i].sno;
                    rakename.appendChild(option);
                }
            }
        }
        //--------->End OF Getting Rake Names<--------------//
        //--------->Save PArt Number<--------------//
        function savepartnumber() {
            var partgroup_sno = document.getElementById('slct_partgroup').value;
            var partname_sno = document.getElementById('slct_partname').value;
            var partnumbername = document.getElementById('txt_part_number').value;
            var desc = document.getElementById('txt_desc').value;
            var rake = document.getElementById('slct_rake').value;
            var minstock = document.getElementById('txt_minstock').value;
            var status = document.getElementById('cmb_status').value;
            var sno = document.getElementById('txt_sno').value;
            var btnval = document.getElementById('save_prtnumber').value;
            var availstock = document.getElementById('txt_availstock').value;
            var unitcost = document.getElementById('txt_cost').value;
            var flag = false;
            if (partgroup_sno == "Select Part Group") {
                $("#lbl_partgrp_error_msg").show();
                flag = true;
            }
            if (partname_sno == "Select Part Name") {
                $("#lbl_part_Name_error_msg").show();
                flag = true;
            }
            if (partnumbername == "") {
                $("#lbl_part_Number_error_msg").show();
                flag = true;
            }
            if (rake == "Select Rake") {
                $("#lbl_rake_error_msg").show();
                flag = true;
            }
            if (minstock == "") {
                $("#lbl_MinStock_error_msg").show();
                flag = true;
            }
            if (availstock == "") {
                $("#lbl_availstock_error_msg").show();
                flag = true;
            }
            if (unitcost == "") {
                $("#lbl_unitcost_error_msg").show();
                flag = true;
            }
            if (flag) {
                return;
            }
            var data = { 'op': 'save_part_number', 'partgroup_sno': partgroup_sno, 'partname_sno': partname_sno, 'partnumbername': partnumbername, 'desc': desc
            , 'rake': rake, 'minstock': minstock, 'status': status, 'sno': sno, 'btnval': btnval, 'availstock': availstock, 'unitcost': unitcost
            };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        if (msg == "OK") {
                            alert("New Part Number Name Successfully Added");
                            $('#partnme_fillform').css('display', 'none');
                            $('#prtnumber_showlogs').css('display', 'block');
                            $('#div_prt_num_table').css('display', 'block');
                            $('#save_prtnumber').val("Save");
                            clearall();
                            retrivedata();
                            $("#lbl_partgrp_error_msg").hide();
                            $("#lbl_part_Name_error_msg").hide();
                            $("#lbl_part_Number_error_msg").hide();
                            $("#lbl_rake_error_msg").hide();
                            $("#lbl_availstock_error_msg").hide();
                            $("#lbl_MinStock_error_msg").hide();
                            $("#lbl_unitcost_error_msg").hide();

                        }
                        else if (msg == "UPDATE") {
                            alert("Part Number Successfully Updated");
                            $('#partnme_fillform').css('display', 'none');
                            $('#prtnumber_showlogs').css('display', 'block');
                            $('#div_prt_num_table').css('display', 'block');
                            $('#save_prtnumber').val("Save");
                            retrivedata();
                            clearall();
                            $("#lbl_partgrp_error_msg").hide();
                            $("#lbl_part_Name_error_msg").hide();
                            $("#lbl_part_Number_error_msg").hide();
                            $("#lbl_rake_error_msg").hide();
                            $("#lbl_MinStock_error_msg").hide();
                            $("#lbl_availstock_error_msg").hide();
                            $("#lbl_unitcost_error_msg").hide();
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
        //--------->End OF Save PArt Number<--------------//
        //--------->Retriving PArt Number<--------------//
        function retrivedata() {
            var table = document.getElementById("tbl_partnumber");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var data = { 'op': 'get_all_part_number_data' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillpartnumberdata(msg);
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
        function fillpartnumberdata(results) {
            var table = document.getElementById("tbl_partnumber");
            for (var i = 0; i < results.length; i++) {
                if (results[i].pnum_Name != null) {
                    var partgroup = results[i].partgroup;
                    var pn_name = results[i].pn_name;
                    var pnum_Name = results[i].pnum_Name;
                    var pnum_desc = results[i].pnum_desc;
                    var rake = results[i].rake;
                    var minimum_stock = results[i].minimum_stock;
                    var availble_qty = results[i].availble_qty;
                    var unitcost = results[i].unitcost;
                    var statuscode = results[i].status;
                    var status = "";
                    if (statuscode == "1") {
                        status = "Enabled";
                    }
                    else {
                        status = "Disabled";
                    }
                    var sno = results[i].sno;
                    var tablerowcnt = document.getElementById("tbl_partnumber").rows.length;
                    $('#tbl_partnumber').append('<tr><th scope="row">' + pnum_Name + '</th><td data-title="Part Group">' + partgroup + '</td><td data-title="Part Name" >' + pn_name + '</td><td data-title="Unit Cost" >' + unitcost + '</td><td data-title="Description" >' + pnum_desc + '</td><td data-title="Rake" >' + rake + '</td><td data-title="Min Stock" >' + minimum_stock + '</td><td data-title="Stock Available" >' + availble_qty + '</td><td data-title="Status" >' + status + '</td><td data-title="sno" style="display:none;">' + sno + '</td><td><input type="button" class="btn btn-primary" name="Update" value ="Modify" onclick="updateclick(this);"/></td></tr>');
                }
            }
        }

        function updateclick(thisid) {
            var row = $(thisid).parents('tr');
            var pnum_Name = row[0].cells[0].innerHTML;
            var partgroup = row[0].cells[1].innerHTML;
            var pn_name = row[0].cells[2].innerHTML;
            var unitcost = row[0].cells[3].innerHTML;
            var pnum_desc = row[0].cells[4].innerHTML;
            var rake = row[0].cells[5].innerHTML;
            var minimum_stock = row[0].cells[6].innerHTML;
            var availble_qty = row[0].cells[7].innerHTML;
            var statuscode = row[0].cells[8].innerHTML;
            var sno = row[0].cells[9].innerHTML;
            var status = "";
            if (statuscode == "Enabled") {
                status = "1";
            }
            else {
                status = "0";
            }
            $("select#slct_partgroup option").each(function () { this.selected = (this.text == partgroup); });
            var partgroup_sno = document.getElementById('slct_partgroup').value;
            var data = { 'op': 'get_Part_NAme_data', 'partgroup_sno': partgroup_sno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        var partname = document.getElementById('slct_partname');
                        var length = partname.options.length;
                        document.getElementById('slct_partname').options.length = null;
                        var opt = document.createElement('option');
                        opt.innerHTML = "Select Part Name";
                        opt.value = "Select Part Name";
                        opt.setAttribute("selected", "selected");
                        opt.setAttribute("disabled", "disabled");
                        opt.setAttribute("class", "dispalynone");
                        partname.appendChild(opt);
                        for (var i = 0; i < msg.length; i++) {
                            if (msg[i].pn_name != null) {
                                var option = document.createElement('option');
                                option.innerHTML = msg[i].pn_name;
                                option.value = msg[i].sno;
                                partname.appendChild(option);
                            }
                        }
                        $("select#slct_partname option").each(function () { this.selected = (this.text == pn_name); });
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
            $("select#slct_rake option").each(function () { this.selected = (this.text == rake); });
            document.getElementById('txt_part_number').value = pnum_Name;
            document.getElementById('txt_desc').value = pnum_desc;
            document.getElementById('txt_minstock').value = minimum_stock;
            document.getElementById('cmb_status').value = status;
            document.getElementById('txt_sno').value = sno;
            document.getElementById('txt_availstock').value = availble_qty;
            $('.hiddenrow').show();
            $('#partnme_fillform').css('display', 'block');
            $('#prtnumber_showlogs').css('display', 'none');
            $('#div_prt_num_table').css('display', 'none');
            $('#save_prtnumber').val("Modify");
        }
        function clearall() {
            document.getElementById('slct_partgroup').value = "Select Part Group";
            document.getElementById('slct_partname').value = "Select Part Name";
            document.getElementById('txt_part_number').value = "";
            document.getElementById('txt_desc').value = "";
            document.getElementById('slct_rake').value = "Select Rake";
            document.getElementById('txt_minstock').value = "";
            document.getElementById('cmb_status').value = "1";
            document.getElementById('txt_sno').value = "";
            document.getElementById('save_prtnumber').value = "Save";
            document.getElementById('txt_availstock').value = "";
            document.getElementById('txt_cost').value = "";
        }
        //--------->End of Retriving PArt Number<--------------//
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="prtnumber_showlogs" style="text-align: center;">
        <input id="add_partnumber" type="button" class="btn btn-primary" name="submit" value='Add Part Number' />
    </div>
    <div id='partnme_fillform' style="display: none;" class='CSSTableGenerator'>
        <section>
            <table cellpadding="1px">
                <tr>
                    <th colspan="2" align="center">
                        <h3>
                            Add PartNumber</h3>
                    </th>
                </tr>
                <tr>
                    <td>
                        Part Group<span style="color: red;">*</span>
                    </td>
                    <td>
                        <select id="slct_partgroup" onchange="getallpartnames()">
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
                        <select id="slct_partname">
                        </select><label id="lbl_part_Name_error_msg" class="errormessage">* Please Select Part
                            Name</label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Part Number Name<span style="color: red;">*</span>
                    </td>
                    <td>
                        <input id="txt_part_number" type="text" name="vendorcode" placeholder="Part Number Name"><label
                            id="lbl_part_Number_error_msg" class="errormessage">* Please Enter Part Number Name</label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Unit Cost
                    </td>
                    <td>
                        <input type="text" id="txt_cost" maxlength="45" name="vendorcode" placeholder="Unit Cost">
                    </td>
                </tr>
                <label id="lbl_unitcost_error_msg" class="errormessage">
                    * Please Select Rake</label>
                <tr>
                    <td>
                        Description
                    </td>
                    <td>
                        <input type="text" id="txt_desc" maxlength="45" name="vendorcode" placeholder="Description">
                    </td>
                </tr>
                <tr>
                    <td>
                        Rake<span style="color: red;">*</span>
                    </td>
                    <td>
                        <select id="slct_rake">
                        </select><label id="lbl_rake_error_msg" class="errormessage">* Please Select Rake</label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Minimum Stock<span style="color: red;">*</span>
                    </td>
                    <td>
                        <input id="txt_minstock" type="text" name="vendorcode" placeholder="Minimun Stock"><label
                            id="lbl_MinStock_error_msg" class="errormessage">* Please Enter Minimun Stock</label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Available Stock<span style="color: red;">*</span>
                    </td>
                    <td>
                        <input id="txt_availstock" type="text" name="vendorcode" placeholder="Available Stock"><label
                            id="lbl_availstock_error_msg" class="errormessage">* Please Enter Available Stock
                            (If not Eneter "0")</label>
                    </td>
                </tr>
                <tr style="display: none;" class="hiddenrow">
                    <td>
                        Status
                    </td>
                    <td>
                        <select id="cmb_status" class="allinputs">
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
                        <input id='save_prtnumber' type="button" class="btn btn-primary" name="submit" value='Save'
                            onclick="savepartnumber()" />
                        <input id='close_prtumber' type="button" class="btn btn-primary" name="Close" value='Close' />
                    </td>
                </tr>
            </table>
        </section>
    </div>
    <div id="div_prt_num_table" class='divcontainer'>
        <table id="tbl_partnumber" class="responsive-table">
            <thead>
                <tr>
                    <th scope="col">
                        Part Number
                    </th>
                    <th scope="col">
                        PartGroup
                    </th>
                    <th scope="col">
                        Part Name
                    </th>
                    <th scope="col">
                        Unit Cost
                    </th>
                    <th scope="col">
                        Description
                    </th>
                    <th scope="col">
                        Rake
                    </th>
                    <th scope="col">
                        Min Stock
                    </th>
                    <th scope="col">
                        Stock Available
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
</asp:Content>

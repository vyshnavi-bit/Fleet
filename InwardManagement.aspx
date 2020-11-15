<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="InwardManagement.aspx.cs" Inherits="InwardManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    <link href="opcss/bootextract.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="dist/css/bootstrap.css" />
    <style type="text/css">
        .row + .row > *
        {
            padding: 0px 0 0 40px;
        }
    </style>
    <script type="text/javascript">
        //Function for only no
        $(document).ready(function () {
            $("#txt_quantity,#txt_perunitrs").keydown(function (event) {
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
        var belowtable_rowid;
        $(function () {
            $('#btn_saveitems').click(function (e) {
                e.preventDefault();
                var partgroupSno = document.getElementById('slct_partgroup').value;
                var partnameSno = document.getElementById('slct_prtnme').value;
                var partnoSno = document.getElementById('slct_prtno').value;
                var d = document.getElementById('slct_partgroup');
                var partgroup = d.options[d.selectedIndex].text;
                var e = document.getElementById('slct_prtnme');
                var partname = e.options[e.selectedIndex].text;
                var f = document.getElementById('slct_prtno');
                var partno = f.options[f.selectedIndex].text;
                var rake = document.getElementById('lbl_rake').innerHTML;
                // var remarks = document.getElementById('txt_fillremarks').value;
                //var partserial = document.getElementById('txt_prtserial').value;
                var quantity = document.getElementById('txt_quantity').value;
                var perunitcost = document.getElementById('txt_perunitrs').value;
                var totalcost = document.getElementById('lbl_totalcost').innerHTML;
                var availstock = document.getElementById('lbl_stkavail').innerHTML;
                var btnval = document.getElementById('btn_saveitems').value;
                var finavalstock = parseFloat(availstock) + parseFloat(quantity);
                var flag = false;
                if (btnval == "Modify") {
                    $(belowtable_rowid).parents("tr:first").remove();
                    var row = $(belowtable_rowid).parents('tr');
                    var total = document.getElementById('lbl_maintotal').innerHTML;
                    var ttl = 0;
                    ttl = row[0].children[8].innerText;
                    total = parseFloat(total) - parseFloat(ttl);
                    document.getElementById('lbl_maintotal').innerHTML = total;
                }
                $('#itemstable> tbody > tr').each(function () {
                    var prtgpno = $(this).find("td:eq(4)").text();
                    if (prtgpno == partnoSno && btnval == "ADD") {
                        alert("This Part Number Already Added");
                        flag = true;
                        return false;
                    }
                });
                if (partgroup == "Select Part Group") {
                    $('#lbl_prtgrp_error_msg').show();
                    flag = true;
                }
                if (partname == "Select Part Name") {
                    $('#lbl_prtnme_error_msg').show();
                    flag = true;
                }
                if (partno == "Select Part Number") {
                    $('#lbl_prtnumb_error_msg').show();
                    flag = true;
                }
                if (quantity == "") {
                    $('#lbl_prtqty_error_msg').show();
                    flag = true;
                }
                if (perunitcost == "") {
                    $('#lbl_unitrs_error_msg').show();
                    flag = true;
                }
                if (flag == true) {
                    return;
                }
                //get the footable object
                var footable = $('table').data('footable');
                //build up the row we are wanting to add
                //var newRow = '<tr><td>Isidra</td><td><a href="#">Boudreaux</a></td><td>Traffic Court Referee</td><td data-value="78025368997">22 Jun 1972</td><td data-value="1"><span class="status-metro status-active" title="Active">Active</span></td><td><a class="row-delete" href="#"><span class="glyphicon glyphicon-remove"></span></a></td></tr>';
                $('#itemstable').append('<tr><td data-title="Part Group"><span id="prtgrp" >' + partgroup + '</span></td><td  style="display:none;">' + partgroupSno + '</td><td data-title="Part Name">' + partname + '</td><td style="display:none;">' + partnameSno + '</td><th scope="row">' + partno + '</th><td style="display:none;">' + partnoSno + '</td><td data-title="Quantity">' + quantity + '</td><td data-title="Unit Cost">' + perunitcost + '</td><td data-title="Total Cost">' + totalcost + '</td><td data-title="Rake">' + rake + '</td><td style="display:none;">' + finavalstock + '</td><td><input type="button" class="btn btn-primary" value="Modify" onclick="updatethis(this)"/></td><td><a class="row-delete"><span onclick="removerow(this)" class="glyphicon glyphicon-remove"></span></a></td></tr>');
                //var newRow = '<tr><td><span id="prtgrp">' + partgroup + '</span></td><td>' + partgroupSno + '</td><td>' + partname + '</td><td>' + partnameSno + '</td><td>' + partno + '</td><td>' + partnoSno + '</td><td>' + partserial + '</td><td>' + quantity + '</td><td>' + perunitcost + '</td><td>' + totalcost + '</td><td>' + remarks + '</td><td>' + rake + '</td><td>' + finavalstock + '</td><td><input type="button" class="btn btn-primary" value="Modify" onclick="updatethis(this)"/></td><td><a class="row-delete" href="#"><span class="glyphicon glyphicon-remove"></span></a></td></tr>';
                //add it
                //footable.appendRow(newRow);
                var total = 0;
                $('#itemstable> tbody > tr').each(function () {
                    var ttl = 0;
                    ttl = $(this).find("td:eq(7)").text();
                    total = parseFloat(ttl) + parseFloat(total);
                });
                document.getElementById('lbl_maintotal').innerHTML = total;
                forclearbelow();
            });
        });
    </script>
    <script type="text/javascript">
        function removerow(thisid) {
            var row = $(thisid).parents('tr');
            var total = document.getElementById('lbl_maintotal').innerHTML;
            var ttl = 0;
            ttl = row[0].children[8].innerText;
            total = parseFloat(total) - parseFloat(ttl);
            document.getElementById('lbl_maintotal').innerHTML = total;
            $(thisid).parent().parent().parent().remove();
        }
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

        $(function () {
            get_vendor_details();
            getallpartgroups();
            getinward_nuber();
        });

        function get_inward_no() {
            var data = { 'op': 'get_inward_no' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillvendordetails(msg);
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

        function get_vendor_details() {
            var data = { 'op': 'get_vendor_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillvendordetails(msg);
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
        function fillvendordetails(vendormsg) {
            var vendoravailableTags = [];
            var vendoravailableTags2 = [];
            var partgroup = document.getElementById('slct_vendorcode');
            var length = partgroup.options.length;
            document.getElementById('slct_vendorcode').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Vendor";
            opt.value = "Select Vendor";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            partgroup.appendChild(opt);
            for (var i = 0; i < vendormsg.length; i++) {
                if (vendormsg[i].vendorname != null) {
                    var option = document.createElement('option');
                    option.innerHTML = vendormsg[i].vendorname;
                    option.value = vendormsg[i].sno;
                    partgroup.appendChild(option);
                }
            }
        }

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
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillpartgroupdata(partgroupmsg) {
            var partgroupTags = [];
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
            //var partgroup_sno = document.getElementById('lbl_partgroupsno').innerHTML;
            var d = document.getElementById('slct_partgroup');
            var partgroup_sno = d.options[d.selectedIndex].value;
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
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillpartname(partnamemsg) {
            var partnameTags = [];
            var partgroup = document.getElementById('slct_prtnme');
            var length = partgroup.options.length;
            document.getElementById('slct_prtnme').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Part Name";
            opt.value = "Select Part Name";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            partgroup.appendChild(opt);
            for (var i = 0; i < partnamemsg.length; i++) {
                if (partnamemsg[i].pn_name != null) {
                    var option = document.createElement('option');
                    option.innerHTML = partnamemsg[i].pn_name;
                    option.value = partnamemsg[i].sno;
                    partgroup.appendChild(option);
                }
            }
        }
        //--------->End OF Getting Part Names<--------------//
        function getallpartnumbers() {
            var d = document.getElementById('slct_partgroup');
            var partgroup_sno = d.options[d.selectedIndex].value;
            var f = document.getElementById('slct_prtnme');
            var partname_sno = f.options[f.selectedIndex].value;
            var data = { 'op': 'get_Part_number_data', 'partgroup_sno': partgroup_sno, 'partname_sno': partname_sno };
            var s = function (msg) {
                if (msg) {

                    fillpartnumberdata(msg);

                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillpartnumberdata(prtnomsg) {
            var partnoTags = [];
            var partgroup = document.getElementById('slct_prtno');
            var length = partgroup.options.length;
            document.getElementById('slct_prtno').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Part Number";
            opt.value = "Select Part Number";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            partgroup.appendChild(opt);
            for (var i = 0; i < prtnomsg.length; i++) {
                if (prtnomsg[i].pnum_Name != null) {
                    var option = document.createElement('option');
                    option.innerHTML = prtnomsg[i].pnum_Name;
                    option.value = prtnomsg[i].sno;
                    partgroup.appendChild(option);
                }
            }
        }

        function getrackandstock() {
            var f = document.getElementById('slct_prtno');
            var partno_sno = f.options[f.selectedIndex].value;
            var data = { 'op': 'get_rackansstock_data', 'partno_sno': partno_sno };
            var s = function (msg) {
                if (msg) {
                    document.getElementById('lbl_rake').innerHTML = msg[0].rack;
                    document.getElementById('lbl_stkavail').innerHTML = msg[0].availble_qty;
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function updatethis(thisid) {
            belowtable_rowid = thisid;
            var row = $(thisid).parents('tr');
            var prtgrp = $(thisid).parent().parent().children().children('#prtgrp').html();
            var partname = row[0].cells[1].innerHTML;
            var partno = row[0].cells[2].innerHTML;
            var quantity = row[0].cells[6].innerHTML;
            var perunitcost = row[0].cells[7].innerHTML;
            var totalcost = row[0].cells[8].innerHTML;
            var rake = row[0].cells[9].innerHTML;
            var stockavail = row[0].cells[10].innerHTML;
            var prevstock = parseFloat(stockavail) - parseFloat(quantity);
            $("select#slct_partgroup option").each(function () { this.selected = (this.text == prtgrp); });
            document.getElementById('lbl_rake').innerHTML = rake;
            document.getElementById('txt_quantity').value = quantity;
            document.getElementById('txt_perunitrs').value = perunitcost;
            document.getElementById('lbl_totalcost').innerHTML = totalcost;
            document.getElementById('btn_saveitems').value = "Modify";
            document.getElementById('lbl_stkavail').innerHTML = prevstock;
            var d = document.getElementById('slct_partgroup');
            var partgroup_sno = d.options[d.selectedIndex].value;
            var data = { 'op': 'get_Part_NAme_data', 'partgroup_sno': partgroup_sno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        var partgroup = document.getElementById('slct_prtnme');
                        var length = partgroup.options.length;
                        document.getElementById('slct_prtnme').options.length = null;
                        var opt = document.createElement('option');
                        opt.innerHTML = "Select Part Name";
                        opt.value = "Select Part Name";
                        opt.setAttribute("selected", "selected");
                        opt.setAttribute("disabled", "disabled");
                        opt.setAttribute("class", "dispalynone");
                        partgroup.appendChild(opt);
                        for (var i = 0; i < msg.length; i++) {
                            if (msg[i].pn_name != null) {
                                var option = document.createElement('option');
                                option.innerHTML = msg[i].pn_name;
                                option.value = msg[i].sno;
                                partgroup.appendChild(option);
                            }
                        }
                        $("select#slct_prtnme option").each(function () { this.selected = (this.text == partname); });
                        var f = document.getElementById('slct_prtnme');
                        var partname_sno = f.options[f.selectedIndex].value;
                        var data = { 'op': 'get_Part_number_data', 'partgroup_sno': partgroup_sno, 'partname_sno': partname_sno };
                        var s = function (msg) {
                            if (msg) {

                                var partgroup = document.getElementById('slct_prtno');
                                var length = partgroup.options.length;
                                document.getElementById('slct_prtno').options.length = null;
                                var opt = document.createElement('option');
                                opt.innerHTML = "Select Part Number";
                                opt.value = "Select Part Number";
                                opt.setAttribute("selected", "selected");
                                opt.setAttribute("disabled", "disabled");
                                opt.setAttribute("class", "dispalynone");
                                partgroup.appendChild(opt);
                                for (var i = 0; i < msg.length; i++) {
                                    if (msg[i].pnum_Name != null) {
                                        var option = document.createElement('option');
                                        option.innerHTML = msg[i].pnum_Name;
                                        option.value = msg[i].sno;
                                        partgroup.appendChild(option);
                                    }
                                }
                                $("select#slct_prtno option").each(function () { this.selected = (this.text == partno); });
                            }
                            else {
                            }
                        };
                        var e = function (x, h, e) {
                        };
                        callHandler(data, s, e);
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

        function onperunitcost_change() {
            var quantity = document.getElementById('txt_quantity').value;
            var perunitcost = document.getElementById('txt_perunitrs').value;
            if (quantity == "") {
                quantity = 0;
            }
            if (perunitcost == "") {
                perunitcost = 0;
            }
            var ttlcst = parseFloat(quantity) * parseFloat(perunitcost);
            document.getElementById('lbl_totalcost').innerHTML = ttlcst;
        }
        function forclearbelow() {
            document.getElementById('slct_partgroup').value = "Select Part Group";
            document.getElementById('slct_prtnme').value = "Select Part Name";
            document.getElementById('slct_prtno').value = "Select Part Number";
            document.getElementById('lbl_rake').innerHTML = "______";
            //document.getElementById('txt_fillremarks').value = "";
            //document.getElementById('txt_prtserial').value = "";
            document.getElementById('txt_quantity').value = "";
            document.getElementById('txt_perunitrs').value = "";
            document.getElementById('lbl_totalcost').innerHTML = "______";
            document.getElementById('lbl_stkavail').innerHTML = "______";
            document.getElementById('btn_saveitems').value = "ADD";
            fillval_error_messages();
        }
        function fillval_error_messages() {
            $('#lbl_prtgrp_error_msg').hide();
            $('#lbl_prtnme_error_msg').hide();
            $('#lbl_prtnumb_error_msg').hide();
            $('#lbl_prtserial_error_msg').hide();
            $('#lbl_prtqty_error_msg').hide();
            $('#lbl_unitrs_error_msg').hide();
        }
        function for_main_saving() {
            var inwarddate = document.getElementById('txt_inwarddate').value;
            var inwardno = document.getElementById('txt_inwardno').value;
            var invoiceno = document.getElementById('txt_invoice').value;
            var invoicedate = document.getElementById('txt_invoicedate').value;
            var dcno = document.getElementById('txt_dcno').value;
            var lrno = document.getElementById('txt_lrno').value;
            var d = document.getElementById('slct_vendorcode');
            var vendor_sno = d.options[d.selectedIndex].value;
            var e = document.getElementById('slct_mdeofinwrd');
            var inwardtype = e.options[e.selectedIndex].value;
            var podate = document.getElementById('txt_podate').value;
            var pono = document.getElementById('txt_pono').value;
            var doorno = document.getElementById('txt_doorno').value;
            //var transport = document.getElementById('txt_transport').value;
            var remarks = document.getElementById('txt_remarks').value;
            var totalcost = document.getElementById('lbl_maintotal').innerHTML;
            var btnval = document.getElementById('btn_mainsave').value;
            var sno = document.getElementById('lbl_sno').innerHTML;
            var fillitems = [];
            $('#itemstable> tbody > tr').each(function () {
                var partnumno_sno = $(this).find("td:eq(4)").text();
                var quantity = $(this).find("td:eq(5)").text();
                var perunit = $(this).find("td:eq(6)").text();
                var totalcost = $(this).find("td:eq(7)").text();
                var availstock = $(this).find("td:eq(9)").text();
                fillitems.push({ "partnumno_sno": partnumno_sno, "quantity": quantity, "perunit": perunit, "totalcost": totalcost, "availstock": availstock });
            });
            var data = { 'op': 'Inward_save_start' };
            var s = function (msg) {
                if (msg) {
                    for (var i = 0; i < fillitems.length; i++) {
                        var Data = { 'op': 'Inward_save_RowData', 'row_detail': fillitems[i], 'end': 'N' };
                        if (i == fillitems.length - 1) {
                            Data = { 'op': 'Inward_save_RowData', 'row_detail': fillitems[i], 'end': 'Y' };
                        }
                        var s = function (msg) {
                            if (msg == 'Y') {

                                var Data = { 'op': 'save_edit_Inward', 'inwarddate': inwarddate, 'inwardno': inwardno, 'invoiceno': invoiceno,
                                    'invoicedate': invoicedate, 'dcno': dcno, 'lrno': lrno, 'vendor_sno': vendor_sno, 'inwardtype': inwardtype,
                                    'podate': podate, 'doorno': doorno, 'remarks': remarks, 'totalcost': totalcost, 'pono': pono, 'btnval': btnval, 'sno': sno
                                };
                                var s = function (msg) {
                                    if (msg) {
                                        alert(msg);
                                        get_inwardnoonly();
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
        function resetall() {
            document.getElementById('txt_inwarddate').value = "";
            //document.getElementById('txt_inwardno').value = "";
            document.getElementById('txt_invoice').value = "";
            document.getElementById('txt_invoicedate').value = "";
            document.getElementById('txt_dcno').value = "";
            document.getElementById('txt_lrno').value = "";
            document.getElementById('slct_vendorcode').value = "Select Vendor";
            document.getElementById('slct_mdeofinwrd').value = "Select Mode of Inward";
            document.getElementById('txt_podate').value = "";
            document.getElementById('txt_pono').value = "";
            document.getElementById('txt_doorno').value = "";
            //document.getElementById('txt_transport').value = "";
            document.getElementById('txt_remarks').value = "";
            document.getElementById('lbl_maintotal').innerHTML = "________";
            var table = document.getElementById("itemstable");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            forclearbelow();
            getinward_nuber();
        }
        $(function () {
            $("#second_div").hide();
            get_inwardnoonly();
            $('#add_Inward').click(function () {
                $('#first_div').css('display', 'none');
                $('#second_div').css('display', 'block');
                resetall();
            });

            $('#btn_close').click(function () {
                $('#first_div').css('display', 'block');
                $('#second_div').css('display', 'none');
                resetall();
            });
        });
        var all_inward_data = [];
        function get_inwardnoonly() {
            var data = { 'op': 'get_inward_only' };
            var s = function (msg) {
                if (msg) {
                    fill_foreground_tbl(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function getallinward() {
            var data = { 'op': 'get_inward_data' };
            var s = function (msg) {
                if (msg) {
                    all_inward_data = msg;
                    fill_foreground_tbl(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function fill_foreground_tbl(msg) {
            var table = document.getElementById("tbl_inward");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            for (var i = 0; i < msg.length; i++) {
                $('#tbl_inward').append('<tr><th scope="row">' + msg[i].inward_id + '</th><td data-title="Invoice Number">' + msg[i].invoice_number + '</td><td data-title="Inward Date" >' + msg[i].inward_dt + '</td><td data-title="sno" style="display:none;">' + msg[i].sno + '</td><td><input type="button" class="btn btn-primary" name="Update" value ="Modify" onclick="update(this);"/></td></tr>');
            }
            $('#first_div').css('display', 'block');
            $('#second_div').css('display', 'none');
            resetall();
        }
        function update(thisid) {
            var table = document.getElementById("itemstable");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var row = $(thisid).parents('tr');
            var inward_sno = row[0].cells[3].innerHTML;
            document.getElementById('lbl_sno').innerHTML = inward_sno;

            var data = { 'op': 'get_inward_data', 'inward_sno': inward_sno };
            var s = function (msg) {
                if (msg) {
                    for (var i = 0; i < msg.length; i++) {
                        if (msg[i].sno == inward_sno) {
                            document.getElementById('txt_inwarddate').value = msg[i].inward_dt;
                            document.getElementById('txt_inwardno').value = msg[i].inward_id;
                            document.getElementById('txt_invoice').value = msg[i].invoice_number;
                            document.getElementById('txt_invoicedate').value = msg[i].invoice_date;
                            document.getElementById('txt_dcno').value = msg[i].dc_number;
                            document.getElementById('txt_lrno').value = msg[i].lr_number;
                            document.getElementById('slct_vendorcode').value = msg[i].vendor_sno;
                            document.getElementById('slct_mdeofinwrd').value = msg[i].inward_type;
                            document.getElementById('txt_podate').value = msg[i].po_date;
                            document.getElementById('txt_pono').value = msg[i].po_number;
                            document.getElementById('txt_doorno').value = "";
                            //document.getElementById('txt_transport').value = "";
                            document.getElementById('txt_remarks').value = msg[i].remarks;
                            document.getElementById('lbl_maintotal').innerHTML = msg[i].total_amount;
                            document.getElementById('btn_mainsave').value = "Modify";
                            for (j = 0; j < msg[i].inwarditems.length; j++) {
                                $('#itemstable').append('<tr><td data-title="Part Group"><span id="prtgrp">' + msg[i].inwarditems[j].PGName + '</span></td><td style="display:none;">' + msg[i].inwarditems[j].PGSno + '</td><td data-title="Part Name">' + msg[i].inwarditems[j].PNName + '</td><td style="display:none;">' + msg[i].inwarditems[j].PNSno + '</td><th scope="row">' + msg[i].inwarditems[j].PrtNumber + '</th><td style="display:none;">' + msg[i].inwarditems[j].partnum_sno + '</td><td data-title="Quantity">' + msg[i].inwarditems[j].quantity + '</td><td data-title="Unit Cost">' + msg[i].inwarditems[j].per_unit_cost + '</td><td data-title="Total Cost">' + msg[i].inwarditems[j].totalcost + '</td><td data-title="Rake">' + msg[i].inwarditems[j].RakeName + '</td><td style="display:none;">' + msg[i].inwarditems[j].availble_qty + '</td><td><input type="button" class="btn btn-primary" value="Modify" onclick="updatethis(this)"/></td><td><a class="row-delete"><span onclick="removerow(this)" class="glyphicon glyphicon-remove"></span></a></td></tr>');
                            }
                        }
                        break;
                    }
                    $('#first_div').css('display', 'none');
                    $('#second_div').css('display', 'block');
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function getinward_nuber() {
            var data = { 'op': 'get_allin_no' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        document.getElementById('txt_inwardno').value = "";
                        document.getElementById('txt_inwardno').value = msg[0].brnch_inward_start;
                        document.getElementById('txt_inwardno').disabled = true;
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Inward Entry<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Inward Entry</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Inward Entry Details
                </h3>
            </div>
            <div class="box-body">
                <div style="width: 100%; height: 100%;" class="tw-bs">
                    <div id="first_div">
                        <div id="inward_showlogs" style="text-align: center;">
                            <input id="add_Inward" type="button" class="btn btn-primary" name="submit" value='Add New Inward' />
                        </div>
                        <div id="div_inwardtable" style="background: #fff;">
                            <table id="tbl_inward" class="table table-bordered table-striped">
                                <thead>
                                    <tr>
                                        <th scope="col">
                                            Inward Number
                                        </th>
                                        <th scope="col">
                                            Invoice Number
                                        </th>
                                        <th scope="col">
                                            Inward Date
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
                    <div style="padding: 20px;" id="second_div">
                        <table align="center">
                            <tr>
                                <td>
                                    <label>
                                        Inward Date</label>
                                    <input id="txt_inwarddate" class="form-control" type="Date" />
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    <label>
                                        Inward Number</label>
                                    <input id="txt_inwardno" class="form-control" type="text" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Invoice Number</label>
                                    <input id="txt_invoice" class="form-control" type="text" />
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    <label>
                                        Invoice Date</label>
                                    <input id="txt_invoicedate" class="form-control" type="date" />
                                    <label id="lbl_sno" type="date" style="display: none;">
                                    </label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        DC Number</label>
                                    <input id="txt_dcno" class="form-control" type="text" />
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    <label>
                                        LR Number</label>
                                    <input id="txt_lrno" class="form-control" type="text" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Vendor Code</label>
                                    <select id="slct_vendorcode" class="form-control">
                                    </select>
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    <label>
                                        PO Date</label>
                                    <input id="txt_podate" class="form-control" type="date" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        PO Number</label>
                                    <input id="txt_pono" class="form-control" type="text" />
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    <label>
                                        Mode of Inward</label>
                                    <select id="slct_mdeofinwrd" class="form-control">
                                        <option value="Select Mode of Inward" disabled selected>Select Mode of Inward</option>
                                        <option value="Cash">Cash</option>
                                        <option value="Credit">Credit</option>
                                        <option value="CheckPaid">CheckPaid</option>
                                        <option value="FOC">FOC</option>
                                        <option value="Refurbished">Refurbished</option>
                                        <option value="Warranty">Warranty</option>
                                        <option value="Transported">Transported</option>
                                        <option value="ByBack">ByBack</option>
                                        <option value="AuditCorrrection">AuditCorrrection</option>
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Door Number</label>
                                    <input id="txt_doorno" class="form-control" type="text" />
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    <label>
                                        Remarks</label>
                                    <input id="txt_remarks" class="form-control" type="text" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div class="box box-success">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Fill Details
                                </h3>
                            </div>
                            <br />
                            <table align="center">
                                <tr>
                                    <td>
                                        <label>
                                            Part Group</label>
                                        <select id="slct_partgroup" class="form-control" onchange="getallpartnames()" style="min-width: 150px;">
                                        </select>
                                        <label class="errormessage" id="lbl_prtgrp_error_msg">
                                            *Please Select Part Group</label>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <label>
                                            Part Name</label>
                                        <select id="slct_prtnme" class="form-control" onchange="getallpartnumbers()" style="min-width: 150px;">
                                            <option value="Select Part Name" selected="selected" disabled="disabled" class="dispalynone">
                                                Select Part Name</option>
                                        </select>
                                        <label class="errormessage" id="lbl_prtnme_error_msg">
                                            *Please Select Part Name</label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Part Number</label>
                                        <select id="slct_prtno" class="form-control" onchange="getrackandstock()" style="min-width: 150px;">
                                            <option value="Select Part Number" selected="selected" disabled="disabled" class="dispalynone">
                                                Select Part Number</option>
                                        </select>
                                        <label class="errormessage" id="lbl_prtnumb_error_msg">
                                            *Please Select Part Number</label>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <label>
                                            Rake</label>
                                        <label class="form-control" id="lbl_rake">
                                            ______</label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Stock Avail</label>
                                        <label class="form-control" id="lbl_stkavail">
                                            ______</label>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <label>
                                            Quantity</label>
                                        <input id="txt_quantity" class="form-control" type="text" onblur="onperunitcost_change()" />
                                        <label class="errormessage" id="lbl_prtqty_error_msg">
                                            *Please Enter Quantity</label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Per Unit Rs</label>
                                        <input id="txt_perunitrs" class="form-control" type="text" onblur="onperunitcost_change()" />
                                        <label class="errormessage" id="lbl_unitrs_error_msg">
                                            *Please Enter Unit Price</label>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <label>
                                            Total Cost</label>
                                        <label class="form-control" class="form-control" id="lbl_totalcost">
                                            ______</label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <input id="btn_saveitems" type="button" class="btn btn-primary" value="ADD" />
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <input id="btn_clearitems" type="button" class="btn btn-danger" value="Clear" onclick="forclearbelow()" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div align="center">
                        </div>
                        <br />
                        <div>
                            <table id="itemstable" class="table table-bordered table-striped">
                                <thead>
                                    <tr>
                                        <th scope="col">
                                            Part Group
                                        </th>
                                        <th scope="col" style="display: none;">
                                            PartGroupSno
                                        </th>
                                        <th scope="col">
                                            Part Name
                                        </th>
                                        <th scope="col" style="display: none;">
                                            PartNameSno
                                        </th>
                                        <th scope="col">
                                            Part No
                                        </th>
                                        <th scope="col" style="display: none;">
                                            PartNoSno
                                        </th>
                                        <%-- <th data-hide="tablet,phone">
                                Part Serial No
                            </th>--%>
                                        <th scope="col">
                                            Quantity
                                        </th>
                                        <th scope="col">
                                            Per Unit Cost
                                        </th>
                                        <th scope="col">
                                            Total Cost
                                        </th>
                                        <%-- <th data-hide="tablet,phone">
                                Remarks
                            </th>--%>
                                        <th scope="col">
                                            Rake
                                        </th>
                                        <th scope="col" style="display: none;">
                                            Availstock
                                        </th>
                                        <th scope="col">
                                        </th>
                                        <th scope="col">
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                        <table align="center">
                            <tr>
                                <td>
                                    <label>
                                        Total Amount</label>
                                    <label id="lbl_maintotal">
                                        ________</label>&nbsp;&nbsp;RS
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input id="btn_mainsave" type="button" class="btn btn-primary" value="Save" onclick="for_main_saving()" />
                                </td>
                                <td>
                                    <input id="btn_clear" type="button" class="btn btn-danger" value="Reset All" onclick="resetall()" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>

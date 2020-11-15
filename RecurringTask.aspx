<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="RecurringTask.aspx.cs" Inherits="RecurringTask" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="js/utility.js" type="text/javascript"></script>

<script type="text/javascript">
    $(function () {
        getdepartment();
    });

    $(function () {
        getallpartgroups();
        $('#btn_mastersub').click(function () {
            $('#masterstable_div').css('display', 'none');
            $('#masterinfo_div').css('display', 'block');
            forerrormsgs();
            clearall();

        });
        $('#btn_addmaster').click(function () {
            $('#masterstable_div').css('display', 'none');
            $('#masterinfo_div').css('display', 'block');
            $('#div_tblmasterdata').css('display', 'none');
            forerrormsgs();
            clearall();

        });
        $('#close_minmaster').click(function () {
            $('#masterstable_div').css('display', 'none');
            $('#masterinfo_div').css('display', 'none');
            $('#div_tblmasterdata').show();
            $('#save_minmaster').val("Save");
            forerrormsgs();
            clearall();
        });
    });

    function getdepartment() {
        var minimaster = "Category";
        //var data = { 'op': 'get_Mini_Master_data', 'minimaster': minimaster, 'filter': 'apply' };
        var data = { 'op': 'get_only_vehtypes' };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {

                    var partgroup = document.getElementById('slct_choosemaster');
                    var length = partgroup.options.length;
                    document.getElementById('slct_choosemaster').options.length = null;
                    var opt = document.createElement('option');
                    opt.innerHTML = "Select Vehicle Type";
                    opt.value = "Select Vehicle Type";
                    opt.setAttribute("selected", "selected");
                    opt.setAttribute("disabled", "disabled");
                    opt.setAttribute("class", "dispalynone");
                    partgroup.appendChild(opt);
                    for (var i = 0; i < msg.length; i++) {
                        if (msg[i].sno != null && msg[i].v_ty_status != "0") {
                            var option = document.createElement('option');
                            option.innerHTML = msg[i].v_ty_name;
                            option.value = msg[i].sno;
                            partgroup.appendChild(option);
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

    var mainfunctionnsg = [];
    function getmasterdata() {
        var table = document.getElementById("tbl_mstrdata");
        for (var i = table.rows.length - 1; i > 0; i--) {
            table.deleteRow(i);
        }
        var d = document.getElementById("slct_choosemaster");
        var category_sno = d.options[d.selectedIndex].value;
        if (category_sno != "") {
            var data = { 'op': 'get_Recurring_data', 'category_sno': category_sno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filldata(msg);
                        mainfunctionnsg = msg;
                        $('#masterstable_div').hide();
                        $('#masterinfo_div').css('display', 'none');
                        $('#div_tblmasterdata').show();

                    }
                    else {
                        $('#masterstable_div').show();
                        $('#masterinfo_div').css('display', 'none');
                        $('#div_tblmasterdata').hide();
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
    }

    function filldata(msg) {
        var table = document.getElementById("tbl_mstrdata");
        for (var i = 0; i < msg.length; i++) {
            var recurrent_name = msg[i].recurrent_name;
            var due_in_day = msg[i].due_in_day;
            var due_in_working_hrs = msg[i].due_in_working_hrs;
            var due_in_kms = msg[i].due_in_kms;
            var sno = msg[i].sno;
            var statuscode = msg[i].status;
            var status = "";
            if (statuscode == "1") {
                status = "Enabled";
            }
            else {
                status = "Disabled";
            }
            var partgroup = "";
            var partname = "";
            var partnumber = "";
            var results = "";
            for (var j = 0; j < msg[i].partdat.length; j++) {
                partgroup = msg[i].partdat[j].partgroup;
                partname = msg[i].partdat[j].partname;
                partnumber = msg[i].partdat[j].partnumber;
                results += partnumber + ",";
            }
            results = results.substring(0, results.length - 1);
            $('#tbl_mstrdata').append('<tr><th scope="row">' + recurrent_name + '</th><td data-title="Due In Day">' + due_in_day + '</td><td data-title="Due In Working Hours" >' + due_in_working_hrs + '</td><td data-title="Due In KMs" >' + due_in_kms + '</td><td data-title="Status" >' + status + '</td><td data-title="sno" style="display:none;">' + sno + '</td><td><input type="button" name="Update" value ="Modify" onclick="updateclick(this);"/></td></tr>');
        }
    }
    function updateclick(thisid) {
        var table = document.getElementById("tbl_subtable");
        for (var i = table.rows.length - 1; i > 0; i--) {
            table.deleteRow(i);
        }
        var row = $(thisid).parents('tr');
        var recurrent_name = row[0].cells[0].innerHTML;
        var due_in_day = row[0].cells[1].innerHTML;
        var due_in_working_hrs = row[0].cells[2].innerHTML;
        var due_in_kms = row[0].cells[3].innerHTML;
        var sno = row[0].cells[5].innerHTML;
        var statuscode = row[0].cells[4].innerHTML;
        var status = "";
        if (statuscode == "Enabled") {
            status = "1";
        }
        else {
            status = "0";
        }
        for (var i = 0; i < mainfunctionnsg.length; i++) {
            if (recurrent_name == mainfunctionnsg[i].recurrent_name) {
                var sno = mainfunctionnsg[i].sno;
                var partgroup = "";
                var partname = "";
                var partnumber = "";
                var partnumber_sno = "";
                for (var j = 0; j < mainfunctionnsg[i].partdat.length; j++) {
                    partgroup = mainfunctionnsg[i].partdat[j].partgroup;
                    partname = mainfunctionnsg[i].partdat[j].partname;
                    partnumber = mainfunctionnsg[i].partdat[j].partnumber;
                    partnumber_sno = mainfunctionnsg[i].partdat[j].partnumber_sno;
                    qty = mainfunctionnsg[i].partdat[j].qty;
                    rectype = mainfunctionnsg[i].partdat[j].rectype;
                    $('#tbl_subtable').append('<tr class=".responsive-table tbody"><td data-title="Part Group">' + partgroup + '</td><td data-title="Part Group" style="display:none;"></td><td data-title="Part Name">' + partname + '</td><td data-title="Part Name" style="display:none;"></td><td data-title="Part Number" >' + partnumber + '</td><td data-title="Part No" style="display:none;">' + partnumber_sno + '</td><td data-title="Quantity">' + qty + '</td><td data-title="Recurrence Type">' + rectype + '</td><th scope="row"><img src="images/PopClose.png" onclick="removethisrow(this)"/></th></tr>');
                }
            }
        }

        document.getElementById('txt_rectaskname').value = recurrent_name;

        document.getElementById('txt_DueInDays').value = due_in_day;
        document.getElementById('txt_InWorkingHours').value = due_in_working_hrs;
        document.getElementById('txt_DueInKMs').value = due_in_kms;

        document.getElementById('txt_sno').value = sno;
        document.getElementById('cmb_status').value = status;
        $(".hiddenrow").show();

        $('#masterstable_div').css('display', 'none');
        $('#masterinfo_div').css('display', 'block');
        $('#div_tblmasterdata').css('display', 'none');
        $('#save_minmaster').val("Modify");
    }
    //--------->Getting Part Groups<--------------//

    function getallpartgroups() {
        var minimaster = "PartGroup";
        var data = { 'op': 'get_Mini_Master_data', 'minimaster': minimaster };
        var s = function (msg) {
            if (msg) {
                //if (msg.length > 0) {
                fillpartgroupdata(msg);
                //}
                // else {

                // }
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
                //if (msg.length > 0) {
                fillpartname(msg);
                //}
                // else {

                //}
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


    //--------->End OF Getting Part Names<--------------//

    //-------------> Getting PArt Number<----------------//


    function getallpartnumbers() {
        var partgroup_sno = document.getElementById('slct_partgroup').value;
        var partname_sno = document.getElementById('slct_partname').value;
        var data = { 'op': 'get_Part_number_data', 'partgroup_sno': partgroup_sno, 'partname_sno': partname_sno };
        var s = function (msg) {
            if (msg) {
                //  if (msg.length > 0) {
                fillpartnumber(msg);
                //  }
                //  else {

                //  }
            }
            else {
            }
        };
        var e = function (x, h, e) {
        }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        callHandler(data, s, e);
    }


    function fillpartnumber(partnumbersg) {
        var partnum = document.getElementById('slct_parnumber');
        var length = partnum.options.length;
        document.getElementById('slct_parnumber').options.length = null;
        var opt = document.createElement('option');
        opt.innerHTML = "Select Part Number";
        opt.value = "Select Part Number";
        opt.setAttribute("selected", "selected");
        opt.setAttribute("disabled", "disabled");
        opt.setAttribute("class", "dispalynone");
        partnum.appendChild(opt);
        for (var i = 0; i < partnumbersg.length; i++) {
            if (partnumbersg[i].pnum_Name != null) {
                var option = document.createElement('option');
                option.innerHTML = partnumbersg[i].pnum_Name;
                option.value = partnumbersg[i].sno;
                partnum.appendChild(option);
            }
        }
    }


    //-------------> End OF Getting PArt Number<----------------//


    //----------->For Adding Row<------------//

    function addrow() {
        $("#lbl_partgro_error_msg").hide();
        $("#lbl_partnme_error_msg").hide();
        $("#lbl_partnumb_error_msg").hide();
        var table = document.getElementById("tbl_subtable");
        var d = document.getElementById("slct_partgroup").value;
        var g = document.getElementById("slct_partgroup");
        var partgroup = g.options[g.selectedIndex].text;

        var e = document.getElementById("slct_partname").value;
        var h = document.getElementById("slct_partname");
        var partname = h.options[h.selectedIndex].text;

        var f = document.getElementById("slct_parnumber").value;
        var j = document.getElementById("slct_parnumber");
        var partnumber = j.options[j.selectedIndex].text;

        var quantity = document.getElementById("txt_Qty").value;
        var parttype = document.getElementById("slct_prttype").value;




        var flag = false;
        if (d == "Select Part Group") {
            $("#lbl_partgro_error_msg").show();

            flag = true;
        }
        if (e == "Select Part Name") {
            $("#lbl_partnme_error_msg").show();
            flag = true;

        }
        if (f == "Select Part Number") {
            $("#lbl_partnumb_error_msg").show();
            flag = true;

        }
        if (quantity == "") {
            $("#lbl_qty_error_msg").show();
            flag = true;

        }
        if (parttype == "Select Recurring Type") {
            $("#lbl_rectype_error_msg").show();
            flag = true;

        }
        if (flag) {
            return;
        }


        var item = [];
        var k = 0;
        var oTable = document.getElementById('tbl_subtable');
        //gets rows of table
        var rowLength = oTable.rows.length;
        //loops through rows
        for (i = 1; i < rowLength; i++) {

            //gets cells of current row
            var oCells = oTable.rows.item(i).cells;

            //gets amount of cells of current row
            var cellLength = oCells.length;

            //loops through each cell in current row

            /* get your cell info here */

            var partnumber1 = oCells.item(5).innerHTML;

            if (partnumber1 == f) {
                alert("This Part Number Already Added");
                return;
            }
        }


        var tablerowcnt = document.getElementById("tbl_subtable").rows.length;
        $('#tbl_subtable').append('<tr class=".responsive-table tbody"><td data-title="Part Group">' + partgroup + '</td><td data-title="Part Group" style="display:none;">' + d + '</td><td data-title="Part Name">' + partname + '</td><td data-title="Part Name" style="display:none;">' + e + '</td><td data-title="Part Number" >' + partnumber + '</td><td data-title="Part No" style="display:none;">' + f + '</td><td data-title="Quantity">' + quantity + '</td><td data-title="Recurrence Type">' + parttype + '</td><th scope="row"><img src="images/PopClose.png" onclick="removethisrow(this)"/></th></tr>');
        halfclear();
    }

    function removethisrow(thisid) {
        $(thisid).parent().parent().remove();
    }

    function forerrormsgs() {
        $("#lbl_rectask_error_msg").hide();
        $("#lbl_rt_dueindays").hide();
        $("#lbl_InWorkingHours").hide();
        $("#lbl_DueInKMs").hide();
        $("#lbl_qty_error_msg").hide();
        $("#lbl_rectype_error_msg").hide();
    }

    function forrecursave() {
        var taskname = document.getElementById('txt_rectaskname').value;
        var sno = document.getElementById('txt_sno').value;
        var btnval = document.getElementById('save_minmaster').value;
        var status = document.getElementById('cmb_status').value;

        var DueInDays = document.getElementById('txt_DueInDays').value;
        var InWorkingHours = document.getElementById('txt_InWorkingHours').value;
        var DueInKMs = document.getElementById('txt_DueInKMs').value;

        var d = document.getElementById('slct_choosemaster');
        var category_sno = d.options[d.selectedIndex].value;

        var item = [];
        var k = 0;
        var oTable = document.getElementById('tbl_subtable');
        //gets rows of table
        var rowLength = oTable.rows.length;

       
        var flag = false;

        if (taskname == "") {
            $("#lbl_rectask_error_msg").show();
            flag = true;

        }
        if (DueInDays == "") {
            $("#lbl_rt_dueindays").show();
            flag = true;

        }
        if (InWorkingHours == "") {
            $("#lbl_InWorkingHours").show();
            flag = true;

        }
        if (DueInKMs == "") {
            $("#lbl_DueInKMs").show();
            flag = true;
        }
        if (rowLength == 1) {
            alert("Please Add Atleat one Part Number");
            flag = true;
        }
        if (flag) {
            return;
        }
        //loops through rows
        for (i = 1; i < rowLength; i++) {

            //gets cells of current row
            var oCells = oTable.rows.item(i).cells;

            //gets amount of cells of current row
            var cellLength = oCells.length;

            //loops through each cell in current row

            /* get your cell info here */
            var partgroup = oCells.item(1).innerHTML;
            var partname = oCells.item(3).innerHTML;
            var partnumber = oCells.item(5).innerHTML;
            var qty = oCells.item(6).innerHTML;
            var rectype = oCells.item(7).innerHTML;
            item.push({ "partgroup": partgroup, 'partname': partname, "partnumber": partnumber, "qty": qty, "rectype": rectype });
        }

        var data = { 'op': 'Recurrence_save_start' };
        var s = function (msg) {
            if (msg) {
                for (var i = 0; i < item.length; i++) {
                    var Data = { 'op': 'Recurrence_save_RowData', 'row_detail': item[i], 'end': 'N' };
                    if (i == item.length - 1) {
                        Data = { 'op': 'Recurrence_save_RowData', 'row_detail': item[i], 'end': 'Y' };
                    }
                    var s = function (msg) {
                        if (msg == 'Y') {

                            var Data = { 'op': 'save_edit_Recurrence', 'taskname': taskname, 'DueInDays': DueInDays, 'InWorkingHours': InWorkingHours, 'DueInKMs': DueInKMs, 'btnval': btnval, 'sno': sno, 'status': status, 'category_sno': category_sno };
                            var s = function (msg) {
                                if (msg) {
                                    alert(msg);
                                    $('#masterstable_div').css('display', 'none');
                                    $('#masterinfo_div').css('display', 'none');
                                    $('#div_tblmasterdata').show();
                                    $('#save_minmaster').val("Save");
                                    forerrormsgs();
                                    clearall();
                                    getmasterdata();

                                }
                            }
                            var e = function (x, h, e) {
                            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                            callHandler(Data, s, e);
                        }

                    }
                    var e = function (x, h, e) {
                    }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
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

    function clearall() {
        document.getElementById('txt_rectaskname').value = "";

        document.getElementById('txt_DueInDays').value = "";
        document.getElementById('txt_InWorkingHours').value = "";
        document.getElementById('txt_DueInKMs').value = "";
        var table = document.getElementById("tbl_subtable");
        for (var i = table.rows.length - 1; i > 0; i--) {
            table.deleteRow(i);
        }
        halfclear();
        $(".hiddenrow").hide();
    }
    function halfclear() {
        document.getElementById("slct_partgroup").value = "Select Part Group";
        document.getElementById("cmb_status").value = "1";
        document.getElementById("slct_partname").value = "";
        document.getElementById("slct_parnumber").value = "";
        document.getElementById("txt_Qty").value = "";
        document.getElementById("slct_prttype").value = "Select Recurring Type";
    }

</script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="minimasters_showlogs"  style="text-align: center;">
        <div style="width:100%;font-size:20px;" align="center">
        <%--<div style="color:#fff;">Choose A Master</div><br />--%>
        <select id="slct_choosemaster" style="background:#fff;font-weight:bold;width:75%;" onchange="getmasterdata()">
        
        </select>
        </div>
        <br />
    </div>

    <div id="masterstable_div" style="background: #fff; display: none;">
        <br />
        <div id="nocontent_div" align="center" style="color: Red;">
            ****No Content Found For This Category****</div>
        <div id="addsome_div" align="center">
            <input id="btn_mastersub" type="button" value="Click Here To Add" />
        </div>
        <br />
    </div>
   


    <div id='masterinfo_div' style="display: none;" class='CSSTableGenerator'>
        <section>
            	<table cellpadding="1px">
                 <tr> <th colspan="2" align="center"><h3 id="h3_header"></h3> Recurring Task </th></tr>
                <tr> <td>Task Name<span style="color:red;">*</span></td><td><input id="txt_rectaskname" type="text" name="vendorcode" placeholder="Task Name"><label id="lbl_rectask_error_msg" class="errormessage" >* Please Enter Recurrence task Name</label></td></tr>
                <tr> <td>Due In Days<span style="color:red;">*</span></td><td><input type="text" id="txt_DueInDays" maxlength="45"  placeholder="Due In Days"><label id="lbl_rt_dueindays" class="errormessage" >* Please Enter Due In Days</label></td></tr>
                <tr> <td>Due In Working Hours<span style="color:red;">*</span></td><td><input type="text" id="txt_InWorkingHours" maxlength="45"  placeholder="Due In Working Hours"><label id="lbl_InWorkingHours" class="errormessage" >* Please Enter In Working Hours</label></td></tr>
                <tr> <td>Due In KMs<span style="color:red;">*</span></td><td><input type="text" id="txt_DueInKMs" maxlength="45"  placeholder="Due In KMs"><label id="lbl_DueInKMs" class="errormessage" >* Please Enter In Working Hours</label></td></tr>

                <tr><td colspan="10"><select id="slct_partgroup" onchange="getallpartnames()"></select><label id="lbl_partgro_error_msg" class="errormessage" >* Please Select Part Group</label>
                <select id="slct_partname" onchange="getallpartnumbers()"></select><label id="lbl_partnme_error_msg" class="errormessage" >* Please Select Part Name</label>
                <select id="slct_parnumber"></select><label id="lbl_partnumb_error_msg" class="errormessage" >* Please Select Part Number</label>
                <input type="text" id="txt_Qty" maxlength="45"  placeholder="Enter Quantity"><label id="lbl_qty_error_msg" class="errormessage" >* Please Enter Quantity</label>
                <select id="slct_prttype">
                <option value="Select Recurring Type" selected disabled>Select Recurring Type</option>
                <option value="Repair">Repair</option>
                <option value="Replace">Replace</option>
                </select><label id="lbl_rectype_error_msg" class="errormessage" >* Please Select recurring type</label>
                
                <input id='btn_add' type="button" value ='ADD' onclick="addrow()"/></td></tr>

                <tr>  <td  colspan="10"> 
                
                <table id="tbl_subtable" class="responsive-table">
            <thead>
                <tr>
                    <th scope="col">
                         Part Group
                    </th>
                    <th scope="col" style="display:none;">
                    Part Group sno
                    </th>
                    <th scope="col">
                        Part Name
                    </th>
                    <th scope="col" style="display:none;">
                    Part name sno
                    </th>
                    <th scope="col">
                        Part Number
                    </th>
                    <th scope="col" style="display:none;">
                    Part Number sno
                    </th>
                    <th scope="col">
                        Quantity
                    </th>
                    <th scope="col">
                        Recurring Type
                    </th>
                    <th scope="col">
                    </th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
        
        
        </td>  </tr>

                <tr style="display:none;" class="hiddenrow"> <td>Status</td><td>
                <select id="cmb_status" class="allinputs">
                <option value="1">Enabled</option>
                <option value="0">Disabled</option>
                </select>
                </td></tr>

                <tr style="display:none;"><td>Sno</td><td><input type="text" id="txt_sno" maxlength="45" name="vendorcode" placeholder="sno"></td></tr>
                <tr> <td colspan="2" align="center">
                <input id='save_minmaster' type="button" name="submit" value ='Save' onclick="forrecursave()"/>
                <input id='close_minmaster' type="button" name="Close" value ='Close'/></td></tr>
                </table>
          </section>
    </div>



    <div >
        <div id="div_tblmasterdata" class='divcontainer' style="display: none;">
         <div id="add_master" align="center">
        <input id='btn_addmaster' type="button" value="Add Recurring Task"/>
    </div>
    <br />


            <table id="tbl_mstrdata" class="responsive-table">
                <thead>
                    <tr>
                        <th scope="col">
                            Recuring Name
                        </th>
                        <th scope="col">
                            Due In Day
                        </th>
                        <th scope="col">
                            Due In Working Hours
                        </th>
                        <th scope="col">
                            Due In KMs
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


<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="MaintananceManagement.aspx.cs" Inherits="MaintananceManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">


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
            retrivedata();
            getallpartgroups();
            $(".hiddenrow").hide();
            $('#add_maintanance').click(function () {
                $('#employee_fillform').css('display', 'block');
                $('#maintanance_showlogs').css('display', 'none');
                $('#div_maintani').css('display', 'none');
                $("#lbl_maintanance_error_msg").hide();
                $(".hiddenrow").hide();
                clearall();
            });

            $('#close_maintanance').click(function () {
                $('#employee_fillform').css('display', 'none');
                $('#maintanance_showlogs').css('display', 'block');
                $('#div_maintani').css('display', 'block');
                $("#lbl_maintanance_error_msg").hide();
                clearall();
            });
        });


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
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
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
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
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
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
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
            $('#tbl_subtable').append('<tr class=".responsive-table tbody"><td data-title="Part Group">' + partgroup + '</td><td data-title="Part Group" style="display:none;">' + d + '</td><td data-title="Part Name">' + partname + '</td><td data-title="Part Name" style="display:none;">' + e + '</td><td data-title="Part Number" >' + partnumber + '</td><td data-title="Part No" style="display:none;">' + f + '</td><th scope="row"><img src="images/PopClose.png" onclick="removethisrow(this)"/></th></tr>');
        }

        function removethisrow(thisid) {
            $(thisid).parent().parent().remove();
        }



        //function for saving maintanance

        function forsaving_maintanance() {
            var maintanancename = document.getElementById('txt_maintanacename').value;
            var sno = document.getElementById('txt_sno').value;
            var btnval = document.getElementById('save_maintanance').value;
            var status = document.getElementById('cmb_status').value;
            var item = [];
            var k = 0;
            var oTable = document.getElementById('tbl_subtable');
            //gets rows of table
            var rowLength = oTable.rows.length;

            if (rowLength == 1) {
                alert("Please Add Atleat one Part Number");
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
                item.push({ "partgroup": partgroup, 'partname': partname, "partnumber": partnumber });
            }

            var flag = false;

            if (maintanancename == "") {
                $("#lbl_maintanance_error_msg").show();
                flag = true;

            }
            if (flag) {
                return;
            }
            var data = { 'op': 'Maintanance_save_start' };
            var s = function (msg) {
                if (msg) {
                    for (var i = 0; i < item.length; i++) {
                        var Data = { 'op': 'Maintanance_save_RowData', 'row_detail': item[i], 'end': 'N' };
                        if (i == item.length - 1) {
                            Data = { 'op': 'Maintanance_save_RowData', 'row_detail': item[i], 'end': 'Y' };
                        }
                        var s = function (msg) {
                            if (msg == 'Y') {

                                var Data = { 'op': 'save_edit_Maintanance', 'maintanancename': maintanancename, 'btnval': btnval, 'sno': sno, 'status': status };
                                var s = function (msg) {
                                    if (msg) {
                                        alert(msg);
                                        $('#employee_fillform').css('display', 'none');
                                        $('#maintanance_showlogs').css('display', 'block');
                                        $('#div_maintani').css('display', 'block');
                                        $('#save_maintanance').val("Save");
                                        clearall();
                                        retrivedata();

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

        var mainfunctionnsg = [];
        function retrivedata() {
            var table = document.getElementById("tbl_maintanance");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var data = { 'op': 'get_all_Maintenance'};
            var s = function (msg) {
                if (msg) {
                    fillmaintenance(msg);
                    mainfunctionnsg = msg;
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillmaintenance(msg) {
            var table = document.getElementById("tbl_maintanance");
            for (var i = 0; i < msg.length; i++) {
                var maintenance_name = msg[i].maintenance_name;
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
                $('#tbl_maintanance').append('<tr><th scope="row">' + maintenance_name + '</th><td data-title="Part Numbers">' + results + '</td><td data-title="Status" >' + status + '</td><td data-title="sno" style="display:none;">' + sno + '</td><td><input type="button" class="btn btn-primary" name="Update" value ="Modify" onclick="updateclick(this);"/></td></tr>');
            }
        }

        function updateclick(thisid) {
            var table = document.getElementById("tbl_subtable");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var row = $(thisid).parents('tr');
            var maintenance_name = row[0].cells[0].innerHTML;
            var sno = row[0].cells[3].innerHTML;
            var statuscode = row[0].cells[2].innerHTML;
            var status = "";
            if (statuscode == "Enabled") {
                status = "1";
            }
            else {
                status = "0";
            }
            for (var i = 0; i < mainfunctionnsg.length; i++) {
                if (maintenance_name == mainfunctionnsg[i].maintenance_name) {
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
                        $('#tbl_subtable').append('<tr class=".responsive-table tbody"><td data-title="Part Group">' + partgroup + '</td><td data-title="Part Group" style="display:none;"></td><td data-title="Part Name">' + partname + '</td><td data-title="Part Name" style="display:none;"></td><td data-title="Part Number" >' + partnumber + '</td><td data-title="Part No" style="display:none;">' + partnumber_sno + '</td><th scope="row"><img src="images/PopClose.png" onclick="removethisrow(this)"/></th></tr>');
                    }
                }

            }
            document.getElementById('txt_maintanacename').value = maintenance_name;
            document.getElementById('txt_sno').value = sno;
            document.getElementById('cmb_status').value = status;
            $(".hiddenrow").show();
            $('#employee_fillform').css('display', 'block');
            $('#maintanance_showlogs').css('display', 'none');
            $('#div_maintani').css('display', 'none');
            $('#save_maintanance').val("Modify");
        }


        function clearall() {
            document.getElementById('txt_maintanacename').value = "";
            var table = document.getElementById("tbl_subtable");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }


            document.getElementById("slct_partgroup").value = "Select Part Group";
            document.getElementById("cmb_status").value = "1";

            document.getElementById("slct_partname").value = "";

            document.getElementById("slct_parnumber").value = "";
        }

    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <div id="maintanance_showlogs" style="text-align: center;">
        <input id="add_maintanance" type="button" class="btn btn-primary" name="submit" value='Add Maintananmce' />
    </div>
    <div id='employee_fillform' style="display: none;" class='CSSTableGenerator'>
        <section>
            	<table cellpadding="1px">
                 <tr> <th colspan="2" align="center"><h3>Add Maintananmce</h3>  </th></tr>
                
                <tr> <td>Maintanance Name<span style="color:red;">*</span></td><td><input id="txt_maintanacename" type="text" name="vendorcode" placeholder="Maintanance Name"><label id="lbl_maintanance_error_msg" class="errormessage" >* Please Enter Maintanance Name</label></td></tr>
                
                <tr><td colspan="10"><select id="slct_partgroup" onchange="getallpartnames()"></select><label id="lbl_partgro_error_msg" class="errormessage" >* Please Select Part Group</label>
                <select id="slct_partname" onchange="getallpartnumbers()"></select><label id="lbl_partnme_error_msg" class="errormessage" >* Please Select Part Name</label>
                <select id="slct_parnumber"></select><label id="lbl_partnumb_error_msg" class="errormessage" >* Please Select Part Number</label>
                <input id='btn_add' type="button" class="btn btn-primary" value ='ADD' onclick="addrow()"/></td></tr>

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
                <input id='save_maintanance' type="button" class="btn btn-primary" name="submit" value ='Save' onclick="forsaving_maintanance()"/>
                <input id='close_maintanance' type="button" class="btn btn-primary" name="Close" value ='Close'/></td></tr>
                </table>
          </section>
    </div>

    <div style="background:#ffffff;">
    
     <div id="div_maintani" class='divcontainer'>
        <table id="tbl_maintanance" class="responsive-table">
            <thead>
                <tr>
                    <th scope="col">
                        Maintenance Type
                    </th>
                    <th scope="col">
                        Part Numbers
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


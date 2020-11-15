<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="HeadMaster.aspx.cs" Inherits="HeadMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/utility.js" type="text/javascript"></script>
    <link href="responsivegridsystem/css/2cols.css" rel="stylesheet" type="text/css" />
    <link href="responsivegridsystem/css/col.css" rel="stylesheet" type="text/css" />
    <link href="responsivegridsystem/css/html5reset.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            get_headdata();
        });
        var categorydata;
        function get_headdata() {
            var table = document.getElementById("tbl_feecategorylist");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var k = 0;
            var colorue = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            var data = { 'op': 'get_head_master_List' };
            var s = function (msg) {
                if (msg) {
                    categorydata = msg;
                    for (var i = 0; i < msg.length; i++) {
                        var tablerowcnt = document.getElementById("tbl_feecategorylist").rows.length;
                        var status;
                        if (msg[i].status == "1") {
                            status = "Enable";
                        }
                        else {
                            status = "Disable";
                        }
                        $('#tbl_feecategorylist').append('<tr style="background-color:' + colorue[k] + '"><td data-title="categorysno">' + msg[i].sno + '</td>' +
                        //'<td scope="Category Name" >' + msg[i].desc + '</td>' +
                        '<td style="font-weight: 600;"><i class="" aria-hidden="true"></i>&nbsp;<span id="1">' + msg[i].desc + '</span></td>' +   
                        '<td data-title="IsTransport">' + status + '</td>' +
                        '<td data-title="IsTransport">' + msg[i].accounttype + '</td>' +
                        '<td data-title="Status" style="display:none;">' + msg[i].refno + '</td>' +
                        //'<td><input type="button" class="btn btn-primary" name="Update" value ="Modify" onclick="updateclick(this);"/></td></tr>');
                        '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls" name="Update" value ="Modify"   onclick="updateclick(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>');
                        k = k + 1;
                        if (k == 4) {
                            k = 0;
                        }
                    }
                }
                else {
                    document.location = "Default.aspx";
                }
            }
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        }
        function HeadofAcvalidation() {
            var description = document.getElementById('txtDecription').value;
            if (description == "") {
                alert("Please Enter Description");
                $('#txtDecription').focus();
                return false;
            }
            var status = document.getElementById('slct_status').value;
            var ddlaccounttype = document.getElementById('ddlaccounttype').value;
            var data = { 'op': 'head_master_saveclick', 'accounttype': ddlaccounttype, 'description': description, 'status': status, 'operation': operation, 'Headsno': Headsno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        alert(msg);
                        btn_cancel_click();
                        get_headdata();
                    }
                }
                else {
                    document.location = "Default.aspx";
                }
            }
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        }
        function btn_cancel_click() {
            document.getElementById('txtDecription').value = "";
            document.getElementById('slct_status').selectedIndex = 0;
            document.getElementById('ddlaccounttype').selectedIndex = 0;
            Headsno = null;
            document.getElementById('btnSave').innerHTML = "SAVE";
        }
        var Headsno;
        var operation = "SAVE";
        function updateclick(thisid) {
            var selectedrow = $(thisid).closest('tr');
            Headsno = selectedrow[0].cells[4].innerHTML;
            operation = "MODIFY";
            document.getElementById('txtDecription').value = $(thisid).parent().parent().find('#1').text();
            document.getElementById('ddlaccounttype').value = selectedrow[0].cells[3].innerHTML;
            if (selectedrow[0].cells[2].innerHTML == "Enable") {
                document.getElementById('slct_status').value = "1";
            }
            else {
                document.getElementById('slct_status').value = "0";
            }
            document.getElementById('btnSave').innerHTML = "Modify";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Head Of Accounts<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Head Of Accounts</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Add Head Of Accounts
                </h3>
            </div>
            <div class="box-body">
                <table align="center">
                    <tr class="divPayTodesc">
                        <td style="height: 40px;">
                           <label> Head Description </label><span style="color: red;">*</span>
                        </td>
                        <td>
                            <input type="text" id="txtDecription" class="form-control" placeholder="Enter Head Description" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 40px;">
                          <label>  Account Type </label><span style="color: red;">*</span>
                        </td>
                        <td>
                            <select class="form-control" id="ddlaccounttype">
                                <option >General Maintaince </option>
                                <option >Vehicle a/c</option>
                                <option >Company a/c</option>
                                <option >Tollgate</option>
                                <option >Tax</option>
                                <option >Store Consumption</option>
                                <option >Miscellaneous a/c</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 40px;">
                          <label>  Status </label>
                        </td>
                        <td>
                            <select class="form-control" id="slct_status">
                                <option value="1">Enable</option>
                                <option value="0">Disable</option>
                            </select>
                        </td>
                    </tr>
                   
                </table>
                <table>
                 <tr>
                        <td>
                        </td>
                        <td style="height: 40px;padding-left: 441px;">
                            <%--<input type="button" id="btnSave" value="SAVE" onclick="HeadofAcvalidation();" class="btn btn-primary"/>--%>
                             <div class="input-group">
                                        <div class="input-group-addon">
                                        <span class="glyphicon glyphicon-ok" id="btnSave1" onclick="HeadofAcvalidation()"></span><span id="btnSave" onclick="HeadofAcvalidation()">SAVE</span>
                                  </div>
                                  </div>
                        </td>
                        <td style="width:10px;"></td>
                                    <td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btn_cancel_click1' onclick="btn_cancel_click()"></span><span id='btn_cancel_click' onclick="btn_cancel_click()">Clear</span>
                            </div>
                            </div>
                            </td>
                    </tr>
                </table>
                </div>
        </div>

                <div class="box box-primary">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i style="padding-right: 5px;" class="fa fa-list"></i>Head Of Account List</h3>
                    </div>
                    <div class="box-body">
                        <div>
                            <table id="tbl_feecategorylist" class="table table-bordered table-striped">
                                <thead>
                                    <tr style="background:#5aa4d0; color: white; font-weight: bold;">
                                        <th scope="col">
                                            Sno
                                        </th>
                                        <th scope="col">
                                            Head Of Accounts
                                        </th>
                                        <th scope="col">
                                            Status
                                        </th>
                                            <th scope="col">
                                            Account Type
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
    </section>
</asp:Content>

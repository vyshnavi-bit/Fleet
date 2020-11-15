<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="AcceptTyreTransfer.aspx.cs" Inherits="AcceptTyreTransfer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/utility.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            get_transferd_tyes();
        });
        function get_transferd_tyes() {
            var table = document.getElementById("tyres_table");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var data = { 'op': 'get_transfered_tyres_thisbrn' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {

                        for (var i = 0; i < msg.length; i++) {
                            $("#tyres_table").append('<tr><td>' + (i + 1) + '</td><td style="display:none;" ><label name="sno">' + msg[i].sno + '</label></td><td><label name="tyre_sno" style="font-weight: normal;">' + msg[i].tyre_sno + '</label></td>' +
            '<td><label name="Brand" style="font-weight: normal;">' + msg[i].brand + '</label></td>' +
            '<td><label name="Type_of_Tyre" style="font-weight: normal;">' + msg[i].tyretype + '</label></td>' +
            '<td><label name="Tube_Type" style="font-weight: normal;">' + msg[i].tubetyre + '</label></td>' +
            '<td><label name="Size" style="font-weight: normal;">' + msg[i].size + '</label></td>' +
            '<td><label name="Remarks" style="font-weight: normal;">' + msg[i].remarks + '</label></td>' +
            '<td><label name="frombranch" style="font-weight: normal;">' + msg[i].branchname + '</label></td>' +
            '<td><input name="Accept" class="btn btn-primary" type="button" value="Accept" onclick="accept_tyre(this)"/></td>' +
            '<td><input name="Reject" class="btn btn-primary" type="button" value="Reject" onclick="Reject_tyre(this)" /></td></tr>');
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

        function accept_tyre(thisid) {
            var sno = $(thisid).parent().parent().children().find("[name=sno]").text();
            var status = "A";
            var data = { 'op': 'accept_reject_tyre', 'sno': sno, 'status': status };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        alert(msg);
                        get_transferd_tyes();
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
        function Reject_tyre(thisid) {
            var sno = $(thisid).parent().parent().children().find("[name=sno]").text();
            var status = "R";
            var data = { 'op': 'accept_reject_tyre', 'sno': sno, 'status': status };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        alert(msg);
                        get_transferd_tyes();
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
    <div class="table-responsive">
        <table class="table table-condensed" id="tyres_table">
            <thead>
                <tr>
                    <th>
                        #
                    </th>
                    <th>
                        Tyre_Sno
                    </th>
                    <th style="display: none;">
                        Sno
                    </th>
                    <th>
                        Brand
                    </th>
                    <th>
                        Type_of_Tyre
                    </th>
                    <th>
                        Tube_Type
                    </th>
                    <th>
                        Size
                    </th>
                    <th>
                        Remarks
                    </th>
                    <th>
                        From Branch
                    </th>
                    <th>
                        Accept
                    </th>
                    <th>
                        Reject
                    </th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
        <div align="right" id="morerows_div" style="display: none;">
            <input id='btn_addmore' type="button" class="btn btn-primary" value='Add More Tyres'
                onclick="addmore()" />
        </div>
    </div>
</asp:Content>

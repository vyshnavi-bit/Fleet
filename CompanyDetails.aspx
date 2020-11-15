<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="CompanyDetails.aspx.cs" Inherits="CompanyDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="css/formstable.css" rel="stylesheet" type="text/css" />
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
        function CallHandlerUsingJson(d, s, e) {<asp:Table runat="server"></asp:Table>
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
            $('btn_cmpnysave').val('Save');
            retrivecompanydata();
        });

        function retrivecompanydata() {
            var data = { 'op': 'getcompanydetails' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        document.getElementById('txt_cmpnyname').value = msg[0].CompanyName;
                        document.getElementById('txt_mobileno').value = msg[0].PhoneNumber;
                        document.getElementById('txt_mail').value = msg[0].Email;
                        document.getElementById('txt_Address').value = msg[0].Address;
                        document.getElementById('txt_sno').value = msg[0].sno;
                        $('btn_cmpnysave').val('Modify');
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
        function validateEmail(email) {
            var reg = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/
            if (reg.test(email)) {
                return true;
            }
            else {
                return false;
            }
        } 
        function savecmpnydetails() {
            var cmpny = document.getElementById('txt_cmpnyname').value;
            var mobileno = document.getElementById('txt_mobileno').value;
            var mail = document.getElementById('txt_mail').value;
            var addr = document.getElementById('txt_Address').value;
            var sno = document.getElementById('txt_sno').value;
            var btnval = document.getElementById('btn_cmpnysave').value;

            if (!validateEmail(mail)) {
                alert("Please Enter Proper Mail ID");
                return;
            }
            var data = { 'op': 'save_cmpnydetails', 'cmpny': cmpny, 'mobileno': mobileno, 'mail': mail, 'addr': addr, 'sno': sno, 'btnval': btnval };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        alert("Company Details Added Successfully");
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
    <div id="showlogs">
        <div id='fillform' class='CSSTableGenerator'>
            <section>
                <table cellpadding="1px">
                    <tr>
                        <th colspan="2" align="center">
                            <h3>
                                Enter/Modify Your Company Details</h3>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            Company Name
                        </td>
                        <td>
                            <input type="text" id="txt_cmpnyname" name="companyname" maxlength="45" placeholder="Company Name">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Phone Number
                        </td>
                        <td>
                            <input type="text" name="phoneno" id="txt_mobileno" maxlength="10" placeholder="Phone number">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Email
                        </td>
                        <td>
                            <input type="text" id="txt_mail" name="vendorcode" placeholder="Email">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Address
                        </td>
                        <td>
                            <input type="text" name="vendorcode" maxlength="45" id="Text1" placeholder="Address">
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td>
                            sno
                        </td>
                        <td>
                            <input type="text" name="vendorcode" maxlength="45" id="txt_sno" placeholder="sno">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <input id="btn_cmpnysave" onclick="savecmpnydetails()" type="button" name="submit"
                                value='Save' />
                        </td>
                    </tr>
                </table>
            </section>
        </div>
    </div>
</asp:Content>

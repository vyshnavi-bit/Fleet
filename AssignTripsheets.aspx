<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="AssignTripsheets.aspx.cs" Inherits="AssignTripsheets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/JTemplate.js" type="text/javascript"></script>
    <script src="js/utility.js" type="text/javascript"></script>
    <script type="text/javascript">
        
        $(function () {
            GetAssignTripSheets();
        });
        function GetAssignTripSheets() {
            var data = { 'op': 'GetAssignTripSheets' };
            var s = function (msg) {
                if (msg) {
                    $('#divFillScreen').removeTemplate();
                    $('#divFillScreen').setTemplateURL('EndTripSheet3.htm');
                    $('#divFillScreen').processTemplate(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function btnViewJobCards(ID) {
            var txtTripDataSno = $(ID).closest("tr").find('#txtTripDataSno').val();
            var TripDate = $(ID).closest("tr").find('#txtTripDate').text();
            var data = { 'op': 'GetBtnViewJobcardclick', 'txtTripDataSno': txtTripDataSno };
            var s = function (msg) {
                if (msg) {
                    $('#divMainAddNewRow').css('display', 'block');
                    document.getElementById("spnTripSheetNo").innerHTML = txtTripDataSno;
                    document.getElementById("spnJobCardDate").innerHTML = TripDate;
                    document.getElementById('divJobCards').innerHTML = "";
                    for (var i = 0; i <= msg.length; i++) {
                        if (typeof msg[i] === "undefined" || msg[i].jobcardname == "" || msg[i].jobcardname == null) {
                        }
                        else {
                            var span = document.createElement("span");
                            span.value = msg[i].status;
                            var checkbox = document.createElement("input");
                            checkbox.type = "checkbox";
                            checkbox.name = "checkbox";
                            checkbox.value = "checkbox";
                            checkbox.id = "checkbox";
                            if (msg[i].status == "Verified") {
                                checkbox.disabled = true;
                                checkbox.checked = true;
                            }
                            else {
                                checkbox.disabled = false;
                            }
                            checkbox.className = 'chkclass';
                            document.getElementById('divJobCards').appendChild(checkbox);
                            var labeljobcardname = document.createElement("span");
                            labeljobcardname.innerHTML = msg[i].jobcardname;
                            document.getElementById('divJobCards').appendChild(labeljobcardname);
                            //                            var labeljobcarddetails = document.createElement("span");
                            //                            labeljobcarddetails.innerHTML = "      -      " + msg[i].jobcarddetails;
                            //                            document.getElementById('divJobCards').appendChild(labeljobcarddetails);
                            document.getElementById('divJobCards').appendChild(span);
                            document.getElementById('divJobCards').appendChild(document.createElement("br"));
                        }
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
        function JobCardViewCloseClick() {
            $('#divMainAddNewRow').css('display', 'none');
        }
        function btnPrintTripSheet(ID) {
            var txtTripDataSno = $(ID).closest("tr").find('#txtTripDataSno').val();
            var data = { 'op': 'btnTripSheetPrintClick', 'txtTripDataSno': txtTripDataSno };
            var s = function (msg) {
                if (msg) {

                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
            window.location = "PrintTripSheet.aspx";
        }
        function btnPrintJobCardSheet(ID) {
            var txtTripDataSno = $(ID).closest("tr").find('#txtTripDataSno').val();
            var data = { 'op': 'btnTripSheetPrintClick', 'txtTripDataSno': txtTripDataSno };
            var s = function (msg) {
                if (msg) {

                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
            window.location = "PrintJobCard.aspx";
        }
        function btnCompletedclick() {
            var TripSheetNo = document.getElementById("spnTripSheetNo").innerHTML;
            var JobCardName = "";
            var ckdvlsdiv = document.getElementById('divJobCards').childNodes;
            for (var i = 0; i < ckdvlsdiv.length; i++) {
                if (ckdvlsdiv[i].type == 'checkbox' && $(ckdvlsdiv[i]).is(":checked")) {
                    var Selected = $(ckdvlsdiv[i]).next().text();
                    JobCardName += Selected + ','
                }
            }
            if (JobCardName == "") {
                alert("Please select job cards");
                return false;
            }
            JobCardName = JobCardName.substring(0, JobCardName.length - 1);
            var data = { 'op': 'UpdateJobcardbtnclick', 'TripSheetNo': TripSheetNo, 'JobCardName': JobCardName };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    var data = { 'op': 'GetBtnViewJobcardclick', 'TripSheetNo': TripSheetNo };
                    var s = function (msg) {
                        if (msg) {
                            $('#divMainAddNewRow').css('display', 'block');
                            document.getElementById("spnTripSheetNo").innerHTML = TripSheetNo;
                            document.getElementById("spnJobCardDate").innerHTML = TripDate;
                            for (var i = 0; i <= msg.length; i++) {
                                if (typeof msg[i] === "undefined" || msg[i].jobcardname == "" || msg[i].jobcardname == null) {
                                }
                                else {
                                    var label = document.createElement("span");
                                    var span = document.createElement("span");
                                    span.value = msg[i].status;
                                    var checkbox = document.createElement("input");
                                    checkbox.type = "checkbox";
                                    checkbox.name = "checkbox";
                                    checkbox.value = "checkbox";
                                    checkbox.id = "checkbox";
                                    if (msg[i].status == "Verified") {
                                        checkbox.disabled = true;
                                        checkbox.checked = true;
                                    }
                                    else {
                                        checkbox.disabled = false;
                                    }
                                    //checkbox.className = 'checkinput';
                                    checkbox.className = 'chkclass';
                                    document.getElementById('divJobCards').appendChild(checkbox);
                                    label.innerHTML = msg[i].jobcardname;
                                    document.getElementById('divJobCards').appendChild(label);
                                    document.getElementById('divJobCards').appendChild(span);
                                    document.getElementById('divJobCards').appendChild(document.createElement("br"));
                                }
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
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function btnTripEndclick(ID) {
            var txtTripDataSno = $(ID).closest("tr").find('#txtTripDataSno').val();
            var data = { 'op': 'btnTripSheetEndClick', 'TripSheetNo': txtTripDataSno };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
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
    <style type="text/css">
        .Spancontrol
        {
            display: block;
            height: 36px;
            padding: 6px 12px;
            font-size: 14px;
            line-height: 1.428571429;
            color: #555;
            vertical-align: middle;
            background-color: #fff;
            background-image: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="divFillScreen">
    </div>
    <div id="divMainAddNewRow" class="pickupclass" style="text-align: center; height: 100%;
        width: 100%; position: absolute; display: none; left: 0%; top: 0%; z-index: 99999;
        background: rgba(192, 192, 192, 0.7);">
        <div id="divAddNewRow" style="border: 5px solid #A0A0A0; position: absolute; top: 30%;
            background-color: White; left: 10%; right: 10%; width: 80%; height: 50%; -webkit-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
            -moz-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65); box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
            border-radius: 10px 10px 10px 10px;">
            <table cellpadding="0" cellspacing="0" style="float: left; width: 100%;" id="tableCollectionDetails"
                class="mainText2" border="1">
                <tr>
                    <td>
                        Trip Ref No
                    </td>
                    <td>
                        <span id="spnTripSheetNo" class="Spancontrol"></span>
                    </td>
                </tr>
                <tr>
                    <td>
                        Job Card Date
                    </td>
                    <td>
                        <span id="spnJobCardDate" class="Spancontrol"></span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <div id="divJobCards" style="border-radius: 7px 7px 7px 7px; padding-top: 3%;font-size: 18px">
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <input type="button" id="Button1" value="Completed" onclick="btnCompletedclick();"
                            style="width: 130px; height: 34px; font-size: 14px;" class="btn btn-primary" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="divclose" style="width: 35px; top: 27.5%; right: 8%; position: absolute;
            z-index: 99999; cursor: pointer;">
            <img src="Images/Close.png" alt="close" onclick="JobCardViewCloseClick();" />
        </div>
    </div>
</asp:Content>

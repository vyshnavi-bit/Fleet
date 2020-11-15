<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="VehicleModuleConfig.aspx.cs" Inherits="VehicleModuleConfig" %>

<%@ Register Assembly="DropDownCheckList" Namespace="UNLV.IAP.WebControls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="DropDownCheckList.js" type="text/javascript"></script>
    <script type="text/javascript">


    </script>
    <style type="text/css">
        input[type=checkbox] {
        transform: scale(1.5);
        }
        input[type=checkbox] {
        width: 30px;
        height: 18px;
        margin-right: 8px;
        cursor: pointer;
        font-size: 10px;
        visibility: hidden;
        }
        input[type=checkbox]:after {
        content: " ";
        background-color: #fff;
        display: inline-block;
        margin-left: 10px;
        padding-bottom: 0px;
        color: #24b6dc;
        width: 16px;
        height: 16px;
        visibility: visible;
        border: 1px solid rgba(18, 18, 19, 0.12);
        padding-left: 3px;
        border-radius: 0px;
        }
        input[type="checkbox"]:not(:disabled):hover:after{
        border: 1px solid #24b6dc;
        }
        input[type=checkbox]:checked:after {
        content: "\2714";
            padding:-5px;
            font-weight:bold;
        }
     </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdateProgress ID="updateProgress1" runat="server">
        <ProgressTemplate>
            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #FFFFFF; opacity: 0.7;">
                <br />
                <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="thumbnails/loading.gif"
                    AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: absolute; top: 35%; left: 40%;" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <section class="content-header">
        <h1>
            Vehicle ModuleConfig<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>ModuleConfig</a></li>
            <li><a href="#">Vehicle ModuleConfig</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Vehicle ModuleConfig
                </h3>
            </div>
            <div class="box-body">
                <asp:UpdatePanel ID="updPanel" runat="server">
                    <ContentTemplate>
                      <div style="width: 100%;">
                          <div style="float: left; width: 60%; ">
                             <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style=" width: 40%; vertical-align: top;">
                                                    <div style=" overflow: auto; height: 250px;" class="borderradius">
                                                        <asp:CheckBox runat="server" ID="ckbZones" Text="All Modules" AutoPostBack="true" CssClass="chkclass"
                                                            OnCheckedChanged="ckbZones_OnCheckedChanged" />
                                                        <asp:CheckBoxList ID="chblZones" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chblZones_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </div>
                                                </td>
                                                <td style="width: 40%; vertical-align: top;">
                                                    <div style="height: 250px; overflow: auto;" class="borderradius">
                                                        <asp:CheckBox runat="server" ID="ckbVehicleTypes" Text="All Vehicle Capacity" AutoPostBack="true" CssClass="chkclass"
                                                            OnCheckedChanged="ckbVehicleTypes_OnCheckedChanged" />
                                                        <asp:CheckBoxList ID="chblVehicleTypes" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chblVehicleTypes_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </div>
                                                </td>
                                                <td style="width: 20%; vertical-align: top;">
                                                    <div style="min-height: 250px; max-height: 305px; overflow: auto; border: 1px solid #d5d5d5;
                                                        border-radius: 4px 4px 4px 4px;">
                                                        <asp:CheckBox runat="server" ID="ckballvehicles" Text="All Vehicles" AutoPostBack="true" Width="180px" 
                                                            OnCheckedChanged="ckballvehicles_SelectedIndexChanged" />
                                                        <asp:CheckBoxList ID="ckbVehicles" Width="160px" runat="server">
                                                        </asp:CheckBoxList>
                                                    </div>
                                                    <asp:Label ID="Label1" runat="server" Text="Vehicles Count :" Font-Size="12px" ForeColor="Red"></asp:Label>
                                                    <asp:Label ID="lblvehcount" runat="server" Text="0" Font-Bold="true" Font-Size="14px"
                                                        ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                              <tr>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnSave" Text="Save" CssClass="btn btn-success" runat="server"  OnClick="btnSave_Click" />
                                                                        <asp:Button ID="btnCancel" CausesValidation="false" CssClass="btn btn-danger" Text="Refresh" Visible="true"
                                                                            runat="server" OnClick="btnCancel_Click" />
                                                                        <asp:Button ID="btnDelete" Text="Delete" CssClass="btn btn-warning" runat="server"  Visible="true"
                                                                            OnClick="btnDelete_Click" />
                                                                    </td>
                                                                </tr>
                                        </table>
                          </div>
                          <div style="float: right; width: 40%;">
                               <table border="0">
                                                                <tr>
                                                                    <td>
                                                                        <div style="height: 180px;width: 189%;overflow: auto;padding-left: 3%;">
                                                                            <asp:GridView ID="grdManageLogins" runat="server" AutoGenerateSelectButton="true" Width="100%"
                                                                                CellPadding="4" ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="grdManageLogins_SelectedIndexChanged">
                                                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                                <EditRowStyle BackColor="#999999" />
                                                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                                                <HeaderStyle BackColor="#00c0ef" Font-Bold="True" ForeColor="White" />
                                                                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                               
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                <tr>
                                                <td>
                                                    <asp:Label ID="lbl_nofifier" ForeColor="Red" runat="server"> </asp:Label>
                                                </td>                                               
                                            </tr>
                                                </table>
                          </div>
                       </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            </div>
        </section>
</asp:Content>


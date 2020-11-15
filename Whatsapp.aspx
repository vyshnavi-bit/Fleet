<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="Whatsapp.aspx.cs" Inherits="Whatsapp" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <section class="content-header">
        <h1>
            Whatsapp Message<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Whatsapp Message</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Whatsapp Message
                </h3>
            </div>
            <div class="box-body">
                
                        <table  style="text-align:center; width:100%;">
                              <tr>
                                <td style="height: 40px; ">
                                <label>
                                   Whatsapp Number </label><span style="color: red;">*</span>
                                  </td>
                                <td>
                                    <asp:TextBox ID="txt_mobileno" runat="server" placeholder="Enter whatsapp Number" CssClass="form-control"></asp:TextBox>                              
                                </td>
                            <td style="width: 5px;">
                             </td>
                              <td style="text-align:left;">
                                
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 40px; ">
                                <label>
                                   Whatsapp Message Detail </label><span style="color: red;">*</span>
                                  </td>
                                <td>
                                    <asp:TextBox ID="txt_whatsappmsg" runat="server" placeholder="Enter whatsapp Messages" CssClass="form-control"></asp:TextBox>                              
                                </td>
                            <td style="width: 5px;">
                             </td>
                              <td style="text-align:left;">
                                 <asp:Button ID="btn_whatsappmsg" runat="server" Text="whatsappmsg" CssClass="btn btn-primary" OnClick="btn_whatsappmsg_Click" />  
                                </td>
                            </tr>
                            </table>
                            <asp:Label ID="Lbl_Errmsg" runat="server" Text="Label"></asp:Label>
              
                
            </div>
           
        </div>
    </section>
</asp:Content>


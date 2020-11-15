using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class ChangePassword : System.Web.UI.Page
{
    MySqlCommand cmd;
    static string UserName = "";
    static VehicleDBMgr vdm;
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected void btnSubmitt_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["Employ_Sno"] != null)
            {
                lblError.Text = "";
                UserName = Session["Employ_Sno"].ToString();
                vdm = new VehicleDBMgr();
                cmd = new MySqlCommand("SELECT emp_pwd FROM employdata WHERE Emp_sno = @Emp_sno");
                cmd.Parameters.Add("@Emp_sno", UserName);
                DataTable dt = vdm.SelectQuery(cmd).Tables[0];//"ManageData", "UserName", new string[] { "UserName=@UserName" }, new string[] { UserName }, new string[] { "" }).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    if (txtNewPassWord.Text == txtConformPassWord.Text)
                    {
                        txtNewPassWord.Text = txtConformPassWord.Text;
                        cmd = new MySqlCommand("Update employdata set emp_pwd=@emp_pwd where Emp_sno=@Emp_sno ");
                        cmd.Parameters.Add("@Emp_sno", UserName);
                        cmd.Parameters.Add("@emp_pwd", txtNewPassWord.Text.Trim());
                        vdm.Update(cmd);
                        lblMessage.Text = "Your Password has been Changed successfully";
                        Response.Redirect("Login.aspx", false);
                    }
                    else
                    {
                        lblError.Text = "Conform password not match";
                    }
                }
                else
                {
                    lblError.Text = "Entered username is incorrect";
                }
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "Password Changed Failed";
        }
    }
}
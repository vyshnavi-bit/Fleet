using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data;
using System.Web.Services;

public partial class LogOut : System.Web.UI.Page
{
    VehicleDBMgr vdm = new VehicleDBMgr();
    MySqlCommand cmd;
    SqlCommand a_cmd;
    string ipaddress;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string sno = Session["Employ_Sno"].ToString();
            cmd = new MySqlCommand("update employdata set  loginstatus=@log where emp_sno=@sno");
            cmd.Parameters.Add("@log", "0");
            cmd.Parameters.Add("@sno", sno);
            vdm.Update(cmd);

            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            cmd = new MySqlCommand("Select max(sno) as transno from login_info where empid=@userid");
            cmd.Parameters.Add("@userid", Session["Employ_Sno"]);
            cmd.Parameters.Add("@UserName", Session["UserName"]);
            DataTable dttime = vdm.SelectQuery(cmd).Tables[0];
            if (dttime.Rows.Count > 0)
            {
                string transno = dttime.Rows[0]["transno"].ToString();
                cmd = new MySqlCommand("UPDATE login_info set logouttime=@logouttime where sno=@sno");
                cmd.Parameters.Add("@logouttime", ServerDateCurrentdate);
                cmd.Parameters.Add("@sno", transno);
                vdm.Update(cmd);
            }

            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            //  window.localStorage.clear();
            //ClearCache();
            //clearchachelocalall();
            Response.Redirect("Default.aspx");
        }
        catch
        {
        }
    }
}
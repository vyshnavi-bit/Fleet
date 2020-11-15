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
using System.Net.Mail;
using System.Net;
using System.Text;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Security.Cryptography;
using System.Net.Sockets;
using System.Text.RegularExpressions;

public partial class Login : System.Web.UI.Page
{
    VehicleDBMgr vdm = new VehicleDBMgr();
    MySqlCommand cmd;
    SqlCommand a_cmd;
    string ipaddress;
    public string username, pwd;
    AccessControldbmanger Accescontrol_db = new AccessControldbmanger();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Employ_Sno"] == "" || Session["Employ_Sno"] == null)
        {
            
        }
        else
        {
            if (Session["leveltype"] == "weigher")
            {
                Response.Redirect("Trip_assign.aspx", false);
            }
            else
            {
                Response.Redirect("DashBoard_alerts.aspx", false);
            }
        }
        username = string.Empty;
        pwd = string.Empty;
        username = Request.QueryString["username"];
        pwd = Request.QueryString["pwd"];
        try
        {
            if (username.Length > 0 && username != null && username != "")
            {
                Usernme_txt.Text = username.Trim();
                Pass_pas.Text = pwd.Trim();
                LoginInfo();
            }
        }
        catch (Exception ex)
        {
        }
        //try
        //{
        //    if (Empid == null)
        //    {
        //        Response.Write("<script></script>");
        //        return;
        //    }
        //    a_cmd = new SqlCommand("SELECT   sno, emp_refno, emp_sno, branch_sno AS brnch_sno, emp_type, branch_address AS brnch_address, branchtype, superbranch, doe, title, address, empname, password FROM fleet_login where  emp_sno=@empid");
        //    a_cmd.Parameters.Add("@empid", Empid);
        //    DataTable dt = Accescontrol_db.SelectQuery(a_cmd).Tables[0];
        //    if (dt.Rows.Count > 0)
        //    {
        //        string username = dt.Rows[0]["empname"].ToString();
        //        cmd = new MySqlCommand("SELECT branchmappingtable.subbranch, branchmappingtable.superbranch FROM branchmappingtable INNER JOIN employdata ON branchmappingtable.subbranch = employdata.branch_id WHERE (employdata.emp_login_id = @Username)");
        //        cmd.Parameters.Add("@Username", username);
        //        DataTable dtBranch = vdm.SelectQuery(cmd).Tables[0];
        //        string PlantID = "";
        //        string Branch = dt.Rows[0]["brnch_sno"].ToString();
        //        if (dtBranch.Rows.Count > 0)
        //        {
        //            PlantID = dtBranch.Rows[0]["superbranch"].ToString();
        //            if (PlantID == "1" || Branch == "1")
        //            {
        //                Session["TitleName"] = "SRI VYSHNAVI DAIRY SPECIALITIES (P) LTD";
        //                Session["Address"] = "R.S.No:381/2,Punabaka village Post,Pellakuru Mandal,Nellore District -524129., ANDRAPRADESH (State).Phone: 9440622077, Fax: 044 – 26177799. , ";
        //                Session["Address1"] = "R.S.No:381/2,Punabaka village Post,";
        //                Session["Address2"] = "Pellakuru Mandal,Nellore District -524129.";
        //                Session["Address3"] = "ANDRAPRADESH (State).Phone: 9440622077, Fax: 044 – 26177799.";
        //                Session["TinNo"] = "37921042267";
        //            }
        //            else
        //            {
        //                Session["TitleName"] = "SRI VYSHNAVI FOODS (P) LTD";
        //                Session["Address"] = " Near Ayyappa Swamy Temple, Shasta Nagar, WYRA-507165,KHAMMAM (District), TELANGANA (State).Phone: 08749 – 251326, Fax: 08749 – 252198.";
        //                Session["TinNo"] = "36550160129";
        //            }
        //        }
        //        Session["Employ_Sno"] = dt.Rows[0]["emp_sno"].ToString();
        //        Session["Branch_ID"] = dt.Rows[0]["brnch_sno"].ToString();
        //        Session["Employ_Type"] = dt.Rows[0]["emp_type"].ToString();
        //        Session["Address"] = dt.Rows[0]["brnch_address"].ToString();
        //        Session["Emp_Type"] = dt.Rows[0]["emp_type"].ToString();
        //        Session["BranchType"] = dt.Rows[0]["branchtype"].ToString();
        //        Session["employname"] = dt.Rows[0]["empname"].ToString();
        //        Session["leveltype"] = dt.Rows[0]["emp_type"].ToString();
        //        //  Session["UserName"] = username;
        //        Session["MID"] = "3";
        //        string leval = dt.Rows[0]["emp_type"].ToString();
        //        if (leval == "weigher")
        //        {
        //            Response.Redirect("Trip_assign.aspx", false);
        //        }
        //        else
        //        {
        //            Response.Redirect("DashBoard_alerts.aspx", false);
        //        }
        //    }
        //    else
        //    {
        //        Response.Redirect("Login.aspx", false);
        //    }
        //}
        //catch (Exception ex)
        //{

        //}
    }

    protected void login_click(object sender, EventArgs e)
    {
        LoginInfo();
    }
    private void LoginInfo()
    {
        try
        {
            string username = Usernme_txt.Text, password = Pass_pas.Text;
            if (username == "")
            {
                Response.Write("<script>alert('Please Enter Username');</script>");
                return;
            }
            if (password == "")
            {
                Response.Write("<script>alert('Please Enter Password');</script>");
                return;
            }
            lbl_username.Text = username;
            lbl_passwords.Text = password;
            cmd = new MySqlCommand("SELECT employdata.emp_sno, employdata.employid,employdata.loginstatus, employdata.employname, employdata.branch_id, employdata.emp_pwd, branch_info.branchname, employdata.emp_status, employdata.emp_login_id, employdata.operatedby, employdata.dept_id, employdata.emp_type, employdata.emp_type,branch_info.brnch_address,branch_info.branchtype, branch_info.brnch_sno FROM employdata INNER JOIN branch_info ON employdata.branch_id = branch_info.brnch_sno WHERE (employdata.emp_login_id = @UN) AND (employdata.emp_pwd = @Pwd)");
            cmd.Parameters.Add("@UN", username);
            cmd.Parameters.Add("@Pwd", password);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string loginflag = dt.Rows[0]["loginstatus"].ToString();
                ////if (loginflag == "1")
                ////{
                ////    this.AlertBoxMessage.InnerText = "Already Some one Login With This User Name";
                ////    this.AlertBox.Visible = true;
                ////}
                ////else
                ////{
                cmd = new MySqlCommand("SELECT branchmappingtable.subbranch, branchmappingtable.superbranch, branch_info.shortname FROM branchmappingtable INNER JOIN employdata ON branchmappingtable.subbranch = employdata.branch_id INNER JOIN branch_info ON branchmappingtable.subbranch = branch_info.brnch_sno WHERE (employdata.emp_login_id = @Username)");
                cmd.Parameters.Add("@Username", username);
                DataTable dtBranch = vdm.SelectQuery(cmd).Tables[0];
                string PlantID = "";
                string Branch = dt.Rows[0]["branch_id"].ToString();
                if (dtBranch.Rows.Count > 0)
                {
                    PlantID = dtBranch.Rows[0]["superbranch"].ToString();
                    if (PlantID == "1" || Branch == "1")
                    {
                        Session["TitleName"] = "SRI VYSHNAVI DAIRY SPECIALITIES (P) LTD";
                        Session["Address"] = "R.S.No:381/2,Punabaka village Post,Pellakuru Mandal,Nellore District -524129., ANDRAPRADESH (State).Phone: 9440622077, Fax: 044 – 26177799. , ";
                        Session["Address1"] = "R.S.No:381/2,Punabaka village Post,";
                        Session["Address2"] = "Pellakuru Mandal,Nellore District -524129.";
                        Session["Address3"] = "ANDRAPRADESH (State).Phone: 9440622077, Fax: 044 – 26177799.";
                        Session["TinNo"] = "37921042267";
                    }
                    else
                    {
                        Session["TitleName"] = "SRI VYSHNAVI FOODS (P) LTD";
                        Session["Address"] = " Near Ayyappa Swamy Temple, Shasta Nagar, WYRA-507165,KHAMMAM (District), TELANGANA (State).Phone: 08749 – 251326, Fax: 08749 – 252198.";
                        Session["TinNo"] = "36550160129";
                    }
                }
                Session["shortname"] = dtBranch.Rows[0]["shortname"].ToString();
                Session["Employ_Sno"] = dt.Rows[0]["emp_sno"].ToString();
                Session["Branch_ID"] = dt.Rows[0]["brnch_sno"].ToString();
                Session["Employ_Type"] = dt.Rows[0]["emp_type"].ToString();
                Session["Address"] = dt.Rows[0]["brnch_address"].ToString();
                Session["Emp_Type"] = dt.Rows[0]["emp_type"].ToString();
                Session["BranchType"] = dt.Rows[0]["branchtype"].ToString();
                Session["employname"] = dt.Rows[0]["employname"].ToString();
                Session["UserName"] = username;

                Session["MID"] = "3";

                ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (ipaddress == "" || ipaddress == null)
                {
                    ipaddress = Request.ServerVariables["REMOTE_ADDR"];
                }
                HttpBrowserCapabilities browser = Request.Browser;
                string devicetype = "";
                string userAgent = Request.ServerVariables["HTTP_USER_AGENT"];
                Regex OS = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                Regex device = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                string device_info = string.Empty;
                if (OS.IsMatch(userAgent))
                {
                    device_info = OS.Match(userAgent).Groups[0].Value;
                }
                if (device.IsMatch(userAgent.Substring(0, 4)))
                {
                    device_info += device.Match(userAgent).Groups[0].Value;
                }
                if (!string.IsNullOrEmpty(device_info))
                {
                    devicetype = device_info;
                    string[] words = devicetype.Split(')');
                    devicetype = words[0].ToString();
                }
                else
                {
                    devicetype = "Desktop";
                }
                //End
                cmd = new MySqlCommand("update employdata set loginstatus=@log where emp_login_id=@username and emp_pwd=@passward");
                cmd.Parameters.Add("@log", "1");
                cmd.Parameters.Add("@username", username);
                cmd.Parameters.Add("@passward", password);
                vdm.Update(cmd);
                DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
                cmd = new MySqlCommand("INSERT INTO login_info(empid, logintime, devicetype, ipaddress) values (@userid, @logintime,@devicetype, @ipaddress)");
                cmd.Parameters.Add("@userid", dt.Rows[0]["emp_sno"].ToString());
                cmd.Parameters.Add("@UserName", Session["UserName"]);
                cmd.Parameters.Add("@logintime", ServerDateCurrentdate);
                cmd.Parameters.Add("@ipaddress", ipaddress);
                cmd.Parameters.Add("@devicetype", devicetype);
                vdm.insert(cmd);
                string leval = dt.Rows[0]["emp_type"].ToString();
                if (leval == "weigher")
                {
                    Response.Redirect("Trip_assign.aspx", false);
                }
                else
                {
                    Response.Redirect("DashBoard_alerts.aspx", false);
                }
                ////}
            }
            else
            {
                lbl_validation.Text = "Username and password Not found";
            }
        }
        catch (Exception ex)
        {
            lbl_validation.Text = ex.Message;
        }
    }

    protected void sessionsclick_click(object sender, EventArgs e)
    {
        try
        {
            string username = lbl_username.Text.ToString();
            string password = lbl_passwords.Text.ToString();
            cmd = new MySqlCommand("update employdata set loginstatus=@log where emp_login_id=@username and emp_pwd=@passward");
            cmd.Parameters.Add("@log", "0");
            cmd.Parameters.Add("@username", username);
            cmd.Parameters.Add("@passward", password);
            vdm.Update(cmd);
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            cmd = new MySqlCommand("SELECT   emp_sno FROM  employdata WHERE  (emp_login_id = @username) AND (emp_pwd = @passward)");
            cmd.Parameters.Add("@username", username);
            cmd.Parameters.Add("@passward", password);
            DataTable dtEMP = vdm.SelectQuery(cmd).Tables[0];
            if (dtEMP.Rows.Count > 0)
            {
                string empid = dtEMP.Rows[0]["emp_sno"].ToString();
                cmd = new MySqlCommand("Select max(sno) as transno from login_info where empid=@userid");
                cmd.Parameters.Add("@userid", empid);
                DataTable dttime = vdm.SelectQuery(cmd).Tables[0];
                if (dttime.Rows.Count > 0)
                {
                    string transno = dttime.Rows[0]["transno"].ToString();
                    cmd = new MySqlCommand("UPDATE login_info set logouttime=@logouttime where sno=@sno");
                    cmd.Parameters.Add("@logouttime", ServerDateCurrentdate);
                    cmd.Parameters.Add("@sno", transno);
                    vdm.Update(cmd);
                }
            }
            this.AlertBox.Visible = false;
        }
        catch
        {

        }
    }
    protected void sessionsclick_Close(object sender, EventArgs e)
    {
        this.AlertBox.Visible = false;
        Response.Redirect("login.aspx");
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.Net;
using System.IO;

public partial class DriverDetails : System.Web.UI.Page
{
    MySqlCommand cmd;
    string BranchID = "";
    VehicleDBMgr vdm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Branch_ID"] == null)
            Response.Redirect("Login.aspx");
        else
        {
            BranchID = Session["Branch_ID"].ToString();
            vdm = new VehicleDBMgr();
            if (!Page.IsPostBack)
            {
                if (!Page.IsCallback)
                {
                    GetReport();
                    lblAddress.Text = Session["Address"].ToString();
                    lblTitle.Text = Session["TitleName"].ToString();
                }
            }
        }
    }
    DataTable Report = new DataTable();
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            DataTable dtDriver = new DataTable();
            Report.Columns.Add("Sno");
            Report.Columns.Add("EMPSno");
            Report.Columns.Add("Emp ID");
            Report.Columns.Add("Driver Name");
            Report.Columns.Add("Licence No");
            Report.Columns.Add("Licence Exp Date");
            Report.Columns.Add("Mob No");
            Report.Columns.Add("Address");
            Report.Columns.Add("Blood Group");
            Report.Columns.Add("Experience");
            Report.Columns.Add("DOJ");
            Report.Columns.Add("Image");
            cmd = new MySqlCommand("SELECT employdata.emp_sno, employdata.employid, employdata.employname, employdata.emp_dob, employdata.emp_licencenum,DATE_FORMAT(employdata.emp_licenceexpire, '%d %b %y') as LicenceExpDate , employdata.emp_status, employdata.emp_type,employdata.emp_login_id, employdata.emp_pwd, employdata.operatedby, minimasters.mm_name, branch_info.branchname,employdata.emp_bloodgrp, employdata.emp_address,DATE_FORMAT(employdata.emp_doj, '%d %b %y') as DOJ, employdata.emp_experience,employdata.Phoneno,employdata.gender, employdata.fathernme,employdata.eduqual, employdata.techqual, employdata.bankac, employdata.marital, employdata.nationality FROM employdata INNER JOIN minimasters ON employdata.dept_id = minimasters.sno INNER JOIN branch_info ON employdata.branch_id = branch_info.brnch_sno WHERE (employdata.branch_id = @BranchID) AND (employdata.emp_type = 'Driver') and (employdata.flag<>0)");
            cmd.Parameters.Add("@BranchID", BranchID);
            dtDriver = vdm.SelectQuery(cmd).Tables[0];
            if (dtDriver.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in dtDriver.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    newrow["EMPSno"] = dr["emp_sno"].ToString();
                    newrow["Emp ID"] = dr["employid"].ToString();
                    newrow["Driver Name"] = dr["employname"].ToString();
                    newrow["Licence No"] = dr["emp_licencenum"].ToString();
                    newrow["Licence Exp Date"] = dr["LicenceExpDate"].ToString();
                    newrow["Mob No"] = dr["Phoneno"].ToString();
                    newrow["Address"] = dr["emp_address"].ToString();
                    newrow["Blood Group"] = dr["emp_bloodgrp"].ToString();
                    newrow["Experience"] = dr["emp_experience"].ToString();
                    newrow["DOJ"] = dr["DOJ"].ToString();
                    Report.Rows.Add(newrow);
                }
                Session["title"] = "DriverDetailsReport";
                Session["filename"] = "DriverDetailsReport";
                Session["xportdata"] = Report;
                grdEmployee.DataSource = Report;
                grdEmployee.DataBind();
            }
            else
            {
                lblmsg.Text = "No data found";
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            grdEmployee.DataSource = Report;
            grdEmployee.DataBind();
        }
    }
}
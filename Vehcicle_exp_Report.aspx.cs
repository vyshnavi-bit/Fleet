using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;

public partial class Vehcicle_exp_Report : System.Web.UI.Page
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
                    txt_FromDate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                    txt_Todate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                    lblAddress.Text = Session["Address"].ToString();
                    lblTitle.Text = Session["TitleName"].ToString();
                }
            }
        }
    }
    private DateTime GetLowDate(DateTime dt)
    {
        double Hour, Min, Sec;
        DateTime DT = DateTime.Now;
        DT = dt;
        Hour = -dt.Hour;
        Min = -dt.Minute;
        Sec = -dt.Second;
        DT = DT.AddHours(Hour);
        DT = DT.AddMinutes(Min);
        DT = DT.AddSeconds(Sec);
        return DT;
    }
    private DateTime GetHighDate(DateTime dt)
    {
        double Hour, Min, Sec;
        DateTime DT = DateTime.Now;
        Hour = 23 - dt.Hour;
        Min = 59 - dt.Minute;
        Sec = 59 - dt.Second;
        DT = dt;
        DT = DT.AddHours(Hour);
        DT = DT.AddMinutes(Min);
        DT = DT.AddSeconds(Sec);
        return DT;
    }
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();

        if (ddlType.SelectedValue == "Vehicle Wise")
        {
            hideVehicles.Visible = true;
            cmd = new MySqlCommand("SELECT minimasters.mm_name, vehicel_master.registration_no, vehicel_master.vm_sno FROM vehicel_master INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno WHERE (vehicel_master.branch_id = @BranchID)");
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
            ddlvehicles.DataSource = dttrips;
            ddlvehicles.DataTextField = "registration_no";
            ddlvehicles.DataValueField = "vm_sno";
            ddlvehicles.DataBind();
        }
        if (ddlType.SelectedValue == "All")
        {
            hideVehicles.Visible = false;
            ddlvehicles.Items.Clear();
        }
        if (ddlType.SelectedValue == "Select Type")
        {
            hideVehicles.Visible = false;
            ddlvehicles.Items.Clear();
        }

    }
    DataTable Report = new DataTable();
    protected void btn_Generate_Click(object sender, EventArgs e)
    {
        try
        {
            lblmsg.Text = "" ;
            DateTime fromdate = DateTime.Now;
            DateTime todate = DateTime.Now;
            string[] datestrig = txt_FromDate.Text.Split(' ');
            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig[0].Split('-');
                    string[] times = datestrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            datestrig = txt_Todate.Text.Split(' ');
            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig[0].Split('-');
                    string[] times = datestrig[1].Split(':');
                    todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            lblType.Text = ddlType.SelectedItem.Text;
            lblFromDate.Text = fromdate.ToString("dd/MM/yyyy");
            lbltodate.Text = todate.ToString("dd/MM/yyyy");
            if (ddlType.SelectedValue == "All")
            {
                Report.Columns.Add("Sno");
                Report.Columns.Add("VehicleNo");
                Report.Columns.Add("Name");
                Report.Columns.Add("Head Name");
                Report.Columns.Add("Amount").DataType = typeof(Double);
                Report.Columns.Add("Incharge");
                Report.Columns.Add("Remarks");
                cmd = new MySqlCommand("SELECT     veh_exp.sno,   veh_exp.name, head_master.head_desc, veh_exp.remarks, veh_exp.incharge, sub_veh_exp.amount, vehicel_master.registration_no FROM veh_exp INNER JOIN sub_veh_exp ON veh_exp.sno = sub_veh_exp.refno INNER JOIN head_master ON sub_veh_exp.head_sno = head_master.sno INNER JOIN vehicel_master ON veh_exp.vehsno = vehicel_master.vm_sno WHERE        (veh_exp.branchid = @BranchID) AND (veh_exp.doe BETWEEN @d1 AND @d2)");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", BranchID);
                DataTable dtExp = vdm.SelectQuery(cmd).Tables[0];
                if (dtExp.Rows.Count > 0)
                {
                    hidepanel.Visible = true;
                    int i = 1;
                    foreach (DataRow dr in dtExp.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["Sno"] = dr["sno"].ToString();
                        newrow["VehicleNo"] = dr["registration_no"].ToString();
                        newrow["Name"] = dr["name"].ToString();
                        newrow["Head Name"] = dr["head_desc"].ToString();
                        double amount = 0;
                        double.TryParse(dr["amount"].ToString(), out amount);
                        newrow["Amount"] = amount;
                        newrow["Incharge"] = dr["incharge"].ToString();
                        newrow["Remarks"] = dr["remarks"].ToString();
                        Report.Rows.Add(newrow);
                    }
                    DataRow newvartical2 = Report.NewRow();
                    newvartical2["Head Name"] = "Total";
                    double val = 0.0;
                    foreach (DataColumn dc in Report.Columns)
                    {
                        if (dc.DataType == typeof(Double))
                        {
                            val = 0.0;
                            double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                            newvartical2[dc.ToString()] = val;
                        }
                    }
                    Report.Rows.Add(newvartical2);
                    grdReports.DataSource = Report;
                    grdReports.DataBind();
                    Session["xportdata"] = Report;
                }
                else
                {
                    hidepanel.Visible = false;
                    lblmsg.Text = "No data were found";
                }
            }
            if (ddlType.SelectedValue == "Vehicle Wise")
            {
                lblVehicleNo.Text = ddlvehicles.SelectedItem.Text;
                Report.Columns.Add("Sno");
                Report.Columns.Add("Name");
                Report.Columns.Add("Head Name");
                Report.Columns.Add("Amount").DataType = typeof(Double);
                Report.Columns.Add("Incharge");
                Report.Columns.Add("Remarks");
                cmd = new MySqlCommand("SELECT veh_exp.name, head_master.head_desc, veh_exp.remarks, veh_exp.incharge, sub_veh_exp.amount FROM veh_exp INNER JOIN sub_veh_exp ON veh_exp.sno = sub_veh_exp.refno INNER JOIN head_master ON sub_veh_exp.head_sno = head_master.sno WHERE  (veh_exp.branchid = @BranchID) AND (veh_exp.doe BETWEEN @d1 AND @d2) AND (veh_exp.vehsno = @Veh_ID)");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@Veh_ID", ddlvehicles.SelectedValue);
                DataTable dtExp = vdm.SelectQuery(cmd).Tables[0];
                if (dtExp.Rows.Count > 0)
                {
                    hidepanel.Visible = true;
                    int i = 1;
                    foreach (DataRow dr in dtExp.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["Sno"] = i++.ToString();
                        newrow["Name"] = dr["name"].ToString();
                        newrow["Head Name"] = dr["head_desc"].ToString();
                        double amount = 0;
                        double.TryParse(dr["amount"].ToString(), out amount);
                        newrow["Amount"] = amount;
                        newrow["Incharge"] = dr["incharge"].ToString();
                        newrow["Remarks"] = dr["remarks"].ToString();
                        Report.Rows.Add(newrow);
                    }
                    DataRow newvartical2 = Report.NewRow();
                    newvartical2["Head Name"] = "Total";
                    double val = 0.0;
                    foreach (DataColumn dc in Report.Columns)
                    {
                        if (dc.DataType == typeof(Double))
                        {
                            val = 0.0;
                            double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                            newvartical2[dc.ToString()] = val;
                        }
                    }
                    Report.Rows.Add(newvartical2);
                    grdReports.DataSource = Report;
                    grdReports.DataBind();
                    Session["xportdata"] = Report;
                }
                else
                {
                    hidepanel.Visible = false;
                    lblmsg.Text = "No data were found";
                }
            }
        }
        catch(Exception ex)
        {
            hidepanel.Visible = false;
            lblmsg.Text = ex.Message;
        }
    }
}
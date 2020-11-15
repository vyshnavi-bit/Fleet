using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class MonthlyVehiclesExpReport : System.Web.UI.Page
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
                    dtp_FromDate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                    dtp_Todate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
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
    DataTable Report = new DataTable();
    protected void btn_Generate_Click(object sender, EventArgs e)
    {
        try
        {
            vdm = new VehicleDBMgr();
            hidepanel.Visible = true;
            DateTime fromdate = DateTime.Now;
            DateTime todate = DateTime.Now;
            string[] datestrig = dtp_FromDate.Text.Split(' ');
            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig[0].Split('-');
                    string[] times = datestrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            datestrig = dtp_Todate.Text.Split(' ');
            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig[0].Split('-');
                    string[] times = datestrig[1].Split(':');
                    todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            TimeSpan dateSpan = todate.Subtract(fromdate);
            int NoOfdays = dateSpan.Days;
            NoOfdays = NoOfdays + 2;
            lblFromDate.Text = fromdate.ToString("dd/MMM/yyyy");
            lbltodate.Text = todate.ToString("dd/MMM/yyyy");
            lblheader.Text = "Vehicles Renewal Report";
            string BranchID = Session["Branch_ID"].ToString();
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
                cmd = new MySqlCommand("SELECT minimasters.mm_name AS VehicelType, minimasters_1.mm_name AS Make, vehicel_master.registration_no, vehicel_master.Capacity, vehicel_master.vm_model, vehicel_master.vm_engine, vehicel_master.vm_chasiss, vehicel_master.vm_owner, vehicel_master.vm_rcexpdate, vehicel_master.vm_poll_exp_date, vehicel_master.vm_isurence_exp_date, vehicel_master.vm_fit_exp_date,vehicel_master.vm_roadtax_exp_date, vehicel_master.fuel_capacity FROM vehicel_master INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno WHERE (vehicel_master.branch_id = @BranchID) and (vehicel_master.vm_owner=@Owner) group by vehicel_master.registration_no order by DATE(vehicel_master.vm_rcexpdate), DATE(vehicel_master.vm_poll_exp_date), DATE(vehicel_master.vm_isurence_exp_date), DATE(vehicel_master.vm_fit_exp_date),DATE(vehicel_master.vm_roadtax_exp_date) ");
                cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dtvehicleExp = vdm.SelectQuery(cmd).Tables[0];
            if (dtvehicleExp.Rows.Count > 0)
            {
                Report = new DataTable();
                Report.Columns.Add("Sno");
                Report.Columns.Add("Vehicle No");
                Report.Columns.Add("Vehicle Type");
                Report.Columns.Add("Vehicle Make");
                Report.Columns.Add("Model");
                Report.Columns.Add("Capacity");
                Report.Columns.Add("Insurannce Exp Date");
                Report.Columns.Add("Pollution Exp Date");
                Report.Columns.Add("Fitness Exp Date");
                Report.Columns.Add("RoadTax Exp Date");
                Report.Columns.Add("Permitt Exp Date");
                int i = 1;
                foreach (DataRow branch in dtvehicleExp.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    newrow["Vehicle No"] = branch["registration_no"].ToString();
                    newrow["Vehicle Type"] = branch["VehicelType"].ToString();
                    newrow["Vehicle Make"] = branch["Make"].ToString();
                    newrow["Model"] = branch["vm_model"].ToString();
                    newrow["Capacity"] = branch["Capacity"].ToString();
                    string InsExpDate = branch["vm_isurence_exp_date"].ToString();
                    if (InsExpDate == "")
                    {
                        newrow["Insurannce Exp Date"] = "No Data";
                    }
                    else
                    {
                        DateTime dtInsExpDate = Convert.ToDateTime(InsExpDate);
                        string strInsexpdate = "";
                        //if (todate > dtInsExpDate)
                        //{
                            strInsexpdate = dtInsExpDate.ToString("dd/MMM/yyyy");
                        //}
                        newrow["Insurannce Exp Date"] = strInsexpdate;
                    }
                    string PolExpDate = branch["vm_poll_exp_date"].ToString();
                    if (PolExpDate == "")
                    {
                        newrow["Pollution Exp Date"] = "No Data";
                    }
                    else
                    {
                        DateTime dtPolExpDate = Convert.ToDateTime(PolExpDate);
                        string strPolexpdate = "";
                        //if (todate > dtPolExpDate)
                        //{
                            strPolexpdate = dtPolExpDate.ToString("dd/MMM/yyyy");
                        //}
                        newrow["Pollution Exp Date"] = strPolexpdate;
                    }
                    string FitExpDate = branch["vm_fit_exp_date"].ToString();
                    if (FitExpDate == "")
                    {
                        newrow["Fitness Exp Date"] = "No Data";
                    }
                    else
                    {
                        DateTime dtFitExpDate = Convert.ToDateTime(FitExpDate);
                        string strFitExpDate = "";
                        //if (todate > dtFitExpDate)
                        //{
                            strFitExpDate = dtFitExpDate.ToString("dd/MMM/yyyy");
                        //}
                        newrow["Fitness Exp Date"] = strFitExpDate;
                    }
                    string RoadtaxExpDate = branch["vm_roadtax_exp_date"].ToString();
                    if (RoadtaxExpDate == "")
                    {
                        newrow["RoadTax Exp Date"] = "No Data";
                    }
                    else
                    {
                        DateTime dtRoadtaxExpDate = Convert.ToDateTime(RoadtaxExpDate);
                        string strRoadtaxExpDate = "";
                        //if (todate > dtRoadtaxExpDate)
                        //{
                            strRoadtaxExpDate = dtRoadtaxExpDate.ToString("dd/MMM/yyyy");
                        //}
                        newrow["RoadTax Exp Date"] = strRoadtaxExpDate;
                    }
                    string PermittExpDate = branch["vm_rcexpdate"].ToString();
                    if (PermittExpDate == "")
                    {
                        newrow["Permitt Exp Date"] = "No Data";
                    }
                    else
                    {
                        DateTime dtPermittExpDate = Convert.ToDateTime(PermittExpDate);
                        string strPermittExpDate = "";
                        //if (todate > dtPermittExpDate)
                        //{
                            strPermittExpDate = dtPermittExpDate.ToString("dd/MMM/yyyy");
                        //}
                        newrow["Permitt Exp Date"] = strPermittExpDate;
                    }
                    Report.Rows.Add(newrow);
                }
                grdReports.DataSource = Report;
                grdReports.DataBind();
            }
            else
            {
                hidepanel.Visible = false;
                lblmsg.Text = "No Data were found";
                grdReports.DataSource = Report;
                grdReports.DataBind();
            }
        }
        catch(Exception ex)
        {
            hidepanel.Visible = false;
            lblmsg.Text = ex.Message;
        }
    }
}
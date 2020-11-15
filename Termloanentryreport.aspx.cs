using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;

public partial class Termloanentryreport : System.Web.UI.Page
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
                    lblAddress.Text = Session["Address"].ToString();
                    lblTitle.Text = Session["TitleName"].ToString();
                    dtp_FromDate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                }
            }
        }
    }
    protected void btn_Generate_Click(object sender, EventArgs e)
    {
        try
        {
            GetReport();
        }
        catch
        {
        }
    }
    DataTable Report = new DataTable();
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            DataTable dtTermLoans = new DataTable();
            Report.Columns.Add("Bank Name");
            Report.Columns.Add("Sno");
            Report.Columns.Add("Vehicle No");
            Report.Columns.Add("Make");
            Report.Columns.Add("Vehicle Type");
            Report.Columns.Add("Type");
            Report.Columns.Add("MFG Date");
            Report.Columns.Add("Capacity");
            Report.Columns.Add("Term Loan No");
            Report.Columns.Add("Loan Amount").DataType = typeof(Double);
            Report.Columns.Add("Instalment Amount").DataType = typeof(Double);
            Report.Columns.Add("Instalment Date");
            Report.Columns.Add("Total Instalment").DataType = typeof(Double);
            Report.Columns.Add("Paid").DataType = typeof(Double);
            Report.Columns.Add("Remaining").DataType = typeof(Double);
            Report.Columns.Add("Balance").DataType = typeof(Double);
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
            lblMonth.Text = fromdate.ToString("MMM/dd/yyyy");
            fromdate = fromdate.AddMonths(-1);
            string strmonth = fromdate.ToString("MMM/dd/yyyy");
            cmd = new MySqlCommand("SELECT  vehicel_master.registration_no AS VehicleNo,DATE_FORMAT( termloanentry.mfgdate, '%d %b %y') as mfgdate,DATE_FORMAT( termloanentry.termloandate, '%d %b %y') as termloandate,termloanentry.type,termloanentry.sno, termloanentry.termloanno,termloanentry.loanamount, termloanentry.instalamount,DATE_FORMAT(termloanentry.instaldate, '%d %b %y') as instaldate , termloanentry.totalinstall, termloanentry.com_install, termloanentry.bankname, vehicel_master.Capacity, minimasters.mm_name AS VehicleType, minimasters_1.mm_code AS Make FROM termloanentry INNER JOIN vehicel_master ON termloanentry.vehsno = vehicel_master.vm_sno INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno   where (vehicel_master.vm_owner=@Owner)    Group by vehicel_master.registration_no,termloanentry.Type order by termloanentry.Bankname");
            cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
            cmd.Parameters.Add("@BranchID", BranchID);
            dtTermLoans = vdm.SelectQuery(cmd).Tables[0];
            if (dtTermLoans.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in dtTermLoans.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    newrow["Bank Name"] = dr["bankname"].ToString();
                    newrow["Vehicle No"] = dr["VehicleNo"].ToString();
                    newrow["Make"] = dr["Make"].ToString();
                    newrow["Vehicle Type"] = dr["VehicleType"].ToString();
                    newrow["Type"] = dr["type"].ToString();
                    newrow["MFG Date"] = dr["mfgdate"].ToString();
                    newrow["Capacity"] = dr["Capacity"].ToString();
                    newrow["Term Loan No"] = dr["termloanno"].ToString();
                    newrow["Loan Amount"] = dr["loanamount"].ToString();
                    double instalamount = 0;
                    double.TryParse(dr["instalamount"].ToString(), out instalamount);
                    newrow["Instalment Amount"] = instalamount.ToString();
                    string instaldate = dr["instaldate"].ToString();
                    DateTime dtinstaldate = Convert.ToDateTime(instaldate);
                    string strdate = dtinstaldate.ToString("dd/MMM/yyyy");
                    string[] strarray = strdate.Split('/');
                    string[] newmonth = strmonth.Split('/');
                    string install = strarray[0] + "/" + newmonth[0] + "/" + newmonth[2];
                    newrow["Instalment Date"] = install.ToString();
                    int totalinstall = 0;
                    int.TryParse(dr["totalinstall"].ToString(), out totalinstall);
                    newrow["Total Instalment"] = totalinstall.ToString();
                    string termloandate = dr["termloandate"].ToString();
                    //string strtermdate = newmonth[1];
                    //int termdate = 0;
                    //int.TryParse(strtermdate, out termdate);
                    DateTime dtnewdate = Convert.ToDateTime(install);
                    DateTime dtterm = Convert.ToDateTime(termloandate);
                    TimeSpan dateSpan = dtnewdate.Subtract(dtterm);
                    int NoOfdays = dateSpan.Days;
                    int month = NoOfdays / 30;
                    //int com_install = 0;
                    //int.TryParse(dr["com_install"].ToString(), out com_install);
                    if (totalinstall > month)
                    {
                        newrow["Paid"] = month.ToString();
                    }
                    else
                    {
                        newrow["Paid"] = totalinstall.ToString();
                    }
                    int Remaining = 0;
                    Remaining = totalinstall - month;
                    if (Remaining < 0)
                    {
                        Remaining = 0;
                    }
                    newrow["Remaining"] = Remaining.ToString();
                    double balance = 0;
                    balance = Remaining * instalamount;
                    balance = Math.Round(balance, 2);
                    newrow["Balance"] = balance.ToString();

                    Report.Rows.Add(newrow);
                }
                DataRow New = Report.NewRow();
                New["Vehicle No"] = "Total";
                double valnewCash = 0.0;
                foreach (DataColumn dc in Report.Columns)
                {
                    if (dc.DataType == typeof(Double))
                    {
                        valnewCash = 0.0;
                        double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out valnewCash);
                        New[dc.ToString()] = valnewCash;
                    }
                }
                Report.Rows.Add(New);
                Session["title"] = "VehicleReport";
                Session["filename"] = "VehicleReport";
                Session["xportdata"] = Report;
                grdReports.DataSource = Report;
                grdReports.DataBind();
            }
            else
            {
                lblmsg.Text = "No data found";
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }

    protected void gvMenu_DataBinding(object sender, EventArgs e)
    {
        try
        {
            GridViewGroup First = new GridViewGroup(grdReports, null, "Bank Name");
           // GridViewGroup thired = new GridViewGroup(grdReports, null, "Type");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void grdReports_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TableCell cell = e.Row.Cells[1];
            TableCell cell1 = e.Row.Cells[0];
            if (cell1.Text == "DAIMLER ING" || cell1.Text == "HDFC BANK" || cell1.Text == "ING VYSHYA BANK")
            {
                cell1.BackColor = System.Drawing.Color.White;
            }
            if (cell1.Text == "12" || cell1.Text == "1")
            {
                cell1.BackColor = System.Drawing.Color.White;
            }
            if (cell1.Text == "37")
            {
                cell1.BackColor = System.Drawing.Color.White;
            }
            if (e.Row.Cells[14].Text == "0")
            {

                e.Row.BackColor = System.Drawing.Color.YellowGreen;
                e.Row.Font.Bold = true;

            }
            if (e.Row.Cells[14].Text == "1")
            {
                e.Row.BackColor = System.Drawing.Color.DeepSkyBlue;
                e.Row.Font.Bold = true;

            }
            if (e.Row.Cells[14].Text == "2")
            {
                e.Row.BackColor = System.Drawing.Color.DeepSkyBlue;
                e.Row.Font.Bold = true;

            }
            if (e.Row.Cells[14].Text == "3")
            {
                e.Row.BackColor = System.Drawing.Color.DeepSkyBlue;
                e.Row.Font.Bold = true;

            }
            if (e.Row.Cells[14].Text == "4")
            {
                e.Row.BackColor = System.Drawing.Color.DeepSkyBlue;
                e.Row.Font.Bold = true;

            }
            if (e.Row.Cells[14].Text == "5")
            {
                e.Row.BackColor = System.Drawing.Color.DeepSkyBlue;
                e.Row.Font.Bold = true;

            }
            if (e.Row.Cells[14].Text == "6")
            {
                e.Row.BackColor = System.Drawing.Color.LightSteelBlue;
                e.Row.Font.Bold = true;

            }
            if (e.Row.Cells[14].Text == "7")
            {
                e.Row.BackColor = System.Drawing.Color.LightSteelBlue;
                e.Row.Font.Bold = true;

            }
            if (e.Row.Cells[14].Text == "8")
            {
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Bold = true;

            }
            if (e.Row.Cells[14].Text == "9")
            {
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Bold = true;

            }
            if (e.Row.Cells[14].Text == "10")
            {
                e.Row.BackColor = System.Drawing.Color.LightBlue;
                e.Row.Font.Bold = true;

            }
            if (e.Row.Cells[14].Text == "11")
            {
                e.Row.BackColor = System.Drawing.Color.LightBlue;
                e.Row.Font.Bold = true;

            }
            //TableCell cell = e.Row.Cells[1];
            //int quantity = int.Parse(cell.Text);
            //if (quantity == 0)
            //{
            //    cell.BackColor = System.Drawing.Color.Red;
            //}
        }

    }
}
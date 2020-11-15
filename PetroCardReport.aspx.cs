using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class PetroCardReport : System.Web.UI.Page
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
            DataTable trips = new DataTable();
            Report.Columns.Add("Sno");
            Report.Columns.Add("Cardname");
            Report.Columns.Add("ACCOUNT ID");
            Report.Columns.Add("DEALER NAME");
            Report.Columns.Add("DEALER CITY");
            Report.Columns.Add("TRANSACTION DATE");
            Report.Columns.Add("ACCOUNTING DATE");
            Report.Columns.Add("TRANSACTION TYPE");
            Report.Columns.Add("CURRENCY");
            Report.Columns.Add("AMOUNT");
            Report.Columns.Add("VOLUME/DOC NO");
            Report.Columns.Add("AMOUNT BALANCE");
            Report.Columns.Add("PETROMILES EARNED");
            Report.Columns.Add("ODOMETER READING");
            Report.Columns.Add("vehicleno");
            Report.Columns.Add("vehicleType");
            Report.Columns.Add("Remarks");
            lblmsg.Text = "";
            VehicleDBMgr SalesDB = new VehicleDBMgr();
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
            lblFromDate.Text = fromdate.ToString("dd/MMM/yyyy");
            lbltodate.Text = todate.ToString("dd/MMM/yyyy");
            lblHeading.Text = " Petro Card Report";
            Session["filename"] = "PetroCard Report";
            Session["title"] = "PetroCard Report";
            cmd = new MySqlCommand("SELECT sno, cardname, accountid, dealername, dealercity, transactiondate, accountingdate,vehicleType, tranactiontype, currency, amount, volume, amountbalance, petromiles_earned, odometer, vehicleno, remarks FROM petrocard_details where transactiondate between @fromdate and @todate");
            cmd.Parameters.Add("@fromdate", GetLowDate(fromdate));
            cmd.Parameters.Add("@todate", GetHighDate(todate));
            DataTable dtoutward = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtoutward.Rows.Count > 0)
            {
                var i = 1;
                //string date=string.Empty;
                //int status=0;
                //string temp = string.Empty;
                foreach (DataRow dr in dtoutward.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["sno"] = i++.ToString();
                    newrow["Cardname"] = dr["cardname"].ToString();
                    newrow["ACCOUNT ID"] = dr["accountid"].ToString();
                    newrow["DEALER NAME"] = dr["dealername"].ToString();
                    newrow["DEALER CITY"] = dr["dealercity"].ToString();
                    newrow["TRANSACTION DATE"] = dr["transactiondate"].ToString();
                    newrow["ACCOUNTING DATE"] = dr["accountingdate"].ToString();
                    newrow["TRANSACTION TYPE"] = dr["tranactiontype"].ToString();
                    newrow["CURRENCY"] = dr["currency"].ToString();
                    newrow["AMOUNT"] = dr["amount"].ToString();
                    newrow["VOLUME/DOC NO"] = dr["volume"].ToString();
                    newrow["AMOUNT BALANCE"] = dr["amountbalance"].ToString();
                    newrow["PETROMILES EARNED"] = dr["petromiles_earned"].ToString();
                    newrow["ODOMETER READING"] = dr["odometer"].ToString();
                    newrow["vehicleno"] = dr["vehicleno"].ToString();
                    newrow["vehicleType"] = dr["vehicleType"].ToString();
                    newrow["Remarks"] = dr["remarks"].ToString();
                    Report.Rows.Add(newrow);
                }
                grdReports.DataSource = Report;
                grdReports.DataBind();
                Session["xportdata"] = Report;
                hidepanel.Visible = true;
            }
            else
            {
                lblmsg.Text = "No data were found";
                hidepanel.Visible = false;
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}
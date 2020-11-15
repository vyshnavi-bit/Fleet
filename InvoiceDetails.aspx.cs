using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;
public partial class InvoiceDetails : System.Web.UI.Page
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
            lblmsg.Text = "";
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

            lblFromDate.Text = fromdate.ToString("dd/MMM/yyyy");
            lbltodate.Text = todate.ToString("dd/MMM/yyyy");
            Session["filename"] = "Invoice Details Report";
            DataTable Report = new DataTable();
            Report.Columns.Add("Sno");
            Report.Columns.Add("VehicleNo");
            Report.Columns.Add("InvoiceNo");
            Report.Columns.Add("Invoice Date");
            Report.Columns.Add("From Date");
            Report.Columns.Add("To Date");
            Report.Columns.Add("Vendor Name");
            //cmd = new MySqlCommand("SELECT DATE_FORMAT(triplogs.doe, '%d/%b/%y') AS TripDate,vehicel_master.vm_owner, locations.Location_name, triplogs.log_rank, triplogs.km, triplogs.charge, triplogs.charge * triplogs.km AS Amount, tripdata.tripsheetno, triplogs.tollgateamnt, triplogs.load_cap, triplogs.unload_cap, tripdata.endfuelvalue, vehicel_master.registration_no AS VehicleNo, employdata.employname AS DriverName, triplogs.odometer FROM  tripdata INNER JOIN triplogs ON tripdata.sno = triplogs.tripsno INNER JOIN locations ON triplogs.place = locations.sno LEFT OUTER JOIN employdata ON tripdata.driverid = employdata.emp_sno LEFT OUTER JOIN vehicel_master ON tripdata.vehicleno = vehicel_master.vm_sno WHERE (tripdata.enddate BETWEEN @d1 AND @d2) AND (tripdata.userid = @BranchID) AND (tripdata.vehicleno = @VehicleNo) AND (tripdata.status = 'C') ORDER BY tripdata.tripdate, triplogs.log_rank");
            cmd = new MySqlCommand("SELECT vehicel_master.registration_no, invoicetable.invoiceno,DATE_FORMAT(invoicetable.fromdate, '%d/%b/%y') AS fromdate ,DATE_FORMAT(invoicetable.todate, '%d/%b/%y') AS todate , vendors_info.vendorname,DATE_FORMAT(invoicetable.invoicedate, '%d/%b/%y') AS invoicedate   FROM invoicetable INNER JOIN vehicel_master ON invoicetable.vehicleno = vehicel_master.vm_sno INNER JOIN  vendors_info ON invoicetable.vendorid = vendors_info.sno WHERE (invoicetable.invoicedate BETWEEN @d1 AND @d2) AND (invoicetable.branchid = @BranchID)");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dtTrips = vdm.SelectQuery(cmd).Tables[0];
            int i = 1;
            if (dtTrips.Rows.Count > 0)
            {
                foreach (DataRow dr in dtTrips.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++;
                    newrow["VehicleNo"] = dr["registration_no"].ToString();
                    string InvoiceNo = Session["shortname"].ToString() + "/TRAN/" + dr["invoiceno"].ToString();
                    newrow["InvoiceNo"] = InvoiceNo;
                    newrow["Invoice Date"] = dr["invoicedate"].ToString();
                    newrow["From Date"] = dr["fromdate"].ToString();
                    newrow["To Date"] = dr["todate"].ToString();
                    newrow["Vendor Name"] = dr["vendorname"].ToString();
                    Report.Rows.Add(newrow);
                }
                Session["xportdata"] = Report;
                dataGridView1.DataSource = Report;
                dataGridView1.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.ToString();
            dataGridView1.DataSource = Report;
            dataGridView1.DataBind();
        }
    }
}
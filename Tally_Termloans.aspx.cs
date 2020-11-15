using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Drawing;
using System.Configuration;
using MySql.Data.MySqlClient;

public partial class termloansrally : System.Web.UI.Page
{
    MySqlCommand cmd;
    string UserName = "";
    VehicleDBMgr vdm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Branch_ID"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        if (!this.IsPostBack)
        {
            if (!Page.IsCallback)
            {
                lblTitle.Text = Session["TitleName"].ToString();
                txtFromdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
            }
        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        getgriddata();
    }

    private DateTime GetLowMonthRetrive(DateTime dt)
    {
        double Day, Hour, Min, Sec;
        DateTime DT = dt;
        DT = dt;
        Day = -dt.Day + 1;
        Hour = -dt.Hour;
        Min = -dt.Minute;
        Sec = -dt.Second;
        DT = DT.AddDays(Day);
        DT = DT.AddHours(Hour);
        DT = DT.AddMinutes(Min);
        DT = DT.AddSeconds(Sec);
        return DT;

    }
    private DateTime GetHighMonth(DateTime dt)
    {
        double Day, Hour, Min, Sec;
        DateTime DT = DateTime.Now;
        Day = 31 - dt.Day;
        Hour = 23 - dt.Hour;
        Min = 59 - dt.Minute;
        Sec = 59 - dt.Second;
        DT = dt;
        DT = DT.AddDays(Day);
        DT = DT.AddHours(Hour);
        DT = DT.AddMinutes(Min);
        DT = DT.AddSeconds(Sec);
        if (DT.Day == 3)
        {
            DT = DT.AddDays(-3);
        }
        else if (DT.Day == 2)
        {
            DT = DT.AddDays(-2);
        }
        else if (DT.Day == 1)
        {
            DT = DT.AddDays(-1);
        }
        return DT;
    }
    public void getgriddata()
    {
        try
        {
            Session["IDate"] = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            DateTime fromdate = DateTime.Now;
            DateTime todate = DateTime.Now;
            string[] dateFromstrig = txtFromdate.Text.Split(' ');
            if (dateFromstrig.Length > 1)
            {
                if (dateFromstrig[0].Split('-').Length > 0)
                {
                    string[] dates = dateFromstrig[0].Split('-');
                    string[] times = dateFromstrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            string strmonth = fromdate.ToString("MMM/dd/yyyy");
            lbl_selfromdate.Text = fromdate.ToString("dd/MM/yyyy");
            vdm = new VehicleDBMgr();
            DataTable Report = new DataTable();
            string DCNO = "";
            Report.Columns.Add("JV No.");
            Report.Columns.Add("JV Date");
            Report.Columns.Add("Ledger Name");
            Report.Columns.Add("Amount");
            Report.Columns.Add("Narration");
            Session["filename"] = " Tally Termloans" + fromdate.ToString("dd/MM/yyyy");
            cmd = new MySqlCommand("SELECT  termloanentry.bankname, vehicel_master.registration_no, termloanentry.type, termloanentry.termloandate, vehicel_master.vm_owner, vehicel_master.vm_model, termloanentry.ledgername,termloanentry.interest_per, termloanentry.instalamount, termloanentry.loanamount, termloanentry.totalinstall, termloanentry.com_install, termloanentry.instaldate, termloantransactions.amount FROM termloanentry INNER JOIN vehicel_master ON termloanentry.vehsno = vehicel_master.vm_sno INNER JOIN termloantransactions ON termloanentry.vehsno = termloantransactions.vehsno WHERE (vehicel_master.vm_owner = @Owner) AND (termloantransactions.doe BETWEEN @d1 AND @d2) GROUP BY termloanentry.type, vehicel_master.registration_no");
            cmd.Parameters.Add("@Owner", Session["shortname"].ToString());
            cmd.Parameters.Add("@d1", GetLowMonthRetrive(fromdate));
            cmd.Parameters.Add("@d2", GetHighMonth(fromdate));
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
            double totamount = 0;
            fromdate = fromdate.AddDays(-1);
            string frmdate = fromdate.ToString("dd-MM-yyyy");
            string[] strjv = frmdate.Split('-');
            foreach (DataRow branch in dtble.Rows)
            {
                DataRow newrow = Report.NewRow();
                newrow["JV No."] = DCNO + strjv[1];
                newrow["JV Date"] = fromdate.AddMonths(-1).ToString("dd-MMM-yyyy");
                newrow["Ledger Name"] = branch["ledgername"].ToString();
                double amount = 0;
                double.TryParse(branch["amount"].ToString(), out amount);
                totamount += amount;
                newrow["Amount"] = "-" + amount;
                newrow["Narration"] = "Being the vehicle interest for the month of " + fromdate.AddMonths(-1).ToString("MMM-yyyy") + " Amount " + amount + " Vehicle Number " + branch["registration_no"].ToString() + " Type " + branch["type"].ToString() + ",Emp Name  " + Session["employname"].ToString();
                Report.Rows.Add(newrow);
            }
            DataRow new_row = Report.NewRow();
            new_row["JV No."] = DCNO + strjv[1];
            new_row["JV Date"] = fromdate.AddMonths(-1).ToString("dd-MMM-yyyy");
            new_row["Ledger Name"] = "Interest on Vehicle Loans";
            new_row["Amount"] = totamount;
            new_row["Narration"] = "Being the vehicle interest for the month of " + fromdate.AddMonths(-1).ToString("MMM-yyyy") + " Amount " + totamount + ",Emp Name  " + Session["employname"].ToString();
            Report.Rows.Add(new_row);
            grdReports.DataSource = Report;

            grdReports.DataBind();
            Session["xportdata"] = Report;
        }
        catch (Exception Ex)
        {
            lblmsg.Text = Ex.Message;
            lblmsg.Visible = true;
        }
    }
}
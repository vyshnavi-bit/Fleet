using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class TyreRethreadReport : System.Web.UI.Page
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
            DataTable dtTyreRethread = new DataTable();
            cmd = new MySqlCommand("SELECT tyre_rethread.sno AS RethreadSno, tyre_rethread.sentdate, tyre_rethread.sentby, vendors_info.vendorname,tyre_rethread.no_of_tyres as NoOfTyres, tyre_rethread.expecteddate, tyre_rethread.remarks, vendors_info.vendor_mob AS PhoneNo FROM tyre_rethread INNER JOIN vendors_info ON tyre_rethread.servicingat = vendors_info.sno WHERE (tyre_rethread.branch_id = @BranchID) AND (tyre_rethread.sentdate BETWEEN @d1 AND @d2)");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@BranchID", BranchID);
            dtTyreRethread = vdm.SelectQuery(cmd).Tables[0];

            if (dtTyreRethread.Rows.Count > 0)
            {
                string title = "Tyre Rethread Report From: " + fromdate.ToString() + "  To: " + todate.ToString();
                Session["title"] = title;
                Session["filename"] = "TyresRethreadReport";
                Session["xportdata"] = dtTyreRethread;
                dataGridView1.DataSource = dtTyreRethread;
                dataGridView1.DataBind();
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
    
    protected void grdReports_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = Convert.ToInt32(e.CommandArgument);
        GridViewRow row = dataGridView1.Rows[rowIndex];
        string RethreadSno = row.Cells[1].Text;
        Session["TyreRethreadSno"] = RethreadSno;
        Response.Redirect("PrintTyreRethread.aspx", false);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;

public partial class SAP_termloans : System.Web.UI.Page
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
                txtFromdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                lblTitle.Text = Session["TitleName"].ToString();
                Fillbranch();
            }
        }
    }
    void Fillbranch()
    {
        vdm = new VehicleDBMgr();
        cmd = new MySqlCommand("SELECT brnch_sno, user_id, branchname,shortname, whcode FROM branch_info");
        DataTable dtbrantch = vdm.SelectQuery(cmd).Tables[0];
        ddlbranch.DataSource = dtbrantch;
        ddlbranch.DataTextField = "branchname";
        ddlbranch.DataValueField = "whcode";
        ddlbranch.DataBind();
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        GetReport();
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
    DataTable Report = new DataTable();
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            pnlHide.Visible = true;
            Session["IDate"] = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            vdm = new VehicleDBMgr();
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
            Session["xporttype"] = "TallyTermloans";
            string DCNO = "";
            Report.Columns.Add("JV No");
            Report.Columns.Add("JV Date");
            Report.Columns.Add("WH Code");
            Report.Columns.Add("Ledger Code");
            Report.Columns.Add("Ledger Name");
            Report.Columns.Add("Amount");
            Report.Columns.Add("Narration");
            Session["filename"] = " Tally Termloans" + fromdate.ToString("dd/MM/yyyy");
            cmd = new MySqlCommand("SELECT sno, vehsno, doe, paymenttype, amount, remarks, branchid, ledgername, vehicleno, whcode,ledgercode FROM termloantransactions WHERE (whcode = @whcode) AND (doe BETWEEN @d1 AND @d2)");       
            // cmd = new MySqlCommand("SELECT termloanentry.ledger_code, termloanentry.bankname, vehicel_master.registration_no, termloanentry.type, termloanentry.termloandate, vehicel_master.vm_owner, vehicel_master.vm_model,termloanentry.ledgername, termloanentry.interest_per, termloanentry.instalamount, termloanentry.loanamount, termloanentry.totalinstall, termloanentry.com_install, termloanentry.instaldate,vehicel_master.whcode, termloantransactions.amount FROM termloanentry INNER JOIN vehicel_master ON termloanentry.vehsno = vehicel_master.vm_sno INNER JOIN termloantransactions ON termloanentry.vehsno = termloantransactions.vehsno WHERE (vehicel_master.whcode = @whcode) AND (termloantransactions.doe BETWEEN @d1 AND @d2) GROUP BY termloanentry.type, vehicel_master.registration_no");
            cmd.Parameters.Add("@whcode", ddlbranch.SelectedValue);
            cmd.Parameters.Add("@d1", GetLowMonthRetrive(fromdate));
            cmd.Parameters.Add("@d2", GetHighMonth(fromdate));
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
            double totamount = 0;
            fromdate = fromdate.AddDays(-1);
            string frmdate = fromdate.ToString("dd-MM-yyyy");
            string[] strjv = frmdate.Split('-');
            //int branchid=0;
            //branchid= Convert.ToInt32(ddlbranch.SelectedValue);
            DCNO = "TL";
            foreach (DataRow branch in dtble.Rows)
            {

                DataRow newrow = Report.NewRow();
                newrow["JV No"] = DCNO + strjv[1];
                newrow["JV Date"] = fromdate.AddMonths(-1).ToString("dd-MMM-yyyy");
                newrow["WH Code"] = branch["whcode"].ToString();
                string ledgercode = branch["ledgername"].ToString();
                newrow["Ledger Code"] = branch["ledgercode"].ToString();
                newrow["Ledger Name"] = branch["ledgername"].ToString();
                double amount = 0;
                double.TryParse(branch["amount"].ToString(), out amount);
                totamount += amount;
                newrow["Amount"] = "-" + amount;
                newrow["Narration"] = "Being the vehicle interest for the month of " + fromdate.AddMonths(-1).ToString("MMM-yyyy") + " Amount " + amount + " Vehicle Number " + branch["vehicleno"].ToString() +  ",Emp Name  " + Session["employname"].ToString();
                Report.Rows.Add(newrow);
            }
            DataRow new_row = Report.NewRow();
            new_row["JV No"] = DCNO + strjv[1];
            new_row["JV Date"] = fromdate.AddMonths(-1).ToString("dd-MMM-yyyy");
            new_row["Ledger Code"] = "5115064";
            new_row["Ledger Name"] = "Interest on Vehicle Loans";
            new_row["WH Code"] = ddlbranch.SelectedValue;
            new_row["Amount"] = totamount;
            new_row["Narration"] = "Being the vehicle interest for the month of " + fromdate.AddMonths(-1).ToString("MMM-yyyy") + " Amount " + totamount + ",Emp Name  " + Session["employname"].ToString();
            Report.Rows.Add(new_row);
            grdReports.DataSource = Report;
            grdReports.DataBind();
            Session["xportdata"] = Report;
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            grdReports.DataSource = Report;
            grdReports.DataBind();
        }
    }
    SqlCommand sqlcmd;
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            vdm = new VehicleDBMgr();
            DateTime CreateDate = VehicleDBMgr.GetTime(vdm.conn);
            SAPdbmanger SAPvdm = new SAPdbmanger();
            DateTime fromdate = DateTime.Now;
            DataTable dt = (DataTable)Session["xportdata"];
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
            foreach (DataRow dr in dt.Rows)
            {
                string AcctCode = dr["Ledger Code"].ToString();
                string whCode = dr["WH Code"].ToString();

                if (AcctCode == "" || whCode == "")
                {
                }
                else
                {
                    sqlcmd = new SqlCommand("Insert into EMROJDT (CreateDate, RefDate, DocDate, TransNo, AcctCode, AcctName, Debit, Credit, B1Upload, Processed,Ref1,ocrcode,series) values (@CreateDate, @RefDate, @DocDate,@TransNo, @AcctCode, @AcctName, @Debit, @Credit, @B1Upload, @Processed,@Ref1,@ocrcode,@series)");
                    sqlcmd.Parameters.Add("@CreateDate", GetLowDate(fromdate));
                    sqlcmd.Parameters.Add("@RefDate", GetLowDate(fromdate));
                    sqlcmd.Parameters.Add("@docdate", GetLowDate(fromdate));
                    sqlcmd.Parameters.Add("@Ref1", dr["JV No"].ToString());
                    sqlcmd.Parameters.Add("@TransNo", dr["JV No"].ToString());
                    sqlcmd.Parameters.Add("@AcctCode", dr["Ledger Code"].ToString());
                    sqlcmd.Parameters.Add("@AcctName", dr["Ledger Name"].ToString());
                    double amount = 0;
                    double.TryParse(dr["Amount"].ToString(), out amount);
                    if (amount < 0)
                    {
                        amount = Math.Abs(amount);
                        double Debit = 0;
                        sqlcmd.Parameters.Add("@Debit", Debit);
                        sqlcmd.Parameters.Add("@Credit", amount);
                    }
                    else
                    {
                        amount = Math.Abs(amount);
                        double Credit = 0;
                        sqlcmd.Parameters.Add("@Debit", amount);
                        sqlcmd.Parameters.Add("@Credit", Credit);
                    }
                    string B1Upload = "N";
                    string Processed = "N";
                    string series = "17";
                    sqlcmd.Parameters.Add("@B1Upload", B1Upload);
                    sqlcmd.Parameters.Add("@Processed", Processed);
                    sqlcmd.Parameters.Add("@ocrcode", whCode);
                    sqlcmd.Parameters.Add("@series", series);
                    SAPvdm.insert(sqlcmd);
                }
            }
            pnlHide.Visible = false;
            DataTable dtempty = new DataTable();
            grdReports.DataSource = dtempty;
            grdReports.DataBind();
            lblmsg.Text = "Successfully Saved";
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.ToString();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

public partial class SAP_Petrocards : System.Web.UI.Page
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
                txtTodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                lblTitle.Text = Session["TitleName"].ToString();
            }
        }
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
            string[] datetostrig = txtTodate.Text.Split(' ');
            if (datetostrig.Length > 1)
            {
                if (datetostrig[0].Split('-').Length > 0)
                {
                    string[] dates = datetostrig[0].Split('-');
                    string[] times = datetostrig[1].Split(':');
                    todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            string strmonth = fromdate.ToString("MMM/dd/yyyy");

            lbl_selfromdate.Text = fromdate.ToString("dd/MM/yyyy");
            Session["xporttype"] = "TallyPetroCards";
            string DCNO = "";
            Report.Columns.Add("JV No");
            Report.Columns.Add("JV Date");
            Report.Columns.Add("WH Code");
            Report.Columns.Add("Ledger Code");
            Report.Columns.Add("Ledger Name");
            Report.Columns.Add("Amount");
            Report.Columns.Add("Narration");
            Session["filename"] = " Tally PetroCards" + fromdate.ToString("dd/MM/yyyy");
            cmd = new MySqlCommand("SELECT sno, cardname, accountid, dealername, dealercity, transactiondate, accountingdate, tranactiontype, currency, amount, volume, amountbalance, petromiles_earned, odometer, vehicleno, remarks, vehicletype FROM petrocard_details WHERE (transactiondate BETWEEN @d1 AND @d2) and (tranactiontype<>'CMS_RECHARGE') order by transactiondate");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
            string frmdate = fromdate.ToString("dd-MM-yyyy");
            string[] strjv = frmdate.Split('-');
            cmd = new MySqlCommand("SELECT Petroledger_code,petroledgername,registration_no,whcode FROM vehicel_master");
            //cmd = new MySqlCommand("SELECT vehicel_master.vm_sno, vehicel_master.registration_no,branch_info.whcode,vehicel_master.petroledgername, vehicel_master.Petroledger_code, vehicel_master.ledgername FROM vehicel_master LEFT OUTER JOIN branch_info ON vehicel_master.branch_id = branch_info.brnch_sno");
            DataTable dtvehicles = vdm.SelectQuery(cmd).Tables[0];
            foreach (DataRow branch in dtble.Rows)
            {
                DataRow newrow = Report.NewRow();
                newrow["JV No"] = "Petro - " + branch["sno"].ToString();
                string transdate = branch["transactiondate"].ToString();
                DateTime dttrans = Convert.ToDateTime(transdate);
                newrow["JV Date"] = dttrans.ToString("dd-MMM-yyyy");
                string vehicletype = branch["vehicletype"].ToString();
                string vehicleno = branch["vehicleno"].ToString();
                string ledgername = "";
                string ledgercode = "";
                string whcode = "";
                if (vehicletype == "CAR" || vehicletype == "BIKE" || vehicletype == "Tanker")
                {
                    foreach (DataRow drroute in dtvehicles.Select("registration_no='" + branch["vehicleno"].ToString() + "'"))
                    {
                        ledgercode = drroute["Petroledger_code"].ToString();
                        ledgername = drroute["petroledgername"].ToString();
                        whcode = drroute["whcode"].ToString();
                    }
                }
                if (vehicletype == "PUFF")
                {
                    ledgercode="5134003";
                    ledgername = "Sales maintenance-Pbk-Diesel";
                    foreach (DataRow drroute in dtvehicles.Select("registration_no='" + branch["vehicleno"].ToString() + "'"))
                    {
                        whcode = drroute["whcode"].ToString();
                    }
                   
                }
                if (vehicletype == "GENERATOR")
                {
                    string GENARATOR = "GENARATOR";
                    ledgercode = "5124005";
                    ledgername = branch["vehicleno"].ToString();
                     foreach (DataRow drroute in dtvehicles.Select("registration_no='" + GENARATOR + "'"))
                    {
                        whcode = drroute["whcode"].ToString();
                    }

                }
                if (vehicleno == "TRAVELLING")
                {
                    ledgername = branch["vehicletype"].ToString();
                }
                newrow["Ledger Code"] = ledgercode;
                newrow["Ledger Name"] = ledgername;
                newrow["WH Code"] = whcode;
                string amount = branch["amount"].ToString();
                newrow["Amount"] = amount;
                newrow["Narration"] = "Being the diesel filled a " + branch["remarks"] + " Dealer Name " + branch["dealername"] + " Amount " + amount + " Vehicle Number " + branch["vehicleno"].ToString() + " Vehicle Type " + branch["vehicletype"].ToString() + " Through petro card no " + branch["accountid"].ToString() + ", Qty " + branch["volume"].ToString() + ",Emp Name  " + Session["employname"].ToString();
                Report.Rows.Add(newrow);


                DataRow newrow1 = Report.NewRow();
                newrow1["JV No"] = "Petro - " + branch["sno"].ToString();
                newrow1["JV Date"] = dttrans.ToString("dd-MMM-yyyy");
                ledgername = "Bharat Petrolium Corporation-Card";
                ledgercode = "VPBK080";
                newrow1["WH Code"] = whcode;
                newrow1["Ledger Code"] = ledgercode;
                newrow1["Ledger Name"] = ledgername;
                newrow1["Amount"] = "-" + amount;
                newrow1["Narration"] = "Being the diesel filled a " + branch["remarks"] + " Amount " + amount + " Vehicle Number " + branch["vehicleno"].ToString() + " Vehicle Type " + branch["vehicletype"].ToString() + " Through petro card no " + branch["accountid"].ToString() + ", Qty " + branch["volume"].ToString() + ",Emp Name  " + Session["employname"].ToString();
                Report.Rows.Add(newrow1);
            }

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
                string WHcode = dr["WH Code"].ToString();
                if (AcctCode == "" || WHcode == "")
                {
                }
                else
                {
                    sqlcmd = new SqlCommand("Insert into EMROJDT (CreateDate, RefDate, DocDate, TransNo, AcctCode, AcctName, Debit, Credit, B1Upload, Processed,Ref1,series,OcrCode) values (@CreateDate, @RefDate, @DocDate,@TransNo, @AcctCode, @AcctName, @Debit, @Credit, @B1Upload, @Processed,@Ref1,@series,@OcrCode)");
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
                    sqlcmd.Parameters.Add("@B1Upload", B1Upload);
                    sqlcmd.Parameters.Add("@Processed", Processed);
                    string series = "17";
                    sqlcmd.Parameters.Add("@OcrCode", dr["WH Code"].ToString());
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
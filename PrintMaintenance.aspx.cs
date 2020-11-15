using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class PrintMaintenance : System.Web.UI.Page
{
    MySqlCommand cmd;
    string BranchID = "";
    DataTable dtAddress = new DataTable();
    VehicleDBMgr vdm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Branch_ID"] == null)
            Response.Redirect("Login.aspx");
        else
        {
            BranchID = Session["Branch_ID"].ToString();
            if (!this.IsPostBack)
            {
                if (!Page.IsCallback)
                {
                    if (Session["MaintenanceID"] == null)
                    {
                    }
                    else
                    {
                        txtmaintenancecode.Text = Session["MaintenanceID"].ToString();
                    }
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
    string TripSno = "0";
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        try
        {
            vdm = new VehicleDBMgr();
            cmd = new MySqlCommand("SELECT minimasters.mm_name AS VehType, minimasters_1.mm_name AS Make, vehicel_master.registration_no,veh_exp.remarks, veh_exp.name, veh_exp.incharge,veh_exp.create_date, veh_exp.doe,vehicel_master.vm_model, veh_exp.amount FROM minimasters INNER JOIN vehicel_master ON minimasters.sno = vehicel_master.vhtype_refno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno INNER JOIN veh_exp ON vehicel_master.vm_sno = veh_exp.vehsno WHERE  (veh_exp.branchid = @BranchID) AND (veh_exp.Maintace_id = @Maintace_id)");
            cmd.Parameters.Add("@Maintace_id", txtmaintenancecode.Text);
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dtTripSheet = vdm.SelectQuery(cmd).Tables[0];
            if (dtTripSheet.Rows.Count > 0)
            {
                string TripTime = dtTripSheet.Rows[0]["create_date"].ToString();
                DateTime dtPlantime = Convert.ToDateTime(TripTime);
                string time = dtPlantime.ToString("dd/MMM/yyyy");
                string strPlantime = dtPlantime.ToString();
                string[] DateTime = strPlantime.Split(' ');
                string[] PlanDateTime = strPlantime.Split(' ');
                lblDate.Text = time;
                lblTime.Text = PlanDateTime[1];
                lblMaintenanceCode.Text = txtmaintenancecode.Text;
                lblVehicleNo.Text = dtTripSheet.Rows[0]["registration_no"].ToString();
                lblMake.Text = dtTripSheet.Rows[0]["Make"].ToString();
                lblVehicleType.Text = dtTripSheet.Rows[0]["VehType"].ToString();
                lblName.Text = dtTripSheet.Rows[0]["name"].ToString();
                lblIncharge.Text = dtTripSheet.Rows[0]["incharge"].ToString();
                lblTowards.Text = dtTripSheet.Rows[0]["remarks"].ToString();
                lblModel.Text = dtTripSheet.Rows[0]["vm_model"].ToString();
                string Amont = dtTripSheet.Rows[0]["amount"].ToString();
                string[] Ones = { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Ninteen" };

                string[] Tens = { "Ten", "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninty" };

                int Num = int.Parse(Amont);

                lblReceived.Text = NumToWordBD(Num) + " Rupees Only";
                ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "PopupOpen(" + txtmaintenancecode.Text + ");", true);

            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    public static string NumToWordBD(Int64 Num)
    {
        string[] Below20 = { "", "One ", "Two ", "Three ", "Four ", 
      "Five ", "Six " , "Seven ", "Eight ", "Nine ", "Ten ", "Eleven ", 
    "Twelve " , "Thirteen ", "Fourteen ","Fifteen ", 
      "Sixteen " , "Seventeen ","Eighteen " , "Nineteen " };
        string[] Below100 = { "", "", "Twenty ", "Thirty ", 
      "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty ", "Ninety " };
        string InWords = "";
        if (Num >= 1 && Num < 20)
            InWords += Below20[Num];
        if (Num >= 20 && Num <= 99)
            InWords += Below100[Num / 10] + Below20[Num % 10];
        if (Num >= 100 && Num <= 999)
            InWords += NumToWordBD(Num / 100) + " Hundred " + NumToWordBD(Num % 100);
        if (Num >= 1000 && Num <= 99999)
            InWords += NumToWordBD(Num / 1000) + " Thousand " + NumToWordBD(Num % 1000);
        if (Num >= 100000 && Num <= 9999999)
            InWords += NumToWordBD(Num / 100000) + " Lakh " + NumToWordBD(Num % 100000);
        if (Num >= 10000000)
            InWords += NumToWordBD(Num / 10000000) + " Crore " + NumToWordBD(Num % 10000000);
        return InWords;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class PrintTyreRethread : System.Web.UI.Page
{
    MySqlCommand cmd;
    DataTable dtAddress = new DataTable();
    VehicleDBMgr vdm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (!Page.IsCallback)
            {
                if (Session["TyreRethreadSno"] == null)
                {
                }
                else
                {
                    txtRethreadSno.Text = Session["TyreRethreadSno"].ToString();
                }
                lblAddress.Text = Session["Address"].ToString();
                lblTitle.Text = Session["TitleName"].ToString();
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
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        try
        {
            vdm = new VehicleDBMgr();
            cmd = new MySqlCommand("SELECT tyre_rethread.sno AS RethreadSno, tyre_rethread.sentdate, tyre_rethread.sentby, vendors_info.vendorname,tyre_rethread.no_of_tyres as NoOfTyres, tyre_rethread.expecteddate, tyre_rethread.remarks, vendors_info.vendor_mob AS PhoneNo FROM tyre_rethread INNER JOIN vendors_info ON tyre_rethread.servicingat = vendors_info.sno WHERE (tyre_rethread.sno=@RethreadSno)");
            cmd.Parameters.Add("@RethreadSno", txtRethreadSno.Text);
            DataTable dtTripSheet = vdm.SelectQuery(cmd).Tables[0];
            if (dtTripSheet.Rows.Count > 0)
            {
                lblTripNo.Text = dtTripSheet.Rows[0]["RethreadSno"].ToString();
                string TripTime = dtTripSheet.Rows[0]["sentdate"].ToString();
                DateTime dtPlantime = Convert.ToDateTime(TripTime);
                string time = dtPlantime.ToString("dd/MMM/yyyy");
                string strPlantime = dtPlantime.ToString();
                string[] DateTime = strPlantime.Split(' ');
                string[] PlanDateTime = strPlantime.Split(' ');
                lblDate.Text = time;
                lblTime.Text = PlanDateTime[1];
                BindEmpty();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    void BindEmpty()
    {
        DataTable Report = new DataTable();
        Report.Columns.Add("Sno");
        Report.Columns.Add("TyreSno");
        Report.Columns.Add("SVDSNo");
        Report.Columns.Add("TyreTube");
        Report.Columns.Add("Brand");
         cmd = new MySqlCommand("SELECT new_tyres_sub.tyre_sno AS TyreSno,new_tyres_sub.svdsno, new_tyres_sub.tyre_tube, minimasters.mm_name AS Brand FROM  tyre_rethread_sub INNER JOIN new_tyres_sub ON tyre_rethread_sub.tyre_sno = new_tyres_sub.sno INNER JOIN minimasters ON new_tyres_sub.brand = minimasters.sno WHERE (tyre_rethread_sub.rethread_sno = @RethreadSno)");
         cmd.Parameters.Add("@RethreadSno", txtRethreadSno.Text);
        DataTable dtTyreRethread = vdm.SelectQuery(cmd).Tables[0];
        if (dtTyreRethread.Rows.Count > 0)
        {
            int i = 1;
            foreach (DataRow dr in dtTyreRethread.Rows)
            {
                DataRow newrow = Report.NewRow();
                newrow["Sno"] = i++.ToString();
                newrow["TyreSno"] = dr["TyreSno"].ToString();
                newrow["SVDSNo"] = dr["svdsno"].ToString();
                newrow["TyreTube"] = dr["tyre_tube"].ToString();
                newrow["Brand"] = dr["Brand"].ToString();
                Report.Rows.Add(newrow);
            }
        }
        grdReports.DataSource = Report;
        grdReports.DataBind();
    }
}
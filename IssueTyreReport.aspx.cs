using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class IssueTyreReport : System.Web.UI.Page
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
                    GetReport();
                    lblTitle.Text = Session["TitleName"].ToString();
                }
            }
        }
    }
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            DataTable dtIssueTyre = new DataTable();
            cmd = new MySqlCommand("SELECT vehicel_master.registration_no, minimasters.mm_name AS VehicleType, minimasters_1.mm_name AS Make, vehicel_master.Capacity, vehicel_master.odometer AS VehicleOdometer, new_tyres_sub.tyre_sno,  new_tyres_sub.svdsno, new_tyres_sub.current_KMS, vehicle_master_sub.Odometer FROM vehicel_master INNER JOIN vehicle_master_sub ON vehicel_master.vm_sno = vehicle_master_sub.vehicle_mstr_sno INNER JOIN new_tyres_sub ON vehicle_master_sub.tyre_sno = new_tyres_sub.sno INNER JOIN minimasters ON vehicel_master.vhtype_refno = minimasters.sno INNER JOIN minimasters minimasters_1 ON vehicel_master.vhmake_refno = minimasters_1.sno WHERE (vehicel_master.branch_id = @BranchID) ORDER BY vehicel_master.registration_no");
            cmd.Parameters.Add("@BranchID", BranchID);
            dtIssueTyre = vdm.SelectQuery(cmd).Tables[0];
            if (dtIssueTyre.Rows.Count > 0)
            {
                string title = "Issue Tyres Report ";
                Session["title"] = title;
                Session["filename"] = "IssueTyresReport ";
                Session["xportdata"] = dtIssueTyre;
                grdReports.DataSource = dtIssueTyre;
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
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class TyreTransaction : System.Web.UI.Page
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
            cmd = new MySqlCommand("SELECT DATE_FORMAT(tyre_changes.transaction_date, '%d %b %y') AS Date , vehicel_master.registration_no AS VehicleNo, minimasters_2.mm_name AS VehicleType, minimasters_3.mm_name AS VehicleMake, vehicel_master.Capacity, tyre_changes.veh_odometer AS Odometer, new_tyres_sub.tyre_sno AS TyreNo, new_tyres_sub.svdsno, minimasters.mm_name AS TyreType, minimasters_1.mm_name AS Brand, new_tyres_sub.cost, axils_tyres_names.tyrename AS TyrePosition, new_tyres_sub.current_KMS AS TotalKMs, tyre_changes.kms,tyre_changes.Fitting_Type as FitType, axils_tyres_names.axleside, tyre_changes.remarks FROM tyre_changes INNER JOIN new_tyres_sub ON tyre_changes.tyre_master_sno = new_tyres_sub.sno INNER JOIN minimasters ON new_tyres_sub.type_of_tyre = minimasters.sno INNER JOIN minimasters minimasters_1 ON new_tyres_sub.brand = minimasters_1.sno INNER JOIN axils_tyres_names ON tyre_changes.tyre_position = axils_tyres_names.sno INNER JOIN vehicel_master ON tyre_changes.vehicle_sno = vehicel_master.vm_sno INNER JOIN minimasters minimasters_2 ON vehicel_master.vhtype_refno = minimasters_2.sno INNER JOIN minimasters minimasters_3 ON vehicel_master.vhmake_refno = minimasters_3.sno WHERE (tyre_changes.branch_id = @BranchID)");
            cmd.Parameters.Add("@BranchID", BranchID);
            dtIssueTyre = vdm.SelectQuery(cmd).Tables[0];
            if (dtIssueTyre.Rows.Count > 0)
            {
                string title = "Tyres Transaction Report ";
                Session["title"] = title;
                Session["filename"] = "Tyres Transaction Report ";
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
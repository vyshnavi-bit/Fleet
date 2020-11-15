using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;

public partial class VehicleModuleConfig : System.Web.UI.Page
{
    MySqlCommand cmd;
    string BranchID = "";
    VehicleDBMgr vdm;
    
    //
    string VehicleTypestring = "";
    string Plantsstring = "";
    DataTable moduledata = new DataTable();
    DataTable vehicledata = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        lbl_nofifier.Text = string.Empty;
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
                    LoadModule();                    
                    UpdateGrid();           
                }
            }            
        }
    }

    void LoadModule()
    {
        chblVehicleTypes.Items.Clear();
        string itemtype = "Vehicle Type";
        cmd = new MySqlCommand(" SELECT DISTINCT Capacity FROM vehicel_master ORDER BY Capacity DESC ");
        // cmd.Parameters.Add("@UserName", UserName);
        DataTable dtMaintaince = vdm.SelectQuery(cmd).Tables[0];
        foreach (DataRow dr in dtMaintaince.Rows)
        {
            chblVehicleTypes.Items.Add(dr["Capacity"].ToString());
        }

        chblZones.Items.Clear();
        cmd = new MySqlCommand("SELECT ID,ModuleName FROM moduleinfo Order By ID");
        // cmd.Parameters.Add("@UserName", UserName);
        DataTable totaldata = vdm.SelectQuery(cmd).Tables[0];
        Session["moduledata"] = totaldata;
        foreach (DataRow dr in totaldata.Rows)
        {
            chblZones.Items.Add(dr["ModuleName"].ToString());
        }
    }
    void checkboxchanged()
    {
        lblvehcount.Text = "";
        ckballvehicles.Checked = false;
        foreach (ListItem chkitem in chblVehicleTypes.Items)
        {
            if (chkitem.Selected == true)
            {
                VehicleTypestring += "'" + chkitem.Text + "',";
            }
        }

        if (VehicleTypestring.Length > 0)
        {
            VehicleTypestring = VehicleTypestring.Remove(VehicleTypestring.Length - 1);
        }
        else
        {
            VehicleTypestring = "'All Vehicle Capacity'";
        }

        foreach (ListItem chkitem in chblZones.Items)
        {
            if (chkitem.Selected == true)
            {
                Plantsstring += "'" + chkitem.Text + "',";
            }
        }
        if (Plantsstring.Length > 0)
        {
            Plantsstring = Plantsstring.Remove(Plantsstring.Length - 1);
        }
        else
        {
            Plantsstring = "'All Modules'";
        }

       
      

        DataTable vendors = new DataTable();

        if (Session["filteredtable"] != null)
        {
            vendors = (DataTable)Session["filteredtable"];
        }
        if (vendors.Rows.Count == 0)
        {
            cmd = new MySqlCommand("SELECT vm_sno, registration_no, Capacity, branch_id FROM  vehicel_master  ORDER BY vm_sno ");//WHERE(branch_id = 40)
            // cmd.Parameters.Add("@UserName", UserName);
            vendors = vdm.SelectQuery(cmd).Tables[0];
            Session["filteredtable"] = vendors;
        }
        DataRow[] filteredrows = null;
        if ((VehicleTypestring != "'All Vehicle Capacity'") && Plantsstring != "'All Modules'")
        {

            filteredrows = vendors.Select("Capacity in (" + VehicleTypestring + ")  ");//and PlantName in (" + Plantsstring + ")

        }
        else if (VehicleTypestring == "'All Vehicle Capacity'" && Plantsstring == "'All Modules'")
        {
            filteredrows = vendors.Select();
        }

        else if (VehicleTypestring != "'All Vehicle Capacity'" && Plantsstring == "'All Modules'")
        {
            filteredrows = vendors.Select("Capacity in (" + VehicleTypestring + ")");
        }
        else if (VehicleTypestring == "'All Vehicle Capacity'" && Plantsstring != "'All Modules'")
        {
            // filteredrows = vendors.Select("Capacity in (" + Plantsstring + ")");
            filteredrows = vendors.Select();
        }
        else
        {
        }

        DataTable filteredtable = new DataTable();
        if (filteredrows.Length > 0)
        {
            filteredtable = filteredrows.CopyToDataTable();
        }
        DataView view = filteredtable.DefaultView;
        DataTable distinctTable = new DataTable();
        if (view.Count > 0)
        {
            distinctTable = view.ToTable("distinctTable", true, "registration_no");
            ckbVehicles.Items.Clear();
            foreach (DataRow dr in distinctTable.Rows)
            {
                if (dr["registration_no"].ToString().Length > 0)
                {
                    ckbVehicles.Items.Add(dr["registration_no"].ToString());
                }
            }

            if (Session["dtallVehicles"] == null)
            {
                //foreach (ListItem chkitem in ckbVehicles.Items)
                //{
                //        chkitem.Selected = true;
                //}
            }
            else
            {
               // cmd = new MySqlCommand("SELECT vm_sno, registration_no, Capacity, branch_id FROM  vehicel_master WHERE(branch_id = 40)");
                // cmd.Parameters.Add("@UserName", UserName);
               // vendors = vdm.SelectQuery(cmd).Tables[0];
                DataTable prev_data = (DataTable)Session["dtallVehicles"];
                foreach (DataRow dr in prev_data.Rows)
                {
                    string VehicleNo = dr["VehicleID"].ToString();
                    foreach (ListItem chkitem in ckbVehicles.Items)
                    {
                        if (chkitem.Text == VehicleNo)
                        {
                            chkitem.Selected = true;
                        }
                    }
                }
            }

        }
        else
        {
            ckbVehicles.Items.Clear();
        }
        lblvehcount.Text = ckbVehicles.Items.Count.ToString();
    }
    protected void ckbZones_OnCheckedChanged(object sender, EventArgs e)
    {
        ckbVehicles.Items.Clear();
        if (ckbZones.Checked == true)
        {
            foreach (ListItem chkitem in chblZones.Items)
            {
                chkitem.Selected = true;
                break;
            }
        }
        else
        {
            if (chblZones.SelectedIndex == 0)
            {
                foreach (ListItem chkitem in chblZones.Items)
                {
                    chkitem.Selected = false;
                    break;
                }
            }
        }
        // checkboxchanged();
    }
    protected void chblZones_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ks = 0;
        try
        {
            ckbVehicles.Items.Clear();
            foreach (ListItem chkitem in chblZones.Items)
            {

                if (chkitem.Selected == true)
                {
                    if (ks == 0)
                    {
                        // ckbZones.Checked = true;
                        ks = 1;
                    }
                    else
                    {
                        chkitem.Selected = false;
                    }
                }
                else
                {
                    if (ks == 1)
                    {
                        chkitem.Selected = false;
                    }
                    else
                    {
                        //  ckbZones.Checked = false;
                        chkitem.Selected = false;
                    }
                }
            }
            checkboxchanged();
        }
        catch
        {
        }
    }
    protected void ckbVehicleTypes_OnCheckedChanged(object sender, EventArgs e)
    {
        ckbVehicles.Items.Clear();
        if (ckbVehicleTypes.Checked == true)
        {
            foreach (ListItem chkitem in chblVehicleTypes.Items)
            {
                chkitem.Selected = true;
            }
        }
        else
        {
            if (chblVehicleTypes.SelectedIndex == 0)
            {
                foreach (ListItem chkitem in chblVehicleTypes.Items)
                {
                    chkitem.Selected = false;
                }
            }
        }
        checkboxchanged();
    }
    protected void chblVehicleTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ckbVehicles.Items.Clear();
            foreach (ListItem chkitem in chblVehicleTypes.Items)
            {
                if (chkitem.Selected == false)
                {
                    ckbVehicleTypes.Checked = false;
                    break;
                }
                //else
                //{
                //    ckbVehicleTypes.Checked = true;
                //}
            }
            checkboxchanged();
        }
        catch
        {
        }
    }
    protected void ckballvehicles_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (ckballvehicles.Checked == true)
        {
            foreach (ListItem chkitem in ckbVehicles.Items)
            {
                chkitem.Selected = true;
            }
        }
        else
        {
            if (ckbVehicles.SelectedIndex == 0)
            {
                foreach (ListItem chkitem in ckbVehicles.Items)
                {
                    chkitem.Selected = false;
                }
            }
        }
    }

    public static string Sno = "";
    protected void grdManageLogins_SelectedIndexChanged(object sender, EventArgs e)
    {
         lbl_nofifier.Text = "";
        foreach (ListItem chkitem in chblZones.Items)
        {
            chkitem.Selected = false;
        }
        foreach (ListItem chkitem in chblVehicleTypes.Items)
        {
            chkitem.Selected = false;
        }
        ckballvehicles.Checked = false;
        ckbZones.Checked = false;
        ckbVehicleTypes.Checked = false;
        


        if (grdManageLogins.SelectedIndex >= -1)
        {
            GridViewRow selectedrw = grdManageLogins.SelectedRow;
            //txtLoginID.Text = selectedrw.Cells[2].Text;
            //txtPassword.Text = selectedrw.Cells[3].Text;
            //ddlUserType.SelectedValue = selectedrw.Cells[4].Text;
             Sno = selectedrw.Cells[1].Text;
            //txt_mobno.Text = selectedrw.Cells[6].Text;
            //txt_emailid.Text = selectedrw.Cells[7].Text;
            btnSave.Text = "Edit";
            cmd = new MySqlCommand(" SELECT vmsc.MID, moduleinfo.ModuleName AS ModuleName, moduleinfo.Status, vmsc.VID, vmsc.VehicleRegistrationNo AS VehicleID, vmsc.Capacity AS Capacity FROM vmsc INNER JOIN moduleinfo ON vmsc.MID = moduleinfo.ID WHERE(vmsc.MID = @MID) ORDER BY vmsc.MID");
            cmd.Parameters.Add("@MID", Sno);
            DataTable dtallVehicles = vdm.SelectQuery(cmd).Tables[0];
            Session["dtallVehicles"] = dtallVehicles;
            if (dtallVehicles.Rows.Count > 0)
            {
               // string authorizedtype = dtallVehicles.Rows[0]["authorizedtype"].ToString();               
                DataView view = new DataView(dtallVehicles);
                DataTable plants = view.ToTable(true, "ModuleName");
                DataTable vehicletypes = view.ToTable(true, "Capacity");

                foreach (DataRow dr in plants.Rows)
                {
                    if (dr["ModuleName"].ToString() != "")
                    {
                        foreach (ListItem chkitem in chblZones.Items)
                        {
                            if (chkitem.Text == dr["ModuleName"].ToString())
                            {
                                chkitem.Selected = true;                                
                            }
                        }
                    }
                }

                foreach (DataRow dr in vehicletypes.Rows)
                {
                    if (dr["Capacity"].ToString() != "")
                    {
                        foreach (ListItem chkitem in chblVehicleTypes.Items)
                        {
                            if (chkitem.Text == dr["Capacity"].ToString())
                            {
                                chkitem.Selected = true;
                                
                            }
                        }
                    }
                }
                checkboxchanged();
                foreach (DataRow dr in dtallVehicles.Rows)
                {
                    string VehicleNo = dr["VehicleID"].ToString();
                    foreach (ListItem chkitem in ckbVehicles.Items)
                    {
                        if (chkitem.Text == VehicleNo)
                        {
                            chkitem.Selected = true;                            
                        }
                    }
                }
            }
        }
        
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_nofifier.Text = "";
            string ModuleName = string.Empty;
            string Mid = string.Empty;
            string VehicleName = string.Empty;
            string VehicleId = string.Empty;
            string VehicleCapacity = string.Empty;

            int flag = 0;
            int flag1 = 0;

            moduledata = (DataTable)Session["moduledata"];
            foreach (ListItem chkitem in chblZones.Items)
            {
                if (chkitem.Selected == true)
                {
                    ModuleName = chkitem.Text;

                    foreach (DataRow dr in moduledata.Rows)
                    {
                        if (ModuleName == dr["ModuleName"].ToString())
                        {
                            Mid = dr["ID"].ToString();
                            Sno = Mid;
                            flag = 1;
                            break;
                        }
                    }
                }
            }
            if (flag == 1)
            {
                vehicledata = (DataTable)Session["filteredtable"];

                if (btnSave.Text == "Save")
                {
                    foreach (ListItem chkitem in ckbVehicles.Items)
                    {
                        if (chkitem.Selected == true)
                        {
                            VehicleName = chkitem.Text;
                            foreach (DataRow dr in vehicledata.Rows)
                            {
                                if (VehicleName == dr["registration_no"].ToString())
                                {
                                    VehicleId = dr["vm_sno"].ToString();
                                    VehicleCapacity = dr["Capacity"].ToString();
                                    flag1 = 1;

                                    cmd = new MySqlCommand("Insert into vmsc (MID,VID,VehicleRegistrationNo,Capacity,Status,CreateDate,CreateBy) values(@Mid,@VehicleID,@VehicleRegistrationNo,@Capacity,@Status,@CreateDate,@CreateBy)");
                                    cmd.Parameters.Add("@Mid", Mid);
                                    cmd.Parameters.Add("@VehicleID", VehicleId);
                                    cmd.Parameters.Add("@VehicleRegistrationNo", VehicleName);
                                    cmd.Parameters.Add("@Capacity", VehicleCapacity);
                                    cmd.Parameters.Add("@Status", 1);
                                    cmd.Parameters.Add("@CreateDate", System.DateTime.Now);
                                    cmd.Parameters.Add("@CreateBy", Session["UserName"].ToString());
                                    vdm.insert(cmd);
                                    lbl_nofifier.Text = string.Empty;
                                    lbl_nofifier.Text = "Data Saved Successfully...";
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    DataTable prev_data = (DataTable)Session["dtallVehicles"];
                    foreach (ListItem obj in ckbVehicles.Items)
                    {
                        if (prev_data.Select("VehicleID='" + obj.Text + "'").Length > 0)
                        {
                            if (!obj.Selected)
                            {
                                //AFTER CONFIRMATION  USE  THIS (DELETE WORKING)
                                cmd = new MySqlCommand("Delete from vmsc where MID=@Refno and VehicleRegistrationNo=@vid");
                                cmd.Parameters.Add("@Refno", Mid);
                                cmd.Parameters.Add("@vid", obj.Text);
                               // vdm.Delete(cmd);
                            }
                        }
                        else
                        {
                            if (obj.Selected)
                            {
                                VehicleName = obj.Text;
                                foreach (DataRow dr in vehicledata.Rows)
                                {
                                    if (VehicleName == dr["registration_no"].ToString())
                                    {
                                        VehicleId = dr["vm_sno"].ToString();
                                        VehicleCapacity = dr["Capacity"].ToString();
                                        flag1 = 1;

                                        cmd = new MySqlCommand("Insert into vmsc (MID,VID,VehicleRegistrationNo,Capacity,Status,CreateDate,CreateBy) values(@Mid,@VehicleID,@VehicleRegistrationNo,@Capacity,@Status,@CreateDate,@CreateBy)");
                                        cmd.Parameters.Add("@Mid", Mid);
                                        cmd.Parameters.Add("@VehicleID", VehicleId);
                                        cmd.Parameters.Add("@VehicleRegistrationNo", VehicleName);
                                        cmd.Parameters.Add("@Capacity", VehicleCapacity);
                                        cmd.Parameters.Add("@Status", 1);
                                        cmd.Parameters.Add("@CreateDate", System.DateTime.Now);
                                        cmd.Parameters.Add("@CreateBy", Session["UserName"].ToString());
                                        vdm.insert(cmd);
                                        lbl_nofifier.Text = string.Empty;
                                        lbl_nofifier.Text = "Data Saved Successfully...";
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }                
                Refresh();
                UpdateGrid();
            }
            else
            {
                lbl_nofifier.Text = "Please, Select Atleast One ModuleName...";
            }
        }
        catch (Exception ex)
        {
            lbl_nofifier.Text = ex.ToString();
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Refresh();
    }
    void Refresh()
    {

        //ddlUserType.ClearSelection();
        Session["dtallVehicles"] = null;
        btnSave.Text = "Save";
        lblvehcount.Text = "0";
        ckbVehicles.Items.Clear();
        foreach (ListItem chkitem in chblVehicleTypes.Items)
        {
            chkitem.Selected = false;
        }
        foreach (ListItem chkitem in ckbVehicles.Items)
        {
            chkitem.Selected = false;
        }
        foreach (ListItem chkitem in chblZones.Items)
        {
            chkitem.Selected = false;
        }
    }
    void UpdateGrid()
    {
        cmd = new MySqlCommand(" SELECT DISTINCT  vmsc.MID, moduleinfo.ModuleName, moduleinfo.Status FROM vmsc INNER JOIN moduleinfo ON vmsc.MID = moduleinfo.ID ");
       // cmd.Parameters.Add("@main_user", UserName);
        DataTable dt = vdm.SelectQuery(cmd).Tables[0];
        grdManageLogins.DataSource = dt;
        grdManageLogins.DataBind();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        //AFTER CONFIRMATION  USE  THIS (DELETE WORKING)
        //if (grdManageLogins.SelectedIndex > -1)
        //{           
        //    cmd = new MySqlCommand("Delete from vmsc where MID=@Mid");
        //    cmd.Parameters.Add("@Mid", Sno);
        //    vdm.Delete(cmd);
        //    Refresh();
        //    UpdateGrid();
        //    btnSave.Text = "Save";
        //    lbl_nofifier.Text = "Record Deleted Successfully";
        //}
    }

}
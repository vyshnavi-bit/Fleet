<%@ WebHandler Language="C#" Class="EmployeeImageHandler" %>

using System;
using System.Web;
using System.Data;
using MySql.Data.MySqlClient;
using System.Net;
public class EmployeeImageHandler : IHttpHandler
{


    /// <summary>  
    /// Summary description for EmployeeImageHandler  
    /// </summary>  
    MySqlCommand cmd;
    DataTable dtAddress = new DataTable();
    VehicleDBMgr vdm;
    string BranchID = "";
    public void ProcessRequest(HttpContext context)
    {
        VehicleDBMgr vdm = new VehicleDBMgr();
        string id = context.Request.QueryString["sno"];
        string [] ravi =id.Split(' ');
        string Idsno = ravi[0];
        string Form = ravi[1];
      //  string BranchID = context.Session["Branch_ID"].ToString();
        if (Form == "test")
        {
            cmd = new MySqlCommand("SELECT IMAGE FROM handoverlogs WHERE (branch_id = @BranchID) AND (sno = @sno)");
            cmd.Parameters.Add("@sno", id);
            cmd.Parameters.Add("@BranchID", 1);
            DataTable ImageLogs = vdm.SelectQuery(cmd).Tables[0];
            context.Response.BinaryWrite((Byte[])ImageLogs.Rows[0]["IMAGE"]);
            context.Response.End();
        }
        if (Form == "DriverData")
        {
            cmd = new MySqlCommand("SELECT IMAGE FROM employdata WHERE (branch_id = @BranchID) AND (emp_sno = @sno)");
            cmd.Parameters.Add("@sno", id);
            cmd.Parameters.Add("@BranchID", 1);
            DataTable ImageLogs = vdm.SelectQuery(cmd).Tables[0];
            string Image = ImageLogs.Rows[0]["IMAGE"].ToString();
            if (Image != "")
            {
                context.Response.BinaryWrite((Byte[])ImageLogs.Rows[0]["IMAGE"]);
                context.Response.End();
            }
        }
        if (Form == "HandOverPrint")
        {
            cmd = new MySqlCommand("SELECT sno, handoversno, imagename,Image FROM handoverlogs WHERE (branch_id = @BranchID) AND (sno = @sno)");
            cmd.Parameters.Add("@sno", id);
            cmd.Parameters.Add("@BranchID", 1);
            DataTable ImageLogs = vdm.SelectQuery(cmd).Tables[0];
            string Image = ImageLogs.Rows[0]["IMAGE"].ToString();
            if (Image != "")
            {
                context.Response.BinaryWrite((Byte[])ImageLogs.Rows[0]["IMAGE"]);
                context.Response.End();
            }
        }
        if (Form == "VehicleData")
        {
            cmd = new MySqlCommand("SELECT image FROM vehicel_master WHERE (branch_id = @BranchID) AND (vm_sno = @VehicleID)");
            cmd.Parameters.Add("@VehicleID", id);
            cmd.Parameters.Add("@BranchID", 1);
            DataTable ImageLogs = vdm.SelectQuery(cmd).Tables[0];
            string Image = ImageLogs.Rows[0]["IMAGE"].ToString();
            if (Image != "")
            {
                context.Response.BinaryWrite((Byte[])ImageLogs.Rows[0]["IMAGE"]);
                context.Response.ContentType = "image/gif";
                context.Response.End();
            }
        }
        if (Form == "RCCopy")
        {
            cmd = new MySqlCommand("SELECT rccopy FROM vehicel_master WHERE (branch_id = @BranchID) AND (vm_sno = @VehicleID)");
            cmd.Parameters.Add("@VehicleID", Idsno);
            cmd.Parameters.Add("@BranchID", 1);
            DataTable ImageLogs = vdm.SelectQuery(cmd).Tables[0];
            string Image = ImageLogs.Rows[0]["rccopy"].ToString();
            if (Image != "")
            {
                context.Response.BinaryWrite((Byte[])ImageLogs.Rows[0]["rccopy"]);
                context.Response.ContentType = "image/gif";
                context.Response.End();
            }
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
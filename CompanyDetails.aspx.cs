﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CompanyDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Employ_Sno"] == "" || Session["Employ_Sno"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        //if (Session["Emp_Type"] != "SuperAdmin")
        //{
        //    Response.Redirect("Default.aspx");
        //}
    }
}
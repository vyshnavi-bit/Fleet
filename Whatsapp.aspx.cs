using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Web.Script.Serialization; // requires the reference 'System.Web.Extensions'
using System.IO;
using System.Text;

public partial class Whatsapp : System.Web.UI.Page
{
    private static string INSTANCE_ID = "16";//"YOUR_INSTANCE_ID_HERE";
    private static string CLIENT_ID = "it@vyshnavi.in";//"YOUR_CLIENT_ID_HERE";
    private static string CLIENT_SECRET = "ce795980658e423fa65a1b757d60dbf2";//"YOUR_CLIENT_SECRET_HERE";
    private static string API_URL = "http://api.whatsmate.net/v3/whatsapp/single/text/message/" + INSTANCE_ID;
    public string[] strarr = new string[10];//'919440622055','919087000700','919619221260';

   public string No = "919619221260";//"919440622055,919087000700,919619221260,919944349060";

    protected void Page_Load(object sender, EventArgs e)
    {
        Lbl_Errmsg.Text = "";
        Lbl_Errmsg.Visible = false;
        if (Session["Branch_ID"] == null)
            Response.Redirect("Login.aspx");
        else
        {
            if (!Page.IsPostBack)
            {
                if (!Page.IsCallback)
                {
                    
                }
            }
        }
    }


    public void SendWhatsappmsg()
    {
        for(int i=0; i<strarr.Length; i++)
        {
            sendMessage(txt_mobileno.Text.Trim(), txt_whatsappmsg.Text.Trim());//"VYSHNAVI-IT TEAM WHATSAPP-COMMUNICATION"
        }

    }

    
    public bool sendMessage(string number, string message)
    {
        bool success = true;

        try
        {
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers["X-WM-CLIENT-ID"] = CLIENT_ID;
                client.Headers["X-WM-CLIENT-SECRET"] = CLIENT_SECRET;

                Payload payloadObj = new Payload() { number = number, message = message };
                string postData = (new JavaScriptSerializer()).Serialize(payloadObj);

                client.Encoding = Encoding.UTF8;
                string response = client.UploadString(API_URL, postData);
                Lbl_Errmsg.Text = "Message Send Successfully...";
                Lbl_Errmsg.Visible = true;
                // Console.WriteLine(response);
            }
        }
        catch (WebException webEx)
        {
            Console.WriteLine(((HttpWebResponse)webEx.Response).StatusCode);
            Stream stream = ((HttpWebResponse)webEx.Response).GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            String body = reader.ReadToEnd();
            Console.WriteLine(body);
            success = false;
            Lbl_Errmsg.Text = webEx.ToString();
            Lbl_Errmsg.Visible = true;
        }

        return success;
    }

    public class Payload
    {
        public string number { get; set; }
        public string message { get; set; }
    }


    public void btn_whatsappmsg_Click(object sender, EventArgs e)
    {
        try
        {
            strarr = No.Split(',');
            SendWhatsappmsg();
        }
        catch (Exception ex)
        {
        }
    }
}
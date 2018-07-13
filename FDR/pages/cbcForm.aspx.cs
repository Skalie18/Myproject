using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using Sars.Systems.Data;
using System.Data;
using System.Xml;
using System.Web.Script.Services;
using Sars.Systems.Security;

using Newtonsoft.Json;

public partial class Default2 : System.Web.UI.Page
{
    public string userId = ADUser.CurrentSID;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                fsVerify.Visible = false;
                FDRPage fp = new FDRPage();
                if (fp.IsUserInRole("CBC Administrator"))
                // if (User.CanPerformFunction("CBC_ApproveDataReady"))
                {
                    if (Request["refno"] != null && Request["year"] != null)
                    {
                        var isApproved = DatabaseReader.CheckIfCBCApproved(Request["refno"].ToString(), int.Parse(Request["year"].ToString()));
                        switch (isApproved)
                        {
                            case -1:
                                fsVerify.Visible = true;
                                break;
                            case 0:
                            case 1:
                                fsVerify.Visible = false;
                                break;
                        }


                    }
                }
                if (!Page.IsPostBack)
                    ViewState["prevPage"] = Request.UrlReferrer;
            }
        }
        catch (Exception x)
        {
            throw new Exception(x.Message);
        }
    }



    [WebMethod(Description = "Gets CBCFormData by Tax Reference No")]
    public object GetCBCDataByTaxRefNo(int incLocal,string taxRefNo, int year)
    {
        string xml = "";
        string jsonString = "";
        try
        {
            var dsResults = DatabaseReader.GetCBCDataByTaxRefNo(incLocal,taxRefNo, year);
            if (dsResults.HasRows())
            {
                xml = dsResults.Tables[0].Rows[0][0].ToString();
                var doc = new XmlDocument();
                doc.LoadXml(xml);
                jsonString = JsonConvert.SerializeXmlNode(doc);
            }

        }
        catch (Exception x)
        {
            throw new Exception(x.Message);
        }
        return jsonString;
    }


    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (ViewState["prevPage"] != null)
            Response.Redirect(ViewState["prevPage"].ToString());
    }



 
}
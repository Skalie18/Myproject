using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FDR.DataLayer;
using Sars.Systems.Data;

public partial class admin_SentLetters : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gvMNE.Bind(DBReadManager.GetSentLetters());
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        gvMNE.Bind(DBReadManager.GetSentLetters(txtTaxRefNo.Text.Trim()));
    }

    protected void gvMNE_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvMNE.NextPage(DBReadManager.GetSentLetters(txtTaxRefNo.Text.Trim()), e.NewPageIndex);
    }
}
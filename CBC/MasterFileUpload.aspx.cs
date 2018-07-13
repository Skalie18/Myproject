using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Sars.Systems.Controls;

public partial class MasterFileUpload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ( !IsPostBack )
        {
            InitFileUploadUI();
            //var a = new LocalFileCategories();
            //a.GetRecords<LocalFileCategories>();
        }
    }
    private void InitFileUploadUI()
    {
        var items = Enumerable.Range(1,  ApplicationConfigurations.MasterFileDefaultNumberOrItems).ToList();
        gvFiles.Bind(items);
    }

    protected void btnUploadFiles_Click(object sender, EventArgs e)
    {
        
    }

    protected void gvFiles_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if(e.Row.RowType != DataControlRowType.DataRow ) { return; }

        var ddlCat = e.Row.FindControl("ddlCat" ) as DropDownField;
        if(ddlCat != null )
        {
            ddlCat.Bind(db.GetAllMasterFileCategories(), "Description", "Id");
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        foreach ( GridViewRow row in gvFiles.Rows )
        {
            if(row.RowType != DataControlRowType.DataRow )
            {
                continue;
            }
            var ddlCat = row.FindControl("ddlCat" ) as DropDownField;
            var fuMaster = row.FindControl("fuMaster" ) as FileUpload;
            if ( ddlCat != null && fuMaster != null )
            {
                if ( fuMaster.HasFile )
                {

                    var postedFile = fuMaster.PostedFile;
                }
            }
        }
    }
}
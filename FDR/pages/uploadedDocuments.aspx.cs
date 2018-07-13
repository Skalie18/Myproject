using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pages_uploadedDocuments : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        if (string.IsNullOrEmpty(Request["fId"]))
        {
            MessageBox.Show("File ID parameter is missing");
            return;
        }
        if (string.IsNullOrEmpty(Request["oId"]))
        {
            MessageBox.Show("Object ID parameter is missing");
            return;
        }

        var fileId = int.Parse(Request["fId"]);
        using (var data = DatabaseReader.getUploadedCaseFilesById(fileId))
        {
            if (data.HasRows)
            {
                gvUploadedFiles.Bind(data);
            }
        }
    }
}
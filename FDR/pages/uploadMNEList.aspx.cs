using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using PushCBCData.BO;
using FDR.DataLayer;
using PushCBCData.BO;

public partial class uploadMNEList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnGetMNEList_Click(object sender, EventArgs e)
    {
        DBWriteManager.TruncateMNEList();
        UploadFile.ProcessFile();
        MessageBox.Show(UploadFile.Message);
    }
}
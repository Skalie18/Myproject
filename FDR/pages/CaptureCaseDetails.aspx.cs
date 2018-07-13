using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Globalization;
using System.Web.UI;

public partial class CaptureCaseDetials : System.Web.UI.Page
{
   


    protected void Page_Load(object sender, EventArgs e)
    {
        
        var files = (List<HttpPostedFile>)Session["Doc"];

        if (IsPostBack)
        {
            Session["Doc"] = files;
        }
        else
        {
            Common.GetAllCountries(ddlCountryList);
            Session["Doc"] = null;
        }
        gvUploadedDocs.Bind(files);
 
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        List<HttpPostedFile> files = new List<HttpPostedFile>();

        files = (List<HttpPostedFile>)Session["Doc"];

        if (fileUpload.HasFiles)
        {
           
            foreach (var item in fileUpload.PostedFiles)
            {
                var filename = Path.GetFileName(item.FileName);
                string filepath = Server.MapPath("../Documents//"+filename);  //Save to the Documents folder
                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                }
                item.SaveAs(filepath);

                if (files ==null) {
              
                    files = new List<HttpPostedFile>();
                    files.Add(item);
                }
                else
                {
                        files.Add(item);
                }

            }
          
            gvUploadedDocs.Bind(files);
            Session["Doc"] = files;
        }

        else
        {
            MessageBox.Show("Please select files to upload!!");
        }

    }



    protected void btnSave_Click(object sender, EventArgs e)
    {

        var docs = (List<HttpPostedFile>)Session["Doc"];
        if (docs !=null) { 
        if (docs.Count != 0) {
            foreach (var item in docs)
            {
                var filename = Path.GetFileName(item.FileName);

                var docName = Server.MapPath("../Documents//" + filename);

                var url = System.Configuration.ConfigurationManager.AppSettings["document-service-url-post"];
                var path = @"D:\txt.txt";
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                using (FileStream inputStream = new FileStream(docName, FileMode.Open, FileAccess.Read))
                {

                    File.AppendAllText(path, "{ ");
                    File.AppendAllText(path, "'objectType':'sars_document',");
                    File.AppendAllText(path, "'objectName':'" + Path.GetFileName(docName) + "',");
                    File.AppendAllText(path, "'contentType':'" + Path.GetExtension(docName).Replace(".", "") + "',");
                    File.AppendAllText(path, " 'author':'" + Environment.UserName + "',");
                    File.AppendAllText(path, " 'properties':");
                    File.AppendAllText(path, "   [");
                    File.AppendAllText(path, "   { 'sars_uuid':'hahahaha'},");
                    File.AppendAllText(path, "   { 'sars_part_no':'10'},");
                    File.AppendAllText(path, "   { 'sars_guid':'SWIMS01'},");
                    File.AppendAllText(path, "   { 'sars_case_no':'1024'},");
                    File.AppendAllText(path, "   { 'sars_shred_ind':'0'},");
                    File.AppendAllText(path, "   { 'sars_application_id':'REST Service 2'},");
                    File.AppendAllText(path, "   { 'sars_transaction_step':'0'},");
                    File.AppendAllText(path, "   { 'sars_parts_total':'0'},");
                    File.AppendAllText(path, "   { 'sars_parts_no':'10'},");
                    File.AppendAllText(path, "   { 'sars_is_replica':'false'},");
                    File.AppendAllText(path, "   { 'sars_archive_flag':'0'}");
                    File.AppendAllText(path, " ],");
                    File.AppendAllText(path, "'content':'");

                    int buffer_size = 30000; //or any multiple of 3
                    byte[] buffer = new byte[buffer_size];
                    int bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                    while (bytesRead > 0)
                    {
                        byte[] buffer2 = buffer;
                        if (bytesRead < buffer_size)
                        {
                            buffer2 = new byte[bytesRead];
                            //Buffer.BlockCopy(buffer, 0, buffer2, 0, bytesRead);
                        }
                        string base64String = System.Convert.ToBase64String(buffer2);
                        File.AppendAllText(path, base64String);
                        bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                    }
                    File.AppendAllText(path, "' }");
                }

                var myReq = WebRequest.Create(url) as HttpWebRequest;
                myReq.ContentType = "application/json; charset=utf-8";
                myReq.Method = "POST";
                myReq.Accept = "application/json; charset=utf-8";

                //var arr = Encoding.ASCII.GetBytes( data);
                var arr = File.ReadAllBytes(path);
                using (var streamWriter = myReq.GetRequestStream())
                {
                    streamWriter.Write(arr, 0, arr.Length);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var httpResponse = (HttpWebResponse)myReq.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    if (!string.IsNullOrEmpty(result))
                    {
                        var x = JsonConvert.DeserializeObject<UploadResponse>(result.Replace("[", "").Replace("]", ""));

                        var uploadedfiles = new CaseDetailsUploadedFiles()
                        {
                            CaseNo = txtCaseNo.Text,
                            TaxRefNo = txtTaxRefNo.Text,
                            FileName = filename,
                            ObjectId = x.objectId,
                            Timestamp = DateTime.Now,
                            Owner = x.owner,
                            FilePath = x.filePath,
                            FileSize = x.fileSize,
                            Message = x.message,
                            DocumentumDate = x.creationDate,
                            UploadedBy = Environment.UserName
                        };
                        int uploaded = DatabaseWriter.SaveUploadedFiles(uploadedfiles);
                    }

                }

                if (File.Exists(docName)) {

                    File.Delete(docName);
                }

            }

        }
    }
        var countryItems = (ddlCountryList.SelectedItem.Text).Split('-');
        var countryName= "";
        if (countryItems != null)
        {
            countryName = countryItems[0];
        }
        else {
            countryName = ddlCountryList.SelectedValue;
        }
       
        var casedetails = new CaseDetails()
        {
            CaseNo = txtCaseNo.Text,
            TaxRefNo = txtTaxRefNo.Text,
            EntityName = txtEntityName.Text,
            RequestorUnit = txtRequestorUnit.Text,
            Year = string.IsNullOrWhiteSpace(txtYear.Text) ? int.Parse(txtYear.Text) : DateTime.Now.Year,
            CaseNotes = txtNotes.Text,
            DateRequested = string.IsNullOrWhiteSpace(txtDateRequested.Text) ? DateTime.Parse(txtDateRequested.Text) : DateTime.Now,
            DateCreated = DateTime.Now,
            DateRecieved = string.IsNullOrWhiteSpace(txtDateRecieved.Text) ? DateTime.Parse(txtDateRecieved.Text) : DateTime.Now,
            CountryCode = ddlCountryList.SelectedValue,
            CountryName = countryName
        };
        decimal saved = DatabaseWriter.SaveCaseDetails(casedetails);
        if (saved < 1)
        {
            var message ="Case Details for Case NO: "+ casedetails.CaseNo + "  Saved Successfully";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + message +  "');window.location ='../Default.aspx';", true);
            
        }
   
    }
    public class DocumentResponse
    {

        public string contentType { get; set; }
        public string objectName { get; set; }
        public string content { get; set; }
        public string objectType { get; set; }
        
        public string author { get; set; }
        public string objectId { get; set; }

    }

    public class UploadResponse
    {
        public string objectId { get; set; }
        public string message { get; set; }
        public string creationDate { get; set; }
        public string owner { get; set; }
        public string filePath { get; set; }
        public string fileSize { get; set; }
    }

    private static DocumentResponse GET(string objectId)
    {


        var url = System.Configuration.ConfigurationManager.AppSettings["document-service-url-get"];
        var myReq = WebRequest.Create(string.Format(url, objectId)) as HttpWebRequest;
        myReq.Method = "GET";
        var httpResponse = (HttpWebResponse)myReq.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            var x = JsonConvert.DeserializeObject<DocumentResponse>(result.Replace("[", "").Replace("]", ""));
            var base64 = x.content;
            var buffer = Convert.FromBase64String(base64);

            var fileName = string.Format(@"D:\Documentum\{0}", x.objectName);
            File.WriteAllBytes(fileName, buffer);

            return x;
        }
    }
    protected void lnkDeleteFile_Command(object sender, CommandEventArgs e)
    {
        var docs = (List<HttpPostedFile>)Session["Doc"];

        if (e.CommandArgument != null)
        {
            var btn = sender as LinkButton;
            if (btn == null)
            {
                return;
            }
            var row = btn.NamingContainer as GridViewRow;
            if (row == null) { return; }
            var gv = row.NamingContainer as GridView;
            if (gv == null) { return; }
            gv.SelectedIndex = row.RowIndex;
            if (gv.SelectedDataKey != null)
            {
                var docName = Server.MapPath("../Documents//" + e.CommandArgument);
                if (File.Exists(docName))
                {
                   var fileToDelete = docs.Where(x => x.FileName.Contains(e.CommandArgument.ToString())).SingleOrDefault();
                    File.Delete(docName);
                    docs.Remove(fileToDelete);
                  MessageBox.Show(e.CommandArgument + "  Deleted Successfully");
                }
                gvUploadedDocs.Bind(docs);
            }
        }
    }

    #region  Document upload from Mzwakhe


    private static UploadResponse POST()
    {
        var docName = @"D:\Users\s2025376\Documents\Documentum\shonie.pdf";

        var url = System.Configuration.ConfigurationManager.AppSettings["document-service-url-post"];
        var path = @"D:\txt.txt";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        using (FileStream inputStream = new FileStream(docName, FileMode.Open, FileAccess.Read))
        {

            File.AppendAllText(path, "{ ");
            File.AppendAllText(path, "'objectType':'sars_document',");
            File.AppendAllText(path, "'objectName':'" + Path.GetFileName(docName) + "',");
            File.AppendAllText(path, "'contentType':'" + Path.GetExtension(docName).Replace(".", "") + "',");
            File.AppendAllText(path, " 'author':'" + Environment.UserName + "',");
            File.AppendAllText(path, " 'properties':");
            File.AppendAllText(path, "   [");
            File.AppendAllText(path, "   { 'sars_uuid':'hahahaha'},");
            File.AppendAllText(path, "   { 'sars_part_no':'10'},");
            File.AppendAllText(path, "   { 'sars_guid':'SWIMS01'},");
            File.AppendAllText(path, "   { 'sars_case_no':'1024'},");
            File.AppendAllText(path, "   { 'sars_shred_ind':'0'},");
            File.AppendAllText(path, "   { 'sars_application_id':'REST Service 2'},");
            File.AppendAllText(path, "   { 'sars_transaction_step':'0'},");
            File.AppendAllText(path, "   { 'sars_parts_total':'0'},");
            File.AppendAllText(path, "   { 'sars_parts_no':'10'},");
            File.AppendAllText(path, "   { 'sars_is_replica':'false'},");
            File.AppendAllText(path, "   { 'sars_archive_flag':'0'}");
            File.AppendAllText(path, " ],");
            File.AppendAllText(path, "'content':'");

            int buffer_size = 30000; //or any multiple of 3
            byte[] buffer = new byte[buffer_size];
            int bytesRead = inputStream.Read(buffer, 0, buffer.Length);
            while (bytesRead > 0)
            {
                byte[] buffer2 = buffer;
                if (bytesRead < buffer_size)
                {
                    buffer2 = new byte[bytesRead];
                    // Buffer.BlockCopy(buffer, 0, buffer2, 0, bytesRead);
                }
                string base64String = System.Convert.ToBase64String(buffer2);
                File.AppendAllText(path, base64String);
                bytesRead = inputStream.Read(buffer, 0, buffer.Length);
            }
            File.AppendAllText(path, "' }");
        }

        var myReq = WebRequest.Create(url) as HttpWebRequest;
        myReq.ContentType = "application/json; charset=utf-8";
        myReq.Method = "POST";
        myReq.Accept = "application/json; charset=utf-8";

        //var arr = Encoding.ASCII.GetBytes( data);
        var arr = File.ReadAllBytes(path);
        using (var streamWriter = myReq.GetRequestStream())
        {
            streamWriter.Write(arr, 0, arr.Length);
            streamWriter.Flush();
            streamWriter.Close();
        }
        var httpResponse = (HttpWebResponse)myReq.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            if (!string.IsNullOrEmpty(result))
            {
                var x = JsonConvert.DeserializeObject<UploadResponse>(result.Replace("[", "").Replace("]", ""));
                return x;
            }
        }

        return null;
    }


    #endregion


  
}
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.Services;
using Sars.Systems.Security;

public partial class _Default : System.Web.UI.Page
{
    [WebMethod]                                 //Default.aspx.cs
    public static string GetUserDetails()
    {
        return "{name:\"Sibusiso\", surname:\"Gigaba\"}";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        /*
        var userProfile = User.GetRole();
        if (string.IsNullOrEmpty(userProfile))
        {
            try
            {
                User.AddToRole("Company Admin");
                //User.AddToRole("System Admin");
            }
            catch (Exception)
            {
                ;
            }
        }*/
        //Create(); 
        //CreateNew();
        //Roles.AddUserToRole("Developers", User.Identity.Name);
        //Sars.Systems.Security.RolesConfiguration.UnInstall();
        //Sars.Systems.Security.RolesConfiguration.Install();
        //this.OpenForm(FormType.ViewMenu);
    }

    private static void Create()
    {
        //var cat = new Categories("[dbo].[GetCategories]", null)
        //{
        //    CategoryID = 0,
        //    CategoryName =  Guid.NewGuid().ToString("D").Substring(0, 15),
        //    Description =  "This include but no limited to bread, sugar and pap",
        //    Picture = File.ReadAllBytes(@"D:\Used Procedures.txt")
        //};
        //cat.ApplyChanges();

        //var categories = cat.GetRecords<Categories>();
    }

    private static void CreateNew()
    {
        //var customer = new Customers
        //{
        //    CustomerId = Guid.NewGuid().ToString("N").Substring(0, 5),
        //    CompanyName = "Sbugig Sulutions",
        //    ContactName = "Ayanda Mfusi",
        //    ContactTitle = "Business Analyst",
        //    Address = "299 Bronkhorst",
        //    City = "PTA",
        //    Region = "GP",
        //    PostalCode = "2000",
        //    Country = "South Africa",
        //    Phone = "012 422 6042",
        //    Fax = "012 422 5000"
        //};

        //customer.ApplyChanges();

        //OR

        //customer.ApplyChanges(schemaName:"dbo");

        //OR

        //customer.ApplyChanges("[dbo].[uspUPSERT_Customers]");

        //OR

        //customer.ApplyChanges("[dbo].[uspUPSERT_Customers]", "dbo");

        //OR
        //var outputOrReturnParameter = new Dictionary<string, ParameterDirection>
        //{
        //    {"@Test1", ParameterDirection.Output},
        //    {"@Test2", ParameterDirection.ReturnValue}
        //};
        //customer.ApplyChanges("[dbo].[uspUPSERT_Customers]", additionalParameters: outputOrReturnParameter);
    }
}
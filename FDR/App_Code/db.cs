using System.Data;
using Sars.Systems.Data;
//using System.DirectoryServices.AccountManagement;
//using System.DirectoryServices;
/// <summary>
/// Summary description for db
/// </summary>
public class db
{
    public static string ConnectionString
    {
        get
        {
            //var domain = System.DirectoryServices.ActiveDirectory.Domain.GetComputerDomain();

            //DirectoryEntry entry = new DirectoryEntry(string.Format( "LDAP://{0}", domain.ToString()));

            //DirectorySearcher dSearch = new DirectorySearcher(entry);

            //var SID = "s1037927";
            //dSearch.Filter = "(&(objectClass=user)(sAMAccountName=" + SID + "))";

            //SearchResult sResult = dSearch.FindOne();
            //var mail = GetProperty(sResult, "mail");


           return System.Configuration.ConfigurationManager.ConnectionStrings["survey"].ConnectionString;
        }
    }

  public static string ExcelConnectionString
    {
        get
        {
           return System.Configuration.ConfigurationManager.ConnectionStrings["excelCon"].ConnectionString;
        }
    }

    public static string AssetsConnectionString
    {
        get
        {
            //var domain = System.DirectoryServices.ActiveDirectory.Domain.GetComputerDomain();

            //DirectoryEntry entry = new DirectoryEntry(string.Format( "LDAP://{0}", domain.ToString()));

            //DirectorySearcher dSearch = new DirectorySearcher(entry);

            //var SID = "s1037927";
            //dSearch.Filter = "(&(objectClass=user)(sAMAccountName=" + SID + "))";

            //SearchResult sResult = dSearch.FindOne();
            //var mail = GetProperty(sResult, "mail");


            return System.Configuration.ConfigurationManager.ConnectionStrings["assets"].ConnectionString;
        }
    }

    //public static string GetProperty(SearchResult searchResult, string PropertyName)
    //{
    //    if (searchResult.Properties.Contains(PropertyName))
    //    {
    //        return searchResult.Properties[PropertyName][0].ToString();
    //    }
    //    else
    //    {
    //        return string.Empty;
    //    }
    //}
    public static DBConnection Connection
    {
        get
        {
            
           
            return new DBConnection(ConnectionString);
        }
    }
    public static DBConnection TransactionConnection
    {
        get
        {
            var oConnection = new DBConnection(ConnectionString, true, IsolationLevel.ReadUncommitted);
            return oConnection;
        }
    }


}

using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;
using Sars.Systems.Data;

/// <summary>
/// Summary description for UserDetails
/// </summary>

[Table]
public class UserDetails : DataAccessObject
{
    public UserDetails()
    {
    }

    public UserDetails(string procedureName, Dictionary<string, object> parameters) : base(procedureName, parameters)
    {
    }

    [Column(Name = "TaxUserName")]
    public string TaxUserName { get; set; }


    [Column(Name = "Capacity")]
    public string Capacity { get; set; }


    [Column(Name = "DialCode")]
    public string DialCode { get; set; }

    [Column(Name = "TelephoneNumber")]
    public string TelephoneNumber { get; set; }

}
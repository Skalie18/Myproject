using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Sars.Systems.Data;
using System.Collections.Generic;

[Table(Name = "CaseDetails")]
public class CaseDetails : Sars.Systems.Data.DataAccessObject
{
    public CaseDetails()
    {
    }
    public CaseDetails(string procedure, Dictionary<string, object> parameters) : base(procedure, parameters)
    {
    }

    [Column(Name = "Id"), DBQueryParameter("@Id")]
    public decimal Id { get; set; }
    [Column(Name = "TaxRefNo"), DBQueryParameter("@TaxRefNo")]
    public string TaxRefNo { get; set; }
    [Column(Name = "CaseNo"), DBQueryParameter("@CaseNo")]
    public string CaseNo { get; set; }
    [Column(Name = "EntityName"), DBQueryParameter("@EntityName")]
    public string EntityName { get; set; }
    [Column(Name = "RequestorUnit"), DBQueryParameter("@RequestorUnit")]
    public string RequestorUnit { get; set; }
    [Column(Name = "Year"), DBQueryParameter("@Year")]
    public int Year { get; set; }
    [Column(Name = "CaseNotes"), DBQueryParameter("@CaseNotes")]
    public string CaseNotes { get; set; }
    [Column(Name = "DateRequested"), DBQueryParameter("@DateRequested")]
    public DateTime DateRequested { get; set; }
    [Column(Name = "DateCreated"), DBQueryParameter("@DateCreated")]
    public DateTime DateCreated { get; set; }
    [Column(Name = "CountryName"), DBQueryParameter("@CountryName")]
    public string CountryName { get; set; }
    [Column(Name = "CountryCode"), DBQueryParameter("@CountryCode")]
    public string CountryCode { get; set; }
    [Column(Name = "DateRecieved"), DBQueryParameter("@DateRecieved")]
    public DateTime DateRecieved { get; set; }
}
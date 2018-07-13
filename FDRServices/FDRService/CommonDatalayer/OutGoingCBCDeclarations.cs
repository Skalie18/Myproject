using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Sars.Systems.Data;
using System.Collections.Generic;

[Table(Name="OutGoingCBCDeclarations")]
public class OutGoingCBCDeclarations : Sars.Systems.Data.DataAccessObject
{
	public OutGoingCBCDeclarations()
	{
	}
	 public OutGoingCBCDeclarations(string procedure, Dictionary<string, object> parameters) : base(procedure, parameters)
	{
	}

	[Column(Name="Id"), DBQueryParameter("@Id")]
	public decimal Id { get; set; }
	[Column(Name="Country"), DBQueryParameter("@Country")]
	public string Country { get; set; }
	[Column(Name="StatusId"), DBQueryParameter("@StatusId")]
	public int StatusId { get; set; }
	[Column(Name="CBCData"), DBQueryParameter("@CBCData")]
	public string CBCData { get; set; }
    [Column(Name = "Year"), DBQueryParameter("@Year")]
    public int Year { get; set; }
    [Column(Name="TimeStamp"), DBQueryParameter("@TimeStamp")]
	public DateTime TimeStamp { get; set; }
	[Column(Name="CreatedBy"), DBQueryParameter("@CreatedBy")]
	public string CreatedBy { get; set; }
	[Column(Name="UpdatedBy"), DBQueryParameter("@UpdatedBy")]
	public string UpdatedBy { get; set; }
	[Column(Name="DateUpdated"), DBQueryParameter("@DateUpdated")]
	public DateTime? DateUpdated { get; set; }

    [Column(Name = "CountryName"), DBQueryParameter("@CountryName")]
    public string CountryName { get; set; }

}
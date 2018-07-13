using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Sars.Systems.Data;
using System.Collections.Generic;

[Table(Name="COMMENTS")]
public class Comments : Sars.Systems.Data.DataAccessObject
{
    
	public Comments()
	{
	}
	 public Comments(string procedure, Dictionary<string, object> parameters) : base(procedure, parameters)
	{
	}

	[Column(Name="ID"), DBQueryParameter("@ID")]
	public decimal ID { get; set; }
	[Column(Name="OutGoingCBCDeclarationsID"), DBQueryParameter("@OutGoingCBCDeclarationsID")]
	public decimal OutGoingCBCDeclarationsID { get; set; }
	[Column(Name="NOTE"), DBQueryParameter("@NOTE")]
	public string Notes { get; set; }
	[Column(Name="DATEADDED"), DBQueryParameter("@DATEADDED")]
	public DateTime DateAdded { get; set; }
	[Column(Name="ADDEDBY"), DBQueryParameter("@ADDEDBY")]
	public string AddedBy { get; set; }
}
using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Sars.Systems.Data;
using System.Collections.Generic;

[Table(Name="MneEnquireRequests")]
public class MneEnquireRequests : Sars.Systems.Data.DataAccessObject
{
	public MneEnquireRequests()
	{
	}
	 public MneEnquireRequests(string procedure, Dictionary<string, object> parameters) : base(procedure, parameters)
	{
	}

	[Column(Name="Id"), DBQueryParameter("@Id")]
	public decimal Id { get; set; }
	[Column(Name="Timestamp"), DBQueryParameter("@Timestamp")]
	public DateTime Timestamp { get; set; }
	[Column(Name="MessageID"), DBQueryParameter("@MessageID")]
	public string MessageID { get; set; }
	[Column(Name="Message"), DBQueryParameter("@Message")]
	public string Message { get; set; }
}
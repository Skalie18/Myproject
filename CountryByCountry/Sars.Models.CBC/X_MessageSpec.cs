using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Sars.Systems.Data;
using System.Collections.Generic;

[Table(Name="X_MessageSpec")]
public class X_MessageSpec : Sars.Systems.Data.DataAccessObject
{
	public X_MessageSpec()
	{
	}
	 public X_MessageSpec(string procedure, Dictionary<string, object> parameters) : base(procedure, parameters)
	{
	}

	[Column(Name="MessageSpec_ID"), DBQueryParameter("@MessageSpec_ID")]
	public decimal MessageSpec_ID { get; set; }
	[Column(Name="Timestamp"), DBQueryParameter("@Timestamp")]
	public DateTime Timestamp { get; set; }
	[Column(Name="MessageSpec"), DBQueryParameter("@MessageSpec")]
	public string MessageSpec { get; set; }
	[Column(Name="TransmittingCountry"), DBQueryParameter("@TransmittingCountry")]
	public string TransmittingCountry { get; set; }
	[Column(Name="ReceivingCountry"), DBQueryParameter("@ReceivingCountry")]
	public string ReceivingCountry { get; set; }
	[Column(Name="MessageType"), DBQueryParameter("@MessageType")]
	public string MessageType { get; set; }
	[Column(Name="Language"), DBQueryParameter("@Language")]
	public string Language { get; set; }
	[Column(Name="Warning"), DBQueryParameter("@Warning")]
	public string Warning { get; set; }
	[Column(Name="Contact"), DBQueryParameter("@Contact")]
	public string Contact { get; set; }
	[Column(Name="MessageRefId"), DBQueryParameter("@MessageRefId")]
	public string MessageRefId { get; set; }
	[Column(Name="ReportingPeriod"), DBQueryParameter("@ReportingPeriod")]
	public DateTime? ReportingPeriod { get; set; }
	[Column(Name="MessageTypeIndic"), DBQueryParameter("@MessageTypeIndic")]
	public string MessageTypeIndic { get; set; }
	[Column(Name="MessageTimestamp"), DBQueryParameter("@MessageTimestamp")]
	public DateTime? MessageTimestamp { get; set; }
	[Column(Name="StatusID"), DBQueryParameter("@StatusID")]
	public int? StatusID { get; set; }
	[Column(Name="FinalStatus"), DBQueryParameter("@FinalStatus")]
	public string FinalStatus { get; set; }
	[Column(Name="UpdatedBy"), DBQueryParameter("@UpdatedBy")]
	public string UpdatedBy { get; set; }
	[Column(Name="DateUpdated"), DBQueryParameter("@DateUpdated")]
	public DateTime? DateUpdated { get; set; }

    [Column(Name = "UID"), DBQueryParameter("@UID")]
    public Guid UID { get; set; }
}
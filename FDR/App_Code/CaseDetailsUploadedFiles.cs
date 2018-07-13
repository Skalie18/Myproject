using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Sars.Systems.Data;
using System.Collections.Generic;

[Table(Name="CaseDetailsUploadedFiles")]
public class CaseDetailsUploadedFiles : Sars.Systems.Data.DataAccessObject
{
	public CaseDetailsUploadedFiles()
	{
	}
	 public CaseDetailsUploadedFiles(string procedure, Dictionary<string, object> parameters) : base(procedure, parameters)
	{
	}

	[Column(Name="Id"), DBQueryParameter("@Id")]
	public int Id { get; set; }
	[Column(Name="TaxRefNo"), DBQueryParameter("@TaxRefNo")]
	public string TaxRefNo { get; set; }
	[Column(Name="CaseNo"), DBQueryParameter("@CaseNo")]
	public string CaseNo { get; set; }
	[Column(Name="FileName"), DBQueryParameter("@FileName")]
	public string FileName { get; set; }
	[Column(Name="ObjectId"), DBQueryParameter("@ObjectId")]
	public string ObjectId { get; set; }
	[Column(Name="FilePath"), DBQueryParameter("@FilePath")]
	public string FilePath { get; set; }
	[Column(Name="FileSize"), DBQueryParameter("@FileSize")]
	public string FileSize { get; set; }
	[Column(Name="Message"), DBQueryParameter("@Message")]
	public string Message { get; set; }
	[Column(Name="Owner"), DBQueryParameter("@Owner")]
	public string Owner { get; set; }
	[Column(Name="DocumentumDate"), DBQueryParameter("@DocumentumDate")]
	public string DocumentumDate { get; set; }
	[Column(Name="UploadedBy"), DBQueryParameter("@UploadedBy")]
	public string UploadedBy { get; set; }
	[Column(Name="Timestamp"), DBQueryParameter("@Timestamp")]
	public DateTime? Timestamp { get; set; }
}
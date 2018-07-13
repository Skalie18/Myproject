using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Sars.Systems.Data;
using System.Collections.Generic;

[Table(Name = "RequestReasonCodes")]
public class RequestReasonCodes : Sars.Systems.Data.DataAccessObject
{
    public RequestReasonCodes()
    {
    }

    public RequestReasonCodes(string procedure, Dictionary<string, object> parameters) : base(procedure, parameters)
    {
    }

    [Column(Name = "Id"), DBQueryParameter("@Id")]
    public int Id { get; set; }

    [Column(Name = "Timestamp"), DBQueryParameter("@Timestamp")]
    public DateTime Timestamp { get; set; }

    [Column(Name = "Code"), DBQueryParameter("@Code")]
    public string Code { get; set; }

    [Column(Name = "Description"), DBQueryParameter("@Description")]
    public string Description { get; set; }

    [Column(Name = "FormName"), DBQueryParameter("@FormName")]
    public string FormName { get; set; }

    [Column(Name = "IsActive"), DBQueryParameter("@IsActive")]
    public bool IsActive { get; set; }

    [Column(Name = "SortOrder"), DBQueryParameter("@SortOrder")]
    public int? SortOrder { get; set; }
}
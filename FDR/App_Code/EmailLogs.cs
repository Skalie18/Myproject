using System;
using System.Data.Linq.Mapping;
/// <summary>
/// Summary description for EmailLogs
/// </summary>
public class EmailLogs
{
    public EmailLogs()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    [Column(Name = "Id")]
    public int Id { get; set; }

    [Column(Name = "SendBy")]
    public string SendBy { get; set; }

    [Column(Name = "SendTo")]
    public string SendTo { get; set; }

    [Column(Name = "EmailMessage")]
    public string EmailMessage { get; set; }

    [Column(Name = "EmailSubject")]
    public string EmailSubject { get; set; }

    [Column(Name = "DateSend")]
    public DateTime? DateSend { get; set; }

    
}
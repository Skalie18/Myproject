using System;
using System.Data.Linq.Mapping;

[Table(Name = "Users")]
public partial class Users
{
    [Column(Name = "UserId")]
    public int UserId { get; set; }

    [Column(Name = "SID")]
    public string SID { get; set; }

    [Column(Name = "Name")]
    public string Name { get; set; }

    [Column(Name = "Surname")]
    public string Surname { get; set; }

    [Column(Name = "OfficeId")]
    public int? OfficeId { get; set; }

    [Column(Name = "UserRoleId")]
    public int? UserRoleId { get; set; }

    [Column(Name = "DivisionId")]
    public int DivisionId { get; set; }

    [Column(Name = "IsActive")]
    public bool IsActive { get; set; }

    [Column(Name = "Timestamp")]
    public DateTime Timestamp { get; set; }

    [Column(Name = "FullName")]
    public string FullName { get; set; }

    [Column(Name = "DepartmentCode")]
    public long? DepartmentCode { get; set; }

    [Column(Name = "ManagerSID")]
    public string ManagerSID { get; set; }

    [Column(Name = "DepartmentDescription")]
    public string DepartmentDescription { get; set; }

    [Column(Name = "DivisionCode")]
    public int? DivisionCode { get; set; }

    [Column(Name = "DivisionDescription")]
    public string DivisionDescription { get; set; }

    [Column(Name = "Mail")]
    public string Mail{ get; set; }
    
    [Column(Name = "EmployeeNumber")]
    public string EmployeeNumber { get; set; }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sars.Systems.Controls;
using CheckBoxField = Sars.Systems.Controls.CheckBoxField;

/// <summary>
/// Summary description for Common
/// </summary>
public static class CommonFunctions
{
    public static string TaxUserId
    {
        get
        {
            var httpContext = HttpContext.Current;
            var httpSession = httpContext.Session;

            return httpSession["TaxUserID"] == null ? "" : httpSession["TaxUserID"] .ToString(); 

        }
    }
    public static string TaxPayerId
    {
        get
        {
            var httpContext = HttpContext.Current;
            var httpSession = httpContext.Session;
              
           return httpSession["TaxPayerID"] == null ? "" : httpSession["TaxPayerID"].ToString();

        }
    }

    public static bool IsValidFsb19(string fsbNo)
    {
        if (string.IsNullOrEmpty(fsbNo))
        {
            return true;
        }
        var firstPart = fsbNo.Substring(0, 7);
        if ( firstPart != "12/8/00" )
        {
            return false;
        }

        if ( fsbNo.Length != 19 )
        { 
            return false;
        }
        var thirdPart = fsbNo.Substring(5, 7);
        if ( Convert.ToInt32(thirdPart) == 0 )
        { 
            return false;
        }

        //var fourthPart = value.substr(13, 6);
        //if (parseInt(fourthPart) === 0) {
        //    field.style.backgroundColor = "red";
        //    field.value = "";
        //    field.focus();
        //    alert("FSB number correct format is (12/8/0012345/678901)");
        //    return false;
        //}

        if ( fsbNo.Substring(2, 1) != "/" )
        { 
            return false;
        }
        if ( fsbNo.Substring(4, 1) != "/" )
        { 
            return false;
        }
        if ( fsbNo.Substring(12, 1) != "/" )
        {
            return false;
        }
        return true;
    }

    public static void AddTableColumns(DataTable targeTable, List<Control> controls)
    {
        if (!controls.Any())
        {
            return;
        }
        foreach (var thisField in controls)
        {
            var control = thisField as ISarsControl;
            if (control != null)
            {
                if (!targeTable.Columns.Contains(control.BoundField))
                {
                    targeTable.Columns.Add(control.BoundField, typeof (string));
                }

                continue;
            }
            var rangeControl = thisField as IDateRangeControl;
            if (rangeControl != null)
            {
                if (!targeTable.Columns.Contains(rangeControl.DateFromBoundField))
                {
                    targeTable.Columns.Add(rangeControl.DateFromBoundField, typeof (string));
                }
                if (!targeTable.Columns.Contains(rangeControl.DateToBoundField))
                {
                    targeTable.Columns.Add(rangeControl.DateToBoundField, typeof (string));
                }
                continue;
            }

            var streetAddressControl = thisField as IStreetAddress;
            if ( streetAddressControl != null )
            {

                if ( !targeTable.Columns.Contains(streetAddressControl.AddressLine1BoundField) )
                {
                    targeTable.Columns.Add(streetAddressControl.AddressLine1BoundField, typeof(string));
                }

                if ( !targeTable.Columns.Contains(streetAddressControl.AddressLine2BoundField) )
                {
                    targeTable.Columns.Add(streetAddressControl.AddressLine2BoundField, typeof(string));
                }


                if ( !targeTable.Columns.Contains(streetAddressControl.AddressLine3BoundField) )
                {
                    targeTable.Columns.Add(streetAddressControl.AddressLine3BoundField, typeof(string));
                }
                if ( !targeTable.Columns.Contains(streetAddressControl.AddressLine4BoundField) )
                {
                    targeTable.Columns.Add(streetAddressControl.AddressLine4BoundField, typeof(string));
                }

                if ( !targeTable.Columns.Contains(streetAddressControl.AddressLine4BoundField) )
                {
                    targeTable.Columns.Add(streetAddressControl.PostalCodeBoundField, typeof(string));
                }
                if ( !targeTable.Columns.Contains(streetAddressControl.PostalCodeBoundField) )
                {
                    targeTable.Columns.Add(streetAddressControl.PostalCodeBoundField, typeof(string));
                }
            }

            var telephone = thisField as ISarsTelephoneControl;
            if ( telephone != null )
            {
                if ( telephone.SeperateDialCode )
                {
                    if ( !targeTable.Columns.Contains(telephone.DialingCodeBoundField) )
                    {
                        targeTable.Columns.Add(telephone.DialingCodeBoundField, typeof(string));
                    }
                    if ( !targeTable.Columns.Contains(telephone.BoundField) )
                    {
                        targeTable.Columns.Add(telephone.BoundField, typeof(string));
                    }
                }
                else
                {
                    if ( !targeTable.Columns.Contains(telephone.BoundField) )
                    {
                        targeTable.Columns.Add(telephone.BoundField, typeof(string));
                    }
                }
            }
        }
    }
    public static void AddNewRow(DataTable targeTable, List<Control> controls)
    {
        if (targeTable == null)
        {
            return;
        }
        if (targeTable.Columns.Count == 0)
        {
            return;
        }
        if (controls == null)
        {
            return;
        }
        if (!controls.Any())
        {
            return;
        }
        var dataRow = targeTable.NewRow();
        foreach (var thisField in controls)
        {
            var control = thisField as ISarsControl;
            if (control != null)
            {
                if (targeTable.Columns.Contains(control.BoundField))
                {
                    if (control is CheckBoxField)
                    {
                        dataRow[control.BoundField] = ((CheckBoxField) control).YesNo;
                    }
                    else
                    {
                        dataRow[control.BoundField] = control.FieldValue ?? string.Empty;
                    }
                }
                continue;
            }
            var rangeControl = thisField as IDateRangeControl;
            if (rangeControl != null)
            {
                if (targeTable.Columns.Contains(rangeControl.DateFromBoundField))
                {
                    dataRow[rangeControl.DateFromBoundField] = rangeControl.DateFromFieldValue ?? string.Empty; 
                }
                if (targeTable.Columns.Contains(rangeControl.DateToBoundField))
                {
                    dataRow[rangeControl.DateToBoundField] = rangeControl.DateToFieldValue ?? string.Empty;
                }
            }
            var streetAddressControl = thisField as IStreetAddress;
            if ( streetAddressControl != null )
            {

                if ( targeTable.Columns.Contains(streetAddressControl.AddressLine1BoundField) )
                {
                    dataRow[streetAddressControl.AddressLine1BoundField] = streetAddressControl.AddressLine1FieldValue;
                }

                if ( targeTable.Columns.Contains(streetAddressControl.AddressLine2BoundField) )
                {
                    dataRow[streetAddressControl.AddressLine2BoundField] = streetAddressControl.AddressLine2FieldValue;
                }
                if ( targeTable.Columns.Contains(streetAddressControl.AddressLine3BoundField) )
                {
                    dataRow[streetAddressControl.AddressLine3BoundField] = streetAddressControl.AddressLine3FieldValue;
                }
                if ( targeTable.Columns.Contains(streetAddressControl.AddressLine4BoundField) )
                {
                    dataRow[streetAddressControl.AddressLine4BoundField] = streetAddressControl.AddressLine4FieldValue;
                }

                if (targeTable.Columns.Contains(streetAddressControl.PostalCodeBoundField))
                {
                    dataRow[streetAddressControl.PostalCodeBoundField] = streetAddressControl.PostalCodeFieldValue;
                }

            }

            var telephone = thisField as ISarsTelephoneControl;
            if ( telephone != null )
            {
                if ( telephone.SeperateDialCode )
                {
                    if ( targeTable.Columns.Contains(telephone.DialingCodeBoundField) )
                    {
                        dataRow[telephone.DialingCodeBoundField] = telephone.DialingCodeFieldValue;
                    }
                    if ( targeTable.Columns.Contains(telephone.BoundField) )
                    {
                        dataRow[telephone.BoundField] = telephone.TelephoneFieldValue;
                    }
                }
                else
                {
                    if ( targeTable.Columns.Contains(telephone.BoundField) )
                    {
                        dataRow[telephone.BoundField] = telephone.FieldValue;
                    }
                }
            }
        }
        targeTable.Rows.Add(dataRow);
    }

    public static string GetCurrentUserName()
    {
        //if (ApplicationConfigurations.DevTesting || ApplicationConfigurations.QATesting)
        //{
        //    //TODO: please remove this, it is ONLY for testing.
        //    var httpContext = HttpContext.Current;
        //    var httpSession = httpContext.Session;

        //    var taxUserId = httpSession["TaxUserID"];
        //    var taxPayerId = httpSession["TaxPayerID"];

        //    return string.Format("User ID: {0} Taxpayer ID : {1}", taxUserId, taxPayerId);
        //}

#if DEBUG
        var httpContext = HttpContext.Current;
            var httpSession = httpContext.Session;

            var taxUserId = httpSession["TaxUserID"];
            var taxPayerId = httpSession["TaxPayerID"];

            return string.Format("User ID: {0} Taxpayer ID : {1}", taxUserId, taxPayerId);
#endif
        return string.Empty;

    }

    public static int GetTaxYear(DateTime date)
    {
        var month = date.Month;
        var year = date.Year;
        if ( month < 3 )
        {
            return year;
        }
        if ( month >= 3 )
        {
            return year + 1;
        }
        return year;
    }

    public static bool IsValidDirectivesDateFormat(string date)
    {
        if (string.IsNullOrEmpty(date))
            return true;
        DateTime newDateTime;
        if (!DateTime.TryParse(date, out newDateTime) )
        {
            MessageBox.Show("Date must be in a format CCYY-MM-DD ");
            return false;
        }
        if (date.Length != 10)
        {
            MessageBox.Show("Date must be in a format CCYY-MM-DD ");
            return false;
        }
        return true;
    }

    public static decimal Sum(decimal[] amounts)
    {
        return amounts.ToList().Sum();
    }
}

public enum RequestForm
{
    /// <summary>
    /// This represents form A&D
    /// </summary>
    AD,
    /// <summary>
    /// 
    /// </summary>
    B,
    /// <summary>
    /// 
    /// </summary>
    C,
    /// <summary>
    /// 
    /// </summary>
    E,
    /// <summary>
    /// 
    /// </summary>
    IRP3A,
    /// <summary>
    /// 
    /// </summary>
    IRP3S,
    /// <summary>
    /// 
    /// </summary>
    ROT01,
    /// <summary>
    /// 
    /// </summary>
    ROT02
}

public static class XDocumentHelper
{
    public static IEnumerable<XElement> DescendantElements(this XDocument xDocument, string nodeName)
    {
        return xDocument.Descendants().Where(p => p.Name.LocalName == nodeName);
    }
}
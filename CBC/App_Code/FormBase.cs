using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Sars.Systems.Controls;
using Sars.Systems.Data;

public class FormBase : Page
{
    protected override void OnPreInit(EventArgs e)
    {
        if (ApplicationConfigurations.DevTesting)
        {
            MasterPageFile = SARSDataSettings.Settings.SecondaryMasterPageFile;
        }
    }
    public DataTable GetTable(string tableName)
    {
        var table = new DataTable(tableName);
        GetControls(this);
        if (!_controls.Any())
        {
            return null;
        }
        CommonFunctions.AddTableColumns(table, _controls);
        CommonFunctions.AddNewRow(table, _controls);
        return table;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<Control> AreAllRequiredFieldsCaptured()
    {
        GetControls(this);
        var requiredFields = new List<Control>();
        if (_controls.Any())
        {
            foreach (var thisField in _controls)
            {
                var webcontrol=(WebControl)thisField;
                if (webcontrol.CssClass == "requireddata")
                {
                    webcontrol.CssClass = string.Empty;
                }
                var control = thisField as ISarsControl;
                if (control != null)
                { 
                    if (control.IsMandatory && control.FieldValue == null)
                    {
                        requiredFields.Add(thisField);
                    }
                }
                var rangeControl = thisField as IDateRangeControl;
                if (rangeControl != null)
                {
                    if (rangeControl.IsMandatory && (rangeControl.DateToFieldValue == null || rangeControl.DateFromFieldValue == null))
                    {
                        requiredFields.Add(thisField);
                    }
                }
            }
        }
        return requiredFields;
    }

    public List<Control> FindFields (Type baseType)
    {
        GetControls(this);
        var requiredFields = new List<Control>();
        if ( _controls.Any() )
        {
            foreach ( var thisField in _controls )
            {
                if( thisField .GetType() == baseType )
                {
                    requiredFields.Add(thisField);
                }
            }
        }
        return requiredFields;
    }
    public void BindFields(DataRow row)
    {
        if (row == null)
        {
            ChangeFieldStatus(CanEdit);
            return;
        }
        GetControls(this);
        if (!_controls.Any())
        {
            return;
        }
        foreach (var thisField in _controls)
        {
            var control = thisField as ISarsControl;
            if (control != null)
            {
                var field = control;
                field.SetValue(db.SetFieldValue<string>(row, field.BoundField));
                ((WebControl) field).Enabled = CanEdit;
                continue;
            }
            var rangeControl = thisField as IDateRangeControl;
            if (rangeControl != null)
            {
                var field = rangeControl;
                field.SetRangeValue(db.SetFieldValue<string>(row, field.DateFromBoundField),
                    db.SetFieldValue<string>(row, field.DateToBoundField));
                ((WebControl) field).Enabled = CanEdit;
                continue;
            }

            var streetAddressControl = thisField as IStreetAddress;
            if (streetAddressControl != null)
            {
                streetAddressControl.AddressLine1 = db.SetFieldValue<string>(row,
                    streetAddressControl.AddressLine1BoundField);
                streetAddressControl.AddressLine2 = db.SetFieldValue<string>(row,
                    streetAddressControl.AddressLine2BoundField);
                streetAddressControl.AddressLine3 = db.SetFieldValue<string>(row,
                    streetAddressControl.AddressLine3BoundField);
                streetAddressControl.AddressLine4 = db.SetFieldValue<string>(row,
                    streetAddressControl.AddressLine4BoundField);
                streetAddressControl.PostalCode = db.SetFieldValue<string>(row,
                    streetAddressControl.PostalCodeBoundField);
                ( ( WebControl ) streetAddressControl ).Enabled = CanEdit;
                continue;
            }
            var telephone = thisField as ISarsTelephoneControl;
            if (telephone != null)
            {
                if (telephone.SeperateDialCode)
                {
                    telephone.SetDialCodeValue(db.SetFieldValue<string>(row, telephone.DialingCodeBoundField));
                    telephone.SetPhoneValue(db.SetFieldValue<string>(row, telephone.BoundField));
                    ((WebControl) telephone).Enabled = CanEdit;
                }
                else
                {
                    telephone.SetValue(db.SetFieldValue<string>(row, telephone.BoundField));
                    ( ( WebControl ) telephone ).Enabled = CanEdit;
                }
            }
        }
    }
    private List<Control> _controls;
    public void GetControls(Control parentControl)
    {
        if (_controls == null) { _controls = new List<Control>(); }
        foreach (Control control in parentControl.Controls)
        {
            if (control is ISarsControl || control is IDateRangeControl || control is IStreetAddress || control is ISarsTelephoneControl )
            {
                if (!_controls.Contains(control))
                {
                    _controls.Add(control);
                }
                //GetControls(control);
            }
            else
            {
                GetControls(control);
            }
        }
    }
    public DataRow FormDataSource
    {
        set
        {
            BindFields(value);
        }
    }
    public bool IsValidated()
    {
        var requidField = AreAllRequiredFieldsCaptured();
        WebControl firstControl = null;
        if (requidField.Any())
        {
            foreach (var control in requidField)
            {
                var moneyField = control as MoneyField;
                if ( moneyField != null )
                {
                    moneyField.TxtRands.BackColor = System.Drawing.ColorTranslator.FromHtml("#faffbd");
                    continue;
                }
                if (firstControl == null)
                {
                    firstControl = ((WebControl) control);
                }
                ((WebControl)control).CssClass = "requireddata";
            }
            if ( firstControl != null )
            {
                firstControl.Focus();
            }
            MessageBox.Show("Please complete all highlighted fields.");
            return false;
        }
      
        return true;
    }
    /// <summary>
    /// 
    /// </summary>
    public decimal DirectiveRequestId
    {
        get
        {
            return !string.IsNullOrEmpty(Request["reqid"])
                ? Convert.ToDecimal(Request["reqid"])
                : ViewState["reqid"] == null
                    ? 0M
                    : Convert.ToDecimal(ViewState["reqid"], CultureInfo.InvariantCulture);
        }
    }

    public string RequestID
    {
        get { return Request["reqid"]; }
    }

    public string DirectiveFormId
    {
        get
        {
            return !string.IsNullOrEmpty(Request["fid"])
                ? Request["fid"]
                : ViewState["fid"] == null
                    ? Guid.NewGuid().ToString()
                    : ViewState["fid"].ToString();
        }
    }

    public string FormID
    {
        get { return Request["fid"]; }
    } 
    public bool CanEdit { get; set; }

    /// <summary>
    /// 
    /// </summary>

    private void LoadData()
    {
        if (!string.IsNullOrEmpty(DirectiveFormId))
        {
            var type = GetType();
            if (type.IsDefined(typeof (FormAttribute), true))
            {
                var attrs = type.GetCustomAttributes(typeof (FormAttribute), true);
                if (attrs.Length == 0)
                {
                    throw new Exception("This form is does not ");
                }
                var form = ((FormAttribute) attrs[0]);
                CanEdit = form.AllowEdit;
                FormDataSource = db.ReadFormData(DirectiveFormId, form.ReadProcedureName);
            }
            else
            {
                CanEdit = true;
            }
        }
    }
    protected new void OnLoad(EventArgs e) 
    {
        if (!IsPostBack)
        {
            LoadData();
        }
    }

    protected void ChangeFieldStatus( bool enableDisable)
    {
        GetControls(this);
        if (!_controls.Any())
        {
            return;
        }

        foreach (var control in _controls)
        {
            var webControl = control as WebControl;
            if (webControl != null)
            {
                webControl.Enabled = enableDisable;
            }
        }
    }

    protected void ChangeAllFieldBackColor(Color fieldColor)
    {
        GetControls(this);
        if ( !_controls.Any() )
        {
            return;
        }

        foreach ( var control in _controls )
        {
            var webControl = control as WebControl;
            if ( webControl != null )
            {
                webControl.BackColor = fieldColor;
            }
        }
    }
    protected void LoadFormErrors(HtmlGenericControl errorContainer, Repeater dataListControl)
    {
        var errors = db.GetRequestErrorXml(Convert.ToDecimal(RequestID));
        var moreErrors = db.GetRequestErrorXmlExtra(Convert.ToDecimal(RequestID));
        if ( !string.IsNullOrEmpty(errors) )
        {
            var data = new RecordSet();
            var reader = new StringReader(errors);
            data.ReadXml(reader);
            reader.Close();

            if( !string.IsNullOrEmpty(moreErrors) )
            {
                var ds = new RecordSet();
                var r = new StringReader(moreErrors);
                ds.ReadXml(r);
                r.Close();

                if ( ds.HasRows )
                {
                    data.Tables[0].Merge(ds.Tables[0]);
                }
            }
            if ( data.HasRows )
            {
                errorContainer.Visible = true;
                dataListControl.DataSource = data.Tables[0];
                dataListControl.DataBind();
            }
            else
            {
                errorContainer.Visible = false;
            }
        }
        else
        {
            if ( !string.IsNullOrEmpty(moreErrors) )
            {
                var ds = new RecordSet();
                var r = new StringReader(moreErrors);
                ds.ReadXml(r);
                r.Close();

                if ( ds.HasRows )
                {

                    errorContainer.Visible = true;
                    dataListControl.DataSource = ds.Tables[0];
                    dataListControl.DataBind();
                }
                else
                {
                    errorContainer.Visible = false;
                }
            }
        }
    }

    protected bool MonitoryValuesValid()
    {
        var money = FindFields(typeof(MoneyField));
        if ( money.Any() )
        {
            var i =0;
            MoneyField toFucus = null;
            var x = money.Cast<MoneyField>().ToList();
            foreach ( var item in x )
            {
                if ( !item.IsEmpty )
                {
                    decimal __MONEY;
                    if(!decimal.TryParse(item.Text, out __MONEY) )
                    {
                        if ( toFucus == null )
                        {
                            toFucus = item;
                            item.BackColor = Color.Red;
                        }
                        i++;
                    }
                }
                //if ( !item.IsValidMoney() )
                //{
                //    if ( toFucus == null )
                //    {
                //        toFucus = item;
                //        item.BackColor = Color.Red;
                //    }
                //    i++;
                //}
            }
            if ( i > 0 )
            {
                toFucus.Focus();
                MessageBox.Show("Please make sure that all Rands and Cents fields are populated correctly.");
                return false;
            }
        }
        return true;
    }

}
[AttributeUsage(AttributeTargets.Class)]
public class FormAttribute : Attribute
{
    public string Name { get; set; }
    public string ReadProcedureName { get; set; }
    public FormAttribute(string formName, string readProcName)
    {
        Name = formName;
        ReadProcedureName = readProcName;
    }

    public bool AllowEdit { get; set; }

    
}
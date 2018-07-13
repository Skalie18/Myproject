using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sars.Systems.AdhocQueries;
using Sars.Systems.Controls;
using Sars.Systems.Data;
using Sars.Systems.Forms;

public partial class lookuptablemanager : System.Web.UI.Page
{
    protected override void OnPreInit(EventArgs e)
    {
        MasterPageFile = Sars.Systems.Data.SARSDataSettings.Settings.MasterPageFile;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var tables = SqlSupport.GetTables();
            ddlTables.Bind(tables.FindAll(t => t.Columns != null && t.Columns.Any() && t.Columns.Count < 7),
                "FullTableName", "Name");
        }
    }
    protected void ddlTables_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTables.SelectedIndex <= 0)
        {
            return;
        }
        var columns = SqlSupport.GetTablesColumns(ddlTables.SelectedValue).FindAll(
                c =>
                    c.DbFieldType != DbFieldType.Binary &&
                    c.DbFieldType != DbFieldType.VarBinary &&
                    c.DbFieldType != DbFieldType.Image);
        gvColumns.Bind(columns);
        ViewState["_columns"] = columns;

        var queryBuilder = string.Format("SELECT * FROM {0}", ddlTables.SelectedItem.Text);
        using (var data = new RecordSet(queryBuilder, QueryType.TransectSQL, null))
        {
            Session["_tabledata"] = data;
            gvData.Bind(data);
        }
    }
    protected void gvColumns_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var txtValue = (TextBox) e.Row.FindControl("txtValue");
            var column = (SqlColumn) e.Row.DataItem;
            if (column.IsComputed || column.IsIdentity || column.IsPrimaryKey)
            {
                txtValue.Enabled = false;
                return;
            }
            if (column.MaximumLength != null) txtValue.MaxLength = column.MaximumLength.Value;
        }
    }
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        if (ddlTables.SelectedIndex <= 0)
        {
            return;
        }
        var columns = SqlSupport.GetTablesColumns(ddlTables.SelectedValue);
        gvColumns.Bind(columns);
    }
    protected void gvData_SelectedIndexChanged(object sender, EventArgs e)
    {
        var row = gvData.SelectedRow;
        var values = new Dictionary<string, string>();
        var i = 0;
        var columns = (List<SqlColumn>) ViewState["_columns"];
        // var colIndex = 0;
        foreach (TableCell column in gvData.HeaderRow.Cells)
        {
            if (column.Visible)
            {
                //colIndex++;
                var columnName = column.Text;
                var col = columns.Find(c => c.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase));
                switch (col.DbFieldType)
                {
                    case DbFieldType.Bit:
                    {
                        var bitValues =
                            row.Controls[i].Controls.OfType<CheckBox>().Select(box => box.Checked).FirstOrDefault();
                        values.Add(columnName, bitValues.ToString());
                        break;
                    }
                    default:
                    {
                        var rowValue = row.Cells[i].Text;
                        values.Add(columnName, rowValue);
                        break;
                    }
                }
            }
            i++;
        }
        Session["_values"] = values;
        foreach (GridViewRow gvColumnsRow in gvColumns.Rows)
        {
            if (gvColumnsRow.RowType != DataControlRowType.DataRow)
            {
                continue;
            }
            foreach (TableCell cell in gvColumnsRow.Cells)
            {
                var lblName = gvColumnsRow.FindControl("lblName") as Label;
                if (lblName != null)
                {
                    var columnText = lblName.Text;
                    if (cell.Visible && values.ContainsKey(columnText))
                    {
                        var columnValue = values[columnText];
                        var txtDate = gvColumnsRow.FindControl("txtDate") as SarsCalendar;
                        var txtValue = gvColumnsRow.FindControl("txtValue") as TextBox;
                        var chkAnswer = gvColumnsRow.FindControl("chkAnswer") as CheckBox;
                        var column = columns.Find(c => c.Name.Equals(columnText, StringComparison.OrdinalIgnoreCase));
                        if (column != null)
                        {
                            switch (column.DbFieldType)
                            {
                                case DbFieldType.Int:
                                case DbFieldType.BigInt:
                                case DbFieldType.Decimal:
                                case DbFieldType.Float:
                                case DbFieldType.Money:
                                case DbFieldType.Numeric:
                                case DbFieldType.Real:
                                {
                                    if (txtValue != null && txtDate != null)
                                    {
                                        ShowHide(new List<Control> {txtDate, chkAnswer}, txtValue);
                                        txtValue.SetValue(HttpUtility.HtmlDecode(columnValue));
                                        txtValue.MaxLength = 10;
                                        txtValue.Width = new Unit(30, UnitType.Pixel);
                                    }
                                    break;
                                }
                                case DbFieldType.Char:
                                case DbFieldType.VarChar:
                                case DbFieldType.NChar:
                                case DbFieldType.NVarchar:
                                {
                                    if (txtValue != null && txtDate != null)
                                    {
                                        ShowHide(new List<Control> {txtDate, chkAnswer}, txtValue);
                                        txtValue.SetValue(HttpUtility.HtmlDecode(columnValue));
                                            if (txtValue.Text.Length > 50)
                                            {
                                                txtValue.Width = new Unit("500px");
                                            }
                                        }
                                    break;
                                }
                                case DbFieldType.Text:
                                case DbFieldType.NText:
                                {
                                    if (txtValue != null && txtDate != null)
                                    {
                                        ShowHide(new List<Control> {txtDate, chkAnswer}, txtValue);
                                        txtValue.SetValue(HttpUtility.HtmlDecode(columnValue));
                                        txtValue.TextMode = TextBoxMode.MultiLine;
                                        
                                    }
                                    break;
                                }
                                case DbFieldType.Date:
                                case DbFieldType.Datetime:
                                case DbFieldType.Datetime2:
                                case DbFieldType.SmallDateTime:
                                {
                                    if (txtValue != null && txtDate != null)
                                    {
                                        ShowHide(new List<Control> {txtValue, chkAnswer}, txtDate);
                                        txtDate.SetValue(HttpUtility.HtmlDecode(columnValue));
                                    }
                                    break;
                                }
                                case DbFieldType.Bit:
                                {
                                    if (chkAnswer != null)
                                    {
                                        ShowHide(new List<Control> {txtValue, txtDate}, chkAnswer);
                                        chkAnswer.Checked = Convert.ToBoolean(columnValue);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        ScriptManager.RegisterStartupScript(this, GetType(), "Show Modal Popup", "showmodalpopup();", true);
    }
    private static void ShowHide(IEnumerable<Control> controlsToHide, Control controlToShow)
    {
        foreach (var control in controlsToHide)
        {
            if (control != null)
            {
                control.Visible = false;
            }
        }
        controlToShow.Visible = true;
    }
    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var onclick = Page.ClientScript.GetPostBackEventReference((Control) sender, "Select$" + e.Row.RowIndex);
            e.Row.Attributes.Add("onclick", onclick);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        var columns = (List<SqlColumn>) ViewState["_columns"];
        SqlColumn column = null;
        var objactValues = new Dictionary<string, object>();
        object p_k_value = null;
        string p_k = null;
        foreach (GridViewRow row in gvColumns.Rows){
            if (row.RowType != DataControlRowType.DataRow){
                continue;
            }
            var lblName = row.FindControl("lblName") as Label;
            var txtValue = row.FindControl("txtValue") as TextBox;
            var txtDate = row.FindControl("txtDate") as SarsCalendar;
            var chkAnswer = row.FindControl("chkAnswer") as CheckBox;
            if (lblName == null || txtValue == null || txtDate == null || chkAnswer == null){
                return;
            }
            object value = null;
            if (txtValue.Visible){
                value = txtValue.Text;
            }
            if (txtDate.Visible){
                value = txtDate.Text;
            }
            if (chkAnswer.Visible){
                value = chkAnswer.Checked;
            }
            var foundColumn = columns.Find(c => c.Name.Equals(lblName.Text));
            if (foundColumn != null && foundColumn.IsPrimaryKey){
                column = foundColumn;
                p_k_value = value;
                p_k = lblName.Text;
            }
            //var column = columns.Find(c => c.Name.Equals(lblName.Text));
            if (foundColumn != null && !foundColumn.IsPrimaryKey){
                objactValues.Add(lblName.Text, value);
            }
        }
        if (column == null)
        {
            return;
        }
        var sql = string.Format("UPDATE {0}.{1} SET ", column.Schema, column.TableName);
        var oParams = new DBParamCollection();
        foreach (var updateValues in objactValues)
        {
            sql += string.Format("{0} = @{1}, ", updateValues.Key, updateValues.Key);
            oParams.Add(string.Format("@{0}", updateValues.Key), updateValues.Value);
        }
        oParams.Add(string.Format("@{0}", p_k), p_k_value);
        sql = sql.TrimEnd(", ".ToCharArray());

        sql += string.Format(" WHERE ({0} = @{0})", p_k);
        using (var oCommand = new DBCommand(sql, QueryType.TransectSQL, oParams ))
        {
            var saved = oCommand.Execute();
            if (saved > 0)
            {
                ddlTables_SelectedIndexChanged(ddlTables, EventArgs.Empty);
                //MessageBox.Show("Record Updated Successfully");
                ScriptManager.RegisterStartupScript(this, GetType(), "Show MessageBox", "showMessageBox();", true);
            }
        }
    }
    protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvData.NextPage(Session["_tabledata"], e.NewPageIndex);
    }
    protected void gvColumns_RowDataBound1(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var column = (SqlColumn) e.Row.DataItem;
            var txtValue = e.Row.FindControl("txtValue") as TextBox;
            var txtDate = e.Row.FindControl("txtDate") as SarsCalendar;
            if (txtValue == null)
            {
                return;
            }
            if (column.IsPrimaryKey || column.IsIdentity || column.IsPrimaryKey)
            {
                txtValue.Enabled = false;
            }
            switch (column.DbFieldType)
            {
                case DbFieldType.Int:
                case DbFieldType.BigInt:
                case DbFieldType.Decimal:
                case DbFieldType.Float:
                case DbFieldType.Money:
                case DbFieldType.Numeric:
                case DbFieldType.Real:
                {
                    txtValue.MaxLength = 10;
                    txtValue.Width = new Unit(30, UnitType.Pixel);
                    break;
                }
                case DbFieldType.Char:
                case DbFieldType.VarChar:
                case DbFieldType.NChar:
                case DbFieldType.NVarchar: 
                {
                    break;
                }
                case DbFieldType.Text:
                case DbFieldType.NText:
                    {
                        txtValue.TextMode = TextBoxMode.MultiLine;
                        break;
                    }
                case DbFieldType.Date:
                case DbFieldType.Datetime:
                case DbFieldType.Datetime2:
                case DbFieldType.SmallDateTime:
                {
                    break;
                }
            }
        }
    }
    protected void gvColumns_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void Cancel(object sender, EventArgs e)
    {
    }
}
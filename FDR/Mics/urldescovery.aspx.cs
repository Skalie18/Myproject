using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Linq;
using System.Web;
using Sars.Systems.Security;

public partial class UrlDescovery : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
            return;
        var path = Request.PhysicalApplicationPath;

        if (path != null)
        {
            var locations = GetFiles(new DirectoryInfo(path));
            if (locations.Count > 0)
            {
                gvUrls.Bind(locations);
            }
        }
    }

    private ICollection<ApplicationUrl> GetFiles(DirectoryInfo directory)
    {
        ICollection<ApplicationUrl> locations = new List<ApplicationUrl>();
        if (!directory.Exists) return locations;
        foreach (var file in directory.GetFiles("*.aspx", SearchOption.AllDirectories))
        {
            var fileName = Path.GetFileNameWithoutExtension(file.Name);
            {
                var fullName = file.FullName.Replace('\\', '/');
                if (Request.ApplicationPath != null)
                {
                    var start = fullName.LastIndexOf(Sars.Systems.Data.SARSDataSettings.Settings.ApplicationBaseFolder, StringComparison.Ordinal);
                    if (start != -1)
                    {
                        var url = fullName.Substring(start);
                        var page = new ApplicationUrl { PageName = fileName, PageUrl = string.Format("/{0}", url) };
                        page.PageUrl = page.PageUrl.Replace(Sars.Systems.Data.SARSDataSettings.Settings.ApplicationBaseFolder,
                            Sars.Systems.Data.SARSDataSettings.Settings.ApplicationName);
                        locations.Add(page);
                    }
                    else
                    {
                        var pageName = Path.GetFileName(fullName);
                        {
                            var url = string.Format("/{0}/{1}", Sars.Systems.Data.SARSDataSettings.Settings.ApplicationName, pageName);
                            var page = new ApplicationUrl { PageName = fileName, PageUrl = url };
                            locations.Add(page);
                        }
                    }
                }
            }
        }
        return locations;
    }

    protected void RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Attributes.Add("onclick",Page.ClientScript.GetPostBackEventReference((Control) sender,"Select$" + e.Row.RowIndex));
            e.Row.Attributes.Add("onmouseover", "style.cursor='pointer'");
            var chkMapped = e.Row.FindControl("chkMapped") as CheckBox;
            if (chkMapped != null)
            {
                var url = DataBinder.Eval(e.Row.DataItem, "PageUrl");
                var function = Functions.GetFunctionByUrl(url.ToString());
                chkMapped.Checked = !string.IsNullOrEmpty(function);
            }

            var ddlFunctions = e.Row.FindControl("ddlFunctions") as DropDownList;
            AddItems(ddlFunctions);
            //var hptMapFunction = e.Row.FindControl("hptMapFunction") as HyperLink;
            //if (hptMapFunction != null)
            //{
            //    hptMapFunction.NavigateUrl = string.Format("~/secureurl/MapUrlToFunction.aspx?mp=Site&mapurl={0}", DataBinder.Eval(e.Row.DataItem, "PageUrl"));
            //}
        }
    }

    private static void AddItems(ListControl ddlFunctions)
    {
        if (ddlFunctions == null) return;
        var activeFunctions = Functions.GetStronglyTypedFunctions();
        if (activeFunctions.Any())
        {
            activeFunctions.ForEach(delegate(Functions f)
            {
                var itm = new ListItem(f.Description, f.FunctionId.ToString());
                var group = f.Group;
                var groupName = group == null ? "NOGROUP" : group.Description.ToUpper();
                itm.Attributes["OptionGroup"] = groupName;
                ddlFunctions.Items.Add(itm);
            });
        }
    }

    protected override void OnPreInit(EventArgs e)
    {
        MasterPageFile = Sars.Systems.Data.SARSDataSettings.Settings.MasterPageFile;
        var name = typeof (DropDownList).FullName;
        if (HttpContext.Current.
            Request.Browser.Adapters[name] == null)
        {
            HttpContext.Current.
                Request.Browser.Adapters.Add(name,
                    typeof (Sars.Systems.Controls.Adapters.DropDownListAdapter).FullName);
        }
    }

    protected void FunctionSelectedIndexChanged(object sender, EventArgs e)
    {
        var ddlFunctions = sender as DropDownList;

        if (ddlFunctions != null)
        {
            var function = ddlFunctions.SelectedValue;
            var row = ddlFunctions.Parent.Parent as GridViewRow;
            if (row != null)
            {
                var url = row.Cells[1].Text.Replace("&nbsp;", "").Trim();
                if (string.IsNullOrEmpty(url))
                {
                    lblError.SetValue("URL/URI is required.");
                    return;
                }

                Uri uri;
                try
                {
                    uri = new Uri(url);
                }
                catch (Exception)
                {
                    uri = new Uri(Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port + url);
                }

                var localPath = uri.LocalPath;
                try
                {
                    Functions.MapUrl(function, localPath);

                }
                catch (SecureException ex)
                {
                    lblError.SetValue(ex.Message);
                }
            }
        }
    }
}
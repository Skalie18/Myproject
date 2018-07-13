using System;
using System.Web;
using System.Web.UI.WebControls;

namespace CraigBlog.Net
{


    public partial class TestDropDownList : System.Web.UI.Page {
        protected override void OnPreInit(EventArgs e)
        {
            HttpContext.Current.
            Request.Browser.Adapters.Add(typeof(DropDownList).FullName, typeof(Sars.Systems.Controls.Adapters.DropDownListAdapter).FullName);
        }
        protected void Page_Load(object sender, EventArgs e) {
            ListItem item1 = new ListItem("Camel", "1");
            item1.Attributes["OptionGroup"] = "Mammals";
            item1.Attributes["title"] = "Sibongile is a Camel";

            ListItem item2 = new ListItem("Lion", "2");
            item2.Attributes["OptionGroup"] = "Mammals";
            item2.Attributes["title"] = "Sibongile is a Lion";

            ListItem item3 = new ListItem("Whale", "3");
            item3.Attributes["OptionGroup"] = "Mammals";
            item3.Attributes["title"] = "Sibongile is a Whale";

            ListItem item4 = new ListItem("Walrus", "4");
            item4.Attributes["OptionGroup"] = "Mammals";

            ListItem item5 = new ListItem("Velociraptor", "5");
            item5.Attributes["OptionGroup"] = "Dinosaurs";

            ListItem item6 = new ListItem("Allosaurus", "6");
            item6.Attributes["OptionGroup"] = "Dinosaurs";

            ListItem item7 = new ListItem("Triceratops", "7");
            item7.Attributes["OptionGroup"] = "Dinosaurs";

            ListItem item8 = new ListItem("Stegosaurus", "8");
            item8.Attributes["OptionGroup"] = "Dinosaurs";

            ListItem item9 = new ListItem("Tyrannosaurus", "9");
            item9.Attributes["OptionGroup"] = "Dinosaurs";

            ddlItems.Items.Add(item1);
            ddlItems.Items.Add(item2);
            ddlItems.Items.Add(item3);
            ddlItems.Items.Add(item4);
            ddlItems.Items.Add(item5);
            ddlItems.Items.Add(item6);
            ddlItems.Items.Add(item7);
            ddlItems.Items.Add(item8);
            ddlItems.Items.Add(item9);
        }
    }
}

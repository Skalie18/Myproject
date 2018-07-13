using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Common;
using System.Xml.Serialization;
using System.Xml;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Web;
using Sars.Systems.Security;
using System.Configuration;


/// <summary>
/// Utility class for CMS progect
/// </summary>
public static class Utils
{
    public static string getshortString(string longNotes)
    {
        if (longNotes.Length > 70)
            return longNotes.Substring(0, 70) + "...";
        else
            return longNotes;
    }
    public static string GetLongString(string longNotes)
    {
        if (longNotes.Length > 110)
            return longNotes.Substring(0, 110) + "...";
        else
            return longNotes;
    }
    public static bool IsValidSID(string SID)
    {
        if (string.IsNullOrEmpty(SID))
        {
            return false;
        }
        if (SID.Length != 8)
        {
            return false;
        }
        if (SID[0].ToString().ToUpper() != "S")
        {
            return false;
        }
        if (!IsNumeric(SID.Substring(1, 3)))
        {
            return false;
        }
        return true;
    }
    public static bool IsNumeric(string value)
    {
        foreach (char c in value)
        {
            if (!char.IsDigit(c))
                return false;
        }
        return true;
    }
    public static bool IsNumeric(TextBox value)
    {
        return IsNumeric(value.Text);
    }
    public static bool IsValidCompanyRegNumber(string regNumber)
    {
        var segments = regNumber.Split("/".ToCharArray());
        if (segments.Length != 3)
            return false;
        if (segments.Any(segment => !IsNumeric(segment)))
        {
            return false;
        }
        if (segments[0].Length != 4)
            return false;
        if (segments[1].Length != 6)
            return false;
        if (segments[2].Length != 2)
            return false;
        return true;
    }
    public static bool IsMoney(string money)
    {
        var _money = 0D;
        var ismoney = Double.TryParse(money, System.Globalization.NumberStyles.Currency, null, out _money);
        return ismoney;
    }
    public static bool IsDecimal(string value)
    {
        var holder = 0M;
        return Decimal.TryParse(value, out holder);
    }

    public static bool IsTelephonePrefixZero(string value)
    {
        return value.StartsWith("0");
    }
    public static void SelectItemByText(RadioButtonList lst, string text)
    {
        var itm = lst.Items.FindByText(text);
        if (itm == null) return;
        var index = lst.Items.IndexOf(itm);
        lst.SelectedIndex = index;
    }
    public static void SelectItemByValue(RadioButtonList lst, string value)
    {
        var itm = lst.Items.FindByValue(value);
        if (itm != null)
        {
            int index = lst.Items.IndexOf(itm);
            lst.SelectedIndex = index;
        }
    }
    public static ListItem SelectItemByText(DropDownList lst, string text)
    {
        var itm = lst.Items.FindByText(text);
        if (itm != null)
        {
            var index = lst.Items.IndexOf(itm);
            lst.SelectedIndex = index;
            return itm;
        }
        return itm;
    }
    public static ListItem SelectItemByValue(DropDownList lst, string value)
    {
        if (null == lst)
            return null;
        var itm = lst.Items.FindByValue(value);
        if (itm != null)
        {
            var index = lst.Items.IndexOf(itm);
            lst.SelectedIndex = index;
            return itm;
        }
        return itm;
    }
    public static void Alert(string message, Page page)
    {
        var script = "<script type='text/javascript'>alert('" + message + "');</script>";
        page.ClientScript.RegisterClientScriptBlock(page.GetType(), "-alert-", script);
    }

    public static void HandleUnKnownErrors()
    {
        MessageBox.Show("UNKNOWN ERROR: Please try again.");
    }
    public static void SortGridView(GridView gridView, List<DbDataRecord> data, string sortExpression, SortDirection direction)
    {
        if (direction == SortDirection.Ascending)
        {
            direction = SortDirection.Descending;
            data.Sort(delegate (DbDataRecord r1, DbDataRecord r2)
            {
                return r1[sortExpression].ToString().CompareTo(r2[sortExpression].ToString());
            });
            gridView.DataSource = data;
            gridView.DataBind();
        }
        else
        {
            direction = SortDirection.Ascending;
            data.Sort(delegate (DbDataRecord r1, DbDataRecord r2)
            {
                return r1[sortExpression].ToString().CompareTo(r2[sortExpression].ToString());
            });
            data.Reverse();
            gridView.DataSource = data;
            gridView.DataBind();
        }
    }

    public static string Serialize<T>(this T value)
    {
        if (value == null)
        {
            return string.Empty;
        }
        try
        {
            var xmlserializer = new XmlSerializer(typeof(T));
            var stringWriter = new System.IO.StringWriter();
            using (var writer = XmlWriter.Create(stringWriter))
            {
                xmlserializer.Serialize(writer, value);
                return stringWriter.ToString();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred", ex);
        }
    }

    public static XmlDocument JsonToXML(string json)
    {
        XmlDocument doc = new XmlDocument();

        using (var reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(json), XmlDictionaryReaderQuotas.Max))
        {
            XElement xml = XElement.Load(reader);
            doc.LoadXml(xml.ToString());
        }

        return doc;
    }

    public static string BuildEmail(FDRPage.Statuses status, string country, string voidAction)
    {
        FDRPage fp = new FDRPage();
        string body = string.Empty;
        var url = fp.Url(status);
        var userList = ADUser.SearchAdUsersBySid(ADUser.CurrentSID);
        var user = userList[0];
        var appUrl = ConfigurationManager.AppSettings["fdrUrl"];
        string strStatus = "";
        using (StreamReader reader = new StreamReader(url))
        {
            body = reader.ReadToEnd();
        }

        if (voidAction == null)
        {
            strStatus = status.ToString();
        }
        else
        {
            strStatus = voidAction;
        }
        body = body.Replace("{country}", country);
        body = body.Replace("{status}", strStatus);
        body = body.Replace("{dateRejected}", DateTime.Now.ToString());
        body = body.Replace("{rejectedBy}", user.FullName);
        body = body.Replace("{url}", appUrl);


        return body;
    }

    public static XmlNode SelectNode(string message, string namespace_, string nodeName)
    {
        var doc = new XmlDocument();
        doc.LoadXml(message);
        var ns = new XmlNamespaceManager(doc.NameTable);
        ns.AddNamespace("", namespace_);
        var header = doc.SelectSingleNode(string.Format("//{0}", nodeName), ns);
        return header;
    }

    public static string GenerateUniqueNo()
    {
        Random rnd = new Random();
        string s = string.Empty;
        for (int i = 0; i < 9; i++)
            s = String.Concat(s, rnd.Next(10).ToString());
        return s;
    }


    //comapares xml nodes
    public static bool IsXmlNodesEqual(XmlNode newXml, XmlNode oldXml)
    {

        if (oldXml==null)
        {
            return false;
        }

        var node1 = newXml;
        var node2 = oldXml;
 
        var xmlDoc = Common.CreateNewDocument();
        xmlDoc.LoadXml(node1.OuterXml);
        XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
        nsmgr.AddNamespace("urn", "urn:oecd:ties:cbc:v1");
        nsmgr.AddNamespace("urn1", "urn:oecd:ties:cbc:v4");

        var docSpec = xmlDoc.ChildNodes[0].SelectSingleNode("DocSpec");
        if (docSpec == null)
        {
            docSpec = xmlDoc.ChildNodes[0].SelectSingleNode("urn:DocSpec", nsmgr);
            if (docSpec == null)
            {
                docSpec = xmlDoc.ChildNodes[0].SelectSingleNode("urn1:DocSpec", nsmgr);
            }
        }
        if (docSpec != null)
        {
            xmlDoc.ChildNodes[0].RemoveChild(docSpec);
            node1 = xmlDoc.DocumentElement;
        }

        xmlDoc = Common.CreateNewDocument();
        xmlDoc.LoadXml(node2.OuterXml);

        XmlNamespaceManager nsmgr1 = new XmlNamespaceManager(xmlDoc.NameTable);
        nsmgr1.AddNamespace("urn", "urn:oecd:ties:cbc:v1");
        nsmgr1.AddNamespace("urn1", "urn:oecd:ties:cbc:v4");

         docSpec = xmlDoc.ChildNodes[0].SelectSingleNode("DocSpec");
        if (docSpec == null)
        {
            docSpec = xmlDoc.ChildNodes[0].SelectSingleNode("urn:DocSpec", nsmgr);
            if (docSpec == null)
            {
                docSpec = xmlDoc.ChildNodes[0].SelectSingleNode("urn1:DocSpec", nsmgr);
            }
        }
        if (docSpec != null)
        {
            xmlDoc.ChildNodes[0].RemoveChild(docSpec);
            node2 = xmlDoc.DocumentElement;
        }

        XElement newElement = GetXElement(node1);
        XElement oldElement = GetXElement(node2);

       
        return XNode.DeepEquals(newElement, oldElement);
    }

    private static XElement GetXElement(this XmlNode node)
    {
        XDocument xDoc = new XDocument();
        using (XmlWriter xmlWriter = xDoc.CreateWriter())
            node.WriteTo(xmlWriter);
        return xDoc.Root;
    }

    public static XmlNode ToXmlNode(this XElement element)
    {
        using (XmlReader xmlReader = element.CreateReader())
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlReader);
            return xmlDoc;
        }
    }

    public static string RemoveOutgoingCBCNameSpace(string xmlDoc)
    {
        var cbcData = Utils.SelectNode(xmlDoc, "tnsf", "http://www.sars.gov.za/enterpriseMessagingModel/ThirdPartyData/SubmitCountryByCountryDeclaration/xml/schemas/version/1.3", "SubmitCountryByCountryDeclarationRequest");
        XDocument doc = XDocument.Parse(cbcData.InnerXml, LoadOptions.PreserveWhitespace);
        doc.Descendants().Attributes().Where(a => a.IsNamespaceDeclaration).Remove();
        if (cbcData != null)
        {

            foreach (var node in doc.Root.DescendantsAndSelf())
            {
                node.Attributes("xmlns").Remove();
                node.Attributes("ft").Remove();
                node.Attributes("bt").Remove();
                node.Attributes("sars").Remove();
                if (node.Name.Namespace != XNamespace.None)
                {
                    node.Name = XNamespace.None.GetName(node.Name.LocalName);
                }
                if (node.Attributes().Where(a => a.IsNamespaceDeclaration || a.Name.Namespace != XNamespace.None).Any())
                {
                    node.ReplaceAttributes(node.Attributes().Select(a => a.IsNamespaceDeclaration ? null : a.Name.Namespace != XNamespace.None ? new XAttribute(XNamespace.None.GetName(a.Name.LocalName), a.Value) : a));

                }
            }
            return doc.ToString();
        }
        return null;
    }

    public static string RemoveForeignCBCNameSpace(string xml)
    {
        var cbcData = XDocument.Parse(xml);
        if (cbcData != null)
        {

            foreach (var node in cbcData.Root.DescendantsAndSelf())
            {
                node.Attributes("xmlns").Remove();
                /* node.Attributes("ft").Remove();
                 node.Attributes("bt").Remove();*/
                if (node.Name.Namespace != XNamespace.None)
                {
                    node.Name = XNamespace.None.GetName(node.Name.LocalName);
                }
                if (node.Attributes().Where(a => a.IsNamespaceDeclaration || a.Name.Namespace != XNamespace.None).Any())
                {
                    node.ReplaceAttributes(node.Attributes().Select(a => a.IsNamespaceDeclaration ? null : a.Name.Namespace != XNamespace.None ? new XAttribute(XNamespace.None.GetName(a.Name.LocalName), a.Value) : a));

                }
            }
            return cbcData.ToString();
        }
        return null;
    }

    public static OutGoingCBCDeclarations GetOutgoingCBCR(string newPackage, string countryCode, int year,
       DateTime reportingPeriod, string userId, string nmPackage = null, decimal id = 0)
    {
        var newPackagedCBC = new OutGoingCBCDeclarations()
        {
            Id = id,
            Country = countryCode,
            CBCData = newPackage.ToString(),
            NSCBCData = nmPackage,
            StatusId = 2,
            Year = year,
            ActionId = 1,
            ReportingPeriod = reportingPeriod,
            CreatedBy = Sars.Systems.Security.ADUser.CurrentSID
        };
        return newPackagedCBC;
    }
    public static string RemoveAllNameSpaces(string xmlDoc)
    {
        XElement xml = RemoveAllNamespaces(XElement.Parse(xmlDoc));
        return xml.ToString();
    }

    private static XElement RemoveAllNamespaces(XElement xmlDoc)
    {
        return new XElement(
           xmlDoc.Name.LocalName,
           xmlDoc.HasElements ?
               xmlDoc.Elements().Select(el => RemoveAllNamespaces(el)) :
               (object)xmlDoc.Value
       );
    }

    public static XmlNode SelectNode(string message, string prifix, string namespace_, string nodeName)
    {
        var doc = new XmlDocument();
        doc.LoadXml(message);
        var ns = new XmlNamespaceManager(doc.NameTable);
        ns.AddNamespace(prifix, namespace_);
        var header = doc.SelectSingleNode(String.Format("//{0}:{1}", prifix, nodeName), ns);
        return header;
    }
}



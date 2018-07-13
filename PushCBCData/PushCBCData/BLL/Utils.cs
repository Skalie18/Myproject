using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace PushCBCData.BLL
{
    class Utils
    {
        public static string RemoveAllNameSpaces(string xmlDoc)
        {
            XElement xml = RemoveAllNamespaces(XElement.Parse(xmlDoc));
            return xml.ToString();
        }

        private static XElement RemoveAllNamespaces(XElement xmlDoc)
        {
            if (!xmlDoc.HasElements)
            {
                var xElement = new XElement(xmlDoc.Name.LocalName);
                xElement.Value = xmlDoc.Value;

                foreach (XAttribute attr in xmlDoc.Attributes())
                    xElement.Add(attr);

                return xElement;
            }

            return new XElement(xmlDoc.Name.LocalName, xmlDoc.Elements().Select(el => RemoveAllNamespaces(el)));
        }

        
    }
}

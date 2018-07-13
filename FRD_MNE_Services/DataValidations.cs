using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace FRD_MNE_Services
{
    public class InternalSchemaData
    {
        public string TargetNamespace { get; set; }
        public string SchemaContent { get; set; }
    }
    public class DataValidations
    {
        public DataValidations() { IsValid = false; }
        public DataValidations(string xml)
        {
            XmlToValidate = xml;
            IsValid = false;
        }

        public DataValidations(XmlNode xmlNode)
        {
            XmlToValidate = xmlNode.OuterXml;
            IsValid = false;
        }

        public List<InternalSchemaData> SchemaList { get; set; }

        public string XmlToValidate { get; set; }
        public string ErrorXml { get; set; }

        public bool IsValid { get; private set;}

        public void ValidateSchema()
        {
            if (SchemaList == null || !SchemaList.Any())
            {
                return;
            }

            var settings = new XmlReaderSettings();

            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;


            SchemaList.ForEach(
                s => settings.Schemas.Add(s.TargetNamespace, XmlReader.Create(new StringReader(s.SchemaContent))));

            settings.ValidationType = ValidationType.Schema;
            settings.ValidationEventHandler += settings_ValidationEventHandler;
            var reader = XmlReader.Create(new StringReader(XmlToValidate), settings);
            while (reader.Read())
            {
            }
            if (string.IsNullOrEmpty(ErrorXml))
            {
                IsValid = true;
            }
            var errorBuilder = new StringBuilder();
            if (_errors.Any())
            {
                errorBuilder.Append("<errors>");
                _errors.ForEach(e => errorBuilder.AppendFormat("<Error>{0}</Error>", e));
                errorBuilder.Append("</errors>");
                ErrorXml = FdrCommon.FormatXml(errorBuilder.ToString());
            }

            IsValid = !_errors.Any();
        }

        private readonly List<string> _errors = new List<string>();
        private void settings_ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Error)
            {
                IsValid = false;
                _errors.Add(e.Message);
            }
        }
    }
}

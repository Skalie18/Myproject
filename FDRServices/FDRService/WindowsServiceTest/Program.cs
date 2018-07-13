using CommonDatalayer;
using FDR.DataLayer;
using FDRService;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace WindowsServiceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Get List of Approved outgoing CBCR's
            var cbcr = DBReadManager.GetOutGoingCBCR(null, 0, 5);

            //Loop through each approved CBCR and generate XML report
            foreach (DataRow row in cbcr.Tables[0].Rows)
            {
                var outCBC = DatabaseReader.OutGoingCBCDeclarationsDetails(row["CountryCode"].ToString(), int.Parse(row["Period"].ToString()));

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(outCBC.CBCData);

                XmlNodeList CBCxml = xmlDoc.GetElementsByTagName("CBC_OECD");
                List<XmlNode> nodees = new List<XmlNode>(CBCxml.Cast<XmlNode>());

                string cbcXML = nodees[0].OuterXml;

                var user = Sars.Systems.Security.ADUser.SearchAdUsersBySid(outCBC.UpdatedBy);
                string email = "";

                if (user != null)
                    email = user[0].Mail;

                //Begin CTS-SenderFileMetadata-1.0
                CTSSenderFileMetadataType sender = new CTSSenderFileMetadataType();

                sender.BinaryEncodingSchemeCdSpecified = true;
                sender.FileCreateTsSpecified = true;
                sender.FileFormatCdSpecified = true;
                sender.FileRevisionIndSpecified = true;

                sender.BinaryEncodingSchemeCd = BinaryEncodingSchemeCdType.NONE;
                sender.CTSCommunicationTypeCd = CTSCommunicationTypeCdType.CBCStatus;
                sender.CTSReceiverCountryCd = (CountryCode_Type)System.Enum.Parse(typeof(CountryCode_Type), row["CountryCode"].ToString());
                sender.CTSSenderCountryCd = (CountryCode_Type)System.Enum.Parse(typeof(CountryCode_Type), "ZA");
                sender.SenderFileId = "";
                sender.FileFormatCd = FileFormatCdType.XML;
                sender.FileCreateTs = DateTime.Now;
                sender.TaxYear = row["Period"].ToString();
                sender.FileRevisionInd = false;
                sender.OriginalCTSTransmissionId = "";
                sender.SenderContactEmailAddressTxt = email;
                
                XmlSerializer xsSer = new XmlSerializer(typeof(CTSSenderFileMetadataType));

                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                string senderMetaDataXML;

                using (var sww = new StringWriter())
                {
                    using (XmlWriter writer = XmlWriter.Create(sww, settings))
                    {
                        xsSer.Serialize(writer, sender);

                        senderMetaDataXML = sww.ToString();
                    }
                }
                //End CTS-SenderFileMetadata-1.0

                //Begin SARSStatusManagementV1.15
                //StatusManagementRequest

                StatusManagementRequestStructure req = new StatusManagementRequestStructure();

                req.StatusDetails = new StatusStructure() { Description = "", Status = "" };
                req.DataType = "";
                req.TaxType = TypeOfTaxType.INCOME_TAX;
                req.TaxDateRange = new TaxDateRangeStructure() { FromTaxDate = DateTime.Now, ToTaxDate = DateTime.Now };
                req.TaxPeriodRange = new TaxPeriodRangeStructure() { FromTaxPeriod = "", ToTaxPeriod = "" };
                req.TaxYearRange = new TaxYearRangeStructure() { FromTaxYear = "", ToTaxYear = "" };
                req.SubReference = new SubReferenceStructure() { Type = SubReferenceStructureType.FORM_ID, Value = "" };
                req.Reference = new ReferenceStructure() { Identifier = ReferenceStructureIdentifier.TAX_REF_NO, Value = new[] { "", "", "" } };
                req.SourceChannel = "";
                req.RequestOperation = RequestOperationType.UPDATE_STATUS;

                XmlSerializer xsSerReq = new XmlSerializer(typeof(StatusManagementRequestStructure));
                string requestXML;

                using (var sww = new StringWriter())
                {
                    using (XmlWriter writer = XmlWriter.Create(sww, settings))
                    {
                        xsSerReq.Serialize(writer, req);

                        requestXML = sww.ToString();
                    }
                }

                //End SARSStatusManagementV1.15

                var str = string.Format("{0}{1}{2}", cbcXML, senderMetaDataXML, requestXML);

                using (ZipFile zip = new ZipFile())
                {
                    zip.AddEntry(row["CountryCode"].ToString() + ".xml", str);
                    zip.Save(@"D:\Sars\" + row["CountryCode"].ToString() + ".zip");//location and name for creating zip file
                }

                XmlSerializer xsSubmit = new XmlSerializer(typeof(CountryByCountryReportManagementRequest));
                var subReq = new CountryByCountryReportManagementRequest();

                subReq.RequestOperation = "SUBMIT_REPORT";
                subReq.DestinationCountry = row["CountryCode"].ToString();
                subReq.Filename = row["CountryCode"].ToString() + ".zip";
                subReq.FileContent = EncodeToBase64(@"D:\Sars\" + row["CountryCode"].ToString() + ".zip");

                var xml = "";

                using (var sww = new StringWriter())
                {
                    using (XmlWriter writer = XmlWriter.Create(sww))
                    {
                        xsSubmit.Serialize(writer, subReq);

                        xml = sww.ToString();

                        XmlDocument xdoc = new XmlDocument();
                        xdoc.LoadXml(xml);
                        xdoc.Save(string.Format("d:\\sars\\OutgoingCBCR_{0}.xml", row["CountryCode"].ToString()));

                        //DatabaseWriter.ApproveOutgoingCBC(row["CountryCode"].ToString(), int.Parse(row["Period"].ToString()), 6, "0");
                    }
                }
            }

        }

        public static string EncodeToBase64(string zipPath)
        {
            using (FileStream fs = new FileStream(zipPath, FileMode.Open, FileAccess.Read))
            {
                byte[] filebytes = new byte[fs.Length];
                fs.Read(filebytes, 0, Convert.ToInt32(fs.Length));
                return Convert.ToBase64String(filebytes);
            }
        }
    }
}

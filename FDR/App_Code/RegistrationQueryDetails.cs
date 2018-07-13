using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using FDR.DataLayer;
using Sars.Systems.Data;
using Sars.Systems.Frd.Registration;

/// <summary>
/// Summary description for RegistrationQueryDetails
/// </summary>
public class RegistrationQueryDetails
{
    public RegistrationQueryDetails()
    {
        RecordSet = new RecordSet();
        PostalAddresses= new List<PostalAddressStructure>();
        PhysicalAddresses = new List<PhysicalAddressStructure>();
    }

    public string RegistrationData { get; set; }
    public bool DetailsFound { get; set; }
    public string Message { get; set; }

    public RecordSet RecordSet { get; set; }

    public List<PostalAddressStructure> PostalAddresses { get; set; }
    public List<PhysicalAddressStructure> PhysicalAddresses { get; set; }

    public PhysicalAddressStructure PreferredAddress { get; set; }

    public string RegistrationName { get; set; }

    public void LookUpRegistrationDetails(string taxReferenceNo)
    {
        var serviceClient = new FDRQueueService();
        var id = serviceClient.GetTaxpayerRegistrationData(taxReferenceNo);
        var data = DBReadManager.GetResponse(id);
        var i = 1;
        while (!data.HasRows && i <= Configurations.QueueResponseTime){
            Thread.Sleep(1);
            i++;
            data = DBReadManager.GetResponse(id);
        }
        if (!data.HasRows && i >= Configurations.QueueResponseTime){
            DetailsFound = false;
            Message = Configurations.QueueTimeoutMessage;
            return;
        }

        if (Convert.ToInt32(data[0]["ReturnCode"]) != 0){
            DetailsFound = false;
            Message = data[0]["ReturnMessage"].ToString();
            //MessageBox.Show(data[0]["ReturnMessage"].ToString());
            return;
        }
        DetailsFound = true;
        var xml = data[0]["Message"].ToString();
        RegistrationData = xml;


        const string _namespace = "http://www.sars.gov.za/enterpriseMessagingModel/RegistrationManagement/xml/schemas/version/3.3";
        var registrationManagementResponse = FdrCommon.SelectNode(xml, "ns0", _namespace, "RegistrationManagementResponse");
        if (registrationManagementResponse != null)
        {
            var reg =
                Sars.Systems.Serialization.XmlObjectSerializer
                    .ConvertXmlToObject<RegistrationManagementResponseStructure>
                    (
                        registrationManagementResponse.OuterXml
                    );
            if (reg != null)
            {
                if (reg.Registration != null && reg.Registration.Parties != null && reg.Registration.Parties.Any())
                {
                    if (reg.Registration.Parties[0].Addresses != null && reg.Registration.Parties[0].Addresses.Any())
                    {
                        foreach (var addressStructure in reg.Registration.Parties[0].Addresses )
                        {
                            if (addressStructure.PhysicalAddress != null)
                            {
                                PhysicalAddresses.Add(addressStructure.PhysicalAddress);
                            }
                            if (addressStructure.PostalAddress != null)
                            {
                                PostalAddresses.Add(addressStructure.PostalAddress);
                            }
                        }
                    }

                    if (reg.Registration.Parties[0].TradingNameDetails != null &&reg.Registration.Parties[0].TradingNameDetails.Any())
                    {
                        foreach (var tradingNameDetailStructure in reg.Registration.Parties[0].TradingNameDetails )
                        {
                            if (!string.IsNullOrWhiteSpace(tradingNameDetailStructure.Name))
                            {
                                RegistrationName = tradingNameDetailStructure.Name;
                                break;
                            }
                        }
                    }
                }
            }
        }

        if ( PhysicalAddresses.Any())
        {
            foreach (var address in PhysicalAddresses)
            {
                if (
                    !string.IsNullOrEmpty(address.PostalCode)&& 
                    !string.IsNullOrEmpty(address.StreetName)&&
                    (!string.IsNullOrEmpty(address.Suburb)|| !string.IsNullOrEmpty(address.City))
                    )
                {
                    PreferredAddress = address;
                    break;
                }
            }
            if (PreferredAddress == null)
            {
                PreferredAddress =
                    PhysicalAddresses.Find(
                        a =>
                            !string.IsNullOrEmpty(a.StreetName) || !string.IsNullOrEmpty(a.ComplexName) ||
                            !string.IsNullOrEmpty(a.UnitNo));
            }
        }
        RecordSet.ReadXml(new StringReader(xml));
    }
}
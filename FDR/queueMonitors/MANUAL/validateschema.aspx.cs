using System;
using System.IO;
using Sars.Systems.Messaging;

public partial class queueMonitors_MANUAL_validateschema : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var file = @"D:\SARS\Validate\IE_SCHEMA_VALIDATION4.txt";

        var cbcXml = FdrCommon.FormatXml(File.ReadAllText(file));
        var _client = new ESBMessagingServiceClient("basic");

        var cbcSchemaVal = _client.ValidateSchema(Configurations.CbCValidationServiceID, cbcXml);

        _client.Close();
    }
}
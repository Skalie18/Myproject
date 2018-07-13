using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using FDR.DataLayer;
using Sars.Systems.Data;
using Sars.Systems.Utilities;

/// <summary>
/// Summary description for Notifications
/// </summary>
public static class Notifications
{
    public static void SendNotification(string reportingPeriod, string countryFrom, RecordSet rolesToNotify)
    {
       
        if (rolesToNotify.HasRows)
        {
            var x = (from s in rolesToNotify.Tables[0].Rows.OfType<DataRow>()
                     where
                        s["EmailAddress"] != null &&
                        s["EmailAddress"].ToString().IsValid(StringValidationType.Email)
                     select s["EmailAddress"].ToString()).ToArray();

            var messageBody = string.Format(
                File.ReadAllText(Path.Combine(Configurations.EmailTemplateBaseFolder, "CBCArrivalNotification.htm"))
                , countryFrom
                , reportingPeriod
                , SARSDataSettings.Settings.ApplicationName);
            FdrCommon.SendEmail(x, messageBody, "CBC Declaration");
        }
    }
}
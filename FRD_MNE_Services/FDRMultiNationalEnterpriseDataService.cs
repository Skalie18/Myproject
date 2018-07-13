using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;

namespace FRD_MNE_Services
{
    public partial class FDRMultiNationalEnterpriseDataService : ServiceBase
    {
        readonly MneRequestProvider _mneRequestProvider = new MneRequestProvider();
        readonly CbcDeclarationReceiver _cbcDeclaration = new CbcDeclarationReceiver();
        readonly MasterLocalFileProvider _fileProvider =new MasterLocalFileProvider();
        public FDRMultiNationalEnterpriseDataService()
        {
            InitializeComponent();
            //_mneRequestProvider.ListenForMneRequests();
            //_cbcDeclaration.ListenForCbcRequests();
            //_fileProvider.ListenMasterLocalFilesRequests();
            //var message = File.ReadAllText(@"\\ptadviis06\TaxDirectives\FDR\CBC\CBC.XML");
            //IsCBCValid(message);
            //DataValidations.IsCbcValid(submitCountryByCountryDeclarationRequest.OuterXml);
        }
        protected override void OnStart(string[] args)
        {
            try
            {
                FdrCommon.LogEvent("FDR Service started");
                var oMneProviderDelegate = new MNEProviderDelegate(_mneRequestProvider.ListenForMneRequests);
                var mneResults = oMneProviderDelegate.BeginInvoke(null, null);
                oMneProviderDelegate.EndInvoke(mneResults);

                var oCbcProviderDelegate = new CBCProviderDelegate(_cbcDeclaration.ListenForCbcRequests);
                var cbcResults = oCbcProviderDelegate.BeginInvoke(null, null);
                oCbcProviderDelegate.EndInvoke(cbcResults);


                var oFileProviderDelegate = new MasterLocalFileProviderDelegate(_fileProvider.ListenMasterLocalFilesRequests);
                var fileResults = oFileProviderDelegate.BeginInvoke(null, null);
                oFileProviderDelegate.EndInvoke(fileResults);

                FdrCommon.LogEvent("NO ISSUES REPORTED");
            }
            catch (Exception esException)
            {
                FdrCommon.LogEvent(esException, EventLogEntryType.Error);
            }
        }
        protected override void OnStop()
        {
            FdrCommon.LogEvent("FDR Service gracefully shutting down");
            _mneRequestProvider.TerminateConnections();
            _cbcDeclaration.TerminateConnections();
            _fileProvider.TerminateConnections();
        }
    }
}

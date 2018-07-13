namespace FDRWinService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.oFDRProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.oFDRServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // oFDRProcessInstaller
            // 
            this.oFDRProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.oFDRProcessInstaller.Password = null;
            this.oFDRProcessInstaller.Username = null;
            // 
            // oFDRServiceInstaller
            // 
            this.oFDRServiceInstaller.Description = "This service does all SARS FDR business brocessing";
            this.oFDRServiceInstaller.DisplayName = "SARS FDR Service";
            this.oFDRServiceInstaller.ServiceName = "FinancialDataReportingService";
            this.oFDRServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.oFDRProcessInstaller,
            this.oFDRServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller oFDRProcessInstaller;
        private System.ServiceProcess.ServiceInstaller oFDRServiceInstaller;
    }
}
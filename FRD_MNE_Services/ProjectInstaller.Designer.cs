namespace FRD_MNE_Services
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
            if (disposing && (components != null))
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
            this.FDRMultiNationalEnterpriseDataServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.FDRMultiNationalEnterpriseDataServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // FDRMultiNationalEnterpriseDataServiceProcessInstaller
            // 
            this.FDRMultiNationalEnterpriseDataServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.FDRMultiNationalEnterpriseDataServiceProcessInstaller.Password = null;
            this.FDRMultiNationalEnterpriseDataServiceProcessInstaller.Username = null;
            // 
            // FDRMultiNationalEnterpriseDataServiceInstaller
            // 
            this.FDRMultiNationalEnterpriseDataServiceInstaller.Description = "This service receives and send queues regarding FDR/MNE/CBC systems";
            this.FDRMultiNationalEnterpriseDataServiceInstaller.DisplayName = "SARS FDR MNE Provider";
            this.FDRMultiNationalEnterpriseDataServiceInstaller.ServiceName = "FDRMultiNationalEnterpriseDataService";
            this.FDRMultiNationalEnterpriseDataServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.FDRMultiNationalEnterpriseDataServiceProcessInstaller,
            this.FDRMultiNationalEnterpriseDataServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller FDRMultiNationalEnterpriseDataServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller FDRMultiNationalEnterpriseDataServiceInstaller;
    }
}
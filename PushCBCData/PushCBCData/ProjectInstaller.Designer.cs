namespace PushCBCData
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
            this.spiPushCBCData = new System.ServiceProcess.ServiceProcessInstaller();
            this.siPushCBCData = new System.ServiceProcess.ServiceInstaller();
            // 
            // spiPushCBCData
            // 
            this.spiPushCBCData.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.spiPushCBCData.Password = null;
            this.spiPushCBCData.Username = null;
            // 
            // siPushCBCData
            // 
            this.siPushCBCData.DisplayName = "PushCBCData";
            this.siPushCBCData.ServiceName = "PushService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.spiPushCBCData,
            this.siPushCBCData});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller spiPushCBCData;
        private System.ServiceProcess.ServiceInstaller siPushCBCData;
    }
}
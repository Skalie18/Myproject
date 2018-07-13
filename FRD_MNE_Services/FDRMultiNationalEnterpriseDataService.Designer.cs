using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FRD_MNE_Services
{
    partial class FDRMultiNationalEnterpriseDataService
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

        /// <summClass1.csary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.ServiceName = "FDRMultiNationalEnterpriseDataService";
        }


   

  
        #endregion

        private delegate void MNEProviderDelegate();
        private delegate void CBCProviderDelegate();
        private delegate void MasterLocalFileProviderDelegate();
    }
}

namespace cPlayer
{
    partial class settings
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(settings));
            this.settingsBrowser = new System.Windows.Forms.WebBrowser();
            this.getBrowserVariables = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // settingsBrowser
            // 
            this.settingsBrowser.AllowWebBrowserDrop = false;
            this.settingsBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingsBrowser.IsWebBrowserContextMenuEnabled = false;
            this.settingsBrowser.Location = new System.Drawing.Point(0, 0);
            this.settingsBrowser.Margin = new System.Windows.Forms.Padding(0);
            this.settingsBrowser.MinimumSize = new System.Drawing.Size(800, 500);
            this.settingsBrowser.Name = "settingsBrowser";
            this.settingsBrowser.ScrollBarsEnabled = false;
            this.settingsBrowser.Size = new System.Drawing.Size(800, 500);
            this.settingsBrowser.TabIndex = 0;
            this.settingsBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.settingsBrowser_DocumentCompleted);
            // 
            // getBrowserVariables
            // 
            this.getBrowserVariables.Tick += new System.EventHandler(this.getBrowserVariables_Tick);
            // 
            // settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.settingsBrowser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "settings";
            this.Text = "Player Settings";
            this.Load += new System.EventHandler(this.settings_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser settingsBrowser;
        private System.Windows.Forms.Timer getBrowserVariables;
    }
}
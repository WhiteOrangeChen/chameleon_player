namespace cPlayer
{
    partial class player
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(player));
            this.playerBrowser = new System.Windows.Forms.WebBrowser();
            this.getBrowserVariables = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // playerBrowser
            // 
            this.playerBrowser.AllowWebBrowserDrop = false;
            this.playerBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playerBrowser.IsWebBrowserContextMenuEnabled = false;
            this.playerBrowser.Location = new System.Drawing.Point(0, 0);
            this.playerBrowser.Margin = new System.Windows.Forms.Padding(0);
            this.playerBrowser.MinimumSize = new System.Drawing.Size(344, 344);
            this.playerBrowser.Name = "playerBrowser";
            this.playerBrowser.ScrollBarsEnabled = false;
            this.playerBrowser.Size = new System.Drawing.Size(344, 344);
            this.playerBrowser.TabIndex = 0;
            this.playerBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.playerWebBrowser_DocumentCompleted);
            this.playerBrowser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.playerBrowser_Navigating);
            // 
            // getBrowserVariables
            // 
            this.getBrowserVariables.Tick += new System.EventHandler(this.getBrowserVariables_Tick);
            // 
            // player
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 344);
            this.Controls.Add(this.playerBrowser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "player";
            this.Text = "Chameleon Player";
            this.Load += new System.EventHandler(this.player_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser playerBrowser;
        private System.Windows.Forms.Timer getBrowserVariables;
    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace cPlayer
{
    public partial class settings : Form
    {
        private Uri settingsUrl = new Uri(Application.StartupPath.ToString() + "/core/settings.htm");
        private bool isDraging = false;
        private int xFramePosition = 0;
        private int yFramePosition = 0;
        private int xPosition = 0;
        private int yPosition = 0;
        private bool isBrowserDocumentMouseMoveFunctionAssigned = false;

        private int cnfX = 0;
        private int cnfY = 0;

        public int borderColorR = 210;
        public int borderColorG = 72;
        public int borderColorB = 38;

        public string trackName = "";

        public string sName = "";
        public string sArtist = "";
        public string sAlbum = "";
        public string sAlbumArtist = "";
        public string sGenre = "";
        public string sYear = "";

        public bool saveChanges = false;
        public string base64Cover = "";
        public string newBase64Cover = "";
        public bool isDefaultCover = false;
        

        public settings(string tn, int x, int y, int r = 210, int g = 72, int b = 38)
        {
            cnfX = x;
            cnfY = y;

            borderColorR = r;
            borderColorG = g;
            borderColorB = b;

            trackName = tn;

            InitializeComponent();
        }

        private void settings_Load(object sender, EventArgs e)
        {
            this.Top = -3600;
            this.Left = -3600;

            this.settingsBrowser.Navigate(settingsUrl, "", null, "User-Agent:Player");
        }

        private void settingsBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.getBrowserVariables.Enabled = true;

            if (!isBrowserDocumentMouseMoveFunctionAssigned)
            {
                this.settingsBrowser.Document.MouseDown += playerBrowser_MouseDown;
                this.settingsBrowser.Document.MouseUp += playerBrowser_MouseUp;
                this.settingsBrowser.Document.MouseMove += playerBrowser_MouseMove;
                this.settingsBrowser.AllowWebBrowserDrop = true;
            }
            isBrowserDocumentMouseMoveFunctionAssigned = true;

        }

        private void getBrowserVariables_Tick(object sender, EventArgs e)
        {
            ///////////////////////////////////////
            // - Check Browser Frame Controls
            if (this.bEval("cFrameControlClosed") == "1")
            {
                this.bEval("cFrameControlClosed=0");
                this.Close();
                return;
            }
            if (this.bEval("cFrameControlSaveChanges") == "1")
            {
                sName = bEval("$('.sName').val()");
                sArtist = bEval("$('.sArtist').val()");
                sAlbum = bEval("$('.sAlbum').val()");
                sAlbumArtist = bEval("$('.sAlbumArtist').val()");
                sGenre = bEval("$('.sGenre').val()");
                sYear = bEval("$('.sYear').val()");

                saveChanges = true;
                this.bEval("cFrameControlSaveChanges=0");
                this.Close();
                return;
            }
            if (this.bEval("cFrameControlSetDefaultCover") == "1")
            {
                this.bEval("cFrameControlSetDefaultCover=0");

                Bitmap coverBmp = new Bitmap(cPlayer.Properties.Resources.defaultCover);
                ImageConverter converter = new ImageConverter();
                this.newBase64Cover = Convert.ToBase64String((byte[])converter.ConvertTo(coverBmp, typeof(byte[])));

                bEval("setDefaultCover('" + newBase64Cover + "','210','72','38');");

                this.isDefaultCover = true;

                borderColorR = 210;
                borderColorG = 72;
                borderColorB = 38;
            }
            if (this.bEval("cFrameControlChooseNewCover") == "1")
            {
                this.bEval("cFrameControlChooseNewCover=0");
                this.bEval("startCoverLoader();");

                OpenFileDialog browseFile = new OpenFileDialog();
                browseFile.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
                browseFile.Title = "Browse Image File";
                if(browseFile.ShowDialog() != DialogResult.Cancel)
                {
                    string filePath=browseFile.FileName;

                    Bitmap coverBmp = new Bitmap(filePath);
                    ImageConverter converter = new ImageConverter();
                    this.newBase64Cover = Convert.ToBase64String((byte[])converter.ConvertTo(coverBmp, typeof(byte[])));

                    int r = 210;
                    int g = 72;
                    int b = 38;

                    MemoryStream bmpStr = new MemoryStream((byte[])converter.ConvertTo(coverBmp, typeof(byte[])));
                    Bitmap bmp = new Bitmap(bmpStr);
                    int total = 0;
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        for (int y = 0; y < bmp.Height; y++)
                        {
                            Color clr = bmp.GetPixel(x, y);

                            r += clr.R;
                            g += clr.G;
                            b += clr.B;

                            total++;
                        }
                    }

                    r /= total;
                    g /= total;
                    b /= total;

                    borderColorR = r;
                    borderColorG = g;
                    borderColorB = b;

                    bEval("chooseNewCover('" + newBase64Cover + "','"+r+"','"+g+"','"+b+"');");
                }
                this.bEval("endCoverLoader();");
            }
            if (this.bEval("jQueryLoadStatus") == "loaded")
            {
                this.bEval("jQueryLoadStatus='used'");

                this.Top = cnfY;
                this.Left = cnfX;

                bEval("initSettingsFrame('" 
                    + Convert.ToBase64String(player.ToByteArray(trackName)) + "','" 
                    + base64Cover + "','" 
                    + borderColorR + "','"
                    + borderColorG + "','"
                    + borderColorB + "','"
                    + Convert.ToBase64String(player.ToByteArray(sName)) + "','"
                    + Convert.ToBase64String(player.ToByteArray(sArtist)) + "','"
                    + Convert.ToBase64String(player.ToByteArray(sAlbum)) + "','"
                    + Convert.ToBase64String(player.ToByteArray(sAlbumArtist)) + "','"
                    + Convert.ToBase64String(player.ToByteArray(sGenre)) + "','"
                    + Convert.ToBase64String(player.ToByteArray(sYear)) + "');");
            }
            // - End
            ///////////////////////////////////////
        }




        //************************************************//
        //------------------ Drag Code ------------------//
        //************************************************//


        private void playerBrowser_MouseDown(object sender, HtmlElementEventArgs e)
        {
            if (bEval("notDraggableZone") != "1")
            {
                isDraging = true;
                xFramePosition = this.Left;
                yFramePosition = this.Top;
                xPosition = e.MousePosition.X;
                yPosition = e.MousePosition.Y;
            }
        }

        private void playerBrowser_MouseUp(object sender, HtmlElementEventArgs e)
        {
            xFramePosition = this.Left;
            yFramePosition = this.Top;
            xPosition = 0;
            yPosition = 0;
            isDraging = false;
        }

        private void playerBrowser_MouseMove(object sender, HtmlElementEventArgs e)
        {
            if (isDraging)
            {
                if (this.Left + (e.MousePosition.X - xPosition) < -this.Width + 4)
                    this.Left = -this.Width + 4;
                else if ((this.Left + (e.MousePosition.X - xPosition)) + (this.Width) > Screen.PrimaryScreen.Bounds.Width + this.Width - 4)
                    this.Left = Screen.PrimaryScreen.Bounds.Width - this.Width + this.Width - 4;
                else
                    this.Left += (e.MousePosition.X - xPosition);

                if (this.Top + (e.MousePosition.Y - yPosition) < -this.Height + 4)
                    this.Top = -this.Width + 4;
                else if ((this.Top + (e.MousePosition.Y - yPosition)) + (this.Height) > Screen.PrimaryScreen.Bounds.Height + this.Height - 4 - 40)
                    this.Top = Screen.PrimaryScreen.Bounds.Height - this.Height + this.Height - 4 - 40;
                else
                    this.Top += (e.MousePosition.Y - yPosition);
            }
        }

        public string bEval(string _s)
        {
            if (this.settingsBrowser.Document.InvokeScript("eval", new object[] { _s }) == null) return "";
            return this.settingsBrowser.Document.InvokeScript("eval", new object[] { _s }).ToString();
        }
    }
}

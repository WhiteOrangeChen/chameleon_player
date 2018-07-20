using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using Microsoft.Win32;

namespace cPlayer
{
    public partial class player : Form
    {
        private Uri playerUrl=new Uri(Application.StartupPath.ToString() + "/core/index.htm");
        private string filePath = "";
        private bool isDraging = false;
        private int xFramePosition = 0;
        private int yFramePosition = 0;
        private int xPosition = 0;
        private int yPosition = 0;
        private bool isBrowserDocumentMouseMoveFunctionAssigned = false;

        private int cnfX = 0;
        private int cnfY = 0;
        private int cnfSettingsX = 0;
        private int cnfSettingsY = 0;
        private bool cnfReplay = false;
        private int cnfVolume = 1;

        private int borderColorR = 210;
        private int borderColorG = 72;
        private int borderColorB = 38;

        private string currentBase64Cover="";


        //************************************************//
        //-------------- Constructor Code ---------------//
        //************************************************//

        public player(string filePath)
        {
            this.filePath = filePath.Replace(@"\", @"/");

            try
            {
                // Check and set file associations
                checkAppAssociations();
                //removeAppAssociations();

                // Check if new_path is not empty
                if (!File.Exists(Application.StartupPath.ToString() + "/config.ini")) initConfigFile();

                string configText = File.ReadAllText(Application.StartupPath.ToString() + "/config.ini");

                string newPatch = "";
                string[] configArgs = configText.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string val in configArgs)
                {
                    if (val.Split(new string[] { "=" }, StringSplitOptions.None)[0] == "new_path") newPatch = val.Split(new string[] { "=" }, StringSplitOptions.None)[1].ToString();
                }

                if (newPatch != "") configText = configText.Replace(newPatch, "");

                File.WriteAllText(Application.StartupPath.ToString() + "/config.ini", configText);
            }
            catch (Exception e) { }

            InitializeComponent();
        }




        //************************************************//
        //------------ App Associations Code ------------//
        //***********************************************//

        public void checkAppAssociations()
        {
            if (Registry.GetValue("HKEY_CLASSES_ROOT\\ChameleonPlayer", String.Empty, String.Empty) == null)
            {
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\Classes\\ChameleonPlayer", "", "Chameleon Player");

                Registry.SetValue("HKEY_CURRENT_USER\\Software\\Classes\\.mp3", "", "ChameleonPlayer.mp3");
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\Classes\\ChameleonPlayer.mp3", "", "mp3");
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\Classes\\ChameleonPlayer.mp3\\DefaultIcon", "", Application.StartupPath.ToString() + "\\core\\extIcons\\mp3.ico");
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\Classes\\ChameleonPlayer.mp3\\Shell\\Open\\Command", "", Application.StartupPath.ToString() + "\\Chameleon Player.exe \"%1\"");

                Registry.SetValue("HKEY_CURRENT_USER\\Software\\Classes\\.m4a", "", "ChameleonPlayer.m4a");
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\Classes\\ChameleonPlayer.m4a", "", "m4a");
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\Classes\\ChameleonPlayer.m4a\\DefaultIcon", "", Application.StartupPath.ToString() + "\\core\\extIcons\\m4a.ico");
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\Classes\\ChameleonPlayer.m4a\\Shell\\Open\\Command", "", Application.StartupPath.ToString() + "\\Chameleon Player.exe \"%1\"");

                Registry.SetValue("HKEY_CURRENT_USER\\Software\\Classes\\.ogg", "", "ChameleonPlayer.ogg");
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\Classes\\ChameleonPlayer.ogg", "", "ogg");
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\Classes\\ChameleonPlayer.ogg\\DefaultIcon", "", Application.StartupPath.ToString() + "\\core\\extIcons\\ogg.ico");
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\Classes\\ChameleonPlayer.ogg\\Shell\\Open\\Command", "", Application.StartupPath.ToString() + "\\Chameleon Player.exe \"%1\"");

                //this call notifies Windows that it needs to redo the file associations and icons
                SHChangeNotify(0x08000000, 0x2000, IntPtr.Zero, IntPtr.Zero);
            }
        }
        public void removeAppAssociations()
        {
            Registry.SetValue("HKEY_CURRENT_USER\\Software\\Classes\\.mp3", "", "");
            Registry.SetValue("HKEY_CURRENT_USER\\Software\\Classes\\.m4a", "", "");
            Registry.SetValue("HKEY_CURRENT_USER\\Software\\Classes\\.ogg", "", "");

            Registry.CurrentUser.DeleteSubKeyTree("Software\\Classes\\ChameleonPlayer");
            Registry.CurrentUser.DeleteSubKeyTree("Software\\Classes\\ChameleonPlayer.m4a");
            Registry.CurrentUser.DeleteSubKeyTree("Software\\Classes\\ChameleonPlayer.mp3");
            Registry.CurrentUser.DeleteSubKeyTree("Software\\Classes\\ChameleonPlayer.ogg");

            //this call notifies Windows that it needs to redo the file associations and icons
            SHChangeNotify(0x08000000, 0x2000, IntPtr.Zero, IntPtr.Zero);
        }




        //************************************************//
        //----------------- Config Code -----------------//
        //************************************************//

        public void initConfigFile()
        {
            File.Create(Application.StartupPath.ToString() + "/config.ini").Dispose();

            var text = "";
            text += "start_position_x=" + "20" + Environment.NewLine;
            text += "start_position_y=" + "20" + Environment.NewLine;
            text += "start_settings_position_x=" + "20" + Environment.NewLine;
            text += "start_settings_position_y=" + "20" + Environment.NewLine;
            text += "replay=" + "0" + Environment.NewLine;
            text += "volume=" + "1" + Environment.NewLine;
            text += "new_path=";

            File.WriteAllText(Application.StartupPath.ToString() + "/config.ini", text);
        }
        public void getConfig()
        {
            try
            {
                if (!File.Exists(Application.StartupPath.ToString() + "/config.ini")) initConfigFile();

                string configText = File.ReadAllText(Application.StartupPath.ToString() + "/config.ini");

                string[] configArgs = configText.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string val in configArgs)
                {
                    if (val.Split(new string[] { "=" }, StringSplitOptions.None)[0] == "start_position_x") cnfX = Convert.ToInt32(val.Split(new string[] { "=" }, StringSplitOptions.None)[1]);
                    if (val.Split(new string[] { "=" }, StringSplitOptions.None)[0] == "start_position_y") cnfY = Convert.ToInt32(val.Split(new string[] { "=" }, StringSplitOptions.None)[1]);
                    if (val.Split(new string[] { "=" }, StringSplitOptions.None)[0] == "start_settings_position_x") cnfSettingsX = Convert.ToInt32(val.Split(new string[] { "=" }, StringSplitOptions.None)[1]);
                    if (val.Split(new string[] { "=" }, StringSplitOptions.None)[0] == "start_settings_position_y") cnfSettingsY = Convert.ToInt32(val.Split(new string[] { "=" }, StringSplitOptions.None)[1]);
                    if (val.Split(new string[] { "=" }, StringSplitOptions.None)[0] == "replay") cnfReplay = (val.Split(new string[] { "=" }, StringSplitOptions.None)[1].ToString() == "1" ? true : false);
                    if (val.Split(new string[] { "=" }, StringSplitOptions.None)[0] == "volume") cnfVolume = Convert.ToInt32(val.Split(new string[] { "=" }, StringSplitOptions.None)[1]);
                }

                if (cnfX != 0 && cnfY != 0)
                {
                    this.Top = cnfY;
                    this.Left = cnfX;
                }
            }
            catch (Exception e) { }
        }
        public void setConfig()
        {
            cnfX = this.Left;
            cnfY = this.Top;

            if (cnfX < -300) cnfX = 20;
            if (cnfY < -300) cnfY = 20;
            if (cnfX > Screen.PrimaryScreen.Bounds.Width + 300) cnfX = 20;
            if (cnfY > Screen.PrimaryScreen.Bounds.Height + 300) cnfY = 20;

            var text = "";
            text += "start_position_x=" + cnfX + Environment.NewLine;
            text += "start_position_y=" + cnfY + Environment.NewLine;
            text += "start_settings_position_x=" + cnfSettingsX + Environment.NewLine;
            text += "start_settings_position_y=" + cnfSettingsY + Environment.NewLine;
            text += "replay=" + (cnfReplay ? "1" : "0") + Environment.NewLine;
            text += "volume=" + cnfVolume + Environment.NewLine;
            text += "new_path=";

            try
            {
                File.WriteAllText(Application.StartupPath.ToString() + "/config.ini", text);
            }
            catch (Exception e) { }
        }




        //************************************************//
        //---------------- Set Media Code ---------------//
        //************************************************//

        public bool setMedia(string filePath)
        {
            this.filePath = filePath;

            if (filePath != string.Empty && filePath != "")
            {
                // Work With Music ID3 Tag
                TagLib.File tagFile = TagLib.File.Create(filePath);


                string album = tagFile.Tag.Album!=null? tagFile.Tag.Album.ToString():"";
                string[] performers = tagFile.Tag.Performers;
                string title = tagFile.Tag.Title!=null? tagFile.Tag.Title.ToString():"";

                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
                string performerByFileName = "";
                string titleByFileName = "";

                if (fileNameWithoutExtension.IndexOf("-") != -1)
                {
                    if (fileNameWithoutExtension.Split(new string[] { "-" }, StringSplitOptions.None).Length == 2)
                    {
                        performerByFileName = fileNameWithoutExtension.Split(new string[] { "-" }, StringSplitOptions.None)[0].Trim();
                        titleByFileName = fileNameWithoutExtension.Split(new string[] { "-" }, StringSplitOptions.None)[1].Trim();
                    }
                }

                if (performerByFileName != "" && titleByFileName != "")
                {
                    if (performers.Length == 0)
                    {
                        tagFile.Tag.Performers = new string[] { performerByFileName };
                    }
                    if (title == null)
                    {
                        tagFile.Tag.Title = titleByFileName;
                    }

                    tagFile.Save();
                }

                performers = tagFile.Tag.Performers;
                title = tagFile.Tag.Title != null ? tagFile.Tag.Title.ToString() : "";
                string performer = performerByFileName;
                if (performers.Length > 0) performer = performers[0].ToString();


                // Generate Cover And Average Color Of Borders
                TagLib.IPicture[] cover = tagFile.Tag.Pictures;
                string base64Cover = "";
                int r = 210;
                int g = 72;
                int b = 38;
                Bitmap coverBmp = new Bitmap(cPlayer.Properties.Resources.defaultCover);
                ImageConverter converter = new ImageConverter();
                if (cover.Length >= 1)
                {
                    {
                        MemoryStream bmpStr = new MemoryStream((byte[])cover[0].Data.Data);
                        Bitmap bmp = new Bitmap(bmpStr);
                        if (bmp.Height > 500)
                        {
                            bmp = new Bitmap(bmp, new Size(((bmp.Width * 500) / bmp.Height), 500));

                            TagLib.IPicture newArt = new TagLib.Picture(new TagLib.ByteVector((byte[])converter.ConvertTo(bmp, typeof(byte[]))));
                            tagFile.Tag.Pictures = new TagLib.IPicture[1] { newArt };
                            tagFile.Save();

                            cover = tagFile.Tag.Pictures;
                        }
                    }

                    base64Cover = Convert.ToBase64String((byte[])cover[0].Data.Data);

                    // Check if default cover is used
                    bool isDefaultCover = false;
                    try
                    {
                        // Get data from custom tag if it exists
                        TagLib.Id3v2.Tag tag = (TagLib.Id3v2.Tag)tagFile.GetTag(TagLib.TagTypes.Id3v2);
                        TagLib.Id3v2.PrivateFrame pFrame = TagLib.Id3v2.PrivateFrame.Get(tag, "isDefaultCover", false);
                        isDefaultCover = Encoding.Unicode.GetString(pFrame.PrivateData.Data).Equals("yes") ? true : false;
                    }
                    catch (Exception e) { }

                    if (!isDefaultCover)
                    {
                        MemoryStream bmpStr = new MemoryStream((byte[])cover[0].Data.Data);
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
                    }

                }
                else
                {
                    base64Cover = Convert.ToBase64String((byte[])converter.ConvertTo(coverBmp, typeof(byte[])));
                    TagLib.IPicture newArt = new TagLib.Picture(new TagLib.ByteVector((byte[])converter.ConvertTo(coverBmp, typeof(byte[]))));
                    tagFile.Tag.Pictures = new TagLib.IPicture[1] { newArt };

                    // Create custom tag if not exists to check if default cover used in future
                    TagLib.Id3v2.Tag tag = (TagLib.Id3v2.Tag)tagFile.GetTag(TagLib.TagTypes.Id3v2);
                    TagLib.Id3v2.PrivateFrame pFrame = TagLib.Id3v2.PrivateFrame.Get(tag, "isDefaultCover", true);
                    pFrame.PrivateData = Encoding.Unicode.GetBytes("yes");

                    tagFile.Save();
                }

                borderColorR = r;
                borderColorG = g;
                borderColorB = b;

                tagFile = null;

                currentBase64Cover = base64Cover;

                //MessageBox.Show("playMedia('file:\\\\\\\\" + filePath.Replace(@"/", @"\").Replace(@"\", @"\\").Replace(@"'", @"\'") + "','" + base64Cover + "','" + r + "','" + g + "','" + b + "','" + fileNameWithoutExtension + "','" + performer + "','" + title + "','" + album + "');");

                //this.bEval("playMedia('" +  filePath + "','" + r + "','" + g + "','" + b + "','" + Convert.ToBase64String(ToByteArray(fileNameWithoutExtension)) + "','" + Convert.ToBase64String(ToByteArray(performer)) + "','" + Convert.ToBase64String(ToByteArray(title)) + "','" + Convert.ToBase64String(ToByteArray(album)) + "');");
                this.bEval("playMedia('" + Convert.ToBase64String(ToByteArray("file:\\\\\\\\" + filePath)) + "','" + base64Cover + "','" + r + "','" + g + "','" + b + "','" + Convert.ToBase64String(ToByteArray(fileNameWithoutExtension)) + "','" + Convert.ToBase64String(ToByteArray(performer)) + "','" + Convert.ToBase64String(ToByteArray(title)) + "','" + Convert.ToBase64String(ToByteArray(album)) + "');");
            }

            return true;
        }


        //************************************************//
        //-------- Navigation/Load/Completed Code -------//
        //***********************************************//

        private void playerBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url != playerUrl)
            {
                if (this.filePath == e.Url.ToString().Replace(@"file:///", "")) bEval("playAgain();");
                else setMedia(e.Url.ToString().Replace(@"file:///", ""));

                e.Cancel = true;
            }
        }

        private void player_Load(object sender, EventArgs e)
        {
            this.Top = -3600;
            this.Left = -3600;
            this.playerBrowser.Navigate(playerUrl, "", null, "User-Agent:Player");
        }

        private void playerWebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            getConfig();
            this.getBrowserVariables.Enabled = true;

            if (!isBrowserDocumentMouseMoveFunctionAssigned)
            {
                this.playerBrowser.Document.MouseDown   += playerBrowser_MouseDown;
                this.playerBrowser.Document.MouseUp     += playerBrowser_MouseUp;
                this.playerBrowser.Document.MouseMove   += playerBrowser_MouseMove;
                this.playerBrowser.AllowWebBrowserDrop  = true;
            }
            isBrowserDocumentMouseMoveFunctionAssigned = true;
        }

        private void getBrowserVariables_Tick(object sender, EventArgs eArgs)
        {
            ///////////////////////////////////////
            // - Check Browser Frame Controls
            if (this.bEval("cFrameControlMinimized") == "1")
            {
                this.WindowState = FormWindowState.Minimized;
                this.bEval("cFrameControlMinimized=0");
            }
            if (this.bEval("cFrameControlSettings") == "1")
            {
                this.bEval("cFrameControlSettings=0");

                TagLib.File tagFile = TagLib.File.Create(filePath);
                settings settingsDialog = new settings(filePath, cnfSettingsX, cnfSettingsY, borderColorR, borderColorG, borderColorB);

                settingsDialog.base64Cover = currentBase64Cover;
                settingsDialog.sName = tagFile.Tag.Title != null ? tagFile.Tag.Title.ToString() : "";
                settingsDialog.sArtist = tagFile.Tag.Performers.Length > 0 ? tagFile.Tag.Performers[0] : "";
                settingsDialog.sAlbum = tagFile.Tag.Album != null ? tagFile.Tag.Album.ToString() : "";
                settingsDialog.sAlbumArtist = tagFile.Tag.AlbumArtists.Length > 0 ? tagFile.Tag.AlbumArtists[0] : "";
                settingsDialog.sGenre = tagFile.Tag.Genres.Length > 0 ? tagFile.Tag.Genres[0] : "";
                settingsDialog.sYear = tagFile.Tag.Year.ToString();

                settingsDialog.ShowDialog();

                if (settingsDialog.saveChanges && filePath == settingsDialog.trackName)
                {

                    TagLib.IPicture[] cover = tagFile.Tag.Pictures;

                    if (settingsDialog.newBase64Cover != "")
                    {
                        currentBase64Cover = settingsDialog.newBase64Cover;
                        borderColorR = settingsDialog.borderColorR;
                        borderColorG = settingsDialog.borderColorG;
                        borderColorB = settingsDialog.borderColorB;
                        ImageConverter converter = new ImageConverter();

                        byte[] imageBytes = Convert.FromBase64String(settingsDialog.newBase64Cover);
                        MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                        Bitmap coverBmp = new Bitmap(ms);

                        if (coverBmp.Height > 500)
                        {
                            coverBmp = new Bitmap(coverBmp, new Size(((coverBmp.Width * 500) / coverBmp.Height), 500));
                        }

                        TagLib.IPicture newArt = new TagLib.Picture(new TagLib.ByteVector((byte[])converter.ConvertTo(coverBmp, typeof(byte[]))));
                        tagFile.Tag.Pictures = new TagLib.IPicture[1] { newArt };
                        this.bEval("setNewCover('" + settingsDialog.newBase64Cover + "','" + settingsDialog.borderColorR + "','" + settingsDialog.borderColorG + "','" + settingsDialog.borderColorB + "');");

                        // Create custom tag if not exists to check if default cover used in future
                        TagLib.Id3v2.Tag tag = (TagLib.Id3v2.Tag)tagFile.GetTag(TagLib.TagTypes.Id3v2);
                        TagLib.Id3v2.PrivateFrame pFrame = TagLib.Id3v2.PrivateFrame.Get(tag, "isDefaultCover", true);

                        // Set new tag value to yes or no
                        if (settingsDialog.isDefaultCover) pFrame.PrivateData = Encoding.Unicode.GetBytes("yes");
                        else pFrame.PrivateData = Encoding.Unicode.GetBytes("no");
                    }
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
                    fileNameWithoutExtension = fileNameWithoutExtension.Replace(@"'", @"\'");

                    this.bEval("setNewInfo('" + Convert.ToBase64String(ToByteArray(fileNameWithoutExtension)) + "','" + Convert.ToBase64String(ToByteArray(settingsDialog.sArtist)) + "','" + Convert.ToBase64String(ToByteArray(settingsDialog.sName)) + "','" + Convert.ToBase64String(ToByteArray(settingsDialog.sAlbum)) + "');");

                    if (tagFile.Tag.Title == null || !tagFile.Tag.Title.ToString().Equals(settingsDialog.sName))
                    {
                        tagFile.Tag.Title = settingsDialog.sName;
                    }
                    if (tagFile.Tag.Performers.Length <= 0 || !tagFile.Tag.Performers[0].ToString().Equals(settingsDialog.sArtist))
                    {
                        tagFile.Tag.Performers = new string[] { settingsDialog.sArtist };
                    }
                    if (tagFile.Tag.Album == null || !tagFile.Tag.Album.ToString().Equals(settingsDialog.sAlbum))
                    {
                        tagFile.Tag.Album = settingsDialog.sAlbum;
                    }
                    if (tagFile.Tag.AlbumArtists.Length <= 0 || !tagFile.Tag.AlbumArtists[0].ToString().Equals(settingsDialog.sAlbumArtist))
                    {
                        tagFile.Tag.AlbumArtists = new string[] { settingsDialog.sAlbumArtist };
                    }
                    if (tagFile.Tag.Genres.Length <= 0 || !tagFile.Tag.Genres[0].ToString().Equals(settingsDialog.sGenre))
                    {
                        tagFile.Tag.Genres = new string[] { settingsDialog.sGenre };
                    }
                    if (!tagFile.Tag.Year.ToString().Equals(settingsDialog.sYear))
                    {
                        tagFile.Tag.Year = Convert.ToUInt32(settingsDialog.sYear);
                    }

                    this.bEval("pauseMedia();");
                    tagFile.Save();
                    this.bEval("returnMedia();");
                }

                cnfSettingsY = settingsDialog.Top;
                cnfSettingsX = settingsDialog.Left;
            }
            if (this.bEval("cFrameControlClosed") == "1")
            {
                 Application.Exit();
            }
            if (this.bEval("isPlayerReady") == "1")
            {
                this.bEval("isPlayerReady=0");
                setMedia(filePath);
            }
            // - End
            ///////////////////////////////////////

            try
            {
                string configText = File.ReadAllText(Application.StartupPath.ToString() + "/config.ini");
                string[] configArgs = configText.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string val in configArgs)
                {
                    if (val.Split(new string[] { "=" }, StringSplitOptions.None)[0] == "new_path")
                    {
                        if (val.Split(new string[] { "=" }, StringSplitOptions.None)[1].ToString() != "")
                        {
                            if (this.filePath == val.Split(new string[] { "=" }, StringSplitOptions.None)[1].ToString()) bEval("playAgain();");
                            else
                            {
                                setMedia(val.Split(new string[] { "=" }, StringSplitOptions.None)[1].ToString());
                                // Bring window to top.
                                TopMost = true;
                                Focus();
                                BringToFront();
                                TopMost = false;
                            }
                        }
                    }
                }
                setConfig();
            }
            catch (Exception e) { }
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
            setConfig();
        }

        private void playerBrowser_MouseMove(object sender, HtmlElementEventArgs e)
        {
            if (isDraging)
            {
                if (this.Left + (e.MousePosition.X - xPosition) < -this.Width+4)
                    this.Left = -this.Width+4;
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
            if (this.playerBrowser.Document.InvokeScript("eval", new object[] { _s }) == null) return "";
            return this.playerBrowser.Document.InvokeScript("eval", new object[] { _s }).ToString();
        }


        public static byte[] ToByteArray(string InString)
        {
            return Encoding.Default.GetBytes(InString);
            //return InString.Select(m => byte.Parse(m.ToString())).ToArray();
            //return Array.ConvertAll<char, byte>(InString.ToCharArray(), c => Convert.ToByte(c.ToString()));
            //System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding(true, true);
            //return encoding.GetBytes(StringToConvert);
            //char[] CharArray = StringToConvert.ToCharArray();
            //byte[] ByteArray = new byte[CharArray.Length];
            //for (int i = 0; i < CharArray.Length; i++) ByteArray[i] = Convert.ToByte("0x" + CharArray[i]);
            //return ByteArray;
        }

        [System.Runtime.InteropServices.DllImport("Shell32.dll")]
        private static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);

    }
}

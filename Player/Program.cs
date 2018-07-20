using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace cPlayer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)
            {
                string configText = File.ReadAllText(Application.StartupPath.ToString() + "/config.ini");

                string prevPath = "";
                string[] configArgs = configText.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string val in configArgs)
                {
                    if (val.Split(new string[] { "=" }, StringSplitOptions.None)[0] == "new_path") prevPath = val.Split(new string[] { "=" }, StringSplitOptions.None)[1].ToString();
                }

                if (prevPath != "") configText = configText.Replace(prevPath, args[0].Replace(@"\", @"/"));
                else configText += args[0].Replace(@"\", @"/");

                try
                {
                    File.WriteAllText(Application.StartupPath.ToString() + "/config.ini", configText);
                }
                catch (Exception e) { }
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(args.Length == 0 ? new player(string.Empty) : new player(args[0]));
        }
    }
}

using PointsLibUi.Extensions;
using System;
using System.IO;
using System.Windows.Forms;

namespace PointsLibUi
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*using (var writer = new System.IO.StreamWriter("c:\\temp\\1mPoints.csv"))
            {
                var r = new Random((int)DateTime.Now.Ticks);
                for (int i = 0; i < 1000000; ++i)
                {
                    double theta = r.NextDouble() * 2 * Math.PI;
                    int d = r.Next(0, 1000000);
                    double y = d * Math.Sin(theta);
                    double x = d * Math.Cos(theta);
                    writer.WriteLine("{0},{1}", x, y);
                }
            }*/

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var model = new MainModel())
            {
                var settingsSerializer = new SettingsSerializer(SettingsFilename);
                var settings = settingsSerializer.SerializeIn() ?? new ApplicationSettings();

                var optionsAdapter = new AlgorithmOptionsPropertyGridAdapter(model);
                MiscExtensions.CopySharedPublicProperties(settings, optionsAdapter);

                Application.Run(new MainForm(model, optionsAdapter));

                MiscExtensions.CopySharedPublicProperties(optionsAdapter, settings);
                settingsSerializer.SerializeOut(settings);
            }
        }

        private static string AppDataDir
        {
            get
            {
                string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Application.ProductName);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                return dir;
            }
        }

        private static string SettingsFilename
        {
            get
            {
                return Path.Combine(AppDataDir, "Settings.xml");
            }
        }
    }
}

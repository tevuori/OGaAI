using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OGaAI
{
	static class Program
	{
		/// <summary>
		/// Hlavní vstupní bod aplikace.
		/// </summary>
		[STAThread]
		public static void Main()
		{
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            if (!isAdbHere())
            {
                downandinstadb();
                Thread.Sleep(100);
            }
            else
            {

            }
		}
        public static bool isFileValit(String path)
        {
            if (File.Exists(path) && path.Contains(".apk"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool isAdbHere()
        {
            if (File.Exists("adb.exe"))
            {
                return true;
            }
            return false;
        }
        public static void downandinstadb()
        {
            Form1 f = new Form1();
            try
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                f.addline("Adb.exe was not found!");
                f.addline("Beginning installation process...");
                for(int i=0;i<3;i++)
                {
                    f.addline(String.Concat(Enumerable.Repeat(".", i)));
                    Thread.Sleep(100);
                }
                Thread.Sleep(50);
                f.addline("Downloading adb");

                using (var client = new WebClient())
                {
                    client.DownloadFile("https://filebin.net/archive/r2vlvrw10oo7cg8r/zip", "adb.zip");
                }
                f.addline(".....");
                f.addline("Working with archive");
                Process proc = new Process();
                proc.StartInfo.FileName = "tar";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.Arguments = "-xf adb.zip";
                proc.StartInfo.RedirectStandardOutput = true;
                proc.Start();
                proc.WaitForExit();

                f.addline(".....");
                f.addline("Deleting archive..");
                Thread.Sleep(200);

                string delpath = Environment.CurrentDirectory + "\\adb.zip";
                File.Delete(delpath);

                Console.WriteLine(" ----- ");
                Console.WriteLine(" ----- ");
                Console.ForegroundColor = ConsoleColor.Green;
                MessageBox.Show("We have installed some necessary packages. Thanks for Using OGaAI.exe!");

                string restartpath = Environment.CurrentDirectory + "\\OGaAI.exe";
                System.Diagnostics.Process.Start(restartpath);
                Thread.Sleep(100);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public static void show_packages()
        {
            Form1 f = new Form1();

            Process proc = new Process();
            proc.StartInfo.FileName = "adb.exe";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.Arguments = "shell pm list packages -3";
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.Start();
            proc.WaitForExit();

            String error = proc.StandardOutput.ReadToEnd();
            f.addline2(error);
            proc.WaitForExit();
        }
    }
}

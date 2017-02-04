using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] tempFolder = System.Reflection.Assembly.GetEntryAssembly().Location.Split('\\');
            string folder = "";
            for (int i = 0; i < tempFolder.Length - 1; i++)
            {
                folder += tempFolder[i] + "\\";
            }

            var githubToken = "updateToken";
            var url = "https://github.com/eranknafo2001/AcupunctureProject/raw/master/AcupunctureProject/bin/Release/AcupunctureProject.exe";
            var path = folder + "AcupunctureProject.exe";

            Console.WriteLine("delete old file");
            while (true)
            {
                try
                {
                    File.Delete(path);
                    break;
                }
                catch (Exception) { }
            }
            Console.WriteLine("download new file");
            using (var client = new System.Net.Http.HttpClient())
            {
                var credentials = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}:", githubToken);
                credentials = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(credentials));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
                var contents = client.GetByteArrayAsync(url).Result;
                System.IO.File.WriteAllBytes(path, contents);
            }

            Console.WriteLine("open new file");
            System.Diagnostics.Process.Start(folder + "AcupunctureProject.exe");
            Environment.Exit(0);
        }
    }
}

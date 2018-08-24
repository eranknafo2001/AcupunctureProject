using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSCP;
using Newtonsoft.Json;

namespace UpdateApp
{
	class Program
	{
		const string settingsFile = @"UpdateSettings.json";
		static void Main(string[] args)
		{
			if (!File.Exists(settingsFile))
				File.WriteAllBytes(settingsFile, Properties.Resources.DefaultSettings);
			dynamic settings = JsonConvert.DeserializeObject(File.ReadAllText(settingsFile));
			if ((bool)settings.isActivated)
			{
				using (Session s = new Session())
				{
					Console.WriteLine("connecting to server");
					s.Open(new SessionOptions
					{
						Protocol = Protocol.Ftp,
						HostName = settings.host,
						UserName = settings.user,
						Password = settings.password
					});
					Console.WriteLine("downloading the number of the newest ver");
					s.GetFiles(@"/LastUpdate.json", @"newUpdate.json",
						false, new TransferOptions
						{ TransferMode = TransferMode.Ascii }).Check();
					Console.WriteLine("downloaded the number of the newest ver");
					dynamic nj = JsonConvert.DeserializeObject(File.ReadAllText(@"newUpdate.json"));
					if (File.Exists(@"LastUpdate.json"))
					{
						dynamic lj = JsonConvert.DeserializeObject(File.ReadAllText(@"LastUpdate.json"));
						File.Delete(@"LastUpdate.json");

						if (nj.ver == lj.ver)
						{
							Console.WriteLine("allready up to date");
							System.Diagnostics.Process.Start("AcupunctureProject.exe");
							return;
						}
					}
					File.Move(@"newUpdate.json", @"LastUpdate.json");
					Console.WriteLine("downloading update");
					var tr = s.GetFiles(@"/Updates/*", @".\",
						false, new TransferOptions
						{
							TransferMode = TransferMode.Binary,
							OverwriteMode = OverwriteMode.Overwrite
						});
					tr.Check();
					foreach (TransferEventArgs transfer in tr.Transfers)
						Console.WriteLine($"the file {transfer.Destination} as been downloaded");
					Console.WriteLine("downloaded update");

					System.Diagnostics.Process.Start("AcupunctureProject.exe");
				}
			}
		}
	}
}

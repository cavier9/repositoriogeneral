using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace acortarurlcjcm.App{
	public class Config
	{
		public string BASE_URL;
	}
	public class OtraConfiguracion{
		public Config Config;
		public OtraConfiguracion() {
			Config = Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(File.ReadAllText("App/Config.json"));
		}
	}
}

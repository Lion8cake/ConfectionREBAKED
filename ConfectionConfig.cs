using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Terraria.ModLoader.Config;

namespace TheConfectionRebirth
{
	public class ConfectionConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		public struct WorldDataValues
		{
			public bool confection;
		}

		// Key value is each twld path
		[DefaultListValue(false)]
		[JsonProperty]
		private Dictionary<string, WorldDataValues> worldData = new Dictionary<string, WorldDataValues>();

		// Methods to avoid public variable getting picked up by serialiser
		public Dictionary<string, WorldDataValues> GetWorldData() { return worldData; }
		public void SetWorldData(Dictionary<string, WorldDataValues> newDict) { worldData = newDict; }
		public static void Save(ModConfig config)
		{
			Directory.CreateDirectory(ConfigManager.ModConfigPath);
			string filename = config.Mod.Name + "_" + config.Name + ".json";
			string path = Path.Combine(ConfigManager.ModConfigPath, filename);
			string json = JsonConvert.SerializeObject((object)config, ConfigManager.serializerSettings);
			File.WriteAllText(path, json);
		}
	}
	public class ConfectionServerConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;

		[Header("ConfectionServersideConfig")]
		[Label("Cookie and Cream Spreading")]
		[Tooltip(@"Enables spreading for the following:
		Cookie Block
		Cream Block
		Pink Fairy Floss
		Purple Fairy Floss
		Blue Fairy Floss
		Cookiest Cookie Block
		All Creamstone gems")]
		[DefaultValue(true)]
		public bool CookieSpread;
	}
}

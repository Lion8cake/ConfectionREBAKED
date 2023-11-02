using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Terraria.ModLoader.Config;

namespace TheConfectionRebirth
{
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

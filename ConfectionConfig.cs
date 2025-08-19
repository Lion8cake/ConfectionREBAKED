using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace TheConfectionRebirth
{
	public class ConfectionServerConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;

		[Header("ConfectionServersideConfig")]
		[DrawTicks]
		[OptionStrings(new string[] { "No Spread", "Partial Spread", "Full Spread" })]
		[DefaultValue("Partial Spread")]
		public string CookieSpread;
	}
}

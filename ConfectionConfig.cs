using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;
using TheConfectionRebirth.Items.Armor;

namespace TheConfectionRebirth
{
	public class ConfectionServerConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;

		[Header("ConfectionServersideConfig")]
		[DrawTicks]
		[OptionStrings(new string[] { "No Spread", "Partial Spread", "Full Spread" })]
		[DefaultValue("Full Spread")]
		public string CookieSpread;
	}
}

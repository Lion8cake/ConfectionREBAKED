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

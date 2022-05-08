using TheConfectionRebirth.Items;
using TheConfectionRebirth.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace TheConfectionRebirth
{
	public static class ConfectionGlobalTile2
	{
	public static ConfectionPlayer Confection(this Player player)
    {
	    return player.GetModPlayer<ConfectionPlayer>();
    }
	}
}
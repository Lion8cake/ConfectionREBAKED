using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Tiles
{
	public class CookiestCookieBlock : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileMergeDirt[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;

			TileID.Sets.CanBeDugByShovel[Type] = true;
			TileID.Sets.ChecksForMerge[Type] = true;
			ConfectionIDs.Sets.ConfectionBiomeSight[Type] = true;
			ConfectionIDs.Sets.Confection[Type] = true;
			ConfectionIDs.Sets.IsExtraConfectionTile[Type] = true;

			Main.tileMerge[Type][ModContent.TileType<Creamstone>()] = true;

			AddMapEntry(new Color(153, 97, 60));
			DustType = DustID.Dirt;
			RegisterItemDrop(ModContent.ItemType<Items.CookiestBlock>());
		}
	}
}

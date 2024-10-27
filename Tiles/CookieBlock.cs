using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
	public class CookieBlock : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileBlockLight[Type] = true;

			TileID.Sets.Conversion.Dirt[Type] = true;
			TileID.Sets.CanBeDugByShovel[Type] = true;
			TileID.Sets.ChecksForMerge[Type] = true;
			TileID.Sets.CanBeClearedDuringOreRunner[Type] = true;
			ConfectionIDs.Sets.ConfectionBiomeSight[Type] = true;
			ConfectionIDs.Sets.Confection[Type] = true;
			ConfectionIDs.Sets.IsExtraConfectionTile[Type] = true;

			Main.tileMerge[Type][ModContent.TileType<Creamstone>()] = true;

			AddMapEntry(new Color(153, 97, 60));
			DustType = DustID.Dirt;
		}
	}
}

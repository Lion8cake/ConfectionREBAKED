using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
	public class CreamBlock : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileBlockLight[Type] = true;
			AddMapEntry(new Color(219, 223, 234));
			TileID.Sets.Conversion.Snow[Type] = true;
			TileID.Sets.CanBeDugByShovel[Type] = true;
			TileID.Sets.Snow[Type] = true;
			TileID.Sets.IcesSnow[Type] = true;
			TileID.Sets.ChecksForMerge[Type] = true;
			TileID.Sets.CanBeClearedDuringOreRunner[Type] = true;
			ConfectionIDs.Sets.ConfectionBiomeSight[Type] = true;
			Main.tileMerge[Type][ModContent.TileType<Creamstone>()] = true;
			DustType = ModContent.DustType<CreamDust>();
			HitSound = SoundID.Item48;
		}
	}
}

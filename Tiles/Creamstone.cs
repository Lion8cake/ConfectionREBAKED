using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
	public class Creamstone : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileStone[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 9000;
			Main.tileBrick[Type] = true;
			Main.tileBlockLight[Type] = true;
			AddMapEntry(new Color(188, 168, 120));
			TileID.Sets.ChecksForMerge[Type] = true;
			TileID.Sets.CanBeClearedDuringOreRunner[Type] = true;
			ConfectionIDs.Sets.CanGrowSaccharite[Type] = true;
			ConfectionIDs.Sets.ConfectionBiomeSight[Type] = true;
			Main.tileMerge[Type][ModContent.TileType<SacchariteBlock>()] = true;
			Main.tileMerge[Type][ModContent.TileType<CreamstoneStalactite>()] = true;
			Main.tileMerge[Type][ModContent.TileType<BlueIceStalactite>()] = true;
			DustType = ModContent.DustType<CreamstoneDust>();

			HitSound = SoundID.Tink;
			MineResist = 2f;
			MinPick = 65;
		}
	}
}

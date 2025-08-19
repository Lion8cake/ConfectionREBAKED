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

			TileID.Sets.Conversion.Stone[Type] = true;
			TileID.Sets.ChecksForMerge[Type] = true;
			TileID.Sets.CanBeClearedDuringOreRunner[Type] = true;
			ConfectionIDs.Sets.CanGrowSaccharite[Type] = true;
			ConfectionIDs.Sets.ConfectionBiomeSight[Type] = true;
			ConfectionIDs.Sets.Confection[Type] = true;
			ConfectionIDs.Sets.IsNaturalConfectionTile[Type] = true;

			Main.tileMerge[Type][ModContent.TileType<SacchariteBlock>()] = true; //For some reason, it wont connect to these tiles
			Main.tileMerge[Type][ModContent.TileType<CreamstoneStalactite>()] = true;
			Main.tileMerge[Type][ModContent.TileType<BlueIceStalactite>()] = true;
			ConfectionGlobalTile.MergesWithCreamGems(Type);

			DustType = ModContent.DustType<CreamstoneDust>();
			AddMapEntry(new Color(188, 168, 120));
			HitSound = SoundID.Tink;
			MineResist = 2f;
			MinPick = 65;
		}
	}
}

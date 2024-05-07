using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
	public class Creamsandstone : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			
			TileID.Sets.Conversion.Sandstone[Type] = true;
			TileID.Sets.ForAdvancedCollision.ForSandshark[Type] = true;
			TileID.Sets.isDesertBiomeSand[Type] = true;
			TileID.Sets.ChecksForMerge[Type] = true;
			TileID.Sets.CanBeClearedDuringOreRunner[Type] = true;
			ConfectionIDs.Sets.CanGrowSaccharite[Type] = true;
			ConfectionIDs.Sets.ConfectionBiomeSight[Type] = true;

			Main.tileMerge[Type][ModContent.TileType<SacchariteBlock>()] = true;
			Main.tileMerge[Type][ModContent.TileType<CreamstoneStalactite>()] = true;
			Main.tileMerge[Type][ModContent.TileType<BlueIceStalactite>()] = true;

			AddMapEntry(new Color(89, 47, 36));
			DustType = ModContent.DustType<CreamsandDust>();
		}

		//Add special merging for hardened creamsand and creamsand
		//Texture needs to be updated to reflect this
		//Main.tileMergeDirt will need to be false/removed
	}
}

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
	public class HardenedCreamsand : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileMergeDirt[Type] = true; //402
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			AddMapEntry(new Color(108, 61, 49));
			TileID.Sets.Conversion.HardenedSand[Type] = true;
			TileID.Sets.ForAdvancedCollision.ForSandshark[Type] = true;
			TileID.Sets.ChecksForMerge[Type] = true;
			TileID.Sets.CanBeClearedDuringOreRunner[Type] = true;
			ConfectionIDs.Sets.CanGrowSaccharite[Type] = true;
			ConfectionIDs.Sets.ConfectionBiomeSight[Type] = true;
			Main.tileMerge[Type][ModContent.TileType<SacchariteBlock>()] = true;
			Main.tileMerge[Type][ModContent.TileType<CreamstoneStalactite>()] = true;
			Main.tileMerge[Type][ModContent.TileType<BlueIceStalactite>()] = true;
			DustType = ModContent.DustType<CreamsandDust>();
		}

		//Add special merging for creamsandstone and creamsand
		//Texture needs to be updated to reflect this
		//Main.tileMergeDirt will need to be false/removed
	}
}

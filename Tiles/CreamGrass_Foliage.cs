using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
	public class CreamGrass_Foliage : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileShine[Type] = 9000;
			Main.tileFrameImportant[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileNoAttach[Type] = true;
			Main.tileNoFail[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.TileCutIgnore.Regrowth[Type] = true;
			TileID.Sets.ReplaceTileBreakUp[Type] = true;
			TileID.Sets.SlowlyDiesInWater[Type] = true;
			TileID.Sets.SwaysInWindBasic[Type] = true;
			TileID.Sets.DrawFlipMode[Type] = 1;
			TileID.Sets.IgnoredByGrowingSaplings[Type] = true;
			ConfectionIDs.Sets.ConfectionBiomeSight[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1); //Literally only here so I can place the foliage with dragonlens
			TileObjectData.newTile.CoordinateHeights = new int[] { 20 };
			TileObjectData.addTile(Type);

			AddMapEntry(new Color(200, 170, 108));
			DustType = ModContent.DustType<CreamsandDust>();
			HitSound = SoundID.Grass;
		}

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak) {
			WorldGen.PlantCheck(i, j);
			return false;
		}
	}
}

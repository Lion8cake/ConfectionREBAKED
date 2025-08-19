using Microsoft.Xna.Framework;
using System.Collections.Generic;
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

			AddMapEntry(new Color(200, 170, 108));
			DustType = ModContent.DustType<CreamGrassDust>();
			HitSound = SoundID.Grass;
		}

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak) {
			WorldGen.PlantCheck(i, j);
			return false;
		}

		public override IEnumerable<Item> GetItemDrops(int i, int j) {
			if (Main.tile[i, j].TileFrameX == 144) {
				yield return new Item(ModContent.ItemType<Items.Placeable.YumDrop>());
			}
		}
	}
}

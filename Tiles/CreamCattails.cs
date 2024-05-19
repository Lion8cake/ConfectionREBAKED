using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
	public class CreamCattails : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileNoFail[Type] = true;
			Main.tileLighted[Type] = true;

			TileID.Sets.TileCutIgnore.Regrowth[Type] = true;

			AddMapEntry(new Color(200, 170, 108));
			DustType = ModContent.DustType<CreamGrassDust>();
			HitSound = SoundID.Grass;
		}

		public override void RandomUpdate(int i, int j) {
			ConfectionWorldGeneration.CheckCreamCatTail(i, j);
			if (Main.tile[i, j].HasTile && WorldGen.genRand.NextBool(8)) {
				WorldGen.GrowCatTail(i, j);
				ConfectionWorldGeneration.CheckCreamCatTail(i, j);
			}
		}

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak) {
			ConfectionWorldGeneration.CheckCreamCatTail(i, j);
			return false;
		}

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch) {
			bool intoRenderTargets = true;
			bool flag = intoRenderTargets || Main.LightingEveryFrame;

			if (Main.tile[i, j].TileFrameX / 18 <= 4 && flag) {
				Main.instance.TilesRenderer.AddSpecialPoint(j, i, (int)TileCounterType.MultiTileGrass);
			}

			return false;
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY) {
			offsetY = 2;
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects) {
			if (i % 2 == 0) {
				spriteEffects = (SpriteEffects)1;
			}
		}

		private enum TileCounterType {
			Tree,
			DisplayDoll,
			HatRack,
			WindyGrass,
			MultiTileGrass,
			MultiTileVine,
			Vine,
			BiomeGrass,
			VoidLens,
			ReverseVine,
			TeleportationPylon,
			MasterTrophy,
			AnyDirectionalGrass,
			Count
		}
	}


	public class CattailGen : GlobalTile {
		public override void RandomUpdate(int i, int j, int type) {
			if (j >= Main.worldSurface) {
				if (Main.tile[i, j].LiquidAmount > 32) {
					if (WorldGen.genRand.NextBool(600)) {
						WorldGen.PlaceTile(i, j, ModContent.TileType<CreamCattails>(), mute: true);
						if (Main.netMode == NetmodeID.Server) {
							NetMessage.SendTileSquare(-1, i, j);
						}
					}
				}
			}
		}
	}
}

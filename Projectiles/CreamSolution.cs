using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TheConfectionRebirth;
using TheConfectionRebirth.Tiles;
using TheConfectionRebirth.Walls;
using TheConfectionRebirth.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameContent.Creative;

namespace TheConfectionRebirth.Projectiles
{
	public class CreamSolution : ModProjectile
	{
	    public ref float Progress => ref Projectile.ai[0];
		
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Confection Spray");
		}

		public override void SetDefaults() {
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 2;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}

		/*public override void AI() {
			int dustType = ModContent.DustType<Dusts.CreamSolution>();

			if (Projectile.owner == Main.myPlayer) {
				Convert((int)(Projectile.position.X + (Projectile.width * 0.5f)) / 16, (int)(Projectile.position.Y + (Projectile.height * 0.5f)) / 16, 2);
			}

			if (Projectile.timeLeft > 133) {
				Projectile.timeLeft = 133;
			}

			if (Progress > 7f) {
				float dustScale = 1f;

				if (Progress == 8f) {
					dustScale = 0.2f;
				}
				else if (Progress == 9f) {
					dustScale = 0.4f;
				}
				else if (Progress == 10f) {
					dustScale = 0.6f;
				}
				else if (Progress == 11f) {
					dustScale = 0.8f;
				}

				Progress += 1f;

				int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100);
				Dust dust = Main.dust[dustIndex];
				dust.noGravity = true;
				dust.scale *= 1.75f;
				dust.velocity.X *= 2f;
				dust.velocity.Y *= 2f;
				dust.scale *= dustScale;
			}
			else {
				Progress += 1f;
			}

			Projectile.rotation += 0.3f * Projectile.direction;
		}

		public void Convert(int i, int j, int size = 4) {
			for (int k = i - size; k <= i + size; k++) {
				for (int l = j - size; l <= j + size; l++) {
					if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size)) {
						int type = Main.tile[k, l].type;
						int wall = Main.tile[k, l].wall;

						if (WallID.Sets.Conversion.Stone[wall]) {
							Main.tile[k, l].wall = (ushort)ModContent.WallType<CreamstoneWall>();
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (WallID.Sets.Conversion.HardenedSand[wall]) {
							Main.tile[k, l].wall = (ushort)ModContent.WallType<HardenedCreamsandWall>();
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (WallID.Sets.Conversion.Sandstone[wall]) {
							Main.tile[k, l].wall = (ushort)ModContent.WallType<CreamsandstoneWall>();
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (WallID.Sets.Conversion.Grass[wall]) {
							Main.tile[k, l].wall = (ushort)ModContent.WallType<CreamGrassWall>();
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else
		                {
			               switch (wall)
			               {
			               case 40:
				           Main.tile[k, l].wall = (ushort)ModContent.WallType<CreamWall>();
				           WorldGen.SquareWallFrame(k, l);
				           NetMessage.SendTileSquare(-1, k, l, 1);
				           break;
						   case 2:
			               case 16:
			               case 59:
			               case 196:
			               case 197:
			               case 198:
		               	   case 199:
				           Main.tile[k, l].wall = (ushort)ModContent.WallType<CookieWall>();
				           WorldGen.SquareWallFrame(k, l);
				           NetMessage.SendTileSquare(-1, k, l, 1);
				           break;
						   case 71:
				           Main.tile[k, l].wall = (ushort)ModContent.WallType<BlueIceWall>();
				           WorldGen.SquareWallFrame(k, l);
				           NetMessage.SendTileSquare(-1, k, l, 1);
				           break;
						   }
						}
 
						if (TileID.Sets.Conversion.Stone[type]) {
							Main.tile[k, l].type = (ushort)ModContent.TileType<Creamstone>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (TileID.Sets.Conversion.Sand[type]) {
							Main.tile[k, l].type = (ushort)ModContent.TileType<Creamsand>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
					    }
						else if (Main.tile[k, l].type == TileID.Dirt) {
							Main.tile[k, l].type = (ushort)ModContent.TileType<CookieBlock>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
					    }
						else if (Main.tile[k, l].type == TileID.SnowBlock) {
							Main.tile[k, l].type = (ushort)ModContent.TileType<CreamBlock>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
					    }
						else if (TileID.Sets.Conversion.Grass[type]) {
							Main.tile[k, l].type = (ushort)ModContent.TileType<CreamGrass>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
					    }
						else if (TileID.Sets.Conversion.Ice[type]) {
							Main.tile[k, l].type = (ushort)ModContent.TileType<BlueIce>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
					    }
						else if (TileID.Sets.Conversion.Sandstone[type]) {
							Main.tile[k, l].type = (ushort)ModContent.TileType<Creamsandstone>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
					    }
						else if (TileID.Sets.Conversion.HardenedSand[type]) {
							Main.tile[k, l].type = (ushort)ModContent.TileType<HardenedCreamsand>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
					    }
					    else if (Main.tile[k, l].type == TileID.Ruby) {
							Main.tile[k, l].type = (ushort)ModContent.TileType<CreamstoneRuby>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
					    }
						else if (Main.tile[k, l].type == TileID.Sapphire) {
							Main.tile[k, l].type = (ushort)ModContent.TileType<CreamstoneSaphire>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
					    }
						else if (Main.tile[k, l].type == TileID.Diamond) {
							Main.tile[k, l].type = (ushort)ModContent.TileType<CreamstoneDiamond>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
					    }
						else if (Main.tile[k, l].type == TileID.Emerald) {
							Main.tile[k, l].type = (ushort)ModContent.TileType<CreamstoneEmerald>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
					    }
						else if (Main.tile[k, l].type == TileID.Amethyst) {
							Main.tile[k, l].type = (ushort)ModContent.TileType<CreamstoneAmethyst>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
					    }
						else if (Main.tile[k, l].type == TileID.Topaz) {
							Main.tile[k, l].type = (ushort)ModContent.TileType<CreamstoneTopaz>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
					    }
						else if (Main.tile[k, l].type == TileID.Cloud) {
							Main.tile[k, l].type = (ushort)ModContent.TileType<PinkFairyFloss>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
					    }
						else if (Main.tile[k, l].type == TileID.RainCloud) {
							Main.tile[k, l].type = (ushort)ModContent.TileType<PurpleFairyFloss>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
					    }
						else if (Main.tile[k, l].type == TileID.SnowCloud) {
							Main.tile[k, l].type = (ushort)ModContent.TileType<BlueFairyFloss>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
					    }
				}
			}
		}
	}*/
}}

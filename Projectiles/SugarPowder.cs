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

namespace TheConfectionRebirth.Projectiles
{
	public class SugarPowder : ModProjectile
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Sugar Powder");
		}

		public override void SetDefaults() {
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 2;
			// projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}

		public override void AI() {
			int dustType = ModContent.DustType<Dusts.CreamSolution>();

			if (Projectile.owner == Main.myPlayer)
				//Convert((int)(Projectile.position.X + Projectile.width / 2) / 16, (int)(Projectile.position.Y + Projectile.height / 2) / 16, 2);

			if (Projectile.timeLeft > 133)
				Projectile.timeLeft = 133;

			if (Projectile.ai[0] > 1f) {
				float dustScale = 1f;

				if (Projectile.ai[0] == 1f)
					dustScale = 0.2f;
				else if (Projectile.ai[0] == 1f)
					dustScale = 0.4f;
				else if (Projectile.ai[0] == 1f)
					dustScale = 0.6f;
				else if (Projectile.ai[0] == 1f)
					dustScale = 0.8f;

				Projectile.ai[0] += 1f;

				for (int i = 0; i < 1; i++) {
					int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 25);
					Dust dust = Main.dust[dustIndex];
					dust.noGravity = true;
					dust.scale *= 1.75f;
					dust.velocity.X *= 2f;
					dust.velocity.Y *= 2f;
					dust.scale *= dustScale;
				}
			}
			else
				Projectile.ai[0] += 1f;

			Projectile.rotation += 0.3f * Projectile.direction;
		}

		/*public void Convert(int i, int j, int size = 4) {
			for (int k = i - size; k <= i + size; k++) {
				for (int l = j - size; l <= j + size; l++) {
					if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size)) {
						int type = Main.tile[k, l].type;
						int wall = Main.tile[k, l].wall;

						if (Main.tile[k, l].type == TileID.Ebonstone) {
							Main.tile[k, l].type = TileID.Stone;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].type == TileID.Crimstone) {
							Main.tile[k, l].type = TileID.Stone;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].type == TileID.CorruptGrass) {
							Main.tile[k, l].type = TileID.Grass;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].type == TileID.CrimsonGrass) {
							Main.tile[k, l].type = TileID.Grass;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].type == TileID.Crimsand) {
							Main.tile[k, l].type = TileID.Sand;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].type == TileID.Ebonsand) {
							Main.tile[k, l].type = TileID.Sand;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].type == TileID.Crimsand) {
							Main.tile[k, l].type = TileID.Sand;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						
				}
			}
		}
	}*/
}}

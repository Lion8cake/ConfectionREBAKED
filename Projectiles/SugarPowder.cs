using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Projectiles
{
    public class SugarPowder : ModProjectile
    {
        public ref float Progress => ref Projectile.ai[0];

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sugar Powder");
        }

        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 2;
            // Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            int dustType = 20;

            if (Projectile.owner == Main.myPlayer)
            {
                Convert((int)(Projectile.position.X + (Projectile.width * 0.5f)) / 16, (int)(Projectile.position.Y + (Projectile.height * 0.5f)) / 16, 2);
            }

            if (Projectile.timeLeft > 35)
            {
                Projectile.timeLeft = 35;
            }

            if (Progress > 7f)
            {
                float dustScale = 1f;

                if (Progress == 8f)
                {
                    dustScale = 0.2f;
                }
                else if (Progress == 9f)
                {
                    dustScale = 0.4f;
                }
                else if (Progress == 10f)
                {
                    dustScale = 0.6f;
                }
                else if (Progress == 11f)
                {
                    dustScale = 0.8f;
                }

                Progress += 1f;


                var dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100);

                dust.noGravity = true;
                dust.scale *= 1.75f;
                dust.velocity.X *= 2f;
                dust.velocity.Y *= 2f;
                dust.scale *= dustScale;
            }
            else
            {
                Progress += 1f;
            }

            Projectile.rotation += 0.3f * Projectile.direction;
        }

        private static void Convert(int i, int j, int size = 4)
        {
            for (int k = i - size; k <= i + size; k++)
            {
                for (int l = j - size; l <= j + size; l++)
                {
                    if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt((size * size) + (size * size)))
                    {
                        int type = Main.tile[k, l].TileType;
                        int wall = Main.tile[k, l].WallType;

                        if (Main.tile[k, l].TileType == TileID.Ebonstone)
                        {
                            Main.tile[k, l].TileType = TileID.Stone;
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                        else if (Main.tile[k, l].TileType == TileID.Crimstone)
                        {
                            Main.tile[k, l].TileType = TileID.Stone;
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                        else if (Main.tile[k, l].TileType == TileID.CorruptGrass)
                        {
                            Main.tile[k, l].TileType = TileID.Grass;
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                        else if (Main.tile[k, l].TileType == TileID.CrimsonGrass)
                        {
                            Main.tile[k, l].TileType = TileID.Grass;
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                        else if (Main.tile[k, l].TileType == TileID.Crimsand)
                        {
                            Main.tile[k, l].TileType = TileID.Sand;
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                        else if (Main.tile[k, l].TileType == TileID.Ebonsand)
                        {
                            Main.tile[k, l].TileType = TileID.Sand;
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                        else if (Main.tile[k, l].TileType == TileID.Crimsand)
                        {
                            Main.tile[k, l].TileType = TileID.Sand;
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                    }
                }
            }
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


				var dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100);

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

		private static void Convert(int i, int j, int size = 4) {
			for (int k = i - size; k <= i + size; k++) {
				for (int l = j - size; l <= j + size; l++) {
					if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt((size * size) + (size * size))) {
						int type = Main.tile[k, l].TileType;
						int wall = Main.tile[k, l].WallType;

						if (Main.tile[k, l].TileType == TileID.Ebonstone) {
							Main.tile[k, l].TileType = TileID.Stone;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.Crimstone) {
							Main.tile[k, l].TileType = TileID.Stone;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.CorruptGrass) {
							Main.tile[k, l].TileType = TileID.Grass;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.CrimsonGrass) {
							Main.tile[k, l].TileType = TileID.Grass;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.Crimsand) {
							Main.tile[k, l].TileType = TileID.Sand;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.Ebonsand) {
							Main.tile[k, l].TileType = TileID.Sand;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.Crimsand) {
							Main.tile[k, l].TileType = TileID.Sand;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						
					}
				}
			}
		}*/
    }
}

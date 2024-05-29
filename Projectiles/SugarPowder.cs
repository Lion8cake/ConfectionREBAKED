using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.NPCs;

namespace TheConfectionRebirth.Projectiles
{
    public class SugarPowder : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 64;
			Projectile.height = 64;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.ignoreWater = true;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
			width = Projectile.width + 32;
			height = Projectile.height + 32;
			return true;
		}

		public override void AI() {
			Projectile.velocity *= 0.95f;
			Projectile.ai[0] += 1f;
			if (Projectile.ai[0] == 180f) {
				Projectile.Kill();
			}
			if (Projectile.ai[1] <= 1f) {
				Projectile.ai[1] = 2f;
				int num954 = ModContent.DustType<SugarDust>();
				int num966 = 30;
				for (int num977 = 0; num977 < num966; num977++) {
					Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, num954, Projectile.velocity.X, Projectile.velocity.Y, 50);
				}
			}
			bool flag34 = Main.myPlayer == Projectile.owner;
			if (flag34) {
				int num988 = (int)(Projectile.position.X / 16f) - 1;
				int num999 = (int)((Projectile.position.X + (float)Projectile.width) / 16f) + 2;
				int num1010 = (int)(Projectile.position.Y / 16f) - 1;
				int num1021 = (int)((Projectile.position.Y + (float)Projectile.height) / 16f) + 2;
				if (num988 < 0) {
					num988 = 0;
				}
				if (num999 > Main.maxTilesX) {
					num999 = Main.maxTilesX;
				}
				if (num1010 < 0) {
					num1010 = 0;
				}
				if (num1021 > Main.maxTilesY) {
					num1021 = Main.maxTilesY;
				}
				Vector2 vector57 = default(Vector2);
				for (int num1032 = num988; num1032 < num999; num1032++) {
					for (int num1043 = num1010; num1043 < num1021; num1043++) {
						vector57.X = num1032 * 16;
						vector57.Y = num1043 * 16;
						if (!(Projectile.position.X + (float)Projectile.width > vector57.X) || !(Projectile.position.X < vector57.X + 16f) || !(Projectile.position.Y + (float)Projectile.height > vector57.Y) || !(Projectile.position.Y < vector57.Y + 16f) || !Main.tile[num1032, num1043].HasTile) {
							continue;
						}
						if (Main.tile[num1032, num1043].TileType == TileID.CorruptGrass || Main.tile[num1032, num1043].TileType == TileID.CrimsonGrass) {
							Main.tile[num1032, num1043].TileType = TileID.Grass;
							WorldGen.SquareTileFrame(num1032, num1043);
							if (Main.netMode == NetmodeID.MultiplayerClient) {
								NetMessage.SendTileSquare(-1, num1032, num1043);
							}
						}
						if (Main.tile[num1032, num1043].TileType == TileID.Ebonstone || Main.tile[num1032, num1043].TileType == TileID.Crimstone) {
							Main.tile[num1032, num1043].TileType = TileID.Stone;
							WorldGen.SquareTileFrame(num1032, num1043);
							if (Main.netMode == NetmodeID.MultiplayerClient) {
								NetMessage.SendTileSquare(-1, num1032, num1043);
							}
						}
						if (Main.tile[num1032, num1043].TileType == TileID.Ebonsand || Main.tile[num1032, num1043].TileType == TileID.Crimsand) {
							Main.tile[num1032, num1043].TileType = TileID.Sand;
							WorldGen.SquareTileFrame(num1032, num1043);
							if (Main.netMode == NetmodeID.MultiplayerClient) {
								NetMessage.SendTileSquare(-1, num1032, num1043);
							}
						}
						if (Main.tile[num1032, num1043].TileType == TileID.CorruptIce || Main.tile[num1032, num1043].TileType == TileID.FleshIce) {
							Main.tile[num1032, num1043].TileType = TileID.IceBlock;
							WorldGen.SquareTileFrame(num1032, num1043);
							if (Main.netMode == NetmodeID.MultiplayerClient) {
								NetMessage.SendTileSquare(-1, num1032, num1043);
							}
						}
						if (Main.tile[num1032, num1043].TileType == TileID.CorruptSandstone || Main.tile[num1032, num1043].TileType == TileID.CrimsonSandstone) {
							Main.tile[num1032, num1043].TileType = TileID.Sandstone;
							WorldGen.SquareTileFrame(num1032, num1043);
							if (Main.netMode == NetmodeID.MultiplayerClient) {
								NetMessage.SendTileSquare(-1, num1032, num1043);
							}
						}
						if (Main.tile[num1032, num1043].TileType == TileID.CorruptHardenedSand || Main.tile[num1032, num1043].TileType == TileID.CrimsonHardenedSand) {
							Main.tile[num1032, num1043].TileType = TileID.HardenedSand;
							WorldGen.SquareTileFrame(num1032, num1043);
							if (Main.netMode == NetmodeID.MultiplayerClient) {
								NetMessage.SendTileSquare(-1, num1032, num1043);
							}
						}
						if (Main.tile[num1032, num1043].TileType == TileID.CorruptJungleGrass || Main.tile[num1032, num1043].TileType == TileID.CrimsonJungleGrass) {
							Main.tile[num1032, num1043].TileType = TileID.JungleGrass;
							WorldGen.SquareTileFrame(num1032, num1043);
							if (Main.netMode == NetmodeID.MultiplayerClient) {
								NetMessage.SendTileSquare(-1, num1032, num1043);
							}
						}
					}
				}
			}
		}

		public override bool? CanDamage() {
			Rectangle rectangle = Projectile.Hitbox;
			if (Main.netMode != 1) {
				for (int n = 0; n < 200; n++) {
					NPC nPC2 = Main.npc[n];
					if (!nPC2.active) {
						continue;
					}
					if (nPC2.type == NPCID.DemonTaxCollector) {
						if (rectangle.Intersects(nPC2.Hitbox)) {
							nPC2.Transform(441);
						}
					}
					else if ((nPC2.type == NPCID.Bunny || nPC2.type == NPCID.BunnySlimed || nPC2.type == NPCID.BunnyXmas || nPC2.type == NPCID.ExplosiveBunny || nPC2.type == NPCID.PartyBunny) && Main.hardMode) {
						if (rectangle.Intersects(nPC2.Hitbox)) {
							nPC2.Transform(ModContent.NPCType<ChocolateBunny>());
						}
					}
					else if (nPC2.type == NPCID.Frog && Main.hardMode) {
						if (rectangle.Intersects(nPC2.Hitbox)) {
							nPC2.Transform(ModContent.NPCType<ChocolateFrog>());
						}
					}
					else {
						if (nPC2.type != 687 || !rectangle.Intersects(nPC2.Hitbox)) {
							continue;
						}
						nPC2.Transform(683);
						Vector2 vector9 = nPC2.Center - new Vector2(20f);
						Utils.PoofOfSmoke(vector9);
						if (Main.netMode == 2) {
							NetMessage.SendData(106, -1, -1, null, (int)vector9.X, vector9.Y);
						}
						if (!NPC.unlockedSlimeYellowSpawn) {
							NPC.unlockedSlimeYellowSpawn = true;
							if (Main.netMode == 2) {
								NetMessage.SendData(7);
							}
						}
					}
				}
			}
			return false;
		}
	}
}

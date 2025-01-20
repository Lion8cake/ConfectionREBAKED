using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Items.Banners;

namespace TheConfectionRebirth.NPCs
{
    public class Dudley : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
        }

        public override void SetDefaults()
        {
            NPC.width = 20;
            NPC.height = 14;
            NPC.damage = 48;
            NPC.defense = 22;
            NPC.lifeMax = 320;
            NPC.HitSound = SoundID.NPCHit13;
            NPC.DeathSound = SoundID.NPCDeath12;
            NPC.value = 650f;
            NPC.knockBackResist = 0f;
			NPC.aiStyle = -1;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<DudleyBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<SandConfectionUndergroundBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.Dudley")
            });
        }

		public override void AI()
		{
			Player player = Main.player[NPC.target];
			float num49 = 0.3f;
			if (NPC.ai[0] == 0f || player.dead || !player.active)
			{
				NPC.TargetClosest();
				NPC.directionY = 1;
				NPC.ai[0] = 1f;
				if (NPC.direction > 0)
				{
					NPC.spriteDirection = 1;
				}
			}
			if (NPC.ai[3] <= 300)
			{
				bool isTurning = false;
				if (NPC.velocity.Y == 0)
				{
					if (player.position.X < NPC.position.X && NPC.velocity.X > -num49)
					{
						NPC.velocity.X -= num49 / 20;
						isTurning = true;
						if (NPC.velocity.X < 0.1f)
							NPC.direction = -1;
					}
					if (player.position.X > NPC.position.X && NPC.velocity.X < num49)
					{
						NPC.velocity.X += num49 / 20;
						isTurning = true;
						if (NPC.velocity.X > 0.1f)
							NPC.direction = 1;
					}
				}
				bool flag61 = false;
				if (Main.netMode != 1)
				{
					if (NPC.ai[2] == 0f && Main.rand.NextBool(7200))
					{
						NPC.ai[2] = 2f;
						NPC.netUpdate = true;
					}
					if (!NPC.collideX && !NPC.collideY)
					{
						NPC.localAI[3] += 1f;
						if (NPC.localAI[3] > 5f)
						{
							NPC.ai[2] = 2f;
							NPC.netUpdate = true;
						}
					}
					else
					{
						NPC.localAI[3] = 0f;
					}
				}
				if (NPC.ai[2] > 0f)
				{
					NPC.ai[1] = 0f;
					NPC.ai[0] = 1f;
					NPC.directionY = 1;
					if (NPC.velocity.Y > num49)
					{
						NPC.rotation += (float)NPC.direction * 0.1f;
					}
					else
					{
						NPC.rotation = 0f;
					}
					NPC.spriteDirection = NPC.direction;
					if (!isTurning)
						NPC.velocity.X = num49 * (float)NPC.direction;
					NPC.noGravity = false;
					int num50 = (int)(NPC.Center.X + (float)(NPC.width / 2 * -NPC.direction)) / 16;
					int num51 = (int)(NPC.position.Y + (float)NPC.height + 8f) / 16;
					if (Main.tile[num50, num51] != null && !Main.tile[num50, num51].TopSlope && NPC.collideY)
					{
						NPC.ai[2] -= 1f;
					}
					num51 = (int)(NPC.position.Y + (float)NPC.height - 4f) / 16;
					num50 = (int)(NPC.Center.X + (float)(NPC.width / 2 * NPC.direction)) / 16;
					if (Main.tile[num50, num51] != null && Main.tile[num50, num51].BottomSlope)
					{
						if (!isTurning)
							NPC.direction *= -1;
					}
					if (NPC.collideX && NPC.velocity.Y == 0f)
					{
						flag61 = true;
						NPC.ai[2] = 0f;
						NPC.directionY = -1;
						NPC.ai[1] = 1f;
					}
					if (NPC.velocity.Y == 0f)
					{
						if (NPC.localAI[1] == NPC.position.X)
						{
							NPC.localAI[2] += 1f;
							if (NPC.localAI[2] > 10f)
							{
								if (!isTurning)
								{
									NPC.direction = 1;
									NPC.velocity.X = (float)NPC.direction * num49;
								}
								NPC.localAI[2] = 0f;
							}
						}
						else
						{
							NPC.localAI[2] = 0f;
							NPC.localAI[1] = NPC.position.X;
						}
					}
				}
				if (NPC.ai[2] != 0f)
				{
					return;
				}
				NPC.noGravity = true;
				if (NPC.ai[1] == 0f)
				{
					if (NPC.collideY)
					{
						NPC.ai[0] = 2f;
					}
					if (!NPC.collideY && NPC.ai[0] == 2f)
					{
						if (!isTurning)
							NPC.direction = -NPC.direction;
						NPC.ai[1] = 1f;
						NPC.ai[0] = 1f;
					}
					if (NPC.collideX)
					{
						NPC.directionY = -NPC.directionY;
						NPC.ai[1] = 1f;
					}
				}
				else
				{
					if (NPC.collideX)
					{
						NPC.ai[0] = 2f;
					}
					if (!NPC.collideX && NPC.ai[0] == 2f)
					{
						NPC.directionY = -NPC.directionY;
						NPC.ai[1] = 0f;
						NPC.ai[0] = 1f;
					}
					if (NPC.collideY)
					{
						if (!isTurning)
							NPC.direction = -NPC.direction;
						NPC.ai[1] = 0f;
					}
				}
				if (!flag61)
				{
					float num52 = NPC.rotation;
					if (NPC.directionY < 0)
					{
						if (NPC.direction < 0)
						{
							if (NPC.collideX)
							{
								NPC.rotation = 1.57f;
								NPC.spriteDirection = -1;
							}
							else if (NPC.collideY)
							{
								NPC.rotation = 3.14f;
								NPC.spriteDirection = 1;
							}
						}
						else if (NPC.collideY)
						{
							NPC.rotation = 3.14f;
							NPC.spriteDirection = -1;
						}
						else if (NPC.collideX)
						{
							NPC.rotation = 4.71f;
							NPC.spriteDirection = 1;
						}
					}
					else if (NPC.direction < 0)
					{
						if (NPC.collideY)
						{
							NPC.rotation = 0f;
							NPC.spriteDirection = -1;
						}
						else if (NPC.collideX)
						{
							NPC.rotation = 1.57f;
							NPC.spriteDirection = 1;
						}
					}
					else if (NPC.collideX)
					{
						NPC.rotation = 4.71f;
						NPC.spriteDirection = -1;
					}
					else if (NPC.collideY)
					{
						NPC.rotation = 0f;
						NPC.spriteDirection = 1;
					}
					float num53 = NPC.rotation;
					NPC.rotation = num52;
					if ((double)NPC.rotation > 6.28)
					{
						NPC.rotation -= 6.28f;
					}
					if (NPC.rotation < 0f)
					{
						NPC.rotation += 6.28f;
					}
					float num54 = Math.Abs(NPC.rotation - num53);
					float num55 = 0.1f;
					if (NPC.rotation > num53)
					{
						if ((double)num54 > 3.14)
						{
							NPC.rotation += num55;
						}
						else
						{
							NPC.rotation -= num55;
							if (NPC.rotation < num53)
							{
								NPC.rotation = num53;
							}
						}
					}
					if (NPC.rotation < num53)
					{
						if ((double)num54 > 3.14)
						{
							NPC.rotation -= num55;
						}
						else
						{
							NPC.rotation += num55;
							if (NPC.rotation > num53)
							{
								NPC.rotation = num53;
							}
						}
					}
				}
				if (!isTurning)
				{
					NPC.velocity.X = num49 * (float)NPC.direction;
				}
				NPC.velocity.Y = num49 * (float)NPC.directionY;
			}
			else
			{
				if (NPC.velocity.Y > num49)
				{
					NPC.rotation += (float)NPC.direction * 0.1f;
				}
				else
				{
					NPC.rotation = 0f;
				}
				NPC.noGravity = false;
			}

			if (NPC.Center.Y < player.Center.Y)
			{
				if (NPC.Center.X > player.Center.X - player.width / 4 && NPC.Center.X < player.Center.X + player.width / 4 && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
				{
					NPC.velocity.Y += 2;
				}
			}

			if (NPC.Center.Y > player.Center.Y && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
			{
				if (NPC.ai[3] < 300)
				{
					NPC.ai[3]++;
				}
			}
			if (NPC.ai[3] >= 300 && NPC.ai[3] < 361)
			{
				NPC.velocity.X = 0f;
				NPC.ai[3]++;
				if (NPC.ai[3] >= 360)
				{
					Vector2 newPos = player.position + new Vector2(player.width / 2, 0) - NPC.Center;
					float finalAngle = (float)Math.Sqrt(newPos.X * newPos.X + newPos.Y * newPos.Y);
					finalAngle = 12f / finalAngle;
					newPos.X *= finalAngle;
					newPos.Y *= finalAngle * (NPC.gravity + (player.Center.Y - NPC.Center.Y < -30 ? 1f : 3f));
					NPC.velocity = newPos;
				}
			}

			if (NPC.ai[3] >= 361 && NPC.collideY)
			{
				NPC.velocity.X = 0f;
				NPC.ai[3] = 0f;
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TaffyApple>(), 50));
            npcLoot.Add(ItemDropRule.Food(ModContent.ItemType<Brownie>(), 125));
        }

		public override void HitEffect(NPC.HitInfo hit)
        {
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			for (int i = 0; i < (NPC.life <= 0 ? 26 : 5); i++)
			{
				int dustID = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<ChocolateBlood>(), 2 * hit.HitDirection, -2f);
				if (Main.rand.NextBool(2))
				{
					Main.dust[dustID].noGravity = true;
					Main.dust[dustID].scale = 1.2f * NPC.scale;
				}
				else
				{
					Main.dust[dustID].scale = 0.7f * NPC.scale;
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			//if (spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && !spawnInfo.AnyInvasionActive() && Main.hardMode && spawnInfo.Player.ZoneDesert && (spawnInfo.Player.ZoneRockLayerHeight || spawnInfo.Player.ZoneUnderworldHeight)) {
			//	return 1f;
			//}
			return 0f;
		}

        public override void FindFrame(int frameHeight)
        {
			if (NPC.velocity == Vector2.Zero && NPC.ai[3] >= 300)
			{
				NPC.frame.Y = frameHeight * 4;
			}
			else
			{
				if (++NPC.frameCounter > 8)
				{
					NPC.frameCounter = 0;
					int count = NPC.frame.Y / frameHeight;
					count++;
					if (count > 3)
					{
						count = 0;
					}
					NPC.frame.Y = count * frameHeight;
				}
			}
		}
    }
}

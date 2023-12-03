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
using TheConfectionRebirth.Pets.DudlingPet;

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
            NPC.width = 26;
            NPC.height = 24;
            NPC.damage = 48;
            NPC.defense = 22;
            NPC.lifeMax = 320;
            NPC.HitSound = SoundID.NPCHit13;
            NPC.DeathSound = SoundID.NPCDeath12;
            NPC.value = 650f;
            NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 67;//-1;
			AIType = 359;
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

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TaffyApple>(), 50));
            npcLoot.Add(ItemDropRule.Food(ModContent.ItemType<Brownie>(), 125));
        }

		/*public override void AI() {
			if (NPC.ai[3] != 0f) {
				NPC.scale = NPC.ai[3];
				int num46 = (int)(12f * NPC.scale);
				int num48 = (int)(12f * NPC.scale);
				if (num46 != NPC.width) {
					NPC.position.X = NPC.position.X + (float)(NPC.width / 2) - (float)num46 - 2f;
					NPC.width = num46;
				}
				if (num48 != NPC.height) {
					NPC.position.Y = NPC.position.Y + (float)NPC.height - (float)num48;
					NPC.height = num48;
				}
			}
			if (NPC.ai[3] == 0f && Main.netMode != 1) {
				NPC.ai[3] = (float)Main.rand.Next(80, 111) * 0.01f;
				NPC.netUpdate = true;
			}


			float num49 = 0.3f;
			if (NPC.ai[0] == 0f) {
				NPC.TargetClosest();
				NPC.directionY = 1;
				NPC.ai[0] = 1f;
				if (NPC.direction > 0) {
					NPC.spriteDirection = 1;
				}
				if (NPC.position.X > Main.player[NPC.target].position.X) {
					NPC.direction = -1;
				}
				else {
					NPC.direction = 1;
				}
			}
			if (Main.rand.NextBool(750)) {
				if (NPC.position.X > Main.player[NPC.target].position.X) {
					NPC.direction = -1;
				}
				else {
					NPC.direction = 1;
				}
			}
			bool flag61 = false;
			if (Main.netMode != 1) {
				if (NPC.ai[2] == 0f && Main.rand.Next(7200) == 0) {
					NPC.ai[2] = 2f;
					NPC.netUpdate = true;
				}
				if (!NPC.collideX && !NPC.collideY) {
					NPC.localAI[3] += 1f;
					if (NPC.localAI[3] > 5f) {
						NPC.ai[2] = 2f;
						NPC.netUpdate = true;
					}
				}
				else {
					NPC.localAI[3] = 0f;
				}
			}
			if (NPC.ai[2] > 0f) {
				NPC.ai[1] = 0f;
				NPC.ai[0] = 1f;
				NPC.directionY = 1;
				if (NPC.velocity.Y > num49) {
					NPC.rotation += (float)NPC.direction * 0.1f;
				}
				else {
					NPC.rotation = 0f;
				}
				NPC.spriteDirection = NPC.direction;
				NPC.velocity.X = num49 * (float)NPC.direction;
				NPC.noGravity = false;
				int num50 = (int)(NPC.Center.X + (float)(NPC.width / 2 * -NPC.direction)) / 16;
				int num51 = (int)(NPC.position.Y + (float)NPC.height + 8f) / 16;
				if (Main.tile[num50, num51] != null && !Main.tile[num50, num51].TopSlope && NPC.collideY) {
					NPC.ai[2] -= 1f;
				}
				num51 = (int)(NPC.position.Y + (float)NPC.height - 4f) / 16;
				num50 = (int)(NPC.Center.X + (float)(NPC.width / 2 * NPC.direction)) / 16;
				if (Main.tile[num50, num51] != null && Main.tile[num50, num51].BottomSlope) {
					NPC.direction *= -1;
				}
				if (NPC.collideX && NPC.velocity.Y == 0f) {
					flag61 = true;
					NPC.ai[2] = 0f;
					NPC.directionY = -1;
					NPC.ai[1] = 1f;
				}
				if (NPC.velocity.Y == 0f) {
					if (NPC.localAI[1] == NPC.position.X) {
						NPC.localAI[2] += 1f;
						if (NPC.localAI[2] > 10f) {
							NPC.direction = 1;
							NPC.velocity.X = (float)NPC.direction * num49;
							NPC.localAI[2] = 0f;
						}
					}
					else {
						NPC.localAI[2] = 0f;
						NPC.localAI[1] = NPC.position.X;
					}
				}
			}
			if (NPC.ai[2] != 0f) {
				return;
			}
			NPC.noGravity = true;
			if (NPC.ai[1] == 0f) {
				if (NPC.collideY) {
					NPC.ai[0] = 2f;
				}
				if (!NPC.collideY && NPC.ai[0] == 2f) {
					NPC.direction = -NPC.direction;
					NPC.ai[1] = 1f;
					NPC.ai[0] = 1f;
				}
				if (NPC.collideX) {
					NPC.directionY = -NPC.directionY;
					NPC.ai[1] = 1f;
				}
			}
			else {
				if (NPC.collideX) {
					NPC.ai[0] = 2f;
				}
				if (!NPC.collideX && NPC.ai[0] == 2f) {
					NPC.directionY = -NPC.directionY;
					NPC.ai[1] = 0f;
					NPC.ai[0] = 1f;
				}
				if (NPC.collideY) {
					NPC.direction = -NPC.direction;
					NPC.ai[1] = 0f;
				}
			}
			if (!flag61) {
				float num52 = NPC.rotation;
				if (NPC.directionY < 0) {
					if (NPC.direction < 0) {
						if (NPC.collideX) {
							NPC.rotation = 1.57f;
							NPC.spriteDirection = -1;
							Main.NewText("1");
						}
						else if (NPC.collideY) {
							NPC.rotation = 3.14f;
							NPC.spriteDirection = 1;
							Main.NewText("2");
						}
					}
					else if (NPC.collideY) {
						NPC.rotation = 3.14f;
						NPC.spriteDirection = -1;
						Main.NewText("3");
					}
					else if (NPC.collideX) {
						NPC.rotation = 4.71f;
						NPC.spriteDirection = 1;
						Main.NewText("4");
					}
				}
				else if (NPC.direction < 0) {
					if (NPC.collideY) {
						NPC.rotation = 0f;
						NPC.spriteDirection = -1;
						Main.NewText("5");
					}
					else if (NPC.collideX) {
						NPC.rotation = 1.57f;
						NPC.spriteDirection = 1;
						Main.NewText("6");
					}
				}
				else if (NPC.collideX) {
					NPC.rotation = 4.71f;
					NPC.spriteDirection = -1;
					Main.NewText("7");
				}
				else if (NPC.collideY) {
					NPC.rotation = 0f;
					NPC.spriteDirection = 1;
					Main.NewText("8");
				}
				float num53 = NPC.rotation;
				NPC.rotation = num52;
				if ((double)NPC.rotation > 6.28) {
					NPC.rotation -= 6.28f;
				}
				if (NPC.rotation < 0f) {
					NPC.rotation += 6.28f;
				}
				float num54 = Math.Abs(NPC.rotation - num53);
				float num55 = 0.1f;
				if (NPC.rotation > num53) {
					if ((double)num54 > 3.14) {
						NPC.rotation += num55;
					}
					else {
						NPC.rotation -= num55;
						if (NPC.rotation < num53) {
							NPC.rotation = num53;
						}
					}
				}
				if (NPC.rotation < num53) {
					if ((double)num54 > 3.14) {
						NPC.rotation -= num55;
					}
					else {
						NPC.rotation += num55;
						if (NPC.rotation > num53) {
							NPC.rotation = num53;
						}
					}
				}
			}
			NPC.velocity.X = num49 * (float)NPC.direction;
			NPC.velocity.Y = num49 * (float)NPC.directionY;
		}*/

		public override void HitEffect(NPC.HitInfo hit)
        {
            int num = NPC.life > 0 ? 1 : 5;
            for (int k = 0; k < num; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<CritterBlood>());
            }
        }
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && !spawnInfo.AnyInvasionActive() && Main.hardMode && spawnInfo.Player.ZoneDesert && (spawnInfo.Player.ZoneRockLayerHeight || spawnInfo.Player.ZoneUnderworldHeight)) {
				return 1f;
			}
			return 0f;
		}

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 1.0;
			/*if (NPC.velocity.X != 0f || NPC.velocity.Y != 0f) {
				NPC.frame.Y = frameHeight * 4;
			}
			else {*/
				if (NPC.frameCounter > 8.0) {
					NPC.frameCounter = 0.0;
					NPC.frame.Y += frameHeight;
					if (NPC.frame.Y > frameHeight * 3) {
						NPC.frame.Y = 0;
					}
				}
			//}
        }
    }
}

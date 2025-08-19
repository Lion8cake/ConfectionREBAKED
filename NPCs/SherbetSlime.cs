using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Newtonsoft.Json.Linq;
using System;
using System.CommandLine.Invocation;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items.Banners;
using TheConfectionRebirth.Items.Placeable;

namespace TheConfectionRebirth.NPCs
{
    public class SherbetSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers
            {
                Position = new(2f, 0f),
                PortraitPositionXOverride = 0f
            });
        }

        public override void SetDefaults()
        {
			NPC.width = 66;
			NPC.height = 46;
			NPC.aiStyle = 1;
			NPC.damage = 85;
			NPC.defense = 22;
			NPC.lifeMax = 420;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = Item.buyPrice(0, 0, 20);
			NPC.knockBackResist = 0.3f;
			NPC.rarity = 1;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<SherbetSlimeBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionBiome>().Type };
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Rain,

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.SherbetSlime")
            });
        }

		public override bool PreAI()
		{
			//color
			float num23 = (float)TheConfectionRebirth.SherbR / 255f;
			float num29 = (float)TheConfectionRebirth.SherbG / 255f;
			float num30 = (float)TheConfectionRebirth.SherbB / 255f;
			num23 *= 1f;
			num29 *= 1f;
			num30 *= 1f;
			Lighting.AddLight((int)((NPC.position.X + (float)(NPC.width / 2)) / 16f), (int)((NPC.position.Y + (float)(NPC.height / 2)) / 16f), num23, num29, num30);

			bool flag = false;
			if (!Main.dayTime || NPC.life != NPC.lifeMax || (double)NPC.position.Y > Main.worldSurface * 16.0 || Main.slimeRain)
			{
				flag = true;
			}
			flag = true;
			NPC.ai[0] += 2f;

			if (NPC.ai[2] > 1f)
			{
				NPC.ai[2] -= 1f;
			}
			if (NPC.wet)
			{
				if (NPC.collideY)
				{
					NPC.velocity.Y = -2f;
				}
				if (NPC.velocity.Y < 0f && NPC.ai[3] == NPC.position.X)
				{
					NPC.direction *= -1;
					NPC.ai[2] = 200f;
				}
				if (NPC.velocity.Y > 0f)
				{
					NPC.ai[3] = NPC.position.X;
				}
				if (NPC.velocity.Y > 2f)
				{
					NPC.velocity.Y *= 0.9f;
				}
				NPC.velocity.Y -= 0.5f;
				if (NPC.velocity.Y < -4f)
				{
					NPC.velocity.Y = -4f;
				}
				if (NPC.ai[2] == 1f && flag)
				{
					NPC.TargetClosest();
				}
			}
			NPC.aiAction = 0;
			if (NPC.ai[2] == 0f)
			{
				NPC.ai[0] = -100f;
				NPC.ai[2] = 1f;
				NPC.TargetClosest();
			}
			if (NPC.velocity.Y == 0f)
			{
				if (NPC.collideY && NPC.oldVelocity.Y != 0f && Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
				{
					NPC.position.X -= NPC.velocity.X + (float)NPC.direction;
				}
				if (NPC.ai[3] == NPC.position.X)
				{
					NPC.direction *= -1;
					NPC.ai[2] = 200f;
				}
				NPC.ai[3] = 0f;
				NPC.velocity.X *= 0.8f;
				if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
				{
					NPC.velocity.X = 0f;
				}
				if (flag)
				{
					NPC.ai[0] += 1f;
				}
				NPC.ai[0] += 1f;
				
				float num27 = -1000f;
				
				int num28 = 0;
				if (NPC.ai[0] >= 0f)
				{
					num28 = 1;
				}
				if (NPC.ai[0] >= num27 && NPC.ai[0] <= num27 * 0.5f)
				{
					num28 = 2;
				}
				if (NPC.ai[0] >= num27 * 2f && NPC.ai[0] <= num27 * 1.5f)
				{
					num28 = 3;
				}
				if (num28 > 0)
				{
					NPC.netUpdate = true;
					if (flag && NPC.ai[2] == 1f)
					{
						NPC.TargetClosest();
					}
					if (num28 == 3)
					{
						NPC.velocity.Y = -8f;

						NPC.velocity.X += 3 * NPC.direction;

						NPC.ai[0] = -200f;
						NPC.ai[3] = NPC.position.X;
					}
					else
					{
						NPC.velocity.Y = -6f;
						NPC.velocity.X += 2 * NPC.direction;
						NPC.ai[0] = -120f;
						if (num28 == 1)
						{
							NPC.ai[0] += num27;
						}
						else
						{
							NPC.ai[0] += num27 * 2f;
						}
					}
				}
				else if (NPC.ai[0] >= -30f)
				{
					NPC.aiAction = 1;
				}
			}
			else if (NPC.target < 255 && ((NPC.direction == 1 && NPC.velocity.X < 3f) || (NPC.direction == -1 && NPC.velocity.X > -3f)))
			{
				if (NPC.collideX && Math.Abs(NPC.velocity.X) == 0.2f)
				{
					NPC.position.X -= 1.4f * (float)NPC.direction;
				}
				if (NPC.collideY && NPC.oldVelocity.Y != 0f && Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
				{
					NPC.position.X -= NPC.velocity.X + (float)NPC.direction;
				}
				if ((NPC.direction == -1 && (double)NPC.velocity.X < 0.01) || (NPC.direction == 1 && (double)NPC.velocity.X > -0.01))
				{
					NPC.velocity.X += 0.2f * (float)NPC.direction;
				}
				else
				{
					NPC.velocity.X *= 0.93f;
				}
			}
			return false;
		}

		public override void FindFrame(int frameHeight)
		{
			int slimeHopingSpeed = 0;
			if (NPC.aiAction == 0)
			{
				slimeHopingSpeed = ((NPC.velocity.Y < 0f) ? 2 : ((NPC.velocity.Y > 0f) ? 3 : ((NPC.velocity.X != 0f) ? 1 : 0)));
			}
			else if (NPC.aiAction == 1)
			{
				slimeHopingSpeed = 4;
			}

			NPC.frameCounter += 1.0;
			if (slimeHopingSpeed > 0)
			{
				NPC.frameCounter += 1.0;
			}
			if (slimeHopingSpeed == 4)
			{
				NPC.frameCounter += 1.0;
			}
			if (NPC.frameCounter >= 8.0)
			{
				NPC.frame.Y += frameHeight;
				NPC.frameCounter = 0.0;
			}
			if (NPC.frame.Y >= frameHeight * Main.npcFrameCount[Type])
			{
				NPC.frame.Y = 0;
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Gel, maximumDropped: 20));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SherbetBricks>(), 1, 30, 60));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
			return ConfectionGlobalNPC.SpawnNPC_ConfectionNPC(spawnInfo, Type);
		}

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                return;
            }

			if (NPC.life > 0)
			{
				for (int i = 0; (double)i < hit.Damage / (double)NPC.lifeMax * 100.0; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.TintableDust, hit.HitDirection, -1f, NPC.alpha, TheConfectionRebirth.SherbertColor);
				}
			}
			else
			{
				for (int i = 0; i < 50; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 4, 2 * hit.HitDirection, -2f, NPC.alpha, TheConfectionRebirth.SherbertColor);
				}
			}
		}
    }
}
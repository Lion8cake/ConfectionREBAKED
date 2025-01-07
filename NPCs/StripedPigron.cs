using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items.Banners;

namespace TheConfectionRebirth.NPCs
{
    public class StripedPigron : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 14;
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers
            {
                Position = new(10f, 5f),
                PortraitPositionXOverride = 0f,
                PortraitPositionYOverride = -12f
            });
        }

        public override void SetDefaults()
        {
			NPC.width = 44;
			NPC.height = 36;
            NPC.aiStyle = -1;
			NPC.damage = 70;
			NPC.defense = 16;
			NPC.lifeMax = 210;
			NPC.HitSound = SoundID.NPCHit27;
			NPC.DeathSound = SoundID.NPCDeath30;
			NPC.knockBackResist = 0.5f;
			NPC.value = 2000f;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<StripedPigronBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<IceConfectionUndergroundBiome>().Type };
		}

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.StripedPigron")
            });
        }

		public override void AI()
		{
			if (!(NPC.shimmerTransparency > 0f))
			{
				if (Main.rand.NextBool(600))
				{
					SoundEngine.PlaySound(Main.rand.NextBool(3) ? SoundID.Zombie39 : Main.rand.NextBool(2) ? SoundID.Zombie40 : SoundID.Zombie38, NPC.position);
				}
			}

			if (Main.rand.NextBool(1000))
			{
				SoundEngine.PlaySound(SoundID.Zombie9, NPC.position);
			}
			NPC.noGravity = true;
			if (!NPC.noTileCollide)
			{
				if (NPC.collideX)
				{
					NPC.velocity.X = NPC.oldVelocity.X * -0.5f;
					if (NPC.direction == -1 && NPC.velocity.X > 0f && NPC.velocity.X < 2f)
					{
						NPC.velocity.X = 2f;
					}
					if (NPC.direction == 1 && NPC.velocity.X < 0f && NPC.velocity.X > -2f)
					{
						NPC.velocity.X = -2f;
					}
				}
				if (NPC.collideY)
				{
					NPC.velocity.Y = NPC.oldVelocity.Y * -0.5f;
					if (NPC.velocity.Y > 0f && NPC.velocity.Y < 1f)
					{
						NPC.velocity.Y = 1f;
					}
					if (NPC.velocity.Y < 0f && NPC.velocity.Y > -1f)
					{
						NPC.velocity.Y = -1f;
					}
				}
			}
			NPC.TargetClosest();
			
			if (Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
			{
				if (NPC.ai[1] > 0f && !Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
				{
					NPC.ai[1] = 0f;
					NPC.ai[0] = 0f;
					NPC.netUpdate = true;
				}
			}
			else if (NPC.ai[1] == 0f)
			{
				NPC.ai[0] += 1f;
			}
			if (NPC.ai[0] >= 300f)
			{
				NPC.ai[1] = 1f;
				NPC.ai[0] = 0f;
				NPC.netUpdate = true;
			}
			if (NPC.ai[1] == 0f)
			{
				NPC.alpha = 0;
				NPC.noTileCollide = false;
			}
			else
			{
				NPC.wet = false;
				NPC.alpha = 200;
				NPC.noTileCollide = true;
			}
			NPC.rotation = NPC.velocity.Y * 0.1f * (float)NPC.direction;
			NPC.TargetClosest();
			if (NPC.direction == -1 && NPC.velocity.X > -4f && NPC.position.X > Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width)
			{
				NPC.velocity.X -= 0.08f;
				if (NPC.velocity.X > 4f)
				{
					NPC.velocity.X -= 0.04f;
				}
				else if (NPC.velocity.X > 0f)
				{
					NPC.velocity.X -= 0.2f;
				}
				if (NPC.velocity.X < -4f)
				{
					NPC.velocity.X = -4f;
				}
			}
			else if (NPC.direction == 1 && NPC.velocity.X < 4f && NPC.position.X + (float)NPC.width < Main.player[NPC.target].position.X)
			{
				NPC.velocity.X += 0.08f;
				if (NPC.velocity.X < -4f)
				{
					NPC.velocity.X += 0.04f;
				}
				else if (NPC.velocity.X < 0f)
				{
					NPC.velocity.X += 0.2f;
				}
				if (NPC.velocity.X > 4f)
				{
					NPC.velocity.X = 4f;
				}
			}
			if (NPC.directionY == -1 && (double)NPC.velocity.Y > -2.5 && NPC.position.Y > Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height)
			{
				NPC.velocity.Y -= 0.1f;
				if ((double)NPC.velocity.Y > 2.5)
				{
					NPC.velocity.Y -= 0.05f;
				}
				else if (NPC.velocity.Y > 0f)
				{
					NPC.velocity.Y -= 0.15f;
				}
				if ((double)NPC.velocity.Y < -2.5)
				{
					NPC.velocity.Y = -2.5f;
				}
			}
			else if (NPC.directionY == 1 && (double)NPC.velocity.Y < 2.5 && NPC.position.Y + (float)NPC.height < Main.player[NPC.target].position.Y)
			{
				NPC.velocity.Y += 0.1f;
				if ((double)NPC.velocity.Y < -2.5)
				{
					NPC.velocity.Y += 0.05f;
				}
				else if (NPC.velocity.Y < 0f)
				{
					NPC.velocity.Y += 0.15f;
				}
				if ((double)NPC.velocity.Y > 2.5)
				{
					NPC.velocity.Y = 2.5f;
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Food(ItemID.Bacon, 15, 1, 1));
            npcLoot.Add(new ItemDropWithConditionRule(ItemID.KitePigron, 25, 1, 1, new Conditions.WindyEnoughForKiteDrops(), 1));
            npcLoot.Add(ItemDropRule.Common(ItemID.PigronMinecart, 100, 1, 1));
			npcLoot.Add(new ItemDropWithConditionRule(ItemID.HamBat, 10, 1, 1, new Conditions.DontStarveIsUp(), 1));
			npcLoot.Add(new ItemDropWithConditionRule(ItemID.HamBat, 25, 1, 1, new Conditions.DontStarveIsNotUp(), 1));
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.spriteDirection = NPC.direction;
			NPC.frameCounter += 1.0;
			if (NPC.frameCounter >= 4.0)
			{
				NPC.frame.Y += frameHeight;
				NPC.frameCounter = 0.0;
			}
			if (NPC.frame.Y >= frameHeight * 14)
			{
				NPC.frame.Y = 0;
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
			//if (spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && !spawnInfo.AnyInvasionActive() && Main.hardMode && (spawnInfo.Player.ZoneRockLayerHeight || spawnInfo.Player.ZoneUnderworldHeight) && spawnInfo.Player.ZoneSnow) {
            //    return 0.05f;
            //}
            return 0f;
        }

		public override void HitEffect(NPC.HitInfo hit)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                return;
            }

            if (NPC.life > 0)
            {
                for (int i = 0; (double)i < hit.Damage / (double)NPC.lifeMax * 50.0; i++)
                {
                    int dustID = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Cloud, 0f, 0f, 0, default(Color), 1.5f);
                    Dust dust = Main.dust[dustID];
                    dust.velocity *= 1.5f;
                    dust.noGravity = true;
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    int dustID = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Cloud, 0f, 0f, 0, default(Color), 1.5f);
                    Dust dust = Main.dust[dustID];
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                }
                for (int i = 0; i < 4; i++)
                {
                    int type = 11 + i;
                    if (type > 13)
                    {
                        type = Main.rand.Next(11, 14);
                    }
                    int goreID = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X, NPC.position.Y + (float)(NPC.height / 2) - 10f), new Vector2((float)hit.HitDirection, 0f), type, NPC.scale);
                    Gore gore = Main.gore[goreID];
                    gore.velocity *= 0.3f;
                }
            }
		}
    }
}

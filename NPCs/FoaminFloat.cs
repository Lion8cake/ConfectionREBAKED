using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Items.Banners;
using Terraria.DataStructures;
using System.IO;
using Newtonsoft.Json.Linq;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria.Audio;
using TheConfectionRebirth.Projectiles;
using TheConfectionRebirth.Buffs;
using TheConfectionRebirth.Items.Armor.WonkyOutfit;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.NPCs
{
    public class FoaminFloat : ModNPC
    {
		public override void SetStaticDefaults()
        {
			Main.npcFrameCount[Type] = 3;
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers
            {
                Position = new(0f, 6f),
                PortraitPositionYOverride = 0f
            });
        }

		public override void SetDefaults()
        {
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.width = 48;
			NPC.height = 36;
			NPC.aiStyle = -1;
			NPC.damage = 45;
			NPC.defense = 8;
			NPC.lifeMax = 140;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 500f;
			NPC.knockBackResist = 0.7f;
			Banner = Type;
            BannerItem = ModContent.ItemType<FoaminFloatBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionUndergroundBiome>().Type };
		}

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.FoaminFloat")
            });
        }

		public override bool? CanFallThroughPlatforms()
		{
            return true;
		}

		public override void AI()
		{
			if (NPC.ai[3] < 200)
			{
				bool flag20 = false;
				if (NPC.justHit)
				{
					NPC.ai[2] = 0f;
				}
				if (NPC.ai[2] >= 0f)
				{
					int num827 = 16;
					bool flag22 = false;
					bool flag23 = false;
					if (NPC.position.X > NPC.ai[0] - (float)num827 && NPC.position.X < NPC.ai[0] + (float)num827)
					{
						flag22 = true;
					}
					else if ((NPC.velocity.X < 0f && NPC.direction > 0) || (NPC.velocity.X > 0f && NPC.direction < 0))
					{
						flag22 = true;
					}
					num827 += 24;
					if (NPC.position.Y > NPC.ai[1] - (float)num827 && NPC.position.Y < NPC.ai[1] + (float)num827)
					{
						flag23 = true;
					}
					if (flag22 && flag23)
					{
						NPC.ai[2] += 1f;
						if (NPC.ai[2] >= 30f && num827 == 16)
						{
							flag20 = true;
						}
						if (NPC.ai[2] >= 60f)
						{
							NPC.ai[2] = -200f;
							NPC.direction *= -1;
							NPC.velocity.X *= -1f;
							NPC.collideX = false;
						}
					}
					else
					{
						NPC.ai[0] = NPC.position.X;
						NPC.ai[1] = NPC.position.Y;
						NPC.ai[2] = 0f;
					}
					NPC.TargetClosest();
				}
				else
				{
					NPC.ai[2] += 1f;
					if (Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) > NPC.position.X + (float)(NPC.width / 2))
					{
						NPC.direction = -1;
					}
					else
					{
						NPC.direction = 1;
					}
				}
				int num828 = (int)((NPC.position.X + (float)(NPC.width / 2)) / 16f) + NPC.direction * 2;
				int num831 = (int)((NPC.position.Y + (float)NPC.height) / 16f);
				bool flag25 = true;
				int num832 = 3;

				NPC.rotation = NPC.velocity.X * 0.1f;
				num832 = ((!(Main.player[NPC.target].Center.Y < NPC.Center.Y)) ? 6 : 12);
				if (Main.netMode != 1 && !NPC.confused)
				{
					NPC.ai[3] += 1f;
					if (NPC.justHit)
					{
						NPC.ai[3] = -45f;
						NPC.localAI[1] = 0f;
					}
					if (Main.netMode != NetmodeID.MultiplayerClient && NPC.ai[3] >= (float)(60 + Main.rand.Next(60)))
					{
						NPC.ai[3] = 0f;
						if (Main.rand.NextBool(4) && Main.player[NPC.target].active && Main.player[NPC.target].Center.Distance(NPC.Center) < 600f)
						{
							NPC.ai[3] = 200f;
							NPC.ai[0] = 0f;
							NPC.ai[1] = 0f;
						}
						if (Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
						{
							float num854 = 10f;
							Vector2 vector248 = new(NPC.position.X + (float)NPC.width * 0.5f - 4f, NPC.position.Y + (float)NPC.height * 0.7f);
							float num855 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector248.X;
							float num856 = Math.Abs(num855) * 0.1f;
							float num857 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector248.Y - num856;
							num855 += (float)Main.rand.Next(-10, 11);
							num857 += (float)Main.rand.Next(-30, 21);
							float num858 = (float)Math.Sqrt(num855 * num855 + num857 * num857);
							float num859 = num858;
							num858 = num854 / num858;
							num855 *= num858;
							num857 *= num858;
							int num860 = 45 / 2;
							int num861 = ModContent.ProjectileType<RootbeerSpray>();
							int num862 = Projectile.NewProjectile(NPC.GetSource_FromAI(), vector248.X, vector248.Y, num855, num857, num861, num860, 0f, Main.myPlayer);
						}
					}
				}

				if (NPC.position.Y + (float)NPC.height > Main.player[NPC.target].position.Y)
				{
					for (int num865 = num831; num865 < num831 + num832; num865++)
					{
						if ((Main.tile[num828, num865].HasUnactuatedTile && Main.tileSolid[Main.tile[num828, num865].TileType]) || Main.tile[num828, num865].LiquidAmount > 0)
						{
							flag25 = false;
							break;
						}
					}
				}
				if (Main.player[NPC.target].npcTypeNoAggro[Type])
				{
					bool flag27 = false;
					for (int num866 = num831; num866 < num831 + num832 - 2; num866++)
					{
						if ((Main.tile[num828, num866].HasUnactuatedTile && Main.tileSolid[Main.tile[num828, num866].TileType]) || Main.tile[num828, num866].LiquidAmount > 0)
						{
							flag27 = true;
							break;
						}
					}
					NPC.directionY = (!flag27).ToDirectionInt();
				}
				if (flag20)
				{
					flag25 = true;
				}
				if (flag25)
				{
					NPC.velocity.Y += 0.1f;
					if (NPC.velocity.Y > 3f)
					{
						NPC.velocity.Y = 3f;
					}
				}
				else
				{
					if (NPC.directionY < 0 && NPC.velocity.Y > 0f)
					{
						NPC.velocity.Y -= 0.1f;
					}
					if (NPC.velocity.Y < -4f)
					{
						NPC.velocity.Y = -4f;
					}
				}
				if (NPC.collideX)
				{
					NPC.velocity.X = NPC.oldVelocity.X * -0.4f;
					if (NPC.direction == -1 && NPC.velocity.X > 0f && NPC.velocity.X < 1f)
					{
						NPC.velocity.X = 1f;
					}
					if (NPC.direction == 1 && NPC.velocity.X < 0f && NPC.velocity.X > -1f)
					{
						NPC.velocity.X = -1f;
					}
				}
				if (NPC.collideY)
				{
					NPC.velocity.Y = NPC.oldVelocity.Y * -0.25f;
					if (NPC.velocity.Y > 0f && NPC.velocity.Y < 1f)
					{
						NPC.velocity.Y = 1f;
					}
					if (NPC.velocity.Y < 0f && NPC.velocity.Y > -1f)
					{
						NPC.velocity.Y = -1f;
					}
				}
				float num868 = 2f;
				if (NPC.direction == -1 && NPC.velocity.X > 0f - num868)
				{
					NPC.velocity.X -= 0.1f;
					if (NPC.velocity.X > num868)
					{
						NPC.velocity.X -= 0.1f;
					}
					else if (NPC.velocity.X > 0f)
					{
						NPC.velocity.X += 0.05f;
					}
					if (NPC.velocity.X < 0f - num868)
					{
						NPC.velocity.X = 0f - num868;
					}
				}
				else if (NPC.direction == 1 && NPC.velocity.X < num868)
				{
					NPC.velocity.X += 0.1f;
					if (NPC.velocity.X < 0f - num868)
					{
						NPC.velocity.X += 0.1f;
					}
					else if (NPC.velocity.X < 0f)
					{
						NPC.velocity.X -= 0.05f;
					}
					if (NPC.velocity.X > num868)
					{
						NPC.velocity.X = num868;
					}
				}
				num868 = 1.5f;
				if (NPC.directionY == -1 && NPC.velocity.Y > 0f - num868)
				{
					NPC.velocity.Y -= 0.04f;
					if (NPC.velocity.Y > num868)
					{
						NPC.velocity.Y -= 0.05f;
					}
					else if (NPC.velocity.Y > 0f)
					{
						NPC.velocity.Y += 0.03f;
					}
					if (NPC.velocity.Y < 0f - num868)
					{
						NPC.velocity.Y = 0f - num868;
					}
				}
				else if (NPC.directionY == 1 && NPC.velocity.Y < num868)
				{
					NPC.velocity.Y += 0.04f;
					if (NPC.velocity.Y < 0f - num868)
					{
						NPC.velocity.Y += 0.05f;
					}
					else if (NPC.velocity.Y < 0f)
					{
						NPC.velocity.Y -= 0.03f;
					}
					if (NPC.velocity.Y > num868)
					{
						NPC.velocity.Y = num868;
					}
				}
			}
			else
			{
				if (NPC.ai[0] != 2f)
					NPC.ai[3]++;
				Player player = Main.player[NPC.target];
				if (NPC.ai[0] == 0f)
				{
					if (!player.active || player.dead)
					{
						NPC.TargetClosest();
					}
					NPC.ai[0] = 1f;
				}
				else if (NPC.ai[0] == 1f)
				{
					NPC.rotation += (float)NPC.direction * 0.3f;
					Vector2 length = player.Center - NPC.Center;
					length = player.Top - NPC.Center;
					float length2 = length.Length();
					float speed = 5.5f;
					speed += length2 / 100f;
					int maxVel = 50;
					length.Normalize();
					length *= speed;
					NPC.velocity = (NPC.velocity * (float)(maxVel - 1) + length) / (float)maxVel;
					if (length2 < 40f && player.active && !player.dead)
					{
						bool isSuffocating = true;
						for (int i = 0; i < Main.maxNPCs; i++)
						{
							NPC npc = Main.npc[i];
							if (npc.active && npc.type == Type && npc.ai[0] == 2f && npc.target == NPC.target)
							{
								isSuffocating = false;
								break;
							}
						}
						if (isSuffocating)
						{
							NPC.Center = player.Top;
							NPC.velocity = Vector2.Zero;
							NPC.ai[0] = 2f;
							NPC.ai[1] = 0f;
							NPC.netUpdate = true;
							player.GetModPlayer<ConfectionPlayer>().shakenOffFoamin = false;
							player.AddBuff(ModContent.BuffType<FoaminSuffocation>(), 1);
						}
					}

				}
				else if (NPC.ai[0] == 2f)
				{
					if (!player.active || player.dead || player.GetModPlayer<ConfectionPlayer>().shakenOffFoamin)
					{
						NPC.ai[0] = 0f;
						NPC.ai[1] = 0f;
						NPC.netUpdate = true;
						NPC.ai[3] = 380;
						NPC.velocity.X = 7.5f * NPC.direction;
					}
					else
					{
						NPC.Center = ((player.gravDir == 1f) ? player.Top : player.Bottom) + new Vector2((float)(player.direction * 4), 0f);
						NPC.gfxOffY = player.gfxOffY;
						NPC.velocity = Vector2.Zero;
						if (!player.creativeGodMode)
						{
							player.AddBuff(ModContent.BuffType<FoaminSuffocation>(), 2);
						}
						if (player.GetModPlayer<ConfectionPlayer>().shaken)
						{
							NPC.rotation = 0.75f * player.direction;
						}
					}
				}
				NPC.hide = NPC.ai[0] == 2f;
				if (NPC.ai[0] == 2)
				{
					if (NPC.rotation != 0)
					{
						float rotIncriment = 0.125f;
						if (NPC.rotation > 0)
						{
							NPC.rotation -= rotIncriment;
							if (NPC.rotation <= 0)
							{
								NPC.rotation = 0;
							}
						}
						else if (NPC.rotation < 0)
						{
							NPC.rotation += rotIncriment;
							if (NPC.rotation >= 0)
							{
								NPC.rotation = 0;
							}
						}
					}
				}
				else
				{
					NPC.rotation = NPC.velocity.X * 0.1f;
				}
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];
					if (i != NPC.whoAmI && npc.active && npc.type == Type && Math.Abs(NPC.position.X - npc.position.X) + Math.Abs(NPC.position.Y - npc.position.Y) < (float)NPC.width)
					{
						if (NPC.position.X < npc.position.X)
						{
							NPC.velocity.X -= 0.05f;
						}
						else
						{
							NPC.velocity.X += 0.05f;
						}
						if (NPC.position.Y < npc.position.Y)
						{
							NPC.velocity.Y -= 0.05f;
						}
						else
						{
							NPC.velocity.Y += 0.05f;
						}
					}
				}
				if (NPC.ai[3] >= 380)
				{
					NPC.ai[3] = 0;
					NPC.ai[0] = 0f;
					NPC.ai[1] = 0f;
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CcretTicket>(), 100));
		}

		public override void FindFrame(int frameHeight)
		{
			bool isSuffocating = NPC.ai[3] >= 200 && NPC.ai[0] == 2;
			if (++NPC.frameCounter > (isSuffocating ? 6 : 24))
			{
				NPC.frameCounter = 0;
				int frame = NPC.frame.Y / frameHeight;
				if (++frame >= Main.npcFrameCount[Type])
				{
					frame = 0;
				}
				NPC.frame.Y = frame * frameHeight;
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
			return ConfectionGlobalNPC.SpawnNPC_ConfectionNPC(spawnInfo, Type);
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) {
			if (NPC.ai[3] >= 200 && NPC.ai[0] == 2f) {
				return false;
			}
			return true;
		}

		public override void DrawBehind(int index) {
			Main.instance.DrawCacheNPCsOverPlayers.Add(index);
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
					int dustID = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Smoke, 0f, 0f, 0, default(Color), 1.5f);
					Main.dust[dustID].noGravity = true;
				}
			}
			else
			{
				for (int i = 0; i < 20; i++)
				{
					int dustID = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Smoke, 0f, 0f, 0, default(Color), 1.5f);
					Dust dust = Main.dust[dustID];
					dust.velocity *= 2f;
					dust.noGravity = true;
				}
				int goreID = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X, NPC.position.Y + (float)(NPC.height / 2) - 10f), new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3)), 61, NPC.scale);
				Gore gore = Main.gore[goreID];
				gore.velocity *= 0.5f;
				goreID = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X, NPC.position.Y + (float)(NPC.height / 2) - 10f), new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3)), 61, NPC.scale);
				gore = Main.gore[goreID];
				gore.velocity *= 0.5f;
				goreID = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X, NPC.position.Y + (float)(NPC.height / 2) - 10f), new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3)), 61, NPC.scale);
				gore = Main.gore[goreID];
				gore.velocity *= 0.5f;
			}
		}
    }
}
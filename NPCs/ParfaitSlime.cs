using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Items.Banners;
using Terraria.DataStructures;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.NPCs
{

    public class ParfaitSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers
            {
                Position = new(2f, 0f)
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 32;
            NPC.damage = 70;
            NPC.defense = 16;
            NPC.lifeMax = 200;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 60f;
            NPC.knockBackResist = 0.5f;
			NPC.aiStyle = -1;
            AnimationType = NPCID.Crimslime;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<ParfaitSlimeBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionUndergroundBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.ParfaitSlime")
            });
        }

		public override void OnSpawn(IEntitySource source)
		{
			NPC.localAI[1] = 0f;
			if (Main.rand.NextBool(2))
			{
				if (Main.xMas)
				{
					NPC.localAI[1] = 1f;
				}
			}
		}

		public override void AI() {
			bool flag = true;
			if (NPC.localAI[0] > 0f) {
				NPC.localAI[0] -= 1f;
			}
			int ShotProjectile = ModContent.ProjectileType<ParfaitSpike>();
			if (!NPC.wet && !Main.player[NPC.target].npcTypeNoAggro[NPC.type]) {
				Vector2 vector7 = new(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
				float num21 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector7.X;
				float num22 = Main.player[NPC.target].position.Y - vector7.Y;
				float num24 = (float)Math.Sqrt(num21 * num21 + num22 * num22);
				if (Main.expertMode && num24 < 200f && Collision.CanHit(new Vector2(NPC.position.X, NPC.position.Y - 20f), NPC.width, NPC.height + 20, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height) && NPC.velocity.Y == 0f) {
					NPC.ai[0] = -40f;
					if (NPC.velocity.Y == 0f) {
						NPC.velocity.X *= 0.9f;
					}
					if (Main.netMode != 1 && NPC.localAI[0] == 0f) {
						Vector2 vector8;
						for (int l = 0; l < 5; l++) {
							vector8 = new((float)(l - 2), -2f);
							vector8.X *= 1f + (float)Main.rand.Next(-50, 51) * 0.02f;
							vector8.Y *= 1f + (float)Main.rand.Next(-50, 51) * 0.02f;
							vector8.Normalize();
							vector8 *= 3f + (float)Main.rand.Next(-50, 51) * 0.01f;
							Projectile.NewProjectile(new EntitySource_Misc(""), vector7.X, vector7.Y, vector8.X, vector8.Y, ShotProjectile, 55 / 4, 0f, Main.myPlayer);
							NPC.localAI[0] = 40f;
						}
					}
				}
				if (num24 < 400f && Collision.CanHit(new Vector2(NPC.position.X, NPC.position.Y - 20f), NPC.width, NPC.height + 20, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height) && NPC.velocity.Y == 0f) {
					NPC.ai[0] = -80f;
					if (NPC.velocity.Y == 0f) {
						NPC.velocity.X *= 0.9f;
					}
					if (Main.netMode != 1 && NPC.localAI[0] == 0f) {
						num22 = Main.player[NPC.target].position.Y - vector7.Y - (float)Main.rand.Next(-30, 20);
						num22 -= num24 * 0.15f;
						num21 = Main.player[NPC.target].position.X - vector7.X - (float)Main.rand.Next(-20, 20);
						num24 = (float)Math.Sqrt(num21 * num21 + num22 * num22);
						num24 = 7f / num24;
						num21 *= num24;
						num22 *= num24;
						NPC.localAI[0] = 32f;
						Projectile.NewProjectile(new EntitySource_Misc(""), vector7.X, vector7.Y, num21, num22, ShotProjectile, 65 / 4, 0f, Main.myPlayer);
					}
				}
			}
			if (NPC.ai[2] > 1f) {
				NPC.ai[2] -= 1f;
			}
			if (NPC.wet) {
				if (NPC.collideY) {
					NPC.velocity.Y = -2f;
				}
				if (NPC.velocity.Y < 0f && NPC.ai[3] == NPC.position.X) {
					NPC.direction *= -1;
					NPC.ai[2] = 200f;
				}
				if (NPC.velocity.Y > 0f) {
					NPC.ai[3] = NPC.position.X;
				}
				if (NPC.velocity.Y > 2f) {
					NPC.velocity.Y *= 0.9f;
				}
				NPC.velocity.Y -= 0.5f;
				if (NPC.velocity.Y < -4f) {
					NPC.velocity.Y = -4f;
				}
				if (NPC.ai[2] == 1f && flag) {
					NPC.TargetClosest();
				}
			}
			NPC.aiAction = 0;
			if (NPC.ai[2] == 0f) {
				NPC.ai[0] = -100f;
				NPC.ai[2] = 1f;
				NPC.TargetClosest();
			}
			if (NPC.velocity.Y == 0f) {
				if (NPC.collideY && NPC.oldVelocity.Y != 0f && Collision.SolidCollision(NPC.position, NPC.width, NPC.height)) {
					NPC.position.X -= NPC.velocity.X + (float)NPC.direction;
				}
				if (NPC.ai[3] == NPC.position.X) {
					NPC.direction *= -1;
					NPC.ai[2] = 200f;
				}
				NPC.ai[3] = 0f;
				NPC.velocity.X *= 0.8f;
				if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1) {
					NPC.velocity.X = 0f;
				}
				if (flag) {
					NPC.ai[0] += 1f;
				}
				NPC.ai[0] += 1f;
				float num27 = -1000f;
				int num28 = 0;
				if (NPC.ai[0] >= 0f) {
					num28 = 1;
				}
				if (NPC.ai[0] >= num27 && NPC.ai[0] <= num27 * 0.5f) {
					num28 = 2;
				}
				if (NPC.ai[0] >= num27 * 2f && NPC.ai[0] <= num27 * 1.5f) {
					num28 = 3;
				}
				if (num28 > 0) {
					NPC.netUpdate = true;
					if (flag && NPC.ai[2] == 1f) {
						NPC.TargetClosest();
					}
					if (num28 == 3) {
						NPC.velocity.Y = -8f;
						NPC.velocity.X += 3 * NPC.direction;
						NPC.ai[0] = -200f;
						NPC.ai[3] = NPC.position.X;
					}
					else {
						NPC.velocity.Y = -6f;
						NPC.velocity.X += 2 * NPC.direction;
						NPC.ai[0] = -120f;
						if (num28 == 1) {
							NPC.ai[0] += num27;
						}
						else {
							NPC.ai[0] += num27 * 2f;
						}
					}
				}
				else if (NPC.ai[0] >= -30f) {
					NPC.aiAction = 1;
				}
			}
			else if (NPC.target < 255 && ((NPC.direction == 1 && NPC.velocity.X < 3f) || (NPC.direction == -1 && NPC.velocity.X > -3f))) {
				if (NPC.collideX && Math.Abs(NPC.velocity.X) == 0.2f) {
					NPC.position.X -= 1.4f * (float)NPC.direction;
				}
				if (NPC.collideY && NPC.oldVelocity.Y != 0f && Collision.SolidCollision(NPC.position, NPC.width, NPC.height)) {
					NPC.position.X -= NPC.velocity.X + (float)NPC.direction;
				}
				if ((NPC.direction == -1 && (double)NPC.velocity.X < 0.01) || (NPC.direction == 1 && (double)NPC.velocity.X > -0.01)) {
					NPC.velocity.X += 0.2f * (float)NPC.direction;
				}
				else {
					NPC.velocity.X *= 0.93f;
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
                return true;

            Texture2D texture = TextureAssets.Npc[Type].Value;
            Rectangle f = NPC.frame;
            if (NPC.localAI[1] == 1f)
            {
                texture = ModContent.Request<Texture2D>(Texture + "_Xmas").Value;
                f.Width = 44;
                f.Height = 34;
                f.Y = (f.Y / 48) * 34;
            }

            spriteBatch.Draw(texture, NPC.Center - screenPos - new Vector2(0f, 4f), f, drawColor, NPC.rotation, f.Size() * 0.5f, NPC.scale, 0, 0f);
            return false;
        }

		public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
			npcLoot.Add(ItemDropRule.Food(ModContent.ItemType<Brownie>(), 150));
			npcLoot.Add(ItemDropRule.Common(ItemID.Gel, maximumDropped: 2));
            npcLoot.Add(ItemDropRule.NormalvsExpert(ItemID.SlimeStaff, 10000, 7000));
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
					int type = ModContent.DustType<CreamsandDust>();
					if (i % 3 == 0)
						type = ModContent.DustType<CreamDust>();
					else if (i % 3 == 1)
						type = ModContent.DustType<CreamstoneDust>();

					Dust.NewDust(NPC.position, NPC.width, NPC.height, type, hit.HitDirection, -1f);
				}
			}
			else
			{
				for (int i = 0; i < 50; i++)
				{
					int type = ModContent.DustType<CreamsandDust>();
					if (i % 3 == 0)
						type = ModContent.DustType<CreamDust>();
					else if (i % 3 == 1)
						type = ModContent.DustType<CreamstoneDust>();

					Dust.NewDust(NPC.position, NPC.width, NPC.height, type, 2 * hit.HitDirection, -2f);
				}
			}
		}
    }
}
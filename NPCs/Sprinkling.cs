using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Items.Banners;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.Audio;
using Terraria.DataStructures;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.NPCs
{
    public class Sprinkling : ModNPC
    {
		public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 10;
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers
            {
                Position = new(2, 6f),
                PortraitPositionXOverride = 2f,
                PortraitPositionYOverride = 4f
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 36;
            NPC.height = 36;
            NPC.damage = 75;
            NPC.defense = 40;
            NPC.lifeMax = 140;
            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath7;
            NPC.value = 60f;
            NPC.noGravity = true;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = -1;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<SprinklingBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionBiome>().Type };
        }

		public override void OnSpawn(IEntitySource source)
		{
			//Spawn variants when naturally spawned (how tf do we do that)
			//Spawning has been added I should do the above ^ 
			NPC.ai[3] = 261;
			if (NPC.ai[0] <= 0)
			{
				if (Main.rand.NextBool(2))
				{
					int type = Type;
					if (Main.halloween)
					{
						type = Main.rand.Next(0, 2) == 1 ? ModContent.NPCType<Sprinkling_Halloween1>() : ModContent.NPCType<Sprinkling_Halloween2>();
					}
					else if (Main.xMas)
					{
						type = ModContent.NPCType<Sprinkling_Xmas>();
					}
					if (type != Type)
					{
						NPC.Transform(type);
					}
				}
			}
			else
			{
				NPC.ai[0] = 0;
			}
		}

		public override void FindFrame(int frameHeight) {
			NPC.frameCounter++;
			int frame = NPC.frame.Y / frameHeight;
			if (NPC.frameCounter >= 8) {
				NPC.frameCounter = 0;
				frame++;
				if (frame >= 10)
				{
					frame = 0;
				}
			}
			if (NPC.frame.Y >= Main.npcFrameCount[Type] * NPC.frame.Height) {
				frame = 0;
			}
			NPC.frame.Y = frameHeight * frame;
		}

		public override void AI() 
		{
			SprinklingAI_Variants(0);
		}

        internal void SprinklingAI_Variants(int variant)
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
			bool flag26 = false;
			int num832 = 4;
			NPC.position += NPC.netOffset;
			NPC.position -= NPC.netOffset;
			if (NPC.position.Y + (float)NPC.height > Main.player[NPC.target].position.Y)
			{
				for (int num865 = num831; num865 < num831 + num832; num865++)
				{
					if ((Main.tile[num828, num865].HasUnactuatedTile && Main.tileSolid[Main.tile[num828, num865].TileType]) || Main.tile[num828, num865].LiquidType > 0)
					{
						if (num865 <= num831 + 1)
						{
							flag26 = true;
						}
						flag25 = false;
						break;
					}
				}
			}
			if (Main.player[NPC.target].npcTypeNoAggro[NPC.type])
			{
				bool flag27 = false;
				for (int num866 = num831; num866 < num831 + num832 - 2; num866++)
				{
					if ((Main.tile[num828, num866].HasUnactuatedTile && Main.tileSolid[Main.tile[num828, num866].TileType]) || Main.tile[num828, num866].LiquidType > 0)
					{
						flag27 = true;
						break;
					}
				}
				NPC.directionY = (!flag27).ToDirectionInt();
			}
			if (flag20)
			{
				flag26 = false;
				flag25 = true;
				NPC.velocity.Y += 2f;
			}
			if (flag25)
			{
				NPC.velocity.Y += 0.2f;
				if (NPC.velocity.Y > 2f)
				{
					NPC.velocity.Y = 2f;
				}
			}
			else
			{
				if ((NPC.directionY < 0 && NPC.velocity.Y > 0f) || flag26)
				{
					NPC.velocity.Y -= 0.2f;
				}
			}
			if (NPC.wet)
			{
				NPC.velocity.Y -= 0.2f;
				if (NPC.velocity.Y < -2f)
				{
					NPC.velocity.Y = -2f;
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
			float num868 = 3f;
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
			num868 = 1.4f;
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

			//Sprinkler AI stuff
			NPC.spriteDirection = NPC.direction;
			if (NPC.ai[3] >= 1000)
			{
				NPC.ai[3]++;
			}
			int sprinkleCount = countSprinkles();
			NPC.defense = 8 * sprinkleCount;
			if (NPC.ai[3] >= 1000 + 300 * 5)
			{
				NPC.ai[3] = 261;
			}
			if (NPC.ai[3] < 0)
			{
				NPC.ai[3] = 1000;
			}

			if (NPC.ai[3] <= 261)
			{
				float speed = 15f;
				float SpeedX = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - NPC.Center.X;
				float SpeedY = Main.player[NPC.target].position.Y + (float)((Main.player[NPC.target].height / 2) - 40) - NPC.Center.Y;
				float trijectory = (float)Math.Sqrt(SpeedX * SpeedX + SpeedY * SpeedY);
				trijectory = speed / trijectory;
				SpeedX *= trijectory;
				SpeedY *= trijectory;

				if (Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
				{
					if (NPC.ai[3] <= 261)
					{
						NPC.ai[3]--;
					}
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						int type = ModContent.ProjectileType<Projectiles.DefenciveSprinkle>();
						int damage = 60;
						if (NPC.ai[3] == 61f)
						{
							int projID = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, SpeedX, SpeedY, type, damage, 0f, Main.myPlayer);
							Projectile proj = Main.projectile[projID];
							proj.friendly = false;
							proj.frame = variant * 5;
							NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projID);
							NPC.netUpdate = true;
							SoundEngine.PlaySound(SoundID.Item5, NPC.position);
						}
						if (NPC.ai[3] == 46f)
						{
							int projID = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, SpeedX, SpeedY, type, damage, 0f, Main.myPlayer);
							Projectile proj = Main.projectile[projID];
							proj.friendly = false;
							proj.frame = variant * 5 + 1;
							NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projID);
							NPC.netUpdate = true;
							SoundEngine.PlaySound(SoundID.Item5, NPC.position);
						}
						if (NPC.ai[3] == 31f)
						{
							int projID = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, SpeedX, SpeedY, type, damage, 0f, Main.myPlayer);
							Projectile proj = Main.projectile[projID];
							proj.friendly = false;
							proj.frame = variant * 5 + 2;
							NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projID);
							NPC.netUpdate = true;
							SoundEngine.PlaySound(SoundID.Item5, NPC.position);
						}
						if (NPC.ai[3] == 16f)
						{
							int projID = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, SpeedX, SpeedY, type, damage, 0f, Main.myPlayer);
							Projectile proj = Main.projectile[projID];
							proj.friendly = false;
							proj.frame = variant * 5 + 3;
							NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projID);
							NPC.netUpdate = true;
							SoundEngine.PlaySound(SoundID.Item5, NPC.position);
						}
						if (NPC.ai[3] == 1f)
						{
							int projID = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, SpeedX, SpeedY, type, damage, 0f, Main.myPlayer);
							Projectile proj = Main.projectile[projID];
							proj.friendly = false;
							proj.frame = variant * 5 + 4;
							NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projID);
							NPC.netUpdate = true;
							SoundEngine.PlaySound(SoundID.Item5, NPC.position);
						}
					}
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return ConfectionGlobalNPC.SpawnNPC_ConfectionNPC(spawnInfo, Type);
		}

		internal int countSprinkles()
		{
			int count = 0;
			if (NPC.ai[3] >= 1000)
			{
				count = (int)((NPC.ai[3] - 1000) / 300);
			}
			if (NPC.ai[3] <= 261)
			{
				count = 5;
				if (NPC.ai[3] <= 61)
					count--;
				if (NPC.ai[3] <= 46)
					count--;
				if (NPC.ai[3] <= 31)
					count--;
				if (NPC.ai[3] <= 16)
					count--;
				if (NPC.ai[3] <= 1)
					count--;
			}
			return count;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.Sprinkling")
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Sprinkles>(), 1, 1, 3));
            npcLoot.Add(ItemDropRule.Common(ItemID.FastClock, 100));
            npcLoot.Add(ItemDropRule.Common(ItemID.Megaphone, 100));
        }

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			if (NPC.life > 0)
			{
				for (int i = 0; i < hit.Damage / (double)NPC.lifeMax * 10.0; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<SprinklingDust>(), 2.5f * (float)hit.HitDirection, -2.5f);
				}
			}
			else
			{
				for (int i = 0; i < 20; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<SprinklingDust>(), hit.HitDirection, -1f);
				}
				int goreID = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X, NPC.position.Y - 10f), new Vector2((float)hit.HitDirection, 0f), 61, NPC.scale);
				Gore gore = Main.gore[goreID];
				gore.velocity *= 0.3f;
				goreID = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X, NPC.position.Y + (float)(NPC.height / 2) - 10f), new Vector2((float)hit.HitDirection, 0f), 62, NPC.scale);
				gore = Main.gore[goreID];
				gore.velocity *= 0.3f;
				goreID = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X, NPC.position.Y + (float)NPC.height - 10f), new Vector2((float)hit.HitDirection, 0f), 63, NPC.scale);
				gore = Main.gore[goreID];
				gore.velocity *= 0.3f;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			return SprinklingDrawing(0, spriteBatch, drawColor, screenPos);
        }

		internal bool SprinklingDrawing(int variant, SpriteBatch spriteBatch, Color drawColor, Vector2 screenPos)
		{
			if (NPC.IsABestiaryIconDummy)
			{
				return true;
			}
			Texture2D texture = TextureAssets.Npc[Type].Value;
			Rectangle frame = NPC.frame;
			SpriteEffects spriteEffects = 0;
			if (NPC.spriteDirection == 1)
			{
				spriteEffects = (SpriteEffects)1;
			}
			Vector2 pos = NPC.Center - screenPos;
			pos.Y += NPC.gfxOffY - 4f;

			Texture2D back = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/NPCs/Sprinkler/SprinklingSprinkleAmmount/Sprinkling_" + variant + "_" + countSprinkles() + "_2");
			Texture2D front = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/NPCs/Sprinkler/SprinklingSprinkleAmmount/Sprinkling_" + variant + "_" + countSprinkles() + "_1");
			texture = ModContent.Request<Texture2D>($"TheConfectionRebirth/NPCs/Sprinkler/Sprinkling_" + variant).Value;
			frame.Y %= front.Height;

			spriteBatch.Draw(back, pos, frame, drawColor, NPC.rotation, frame.Size() * 0.5f, NPC.scale, spriteEffects, 0f);
			spriteBatch.Draw(texture, pos, new(0, 0, 58, 50), drawColor, NPC.rotation, new(29, 25), NPC.scale, spriteEffects, 0f);
			spriteBatch.Draw(front, pos, frame, drawColor, NPC.rotation, frame.Size() * 0.5f, NPC.scale, spriteEffects, 0f);
			return false;
		}
    }
}
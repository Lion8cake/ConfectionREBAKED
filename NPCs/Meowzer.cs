using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items.Banners;
using TheConfectionRebirth.Items.Placeable;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Projectiles;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System.IO;
using System.Data.OleDb;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.NPCs
{
	public class Meowzer : ModNPC
	{
		int tailHitbox = -1; //NPC whoAmI for the owned tail

		Vector2 oldVel;

		Vector2 lastSeenPos;

		public override void SetStaticDefaults()
		{
			var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers
			{
				CustomTexturePath = "TheConfectionRebirth/NPCs/Meowzer_Bestiary",
				Position = new Vector2(0f, -12f),
				PortraitPositionXOverride = 0f,
				PortraitPositionYOverride = 12f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifier);
			NPCID.Sets.TrailCacheLength[Type] = 40;
			NPCID.Sets.TrailingMode[Type] = 1;
		}

		public override void SetDefaults()
		{
			NPC.width = 38;
			NPC.height = 50;
			NPC.aiStyle = -1;
			NPC.lifeMax = 280;
			NPC.defense = 24;
			NPC.damage = 70;
			NPC.knockBackResist = 0.2f;
			NPC.value = Item.buyPrice(0, 0, 7, 50);
			NPC.npcSlots = 1f;
			NPC.noGravity = true;
			NPC.netAlways = true;
			NPC.HitSound = SoundID.NPCHit7;
			NPC.DeathSound = SoundID.Item2;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<MeowzerBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionBiome>().Type };
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
				new BestiaryBackground(ModContent.Request<Texture2D>("TheConfectionRebirth/Biomes/ConfectionBiomeMapBackground"), new Color(35, 40, 40)),
				new BestiaryBackgroundOverlay(Main.Assets.Request<Texture2D>("Images/MapBGOverlay4"), Color.White),
				new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.Meowzer")
			});
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(tailHitbox);
			writer.WriteVector2(oldVel);
			writer.WriteVector2(lastSeenPos);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			tailHitbox = reader.ReadInt32();
			oldVel = reader.ReadVector2();
			lastSeenPos = reader.ReadVector2();
		}

		public override void AI()
		{
			float lockOnAmount = (float)Math.Sin(NPC.ai[1]);
			if (NPC.ai[3] <= 0f)
			{
				NPC.lifeMax = NPC.life = 560;
				int index = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<MeowzerCannon>());
				NPC npc = Main.npc[index];
				npc.ai[0] = NPC.whoAmI;
				tailHitbox = npc.whoAmI;
				NPC.ai[3] = 1f;
			}
			if ((Main.npc[tailHitbox] == null || !Main.npc[tailHitbox].active || tailHitbox < 0 || tailHitbox > Main.maxNPCs) && NPC.ai[3] > 0f && NPC.ai[3] < 400f)
			{
				NPC.ai[3] = 400f;
				//Enrage
				ResetFields(ref lockOnAmount);
				SoundEngine.PlaySound(SoundID.NPCDeath30, NPC.Center);
			}

			if (oldVel != Vector2.Zero)
				NPC.velocity = new Vector2(MathHelper.Lerp(oldVel.X, 0, lockOnAmount), MathHelper.Lerp(oldVel.Y, 0, lockOnAmount + 0.02f));

			NPC.spriteDirection = NPC.velocity.X < 0 ? -1 : 1;
			NPC.noTileCollide = NPC.ai[3] == 400f;

			if (NPC.ai[3] >= 400f) //No cannon AI
			{
				//aistyle 2
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
				if (Main.dayTime)
				{
					NPC.EncourageDespawn(10);
					NPC.directionY = -1;
					if (NPC.velocity.Y > 0f)
					{
						NPC.direction = 1;
					}
					NPC.direction = -1;
					if (NPC.velocity.X > 0f)
					{
						NPC.direction = 1;
					}
				}
				else
				{
					NPC.TargetClosest();
				}
				
				float num2 = 4f;
				float num3 = 1.5f;
				num2 *= 1f + (1f - NPC.scale);
				num3 *= 1f + (1f - NPC.scale);
				if (NPC.direction == -1 && NPC.velocity.X > 0f - num2)
				{
					NPC.velocity.X -= 0.1f;
					if (NPC.velocity.X > num2)
					{
						NPC.velocity.X -= 0.1f;
					}
					else if (NPC.velocity.X > 0f)
					{
						NPC.velocity.X += 0.05f;
					}
					if (NPC.velocity.X < 0f - num2)
					{
						NPC.velocity.X = 0f - num2;
					}
				}
				else if (NPC.direction == 1 && NPC.velocity.X < num2)
				{
					NPC.velocity.X += 0.1f;
					if (NPC.velocity.X < 0f - num2)
					{
						NPC.velocity.X += 0.1f;
					}
					else if (NPC.velocity.X < 0f)
					{
						NPC.velocity.X -= 0.05f;
					}
					if (NPC.velocity.X > num2)
					{
						NPC.velocity.X = num2;
					}
				}
				if (NPC.directionY == -1 && NPC.velocity.Y > 0f - num3)
				{
					NPC.velocity.Y -= 0.04f;
					if (NPC.velocity.Y > num3)
					{
						NPC.velocity.Y -= 0.05f;
					}
					else if (NPC.velocity.Y > 0f)
					{
						NPC.velocity.Y += 0.03f;
					}
					if (NPC.velocity.Y < 0f - num3)
					{
						NPC.velocity.Y = 0f - num3;
					}
				}
				else if (NPC.directionY == 1 && NPC.velocity.Y < num3)
				{
					NPC.velocity.Y += 0.04f;
					if (NPC.velocity.Y < 0f - num3)
					{
						NPC.velocity.Y += 0.05f;
					}
					else if (NPC.velocity.Y < 0f)
					{
						NPC.velocity.Y -= 0.03f;
					}
					if (NPC.velocity.Y > num3)
					{
						NPC.velocity.Y = num3;
					}
					
				}
				
				if (NPC.wet)
				{
					if (NPC.velocity.Y > 0f)
					{
						NPC.velocity.Y *= 0.95f;
					}
					NPC.velocity.Y -= 0.5f;
					if (NPC.velocity.Y < -4f)
					{
						NPC.velocity.Y = -4f;
					}
					NPC.TargetClosest();
				}

				//tail
				/*if (index[1]++ > 10)
				{
					index[1] = 0;
					if (index[0]++ > 2)
						index[0] = 0;
					tailSeg[index[0]] = NPC.Center;
				}*/
			}
			else //cannon AI
			{
				//aiStyle 22
				bool flag20 = false;
				bool retreat = Main.dayTime && NPC.ai[3] < 240f;
				bool lockDir = NPC.ai[3] >= 241;
				NPC tail = Main.npc[tailHitbox];
				if (NPC.justHit)
				{
					NPC.ai[2] = 0f;
				}
				if (retreat)
				{
					if (NPC.velocity.X == 0f)
					{
						NPC.velocity.X = (float)Main.rand.Next(-1, 2) * 1.5f;
						NPC.netUpdate = true;
					}
					NPC.EncourageDespawn(10);
				}
				else if (NPC.ai[2] >= 0f)
				{
					int num827 = 16;
					bool flag22 = false;
					bool flag23 = false;
					float targetX = tail.ai[2];
					if (NPC.position.X > targetX - (float)num827 && NPC.position.X < targetX + (float)num827)
					{
						flag22 = true;
					}
					else if ((NPC.velocity.X < 0f && NPC.direction > 0) || (NPC.velocity.X > 0f && NPC.direction < 0))
					{
						flag22 = true;
					}
					num827 += 24;
					float targetY = tail.ai[3];
					if (NPC.position.Y > targetY - (float)num827 && NPC.position.Y < targetY + (float)num827)
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
							if (!lockDir)
								NPC.direction *= -1;
							NPC.velocity.X *= -1f;
							NPC.collideX = false;
						}
					}
					else
					{
						tail.ai[2] = NPC.position.X;
						tail.ai[3] = NPC.position.Y;
						NPC.ai[2] = 0f;
					}
					NPC.TargetClosest();
				}
				else
				{
					NPC.ai[2] += 1f;
					if (!lockDir)
					{
						if (Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) > NPC.position.X + (float)(NPC.width / 2))
						{
							NPC.direction = -1;
						}
						else
						{
							NPC.direction = 1;
						}
					}
				}

				
				//Meowzer Shoot
				NPC.rotation *= NPC.direction == -1 ? -0.01f : 0.05f;
				if (tail.localAI[0] >= 0)
				{
					if (tail.localAI[0] >= ((float)Math.PI / 30) * 31)
						tail.localAI[0] = -1;

					tail.localAI[0] += (float)Math.PI / 30;
					float sine = (float)(0.5 * Math.Cos(tail.localAI[0]) + 0.5);
					NPC.position.X += (tail.localAI[1] * sine) * -0.25f;
					NPC.position.Y += (tail.localAI[2] * sine) * -0.25f;
				}
				lockOnAmount = (float)Math.Sin(NPC.ai[1]);
				if (NPC.ai[0] > 0)
				{
					NPC.ai[1] -= (float)Math.PI / 30;
					if (NPC.ai[1] < 0)
					{
						ResetFields(ref lockOnAmount);
					}
				}
				if (NPC.ai[3] > 0)
				{
					NPC.ai[3]++;
					if (!retreat)
					{
						if (NPC.ai[3] >= 241)
						{
							if ((NPC.target == -1 ? null : Main.player[NPC.target]) != null)
							{
								if (NPC.ai[3] <= 251)
								{
									NPC.direction = Main.player[NPC.target].MountedCenter.X < NPC.position.X ? -1 : 1;
								}
								Player targ = Main.player[NPC.target];
								if (Collision.CanHitLine(targ.MountedCenter, targ.width, targ.height, NPC.Center, NPC.width, NPC.height))
									tail.ai[1] = 1f;
								if (tail.ai[1] >= 1f)
								{
									if (Collision.CanHitLine(targ.MountedCenter, targ.width, targ.height, NPC.Center, NPC.width, NPC.height) && tail.ai[1] < 2f)
										lastSeenPos = targ.MountedCenter;
									else
										tail.ai[1] = 2f;
									if (oldVel == Vector2.Zero)
										oldVel = NPC.velocity;
									if (lockOnAmount < 1f)
										NPC.ai[1] += (float)Math.PI / 60;
									if (NPC.ai[1] >= Math.PI * 0.475f)
									{
										//Charge
										float scale = NPC.ai[3] / 401;
										NPC.localAI[2] += 0.03f;
										if (NPC.localAI[2] > 1)
											NPC.localAI[2] = 1;
										NPC.localAI[0] = Main.rand.NextFloat(-0.75f, 0.75f) * scale;
										NPC.localAI[1] = Main.rand.NextFloat(-0.75f, 0.75f) * scale;
										if (NPC.frameCounter++ > 15)
										{
											NPC.frameCounter = 0;
											int testy = NPC.frame.Y;
											testy++;
											if (testy > 3)
												testy = 0;
											NPC.frame.Y = testy;
										}
										//Fire
										if (NPC.ai[3] >= 341)
										{
											if (Main.netMode == NetmodeID.MultiplayerClient)
												return;

											Vector2 val = lastSeenPos;
											Vector2 val2 = val - NPC.Center;
											float num2 = 10f;
											float num3 = (float)Math.Sqrt(val2.X * val2.X + val2.Y * val2.Y);
											if (num3 > num2)
												num3 = num2 / num3;
											val2 *= num3;
											tail.localAI[1] = val2.X;
											tail.localAI[2] = val2.Y;
											Vector2 firePos = NPC.Center + new Vector2((NPC.spriteDirection == -1 ? 20f : 0f), -45f);

											float divident = Main.expertMode ? 0.4f : 0.5f;
											int damage = (int)Math.Round(125 * divident);

											Projectile.NewProjectile(NPC.GetSource_FromAI(), firePos, val2 *= 0.5f, ModContent.ProjectileType<MeowzerBeam>(), damage, 2.5f);
											NPC.ai[0] = 1f;
											NPC.ai[3] = 1f;
											tail.localAI[0] = ((float)Math.PI / 30);
										}
									}
								}
								else
								{
									NPC.ai[3] = 1f;
								}
							}
						}
						else
						{
							NPC.localAI[2] = 0;
						}
					}
					else
					{
						NPC.ai[3] = 1f;
					}
				}

				int num828 = (int)((NPC.position.X + (float)(NPC.width / 2)) / 16f) + NPC.direction * 2;
				int num831 = (int)((NPC.position.Y + (float)NPC.height) / 16f);
				bool flag25 = true;
				int num832 = 3;
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
				if (Main.player[NPC.target].npcTypeNoAggro[NPC.type])
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
				float num868 = 1.5f;				
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
				num868 = 1f;
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
		}

		public void ResetFields(ref float lockOnAmount)
		{
			lockOnAmount = 0;
			NPC.ai[0] = 0;
			oldVel = Vector2.Zero;
			if (NPC.ai[3] > 0f)
			{
				Main.npc[tailHitbox].ai[1] = 0f;
			}
			NPC.localAI[0] = 0;
			NPC.localAI[1] = 0;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (NPC.ai[3] < 400f)
			{
				Texture2D idle = ModContent.Request<Texture2D>("TheConfectionRebirth/NPCs/Meowzer_Idle").Value;
				Texture2D cannon = ModContent.Request<Texture2D>("TheConfectionRebirth/NPCs/MeowzerCannon").Value;
				Texture2D charge = ModContent.Request<Texture2D>("TheConfectionRebirth/NPCs/MeowzerCannon_Charge").Value;
				float lockOnAmount = (float)Math.Sin(NPC.ai[1]);
				float cannonRotation = 0f;
				if ((NPC.target == -1 ? null : Main.player[NPC.target]) != null)
				{
					float rot = (float)Math.Atan2((double)(lastSeenPos.Y - NPC.Center.Y), (double)(lastSeenPos.X - NPC.Center.X));
					cannonRotation = MathHelper.Lerp(NPC.rotation + (NPC.spriteDirection == 1 ? 1.5f : -1.5f), rot + 1.75f, lockOnAmount);
				}
				Vector2 pos = NPC.Center - Main.screenPosition + (NPC.spriteDirection == 1 ? new Vector2(-15, 0) : Vector2.Zero);
				Vector2 cannonOrg = new Vector2(cannon.Width / 2, cannon.Height / 2);
				SpriteEffects spriteEffects = SpriteEffects.None;
				if (NPC.spriteDirection == 1)
				{
					spriteEffects = SpriteEffects.FlipHorizontally;
				}
				cannonOrg.X += NPC.localAI[0];
				cannonOrg.Y += NPC.localAI[1];
				spriteBatch.Draw(idle, pos + new Vector2(10, -20), null, drawColor, NPC.rotation, new Vector2(idle.Width / 2, idle.Height / 2), 1, spriteEffects, 0);
				spriteBatch.Draw(cannon, pos + new Vector2((NPC.spriteDirection == -1 ? 20f : 0f), -45f), null, drawColor, cannonRotation, cannonOrg, 1, spriteEffects, 0);
				int cannonFrames = 4;
				spriteBatch.Draw(charge, pos + new Vector2((NPC.spriteDirection == -1 ? 20f : 0f), -45f), new Rectangle(0, (charge.Height / cannonFrames) * NPC.frame.Y, charge.Width, charge.Height / cannonFrames), MeowzerBeam.LerpColor(Color.Transparent, drawColor, NPC.localAI[2]), cannonRotation, cannonOrg, 1, spriteEffects, 0);
			}
			else
			{
				Texture2D tex = ModContent.Request<Texture2D>("TheConfectionRebirth/NPCs/Meowzer_Tail").Value;
				Texture2D tex2 = TextureAssets.Npc[Type].Value;
				for (int a = 0; a < NPC.oldPos.Length / 10; a++)
				{
					if (NPC.oldPos[a * 10] != Vector2.Zero)
					{
						Rectangle rect = new Rectangle(0, 14 * (a - 1), 14, 14);
						spriteBatch.Draw(tex, NPC.oldPos[a * 10] + new Vector2(NPC.width / 2, NPC.height / 2) - Main.screenPosition, rect, drawColor, 0, new Vector2(7), 1, SpriteEffects.None, 0);
					}
				}
				spriteBatch.Draw(tex2, NPC.Center - Main.screenPosition, null, drawColor, NPC.rotation *= 0.25f, new Vector2(tex2.Width / 2, tex2.Height / 2), NPC.scale, NPC.velocity.X < 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
			}
			return false;
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			if (NPC.life <= 0)
			{
				for (int i = 0; i < 50; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<PastryDust>(), 2.5f * (float)hit.HitDirection, -2.5f);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("MeowzerGore0").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("MeowzerGore1").Type);
				for (int j = 0; j < 2; j++)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("MeowzerGore2").Type);
				}
			}
			else
			{
				for (int i = 0; i < hit.Damage / (double)NPC.lifeMax * 10.0; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<PastryDust>(), hit.HitDirection, -1f);
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.Gel, 1, 5, 10));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PastryBlock>(), 2, 15, 25));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ToastyToaster>(), 20));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PastryTart>(), 95, 1));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			//if (spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && !spawnInfo.AnyInvasionActive() && Main.hardMode && !spawnInfo.Player.ZoneRockLayerHeight && !spawnInfo.Player.ZoneDirtLayerHeight && !Main.dayTime) {
			//	return 0.8f;
			//}
			return 0f;
		}
	}
}
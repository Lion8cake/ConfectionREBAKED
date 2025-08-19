using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Events;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items;

namespace TheConfectionRebirth.NPCs
{
    public class SugarGhoul : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 8;
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers
            {
                Velocity = 1f
            });
        }

        public override void SetDefaults()
        {
			NPC.width = 24;
			NPC.height = 44;
			NPC.aiStyle = 3;
			NPC.damage = 58;
			NPC.defense = 34;
			NPC.lifeMax = 270;
			NPC.HitSound = SoundID.NPCHit37;
			NPC.DeathSound = SoundID.NPCDeath40;
			NPC.knockBackResist = 0.4f;
			NPC.value = 750f;
			NPC.npcSlots = 0.5f;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<SandConfectionUndergroundBiome>().Type };
		}

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.SugarGhoul")
            });
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return ConfectionGlobalNPC.SpawnNPC_ConfectionNPC(spawnInfo, Type);
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
            npcLoot.Add(ItemDropRule.Common(ItemID.AncientCloth, 10));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CreamPuff>(), 15));
		}

		public override void FindFrame(int frameHeight)
		{
			if (NPC.velocity.Y == 0f)
			{
				if (NPC.direction != 0)
				{
					NPC.spriteDirection = NPC.direction;
				}
				if (NPC.velocity.X == 0f)
				{
					NPC.frame.Y = 0;
					NPC.frameCounter = 0.0;
					return;
				}
				if (NPC.frame.Y <= frameHeight)
				{
					NPC.frame.Y = frameHeight * 2;
				}
				NPC.frameCounter += Math.Abs(NPC.velocity.X);
				NPC.frameCounter += 1.0;
				if (NPC.frameCounter > 9.0)
				{
					NPC.frame.Y += frameHeight;
					NPC.frameCounter = 0.0;
				}
				if (NPC.frame.Y / frameHeight >= Main.npcFrameCount[Type])
				{
					NPC.frame.Y = frameHeight * 2;
				}
			}
			else
			{
				NPC.frame.Y = frameHeight;
				NPC.frameCounter = 0.0;
			}
		}

		public override bool PreAI()
		{
			if (!(NPC.shimmerTransparency > 0f))
			{
				if (Main.rand.NextBool(700))
				{
					SoundEngine.PlaySound(Main.rand.NextBool(2) ? SoundID.Zombie55 : SoundID.Zombie56, NPC.position);
				}
			}

			if (Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height == NPC.position.Y + (float)NPC.height)
			{
				NPC.directionY = -1;
			}
			bool flag = false;
			Rectangle hitbox;
			bool flag23 = false;
			bool flag24 = false;
			if (NPC.velocity.X == 0f)
			{
				flag24 = true;
			}
			if (NPC.justHit)
			{
				flag24 = false;
			}
			int num154 = 60;
			bool flag25 = false;
			bool flag26 = false;
			bool flag27 = false;
			bool flag2 = true;
			if (!flag27 && flag2)
			{
				if (NPC.velocity.Y == 0f && ((NPC.velocity.X > 0f && NPC.direction < 0) || (NPC.velocity.X < 0f && NPC.direction > 0)))
				{
					flag25 = true;
				}
				if (NPC.position.X == NPC.oldPosition.X || NPC.ai[3] >= (float)num154 || flag25)
				{
					NPC.ai[3] += 1f;
				}
				else if ((double)Math.Abs(NPC.velocity.X) > 0.9 && NPC.ai[3] > 0f)
				{
					NPC.ai[3] -= 1f;
				}
				if (NPC.ai[3] > (float)(num154 * 10))
				{
					NPC.ai[3] = 0f;
				}
				if (NPC.justHit)
				{
					NPC.ai[3] = 0f;
				}
				if (NPC.ai[3] == (float)num154)
				{
					NPC.netUpdate = true;
				}
				hitbox = Main.player[NPC.target].Hitbox;
				if (hitbox.Intersects(NPC.Hitbox))
				{
					NPC.ai[3] = 0f;
				}
			}
			
			if (NPC.ai[3] < (float)num154)
			{
				NPC.TargetClosest();
				if (NPC.directionY > 0 && Main.player[NPC.target].Center.Y <= NPC.Bottom.Y)
				{
					NPC.directionY = -1;
				}
			}
			else if (!(NPC.ai[2] > 0f))
			{
				if (Main.IsItDay() && (double)(NPC.position.Y / 16f) < Main.worldSurface)
				{
					NPC.EncourageDespawn(10);
				}
				if (NPC.velocity.X == 0f)
				{
					if (NPC.velocity.Y == 0f)
					{
						NPC.ai[0] += 1f;
						if (NPC.ai[0] >= 2f)
						{
							NPC.direction *= -1;
							NPC.spriteDirection = NPC.direction;
							NPC.ai[0] = 0f;
						}
					}
				}
				else
				{
					NPC.ai[0] = 0f;
				}
				if (NPC.direction == 0)
				{
					NPC.direction = 1;
				}
			}
			
			if (NPC.velocity.X < -2f || NPC.velocity.X > 2f)
			{
				if (NPC.velocity.Y == 0f)
				{
					NPC.velocity *= 0.8f;
				}
			}
			else if (NPC.velocity.X < 2f && NPC.direction == 1)
			{
				NPC.velocity.X += 0.07f;
				if (NPC.velocity.X > 2f)
				{
					NPC.velocity.X = 2f;
				}
			}
			else if (NPC.velocity.X > -2f && NPC.direction == -1)
			{
				NPC.velocity.X -= 0.07f;
				if (NPC.velocity.X < -2f)
				{
					NPC.velocity.X = -2f;
				}
			}
			
			Vector3 rgb3 = new Vector3(0f, 0.19f, 0.93f) * 0.4f; //Color
			Lighting.AddLight(NPC.Top + new Vector2(0f, 15f), rgb3);
			
			if (NPC.velocity.Y == 0f || flag)
			{
				int num91 = (int)(NPC.position.Y + (float)NPC.height + 7f) / 16;
				int num92 = (int)(NPC.position.Y - 9f) / 16;
				int num93 = (int)NPC.position.X / 16;
				int num94 = (int)(NPC.position.X + (float)NPC.width) / 16;
				int num205 = (int)(NPC.position.X + 8f) / 16;
				int num95 = (int)(NPC.position.X + (float)NPC.width - 8f) / 16;
				bool flag15 = false;
				for (int num96 = num205; num96 <= num95; num96++)
				{
					if (num96 >= num93 && num96 <= num94 && Main.tile[num96, num91] == null)
					{
						flag15 = true;
						continue;
					}
					if (Main.tile[num96, num92] != null && Main.tile[num96, num92].HasUnactuatedTile && Main.tileSolid[Main.tile[num96, num92].TileType])
					{
						flag23 = false;
						break;
					}
					if (!flag15 && num96 >= num93 && num96 <= num94 && Main.tile[num96, num91].HasUnactuatedTile && Main.tileSolid[Main.tile[num96, num91].TileType])
					{
						flag23 = true;
					}
				}
				if (!flag23 && NPC.velocity.Y < 0f)
				{
					NPC.velocity.Y = 0f;
				}
				if (flag15)
				{
					return false;
				}
			}
			
			if (NPC.velocity.Y >= 0f)
			{
				int num97 = 0;
				if (NPC.velocity.X < 0f)
				{
					num97 = -1;
				}
				if (NPC.velocity.X > 0f)
				{
					num97 = 1;
				}
				Vector2 vector30 = NPC.position;
				vector30.X += NPC.velocity.X;
				int num98 = (int)((vector30.X + (float)(NPC.width / 2) + (float)((NPC.width / 2 + 1) * num97)) / 16f);
				int num100 = (int)((vector30.Y + (float)NPC.height - 1f) / 16f);
				if (WorldGen.InWorld(num98, num100, 4))
				{
					if ((float)(num98 * 16) < vector30.X + (float)NPC.width && (float)(num98 * 16 + 16) > vector30.X && ((Main.tile[num98, num100].HasUnactuatedTile && !Main.tile[num98, num100].TopSlope && !Main.tile[num98, num100 - 1].TopSlope && Main.tileSolid[Main.tile[num98, num100].TileType] && !Main.tileSolidTop[Main.tile[num98, num100].TileType]) || 
						(Main.tile[num98, num100 - 1].IsHalfBlock && Main.tile[num98, num100 - 1].HasUnactuatedTile)) && (!Main.tile[num98, num100 - 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num98, num100 - 1].TileType] || Main.tileSolidTop[Main.tile[num98, num100 - 1].TileType] || (Main.tile[num98, num100 - 1].IsHalfBlock && (!Main.tile[num98, num100 - 4].HasUnactuatedTile || 
						!Main.tileSolid[Main.tile[num98, num100 - 4].TileType] || Main.tileSolidTop[Main.tile[num98, num100 - 4].TileType]))) && (!Main.tile[num98, num100 - 2].HasUnactuatedTile || !Main.tileSolid[Main.tile[num98, num100 - 2].TileType] || Main.tileSolidTop[Main.tile[num98, num100 - 2].TileType]) && (!Main.tile[num98, num100 - 3].HasUnactuatedTile || !Main.tileSolid[Main.tile[num98, num100 - 3].TileType] || Main.tileSolidTop[Main.tile[num98, num100 - 3].TileType]) && (!Main.tile[num98 - num97, num100 - 3].HasUnactuatedTile || !Main.tileSolid[Main.tile[num98 - num97, num100 - 3].TileType]))
					{
						float num101 = num100 * 16;
						if (Main.tile[num98, num100].IsHalfBlock)
						{
							num101 += 8f;
						}
						if (Main.tile[num98, num100 - 1].IsHalfBlock)
						{
							num101 -= 8f;
						}
						if (num101 < vector30.Y + (float)NPC.height)
						{
							float num102 = vector30.Y + (float)NPC.height - num101;
							float num103 = 16.1f;
							if (num102 <= num103)
							{
								NPC.gfxOffY += NPC.position.Y + (float)NPC.height - num101;
								NPC.position.Y = num101 - (float)NPC.height;
								if (num102 < 9f)
								{
									NPC.stepSpeed = 1f;
								}
								else
								{
									NPC.stepSpeed = 2f;
								}
							}
						}
					}
				}
			}
			if (flag23)
			{
				int num104 = (int)((NPC.position.X + (float)(NPC.width / 2) + (float)(15 * NPC.direction)) / 16f);
				int num105 = (int)((NPC.position.Y + (float)NPC.height - 15f) / 16f);
				//Main.tile[num104, num105 + 1].halfBrick();
				if (Main.tile[num104, num105 - 1].HasUnactuatedTile && (TileLoader.IsClosedDoor(Main.tile[num104, num105 - 1]) || Main.tile[num104, num105 - 1].TileType == 388) && flag26)
				{
					NPC.ai[2] += 1f;
					NPC.ai[3] = 0f;
					if (NPC.ai[2] >= 60f)
					{
						bool flag17 = Main.player[NPC.target].ZoneGraveyard && Main.rand.NextBool(60);

						NPC.velocity.X = 0.5f * (float)(-NPC.direction);
						int num106 = 5;
						if (Main.tile[num104, num105 - 1].TileType == 388)
						{
							num106 = 2;
						}
						NPC.ai[1] += num106;

						NPC.ai[2] = 0f;
						bool flag18 = false;
						if (NPC.ai[1] >= 10f)
						{
							flag18 = true;
							NPC.ai[1] = 10f;
						}
						
						WorldGen.KillTile(num104, num105 - 1, fail: true);
						if ((Main.netMode != 1 || !flag18) && flag18 && Main.netMode != 1)
						{
							if (TileLoader.IsClosedDoor(Main.tile[num104, num105 - 1]))
							{
								bool flag19 = WorldGen.OpenDoor(num104, num105 - 1, NPC.direction);
								if (!flag19)
								{
									NPC.ai[3] = num154;
									NPC.netUpdate = true;
								}
								if (Main.netMode == 2 && flag19)
								{
									NetMessage.SendData(19, -1, -1, null, 0, num104, num105 - 1, NPC.direction);
								}
							}
							if (Main.tile[num104, num105 - 1].TileType == 388)
							{
								bool flag20 = WorldGen.ShiftTallGate(num104, num105 - 1, closing: false);
								if (!flag20)
								{
									NPC.ai[3] = num154;
									NPC.netUpdate = true;
								}
								if (Main.netMode == 2 && flag20)
								{
									NetMessage.SendData(19, -1, -1, null, 4, num104, num105 - 1);
								}
							}
						}
					}
				}
				else
				{
					int num107 = NPC.spriteDirection;
					if ((NPC.velocity.X < 0f && num107 == -1) || (NPC.velocity.X > 0f && num107 == 1))
					{
						if (NPC.height >= 32 && Main.tile[num104, num105 - 2].HasUnactuatedTile && Main.tileSolid[Main.tile[num104, num105 - 2].TileType])
						{
							if (Main.tile[num104, num105 - 3].HasUnactuatedTile && Main.tileSolid[Main.tile[num104, num105 - 3].TileType])
							{
								NPC.velocity.Y = -8f;
								NPC.netUpdate = true;
							}
							else
							{
								NPC.velocity.Y = -7f;
								NPC.netUpdate = true;
							}
						}
						else if (Main.tile[num104, num105 - 1].HasUnactuatedTile && Main.tileSolid[Main.tile[num104, num105 - 1].TileType])
						{
							NPC.velocity.Y = -6f;
							NPC.netUpdate = true;
						}
						else if (NPC.position.Y + (float)NPC.height - (float)(num105 * 16) > 20f && Main.tile[num104, num105].HasUnactuatedTile && !Main.tile[num104, num105].TopSlope && Main.tileSolid[Main.tile[num104, num105].TileType])
						{
							NPC.velocity.Y = -5f;
							NPC.netUpdate = true;
						}
						else if (NPC.directionY < 0 && (!Main.tile[num104, num105 + 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num104, num105 + 1].TileType]) && (!Main.tile[num104 + NPC.direction, num105 + 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num104 + NPC.direction, num105 + 1].TileType]))
						{
							NPC.velocity.Y = -8f;
							NPC.velocity.X *= 1.5f;
							NPC.netUpdate = true;
						}
						else if (flag26)
						{
							NPC.ai[1] = 0f;
							NPC.ai[2] = 0f;
						}
						if (NPC.velocity.Y == 0f && flag24 && NPC.ai[3] == 1f)
						{
							NPC.velocity.Y = -5f;
						}
						if (NPC.velocity.Y == 0f && Main.expertMode && Main.player[NPC.target].Bottom.Y < NPC.Top.Y && Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) < (float)(Main.player[NPC.target].width * 3) && Collision.CanHit(NPC, Main.player[NPC.target]))
						{
							if (NPC.velocity.Y == 0f)
							{
								int num112 = 6;
								if (Main.player[NPC.target].Bottom.Y > NPC.Top.Y - (float)(num112 * 16))
								{
									NPC.velocity.Y = -7.9f;
								}
								else
								{
									int num113 = (int)(NPC.Center.X / 16f);
									int num114 = (int)(NPC.Bottom.Y / 16f) - 1;
									for (int num115 = num114; num115 > num114 - num112; num115--)
									{
										if (Main.tile[num113, num115].HasUnactuatedTile && TileID.Sets.Platforms[Main.tile[num113, num115].TileType])
										{
											NPC.velocity.Y = -7.9f;
											break;
										}
									}
								}
							}
						}
					}
					if (NPC.velocity.Y == 0f)
					{
						int num116 = 100;
						int num117 = 50;
						if (Math.Abs(NPC.position.X + (float)(NPC.width / 2) - (Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2))) < (float)num116 && Math.Abs(NPC.position.Y + (float)(NPC.height / 2) - (Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2))) < (float)num117 && ((NPC.direction > 0 && NPC.velocity.X >= 1f) || (NPC.direction < 0 && NPC.velocity.X <= -1f)))
						{
							NPC.velocity.X *= 2f;
							if (NPC.velocity.X > 3f)
							{
								NPC.velocity.X = 3f;
							}
							if (NPC.velocity.X < -3f)
							{
								NPC.velocity.X = -3f;
							}
							NPC.velocity.Y = -4f;
							NPC.netUpdate = true;
						}
					}
				}
			}
			else if (flag26)
			{
				NPC.ai[1] = 0f;
				NPC.ai[2] = 0f;
			}
			return false;
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
		{
			target.AddBuff(BuffID.Weak, 900);
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D glow = ModContent.Request<Texture2D>(Texture + "_Glow").Value;
			SpriteEffects spriteEffects = (SpriteEffects)0;
			if (NPC.spriteDirection == 1)
			{
				spriteEffects = (SpriteEffects)1;
			}
			float height = Main.NPCAddHeight(NPC);
			Vector2 halfSize = new Vector2(TextureAssets.Npc[Type].Width() / 2, TextureAssets.Npc[Type].Height() / Main.npcFrameCount[Type] / 2);
			spriteBatch.Draw(glow, NPC.Bottom - screenPos + new Vector2((float)(-TextureAssets.Npc[Type].Width()) * NPC.scale / 2f + halfSize.X * NPC.scale, (float)(-TextureAssets.Npc[Type].Height()) * NPC.scale / (float)Main.npcFrameCount[Type] + 4f + halfSize.Y * NPC.scale + height + NPC.gfxOffY), (Rectangle?)NPC.frame, new Color(200, 200, 200, 100), NPC.rotation, halfSize, NPC.scale, spriteEffects, 0f);
		}

		public override void HitEffect(NPC.HitInfo hit)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                return;
            }

			if (NPC.life > 0)
			{
				for (int i = 0; (double)i < hit.Damage / (double)NPC.lifeMax * 20.0; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, hit.HitDirection, -1f);
					if (Main.rand.NextBool(4))
					{
						Dust dust = Main.dust[Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.IceRod)];
						dust.noGravity = true;
						dust.scale = 1.5f;
						dust.fadeIn = 1f;
						dust.velocity *= 3f;
					}
				}
			}
			else
			{
				for (int i = 0; i < 20; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f);
					if (Main.rand.NextBool(3))
					{
						Dust dust = Main.dust[Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.IceRod)];
						dust.noGravity = true;
						dust.scale = 1.5f;
						dust.fadeIn = 1f;
						dust.velocity *= 3f;
					}
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SugarGhoulGore1").Type, NPC.scale);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SugarGhoulGore2").Type, NPC.scale);
				Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X, NPC.position.Y + 20f), NPC.velocity, Mod.Find<ModGore>("SugarGhoulGore3").Type, NPC.scale);
				Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X, NPC.position.Y + 20f), NPC.velocity, Mod.Find<ModGore>("SugarGhoulGore3").Type, NPC.scale);
				Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X, NPC.position.Y + 34f), NPC.velocity, Mod.Find<ModGore>("SugarGhoulGore4").Type, NPC.scale);
				Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X, NPC.position.Y + 34f), NPC.velocity, Mod.Find<ModGore>("SugarGhoulGore4").Type, NPC.scale);
			}
		}
    }
}

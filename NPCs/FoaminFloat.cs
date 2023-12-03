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

namespace TheConfectionRebirth.NPCs
{
    public class FoaminFloat : ModNPC
    {
		private bool suffocateAttack = false;

		private int suffocateImmunity = 0;

		private int oldDirectionPlayer;

		private int playerFacingLeft = 0;

		private int playerFacingRight = 0;

		private int shakeTimer = 0;

		private bool hasbegunshaking = false;

		private float State = 0f;

		public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new(0)
            {
                Position = new(0f, 6f),
                PortraitPositionYOverride = 0f
            });
        }

		public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 40;
            NPC.damage = 45;
            NPC.defense = 8;
            NPC.lifeMax = 140;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = 60f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0.5f;
			NPC.aiStyle = -1;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<FoaminFloatBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionUndergroundBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.FoaminFloat")
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && !spawnInfo.AnyInvasionActive() && Main.hardMode && (spawnInfo.Player.ZoneRockLayerHeight || spawnInfo.Player.ZoneUnderworldHeight)) {
                return 0.5f;
            }
            return 0f;
        }

		public override void SendExtraAI(BinaryWriter writer) {
			writer.Write(suffocateAttack);
			writer.Write(suffocateImmunity);
			writer.Write(oldDirectionPlayer);
			writer.Write(playerFacingLeft);
			writer.Write(playerFacingRight);
			writer.Write(shakeTimer);
			writer.Write(hasbegunshaking);
			writer.Write(State);
		}

		public override void ReceiveExtraAI(BinaryReader reader) {
			suffocateAttack = reader.ReadBoolean();
			suffocateImmunity = reader.ReadInt32();
			oldDirectionPlayer = reader.ReadInt32();
			playerFacingLeft = reader.ReadInt32();
			playerFacingRight = reader.ReadInt32();
			shakeTimer = reader.ReadInt32();
			hasbegunshaking = reader.ReadBoolean();
			State = reader.ReadInt32();
		}

		public override void AI() {
			if (suffocateAttack == false)
			{
				bool flag20 = false;
				if (State < 300f) {
					if (NPC.ai[2] >= 0f) {
						int num827 = 16;
						bool flag22 = false;
						bool flag23 = false;
						if (NPC.position.X > NPC.ai[0] - (float)num827 && NPC.position.X < NPC.ai[0] + (float)num827) {
							flag22 = true;
						}
						else if ((NPC.velocity.X < 0f && NPC.direction > 0) || (NPC.velocity.X > 0f && NPC.direction < 0)) {
							flag22 = true;
						}
						num827 += 24;
						if (NPC.position.Y > NPC.ai[1] - (float)num827 && NPC.position.Y < NPC.ai[1] + (float)num827) {
							flag23 = true;
						}
						if (flag22 && flag23) {
							NPC.ai[2] += 1f;
							if (NPC.ai[2] >= 30f && num827 == 16) {
								flag20 = true;
							}
							if (NPC.ai[2] >= 60f) {
								NPC.ai[2] = -200f;
								NPC.direction *= -1;
								NPC.velocity.X *= -1f;
								NPC.collideX = false;
							}
						}
						else {
							NPC.ai[0] = NPC.position.X;
							NPC.ai[1] = NPC.position.Y;
							NPC.ai[2] = 0f;
						}
						NPC.TargetClosest();
					}
					else {
						NPC.ai[2] += 1f;
						if (Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) > NPC.position.X + (float)(NPC.width / 2)) {
							NPC.direction = -1;
						}
						else {
							NPC.direction = 1;
						}
					}
				}
				else if (State >= 300f) {
					if (Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) > NPC.position.X + (float)(NPC.width / 2)) {
						NPC.direction = 1;
					}
					else {
						NPC.direction = -1;
					}
				}
				int num828 = (int)((NPC.position.X + (float)(NPC.width / 2)) / 16f) + NPC.direction * 2;
				int num831 = (int)((NPC.position.Y + (float)NPC.height) / 16f);
				bool flag25 = true;
				bool flag26 = false;
				int num832 = 3;
				num832 = ((!(Main.player[NPC.target].Center.Y < NPC.Center.Y)) ? 6 : 12);
				if (Main.netMode != 1 && !NPC.confused) {
					if (State < 300f) {
						NPC.ai[3] += 1f;
						if (NPC.justHit) {
							NPC.ai[3] = -45f;
							NPC.localAI[1] = 0f;
						}
						if (Main.netMode != 1 && NPC.ai[3] >= (float)(60 + Main.rand.Next(60))) {
							NPC.ai[3] = 0f;
							if (Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height)) {
								float num854 = 5f;
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
								int num860 = 40;
								int num861 = ModContent.ProjectileType<Projectiles.RootbeerSpray>();
								int num862 = Projectile.NewProjectile(new EntitySource_Misc(""), vector248.X, vector248.Y, num855, num857, num861, num860, 0f, Main.myPlayer);
							}
						}
					}
					if (State >= 600f || !Main.expertMode) {
						State = 0f;
					}
					else if (State <= 600) {
						State++;
					}
				}
				if (NPC.position.Y + (float)NPC.height > Main.player[NPC.target].position.Y) {
					for (int num865 = num831; num865 < num831 + num832; num865++) {
						if ((Main.tile[num828, num865].HasUnactuatedTile && Main.tileSolid[Main.tile[num828, num865].TileType]) || Main.tile[num828, num865].LiquidAmount > 0) {
							if (num865 <= num831 + 1) {
								flag26 = true;
							}
							flag25 = false;
							break;
						}
					}
				}
				if (Main.player[NPC.target].npcTypeNoAggro[NPC.type]) {
					bool flag27 = false;
					for (int num866 = num831; num866 < num831 + num832 - 2; num866++) {
						if ((Main.tile[num828, num866].HasUnactuatedTile && Main.tileSolid[Main.tile[num828, num866].TileType]) || Main.tile[num828, num866].LiquidAmount > 0) {
							flag27 = true;
							break;
						}
					}
					NPC.directionY = (!flag27).ToDirectionInt();
				}
				if (flag20) {
					flag26 = false;
					flag25 = true;
				}
				if (flag25) {
					NPC.velocity.Y += 0.1f;
					if (NPC.velocity.Y > 3f) {
						NPC.velocity.Y = 3f;
					}
				}
				else {
					if (NPC.directionY < 0 && NPC.velocity.Y > 0f) {
						NPC.velocity.Y -= 0.1f;
					}
					if (NPC.velocity.Y < -4f) {
						NPC.velocity.Y = -4f;
					}
				}
				if (NPC.collideX) {
					NPC.velocity.X = NPC.oldVelocity.X * -0.4f;
					if (NPC.direction == -1 && NPC.velocity.X > 0f && NPC.velocity.X < 1f) {
						NPC.velocity.X = 1f;
					}
					if (NPC.direction == 1 && NPC.velocity.X < 0f && NPC.velocity.X > -1f) {
						NPC.velocity.X = -1f;
					}
				}
				if (NPC.collideY) {
					NPC.velocity.Y = NPC.oldVelocity.Y * -0.25f;
					if (NPC.velocity.Y > 0f && NPC.velocity.Y < 1f) {
						NPC.velocity.Y = 1f;
					}
					if (NPC.velocity.Y < 0f && NPC.velocity.Y > -1f) {
						NPC.velocity.Y = -1f;
					}
				}
				float num868 = (State >= 300 ? 10f : 2f);
				if (NPC.direction == -1 && NPC.velocity.X > 0f - num868) {
					NPC.velocity.X -= 0.1f;
					if (NPC.velocity.X > num868) {
						NPC.velocity.X -= 0.1f;
					}
					else if (NPC.velocity.X > 0f) {
						NPC.velocity.X += 0.05f;
					}
					if (NPC.velocity.X < 0f - num868) {
						NPC.velocity.X = 0f - num868;
					}
				}
				else if (NPC.direction == 1 && NPC.velocity.X < num868) {
					NPC.velocity.X += 0.1f;
					if (NPC.velocity.X < 0f - num868) {
						NPC.velocity.X += 0.1f;
					}
					else if (NPC.velocity.X < 0f) {
						NPC.velocity.X -= 0.05f;
					}
					if (NPC.velocity.X > num868) {
						NPC.velocity.X = num868;
					}
				}
				num868 = 1.5f;
				if (NPC.directionY == -1 && NPC.velocity.Y > 0f - num868) {
					NPC.velocity.Y -= 0.04f;
					if (NPC.velocity.Y > num868) {
						NPC.velocity.Y -= 0.05f;
					}
					else if (NPC.velocity.Y > 0f) {
						NPC.velocity.Y += 0.03f;
					}
					if (NPC.velocity.Y < 0f - num868) {
						NPC.velocity.Y = 0f - num868;
					}
				}
				else if (NPC.directionY == 1 && NPC.velocity.Y < num868) {
					NPC.velocity.Y += 0.04f;
					if (NPC.velocity.Y < 0f - num868) {
						NPC.velocity.Y += 0.05f;
					}
					else if (NPC.velocity.Y < 0f) {
						NPC.velocity.Y -= 0.03f;
					}
					if (NPC.velocity.Y > num868) {
						NPC.velocity.Y = num868;
					}
				}
				suffocateImmunity++;
			}
			else if (suffocateAttack == true) {
				NPC.position = (Main.player[NPC.target].position + new Vector2(-10f, -20f));
				int BuffType = ModContent.BuffType<Buffs.FoaminSuffocation>();
				Main.player[NPC.target].AddBuff(BuffType, 10);
				Main.buffNoTimeDisplay[BuffType] = true;
				if (oldDirectionPlayer != Main.player[NPC.target].direction) {
					hasbegunshaking = true;
				}
				if (hasbegunshaking) {
					shakeTimer++;
					if (oldDirectionPlayer != Main.player[NPC.target].direction && Main.player[NPC.target].direction == 1) {
						playerFacingRight++;
					}
					if (oldDirectionPlayer != Main.player[NPC.target].direction && Main.player[NPC.target].direction == -1) {
						playerFacingLeft++;
					}
				}
				if ((shakeTimer >= 60 * 3 && playerFacingLeft < 10 && playerFacingRight < 10) || (shakeTimer > 60 && playerFacingLeft < 3 && playerFacingRight < 3)) {
					hasbegunshaking = false;
					shakeTimer = 0;
					playerFacingRight = 0;
					playerFacingLeft = 0;
				}
				if (shakeTimer >= 60 * 3) {
					if (playerFacingRight >= 10 && playerFacingLeft >= 10) {
						suffocateAttack = false;
						NPC.velocity.X = (Main.rand.NextBool(2) ? -15 : 15);
						hasbegunshaking = false;
						shakeTimer = 0;
						playerFacingRight = 0;
						playerFacingLeft = 0;
						State = 0f;
					}
				}
				oldDirectionPlayer = Main.player[NPC.target].direction;
				suffocateImmunity = 0;
			}
			if (Main.player[NPC.target].dead) {
				suffocateAttack = false;
			}
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) {
			if (suffocateAttack == true) {
				return false;
			}
			return true;
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo) {
			if (suffocateImmunity > 60) {
				suffocateAttack = true;
			}
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

            if (NPC.life <= 0)
            {
                var entitySource = NPC.GetSource_Death();

                for (int i = 0; i < 3; i++)
                {
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), 13);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), 12);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), 11);
                }
            }
        }
    }
}
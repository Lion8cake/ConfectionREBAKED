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

namespace TheConfectionRebirth.NPCs
{
    public class Sprinkling : ModNPC
    {
        internal sbyte Index;

		private int SprinkleAmmount = 5;
		private int SprinkleRegen = 0;
		private bool SprinkleShooting = false;

        public static Asset<Texture2D>[][] Assets;

		private int frame = 0;

        public override void Load()
        {
            Asset<Texture2D> wtf = ModContent.Request<Texture2D>(Texture);
            VariationManager<Sprinkling>.AddGroup("Normal", wtf);
            VariationManager<Sprinkling>.AddGroup("Corn", wtf, () => false && Main.halloween);
            VariationManager<Sprinkling>.AddGroup("Eye", wtf, () => Main.halloween);
            VariationManager<Sprinkling>.AddGroup("Gift", wtf, () => Main.xMas);

            if (Main.dedServ)
                return;

            Assets = new Asset<Texture2D>[VariationManager<Sprinkling>.Count][];
            for (int i = 0; i < Assets.GetLength(0); i++)
            {
                Assets[i] = new Asset<Texture2D>[3];
                for (int j = 0; j < 3; j++)
                {
                    Assets[i][j] = ModContent.Request<Texture2D>($"TheConfectionRebirth/NPCs/Sprinkler/Sprinkling_{i}_{j}");
                }
            }
        }

		public override void Unload()
		{
			VariationManager<Sprinkling>.Clear();
            Assets = null;
		}

		public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 10;
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new(0)
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
            Index = -1;
        }

        public override bool PreAI()
        {
            if (Index == -1)
            {
                Index = (sbyte)VariationManager<Sprinkling>.GetRandomGroup().Index;

                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
            }

            return true;
        }

		public override void FindFrame(int frameHeight) {
			NPC.frameCounter++;
			if (NPC.frameCounter >= 8) {
				frame++;
				NPC.frameCounter = 0;
			}
			if (frame >= 10 && NPC.frameCounter >= 8) {
				frame = 0;
			}
			if (NPC.frame.Y >= 500) {
				frame = 0;
			}
			NPC.frame.Y = frameHeight * frame;
		}

		public override void AI() {
			bool flag20 = false;
			if (NPC.justHit) {
				NPC.ai[2] = 0f;
			}
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
			int num828 = (int)((NPC.position.X + (float)(NPC.width / 2)) / 16f) + NPC.direction * 2;
			int num831 = (int)((NPC.position.Y + (float)NPC.height) / 16f);
			bool flag25 = true;
			bool flag26 = false;
			int num832 = 4;
			NPC.position += NPC.netOffset;
			NPC.position -= NPC.netOffset;
			if (NPC.position.Y + (float)NPC.height > Main.player[NPC.target].position.Y) {
				for (int num865 = num831; num865 < num831 + num832; num865++) {
					if ((Main.tile[num828, num865].HasUnactuatedTile && Main.tileSolid[Main.tile[num828, num865].TileType]) || Main.tile[num828, num865].LiquidType > 0) {
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
					if ((Main.tile[num828, num866].HasUnactuatedTile && Main.tileSolid[Main.tile[num828, num866].TileType]) || Main.tile[num828, num866].LiquidType > 0) {
						flag27 = true;
						break;
					}
				}
				NPC.directionY = (!flag27).ToDirectionInt();
			}
			if (flag20) {
				flag26 = false;
				flag25 = true;
				NPC.velocity.Y += 2f;
			}
			if (flag25) {
				NPC.velocity.Y += 0.2f;
				if (NPC.velocity.Y > 2f) {
					NPC.velocity.Y = 2f;
				}
			}
			else {
				if ((NPC.directionY < 0 && NPC.velocity.Y > 0f) || flag26) {
					NPC.velocity.Y -= 0.2f;
				}
			}
			if (NPC.wet) {
				NPC.velocity.Y -= 0.2f;
				if (NPC.velocity.Y < -2f) {
					NPC.velocity.Y = -2f;
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
			float num868 = 3f;
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
			num868 = 1.4f;
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

			//Sprinkler AI stuff
			SprinkleRegen++;
			if (SprinkleAmmount != 5 && SprinkleRegen == 300 && SprinkleShooting == false) {
				SprinkleAmmount++;
				SprinkleRegen = 0;
			}
			NPC.defense = 8 * SprinkleAmmount;
			if (SprinkleAmmount == 5) {
				SprinkleShooting = true;
			}
			else if (SprinkleAmmount == 0) {
				SprinkleShooting = false;
				if (SprinkleRegen > 300) {
					SprinkleRegen = 0;
				}
			}
			if (SprinkleShooting) {
				float num845 = 15f;
				Vector2 ProjectilePos = new(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
				float SpeedX = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - ProjectilePos.X;
				float SpeedY = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - ProjectilePos.Y;
				float num848 = (float)Math.Sqrt(SpeedX * SpeedX + SpeedY * SpeedY);
				float num849 = num848;
				num848 = num845 / num848;
				SpeedX *= num848;
				SpeedY *= -num848;

				if (NPC.ai[3] >= 261) {
					NPC.ai[3] = 0;
				}

				if (Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height)) {
					if (NPC.ai[3] >= 0) {
						NPC.ai[3]++;
					}
					if (Main.netMode != 1) {
						int num286 = ModContent.ProjectileType<Projectiles.DefenciveSprinkle>();
						int num285 = 60;
						if (NPC.ai[3] == 201f) {
							int num287 = Projectile.NewProjectile(NPC.GetSource_FromAI(), ProjectilePos.X, ProjectilePos.Y, SpeedX, SpeedY, num286, num285, 0f, Main.myPlayer);
							Main.projectile[num287].friendly = false;
							Main.projectile[num287].frame = Index * 5;
							NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, num287);
							NPC.netUpdate = true;
							SoundEngine.PlaySound(SoundID.Item5, NPC.position);
							SprinkleAmmount--;
						}
						if (NPC.ai[3] == 216f) {
							int num287 = Projectile.NewProjectile(NPC.GetSource_FromAI(), ProjectilePos.X, ProjectilePos.Y, SpeedX, SpeedY, num286, num285, 0f, Main.myPlayer);
							Main.projectile[num287].friendly = false;
							Main.projectile[num287].frame = Index * 5 + 1;
							NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, num287);
							NPC.netUpdate = true;
							SoundEngine.PlaySound(SoundID.Item5, NPC.position);
							SprinkleAmmount--;
						}
						if (NPC.ai[3] == 231f) {
							int num287 = Projectile.NewProjectile(NPC.GetSource_FromAI(), ProjectilePos.X, ProjectilePos.Y, SpeedX, SpeedY, num286, num285, 0f, Main.myPlayer);
							Main.projectile[num287].friendly = false;
							Main.projectile[num287].frame = Index * 5 + 2;
							NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, num287);
							NPC.netUpdate = true;
							SoundEngine.PlaySound(SoundID.Item5, NPC.position);
							SprinkleAmmount--;
						}
						if (NPC.ai[3] == 246f) {
							int num287 = Projectile.NewProjectile(NPC.GetSource_FromAI(), ProjectilePos.X, ProjectilePos.Y, SpeedX, SpeedY, num286, num285, 0f, Main.myPlayer);
							Main.projectile[num287].friendly = false;
							Main.projectile[num287].frame = Index * 5 + 3;
							NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, num287);
							NPC.netUpdate = true;
							SoundEngine.PlaySound(SoundID.Item5, NPC.position);
							SprinkleAmmount--;
						}
						if (NPC.ai[3] == 261f) {
							int num287 = Projectile.NewProjectile(NPC.GetSource_FromAI(), ProjectilePos.X, ProjectilePos.Y, SpeedX, SpeedY, num286, num285, 0f, Main.myPlayer);
							Main.projectile[num287].friendly = false;
							Main.projectile[num287].frame = Index * 5 + 4;
							NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, num287);
							NPC.netUpdate = true;
							SoundEngine.PlaySound(SoundID.Item5, NPC.position);
							SprinkleAmmount--;
						}
					}
				}
			}
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

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            Rectangle frame = NPC.frame;
            Vector2 pos = NPC.Center - screenPos;
            pos.Y += NPC.gfxOffY - 4f;
            if (NPC.IsABestiaryIconDummy)
            {
				spriteBatch.Draw(texture, pos, frame, drawColor, NPC.rotation, NPC.frame.Size() * 0.5f, NPC.scale, DS.FlipTex(NPC.direction), 0f);
                return true;
            }

            int index = Utils.Clamp(Index, 0, 4);
            if (index == 4)
                index = 0;

			//Texture2D back = Assets[index][2].Value; //may be slightly better code wise but personally i cant decipher what its doing
			//Texture2D front = Assets[index][1].Value; //mf when he says I need to start adding more explinations to my code and i have no shit idea on how variations work
			Texture2D back = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/NPCs/Sprinkler/SprinklingSprinkleAmmount/Sprinkling_" + Index + "_" + SprinkleAmmount + "_" + 2);
			Texture2D front = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/NPCs/Sprinkler/SprinklingSprinkleAmmount/Sprinkling_" + Index + "_" + SprinkleAmmount + "_" + 1);
			texture = Assets[index][0].Value;
            frame.Y %= front.Height;

            spriteBatch.Draw(back, pos, frame, drawColor, NPC.rotation, frame.Size() * 0.5f, NPC.scale, DS.FlipTex(NPC.direction), 0f);
            spriteBatch.Draw(texture, pos, new(0, 0, 58, 50), drawColor, NPC.rotation, new(29, 25), NPC.scale, DS.FlipTex(NPC.direction), 0f);
            spriteBatch.Draw(front, pos, frame, drawColor, NPC.rotation, frame.Size() * 0.5f, NPC.scale, DS.FlipTex(NPC.direction), 0f);
            return false;
        }

        public override void SendExtraAI(BinaryWriter writer) => writer.Write(Index);

        public override void ReceiveExtraAI(BinaryReader reader) => Index = reader.ReadSByte();
    }
}
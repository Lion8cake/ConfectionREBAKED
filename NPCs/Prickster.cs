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
using TheConfectionRebirth.Items.Armor;
using TheConfectionRebirth.Items.Banners;
using TheConfectionRebirth.Items.Placeable;

namespace TheConfectionRebirth.NPCs
{
    public class Prickster : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Prickster");
            Main.npcFrameCount[NPC.type] = 6;
        }

        private Player targetedPlayer;

        private int numHops = 0, numBounces = 0, maxHops = 2, maxBounces = 3;

        private int directionBeforeBounce = 0;

        private bool noticed = false;

        private bool launched = false;

        private const int AI_State_Slot = 0;
        private const int AI_Timer_Slot = 1;

        private const int State_Asleep = 0;
        private const int State_Notice = 1;
        private const int State_Walk = 2;
        private const int State_Bounce = 3;

        public float AI_State
        {
            get => NPC.ai[AI_State_Slot];
            set => NPC.ai[AI_State_Slot] = value;
        }
        public float AI_Timer
        {
            get => NPC.ai[AI_Timer_Slot];
            set => NPC.ai[AI_Timer_Slot] = value;
        }


        public override void SetDefaults()
        {
            NPC.width = 58;
            NPC.height = 52;
            NPC.damage = 75;
            NPC.defense = 35;
            NPC.lifeMax = 220;
            NPC.HitSound = SoundID.NPCHit24;
            NPC.DeathSound = SoundID.NPCDeath27;
            NPC.value = 500f; ; // amount of money dropped
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1; // custom ai
            NPC.noTileCollide = false;
            AIType = NPCID.GiantTortoise;
            AnimationType = NPCID.GiantTortoise;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<PricksterBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionUndergroundBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("A tortois descuised as a pile of scharrarite ready to attack when disturbed.")
            });
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.Player.ZoneRockLayerHeight && spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiomeBiome>()))
			{
				return 0.05f;
			}
			return 0f;
		}

        public override int SpawnNPC(int tileX, int tileY) // when the NPC is spawned
        {
            return base.SpawnNPC(tileX, tileY); // set to either 2x3 size or 3x2 size anywhere not inside of a block
        }

        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            if (noticed == false)
            {
                targetedPlayer = player;
                AI_State = State_Notice;
                noticed = true;
            }
        }

        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            if (noticed == false)
            {
                targetedPlayer = Main.player[NPC.FindClosestPlayer()];
                AI_State = State_Notice;
                noticed = true;
            }
        }

        /*public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.AdhesiveBandage, 1, 1, 100));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Saccharite>(), 5, 13, 1));
		}*/

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
			npcLoot.Add(ItemDropRule.Common(ItemID.AdhesiveBandage, 100, 1, 1));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Saccharite>(), 1, 5, 12));
        }

        public override void AI()
        {
            if (AI_State != State_Asleep)
            {
                if (NPC.position.X > targetedPlayer.position.X)
                {
                    NPC.direction = 1;
                }
                else
                {
                    NPC.direction = -1;
                }
            }

            if (AI_State == State_Asleep)
            {
                // do nothing :)
            }

            // In this state, a player has been targeted
            else if (AI_State == State_Notice)
            {
                if (targetedPlayer.Distance(NPC.Center) < 500f)
                {
                    NPC.velocity = new Vector2(0f, 0f);
                    AI_State = State_Walk;
                }
            }

            else if (AI_State == State_Walk) // walk in this case is a short hop
            {
                AI_Timer++;
                if (NPC.velocity.Y == 0)
                {
                    NPC.rotation = 0f;
                }

                if (numHops == maxHops && NPC.velocity.Y == 0) // hops 3 times then goes to bounce state
                {
                    AI_State = State_Bounce;
                    numHops = 0;
                    AI_Timer = 0;
                }
                else if (NPC.velocity.Y == 0 && AI_Timer % 100 == 0) // if on ground and after 100 ticks
                {
                    NPC.velocity += new Vector2(-NPC.direction * 4, -5f);
                    numHops++;
                }

                if (NPC.velocity.Y == 0)
                {
                    NPC.velocity.X = 0;
                }
            }

            else if (AI_State == State_Bounce)
            {
                AI_Timer++;

                if (numBounces >= maxBounces && (NPC.velocity == new Vector2(0f, 0f) && launched == true))
                {
                    NPC.HealEffect(numBounces);
                    ResetValuesAfterBounce();
                }

                if (NPC.velocity.Y != 0 && (NPC.collideX == false && NPC.collideY == false)) // if in air
                {
                    NPC.rotation += directionBeforeBounce + NPC.velocity.X / (AI_Timer * 0.5f); // SPINNIES!!
                }
                else if (NPC.velocity.Y == 0) // if on ground
                {
                    if (AI_Timer % 100 == 0)
                    {
                        if (Above(NPC.position.ToTileCoordinates().Y + 1 /*adjust for height ig idfk*/, targetedPlayer.position.ToTileCoordinates().Y) != false) // if player is not below NPC
                        {
                            if (NPC.position.X > targetedPlayer.position.X)
                            {
                                directionBeforeBounce = 1;
                            }
                            else
                            {
                                directionBeforeBounce = -1;
                            }
                            NPC.velocity += new Vector2(-NPC.direction * Math.Abs(NPC.position.ToTileCoordinates().X - targetedPlayer.position.ToTileCoordinates().X) / 2f, Math.Abs(NPC.position.ToTileCoordinates().Y - targetedPlayer.position.ToTileCoordinates().Y)); // hopefully go flying towards the player
                            launched = true;
                        }
                        else
                        {
                            if (NPC.position.X > targetedPlayer.position.X)
                            {
                                directionBeforeBounce = 1;
                            }
                            else
                            {
                                directionBeforeBounce = -1;
                            }
                            NPC.velocity += new Vector2(-NPC.direction * Math.Abs(NPC.position.X - targetedPlayer.position.X) * 0.03f, 5f); // hopefully go flying towards the player
                            launched = true;
                        }
                    }
                }
                if (NPC.collideX && numBounces < maxBounces && NPC.velocity.X != 0 && NPC.velocity.Y != 0)
                {
                    NPC.velocity.X = NPC.velocity.X * directionBeforeBounce;
                    numBounces++;
                }

                if ((NPC.velocity.Y == 0 && NPC.velocity.X != 0) || (numBounces < maxBounces && NPC.velocity == new Vector2(0f, 0f) && launched == true)) // we dont want the thing on a slip n slide!
                {
                    ResetValuesAfterBounce();
                }

                if (NPC.collideY && numBounces < maxBounces && NPC.velocity.Y != 0)
                {
                    NPC.velocity.Y = -NPC.velocity.Y;
                    numBounces++;
                }
            }
        }

        private bool Above(float val1, float val2)
        {
            return val1 > val2;
        }

        private void ResetValuesAfterBounce()
        {
            NPC.velocity = new Vector2(0, 0);
            AI_State = State_Walk;
            numBounces = 0;
            AI_Timer = 0;
            launched = false;
            NPC.rotation = 0f;
        }

        private const int Frame_Asleep = 0;
        private const int Frame_Notice_1 = 1;
        private const int Frame_Notice_2 = 2;
        private const int Frame_Walk_1 = 3;
        private const int Frame_Walk_2 = 4;
        private const int Frame_Bounce = 5;

        public override void FindFrame(int frameHeight)
        {
            NPC.frame = new Rectangle(0, 0, 58, 52);
            NPC.spriteDirection = NPC.direction;

            if (AI_State == State_Asleep)
            {
                NPC.frame.Y = Frame_Asleep * frameHeight;
            }
            else if (AI_State == State_Notice)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter < 5)
                {
                    NPC.frame.Y = Frame_Notice_1 * frameHeight;
                }
                else if (NPC.frameCounter < 10)
                {
                    NPC.frame.Y = Frame_Notice_2 * frameHeight;
                }
                else
                {
                    NPC.frameCounter = 0;
                }
            }
            else if (AI_State == State_Bounce)
            {
                NPC.frame.Y = Frame_Bounce * frameHeight;
            }
            else if (AI_State == State_Walk)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter < 10)
                {
                    NPC.frame.Y = Frame_Walk_1 * frameHeight;
                }
                else if (NPC.frameCounter < 20)
                {
                    NPC.frame.Y = Frame_Walk_2 * frameHeight;
                }
                else
                {
                    NPC.frameCounter = 0;
                }
            }
        }
    }
}

/*
 * AISTYLE 39, which is implimented by giant tortoise and ice tortoise
 * if (this.target < 0 || Main.player[this.target].dead || base.direction == 0)
				{
					this.TargetClosest();
				}
				bool flag29 = true;
				int num537 = 0;
				if (base.velocity.X < 0f)
				{
					num537 = -1;
				}
				if (base.velocity.X > 0f)
				{
					num537 = 1;
				}
				Vector2 vector66 = base.position;
				vector66.X += base.velocity.X;
				int num538 = (int)((vector66.X + (float)(base.width / 2) + (float)((base.width / 2 + 1) * num537)) / 16f);
				int num539 = (int)((vector66.Y + (float)base.height - 1f) / 16f);
				if ((float)(num538 * 16) < vector66.X + (float)base.width && (float)(num538 * 16 + 16) > vector66.X && ((Main.tile[num538, num539].nactive() && !Main.tile[num538, num539].topSlope() && !Main.tile[num538, num539 - 1].topSlope() && ((Main.tileSolid[Main.tile[num538, num539].type] && !Main.tileSolidTop[Main.tile[num538, num539].type]) || (flag29 && Main.tileSolidTop[Main.tile[num538, num539].type] && (!Main.tileSolid[Main.tile[num538, num539 - 1].type] || !Main.tile[num538, num539 - 1].nactive()) && Main.tile[num538, num539].type != 16 && Main.tile[num538, num539].type != 18 && Main.tile[num538, num539].type != 134))) || (Main.tile[num538, num539 - 1].halfBrick() && Main.tile[num538, num539 - 1].nactive())) && (!Main.tile[num538, num539 - 1].nactive() || !Main.tileSolid[Main.tile[num538, num539 - 1].type] || Main.tileSolidTop[Main.tile[num538, num539 - 1].type] || (Main.tile[num538, num539 - 1].halfBrick() && (!Main.tile[num538, num539 - 4].nactive() || !Main.tileSolid[Main.tile[num538, num539 - 4].type] || Main.tileSolidTop[Main.tile[num538, num539 - 4].type]))) && (!Main.tile[num538, num539 - 2].nactive() || !Main.tileSolid[Main.tile[num538, num539 - 2].type] || Main.tileSolidTop[Main.tile[num538, num539 - 2].type]) && (!Main.tile[num538, num539 - 3].nactive() || !Main.tileSolid[Main.tile[num538, num539 - 3].type] || Main.tileSolidTop[Main.tile[num538, num539 - 3].type]) && (!Main.tile[num538 - num537, num539 - 3].nactive() || !Main.tileSolid[Main.tile[num538 - num537, num539 - 3].type] || Main.tileSolidTop[Main.tile[num538 - num537, num539 - 3].type]))
				{
					float num540 = num539 * 16;
					if (Main.tile[num538, num539].halfBrick())
					{
						num540 += 8f;
					}
					if (Main.tile[num538, num539 - 1].halfBrick())
					{
						num540 -= 8f;
					}
					if (num540 < vector66.Y + (float)base.height)
					{
						float num541 = vector66.Y + (float)base.height - num540;
						if ((double)num541 <= 16.1)
						{
							this.gfxOffY += base.position.Y + (float)base.height - num540;
							base.position.Y = num540 - (float)base.height;
							if (num541 < 9f)
							{
								this.stepSpeed = 0.75f;
							}
							else
							{
								this.stepSpeed = 1.5f;
							}
						}
					}
				}
				if (this.justHit && this.type != 417)
				{
					this.ai[0] = 0f;
					this.ai[1] = 0f;
					this.TargetClosest();
				}
				if (this.type == 154 && Main.rand.Next(10) == 0)
				{
					int num542 = Dust.NewDust(new Vector2(base.position.X, base.position.Y), base.width, base.height, 67, base.velocity.X * 0.5f, base.velocity.Y * 0.5f, 90, default(Color), 1.5f);
					Main.dust[num542].noGravity = true;
					Dust dust32 = Main.dust[num542];
					Dust dust2 = dust32;
					dust2.velocity *= 0.2f;
				}
				if (this.ai[0] == 0f)
				{
					if (base.velocity.X < 0f)
					{
						base.direction = -1;
					}
					else if (base.velocity.X > 0f)
					{
						base.direction = 1;
					}
					this.spriteDirection = base.direction;
					Vector2 vector67 = new Vector2(base.position.X + (float)base.width * 0.5f, base.position.Y + (float)base.height * 0.5f);
					float num543 = Main.player[this.target].position.X + (float)Main.player[this.target].width * 0.5f - vector67.X;
					float num544 = Main.player[this.target].position.Y - vector67.Y;
					float num545 = (float)Math.Sqrt(num543 * num543 + num544 * num544);
					bool flag30 = Collision.CanHit(base.position, base.width, base.height, Main.player[this.target].position, Main.player[this.target].width, Main.player[this.target].height);
					if (this.type >= 496 && this.type <= 497)
					{
						if (num545 > 200f && flag30)
						{
							this.ai[1] += 2f;
						}
						if (num545 > 600f && (flag30 || base.position.Y + (float)base.height > Main.player[this.target].position.Y - 200f))
						{
							this.ai[1] += 4f;
						}
					}
					else
					{
						if (num545 > 200f && flag30)
						{
							this.ai[1] += 4f;
						}
						if (num545 > 600f && (flag30 || base.position.Y + (float)base.height > Main.player[this.target].position.Y - 200f))
						{
							this.ai[1] += 10f;
						}
						if (base.wet)
						{
							this.ai[1] = 1000f;
						}
					}
					this.defense = this.defDefense;
					this.damage = this.defDamage;
					if (this.type >= 496 && this.type <= 497)
					{
						this.knockBackResist = 0.75f * Main.knockBackMultiplier;
					}
					else
					{
						this.knockBackResist = 0.3f * Main.knockBackMultiplier;
					}
					this.ai[1] += 1f;
					if (this.ai[1] >= 400f)
					{
						this.ai[1] = 0f;
						this.ai[0] = 1f;
					}
					if (!this.justHit && base.velocity.X != base.oldVelocity.X)
					{
						base.direction *= -1;
					}
					if (base.velocity.Y == 0f && Main.player[this.target].position.Y < base.position.Y + (float)base.height)
					{
						int num546;
						int num547;
						if (base.direction > 0)
						{
							num546 = (int)(((double)base.position.X + (double)base.width * 0.5) / 16.0);
							num547 = num546 + 3;
						}
						else
						{
							num547 = (int)(((double)base.position.X + (double)base.width * 0.5) / 16.0);
							num546 = num547 - 3;
						}
						int num548 = (int)((base.position.Y + (float)base.height + 2f) / 16f) - 1;
						int num549 = num548 + 4;
						bool flag31 = false;
						for (int num550 = num546; num550 <= num547; num550++)
						{
							for (int num551 = num548; num551 <= num549; num551++)
							{
								if (Main.tile[num550, num551] != null && Main.tile[num550, num551].nactive() && Main.tileSolid[Main.tile[num550, num551].type])
								{
									flag31 = true;
								}
							}
						}
						if (!flag31)
						{
							base.direction *= -1;
							base.velocity.X = 0.1f * (float)base.direction;
						}
					}
					if (this.type >= 496 && this.type <= 497)
					{
						float num552 = 0.5f;
						if (base.velocity.X < 0f - num552 || base.velocity.X > num552)
						{
							if (base.velocity.Y == 0f)
							{
								base.velocity *= 0.8f;
							}
						}
						else if (base.velocity.X < num552 && base.direction == 1)
						{
							base.velocity.X += 0.07f;
							if (base.velocity.X > num552)
							{
								base.velocity.X = num552;
							}
						}
						else if (base.velocity.X > 0f - num552 && base.direction == -1)
						{
							base.velocity.X -= 0.07f;
							if (base.velocity.X < 0f - num552)
							{
								base.velocity.X = 0f - num552;
							}
						}
						return;
					}
					float num553 = 1f;
					if (num545 < 400f)
					{
						if (base.velocity.X < 0f - num553 || base.velocity.X > num553)
						{
							if (base.velocity.Y == 0f)
							{
								base.velocity *= 0.8f;
							}
						}
						else if (base.velocity.X < num553 && base.direction == 1)
						{
							base.velocity.X += 0.07f;
							if (base.velocity.X > num553)
							{
								base.velocity.X = num553;
							}
						}
						else if (base.velocity.X > 0f - num553 && base.direction == -1)
						{
							base.velocity.X -= 0.07f;
							if (base.velocity.X < 0f - num553)
							{
								base.velocity.X = 0f - num553;
							}
						}
					}
					else if (base.velocity.X < -1.5f || base.velocity.X > 1.5f)
					{
						if (base.velocity.Y == 0f)
						{
							base.velocity *= 0.8f;
						}
					}
					else if (base.velocity.X < 1.5f && base.direction == 1)
					{
						base.velocity.X += 0.07f;
						if (base.velocity.X > 1.5f)
						{
							base.velocity.X = 1.5f;
						}
					}
					else if (base.velocity.X > -1.5f && base.direction == -1)
					{
						base.velocity.X -= 0.07f;
						if (base.velocity.X < -1.5f)
						{
							base.velocity.X = -1.5f;
						}
					}
				}
				else if (this.ai[0] == 1f)
				{
					base.velocity.X *= 0.5f;
					if (this.type >= 496 && this.type <= 497)
					{
						this.ai[1] += 0.5f;
					}
					else
					{
						this.ai[1] += 1f;
					}
					if (this.ai[1] >= 30f)
					{
						this.netUpdate = true;
						this.TargetClosest();
						this.ai[1] = 0f;
						this.ai[2] = 0f;
						this.ai[0] = 3f;
						if (this.type == 417)
						{
							this.ai[0] = 6f;
							this.ai[2] = Main.rand.Next(2, 5);
						}
					}
				}
				else if (this.ai[0] == 3f)
				{
					if (this.type == 154 && Main.rand.Next(3) < 2)
					{
						int num554 = Dust.NewDust(new Vector2(base.position.X, base.position.Y), base.width, base.height, 67, base.velocity.X * 0.5f, base.velocity.Y * 0.5f, 90, default(Color), 1.5f);
						Main.dust[num554].noGravity = true;
						Dust dust33 = Main.dust[num554];
						Dust dust2 = dust33;
						dust2.velocity *= 0.2f;
					}
					if (Main.expertMode)
					{
						if (this.type >= 496 && this.type <= 497)
						{
							this.damage = (int)((double)this.defDamage * 1.5 * 0.9);
						}
						else
						{
							this.damage = (int)((double)(this.defDamage * 2) * 0.9);
						}
					}
					else if (this.type >= 496 && this.type <= 497)
					{
						this.damage = (int)((double)this.defDamage * 1.5);
					}
					else
					{
						this.damage = this.defDamage * 2;
					}
					this.defense = this.defDefense * 2;
					this.ai[1] += 1f;
					if (this.ai[1] == 1f)
					{
						this.netUpdate = true;
						this.TargetClosest();
						this.ai[2] += 0.3f;
						this.rotation += this.ai[2] * (float)base.direction;
						this.ai[1] += 1f;
						bool flag32 = Collision.CanHit(base.position, base.width, base.height, Main.player[this.target].position, Main.player[this.target].width, Main.player[this.target].height);
						float num555 = 10f;
						if (!flag32)
						{
							num555 = 6f;
						}
						if (this.type >= 496 && this.type <= 497)
						{
							num555 *= 0.75f;
						}
						Vector2 vector68 = new Vector2(base.position.X + (float)base.width * 0.5f, base.position.Y + (float)base.height * 0.5f);
						float num556 = Main.player[this.target].position.X + (float)Main.player[this.target].width * 0.5f - vector68.X;
						float num557 = Math.Abs(num556) * 0.2f;
						if (this.directionY > 0)
						{
							num557 = 0f;
						}
						float num558 = Main.player[this.target].position.Y - vector68.Y - num557;
						float num559 = (float)Math.Sqrt(num556 * num556 + num558 * num558);
						this.netUpdate = true;
						num559 = num555 / num559;
						num556 *= num559;
						num558 *= num559;
						if (!flag32)
						{
							num558 = -10f;
						}
						base.velocity.X = num556;
						base.velocity.Y = num558;
						this.ai[3] = base.velocity.X;
					}
					else
					{
						if (base.position.X + (float)base.width > Main.player[this.target].position.X && base.position.X < Main.player[this.target].position.X + (float)Main.player[this.target].width && base.position.Y < Main.player[this.target].position.Y + (float)Main.player[this.target].height)
						{
							base.velocity.X *= 0.8f;
							this.ai[3] = 0f;
							if (base.velocity.Y < 0f)
							{
								base.velocity.Y += 0.2f;
							}
						}
						if (this.ai[3] != 0f)
						{
							base.velocity.X = this.ai[3];
							base.velocity.Y -= 0.22f;
						}
						if (this.ai[1] >= 90f)
						{
							this.noGravity = false;
							this.ai[1] = 0f;
							this.ai[0] = 4f;
						}
					}
					if (base.wet && this.directionY < 0)
					{
						base.velocity.Y -= 0.3f;
					}
					this.rotation += this.ai[2] * (float)base.direction;
				}
				else if (this.ai[0] == 4f)
				{
					if (base.wet && this.directionY < 0)
					{
						base.velocity.Y -= 0.3f;
					}
					base.velocity.X *= 0.96f;
					if (this.ai[2] > 0f)
					{
						this.ai[2] -= 0.01f;
						this.rotation += this.ai[2] * (float)base.direction;
					}
					else if (base.velocity.Y >= 0f)
					{
						this.rotation = 0f;
					}
					if (this.ai[2] <= 0f && (base.velocity.Y == 0f || base.wet))
					{
						this.netUpdate = true;
						this.rotation = 0f;
						this.ai[2] = 0f;
						this.ai[1] = 0f;
						this.ai[0] = 5f;
					}
				}
				else if (this.ai[0] == 6f)
				{
					this.damage = (int)((float)this.defDamage * (Main.expertMode ? 1.4f : 1.8f));
					this.defense = this.defDefense * 2;
					this.knockBackResist = 0f;
					if (Main.rand.Next(3) < 2)
					{
						int num560 = Dust.NewDust(base.Center - new Vector2(30f), 60, 60, 6, base.velocity.X * 0.5f, base.velocity.Y * 0.5f, 90, default(Color), 1.5f);
						Main.dust[num560].noGravity = true;
						Dust dust34 = Main.dust[num560];
						Dust dust2 = dust34;
						dust2.velocity *= 0.2f;
						Main.dust[num560].fadeIn = 1f;
					}
					this.ai[1] += 1f;
					if (this.ai[3] > 0f)
					{
						if (this.ai[3] == 1f)
						{
							Vector2 vector69 = base.Center - new Vector2(50f);
							for (int num561 = 0; num561 < 32; num561++)
							{
								int num562 = Dust.NewDust(vector69, 100, 100, 6, 0f, 0f, 100, default(Color), 2.5f);
								Main.dust[num562].noGravity = true;
								Dust dust35 = Main.dust[num562];
								Dust dust2 = dust35;
								dust2.velocity *= 3f;
								num562 = Dust.NewDust(vector69, 100, 100, 6, 0f, 0f, 100, default(Color), 1.5f);
								dust35 = Main.dust[num562];
								dust2 = dust35;
								dust2.velocity *= 2f;
								Main.dust[num562].noGravity = true;
							}
							for (int num563 = 0; num563 < 4; num563++)
							{
								int num564 = Gore.NewGore(vector69 + new Vector2((float)(50 * Main.rand.Next(100)) / 100f, (float)(50 * Main.rand.Next(100)) / 100f) - Vector2.One * 10f, default(Vector2), Main.rand.Next(61, 64));
								Gore gore = Main.gore[num564];
								Gore gore2 = gore;
								gore2.velocity *= 0.3f;
								Main.gore[num564].velocity.X += (float)Main.rand.Next(-10, 11) * 0.05f;
								Main.gore[num564].velocity.Y += (float)Main.rand.Next(-10, 11) * 0.05f;
							}
						}
						for (int num565 = 0; num565 < 5; num565++)
						{
							int num566 = Dust.NewDust(base.position, base.width, base.height, 31, 0f, 0f, 100, default(Color), 1.5f);
							Main.dust[num566].velocity = Main.dust[num566].velocity * Main.rand.NextFloat();
						}
						this.ai[3]++;
						if (this.ai[3] >= 10f)
						{
							this.ai[3] = 0f;
						}
					}
					if (this.ai[1] == 1f)
					{
						this.netUpdate = true;
						this.TargetClosest();
						bool flag33 = Collision.CanHit(base.position, base.width, base.height, Main.player[this.target].position, Main.player[this.target].width, Main.player[this.target].height);
						float num567 = 16f;
						if (!flag33)
						{
							num567 = 10f;
						}
						Vector2 vector70 = new Vector2(base.position.X + (float)base.width * 0.5f, base.position.Y + (float)base.height * 0.5f);
						float num568 = Main.player[this.target].position.X + (float)Main.player[this.target].width * 0.5f - vector70.X;
						float num569 = Math.Abs(num568) * 0.2f;
						if (this.directionY > 0)
						{
							num569 = 0f;
						}
						float num570 = Main.player[this.target].position.Y - vector70.Y - num569;
						float num571 = (float)Math.Sqrt(num568 * num568 + num570 * num570);
						this.netUpdate = true;
						num571 = num567 / num571;
						num568 *= num571;
						num570 *= num571;
						if (!flag33)
						{
							num570 = -12f;
						}
						base.velocity.X = num568;
						base.velocity.Y = num570;
					}
					else
					{
						if (base.position.X + (float)base.width > Main.player[this.target].position.X && base.position.X < Main.player[this.target].position.X + (float)Main.player[this.target].width && base.position.Y < Main.player[this.target].position.Y + (float)Main.player[this.target].height)
						{
							base.velocity.X *= 0.9f;
							if (base.velocity.Y < 0f)
							{
								base.velocity.Y += 0.2f;
							}
						}
						if (this.ai[2] == 0f || this.ai[1] >= 1200f)
						{
							this.ai[1] = 0f;
							this.ai[0] = 5f;
						}
					}
					if (base.wet && this.directionY < 0)
					{
						base.velocity.Y -= 0.3f;
					}
					this.rotation += MathHelper.Clamp(base.velocity.X / 10f * (float)base.direction, -(float)Math.PI / 10f, (float)Math.PI / 10f);
				}
				else if (this.ai[0] == 5f)
				{
					this.rotation = 0f;
					base.velocity.X = 0f;
					if (this.type >= 496 && this.type <= 497)
					{
						this.ai[1] += 0.5f;
					}
					else
					{
						this.ai[1] += 1f;
					}
					if (this.ai[1] >= 30f)
					{
						this.TargetClosest();
						this.netUpdate = true;
						this.ai[1] = 0f;
						this.ai[0] = 0f;
					}
					if (base.wet)
					{
						this.ai[0] = 3f;
						this.ai[1] = 0f;
					}
				}
			}
*/
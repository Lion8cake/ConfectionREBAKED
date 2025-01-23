using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Items.Banners;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.NPCs
{
    public class IcecreamGal : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 16;
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers
            {
                Velocity = 0.5f
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 18;
            NPC.height = 40;
            NPC.damage = 64;
            NPC.defense = 8;
            NPC.lifeMax = 1000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = 40000f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = -1;
			NPC.rarity = 2;
			NPC.npcSlots = 2; // Advanced, boss-like AI is used, so the max number of spawns of this NPC should be a little smaller.
            AnimationType = NPCID.Mummy; //I am NOT translating this into solid code
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<IcecreamGalBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionUndergroundBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.IcecreamGal")
            });
        }

		public override void OnSpawn(IEntitySource source)
		{
			NPC.ai[1] = -1;
			NPC.ai[3] = -1;
			NPC.target = -1;
		}

		//Majority of the AI is made by SOvr9000, thanks bro
		public override void AI()
		{
			//target a player
			if (NPC.target == -1)
				NPC.TargetClosest();
			//stand still
			if (NPC.ai[3] == -1f)
			{
				if (NPC.target >= 0)
				{
					Vector2 pos = new(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
					float distX = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - pos.X;
					float distY = Main.player[NPC.target].position.Y - pos.Y;
					float distance = (float)Math.Sqrt(distX * distX + distY * distY);
					if (distance < 200f && Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
					{
						NPC.ai[3] = 0f;
					}
					if (Main.player[NPC.target].Center.X > NPC.Center.X)
					{
						NPC.direction = 1;
					}
					else
					{
						NPC.direction = -1;
					}
				}
				if (NPC.velocity.X != 0f || NPC.velocity.Y < 0f || NPC.velocity.Y > 2f || NPC.life != NPC.lifeMax)
				{
					NPC.ai[3] = 0f;
				}
			}
			else
			{
				//movement
				if (NPC.target < Main.maxPlayers && NPC.target > -1)
				{
					Player targetPlayer = Main.player[NPC.target];
					if (targetPlayer == null || !targetPlayer.active || targetPlayer.statLife <= 0)
					{
						NPC.target = -1;
					}
					else
					{
						float speedAmount = 3.5f;
						float acceleration = 0.4f;
						float jumpingForce = 13f;
						float hopForce = 4f;
						float dx = targetPlayer.Center.X - NPC.Center.X;
						float hdist = MathF.Abs(dx); // horizontal distance
						float speed = MathF.Min(speedAmount, hdist / 20f);
						NPC.velocity.X += (dx > 0f ? 1 : -1) * acceleration;
						if (MathF.Abs(NPC.velocity.X) > speed)
						{
							NPC.velocity.X *= speed / MathF.Abs(NPC.velocity.X); // Normalize so that it doesn't move too fast horizontally
						}
						if (NPC.collideY && NPC.oldVelocity.Y >= 0)
						{
							float tileJumpHeight = 18f; //18 tiles in jump height
							if (hdist <= tileJumpHeight * 16f)
							{
								NPC.velocity.Y = -jumpingForce;
								// Chance to summon stars
								if (NPC.ai[1] == -1f && Main.rand.NextBool(4))
								{
									NPC.ai[1] = 0f; // Triggers the animation
								}
							}
							else
							{
								NPC.velocity.Y = -hopForce;
							}
						}
					}
				}
				else
				{
					NPC.velocity.X *= 0.6f;
				}
				if (NPC.velocity.X > 0)
				{
					NPC.direction = 1;
				}
				else if (NPC.velocity.X < 0)
				{
					NPC.direction = -1;
				}
				//Spawn Star projectiles
				if (NPC.ai[1] >= 0f && NPC.ai[1] < 6)
				{
					// ai value increments by one only when a star has finished the summoning animation
					bool foundOwnedProjectile = FindStarsAtIndex((int)NPC.ai[1]);
					if (!foundOwnedProjectile)
					{
						Projectile.NewProjectile(NPC.GetSource_None(), NPC.Center - Vector2.One * 5f, Vector2.Zero, ModContent.ProjectileType<IcecreamStar>(), 32, 6, -1, NPC.whoAmI, NPC.ai[1]);
					}
				}
				//Rotate stars around the icecream gal
				for (int i = 0; i < 6; i++)
				{
					if (GetStarAtIndex(i) == null)
						continue;
					Projectile star = GetStarAtIndex(i);
					float circulationSpeed = 0.045f;
					float t = (float)Main.time * circulationSpeed + i * MathF.Tau / 6;
					float nextRadius;
					float radiousAmount = 30f;
					if (star.ai[2] == 1f)
					{
						nextRadius = radiousAmount;
					}
					else
					{
						float summonSpeed = 4f / 3f;
						NPC.ai[2] += summonSpeed;
						nextRadius = NPC.ai[2];
						if (nextRadius >= radiousAmount)
						{
							star.ai[2] = 1f; // Mark as fully summoned
							NPC.ai[1]++; // Allow to summon the next star
							NPC.ai[2] = 0f;
						}
					}
					star.Center = NPC.Center + new Vector2(nextRadius * MathF.Cos(t), nextRadius * MathF.Sin(t));
				}
				//Shoot stars
				if (NPC.target < Main.maxPlayers && NPC.target > -1)
				{
					NPC.ai[3]++;
					if (NPC.ai[3] >= 25)
					{
						if (NPC.ai[1] == 6)
						{
							Player targetPlayer = Main.player[NPC.target];
							//int[] remaining = starRange.Where(i => starProjectiles[i] != -1).ToArray();
							//int i = remaining[Main.rand.Next(remaining.Length)];

							bool[] remaining = LastRemainingStars();
							int i = Main.rand.Next(remaining.Length);
							while (!remaining[i])
							{
								i++;
								if (i >= remaining.Length)
								{
									i = 0;
								}
							}
							Projectile star = GetStarAtIndex(i);
							float projSpeed = 11f;
							Vector2 delta = targetPlayer.Center - star.Center;
							star.velocity = delta * projSpeed / delta.Length();
							star.ai[1] = -1;
							// Basically what happens above is that stars are selected randomly from the circle to be shot toward the target player
							if (DoesIcecreamGalHaveNoStars())
							{
								NPC.ai[1] = -1f; // Preparing to summon stars again sometime
								NPC.ai[2] = 0f;
							}
						}
						NPC.ai[3] = 0;
					}
				}
			}
		}

		private Projectile GetStarAtIndex(int index)
		{
			Projectile proj = null;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile projectile = Main.projectile[i];
				if (projectile.active && projectile.type == ModContent.ProjectileType<IcecreamStar>() && projectile.ai[0] == NPC.whoAmI && projectile.ai[1] == index)
				{
					proj = projectile;
					break;
				}
			}
			return proj;
		}

		private bool FindStarsAtIndex(int index)
		{
			bool found = false;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile projectile = Main.projectile[i];
				if (projectile.active && projectile.type == ModContent.ProjectileType<IcecreamStar>() && projectile.ai[0] == NPC.whoAmI && projectile.ai[1] == index)
				{
					found = true;
					break;
				}
			}
			return found;
		}

		private bool DoesIcecreamGalHaveNoStars()
		{
			bool found = true;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile projectile = Main.projectile[i];
				int num = (int)projectile.ai[1];
				if (projectile.active && projectile.type == ModContent.ProjectileType<IcecreamStar>() && projectile.ai[0] == NPC.whoAmI && (num < 6 && num >= 0))
				{
					found = false;
					break;
				}
			}
			return found;
		}

		private bool[] LastRemainingStars()
		{
			bool[] stars = new bool[6] { false, false, false, false, false, false };
			for (int j = 0; j < stars.Length; j++)
			{
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile projectile = Main.projectile[i];
					if (projectile.active && projectile.type == ModContent.ProjectileType<IcecreamStar>() && projectile.ai[0] == NPC.whoAmI && projectile.ai[1] == j)
					{
						stars[j] = true;
						break;
					}
				}
			}
			return stars;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(new CommonDrop(ModContent.ItemType<SoulofDelight>(), 1, amountDroppedMinimum: 3, amountDroppedMaximum: 5));
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = NPC.direction;
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

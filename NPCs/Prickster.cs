using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using System;
using Terraria;
using Terraria.Cinematics;
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
            Main.npcFrameCount[NPC.type] = 6;
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers
			{
				Frame = 3
			});
        }

        public override void SetDefaults()
        {
			NPC.width = 38;
			NPC.height = 28;
			NPC.aiStyle = -1; //-1
			NPC.damage = 75;
            NPC.defense = 35;
			NPC.lifeMax = 220;
			NPC.HitSound = SoundID.NPCHit24;
			NPC.knockBackResist = 0.3f;
			NPC.DeathSound = SoundID.NPCDeath27;
			NPC.value = 500f;
			NPC.noGravity = false;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<PricksterBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionUndergroundBiome>().Type };
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.Prickster")
            });
        }

		public override void AI()
		{
			if (NPC.target < 0 || Main.player[NPC.target].dead || NPC.direction == 0)
			{
				NPC.TargetClosest();
			}
			if (NPC.ai[0] != 1f && NPC.ai[0] != 3f)
			{
				if (NPC.ai[3] != 0)
				{
					NPC.ai[3] = 0;
				}
			}
			if (NPC.justHit)
			{
				NPC.ai[0] = 1f;
				NPC.ai[1] = 0f;
				NPC.TargetClosest();
			}
			//Hide
			if (NPC.ai[0] == 0)
			{
				NPC.TargetClosest();
				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					return;
				}
				if (NPC.velocity.X != 0f || NPC.velocity.Y < 0f || (double)NPC.velocity.Y > 0.3)
				{
					NPC.ai[0] = 1f;
					NPC.netUpdate = true;
					return;
				}
				Rectangle rectangle = new((int)Main.player[NPC.target].position.X, (int)Main.player[NPC.target].position.Y, Main.player[NPC.target].width, Main.player[NPC.target].height);
				Rectangle rectangle2 = new Rectangle((int)NPC.position.X - 100, (int)NPC.position.Y - 100, NPC.width + 200, NPC.height + 200);
				if (rectangle2.Intersects(rectangle) || NPC.life < NPC.lifeMax)
				{
					NPC.ai[0] = 1f;
					NPC.netUpdate = true;
				}
			}
			//movement
			if (NPC.ai[0] == 1f)
			{
				if (NPC.velocity.X < 0f)
				{
					NPC.direction = -1;
				}
				else if (NPC.velocity.X > 0f)
				{
					NPC.direction = 1;
				}
				NPC.rotation = 0f;
				NPC.spriteDirection = NPC.direction;
				Vector2 vector289 = new(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
				float num1153 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector289.X;
				float num1154 = Main.player[NPC.target].position.Y - vector289.Y;
				float num1155 = (float)Math.Sqrt(num1153 * num1153 + num1154 * num1154);
				bool flag34 = Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height);
				if (num1155 > 200f && flag34)
				{
					NPC.ai[1] += 4f;
				}
				if (num1155 > 600f && (flag34 || NPC.position.Y + (float)NPC.height > Main.player[NPC.target].position.Y - 200f))
				{
					NPC.ai[1] += 10f;
				}
				if (NPC.wet)
				{
					NPC.ai[1] = 1000f;
				}
				NPC.defense = NPC.defDefense;
				NPC.damage = NPC.defDamage;
				NPC.knockBackResist = 0.3f * Main.GameModeInfo.KnockbackToEnemiesMultiplier;
				NPC.ai[1] += 1f;
				if (NPC.ai[1] >= 400f)
				{
					NPC.ai[1] = 0f;
					NPC.ai[0] = 2f;
				}
				if (!NPC.justHit && NPC.velocity.X != NPC.oldVelocity.X)
				{
					NPC.direction *= -1;
				}
				if (NPC.velocity.Y == 0f && Main.player[NPC.target].position.Y < NPC.position.Y + (float)NPC.height)
				{
					int num1156;
					int num1157;
					if (NPC.direction > 0)
					{
						num1156 = (int)(((double)NPC.position.X + (double)NPC.width * 0.5) / 16.0);
						num1157 = num1156 + 3;
					}
					else
					{
						num1157 = (int)(((double)NPC.position.X + (double)NPC.width * 0.5) / 16.0);
						num1156 = num1157 - 3;
					}
					int num1158 = (int)((NPC.position.Y + (float)NPC.height + 2f) / 16f) - 1;
					int num1159 = num1158 + 4;
					bool flag36 = false;
					for (int num1160 = num1156; num1160 <= num1157; num1160++)
					{
						for (int num1161 = num1158; num1161 <= num1159; num1161++)
						{
							if (Main.tile[num1160, num1161] != null && Main.tile[num1160, num1161].HasUnactuatedTile && Main.tileSolid[Main.tile[num1160, num1161].TileType])
							{
								flag36 = true;
							}
						}
					}
					if (!flag36)
					{
						NPC.direction *= -1;
						NPC.velocity.X = 0.1f * (float)NPC.direction;
					}
				}
				float jumpHeight = 5f;
				float jumpLength = 2 + Main.rand.Next(0, 4);
				if (NPC.velocity == Vector2.Zero)
					NPC.ai[3]++;
				if (NPC.ai[3] >= 40f)
				{
					NPC.velocity.Y = -jumpHeight;
					NPC.velocity.X = jumpLength * NPC.direction;
					NPC.ai[3] = 0f;
				}
				if (NPC.velocity.Y == 0f)
				{
					NPC.velocity.X = 0f;
				}
			}
			//curl up and prepare for throw
			else if (NPC.ai[0] == 2f)
			{
				NPC.rotation = 0f;
				NPC.velocity.X *= 0.5f;
				NPC.ai[1] += 1f;
				if (NPC.ai[1] >= 30f)
				{
					NPC.netUpdate = true;
					NPC.TargetClosest();
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
					NPC.ai[0] = 3f;
				}
			}
			//throw yourself at the target
			else if (NPC.ai[0] == 3f)
			{
				float num1167 = 2f;
				NPC.damage = NPC.GetAttackDamage_LerpBetweenFinalValues((float)NPC.defDamage * num1167, (float)NPC.defDamage * num1167 * 0.9f);
				NPC.defense = NPC.defDefense * 2;
				NPC.ai[1] += 1f;
				if (NPC.ai[1] == 1f)
				{
					NPC.netUpdate = true;
					NPC.TargetClosest();
					NPC.ai[2] += 0.3f;
					NPC.rotation += NPC.ai[2] * (float)NPC.direction;
					NPC.ai[1] += 1f;
					bool flag37 = Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height);
					float num1168 = 10f;
					if (!flag37)
					{
						num1168 = 6f;
					}
					Vector2 vector290 = new(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
					float num1169 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector290.X;
					float num1170 = Math.Abs(num1169) * 0.2f;
					if (NPC.directionY > 0)
					{
						num1170 = 0f;
					}
					float num1171 = Main.player[NPC.target].position.Y - vector290.Y - num1170;
					float num1172 = (float)Math.Sqrt(num1169 * num1169 + num1171 * num1171);
					NPC.netUpdate = true;
					num1172 = num1168 / num1172;
					num1169 *= num1172;
					num1171 *= num1172;
					if (!flag37)
					{
						num1171 = -10f;
					}
					NPC.velocity.X = num1169;
					NPC.velocity.Y = num1171;
					NPC.ai[3] = NPC.velocity.X;
				}
				else
				{
					if (NPC.position.X + (float)NPC.width > Main.player[NPC.target].position.X && NPC.position.X < Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width && NPC.position.Y < Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height)
					{
						NPC.velocity.X *= 0.8f;
						NPC.ai[3] = 0f;
						if (NPC.velocity.Y < 0f)
						{
							NPC.velocity.Y += 0.2f;
						}
					}
					if (NPC.ai[3] != 0f)
					{
						NPC.velocity.X = NPC.ai[3];
						NPC.velocity.Y -= 0.22f;
					}
					if (NPC.ai[1] >= 90f)
					{
						NPC.noGravity = false;
						NPC.ai[1] = 0f;
						NPC.ai[0] = 4f;
					}
				}
				if (NPC.wet && NPC.directionY < 0)
				{
					NPC.velocity.Y -= 0.3f;
				}
				NPC.rotation += NPC.ai[2] * (float)NPC.direction;
			}
			//Slam down on the target
			else if (NPC.ai[0] == 4f)
			{
				if (NPC.wet && NPC.directionY < 0)
				{
					NPC.velocity.Y -= 0.3f;
				}
				NPC.velocity.X *= 0.96f;
				if (NPC.ai[2] > 0f)
				{
					NPC.ai[2] -= 0.01f;
					NPC.rotation += NPC.ai[2] * (float)NPC.direction;
				}
				else if (NPC.velocity.Y >= 0f)
				{
					NPC.rotation = 0f;
				}
				if (NPC.ai[2] <= 0f && (NPC.velocity.Y == 0f || NPC.wet))
				{
					NPC.netUpdate = true;
					NPC.rotation = 0f;
					NPC.ai[2] = 0f;
					NPC.ai[1] = 0f;
					NPC.ai[0] = 5f;
				}
			}
			//uncurl up, used for animation
			else if (NPC.ai[0] == 5f)
			{
				NPC.rotation = 0f;
				NPC.velocity.X = 0f;
				NPC.ai[1] += 1f;
				if (NPC.ai[1] >= 30f)
				{
					NPC.TargetClosest();
					NPC.netUpdate = true;
					NPC.ai[1] = 0f;
					NPC.ai[0] = 1f;
				}
				if (NPC.wet)
				{
					NPC.ai[0] = 3f;
					NPC.ai[1] = 0f;
				}
			}
			if ((NPC.ai[0] == 3 && NPC.ai[1] > 10) || NPC.ai[3] == 4)
			{
				if (NPC.collideY)
				{
					NPC.velocity.Y = -NPC.velocity.Y;
				}
				if (NPC.collideX)
				{
					NPC.velocity.X = -NPC.velocity.X;
				}
			}
		}

		public override void FindFrame(int frameHeight)
		{
			if (NPC.ai[0] == 0)
			{
				NPC.frame.Y = 0;
			}
			else if (NPC.ai[0] == 1)
			{
				int frameCount = NPC.frame.Y / frameHeight;
				if (frameCount == 0 || frameCount == 5)
				{
					frameCount = 1;
				}
				else if (frameCount == 1 || frameCount == 2)
				{
					if (++NPC.frameCounter >= 8)
					{
						frameCount++;
						NPC.frameCounter = 0;
					}
				}
				else
				{
					frameCount = 3;
					if (NPC.ai[3] == 0)
					{
						frameCount = 4;
					}
				}
				NPC.frame.Y = frameCount * frameHeight;
			}
			else
			{
				int frameCount = NPC.frame.Y / frameHeight;
				if (frameCount == 3 || frameCount == 4)
				{
					frameCount = 2;
				}
				else if (frameCount == 1 || frameCount == 2)
				{
					if (++NPC.frameCounter >= 8)
					{
						frameCount--;
						NPC.frameCounter = 0;
					}
				}
				else
				{
					frameCount = 5;
				}
				NPC.frame.Y = frameCount * frameHeight;
			}
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
		{
			if (Main.rand.NextBool(2))
			{
				target.AddBuff(BuffID.Bleeding, 420);
			}
		}

		public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
		{
			if (NPC.ai[0] <= 0)
			{
				NPC.ai[0] = 1f;
			}
		}

		public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
		{
			if (NPC.ai[0] <= 0)
			{
				NPC.ai[0] = 1f;
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			//ALERT!!, spawn perfectly on the Y tile position, otherwise it will argo

			//if (spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && !spawnInfo.AnyInvasionActive() && Main.hardMode && (spawnInfo.Player.ZoneRockLayerHeight || spawnInfo.Player.ZoneUnderworldHeight)) {
			//	return 0.05f;
			//}
			return 0f;
		}

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Saccharite>(), 1, 2, 8));
			npcLoot.Add(ItemDropRule.Common(ItemID.AdhesiveBandage, 100, 1, 1));
        }

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			if (NPC.life > 0)
			{
				for (int i = 0; (double)i < hit.Damage / (double)NPC.lifeMax * 30.0; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f);
				}
			}
			else
			{
				for (int i = 0; i < 15; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2 * hit.HitDirection, -2f);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("PricksterGore1").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("PricksterGore2").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("PricksterGore3").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("PricksterGore4").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("PricksterGore5").Type);
			}
		}
	}
}
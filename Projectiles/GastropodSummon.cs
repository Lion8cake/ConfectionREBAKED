using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.DataStructures;
using TheConfectionRebirth.Buffs;
using System.Collections.Generic;

namespace TheConfectionRebirth.Projectiles
{
	public class GastropodSummon : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			Main.projFrames[Projectile.type] = 4;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.netImportant = true;
			Projectile.friendly = true;
			Projectile.minionSlots = 1f;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 18000;
			Projectile.penetrate = -1;
			Projectile.timeLeft *= 5;
			Projectile.minion = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.tileCollide = false;
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override bool MinionContactDamage()
		{
			return false;
		}

		public override void AI() 
		{
			Player player = Main.player[Projectile.owner];
			if (!player.active)
			{
				Projectile.active = false;
				return;
			}

			if (!player.dead && player.HasBuff(ModContent.BuffType<GastropodSummonBuff>()))
			{
				player.GetModPlayer<ConfectionPlayer>().gastropodMinion = true;
			}
			if (player.GetModPlayer<ConfectionPlayer>().gastropodMinion)
			{
				Projectile.timeLeft = 2;
			}

			float distanceFromTarget = 1000f;
			Vector2 targetCenter = Projectile.position;
			bool foundTarget = false;
			bool cannotReachPlayerTarget = false; //thx terror penguin
			if (player.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				if (Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
				{
					distanceFromTarget = Vector2.Distance(Projectile.Center, targetCenter);
					targetCenter = npc.Center;
					foundTarget = true;
				}
				else
				{
					cannotReachPlayerTarget = true;
				}
			}
			if (!player.HasMinionAttackTargetNPC || cannotReachPlayerTarget)
			{
				for (int k = 0; k < Main.maxNPCs; k++)
				{
					NPC npc = Main.npc[k];
					if (npc.CanBeChasedBy(this, false))
					{
						float distance = Vector2.Distance(npc.Center, Projectile.Center);
						if ((distance < distanceFromTarget || !foundTarget) && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
						{
							distanceFromTarget = distance;
							targetCenter = npc.Center;
							foundTarget = true;
						}
					}
				}
			}

			if (!foundTarget)
			{
				//Idle
				Projectile.ai[0] = 0;
				if (Projectile.ai[1] <= 0f)
				{
					Projectile.velocity = Vector2.Zero;
					float rotationDist = 6f;
					AI_GetMyGroupIndexAndFillBlackList(null, out var index, out var totalIndexesInGroup);
					float f = ((float)index / (float)totalIndexesInGroup + player.miscCounterNormalized * 2f) * ((float)Math.PI * 2f);
					float num = 24f + (float)totalIndexesInGroup * rotationDist;
					Vector2 vector = player.position - player.oldPosition;
					Projectile.Center += vector;
					Vector2 vector2 = f.ToRotationVector2();
					Projectile.localAI[0] = vector2.Y;
					Vector2 value = player.Center + vector2 * new Vector2(1f, 0.05f) * num;
					Projectile.Center = Vector2.Lerp(Projectile.Center, value, 0.1f);
				}
				else
				{
					Vector2 dir = player.Center - Projectile.Center;
					if (dir.Length() > 20f)
					{
						dir.Normalize();
						Projectile.velocity = (Projectile.velocity * 2f + dir * 8f) / (2f + 1);
					}
					else
					{
						Projectile.ai[1] = 0f;
					}
				}
			}
			else
			{
				Projectile.ai[1] = 1f;
				Projectile.localAI[0] = 0f;
				Projectile.ai[0]++;
				if (Projectile.ai[0] >= 32)
				{
					Vector2 newPos = targetCenter - Projectile.position;
					float finalAngle = (float)Math.Sqrt(newPos.X * newPos.X + newPos.Y * newPos.Y);
					finalAngle = 12f / finalAngle;
					newPos *= finalAngle;
					Vector2 velocity = (newPos);
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity, ModContent.ProjectileType<GastropodSummonPinkLazer>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
					Projectile.ai[0] = 0;
				}
				if (Projectile.ai[0] % 4 == 0)
				{
					Vector2 dir = targetCenter - Projectile.Center;
					if (dir.Length() > 80f)
					{
						dir.Normalize();
						Projectile.velocity = (Projectile.velocity * 2f + dir * 8f) / (2f + 1);
					}
					else
					{
						Projectile.velocity *= (float)Math.Pow(1f, 40.0 / 2f);
						if (Projectile.Center.Y > targetCenter.Y)
						{
							Projectile.velocity.Y -= 1f;
						}
					}
				}
			}

			if (Projectile.ai[1] <= 0f)
			{
				Projectile.direction = Projectile.localAI[0] <= 0 ? -1 : 1;
				Projectile.spriteDirection = Projectile.direction;
				if (Projectile.localAI[0] < 0.5f && Projectile.localAI[0] > -0.5f)
				{
					Projectile.frame = 0;
				}
				else
				{
					Projectile.frame = 1;
				}
			}
			else
			{
				Projectile.direction = Projectile.velocity.X >= 0 ? -1 : 1;
				Projectile.spriteDirection = Projectile.direction;
				if (Projectile.ai[0] > 26)
				{
					Projectile.frame = 3;
				}
				else if (Projectile.ai[0] > 14)
				{
					Projectile.frame = 2;
				}
				else if (Math.Abs(Projectile.velocity.X) > 0)
				{
					Projectile.frame = 1;
				}
				else
				{
					Projectile.frame = 0;
				}
			}
			Lighting.AddLight(Projectile.Center, 0.4f / 2, 0 / 2, 0.25f / 2);
		}

		private void AI_GetMyGroupIndexAndFillBlackList(List<int> blackListedTargets, out int index, out int totalIndexesInGroup)
		{
			index = 0;
			totalIndexesInGroup = 0;
			for (int i = 0; i < 1000; i++)
			{
				Projectile projectile = Main.projectile[i];
				if (projectile.active && projectile.owner == Projectile.owner && projectile.type == Projectile.type)
				{
					if (Projectile.whoAmI > i)
					{
						index++;
					}
					totalIndexesInGroup++;
				}
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			return false;
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			if (Projectile.localAI[0] <= 0f)
			{
				behindProjectiles.Add(index);
			}
			else
			{
				overPlayers.Add(index);
			}
		}
	}
}

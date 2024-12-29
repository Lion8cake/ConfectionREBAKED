using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using TheConfectionRebirth.Buffs;
using System.Collections.Generic;

namespace TheConfectionRebirth.Projectiles
{
	public class MeawzerSummon : ModProjectile
	{
		public override void SetStaticDefaults()
		{
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

			if (!player.dead && player.HasBuff(ModContent.BuffType<MeawzerSummonBuff>()))
			{
				player.GetModPlayer<ConfectionPlayer>().meawzerMinion = true;
			}
			if (player.GetModPlayer<ConfectionPlayer>().meawzerMinion)
			{
				Projectile.timeLeft = 2;
			}

			float distanceFromTarget = 700f;
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
				float num25 = 6f;
				Vector2 v2 = player.Center - Projectile.Center + new Vector2(0f, -60f);

				Projectile.netUpdate = true;
				v2 = player.Center - Projectile.Center;
				int num26 = 1;
				for (int num27 = 0; num27 < Projectile.whoAmI; num27++)
				{
					if (Main.projectile[num27].active && Main.projectile[num27].owner == Projectile.owner && Main.projectile[num27].type == Type)
					{
						num26++;
					}
				}
				float num28 = v2.Length();
				if (num28 > 400f)
				{
					num26 = 1;
				}
				v2.X -= 10 * player.direction;
				v2.X -= num26 * 40 * player.direction;
				v2.Y -= 10f;
				num25 = (int)((double)num25 * 0.75);
				num28 = v2.Length();
				if (num28 > 10f)
				{
					v2 = v2.SafeNormalize(Vector2.Zero);
					if (num28 < 50f)
					{
						num25 /= 2f;
					}
						
					v2 *= num25;
					Projectile.velocity = (Projectile.velocity * 20f + v2) / 21f;
					if (num28 > 400f)
					{
						Projectile.velocity *= 1.035f;
					}
				}
				else
				{
					Projectile.velocity *= 0.9f;
				}
				Projectile.direction = Projectile.Center.X - player.Center.X > 0 ? 1 : -1;
			}
			else
			{
				Projectile.localAI[0] = 0f;
				Projectile.ai[0]++;
				if (Projectile.ai[0] >= 50)
				{
					Projectile.velocity = Vector2.Zero;
					Vector2 newPos = targetCenter - Projectile.position;
					float finalAngle = (float)Math.Sqrt(newPos.X * newPos.X + newPos.Y * newPos.Y);
					finalAngle = 12f / finalAngle;
					newPos *= finalAngle;
					Vector2 velocity = (newPos);
					Vector2 position = Projectile.Center + new Vector2(Projectile.spriteDirection * -8, -4);
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), position, velocity, ModContent.ProjectileType<MeawzerSummonLazer>(), (int)(Projectile.damage * 1.25f), Projectile.knockBack, Projectile.owner);

					for (int i = 0; i < 14; i++)
					{
						Vector2 dustpos = position + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / 14 * i));
						Dust dust = Dust.NewDustPerfect(dustpos, 66);
						dust.noGravity = true;
						dust.velocity = Vector2.Normalize(dust.position - position) * 0.75f;
						dust.noLight = false;
						dust.fadeIn = 1f;
						dust.scale = 0.1f;
					}

					Projectile.ai[0] = 0;
				}
				else if (Projectile.ai[0] % 4 == 0)
				{
					Vector2 dir = targetCenter - Projectile.Center;
					if (dir.Length() > 160f)
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
				Projectile.direction = Projectile.Center.X - targetCenter.X > 0 ? 1 : -1;
			}
			Projectile.spriteDirection = Projectile.direction;
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
	}
}

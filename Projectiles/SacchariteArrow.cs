using Terraria.ModLoader;
using Terraria;
using TheConfectionRebirth.Dusts;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using TheConfectionRebirth.Buffs;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;

namespace TheConfectionRebirth.Projectiles
{
	public class SacchariteArrow : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.arrow = true;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 1200;
			Projectile.penetrate = -1;
			Projectile.idStaticNPCHitCooldown = 30;
			Projectile.usesIDStaticNPCImmunity = true;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<SacchariteDust>());
			}
		}

		public override void OnSpawn(IEntitySource source)
		{
			Projectile.ai[1] = -1;
		}

		public override bool PreAI()
		{
			Projectile.ignoreWater = true;
			if (Projectile.ai[1] > -1)
			{
				Vector2 center9 = Projectile.Center;
				Projectile.tileCollide = false;
				int attackCount = 5;
				bool isValid = false;
				bool visuallyAttack = false;
				Projectile.localAI[2]++;
				if (Projectile.localAI[2] % 30f == 0f)
				{
					visuallyAttack = true;
				}
				int npcID = (int)Projectile.ai[1];
				if (Projectile.localAI[2] >= (float)(60 * attackCount))
				{
					isValid = true;
				}
				else if (npcID < 0 || npcID >= 200)
				{
					isValid = true;
				}
				else if (Main.npc[npcID].active && !Main.npc[npcID].dontTakeDamage)
				{
					NPC npc = Main.npc[npcID];
					Projectile.Center = npc.Center - Projectile.velocity * 2f;
					Projectile.gfxOffY = npc.gfxOffY;
					npc.AddBuff(ModContent.BuffType<SacchariteAmmoInjection>(), 2);
					if (visuallyAttack)
					{
						npc.HitEffect(0, 1.0);
					}
				}
				else
				{
					isValid = true;
				}
				if (isValid)
				{
					Projectile.Kill();
				}
				return false;
			}
			if (Projectile.ai[2] > 0)
			{
				Projectile.ai[2] += 0.1f;
				return false;
			}
			return true;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (Projectile.ai[1] > -1)
			{
				return false;
			}
			return true;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (!target.friendly)
			{
				if (Projectile.ai[2] <= 0)
				{
					Projectile.ai[1] = target.whoAmI;
					Projectile.velocity = (target.Center - Projectile.Center) * 0.75f;
					Projectile.netUpdate = true;
				}
				else
				{
					hit.Damage /= 2;
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.ai[1] <= -1)
			{
				Projectile.ai[2] = 0.7f;
				Projectile.netUpdate = true;
				Projectile.tileCollide = false;
				Projectile.position += Projectile.velocity;
				Projectile.velocity = oldVelocity;
				Projectile.velocity.Normalize();
				Projectile.velocity *= 3f;
			}
			return false;
		}

		public override bool ShouldUpdatePosition()
		{
			if (Projectile.ai[2] >= 1f)
			{
				return false;
			}
			return true;
		}
	}
}

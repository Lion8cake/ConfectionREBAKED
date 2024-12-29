using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using TheConfectionRebirth.Buffs;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Projectiles
{
	public class SacchariteBullet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 3;
		}

		public override void SetDefaults()
		{
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.light = 0.5f;
			Projectile.alpha = 255;
			Projectile.scale = 1.2f;
			Projectile.timeLeft = 1200; //usually 600 but doubled due to extra updates
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.extraUpdates = 1; 
			Projectile.idStaticNPCHitCooldown = 30;
			Projectile.usesIDStaticNPCImmunity = true;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dustid = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<SacchariteDust>());
				Dust dust = Main.dust[dustid];
				dust.scale *= 0.75f;
				dust.velocity *= 0.5f;
			}
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			if (Projectile.ai[2] > 0)
			{
				hitbox.Inflate(6, 6);
			}
		}

		public override void OnSpawn(IEntitySource source)
		{
			Projectile.ai[1] = -1;
		}

		public override bool PreAI()
		{
			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 15;
			}
			if (Projectile.alpha < 0)
			{
				Projectile.alpha = 0;
			}
			Projectile.ignoreWater = true;
			if (Projectile.ai[1] > -1)
			{
				Projectile.frame = 1;
				Vector2 center9 = Projectile.Center;
				Projectile.tileCollide = false;
				int attackCount = 5 * 2; //doubled due to the extra updates also being doubled
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
				Projectile.frame = 2;
				Projectile.knockBack = 0;
				Projectile.ai[2] += 0.1f;
				return false;
			}
			Projectile.frame = 0;
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
					hit.Damage /= 4;
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

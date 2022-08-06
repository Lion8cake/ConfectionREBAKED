using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Projectiles
{
	public class CandleFlames : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Candle Flames"); 
		}

		public override void SetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.alpha = 255; 
			Projectile.friendly = true; 
			Projectile.hostile = false; 
			Projectile.penetrate = 3; 
			Projectile.timeLeft = 90; 
			Projectile.ignoreWater = false;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.extraUpdates = 2;
		}

		public override void AI()
		{
			if (Projectile.wet)
			{
				Projectile.Kill(); 
			}
			float dustScale = 1f;
			if (Projectile.ai[0] == 0f)
				dustScale = 0.25f;
			else if (Projectile.ai[0] == 1f)
				dustScale = 0.5f;
			else if (Projectile.ai[0] == 2f)
				dustScale = 0.75f;

			if (Main.rand.Next(2) == 0) {
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100);
				
				if (Main.rand.NextBool(3)) {
					dust.noGravity = true;
					dust.scale *= 3f;
					dust.velocity.X *= 2f;
					dust.velocity.Y *= 2f;
				}

				dust.scale *= 1.5f;
				dust.velocity *= 1.2f;
				dust.scale *= dustScale;
			}
			Projectile.ai[0] += 1f;
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.AddBuff(BuffID.OnFire, 240000000);
        }

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.OnFire, 240000000, false);
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			int size = 30;
			hitbox.X -= size;
			hitbox.Y -= size;
			hitbox.Width += size * 2;
			hitbox.Height += size * 2;
		}
	}
}
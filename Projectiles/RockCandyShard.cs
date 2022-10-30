using System;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Projectiles
{
	public class RockCandyShard : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.aiStyle = 14;
			Projectile.penetrate = 4;
			Projectile.timeLeft = 360;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.DamageType = DamageClass.Ranged;
		}
	
		public override void AI()
		{
			Projectile.velocity.X *= 0.9f;
			Projectile.velocity.Y *= 0.99f;
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
		}
	}
}
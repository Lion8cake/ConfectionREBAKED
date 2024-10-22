using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Projectiles
{
    public class RocketPop : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 10;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 0.785f;
			if (Projectile.ai[0] >= 1f)
			{
				Projectile.velocity.X *= 0.9f;
				Projectile.velocity.Y *= 0.9f;
			}
			if ((Projectile.velocity.X < 0.01f && Projectile.velocity.X > -0.01f) || (Projectile.velocity.Y < 0.01f && Projectile.velocity.Y > -0.01f))
			{
				Projectile.Kill();
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.velocity.X *= 0.2f;
			Projectile.velocity.Y *= 0.2f;
			Projectile.ai[0] = 1f;
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			Projectile.velocity.X *= 0.2f;
			Projectile.velocity.Y *= 0.2f;
			Projectile.ai[0] = 1f;
		}

		public override void OnKill(int timeLeft)
		{
			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<RocketPopExplosion>(), (int)(Projectile.damage * 0.3), Projectile.knockBack);
		}
	}
}
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Projectiles
{
    public class BearClaw : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.aiStyle = 3;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Melee;
        }

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Projectile.ai[0] == 1f)
			{
				Projectile.velocity = Vector2.Zero;
			}
			ParticleSystem.AddParticle(new Spawn_BearClaw(), Projectile.Center, new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), 1));
		}
	}
}

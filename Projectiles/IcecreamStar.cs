using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Projectiles
{
    public class IcecreamStar : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.hostile = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 1200;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.01f / 255f, (255 - Projectile.alpha) * 0.3f / 255f, (255 - Projectile.alpha) * 0.45f / 255f);
            if (!Main.npc[(int)Projectile.ai[0]].active && Projectile.velocity == Vector2.Zero)
            {
                Projectile.Kill();
            }
            if (Projectile.ai[1] == -1)
            {
                Projectile.rotation += 0.3f * Projectile.direction;
                Projectile.tileCollide = true;
            }
            else
            {
                Projectile.tileCollide = false;
            }
        }

        public override void OnKill(int timeLeft)
        {
			for (int i = 0; i < 10; i++)
			{
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<CreamDust>());
			}
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }
    }
}
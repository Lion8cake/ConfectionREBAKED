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
            //Projectile.scale = 1.4f;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.01f / 255f, (255 - Projectile.alpha) * 0.3f / 255f, (255 - Projectile.alpha) * 0.45f / 255f);
            /*for (int num103 = 0; num103 < 2; num103++) {
                float num104 = Projectile.velocity.X / 3f * num103;
                float num100 = Projectile.velocity.Y / 3f * num103;
                int num101 = 4;
                int frostDust = Dust.NewDust(new Vector2(Projectile.position.X + num101, Projectile.position.Y + num101), Projectile.width - num101 * 2, Projectile.height - num101 * 2, 92, 0f, 0f, 100, default(Color), 1.2f);
                Dust obj = Main.dust[frostDust];
                obj.noGravity = true;
                obj.velocity *= 0.1f;
                obj.velocity += Projectile.velocity * 0.1f;
                obj.position.X -= num104;
                obj.position.Y -= num100;
            }
            if (Main.rand.NextBool(10)) {
                int num102 = 4;
                int frostDustSmol = Dust.NewDust(new Vector2(Projectile.position.X + num102, Projectile.position.Y + num102), Projectile.width - num102 * 2, Projectile.height - num102 * 2, 92, 0f, 0f, 100, default(Color), 0.6f);
                Dust obj2 = Main.dust[frostDustSmol];
                obj2.velocity *= 0.25f;
                Dust obj3 = Main.dust[frostDustSmol];
                obj3.velocity += Projectile.velocity * 0.5f;
            }*/

            if (Projectile.timeLeft < 1100)
            {
                Projectile.rotation += 0.3f * Projectile.direction;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<CreamDust>(), Projectile.oldVelocity.X * 0.75f, Projectile.oldVelocity.Y * 0.75f);
            }
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }

    }
}
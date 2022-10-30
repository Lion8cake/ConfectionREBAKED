using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Projectiles
{
    public class CreamySpray : ModProjectile
    {
        public override string Texture => "TheConfectionRebirth/Projectiles/CreamBolt";

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 6;
            Projectile.extraUpdates = 2;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.01f / 255f, (255 - Projectile.alpha) * 0.15f / 255f, (255 - Projectile.alpha) * 0.05f / 255f);
            Projectile.scale -= 0.002f;
            if (Projectile.scale <= 0f)
            {
                Projectile.Kill();
            }
            if (Projectile.ai[0] <= 3f)
            {
                Projectile.ai[0] += 1f;
                return;
            }
            Projectile.velocity.Y = Projectile.velocity.Y + 0.075f;
            for (int num151 = 0; num151 < 3; num151++)
            {
                float num152 = Projectile.velocity.X / 3f * num151;
                float num153 = Projectile.velocity.Y / 3f * num151;
                int num154 = 14;
                int num155 = Dust.NewDust(new Vector2(Projectile.position.X + num154, Projectile.position.Y + num154), Projectile.width - num154 * 2, Projectile.height - num154 * 2, 133, 0f, 0f, 100);
                Dust obj = Main.dust[num155];
                obj.noGravity = true;
                obj.velocity *= 0.1f;
                obj.velocity += Projectile.velocity * 0.5f;
                obj.position.X -= num152;
                obj.position.Y -= num153;
            }
            if (Main.rand.NextBool(8))
            {
                int num156 = 16;
                int num133 = Dust.NewDust(new Vector2(Projectile.position.X + num156, Projectile.position.Y + num156), Projectile.width - num156 * 2, Projectile.height - num156 * 2, 133, 0f, 0f, 100, default(Color), 0.5f);
                Dust obj2 = Main.dust[num133];
                obj2.velocity *= 0.25f;
                Dust obj3 = Main.dust[num133];
                obj3.velocity += Projectile.velocity * 0.5f;
            }
        }
    }
}
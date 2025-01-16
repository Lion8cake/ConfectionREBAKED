using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Projectiles
{
    public class RootbeerSpray : ModProjectile
    {
        public override string Texture => "TheConfectionRebirth/Projectiles/CreamBolt";

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            // projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 6;
            Projectile.extraUpdates = 2;
            Projectile.hostile = true;
        }

        public override void AI()
        {
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
            for (int i = 0; i < 3; i++)
            {
                float posX = Projectile.velocity.X / 3f * i;
                float posY = Projectile.velocity.Y / 3f * i;
                int offset = 14;
                int dustID = Dust.NewDust(new Vector2(Projectile.position.X + offset, Projectile.position.Y + offset), Projectile.width - offset * 2, Projectile.height - offset * 2, ModContent.DustType<Dusts.RootbeerSplash>(), 0f, 0f, 100);
                Dust dust = Main.dust[dustID];
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += Projectile.velocity * 0.5f;
                dust.position.X -= posX;
                dust.position.Y -= posY;
            }
            if (Main.rand.NextBool(8))
            {
                int offset = 16;
                int dustID = Dust.NewDust(new Vector2(Projectile.position.X + offset, Projectile.position.Y + offset), Projectile.width - offset * 2, Projectile.height - offset * 2, ModContent.DustType<Dusts.RootbeerSplash>(), 0f, 0f, 100, default(Color), 0.5f);
                Dust dust = Main.dust[dustID];
                dust.velocity *= 0.25f;
                dust.velocity += Projectile.velocity * 0.5f;
            }
        }
    }
}
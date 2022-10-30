using AltLibrary.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Projectiles
{
    public class SugarWater : ModProjectile
    {
        public ref float Progress => ref Projectile.ai[0];

        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 2;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            int dustType = ModContent.DustType<Dusts.CreamSolution>();

            if (Projectile.owner == Main.myPlayer)
            {
                ALConvert.SimulateSolution<ConfectionBiome>(Projectile);
            }

            if (Projectile.timeLeft > 10)
            {
                Projectile.timeLeft = 10;
            }

            if (Progress > 7f)
            {
                float dustScale = 5f;

                Progress += 1f;


                var dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100);

                dust.noGravity = true;
                dust.scale *= 5f;
                dust.velocity.X *= 2f;
                dust.velocity.Y *= 2f;
                dust.scale *= dustScale;
            }
            else
            {
                Progress += 1f;
            }

            Projectile.rotation += 0.3f * Projectile.direction;
        }
    }
}
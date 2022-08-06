using TheConfectionRebirth.Dusts;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace TheConfectionRebirth.Projectiles
{
    public class TrueIchorBolt : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.timeLeft = 30;
            Projectile.alpha = 0;
            Projectile.tileCollide = true;
            Projectile.penetrate = 5;
            DrawOffsetX = -24;
            Projectile.DamageType = DamageClass.Melee;
        }
        public override void AI()
        {
            Projectile.alpha += 20;
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
            if (Projectile.timeLeft <= 25)
            {
                Projectile.scale *= 0.97f;
            }
            if (Projectile.timeLeft <= 20)
            {
                Projectile.scale *= 0.95f;
            }
            if (Projectile.timeLeft <= 15)
            {
                Projectile.scale *= 0.93f;
            }
            if (Projectile.timeLeft <= 10)
            {
                Projectile.scale *= 0.91f;
            }
            float num1 = 1f;
            if (Projectile.timeLeft <= 15)
            {
                num1 = 0.5f;
            }
            int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<IchorDrops>(), Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            Main.dust[dust].noGravity = true;
            if (Main.rand.Next(3) == 0)
            {
                int dust1 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<IchorDrops>(), Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
                Main.dust[dust1].noGravity = true;
            }
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            if (Projectile.timeLeft >= 15)
            {
                int size = 20;
                hitbox.X -= size;
                hitbox.Y -= size;
                hitbox.Width += size * 2;
                hitbox.Height += size * 2;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, Projectile.alpha);
        }
    }
}
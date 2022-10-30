using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Projectiles
{
    public class BakersDozen : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.aiStyle = 3;
            Projectile.timeLeft = 600;
            AIType = 52;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.frame = 0;
        }
    }
}

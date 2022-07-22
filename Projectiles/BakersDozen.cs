using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Projectiles
{
    public class BakersDozen : ModProjectile
    {
        public static int castType = 0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Baker's Dozen");
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
            Projectile.frame = castType;
            castType++;
            if (castType == 4)
                castType = 0;
        }
    }
}

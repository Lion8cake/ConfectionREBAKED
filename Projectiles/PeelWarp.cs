using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Projectiles
{
    class PeelWarp : ModProjectile
    {
        public override string Texture => "TheConfectionRebirth/Projectiles/DimWarp";

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 38;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 999999 * 999;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 8;
        }

        public override void AI()
        {
            if (Projectile.owner == Main.myPlayer)
            {
                Main.player[Projectile.owner].GetModPlayer<ConfectionPlayer>().BananawarpPeelWarp = Projectile;
                Projectile.velocity = new Vector2(0, 0);
            }

            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
        public override bool PreKill(int timeLeft)
        {
            Main.player[Projectile.owner].GetModPlayer<ConfectionPlayer>().BananawarpPeelWarp = null;
            return true;
        }
    }
}

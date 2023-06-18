using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TheConfectionRebirth.Projectiles
{
    class DimWarp : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.Name = "Dimensional Warp";
            Projectile.width = 32;
            Projectile.height = 38;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 900;
        }
		
		public override void SetStaticDefaults() {
			Main.projFrames[Projectile.type] = 8;
		}

        public override void AI()
        {
            if (Projectile.owner == Main.myPlayer)
            {
                Main.player[Projectile.owner].GetModPlayer<ConfectionPlayer>().DimensionalWarp = Projectile;
                Projectile.velocity = new Vector2(0,0);
            }
			
			if (++Projectile.frameCounter >= 5) {
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= 8) {
					Projectile.frame = 0;
				}
			}
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
        public override bool PreKill(int timeLeft)
        {
            Main.player[Projectile.owner].GetModPlayer<ConfectionPlayer>().DimensionalWarp = null;
            return true;
        }
    }
}

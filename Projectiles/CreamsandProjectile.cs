using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Projectiles
{
    public class CreamsandProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Projectile.knockBack = 6f;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            if (Main.rand.NextBool(2))
            {
                int num129 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<ChipDust>(), 0f, Projectile.velocity.Y / 2f, 0, default(Color), 1f);
                Dust dust = Main.dust[num129];
                dust.velocity.X *= 0.4f;
            }

            Projectile.tileCollide = true;
            Projectile.localAI[1] = 0f;

            Projectile.velocity.Y = Projectile.velocity.Y + 0.41f;

            Projectile.rotation -= 0.1f;

            if (Projectile.velocity.Y < -10f)
            {
                Projectile.velocity.Y = -10f;
            }
        }

        public override void Kill(int timeLeft)
        {
            int i = (int)(Projectile.position.X + Projectile.width / 2) / 16;
            int j = (int)(Projectile.position.Y + Projectile.height / 2) / 16;
            int tileToPlace = 0;
            {
                tileToPlace = ModContent.TileType<Tiles.Creamsand>();
            }

            if (!Main.tile[i, j].HasTile && tileToPlace >= 0)
            {
                WorldGen.PlaceTile(i, j, tileToPlace, false, true, -1, 0);
            }
        }
    }
}
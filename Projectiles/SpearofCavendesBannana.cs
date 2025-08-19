using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Gores;

namespace TheConfectionRebirth.Projectiles
{
    public class SpearofCavendesBannana : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
			Projectile.aiStyle = 1;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 15; k++)
            {
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<CreamDust>(), Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f);
			}
            int goreID = Gore.NewGore(new EntitySource_Death(Projectile), Projectile.position + (Projectile.velocity / 2), Vector2.Zero, ModContent.GoreType<BanannaPeel>());
            Main.gore[goreID].rotation = Projectile.rotation + MathHelper.Pi;
        }
    }
}
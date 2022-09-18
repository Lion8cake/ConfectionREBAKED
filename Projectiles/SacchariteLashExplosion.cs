using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Projectiles
{
    public class SacchariteLashExplosion : ModProjectile
    {
        public override string Texture => "TheConfectionRebirth/Projectiles/CreamBolt";

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 48;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 60;
        }

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];

            player.heldProj = Projectile.whoAmI;

            return false;
        }

        public override void AI()
        {
            Projectile.velocity.Y += Projectile.ai[0];
        }

        public override void Kill(int timeLeft)
        {
            if (Main.myPlayer != Projectile.owner)
            {
                return;
            }
            int choice = Main.rand.Next(1);
            if (choice == 0)
            {
                Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center.X, Projectile.Center.Y, -8 + Main.rand.Next(0, 17), -8 + Main.rand.Next(0, 17), ModContent.ProjectileType<RockCandyShard>(), 24, 1f, Main.myPlayer, 0f, 0f);
            }

            int num = Main.rand.Next(1);
            if (num == 0)
            {
                Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center.X, Projectile.Center.Y, -8 + Main.rand.Next(0, 17), -8 + Main.rand.Next(0, 17), ModContent.ProjectileType<RockCandyShard>(), 24, 1f, Main.myPlayer, 0f, 0f);
            }
        }
    }
}
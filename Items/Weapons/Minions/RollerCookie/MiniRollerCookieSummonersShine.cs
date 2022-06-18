using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Weapons.Minions.RollerCookie
{
    public class MiniRollerCookieSummonersShine : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 6;
        }
        static int castType = 0;
        public override void SetDefaults()
        {
            Projectile.frame = castType;
            castType++;
            if (castType == 6)
                castType = 0;
            Projectile.width = 14;
            Projectile.height = 14;
            DrawOffsetX = -2;
            DrawOriginOffsetY = -2;
            Projectile.extraUpdates = 1;

            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
            Projectile.penetrate = -1;

            Projectile.timeLeft = 1200;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override bool? CanCutTiles()
        {
            return false;
        }
        public override void AI()
        {
            const float blocksPerRotation = 9;
            const float rotationPerBlock = 1 / blocksPerRotation;
            const float normalGravity = 0.1f;

            RollerCookieSummonProj.Unadhere(Projectile, (int)Projectile.ai[0]);

            if (Projectile.ai[1] == 0)
            {
                if (Projectile.velocity.Y == 0)
                {
                    Projectile.ai[1] = Math.Sign(Projectile.velocity.X);
                    if (Projectile.ai[1] == 0)
                    {
                        Projectile.ai[1] = 1;
                    }
                }
            }
            else {
                Projectile.velocity.X = Projectile.ai[1] * 4;
            }

            int flipDirection = Projectile.velocity.X > 0 ? 1 : -1;
            Projectile.rotation += Projectile.velocity.Length() * flipDirection * rotationPerBlock;

            if (Projectile.ai[0] == 0)
                Projectile.velocity.Y += normalGravity;
            else
                Projectile.velocity.Y = Math.Max(2, Projectile.velocity.Y) + 0.3f;

            RollerCookieSummonProj.Adhere(Projectile, (int)Projectile.ai[0]);

            Projectile.ai[0] = RollerCookieSummonProj.SwitchAdhering(Projectile, (int)Projectile.ai[0]);

            Lighting.AddLight(Projectile.Center, new Vector3(0.67f, 0.44f, 0.0f) * 0.7f);
            RollerCookieSummonProj.CreateDust(Projectile);

        }
    }
}

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TheConfectionRebirth.Projectiles
{
    public class SweetHook : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.IlluminantHook);
        }

        public override bool? CanUseGrapple(Player player)
        {
            int hooksOut = 0;
            for(int i = 0; i < 1000; i++)
            {
                if(Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == Projectile.type)
                {
                    hooksOut++;
                }
            }
            if(hooksOut > 3)
            {
                return false;
            }
            return true;
        }

        public override float GrappleRange()
        {
            return 350f;
        }

        public override void NumGrappleHooks(Player player, ref int numHooks)
        {
            numHooks = 1;
        }

        public override void GrappleRetreatSpeed(Player player, ref float speed)
        {
            speed = 15f;
        }

        public override void GrapplePullSpeed(Player player, ref float speed)
        {
            speed = 15f;
        }
    }
}

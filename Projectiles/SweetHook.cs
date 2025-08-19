using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;

namespace TheConfectionRebirth.Projectiles
{
    public class SweetHook : ModProjectile
    {
		private static Asset<Texture2D> chainTexture = ModContent.Request<Texture2D>("TheConfectionRebirth/Projectiles/SweetHook_Chain");

		public override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.netImportant = true;
			Projectile.aiStyle = 7;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft *= 10;
			DrawOriginOffsetY = 8;
		}

		public override void GrappleTargetPoint(Player player, ref float grappleX, ref float grappleY)
		{
			Vector2 dirToPlayer = Projectile.DirectionTo(player.Center);
			float hangDist = 10f;
			grappleX += dirToPlayer.X * hangDist;
			grappleY += dirToPlayer.Y * hangDist;
		}

		public override bool PreDrawExtras()
		{
			Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
			if (Main.player[Projectile.owner].mount.Active && Main.player[Projectile.owner].mount.Type == 52)
			{
				mountedCenter += new Vector2((float)(Main.player[Projectile.owner].direction * 14), -10f);
			}
			Texture2D value10 = (Texture2D)chainTexture;
			Vector2 center = Projectile.Center;
			Rectangle? sourceRectangle = null;
			Vector2 origin4 = new((float)value10.Width * 0.5f, (float)value10.Height * 0.5f);
			float num105 = value10.Height;
			Vector2 vector21 = mountedCenter - center;
			float rotation8 = (float)Math.Atan2(vector21.Y, vector21.X) - 1.57f;
			bool flag20 = true;
			if (float.IsNaN(center.X) && float.IsNaN(center.Y))
			{
				flag20 = false;
			}
			if (float.IsNaN(vector21.X) && float.IsNaN(vector21.Y))
			{
				flag20 = false;
			}
			while (flag20)
			{
				if (vector21.Length() < num105 + 1f)
				{
					flag20 = false;
					continue;
				}
				Vector2 vector22 = vector21;
				vector22.Normalize();
				center += vector22 * num105;
				vector21 = mountedCenter - center;
				Color color25 = Lighting.GetColor((int)center.X / 16, (int)(center.Y / 16f));
				Main.EntitySpriteDraw(value10, center - Main.screenPosition, sourceRectangle, color25, rotation8, origin4, 1f, (SpriteEffects)0);
			}
			return false;
		}

        public override bool? CanUseGrapple(Player player)
        {
            int hooksOut = 0;
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == Projectile.type)
                {
                    hooksOut++;
                }
            }
            if (hooksOut > 0)
            {
                return false;
            }
            return true;
        }

        public override float GrappleRange()
        {
            return 480f;
        }

        public override void NumGrappleHooks(Player player, ref int numHooks)
        {
            numHooks = 1;
        }

        public override void GrappleRetreatSpeed(Player player, ref float speed)
        {
            speed = 18f;
        }

        public override void GrapplePullSpeed(Player player, ref float speed)
        {
            speed = 11f;
        }
    }
}

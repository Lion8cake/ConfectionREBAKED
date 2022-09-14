using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Weapons.Minions.DuchessPrincess
{
    internal class DuchessPrincessRockCandy : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.extraUpdates = 2;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.localNPCHitCooldown = 60;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] = 1;
                Projectile.frame = Main.rand.Next(0, 5);
            }
            if (Projectile.ai[0] == -1)
                return;
            NPC target = Main.npc[(int)Projectile.ai[0]];
            if (!target.active)
            {
                Projectile.ai[0] = -1;
                return;
            }
            Vector2 disp = target.Center - Projectile.Center;
            if (disp == Vector2.Zero)
                return;

            disp.Normalize();

            float dotNum = Vector2.Dot(Projectile.velocity, disp);
            Vector2 dot = disp * dotNum;
            Projectile.velocity -= dot;
            Projectile.velocity *= 0.995f;
            Projectile.velocity += dot;

            Projectile.velocity += disp * 0.1f;
            Projectile.rotation += Projectile.velocity.X * 10;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            switch (Projectile.ai[1])
            {
                case 0:
                    lightColor = Color.Green;
                    break;
                case 1:
                    lightColor = Color.LightBlue;
                    break;
                case 2:
                    lightColor = Color.PaleVioletRed;
                    break;
                case 3:
                    lightColor = Color.Turquoise;
                    break;
                case 4:
                    lightColor = Color.LightYellow;
                    break;
                case 5:
                    lightColor = Color.Pink;
                    break;
                case 6:
                    lightColor = Color.LimeGreen;
                    break;
            }

            lightColor.A = 10;


            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int x = 0; x < 15; x+=2)
            {
                Vector2 drawPos = (Projectile.oldPos[x] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = lightColor * ((Projectile.oldPos.Length - x) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, texture.Frame(1, 5, 0, Projectile.frame), color, Projectile.oldRot[x], drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;
        }
    }
}

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
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 45;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.extraUpdates = 2;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.localNPCHitCooldown = 200;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.tileCollide = false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[1] >= 7 && target.whoAmI == Projectile.ai[0])
            {
                ChangeTarget();
            }    
        }

        void ChangeTarget()
        {
            Player player = Main.player[Projectile.owner];
            float dist = 0;
            for (int x = 0; x < Main.npc.Length; x++)
            {
                NPC npc = Main.npc[x];
                if (npc.CanBeChasedBy(Projectile) || npc.CanBeChasedBy(player))
                {
                    float disttest = (npc.Center - Projectile.Center).LengthSquared();
                    if (disttest > dist && disttest < 16 * 40 * 16 * 40 && (Projectile.ai[0] == x || Main.rand.Next(1, 3) < 3))
                    {
                        Projectile.ai[0] = x;
                        dist = disttest;
                    }
                }
            }
            if (dist == 0)
                Projectile.ai[0] = -1;
        }
        public override void AI()
        {
            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] = 1;
                Projectile.frame = Main.rand.Next(0, 5);

                if (SummonersShineCompat.SummonersShine != null && Projectile.ai[1] >= 7)
                    Projectile.timeLeft = (int)(Projectile.timeLeft * Projectile.SummonersShine_GetMinionPower(0));
            }
            if (Projectile.ai[0] == -1)
            {
                if (Projectile.ai[1] >= 7)
                    ChangeTarget();
                return;
            }
            NPC target = Main.npc[(int)Projectile.ai[0]];
            if (!target.active)
            {
                if (Projectile.ai[1] >= 7)
                    ChangeTarget();
                else
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
            float correction = Projectile.ai[1] >= 7 ? 0.98f : 0.995f;
            Projectile.velocity *= correction;
            Projectile.velocity += dot;

            Projectile.velocity += disp * 0.1f;
            Projectile.rotation += Projectile.velocity.X * 10;
        }
        public override bool PreDraw(ref Color lightColor)
        {

            float alphaRatio = 1;
            if (Projectile.timeLeft < 120)
            {
                alphaRatio = (Projectile.timeLeft / 120f);
            }
            int colorNum = (int)(Projectile.ai[1] % 7);
            switch (colorNum)
            {
                case 0:
                    lightColor = new(100, 255, 100);
                    break;
                case 1:
                    lightColor = new(150, 200, 255);
                    break;
                case 2:
                    lightColor = Color.PaleVioletRed;
                    break;
                case 3:
                    lightColor = new(200, 200, 100);
                    break;
                case 4:
                    lightColor = new(200, 200, 50);
                    break;
                case 5:
                    lightColor = new(255, 50, 100);
                    break;
                case 6:
                    lightColor = new(150, 255, 100);
                    break;
            }
            lightColor.A = 150;

            lightColor *= alphaRatio;


            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);

            bool longTrail = Projectile.ai[1] < 7;
            float trailLength = longTrail ? 15 : 45;
            int interval = longTrail ? 2 : 1;
            for (int x = 0; x < trailLength; x += interval)
            {
                Vector2 drawPos = (Projectile.oldPos[x] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = lightColor * ((trailLength - x) / trailLength);
                Main.EntitySpriteDraw(texture, drawPos, texture.Frame(1, 5, 0, Projectile.frame), color, Projectile.oldRot[x], drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;
        }
    }
}

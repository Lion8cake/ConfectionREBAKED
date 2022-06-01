using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Projectiles
{
    public class CosmicCookie : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            //Projectile.alpha = byte.MaxValue;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            AIType = 728;
        }

        public override void AI()
        {
            Projectile.velocity.Y += Projectile.ai[0];
            Projectile.rotation += 0.3f * Projectile.direction;
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<CosmicStarDust>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.penetrate--;
            if (Projectile.penetrate <= 0)
            {
                Projectile.Kill();
            }
            else
            {
                Projectile.ai[0] += 0.1f;
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y;
                }
                Projectile.velocity *= 0.75f;
                SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            }
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;
            Texture2D texture = TextureAssets.Projectile[12].Value;
            Rectangle r = new(0, 0, texture.Width, texture.Height);
            Vector2 origin1 = r.Size() / 2f;
            Color alpha = Projectile.GetAlpha(lightColor);
            Texture2D texture2D = TextureAssets.Extra[91].Value;
            Rectangle rectangle = texture2D.Frame();
            Vector2 origin2 = new(rectangle.Width / 2f, 10f);
            Vector2 vector2_1 = new(0.0f, Projectile.gfxOffY);
            Vector2 spinningpoint = new(0.0f, -10f);
            float num2 = (float)Main.timeForVisualEffects / 60f;
            Vector2 vector2_2 = Projectile.Center + Projectile.velocity;
            Color color1 = Color.Blue * 0.2f;
            Color color2 = Color.White * 0.5f;
            color2.A = 0;
            float num3 = 0.0f;
            if (Main.tenthAnniversaryWorld)
            {
                color1 = Color.HotPink * 0.3f;
                color2 = Color.White * 0.75f;
                color2.A = 0;
                num3 = -0.1f;
            }
            if (Projectile.type == Type)
            {
                color1 = Color.BlueViolet * 0.2f;
                color2 = Color.White * 0.5f;
                color2.A = 50;
                num3 = -0.2f;
            }
            Color color4 = color1;
            color4.A = 0;
            Color color5 = color1;
            color5.A = 0;
            Color color6 = color1;
            color6.A = 0;
            Main.EntitySpriteDraw(texture2D, vector2_2 - Main.screenPosition + vector2_1 + spinningpoint.RotatedBy(6.28318548202515 * (double)num2), new Rectangle?(rectangle), color4, Projectile.velocity.ToRotation() + 1.570796f, origin2, 1.5f + num3, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture2D, vector2_2 - Main.screenPosition + vector2_1 + spinningpoint.RotatedBy(6.28318548202515 * (double)num2 + 2.09439516067505), new Rectangle?(rectangle), color5, Projectile.velocity.ToRotation() + 1.570796f, origin2, 1.1f + num3, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture2D, vector2_2 - Main.screenPosition + vector2_1 + spinningpoint.RotatedBy(6.28318548202515 * (double)num2 + 4.1887903213501), new Rectangle?(rectangle), color6, Projectile.velocity.ToRotation() + 1.570796f, origin2, 1.3f + num3, SpriteEffects.None, 0);
            Vector2 vector2_3 = Projectile.Center - Projectile.velocity * 0.5f;
            for (float num4 = 0.0f; (double)num4 < 1.0; num4 += 0.5f)
            {
                float num5 = (float)(((double)num2 % 0.5 / 0.5 + (double)num4) % 1.0);
                float num6 = num5 * 2f;
                if ((double)num6 > 1.0)
                    num6 = 2f - num6;
                Main.EntitySpriteDraw(texture2D, vector2_3 - Main.screenPosition + vector2_1, new Rectangle?(rectangle), color2 * num6, Projectile.velocity.ToRotation() + 1.570796f, origin2, (float)(0.300000011920929 + (double)num5 * 0.5), SpriteEffects.None, 0);
            }
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + new Vector2(0.0f, Projectile.gfxOffY), new Rectangle?(r), alpha, Projectile.rotation, origin1, Projectile.scale + 0.1f, spriteEffects, 0);
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Vector2 v = Main.rand.NextVector2CircularEdge(200f, 200f);
            if (v.Y < 0.0)
                v.Y *= -1f;
            v.Y += 100f;
            Vector2 velocity = v.SafeNormalize(Vector2.UnitY) * 6f;
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center - velocity * 20f, velocity, ProjectileID.SuperStarSlash, Projectile.damage / 2, 0.0f, Projectile.owner, ai1: target.Center.Y);
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            Vector2 v = Main.rand.NextVector2CircularEdge(200f, 200f);
            if (v.Y < 0.0)
                v.Y *= -1f;
            v.Y += 100f;
            Vector2 velocity = v.SafeNormalize(Vector2.UnitY) * 6f;
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center - velocity * 20f, velocity, ProjectileID.SuperStarSlash, Projectile.damage / 2, 0.0f, Projectile.owner, ai1: target.Center.Y);
        }

        //Make the projectile shoot out cookie star dusts
        public override void Kill(int timeLeft)
        {
            for (int k = 15; k < 50; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<CosmicStarDust>(), Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0) * Projectile.Opacity;
        }
    }
}
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.GameContent.Drawing;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Projectiles
{
    public class TrueSucrosaSlash : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.ownerHitCheck = true;
            Projectile.ownerHitCheckDistance = 300f;
            Projectile.scale = 2f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.alpha = 255;
        }
        public override void AI()
        {
            Projectile.localAI[0]++;
            Player player = Main.player[Projectile.owner];
            float num = Projectile.localAI[0] / Projectile.ai[1];
            float num2 = Projectile.ai[0];
            float num3 = Projectile.velocity.ToRotation();
            Projectile.rotation = (float)Math.PI * num2 * num + num3 + num2 * (float)Math.PI + player.fullRotation;
            float num5 = 1f;
            float num6 = 1.2f;

            Projectile.Center = player.RotatedRelativePoint(player.MountedCenter) - Projectile.velocity;
            Projectile.scale = num6 + num * num5;

            if (!Projectile.noEnchantmentVisuals)
            {
                UpdateEnchantmentVisuals();
            }

            float num8 = Projectile.rotation + Main.rand.NextFloatDirection() * ((float)Math.PI / 2f) * 0.7f;
            Vector2 vector2 = Projectile.Center + num8.ToRotationVector2() * 84f * Projectile.scale;
            Vector2 vector3 = (num8 + Projectile.ai[0] * ((float)Math.PI / 2f)).ToRotationVector2();
            //if (Main.rand.NextFloat() * 2f < Projectile.Opacity)
            {
                Dust dust2 = Dust.NewDustPerfect(Projectile.Center + num8.ToRotationVector2() * (Main.rand.NextFloat() * 80f * Projectile.scale + 20f * Projectile.scale), ModContent.DustType<NeapoliniteDust>(), vector3 * 1f, ModContent.DustType<NeapoliniteDust>(), Color.Lerp(Color.Gold, Color.Purple, Main.rand.NextFloat() * 1f), 0.4f);
                dust2.fadeIn = 0.4f + Main.rand.NextFloat() * 0.15f;
                dust2.noGravity = true;
            }
            //if (Main.rand.NextFloat() * 1.5f < Projectile.Opacity)
            {
                Dust.NewDustPerfect(vector2, ModContent.DustType<NeapoliniteDust>(), vector3 * 1f, 100, Color.White * Projectile.Opacity, 1.2f * Projectile.Opacity);
            }
            //Projectile.scale *= Projectile.ai[2];
            if (Projectile.localAI[0] >= Projectile.ai[1])
            {
                Projectile.Kill();
            }
        }
        public override void CutTiles()
        {
            Vector2 vector2 = (Projectile.rotation - (float)Math.PI / 4f).ToRotationVector2() * 60f * Projectile.scale;
            Vector2 vector3 = (Projectile.rotation + (float)Math.PI / 4f).ToRotationVector2() * 60f * Projectile.scale;
            float num2 = 60f * Projectile.scale;
            Utils.PlotTileLine(Projectile.Center + vector2, Projectile.Center + vector3, num2, DelegateMethods.CutTiles);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            DrawProj_Excalibur(Projectile);
            return true;
        }
        public override bool? CanCutTiles() => true;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
            ParticleOrchestraSettings particleOrchestraSettings = default(ParticleOrchestraSettings);
            particleOrchestraSettings.PositionInWorld = positionInWorld;
            ParticleOrchestraSettings settings = particleOrchestraSettings;
            ParticleOrchestrator.RequestParticleSpawn(false, ParticleOrchestraType.RainbowRodHit, settings, Projectile.owner);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float coneLength2 = 94f * Projectile.scale;
            float num3 = (float)Math.PI * 2f / 25f * Projectile.ai[0];
            float maximumAngle2 = (float)Math.PI / 4f;
            float num4 = Projectile.rotation + num3;
            if (targetHitbox.IntersectsConeSlowMoreAccurate(Projectile.Center, coneLength2, num4, maximumAngle2))
            {
                return true;
            }
            float num5 = Utils.Remap(Projectile.localAI[0], Projectile.ai[1] * 0.3f, Projectile.ai[1] * 0.5f, 1f, 0f);
            if (num5 > 0f)
            {
                float coneRotation2 = num4 - (float)Math.PI / 4f * Projectile.ai[0] * num5;
                if (targetHitbox.IntersectsConeSlowMoreAccurate(Projectile.Center, coneLength2, coneRotation2, maximumAngle2))
                {
                    return true;
                }
            }
            return false;
        }

        private static void DrawPrettyStarSparkle(float opacity, SpriteEffects dir, Vector2 drawpos, Color drawColor, Color shineColor, Color SparkleColor, Color BigSparkleColor, float flareCounter, float fadeInStart, float fadeInEnd, float fadeOutStart, float fadeOutEnd, float rotation, Vector2 scale, Vector2 fatness)
        {
            Texture2D value = Main.Assets.Request<Texture2D>("Images/Projectile_658").Value;
            Color color = shineColor * opacity * 0.5f;
            color.A = 0;
            Vector2 origin = value.Size() / 2f;
            Color color2 = drawColor * 0.5f;
            float num = Utils.GetLerpValue(fadeInStart, fadeInEnd, flareCounter, clamped: true) * Utils.GetLerpValue(fadeOutEnd, fadeOutStart, flareCounter, clamped: true);
            Vector2 vector = new Vector2(fatness.X * 0.5f, scale.X) * num;
            Vector2 vector2 = new Vector2(fatness.Y * 0.5f, scale.Y) * num;
            color *= num;
            color2 *= num;
            Main.EntitySpriteDraw(value, drawpos, null, SparkleColor, (float)Math.PI / 2f + rotation, origin, vector, dir, 0);
            Main.EntitySpriteDraw(value, drawpos, null, SparkleColor, 0f + rotation, origin, vector2, dir, 0);
            Main.EntitySpriteDraw(value, drawpos, null, SparkleColor, (float)Math.PI / 2f + rotation, origin, vector * 0.6f, dir, 0);
            Main.EntitySpriteDraw(value, drawpos, null, SparkleColor, 0f + rotation, origin, vector2 * 0.6f, dir, 0);
        }
        public void DrawProj_Excalibur(Projectile proj)
        {
            Vector2 vector = proj.Center - Main.screenPosition;
            Asset<Texture2D> val = ModContent.Request<Texture2D>("TheConfectionRebirth/Projectiles/SucrosaSlash");
            Rectangle rectangle = val.Frame(1, 4);
            Vector2 origin = rectangle.Size() / 2f;
            float num = proj.scale * 1.1f;
            SpriteEffects effects = ((!(proj.ai[0] >= 0f)) ? SpriteEffects.FlipVertically : SpriteEffects.None);
            float num2 = proj.localAI[0] / proj.ai[1];
            float num3 = Utils.Remap(num2, 0f, 0.6f, 0f, 1f) * Utils.Remap(num2, 0.6f, 1f, 1f, 0f);
            float num4 = 0.975f;
            float fromValue = Lighting.GetColor(proj.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
            fromValue = Utils.Remap(fromValue, 0.2f, 1f, 0f, 1f);
            Main.spriteBatch.Draw(val.Value, vector, rectangle, new Color(117, 43, 78) * fromValue * num3, proj.rotation + proj.ai[0] * ((float)Math.PI / 4f) * -1f * (1f - num2), origin, num, effects, 0f);
            Color questionmarkcolor = Color.White * num3 * 0.5f;
            questionmarkcolor.A = (byte)(questionmarkcolor.A * (1f - fromValue));
            Color color5 = questionmarkcolor * fromValue * 0.5f;
            color5.G = (byte)(color5.G * fromValue);
            color5.B = (byte)(color5.R * (0.25f + fromValue * 0.75f));
            Main.spriteBatch.Draw(val.Value, vector, rectangle, color5 * 0.15f, proj.rotation + proj.ai[0] * 0.01f, origin, num * 0.8f, effects, 0f);
            Main.spriteBatch.Draw(val.Value, vector, rectangle, new Color(117, 43, 78) * 0.4f, proj.rotation, origin, num * 0.8f, effects, 0f);
            Main.spriteBatch.Draw(val.Value, vector, rectangle, new Color(102, 61, 49) * 0.4f, proj.rotation, origin, num * num4 * 0.8f, effects, 0f);
            Main.spriteBatch.Draw(val.Value, vector, val.Frame(1, 4, 0, 3), Color.White * 0.6f * num3, proj.rotation + proj.ai[0] * 0.01f, origin, num * 0.8f, effects, 0f);
            Main.spriteBatch.Draw(val.Value, vector, val.Frame(1, 4, 0, 3), Color.White * 0.5f * num3, proj.rotation + proj.ai[0] * -0.05f, origin, num * 0.8f * 0.8f, effects, 0f);
            Main.spriteBatch.Draw(val.Value, vector, val.Frame(1, 4, 0, 3), Color.White * 0.4f * num3, proj.rotation + proj.ai[0] * -0.1f, origin, num * 0.6f * 0.8f, effects, 0f);
            for (float num5 = 0f; num5 < 8f; num5++)
            {
                float num6 = proj.rotation + proj.ai[0] * num5 * ((float)Math.PI * -2f) * 0.025f + Utils.Remap(num2, 0f, 1.2f, 0f, (float)Math.PI / 4f) * proj.ai[0];
                Vector2 drawpos = vector + num6.ToRotationVector2() * (val.Value.Width * 0.5f - 6f) * num * 1f;
                float num7 = num5 / 9f;
                DrawPrettyStarSparkle(proj.Opacity, SpriteEffects.None, drawpos, new Color(255, 255, 255, 0) * num3 * num7, new Color(117, 43, 78), new Color(25, 25, 25, 0), new Color(146, 48, 89, 0), num2, 0f, 0.5f, 0.5f, 1f, num6, new Vector2(0f, Utils.Remap(num2, 0f, 1f, 3f, 0f)) * num, Vector2.One * num);
            }
            Vector2 drawpos2 = vector + (proj.rotation + Utils.Remap(num2, 0f, 1f, 0f, (float)Math.PI / 4f) * proj.ai[0]).ToRotationVector2() * (val.Value.Width * 0.5f - 4f) * num * 0.8f;
            DrawPrettyStarSparkle(proj.Opacity, SpriteEffects.None, drawpos2, new Color(255, 255, 255, 0) * num3 * 0.5f, new Color(117, 43, 78), new Color(146, 48, 89, 0), new Color(146, 48, 89, 0), num2, 0f, 0.5f, 0.5f, 1f, 0f, new Vector2(2f, Utils.Remap(num2, 0f, 1f, 4f, 1f)) * num, Vector2.One * num);
        }
        private void UpdateEnchantmentVisuals()
        {
            if (Projectile.npcProj)
            {
                return;
            }
            Vector2 boxPosition = Projectile.position;
            int boxWidth = Projectile.width;
            int boxHeight = Projectile.height;
            for (float num = -(float)Math.PI / 4f; num <= (float)Math.PI / 4f; num += (float)Math.PI / 2f)
            {
                Rectangle r = Utils.CenteredRectangle(Projectile.Center + (Projectile.rotation + num).ToRotationVector2() * 70f * Projectile.scale, new Vector2(60f * Projectile.scale, 60f * Projectile.scale));
                EmitEnchantmentVisualsAt(r.TopLeft(), r.Width, r.Height);
            }
        }
        public void EmitEnchantmentVisualsAt(Vector2 boxPosition, int boxWidth, int boxHeight)
        {
            Player player = Main.player[Projectile.owner];
            if (player.frostBurn && (Projectile.DamageType == DamageClass.Melee || Projectile.DamageType == DamageClass.Ranged) && Projectile.friendly && !Projectile.hostile && !Projectile.noEnchantments && Main.rand.Next(2 * (1 + Projectile.extraUpdates)) == 0)
            {
                int num = Dust.NewDust(boxPosition, boxWidth, boxHeight, 135, Projectile.velocity.X * 0.2f + (float)(Projectile.direction * 3), Projectile.velocity.Y * 0.2f, 100, default(Color), 2f);
                Main.dust[num].noGravity = true;
                Main.dust[num].velocity *= 0.7f;
                Main.dust[num].velocity.Y -= 0.5f;
            }
            if (Projectile.DamageType == DamageClass.Melee && player.magmaStone && !Projectile.noEnchantments && Main.rand.Next(3) != 0)
            {
                int num2 = Dust.NewDust(new Vector2(boxPosition.X - 4f, boxPosition.Y - 4f), boxWidth + 8, boxHeight + 8, 6, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default(Color), 2f);
                if (Main.rand.Next(2) == 0)
                {
                    Main.dust[num2].scale = 1.5f;
                }
                Main.dust[num2].noGravity = true;
                Main.dust[num2].velocity.X *= 2f;
                Main.dust[num2].velocity.Y *= 2f;
            }
            if (Projectile.DamageType != DamageClass.Melee || player.meleeEnchant <= 0 || Projectile.noEnchantments)
            {
                return;
            }
            if (player.meleeEnchant == 1 && Main.rand.Next(3) == 0)
            {
                int num3 = Dust.NewDust(boxPosition, boxWidth, boxHeight, 171, 0f, 0f, 100);
                Main.dust[num3].noGravity = true;
                Main.dust[num3].fadeIn = 1.5f;
                Main.dust[num3].velocity *= 0.25f;
            }
            if (player.meleeEnchant == 1)
            {
                if (Main.rand.Next(3) == 0)
                {
                    int num4 = Dust.NewDust(boxPosition, boxWidth, boxHeight, 171, 0f, 0f, 100);
                    Main.dust[num4].noGravity = true;
                    Main.dust[num4].fadeIn = 1.5f;
                    Main.dust[num4].velocity *= 0.25f;
                }
            }
            else if (player.meleeEnchant == 2)
            {
                if (Main.rand.Next(2) == 0)
                {
                    int num5 = Dust.NewDust(boxPosition, boxWidth, boxHeight, 75, Projectile.velocity.X * 0.2f + (float)(Projectile.direction * 3), Projectile.velocity.Y * 0.2f, 100, default(Color), 2.5f);
                    Main.dust[num5].noGravity = true;
                    Main.dust[num5].velocity *= 0.7f;
                    Main.dust[num5].velocity.Y -= 0.5f;
                }
            }
            else if (player.meleeEnchant == 3)
            {
                if (Main.rand.Next(2) == 0)
                {
                    int num6 = Dust.NewDust(boxPosition, boxWidth, boxHeight, 6, Projectile.velocity.X * 0.2f + (float)(Projectile.direction * 3), Projectile.velocity.Y * 0.2f, 100, default(Color), 2.5f);
                    Main.dust[num6].noGravity = true;
                    Main.dust[num6].velocity *= 0.7f;
                    Main.dust[num6].velocity.Y -= 0.5f;
                }
            }
            else if (player.meleeEnchant == 4)
            {
                int num7 = 0;
                if (Main.rand.Next(2) == 0)
                {
                    num7 = Dust.NewDust(boxPosition, boxWidth, boxHeight, 57, Projectile.velocity.X * 0.2f + (float)(Projectile.direction * 3), Projectile.velocity.Y * 0.2f, 100, default(Color), 1.1f);
                    Main.dust[num7].noGravity = true;
                    Main.dust[num7].velocity.X /= 2f;
                    Main.dust[num7].velocity.Y /= 2f;
                }
            }
            else if (player.meleeEnchant == 5)
            {
                if (Main.rand.Next(2) == 0)
                {
                    int num8 = Dust.NewDust(boxPosition, boxWidth, boxHeight, 169, 0f, 0f, 100);
                    Main.dust[num8].velocity.X += Projectile.direction;
                    Main.dust[num8].velocity.Y += 0.2f;
                    Main.dust[num8].noGravity = true;
                }
            }
            else if (player.meleeEnchant == 6)
            {
                if (Main.rand.Next(2) == 0)
                {
                    int num9 = Dust.NewDust(boxPosition, boxWidth, boxHeight, 135, 0f, 0f, 100);
                    Main.dust[num9].velocity.X += Projectile.direction;
                    Main.dust[num9].velocity.Y += 0.2f;
                    Main.dust[num9].noGravity = true;
                }
            }
            else if (player.meleeEnchant == 7)
            {
                Vector2 vector = Projectile.velocity;
                if (vector.Length() > 4f)
                {
                    vector *= 4f / vector.Length();
                }
                if (Main.rand.Next(20) == 0)
                {
                    int num10 = Main.rand.Next(139, 143);
                    int num11 = Dust.NewDust(boxPosition, boxWidth, boxHeight, num10, vector.X, vector.Y, 0, default(Color), 1.2f);
                    Main.dust[num11].velocity.X *= 1f + (float)Main.rand.Next(-50, 51) * 0.01f;
                    Main.dust[num11].velocity.Y *= 1f + (float)Main.rand.Next(-50, 51) * 0.01f;
                    Main.dust[num11].velocity.X += (float)Main.rand.Next(-50, 51) * 0.05f;
                    Main.dust[num11].velocity.Y += (float)Main.rand.Next(-50, 51) * 0.05f;
                    Main.dust[num11].scale *= 1f + (float)Main.rand.Next(-30, 31) * 0.01f;
                }
                if (Main.rand.Next(40) == 0)
                {
                    int num12 = Main.rand.Next(276, 283);
                    int num13 = Gore.NewGore(Projectile.GetSource_FromAI(), Projectile.position, vector, num12);
                    Main.gore[num13].velocity.X *= 1f + (float)Main.rand.Next(-50, 51) * 0.01f;
                    Main.gore[num13].velocity.Y *= 1f + (float)Main.rand.Next(-50, 51) * 0.01f;
                    Main.gore[num13].scale *= 1f + (float)Main.rand.Next(-20, 21) * 0.01f;
                    Main.gore[num13].velocity.X += (float)Main.rand.Next(-50, 51) * 0.05f;
                    Main.gore[num13].velocity.Y += (float)Main.rand.Next(-50, 51) * 0.05f;
                }
            }
            else if (player.meleeEnchant == 8 && Main.rand.Next(4) == 0)
            {
                int num14 = Dust.NewDust(boxPosition, boxWidth, boxHeight, 46, 0f, 0f, 100);
                Main.dust[num14].noGravity = true;
                Main.dust[num14].fadeIn = 1.5f;
                Main.dust[num14].velocity *= 0.25f;
            }
        }
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }
    }

    public class TrueSucrosaSlash2 : ModProjectile
    {
        public override string Texture => "TheConfectionRebirth/Projectiles/TrueSucrosaSlash";

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.ownerHitCheck = true;
            Projectile.ownerHitCheckDistance = 300f;
            Projectile.scale = 4f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.alpha = 255;
        }
        public override void AI()
        {
            Projectile.localAI[0]++;
            Player player = Main.player[Projectile.owner];
            float num = Projectile.localAI[0] / Projectile.ai[1];
            float num2 = Projectile.ai[0];
            float num3 = Projectile.velocity.ToRotation();
            Projectile.rotation = (float)Math.PI * num2 * num + num3 + num2 * (float)Math.PI + player.fullRotation;
            float num5 = 1f;
            float num6 = 1.2f;

            Projectile.Center = player.RotatedRelativePoint(player.MountedCenter) - Projectile.velocity;
            Projectile.scale = num6 + num * num5;

            if (!Projectile.noEnchantmentVisuals)
            {
                UpdateEnchantmentVisuals();
            }

            float num8 = Projectile.rotation + Main.rand.NextFloatDirection() * ((float)Math.PI / 2f) * 0.7f;
            Vector2 vector2 = Projectile.Center + num8.ToRotationVector2() * 84f * Projectile.scale;
            Vector2 vector3 = (num8 + Projectile.ai[0] * ((float)Math.PI / 2f)).ToRotationVector2();
            //if (Main.rand.NextFloat() * 2f < Projectile.Opacity)
            {
                Dust dust2 = Dust.NewDustPerfect(Projectile.Center + num8.ToRotationVector2() * (Main.rand.NextFloat() * 80f * Projectile.scale + 20f * Projectile.scale), ModContent.DustType<NeapoliniteCrumbs>(), vector3 * 1f, ModContent.DustType<NeapoliniteDust>(), Color.Lerp(Color.Gold, Color.Purple, Main.rand.NextFloat() * 1f), 0.4f);
                dust2.fadeIn = 0.4f + Main.rand.NextFloat() * 0.15f;
                dust2.noGravity = true;
            }
            //if (Main.rand.NextFloat() * 1.5f < Projectile.Opacity)
            {
                Dust.NewDustPerfect(vector2, ModContent.DustType<NeapoliniteCrumbs>(), vector3 * 1f, 100, Color.White * Projectile.Opacity, 1.2f * Projectile.Opacity);
            }
            //Projectile.scale *= Projectile.ai[2];
            if (Projectile.localAI[0] >= Projectile.ai[1])
            {
                Projectile.Kill();
            }
        }
        public override void CutTiles()
        {
            Vector2 vector2 = (Projectile.rotation - (float)Math.PI / 4f).ToRotationVector2() * 60f * Projectile.scale;
            Vector2 vector3 = (Projectile.rotation + (float)Math.PI / 4f).ToRotationVector2() * 60f * Projectile.scale;
            float num2 = 60f * Projectile.scale;
            Utils.PlotTileLine(Projectile.Center + vector2, Projectile.Center + vector3, num2, DelegateMethods.CutTiles);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            DrawProj_Excalibur(Projectile);
            return true;
        }
        public override bool? CanCutTiles() => true;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
            ParticleOrchestraSettings particleOrchestraSettings = default(ParticleOrchestraSettings);
            particleOrchestraSettings.PositionInWorld = positionInWorld;
            ParticleOrchestraSettings settings = particleOrchestraSettings;
            ParticleOrchestrator.RequestParticleSpawn(false, ParticleOrchestraType.RainbowRodHit, settings, Projectile.owner);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float coneLength2 = 94f * Projectile.scale;
            float num3 = (float)Math.PI * 2f / 25f * Projectile.ai[0];
            float maximumAngle2 = (float)Math.PI / 4f;
            float num4 = Projectile.rotation + num3;
            if (targetHitbox.IntersectsConeSlowMoreAccurate(Projectile.Center, coneLength2, num4, maximumAngle2))
            {
                return true;
            }
            float num5 = Utils.Remap(Projectile.localAI[0], Projectile.ai[1] * 0.3f, Projectile.ai[1] * 0.5f, 1f, 0f);
            if (num5 > 0f)
            {
                float coneRotation2 = num4 - (float)Math.PI / 4f * Projectile.ai[0] * num5;
                if (targetHitbox.IntersectsConeSlowMoreAccurate(Projectile.Center, coneLength2, coneRotation2, maximumAngle2))
                {
                    return true;
                }
            }
            return false;
        }
        private static void DrawPrettyStarSparkle(float opacity, SpriteEffects dir, Vector2 drawpos, Color drawColor, Color shineColor, Color SparkleColor, Color BigSparkleColor, float flareCounter, float fadeInStart, float fadeInEnd, float fadeOutStart, float fadeOutEnd, float rotation, Vector2 scale, Vector2 fatness)
        {
            Texture2D value = Main.Assets.Request<Texture2D>("Images/Projectile_658").Value;
            Color color = shineColor * opacity * 0.5f;
            color.A = 0;
            Vector2 origin = value.Size() / 2f;
            Color color2 = drawColor * 0.5f;
            float num = Utils.GetLerpValue(fadeInStart, fadeInEnd, flareCounter, clamped: true) * Utils.GetLerpValue(fadeOutEnd, fadeOutStart, flareCounter, clamped: true);
            Vector2 vector = new Vector2(fatness.X * 0.5f, scale.X) * num;
            Vector2 vector2 = new Vector2(fatness.Y * 0.5f, scale.Y) * num;
            color *= num;
            color2 *= num;
            Main.EntitySpriteDraw(value, drawpos, null, SparkleColor, (float)Math.PI / 2f + rotation, origin, vector, dir, 0);
            Main.EntitySpriteDraw(value, drawpos, null, SparkleColor, 0f + rotation, origin, vector2, dir, 0);
            Main.EntitySpriteDraw(value, drawpos, null, SparkleColor, (float)Math.PI / 2f + rotation, origin, vector * 0.6f, dir, 0);
            Main.EntitySpriteDraw(value, drawpos, null, SparkleColor, 0f + rotation, origin, vector2 * 0.6f, dir, 0);
        }
        public void DrawProj_Excalibur(Projectile proj)
        {
            Vector2 vector = proj.Center - Main.screenPosition;
            Asset<Texture2D> val = ModContent.Request<Texture2D>("TheConfectionRebirth/Projectiles/SucrosaSlash");
            Rectangle rectangle = val.Frame(1, 4);
            Vector2 origin = rectangle.Size() / 2f;
            float num = proj.scale * 1.1f;
            SpriteEffects effects = ((!(proj.ai[0] >= 0f)) ? SpriteEffects.FlipVertically : SpriteEffects.None);
            float num2 = proj.localAI[0] / proj.ai[1];
            float num3 = Utils.Remap(num2, 0f, 0.6f, 0f, 1f) * Utils.Remap(num2, 0.6f, 1f, 1f, 0f);
            float num4 = 0.975f;
            float fromValue = Lighting.GetColor(proj.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
            fromValue = Utils.Remap(fromValue, 0.2f, 1f, 0f, 1f);
            Main.spriteBatch.Draw(val.Value, vector, rectangle, new Color(102, 61, 49) * fromValue * num3, proj.rotation + proj.ai[0] * ((float)Math.PI / 4f) * -1f * (1f - num2), origin, num, effects, 0f);
            Color questionmarkcolor = Color.White * num3 * 0.5f;
            questionmarkcolor.A = (byte)(questionmarkcolor.A * (1f - fromValue));
            Color color5 = questionmarkcolor * fromValue * 0.5f;
            color5.G = (byte)(color5.G * fromValue);
            color5.B = (byte)(color5.R * (0.25f + fromValue * 0.75f));
            Main.spriteBatch.Draw(val.Value, vector, rectangle, color5 * 0.15f, proj.rotation + proj.ai[0] * 0.01f, origin, num * 1.2f, effects, 0f);
            Main.spriteBatch.Draw(val.Value, vector, rectangle, new Color(26, 54, 85) * 0.4f, proj.rotation, origin, num * 0.8f, effects, 0f);
            Main.spriteBatch.Draw(val.Value, vector, rectangle, new Color(26, 86, 115) * 0.4f, proj.rotation, origin, num * num4 * 1.2f, effects, 0f);
            Main.spriteBatch.Draw(val.Value, vector, val.Frame(1, 4, 0, 3), Color.White * 0.6f * num3, proj.rotation + proj.ai[0] * 0.01f, origin, num * 1.2f, effects, 0f);
            Main.spriteBatch.Draw(val.Value, vector, val.Frame(1, 4, 0, 3), Color.White * 0.5f * num3, proj.rotation + proj.ai[0] * -0.05f, origin, num * 0.8f * 1.2f, effects, 0f);
            Main.spriteBatch.Draw(val.Value, vector, val.Frame(1, 4, 0, 3), Color.White * 0.4f * num3, proj.rotation + proj.ai[0] * -0.1f, origin, num * 0.6f * 1.2f, effects, 0f);
            for (float num5 = 0f; num5 < 8f; num5++)
            {
                float num6 = proj.rotation + proj.ai[0] * num5 * ((float)Math.PI * -2f) * 0.025f + Utils.Remap(num2, 0f, 1f, 0f, (float)Math.PI / 4f) * proj.ai[0];
                Vector2 drawpos = vector + num6.ToRotationVector2() * (val.Value.Width * 0.5f - 6f) * num * 0.6f;
                float num7 = num5 / 9f;
                DrawPrettyStarSparkle(proj.Opacity, SpriteEffects.None, drawpos, new Color(255, 255, 255, 0) * num3 * num7, new Color(26, 54, 85), new Color(26, 54, 85), new Color(32, 104, 147, 0), num2, 0f, 0.5f, 0.5f, 1f, num6, new Vector2(0f, Utils.Remap(num2, 0f, 1f, 3f, 0f)) * num, Vector2.One * num);
            }
            Vector2 drawpos2 = vector + (proj.rotation + Utils.Remap(num2, 0f, 1f, 0f, (float)Math.PI / 4f) * proj.ai[0]).ToRotationVector2() * (val.Value.Width * 0.5f - 4f) * num * 1.2f;
            DrawPrettyStarSparkle(proj.Opacity, SpriteEffects.None, drawpos2, new Color(255, 255, 255, 0) * num3 * 0.5f, new Color(26, 54, 85), new Color(32, 104, 147, 0), new Color(32, 104, 147, 0), num2, 0f, 0.5f, 0.5f, 1f, 0f, new Vector2(2f, Utils.Remap(num2, 0f, 1f, 4f, 1f)) * num, Vector2.One * num);
        }
        private void UpdateEnchantmentVisuals()
        {
            if (Projectile.npcProj)
            {
                return;
            }
            Vector2 boxPosition = Projectile.position;
            int boxWidth = Projectile.width;
            int boxHeight = Projectile.height;
            for (float num = -(float)Math.PI / 4f; num <= (float)Math.PI / 4f; num += (float)Math.PI / 2f)
            {
                Rectangle r = Utils.CenteredRectangle(Projectile.Center + (Projectile.rotation + num).ToRotationVector2() * 70f * Projectile.scale, new Vector2(60f * Projectile.scale, 60f * Projectile.scale));
                EmitEnchantmentVisualsAt(r.TopLeft(), r.Width, r.Height);
            }
        }
        public void EmitEnchantmentVisualsAt(Vector2 boxPosition, int boxWidth, int boxHeight)
        {
            Player player = Main.player[Projectile.owner];
            if (player.frostBurn && (Projectile.DamageType == DamageClass.Melee || Projectile.DamageType == DamageClass.Ranged) && Projectile.friendly && !Projectile.hostile && !Projectile.noEnchantments && Main.rand.Next(2 * (1 + Projectile.extraUpdates)) == 0)
            {
                int num = Dust.NewDust(boxPosition, boxWidth, boxHeight, 135, Projectile.velocity.X * 0.2f + (float)(Projectile.direction * 3), Projectile.velocity.Y * 0.2f, 100, default(Color), 2f);
                Main.dust[num].noGravity = true;
                Main.dust[num].velocity *= 0.7f;
                Main.dust[num].velocity.Y -= 0.5f;
            }
            if (Projectile.DamageType == DamageClass.Melee && player.magmaStone && !Projectile.noEnchantments && Main.rand.Next(3) != 0)
            {
                int num2 = Dust.NewDust(new Vector2(boxPosition.X - 4f, boxPosition.Y - 4f), boxWidth + 8, boxHeight + 8, 6, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default(Color), 2f);
                if (Main.rand.Next(2) == 0)
                {
                    Main.dust[num2].scale = 1.5f;
                }
                Main.dust[num2].noGravity = true;
                Main.dust[num2].velocity.X *= 2f;
                Main.dust[num2].velocity.Y *= 2f;
            }
            if (Projectile.DamageType != DamageClass.Melee || player.meleeEnchant <= 0 || Projectile.noEnchantments)
            {
                return;
            }
            if (player.meleeEnchant == 1 && Main.rand.Next(3) == 0)
            {
                int num3 = Dust.NewDust(boxPosition, boxWidth, boxHeight, 171, 0f, 0f, 100);
                Main.dust[num3].noGravity = true;
                Main.dust[num3].fadeIn = 1.5f;
                Main.dust[num3].velocity *= 0.25f;
            }
            if (player.meleeEnchant == 1)
            {
                if (Main.rand.Next(3) == 0)
                {
                    int num4 = Dust.NewDust(boxPosition, boxWidth, boxHeight, 171, 0f, 0f, 100);
                    Main.dust[num4].noGravity = true;
                    Main.dust[num4].fadeIn = 1.5f;
                    Main.dust[num4].velocity *= 0.25f;
                }
            }
            else if (player.meleeEnchant == 2)
            {
                if (Main.rand.Next(2) == 0)
                {
                    int num5 = Dust.NewDust(boxPosition, boxWidth, boxHeight, 75, Projectile.velocity.X * 0.2f + (float)(Projectile.direction * 3), Projectile.velocity.Y * 0.2f, 100, default(Color), 2.5f);
                    Main.dust[num5].noGravity = true;
                    Main.dust[num5].velocity *= 0.7f;
                    Main.dust[num5].velocity.Y -= 0.5f;
                }
            }
            else if (player.meleeEnchant == 3)
            {
                if (Main.rand.Next(2) == 0)
                {
                    int num6 = Dust.NewDust(boxPosition, boxWidth, boxHeight, 6, Projectile.velocity.X * 0.2f + (float)(Projectile.direction * 3), Projectile.velocity.Y * 0.2f, 100, default(Color), 2.5f);
                    Main.dust[num6].noGravity = true;
                    Main.dust[num6].velocity *= 0.7f;
                    Main.dust[num6].velocity.Y -= 0.5f;
                }
            }
            else if (player.meleeEnchant == 4)
            {
                int num7 = 0;
                if (Main.rand.Next(2) == 0)
                {
                    num7 = Dust.NewDust(boxPosition, boxWidth, boxHeight, 57, Projectile.velocity.X * 0.2f + (float)(Projectile.direction * 3), Projectile.velocity.Y * 0.2f, 100, default(Color), 1.1f);
                    Main.dust[num7].noGravity = true;
                    Main.dust[num7].velocity.X /= 2f;
                    Main.dust[num7].velocity.Y /= 2f;
                }
            }
            else if (player.meleeEnchant == 5)
            {
                if (Main.rand.Next(2) == 0)
                {
                    int num8 = Dust.NewDust(boxPosition, boxWidth, boxHeight, 169, 0f, 0f, 100);
                    Main.dust[num8].velocity.X += Projectile.direction;
                    Main.dust[num8].velocity.Y += 0.2f;
                    Main.dust[num8].noGravity = true;
                }
            }
            else if (player.meleeEnchant == 6)
            {
                if (Main.rand.Next(2) == 0)
                {
                    int num9 = Dust.NewDust(boxPosition, boxWidth, boxHeight, 135, 0f, 0f, 100);
                    Main.dust[num9].velocity.X += Projectile.direction;
                    Main.dust[num9].velocity.Y += 0.2f;
                    Main.dust[num9].noGravity = true;
                }
            }
            else if (player.meleeEnchant == 7)
            {
                Vector2 vector = Projectile.velocity;
                if (vector.Length() > 4f)
                {
                    vector *= 4f / vector.Length();
                }
                if (Main.rand.Next(20) == 0)
                {
                    int num10 = Main.rand.Next(139, 143);
                    int num11 = Dust.NewDust(boxPosition, boxWidth, boxHeight, num10, vector.X, vector.Y, 0, default(Color), 1.2f);
                    Main.dust[num11].velocity.X *= 1f + (float)Main.rand.Next(-50, 51) * 0.01f;
                    Main.dust[num11].velocity.Y *= 1f + (float)Main.rand.Next(-50, 51) * 0.01f;
                    Main.dust[num11].velocity.X += (float)Main.rand.Next(-50, 51) * 0.05f;
                    Main.dust[num11].velocity.Y += (float)Main.rand.Next(-50, 51) * 0.05f;
                    Main.dust[num11].scale *= 1f + (float)Main.rand.Next(-30, 31) * 0.01f;
                }
                if (Main.rand.Next(40) == 0)
                {
                    int num12 = Main.rand.Next(276, 283);
                    int num13 = Gore.NewGore(Projectile.GetSource_FromAI(), Projectile.position, vector, num12);
                    Main.gore[num13].velocity.X *= 1f + (float)Main.rand.Next(-50, 51) * 0.01f;
                    Main.gore[num13].velocity.Y *= 1f + (float)Main.rand.Next(-50, 51) * 0.01f;
                    Main.gore[num13].scale *= 1f + (float)Main.rand.Next(-20, 21) * 0.01f;
                    Main.gore[num13].velocity.X += (float)Main.rand.Next(-50, 51) * 0.05f;
                    Main.gore[num13].velocity.Y += (float)Main.rand.Next(-50, 51) * 0.05f;
                }
            }
            else if (player.meleeEnchant == 8 && Main.rand.Next(4) == 0)
            {
                int num14 = Dust.NewDust(boxPosition, boxWidth, boxHeight, 46, 0f, 0f, 100);
                Main.dust[num14].noGravity = true;
                Main.dust[num14].fadeIn = 1.5f;
                Main.dust[num14].velocity *= 0.25f;
            }
        }
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        } 
    }
}
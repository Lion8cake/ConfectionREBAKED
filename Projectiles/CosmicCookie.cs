using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Gores;
using TheConfectionRebirth.Items.Weapons;

namespace TheConfectionRebirth.Projectiles
{
    public class CosmicCookie : ModProjectile
    {
		public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
			Projectile.aiStyle = 151;
			Projectile.alpha = 255;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = true;            
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
		}

		public override bool PreAI()
		{
			Projectile.alpha -= 10;
			int num = 100;
			if (Projectile.alpha < num)
			{
				Projectile.alpha = num;
			}
			if (Projectile.soundDelay == 0)
			{
				Projectile.soundDelay = 20 + Main.rand.Next(40);
				SoundEngine.PlaySound(in SoundID.Item9, Projectile.position);
			}
			Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.005f * (float)Projectile.direction;
			Vector2 vector = new((float)Main.screenWidth, (float)Main.screenHeight);
			Rectangle hitbox = Projectile.Hitbox;
			if (hitbox.Intersects(Utils.CenteredRectangle(Main.screenPosition + vector / 2f, vector + new Vector2(400f))) && Main.rand.NextBool(6))
			{
				Gore.NewGore(Projectile.GetSource_FromAI(), Projectile.position, Projectile.velocity * 0.2f, Main.rand.NextBool(4) ? ModContent.GoreType<CosmicSacchariteStar>() : ModContent.GoreType<CosmicBrownieStar>());
			}
			for (int i = 0; i < 2; i++)
			{
				if (Main.rand.NextBool(8))
				{
					int num2 = ModContent.DustType<ChocolateFlame>();
					Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, num2, 0f, 0f, 127);
					dust.velocity *= 0.25f;
					dust.scale = 1.3f;
					dust.noGravity = true;
					dust.velocity += Projectile.velocity.RotatedBy((float)Math.PI / 8f * (1f - (float)(2 * i))) * 0.2f;
				}
			}
			return false; //so the projectile still is classified as ai 151
		}

        public override bool PreDraw(ref Color lightColor)
        {
			SpriteEffects dir = (SpriteEffects)0;
			if (Projectile.spriteDirection == -1)
			{
				dir = (SpriteEffects)1;
			}
			Texture2D value122 = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle rectangle25 = new(0, 0, value122.Width, value122.Height);
			Vector2 origin33 = rectangle25.Size() / 2f;
			Color alpha13 = Projectile.GetAlpha(lightColor);
			Texture2D value123 = TextureAssets.Extra[91].Value;
			Rectangle value124 = value123.Frame();
			Vector2 origin10 = new((float)value124.Width / 2f, 10f);
			Vector2 vector122 = new(0f, Projectile.gfxOffY);
			Vector2 spinningpoint = new(0f, -10f);
			float num189 = (float)Main.timeForVisualEffects / 60f;
			Vector2 vector123 = Projectile.Center + Projectile.velocity;
			Color color119 = Color.Blue * 0.2f;
			Color color120 = Color.White * 0.5f;
			color120.A = 0;
			float num190 = 0f;
			if (true)
			{
				color119 = Color.Brown * 0.2f;
				color120 = new Color(28, 17, 15) * 0.5f;
				color120.A = 50;
				num190 = -0.2f;
			}
			Color color121 = color119;
			color121.A = 0;
			Color color122 = color119;
			color122.A = 0;
			Color color123 = color119;
			color123.A = 0;
			Vector2 val10 = vector123 - Main.screenPosition + vector122;
			Vector2 spinningpoint17 = spinningpoint;
			double radians6 = (float)Math.PI * 2f * num189;
			Main.EntitySpriteDraw(value123, val10 + spinningpoint17.RotatedBy(radians6), value124, color121, Projectile.velocity.ToRotation() + (float)Math.PI / 2f, origin10, 1.5f + num190, (SpriteEffects)0);
			Vector2 val11 = vector123 - Main.screenPosition + vector122;
			Vector2 spinningpoint18 = spinningpoint;
			double radians7 = (float)Math.PI * 2f * num189 + (float)Math.PI * 2f / 3f;
			Main.EntitySpriteDraw(value123, val11 + spinningpoint18.RotatedBy(radians7), value124, color122, Projectile.velocity.ToRotation() + (float)Math.PI / 2f, origin10, 1.1f + num190, (SpriteEffects)0);
			Vector2 val12 = vector123 - Main.screenPosition + vector122;
			Vector2 spinningpoint19 = spinningpoint;
			double radians8 = (float)Math.PI * 2f * num189 + 4.1887903f;
			Main.EntitySpriteDraw(value123, val12 + spinningpoint19.RotatedBy(radians8), value124, color123, Projectile.velocity.ToRotation() + (float)Math.PI / 2f, origin10, 1.3f + num190, (SpriteEffects)0);
			Vector2 vector124 = Projectile.Center - Projectile.velocity * 0.5f;
			for (float num191 = 0f; num191 < 1f; num191 += 0.5f)
			{
				float num192 = num189 % 0.5f / 0.5f;
				num192 = (num192 + num191) % 1f;
				float num193 = num192 * 2f;
				if (num193 > 1f)
				{
					num193 = 2f - num193;
				}
				Main.EntitySpriteDraw(value123, vector124 - Main.screenPosition + vector122, value124, color120 * num193, Projectile.velocity.ToRotation() + (float)Math.PI / 2f, origin10, 0.3f + num192 * 0.5f, (SpriteEffects)0);
			}
			Main.EntitySpriteDraw(value122, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), rectangle25, alpha13, Projectile.rotation, origin33, Projectile.scale + 0.1f, dir);
			return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			Damage_CosmicBrownie_Starfall();
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info) {
			if (info.PvP) {
				Damage_CosmicBrownie_Starfall();
			}
		}

		public void Damage_CosmicBrownie_Starfall()
		{
			for (int num15 = 0; num15 < 3; num15++)
			{
				float x = Projectile.position.X + (float)Main.rand.Next(-400, 400);
				float y = Projectile.position.Y - (float)Main.rand.Next(500, 800);
				Vector2 vector = new(x, y);
				float num16 = Projectile.position.X + (float)(Projectile.width / 2) - vector.X;
				float num17 = Projectile.position.Y + (float)(Projectile.height / 2) - vector.Y;
				float num18 = (float)Math.Sqrt(num16 * num16 + num17 * num17);
				num18 = 23f / num18;
				num16 *= num18;
				num17 *= num18;
				int num19 = (int)(Projectile.damage / 2);
				if (Main.masterMode)
				{
					num19 *= 3;
				}
				else if (Main.expertMode)
				{
					num19 *= 2;
				}
				Projectile.NewProjectile(Projectile.GetSource_Death(), x, y, num16, num17, ModContent.ProjectileType<CosmicBrownie>(), num19, 5f, Projectile.owner, 0f, Projectile.position.Y);
			}
		}

		public override void OnKill(int timeLeft)
        {
			SoundEngine.PlaySound(in SoundID.Item10, Projectile.position);
			for (int num562 = 0; num562 < 7; num562++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Enchanted_Gold, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 150, default(Color), 0.8f);
			}
			for (float num564 = 0f; num564 < 1f; num564 += 0.125f)
			{
				Vector2 center25 = Projectile.Center;
				Vector2 unitY11 = Vector2.UnitY;
				double radians39 = num564 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f;
				Dust.NewDustPerfect(center25, DustID.FireworksRGB, unitY11.RotatedBy(radians39) * (4f + Main.rand.NextFloat() * 4f), 255, new Color(69, 40, 33)).noGravity = true;
			}
			for (float num565 = 0f; num565 < 1f; num565 += 0.25f)
			{
				Vector2 center26 = Projectile.Center;
				Vector2 unitY12 = Vector2.UnitY;
				double radians40 = num565 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f;
				Dust.NewDustPerfect(center26, DustID.FireworksRGB, unitY12.RotatedBy(radians40) * (2f + Main.rand.NextFloat() * 3f), 150, new(91, 209, 234)).noGravity = true;
			}
			Vector2 vector52 = new((float)Main.screenWidth, (float)Main.screenHeight);
			Rectangle val6 = Projectile.Hitbox;
			if (val6.Intersects(Utils.CenteredRectangle(Main.screenPosition + vector52 / 2f, vector52 + new Vector2(400f))))
			{
				for (int num566 = 0; num566 < 7; num566++)
				{
					Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, Main.rand.NextVector2CircularEdge(0.5f, 0.5f) * Projectile.velocity, Main.rand.NextBool(8) ? ModContent.GoreType<CosmicSacchariteStar>() : ModContent.GoreType<CosmicBrownieStar>());
				}
			}
		}

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0) * Projectile.Opacity;
        }
    }
}
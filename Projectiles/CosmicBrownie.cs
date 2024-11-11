using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using TheConfectionRebirth.Gores;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Projectiles
{
	public class CosmicBrownie : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = 5;
			Projectile.friendly = true;
			Projectile.penetrate = 2;
			Projectile.alpha = 50;
			Projectile.tileCollide = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		public override bool PreAI()
		{
			if (Projectile.Center.Y > Projectile.ai[1])
			{
				Projectile.tileCollide = true;
			}
			if (Projectile.soundDelay == 0)
			{
				Projectile.soundDelay = 20 + Main.rand.Next(40);
				SoundEngine.PlaySound(in SoundID.Item9, Projectile.position);
			}
			Projectile.alpha -= 15;
			int num899 = 100;
			if (Projectile.Center.Y >= Projectile.ai[1])
			{
				num899 = 0;
			}
			if (Projectile.alpha < num899)
			{
				Projectile.alpha = num899;
			}
			Projectile.localAI[0] += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.01f * (float)Projectile.direction;
			Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.01f * (float)Projectile.direction;
			Vector2 vector24 = new((float)Main.screenWidth, (float)Main.screenHeight);
			Rectangle hitbox = Projectile.Hitbox;
			if (hitbox.Intersects(Utils.CenteredRectangle(Main.screenPosition + vector24 / 2f, vector24 + new Vector2(400f))) && Main.rand.NextBool(6))
			{
				Gore.NewGore(Projectile.GetSource_FromAI(), Projectile.position, Projectile.velocity * 0.2f, ModContent.GoreType<CosmicBrownieStar>());
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
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects dir = (SpriteEffects)0;
			if (Projectile.spriteDirection == -1)
			{
				dir = (SpriteEffects)1;
			}
			Texture2D value128 = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle rectangle26 = new(0, 0, value128.Width, value128.Height);
			Vector2 origin13 = rectangle26.Size() / 2f;
			Color color125 = Projectile.GetAlpha(lightColor);
			Texture2D value129 = TextureAssets.Extra[91].Value;
			Rectangle value130 = value129.Frame();
			Vector2 origin14 = new((float)value130.Width / 2f, 10f);
			Vector2 vector130 = new(0f, Projectile.gfxOffY);
			Vector2 spinningpoint4 = new(0f, -5f);
			float num195 = (float)Main.timeForVisualEffects / 60f;
			Vector2 vector131 = Projectile.Center + Projectile.velocity;
			float num196 = 1.5f;
			float num197 = 1.1f;
			float num198 = 1.3f;
			Color color126 = Color.HotPink * 0.1f;
			Color color127 = Color.Brown * 0.3f;
			color127.A = 0;
			byte a = 0;
			float num199 = 1f;
			bool flag29 = true;
			float num200 = Projectile.scale + 0.1f;
			Color color129 = color126;
			Color color130 = color126;
			Color color131 = color126;
			if (flag29)
			{
				color129.A = a;
				color130.A = a;
				color131.A = a;
			}
			Vector2 val17 = vector131 - Main.screenPosition + vector130;
			Vector2 spinningpoint21 = spinningpoint4;
			double radians10 = (float)Math.PI * 2f * num195;
			Main.EntitySpriteDraw(value129, val17 + spinningpoint21.RotatedBy(radians10), value130, color129, Projectile.velocity.ToRotation() + (float)Math.PI / 2f, origin14, num196, (SpriteEffects)0);
			Vector2 val18 = vector131 - Main.screenPosition + vector130;
			Vector2 spinningpoint22 = spinningpoint4;
			double radians11 = (float)Math.PI * 2f * num195 + (float)Math.PI * 2f / 3f;
			Main.EntitySpriteDraw(value129, val18 + spinningpoint22.RotatedBy(radians11), value130, color130, Projectile.velocity.ToRotation() + (float)Math.PI / 2f, origin14, num197, (SpriteEffects)0);
			Vector2 val19 = vector131 - Main.screenPosition + vector130;
			Vector2 spinningpoint23 = spinningpoint4;
			double radians12 = (float)Math.PI * 2f * num195 + 4.1887903f;
			Main.EntitySpriteDraw(value129, val19 + spinningpoint23.RotatedBy(radians12), value130, color131, Projectile.velocity.ToRotation() + (float)Math.PI / 2f, origin14, num198, (SpriteEffects)0);
			Vector2 vector132 = Projectile.Center - Projectile.velocity * 0.5f;
			for (float num207 = 0f; num207 < 1f; num207 += 0.5f)
			{
				float num208 = num195 % 0.5f / 0.5f;
				num208 = (num208 + num207) % 1f;
				float num209 = num208 * 2f;
				if (num209 > 1f)
				{
					num209 = 2f - num209;
				}
				Main.EntitySpriteDraw(value129, vector132 - Main.screenPosition + vector130, value130, color127 * num209, Projectile.velocity.ToRotation() + (float)Math.PI / 2f, origin14, (0.5f + num208 * 0.5f) * num199, (SpriteEffects)0);
			}
			Main.EntitySpriteDraw(value128, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), rectangle26, color125, Projectile.rotation, origin13, num200, dir);
			return false;
		}

		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(in SoundID.Item10, Projectile.position);
			for (int num568 = 0; num568 < 10; num568++)
			{
				Dust dust304 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<NeapoliniteDust>(), Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 150, default, 1.2f);
				dust304.noGravity = true;
				dust304.velocity.X *= 2f;
			}
			for (int num569 = 0; num569 < 3; num569++)
			{
				Gore gore57 = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, new Vector2(Projectile.velocity.X * 0.05f, Projectile.velocity.Y * 0.05f), Main.rand.NextBool(3) ? ModContent.GoreType<CosmicSacchariteStar>() : ModContent.GoreType<CosmicBrownieStar>());
				Gore gore10 = gore57;
				Gore gore64 = gore10;
				gore64.velocity *= 2f;
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0) * Projectile.Opacity;
		}
	}
}

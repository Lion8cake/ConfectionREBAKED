using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using TheConfectionRebirth.Dusts;
using Terraria.Audio;
using System;
using Terraria.DataStructures;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using Stubble.Core.Classes;

namespace TheConfectionRebirth.Projectiles
{
	public class RockCandy : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 38;
			Projectile.height = 38;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.aiStyle = -1;
		}

		public override void PostDraw(Color lightColor)
		{
			Texture2D texture = ModContent.Request<Texture2D>(Texture + "_Extra").Value;
			int frame = 2;
			Rectangle frameRect = new Rectangle(0, texture.Height / 3 * frame, texture.Width, texture.Height / 3);
			float speed = Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y);
			float glowR = 1.45f;
			float glowG = 2.41f;
			float glowB = 2.47f;
			Color color = Color.Transparent;
			float speedAmount = 0.25f;
			if (speed < speedAmount)
			{

				color = new Color(
					MathHelper.Lerp(0, glowR / 1.25f, speedAmount - speed),
					MathHelper.Lerp(0, glowG / 1.25f, speedAmount - speed),
					MathHelper.Lerp(0, glowB / 1.25f, speedAmount - speed),
					speedAmount - speed);
			}
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, frameRect, color, Projectile.rotation, new Vector2(frameRect.Width / 2, frameRect.Height / 2), Projectile.scale, SpriteEffects.None);
			frame = 1;
			frameRect = new Rectangle(0, texture.Height / 3 * frame, texture.Width, texture.Height / 3);
			color = Color.Transparent;
			speedAmount = 1f;
			if (speed < speedAmount)
			{
				color = new Color(
					MathHelper.Lerp(0, glowR / 1.25f, speedAmount - speed),
					MathHelper.Lerp(0, glowG / 1.25f, speedAmount - speed),
					MathHelper.Lerp(0, glowB / 1.25f, speedAmount - speed),
					speedAmount - speed);
			}
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, frameRect, color, Projectile.rotation, new Vector2(frameRect.Width / 2, frameRect.Height / 2), Projectile.scale, SpriteEffects.None);
			frame = 0;
			frameRect = new Rectangle(0, texture.Height / 3 * frame, texture.Width, texture.Height / 3);
			color = Color.Transparent;
			speedAmount = 2f;
			if (speed < speedAmount)
			{
				color = new Color(
					MathHelper.Lerp(0, glowR / 1.25f, speedAmount - speed),
					MathHelper.Lerp(0, glowG / 1.25f, speedAmount - speed),
					MathHelper.Lerp(0, glowB / 1.25f, speedAmount - speed),
					speedAmount - speed);
			}
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, frameRect, color, Projectile.rotation, new Vector2(frameRect.Width / 2, frameRect.Height / 2), Projectile.scale, SpriteEffects.None);
		}

		public override void AI()
		{
			Projectile.rotation += (-Math.Abs(Projectile.velocity.X) - Math.Abs(Projectile.velocity.Y)) * 0.025f;
			Projectile.velocity *= 0.973f;

			if (Math.Abs(Projectile.velocity.X) < 0.05f && Math.Abs(Projectile.velocity.Y) < 0.05f)
			{
				Projectile.Kill();
				return;
			}
			Lighting.AddLight(Projectile.Center, 0.91f / 4, 2.09f / 4, 2.34f / 4);
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 4; i++)
			{
				Vector2 velocity = new Vector2(10f);
				float angle = MathHelper.PiOver2 * i;
				velocity = velocity.RotatedBy(angle + Projectile.rotation);
				Vector2 pos = Projectile.Center + velocity;
				Projectile.NewProjectile(Projectile.GetSource_Death(), pos, velocity, ModContent.ProjectileType<RockCandyShard>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
			}
			for (int i = 0; i < 10; i++)
			{
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<SacchariteDust>());
			}
			for (int i = 0; i < 15; i++)
			{
				int dustID = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<SacchariteDust>());
				Dust dust = Main.dust[dustID];
				dust.noGravity = false;
				dust.velocity = Vector2.Zero;
			}
			SoundEngine.PlaySound(new SoundStyle("TheConfectionRebirth/Sounds/Items/CrunchBallBreak"), Projectile.position);
		}
	}
}

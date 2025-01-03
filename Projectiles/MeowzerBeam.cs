using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TheConfectionRebirth.Projectiles
{
	public class MeowzerBeam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Bullet);
			AIType = ProjectileID.Bullet;
			Projectile.width = 30;
			Projectile.height = 42;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 600;
			Projectile.coldDamage = true;
			Projectile.friendly = false;
			Projectile.hostile = true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D beam = ModContent.Request<Texture2D>("TheConfectionRebirth/Projectiles/MeowzerBeam").Value;
			Texture2D tex = ModContent.Request<Texture2D>("TheConfectionRebirth/Projectiles/MeowzerBeam_Prime").Value;
			float val = (float)Projectile.ai[2] / 255;

			Vector2 drawOrigin = new Vector2(tex.Width / 4, tex.Height / 2);
			drawOrigin.X -= 15f;
			drawOrigin.Y -= 25f;
			Rectangle refRect = new Rectangle(tex.Width / 2, 0, tex.Width / 2, tex.Height);
			Rectangle rectangle = new Rectangle(refRect.X, refRect.Y, (tex.Width / 2), (tex.Height / Main.projFrames[Projectile.type]));
			Main.EntitySpriteDraw(beam, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, beam.Width / 2, beam.Height), LerpColor(Color.Transparent, lightColor, val), Projectile.rotation, drawOrigin + new Vector2(15, -5), Projectile.scale * 1.25f, SpriteEffects.None, 0);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(15f, Projectile.gfxOffY - 5);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(beam, drawPos, new Rectangle(beam.Width / 2, 0, beam.Width / 2, beam.Height), LerpColor(Color.Transparent, color, val * 0.5f), Projectile.rotation, drawOrigin + new Vector2(15, 0), Projectile.scale, SpriteEffects.None, 0);
				Main.EntitySpriteDraw(tex, drawPos, rectangle, color, Projectile.rotation, drawOrigin + new Vector2(15, -5), Projectile.scale, SpriteEffects.None, 0);
			}
			return false;
		}

		public static int LerpRound(float val1, float val2, float amount) 
		{ 
			return (int)Math.Round(MathHelper.Lerp(val1, val2, amount)); 
		}

		public static Color LerpColor(Color color1, Color color2, float lerp)
		{
			return new Color(LerpRound(color1.R, color2.R, lerp), LerpRound(color1.G, color2.G, lerp), LerpRound(color1.B, color2.B, lerp), LerpRound(color1.A, color2.A, lerp));
		}

		public override void OnSpawn(IEntitySource source)
		{
			SoundEngine.PlaySound(SoundID.Item67, Projectile.Center);
			if (Main.rand.NextBool(4))
				SoundEngine.PlaySound(SoundID.Item57, Projectile.Center);
		}

		public override void AI()
		{
			if (Projectile.timeLeft < 600 - 20)
			{
				float val = MathHelper.Lerp(0, 255, (float)Projectile.ai[2] / 255);
				Projectile.ai[2] += 5;
				if (Projectile.ai[2] > 255)
					Projectile.ai[2] = 255;
				Lighting.AddLight(Projectile.Center, new Vector3(val *= 0.015f));
			}
		}

		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.NPCDeath3, Projectile.Center);
			for (int i = 0; i < 14; i++)
			{
				Vector2 position = Projectile.Center + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / 14 * i));
				Dust dust = Dust.NewDustPerfect(position, 66);
				dust.noGravity = true;
				dust.velocity = Vector2.Normalize(dust.position - Projectile.Center) * 8.75f;
				dust.noLight = false;
				dust.fadeIn = 1f;
			}
		}
	}
}

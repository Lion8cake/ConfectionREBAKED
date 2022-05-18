using TheConfectionRebirth.Dusts;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace TheConfectionRebirth.Projectiles
{
	public class CosmicBlueTrail : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.width = 140;
			Projectile.height = 34;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.light = 0.5f;
		}

		public override void AI() {
		    Projectile.velocity.Y += Projectile.ai[0];
			Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0f ? 1 : -1;
			Projectile.rotation = Projectile.velocity.ToRotation();
			if (Projectile.spriteDirection == -1) {
				Projectile.rotation += MathHelper.Pi;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
			Projectile.ai[0] += 0.1f;
			Projectile.velocity *= 0.75f;
		}
		
		public override bool PreDraw(ref Color lightColor) {
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}

			return true;
		}
		
		//Make the projectile shoot out cookie star dusts
		/*public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 15; i++)
			{
				int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
				Dust obj = Main.dust[dustIndex];
				obj.velocity *= 1.4f;
			}
			for (int j = 0; j < 10; j++)
			{
				int dustIndex2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
				Main.dust[dustIndex2].noGravity = true;
				Dust obj2 = Main.dust[dustIndex2];
				obj2.velocity *= 5f;
				dustIndex2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
				Dust obj3 = Main.dust[dustIndex2];
				obj3.velocity *= 3f;
			}
		}*/ 
	}
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace TheConfectionRebirth.Projectiles
{
	public class ChocolateChip : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.CultistIsResistantTo[Type] = true;
			ProjectileID.Sets.TrailingMode[Type] = 2;
			ProjectileID.Sets.TrailCacheLength[Type] = 20;
		}

		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.aiStyle = - 1;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 120;
			Projectile.penetrate = -1;
			Projectile.scale = 1f + (float)Main.rand.Next(30) * 0.01f;
			Projectile.extraUpdates = 2;
			Projectile.DamageType = DamageClass.Ranged;
		}

		public override void AI()
		{
			//This uses AIStyle 41, 41 only consists of 4 lines so ill keep the initial frame code incase i want/need it
			/*if (Projectile.localAI[0] == 0f)
			{
				Projectile.localAI[0] = 1f;
				frame = Main.rand.Next(3);
			}*/
			Projectile.rotation += Projectile.velocity.X * 0.01f;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, 255, 255, 200);
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects dir = (SpriteEffects)0;
			if (Projectile.spriteDirection == -1)
			{
				dir = (SpriteEffects)1;
			}
			Vector2 pos = Projectile.position + new Vector2((float)Projectile.width, (float)Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Color color = Projectile.GetAlpha(lightColor);
			Vector2 origin = new Vector2((float)texture.Width, (float)texture.Height) / 2f;
			float alpha = 0.3f;
			Color color2 = new Color(255 - 80, 255 - 80, 255 - 80, 255) * alpha;
			float size = Projectile.scale * 1.025f;
			Vector2 rotPoint = new Vector2(2f * size, 0f);
			double rot = Projectile.rotation;
			Vector2 posRotation = Utils.RotatedBy(rotPoint, rot, default(Vector2));
			for (float offset = 0f; offset < 4f; offset += 1f)
			{
				Main.EntitySpriteDraw(texture, pos + -Projectile.velocity * offset * 1.25f, null, color2 * 0.7f, Projectile.rotation, origin, size, dir);
			}
			for (float offset = 0f; offset < 3f; offset += 1f)
			{
				double radians = offset * ((float)Math.PI / 2f);
				Main.EntitySpriteDraw(texture, pos + posRotation.RotatedBy(radians, default(Vector2)), null, color2 * 0.9f, Projectile.rotation, origin, size, dir);
			}
			Main.EntitySpriteDraw(texture, pos, null, color, Projectile.rotation, origin, Projectile.scale, dir);
			return false;
		}
	}
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Utilities;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria.Audio;

namespace TheConfectionRebirth.Projectiles
{
	public class BakersDozenSlash : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Type] = 20;
			ProjectileID.Sets.TrailingMode[Type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.aiStyle = 152;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
			Projectile.scale = 1f + (float)Main.rand.Next(30) * 0.01f;
			Projectile.extraUpdates = 2;
			Projectile.timeLeft = 10 * Projectile.MaxUpdates;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
		}

		public override bool PreAI()
		{
			float num = (float)Math.PI / 2f;
			Projectile.alpha -= 10;
			int num2 = 100;
			if (Projectile.alpha < num2)
			{
				Projectile.alpha = num2;
			}
			if (Projectile.ai[0] != 0f)
			{
				int num3 = 10 * Projectile.MaxUpdates;
				Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.ai[0] / (float)num3);
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + num;
			Projectile.tileCollide = false;
			return false;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, 255, 255, 200);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects dir = (SpriteEffects)0;
			if (Projectile.spriteDirection == -1)
			{
				dir = (SpriteEffects)1;
			}
			Projectile proj = Projectile;
			Texture2D value109 = TextureAssets.Projectile[proj.type].Value;
			int num144 = TextureAssets.Projectile[proj.type].Height() / Main.projFrames[proj.type];
			Rectangle rectangle23 = new(0, num144 * proj.frame, value109.Width, num144);
			Vector2 origin30 = rectangle23.Size() / 2f;
			Vector2 zero = Vector2.Zero;
			
			int num147 = 0;
			int num148 = -2;
			int num149 = 18;

			for (int num152 = num149; (num148 > 0 && num152 < num147) || (num148 < 0 && num152 > num147); num152 += num148)
			{
				if (num152 >= proj.oldPos.Length)
				{
					continue;
				}
				int colorType = (int)Projectile.ai[2];
				Color color = colorType switch
				{
					1 => new Color(118, 70, 50, 1),
					2 => new Color(188, 168, 120, 1),
					3 => new Color(61, 204, 144, 1),
					_ => new Color(225, 130, 227, 1)
				};

				float num157 = num147 - num152;
				if (num148 < 0)
				{
					num157 = num149 - num152;
				}
				color *= num157 / ((float)ProjectileID.Sets.TrailCacheLength[proj.type] * 1.5f);
				Vector2 vector117 = proj.oldPos[num152];
				float num158 = proj.rotation;
				SpriteEffects effects2 = dir;
				if (ProjectileID.Sets.TrailingMode[proj.type] == 2 || ProjectileID.Sets.TrailingMode[proj.type] == 3 || ProjectileID.Sets.TrailingMode[proj.type] == 4)
				{
					num158 = proj.oldRot[num152];
					effects2 = (SpriteEffects)(proj.oldSpriteDirection[num152] == -1 ? 1 : 0);
				}
				if (vector117 == Vector2.Zero)
				{
					continue;
				}
				
				Vector2 position27 = vector117 + zero + proj.Size / 2f - Main.screenPosition + new Vector2(0f, proj.gfxOffY);
				Main.EntitySpriteDraw(value109, position27, rectangle23, color, num158 + proj.rotation * 0f * (float)(num152 - 1) * (float)(-((Enum)dir).HasFlag((Enum)(object)(SpriteEffects)1).ToDirectionInt()), origin30, MathHelper.Lerp(proj.scale, 1.3f, (float)num152 / 15f), effects2);
			}
			return false;
		}
	}
}

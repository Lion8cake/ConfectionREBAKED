﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Buffs;

namespace TheConfectionRebirth.Projectiles
{
	public class GummyWormWhip : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			ProjectileID.Sets.IsAWhip[Type] = true;
			Main.projFrames[Type] = 5;
		}

		public override void SetDefaults()
		{
			Projectile.DefaultToWhip();

			Projectile.WhipSettings.Segments = 20;
			Projectile.WhipSettings.RangeMultiplier = 1.75f;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) 
		{
			target.AddBuff(ModContent.BuffType<GummyWormWhipTagDamage>(), 240);
			Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
		}

		private void DrawLine(List<Vector2> list) 
		{
			Texture2D texture = TextureAssets.FishingLine.Value;
			Rectangle frame = texture.Frame();
			Vector2 origin = new Vector2(frame.Width / 2, 2);

			Vector2 pos = list[0];
			for (int i = 0; i < list.Count - 1; i++) {
				Vector2 element = list[i];
				Vector2 diff = list[i + 1] - element;

				float rotation = diff.ToRotation() - MathHelper.PiOver2;

				Color color = Lighting.GetColor(element.ToTileCoordinates(), LineColor(Projectile.frame));
				Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

				Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

				pos += diff;
			}
		}

		public static Color LineColor(int frame)
		{
			if (frame == 0)
			{
				return Color.OrangeRed;
			}
			else if (frame == 1)
			{
				return Color.YellowGreen;
			}
			else if (frame == 2)
			{
				return Color.Teal;
			}
			else if (frame == 3)
			{
				return Color.BlueViolet;
			}
			else if (frame == 4)
			{
				return Color.DeepPink;
			}
			return Color.White;
		}

		public override bool PreDraw(ref Color lightColor) 
		{
			List<Vector2> list = new List<Vector2>();
			Projectile.FillWhipControlPoints(Projectile, list);

			DrawLine(list);

			SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			Main.instance.LoadProjectile(Type);
			Texture2D texture = TextureAssets.Projectile[Type].Value;

			Vector2 pos = list[0];

			for (int i = 0; i < list.Count - 1; i++) {
				Rectangle frame = new Rectangle(18 * Projectile.frame, 0, 18, 24);
				Vector2 origin = new Vector2(9, 8);
				float scale = 1;

				if (i == list.Count - 2) {
					frame.Y = 96;
					frame.Height = 24;

					Projectile.GetWhipSettings(Projectile, out float timeToFlyOut, out int _, out float _);
					float t = Projectile.ai[0] / timeToFlyOut;
					scale = MathHelper.Lerp(0.5f, 1.5f, Utils.GetLerpValue(0.1f, 0.7f, t, true) * Utils.GetLerpValue(0.9f, 0.7f, t, true));
				}
				else if (i > 10) {
					frame.Y = 72;
					frame.Height = 24;
				}
				else if (i > 5) {
					frame.Y = 48;
					frame.Height = 24;
				}
				else if (i > 0) {
					frame.Y = 24;
					frame.Height = 24;
				}

				Vector2 element = list[i];
				Vector2 diff = list[i + 1] - element;

				float rotation = diff.ToRotation() - MathHelper.PiOver2;
				Color color = Lighting.GetColor(element.ToTileCoordinates());

				Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, flip, 0);

				pos += diff;
			}
			return false;
		}
	}
}

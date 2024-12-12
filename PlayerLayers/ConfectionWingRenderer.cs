using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Items.Accessories;

namespace TheConfectionRebirth.PlayerLayers
{
	public class ConfectionWingRenderer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition()
		{
			return new BeforeParent(PlayerDrawLayers.Wings);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			if (drawInfo.drawPlayer.dead || drawInfo.hideEntirePlayer || drawPlayer.wings <= 0)
			{
				return;
			}
			Vector2 directions = drawPlayer.Directions;
			Vector2 vector = drawInfo.Position - Main.screenPosition + drawPlayer.Size / 2f;
			vector = drawInfo.Position - Main.screenPosition + new Vector2((float)(drawPlayer.width / 2), (float)(drawPlayer.height - drawPlayer.bodyFrame.Height / 2)) + new Vector2(0f, 7f);
			Main.instance.LoadWings(drawPlayer.wings);
			if (drawPlayer.wings == ModContent.GetInstance<WildAiryBlue>().Item.wingSlot)
			{
				if (!drawPlayer.ShouldDrawWingsThatAreAlwaysAnimated())
				{
					return;
				}
				DrawAiryBlueTrail(ref drawInfo, directions);
				if (Main.rand.NextBool(2))
				{
					int variant = Main.rand.Next(4);
					bool grav = directions.Y < 0;
					Vector2 pos = drawPlayer.position + new Vector2(-34 * drawPlayer.direction, grav ? -8 : 22);
					pos += variant switch
					{
						0 => new Vector2(0, grav ? 12 : 0),
						1 => new Vector2(0, grav ? 8 : 4),
						2 => new Vector2(0, grav ? 4 : 8),
						_ => new Vector2(0, grav ? 0 : 12)
					};
					Vector2 spawn = variant switch
					{
						0 => new Vector2(4, 8),
						1 => new Vector2(4, 8),
						2 => new Vector2(4, 8),
						_ => new Vector2(4, 14)
					};
					Color color = variant switch
					{
						0 => new Color(162, 119, 249),
						1 => new Color(157, 253, 186),
						2 => new Color(254, 249, 214),
						_ => new Color(254, 169, 231)
					};
					Dust dust = Dust.NewDustDirect(pos, (int)spawn.X, (int)spawn.Y, ModContent.DustType<WildAiryTintDust>());
					dust.color = color;
					dust.fadeIn = 1f;
					dust.shader = GameShaders.Armor.GetSecondaryShader(drawPlayer.cWings, drawPlayer);
					drawInfo.DustCache.Add(dust.dustIndex);
				}
				DrawData wing = new DrawData(TextureAssets.Wings[drawPlayer.wings].Value, (vector + new Vector2(-9, 2) * directions).Floor(), (Rectangle?)new Rectangle(0, TextureAssets.Wings[drawPlayer.wings].Height() / 4 * drawPlayer.wingFrame, TextureAssets.Wings[drawPlayer.wings].Width(), TextureAssets.Wings[drawPlayer.wings].Height() / 4), drawInfo.colorArmorBody, drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawPlayer.wings].Height() / 4 / 2)), 1f, drawInfo.playerEffect, 0f);
				wing.shader = drawInfo.cWings;
				drawInfo.DrawDataCache.Add(wing);
				Texture2D glow = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Items/Accessories/WildAiryBlue_Wings_Glow");
				DrawData wing2 = new DrawData(glow, (vector + new Vector2(-9, 2) * directions).Floor(), (Rectangle?)new Rectangle(0, TextureAssets.Wings[drawPlayer.wings].Height() / 4 * drawPlayer.wingFrame, TextureAssets.Wings[drawPlayer.wings].Width(), TextureAssets.Wings[drawPlayer.wings].Height() / 4), Color.White, drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawPlayer.wings].Height() / 4 / 2)), 1f, drawInfo.playerEffect, 0f);
				wing2.shader = drawInfo.cWings;
				drawInfo.DrawDataCache.Add(wing2);
				return;
			}
		}

		public static void DrawAiryBlueTrail(ref PlayerDrawSet drawinfo, Vector2 dirsVec)
		{
			if (drawinfo.shadow != 0f)
			{
				return;
			}
			int num = Math.Min(drawinfo.drawPlayer.availableAdvancedShadowsCount - 1, 30);
			float num2 = 0f;
			for (int num3 = num; num3 > 0; num3--)
			{
				EntityShadowInfo advancedShadow = drawinfo.drawPlayer.GetAdvancedShadow(num3);
				float num10 = num2;
				Vector2 position = drawinfo.drawPlayer.GetAdvancedShadow(num3 - 1).Position;
				num2 = num10 + Vector2.Distance(advancedShadow.Position, position);
			}
			float num4 = MathHelper.Clamp(num2 / 160f, 0f, 1f);
			Texture2D value = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Items/Accessories/WildAiryBlue_Trail");
			float x = 1.7f;
			Vector2 origin = new((float)(value.Width / 2), (float)(value.Height / 2));
			Vector2 val = new Vector2((float)drawinfo.drawPlayer.width, (float)drawinfo.drawPlayer.height) / 2f;
			Color white = Color.White;
			white.A = 64;
			Vector2 vector2 = val;
			vector2 = drawinfo.drawPlayer.DefaultSize * new Vector2(0.5f, 1f) + new Vector2(-34f * drawinfo.drawPlayer.direction, -4f);
			if (dirsVec.Y < 0f)
			{
				vector2 = drawinfo.drawPlayer.DefaultSize * new Vector2(0.5f, 0f) + new Vector2(-34f * drawinfo.drawPlayer.direction, 0f);
			}
			Vector2 scale = default(Vector2);
			for (int num5 = num; num5 > 0; num5--)
			{
				EntityShadowInfo advancedShadow2 = drawinfo.drawPlayer.GetAdvancedShadow(num5);
				EntityShadowInfo advancedShadow3 = drawinfo.drawPlayer.GetAdvancedShadow(num5 - 1);
				Vector2 pos = advancedShadow2.Position + vector2 + advancedShadow2.HeadgearOffset;
				Vector2 pos2 = advancedShadow3.Position + vector2 + advancedShadow3.HeadgearOffset;
				pos = drawinfo.drawPlayer.RotatedRelativePoint(pos, reverseRotation: true, addGfxOffY: false);
				pos2 = drawinfo.drawPlayer.RotatedRelativePoint(pos2, reverseRotation: true, addGfxOffY: false);
				float num6 = (pos2 - pos).ToRotation() - (float)Math.PI / 2f;
				num6 = (float)Math.PI / 2f * (float)drawinfo.drawPlayer.direction;
				float num7 = Math.Abs(pos2.X - pos.X);
				scale = new(x, num7 / (float)value.Height);
				scale.X *= 0.75f;
				float num8 = 1f - (float)num5 / (float)num;
				num8 *= num8;
				num8 *= Utils.GetLerpValue(0f, 4f, num7, clamped: true);
				num8 *= 0.5f;
				num8 *= num8;
				Color color = white * num8 * num4;
				SpriteEffects effects = dirsVec.Y < 0f ? dirsVec.X > 0f ? SpriteEffects.FlipHorizontally : SpriteEffects.None : drawinfo.playerEffect;
				if (!(color == Color.Transparent))
				{
					DrawData item = new DrawData(value, pos - Main.screenPosition, null, color, num6, origin, scale, effects);
					item.shader = drawinfo.cWings;
					drawinfo.DrawDataCache.Add(item);
					for (float num9 = 0.25f; num9 < 1f; num9 += 0.25f)
					{
						item = new DrawData(value, Vector2.Lerp(pos, pos2, num9) - Main.screenPosition, null, color, num6, origin, scale, effects);
						item.shader = drawinfo.cWings;
						drawinfo.DrawDataCache.Add(item);
					}
				}
			}
		}
	}
}

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria;
using TheConfectionRebirth.Mounts;
using Terraria.Utilities;

namespace TheConfectionRebirth.PlayerLayers
{
	public class RollerCycleTrialRendering : PlayerDrawLayer
	{
		public override Position GetDefaultPosition()
		{
			return new BeforeParent(PlayerDrawLayers.MountBack);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			drawPlayer.GetModPlayer<ConfectionPlayer>().rollerCycleTimer++;
			if (drawPlayer.GetModPlayer<ConfectionPlayer>().rollerCycleTimer >= 40)
			{
				drawPlayer.GetModPlayer<ConfectionPlayer>().rollerCycleTimer = 0;
			}
			int frameCount = drawPlayer.GetModPlayer<ConfectionPlayer>().rollerCycleTimer;
			float speed = Math.Abs(drawPlayer.velocity.X);
			if (drawPlayer.mount.Type == ModContent.GetInstance<Rollercycle>().Type && speed > 10f)
			{
				if (drawInfo.shadow != 0f)
				{
					return;
				}
				int num = (int)Math.Min(drawPlayer.availableAdvancedShadowsCount - 1, 30);
				float num2 = 0f;
				for (int num3 = num; num3 > 0; num3--)
				{
					EntityShadowInfo advancedShadow = drawPlayer.GetAdvancedShadow(num3);
					float num10 = num2;
					Vector2 position = drawPlayer.GetAdvancedShadow(num3 - 1).Position;
					num2 = num10 + Vector2.Distance(advancedShadow.Position, position);
				}
				float num4 = MathHelper.Clamp(num2 / 160f, 0f, 1f);
				Main.instance.LoadProjectile(250);
				Texture2D value = TextureAssets.Projectile[250].Value;
				string confectionierTrailPath = "TheConfectionRebirth/Mounts/RollerCycleTrails/RollerCycleTrail";
				int frames = 1;
				Color white = Color.White;
				white.A = 64;
				bool spriteIsLarge = false;
				if (drawPlayer.name.Contains("Lion8cake"))
				{
					value = ModContent.Request<Texture2D>(confectionierTrailPath + "_Lion8cake").Value;
					spriteIsLarge = true;
				}
				if (drawPlayer.name == "Darkrious")
				{
					value = ModContent.Request<Texture2D>(confectionierTrailPath + "_Darkrious").Value;
				}
				else if (drawPlayer.name == "Snacks")
				{
					value = ModContent.Request<Texture2D>(confectionierTrailPath + "_Snacks").Value;
				}
				else if (drawPlayer.name == "Larfleeze")
				{
					value = ModContent.Request<Texture2D>(confectionierTrailPath + "_Larfleeze").Value;
					frames = 4;
					white = Color.White;
				}
				else if (drawPlayer.name == "BasicallyIamCat")
				{
					value = ModContent.Request<Texture2D>(confectionierTrailPath + "_FoxXD").Value;
					spriteIsLarge = true;
				}
				else if (drawPlayer.name == "neobind")
				{
					value = ModContent.Request<Texture2D>(confectionierTrailPath + "_NeoBind").Value;
					spriteIsLarge = true;
				}
				float x = 1.7f;
				int currentFrame = (int)(frameCount * 0.1);
				Rectangle? framing = new Rectangle(0, (value.Height / frames) * currentFrame, value.Width, value.Height / frames);
				Vector2 origin = new((float)(value.Width / 2), (float)(value.Height / frames / 2));
				Vector2 val = new Vector2(drawPlayer.width, drawPlayer.height) / 2f;
				Vector2 vector2 = val;
				vector2 = drawInfo.drawPlayer.DefaultSize * new Vector2(0.5f, 1f) + new Vector2(-13f * drawPlayer.direction, 8f);
				if (drawPlayer.Directions.Y < 0f)
				{
					vector2 = drawPlayer.DefaultSize * new Vector2(0.5f, 0f) + new Vector2(-13f * drawPlayer.direction, -8f);
				}
				int additionalShowPos = 0;
				if (frames == 4)
				{
					vector2 = new Vector2(8f, drawPlayer.Directions.Y * 8f);
					additionalShowPos = 1;
				}
				for (int num5 = num; num5 > 0; num5--)
				{
					EntityShadowInfo advancedShadow2 = drawPlayer.GetAdvancedShadow(num5 - additionalShowPos);
					EntityShadowInfo advancedShadow3 = drawPlayer.GetAdvancedShadow(num5 - 1 - additionalShowPos);
					Vector2 pos = advancedShadow2.Position + vector2 + advancedShadow2.HeadgearOffset;
					Vector2 pos2 = advancedShadow3.Position + vector2 + advancedShadow3.HeadgearOffset;
					pos = drawPlayer.RotatedRelativePoint(pos, reverseRotation: true, addGfxOffY: false);
					pos2 = drawPlayer.RotatedRelativePoint(pos2, reverseRotation: true, addGfxOffY: false);
					float num6 = (pos2 - pos).ToRotation() - (float)Math.PI / 2f;
					num6 = (float)Math.PI / 2f * (float)drawPlayer.direction;
					float num7 = Math.Abs(pos2.X - pos.X);
					Vector2 scale = new(x, num7 / (float)value.Height / frames);
					if (spriteIsLarge)
					{
						scale.X *= 0.75f;
					}
					float num8 = 1f - (float)num5 / (float)num;
					num8 *= num8;
					num8 *= Utils.GetLerpValue(0f, 4f, num7, clamped: true);
					if (frames == 4)
					{
						if (num5 % 2 == 0)
						{
							continue;
						}
						scale = new(2, 2);
					}
					else
					{
						num8 *= 0.5f;
					}
					num8 *= num8;
					float speedColor = (speed - 10);
					if (speedColor > 2)
						speedColor = 2f;
					speedColor /= 2;
					Color color = white * num8 * num4 * speedColor;
					if (!(color == Color.Transparent))
					{
						DrawData item = new DrawData(value, pos - Main.screenPosition, framing, color, num6, origin, scale, drawInfo.playerEffect);
						item.shader = drawPlayer.cMount;
						drawInfo.DrawDataCache.Add(item);
						for (float num9 = 0.25f; num9 < 1f; num9 += 0.25f)
						{
							if (frames == 4)
							{
								break;
							}
							item = new DrawData(value, Vector2.Lerp(pos, pos2, num9) - Main.screenPosition, framing, color, num6, origin, scale, drawInfo.playerEffect);
							item.shader = drawPlayer.cMount;
							drawInfo.DrawDataCache.Add(item);
						}
					}
				}
			}
		}
	}
}

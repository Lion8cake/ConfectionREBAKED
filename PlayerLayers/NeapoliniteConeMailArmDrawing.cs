using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace TheConfectionRebirth.PlayerLayers
{
	public class NeapoliniteConeMailArmDrawing : PlayerDrawLayer
	{
		public override Position GetDefaultPosition()
		{
			return new AfterParent(PlayerDrawLayers.ArmOverItem);
		}

		protected override void Draw(ref PlayerDrawSet drawinfo)
		{
			int HelmetType = ConfectionPlayer.NeapoliniteHelmetNumber(drawinfo);
			if (drawinfo.usesCompositeTorso)
			{
				DrawArmComposite(ref drawinfo);
			}
			else if (drawinfo.drawPlayer.body > 0 && HelmetType > 0) //Old hand rendering, again here for when other broken mods use the old renderer
			{
				Texture2D arms = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Items/Armor/NeapoliniteSet/NeapoliniteConeMail_Body_" + HelmetType);
				Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
				int num = drawinfo.armorAdjust;
				bodyFrame.X += num;
				bodyFrame.Width -= num;
				if (drawinfo.drawPlayer.direction == -1)
				{
					num = 0;
				}
				if (drawinfo.drawPlayer.invis && (drawinfo.drawPlayer.body == 21 || drawinfo.drawPlayer.body == 22))
				{
					return;
				}
				DrawData item;
				item = new DrawData(arms, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cBody;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		public static void DrawArmComposite(ref PlayerDrawSet drawinfo)
		{
			int HelmetType = ConfectionPlayer.NeapoliniteHelmetNumber(drawinfo);
			Vector2 vector = new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2));
			Vector2 vector2 = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
			vector2.Y -= 2f;
			vector += vector2 * (float)(-((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)2).ToDirectionInt());
			float bodyRotation = drawinfo.drawPlayer.bodyRotation;
			float rotation = drawinfo.drawPlayer.bodyRotation + drawinfo.compositeFrontArmRotation;
			Vector2 bodyVect = drawinfo.bodyVect;
			Vector2 compositeOffset_FrontArm = GetCompositeOffset_FrontArm(ref drawinfo);
			bodyVect += compositeOffset_FrontArm;
			vector += compositeOffset_FrontArm;
			Vector2 position = vector + drawinfo.frontShoulderOffset;
			if (drawinfo.compFrontArmFrame.X / drawinfo.compFrontArmFrame.Width >= 7)
			{
				vector += new Vector2((float)((!((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)1)) ? 1 : (-1)), (float)((!((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)2)) ? 1 : (-1)));
			}
			_ = drawinfo.drawPlayer.invis;
			int num2 = (drawinfo.compShoulderOverFrontArm ? 1 : 0);
			int num3 = ((!drawinfo.compShoulderOverFrontArm) ? 1 : 0);
			int num4 = ((!drawinfo.compShoulderOverFrontArm) ? 1 : 0);
			bool flag = !drawinfo.hidesTopSkin;
			if (drawinfo.drawPlayer.body > 0 && HelmetType > 0)
			{
				if (!drawinfo.drawPlayer.invis)
				{
					Texture2D value = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Items/Armor/NeapoliniteSet/NeapoliniteConeMail_Body_" + HelmetType);
					for (int i = 0; i < 2; i++)
					{
						if (i == num2 && !drawinfo.hideCompositeShoulders)
						{
							PlayerDrawLayers.DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.FrontShoulder, new DrawData(value, position, drawinfo.compFrontShoulderFrame, drawinfo.colorArmorBody, bodyRotation, bodyVect, 1f, drawinfo.playerEffect)
							{
								shader = drawinfo.cBody
							});
						}
						if (i == num3)
						{
							PlayerDrawLayers.DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.FrontArm, new DrawData(value, vector, drawinfo.compFrontArmFrame, drawinfo.colorArmorBody, rotation, bodyVect, 1f, drawinfo.playerEffect)
							{
								shader = drawinfo.cBody
							});
						}
					}
				}
			}
			if (drawinfo.drawPlayer.handon > 0)
			{
				Texture2D value2 = TextureAssets.AccHandsOnComposite[drawinfo.drawPlayer.handon].Value;
				PlayerDrawLayers.DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.FrontArmAccessory, new DrawData(value2, vector, drawinfo.compFrontArmFrame, drawinfo.colorArmorBody, rotation, bodyVect, 1f, drawinfo.playerEffect)
				{
					shader = drawinfo.cHandOn
				});
			}
		}

		private static Vector2 GetCompositeOffset_FrontArm(ref PlayerDrawSet drawinfo)
		{
			return new Vector2((float)(-5 * ((!((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)1)) ? 1 : (-1))), 0f);
		}
	}
}

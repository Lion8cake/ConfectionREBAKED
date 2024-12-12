using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace TheConfectionRebirth.PlayerLayers
{
	public class NeapoliniteConeMailDrawing : PlayerDrawLayer
	{
		public override Position GetDefaultPosition()
		{
			return new AfterParent(PlayerDrawLayers.Torso);
		}

		protected override void Draw(ref PlayerDrawSet drawinfo)
		{
			int HelmetType = ConfectionPlayer.NeapoliniteHelmetNumber(drawinfo);
			if (drawinfo.usesCompositeTorso)
			{
				DrawComposite(ref drawinfo);
			}
			else if (drawinfo.drawPlayer.body > 0 && HelmetType > 0) //Old chestplate renderer, here incase some weird fucked up mod wants to use the old renderer
			{
				Texture2D male = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Items/Armor/NeapoliniteSet/NeapoliniteConeMail_Body_" + HelmetType);
				Texture2D female = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Items/Armor/NeapoliniteSet/NeapoliniteConeMail_Body_" + HelmetType);
				Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
				int num = drawinfo.armorAdjust;
				bodyFrame.X += num;
				bodyFrame.Width -= num;
				if (drawinfo.drawPlayer.direction == -1)
				{
					num = 0;
				}
				if (!drawinfo.drawPlayer.invis || (drawinfo.drawPlayer.body != 21 && drawinfo.drawPlayer.body != 22))
				{
					Texture2D texture = (drawinfo.drawPlayer.Male ? male : female);
					DrawData item2 = new DrawData(texture, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
					item2.shader = drawinfo.cBody;
					drawinfo.DrawDataCache.Add(item2);
				}
			}
		}

		public static void DrawComposite(ref PlayerDrawSet drawinfo)
		{
			int HelmetType = ConfectionPlayer.NeapoliniteHelmetNumber(drawinfo);
			Vector2 vector = new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2));
			Vector2 vector2 = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
			vector2.Y -= 2f;
			vector += vector2 * (float)(-((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)2).ToDirectionInt());
			float bodyRotation = drawinfo.drawPlayer.bodyRotation;
			Vector2 val = vector;
			Vector2 bodyVect = drawinfo.bodyVect;
			Vector2 compositeOffset_BackArm = GetCompositeOffset_BackArm(ref drawinfo);
			_ = val + compositeOffset_BackArm;
			bodyVect += compositeOffset_BackArm;
			if (drawinfo.drawPlayer.body > 0 && HelmetType > 0)
			{
				if (!drawinfo.drawPlayer.invis)
				{
					Texture2D value = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Items/Armor/NeapoliniteSet/NeapoliniteConeMail_Body_" + HelmetType);
					PlayerDrawLayers.DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.Torso, new DrawData(value, vector, drawinfo.compTorsoFrame, drawinfo.colorArmorBody, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect)
					{
						shader = drawinfo.cBody
					});
				}
			}
			if (drawinfo.drawFloatingTube)
			{
				drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Extra[105].Value, vector, (Rectangle?)new Rectangle(0, 56, 40, 56), drawinfo.floatingTubeColor, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f)
				{
					shader = drawinfo.cFloatingTube
				});
			}
		}

		private static Vector2 GetCompositeOffset_BackArm(ref PlayerDrawSet drawinfo)
		{
			return new Vector2((float)(6 * ((!((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)1)) ? 1 : (-1))), (float)(2 * ((!((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)2)) ? 1 : (-1))));
		}
	}
}

using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace TheConfectionRebirth.PlayerLayers
{
	public class SnickersDevsetUnicookie : PlayerDrawLayer
	{
		public override Position GetDefaultPosition()
		{
			return new AfterParent(PlayerDrawLayers.Leggings);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			if (!drawPlayer.dead && ((drawPlayer.armor[12].type == ItemID.None && drawPlayer.armor[2].type == ModContent.ItemType<Items.Armor.SnickerDevOutfit.Unicookie>()) || drawPlayer.armor[12].type == ModContent.ItemType<Items.Armor.SnickerDevOutfit.Unicookie>()))
			{
				drawPlayer.GetModPlayer<ConfectionPlayer>().snickerDevCookieRot += (float)(drawPlayer.legFrame.Y > drawPlayer.legFrame.Height * 5 && !Main.gamePaused ? (Main.gameMenu ? 4f : drawPlayer.velocity.X) : 0f) * 0.075f;

				Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Items/Armor/SnickerDevOutfit/Unicookie_Wheel");
				Vector2 val = drawInfo.Position + drawInfo.drawPlayer.Size * new Vector2(0.5f, 0.5f + 0.5f * drawInfo.drawPlayer.gravDir);
				Vector2 position = val - Main.screenPosition + drawInfo.drawPlayer.legPosition + new Vector2(drawPlayer.direction, drawPlayer.gravDir == 1f ? -2 : -4);
				if (drawInfo.isSitting)
				{
					position.Y += drawInfo.seatYOffset;
				}
				position += drawInfo.legsOffset;
				position = position.Floor();
				Rectangle frame = new Rectangle(0, 0, texture.Width, texture.Height);
				Color color = drawInfo.colorArmorLegs;
				float rotation = drawPlayer.GetModPlayer<ConfectionPlayer>().snickerDevCookieRot;
				Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
				SpriteEffects spriteEffects = drawInfo.playerEffect;

				DrawData drawData = new DrawData(texture, position, frame, color, rotation, origin, 1f, spriteEffects);
				drawData.shader = drawInfo.cLegs;
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}
}

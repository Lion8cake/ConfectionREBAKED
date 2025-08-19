using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace TheConfectionRebirth.PlayerLayers
{
	public class TopCakeCandlesDrawing : PlayerDrawLayer
	{
		public override Position GetDefaultPosition()
		{
			return new AfterParent(PlayerDrawLayers.Head);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			if (!drawPlayer.dead && ((drawPlayer.armor[10].type == ItemID.None && drawPlayer.armor[0].type == ModContent.ItemType<Items.Armor.BirthdayOutfit.TopCake>()) || drawPlayer.armor[10].type == ModContent.ItemType<Items.Armor.BirthdayOutfit.TopCake>()))
			{
				Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Items/Armor/BirthdayOutfit/TopCake_Candles");
				Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Items/Armor/BirthdayOutfit/TopCake_Candles_Glow");

				float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2;
				float drawY = (int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 - 5.5f;
				Vector2 origin = drawInfo.headVect;
				Vector2 position = new Vector2(drawX, drawY) + drawPlayer.headPosition - Main.screenPosition;
				Rectangle frame = drawPlayer.bodyFrame;
				float rotation = drawPlayer.headRotation;
				SpriteEffects spriteEffects = drawInfo.playerEffect;

				DrawData drawData = new DrawData(texture, position, frame, drawInfo.colorArmorHead, rotation, origin, 1f, spriteEffects);
				drawData.shader = drawInfo.cHead;
				drawInfo.DrawDataCache.Add(drawData);

				DrawData drawData2 = new DrawData(texture2, position, frame, Color.White, rotation, origin, 1f, spriteEffects);
				drawData2.shader = drawInfo.cHead;
				drawInfo.DrawDataCache.Add(drawData2);

				if (Main.rand.NextBool(40))
				{
					Rectangle spawnPos = Utils.CenteredRectangle(drawInfo.Position + drawPlayer.Size / 2f + new Vector2(0f, drawPlayer.gravDir * -28f), new Vector2(14f, 4f));
					int dustID = Dust.NewDust(spawnPos.TopLeft(), spawnPos.Width, spawnPos.Height, DustID.SpelunkerGlowstickSparkle, 0f);
					Dust dust = Main.dust[dustID];
					dust.fadeIn = 1f;
					dust.velocity.Y = -2f;
					dust.shader = GameShaders.Armor.GetSecondaryShader(drawPlayer.cHead, drawPlayer);
					drawInfo.DustCache.Add(dustID);
				}
			}
		}
	}
}

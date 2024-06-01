using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using static Terraria.WaterfallManager;

namespace TheConfectionRebirth.Biomes
{
    public class ChocolateRainWaterfallStyle : ConfectionModWaterfallStyle
    {
		public override bool PreDraw(int i, int x, int y, SpriteBatch spriteBatch) {
			int waterfallDist = (int)typeof(WaterfallManager).GetField("waterfallDist", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.waterfallManager);
			int rainFrameForeground = (int)typeof(WaterfallManager).GetField("rainFrameForeground", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.waterfallManager);
			int rainFrameBackground = (int)typeof(WaterfallManager).GetField("rainFrameBackground", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.waterfallManager);
			WaterfallData[] waterfalls = (WaterfallData[])typeof(WaterfallManager).GetField("waterfalls", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.waterfallManager);
			int num15;
			if (Main.drewLava) {
				return false;
			}
			num15 = waterfallDist / 4;
			if (waterfalls[i].stopAtStep > num15) {
				waterfalls[i].stopAtStep = num15;
			}
			if (waterfalls[i].stopAtStep == 0 || (float)(y + num15) < Main.screenPosition.Y / 16f || (float)x < Main.screenPosition.X / 16f - 20f || (float)x > (Main.screenPosition.X + (float)Main.screenWidth) / 16f + 20f) {
				return false;
			}
			int num16;
			int num17;
			if (x % 2 == 0) {
				num16 = rainFrameForeground + 3;
				if (num16 > 7) {
					num16 -= 8;
				}
				num17 = rainFrameBackground + 2;
				if (num17 > 7) {
					num17 -= 8;
				}
			}
			else {
				num16 = rainFrameForeground;
				num17 = rainFrameBackground;
			}
			Rectangle value = new(num17 * 18, 0, 16, 16);
			Rectangle value2 = new(num16 * 18, 0, 16, 16);
			Vector2 origin = new(8f, 8f);
			Vector2 position = ((y % 2 != 0) ? (new Vector2((float)(x * 16 + 8), (float)(y * 16 + 8)) - Main.screenPosition) : (new Vector2((float)(x * 16 + 9), (float)(y * 16 + 8)) - Main.screenPosition));
			Tile tile = Main.tile[x, y - 1];
			if (tile.HasTile && tile.BottomSlope) {
				position.Y -= 16f;
			}
			bool flag = false;
			float rotation = 0f;
			for (int j = 0; j < num15; j++) {
				Color color6 = Lighting.GetColor(x, y);
				float num18 = 0.6f;
				float num19 = 0.3f;
				if (j > num15 - 8) {
					float num20 = (float)(num15 - j) / 8f;
					num18 *= num20;
					num19 *= num20;
				}
				Color color2 = color6 * num18;
				Color color3 = color6 * num19;
				Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture + "_Back").Value, position, (Rectangle?)value, color3, rotation, origin, 1f, (SpriteEffects)0, 0f);
				Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, position, (Rectangle?)value2, color2, rotation, origin, 1f, (SpriteEffects)0, 0f);
				if (flag) {
					break;
				}
				y++;
				Tile tile2 = Main.tile[x, y];
				if (WorldGen.SolidTile(tile2)) {
					flag = true;
				}
				if (tile2.LiquidAmount > 0) {
					int num21 = (int)(16f * ((float)(int)tile2.LiquidAmount / 255f)) & 0xFE;
					if (num21 >= 15) {
						break;
					}
					value2.Height -= num21;
					value.Height -= num21;
				}
				if (y % 2 == 0) {
					position.X += 1f;
				}
				else {
					position.X -= 1f;
				}
				position.Y += 16f;
			}
			waterfalls[i].stopAtStep = 0;
			return false;
		}
	}
}

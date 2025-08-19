using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ModLoader;
using static Terraria.WaterfallManager;

namespace TheConfectionRebirth.Biomes
{
    public class CreamSnowWaterfallStyle : ConfectionModWaterfallStyle
    {
		public override bool PreDraw(int currentWaterfallData, int i, int j, SpriteBatch spriteBatch) {
			int waterfallDist = (int)typeof(WaterfallManager).GetField("waterfallDist", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.waterfallManager);
			int rainFrameForeground = (int)typeof(WaterfallManager).GetField("rainFrameForeground", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.waterfallManager);
			int rainFrameBackground = (int)typeof(WaterfallManager).GetField("rainFrameBackground", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.waterfallManager);
			int snowFrameForeground = (int)typeof(WaterfallManager).GetField("snowFrameForeground", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.waterfallManager);
			WaterfallData[] waterfalls = (WaterfallData[])typeof(WaterfallManager).GetField("waterfalls", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.waterfallManager);
			int num15;
			if (Main.drewLava) {
				return false;
			}
			num15 = waterfallDist / 2;
			if (waterfalls[currentWaterfallData].stopAtStep > num15) {
				waterfalls[currentWaterfallData].stopAtStep = num15;
			}
			if (waterfalls[currentWaterfallData].stopAtStep == 0 || (float)(j + num15) < Main.screenPosition.Y / 16f || (float)i < Main.screenPosition.X / 16f - 20f || (float)i > (Main.screenPosition.X + (float)Main.screenWidth) / 16f + 20f) {
				return false;
			}
			int num16;
			int num17;
			if (i % 2 == 0) {
				num16 = rainFrameForeground + 3;
				if (num16 > 7) {
					num16 -= 8;
				}
				num17 = rainFrameBackground + 2;
				if (num17 > 7) {
					num17 -= 8;
				}
				num16 = snowFrameForeground + 3;
				if (num16 > 7) {
					num16 -= 8;
				}
			}
			else {
				num16 = rainFrameForeground;
				num17 = rainFrameBackground;
				num16 = snowFrameForeground;
			}
			Rectangle value = new (num17 * 18, 0, 16, 16);
			Rectangle value2 = new(num16 * 18, 0, 16, 16);
			Vector2 origin = new(8f, 8f);
			Vector2 position = ((j % 2 != 0) ? (new Vector2((float)(i * 16 + 8), (float)(j * 16 + 8)) - Main.screenPosition) : (new Vector2((float)(i * 16 + 9), (float)(j * 16 + 8)) - Main.screenPosition));
			Tile tile = Main.tile[i, j - 1];
			if (tile.HasTile && tile.BottomSlope) {
				position.Y -= 16f;
			}
			bool flag = false;
			float rotation = 0f;
			for (int j2 = 0; j2 < num15; j2++) {
				Color color6 = Lighting.GetColor(i, j);
				float num18 = 0.6f;
				float num19 = 0.3f;
				if (j2 > num15 - 8) {
					float num20 = (float)(num15 - j2) / 8f;
					num18 *= num20;
					num19 *= num20;
				}
				Color color2 = color6 * num18;
				Color color3 = color6 * num19;
				spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, position, (Rectangle?)value2, color2, 0f, origin, 1f, (SpriteEffects)0, 0f);
				if (flag) {
					break;
				}
				j++;
				Tile tile2 = Main.tile[i, j];
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
				if (j % 2 == 0) {
					position.X += 1f;
				}
				else {
					position.X -= 1f;
				}
				position.Y += 16f;
			}
			waterfalls[currentWaterfallData].stopAtStep = 0;
			return false;
		}
	}
}

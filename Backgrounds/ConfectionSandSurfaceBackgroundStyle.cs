using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Reflection;
using System;
using Terraria.Graphics.Effects;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent;

namespace TheConfectionRebirth.Backgrounds
{
    public class ConfectionSandSurfaceBackgroundStyle : ModSurfaceBackgroundStyle
    {
		public override void ModifyFarFades(float[] fades, float transitionSpeed) {
			for (int i = 0; i < fades.Length; i++) {
				if (i == Slot) {
					fades[i] += transitionSpeed;
					if (fades[i] > 1f) {
						fades[i] = 1f;
					}
				}
				else {
					fades[i] -= transitionSpeed;
					if (fades[i] < 0f) {
						fades[i] = 0f;
					}
				}
			}
		}

		public override int ChooseFarTexture() {
			return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSandSurfaceFar");
		}

		public override int ChooseMiddleTexture() {
			return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSandSurfaceMid");
		}

		public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b) {
			return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSandSurfaceClose");
		}

		public override bool PreDrawCloseBackground(SpriteBatch spriteBatch)
		{
			float bgScale = (float)typeof(Main).GetField("bgScale", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
			float screenOff = (float)typeof(Main).GetField("screenOff", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance);
			double bgParallax = (double)typeof(Main).GetField("bgParallax", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance);
			int bgTopY = (int)typeof(Main).GetField("bgTopY", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance);
			float scAdj = (float)typeof(Main).GetField("scAdj", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance);
			int bgWidthScaled = (int)typeof(Main).GetField("bgWidthScaled", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
			int bgStartX = (int)typeof(Main).GetField("bgStartX", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance);
			int bgLoops = (int)typeof(Main).GetField("bgLoops", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance);
			Color ColorOfSurfaceBackgroundsModified = (Color)typeof(Main).GetField("ColorOfSurfaceBackgroundsModified", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);

			string TexturePath = "TheConfectionRebirth/Backgrounds/ConfectionSandSurfaceClose1";
			string TexturePath2 = "TheConfectionRebirth/Backgrounds/ConfectionSandSurfaceClose2";
			string TexturePath3 = null;
			bool flag = false;
			if ((!Main.remixWorld || (Main.gameMenu && !WorldGen.remixWorldGen)) && (!WorldGen.remixWorldGen || !WorldGen.drunkWorldGen))
			{
				flag = true;
			}
			if (Main.mapFullscreen)
			{
				flag = false;
			}
			int num = 30;
			if (Main.gameMenu)
			{
				num = 0;
			}
			if (WorldGen.drunkWorldGen)
			{
				num = -180;
			}
			float num12 = (float)Main.worldSurface;
			if (num12 == 0f)
			{
				num12 = 1f;
			}
			float num17 = Main.screenPosition.Y + (float)(Main.screenHeight / 2) - 600f;
			double backgroundTopMagicNumber = (num17 - screenOff / 2f) / (num12 * 16f);
			backgroundTopMagicNumber = 0f - MathHelper.Lerp((float)backgroundTopMagicNumber, 1f, 0f);
			backgroundTopMagicNumber = (0f - num17 + screenOff / 2f) / (num12 * 16f);
			float bgGlobalScaleMultiplier = 2f;
			int pushBGTopHack = 0;
			int num3 = -180;
			bool flag2 = true;
			int num4 = 0;
			if (Main.gameMenu)
			{
				num4 -= num3;
			}
			pushBGTopHack = num4;
			pushBGTopHack += num;
			if (flag2)
			{
				pushBGTopHack += num3;
			}
			if (flag)
			{
				Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>(TexturePath2);
				bgScale = 1.25f;
				bgScale *= bgGlobalScaleMultiplier;
				bgWidthScaled = (int)((float)texture2.Width * bgScale);
				bgParallax = 0.37;
				SkyManager.Instance.DrawToDepth(spriteBatch, 1f / (float)bgParallax);
				bgStartX = (int)(0.0 - Math.IEEERemainder((double)Main.screenPosition.X * bgParallax, bgWidthScaled) - (double)(bgWidthScaled / 2));
				bgTopY = (int)(backgroundTopMagicNumber * 1800.0 + 1750.0) + (int)scAdj + pushBGTopHack;
				if (Main.gameMenu)
				{
					bgTopY = 320 + pushBGTopHack;
				}
				bgLoops = Main.screenWidth / bgWidthScaled + 2;
				if ((double)Main.screenPosition.Y < Main.worldSurface * 16.0 + 16.0)
				{
					for (int i = 0; i < bgLoops; i++)
					{
						spriteBatch.Draw(texture2, new Vector2((float)(bgStartX + bgWidthScaled * i), (float)bgTopY), (Rectangle?)new Rectangle(0, 0, texture2.Width, texture2.Height), ColorOfSurfaceBackgroundsModified, 0f, default(Vector2), bgScale, (SpriteEffects)0, 0f);
					}
				}
				Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(TexturePath);
				bgScale = 1.34f;
				bgScale *= bgGlobalScaleMultiplier;

				bgWidthScaled = (int)((float)texture.Width * bgScale);
				bgParallax = 0.49;
				SkyManager.Instance.DrawToDepth(spriteBatch, 1f / (float)bgParallax);
				bgStartX = (int)(0.0 - Math.IEEERemainder((double)Main.screenPosition.X * bgParallax, bgWidthScaled) - (double)(bgWidthScaled / 2));
				bgTopY = (int)(backgroundTopMagicNumber * 2100.0 + 2150.0) + (int)scAdj + pushBGTopHack;
				if (Main.gameMenu)
				{
					bgTopY = 480 + pushBGTopHack;
					bgStartX -= 120;
				}
				bgLoops = Main.screenWidth / bgWidthScaled + 2;
				if ((double)Main.screenPosition.Y < Main.worldSurface * 16.0 + 16.0)
				{
					for (int j = 0; j < bgLoops; j++)
					{
						spriteBatch.Draw(texture, new Vector2((float)(bgStartX + bgWidthScaled * j), (float)bgTopY), (Rectangle?)new Rectangle(0, 0, texture.Width, texture.Height), ColorOfSurfaceBackgroundsModified, 0f, default(Vector2), bgScale, (SpriteEffects)0, 0f);
					}
				}
			}
			if (flag2)
			{
				pushBGTopHack -= num3;
			}
			Texture2D value = TextureAssets.MagicPixel.Value;
			float flashPower = WorldGen.BackgroundsCache.GetFlashPower(9);
			Color color = Color.Black * flashPower;
			spriteBatch.Draw(value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), color);
			return false;
		}

		//For the future it awaits
		/*public override void ModifyStyleFade(float[] fades, float transitionSpeed)
        {
            for (int i = 0; i < fades.Length; i++)
            {
                if (i == Slot)
                {
                    fades[i] += transitionSpeed;
                    if (fades[i] > 1f)
                    {
                        fades[i] = 1f;
                    }
                }
                else
                {
                    fades[i] -= transitionSpeed;
                    if (fades[i] < 0f)
                    {
                        fades[i] = 0f;
                    }
                }
            }
        }

        public override int ChooseFarTexture(in BackgroundLayerParams layerParams)
        {
            return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSandSurfaceFar");
        }

        public override int ChooseMiddleTexture(in BackgroundLayerParams layerParams)
        {
            return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSandSurfaceMid");
        }

        public override int ChooseCloseTexture(in BackgroundLayerParams layerParams)
        {
            return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSandSurfaceClose");
        }*/
	}
}
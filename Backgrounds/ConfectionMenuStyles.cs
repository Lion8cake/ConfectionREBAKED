using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Backgrounds {
	public class ConfectionMenuStyle0 : ModSurfaceBackgroundStyle {
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
			return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSurfaceFar");
		}

		public override int ChooseMiddleTexture() {
			return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSurfaceMid");
		}

		public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b) {
			return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSurfaceClose1");
		}

		public override bool PreDrawCloseBackground(SpriteBatch spriteBatch) {
			string TexturePath = "TheConfectionRebirth/Backgrounds/ConfectionSurfaceClose1";
			string TexturePath2 = "TheConfectionRebirth/Backgrounds/ConfectionSurfaceClose2";
			string TexturePath3 = "TheConfectionRebirth/Projectiles/CreamBolt";
			DrawMenuBG(TexturePath, TexturePath2, TexturePath3, 0);
			return false;
		}

		public static void DrawMenuBG(string TexturePath, string TexturePath2, string TexturePath3, int backgroundID)
		{
			float bgScale = Main.instance.GetBGScale();
			float screenOff = Main.instance.GetScreenOff();
			double bgParallax = Main.instance.GetBGParallax();
			int bgTopY = Main.instance.GetBGTopY();
			float scAdj = Main.instance.GetSCAdj();
			int bgWidthScaled = Main.instance.GetBGWidthScaled();
			int bgStartX = Main.instance.GetBGStartX();
			int bgLoops = Main.instance.GetBGLoops();
			Color ColorOfSurfaceBackgroundsModified = Main.instance.GetColorOFSurfaceBackgroundsModified();
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
				if (TexturePath3 != null)
				{
					bgScale = 1.25f;
					bgParallax = 0.4;
					bgTopY = (int)(backgroundTopMagicNumber * 1800.0 + 1500.0) + (int)scAdj + pushBGTopHack;
					bgScale *= bgGlobalScaleMultiplier;
					bgWidthScaled = (int)((float)ModContent.Request<Texture2D>(TexturePath3).Width() * bgScale);
					SkyManager.Instance.DrawToDepth(Main.spriteBatch, 1.2f / (float)bgParallax);
					bgStartX = (int)(0.0 - Math.IEEERemainder((double)Main.screenPosition.X * bgParallax, bgWidthScaled) - (double)(bgWidthScaled / 2));
					if (Main.gameMenu)
						bgTopY = 320 + pushBGTopHack;

					bgLoops = Main.screenWidth / bgWidthScaled + 2;
					if ((double)Main.screenPosition.Y < Main.worldSurface * 16.0 + 16.0)
					{
						for (int i = 0; i < bgLoops; i++)
						{
							Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(TexturePath3), new Vector2(bgStartX + bgWidthScaled * i, bgTopY), new Rectangle(0, 0, ModContent.Request<Texture2D>(TexturePath3).Width(), ModContent.Request<Texture2D>(TexturePath3).Height()), ColorOfSurfaceBackgroundsModified, 0f, default(Vector2), bgScale, SpriteEffects.None, 0f);
						}
					}
				}
				if (TexturePath2 != null)
				{
					bgScale = 1.31f;
					bgParallax = 0.43;
					bgTopY = (int)(backgroundTopMagicNumber * 1950.0 + 1750.0) + (int)scAdj + pushBGTopHack;
					bgScale *= bgGlobalScaleMultiplier;
					if (backgroundID == 4)
						bgParallax = 0.27;
					bgWidthScaled = (int)((float)ModContent.Request<Texture2D>(TexturePath2).Width() * bgScale);
					SkyManager.Instance.DrawToDepth(Main.spriteBatch, 1f / (float)bgParallax);
					bgStartX = (int)(0.0 - Math.IEEERemainder((double)Main.screenPosition.X * bgParallax, bgWidthScaled) - (double)(bgWidthScaled / 2));
					if (Main.gameMenu)
					{
						bgTopY = 400 + pushBGTopHack;
						bgStartX -= 80;
					}

					bgLoops = Main.screenWidth / bgWidthScaled + 2;
					if ((double)Main.screenPosition.Y < Main.worldSurface * 16.0 + 16.0)
					{
						for (int i = 0; i < bgLoops; i++)
						{
							Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(TexturePath2), new Vector2(bgStartX + bgWidthScaled * i, bgTopY), new Rectangle(0, 0, ModContent.Request<Texture2D>(TexturePath2).Width(), ModContent.Request<Texture2D>(TexturePath2).Height()), ColorOfSurfaceBackgroundsModified, 0f, default(Vector2), bgScale, SpriteEffects.None, 0f);
						}
					}
				}
				if (TexturePath != null)
				{
					bgScale = 1.34f;
					bgParallax = 0.49;
					bgTopY = (int)(backgroundTopMagicNumber * 2100.0 + 2000.0) + (int)scAdj + pushBGTopHack;
					bgScale *= bgGlobalScaleMultiplier;
					if (backgroundID == 4)
						bgParallax = 0.4;
					bgWidthScaled = (int)((float)ModContent.Request<Texture2D>(TexturePath).Width() * bgScale);
					SkyManager.Instance.DrawToDepth(Main.spriteBatch, 1f / (float)bgParallax);
					bgStartX = (int)(0.0 - Math.IEEERemainder((double)Main.screenPosition.X * bgParallax, bgWidthScaled) - (double)(bgWidthScaled / 2));
					if (Main.gameMenu)
					{
						bgTopY = 480 + pushBGTopHack;
						bgStartX -= 120;
					}

					bgLoops = Main.screenWidth / bgWidthScaled + 2;
					if ((double)Main.screenPosition.Y < Main.worldSurface * 16.0 + 16.0)
					{
						for (int i = 0; i < bgLoops; i++)
						{
							Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(TexturePath), new Vector2(bgStartX + bgWidthScaled * i, bgTopY), new Rectangle(0, 0, ModContent.Request<Texture2D>(TexturePath).Width(), ModContent.Request<Texture2D>(TexturePath).Height()), ColorOfSurfaceBackgroundsModified, 0f, default(Vector2), bgScale, SpriteEffects.None, 0f);
						}
					}
				}
			}
			if (flag2)
			{
				pushBGTopHack -= num3;
			}
		}
	}

	public class ConfectionMenuStyle1 : ModSurfaceBackgroundStyle {
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
			return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionFarBG");
		}

		public override int ChooseMiddleTexture() {
			return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionMidBG");
		}

		public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b) {
			return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSurface1Close1");
		}

		public override bool PreDrawCloseBackground(SpriteBatch spriteBatch) {
			string TexturePath = "TheConfectionRebirth/Backgrounds/ConfectionSurface1Close1";
			string TexturePath2 = "TheConfectionRebirth/Backgrounds/ConfectionSurface1Close2";
			string TexturePath3 = "TheConfectionRebirth/Backgrounds/ConfectionSurface1Close3";
			ConfectionMenuStyle0.DrawMenuBG(TexturePath, TexturePath2, TexturePath3, 1);
			return false;
		}
	}

	public class ConfectionMenuStyle2 : ModSurfaceBackgroundStyle {
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
			return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionFarBG");
		}

		public override int ChooseMiddleTexture() {
			return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionMidBG");
		}

		public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b) {
			return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSurface2Close1");
		}

		public override bool PreDrawCloseBackground(SpriteBatch spriteBatch) {
			string TexturePath = "TheConfectionRebirth/Backgrounds/ConfectionSurface2Close1";
			string TexturePath2 = "TheConfectionRebirth/Backgrounds/ConfectionSurface2Close2";
			string TexturePath3 = "TheConfectionRebirth/Backgrounds/ConfectionSurface2Close3";
			ConfectionMenuStyle0.DrawMenuBG(TexturePath, TexturePath2, TexturePath3, 2);
			return false;
		}
	}

	public class ConfectionMenuStyle3 : ModSurfaceBackgroundStyle {
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
			return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionFarBG");
		}

		public override int ChooseMiddleTexture() {
			return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionMidBG");
		}

		public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b) {
			return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSurface3Close1");
		}

		public override bool PreDrawCloseBackground(SpriteBatch spriteBatch) {
			string TexturePath = "TheConfectionRebirth/Backgrounds/ConfectionSurface3Close1";
			string TexturePath2 = "TheConfectionRebirth/Backgrounds/ConfectionSurface3Close2";
			string TexturePath3 = "TheConfectionRebirth/Backgrounds/ConfectionSurface3Close3";
			ConfectionMenuStyle0.DrawMenuBG(TexturePath, TexturePath2, TexturePath3, 3);
			return false;
		}
	}

	public class ConfectionMenuStyle4 : ModSurfaceBackgroundStyle
	{
		public override void ModifyFarFades(float[] fades, float transitionSpeed)
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

		public override int ChooseFarTexture()
		{
			return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionFarBG");
		}

		public override int ChooseMiddleTexture()
		{
			return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionMidBG");
		}

		public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
		{
			return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSurface4Close1");
		}

		public override bool PreDrawCloseBackground(SpriteBatch spriteBatch)
		{
			string TexturePath = "TheConfectionRebirth/Backgrounds/ConfectionSurface4Close1";
			string TexturePath2 = "TheConfectionRebirth/Backgrounds/ConfectionSurface4Close2";
			string TexturePath3 = "TheConfectionRebirth/Projectiles/CreamBolt";
			ConfectionMenuStyle0.DrawMenuBG(TexturePath, TexturePath2, TexturePath3, 4);
			return false;
		}
	}
}
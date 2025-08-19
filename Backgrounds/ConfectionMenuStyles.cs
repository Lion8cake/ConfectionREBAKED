using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Reflection;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

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
			float bgScale = (float)typeof(Main).GetField("bgScale", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
			float screenOff = (float)typeof(Main).GetField("screenOff", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance);
			double bgParallax = (double)typeof(Main).GetField("bgParallax", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance);
			int bgTopY = (int)typeof(Main).GetField("bgTopY", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance);
			float scAdj = (float)typeof(Main).GetField("scAdj", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance);
			int bgWidthScaled = (int)typeof(Main).GetField("bgWidthScaled", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
			int bgStartX = (int)typeof(Main).GetField("bgStartX", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance);
			int bgLoops = (int)typeof(Main).GetField("bgLoops", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance);
			Color ColorOfSurfaceBackgroundsModified = (Color)typeof(Main).GetField("ColorOfSurfaceBackgroundsModified", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);

			string TexturePath = "TheConfectionRebirth/Backgrounds/ConfectionSurfaceClose1";
			string TexturePath2 = "TheConfectionRebirth/Backgrounds/ConfectionSurfaceClose2";
			string TexturePath3 = "TheConfectionRebirth/Projectiles/CreamBolt";

			bool flag = false;
			if ((!Main.remixWorld || (Main.gameMenu && !WorldGen.remixWorldGen)) && (!WorldGen.remixWorldGen || !WorldGen.drunkWorldGen)) {
				flag = true;
			}
			if (Main.mapFullscreen) {
				flag = false;
			}
			int num = 30;
			if (Main.gameMenu) {
				num = 0;
			}
			if (WorldGen.drunkWorldGen) {
				num = -180;
			}
			float num12 = (float)Main.worldSurface;
			if (num12 == 0f) {
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
			if (Main.gameMenu) {
				num4 -= num3;
			}
			pushBGTopHack = num4;
			pushBGTopHack += num;
			if (flag2) {
				pushBGTopHack += num3;
			}
			if (flag) {
				bgScale = 1.25f;
				bgParallax = 0.4;
				bgTopY = (int)(backgroundTopMagicNumber * 1800.0 + 1500.0) + (int)scAdj + pushBGTopHack;
				//Main.instance.SetBackgroundOffsets((Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurfaceClose1"), backgroundTopMagicNumber, pushBGTopHack);
				bgScale *= bgGlobalScaleMultiplier;
				//Main.instance.LoadBackground(ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurfaceClose1").Value);
				bgWidthScaled = (int)((float)ModContent.Request<Texture2D>(TexturePath3).Width() * bgScale);
				SkyManager.Instance.DrawToDepth(Main.spriteBatch, 1.2f / (float)bgParallax);
				bgStartX = (int)(0.0 - Math.IEEERemainder((double)Main.screenPosition.X * bgParallax, bgWidthScaled) - (double)(bgWidthScaled / 2));
				if (Main.gameMenu)
					bgTopY = 320 + pushBGTopHack;

				bgLoops = Main.screenWidth / bgWidthScaled + 2;
				if ((double)Main.screenPosition.Y < Main.worldSurface * 16.0 + 16.0) {
					for (int i = 0; i < bgLoops; i++) {
						Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(TexturePath3), new Vector2(bgStartX + bgWidthScaled * i, bgTopY), new Rectangle(0, 0, ModContent.Request<Texture2D>(TexturePath3).Width(), ModContent.Request<Texture2D>(TexturePath3).Height()), ColorOfSurfaceBackgroundsModified, 0f, default(Vector2), bgScale, SpriteEffects.None, 0f);
					}
				}

				bgScale = 1.31f;
				bgParallax = 0.43;
				bgTopY = (int)(backgroundTopMagicNumber * 1950.0 + 1750.0) + (int)scAdj + pushBGTopHack;
				//Main.instance.SetBackgroundOffsets(textureSlot2, backgroundTopMagicNumber, pushBGTopHack);
				bgScale *= bgGlobalScaleMultiplier;
				//Main.instance.LoadBackground(textureSlot2);
				bgWidthScaled = (int)((float)ModContent.Request<Texture2D>(TexturePath2).Width() * bgScale);
				SkyManager.Instance.DrawToDepth(Main.spriteBatch, 1f / (float)bgParallax);
				bgStartX = (int)(0.0 - Math.IEEERemainder((double)Main.screenPosition.X * bgParallax, bgWidthScaled) - (double)(bgWidthScaled / 2));
				if (Main.gameMenu) {
					bgTopY = 400 + pushBGTopHack;
					bgStartX -= 80;
				}

				bgLoops = Main.screenWidth / bgWidthScaled + 2;
				if ((double)Main.screenPosition.Y < Main.worldSurface * 16.0 + 16.0) {
					for (int i = 0; i < bgLoops; i++) {
						Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(TexturePath2), new Vector2(bgStartX + bgWidthScaled * i, bgTopY), new Rectangle(0, 0, ModContent.Request<Texture2D>(TexturePath2).Width(), ModContent.Request<Texture2D>(TexturePath2).Height()), ColorOfSurfaceBackgroundsModified, 0f, default(Vector2), bgScale, SpriteEffects.None, 0f);
					}
				}
				bgScale = 1.34f;
				bgParallax = 0.49;
				bgTopY = (int)(backgroundTopMagicNumber * 2100.0 + 2000.0) + (int)scAdj + pushBGTopHack;
				//Main.instance.SetBackgroundOffsets(textureSlot3, backgroundTopMagicNumber, pushBGTopHack);
				bgScale *= bgGlobalScaleMultiplier;
				//Main.instance.LoadBackground(textureSlot3);
				bgWidthScaled = (int)((float)ModContent.Request<Texture2D>(TexturePath).Width() * bgScale);
				SkyManager.Instance.DrawToDepth(Main.spriteBatch, 1f / (float)bgParallax);
				bgStartX = (int)(0.0 - Math.IEEERemainder((double)Main.screenPosition.X * bgParallax, bgWidthScaled) - (double)(bgWidthScaled / 2));
				if (Main.gameMenu) {
					bgTopY = 480 + pushBGTopHack;
					bgStartX -= 120;
				}

				bgLoops = Main.screenWidth / bgWidthScaled + 2;
				if ((double)Main.screenPosition.Y < Main.worldSurface * 16.0 + 16.0) {
					for (int i = 0; i < bgLoops; i++) {
						Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(TexturePath), new Vector2(bgStartX + bgWidthScaled * i, bgTopY), new Rectangle(0, 0, ModContent.Request<Texture2D>(TexturePath).Width(), ModContent.Request<Texture2D>(TexturePath).Height()), ColorOfSurfaceBackgroundsModified, 0f, default(Vector2), bgScale, SpriteEffects.None, 0f);
					}
				}
			}
			if (flag2) {
				pushBGTopHack -= num3;
			}
			return false;
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
			float bgScale = (float)typeof(Main).GetField("bgScale", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
			float screenOff = (float)typeof(Main).GetField("screenOff", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance);
			double bgParallax = (double)typeof(Main).GetField("bgParallax", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance);
			int bgTopY = (int)typeof(Main).GetField("bgTopY", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance);
			float scAdj = (float)typeof(Main).GetField("scAdj", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance);
			int bgWidthScaled = (int)typeof(Main).GetField("bgWidthScaled", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
			int bgStartX = (int)typeof(Main).GetField("bgStartX", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance);
			int bgLoops = (int)typeof(Main).GetField("bgLoops", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance);
			Color ColorOfSurfaceBackgroundsModified = (Color)typeof(Main).GetField("ColorOfSurfaceBackgroundsModified", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);

			string TexturePath = "TheConfectionRebirth/Backgrounds/ConfectionSurface1Close1";
			string TexturePath2 = "TheConfectionRebirth/Backgrounds/ConfectionSurface1Close2";
			string TexturePath3 = "TheConfectionRebirth/Backgrounds/ConfectionSurface1Close3";

			bool flag = false;
			if ((!Main.remixWorld || (Main.gameMenu && !WorldGen.remixWorldGen)) && (!WorldGen.remixWorldGen || !WorldGen.drunkWorldGen)) {
				flag = true;
			}
			if (Main.mapFullscreen) {
				flag = false;
			}
			int num = 30;
			if (Main.gameMenu) {
				num = 0;
			}
			if (WorldGen.drunkWorldGen) {
				num = -180;
			}
			float num12 = (float)Main.worldSurface;
			if (num12 == 0f) {
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
			if (Main.gameMenu) {
				num4 -= num3;
			}
			pushBGTopHack = num4;
			pushBGTopHack += num;
			if (flag2) {
				pushBGTopHack += num3;
			}
			if (flag) {
				bgScale = 1.25f;
				bgParallax = 0.4;
				bgTopY = (int)(backgroundTopMagicNumber * 1800.0 + 1500.0) + (int)scAdj + pushBGTopHack;
				//Main.instance.SetBackgroundOffsets((Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurfaceClose1"), backgroundTopMagicNumber, pushBGTopHack);
				bgScale *= bgGlobalScaleMultiplier;
				//Main.instance.LoadBackground(ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurfaceClose1").Value);
				bgWidthScaled = (int)((float)ModContent.Request<Texture2D>(TexturePath3).Width() * bgScale);
				SkyManager.Instance.DrawToDepth(Main.spriteBatch, 1.2f / (float)bgParallax);
				bgStartX = (int)(0.0 - Math.IEEERemainder((double)Main.screenPosition.X * bgParallax, bgWidthScaled) - (double)(bgWidthScaled / 2));
				if (Main.gameMenu)
					bgTopY = 320 + pushBGTopHack;

				bgLoops = Main.screenWidth / bgWidthScaled + 2;
				if ((double)Main.screenPosition.Y < Main.worldSurface * 16.0 + 16.0) {
					for (int i = 0; i < bgLoops; i++) {
						Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(TexturePath3), new Vector2(bgStartX + bgWidthScaled * i, bgTopY), new Rectangle(0, 0, ModContent.Request<Texture2D>(TexturePath3).Width(), ModContent.Request<Texture2D>(TexturePath3).Height()), ColorOfSurfaceBackgroundsModified, 0f, default(Vector2), bgScale, SpriteEffects.None, 0f);
					}
				}

				bgScale = 1.31f;
				bgParallax = 0.43;
				bgTopY = (int)(backgroundTopMagicNumber * 1950.0 + 1750.0) + (int)scAdj + pushBGTopHack;
				//Main.instance.SetBackgroundOffsets(textureSlot2, backgroundTopMagicNumber, pushBGTopHack);
				bgScale *= bgGlobalScaleMultiplier;
				//Main.instance.LoadBackground(textureSlot2);
				bgWidthScaled = (int)((float)ModContent.Request<Texture2D>(TexturePath2).Width() * bgScale);
				SkyManager.Instance.DrawToDepth(Main.spriteBatch, 1f / (float)bgParallax);
				bgStartX = (int)(0.0 - Math.IEEERemainder((double)Main.screenPosition.X * bgParallax, bgWidthScaled) - (double)(bgWidthScaled / 2));
				if (Main.gameMenu) {
					bgTopY = 400 + pushBGTopHack;
					bgStartX -= 80;
				}

				bgLoops = Main.screenWidth / bgWidthScaled + 2;
				if ((double)Main.screenPosition.Y < Main.worldSurface * 16.0 + 16.0) {
					for (int i = 0; i < bgLoops; i++) {
						Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(TexturePath2), new Vector2(bgStartX + bgWidthScaled * i, bgTopY), new Rectangle(0, 0, ModContent.Request<Texture2D>(TexturePath2).Width(), ModContent.Request<Texture2D>(TexturePath2).Height()), ColorOfSurfaceBackgroundsModified, 0f, default(Vector2), bgScale, SpriteEffects.None, 0f);
					}
				}
				bgScale = 1.34f;
				bgParallax = 0.49;
				bgTopY = (int)(backgroundTopMagicNumber * 2100.0 + 2000.0) + (int)scAdj + pushBGTopHack;
				//Main.instance.SetBackgroundOffsets(textureSlot3, backgroundTopMagicNumber, pushBGTopHack);
				bgScale *= bgGlobalScaleMultiplier;
				//Main.instance.LoadBackground(textureSlot3);
				bgWidthScaled = (int)((float)ModContent.Request<Texture2D>(TexturePath).Width() * bgScale);
				SkyManager.Instance.DrawToDepth(Main.spriteBatch, 1f / (float)bgParallax);
				bgStartX = (int)(0.0 - Math.IEEERemainder((double)Main.screenPosition.X * bgParallax, bgWidthScaled) - (double)(bgWidthScaled / 2));
				if (Main.gameMenu) {
					bgTopY = 480 + pushBGTopHack;
					bgStartX -= 120;
				}

				bgLoops = Main.screenWidth / bgWidthScaled + 2;
				if ((double)Main.screenPosition.Y < Main.worldSurface * 16.0 + 16.0) {
					for (int i = 0; i < bgLoops; i++) {
						Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(TexturePath), new Vector2(bgStartX + bgWidthScaled * i, bgTopY), new Rectangle(0, 0, ModContent.Request<Texture2D>(TexturePath).Width(), ModContent.Request<Texture2D>(TexturePath).Height()), ColorOfSurfaceBackgroundsModified, 0f, default(Vector2), bgScale, SpriteEffects.None, 0f);
					}
				}
			}
			if (flag2) {
				pushBGTopHack -= num3;
			}
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
			float bgScale = (float)typeof(Main).GetField("bgScale", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
			float screenOff = (float)typeof(Main).GetField("screenOff", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance);
			double bgParallax = (double)typeof(Main).GetField("bgParallax", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance);
			int bgTopY = (int)typeof(Main).GetField("bgTopY", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance);
			float scAdj = (float)typeof(Main).GetField("scAdj", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance);
			int bgWidthScaled = (int)typeof(Main).GetField("bgWidthScaled", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
			int bgStartX = (int)typeof(Main).GetField("bgStartX", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance);
			int bgLoops = (int)typeof(Main).GetField("bgLoops", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance);
			Color ColorOfSurfaceBackgroundsModified = (Color)typeof(Main).GetField("ColorOfSurfaceBackgroundsModified", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);

			string TexturePath = "TheConfectionRebirth/Backgrounds/ConfectionSurface2Close1";
			string TexturePath2 = "TheConfectionRebirth/Backgrounds/ConfectionSurface2Close2";
			string TexturePath3 = "TheConfectionRebirth/Backgrounds/ConfectionSurface2Close3";

			bool flag = false;
			if ((!Main.remixWorld || (Main.gameMenu && !WorldGen.remixWorldGen)) && (!WorldGen.remixWorldGen || !WorldGen.drunkWorldGen)) {
				flag = true;
			}
			if (Main.mapFullscreen) {
				flag = false;
			}
			int num = 30;
			if (Main.gameMenu) {
				num = 0;
			}
			if (WorldGen.drunkWorldGen) {
				num = -180;
			}
			float num12 = (float)Main.worldSurface;
			if (num12 == 0f) {
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
			if (Main.gameMenu) {
				num4 -= num3;
			}
			pushBGTopHack = num4;
			pushBGTopHack += num;
			if (flag2) {
				pushBGTopHack += num3;
			}
			if (flag) {
				bgScale = 1.25f;
				bgParallax = 0.4;
				bgTopY = (int)(backgroundTopMagicNumber * 1800.0 + 1500.0) + (int)scAdj + pushBGTopHack;
				//Main.instance.SetBackgroundOffsets((Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurfaceClose1"), backgroundTopMagicNumber, pushBGTopHack);
				bgScale *= bgGlobalScaleMultiplier;
				//Main.instance.LoadBackground(ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurfaceClose1").Value);
				bgWidthScaled = (int)((float)ModContent.Request<Texture2D>(TexturePath3).Width() * bgScale);
				SkyManager.Instance.DrawToDepth(Main.spriteBatch, 1.2f / (float)bgParallax);
				bgStartX = (int)(0.0 - Math.IEEERemainder((double)Main.screenPosition.X * bgParallax, bgWidthScaled) - (double)(bgWidthScaled / 2));
				if (Main.gameMenu)
					bgTopY = 320 + pushBGTopHack;

				bgLoops = Main.screenWidth / bgWidthScaled + 2;
				if ((double)Main.screenPosition.Y < Main.worldSurface * 16.0 + 16.0) {
					for (int i = 0; i < bgLoops; i++) {
						Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(TexturePath3), new Vector2(bgStartX + bgWidthScaled * i, bgTopY), new Rectangle(0, 0, ModContent.Request<Texture2D>(TexturePath3).Width(), ModContent.Request<Texture2D>(TexturePath3).Height()), ColorOfSurfaceBackgroundsModified, 0f, default(Vector2), bgScale, SpriteEffects.None, 0f);
					}
				}

				bgScale = 1.31f;
				bgParallax = 0.43;
				bgTopY = (int)(backgroundTopMagicNumber * 1950.0 + 1750.0) + (int)scAdj + pushBGTopHack;
				//Main.instance.SetBackgroundOffsets(textureSlot2, backgroundTopMagicNumber, pushBGTopHack);
				bgScale *= bgGlobalScaleMultiplier;
				//Main.instance.LoadBackground(textureSlot2);
				bgWidthScaled = (int)((float)ModContent.Request<Texture2D>(TexturePath2).Width() * bgScale);
				SkyManager.Instance.DrawToDepth(Main.spriteBatch, 1f / (float)bgParallax);
				bgStartX = (int)(0.0 - Math.IEEERemainder((double)Main.screenPosition.X * bgParallax, bgWidthScaled) - (double)(bgWidthScaled / 2));
				if (Main.gameMenu) {
					bgTopY = 400 + pushBGTopHack;
					bgStartX -= 80;
				}

				bgLoops = Main.screenWidth / bgWidthScaled + 2;
				if ((double)Main.screenPosition.Y < Main.worldSurface * 16.0 + 16.0) {
					for (int i = 0; i < bgLoops; i++) {
						Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(TexturePath2), new Vector2(bgStartX + bgWidthScaled * i, bgTopY), new Rectangle(0, 0, ModContent.Request<Texture2D>(TexturePath2).Width(), ModContent.Request<Texture2D>(TexturePath2).Height()), ColorOfSurfaceBackgroundsModified, 0f, default(Vector2), bgScale, SpriteEffects.None, 0f);
					}
				}
				bgScale = 1.34f;
				bgParallax = 0.49;
				bgTopY = (int)(backgroundTopMagicNumber * 2100.0 + 2000.0) + (int)scAdj + pushBGTopHack;
				//Main.instance.SetBackgroundOffsets(textureSlot3, backgroundTopMagicNumber, pushBGTopHack);
				bgScale *= bgGlobalScaleMultiplier;
				//Main.instance.LoadBackground(textureSlot3);
				bgWidthScaled = (int)((float)ModContent.Request<Texture2D>(TexturePath).Width() * bgScale);
				SkyManager.Instance.DrawToDepth(Main.spriteBatch, 1f / (float)bgParallax);
				bgStartX = (int)(0.0 - Math.IEEERemainder((double)Main.screenPosition.X * bgParallax, bgWidthScaled) - (double)(bgWidthScaled / 2));
				if (Main.gameMenu) {
					bgTopY = 480 + pushBGTopHack;
					bgStartX -= 120;
				}

				bgLoops = Main.screenWidth / bgWidthScaled + 2;
				if ((double)Main.screenPosition.Y < Main.worldSurface * 16.0 + 16.0) {
					for (int i = 0; i < bgLoops; i++) {
						Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(TexturePath), new Vector2(bgStartX + bgWidthScaled * i, bgTopY), new Rectangle(0, 0, ModContent.Request<Texture2D>(TexturePath).Width(), ModContent.Request<Texture2D>(TexturePath).Height()), ColorOfSurfaceBackgroundsModified, 0f, default(Vector2), bgScale, SpriteEffects.None, 0f);
					}
				}
			}
			if (flag2) {
				pushBGTopHack -= num3;
			}
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
			float bgScale = (float)typeof(Main).GetField("bgScale", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
			float screenOff = (float)typeof(Main).GetField("screenOff", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance);
			double bgParallax = (double)typeof(Main).GetField("bgParallax", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance);
			int bgTopY = (int)typeof(Main).GetField("bgTopY", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance);
			float scAdj = (float)typeof(Main).GetField("scAdj", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance);
			int bgWidthScaled = (int)typeof(Main).GetField("bgWidthScaled", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
			int bgStartX = (int)typeof(Main).GetField("bgStartX", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance);
			int bgLoops = (int)typeof(Main).GetField("bgLoops", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance);
			Color ColorOfSurfaceBackgroundsModified = (Color)typeof(Main).GetField("ColorOfSurfaceBackgroundsModified", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);

			string TexturePath = "TheConfectionRebirth/Backgrounds/ConfectionSurface3Close1";
			string TexturePath2 = "TheConfectionRebirth/Backgrounds/ConfectionSurface3Close2";
			string TexturePath3 = "TheConfectionRebirth/Backgrounds/ConfectionSurface3Close3";

			bool flag = false;
			if ((!Main.remixWorld || (Main.gameMenu && !WorldGen.remixWorldGen)) && (!WorldGen.remixWorldGen || !WorldGen.drunkWorldGen)) {
				flag = true;
			}
			if (Main.mapFullscreen) {
				flag = false;
			}
			int num = 30;
			if (Main.gameMenu) {
				num = 0;
			}
			if (WorldGen.drunkWorldGen) {
				num = -180;
			}
			float num12 = (float)Main.worldSurface;
			if (num12 == 0f) {
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
			if (Main.gameMenu) {
				num4 -= num3;
			}
			pushBGTopHack = num4;
			pushBGTopHack += num;
			if (flag2) {
				pushBGTopHack += num3;
			}
			if (flag) {
				bgScale = 1.25f;
				bgParallax = 0.4;
				bgTopY = (int)(backgroundTopMagicNumber * 1800.0 + 1500.0) + (int)scAdj + pushBGTopHack;
				//Main.instance.SetBackgroundOffsets((Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurfaceClose1"), backgroundTopMagicNumber, pushBGTopHack);
				bgScale *= bgGlobalScaleMultiplier;
				//Main.instance.LoadBackground(ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurfaceClose1").Value);
				bgWidthScaled = (int)((float)ModContent.Request<Texture2D>(TexturePath3).Width() * bgScale);
				SkyManager.Instance.DrawToDepth(Main.spriteBatch, 1.2f / (float)bgParallax);
				bgStartX = (int)(0.0 - Math.IEEERemainder((double)Main.screenPosition.X * bgParallax, bgWidthScaled) - (double)(bgWidthScaled / 2));
				if (Main.gameMenu)
					bgTopY = 320 + pushBGTopHack;

				bgLoops = Main.screenWidth / bgWidthScaled + 2;
				if ((double)Main.screenPosition.Y < Main.worldSurface * 16.0 + 16.0) {
					for (int i = 0; i < bgLoops; i++) {
						Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(TexturePath3), new Vector2(bgStartX + bgWidthScaled * i, bgTopY), new Rectangle(0, 0, ModContent.Request<Texture2D>(TexturePath3).Width(), ModContent.Request<Texture2D>(TexturePath3).Height()), ColorOfSurfaceBackgroundsModified, 0f, default(Vector2), bgScale, SpriteEffects.None, 0f);
					}
				}

				bgScale = 1.31f;
				bgParallax = 0.43;
				bgTopY = (int)(backgroundTopMagicNumber * 1950.0 + 1750.0) + (int)scAdj + pushBGTopHack;
				//Main.instance.SetBackgroundOffsets(textureSlot2, backgroundTopMagicNumber, pushBGTopHack);
				bgScale *= bgGlobalScaleMultiplier;
				//Main.instance.LoadBackground(textureSlot2);
				bgWidthScaled = (int)((float)ModContent.Request<Texture2D>(TexturePath2).Width() * bgScale);
				SkyManager.Instance.DrawToDepth(Main.spriteBatch, 1f / (float)bgParallax);
				bgStartX = (int)(0.0 - Math.IEEERemainder((double)Main.screenPosition.X * bgParallax, bgWidthScaled) - (double)(bgWidthScaled / 2));
				if (Main.gameMenu) {
					bgTopY = 400 + pushBGTopHack;
					bgStartX -= 80;
				}

				bgLoops = Main.screenWidth / bgWidthScaled + 2;
				if ((double)Main.screenPosition.Y < Main.worldSurface * 16.0 + 16.0) {
					for (int i = 0; i < bgLoops; i++) {
						Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(TexturePath2), new Vector2(bgStartX + bgWidthScaled * i, bgTopY), new Rectangle(0, 0, ModContent.Request<Texture2D>(TexturePath2).Width(), ModContent.Request<Texture2D>(TexturePath2).Height()), ColorOfSurfaceBackgroundsModified, 0f, default(Vector2), bgScale, SpriteEffects.None, 0f);
					}
				}
				bgScale = 1.34f;
				bgParallax = 0.49;
				bgTopY = (int)(backgroundTopMagicNumber * 2100.0 + 2000.0) + (int)scAdj + pushBGTopHack;
				//Main.instance.SetBackgroundOffsets(textureSlot3, backgroundTopMagicNumber, pushBGTopHack);
				bgScale *= bgGlobalScaleMultiplier;
				//Main.instance.LoadBackground(textureSlot3);
				bgWidthScaled = (int)((float)ModContent.Request<Texture2D>(TexturePath).Width() * bgScale);
				SkyManager.Instance.DrawToDepth(Main.spriteBatch, 1f / (float)bgParallax);
				bgStartX = (int)(0.0 - Math.IEEERemainder((double)Main.screenPosition.X * bgParallax, bgWidthScaled) - (double)(bgWidthScaled / 2));
				if (Main.gameMenu) {
					bgTopY = 480 + pushBGTopHack;
					bgStartX -= 120;
				}

				bgLoops = Main.screenWidth / bgWidthScaled + 2;
				if ((double)Main.screenPosition.Y < Main.worldSurface * 16.0 + 16.0) {
					for (int i = 0; i < bgLoops; i++) {
						Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(TexturePath), new Vector2(bgStartX + bgWidthScaled * i, bgTopY), new Rectangle(0, 0, ModContent.Request<Texture2D>(TexturePath).Width(), ModContent.Request<Texture2D>(TexturePath).Height()), ColorOfSurfaceBackgroundsModified, 0f, default(Vector2), bgScale, SpriteEffects.None, 0f);
					}
				}
			}
			if (flag2) {
				pushBGTopHack -= num3;
			}
			return false;
		}
	}
}
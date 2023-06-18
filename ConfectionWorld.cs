using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TheConfectionRebirth.Tiles;

<<<<<<< HEAD
<<<<<<< Updated upstream
namespace TheConfectionRebirth
{
    public class ConfectionWorld : ModSystem
    {
        internal static int[] ConfectionSurfaceBG = new int[TheConfectionRebirth.bgVarAmount] { -1, -1, -1, -1};
        internal static bool Secret;
        public static int ConfTileCount { get; set; }
        public static float ConfTileInfo => ConfTileCount / 100;
=======
namespace TheConfectionRebirth {
	public class ConfectionWorld : ModSystem {
		public static int ConfTileCount { get; set; }
		public static float ConfTileInfo => ConfTileCount / 100;
>>>>>>> Stashed changes
=======
namespace TheConfectionRebirth {
	public class ConfectionWorld : ModSystem {
		internal static int[] ConfectionSurfaceBG = new int[TheConfectionRebirth.bgVarAmount] { -1, -1, -1, -1 };
		internal static bool Secret;
		public static int ConfTileCount { get; set; }
		public static float ConfTileInfo => ConfTileCount / 100;
>>>>>>> 21d961fc5a6b7f1a1395f5a436ef383fb42b52eb
		public static bool IsEaster => DateTime.Now.Day >= 2 && DateTime.Now.Day <= 24 && DateTime.Now.Month.Equals(4);
		public static float DifficultyScale {
			get {
				float d = Main.expertMode ? 2f : (Main.masterMode ? 3f : 1f);
				if (Main.expertMode)
					d = CreativePowerManager.Instance.GetPower<CreativePowers.DifficultySliderPower>().StrengthMultiplierToGiveNPCs;
				return d;
			}
		}

		public override void Load() => Terraria.On_Main.SetBackColor += Main_SetBackColor;

		public override void Unload() => Terraria.On_Main.SetBackColor -= Main_SetBackColor;

<<<<<<< HEAD
<<<<<<< Updated upstream
		private void Main_SetBackColor(On.Terraria.Main.orig_SetBackColor orig, Main.InfoToSetBackColor info, out Color sunColor, out Color moonColor)
		{
=======
		private void Main_SetBackColor(Terraria.On_Main.orig_SetBackColor orig, Main.InfoToSetBackColor info, out Color sunColor, out Color moonColor) {
>>>>>>> Stashed changes
=======
		private void Main_SetBackColor(On.Terraria.Main.orig_SetBackColor orig, Main.InfoToSetBackColor info, out Color sunColor, out Color moonColor) {
>>>>>>> 21d961fc5a6b7f1a1395f5a436ef383fb42b52eb
			double num = Main.time;
			Color bgColorToSet = Color.White;
			sunColor = Color.White;
			moonColor = Color.White;
			bool isInGameMenuOrIsServer = info.isInGameMenuOrIsServer;
			if (Main.dayTime) {
				if (num < 13500.0) {
					float num2 = (float)(num / 13500.0);
					sunColor.R = (byte)((num2 * 200f) + 55f);
					sunColor.G = (byte)((num2 * 180f) + 75f);
					sunColor.B = (byte)((num2 * 250f) + 5f);
					bgColorToSet.R = (byte)((num2 * 230f) + 25f);
					bgColorToSet.G = (byte)((num2 * 220f) + 35f);
					bgColorToSet.B = (byte)((num2 * 220f) + 35f);
				}
				if (num > 45900.0) {
					float num2 = (float)(1.0 - (((num / 54000.0) - 0.85) * 6.666666666666667));
					sunColor.R = (byte)((num2 * 120f) + 55f);
					sunColor.G = (byte)((num2 * 100f) + 25f);
					sunColor.B = (byte)((num2 * 120f) + 55f);
					bgColorToSet.R = (byte)((num2 * 200f) + 35f);
					bgColorToSet.G = (byte)((num2 * 85f) + 35f);
					bgColorToSet.B = (byte)((num2 * 135f) + 35f);
				}
				else if (num > 37800.0) {
					float num2 = (float)(1.0 - (((num / 54000.0) - 0.7) * 6.666666666666667));
					sunColor.R = (byte)((num2 * 80f) + 175f);
					sunColor.G = (byte)((num2 * 130f) + 125f);
					sunColor.B = (byte)((num2 * 100f) + 155f);
					bgColorToSet.R = (byte)((num2 * 20f) + 235f);
					bgColorToSet.G = (byte)((num2 * 135f) + 120f);
					bgColorToSet.B = (byte)((num2 * 85f) + 170f);
				}
			}
			if (!Main.dayTime) {
				if (info.BloodMoonActive) {
					if (num < 16200.0) {
						float num2 = (float)(1.0 - num / 16200.0);
						moonColor.R = (byte)(num2 * 10f + 205f);
						moonColor.G = (byte)(num2 * 170f + 55f);
						moonColor.B = (byte)(num2 * 200f + 55f);
						bgColorToSet.R = (byte)(40f - num2 * 40f + 35f);
						bgColorToSet.G = (byte)(num2 * 20f + 15f);
						bgColorToSet.B = (byte)(num2 * 20f + 15f);
					}
					else if (num >= 16200.0) {
						float num2 = (float)((num / 32400.0 - 0.5) * 2.0);
						moonColor.R = (byte)(num2 * 50f + 205f);
						moonColor.G = (byte)(num2 * 100f + 155f);
						moonColor.B = (byte)(num2 * 100f + 155f);
						moonColor.R = (byte)(num2 * 10f + 205f);
						moonColor.G = (byte)(num2 * 170f + 55f);
						moonColor.B = (byte)(num2 * 200f + 55f);
						bgColorToSet.R = (byte)(40f - num2 * 40f + 35f);
						bgColorToSet.G = (byte)(num2 * 20f + 15f);
						bgColorToSet.B = (byte)(num2 * 20f + 15f);
					}
				}
				else if (num < 16200.0) {
					float num2 = (float)(1.0 - num / 16200.0);
					moonColor.R = (byte)(num2 * 10f + 205f);
					moonColor.G = (byte)(num2 * 70f + 155f);
					moonColor.B = (byte)(num2 * 100f + 155f);
					bgColorToSet.R = (byte)(num2 * 30f + 5f);
					bgColorToSet.G = (byte)(num2 * 30f + 5f);
					bgColorToSet.B = (byte)(num2 * 30f + 5f);
				}
				else if (num >= 16200.0) {
					float num2 = (float)(((num / 32400.0) - 0.5) * 2.0);
					moonColor.R = (byte)((num2 * 50f) + 205f);
					moonColor.G = (byte)((num2 * 100f) + 155f);
					moonColor.B = (byte)((num2 * 100f) + 155f);
					bgColorToSet.R = (byte)((num2 * 20f) + 5f);
					bgColorToSet.G = (byte)((num2 * 30f) + 5f);
					bgColorToSet.B = (byte)((num2 * 30f) + 5f);
				}
				if (Main.dontStarveWorld) {
					DontStarveSeed.ModifyNightColor(ref bgColorToSet, ref moonColor);
				}
			}
			if (Main.cloudAlpha > 0f) {
				float num3 = 1f - (Main.cloudAlpha * 0.9f * Main.atmo);
				bgColorToSet.R = (byte)(bgColorToSet.R * num3);
				bgColorToSet.G = (byte)(bgColorToSet.G * num3);
				bgColorToSet.B = (byte)(bgColorToSet.B * num3);
			}
			if (info.GraveyardInfluence > 0f) {
				float num4 = 1f - (info.GraveyardInfluence * 0.6f);
				bgColorToSet.R = (byte)(bgColorToSet.R * num4);
				bgColorToSet.G = (byte)(bgColorToSet.G * num4);
				bgColorToSet.B = (byte)(bgColorToSet.B * num4);
			}
			if (isInGameMenuOrIsServer && !Main.dayTime) {
				bgColorToSet.R = 35;
				bgColorToSet.G = 35;
				bgColorToSet.B = 35;
			}
			if (info.CorruptionBiomeInfluence > 0f) {
				float num5 = info.CorruptionBiomeInfluence;
				if (num5 > 1f) {
					num5 = 1f;
				}
				int r = bgColorToSet.R;
				int g = bgColorToSet.G;
				int b = bgColorToSet.B;
				r -= (int)(90f * num5 * (bgColorToSet.R / 255f));
				g -= (int)(140f * num5 * (bgColorToSet.G / 255f));
				b -= (int)(70f * num5 * (bgColorToSet.B / 255f));
				if (r < 15) {
					r = 15;
				}
				if (g < 15) {
					g = 15;
				}
				if (b < 15) {
					b = 15;
				}
				DontStarveSeed.FixBiomeDarkness(ref bgColorToSet, ref r, ref g, ref b);
				bgColorToSet.R = (byte)r;
				bgColorToSet.G = (byte)g;
				bgColorToSet.B = (byte)b;
				r = sunColor.R;
				g = sunColor.G;
				b = sunColor.B;
				r -= (int)(100f * num5 * (sunColor.R / 255f));
				g -= (int)(100f * num5 * (sunColor.G / 255f));
				b -= (int)(0f * num5 * (sunColor.B / 255f));
				if (r < 15) {
					r = 15;
				}
				if (g < 15) {
					g = 15;
				}
				if (b < 15) {
					b = 15;
				}
				sunColor.R = (byte)r;
				sunColor.G = (byte)g;
				sunColor.B = (byte)b;
			}
			if (info.CrimsonBiomeInfluence > 0f) {
				float num6 = info.CrimsonBiomeInfluence;
				if (num6 > 1f) {
					num6 = 1f;
				}
				int r2 = bgColorToSet.R;
				int g2 = bgColorToSet.G;
				int b2 = bgColorToSet.B;
				r2 -= (int)(40f * num6 * (bgColorToSet.G / 255f));
				g2 -= (int)(110f * num6 * (bgColorToSet.G / 255f));
				b2 -= (int)(140f * num6 * (bgColorToSet.B / 255f));
				if (r2 < 15) {
					r2 = 15;
				}
				if (g2 < 15) {
					g2 = 15;
				}
				if (b2 < 15) {
					b2 = 15;
				}
				DontStarveSeed.FixBiomeDarkness(ref bgColorToSet, ref r2, ref g2, ref b2);
				bgColorToSet.R = (byte)r2;
				bgColorToSet.G = (byte)g2;
				bgColorToSet.B = (byte)b2;
				r2 = sunColor.R;
				g2 = sunColor.G;
				b2 = sunColor.B;
				g2 -= (int)(90f * num6 * (sunColor.G / 255f));
				b2 -= (int)(110f * num6 * (sunColor.B / 255f));
				if (r2 < 15) {
					r2 = 15;
				}
				if (g2 < 15) {
					g2 = 15;
				}
				if (b2 < 15) {
					b2 = 15;
				}
				sunColor.R = (byte)r2;
				sunColor.G = (byte)g2;
				sunColor.B = (byte)b2;
			}
			if (ConfTileInfo > 0f) {
				float num6 = ConfTileInfo;
				if (num6 > 1f) {
					num6 = 1f;
				}
				int r2 = bgColorToSet.R;
				int g2 = bgColorToSet.G;
				int b2 = bgColorToSet.B;
				g2 -= (int)(30f * num6 * (bgColorToSet.G / 255f));
				if (r2 < 15) {
					r2 = 15;
				}
				if (g2 < 15) {
					g2 = 15;
				}
				if (b2 < 15) {
					b2 = 15;
				}
				DontStarveSeed.FixBiomeDarkness(ref bgColorToSet, ref r2, ref g2, ref b2);
				bgColorToSet.R = (byte)r2;
				bgColorToSet.G = (byte)g2;
				bgColorToSet.B = (byte)b2;
				r2 = sunColor.R;
				g2 = sunColor.G;
				b2 = sunColor.B;
				g2 -= (int)(30f * num6 * (sunColor.G / 255f));
				if (r2 < 15) {
					r2 = 15;
				}
				if (g2 < 15) {
					g2 = 15;
				}
				if (b2 < 15) {
					b2 = 15;
				}
				sunColor.R = (byte)r2;
				sunColor.G = (byte)g2;
				sunColor.B = (byte)b2;
			}
			if (info.JungleBiomeInfluence > 0f) {
				float num7 = info.JungleBiomeInfluence;
				if (num7 > 1f) {
					num7 = 1f;
				}
				int r3 = bgColorToSet.R;
				int G = bgColorToSet.G;
				int b3 = bgColorToSet.B;
				r3 -= (int)(40f * num7 * (bgColorToSet.R / 255f));
				b3 -= (int)(70f * num7 * (bgColorToSet.B / 255f));
				if (G > 255) {
					G = 255;
				}
				if (G < 15) {
					G = 15;
				}
				if (r3 > 255) {
					r3 = 255;
				}
				if (r3 < 15) {
					r3 = 15;
				}
				if (b3 < 15) {
					b3 = 15;
				}
				DontStarveSeed.FixBiomeDarkness(ref bgColorToSet, ref r3, ref G, ref b3);
				bgColorToSet.R = (byte)r3;
				bgColorToSet.G = (byte)G;
				bgColorToSet.B = (byte)b3;
				r3 = sunColor.R;
				G = sunColor.G;
				b3 = sunColor.B;
				r3 -= (int)(30f * num7 * (sunColor.R / 255f));
				b3 -= (int)(10f * num7 * (sunColor.B / 255f));
				if (r3 < 15) {
					r3 = 15;
				}
				if (G < 15) {
					G = 15;
				}
				if (b3 < 15) {
					b3 = 15;
				}
				sunColor.R = (byte)r3;
				sunColor.G = (byte)G;
				sunColor.B = (byte)b3;
			}
			if (info.MushroomBiomeInfluence > 0f) {
				float mushroomBiomeInfluence = info.MushroomBiomeInfluence;
				int r4 = bgColorToSet.R;
				int g3 = bgColorToSet.G;
				int b4 = bgColorToSet.B;
				g3 -= (int)(250f * mushroomBiomeInfluence * (bgColorToSet.G / 255f));
				r4 -= (int)(250f * mushroomBiomeInfluence * (bgColorToSet.R / 255f));
				b4 -= (int)(250f * mushroomBiomeInfluence * (bgColorToSet.B / 255f));
				if (g3 < 15) {
					g3 = 15;
				}
				if (r4 < 15) {
					r4 = 15;
				}
				if (b4 < 15) {
					b4 = 15;
				}
				DontStarveSeed.FixBiomeDarkness(ref bgColorToSet, ref r4, ref g3, ref b4);
				bgColorToSet.R = (byte)r4;
				bgColorToSet.G = (byte)g3;
				bgColorToSet.B = (byte)b4;
				r4 = sunColor.R;
				g3 = sunColor.G;
				b4 = sunColor.B;
				g3 -= (int)(10f * mushroomBiomeInfluence * (sunColor.G / 255f));
				r4 -= (int)(30f * mushroomBiomeInfluence * (sunColor.R / 255f));
				b4 -= (int)(10f * mushroomBiomeInfluence * (sunColor.B / 255f));
				if (r4 < 15) {
					r4 = 15;
				}
				if (g3 < 15) {
					g3 = 15;
				}
				if (b4 < 15) {
					b4 = 15;
				}
				sunColor.R = (byte)r4;
				sunColor.G = (byte)g3;
				sunColor.B = (byte)b4;
				r4 = moonColor.R;
				g3 = moonColor.G;
				b4 = moonColor.B;
				g3 -= (int)(140f * mushroomBiomeInfluence * (moonColor.R / 255f));
				r4 -= (int)(170f * mushroomBiomeInfluence * (moonColor.G / 255f));
				b4 -= (int)(190f * mushroomBiomeInfluence * (moonColor.B / 255f));
				if (r4 < 15) {
					r4 = 15;
				}
				if (g3 < 15) {
					g3 = 15;
				}
				if (b4 < 15) {
					b4 = 15;
				}
				moonColor.R = (byte)r4;
				moonColor.G = (byte)g3;
				moonColor.B = (byte)b4;
			}
			byte minimalLight = 15;
			switch (Main.GetMoonPhase()) {
				case MoonPhase.Full:
					minimalLight = 19;
					break;
				case MoonPhase.ThreeQuartersAtLeft:
				case MoonPhase.ThreeQuartersAtRight:
					minimalLight = 17;
					break;
				case MoonPhase.HalfAtLeft:
				case MoonPhase.HalfAtRight:
					minimalLight = 15;
					break;
				case MoonPhase.QuarterAtLeft:
				case MoonPhase.QuarterAtRight:
					minimalLight = 13;
					break;
				case MoonPhase.Empty:
					minimalLight = 11;
					break;
			}
			if (Main.dontStarveWorld) {
				DontStarveSeed.ModifyMinimumLightColorAtNight(ref minimalLight);
			}
			if (bgColorToSet.R < minimalLight) {
				bgColorToSet.R = minimalLight;
			}
			if (bgColorToSet.G < minimalLight) {
				bgColorToSet.G = minimalLight;
			}
			if (bgColorToSet.B < minimalLight) {
				bgColorToSet.B = minimalLight;
			}
			if (info.BloodMoonActive) {
				if (bgColorToSet.R < 25) {
					bgColorToSet.R = 25;
				}
				if (bgColorToSet.G < 25) {
					bgColorToSet.G = 25;
				}
				if (bgColorToSet.B < 25) {
					bgColorToSet.B = 25;
				}
			}
			if (Main.eclipse && Main.dayTime) {
				const float num8 = 1242f;
				Main.eclipseLight = (float)(num / (double)num8);
				if (Main.eclipseLight > 1f) {
					Main.eclipseLight = 1f;
				}
			}
			else if (Main.eclipseLight > 0f) {
				Main.eclipseLight -= 0.01f;
				if (Main.eclipseLight < 0f) {
					Main.eclipseLight = 0f;
				}
			}
			if (Main.eclipseLight > 0f) {
				float num9 = 1f - (0.925f * Main.eclipseLight);
				float num10 = 1f - (0.96f * Main.eclipseLight);
				float num11 = 1f - (1f * Main.eclipseLight);
				int num12 = (int)(bgColorToSet.R * num9);
				int num13 = (int)(bgColorToSet.G * num10);
				int num14 = (int)(bgColorToSet.B * num11);
				bgColorToSet.R = (byte)num12;
				bgColorToSet.G = (byte)num13;
				bgColorToSet.B = (byte)num14;
				sunColor.R = byte.MaxValue;
				sunColor.G = 127;
				sunColor.B = 67;
				if (bgColorToSet.R < 20) {
					bgColorToSet.R = 20;
				}
				if (bgColorToSet.G < 10) {
					bgColorToSet.G = 10;
				}
				if (!Lighting.NotRetro) {
					if (bgColorToSet.R < 20) {
						bgColorToSet.R = 20;
					}
					if (bgColorToSet.G < 14) {
						bgColorToSet.G = 14;
					}
					if (bgColorToSet.B < 6) {
						bgColorToSet.B = 6;
					}
				}
			}
			if (Main.lightning > 0f) {
				float value = bgColorToSet.R / 255f;
				float value2 = bgColorToSet.G / 255f;
				float value3 = bgColorToSet.B / 255f;
				value = MathHelper.Lerp(value, 1f, Main.lightning);
				value2 = MathHelper.Lerp(value2, 1f, Main.lightning);
				value3 = MathHelper.Lerp(value3, 1f, Main.lightning);
				bgColorToSet.R = (byte)(value * 255f);
				bgColorToSet.G = (byte)(value2 * 255f);
				bgColorToSet.B = (byte)(value3 * 255f);
			}
			if (!info.BloodMoonActive) {
				moonColor = Color.White;
			}
			Main.ColorOfTheSkies = bgColorToSet;
		}

		public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts) {
			ConfTileCount = tileCounts[ModContent.TileType<CreamGrass>()] + tileCounts[ModContent.TileType<CreamGrassMowed>()] + tileCounts[ModContent.TileType<CreamGrass_Foliage>()] + tileCounts[ModContent.TileType<CreamVines>()] + tileCounts[ModContent.TileType<Creamstone>()] + tileCounts[ModContent.TileType<Creamsand>()] + tileCounts[ModContent.TileType<BlueIce>()] + tileCounts[ModContent.TileType<HardenedCreamsand>()] + tileCounts[ModContent.TileType<Creamsandstone>()];
			ConfTileCount += tileCounts[ModContent.TileType<CreamstoneAmethyst>()] + tileCounts[ModContent.TileType<CreamstoneTopaz>()] + tileCounts[ModContent.TileType<CreamstoneSaphire>()] + tileCounts[ModContent.TileType<CreamstoneEmerald>()] + tileCounts[ModContent.TileType<CreamstoneRuby>()] + tileCounts[ModContent.TileType<CreamstoneDiamond>()];
			ConfTileCount += tileCounts[ModContent.TileType<CookieBlock>()] + tileCounts[ModContent.TileType<CreamBlock>()];
<<<<<<< HEAD
<<<<<<< Updated upstream
            Main.SceneMetrics.EvilTileCount -= ConfTileCount;
            if (Main.SceneMetrics.EvilTileCount < 0)
                Main.SceneMetrics.EvilTileCount = 0;
            Main.SceneMetrics.BloodTileCount -= ConfTileCount;
            if (Main.SceneMetrics.BloodTileCount < 0)
                Main.SceneMetrics.BloodTileCount = 0;
        }
=======
			Main.SceneMetrics.EvilTileCount -= ConfTileCount;
			if (Main.SceneMetrics.EvilTileCount < 0)
				Main.SceneMetrics.EvilTileCount = 0;
			Main.SceneMetrics.BloodTileCount -= ConfTileCount;
			if (Main.SceneMetrics.BloodTileCount < 0)
				Main.SceneMetrics.BloodTileCount = 0;
		}
>>>>>>> 21d961fc5a6b7f1a1395f5a436ef383fb42b52eb

		public override void SaveWorldData(TagCompound tag) {
			tag[nameof(ConfectionSurfaceBG) + "2"] = ConfectionSurfaceBG;
			tag[nameof(Secret)] = Secret;
		}

		public override void LoadWorldData(TagCompound tag) {
			if (tag.ContainsKey(nameof(ConfectionSurfaceBG) + "2"))
				ConfectionSurfaceBG = tag.GetIntArray(nameof(ConfectionSurfaceBG) + "2");
			Secret = tag.ContainsKey(nameof(Secret)) && tag.GetBool(nameof(Secret));
		}

		public override void NetSend(BinaryWriter writer) {
			writer.Write(TheConfectionRebirth.bgVarAmount);
			for (int i = 0; i < TheConfectionRebirth.bgVarAmount; i++)
				writer.Write(ConfectionSurfaceBG[i]);
			writer.Write(Secret);
		}

<<<<<<< HEAD
        public override void NetReceive(BinaryReader reader)
        {
            int bgVar = reader.ReadInt32();
            ConfectionSurfaceBG = new int[TheConfectionRebirth.bgVarAmount] { -1, -1, -1, -1 };
            for (int i = 0; i < bgVar; i++)
                ConfectionSurfaceBG[i] = reader.ReadInt32();
            Secret = reader.ReadBoolean();
        }
    }
}
=======
			Main.SceneMetrics.EvilTileCount -= ConfTileCount;
			if (Main.SceneMetrics.EvilTileCount < 0)
				Main.SceneMetrics.EvilTileCount = 0;
			Main.SceneMetrics.BloodTileCount -= ConfTileCount;
			if (Main.SceneMetrics.BloodTileCount < 0)
				Main.SceneMetrics.BloodTileCount = 0;
		}
	}
}
>>>>>>> Stashed changes
=======
		public override void NetReceive(BinaryReader reader) {
			int bgVar = reader.ReadInt32();
			ConfectionSurfaceBG = new int[TheConfectionRebirth.bgVarAmount] { -1, -1, -1, -1 };
			for (int i = 0; i < bgVar; i++)
				ConfectionSurfaceBG[i] = reader.ReadInt32();
			Secret = reader.ReadBoolean();
		}
	}
}
>>>>>>> 21d961fc5a6b7f1a1395f5a436ef383fb42b52eb

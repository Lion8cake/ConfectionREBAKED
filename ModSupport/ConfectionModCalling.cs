using Terraria.ModLoader;
using Terraria.Achievements;
using System;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;

namespace TheConfectionRebirth.ModSupport
{
    public class ConfectionModCalling : ModSystem
    {
		public static readonly Mod? Achievements = ModLoader.TryGetMod("TMLAchievements", out Mod obtainedMod) ? obtainedMod : null;

		public static readonly Mod? FargoSeeds = ModLoader.TryGetMod("FargoSeeds", out Mod obtainedMod) ? obtainedMod : null;

		public static readonly Mod? BiomeLava = ModLoader.TryGetMod("BiomeLava", out Mod obtainedMod) ? obtainedMod : null;

		/// <summary>
		/// Checks if fargos best of both worlds is enabled
		/// </summary>
		public static bool FargoBoBW = false;

		/// <summary>
		/// Call this method to update the FargoBoBW bool before using it.
		/// </summary>
		public static void UpdateFargoBoBW()
		{
			FargoBoBW = ModContent.GetInstance<FargoSeedConfectionConfiguration>().BothGoods && FargoSeeds != null;
		}

		public override void PostSetupContent()
		{
            if (Achievements == null)
            {
                return;
            }

            //Achievements.Call("AddAchievement", ModContent.GetInstance<TheConfectionRebirth>(), "BirthdayRide", AchievementCategory.Challenger, "TheConfectionRebirth/Assets/BirthdayRide", null, false, false, 8f, new string[] { "Event_BirthdaySuitRollerCookieRide" });
            //Achievements.Call("AddAchievement", ModContent.GetInstance<TheConfectionRebirth>(), "DrixerMixer", AchievementCategory.Collector, "TheConfectionRebirth/Assets/DrixerMixer", null, false, false, 8f, new string[] { "Craft_" + ModContent.ItemType<Items.Weapons.Drixer>() });
			//Achievements.Call("AddAchievement", ModContent.GetInstance<TheConfectionRebirth>(), "TheBeamOfCream", AchievementCategory.Collector, "TheConfectionRebirth/Assets/TheBeamOfCream", null, false, false, 8f, new string[] { "Craft_" + ModContent.ItemType<Items.Weapons.CreamBeam>() });
		}

		public override void Load()
		{
			if (BiomeLava == null)
			{
				return; 
			}

			Mod mod = ModContent.GetInstance<TheConfectionRebirth>();

			string Name = "ChocolateLavaStyle";

			string Texture = mod.Name + "/ModSupport/BiomeLava/Assets/Confection/ChocoLava";

			string BlockTexture = Texture + "_Block";

			string SlopeTexture = Texture + "_Slope";

			string WaterfallTexture = Texture + "_Waterfall";

			Func<int, int, float, float, float, Vector3> ModifyLightDelegate = ModifyLight;

			Func<int> GetSplashDustDelegate = GetSplashDust;

			Func<int> GetDropletGoreDelegate = GetDropletGore;

			Func<bool> IsLavaActiveDelegate = IsLavaActive;

			Func<bool> lavafallGlowmaskDelegate = lavafallGlowmask;

			Func<Player, NPC, int, Action> InflictDebuffDelegate = InflictDebuff;

			Func<bool> InflictsOnFireDelegate = InflictsOnFire;

			BiomeLava.Call("ModLavaStyle", mod, Name, Texture, BlockTexture, SlopeTexture, WaterfallTexture, GetSplashDustDelegate, GetDropletGoreDelegate, ModifyLightDelegate, IsLavaActiveDelegate, lavafallGlowmaskDelegate, InflictDebuffDelegate, InflictsOnFireDelegate);

			static Vector3 ModifyLight(int i, int j, float r, float g, float b)
			{
				return new Vector3(0.6f, 0.3f, 0.2f);
			}

			static int GetSplashDust()
			{
				return ModContent.DustType<HotChocoBubble>();
			}

			static int GetDropletGore()
			{
				return ModContent.GoreType<HotChocoDroplet>();
			}

			static bool IsLavaActive()
			{
				return true;
			}

			static bool lavafallGlowmask()
			{
				return false;
			}

			static Action InflictDebuff(Player player, NPC npc, int onfireDuration)
			{
				return null;
			}

			static bool InflictsOnFire()
			{
				return true;
			}
		}
	}

	public class HotChocoBubble : ModDust
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ConfectionModCalling.BiomeLava != null;
		}

		public override string Texture => "TheConfectionRebirth/ModSupport/BiomeLava/Assets/Confection/ChocoLavaBubble";

		public override void OnSpawn(Dust dust)
		{
			dust.velocity *= 0.1f;
			dust.velocity.Y = -0.5f;
		}

		public override bool Update(Dust dust)
		{
			if (dust.scale > 10f)
			{
				dust.active = false;
			}
			Dust.lavaBubbles++;
			dust.position += dust.velocity;
			if (!dust.noGravity)
			{
				dust.velocity.Y += 0.1f;
			}
			if (dust.noGravity)
			{
				dust.scale += 0.03f;
				if (dust.scale < 1f)
				{
					dust.velocity.Y += 0.075f;
				}
				dust.velocity.X *= 1.08f;
				if (dust.velocity.X > 0f)
				{
					dust.rotation += 0.01f;
				}
				else
				{
					dust.rotation -= 0.01f;
				}
				float num109 = dust.scale * 0.6f;
				if (num109 > 1f)
				{
					num109 = 1f;
				}
				Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f + 1f), num109 * 0.6f, num109 * 0.3f, num109 * 0.2f);
			}
			else
			{
				if (!Collision.WetCollision(new Vector2(dust.position.X, dust.position.Y - 8f), 4, 4))
				{
					dust.scale = 0f;
				}
				else
				{
					dust.alpha += Main.rand.Next(2);
					if (dust.alpha > 255)
					{
						dust.scale = 0f;
					}
					dust.velocity.Y = -0.5f;
					dust.alpha++;
					dust.scale -= 0.01f;
					dust.velocity.Y = -0.2f;
					dust.velocity.X += (float)Main.rand.Next(-10, 10) * 0.002f;
					if ((double)dust.velocity.X < -0.25)
					{
						dust.velocity.X = -0.25f;
					}
					if ((double)dust.velocity.X > 0.25)
					{
						dust.velocity.X = 0.25f;
					}
				}
				float num3 = dust.scale * 0.3f + 0.4f;
				if (num3 > 1f)
				{
					num3 = 1f;
				}
				Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num3 * 0.6f, num3 * 0.3f, num3 * 0.2f);
			}
			dust.rotation += dust.velocity.X * 0.5f;
			if (dust.fadeIn > 0f && dust.fadeIn < 100f)
			{
				dust.scale += 0.03f;
				if (dust.scale > dust.fadeIn)
				{
					dust.fadeIn = 0f;
				}
			}
			dust.scale -= 0.01f;
			if (dust.noGravity)
			{
				dust.velocity *= 0.92f;
				if (dust.fadeIn == 0f)
				{
					dust.scale -= 0.04f;
				}
			}
			if (dust.position.Y > Main.screenPosition.Y + (float)Main.screenHeight)
			{
				dust.active = false;
			}
			float num17 = 0.1f;
			if ((double)Dust.dCount == 0.5)
			{
				dust.scale -= 0.001f;
			}
			if ((double)Dust.dCount == 0.6)
			{
				dust.scale -= 0.0025f;
			}
			if ((double)Dust.dCount == 0.7)
			{
				dust.scale -= 0.005f;
			}
			if ((double)Dust.dCount == 0.8)
			{
				dust.scale -= 0.01f;
			}
			if ((double)Dust.dCount == 0.9)
			{
				dust.scale -= 0.02f;
			}
			if ((double)Dust.dCount == 0.5)
			{
				num17 = 0.11f;
			}
			if ((double)Dust.dCount == 0.6)
			{
				num17 = 0.13f;
			}
			if ((double)Dust.dCount == 0.7)
			{
				num17 = 0.16f;
			}
			if ((double)Dust.dCount == 0.8)
			{
				num17 = 0.22f;
			}
			if ((double)Dust.dCount == 0.9)
			{
				num17 = 0.25f;
			}
			if (dust.scale < num17)
			{
				dust.active = false;
			}
			return false;
		}


		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			float num = (float)(255 - dust.alpha) / 255f;
			int num4;
			int num3;
			int num2;
			num = (num + 3f) / 4f;
			num4 = (int)((int)lightColor.R * num);
			num3 = (int)((int)lightColor.G * num);
			num2 = (int)((int)lightColor.B * num);
			int num6 = lightColor.A - dust.alpha;
			if (num6 < 0)
			{
				num6 = 0;
			}
			if (num6 > 255)
			{
				num6 = 255;
			}
			return new Color(num4, num3, num2, num6);
		}
	}

	public abstract class HotChocoDroplet : ModGore
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ConfectionModCalling.BiomeLava != null;
		}

		public override string Texture => "TheConfectionRebirth/ModSupport/BiomeLava/Assets/Confection/ChocoLavaDrip";

		public override void OnSpawn(Gore gore, IEntitySource source)
		{
			gore.numFrames = 15;
			gore.behindTiles = true;
			gore.timeLeft = Gore.goreTime * 3;
		}

		public override bool Update(Gore gore)
		{
			gore.alpha = gore.position.Y < (Main.worldSurface * 16.0) + 8.0
				? 0
				: 100;

			int frameDuration = 4;
			gore.frameCounter += 1;
			if (gore.frame <= 4)
			{
				int tileX = (int)(gore.position.X / 16f);
				int tileY = (int)(gore.position.Y / 16f) - 1;
				if (WorldGen.InWorld(tileX, tileY) && !Main.tile[tileX, tileY].HasTile)
				{
					gore.active = false;
				}

				if (gore.frame == 0 || gore.frame == 1 || gore.frame == 2)
				{
					frameDuration = 24 + Main.rand.Next(256);
				}

				if (gore.frame == 3)
				{
					frameDuration = 24 + Main.rand.Next(96);
				}

				if (gore.frameCounter >= frameDuration)
				{
					gore.frameCounter = 0;
					gore.frame += 1;
					if (gore.frame == 5)
					{
						int droplet = Gore.NewGore(new EntitySource_Misc(nameof(HotChocoDroplet)), gore.position, gore.velocity, gore.type);
						Main.gore[droplet].frame = 9;
						Main.gore[droplet].velocity *= 0f;
					}
				}
			}
			else if (gore.frame <= 6)
			{
				frameDuration = 8;
				if (gore.frameCounter >= frameDuration)
				{
					gore.frameCounter = 0;
					gore.frame += 1;
					if (gore.frame == 7)
					{
						gore.active = false;
					}
				}
			}
			else if (gore.frame <= 9)
			{
				frameDuration = 6;
				gore.velocity.Y += 0.2f;
				if (gore.velocity.Y < 0.5f)
				{
					gore.velocity.Y = 0.5f;
				}

				if (gore.velocity.Y > 12f)
				{
					gore.velocity.Y = 12f;
				}

				if (gore.frameCounter >= frameDuration)
				{
					gore.frameCounter = 0;
					gore.frame += 1;
				}

				if (gore.frame > 9)
				{
					gore.frame = 7;
				}
			}
			else
			{
				gore.velocity.Y += 0.1f;
				if (gore.frameCounter >= frameDuration)
				{
					gore.frameCounter = 0;
					gore.frame += 1;
				}

				gore.velocity *= 0f;
				if (gore.frame > 14)
				{
					gore.active = false;
				}
			}
			float num24 = 1f;
			float num25 = 1f;
			float num26 = 1f;
			float num27 = 0.6f;
			num27 = ((gore.frame == 0) ? (num27 * 0.1f) : ((gore.frame == 1) ? (num27 * 0.2f) : ((gore.frame == 2) ? (num27 * 0.3f) : ((gore.frame == 3) ? (num27 * 0.4f) : ((gore.frame == 4) ? (num27 * 0.5f) : ((gore.frame == 5) ? (num27 * 0.4f) : ((gore.frame == 6) ? (num27 * 0.2f) : ((gore.frame <= 9) ? (num27 * 0.5f) : ((gore.frame == 10) ? (num27 * 0.5f) : ((gore.frame == 11) ? (num27 * 0.4f) : ((gore.frame == 12) ? (num27 * 0.3f) : ((gore.frame == 13) ? (num27 * 0.2f) : ((gore.frame != 14) ? 0f : (num27 * 0.1f))))))))))))));
			num24 = 0.6f * num27; //R
			num25 = 0.3f * num27; //G
			num26 = 0.2f * num27; //B
			Lighting.AddLight(gore.position + new Vector2(8f, 8f), num24, num25, num26);

			Vector2 oldVelocity = gore.velocity;
			gore.velocity = Collision.TileCollision(gore.position, gore.velocity, 16, 14);
			if (gore.velocity != oldVelocity)
			{
				if (gore.frame < 10)
				{
					gore.frame = 10;
					gore.frameCounter = 0;
				}
			}
			else if (Collision.WetCollision(gore.position + gore.velocity, 16, 14))
			{
				if (gore.frame < 10)
				{
					gore.frame = 10;
					gore.frameCounter = 0;
				}

				int tileX = (int)(gore.position.X + 8f) / 16;
				int tileY = (int)(gore.position.Y + 14f) / 16;
				if (Main.tile[tileX, tileY] != null && Main.tile[tileX, tileY].LiquidAmount > 0)
				{
					gore.velocity *= 0f;
					gore.position.Y = (tileY * 16) - (Main.tile[tileX, tileY].LiquidAmount / 16);
				}
			}

			gore.position += gore.velocity;
			return false;
		}

		public override Color? GetAlpha(Gore gore, Color lightColor)
		{
			return new Color(255, 255, 255, 200);
		}
	}
}

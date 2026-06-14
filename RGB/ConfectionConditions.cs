using ReLogic.Peripherals.RGB;
using System;
using Terraria;
using Terraria.GameContent.RGB;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;

namespace TheConfectionRebirth.RGB
{
	public static class ConfectionConditions 
	{
		private class SimpleCondition : ChromaCondition
		{
			private Func<bool> _condition;

			public SimpleCondition(Func<bool> condition) => _condition = condition;

			public override bool IsActive()
			{
				return _condition();
			}
		}

		private class SceneCondition : SimpleCondition
		{
			public SceneCondition(Func<SceneMetrics, bool> condition)
				: base(() => condition(Main.SceneMetrics))
			{
			}
		}

		public static class SurfaceBiome
		{
			public static readonly ChromaCondition Confection = new SceneCondition((SceneMetrics scene) => !Main.gameMenu && ModLoader.HasMod(nameof(TheConfectionRebirth)) && ConfectionBiome.InModBiome(Main.LocalPlayer) && scene.ZoneOverworldHeight);
		}

		public static class UndergroundBiome
		{
			public static readonly ChromaCondition ConfectionIce = new SceneCondition((SceneMetrics scene) => !Main.gameMenu && ModLoader.HasMod(nameof(TheConfectionRebirth)) && scene.ZoneSnow && ConfectionBiome.InModBiome(Main.LocalPlayer));

			public static readonly ChromaCondition Confection = new SceneCondition((SceneMetrics scene) => !Main.gameMenu && ModLoader.HasMod(nameof(TheConfectionRebirth)) && ConfectionBiome.InModBiome(Main.LocalPlayer) && !scene.ZoneOverworldHeight);

			public static readonly ChromaCondition ConfectionDesert = new SceneCondition((SceneMetrics scene) => !Main.gameMenu && ModLoader.HasMod(nameof(TheConfectionRebirth)) && scene.ZoneDesert && ConfectionBiome.InModBiome(Main.LocalPlayer));
		}

		public static class Boss
		{
		}

		public static readonly ChromaCondition InConfectionMenu = new SimpleCondition(() => Main.gameMenu && !ConfectionReflectionUtilities.GetIsLoading() && ModLoader.HasMod(nameof(TheConfectionRebirth)) && ModContent.GetInstance<ConfectionMenu>().IsSelected && !Main.drunkWorld);
	}
}

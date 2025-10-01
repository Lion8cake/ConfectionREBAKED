using ReLogic.Peripherals.RGB;
using System;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using static Terraria.GameContent.RGB.CommonConditions;

namespace TheConfectionRebirth.RGB
{
	public static class ConfectionConditions 
	{
		private class SimpleCondition : ConditionBase
		{
			private Func<Player, bool> _condition;

			public SimpleCondition(Func<Player, bool> condition)
			{
				_condition = condition;
			}

			public override bool IsActive()
			{
				return _condition(CurrentPlayer);
			}
		}

		public static class SurfaceBiome
		{
			public static readonly ChromaCondition Confection = new SimpleCondition((Player player) => !Main.gameMenu && ModLoader.HasMod(nameof(TheConfectionRebirth)) && ConfectionBiome.InModBiome(player) && player.ZoneOverworldHeight);
		}

		public static class UndergroundBiome
		{
			public static readonly ChromaCondition ConfectionIce = new SimpleCondition((Player player) => !Main.gameMenu && ModLoader.HasMod(nameof(TheConfectionRebirth)) && InIce(player) && ConfectionBiome.InModBiome(player));

			public static readonly ChromaCondition Confection = new SimpleCondition((Player player) => !Main.gameMenu && ModLoader.HasMod(nameof(TheConfectionRebirth)) && ConfectionBiome.InModBiome(player) && !player.ZoneOverworldHeight);

			public static readonly ChromaCondition ConfectionDesert = new SimpleCondition((Player player) => !Main.gameMenu && ModLoader.HasMod(nameof(TheConfectionRebirth)) && InDesert(player) && ConfectionBiome.InModBiome(player));

			private static bool InIce(Player player)
			{
				if (player.ZoneSnow)
				{
					return !player.ZoneOverworldHeight;
				}
				return false;
			}

			private static bool InDesert(Player player)
			{
				if (player.ZoneDesert)
				{
					return !player.ZoneOverworldHeight;
				}
				return false;
			}
		}

		public static class Boss
		{
		}

		public static readonly ChromaCondition InConfectionMenu = new SimpleCondition((Player player) => Main.gameMenu && !ConfectionReflectionUtilities.GetIsLoading() && ModLoader.HasMod(nameof(TheConfectionRebirth)) && ModContent.GetInstance<ConfectionMenu>().IsSelected && !Main.drunkWorld);
	}
}

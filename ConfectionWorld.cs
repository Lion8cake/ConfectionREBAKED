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
using TheConfectionRebirth.Items.Weapons;
using Terraria.ID;
using Terraria.Utilities;
using System.Linq;
using Terraria.Localization;

namespace TheConfectionRebirth {
	public class ConfectionWorld : ModSystem {

		public static RecipeGroup SoulofLightRecipeGroup;
		public static RecipeGroup SoulofNightRecipeGroup;
		public static RecipeGroup DarkShardRecipeGroup;
		public static RecipeGroup PixieDustRecipeGroup;
		public static RecipeGroup UnicornHornRecipeGroup;
		public static RecipeGroup CrystalShardRecipeGroup;
		public static RecipeGroup HallowedBarRecipeGroup;
		public static RecipeGroup PrincessFishRecipeGroup;
		public static RecipeGroup PrismiteRecipeGroup;
		public static RecipeGroup ChaosFishRecipeGroup;
		public static RecipeGroup HallowedSeedsRecipeGroup;
		public static RecipeGroup PearlstoneRecipeGroup;

		public override void Unload() {
			SoulofLightRecipeGroup = null;
			SoulofNightRecipeGroup = null;
			DarkShardRecipeGroup = null;
			PixieDustRecipeGroup = null;
			UnicornHornRecipeGroup = null;
			CrystalShardRecipeGroup = null;
			HallowedBarRecipeGroup = null;
			PrincessFishRecipeGroup = null;
			PrismiteRecipeGroup = null;
			ChaosFishRecipeGroup = null;
			HallowedSeedsRecipeGroup = null;
			PearlstoneRecipeGroup = null;
		}

		//Recipes, not making a new file for this like last time
		public override void AddRecipes() {
			Recipe recipe = Recipe.Create(ItemID.TerraBlade);
			recipe.AddIngredient(ItemID.TrueNightsEdge)
			.AddIngredient(ModContent.ItemType<TrueSucrosa>())
			.AddIngredient(ItemID.BrokenHeroSword)
			.AddTile(TileID.MythrilAnvil)
			.Register();
			Recipe recipe2 = Recipe.Create(ItemID.TerraBlade);
			recipe2.AddIngredient(ModContent.ItemType<TrueDeathsRaze>())
			.AddIngredient(ItemID.TrueExcalibur)
			.AddIngredient(ItemID.BrokenHeroSword)
			.AddTile(TileID.MythrilAnvil)
			.Register();
			Recipe recipe3 = Recipe.Create(ItemID.TerraBlade);
			recipe3.AddIngredient(ModContent.ItemType<TrueDeathsRaze>())
			.AddIngredient(ModContent.ItemType<TrueSucrosa>())
			.AddIngredient(ItemID.BrokenHeroSword)
			.AddTile(TileID.MythrilAnvil)
			.Register();
		}

		public override void AddRecipeGroups() {
			RecipeGroup.recipeGroups[RecipeGroupID.Wood].ValidItems.Add(ModContent.ItemType<Items.Placeable.CreamWood>());
			RecipeGroup.recipeGroups[RecipeGroupID.Fruit].ValidItems.Add(ModContent.ItemType<Items.Cherimoya>());
			RecipeGroup.recipeGroups[RecipeGroupID.Sand].ValidItems.Add(ModContent.ItemType<Items.Placeable.Creamsand>());
			RecipeGroup.recipeGroups[RecipeGroupID.Sand].ValidItems.Add(ModContent.ItemType<Items.Placeable.HardenedCreamsand>());

			SoulofLightRecipeGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.SoulofLight)}", ItemID.SoulofLight, ModContent.ItemType<Items.SoulofDelight>());
			RecipeGroup.RegisterGroup("SoulofLight", SoulofLightRecipeGroup);
			SoulofNightRecipeGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.SoulofNight)}", ItemID.SoulofNight, ModContent.ItemType<Items.SoulofSpite>());
			RecipeGroup.RegisterGroup("SoulofNight", SoulofNightRecipeGroup);
			DarkShardRecipeGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.DarkShard)}", ItemID.DarkShard, ModContent.ItemType<Items.CanofMeat>());
			RecipeGroup.RegisterGroup("DarkShard", DarkShardRecipeGroup);
			PixieDustRecipeGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.PixieDust)}", ItemID.PixieDust, ModContent.ItemType<Items.Sprinkles>());
			RecipeGroup.RegisterGroup("PixieDust", PixieDustRecipeGroup);
			UnicornHornRecipeGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.UnicornHorn)}", ItemID.UnicornHorn, ModContent.ItemType<Items.CookieDough>());
			RecipeGroup.RegisterGroup("UnicornHorn", UnicornHornRecipeGroup);
			CrystalShardRecipeGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.CrystalShard)}", ItemID.CrystalShard, ModContent.ItemType<Items.Placeable.Saccharite>());
			RecipeGroup.RegisterGroup("CrystalShard", CrystalShardRecipeGroup);
			CrystalShardRecipeGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.CrystalShard)}", ItemID.CrystalShard, ModContent.ItemType<Items.Placeable.Saccharite>());
			RecipeGroup.RegisterGroup("CrystalShard", CrystalShardRecipeGroup);
			HallowedBarRecipeGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.HallowedBar)}", ItemID.HallowedBar, ModContent.ItemType<Items.Placeable.NeapoliniteBar>());
			RecipeGroup.RegisterGroup("HallowedBar", HallowedBarRecipeGroup);
			PrincessFishRecipeGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.PrincessFish)}", ItemID.PrincessFish, ModContent.ItemType<Items.CookieCarp>());
			RecipeGroup.RegisterGroup("PrincessFish", PrincessFishRecipeGroup);
			PrismiteRecipeGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.Prismite)}", ItemID.Prismite, ModContent.ItemType<Items.Cakekite>());
			RecipeGroup.RegisterGroup("Prismite", PrismiteRecipeGroup);
			ChaosFishRecipeGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.ChaosFish)}", ItemID.ChaosFish, ModContent.ItemType<Items.SugarFish>());
			RecipeGroup.RegisterGroup("ChaosFish", ChaosFishRecipeGroup);
			HallowedSeedsRecipeGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.HallowedSeeds)}", ItemID.HallowedSeeds, ModContent.ItemType<Items.Placeable.CreamBeans>());
			RecipeGroup.RegisterGroup("HallowedSeeds", HallowedSeedsRecipeGroup);
			PearlstoneRecipeGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.PearlstoneBlock)}", ItemID.PearlstoneBlock, ModContent.ItemType<Items.Placeable.Creamstone>());
			RecipeGroup.RegisterGroup("PearlstoneBlock", PearlstoneRecipeGroup);
		}

		public override void PostAddRecipes() {
			for (int i = 0; i < Recipe.numRecipes; i++) {
				Recipe recipe = Main.recipe[i];
				if (recipe.HasResult(ItemID.NightsEdge) && recipe.HasIngredient(ItemID.BloodButcherer)) {
					recipe.ReplaceResult(ModContent.ItemType<DeathsRaze>());
				}
				if (recipe.HasResult(ItemID.GoldenShower) && recipe.HasIngredient(ItemID.SoulofNight)) {
					recipe.RemoveIngredient(ItemID.SoulofNight);
					recipe.AddIngredient(ModContent.ItemType<Items.SoulofSpite>(), 15);
				}
				if (recipe.HasResult(4142) && recipe.HasIngredient(ItemID.SoulofNight)) {
					recipe.RemoveIngredient(ItemID.SoulofNight);
					recipe.AddIngredient(ModContent.ItemType<Items.SoulofSpite>(), 10);
				}
				if (recipe.HasResult(ItemID.MechanicalWorm) && recipe.HasIngredient(ItemID.SoulofNight) && recipe.HasIngredient(ItemID.Vertebrae)) {
					recipe.RemoveIngredient(ItemID.SoulofNight);
					recipe.AddIngredient(ModContent.ItemType<Items.SoulofSpite>(), 6);
				}
				if (recipe.HasResult(ItemID.Megashark)) {
					recipe.AddIngredient(ItemID.SoulofLight, 15);
				}
				if (recipe.TryGetIngredient(ItemID.SoulofLight, out var SoL) && !ConfectionIDs.Sets.RecipeBlacklist.SoulofLightOnlyItem[recipe.createItem.type]) {
					recipe.AddRecipeGroup("SoulofLight", SoL.stack);
					recipe.RemoveIngredient(SoL);
				}
				if (recipe.TryGetIngredient(ItemID.SoulofNight, out var SoN) && !ConfectionIDs.Sets.RecipeBlacklist.SoulofNightOnlyItem[recipe.createItem.type]) {
					recipe.AddRecipeGroup("SoulofNight", SoN.stack);
					recipe.RemoveIngredient(SoN);
				}
				if (recipe.TryGetIngredient(ItemID.DarkShard, out var DS) && !ConfectionIDs.Sets.RecipeBlacklist.DarkShardOnlyItem[recipe.createItem.type]) {
					recipe.AddRecipeGroup("DarkShard", DS.stack);
					recipe.RemoveIngredient(DS);
				}
				if (recipe.TryGetIngredient(ItemID.PixieDust, out var PD) && !ConfectionIDs.Sets.RecipeBlacklist.PixieDustOnlyItem[recipe.createItem.type]) {
					recipe.AddRecipeGroup("PixieDust", PD.stack);
					recipe.RemoveIngredient(PD);
				}
				if (recipe.TryGetIngredient(ItemID.UnicornHorn, out var UH) && !ConfectionIDs.Sets.RecipeBlacklist.UnicornHornOnlyItem[recipe.createItem.type]) {
					recipe.AddRecipeGroup("UnicornHorn", UH.stack);
					recipe.RemoveIngredient(UH);
				}
				if (recipe.TryGetIngredient(ItemID.CrystalShard, out var CS) && !ConfectionIDs.Sets.RecipeBlacklist.CrystalShardOnlyItem[recipe.createItem.type]) {
					recipe.AddRecipeGroup("CrystalShard", CS.stack);
					recipe.RemoveIngredient(CS);
				}
				if (recipe.TryGetIngredient(ItemID.HallowedBar, out var HB) && !ConfectionIDs.Sets.RecipeBlacklist.HallowedBarOnlyItem[recipe.createItem.type]) {
					recipe.AddRecipeGroup("HallowedBar", HB.stack);
					recipe.RemoveIngredient(HB);
				}
				if (recipe.TryGetIngredient(ItemID.PrincessFish, out var PF) && !ConfectionIDs.Sets.RecipeBlacklist.PrincessFishOnlyItem[recipe.createItem.type]) {
					recipe.AddRecipeGroup("PrincessFish", PF.stack);
					recipe.RemoveIngredient(PF);
				}
				if (recipe.TryGetIngredient(ItemID.Prismite, out var Prismite) && !ConfectionIDs.Sets.RecipeBlacklist.PrismiteOnlyItem[recipe.createItem.type]) {
					recipe.AddRecipeGroup("Prismite", Prismite.stack);
					recipe.RemoveIngredient(Prismite);
				}
				if (recipe.TryGetIngredient(ItemID.ChaosFish, out var CF) && !ConfectionIDs.Sets.RecipeBlacklist.ChaosFishOnlyItem[recipe.createItem.type]) {
					recipe.AddRecipeGroup("ChaosFish", CF.stack);
					recipe.RemoveIngredient(CF);
				}
				if (recipe.TryGetIngredient(ItemID.HallowedSeeds, out var HS) && !ConfectionIDs.Sets.RecipeBlacklist.HallowedSeedsOnlyItem[recipe.createItem.type]) {
					recipe.AddRecipeGroup("HallowedSeeds", HS.stack);
					recipe.RemoveIngredient(HS);
				}
				if (recipe.TryGetIngredient(ItemID.PearlstoneBlock, out var Pearlstone) && !ConfectionIDs.Sets.RecipeBlacklist.PearlstoneOnlyItem[recipe.createItem.type]) {
					recipe.AddRecipeGroup("PearlstoneBlock", Pearlstone.stack);
					recipe.RemoveIngredient(Pearlstone);
				}
			}
		}

		public static int ConfTileCount { get; set; }
		public static float ConfTileInfo => ConfTileCount / 100;
		public static bool IsEaster => DateTime.Now.Day >= 2 && DateTime.Now.Day <= 24 && DateTime.Now.Month.Equals(4);
		public static float DifficultyScale {
			get {
				float d = Main.expertMode ? 2f : (Main.masterMode ? 3f : 1f);
				if (Main.expertMode)
					d = CreativePowerManager.Instance.GetPower<CreativePowers.DifficultySliderPower>().StrengthMultiplierToGiveNPCs;
				return d;
			}
		}

		public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor) {
			if (ConfTileInfo > 0f) {
				backgroundColor.R = (byte)Math.Min((int)(200f * 0.5f * (backgroundColor.R / (255f - 200f))), 255f);
				backgroundColor.G = (byte)Math.Min((int)(85f * 0.5f * (backgroundColor.G / (255f - 200f))), 255f);
				backgroundColor.B = (byte)Math.Min((int)(135f * 0.5f * (backgroundColor.B / (255f - 200f))), 255f);
			}
		}

		public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts) {
			ConfTileCount = tileCounts[ModContent.TileType<CreamGrass>()] + tileCounts[ModContent.TileType<CreamGrassMowed>()] + tileCounts[ModContent.TileType<CreamGrass_Foliage>()] + tileCounts[ModContent.TileType<CreamVines>()] + tileCounts[ModContent.TileType<Creamstone>()] + tileCounts[ModContent.TileType<Creamsand>()] + tileCounts[ModContent.TileType<BlueIce>()] + tileCounts[ModContent.TileType<HardenedCreamsand>()] + tileCounts[ModContent.TileType<Creamsandstone>()];
			ConfTileCount += tileCounts[ModContent.TileType<CreamstoneAmethyst>()] + tileCounts[ModContent.TileType<CreamstoneTopaz>()] + tileCounts[ModContent.TileType<CreamstoneSaphire>()] + tileCounts[ModContent.TileType<CreamstoneEmerald>()] + tileCounts[ModContent.TileType<CreamstoneRuby>()] + tileCounts[ModContent.TileType<CreamstoneDiamond>()];
			ConfTileCount += tileCounts[ModContent.TileType<CookieBlock>()] + tileCounts[ModContent.TileType<CreamBlock>()];
            Main.SceneMetrics.EvilTileCount -= ConfTileCount;
            if (Main.SceneMetrics.EvilTileCount < 0)
                Main.SceneMetrics.EvilTileCount = 0;
            Main.SceneMetrics.BloodTileCount -= ConfTileCount;
            if (Main.SceneMetrics.BloodTileCount < 0)
                Main.SceneMetrics.BloodTileCount = 0;
			Main.SceneMetrics.SnowTileCount += tileCounts[ModContent.TileType<CreamBlock>()]
				+ tileCounts[ModContent.TileType<BlueIce>()];

			Main.SceneMetrics.SandTileCount += tileCounts[ModContent.TileType<Creamsand>()]
				+ tileCounts[ModContent.TileType<HardenedCreamsand>()]
				+ tileCounts[ModContent.TileType<Creamsandstone>()];
		}
	}
}

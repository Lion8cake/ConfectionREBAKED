using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Tiles;
using Terraria.ID;
using MonoMod.Cil;
using System.Reflection;
using Terraria.Utilities;
using Microsoft.Xna.Framework;
using Terraria.Map;
using TheConfectionRebirth.Tiles.Trees;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Drawing;
using Terraria.GameContent;
using Mono.Cecil.Cil;
using static Terraria.WaterfallManager;
using Terraria.GameContent.Events;
using Terraria.DataStructures;
using TheConfectionRebirth.Walls;
using TheConfectionRebirth.Hooks;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using ReLogic.Content;
using Terraria.GameContent.UI.States;
using Terraria.IO;
using Terraria.Localization;
using static TheConfectionRebirth.NPCs.ConfectionGlobalNPC;
using Terraria.GameContent.ItemDropRules;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Projectiles;
using Terraria.Graphics;
using TheConfectionRebirth.Items.Weapons;
using static Terraria.Graphics.FinalFractalHelper;

namespace TheConfectionRebirth
{
	public class TheConfectionRebirth : Mod
	{
		//Edit the following
		//WorldGen.cs
		//PlantCheck (done) (i think) - crimson mushrooms dont convert to yumdrops and vise versa for some dogshit reason - doesnt convert some purity grass correctly
		//TileFrame - Vines dont properly convert - works only if fps is lower than 30 //Im not the only one having this issue it seems //Update, issue with tml and the TileFrame method being too big

		private Asset<Texture2D> texOuterHallow;
		private Asset<Texture2D> texOuterConfection;

		private bool[] ZenithSeedWorlds;

		public override void Load() {
			ConfectionWindUtilities.Load();

			if (Main.netMode != NetmodeID.Server)
			{
				texOuterHallow = Assets.Request<Texture2D>("Assets/Loading/Outer_Hallow");
				texOuterConfection = Assets.Request<Texture2D>("Assets/Loading/Outer_Confection");
			}

			var fractalProfiles = (Dictionary<int, FinalFractalProfile>)typeof(FinalFractalHelper).GetField("_fractalProfiles", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
			fractalProfiles.Add(ModContent.ItemType<TrueSucrosa>(), new FinalFractalProfile(70f, new Color(224, 92, 165))); //Add the True Sucrosa with a pink trail
			fractalProfiles.Add(ModContent.ItemType<Sucrosa>(), new FinalFractalProfile(70f, new Color(224, 92, 165))); //Add the Sucrosa with a pink trail

			On_Player.PlaceThing_Tiles_PlaceIt_KillGrassForSolids += KillConjoinedGrass_PlaceThing;
			On_Player.DoesPickTargetTransformOnKill += PickaxeKillTile;
			IL_Liquid.DelWater += BurnGrass;
			IL_WorldGen.PlantCheck += PlantTileFrameIL;
			On_Player.DoBootsEffect_PlaceFlowersOnTile += FlowerBootsEdit;
			On_WorldGen.IsFitToPlaceFlowerIn += Flowerplacement;
			On_WorldGen.PlaceTile += PlaceTile;
			IL_WorldGen.TileFrame += VineTileFrame;
			IL_MapHelper.CreateMapTile += CactusMapColor;
			On_WorldGen.PlaceLilyPad += LilyPadPreventer;
			IL_WorldGen.CheckCatTail += CheckCattailEdit;
			IL_WorldGen.PlaceCatTail += PlaceCattailEdit;
			On_WorldGen.GrowCatTail += GrowCattailEdit;
			IL_WorldGen.CheckLilyPad += CheckLilyPadEdit;
			IL_WorldGen.PlaceLilyPad += PlaceLilyPadEdit;
			On_TileDrawing.DrawSingleTile += LilyPadDrawingPreventer;
			On_Liquid.DelWater += LilyPadCheck;
			On_Main.DrawTileInWater += LilyPadDrawing;
			IL_WorldGen.PlantSeaOat += PlantSeaOatEdit;
			IL_WorldGen.PlaceOasisPlant += PlaceOasisPlant;
			On_TileDrawing.DrawMultiTileGrassInWind += MultiTileGrassDetour;
			On_WorldGen.PlaceOasisPlant += PlantOasisPlantEdit;
			On_Player.MowGrassTile += LAWWWWNNNNMOOOWWWWWWAAAAAA;
			On_SmartCursorHelper.Step_LawnMower += SMARTLAWWWWWWNNNNNMOWWWWAAAAASSSSS;
			IL_NPC.SpawnNPC += LawnSpawnPrevention;
			On_SmartCursorHelper.Step_GrassSeeds += CreamBeansSmartCursor;
			IL_WaterfallManager.FindWaterfalls += CloudWaterfalls;
			On_TileDrawing.DrawMultiTileVinesInWind += On_TileDrawing_DrawMultiTileVinesInWind;
			IL_Sandstorm.EmitDust += CreamsandSandstorm;
			On_WorldGen.Convert += Convert;
			IL_Player.Update += TileFallDamage;
			On_Player.PlaceThing_PaintScrapper_LongMoss += MossScapper;

			IL_UIWorldCreation.BuildPage += ConfectionSelectionMenu.ILBuildPage;
			IL_UIWorldCreation.MakeInfoMenu += ConfectionSelectionMenu.ILMakeInfoMenu;
			IL_UIWorldCreation.ShowOptionDescription += ConfectionSelectionMenu.ILShowOptionDescription;
			On_UIWorldCreation.SetDefaultOptions += ConfectionSelectionMenu.OnSetDefaultOptions;
			IL_UIWorldCreation.SetupGamepadPoints += ConfectionSelectionMenu.ILSetUpGamepadPoints;

			IL_UIGenProgressBar.DrawSelf += AddGoodToWorldgenBar;
			On_UIWorldListItem.DrawSelf += ConfectionWorldIconEdit;
			IL_Lang.GetDryadWorldStatusDialog += DryadWorldStatusEdit;
			IL_WorldGen.AddUpAlignmentCounts += AddUpAligmenttmodEvilsandGoods;
			IL_WorldGen.CountTiles += SettmodvilsandGoods;
			On_ItemDropDatabase.RegisterBoss_Twins += On_ItemDropDatabase_RegisterBoss_Twins;
			On_Main.DrawMapFullscreenBackground += On_Main_DrawMapFullscreenBackground;
			IL_Main.SetBackColor += ConfectionBiomeLightColor;
			IL_Projectile.Damage += CosmicCookieReflection;
			On_Projectile.CanBeReflected += CosmicCookieCanBeReflect;
		}

		public override void Unload() {
			ConfectionWindUtilities.Unload();

			var fractalProfiles = (Dictionary<int, FinalFractalProfile>)typeof(FinalFractalHelper).GetField("_fractalProfiles", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
			fractalProfiles.Remove(ModContent.ItemType<TrueSucrosa>());
			fractalProfiles.Remove(ModContent.ItemType<Sucrosa>());


			On_Player.PlaceThing_Tiles_PlaceIt_KillGrassForSolids -= KillConjoinedGrass_PlaceThing;
			On_Player.DoesPickTargetTransformOnKill -= PickaxeKillTile;
			IL_Liquid.DelWater -= BurnGrass;
			IL_WorldGen.PlantCheck -= PlantTileFrameIL;
			On_Player.DoBootsEffect_PlaceFlowersOnTile -= FlowerBootsEdit;
			On_WorldGen.IsFitToPlaceFlowerIn -= Flowerplacement;
			On_WorldGen.PlaceTile -= PlaceTile;
			IL_WorldGen.TileFrame -= VineTileFrame;
			IL_MapHelper.CreateMapTile -= CactusMapColor;
			On_WorldGen.PlaceLilyPad -= LilyPadPreventer;
			IL_WorldGen.CheckCatTail -= CheckCattailEdit;
			IL_WorldGen.PlaceCatTail -= PlaceCattailEdit;
			On_WorldGen.GrowCatTail -= GrowCattailEdit;
			IL_WorldGen.CheckLilyPad -= CheckLilyPadEdit;
			IL_WorldGen.PlaceLilyPad -= PlaceLilyPadEdit;
			On_TileDrawing.DrawSingleTile -= LilyPadDrawingPreventer;
			On_Liquid.DelWater -= LilyPadCheck;
			On_Main.DrawTileInWater -= LilyPadDrawing;
			IL_WorldGen.PlantSeaOat -= PlantSeaOatEdit;
			IL_WorldGen.PlaceOasisPlant -= PlaceOasisPlant;
			On_TileDrawing.DrawMultiTileGrassInWind -= MultiTileGrassDetour; 
			On_WorldGen.PlaceOasisPlant -= PlantOasisPlantEdit;
			On_Player.MowGrassTile -= LAWWWWNNNNMOOOWWWWWWAAAAAA;
			On_SmartCursorHelper.Step_LawnMower -= SMARTLAWWWWWWNNNNNMOWWWWAAAAASSSSS;
			IL_NPC.SpawnNPC -= LawnSpawnPrevention;
			On_SmartCursorHelper.Step_GrassSeeds -= CreamBeansSmartCursor;
			IL_WaterfallManager.FindWaterfalls -= CloudWaterfalls;
			On_TileDrawing.DrawMultiTileVinesInWind -= On_TileDrawing_DrawMultiTileVinesInWind;
			IL_Sandstorm.EmitDust -= CreamsandSandstorm;
			On_WorldGen.Convert -= Convert;
			IL_Player.Update -= TileFallDamage;
			On_Player.PlaceThing_PaintScrapper_LongMoss -= MossScapper;
			IL_UIGenProgressBar.DrawSelf -= AddGoodToWorldgenBar;
			On_UIWorldListItem.DrawSelf -= ConfectionWorldIconEdit;
			IL_Lang.GetDryadWorldStatusDialog -= DryadWorldStatusEdit;
			IL_WorldGen.AddUpAlignmentCounts -= AddUpAligmenttmodEvilsandGoods;
			IL_WorldGen.CountTiles -= SettmodvilsandGoods;
			On_ItemDropDatabase.RegisterBoss_Twins -= On_ItemDropDatabase_RegisterBoss_Twins;
			On_Main.DrawMapFullscreenBackground -= On_Main_DrawMapFullscreenBackground;
			IL_Main.SetBackColor -= ConfectionBiomeLightColor;
			IL_Projectile.Damage -= CosmicCookieReflection;
			On_Projectile.CanBeReflected -= CosmicCookieCanBeReflect;
		}

		#region Star Cannon Projectile addition
		private bool CosmicCookieCanBeReflect(On_Projectile.orig_CanBeReflected orig, Projectile self)
		{
			if (self.active && self.friendly && !self.hostile && self.damage > 0)
			{
				if (self.type == ModContent.ProjectileType<CosmicCookie>())
				{
					return true;
				}
			}
			return orig.Invoke(self);
		}

		private void CosmicCookieReflection(ILContext il)
		{
			ILCursor c = new(il);
			ILLabel lable = c.DefineLabel();
			c.GotoNext(MoveType.After, i => i.MatchLdarg0(), i => i.MatchLdfld<Projectile>("type"), i => i.MatchLdcI4(955), i => i.MatchBeq(out lable));
			c.EmitLdarg0();
			c.EmitLdfld(typeof(Projectile).GetField("type"));
			c.EmitDelegate((int type) =>
			{
				return type == ModContent.ProjectileType<CosmicCookie>();
			});
			c.EmitBrtrue(lable);
		}
		#endregion

		#region BiomeColor
		private void ConfectionBiomeLightColor(ILContext il)
		{
			ILCursor c = new(il);
			c.GotoNext(MoveType.After, i => i.MatchLdcI4(15), i => i.MatchStloc3());
			c.EmitLdloca(1);
			c.EmitLdarga(1);
			c.EmitDelegate((ref Color bgColorToSet, ref Color sunColor) =>
			{
				float ConfectionBiomeInfluence = (float)ModContent.GetInstance<ConfectionBiomeTileCount>().confectionBlockCount / (float)ConfectionBiomeTileCount.ConfectionTileMax;
				if (ConfectionBiomeInfluence > 0f)
				{
					float num10 = ConfectionBiomeInfluence;
					if (num10 > 1f)
					{
						num10 = 1f;
					}
					int r = bgColorToSet.R;
					int g = bgColorToSet.G;
					int b = bgColorToSet.B;
					r -= (int)(10f * num10 * (bgColorToSet.R / 255f));
					g -= (int)(60f * num10 * (bgColorToSet.G / 255f));
					b -= (int)(40f * num10 * (bgColorToSet.B / 255f));
					if (r < 15)
					{
						r = 15;
					}
					if (g < 15)
					{
						g = 15;
					}
					if (b < 15)
					{
						b = 15;
					}
					DontStarveSeed.FixBiomeDarkness(ref bgColorToSet, ref r, ref g, ref b);
					bgColorToSet.R = (byte)r;
					bgColorToSet.G = (byte)g;
					bgColorToSet.B = (byte)b;
					r = sunColor.R;
					g = sunColor.G;
					b = sunColor.B;
					r -= (int)(100f * num10 * (sunColor.R / 255f));
					g -= (int)(100f * num10 * (sunColor.G / 255f));
					b -= (int)(0f * num10 * (sunColor.B / 255f));
					if (r < 15)
					{
						r = 15;
					}
					if (g < 15)
					{
						g = 15;
					}
					if (b < 15)
					{
						b = 15;
					}
					sunColor.R = (byte)r;
					sunColor.G = (byte)g;
					sunColor.B = (byte)b;
				}
			});
		}
		#endregion

		#region MapBackgroundColorFixer
		private void On_Main_DrawMapFullscreenBackground(On_Main.orig_DrawMapFullscreenBackground orig, Vector2 screenPosition, int screenWidth, int screenHeight)
		{
			if (Main.LocalPlayer.InModBiome(ModContent.GetInstance<ConfectionBiome>()))
			{
				Texture2D MapBGAsset = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Biomes/ConfectionBiomeMapBackground");
				Color color = Color.White;
				if ((double)screenPosition.Y > Main.worldSurface * 16.0)
				{
					MapBGAsset = Main.player[Main.myPlayer].ZoneDesert ? (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Biomes/ConfectionUndergroundDesertMapBackground") : ((!Main.player[Main.myPlayer].ZoneSnow) ? (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Biomes/ConfectionUndergroundMapBackground") : (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Biomes/ConfectionUndergroundIceMapBackground"));
				}
				else
				{
					color = Main.ColorOfTheSkies;
					MapBGAsset = ((!Main.player[Main.myPlayer].ZoneDesert) ? (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Biomes/ConfectionBiomeMapBackground") : ((Main.player[Main.myPlayer].ZoneSnow) ? (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Biomes/ConfectionIceBiomeMapBackground") : (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Biomes/ConfectionDesertBiomeMapBackground")));
				}
				Main.spriteBatch.Draw(MapBGAsset, new Rectangle(0, 0, screenWidth, screenHeight), color);
			}
			else
			{
				orig.Invoke(screenPosition, screenWidth, screenHeight);
			}
		}
		#endregion

		#region TwinsDropDetour
		private void On_ItemDropDatabase_RegisterBoss_Twins(On_ItemDropDatabase.orig_RegisterBoss_Twins orig, ItemDropDatabase self)
		{
			orig.Invoke(self);
			LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new Conditions.MissingTwin());
			LeadingConditionRule leadingConditionRule2 = new LeadingConditionRule(new Conditions.NotExpert());
			LeadingConditionRule leadingConditionRule3 = new LeadingConditionRule(new DrunkWorldIsNotActive());
			LeadingConditionRule ConfectionCondition = new LeadingConditionRule(new ConfectionDropRule());
			LeadingConditionRule HallowCondition = new LeadingConditionRule(new HallowDropRule());
			LeadingConditionRule DrunkCondition = new LeadingConditionRule(new DrunkWorldIsActive());
			leadingConditionRule.OnSuccess(leadingConditionRule2);
			leadingConditionRule2.OnSuccess(leadingConditionRule3);
			leadingConditionRule3.OnSuccess(ConfectionCondition);
			leadingConditionRule3.OnSuccess(HallowCondition);
			leadingConditionRule2.OnSuccess(DrunkCondition);
			ConfectionCondition.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.NeapoliniteOre>(), 1, 15 * 5, 30 * 5));
			HallowCondition.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.HallowedOre>(), 1, 15 * 5, 30 * 5));
			DrunkCondition.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.HallowedOre>(), 1, 8 * 5, 15 * 5));
			DrunkCondition.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.NeapoliniteOre>(), 1, 8 * 5, 15 * 5));
			self.RegisterToMultipleNPCs(leadingConditionRule, 126, 125);
		}
		#endregion

		#region Dynamic Dryad Text
		private void SettmodvilsandGoods(ILContext il)
		{
			ILCursor c = new(il);
			c.GotoNext(MoveType.After, i => i.MatchLdsfld<WorldGen>("totalGood2"), i => i.MatchStsfld<WorldGen>("totalGood"));
			c.EmitDelegate(() =>
			{
				ConfectionWorldGeneration.totalCandy = ConfectionWorldGeneration.totalCandy2;
			});
			c.GotoNext(MoveType.After, i => i.MatchCall("System.Math", "Round"), i => i.MatchConvU1(), i => i.MatchStsfld<WorldGen>("tBlood"));
			c.EmitDelegate(() =>
			{
				ConfectionWorldGeneration.tCandy = (byte)Math.Round((double)ConfectionWorldGeneration.totalCandy / (double)WorldGen.totalSolid * 100.0);
				if (ConfectionWorldGeneration.tCandy == 0 && ConfectionWorldGeneration.totalCandy > 0)
				{
					ConfectionWorldGeneration.tCandy = 1;
				}
			});
			c.GotoNext(MoveType.After, i => i.MatchLdcI4(0), i => i.MatchStsfld<WorldGen>("totalGood2"));
			c.EmitDelegate(() =>
			{
				ConfectionWorldGeneration.totalCandy2 = 0;
			});
		}

		private void AddUpAligmenttmodEvilsandGoods(ILContext il)
		{
			ILCursor c = new(il);
			c.GotoNext(MoveType.After, i => i.MatchLdcI4(0), i => i.MatchStsfld<WorldGen>("totalGood2"), i => i.MatchLdcI4(0), i => i.MatchStsfld<WorldGen>("totalBlood2"));
			c.EmitDelegate(() =>
			{
				ConfectionWorldGeneration.totalCandy2 = 0;
			});
			c.GotoNext(MoveType.After, i => i.MatchLdloc2(), i => i.MatchLdsfld("Terraria.ID.TileID/Sets", "CrimsonCountCollection"), i => i.MatchCallvirt<List<int>>("get_Count"), i => i.MatchBlt(out _));
			c.EmitDelegate(() =>
			{
				for (int l = 0; l < ConfectionIDs.Sets.ConfectCountCollection.Count; l++)
				{
					ConfectionWorldGeneration.totalCandy2 += WorldGen.tileCounts[ConfectionIDs.Sets.ConfectCountCollection[l]];
				}
			});
			c.GotoNext(MoveType.Before, i => i.MatchLdsfld<WorldGen>("tileCounts"), i => i.MatchLdcI4(0), i => i.MatchLdsfld<WorldGen>("tileCounts"));
			c.EmitDelegate(() =>
			{
				WorldGen.totalSolid2 +=
				WorldGen.tileCounts[ModContent.TileType<Creamstone>()] +
				WorldGen.tileCounts[ModContent.TileType<CreamGrass>()] +
				WorldGen.tileCounts[ModContent.TileType<CreamGrassMowed>()] +
				WorldGen.tileCounts[ModContent.TileType<Creamsand>()] +
				WorldGen.tileCounts[ModContent.TileType<BlueIce>()] +
				WorldGen.tileCounts[ModContent.TileType<Creamsandstone>()] +
				WorldGen.tileCounts[ModContent.TileType<HardenedCreamsand>()] +
				WorldGen.tileCounts[ModContent.TileType<CookieBlock>()] +
				WorldGen.tileCounts[ModContent.TileType<CreamBlock>()] +
				WorldGen.tileCounts[ModContent.TileType<PinkFairyFloss>()] +
				WorldGen.tileCounts[ModContent.TileType<PurpleFairyFloss>()] +
				WorldGen.tileCounts[ModContent.TileType<BlueFairyFloss>()] +
				WorldGen.tileCounts[ModContent.TileType<CreamstoneAmethyst>()] +
				WorldGen.tileCounts[ModContent.TileType<CreamstoneSaphire>()] +
				WorldGen.tileCounts[ModContent.TileType<CreamstoneTopaz>()] +
				WorldGen.tileCounts[ModContent.TileType<CreamstoneRuby>()] +
				WorldGen.tileCounts[ModContent.TileType<CreamstoneDiamond>()] +
				WorldGen.tileCounts[ModContent.TileType<CreamstoneEmerald>()] +
				WorldGen.tileCounts[ModContent.TileType<CreamstoneMossGreen>()] +
				WorldGen.tileCounts[ModContent.TileType<CreamstoneMossBrown>()] +
				WorldGen.tileCounts[ModContent.TileType<CreamstoneMossRed>()] +
				WorldGen.tileCounts[ModContent.TileType<CreamstoneMossBlue>()] +
				WorldGen.tileCounts[ModContent.TileType<CreamstoneMossPurple>()] +
				WorldGen.tileCounts[ModContent.TileType<CreamstoneMossLava>()] +
				WorldGen.tileCounts[ModContent.TileType<CreamstoneMossKrypton>()] +
				WorldGen.tileCounts[ModContent.TileType<CreamstoneMossXenon>()] +
				WorldGen.tileCounts[ModContent.TileType<CreamstoneMossArgon>()] +
				WorldGen.tileCounts[ModContent.TileType<CreamstoneMossNeon>()] +
				WorldGen.tileCounts[ModContent.TileType<CreamstoneMossHelium>()];
			});
		}

		private void DryadWorldStatusEdit(ILContext il)
		{
			ILCursor c = new(il);
			ILLabel IL_01b2 = c.DefineLabel();
			ILLabel IL_013f = c.DefineLabel();
			c.GotoNext(MoveType.Before, i => i.MatchLdloc0(), i => i.MatchRet());
			c.EmitBr(IL_013f);
			c.GotoNext(MoveType.After, i => i.MatchLdstr("DryadSpecialText.WorldStatusHallow"), i => i.MatchLdsfld<Main>("worldName"), i => i.MatchLdloc1(), i => i.MatchBox(out _), i => i.MatchCall("Terraria.Localization.Language", "GetTextValue"), i => i.MatchStloc0());
			c.MarkLabel(IL_013f);
			c.GotoNext(MoveType.Before, i => i.MatchLdloc0(), i => i.MatchLdstr(" "), i => i.MatchLdloc(4));
			c.EmitLdarg0();
			c.EmitLdindU1();
			c.EmitBrfalse(IL_01b2);
			c.EmitLdloc0();
			c.EmitRet();
			c.MarkLabel(IL_01b2);
			c.Goto(0);

			//This adds the tgoods and tbads to the dialogs so dialogs such as "the world is in balance" or "you have more work to do" can function properly with modded edits to the dryad dialog system
			c.GotoNext(MoveType.Before, i => i.MatchConvR8(), i => i.MatchLdcR8(1.2), i => i.MatchMul()); 
			c.EmitDelegate(() =>
			{
				return tmodGoodCount() * 1.2;
			});
			c.EmitConvR8();
			c.EmitAdd(); //(tGood + tmodGood) * 1.2
			c.GotoNext(MoveType.Before, i => i.MatchConvR8(), i => i.MatchLdcR8(0.8), i => i.MatchMul());
			c.EmitDelegate(() =>
			{
				return tmodGoodCount() * 0.8;
			});
			c.EmitConvR8();
			c.EmitAdd(); //(tGood + tmodGood) * 0.8

			//This commented out code is for the 'fairy tale' dialog, confection has its own text so we wont be needing to add the confection's onto the hallow's total
			//Uncomment this code to allow your tGood to count towards the fairy tale dialog

			//c.GotoNext(MoveType.After, i => i.MatchAdd(), i => i.MatchConvR8(), i => i.MatchBle(out _), i => i.MatchLdloc1());
			//c.EmitDelegate(tmodGoodCount);
			//c.EmitAdd(); //(tGood + tmodGood) >= tEvil + tBlood 

			c.GotoNext(MoveType.After, i => i.MatchLdloc1(), i => i.MatchLdcI4(20), i => i.MatchAdd());
			c.EmitDelegate(tmodGoodCount);
			c.EmitAdd(); //(tEvil + tBlood > tGood + 20 + tmodGood)
			c.Goto(0);

			//Applies the addition of tmodEvil to tEvil and tBlood for each boolean check
			for (int j = 0; j < 5; j++)
			{
				c.GotoNext(MoveType.After, i => i.MatchLdloc2(), i => i.MatchLdloc3(), i => i.MatchAdd());
				c.EmitDelegate(tmodEvilCount);
				c.EmitAdd();
			}

			//We add extra code for both the percentages display but also displaying our own world description too
			c.GotoNext(MoveType.After, i => i.MatchLdstr("DryadSpecialText.WorldDescriptionBalanced"), i => i.MatchCall("Terraria.Localization.Language", "GetTextValue"), i => i.MatchStloc(4));
			c.EmitLdarg(0);
			c.EmitLdloca(0);
			c.EmitLdarg(0);
			c.EmitLdindU1();
			//Percentage adder/editor
			c.EmitDelegate((ref string text, bool worldIsEntirelyPure) => 
			{
				int tCandy = ConfectionWorldGeneration.tCandy;
				bool flag = worldIsEntirelyPure;
				if (flag)
				{
					if (tCandy > 0)
					{
						text = Language.GetTextValue("Mods.TheConfectionRebirth.DryadSpecialText.WorldStatusCandy", Main.worldName, tCandy);
						flag = false;
					}
				}
				else
				{
					if (tCandy > 0)
					{
						string localText;
						string textStart = Main.worldName + Language.GetTextValue("Mods.TheConfectionRebirth.DryadSpecialText.WorldStatusWorldNameIs");
						string textEnd = text.Substring(textStart.Length);
						if (text.Contains(Language.GetTextValue("Mods.TheConfectionRebirth.DryadSpecialText.WorldStatusContainsAnd")))
						{
							localText = Language.GetTextValue("Mods.TheConfectionRebirth.DryadSpecialText.WorldStatusCandyComma", tCandy);
						}
						else
						{
							localText = Language.GetTextValue("Mods.TheConfectionRebirth.DryadSpecialText.WorldStatusCandyAnd", tCandy);
						}
						text = textStart + localText + " " + textEnd;
					}
				}
				return flag;
			});
			c.EmitStindI1();
			c.EmitLdloca(4);
			//description editor
			c.EmitDelegate((ref string arg) => 
			{
				int tCandy = ConfectionWorldGeneration.tCandy;
				int tEvil = WorldGen.tEvil;
				int tBlood = WorldGen.tBlood;
				if (arg != Language.GetTextValue("DryadSpecialText.WorldDescriptionBalanced") && (tCandy >= tEvil + tBlood + 1))
				{
					arg = Language.GetTextValue("Mods.TheConfectionRebirth.DryadSpecialText.WorldDescriptionSweeterAir");
				}
			});
		}

		private int tmodEvilCount()
		{
			//How this works is that you combine all your tmodEvil stats into the return
			//for example:
			//return tEvil2 + tSick + tSpicy + tBlood2;
			return 0;
		}

		private int tmodGoodCount()
		{
			int tCandy = ConfectionWorldGeneration.tCandy;
			return tCandy;
		}
		#endregion

		#region World Icon Edit
		private void ConfectionWorldIconEdit(On_UIWorldListItem.orig_DrawSelf orig, UIWorldListItem self, SpriteBatch spriteBatch)
		{
			orig.Invoke(self, spriteBatch);
			bool data = self.Data.TryGetHeaderData(ModContent.GetInstance<ConfectionWorldGeneration>(), out var _data);
			UIElement WorldIcon = (UIElement)typeof(UIWorldListItem).GetField("_worldIcon", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self);
			WorldFileData Data = (WorldFileData)typeof(AWorldListItem).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self);
			WorldIcon.RemoveAllChildren();
			if (data)
			{
				#region RegularSeedIcon
				if (_data.GetBool("HasConfection") && !Data.RemixWorld && !Data.DrunkWorld && !Data.Anniversary && !Data.DontStarve && !Data.ForTheWorthy && !Data.ZenithWorld && !Data.NotTheBees && !Data.NoTrapsWorld && Data.IsHardMode)
				{
					UIElement worldIcon = WorldIcon;
					UIImage element = new UIImage(ModContent.Request<Texture2D>("TheConfectionRebirth/Assets/WorldIcons/ConfectionNormal"))
					{
						Top = new StyleDimension(0f, 0f),
						Left = new StyleDimension(1f, 0f),
						IgnoresMouseInteraction = true
					};
					worldIcon.Append(element);
				}
				#endregion

				#region AnniversarySeedIcon
				if (_data.GetBool("HasConfection") && !Data.RemixWorld && !Data.DrunkWorld && Data.Anniversary && !Data.DontStarve && !Data.ForTheWorthy && !Data.ZenithWorld && !Data.NotTheBees && !Data.NoTrapsWorld && Data.IsHardMode)
				{
					UIElement worldIcon = WorldIcon;
					UIImage element = new UIImage(ModContent.Request<Texture2D>("TheConfectionRebirth/Assets/WorldIcons/ConfectionAnniversary"))
					{
						Top = new StyleDimension(0f, 0f),
						Left = new StyleDimension(0f, 0f),
						IgnoresMouseInteraction = true
					};
					worldIcon.Append(element);
				}
				#endregion

				#region DontStarveSeedIcon
				if (_data.GetBool("HasConfection") && !Data.RemixWorld && !Data.DrunkWorld && !Data.Anniversary && Data.DontStarve && !Data.ForTheWorthy && !Data.ZenithWorld && !Data.NotTheBees && !Data.NoTrapsWorld && Data.IsHardMode)
				{
					UIElement worldIcon = WorldIcon;
					UIImage element = new UIImage(ModContent.Request<Texture2D>("TheConfectionRebirth/Assets/WorldIcons/ConfectionDontStarve"))
					{
						Top = new StyleDimension(0f, 0f),
						Left = new StyleDimension(0f, 0f),
						IgnoresMouseInteraction = true
					};
					worldIcon.Append(element);
				}
				#endregion

				#region DrunkSeedIcon
				if (!Data.RemixWorld && Data.DrunkWorld && !Data.Anniversary && !Data.DontStarve && !Data.ForTheWorthy && !Data.ZenithWorld && !Data.NotTheBees && !Data.NoTrapsWorld && Data.IsHardMode)
				{
					UIElement worldIcon = WorldIcon;
					UIImage element = new UIImage(ModContent.Request<Texture2D>("TheConfectionRebirth/Assets/WorldIcons/ConfectionDrunk"))
					{
						Top = new StyleDimension(0f, 0f),
						Left = new StyleDimension(1f, 0f),
						IgnoresMouseInteraction = true
					};
					worldIcon.Append(element);
				}
				#endregion

				#region FTWSeedIcon
				if (_data.GetBool("HasConfection") && !Data.RemixWorld && !Data.DrunkWorld && !Data.Anniversary && !Data.DontStarve && Data.ForTheWorthy && !Data.ZenithWorld && !Data.NotTheBees && !Data.NoTrapsWorld && Data.IsHardMode)
				{
					UIElement worldIcon = WorldIcon;
					UIImage element = new UIImage(ModContent.Request<Texture2D>("TheConfectionRebirth/Assets/WorldIcons/ConfectionForTheWorthy"))
					{
						Top = new StyleDimension(0f, 0f),
						Left = new StyleDimension(0f, 0f),
						IgnoresMouseInteraction = true
					};
					worldIcon.Append(element);
				}
				#endregion

				#region NotTheBeesSeedIcon
				if (_data.GetBool("HasConfection") && !Data.RemixWorld && !Data.DrunkWorld && !Data.Anniversary && !Data.DontStarve && !Data.ForTheWorthy && !Data.ZenithWorld && Data.NotTheBees && !Data.NoTrapsWorld && Data.IsHardMode)
				{
					UIElement worldIcon = WorldIcon;
					UIImage element = new UIImage(ModContent.Request<Texture2D>("TheConfectionRebirth/Assets/WorldIcons/ConfectionNotTheBees"))
					{
						Top = new StyleDimension(0f, 0f),
						Left = new StyleDimension(0f, 0f),
						IgnoresMouseInteraction = true
					};
					worldIcon.Append(element);
				}
				#endregion

				#region NoTrapsSeedIcon
				if (_data.GetBool("HasConfection") && !Data.RemixWorld && !Data.DrunkWorld && !Data.Anniversary && !Data.DontStarve && !Data.ForTheWorthy && !Data.ZenithWorld && !Data.NotTheBees && Data.NoTrapsWorld && Data.IsHardMode)
				{
					UIElement worldIcon = WorldIcon;
					UIImage element = new UIImage(ModContent.Request<Texture2D>("TheConfectionRebirth/Assets/WorldIcons/ConfectionTrap"))
					{
						Top = new StyleDimension(0f, 0f),
						Left = new StyleDimension(1f, 0f),
						IgnoresMouseInteraction = true
					};
					worldIcon.Append(element);
				}
				#endregion

				#region RemixSeedIcon
				if (_data.GetBool("HasConfection") && Data.RemixWorld && !Data.DrunkWorld && !Data.Anniversary && !Data.DontStarve && !Data.ForTheWorthy && !Data.ZenithWorld && !Data.NotTheBees && !Data.NoTrapsWorld && Data.IsHardMode)
				{
					UIElement worldIcon = WorldIcon;
					UIImage element = new UIImage(ModContent.Request<Texture2D>("TheConfectionRebirth/Assets/WorldIcons/ConfectionRemix"))
					{
						Top = new StyleDimension(0f, 0f),
						Left = new StyleDimension(1f, 0f),
						IgnoresMouseInteraction = true
					};
					worldIcon.Append(element);
				}
				#endregion

				#region ZenithSeedIcon
				if (Data.RemixWorld && Data.DrunkWorld)
				{
					UIElement worldIcon = WorldIcon;
					Asset<Texture2D> obj = ModContent.Request<Texture2D>("TheConfectionRebirth/Assets/WorldIcons/ConfectionEverything", (AssetRequestMode)1);
					int _glitchVariation = (int)typeof(AWorldListItem).GetField("_glitchVariation", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self);
					int _glitchFrame = (int)typeof(AWorldListItem).GetField("_glitchFrame", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self);
					int _glitchFrameCounter = (int)typeof(AWorldListItem).GetField("_glitchFrameCounter", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self);
					if (_glitchFrame == 0 && _glitchFrameCounter == 0 && _glitchVariation < 3)
					{
						ZenithSeedWorlds[worldIcon.UniqueId] = Main.rand.NextBool(2);
					}
					int width = ZenithSeedWorlds[worldIcon.UniqueId] ? 0 : _glitchVariation;
					int height = ZenithSeedWorlds[worldIcon.UniqueId] ? 0 : _glitchFrame;
					UIImageFramed uIImageFramed = new UIImageFramed(obj, obj.Frame(7, 16, width, height));
					uIImageFramed.Left = new StyleDimension(0f, 0f);
					worldIcon.Append(uIImageFramed);
				}
				#endregion
			}
		}
		#endregion

		#region worldgeneration bar edit
		private void AddGoodToWorldgenBar(ILContext il)
		{
			ILCursor c = new(il);
			c.GotoNext(MoveType.After,
				i => i.MatchLdfld<UIGenProgressBar>("_texOuterCrimson"),
				i => i.MatchCallvirt<Asset<Texture2D>>("get_Value"),
				i => i.MatchLdloc(6),
				i => i.MatchCall("Terraria.Utils", "TopLeft"));
			c.EmitLdcI4(0); //new Rectange(0, 0, 266, 70)
			c.EmitLdcI4(0);
			c.EmitLdcI4(266);
			c.EmitLdcI4(70);
			c.EmitNewobj(typeof(Rectangle).GetConstructor([typeof(int), typeof(int), typeof(int), typeof(int)]));
			c.GotoNext(MoveType.Before, i => i.MatchCallvirt<SpriteBatch>("Draw"));
			c.Remove();
			c.EmitDelegate((SpriteBatch spriteBatch, Texture2D tex, Vector2 topLeft, Rectangle rect, Color white) => {
				spriteBatch.Draw(tex, topLeft, rect, white);
				bool flag = ConfectionWorldGeneration.confectionorHallow;
				if (WorldGen.drunkWorldGen && Main.rand.NextBool(2))
				{
					flag = !flag;
				}
				spriteBatch.Draw(flag ? texOuterConfection.Value : texOuterHallow.Value, topLeft, white);
			}); //thanks alf for the delegate :sob:
		}
		#endregion

		#region PaintScrapperSupport
		private void MossScapper(On_Player.orig_PlaceThing_PaintScrapper_LongMoss orig, Player self, int x, int y)
		{
			orig.Invoke(self, x, y);
			Tile tile = Main.tile[x, y];
			ushort type = tile.TileType;
			if (
				type != ModContent.TileType<CreamstoneMossGreen>() && 
				type != ModContent.TileType<CreamstoneMossBrown>() && 
				type != ModContent.TileType<CreamstoneMossRed>() && 
				type != ModContent.TileType<CreamstoneMossBlue>() && 
				type != ModContent.TileType<CreamstoneMossPurple>() &&
				type != ModContent.TileType<CreamstoneMossLava>() &&
				type != ModContent.TileType<CreamstoneMossKrypton>() &&
				type != ModContent.TileType<CreamstoneMossXenon>() &&
				type != ModContent.TileType<CreamstoneMossArgon>() &&
				type != ModContent.TileType<CreamstoneMossNeon>() &&
				type != ModContent.TileType<CreamstoneMossHelium>())
			{
				return;
			}
			self.cursorItemIconEnabled = true;
			if (!self.ItemTimeIsZero || self.itemAnimation <= 0 || !self.controlUseItem)
			{
				return;
			}
			WorldGen.KillTile(x, y, true, false);
			self.ApplyItemTime(self.inventory[self.selectedItem]);
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, x, y);
			}
			int itemType = 0;
			if (type == ModContent.TileType<CreamstoneMossGreen>())
			{
				itemType = ItemID.GreenMoss;
			}
			else if (type == ModContent.TileType<CreamstoneMossBrown>())
			{
				itemType = ItemID.BrownMoss;
			}
			else if (type == ModContent.TileType<CreamstoneMossRed>())
			{
				itemType = ItemID.RedMoss;
			}
			else if (type == ModContent.TileType<CreamstoneMossBlue>())
			{
				itemType = ItemID.BlueMoss;
			}
			else if (type == ModContent.TileType<CreamstoneMossPurple>())
			{
				itemType = ItemID.PurpleMoss;
			}
			else if (type == ModContent.TileType<CreamstoneMossLava>())
			{
				itemType = ItemID.LavaMoss;
			}
			else if (type == ModContent.TileType<CreamstoneMossKrypton>())
			{
				itemType = ItemID.KryptonMoss;
			}
			else if (type == ModContent.TileType<CreamstoneMossXenon>())
			{
				itemType = ItemID.XenonMoss;
			}
			else if (type == ModContent.TileType<CreamstoneMossArgon>())
			{
				itemType = ItemID.ArgonMoss;
			}
			else if (type == ModContent.TileType<CreamstoneMossNeon>())
			{
				itemType = ItemID.VioletMoss;
			}
			else if (type == ModContent.TileType<CreamstoneMossHelium>())
			{
				itemType = ItemID.RainbowMoss;
			}
			int number = Item.NewItem(new EntitySource_ItemUse(self, self.HeldItem), x * 16, y * 16, 16, 16, itemType);
			NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number, 1f);
		}
		#endregion

		#region solution Conversion
		private void Convert(On_WorldGen.orig_Convert orig, int i, int j, int conversionType, int size) {
			//Conversion Type:
			//7 = Forest
			//6 = Snow
			//5 = Desert
			//4 = Crimson
			//3 = Glowing Mushroom
			//2 = Hallow
			//1 = Corruption
			//0/default = purity
			orig.Invoke(i, j, conversionType, size);
			for (int k = i - size; k <= i + size; k++) {
				for (int l = j - size; l <= j + size; l++) {
					if (!WorldGen.InWorld(k, l, 1) || Math.Abs(k - i) + Math.Abs(l - j) >= 6) {
						continue;
					}
					Tile tile = Main.tile[k, l];
					int type = tile.TileType;
					int wall = tile.WallType;
					if (wall == ModContent.WallType<CookieStonedWall>() || wall == ModContent.WallType<Walls.CookieStonedWallArtificial>()) {
						Main.tile[k, l].WallType = WallID.Cave6Unsafe;
						WorldGen.SquareWallFrame(k, l);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (wall == ModContent.WallType<PinkFairyFlossWall>()) {
						Main.tile[k, l].WallType = WallID.Cloud;
						WorldGen.SquareWallFrame(k, l);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (wall == ModContent.WallType<BlueCreamyMossyWall>() || wall == ModContent.WallType<BlueCreamyMossyWallSafe>())
					{
						Main.tile[k, l].WallType = WallID.Cave4Unsafe;
						WorldGen.SquareWallFrame(k, l);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (wall == ModContent.WallType<BrownCreamyMossyWall>() || wall == ModContent.WallType<BrownCreamyMossyWallSafe>())
					{
						Main.tile[k, l].WallType = WallID.Cave2Unsafe;
						WorldGen.SquareWallFrame(k, l);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (wall == ModContent.WallType<GreenCreamyMossyWall>() || wall == ModContent.WallType<GreenCreamyMossyWallSafe>())
					{
						Main.tile[k, l].WallType = WallID.CaveUnsafe;
						WorldGen.SquareWallFrame(k, l);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (wall == ModContent.WallType<PurpleCreamyMossyWall>() || wall == ModContent.WallType<PurpleCreamyMossyWallSafe>())
					{
						Main.tile[k, l].WallType = WallID.Cave5Unsafe;
						WorldGen.SquareWallFrame(k, l);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (wall == ModContent.WallType<RedCreamyMossyWall>() || wall == ModContent.WallType<RedCreamyMossyWallSafe>())
					{
						Main.tile[k, l].WallType = WallID.Cave3Unsafe;
						WorldGen.SquareWallFrame(k, l);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (wall == ModContent.WallType<CreamstoneAmethystWall>() || wall == ModContent.WallType<CreamstoneAmethystWallSafe>())
					{
						Main.tile[k, l].WallType = WallID.AmethystUnsafe;
						WorldGen.SquareWallFrame(k, l);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (wall == ModContent.WallType<CreamstoneTopazWall>() || wall == ModContent.WallType<CreamstoneTopazWallSafe>())
					{
						Main.tile[k, l].WallType = WallID.TopazUnsafe;
						WorldGen.SquareWallFrame(k, l);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (wall == ModContent.WallType<CreamstoneSapphireWall>() || wall == ModContent.WallType<CreamstoneSapphireWallSafe>())
					{
						Main.tile[k, l].WallType = WallID.SapphireUnsafe;
						WorldGen.SquareWallFrame(k, l);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (wall == ModContent.WallType<CreamstoneEmeraldWall>() || wall == ModContent.WallType<CreamstoneEmeraldWallSafe>())
					{
						Main.tile[k, l].WallType = WallID.EmeraldUnsafe;
						WorldGen.SquareWallFrame(k, l);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (wall == ModContent.WallType<CreamstoneRubyWall>() || wall == ModContent.WallType<CreamstoneRubyWallSafe>())
					{
						Main.tile[k, l].WallType = WallID.RubyUnsafe;
						WorldGen.SquareWallFrame(k, l);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (wall == ModContent.WallType<CreamstoneDiamondWall>() || wall == ModContent.WallType<CreamstoneDiamondWallSafe>())
					{
						Main.tile[k, l].WallType = WallID.DiamondUnsafe;
						WorldGen.SquareWallFrame(k, l);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}

					if (type == ModContent.TileType<CookieBlock>()) {
						Main.tile[k, l].TileType = TileID.Dirt;
						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (type == ModContent.TileType<CreamBlock>()) {
						Main.tile[k, l].TileType = TileID.SnowBlock;
						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (Main.tile[k, l].TileType == ModContent.TileType<CreamstoneRuby>()) {
						Main.tile[k, l].TileType = TileID.Ruby;
						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (Main.tile[k, l].TileType == ModContent.TileType<CreamstoneSaphire>()) {
						Main.tile[k, l].TileType = TileID.Sapphire;
						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (Main.tile[k, l].TileType == ModContent.TileType<CreamstoneDiamond>()) {
						Main.tile[k, l].TileType = TileID.Diamond;
						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (Main.tile[k, l].TileType == ModContent.TileType<CreamstoneEmerald>()) {
						Main.tile[k, l].TileType = TileID.Emerald;
						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (Main.tile[k, l].TileType == ModContent.TileType<CreamstoneAmethyst>()) {
						Main.tile[k, l].TileType = TileID.Amethyst;
						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (Main.tile[k, l].TileType == ModContent.TileType<CreamstoneTopaz>()) {
						Main.tile[k, l].TileType = TileID.Topaz;
						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (Main.tile[k, l].TileType == ModContent.TileType<PinkFairyFloss>()) {
						Main.tile[k, l].TileType = TileID.Cloud;
						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (Main.tile[k, l].TileType == ModContent.TileType<PurpleFairyFloss>()) {
						Main.tile[k, l].TileType = TileID.RainCloud;
						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (Main.tile[k, l].TileType == ModContent.TileType<BlueFairyFloss>()) {
						Main.tile[k, l].TileType = TileID.SnowCloud;
						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (Main.tile[k, l].TileType == (ushort)ModContent.TileType<CreamstoneMossGreen>()) {
						Main.tile[k, l].TileType = TileID.GreenMoss;
						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (Main.tile[k, l].TileType == (ushort)ModContent.TileType<CreamstoneMossBrown>()) {
						Main.tile[k, l].TileType = TileID.BrownMoss;
						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (Main.tile[k, l].TileType == (ushort)ModContent.TileType<CreamstoneMossRed>()) {
						Main.tile[k, l].TileType = TileID.RedMoss;
						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (Main.tile[k, l].TileType == (ushort)ModContent.TileType<CreamstoneMossBlue>()) {
						Main.tile[k, l].TileType = TileID.BlueMoss;
						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (Main.tile[k, l].TileType == (ushort)ModContent.TileType<CreamstoneMossPurple>()) {
						Main.tile[k, l].TileType = TileID.PurpleMoss;
						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (Main.tile[k, l].TileType == (ushort)ModContent.TileType<CreamstoneMossLava>()) {
						Main.tile[k, l].TileType = TileID.LavaMoss;
						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (Main.tile[k, l].TileType == (ushort)ModContent.TileType<CreamstoneMossKrypton>()) {
						Main.tile[k, l].TileType = TileID.KryptonMoss;
						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (Main.tile[k, l].TileType == (ushort)ModContent.TileType<CreamstoneMossXenon>()) {
						Main.tile[k, l].TileType = TileID.XenonMoss;
						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (Main.tile[k, l].TileType == (ushort)ModContent.TileType<CreamstoneMossArgon>()) {
						Main.tile[k, l].TileType = TileID.ArgonMoss;
						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (Main.tile[k, l].TileType == (ushort)ModContent.TileType<CreamstoneMossNeon>())
					{
						Main.tile[k, l].TileType = TileID.VioletMoss;
						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (Main.tile[k, l].TileType == (ushort)ModContent.TileType<CreamstoneMossHelium>())
					{
						Main.tile[k, l].TileType = TileID.RainbowMoss;
						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
				}
			}
		}
		#endregion

		#region Sandstorm
		private void CreamsandSandstorm(ILContext il) {
			ILCursor c = new(il);
			c.GotoNext(MoveType.Before,
				i => i.MatchLdcR4(0.2f),
				i => i.MatchLdcR4(0.35f),
				i => i.MatchLdsfld<Sandstorm>("Severity"),
				i => i.MatchCall("Microsoft.Xna.Framework.MathHelper", "Lerp"),
				i => i.MatchStloc(18));
			c.EmitLdloca(17);
			c.EmitDelegate((ref WeightedRandom<Color> weightedRandom) => {
				weightedRandom.Add(new Color(99, 57, 46),
					Main.SceneMetrics.GetTileCount((ushort)ModContent.TileType<Tiles.Creamsand>()) +
					Main.SceneMetrics.GetTileCount((ushort)ModContent.TileType<Tiles.Creamsandstone>()) +
					Main.SceneMetrics.GetTileCount((ushort)ModContent.TileType<Tiles.HardenedCreamsand>()));
			});
		}
		#endregion

		#region WindEdits
		private void On_TileDrawing_DrawMultiTileVinesInWind(On_TileDrawing.orig_DrawMultiTileVinesInWind orig, TileDrawing self, Vector2 screenPosition, Vector2 offSet, int topLeftX, int topLeftY, int sizeX, int sizeY) {
			if (Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<Tiles.ConfectionBanners>()) {
				sizeY = 3;
			}
			/*else if (Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<Tiles.Furniture.CreamwoodChandelier>()) {
				sizeX = 3;
				sizeY = 3;
			}
			else if (Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<Tiles.Furniture.SacchariteChandelier>()) {
				sizeX = 3;
				sizeY = 3;
			}
			else if (Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<Tiles.CherryBugBottle>()) {
				sizeY = 2;
			}
			else if (Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<Tiles.RoyalCherryBugBottle>()) {
				sizeY = 2;
			}
			else if (Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<Tiles.SoulofDelightinaBottle>()) {
				sizeY = 2;
			}
			else if (Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<Tiles.SoulofSpiteinaBottle>()) {
				sizeY = 2;
			}*/
			orig.Invoke(self, screenPosition, offSet, topLeftX, topLeftY, sizeX, sizeY);
		}
		#endregion

		#region Rain&SnowClouds
		private void TileFallDamage(ILContext il)
		{
			ILCursor c = new(il);
			c.GotoNext(MoveType.After, i => i.MatchLdfld<Player>("fallStart"), i => i.MatchSub(), i => i.MatchStloc(56));
			c.EmitLdarg0();
			c.EmitLdloca(56); //num26
			c.EmitDelegate((Player self, ref int num26) =>
			{
				if (num26 > 0 || (self.gravDir == -1f && num26 < 0))
				{
					int num112 = (int)(self.position.X / 16f);
					int num28 = (int)((self.position.X + (float)self.width) / 16f);
					int num29 = (int)((self.position.Y + (float)self.height + 1f) / 16f);
					if (self.gravDir == -1f)
					{
						num29 = (int)((self.position.Y - 1f) / 16f);
					}
					for (int num30 = num112; num30 <= num28; num30++)
					{
						if (Main.tile[num30, num29] != null && Main.tile[num30, num29].HasTile && (Main.tile[num30, num29].TileType == ModContent.TileType<PinkFairyFloss>() || Main.tile[num30, num29].TileType == ModContent.TileType<PurpleFairyFloss>() || Main.tile[num30, num29].TileType == ModContent.TileType<BlueFairyFloss>()))
						{
							num26 = 0;
							break;
						}
					}
				}
			});
		}

		private void CloudWaterfalls(ILContext il) {
			ILCursor c = new ILCursor(il);
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdloc(2),
				i => i.MatchStloc(5),
				i => i.MatchBr(out _),
				i => i.MatchLdsflda<Main>("tile"),
				i => i.MatchLdloc(4),
				i => i.MatchLdloc(5),
				i => i.MatchCall<Tilemap>("get_Item"),
				i => i.MatchStloc(6));
			c.EmitLdloc(4);
			c.EmitLdloc(5);
			c.EmitLdarg0();
			c.EmitLdflda(typeof(WaterfallManager).GetField("currentMax", BindingFlags.NonPublic | BindingFlags.Instance));
			c.EmitLdarg0();
			c.EmitLdfld(typeof(WaterfallManager).GetField("qualityMax", BindingFlags.NonPublic | BindingFlags.Instance));
			c.EmitLdarg0();
			c.EmitLdfld(typeof(WaterfallManager).GetField("waterfalls", BindingFlags.NonPublic | BindingFlags.Instance));
			c.EmitDelegate((int i, int j, ref int currentMax, int qualityMax, WaterfallData[] waterfalls) => {
				Tile tile2 = Framing.GetTileSafely(i, j - 1);
				if (Main.tile[i, j].TileType == ModContent.TileType<PurpleFairyFloss>()) {
					Tile tile5 = Framing.GetTileSafely(i, j + 1);
					if (!WorldGen.SolidTile(tile5) && tile5.Slope == 0 && currentMax < qualityMax) {
						waterfalls[currentMax].type = ModContent.Find<ModWaterfallStyle>("TheConfectionRebirth/ChocolateRainWaterfallStyle").Slot;
						waterfalls[currentMax].x = i;
						waterfalls[currentMax].y = j + 1;
						currentMax++;
					}
				}
				if (Main.tile[i, j].TileType == ModContent.TileType <BlueFairyFloss>()) {
					Tile tile6 = Framing.GetTileSafely(i, j + 1);
					if (!WorldGen.SolidTile(tile6) && tile6.Slope == 0 && currentMax < qualityMax) {
						waterfalls[currentMax].type = ModContent.Find<ModWaterfallStyle>("TheConfectionRebirth/CreamSnowWaterfallStyle").Slot;
						waterfalls[currentMax].x = i;
						waterfalls[currentMax].y = j + 1;
						currentMax++;
					}
				}
			});
		}
		#endregion

		#region SmartCursorBeans
		private void CreamBeansSmartCursor(On_SmartCursorHelper.orig_Step_GrassSeeds orig, object providedInfo, ref int focusedX, ref int focusedY) {
			orig.Invoke(providedInfo, ref focusedX, ref focusedY);
			var SmartCursorUsageInfo = typeof(SmartCursorHelper).GetNestedType("SmartCursorUsageInfo", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
			Item item = (Item)SmartCursorUsageInfo.GetField("item", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			int reachableStartX = (int)SmartCursorUsageInfo.GetField("reachableStartX", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			int reachableEndX = (int)SmartCursorUsageInfo.GetField("reachableEndX", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			int reachableStartY = (int)SmartCursorUsageInfo.GetField("reachableStartY", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			int reachableEndY = (int)SmartCursorUsageInfo.GetField("reachableEndY", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			Vector2 mouse = (Vector2)SmartCursorUsageInfo.GetField("mouse", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			List<Tuple<int, int>> _targets = (List<Tuple<int, int>>)typeof(SmartCursorHelper).GetField("_targets", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).GetValue(null);
			if (focusedX > -1 || focusedY > -1) {
				return;
			}
			int type = item.type;
			if ((type < 0 || !ItemID.Sets.GrassSeeds[type]) && type != ModContent.ItemType<Items.Placeable.CreamBeans>()) {
				return;
			}
			_targets.Clear();
			for (int i = reachableStartX; i <= reachableEndX; i++) {
				for (int j = reachableStartY; j <= reachableEndY; j++) {
					Tile tile = Main.tile[i, j];
					bool flag = !Main.tile[i - 1, j].HasTile || !Main.tile[i, j + 1].HasTile || !Main.tile[i + 1, j].HasTile || !Main.tile[i, j - 1].HasTile;
					bool flag2 = !Main.tile[i - 1, j - 1].HasTile || !Main.tile[i - 1, j + 1].HasTile || !Main.tile[i + 1, j + 1].HasTile || !Main.tile[i + 1, j - 1].HasTile;
					if (tile.HasTile && !tile.IsActuated && (flag || flag2)) {
						bool flag3 = false;
						if (type == ModContent.ItemType<Items.Placeable.CreamBeans>()) {
							flag3 = tile.TileType == ModContent.TileType<Tiles.CookieBlock>() || tile.TileType == TileID.Dirt;
						}
						if (flag3) {
							_targets.Add(new Tuple<int, int>(i, j));
						}
					}
				}
			}
			if (_targets.Count > 0) {
				float num = -1f;
				Tuple<int, int> tuple = _targets[0];
				for (int k = 0; k < _targets.Count; k++) {
					float num2 = Vector2.Distance(new Vector2((float)_targets[k].Item1, (float)_targets[k].Item2) * 16f + Vector2.One * 8f, mouse);
					if (num == -1f || num2 < num) {
						num = num2;
						tuple = _targets[k];
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, reachableStartX, reachableStartY, reachableEndX, reachableEndY)) {
					focusedX = tuple.Item1;
					focusedY = tuple.Item2;
				}
			}
			_targets.Clear();
		}
#endregion

		#region LAAAAAAWWWWWWNNNNNMOWWWWWWAAAAAAA!!!!!
		private void LawnSpawnPrevention(ILContext il) {
			ILCursor c = new(il);
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdloca(9),
				i => i.MatchLdloc(77),
				i => i.MatchStfld<NPCSpawnInfo>("PlayerFloorY"));
			c.EmitLdloc(6); //num35
			c.EmitLdloca(2); // ref flag12
			c.EmitDelegate((int num35, ref bool flag12) => {
				if (num35 == ModContent.TileType<CreamGrassMowed>() && !Main.bloodMoon && !Main.eclipse && Main.invasionType <= 0 && !Main.pumpkinMoon && !Main.snowMoon && !Main.slimeRain && Main.rand.Next(100) < 10) {
					flag12 = false;
				}
			});
		}

		private void SMARTLAWWWWWWNNNNNMOWWWWAAAAASSSSS(On_SmartCursorHelper.orig_Step_LawnMower orig, object providedInfo, ref int fX, ref int fY) {
			orig.Invoke(providedInfo, ref fX, ref fY);
			var SmartCursorUsageInfo = typeof(SmartCursorHelper).GetNestedType("SmartCursorUsageInfo", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
			Item item = (Item)SmartCursorUsageInfo.GetField("item", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			int screenTargetX = (int)SmartCursorUsageInfo.GetField("screenTargetX", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			int screenTargetY = (int)SmartCursorUsageInfo.GetField("screenTargetY", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			int reachableStartX = (int)SmartCursorUsageInfo.GetField("reachableStartX", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			int reachableEndX = (int)SmartCursorUsageInfo.GetField("reachableEndX", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			int reachableStartY = (int)SmartCursorUsageInfo.GetField("reachableStartY", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			int reachableEndY = (int)SmartCursorUsageInfo.GetField("reachableEndY", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			Vector2 mouse = (Vector2)SmartCursorUsageInfo.GetField("mouse", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			List<Tuple<int, int>> _targets = (List<Tuple<int, int>>)typeof(SmartCursorHelper).GetField("_targets", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).GetValue(null);
			_ = screenTargetX;
			_ = screenTargetY;
			if (item.type != 4049 || fX != -1 || fY != -1) {
				return;
			}
			_targets.Clear();
			for (int i = reachableStartX; i <= reachableEndX; i++) {
				for (int j = reachableStartY; j <= reachableEndY; j++) {
					Tile tile = Main.tile[i, j];
					if (tile.HasTile && (tile.TileType == ModContent.TileType<CreamGrass>())) {
						_targets.Add(new Tuple<int, int>(i, j));
					}
				}
			}
			if (_targets.Count > 0) {
				float num = -1f;
				Tuple<int, int> tuple = _targets[0];
				for (int k = 0; k < _targets.Count; k++) {
					float num2 = Vector2.Distance(new Vector2((float)_targets[k].Item1, (float)_targets[k].Item2) * 16f + Vector2.One * 8f, mouse);
					if (num == -1f || num2 < num) {
						num = num2;
						tuple = _targets[k];
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, reachableStartX, reachableStartY, reachableEndX, reachableEndY)) {
					fX = tuple.Item1;
					fY = tuple.Item2;
				}
			}
			_targets.Clear();
		}

		private void LAWWWWNNNNMOOOWWWWWWAAAAAA(On_Player.orig_MowGrassTile orig, Player self, Vector2 thePos) {
			orig.Invoke(self, thePos);
			Point point = thePos.ToTileCoordinates();
			Tile tile = Main.tile[point.X, point.Y];
			ushort num = 0;
			if (tile.TileType == ModContent.TileType<CreamGrass>()) {
				num = (ushort)ModContent.TileType<CreamGrassMowed>();
			}
			if (num != 0) {
				int num2 = WorldGen.KillTile_GetTileDustAmount(fail: true, tile, point.X, point.Y);
				for (int i = 0; i < num2; i++) {
					WorldGen.KillTile_MakeTileDust(point.X, point.Y, tile);
				}
				tile.TileType = num;
				if (Main.netMode == 1) {
					NetMessage.SendTileSquare(-1, point.X, point.Y);
				}
			}

		}
		#endregion

		#region OasisPlants

		private void PlantOasisPlantEdit(On_WorldGen.orig_PlaceOasisPlant orig, int X, int Y, ushort type) {
			if (type == 530) {
				if (Main.tile[X, Y + 1].TileType == ModContent.TileType<Creamsand>()) {
					type = (ushort)ModContent.TileType<CreamOasisPlants>();
				}
			}
			orig.Invoke(X, Y, type);
		}

		private void MultiTileGrassDetour(On_TileDrawing.orig_DrawMultiTileGrassInWind orig, TileDrawing self, Vector2 screenPosition, Vector2 offSet, int topLeftX, int topLeftY, int sizeX, int sizeY) {
			if (Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<CreamOasisPlants>()) {
				sizeX = 3;
				sizeY = 2;
			}
			orig.Invoke(self, screenPosition, offSet, topLeftX, topLeftY, sizeX, sizeY);
		}
		#endregion

		#region SeaOats
		private void PlaceOasisPlant(ILContext il) {
			ILCursor c = new(il);
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdcI4(529),
				i => i.MatchBeq(out _),
				i => i.MatchLdcI4(0),
				i => i.MatchStloc2());
			c.EmitLdloc(3); //i
			c.EmitLdloc(4); //j
			c.EmitLdloca(2); //ref flag
			c.EmitDelegate((int i, int j, ref bool flag) => {
				if (Main.tile[i, j].TileType == ModContent.TileType<CreamSeaOats>() && !flag) {
					flag = true;
				}
			});
		}

		private void PlantSeaOatEdit(ILContext il) {
			ILCursor c = new(il);
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdcI4(529),
				i => i.MatchStindI2(),
				i => i.MatchLdsflda<Main>("tile"),
				i => i.MatchLdarg0(),
				i => i.MatchLdarg1(),
				i => i.MatchCall<Tilemap>("get_Item"),
				i => i.MatchStloc1());
			c.EmitLdarg0(); //x
			c.EmitLdarg1(); //y
			c.EmitDelegate((int x, int y) => {
				if (Main.tile[x, y + 1].TileType == ModContent.TileType<Creamsand>()) {
					Main.tile[x, y].TileType = (ushort)ModContent.TileType<CreamSeaOats>();
				}
			});
		}
		#endregion

		#region LilyPads
		private void LilyPadDrawing(On_Main.orig_DrawTileInWater orig, Vector2 drawOffset, int x, int y) {
			orig.Invoke(drawOffset, x, y);
			if (Main.tile[x, y] != null && Main.tile[x, y].HasTile && Main.tile[x, y].TileType == ModContent.TileType<CreamLilyPads>()) {
				Main.instance.LoadTiles(Main.tile[x, y].TileType);
				Tile tile = Main.tile[x, y];
				int num = tile.LiquidAmount / 16;
				num -= 3;
				if (WorldGen.SolidTile(x, y - 1) && num > 8) {
					num = 8;
				}
				Rectangle value = new((int)tile.TileFrameX, (int)tile.TileFrameY, 16, 16);
				Main.spriteBatch.Draw(TextureAssets.Tile[tile.TileType].Value, new Vector2((float)(x * 16), (float)(y * 16 - num)) + drawOffset, (Rectangle?)value, Lighting.GetColor(x, y), 0f, default(Vector2), 1f, (SpriteEffects)0, 0f);
			}
		}

		private void LilyPadCheck(On_Liquid.orig_DelWater orig, int l) {
			orig.Invoke(l);
			int num = Main.liquid[l].x;
			int num2 = Main.liquid[l].y;
			Tile tile4 = Main.tile[num, num2];
			if (!Main.tileAlch[tile4.TileType] && tile4.TileType == ModContent.TileType<CreamLilyPads>()) {
				if (Liquid.quickFall) {
					WorldGen.CheckLilyPad(num, num2);
				}
				else if (Main.tile[num, num2 + 1].LiquidAmount < byte.MaxValue || Main.tile[num, num2 - 1].LiquidAmount > 0) {
					WorldGen.SquareTileFrame(num, num2);
				}
				else {
					WorldGen.CheckLilyPad(num, num2);
				}
			}
		}

		private void LilyPadDrawingPreventer(On_TileDrawing.orig_DrawSingleTile orig, TileDrawing self, Terraria.DataStructures.TileDrawInfo drawData, bool solidLayer, int waterStyleOverride, Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY) {
			drawData.tileCache = Main.tile[tileX, tileY]; //Doesnt quite work yet, probably something to do with lilypads being drawn elsewhere (not inside of TileDrawing), probs use vs to look for any instance of LilyPad or 518
			drawData.typeCache = drawData.tileCache.TileType;
			drawData.tileFrameX = drawData.tileCache.TileFrameX;
			drawData.tileFrameY = drawData.tileCache.TileFrameY;
			drawData.tileLight = Lighting.GetColor(tileX, tileY);
			if (drawData.tileCache.LiquidAmount > 0 && drawData.tileCache.TileType == ModContent.TileType<CreamLilyPads>()) {
				return;
			}
			orig.Invoke(self, drawData, solidLayer, waterStyleOverride, screenPosition, screenOffset, tileX, tileY);
		}

		private void PlaceLilyPadEdit(ILContext il) {
			ILCursor c = new(il);
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdcI4(5),
				i => i.MatchStloc(1),
				i => i.MatchLdcI4(0),
				i => i.MatchStloc(2));
			c.EmitLdarg(0); //x
			c.EmitLdloc1(); //num2
			c.EmitLdloc0(); //num
			c.EmitLdloca(2); //ref num3
			c.EmitDelegate((int x, int num2, int num, ref int num3) => {
				for (int i = x - num2; i <= x + num2; i++) {
					for (int k = num - num2; k <= num + num2; k++) {
						if (Main.tile[i, k].HasTile && Main.tile[i, k].TileType == ModContent.TileType<CreamLilyPads>()) {
							num3++;
						}
					}
				}
			});
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdindU2(),
				i => i.MatchStloc(5),
				i => i.MatchLdcI4(-1), 
				i => i.MatchStloc(6));
			c.EmitLdloc(5); //type
			c.EmitLdloca(6); //ref num5
			c.EmitDelegate((int type, ref int num5) => {
				if (type == ModContent.TileType<CreamGrass>() || type == ModContent.TileType<CreamGrassMowed>() || type == ModContent.TileType<Creamsand>()) {
					num5 = ModContent.TileType<CreamLilyPads>();
				}
			});
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdloca(7),
				i => i.MatchCall<Tile>("get_frameY"),
				i => i.MatchLdloc(6),
				i => i.MatchConvI2(),
				i => i.MatchStindI2());
			c.EmitLdarg(0); //x
			c.EmitLdloc(0); //num
			c.EmitLdloc(6); //num5
			c.EmitDelegate((int x, int num, int num5) => {
				if (num5 == ModContent.TileType<CreamLilyPads>()) {
					Main.tile[x, num].TileType = (ushort)num5;
					Main.tile[x, num].TileFrameY = 0;
				}
			});
		}

		private void CheckLilyPadEdit(ILContext il) {
			ILCursor c = new(il);
			ILLabel IL_0000 = c.DefineLabel();
			c.GotoNext(
				MoveType.After,
				i => i.MatchStloc(1),
				i => i.MatchLdcI4(-1), //also known as Ldc.i4.m1
				i => i.MatchStloc(2));
			c.EmitLdloc(1); //type
			c.EmitLdloca(2); //ref num2
			c.EmitLdarg0(); //x
			c.EmitLdarg1(); //y
			c.EmitLdloca(3); //ref tile
			c.EmitDelegate((int type, ref int num2, int x, int y, ref Tile tile) => { //we inject this to change the num2 to our tile when under a certain tile type (we use the TileID for creamlilypads as num2 for the same reasons as CheckCattail())
				if (type == ModContent.TileType<CreamGrass>() || type == ModContent.TileType<Creamsand>()) {
					num2 = -1;
					int num3 = ModContent.TileType<CreamLilyPads>();
					tile = Main.tile[x, y];
					if (num3 != tile.TileType) {
						tile.TileType = (ushort)num3;
						tile.TileFrameY = 0;
						if (Main.netMode == 2) {
							NetMessage.SendTileSquare(-1, x, y);
						}
					}
					tile = Main.tile[x, y - 1];
					if (tile.LiquidType > 0) {
						if (!tile.HasTile) {
							tile.HasTile = true;
							tile.TileType = (ushort)ModContent.TileType<CreamLilyPads>();
							ref short frameX = ref tile.TileFrameX;
							tile = Main.tile[x, y];
							frameX = tile.TileFrameX;
							tile = Main.tile[x, y - 1];
							ref short frameY = ref tile.TileFrameY;
							tile = Main.tile[x, y];
							frameY = tile.TileFrameY;
							tile = Main.tile[x, y - 1];
							tile.IsHalfBlock = false;
							tile.Slope = 0;
							tile = Main.tile[x, y];
							tile.HasTile = false;
							tile.TileType = 0;
							WorldGen.SquareTileFrame(x, y - 1, resetFrame: false);
							if (Main.netMode == 2) {
								NetMessage.SendTileSquare(-1, x, y - 1, 1, 2);
							}
							return;
						}
					}
					tile = Main.tile[x, y];
					if (tile.LiquidAmount != 0) {
						return;
					}
					Tile tileSafely = Framing.GetTileSafely(x, y + 1);
					if (!tileSafely.HasTile) {
						tile = Main.tile[x, y + 1];
						tile.HasTile = true;
						tile.TileType = (ushort)ModContent.TileType<CreamLilyPads>();
						ref short frameX2 = ref tile.TileFrameX;
						tile = Main.tile[x, y];
						frameX2 = tile.TileFrameX;
						tile = Main.tile[x, y + 1];
						ref short frameY2 = ref tile.TileFrameY;
						tile = Main.tile[x, y];
						frameY2 = tile.TileFrameY;
						tile = Main.tile[x, y + 1];
						tile.IsHalfBlock = false;
						tile.Slope = 0;
						tile = Main.tile[x, y];
						tile.HasTile = false;
						tile = Main.tile[x, y];
						tile.TileType = 0;
						WorldGen.SquareTileFrame(x, y + 1, resetFrame: false);
						if (Main.netMode == 2) {
							NetMessage.SendTileSquare(-1, x, y, 1, 2);
						}
					}
					else if (tileSafely.HasTile && !TileID.Sets.Platforms[tileSafely.TileType] && (!Main.tileSolid[tileSafely.TileType] || Main.tileSolidTop[tileSafely.TileType])) {
						WorldGen.KillTile(x, y);
						if (Main.netMode == 2) {
							NetMessage.SendData(17, -1, -1, null, 0, x, y);
						}
					}
				}
			});
			c.EmitLdloc(1); //type
			c.EmitDelegate((int type) => {
				return type == ModContent.TileType<CreamGrass>() || type == ModContent.TileType<Creamsand>() || type == ModContent.TileType<CreamGrassMowed>();
			});
			c.EmitBrfalse(IL_0000);
			c.EmitRet(); //return
			c.MarkLabel(IL_0000);

			c.GotoNext(
				MoveType.After,
				i => i.MatchLdloca(3),
				i => i.MatchCall<Tile>("get_frameY"),
				i => i.MatchLdloc(2),
				i => i.MatchConvI2(),
				i => i.MatchStindI2());
			c.EmitLdloca(3); //ref Tile
			c.EmitDelegate((ref Tile tile) => { //set the TileType to lilyPad since so the conversion between our and their tiles dont result in unintended consiquences
				tile.TileType = TileID.LilyPad;
			});
		}
		#endregion

		#region Cattails
		public static int ClimbCreamCatTail(int originx, int originy) {
			int num = 0;
			int num2 = originy;
			while (num2 > 10) {
				Tile tile = Main.tile[originx, num2];
				if (!tile.HasTile || tile.TileType != ModContent.TileType<CreamCattails>()) {
					break;
				}
				if (tile.TileFrameX >= 180) {
					num++;
					break;
				}
				num2--;
				num++;
			}
			return num;
		}

		private void GrowCattailEdit(On_WorldGen.orig_GrowCatTail orig, int x, int j) {
			ConfectionWorldGeneration.GrowCreamCatTail(x, j);
			orig.Invoke(x, j);
		}

		private void PlaceCattailEdit(ILContext il) {
			ILCursor c = new(il);
			ILLabel IL_0000 = c.DefineLabel();
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdcI4(7),
				i => i.MatchStloc(2),
				i => i.MatchLdcI4(0),
				i => i.MatchStloc(3));
			c.EmitLdarg(0); //x
			c.EmitLdloc2(); //num2
			c.EmitLdloc0(); //num
			c.EmitLdloca(3); //ref num3
			c.EmitDelegate((int x, int num2, int num, ref int num3) => {
				for (int i = x - num2; i <= x + num2; i++) { //
					for (int k = num - num2; k <= num + num2; k++) {
						if (Main.tile[i, k].HasTile && Main.tile[i, k].TileType == ModContent.TileType<CreamCattails>()) {
							num3++;
							break;
						}
					}
				}
			});
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdloc1(),
				i => i.MatchRet(),
				i => i.MatchLdcI4(-1), //also known as m1
				i => i.MatchStloc(7));
			c.EmitLdloc(6); //type
			c.EmitLdloca(7); //ref num5
			c.EmitDelegate((int type, ref int num5) => {
				if (type == ModContent.TileType<CreamGrass>() || type == ModContent.TileType<CreamGrassMowed>() || type == ModContent.TileType<Creamsand>()) {
					num5 = ModContent.TileType<CreamCattails>();
				}
			});
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdloc1(),
				i => i.MatchRet(),
				i => i.MatchLdloc(4),
				i => i.MatchLdcI4(1),
				i => i.MatchSub(),
				i => i.MatchStloc0());
			c.EmitLdloc(7); //num5
			c.EmitDelegate((int num5) => {
				return num5 == ModContent.TileType<CreamCattails>();
			});
			c.EmitBrfalse(IL_0000);
			c.EmitLdloc(7); //num5
			c.EmitLdarg(0); //x
			c.EmitLdloc0(); //num
			c.EmitDelegate((int num5, int x, int num) => {
				Tile tile2 = Main.tile[x, num];
				tile2.HasTile = true;
				Main.tile[x, num].TileType = (ushort)num5;
				Main.tile[x, num].TileFrameX = 0;
				Main.tile[x, num].TileFrameY = 0;
				tile2.IsHalfBlock = false;
				tile2.Slope = 0;
				Main.tile[x, num].CopyPaintAndCoating(Main.tile[x, num + 1]);
				WorldGen.SquareTileFrame(x, num);

			});
			//return new Point(x, num);
			c.EmitLdarg0(); //x
			c.EmitLdloc0(); //num
			c.EmitNewobj(typeof(Point).GetConstructor([typeof(int), typeof(int)])); //new Point()
			c.EmitRet(); //return
			c.MarkLabel(IL_0000); //thanks fox for figuring out a solution for me for the in if bound return :3
		}

		private void CheckCattailEdit(ILContext il) {
			ILCursor c = new(il);
			c.GotoNext(
				MoveType.After,
				i => i.MatchStloc(5),
				i => i.MatchLdcI4(-1), //also known as Ldc.i4.m1
				i => i.MatchStloc(6));
			c.EmitLdloc(5); //type
			c.EmitLdloca(6); //ref num5
			c.EmitDelegate((int type, ref int num5) => { //injects this delegate just before the switch to set num5 to the TileFrameY
				if (type == ModContent.TileType<CreamGrass>() || type == ModContent.TileType<CreamGrassMowed>() || type == ModContent.TileType<Creamsand>()) { 
					num5 = ModContent.TileType<CreamCattails>(); 
				}
			}); //num5 is usually used for tileframeY of the cattail type (normal, desert, hallow(unused), corruption, crimson, mushroom)
				//but here we use the TileID so that we dont clash with other mods
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdcI4(519),
				i => i.MatchBeq(out _),
				i => i.MatchLdloc0(),
				i => i.MatchLdcI4(1), 
				i => i.MatchAdd(), 
				i => i.MatchStloc0());
			c.EmitLdarg0(); //x
			c.EmitLdloc2(); //num2
			c.EmitLdloc1(); //flag
			c.EmitLdloca(0); //ref num
			c.EmitLdloca(6); //ref num5
			c.EmitDelegate((int x, int num2, bool flag, ref int num, ref int num5) => { //to make injection easier, we insert this after the incrimenting of num,
																						//this converts the tile if num5 is the creamcattail ID
				if (Main.tile[x, num2].TileType == ModContent.TileType<CreamCattails>()) {
					CreamcattailCheck(x, num2, ref num, ref flag);
				}
				if (!flag) {
					if (num5 == ModContent.TileType<CreamCattails>()) {
						for (int k = num; k < num2; k++) {
							if (Main.tile[x, k] != null && Main.tile[x, k].HasTile) {
								Main.tile[x, k].TileType = (ushort)num5;
								Main.tile[x, k].TileFrameY = 0;
								if (Main.netMode == NetmodeID.Server) {
									NetMessage.SendTileSquare(-1, x, num);
								}
							}
						}
						//return; //commented out since returns dont work in IL edits, id have to call a ret instruction here 
					}
				}
			});
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdloc(6),
				i => i.MatchConvI2(),
				i => i.MatchStindI2());
			c.EmitLdarg1(); //x
			c.EmitLdloc(11); //k
			c.EmitDelegate((int x, int k) => { //Adds tiletype to make sure we arent placing a CreamCattail instead
				Main.tile[x, k].TileType = TileID.Cattail;
			});
		}

		private void CreamcattailCheck(int x, int y, ref int num, ref bool flag) {
			int num2 = y;
			while ((!Main.tile[x, num2].HasTile || !Main.tileSolid[Main.tile[x, num2].TileType] || Main.tileSolidTop[Main.tile[x, num2].TileType]) && num2 < Main.maxTilesY - 50) {
				if (Main.tile[x, num2].HasTile && Main.tile[x, num2].TileType != ModContent.TileType<CreamCattails>()) {
					flag = true;
				}
				if (!Main.tile[x, num2].HasTile) {
					break;
				}
				num2++;
				if (Main.tile[x, num2] == null) {
					return;
				}
			}
			num = num2 - 1;
			while (Main.tile[x, num] != null && Main.tile[x, num].LiquidAmount > 0 && num > 50) {
				if ((Main.tile[x, num].HasTile && Main.tile[x, num].TileType != ModContent.TileType<CreamCattails>()) || Main.tile[x, num].LiquidType != 0) {
					flag = true;
				}
				num--;
				if (Main.tile[x, num] == null) {
					return;
				}
			}
			num++;
			int num3 = num;
			int num4 = 8;//WorldGen.catTailDistance;
			if (num2 - num3 > num4) {
				flag = true;
			}
			if (!Main.tile[x, num2].HasTile) {
				flag = true;
			}
			num = num2 - 1;
			if (Main.tile[x, num] != null && !Main.tile[x, num].HasTile) {
				for (int num6 = num; num6 >= num3; num6--) {
					if (Main.tile[x, num6] == null) {
						return;
					}
					if (Main.tile[x, num6].HasTile && Main.tile[x, num6].TileType == ModContent.TileType<CreamCattails>()) {
						num = num6;
						break;
					}
				}
			}
			while (Main.tile[x, num] != null && Main.tile[x, num].HasTile && Main.tile[x, num].TileType == ModContent.TileType<CreamCattails>()) {
				num--;
			}
			num++;
			if (Main.tile[x, num2 - 1] != null && Main.tile[x, num2 - 1].LiquidAmount < 127 && WorldGen.genRand.NextBool(4)) {
				flag = true;
			}
			if (Main.tile[x, num] != null && Main.tile[x, num].TileFrameX >= 180 && Main.tile[x, num].LiquidAmount > 127 && WorldGen.genRand.NextBool(4)) {
				flag = true;
			}
			if (Main.tile[x, num] != null && Main.tile[x, num2 - 1] != null && Main.tile[x, num].TileFrameX > 18) {
				if (Main.tile[x, num2 - 1].TileFrameX < 36 || Main.tile[x, num2 - 1].TileFrameX > 72) {
					flag = true;
				}
				else if (Main.tile[x, num].TileFrameX < 90) {
					flag = true;
				}
				else if (Main.tile[x, num].TileFrameX >= 108 && Main.tile[x, num].TileFrameX <= 162) {
					Main.tile[x, num].TileFrameX = 90;
				}
			}
			if (num2 > num + 4 && Main.tile[x, num + 4] != null && Main.tile[x, num + 3] != null && Main.tile[x, num + 4].LiquidAmount == 0 && Main.tile[x, num + 3].TileType == ModContent.TileType<CreamCattails>()) {
				flag = true;
			}
		}

		private bool LilyPadPreventer(On_WorldGen.orig_PlaceLilyPad orig, int x, int j) {
			int num = j;
			while (Main.tile[x, num].LiquidAmount > 0 && num > 50) {
				num--;
			}
			num++;
			int l;
			for (l = num; (!Main.tile[x, l].HasTile || !Main.tileSolid[Main.tile[x, l].TileType] || Main.tileSolidTop[Main.tile[x, l].TileType]) && l < Main.maxTilesY - 50; l++) {
				if (Main.tile[x, l].HasTile && Main.tile[x, l].TileType == ModContent.TileType<CreamCattails>()) {
					return false;
				}
			}
			return orig.Invoke(x, j);
		}
		#endregion

		#region Oats, Pads and cattails
		private bool PlaceTile(On_WorldGen.orig_PlaceTile orig, int i, int j, int Type, bool mute, bool forced, int plr, int style) {
			int num = Type;
			if (i >= 0 && j >= 0 && i < Main.maxTilesX && j < Main.maxTilesY) {
				Tile tile = Main.tile[i, j];
				if (forced || Collision.EmptyTile(i, j) || !Main.tileSolid[num] || num == ModContent.TileType<CreamGrass>() && (tile.TileType == 0 || tile.TileType == ModContent.TileType<CookieBlock>()) && tile.HasTile) {
					if (num == ModContent.TileType<CreamGrass>() && ((tile.TileType != 0 && tile.TileType != ModContent.TileType<CookieBlock>()) || !tile.HasTile)) {
						return false;
					}
				}
				if (forced || Collision.EmptyTile(i, j) || !Main.tileSolid[num]) {
					if (num == ModContent.TileType<CreamGrass_Foliage>()) {
						if (WorldGen.IsFitToPlaceFlowerIn(i, j, num)) {
							if (tile.WallType >= 0 && WallID.Sets.AllowsPlantsToGrow[tile.WallType] && Main.tile[i, j + 1].WallType >= 0 && Main.tile[i, j + 1].WallType < WallLoader.WallCount && WallID.Sets.AllowsPlantsToGrow[Main.tile[i, j + 1].WallType]) {
								if (WorldGen.genRand.NextBool(50) || WorldGen.genRand.NextBool(40)) {
									tile.HasTile = true;
									tile.TileType = (ushort)num;
									tile.TileFrameX = 144;
								}
								else if (WorldGen.genRand.NextBool(35) || (Main.tile[i, j].WallType >= 63 && Main.tile[i, j].WallType <= 70)) {
									tile.HasTile = true;
									tile.TileType = (ushort)num;
									int num3 = WorldGen.genRand.NextFromList<int>(6, 7, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
									tile.TileFrameX = (short)(num3 * 18);
								}
								else {
									tile.HasTile = true;
									tile.TileType = (ushort)num;
									tile.TileFrameX = (short)(WorldGen.genRand.Next(6) * 18);
								}
							}
						}
					}
					if (tile.LiquidAmount > 0 || tile.CheckingLiquid) {
						int num5 = num; 
						if (!TileID.Sets.Torch[num]) {
							if (num5 <= ModContent.TileType<CreamSeaOats>()) {
								if (num5 == ModContent.TileType<CreamSeaOats>()) {
									return false;
								}
							}
						}
					}
					else if (num == ModContent.TileType<CreamLilyPads>()) {
						WorldGen.PlaceLilyPad(i, j);
					}
					else if (num == ModContent.TileType<CreamCattails>()) {
						WorldGen.PlaceCatTail(i, j);
					}
					else if (num == ModContent.TileType<CreamSeaOats>()) {
						ConfectionWorldGeneration.PlantSeaOat(i, j);
					}
				}
			}
			return orig.Invoke(i, j, Type, mute, forced, plr, style);
		}
		#endregion

		#region CactusMapColor
		private void CactusMapColor(ILContext il) {
			ILCursor c = new(il);
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdsfld("Terraria.Map.MapHelper", "tileLookup"),
				i => i.MatchLdloc(7),
				i => i.MatchLdelemU2(),
				i => i.MatchStloc3());
			c.EmitLdarg0(); //i (aka X)
			c.EmitLdarg1(); //j (aka Y)
			c.EmitLdloca(3); //num5
			c.EmitDelegate((int i, int j, ref int num5) => {
				Tile tile = Main.tile[i, j];
				if (tile != null) {
					GetCactusType(i, j, tile.TileFrameX, tile.TileFrameY, out var sandType);
					if (Main.tile[i, j].TileType == TileID.Cactus && TileLoader.CanGrowModCactus(sandType) && sandType != 0 && sandType == ModContent.TileType<Creamsand>()) {
						num5 = MapHelper.tileLookup[ModContent.TileType<SprinkleCactusDudTile>()];
					}
				}
			});
		}

		public static void GetCactusType(int tileX, int tileY, int frameX, int frameY, out int type) {
			type = 0;
			int num = tileX;
			if (frameX == 36) {
				num--;
			}
			if (frameX == 54) {
				num++;
			}
			if (frameX == 108) {
				num = ((frameY != 18) ? (num + 1) : (num - 1));
			}
			int num2 = tileY;
			bool flag = false;
			Tile tile = Main.tile[num, num2];
			if (tile == null) {
				return;
			}
			if (tile.TileType == 80 && tile.HasTile) {
				flag = true;
			}
			while (tile != null && (!tile.HasTile || !Main.tileSolid[tile.TileType] || !flag)) {
				if (tile.TileType == 80 && tile.HasTile) {
					flag = true;
				}
				num2++;
				if (num2 > tileY + 20) {
					break;
				}
				if (num2 <= Main.maxTilesY)
					tile = Main.tile[num, num2];
			}
			type = tile.TileType;
		}
		#endregion

		#region Vines
		private void VineTileFrame(ILContext il) {
			var cursor = new ILCursor(il); //Code thats USED here (none commented out) is made by Ghasttear1

			// Add vine condition for conversion
			cursor.GotoNext(MoveType.Before, i => i.MatchStloc(121));
			cursor.Emit(OpCodes.Ldloc, 84); // up
			cursor.EmitDelegate((ushort origValue, int up) =>
			{
				if (up == ModContent.TileType<CreamVines>() || up == ModContent.TileType<CreamGrass>()) {
					return (ushort)ModContent.TileType<CreamVines>();
				}
				return origValue;
			});

			// Add vine condition for kill
			cursor.GotoNext(MoveType.Before, i => i.MatchStloc(122));
			cursor.Emit(OpCodes.Ldloc, 3); // num
			cursor.Emit(OpCodes.Ldloc, 84); // up
			cursor.EmitDelegate((bool origValue, int num, int up) =>
			{
				if (num == ModContent.TileType<CreamVines>() && up != ModContent.TileType<CreamGrass>()) {
					return true;
				}
				return origValue;
			});
			/*ILCursor c = new(il);
			ILLabel IL_0000 = c.DefineLabel();
			ILLabel IL_80ab = null;
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdcI4(52),
				i => i.MatchStloc(121),
				i => i.MatchLdloc(120),
				i => i.MatchBrfalse(out IL_80ab));
			if (IL_80ab == null) {
				ModContent.GetInstance<TheConfectionRebirth>().Logger.Debug("Vine Tile framing conversion could not be found");
				return;
			}
			c.Prev.Operand = IL_0000;
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdcI4(382),
				i => i.MatchStloc(121));
			c.MarkLabel(IL_0000);
			c.EmitLdloc(84); //up
			c.EmitLdloca(121); //ref num37
			c.EmitDelegate((int up, ref ushort num37) => {
				if (up == ModContent.TileType<CreamGrass>() || up == ModContent.TileType<CreamVines>()) {
					num37 = (ushort)ModContent.TileType<CreamVines>();
				}
			});
			MonoModHooks.DumpIL(this, il);*/
			/*c.GotoNext(
				MoveType.After,
				i => i.MatchLdcI4(0),
				i => i.MatchStloc(121));
			c.EmitLdloc(84); //up
			c.EmitLdloca(121); //ref num37
			c.EmitDelegate((int up, ref ushort num37) => {
				if (up == ModContent.TileType<CreamGrass>() || up == ModContent.TileType<CreamVines>()) {
					num37 = (ushort)ModContent.TileType<CreamVines>();
				}
			});
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdcI4(0),
				i => i.MatchStloc(122));
			c.EmitLdloc3(); //num
			c.EmitLdloc(84); //up
			c.EmitLdloca(122); //ref flag5
			c.EmitDelegate((int num, int up, ref bool flag5) => {
				if (num == ModContent.TileType<CreamVines>() && up != ModContent.TileType<CreamGrass>()) {
					flag5 = true;
				}
			});
			//MonoModHooks.DumpIL(this, il);*/
		}
		#endregion

		#region Creamgrass and foliage
		private bool Flowerplacement(On_WorldGen.orig_IsFitToPlaceFlowerIn orig, int x, int y, int typeAttemptedToPlace) {
			if (y < 1 || y > Main.maxTilesY - 1) {
				return false;
			}
			Tile tile = Main.tile[x, y + 1];
			if (tile.HasTile && tile.Slope == 0 && !tile.IsHalfBlock) {
				if ((tile.TileType != ModContent.TileType<CreamGrass>() && tile.TileType != ModContent.TileType<CreamGrassMowed>()) || typeAttemptedToPlace != ModContent.TileType<CreamGrass_Foliage>()) {
					return false;
				}
				return true;
			}
			return orig.Invoke(x, y, typeAttemptedToPlace);
		}

		private bool FlowerBootsEdit(On_Player.orig_DoBootsEffect_PlaceFlowersOnTile orig, Player self, int X, int Y) {
			Tile tile = Main.tile[X, Y];
			if (tile == null) {
				return false;
			}
			if (!tile.HasTile && tile.LiquidAmount == 0 && Main.tile[X, Y + 1] != null && WorldGen.SolidTile(X, Y + 1)) {
				tile.TileFrameY = 0;
				tile.Slope = 0;
				tile.IsHalfBlock = false;
				if (Main.tile[X, Y + 1].TileType == ModContent.TileType<CreamGrass>() || Main.tile[X, Y + 1].TileType == ModContent.TileType<CreamGrassMowed>()) {
					int[] ShortgrassArray = new int[] { 6, 7, 10, 15, 16, 17 };
					//if (Main.rand.Next(2) == 0) {
					tile.HasTile = true;
					tile.TileType = (ushort)ModContent.TileType<CreamGrass_Foliage>();
					tile.TileFrameX = (short)(18 * ShortgrassArray[Main.rand.Next(6)]);
					tile.CopyPaintAndCoating(Main.tile[X, Y + 1]);
					while (tile.TileFrameX == 90) {
						tile.TileFrameX = (short)(18 * Main.rand.Next(4, 7));
					}
					/*}
					else {
						tile.active(active: true);
						tile.type = 113;
						tile.frameX = (short)(18 * Main.rand.Next(2, 8));
						tile.CopyPaintAndCoating(Main.tile[X, Y + 1]);
						while (tile.frameX == 90) {
							tile.frameX = (short)(18 * Main.rand.Next(2, 8));
						}
					}*/
					if (Main.netMode == NetmodeID.MultiplayerClient) {
						NetMessage.SendTileSquare(-1, X, Y);
					}
					return true;
				}
			}
			return orig.Invoke(self, X, Y);
		}

		private void PlantTileFrameIL(ILContext il) {
			try {
				ILCursor c = new(il);
				ILLabel IL_0433 = c.DefineLabel();
				c.GotoNext(
					MoveType.Before,
					i => i.MatchLdcI4(0),
					i => i.MatchStloc2(),
					i => i.MatchLdloc1(),
					i => i.MatchLdcI4(3),
					i => i.MatchBeq(out _)
					);
				c.MarkLabel(IL_0433);
				c.GotoPrev(
					MoveType.Before,
					i => i.MatchLdloca(5),
					i => i.MatchCall<Tile>("get_type"),
					i => i.MatchPop(),
					i => i.MatchLdloc1(),
					i => i.MatchLdcI4(3),
					i => i.MatchBneUn(out _)
					);
				c.GotoPrev(
					MoveType.Before,
					i => i.MatchLdloca(5),
					i => i.MatchCall<Tile>("nactive"),
					i => i.MatchBrfalse(out _)
					);
				if (IL_0433 == null) {
					ModContent.GetInstance<TheConfectionRebirth>().Logger.Debug("Plant Check's main tile framing IlLable could not be found");
					return;
				}
				c.EmitLdloc1(); //num2
				c.EmitLdloc0(); //num
				c.EmitDelegate((int num2, int num) => {
					return (!(num2 != ModContent.TileType<CreamGrass_Foliage>() || num == ModContent.TileType<CreamGrass>() || num == ModContent.TileType<CreamGrassMowed>()));
				});
				c.EmitBrtrue(IL_0433);

				c.GotoNext(
					MoveType.After,
					i => i.MatchLdcI4(0),
					i => i.MatchStloc2()
					);
				c.EmitLdloc0(); //num
				c.EmitLdloca(1); //num2
				c.EmitLdloca(2); //flag
				c.EmitLdloca(5); //tile
				c.EmitLdarg0(); //x
				c.EmitLdarg1(); //y
				c.EmitDelegate((int num, ref int num2, ref bool flag, ref Tile tile, int x, int y) => {
					if (num2 == ModContent.TileType<CreamGrass_Foliage>()) {
						tile = Main.tile[x, y]; //The last use of the tile variable uses the coords [x + 1, y + 1], so we reset it here
						flag = tile.TileFrameX == 144; //This is supposed to convert crimson mushrooms to yumdrops but doesnt????
					}
					if (num == ModContent.TileType<CreamGrass>() || num == ModContent.TileType<CreamGrassMowed>()) {
						num2 = ModContent.TileType<CreamGrass_Foliage>();
					}
				});
			}
			catch (Exception) {
			}
		}

		private void BurnGrass(ILContext il) {
			ILCursor c = new(il);
			if (!c.TryGotoNext(
				MoveType.After,
				i => i.MatchLdloca(10),
				i => i.MatchCall<Tile>("active"), //Gets the if statement checking if the tile at tile5 is active
				i => i.MatchBrfalse(out _))) //aka this places this BEFORE the check for normal grasses (normal, hallowed, corruption, (anything that uses dirt)
			{
				ModContent.GetInstance<TheConfectionRebirth>().Logger.Debug("The Confection REBAKED: lava tile burning instructions not found");
				return;
			}
			c.EmitLdloca(10); //ref tile5
			c.EmitLdloc(8); //i
			c.EmitLdloc(9); //j
			c.EmitLdloc(0); //num
			c.EmitLdloc(1); //num2
			c.EmitDelegate((ref Tile tile5, int i, int j, int num, int num2) => {
				if (tile5.TileType == ModContent.TileType<CreamGrass>() || tile5.TileType == ModContent.TileType<CreamGrassMowed>()) { //Turns Creamgrass or golf creamgrass into Cookie block when lava is near
					tile5.TileType = (ushort)ModContent.TileType<CookieBlock>();
					WorldGen.SquareTileFrame(i, j);
					if (Main.netMode == NetmodeID.Server) {
						NetMessage.SendTileSquare(-1, num, num2, 3);
					}
				}
			});
		}

		private bool PickaxeKillTile(On_Player.orig_DoesPickTargetTransformOnKill orig, Player self, HitTile hitCounter, int damage, int x, int y, int pickPower, int bufferIndex, Tile tileTarget) {
			if (hitCounter.AddDamage(bufferIndex, damage, updateAmount: false) >= 100 && (
				tileTarget.TileType == ModContent.TileType<CreamGrass>() 
				|| tileTarget.TileType == ModContent.TileType<CreamGrassMowed>() 
				|| tileTarget.TileType == ModContent.TileType<CreamstoneMossGreen>()
				|| tileTarget.TileType == ModContent.TileType<CreamstoneMossBrown>()
				|| tileTarget.TileType == ModContent.TileType<CreamstoneMossBlue>()
				|| tileTarget.TileType == ModContent.TileType<CreamstoneMossRed>()
				|| tileTarget.TileType == ModContent.TileType<CreamstoneMossPurple>()
				|| tileTarget.TileType == ModContent.TileType<CreamstoneMossKrypton>()
				|| tileTarget.TileType == ModContent.TileType<CreamstoneMossXenon>()
				|| tileTarget.TileType == ModContent.TileType<CreamstoneMossArgon>()
				|| tileTarget.TileType == ModContent.TileType<CreamstoneMossNeon>()
				|| tileTarget.TileType == ModContent.TileType<CreamstoneMossLava>()
				|| tileTarget.TileType == ModContent.TileType<CreamstoneMossHelium>())) {
				return true;
			}
			else {
				return orig.Invoke(self, hitCounter, damage, x, y, pickPower, bufferIndex, tileTarget);
			}
		}

		private void KillConjoinedGrass_PlaceThing(On_Player.orig_PlaceThing_Tiles_PlaceIt_KillGrassForSolids orig, Player self) {
			orig.Invoke(self);
			for (int i = Player.tileTargetX - 1; i <= Player.tileTargetX + 1; i++) {
				for (int j = Player.tileTargetY - 1; j <= Player.tileTargetY + 1; j++) {
					Tile tile = Main.tile[i, j];
					if (!tile.HasTile || self.inventory[self.selectedItem].createTile == tile.TileType || (tile.TileType != ModContent.TileType<CreamGrass>() && tile.TileType != ModContent.TileType<CreamGrassMowed>())) {
						continue;
					}
					bool flag = true;
					for (int k = i - 1; k <= i + 1; k++) {
						for (int l = j - 1; l <= j + 1; l++) {
							if (!WorldGen.SolidTile(k, l)) {
								flag = false;
							}
						}
					}
					if (flag) {
						WorldGen.KillTile(i, j, fail: true);
						if (Main.netMode == NetmodeID.MultiplayerClient) {
							NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j, 1f);
						}
					}
				}
			}
		}
		#endregion
	}
}

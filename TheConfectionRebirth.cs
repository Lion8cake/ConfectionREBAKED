using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using ReLogic.Content;
using ReLogic.Peripherals.RGB;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.Achievements;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Animations;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Events;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Liquid;
using Terraria.GameContent.RGB;
using Terraria.GameContent.Skies.CreditsRoll;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.Graphics;
using Terraria.Graphics.Capture;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.Net;
using Terraria.UI;
using Terraria.Utilities;
using TheConfectionRebirth.Achievements;
using TheConfectionRebirth.Backgrounds;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Hooks;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Items.Accessories;
using TheConfectionRebirth.Items.Weapons;
using TheConfectionRebirth.NPCs;
using TheConfectionRebirth.Projectiles;
using TheConfectionRebirth.RGB;
using TheConfectionRebirth.Tiles;
using TheConfectionRebirth.Walls;
using static Terraria.Graphics.Capture.CaptureInterface;
using static Terraria.Graphics.FinalFractalHelper;
using static Terraria.WaterfallManager;
using static TheConfectionRebirth.NPCs.ConfectionGlobalNPC;

namespace TheConfectionRebirth
{
	public class TheConfectionRebirth : Mod
	{
		//Edit the following
		//WorldGen.cs
		//PlantCheck (done) (i think) - crimson mushrooms dont convert to yumdrops and vise versa for some dogshit reason - doesnt convert some purity grass correctly
		//TileFrame - Vines dont properly convert - works only if fps is lower than 30 //Im not the only one having this issue it seems //Update, issue with tml and the TileFrame method being too big
		//TODO: use tmods conversion set or smth idk havent used it before

		private Asset<Texture2D> texOuterHallow;
		private Asset<Texture2D> texOuterConfection;

		private Dictionary<int, bool> ZenithSeedWorlds = new Dictionary<int, bool>();

		public int sherbertStyle;

		public float sherbertStyleTimer;

		public static int SherbR;

		public static int SherbG;

		public static int SherbB;

		public static bool easter;

		public static bool isConfectionerBirthday;

		public static Color SherbertColor => new Color(SherbR, SherbG, SherbB);

		public static TheConfectionRebirth instance = null;

		public static ShaderData GummyWyrmShaderData { get; private set; }

		public override void Load() 
		{
			instance = this;
			ConfectionReflectionUtilities.Load();

			if (!Main.dedServ)
			{
				texOuterHallow = Assets.Request<Texture2D>("Assets/Loading/Outer_Hallow");
				texOuterConfection = Assets.Request<Texture2D>("Assets/Loading/Outer_Confection");
				GummyWyrmShaderData = new(ModContent.Request<Effect>("TheConfectionRebirth/Shaders/GummyWyrmShader", AssetRequestMode.ImmediateLoad), "GummyWyrmPass");
				Main.Chroma.RegisterShader(new ConfectionSurfaceShader(), ConfectionConditions.InConfectionMenu, ShaderLayer.Menu);
				Main.Chroma.RegisterShader(new ConfectionSurfaceShader(), ConfectionConditions.SurfaceBiome.Confection, ShaderLayer.BiomeModifier);
				Main.Chroma.RegisterShader(new UndergroundConfectionShader(), ConfectionConditions.UndergroundBiome.Confection, ShaderLayer.Biome);
				Main.Chroma.RegisterShader(new IceShader(new Color(60, 25, 10), new Color(230, 90, 20)), ConfectionConditions.UndergroundBiome.ConfectionIce, ShaderLayer.BiomeModifier);
				Main.Chroma.RegisterShader(new DesertShader(new Color(17, 11, 10), new Color(200, 90, 50)), ConfectionConditions.UndergroundBiome.ConfectionDesert, ShaderLayer.BiomeModifier);
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
			//IL_WorldGen.TileFrame += VineTileFrame; //gets garbage collected, useless
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
			//IL_NPC.SpawnNPC += LawnSpawnPrevention; //gets garbage collected, useless
			On_SmartCursorHelper.Step_GrassSeeds += CreamBeansSmartCursor;
			IL_WaterfallManager.FindWaterfalls += CloudWaterfalls;
			On_TileDrawing.DrawMultiTileVinesInWind += On_TileDrawing_DrawMultiTileVinesInWind;
			IL_Sandstorm.EmitDust += CreamsandSandstorm;
			On_WorldGen.Convert_int_int_int_int += Convert;
			IL_Player.Update += TileFallDamage;
			On_Player.PlaceThing_PaintScrapper_LongMoss += MossScapper;

			IL_UIWorldCreation.BuildPage += ConfectionSelectionMenu.ILBuildPage;
			IL_UIWorldCreation.MakeInfoMenu += ConfectionSelectionMenu.ILMakeInfoMenu;
			IL_UIWorldCreation.ShowOptionDescription += ConfectionSelectionMenu.ILShowOptionDescription;
			On_UIWorldCreation.SetDefaultOptions += ConfectionSelectionMenu.OnSetDefaultOptions;
			IL_UIWorldCreation.SetupGamepadPoints += ConfectionSelectionMenu.ILSetUpGamepadPoints;

			IL_UIGenProgressBar.DrawSelf += AddGoodToWorldgenBar;
			On_UIWorldListItem.ctor += ConfectionWorldIconEdit;
			IL_Lang.GetDryadWorldStatusDialog += DryadWorldStatusEdit;
			IL_WorldGen.AddUpAlignmentCounts += AddUpAligmenttmodEvilsandGoods;
			IL_WorldGen.CountTiles += SettmodvilsandGoods;
			On_ItemDropDatabase.RegisterBoss_Twins += On_ItemDropDatabase_RegisterBoss_Twins;
			On_Main.DrawMapFullscreenBackground += On_Main_DrawMapFullscreenBackground;
			IL_Main.SetBackColor += ConfectionBiomeLightColor;
			IL_Projectile.Damage += CosmicCookieReflection;
			On_Projectile.CanBeReflected += CosmicCookieCanBeReflect;
			On_Projectile.Shimmer += OnShimmer;
			IL_NPC.BigMimicSummonCheck += PreventCrimsonMimics;
			IL_Player.TryGettingDevArmor += ConfectionDevSets;
			On_PlayerDrawLayers.DrawPlayer_09_Wings += PreventWingDrawing;
			IL_TileDrawing.DrawSingleTile += BetterDrawEffects;
			IL_TileDrawing.DrawTiles_EmitParticles += TintTileSparkle;
			IL_PlayerDrawLayers.DrawPlayer_27_HeldItem += SherbertTorchHeldFlameEdit;
			On_Player.ItemCheck_ApplyHoldStyle_Inner += FlareGunHoldStyle;
			On_WorldGen.RandomizeBackgroundBasedOnPlayer += PreventOtherBackgroundChanges;
			On_Sandstorm.ShouldSandstormDustPersist += On_Sandstorm_ShouldSandstormDustPersist;
			On_Mount.Hover += RotatePixieMountHover;
			IL_ShopHelper.AddHappinessReportText += EditNPCHappiness;
			On_Main.UpdateTime_StartDay += StartDay_SpecialDates;
			On_NPC.BannerID += VariantBanners;
			On_Main.DrawInfoAccs += ReplaceCounterLastHit;
			IL_CreditsRollComposer.FillSegments += FillCreditSegmentILEdit;
			IL_CreditsRollEvent.TryStartingCreditsRoll += CreditsRollIngameTimeDurationExtention;
			IL_CreditsRollEvent.UpdateTime += CreditsRollIngameTimeDurationExtention;
			IL_CreditsRollEvent.SetRemainingTimeDirect += CreditsRollIngameTimeDurationExtention;
			On_NPCKillsTracker.GetKillCount_NPC += syncKillCount;
			IL_NPC.UpdateCollision += preventModdedSandsharkCollision;
			On_NPC.ApplyTileCollision += SandsharkCollision;
			On_CommonCode.ModifyItemDropFromNPC += ItemDropColors;
			IL_CaptureInterface.ModeChangeSettings.DrawWaterChoices += DrawModdedCaptureIcons;
			On_CaptureBiome.GetCaptureBiome += makeCaptureBiomeSlot;
			On_CaptureInterface.ModeChangeSettings.GetRect += increaseCaptureSettingsHeight;
			IL_CaptureInterface.ModeChangeSettings.Draw += moveCaptureDefaultsText;
			IL_CaptureInterface.ModeChangeSettings.Update += moveCaptureDefaultsHitbox;
			On_NPC.UpdateNPC_CritterSounds += ModNPCCritterSounds;

			IL_AchievementAdvisor.Initialize += EditAchievementRecomendations;
			//Edit the init BEFORE calling
			if (!Main.dedServ)
			{
				Main.AchievementAdvisor.SetCards(new List<AchievementAdvisorCard>());
				Main.AchievementAdvisor.Initialize();
			}

			On_AchievementAdvisorCard.IsAchievableInWorld += IsAchieveableInConfectionWorld;
			IL_Recipe.UpdateWhichItemsAreMaterials += RemoveMaterialFromUnusedRecipeGroups;
		}

		public override void Unload() {
			On_Player.PlaceThing_Tiles_PlaceIt_KillGrassForSolids -= KillConjoinedGrass_PlaceThing;
			On_Player.DoesPickTargetTransformOnKill -= PickaxeKillTile;
			IL_Liquid.DelWater -= BurnGrass;
			IL_WorldGen.PlantCheck -= PlantTileFrameIL;
			On_Player.DoBootsEffect_PlaceFlowersOnTile -= FlowerBootsEdit;
			On_WorldGen.IsFitToPlaceFlowerIn -= Flowerplacement;
			On_WorldGen.PlaceTile -= PlaceTile;
			//IL_WorldGen.TileFrame -= VineTileFrame;
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
			//IL_NPC.SpawnNPC -= LawnSpawnPrevention;
			On_SmartCursorHelper.Step_GrassSeeds -= CreamBeansSmartCursor;
			IL_WaterfallManager.FindWaterfalls -= CloudWaterfalls;
			On_TileDrawing.DrawMultiTileVinesInWind -= On_TileDrawing_DrawMultiTileVinesInWind;
			IL_Sandstorm.EmitDust -= CreamsandSandstorm;
			On_WorldGen.Convert_int_int_int_int -= Convert;
			IL_Player.Update -= TileFallDamage;
			On_Player.PlaceThing_PaintScrapper_LongMoss -= MossScapper;
			IL_UIGenProgressBar.DrawSelf -= AddGoodToWorldgenBar;
			On_UIWorldListItem.ctor -= ConfectionWorldIconEdit;
			IL_Lang.GetDryadWorldStatusDialog -= DryadWorldStatusEdit;
			IL_WorldGen.AddUpAlignmentCounts -= AddUpAligmenttmodEvilsandGoods;
			IL_WorldGen.CountTiles -= SettmodvilsandGoods;
			On_ItemDropDatabase.RegisterBoss_Twins -= On_ItemDropDatabase_RegisterBoss_Twins;
			On_Main.DrawMapFullscreenBackground -= On_Main_DrawMapFullscreenBackground;
			IL_Main.SetBackColor -= ConfectionBiomeLightColor;
			IL_Projectile.Damage -= CosmicCookieReflection;
			On_Projectile.CanBeReflected -= CosmicCookieCanBeReflect;
			On_Projectile.Shimmer -= OnShimmer;
			IL_NPC.BigMimicSummonCheck -= PreventCrimsonMimics;
			IL_Player.TryGettingDevArmor -= ConfectionDevSets;
			On_PlayerDrawLayers.DrawPlayer_09_Wings -= PreventWingDrawing;
			IL_TileDrawing.DrawSingleTile -= BetterDrawEffects;
			IL_TileDrawing.DrawTiles_EmitParticles -= TintTileSparkle;
			IL_PlayerDrawLayers.DrawPlayer_27_HeldItem -= SherbertTorchHeldFlameEdit;
			On_Player.ItemCheck_ApplyHoldStyle_Inner -= FlareGunHoldStyle;
			On_WorldGen.RandomizeBackgroundBasedOnPlayer -= PreventOtherBackgroundChanges;
			On_Sandstorm.ShouldSandstormDustPersist -= On_Sandstorm_ShouldSandstormDustPersist;
			On_Mount.Hover -= RotatePixieMountHover;
			IL_ShopHelper.AddHappinessReportText -= EditNPCHappiness;
			On_Main.UpdateTime_StartDay -= StartDay_SpecialDates;
			On_NPC.BannerID -= VariantBanners;
			On_Main.DrawInfoAccs -= ReplaceCounterLastHit;
			IL_CreditsRollComposer.FillSegments -= FillCreditSegmentILEdit;
			IL_CreditsRollEvent.TryStartingCreditsRoll -= CreditsRollIngameTimeDurationExtention;
			IL_CreditsRollEvent.UpdateTime -= CreditsRollIngameTimeDurationExtention;
			IL_CreditsRollEvent.SetRemainingTimeDirect -= CreditsRollIngameTimeDurationExtention;
			On_NPCKillsTracker.GetKillCount_NPC -= syncKillCount;
			IL_NPC.UpdateCollision -= preventModdedSandsharkCollision;
			On_NPC.ApplyTileCollision -= SandsharkCollision;
			On_CommonCode.ModifyItemDropFromNPC -= ItemDropColors;
			IL_CaptureInterface.ModeChangeSettings.DrawWaterChoices -= DrawModdedCaptureIcons;
			On_CaptureBiome.GetCaptureBiome -= makeCaptureBiomeSlot;
			On_CaptureInterface.ModeChangeSettings.GetRect -= increaseCaptureSettingsHeight;
			IL_CaptureInterface.ModeChangeSettings.Draw -= moveCaptureDefaultsText;
			IL_CaptureInterface.ModeChangeSettings.Update -= moveCaptureDefaultsHitbox;
			On_NPC.UpdateNPC_CritterSounds -= ModNPCCritterSounds;

			IL_AchievementAdvisor.Initialize -= EditAchievementRecomendations;
			//Edit the init BEFORE calling
			if (!Main.dedServ)
			{
				Main.AchievementAdvisor.SetCards(new List<AchievementAdvisorCard>());
				Main.AchievementAdvisor.Initialize();
			}

			On_AchievementAdvisorCard.IsAchievableInWorld -= IsAchieveableInConfectionWorld;
			IL_Recipe.UpdateWhichItemsAreMaterials -= RemoveMaterialFromUnusedRecipeGroups;

			var fractalProfiles = (Dictionary<int, FinalFractalProfile>)typeof(FinalFractalHelper).GetField("_fractalProfiles", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
			fractalProfiles.Remove(ModContent.ItemType<TrueSucrosa>());
			fractalProfiles.Remove(ModContent.ItemType<Sucrosa>());

			ConfectionReflectionUtilities.Unload();
		}


		public override object Call(params object[] args)
		{
			//For Content creators: Message me (Lion8cake) on discord if you have any mod call suggestions
			return args switch
			{
			["confectionorHallow"] => ConfectionWorldGeneration.confectionorHallow,
			["SetconfectionorHallow", bool boolean] => ConfectionWorldGeneration.confectionorHallow = boolean,

			//IDs
			["ConvertsToConfection", int tileID, int num] => ConfectionIDs.Sets.ConvertsToConfection[tileID] = num,
			["SoulofLightOnlyItem", int itemID, bool flag] => ConfectionIDs.Sets.RecipeBlacklist.SoulofLightOnlyItem[itemID] = flag,
			["SoulofNightOnlyItem", int itemID, bool flag] => ConfectionIDs.Sets.RecipeBlacklist.SoulofNightOnlyItem[itemID] = flag,
			["DarkShardOnlyItem", int itemID, bool flag] => ConfectionIDs.Sets.RecipeBlacklist.DarkShardOnlyItem[itemID] = flag,
			["PixieDustOnlyItem", int itemID, bool flag] => ConfectionIDs.Sets.RecipeBlacklist.PixieDustOnlyItem[itemID] = flag,
			["UnicornHornOnlyItem", int itemID, bool flag] => ConfectionIDs.Sets.RecipeBlacklist.UnicornHornOnlyItem[itemID] = flag,
			["CrystalShardOnlyItem", int itemID, bool flag] => ConfectionIDs.Sets.RecipeBlacklist.CrystalShardOnlyItem[itemID] = flag,
			["HallowedBarOnlyItem", int itemID, bool flag] => ConfectionIDs.Sets.RecipeBlacklist.HallowedBarOnlyItem[itemID] = flag,
			["PrincessFishOnlyItem", int itemID, bool flag] => ConfectionIDs.Sets.RecipeBlacklist.PrincessFishOnlyItem[itemID] = flag,
			["PrismiteOnlyItem", int itemID, bool flag] => ConfectionIDs.Sets.RecipeBlacklist.PrismiteOnlyItem[itemID] = flag,
			["ChaosFishOnlyItem", int itemID, bool flag] => ConfectionIDs.Sets.RecipeBlacklist.ChaosFishOnlyItem[itemID] = flag,
			["HallowedSeedsOnlyItem", int itemID, bool flag] => ConfectionIDs.Sets.RecipeBlacklist.HallowedSeedsOnlyItem[itemID] = flag,
			["PearlstoneOnlyItem", int itemID, bool flag] => ConfectionIDs.Sets.RecipeBlacklist.PearlstoneOnlyItem[itemID] = flag,
				_ => throw new Exception("TheConfectionRebirth: Unknown mod call, make sure you are calling the right method/field with the right parameters!")
			};
		}

		#region RecipeGroupMaterialTextPatcher
		private void RemoveMaterialFromUnusedRecipeGroups(ILContext il)
		{
			ILCursor c = new(il);
			ILLabel IL_0099 = null;
			int groupItem_varNum = -1;
			int recipeGround_varNum = -1;

			c.GotoNext(MoveType.After, i => i.MatchLdfld<RecipeGroup>("ValidItems"), i => i.MatchCallvirt(out _), i => i.MatchStloc(out _), i => i.MatchBr(out IL_0099), i => i.MatchLdloca(out recipeGround_varNum), i => i.MatchCall(out _), i => i.MatchStloc(out groupItem_varNum));
			c.EmitLdloca(groupItem_varNum);
			c.EmitLdloca(recipeGround_varNum);
			c.EmitDelegate((ref int item, ref RecipeGroup value) =>
			{
				bool isActuallyUsed = false;
				for (int i = 0; i < Recipe.numRecipes; i++)
				{
					if (Main.recipe[i].HasRecipeGroup(value))
					{
						isActuallyUsed = true;
						break;
					}
				}
				if (isActuallyUsed)
				{
					ItemID.Sets.IsAMaterial[item] = true;
				}
			});
			c.EmitBr(IL_0099);
		}
		#endregion

		#region Achievement edits
		private bool IsAchieveableInConfectionWorld(On_AchievementAdvisorCard.orig_IsAchievableInWorld orig, AchievementAdvisorCard self)
		{
			if (self.achievement.Name == ModContent.GetInstance<DrixerMixer>().Achievement.Name)
			{
				return ConfectionWorldGeneration.confectionorHallow;
			}
			else if (self.achievement.Name == "DRAX_ATTAX")
			{
				return !ConfectionWorldGeneration.confectionorHallow;
			}
			return orig.Invoke(self);
		}

		private void EditAchievementRecomendations(ILContext il)
		{
			ILCursor c = new ILCursor(il);
			int achievementIndex_varNum = -1;

			c.EmitLdarga(0);
			c.EmitDelegate((ref AchievementAdvisor self) =>
			{
				self.SetCards(new List<AchievementAdvisorCard>());
			});

			c.GotoNext(MoveType.Before, i => i.MatchLdarg(0), i => i.MatchLdfld<AchievementAdvisor>("_cards"), i => i.MatchCall<Main>("get_Achievements"), i => i.MatchLdstr("DRAX_ATTAX"), i => i.MatchCallvirt<AchievementManager>("GetAchievement"), i => i.MatchLdloc(out achievementIndex_varNum));
			c.EmitLdloca(achievementIndex_varNum);
			c.EmitLdarga(0);
			c.EmitDelegate((ref float num, ref AchievementAdvisor self) =>
			{
				List<AchievementAdvisorCard> _cards = self.GetCards();
				_cards.Add(new AchievementAdvisorCard(ModContent.GetInstance<DrixerMixer>().Achievement, num++));
				self.SetCards(_cards);
			});
		}
		#endregion

		#region Friendly NPC Sounds
		private void ModNPCCritterSounds(On_NPC.orig_UpdateNPC_CritterSounds orig, NPC self)
		{
			orig.Invoke(self);
			if (self.type == ModContent.NPCType<NPCs.Birdnana>() || self.type == ModContent.NPCType<NPCs.Pip>())
			{
				if (!Main.dayTime || !(Main.time < 18000.0))
				{
					return;
				}
				int maxValue = 400;
				if (!Main.rand.NextBool(maxValue))
				{
					return;
				}
				if (!Main.rand.NextBool(3))
				{
					SoundEngine.PlaySound(SoundID.Bird, self.position); //style 14, styles dont exist for sound ID 32 (bird)
				}
				else
				{
					SoundEngine.PlaySound(SoundID.Bird, self.position); //style 18, styles dont exist for sound ID 32 (bird)
				}
			}
			else if (self.type == ModContent.NPCType<NPCs.ChocolateFrog>())
			{
				if ((double)Math.Abs(self.velocity.X) < 0.5 && (!Main.dayTime || (double)self.position.Y > Main.worldSurface * 16.0) && Main.rand.NextBool(200))
				{
					SoundEngine.PlaySound(SoundID.Frog, self.position);
				}
			}
		}
		#endregion

		//TODO: (patch Il edits to be dynamic)
		#region Capture Biome Icons 
		internal static int biomeCaptureCount = 1; //Confection, Confection Desert

		internal static int[] biomeCapturesIndexs = new int[biomeCaptureCount];

		/// <summary>
		/// Sets the Icon texture, the texture's frame's X position, and the icon text. Make sure you check if the captureIconID is yours before editing the icon/frame X
		/// </summary>
		/// <param name="captureIconID"></param>
		/// <param name="iconHitbox"></param>
		/// <param name="mouse"></param>
		/// <param name="iconTexture"></param>
		/// <param name="textureXposition"></param>
		internal void CaptureIconSetInfo(int captureIconID, Rectangle iconHitbox, Point mouse, ref Texture2D iconTexture, ref int textureXposition)
		{
			if (captureIconID == biomeCapturesIndexs[0]) //if icon is our 1st icon
			{
				iconTexture = (Texture2D)Assets.Request<Texture2D>("Assets/ConfectionCaptureBiomeIcon"); //edit texture to be ours
				textureXposition = 0; //frame X for our texture
				if (iconHitbox.Contains(mouse))
				{
					Main.instance.MouseText(Language.GetTextValue("Mods.TheConfectionRebirth.CaptureBiomeChoice.Confection"), 0, 0); //text for our icon
				}
			}
		}

		/// <summary>
		/// returns the capture biome of the current icon loaded. Use chosenbiome to get the icon ID of the icon selected. Return null to set to the default capture biome.
		/// </summary>
		/// <param name="chosenBiome"></param>
		/// <returns></returns>
		internal CaptureBiome? CaptureIconBiomeSettings(int chosenBiome)
		{
			if (chosenBiome == biomeCapturesIndexs[0]) //biomeChoice is bacially the icon ID
			{
				return new CaptureBiome(ModContent.GetInstance<ConfectionSurfaceBackgroundStyle>().Slot, ModContent.GetInstance<CreamWaterStyle>().Slot); //create a new capture biome
			}
			return null;
		}

		private static int maxModdedCaptureCounts = 0;

		private void moveCaptureDefaultsHitbox(ILContext il)
		{
			//offsets the hitbox of the reset to defaults button
			ILCursor c = new(il);
			c.GotoNext(MoveType.After, i => i.MatchMul(), i => i.MatchAdd(), i => i.MatchStfld<Rectangle>("Y"));
			c.EmitLdloc(3); //i
			c.EmitLdloca(1); //ref rect
			c.EmitLdloc(2);
			c.EmitDelegate((int i, ref Rectangle rectangle, int y) =>
			{
				int y2 = y + i * 20;
				if (i == 6)
				{
					rectangle.Y = y2 + 24 * ((int)Math.Ceiling(((double)maxModdedCaptureCounts + 13.00) / 7.00) - 2);
				}
			});
		}

		private void moveCaptureDefaultsText(ILContext il)
		{
			ILCursor c = new(il);
			c.GotoNext(MoveType.After, i => i.MatchCall<Color>("get_White"), i => i.MatchStloc(4)); //offsets the visual placement of the reset to defaults button
			c.EmitLdloc(1); //i
			c.EmitLdloca(0); //ref rect
			c.EmitLdarg(0);
			c.EmitDelegate((int i, ref Rectangle rect, object self) =>
			{
				Rectangle rect2 = GetRect();
				if (i == 6)
				{
					rect.Y = rect2.Y + 24 * ((int)Math.Ceiling(((double)maxModdedCaptureCounts + 13.00) / 7.00) - 2);
				}
			});
			//in code, the icons for the biome settings is actually placed AFTER the reset to defaults button but is visually displaced to be above
			//so here we have to reset back to the original Y so the icons aren't offset
			//c.GotoNext(MoveType.After, i => i.MatchCall("Terraria.UI.Chat.ChatManager", "DrawColorCodedStringWithShadow"), i => i.MatchPop()); //due to an issue with monomod, we are unable to match to these instructions
			c.GotoNext(MoveType.Before, i => i.MatchLdloc1(), i => i.MatchLdcI4(1), i => i.MatchAdd(), i => i.MatchStloc(1));
			c.EmitLdloc(1);
			c.EmitLdloca(0);
			c.EmitLdarg(0);
			c.EmitDelegate((int i, ref Rectangle rect, object self) =>
			{
				Rectangle rect2 = GetRect();
				if (i == 6)
				{
					rect.Y = rect2.Y;
				}
			});
		}

		private Rectangle GetRect()
		{
			Rectangle result = new(0, 0, 224, 170);
			if (Settings.ScreenAnchor == 0)
			{
				result.X = 227 - result.Width / 2;
				result.Y = 80;
			}
			if (maxModdedCaptureCounts > 0)
			{
				result.Height = 170 + 24 * ((int)Math.Ceiling(((double)maxModdedCaptureCounts + 13.00) / 7.00) - 2); //offset the back panel
			}
			return result;
		}

		private Rectangle increaseCaptureSettingsHeight(On_CaptureInterface.ModeChangeSettings.orig_GetRect orig, object self)
		{
			Rectangle rect = orig.Invoke(self);
			if (maxModdedCaptureCounts > 0)
			{
				rect.Height = 170 + 24 * ((int)Math.Ceiling(((double)maxModdedCaptureCounts + 13.00) / 7.00) - 2); //offset the back panel
			}
			return rect;
		}

		private CaptureBiome makeCaptureBiomeSlot(On_CaptureBiome.orig_GetCaptureBiome orig, int biomeChoice)
		{
			CaptureBiome? capture = CaptureIconBiomeSettings(biomeChoice);
			if (capture != null)
			{
				return capture;
			}
			return orig.Invoke(biomeChoice);
		}

		private void DrawModdedCaptureIcons(ILContext il)
		{
			ILCursor c = new ILCursor(il);
			ILLabel IL_01ca = c.DefineLabel();
			ILLabel IL_0000 = c.DefineLabel();

			//add your mod's icons
			//Bit choppy as it uses X of the icon frame to get the mod icon count from all the mods, any other mod that doesnt use this solution may break this edit
			c.GotoNext(MoveType.After, i => i.MatchCall<Rectangle>(".ctor")); //go after the rectangle to save the modded icons to the X, the X gets rewritten down the line but this allows us to get a full collection of all the modded icons added by other mods
			c.EmitLdloca(0); //ref r
			c.EmitDelegate((ref Rectangle r) =>
			{
				int prevCount = r.X; //get the current modded icon count from the rectangle's X
				if (biomeCaptureCount > 0) //check if this mod has more than 0 icons
				{
					for (int p = 0; p < biomeCaptureCount; p++)
					{
						r.X++; //incriment the modded icon count for every icon added
						biomeCapturesIndexs[p] = r.X + 12; //index is saved as modded icon number + 12 (12 is the vanilla icon count
					}
				}
			});

			//add continue loop
			c.GotoNext(MoveType.After, i => i.MatchBeq(out IL_01ca)); //get the continue (ie the step the continue jumps to
			c.MarkLabel(IL_0000); //mark the lable for where the if statement should jump out to 
			c.GotoPrev(MoveType.Before, i => i.MatchLdcI4(1), i => i.MatchBneUn(out _)); //find the i == 1 condition, seperate the i from the i == 1, use the i in the delegate and then inject a new i to not break opcodes
			c.EmitLdloc(0); //r
			c.EmitLdloc(2); //j (or x)
			c.EmitDelegate((int i, Rectangle r, int j) => {
				return i >= ((int)Math.Ceiling(((double)r.X + 13.00) / 7.00) - 1) && j >= (r.X + 13) - (i * 7); //the if statement for the continue statement, this makes all other icons outside of the bounds of whats added to be not drawn
			});
			c.EmitBrtrue(IL_01ca); //if the return was true, continue
			c.EmitBr(IL_0000); //jump to the rest of the code, skipping the 'if (i == 1 && j == 6) { continue; }'
			c.EmitLdloc(1); //readd the i used by the delegate to not break opcodes, despite this code now being unreachable

			//save icon count
			c.GotoNext(MoveType.Before, i => i.MatchLdloca(0), i => i.MatchLdarg(2), i => i.MatchLdfld<Point>("X")); //before r.X is overritten
			c.EmitLdloc(0); //r
			c.EmitDelegate((Rectangle r) =>
			{
				maxModdedCaptureCounts = r.X; //save the maximum number of modded icons, this is to readd this number later as well as to get a local count of all modded icons added
			});

			//Add offset
			//This is to manually add an offset for added icons, vanilla manually adds a 12 pixel offset for the second row but here we use a calculation to offset the X if there is not enough in a row
			c.GotoNext(MoveType.After, i => i.MatchLdcI4(12), i => i.MatchLdloc1(), i => i.MatchMul()); //find where 12 * i is in the r.X calculation
			c.EmitLdloc(1); //i (or y)
			c.EmitDelegate((int outdatedOffset, int i) => //the old offset is here, this is for any other mod as their offset delegate will be used here if modded icons are already added
			{
				if (i >= (int)Math.Ceiling(((double)maxModdedCaptureCounts + 13.00) / 7.00) - 1) //if at the last row
				{
					return (int)(24 * ((double)(7 - ((maxModdedCaptureCounts + 13) - (i * 7))) / 2.00)); //return offset based on the final row count
				}
				return 0; //otherwise return 0 to not offset
			});

			//Modify Textures
			//here mods modify their assets, text and their frame X position of the image, frame X is included for any mod that was to string their icons in a row like Extra 130
			c.GotoNext(MoveType.After, i => i.MatchCall<Color>("get_White"), i => i.MatchPop()); //go after the unused color local variable
			c.EmitLdloc(3); //num2
			c.EmitLdloc(0); //r
			c.EmitLdarg(3); //mouse
			c.EmitLdloca(5); //value
			c.EmitLdloca(6); //x
			c.EmitDelegate((int captureIconID, Rectangle iconHitbox, Point mouse, ref Texture2D iconTexture, ref int textureXposition) =>
			{
				CaptureIconSetInfo(captureIconID, iconHitbox, mouse, ref iconTexture, ref textureXposition);
			});

			//reset mod icon count
			//c.GotoNext(MoveType.After, i => i.MatchCall<Color>("op_Multiply"), i => i.MatchCallvirt<SpriteBatch>("Draw")); //after the last draw call //due to a monomod issue, these instruction matchings do not work well
			c.GotoNext(MoveType.Before, i => i.MatchLdloc2(), i => i.MatchLdcI4(1), i => i.MatchAdd(), i => i.MatchStloc2());
			c.EmitLdloca(0); //ref r
			c.EmitDelegate((ref Rectangle r) =>
			{
				r.X = maxModdedCaptureCounts; //readd the modded icon count to the r.X
			});

			//modify the Y loop
			c.GotoNext(MoveType.After, i => i.MatchLdloc(1), i => i.MatchLdcI4(2)); //find the i < 2
			c.EmitLdloc(0); //r
			c.EmitDelegate((int oldMaxY, Rectangle r) => //as the same as the offset, we get the old maximum Y number as it will also get other mod maximum Y's
			{
				return (int)Math.Ceiling(((double)r.X + 13.00) / 7.00); //calculate the number of rows there are depending on the count of icons
			});
		}
		#endregion

		#region SandsharkEdits
		private void preventModdedSandsharkCollision(ILContext il)
		{
			ILCursor c = new(il);
			ILLabel IL_0175 = c.DefineLabel();
			c.GotoNext(MoveType.After, i => i.MatchLdarg(0), i => i.MatchLdfld<NPC>("type"), i => i.MatchLdcI4(72), i => i.MatchBeq(out IL_0175));
			c.EmitLdarg0();
			c.EmitDelegate((NPC self) =>
			{
				return self.type == ModContent.NPCType<SacchariteSharpnose>();
			});
			c.EmitBrtrue(IL_0175);
		}

		private void SandsharkCollision(On_NPC.orig_ApplyTileCollision orig, NPC self, bool fall, Vector2 cPosition, int cWidth, int cHeight)
		{
			if (self.type == ModContent.NPCType<SacchariteSharpnose>())
			{
				typeof(NPC).GetMethod("Collision_MoveSandshark", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { fall, cPosition, cWidth, cHeight });
			}
			else
			{
				orig.Invoke(self, fall, cPosition, cWidth, cHeight);
			}
		}

		private void ItemDropColors(On_CommonCode.orig_ModifyItemDropFromNPC orig, NPC npc, int itemIndex)
		{
			Item item = Main.item[itemIndex];
			if (item.type == ItemID.SharkFin && npc.type == ModContent.NPCType<SacchariteSharpnose>())
			{
				item.color = new Color(182, 115, 82, 255);
				NetMessage.SendData(MessageID.ItemTweaker, -1, -1, null, itemIndex, 1f);
			}
			else
			{
				orig.Invoke(npc, itemIndex);
			}
		}
		#endregion

		#region credits
		private SegmentInforReport PlaySegment_ModdedTextRoll(CreditsRollComposer self, int startTime, string sourceCategory, Vector2 anchorOffset = default(Vector2))
		{
			//We have our own text roll segment due to tmodloader using Hjson instead of json meaning that sometimes the order of names becomes backwards
			//if you want to use vanilla text for some reason i would recomend that you reflect CreditsRollComposer.PlaySegment_TextRoll
			List<IAnimationSegment> _segments = (List<IAnimationSegment>)typeof(CreditsRollComposer).GetField("_segments", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(self);
			anchorOffset.Y -= 40f;
			int num = 80;
			LocalizedText[] array = Language.FindAll(Lang.CreateDialogFilter(sourceCategory + ".", null));
			for (int i = 0; i < array.Length; i++)
			{
				_segments.Add(new Segments.LocalizedTextSegment(startTime + i * num, Language.GetText(sourceCategory + "." + (i + 1)), anchorOffset));
			}
			SegmentInforReport result = default(SegmentInforReport);
			result.totalTime = array.Length * num + num * -1;
			return result;
		}

		private SegmentInforReport PlaySegment_LionEightCake_HungryStyalist(CreditsRollComposer self, int startTime, Vector2 sceneAnchorPosition)
		{
			//Our own animation, reffer to the Terraria.GameContent.Skies.Credits.CreditsRollComposer for examples of used and unused animations
			List<IAnimationSegment> _segments = (List<IAnimationSegment>)typeof(CreditsRollComposer).GetField("_segments", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(self);
			Vector2 _backgroundOffset = (Vector2)typeof(CreditsRollComposer).GetField("_backgroundOffset", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(self);
			Vector2 _originAtBottom = (Vector2)typeof(CreditsRollComposer).GetField("_originAtBottom", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(self);
			Vector2 _emoteBubbleOffsetWhenOnRight = (Vector2)typeof(CreditsRollComposer).GetField("_emoteBubbleOffsetWhenOnRight", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(self);
			Vector2 _emoteBubbleOffsetWhenOnLeft = (Vector2)typeof(CreditsRollComposer).GetField("_emoteBubbleOffsetWhenOnLeft", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(self);
			Vector2 GetSceneFixVector = (Vector2)typeof(CreditsRollComposer).GetMethod("GetSceneFixVector", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Invoke(self, new object[] { });
			//Reflection chunk for all the little trinkets that use it

			sceneAnchorPosition += GetSceneFixVector;
			int duration = startTime; //Set an initial time
			sceneAnchorPosition.X += 10;
			Asset<Texture2D> SceneAsset = ModContent.Request<Texture2D>("TheConfectionRebirth/Assets/ConfectionScene", AssetRequestMode.ImmediateLoad);  //Make sure that its ImmediateLoad otherwise 90% of the time it wont load
			Rectangle SceneAssetFrame = SceneAsset.Frame();
			DrawData SceneAssetDrawData = new DrawData(SceneAsset.Value, Vector2.Zero, SceneAssetFrame, Color.White, 0f, SceneAssetFrame.Size() * new Vector2(0.5f, 1f) + new Vector2((float)10, -42f), 1f, (SpriteEffects)0);
			Segments.AnimationSegmentWithActions<Segments.LooseSprite> SceneAssetSegment = new Segments.SpriteSegment(SceneAsset, duration, SceneAssetDrawData, sceneAnchorPosition + _backgroundOffset).UseShaderEffect(new Segments.SpriteSegment.MaskedFadeEffect()).Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 60));
			_segments.Add(SceneAssetSegment); //Spawn the background
			Segments.AnimationSegmentWithActions<NPC> StyalistNPCSegment = new Segments.NPCSegment(startTime, NPCID.Stylist, sceneAnchorPosition + new Vector2(-100f, 0f), _originAtBottom).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51)).Then(new Actions.NPCs.LookAt(1))
				.Then(new Actions.NPCs.Move(new(1.7f, 0f), 170)); //spawn the stylist moving right at a speed of 1.7
			Segments.EmoteSegment HungryEmote = new Segments.EmoteSegment(EmoteID.Hungry, duration, 120, sceneAnchorPosition + new Vector2(-116f, 0f) + _emoteBubbleOffsetWhenOnLeft + new Vector2(1.7f, 0f) * (float)10, (SpriteEffects)1, new Vector2(1.7f, 0f));
			//Emote with hungry (she is very hungry)
			SceneAssetSegment.Then(new Actions.Sprites.Wait((int)StyalistNPCSegment.DedicatedTimeNeeded));
			duration += (int)StyalistNPCSegment.DedicatedTimeNeeded;
			StyalistNPCSegment.Then(new Actions.NPCs.Move(new(0.6f, 0f), 51)).With(new Actions.NPCs.Fade(5, 51)); //Fade and slow in movement after the movement duration
			duration += 51;
			StyalistNPCSegment.Then(new Actions.NPCs.Wait(90)).With(new Actions.NPCs.LookAt(-1)); //Make the styalist look left
			duration += 90; //Wait a second and a half
			StyalistNPCSegment.Then(new Actions.NPCs.Move(new(-1.7f, 0f), 160)).With(new Actions.NPCs.Fade(-5, 51)); //move the the stylist right
			Segments.EmoteSegment RUNEmote = new Segments.EmoteSegment(EmoteID.EmoteRun, duration, 120, sceneAnchorPosition + new Vector2(206f, 0f) + _emoteBubbleOffsetWhenOnRight + new Vector2(1.7f, 0f) * (float)10, (SpriteEffects)0, new Vector2(-1.7f, 0f));
			//Emote with run (she is very scared)
			duration += 30;
			Segments.AnimationSegmentWithActions<NPC> hoardEnemy1 = new Segments.NPCSegment(duration, ModContent.NPCType<SweetGummy>(), sceneAnchorPosition + new Vector2(250f, 0f), _originAtBottom).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51)).Then(new Actions.NPCs.LookAt(-1))
				.Then(new Actions.NPCs.Move(new(-1.7f, 0f), 130));
			hoardEnemy1.Then(new Actions.NPCs.Move(new(-0.6f, 0f), 51)).With(new Actions.NPCs.Fade(5, 51));
			duration += 7; //Spawn a sweet gummy and wait 7 frames
			Segments.AnimationSegmentWithActions<NPC> hoardEnemy2 = new Segments.NPCSegment(duration, ModContent.NPCType<WildWilly>(), sceneAnchorPosition + new Vector2(250f, 0f), _originAtBottom).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51)).Then(new Actions.NPCs.LookAt(-1))
				.Then(new Actions.NPCs.Move(new(-1.7f, 0f), 123));
			hoardEnemy2.Then(new Actions.NPCs.Move(new(-0.6f, 0f), 51)).With(new Actions.NPCs.Fade(5, 51));
			duration += 22; //Spawn a Wild Willy and wait 22 frames
			Segments.AnimationSegmentWithActions<NPC> hoardEnemy3 = new Segments.NPCSegment(duration, ModContent.NPCType<IcecreamGal>(), sceneAnchorPosition + new Vector2(250f, 0f), _originAtBottom).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51)).Then(new Actions.NPCs.LookAt(-1))
				.Then(new Actions.NPCs.Move(new(-1.7f, 0f), 101));
			hoardEnemy3.Then(new Actions.NPCs.Move(new(-0.6f, 0f), 51)).With(new Actions.NPCs.Fade(5, 51));
			duration += 20; //Spawn a Icecream Gal and wait 20 frames
			/*Segments.AnimationSegmentWithActions<NPC> hoardEnemy4 = new Segments.NPCSegment(duration, ModContent.NPCType<NPCs.CreamsandWitchPhase2>(), sceneAnchorPosition + new Vector2(250f, 0f), _originAtBottom).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51)).Then(new Actions.NPCs.LookAt(-1))
				.Then(new Actions.NPCs.Move(new(-1.7f, 0f), 81));
			hoardEnemy4.Then(new Actions.NPCs.Move(new(-0.6f, 0f), 51)).With(new Actions.NPCs.Fade(5, 51));*/
			duration += 24; //Spawn a Creamsand witch (standing npc) and wait 24 frames
			Asset<Texture2D> rollerxCookieTexture = ModContent.Request<Texture2D>("TheConfectionRebirth/NPCs/Rollercookie", AssetRequestMode.ImmediateLoad); //Make sure that its ImmediateLoad otherwise 90% of the time it wont load
			int FrameCountX = 9; //For less repeated code
			int FrameCountY = 4; //For less repeated code
			Rectangle rollerCookieFrame = rollerxCookieTexture.Frame(FrameCountX, FrameCountY, 0, 0);
			DrawData rollerCookieDrawData = new DrawData(rollerxCookieTexture.Value, Vector2.Zero, rollerCookieFrame, Color.White, 0f, (rollerxCookieTexture.Size() / new Vector2(FrameCountX, FrameCountY)) / 2f, 1f, (SpriteEffects)0);
			Segments.AnimationSegmentWithActions<Segments.LooseSprite> hoardEnemy5 = new Segments.SpriteSegment(rollerxCookieTexture, duration, rollerCookieDrawData, sceneAnchorPosition + new Vector2(250f, -28f)).Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 51)).Then(new Actions.Sprites.SimulateGravity(new Vector2(-1.7f, 0f), Vector2.Zero, -1.7f * 0.05f, 57));
			hoardEnemy5.Then(new Actions.Sprites.SimulateGravity(new Vector2(-0.8f, 0f), Vector2.Zero, -0.8f * 0.05f, 51)).With(new Actions.Sprites.Fade(0f, 51)); //We use Simulate Gravity to make the Roller cookie rotate and move
			StyalistNPCSegment.Then(new Actions.NPCs.Move(new(-0.6f, 0f), 51)).With(new Actions.NPCs.Fade(5, 51)); //Final bits of movement for the Styalist
			SceneAssetSegment.Then(new Actions.Sprites.Wait(230));
			duration += 60;
			SceneAssetSegment.Then(new Actions.Sprites.Wait(130)).With(new Actions.Sprites.Fade(0f, 127)); //Fade the background frame

			_segments.Add(StyalistNPCSegment); //Spawn each element/segment 
			_segments.Add(HungryEmote);
			_segments.Add(RUNEmote);
			_segments.Add(hoardEnemy1);
			_segments.Add(hoardEnemy2);
			_segments.Add(hoardEnemy3);
			/*_segments.Add(hoardEnemy4);*/
			_segments.Add(hoardEnemy5);
			duration += 120; //Give a final duration time until the next part of the credits loads
			SegmentInforReport FinalDurationTime = default(SegmentInforReport);
			FinalDurationTime.totalTime = duration - startTime;
			return FinalDurationTime; //Return the duration of the animation so the next text or animation can play fluently and straight after
		}

		private void FillCreditSegmentILEdit(ILContext il)
		{
			ILCursor c = new ILCursor(il); //place a IL Cursor
			int num_varNum = -1;
			int val2_varNum = -1; //we save the variable IDs to be used
			int num3_varNum = -1; //variable numbers can shift if variables are added by tmodloader, so to prevent the code from being broken, we dynamically get and use the IDs
			c.GotoNext(i => i.MatchLdcI4(210), i => i.MatchStloc(out num3_varNum));
			c.GotoNext(MoveType.Before, i => i.MatchLdloc(out num_varNum), i => i.MatchLdarg0(), i => i.MatchLdloc(num_varNum), i => i.MatchLdstr("CreditsRollCategory_Creator"), i => i.MatchLdloc(out val2_varNum));
			//make sure all instructions match, movetype will place our code before the first instruction once all instructions match
			c.EmitLdarg(0); //Emit ldarg_0 (self)
			c.EmitLdloca(num_varNum); //Emit ldloc_0 (num)
			c.EmitLdloca(num3_varNum); //Emit ldloc_2 (num3)
			c.EmitLdloca(val2_varNum); //Emit ldloc_3 (vector2 or val2)
			c.EmitDelegate((CreditsRollComposer self, ref int num, ref int num3, ref Vector2 vector2) => { //Get the needed variables and instance
																										   //Edit inside here for more text and animations, shown here is just how to add 1 text and 1 animation
				num += PlaySegment_ModdedTextRoll(self, num, "Mods.TheConfectionRebirth.CreditsRollCategory_ConfectionTeam", vector2).totalTime; //Play our credit text
				num += num3; //wait
				num += PlaySegment_LionEightCake_HungryStyalist(self, num, vector2).totalTime; //Play our custom animation
				num += num3; //wait
			});
		}

		private void CreditsRollIngameTimeDurationExtention(ILContext il)
		{
			ILCursor c = new ILCursor(il); //place a IL cursor
			c.GotoNext(MoveType.After, i => i.MatchLdcI4(28800)); //Look for a LDC I4 instruction with 28800 (all timers use this)
			c.EmitDelegate((int maxDuration) => maxDuration + 60 * 35); //Adds ontop of the max duration to account for the custom credits
		}
		#endregion

		//Reminder, may be unstable (idk its been 6 months)
		#region NPCTypeVariantsFixes
		private void ReplaceCounterLastHit(On_Main.orig_DrawInfoAccs orig, Main self)
		{
			int lastHitType = Main.LocalPlayer.lastCreatureHit;
			if (lastHitType == ModContent.NPCType<BirthdayCookie>())
			{
				Main.LocalPlayer.lastCreatureHit = ModContent.NPCType<Rollercookie>();
			}
			if (lastHitType == ModContent.NPCType<Sprinkler>() || 
				lastHitType == ModContent.NPCType<Sprinkling_Halloween1>() || 
				lastHitType == ModContent.NPCType<Sprinkler_Halloween1>() ||
				lastHitType == ModContent.NPCType<Sprinkling_Halloween2>() ||
				lastHitType == ModContent.NPCType<Sprinkler_Halloween2>() ||
				lastHitType == ModContent.NPCType<Sprinkling_Xmas>() ||
				lastHitType == ModContent.NPCType<Sprinkler_Xmas>())
			{
				Main.LocalPlayer.lastCreatureHit = ModContent.NPCType<Sprinkling>();
			}
			orig.Invoke(self);
		}

		private int VariantBanners(On_NPC.orig_BannerID orig, NPC self)
		{
			int currentType = orig.Invoke(self);
			if (currentType == ModContent.NPCType<BirthdayCookie>())
			{
				currentType = ModContent.NPCType<Rollercookie>();
			}
			if (currentType == ModContent.NPCType<Sprinkling_Halloween1>() ||
				currentType == ModContent.NPCType<Sprinkling_Halloween2>() ||
				currentType == ModContent.NPCType<Sprinkling_Xmas>())
			{
				currentType = ModContent.NPCType<Sprinkling>();
			}
			if (currentType == ModContent.NPCType<Sprinkler>() || 
				currentType == ModContent.NPCType<Sprinkler_Halloween1>() || 
				currentType == ModContent.NPCType<Sprinkler_Halloween2>() || 
				currentType == ModContent.NPCType<Sprinkler_Xmas>())
			{
				currentType = NPCID.None; //Prevents sprinklers from counting as a kill for sprinklings when killed
			}
			return currentType;
		}

		private int syncKillCount(On_NPCKillsTracker.orig_GetKillCount_NPC orig, NPCKillsTracker self, NPC npc)
		{
			int prevKills = orig.Invoke(self, npc);
			if (npc.type == ModContent.NPCType<Sprinkling>() || npc.type == ModContent.NPCType<Sprinkler>())
			{
				prevKills = NPC.killCount[ModContent.NPCType<Sprinkling>()];
			}
			return prevKills;
		}
		#endregion

		#region SpecialTimesandDates
		public static void checkBirthdays()
		{
			DateTime now = DateTime.Now;
			int day = now.Day;
			int month = now.Month;

			bool confectionBirthday = day == 3 && month == 12;
			bool lionsBirthday = day == 2 && month == 10;
			bool darkBirthday = day == 21 && month == 10;
			bool larfBirthday = day == 19 && month == 8;
			bool snickerBirthday = day == 9 && month == 9;
			bool redsBirthday = day == 12 && month == 11;
			bool cenxBirthday = day == 29 && month == 11;

			if (confectionBirthday || lionsBirthday || darkBirthday || larfBirthday || snickerBirthday || redsBirthday || cenxBirthday)
			{
				isConfectionerBirthday = true;
			}
			else
			{
				isConfectionerBirthday = false;
			}
		}

		public static void checkEaster()
		{
			DateTime now = DateTime.Now;
			int year = now.Year;
			int dayOfYear = now.DayOfYear;
			FindEasterTime(year, out int dedDay, out int dedMonth);
			int daySince0 = 0;
			while (dedMonth > 0)
			{
				daySince0 = DateTime.DaysInMonth(year, dedMonth);
				dedMonth--;
			}
			daySince0 += dedDay;
			int rangeAround = 7; //puts a week surrounding before and after easter
			int minDays = daySince0 - rangeAround;
			int maxDays = daySince0 + rangeAround;

			bool isFestiveDay = dayOfYear >= minDays && dayOfYear <= maxDays;

			if (isFestiveDay)
			{
				easter = true;
			}
			else
			{
				easter = false;
			}
		}

		public static void FindEasterTime(int year, out int easterDay, out int easterMonth)
		{
			//Used wikipedia for the calculations over this
			//good greif
			int a = year % 19;
			int b = year % 4;
			int c = year % 7;
			int k = year / 100;
			int p = (13 + 8 * k) / 25;
			int q = k / 4;
			int M = (15 - p + k - q) % 30;
			int N = (4 + k - q) % 7;

			int d = (19 * a + M) % 30;
			int e = (2 * b + 4 * c + 6 * d + N) % 7;

			easterDay = 22 + d + e;
			easterMonth = 3;
			if (easterDay > 31)
			{
				easterMonth++;
				easterDay = d + e - 9;
			}
		}

		private void StartDay_SpecialDates(On_Main.orig_UpdateTime_StartDay orig, ref bool stopEvents)
		{
			checkEaster();
			checkBirthdays();
			if (Main.drunkWorld && Main.netMode != NetmodeID.MultiplayerClient)
			{
				ConfectionWorldGeneration.confectionorHallow = !ConfectionWorldGeneration.confectionorHallow;
			}
			orig.Invoke(ref stopEvents);
		}
		#endregion

		#region Edit Happiness text
		private void EditNPCHappiness(ILContext il)
		{
			ILCursor c = new(il);
			int textVW_numVar = -1;
			int text_varNum = -1;
			c.GotoNext(i => i.MatchLdloc(out text_varNum), i => i.MatchLdstr("Transformed"));
			c.GotoNext(MoveType.After, i => i.MatchCall("Terraria.Localization.Language", "GetTextValueWith"), i => i.MatchStloc(out textVW_numVar));
			c.EmitLdloca(textVW_numVar);
			c.EmitLdloc(text_varNum);
			c.EmitLdarg(1);
			c.EmitLdarg(2);
			c.EmitDelegate((ref string textValueWith, string targetNPC, string textKeyInCategory, object substitutes) =>
			{
				string valueToCheck = substitutes == null ? "" : substitutes.ToString();
				bool inConfection = valueToCheck.Contains(Language.GetTextValue("Mods.TheConfectionRebirth.Biomes.ConfectionBiome.TownNPCDialogueName"));
				bool nurse = textKeyInCategory == "LikeBiome" && targetNPC == "TownNPCMood_Nurse";
				bool bartender = textKeyInCategory == "LikeBiome" && targetNPC == "TownNPCMood_DD2Bartender";
				bool partyGrill = textKeyInCategory == "LikeBiome" && targetNPC == "TownNPCMood_PartyGirl";
				bool wizard = textKeyInCategory == "LikeBiome" && targetNPC == "TownNPCMood_Wizard";
				bool clothier = textKeyInCategory == "DislikeBiome" && targetNPC == "TownNPCMood_Clothier";
				bool witchDoctor = textKeyInCategory == "DislikeBiome" && targetNPC == "TownNPCMood_WitchDoctor";
				bool taxCollector = textKeyInCategory == "DislikeBiome" && targetNPC == "TownNPCMood_TaxCollector";
				
				if ((nurse || bartender || partyGrill || wizard || clothier || witchDoctor || taxCollector) && inConfection)
				{
					textValueWith = Language.GetTextValueWith("Mods.TheConfectionRebirth." + targetNPC + "." + textKeyInCategory + "_Confection", substitutes);
				}
			});
		}
		#endregion

		#region Pixie Mount
		private bool RotatePixieMountHover(On_Mount.orig_Hover orig, Mount self, Player mountedPlayer)
		{
			bool flag = orig.Invoke(self, mountedPlayer);
			if (self._type == ModContent.MountType<Mounts.PixieStickMount>())
			{
				float value = (0f - mountedPlayer.velocity.Y) / self._data.dashSpeed;
				value = MathHelper.Clamp(value, -1f, 1f);
				float value2 = mountedPlayer.velocity.X / self._data.dashSpeed;
				value2 = MathHelper.Clamp(value2, -1f, 1f);
				float num12 = -(float)Math.PI / 16f * value * (float)mountedPlayer.direction;
				float num3 = (float)Math.PI / 16f * value2;
				float fullRotation3 = num12 + num3;
				mountedPlayer.fullRotation = fullRotation3;
				mountedPlayer.fullRotationOrigin = new Vector2((float)(mountedPlayer.width / 2), (float)mountedPlayer.height);
			}
			return flag;
		}
		#endregion

		#region Prevent forest background randomising when in the confection
		private void PreventOtherBackgroundChanges(On_WorldGen.orig_RandomizeBackgroundBasedOnPlayer orig, UnifiedRandom random, Player player)
		{
			if (player.InModBiome<ConfectionBiome>() && !player.ZoneDesert)
			{
				WorldGen.BackgroundsCache.UpdateCache();
			}
			else
			{
				orig.Invoke(random, player);
			}
		}
		#endregion

		#region Sherbet Flare Flare Gun hold out dust
		private void FlareGunHoldStyle(On_Player.orig_ItemCheck_ApplyHoldStyle_Inner orig, Player self, float mountOffset, Item sItem, Rectangle heldItemFrame)
		{
			orig.Invoke(self, mountOffset, sItem, heldItemFrame);
			if (sItem.holdStyle == 1 && !self.pulley)
			{
				if (!Main.dedServ && sItem.type == ItemID.FlareGun)
				{
					float x = self.position.X + (float)(self.width / 2) + (float)(38 * self.direction);
					if (self.direction == 1)
					{
						x -= 10f;
					}
					float y = self.MountedCenter.Y - 4f * self.gravDir;
					if (self.gravDir == -1f)
					{
						y -= 8f;
					}
					self.RotateRelativePoint(ref x, ref y);
					int num3 = 0;
					for (int i = 54; i < 58; i++)
					{
						if (self.inventory[i].stack > 0 && self.inventory[i].ammo == 931)
						{
							num3 = self.inventory[i].type;
							break;
						}
					}
					if (num3 == 0)
					{
						for (int j = 0; j < 54; j++)
						{
							if (self.inventory[j].stack > 0 && self.inventory[j].ammo == 931)
							{
								num3 = self.inventory[j].type;
								break;
							}
						}
					}
					if (num3 == ModContent.ItemType<Items.Weapons.SherbetFlare>())
					{
						int num4 = Dust.NewDust(new Vector2(x, y + self.gfxOffY), 6, 6, ModContent.DustType<SherbetDust>(), 0f, 0f, 100, default(Color), 1.6f);
						Main.dust[num4].noGravity = true;
						Main.dust[num4].velocity.Y -= 4f * self.gravDir;
						Main.dust[num4].color = SherbertColor;
						Main.dust[num4].scale *= 0.5f;
						Dust obj = Main.dust[num4];
						obj.velocity *= 0.75f;
					}
				}
			}
		}
		#endregion

		#region Sherbert Torch Held Flame Edit
		private void SherbertTorchHeldFlameEdit(ILContext il)
		{
			ILCursor c = new(il);
			int num_varNum = -1;
			int color4_varNum = -1;
			c.GotoNext(i => i.MatchLdloc(out _), i => i.MatchLdfld<Item>("type"), i => i.MatchStloc(out num_varNum));
			c.GotoNext(MoveType.After, i => i.MatchLdloca(out color4_varNum), i => i.MatchLdcI4(100), i => i.MatchLdcI4(100), i => i.MatchLdcI4(100), i => i.MatchLdcI4(0), i => i.MatchCall<Color>(".ctor"), 
				i => i.MatchLdcI4(7), i => i.MatchStloc(out _),
				i => i.MatchLdcR4(1f), i => i.MatchStloc(out _),
				i => i.MatchLdcR4(0), i => i.MatchStloc(out _));
			c.EmitLdloc(num_varNum);
			c.EmitLdloca(color4_varNum);
			c.EmitDelegate((int num, ref Color color4) =>
			{
				if (num == ModContent.ItemType<Items.Placeable.SherbetTorch>())
				{
					color4 = new Color(SherbR, SherbG, SherbB, 0);
				}
			});
		}
		#endregion

		#region Tile sparkle tint
		private void TintTileSparkle(ILContext il)
		{
			ILCursor c = new(il);
			int color_varNum = -1;
			c.GotoNext(MoveType.After,i => i.MatchRet(), i => i.MatchCall<Color>("get_White"),  i => i.MatchStloc(out color_varNum));
			c.EmitLdarg(1); //x
			c.EmitLdarg(2); //y
			c.EmitLdarg(3); //tileCache
			c.EmitLdarg(4); //typeCache
			c.EmitLdloca(color_varNum); //ref newColor
			c.EmitDelegate((int i, int j, Tile tileCache, int typeCache, ref Color tileShineColor) =>
			{
				if (typeCache == ModContent.TileType<CreamstoneSaphire>())
				{
					tileShineColor = new Color(0, 0, 255, 255);
				}
				if (typeCache == ModContent.TileType<CreamstoneRuby>())
				{
					tileShineColor = new Color(255, 0, 0, 255);
				}
				if (typeCache == ModContent.TileType<CreamstoneEmerald>())
				{
					tileShineColor = new Color(0, 255, 0, 255);
				}
				if (typeCache == ModContent.TileType<CreamstoneTopaz>())
				{
					tileShineColor = new Color(255, 255, 0, 255);
				}
				if (typeCache == ModContent.TileType<CreamstoneAmethyst>())
				{
					tileShineColor = new Color(255, 0, 255, 255);
				}
				if (typeCache == ModContent.TileType<CreamstoneDiamond>())
				{
					tileShineColor = new Color(255, 255, 255, 255);
				}
				/*if (typeCache == ModContent.TileType<CreamstoneAmber>())
				{
					tileShineColor = new Color(255, 255, 0, 255);
				}*/
			});
		}
		#endregion

		//TODO: dynamically retrive var nums
		#region fix for DrawEffects being broken for ModTile
		private void BetterDrawEffects(ILContext il)
		{
			ILCursor c = new(il);
			c.GotoNext(MoveType.After, i => i.MatchLdfld<TileDrawInfo>("colorTint"), i => i.MatchCall<TileDrawing>("GetFinalLight"), i => i.MatchStfld<TileDrawInfo>("finalColor"));
			c.EmitLdarg(6); //tileX
			c.EmitLdarg(7); //tileY
			c.EmitLdarg(1); //instanced TileDrawInfo type is gotten from
			c.EmitLdfld(typeof(TileDrawInfo).GetField("typeCache")); //type
			c.EmitLdarga(1); //drawData ref 
			c.EmitDelegate((int i, int j, int type, ref TileDrawInfo drawdata) =>
			{
				//Use Main.spriteBatch for rendering
				if (type == ModContent.TileType<SherbetBricks>())
				{
					Color color = new Color(SherbR, SherbG, SherbB, 255);
					if (Main.tile[i, j].IsActuated)
					{
						color = ConfectionWorldGeneration.ActColor(color, Main.tile[i, j]);
					}
					drawdata.finalColor = color;
				}
			});
		}
		#endregion

		#region Prevent Wing Rendering (WHY DOES TMOD NOT SUPPORT THIS????)
		private void PreventWingDrawing(On_PlayerDrawLayers.orig_DrawPlayer_09_Wings orig, ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.wings == ModContent.GetInstance<WildAiryBlue>().Item.wingSlot)
			{
				return;
			}
			orig.Invoke(ref drawinfo);
		}
		#endregion

		#region Drop Confection Dev Sets
		private void ConfectionDevSets(ILContext il)
		{
			ILCursor c = new ILCursor(il);
			c.GotoNext(MoveType.After, i => i.MatchCall<Main>("get_rand"), i => i.MatchLdcI4(18));
			c.EmitDelegate((int oldVal) => {
				int currentDevsetCount = 1; //we only have 1 dev set
				return oldVal + currentDevsetCount;
			});
			c.GotoNext(MoveType.After, i => i.MatchLdloc(out _), i => i.MatchSwitch(out _));
			c.EmitLdarg1();
			c.EmitLdarg0();
			c.EmitDelegate((IEntitySource source, Player player) => //emitting this delegate after the switch basically gives the switch a default to land to if no other options are chosen
			{
				//no need for a second switch as we only have 1 dev set
				player.QuickSpawnItem(source, ModContent.ItemType<Items.Armor.SnickerDevOutfit.ConeHead>());
				player.QuickSpawnItem(source, ModContent.ItemType<Items.Armor.SnickerDevOutfit.Knickercobbler>());
				player.QuickSpawnItem(source, ModContent.ItemType<Items.Armor.SnickerDevOutfit.Unicookie>());
			});
		}
		#endregion

		//TODO: dynamically retrive var nums
		#region Stop Key of Night spawning Crimson Mimics and add modded keys
		private void PreventCrimsonMimics(ILContext il)
		{
			ILCursor c = new(il);
			ILLabel label = c.DefineLabel();
			//key of light
			c.GotoNext(MoveType.After, i => i.MatchLdcI4(0), i => i.MatchBle(out _));
			c.EmitLdloc(0);
			c.EmitLdloc(4);
			c.EmitDelegate((int num, int i) =>
			{
				return Main.chest[num].item[i].type == ModContent.ItemType<KeyofDelight>() || Main.chest[num].item[i].type == ModContent.ItemType<KeyofSpite>();
			});
			c.EmitBrtrue(label);

			c.GotoNext(MoveType.After, i => i.MatchLdcI4(3092), i => i.MatchBneUn(out _));
			c.MarkLabel(label);

			c.GotoNext(MoveType.After, i => i.MatchStloc1());
			c.EmitLdloc(0);
			c.EmitLdloc(4);
			c.EmitLdarg(2);
			c.EmitDelegate((int num, int i, Player user) =>
			{
				if (Main.chest[num].item[i].type == ModContent.ItemType<KeyofDelight>() || Main.chest[num].item[i].type == ModContent.ItemType<KeyofSpite>())
				{
					Main.player[user.whoAmI].GetModPlayer<ConfectionPlayer>().mimicSpawnKeyType = Main.chest[num].item[i].type;
				}
			});

			c.GotoNext(MoveType.After, i => i.MatchLdcI4(475), i => i.MatchStloc(8));
			c.EmitLdloca(8); //NPC type
			c.EmitLdloca(2); //evil check
			c.EmitLdloca(1); //good check
			c.EmitLdarg(2);
			c.EmitDelegate((ref int npcType, ref int isEvilMimic, ref int isGoodMimic, Player user) =>
			{
				if (isEvilMimic == 1)
				{
					npcType = NPCID.BigMimicCorruption; 
				}
				if (isGoodMimic == 1 && Main.player[user.whoAmI].GetModPlayer<ConfectionPlayer>().mimicSpawnKeyType > ItemID.None) //all modded keys go here because of weird logic the key of night's IL code is done by preventing easy access to making an 'or' statement
				{
					npcType = Main.player[user.whoAmI].GetModPlayer<ConfectionPlayer>().mimicSpawnKeyType == ModContent.ItemType<KeyofSpite>() ? NPCID.BigMimicCrimson : ModContent.NPCType<BigMimicConfection>();
				}
				isEvilMimic = 0;
			});
		}
		#endregion

		#region Projectile Shimmer Interactions
		private void OnShimmer(On_Projectile.orig_Shimmer orig, Projectile self)
		{
			Projectile Projectile = self;
			if ((Projectile.type == ModContent.ProjectileType<Projectiles.CreamofKickin>()) && (Projectile.ai[0] == 2f || Projectile.ai[0] == 4f))
			{
				return;
			}
			if (Projectile.type == ModContent.ProjectileType<Projectiles.CreamofKickin>())
			{
				if (Projectile.velocity.Y > 0f)
				{
					Projectile.velocity.Y *= -1f;
					Projectile.netUpdate = true;
				}
				Projectile.velocity.Y -= 0.4f;
				if (Projectile.velocity.Y < -8f)
				{
					Projectile.velocity.Y = -8f;
				}
			}
			orig.Invoke(self);
		}
		#endregion

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
			int bgColorSet_varNum = -1;
			c.GotoNext(i => i.MatchCall<Color>("get_White"), i => i.MatchStloc(out bgColorSet_varNum));
			c.GotoNext(MoveType.After, i => i.MatchLdarg(2), i => i.MatchLdloc(out _), i => i.MatchConvU1(), i => i.MatchCall<Color>("set_B"),
				i => i.MatchLdcI4(15), i => i.MatchStloc(out _));
			c.EmitLdloca(bgColorSet_varNum);
			c.EmitLdarga(1);
			c.EmitDelegate((ref Color bgColorToSet, ref Color sunColor) =>
			{
				float ConfectionBiomeInfluence = (float)ModContent.GetInstance<ConfectionBiomeTileCount>().confectionBlockCount / (float)ConfectionBiomeTileCount.ConfectionTileMax;
				if (MenuLoader.CurrentMenu.Name == ModContent.GetInstance<ConfectionMenu>().Name)
				{
					ModContent.GetInstance<ConfectionMenu>().menuScreenTint += 20;
					if (ModContent.GetInstance<ConfectionMenu>().menuScreenTint >= ConfectionBiomeTileCount.ConfectionTileMax)
					{
						ModContent.GetInstance<ConfectionMenu>().menuScreenTint = ConfectionBiomeTileCount.ConfectionTileMax;
					}
				}
				else
				{
					ModContent.GetInstance<ConfectionMenu>().menuScreenTint -= 20;
					if (ModContent.GetInstance<ConfectionMenu>().menuScreenTint < 0)
					{
						ModContent.GetInstance<ConfectionMenu>().menuScreenTint = 0;
					}
				}
				if (Main.gameMenu)
				{
					ConfectionBiomeInfluence = (float)ModContent.GetInstance<ConfectionMenu>().menuScreenTint / (float)ConfectionBiomeTileCount.ConfectionTileMax;
				}
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

		//TODO: add dynamic varibale getters
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
		private void ConfectionWorldIconEdit(On_UIWorldListItem.orig_ctor orig, UIWorldListItem self, WorldFileData data, int orderInList, bool canBePlayed)
		{
			orig.Invoke(self, data, orderInList, canBePlayed);
			bool confData = self.Data.TryGetHeaderData(ModContent.GetInstance<ConfectionWorldGeneration>(), out var _data);
			UIElement WorldIcon = (UIElement)typeof(UIWorldListItem).GetField("_worldIcon", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self);
			WorldFileData Data = (WorldFileData)typeof(AWorldListItem).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self);
			if (confData)
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
					if (!ZenithSeedWorlds.ContainsKey(worldIcon.UniqueId))
					{
						ZenithSeedWorlds.Add(worldIcon.UniqueId, false);
					}
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
				i => i.MatchLdloc(out _),
				i => i.MatchCall("Terraria.Utils", "TopLeft"));
			c.EmitLdcI4(0); //new Rectange(0, 0, 266, 70)
			c.EmitLdcI4(0);
			c.EmitLdcI4(266);
			c.EmitLdcI4(70);
			c.EmitNewobj(typeof(Rectangle).GetConstructor([typeof(int), typeof(int), typeof(int), typeof(int)]));
			c.GotoNext(MoveType.Before, i => i.MatchCallvirt<SpriteBatch>("Draw"));
			c.EmitDelegate((SpriteBatch spriteBatch, Texture2D tex, Vector2 topLeft, Rectangle rect, Color white) => {
				spriteBatch.Draw(tex, topLeft, rect, white);
				bool flag = ConfectionWorldGeneration.confectionorHallow;
				if (WorldGen.drunkWorldGen && Main.rand.NextBool(2))
				{
					flag = !flag;
				}
				spriteBatch.Draw(flag ? texOuterConfection.Value : texOuterHallow.Value, topLeft, white);
			}); //thanks alf for the delegate :sob:
			c.EmitLdarg(1);
			c.EmitDelegate(() =>
			{
				return TextureAssets.MagicPixel.Value;
			});
			c.EmitDelegate(() =>
			{
				return Vector2.Zero;
			});
			c.EmitDelegate(() =>
			{
				return Color.Transparent;
			});
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
		private void Convert(On_WorldGen.orig_Convert_int_int_int_int orig, int i, int j, int conversionType, int size) {
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
					if (wall == ModContent.WallType<CookieWall>() || wall == ModContent.WallType<CookieWallArtificial>())
					{
						Main.tile[k, l].WallType = WallID.DirtUnsafe;
						WorldGen.SquareWallFrame(k, l);
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else if (wall == ModContent.WallType<CookieStonedWall>() || wall == ModContent.WallType<CookieStonedWallArtificial>()) {
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
					else if (Main.tile[k, l].TileType == (ushort)ModContent.TileType<SacchariteBlock>() || Main.tile[k, l].TileType == (ushort)ModContent.TileType<EnchantedSacchariteBlock>())
					{
						if (Main.rand.NextBool(3))
						{
							WorldGen.KillTile(k, l);
						}
						else
						{
							WorldGen.KillTile(k, l, noItem: true);
						}
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
			int weightedRand_varNum = -1;
			c.GotoNext(i => i.MatchNewobj<WeightedRandom<Color>> (".ctor"), i => i.MatchStloc(out weightedRand_varNum));
			c.GotoNext(MoveType.Before,
				i => i.MatchLdcR4(0.2f),
				i => i.MatchLdcR4(0.35f),
				i => i.MatchLdsfld<Sandstorm>("Severity"),
				i => i.MatchCall("Microsoft.Xna.Framework.MathHelper", "Lerp"),
				i => i.MatchStloc(out _));
			c.EmitLdloca(weightedRand_varNum);
			c.EmitDelegate((ref WeightedRandom<Color> weightedRandom) => {
				weightedRandom.Add(new Color(99, 57, 46),
					Main.SceneMetrics.GetTileCount((ushort)ModContent.TileType<Tiles.Creamsand>()) +
					Main.SceneMetrics.GetTileCount((ushort)ModContent.TileType<Tiles.Creamsandstone>()) +
					Main.SceneMetrics.GetTileCount((ushort)ModContent.TileType<Tiles.HardenedCreamsand>()));
			});
		}

		private bool On_Sandstorm_ShouldSandstormDustPersist(On_Sandstorm.orig_ShouldSandstormDustPersist orig)
		{
			if (Sandstorm.Happening && Main.LocalPlayer.ZoneSandstorm && (Main.bgStyle == 2 || Main.bgStyle == 5 || Main.bgStyle == ModContent.GetInstance<ConfectionSandSurfaceBackgroundStyle>().Slot))
			{
				return Main.bgDelay < 50;
			}
			return orig.Invoke();
		}
		#endregion

		#region WindEdits
		private void On_TileDrawing_DrawMultiTileVinesInWind(On_TileDrawing.orig_DrawMultiTileVinesInWind orig, TileDrawing self, Vector2 screenPosition, Vector2 offSet, int topLeftX, int topLeftY, int sizeX, int sizeY) 
		{
			if (Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<Tiles.Furniture.CreamwoodChandelier>()) 
			{
				sizeX = 3;
				sizeY = 3;
			}
			else if (Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<Tiles.Furniture.CreamwoodLantern>())
			{
				sizeX = 1;
				sizeY = 2;
			}
			else if (Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<Tiles.Furniture.SacchariteChandelier>()) 
			{
				sizeX = 3;
				sizeY = 3;
			}
			else if (Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<Tiles.Furniture.SacchariteLantern>())
			{
				sizeX = 1;
				sizeY = 2;
			}
			else if (Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<CherryBugBottle>()) 
			{
				sizeY = 2;
			}
			else if (Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<RoyalCherryBugBottle>()) 
			{
				sizeY = 2;
			}
			else if (Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<SoulofDelightinaBottle>()) 
			{
				sizeY = 2;
			}
			else if (Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<SoulofSpiteinaBottle>()) 
			{
				sizeY = 2;
			}
			orig.Invoke(self, screenPosition, offSet, topLeftX, topLeftY, sizeX, sizeY);
		}
		#endregion

		//TODO: from this point down, make variables dynamic
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
						num5 = MapHelper.tileLookup[ModContent.TileType<CreamOasisPlants>()];
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
			bool flag = orig.Invoke(x, y, typeAttemptedToPlace);
			Tile tile = Main.tile[x, y + 1];
			if (tile.HasTile && tile.Slope == 0 && !tile.IsHalfBlock) {
				if ((tile.TileType == ModContent.TileType<CreamGrass>() || tile.TileType == ModContent.TileType<CreamGrassMowed>()) && typeAttemptedToPlace != ModContent.TileType<CreamGrass_Foliage>()) {
					flag = false;
				}
			}
			return flag;
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
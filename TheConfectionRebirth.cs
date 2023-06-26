using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.RuntimeDetour.HookGen;
using ReLogic.Content;
using System;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using TheConfectionRebirth.Backgrounds;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items.Archived;
using TheConfectionRebirth.ModSupport;
using System.Collections.Generic;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using System.Linq;
using Terraria.IO;
using Terraria.ModLoader.IO;
using System.IO;
using Terraria.GameContent.ItemDropRules;
using static TheConfectionRebirth.NPCs.BagDrops;
using Terraria.Graphics.Effects;
using TheConfectionRebirth.Hooks;
using Terraria.Localization;
using Terraria.GameContent.Personalities;

namespace TheConfectionRebirth {
	public class TheConfectionRebirth : Mod
	{

		public static int[] confectBG = new int[3];

		//\ private variables i cannot be btohered reflecting
		private static float bgScale = 1f;

		private double bgParallax;

		private int bgStartX;

		private int bgLoops;

		private int bgStartY;

		private int bgLoopsY;

		private int bgTopY;

		private static int bgWidthScaled = (int)(1024f * bgScale);

		private float scAdj;

		internal float screenOff;

		private static TileTest v = new();
		public static bool OurFavoriteDay => new DateTimeMatch(DateTime.Now, new DateTime(2022, 12, 11), new DateTime(2022, 10, 2), new DateTime(2022, 5, 16)).ToBoolean();
		public static TileTest tileMerge => v;

		public class TileTest {
			public bool this[int tile1, int tile2] {
				get => Main.tileMerge[tile1][tile2] || Main.tileMerge[tile2][tile1];
				set => ConfectionUtils.Merge(tile1, tile2);
			}
		}

		private struct DateTimeMatch {
			private readonly bool value;
			public DateTimeMatch(DateTime time, params DateTime[] matchFor) {
				value = false;
				foreach (var d in matchFor) {
					if (time.Day.Equals(d.Day) && time.Month.Equals(d.Month)) {
						value = true;
						break;
					}
				}
			}

			public bool ToBoolean() => value;
		}

		public override void PostSetupContent()
		{
			SummonersShineThoughtBubble.PostSetupContent();
			StackableBuffData.PostSetupContent();
			ModSupport.ModSupportBaseClass.HookAll();
		}

		public override void Load()
		{
			Terraria.On_Main.UpdateAudio_DecideOnTOWMusic += Main_UpdateAudio_DecideOnTOWMusic;

			Terraria.GameContent.UI.States.IL_UIWorldCreation.BuildPage += ConfectionSelectionMenu.ILBuildPage;
			Terraria.GameContent.UI.States.IL_UIWorldCreation.MakeInfoMenu += ConfectionSelectionMenu.ILMakeInfoMenu;
			Terraria.GameContent.UI.States.IL_UIWorldCreation.ShowOptionDescription +=
				ConfectionSelectionMenu.ILShowOptionDescription;

			Terraria.GameContent.UI.States.On_UIWorldSelect.UpdateWorldsList += On_UIWorldSelect_UpdateWorldsList;
			Terraria.On_Player.MowGrassTile += On_Player_MowGrassTile;
			Terraria.GameContent.ItemDropRules.On_ItemDropDatabase.RegisterBoss_Twins += On_ItemDropDatabase_RegisterBoss_Twins;
			On_Lang.GetDryadWorldStatusDialog += On_Lang_GetDryadWorldStatusDialog;
		}

		public override void Unload()
		{
			On_Main.UpdateAudio_DecideOnTOWMusic -= Main_UpdateAudio_DecideOnTOWMusic;
			Terraria.GameContent.UI.States.On_UIWorldSelect.UpdateWorldsList -= On_UIWorldSelect_UpdateWorldsList;
			Terraria.On_Player.MowGrassTile -= On_Player_MowGrassTile;
			Terraria.GameContent.ItemDropRules.On_ItemDropDatabase.RegisterBoss_Twins -= On_ItemDropDatabase_RegisterBoss_Twins;
			On_Lang.GetDryadWorldStatusDialog -= On_Lang_GetDryadWorldStatusDialog;
		}

		#region DryadText
		private string On_Lang_GetDryadWorldStatusDialog(On_Lang.orig_GetDryadWorldStatusDialog orig, out bool worldIsEntirelyPure) {
			orig.Invoke(out worldIsEntirelyPure);
			string text = "";
			worldIsEntirelyPure = false;
			int tGood = WorldGen.tGood;
			int tEvil = WorldGen.tEvil;
			int tBlood = WorldGen.tBlood;
			int tCandy = ConfectionWorldGeneration.tCandy;
			if (tGood > 0 && tEvil > 0 && tBlood > 0 && tCandy > 0) {
				text = Language.GetTextValue("Mods.TheConfectionRebirth.DryadSpecialText.WorldStatusAll", Main.worldName, tGood, tEvil, tBlood, tCandy);
			}

			else if (tGood > 0 && tCandy > 0 && tEvil > 0) {
				text = Language.GetTextValue("Mods.TheConfectionRebirth.DryadSpecialText.WorldStatusHallowCandyCorrupt", Main.worldName, tGood, tCandy, tEvil);
			}
			else if (tGood > 0 && tCandy > 0 && tBlood > 0) {
				text = Language.GetTextValue("Mods.TheConfectionRebirth.DryadSpecialText.WorldStatusHallowCandyCrimson", Main.worldName, tGood, tCandy, tBlood);
			}
			else if (tCandy > 0 && tEvil > 0 && tBlood > 0) {
				text = Language.GetTextValue("Mods.TheConfectionRebirth.DryadSpecialText.WorldStatusCandyCorruptCrimson", Main.worldName, tCandy, tEvil, tBlood);
			}
			else if (tGood > 0 && tEvil > 0 && tBlood > 0) {
				text = Language.GetTextValue("DryadSpecialText.WorldStatusAll", Main.worldName, tGood, tEvil, tBlood);
			}

			else if (tGood > 0 && tEvil > 0) {
				text = Language.GetTextValue("DryadSpecialText.WorldStatusHallowCorrupt", Main.worldName, tGood, tEvil);
			}
			else if (tGood > 0 && tBlood > 0) {
				text = Language.GetTextValue("DryadSpecialText.WorldStatusHallowCrimson", Main.worldName, tGood, tBlood);
			}
			else if (tCandy > 0 && tEvil > 0) {
				text = Language.GetTextValue("Mods.TheConfectionRebirth.DryadSpecialText.WorldStatusCandyCorrupt", Main.worldName, tCandy, tEvil);
			}
			else if (tCandy > 0 && tBlood > 0) {
				text = Language.GetTextValue("Mods.TheConfectionRebirth.DryadSpecialText.WorldStatusCandyCrimson", Main.worldName, tCandy, tBlood);
			}
			else if (tEvil > 0 && tBlood > 0) {
				text = Language.GetTextValue("DryadSpecialText.WorldStatusCorruptCrimson", Main.worldName, tEvil, tBlood);
			}
			else if (tGood > 0 && tCandy > 0) {
				text = Language.GetTextValue("Mods.TheConfectionRebirth.DryadSpecialText.WorldStatusHallowCandy", Main.worldName, tGood, tCandy);
			}

			else if (tCandy > 0) {
				text = Language.GetTextValue("Mods.TheConfectionRebirth.DryadSpecialText.WorldStatusCandy", Main.worldName, tCandy);
			}
			else if (tEvil > 0) {
				text = Language.GetTextValue("DryadSpecialText.WorldStatusCorrupt", Main.worldName, tEvil);
			}
			else if (tBlood > 0) {
				text = Language.GetTextValue("DryadSpecialText.WorldStatusCrimson", Main.worldName, tBlood);
			}
			else {
				if (tGood <= 0) {
					text = Language.GetTextValue("DryadSpecialText.WorldStatusPure", Main.worldName);
					worldIsEntirelyPure = true;
					return text;
				}
				text = Language.GetTextValue("DryadSpecialText.WorldStatusHallow", Main.worldName, tGood);
			}
			string arg = (((double)(tGood + tCandy) * 1.2 >= (double)(tEvil + tBlood) && (double)(tGood + tCandy) * 0.8 <= (double)(tEvil + tBlood)) ? Language.GetTextValue("DryadSpecialText.WorldDescriptionBalanced") : ((tGood >= tEvil + tBlood + tCandy) ? Language.GetTextValue("DryadSpecialText.WorldDescriptionFairyTale") : ((tCandy >= tEvil + tBlood + tGood) ? Language.GetTextValue("Mods.TheConfectionRebirth.DryadSpecialText.WorldDescriptionSweeterAir") : ((tEvil + tBlood > (tGood + tCandy) + 20) ? Language.GetTextValue("DryadSpecialText.WorldDescriptionGrim") : ((tEvil + tBlood <= 5) ? Language.GetTextValue("DryadSpecialText.WorldDescriptionClose") : Language.GetTextValue("DryadSpecialText.WorldDescriptionWork"))))));
			return $"{text} {arg}";
		}
		#endregion

		#region TwinsDropDetour
		private void On_ItemDropDatabase_RegisterBoss_Twins(On_ItemDropDatabase.orig_RegisterBoss_Twins orig, ItemDropDatabase self)
		{
			orig.Invoke(self);
			LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new Conditions.MissingTwin());
			LeadingConditionRule leadingConditionRule2 = new LeadingConditionRule(new Conditions.NotExpert());
			LeadingConditionRule ConfectionCondition = new LeadingConditionRule(new ConfectionDropRule());
			LeadingConditionRule HallowCondition = new LeadingConditionRule(new HallowDropRule());
			leadingConditionRule.OnSuccess(leadingConditionRule2);
			leadingConditionRule2.OnSuccess(ConfectionCondition);
			leadingConditionRule2.OnSuccess(HallowCondition);
			ConfectionCondition.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.NeapoliniteOre>(), 1, 15 * 5, 30 * 5));
			HallowCondition.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.HallowedOre>(), 1, 15 * 5, 30 * 5));
			self.RegisterToMultipleNPCs(leadingConditionRule, 126, 125);
		}
		#endregion

		#region LAWNMOWAHHH
		private void On_Player_MowGrassTile(On_Player.orig_MowGrassTile orig, Player self, Vector2 thePos)
		{
			orig.Invoke(self, thePos);
			Point point = thePos.ToTileCoordinates();
			Tile tile = Main.tile[point.X, point.Y];
			if (tile == null || !WorldGen.CanKillTile(point.X, point.Y, WorldGen.SpecialKillTileContext.MowingTheGrass))
			{
				return;
			}
			ushort num = 0;
			if (tile.TileType == ModContent.TileType<Tiles.CreamGrass>())
			{
				num = (ushort)ModContent.TileType<Tiles.CreamGrassMowed>();
			}
			if (num != 0)
			{
				int num2 = WorldGen.KillTile_GetTileDustAmount(fail: true, tile, point.X, point.Y);
				for (int i = 0; i < num2; i++)
				{
					WorldGen.KillTile_MakeTileDust(point.X, point.Y, tile);
				}
				tile.TileType = num;
				if (Main.netMode == 1)
				{
					NetMessage.SendTileSquare(-1, point.X, point.Y);
				}
			}
		}
		#endregion

		#region WorldUiOverlay
		private void On_UIWorldSelect_UpdateWorldsList(Terraria.GameContent.UI.States.On_UIWorldSelect.orig_UpdateWorldsList orig, Terraria.GameContent.UI.States.UIWorldSelect self)
		{
			orig(self);

			UIList WorldList = (UIList)typeof(UIWorldSelect).GetField("_worldList", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self);
			foreach (var item in WorldList)
			{
				if (item is UIWorldListItem)
				{
					UIElement _WorldIcon = (UIElement)typeof(UIWorldListItem).GetField("_worldIcon", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(item);
					//_WorldIcon = GetIconElement();

					UIElement WorldIcon = (UIElement)typeof(UIWorldListItem).GetField("_worldIcon", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(item);
					WorldFileData Data = (WorldFileData)typeof(AWorldListItem).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(item);

					var path = Path.ChangeExtension(Data.Path, ".twld");

					ConfectionConfig config = ModContent.GetInstance<ConfectionConfig>();
					Dictionary<string, ConfectionConfig.WorldDataValues> tempDict = config.GetWorldData();

					if (!tempDict.ContainsKey(path))
					{
						byte[] buf = FileUtilities.ReadAllBytes(path, Data.IsCloudSave);
						var stream = new MemoryStream(buf);
						var tag = TagIO.FromStream(stream);
						bool containsMod = false;

						if (tag.ContainsKey("modData"))
						{
							foreach (TagCompound modDataTag in tag.GetList<TagCompound>("modData").Skip(2))
							{
								if (modDataTag.Get<string>("mod") == ModContent.GetInstance<ConfectionConfig>().Mod.Name)
								{
									TagCompound dataTag = modDataTag.Get<TagCompound>("data");
									ConfectionConfig.WorldDataValues worldData;

									worldData.confection = dataTag.Get<bool>("TheConfectionRebirth:confectionorHallow");
									tempDict[path] = worldData;

									containsMod = true;

									break;
								}
							}

							if (!containsMod)
							{
								ConfectionConfig.WorldDataValues worldData;

								worldData.confection = false;
								tempDict[path] = worldData;
							}

							config.SetWorldData(tempDict);
							ConfectionConfig.Save(config);
						}
					}

					#region RegularSeedIcon
					if (tempDict[path].confection && !Data.RemixWorld && !Data.DrunkWorld && !Data.Anniversary && !Data.DontStarve && !Data.ForTheWorthy && !Data.ZenithWorld && !Data.NotTheBees && !Data.NoTrapsWorld && Data.IsHardMode)
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
					if (tempDict[path].confection && !Data.RemixWorld && !Data.DrunkWorld && Data.Anniversary && !Data.DontStarve && !Data.ForTheWorthy && !Data.ZenithWorld && !Data.NotTheBees && !Data.NoTrapsWorld && Data.IsHardMode)
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
					if (tempDict[path].confection && !Data.RemixWorld && !Data.DrunkWorld && !Data.Anniversary && Data.DontStarve && !Data.ForTheWorthy && !Data.ZenithWorld && !Data.NotTheBees && !Data.NoTrapsWorld && Data.IsHardMode)
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
					if (/*tempDict[path].confection && */!Data.RemixWorld && Data.DrunkWorld && !Data.Anniversary && !Data.DontStarve && !Data.ForTheWorthy && !Data.ZenithWorld && !Data.NotTheBees && !Data.NoTrapsWorld && Data.IsHardMode)
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
					if (tempDict[path].confection && !Data.RemixWorld && !Data.DrunkWorld && !Data.Anniversary && !Data.DontStarve && Data.ForTheWorthy && !Data.ZenithWorld && !Data.NotTheBees && !Data.NoTrapsWorld && Data.IsHardMode)
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
					if (tempDict[path].confection && !Data.RemixWorld && !Data.DrunkWorld && !Data.Anniversary && !Data.DontStarve && !Data.ForTheWorthy && !Data.ZenithWorld && Data.NotTheBees && !Data.NoTrapsWorld && Data.IsHardMode)
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
					if (tempDict[path].confection && !Data.RemixWorld && !Data.DrunkWorld && !Data.Anniversary && !Data.DontStarve && !Data.ForTheWorthy && !Data.ZenithWorld && !Data.NotTheBees && Data.NoTrapsWorld && Data.IsHardMode)
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
					if (tempDict[path].confection && Data.RemixWorld && !Data.DrunkWorld && !Data.Anniversary && !Data.DontStarve && !Data.ForTheWorthy && !Data.ZenithWorld && !Data.NotTheBees && !Data.NoTrapsWorld && Data.IsHardMode)
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

					#region RemixSeedIcon
					if (tempDict[path].confection && Data.RemixWorld && Data.DrunkWorld)
					{
						UIElement worldIcon = WorldIcon;
						Asset<Texture2D> obj = ModContent.Request<Texture2D>("TheConfectionRebirth/Assets/WorldIcons/ConfectionEverything", (AssetRequestMode)1);
						UIImageFramed uIImageFramed = new UIImageFramed(obj, obj.Frame(7, 16));
						uIImageFramed.Left = new StyleDimension(0f, 0f);
						uIImageFramed.OnUpdate += UpdateGlitchAnimation;
						worldIcon.Append(uIImageFramed);
					}
					#endregion
				}
			}
		}

		protected UIElement GetIconElement()
		{
			WorldFileData Data = (WorldFileData)typeof(AWorldListItem).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(null);
			if (Data.DrunkWorld && Data.RemixWorld)
			{
				//Asset<Texture2D> obj = Main.Assets.Request<Texture2D>("Images/UI/IconEverythingAnimated");
				Asset<Texture2D> obj = ModContent.Request<Texture2D>("TheConfectionRebirth/Assets/WorldIcons/ConfectionEverything");
				UIImageFramed uIImageFramed = new UIImageFramed(obj, obj.Frame(7, 16));
				uIImageFramed.Left = new StyleDimension(4f, 0f);
				uIImageFramed.OnUpdate += UpdateGlitchAnimation;
				return uIImageFramed;
			}
			return null;
		}

		protected int _glitchFrameCounter;

		protected int _glitchFrame;

		protected int _glitchVariation;

		private void UpdateGlitchAnimation(UIElement affectedElement)
		{
			_ = _glitchFrame;
			int minValue = 3;
			int num = 3;
			if (_glitchFrame == 0)
			{
				minValue = 15;
				num = 120;
			}
			if (++_glitchFrameCounter >= Main.rand.Next(minValue, num + 1))
			{
				_glitchFrameCounter = 0;
				_glitchFrame = (_glitchFrame + 1) % 16;
				if ((_glitchFrame == 4 || _glitchFrame == 8 || _glitchFrame == 12) && Main.rand.Next(3) == 0)
				{
					_glitchVariation = Main.rand.Next(7);
				}
			}
			(affectedElement as UIImageFramed).SetFrame(7, 16, _glitchVariation, _glitchFrame, 0, 0);
		}
		#endregion

		#region OtherworldlyMusic
		private void Main_UpdateAudio_DecideOnTOWMusic(Terraria.On_Main.orig_UpdateAudio_DecideOnTOWMusic orig, Main self)
		{
			orig.Invoke(self);
			Player player = Main.CurrentPlayer;
			if (player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && !Main.LocalPlayer.ZoneRockLayerHeight)
			{
				Main.newMusic = 88; //Hallow Otherworldly
			}
			if (player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && Main.LocalPlayer.ZoneRockLayerHeight)
			{
				Main.newMusic = 78; //Underground Hallow Otherworldly
			}
		}
		#endregion

	}
}

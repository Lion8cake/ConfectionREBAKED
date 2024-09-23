using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.Utils;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.States;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Gamepad;
using TheConfectionRebirth.UI;

namespace TheConfectionRebirth.Hooks;

internal static class ConfectionSelectionMenu {
	private static readonly GroupOptionButton<HallowOptions>[] HallowedButtons =
			new GroupOptionButton<HallowOptions>[Enum.GetValues<HallowOptions>().Length];

	public static void ILBuildPage(ILContext il) {
		var c = new ILCursor(il);

		// Increase world gen container size
		c.GotoNext(i => i.MatchStloc(0));
		c.Emit(OpCodes.Ldc_I4, 48);
		c.Emit(OpCodes.Add);
	}

	public static void ILMakeInfoMenu(ILContext il) {
		var c = new ILCursor(il);

		// Getting spacing indexes for copying later
		c.GotoNext(i => i.MatchLdstr("evil"));
		c.GotoNext(i => i.MatchLdloc(1));
		int startOfSpacing = c.Index;
		c.GotoNext(i => i.MatchCall(out _));
		int endOfSpacing = c.Index + 1;

		// Navigate to position to add options 
		c.Index = c.Instrs.Count - 1;
		c.GotoPrev(i => i.MatchLdcR4(48));
		c.GotoNext(i => i.MatchCall(out _));
		c.Index++;

		// Adding Hallowed options
		c.Emit(OpCodes.Ldarg_0); // self
		c.Emit(OpCodes.Ldloc_0); // container
		c.Emit(OpCodes.Ldloc_1); // accumulatedHeight
		c.Emit(OpCodes.Ldloc, 10); // usableWidthPercent
		c.EmitDelegate((UIWorldCreation self, UIElement container, float accumulatedHeight, float usableWidthPercent) =>
			AddUnderworldOptions(self, container, accumulatedHeight, ClickHallowedOption, "hallow",
				usableWidthPercent));

		// Copying IL for spacing and horizontal bar
		c.Instrs.InsertRange(c.Index, c.Instrs.ToArray()[startOfSpacing..endOfSpacing]);
	}

	public static void OnSetDefaultOptions(On_UIWorldCreation.orig_SetDefaultOptions orig, UIWorldCreation self) {
		orig(self);

		ModContent.GetInstance<ConfectionWorldGeneration>().SelectedHallowOption = HallowOptions.Random;
		foreach (GroupOptionButton<HallowOptions> underworldButton in HallowedButtons) {
			underworldButton.SetCurrentOption(HallowOptions.Random);
		}
	}

	public static void ILShowOptionDescription(ILContext il) {
		var c = new ILCursor(il);

		// Navigate to before final break
		c.Index = c.Instrs.Count - 1;
		c.GotoPrev(i => i.MatchBrfalse(out _));

		// Add description handling logic
		c.Emit(OpCodes.Pop);
		c.Emit(OpCodes.Ldloc_0); // localizedText
		c.Emit(OpCodes.Ldarg_2); // listeningElement
		c.EmitDelegate((LocalizedText localizedText, UIElement listeningElement) =>
			listeningElement is not GroupOptionButton<HallowOptions> underworldButton ? localizedText : underworldButton.Description);
		c.Emit(OpCodes.Stloc_0);
		c.Emit(OpCodes.Ldloc_0);
	}

	private static void AddUnderworldOptions(UIWorldCreation self, UIElement container, float accumulatedHeight,
											 UIElement.MouseEvent clickEvent, string tagGroup,
											 float usableWidthPercent) {
		LocalizedText[] titles = {
			Language.GetText(Language.GetTextValue("Mods.TheConfectionRebirth.HallowSelection.Random.Title")),
			Language.GetText(Language.GetTextValue("Mods.TheConfectionRebirth.HallowSelection.Hallow.Title")),
			Language.GetText(Language.GetTextValue("Mods.TheConfectionRebirth.HallowSelection.Confection.Title")),
		};
		LocalizedText[] descriptions = {
			Language.GetText(Language.GetTextValue("Mods.TheConfectionRebirth.HallowSelection.Random.Description")),
			Language.GetText(Language.GetTextValue("Mods.TheConfectionRebirth.HallowSelection.Hallow.Description")),
			Language.GetText(Language.GetTextValue("Mods.TheConfectionRebirth.HallowSelection.Confection.Description")),
		};
		Color[] colors = {
			Color.White,
			Color.LightPink,
			Color.LightYellow,
		};
		Asset<Texture2D>[] icons = {
			Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/IconEvilRandom"),
			ModContent.Request<Texture2D>("TheConfectionRebirth/Assets/WorldCreation/IconGoodHallow"),
			ModContent.Request<Texture2D>("TheConfectionRebirth/Assets/WorldCreation/IconGoodConfection"),
		};
		for (int i = 0; i < HallowedButtons.Length; i++) {
			var groupOptionButton = new global::TheConfectionRebirth.UI.GroupOptionButton<HallowOptions>(
				Enum.GetValues<HallowOptions>()[i],
				titles[i],
				descriptions[i],
				colors[i],
				icons[i],
				1f,
				1f,
				16f) {
				Width = StyleDimension.FromPixelsAndPercent(
					-4 * (HallowedButtons.Length - 1),
					1f / HallowedButtons.Length * usableWidthPercent),
				Left = StyleDimension.FromPercent(1f - usableWidthPercent),
				HAlign = i / (float)(HallowedButtons.Length - 1),
			};
			groupOptionButton.Top.Set(accumulatedHeight, 0f);
			groupOptionButton.OnLeftMouseDown += clickEvent;
			groupOptionButton.OnMouseOver += self.ShowOptionDescription;
			groupOptionButton.OnMouseOut += self.ClearOptionDescription;
			groupOptionButton.SetSnapPoint(tagGroup, i);
			container.Append(groupOptionButton);
			HallowedButtons[i] = groupOptionButton;
		}
	}

	private static void ClickHallowedOption(UIMouseEvent evt, UIElement listeningElement) {
		var groupOptionButton = (GroupOptionButton<HallowOptions>)listeningElement;
		ModContent.GetInstance<ConfectionWorldGeneration>().SelectedHallowOption = groupOptionButton.OptionValue;

		foreach (GroupOptionButton<HallowOptions> underworldButton in HallowedButtons) {
			underworldButton.SetCurrentOption(groupOptionButton.OptionValue);
		}
	}

	public static void OnSetupGamepadPoints(On_UIWorldCreation.orig_SetupGamepadPoints orig, UIWorldCreation self, SpriteBatch spriteBatch) {
		orig(self, spriteBatch);
		int num = 3006;
		List<SnapPoint> snapPoints = self.GetSnapPoints();
		List<SnapPoint> snapGroup = GetSnapGroup(self, snapPoints, "size");
		List<SnapPoint> snapGroup2 = GetSnapGroup(self, snapPoints, "difficulty");
		List<SnapPoint> snapGroup3 = GetSnapGroup(self, snapPoints, "evil");
		num += snapGroup.Count + snapGroup2.Count;
		List<SnapPoint> snapGroup4 = GetSnapGroup(self, snapPoints, "hallow");

		UILinkPoint uILinkPoint;
		UILinkPoint uILinkPoint2 = UILinkPointNavigator.Points[3000];
		UILinkPoint uILinkPoint3 = UILinkPointNavigator.Points[3001];

		UILinkPoint[] array = new UILinkPoint[snapGroup3.Count];
		for (int l = 0; l < snapGroup4.Count; l++) {
			UILinkPointNavigator.SetPosition(num, snapGroup3[l].Position);
			uILinkPoint = UILinkPointNavigator.Points[num];
			array[l] = uILinkPoint;
			num++;
		}
		UILinkPoint[] array2 = new UILinkPoint[snapGroup4.Count];
		for (int l = 0; l < snapGroup4.Count; l++) {
			UILinkPointNavigator.SetPosition(num, snapGroup4[l].Position);
			uILinkPoint = UILinkPointNavigator.Points[num];
			uILinkPoint.Unlink();
			array2[l] = uILinkPoint;
			num++;
		}

		TheConfectionRebirth.Instance.Logger.Info(array);
		TheConfectionRebirth.Instance.Logger.Info(array2);
		LoopHorizontalLineLinks(self, array2);
		EstablishUpDownRelationship(self, array, array2);
		for (int n = 0; n < array2.Length; n++) {
			array2[n].Down = uILinkPoint2.ID;
		}

		array2[^1].Down = uILinkPoint3.ID;
		uILinkPoint3.Up = array2[^1].ID;
		uILinkPoint2.Up = array2[0].ID;
	}

	private static List<SnapPoint> GetSnapGroup(UIWorldCreation self, List<SnapPoint> snapPoints, string group) {
		return (List<SnapPoint>)typeof(UIWorldCreation).GetMethod("GetSnapGroup", BindingFlags.NonPublic | BindingFlags.Instance)?
			.Invoke(self, [snapPoints, group]);
	}

	private static void LoopHorizontalLineLinks(UIWorldCreation self, UILinkPoint[] pointsLine) {
		typeof(UIWorldCreation).GetMethod("LoopHorizontalLineLinks", BindingFlags.NonPublic | BindingFlags.Instance)?
			.Invoke(self, [pointsLine]);
	}

	private static void EstablishUpDownRelationship(UIWorldCreation self, UILinkPoint[] topSide, UILinkPoint[] bottomSide) {
		typeof(UIWorldCreation).GetMethod("EstablishUpDownRelationship", BindingFlags.NonPublic | BindingFlags.Instance)?
			.Invoke(self, [topSide, bottomSide]);
	}
}
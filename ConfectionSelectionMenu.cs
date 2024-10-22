using System;
using System.Collections.Generic;
using System.Linq;
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

namespace TheConfectionRebirth.Hooks 
{
	internal static class ConfectionSelectionMenu
	{
		private static readonly GroupOptionButton<HallowOptions>[] HallowedButtons =
				new GroupOptionButton<HallowOptions>[Enum.GetValues<HallowOptions>().Length];

		public static void ILBuildPage(ILContext il)
		{
			var c = new ILCursor(il);

			// Increase world gen container size
			c.GotoNext(i => i.MatchStloc(0));
			c.Emit(OpCodes.Ldc_I4, 48);
			c.Emit(OpCodes.Add);
			c.GotoNext(i => i.MatchLdcR4(170f)) // Fix page position
				.GotoNext(i => i.MatchLdloc0());
			c.Emit(OpCodes.Ldc_R4, 38f);
			c.Emit(OpCodes.Add);
		}

		public static void ILMakeInfoMenu(ILContext il)
		{
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
				AddHallowOptions(self, container, accumulatedHeight, ClickHallowedOption, "hallow",
					usableWidthPercent));

			// Copying IL for spacing and horizontal bar
			c.Instrs.InsertRange(c.Index, c.Instrs.ToArray()[startOfSpacing..endOfSpacing]);
		}

		internal static void ILSetUpGamepadPoints(ILContext il)
		{
			var c = new ILCursor(il);
			List<SnapPoint> snapGroupHallow = null;
			UILinkPoint[] arrayUW = null;
			c.GotoNext(MoveType.After, i => i.MatchLdarg0(), i => i.MatchLdloc1(), i => i.MatchLdstr("evil"), i => i.MatchCall<UIWorldCreation>("GetSnapGroup"), i => i.MatchStloc(10));
			c.EmitLdloc1(); //snapPoints
			c.EmitDelegate((List<SnapPoint> snapPoints) => {
				snapGroupHallow = GetSnapGroup(snapPoints, "hallow");
			});
			c.GotoNext(MoveType.After, i => i.MatchLdloc(26), i => i.MatchLdloc(10), i => i.MatchCallvirt<List<SnapPoint>>("get_Count"), i => i.MatchBlt(out _));
			c.EmitLdloc0(); //num
			c.EmitLdloc(12); //uILinkPoint
			c.EmitDelegate((int num, UILinkPoint uILinkPoint) =>
			{
				arrayUW = new UILinkPoint[snapGroupHallow.Count];
				for (int l = 0; l < snapGroupHallow.Count; l++)
				{
					UILinkPointNavigator.SetPosition(num, snapGroupHallow[l].Position);
					uILinkPoint = UILinkPointNavigator.Points[num];
					uILinkPoint.Unlink();
					arrayUW[l] = uILinkPoint;
					num++;
				}
			});
			c.GotoNext(MoveType.After, i => i.MatchLdloc(28), i => i.MatchLdloc(20), i => i.MatchLdlen(), i => i.MatchConvI4(), i => i.MatchBlt(out _));
			c.EmitLdloc(20); //array3 //Evils button
			c.EmitLdloc(12); //uILinkPoint2 //Create button
			c.EmitDelegate((UILinkPoint[] array3, UILinkPoint uILinkPoint2) =>
			{
				LoopHorizontalLineLinks(arrayUW);
				EstablishUpDownRelationship(array3, arrayUW);
				for (int n = 0; n < arrayUW.Length; n++)
				{
					arrayUW[n].Down = uILinkPoint2.ID;
				}
			});
			c.GotoNext(MoveType.After, i => i.MatchLdloc(12), i => i.MatchLdloc(20), i => i.MatchLdcI4(0), i => i.MatchLdelemRef(), i => i.MatchLdfld<UILinkPoint>("ID"), i => i.MatchStfld<UILinkPoint>("Up"));
			c.EmitLdloc(20); //array3 //Evils button
			c.EmitLdloc(13); //uILinkPoint3 //Back button
			c.EmitLdloc(12); //uILinkPoint2 //Create button
			c.EmitDelegate((UILinkPoint[] array3, UILinkPoint uILinkPoint3, UILinkPoint uILinkPoint2) =>
			{
				array3[^1].Down = arrayUW[^1].ID;
				arrayUW[^1].Down = uILinkPoint3.ID;
				uILinkPoint3.Up = arrayUW[^1].ID;
				uILinkPoint2.Up = arrayUW[0].ID;
			});
		}

		#region voidsFromUIWorldCreation
		private static List<SnapPoint> GetSnapGroup(List<SnapPoint> ptsOnPage, string groupName) //Should just reflect the UIWorldCreation.GetSnapGroup method tbh
		{
			List<SnapPoint> list = ptsOnPage.Where((SnapPoint a) => a.Name == groupName).ToList();
			list.Sort(SortPoints);
			return list;
		}

		private static int SortPoints(SnapPoint a, SnapPoint b)
		{
			return a.Id.CompareTo(b.Id);
		}

		private static void LoopHorizontalLineLinks(UILinkPoint[] pointsLine)
		{
			for (int i = 1; i < pointsLine.Length - 1; i++)
			{
				pointsLine[i - 1].Right = pointsLine[i].ID;
				pointsLine[i].Left = pointsLine[i - 1].ID;
				pointsLine[i].Right = pointsLine[i + 1].ID;
				pointsLine[i + 1].Left = pointsLine[i].ID;
			}
		}

		private static void EstablishUpDownRelationship(UILinkPoint[] topSide, UILinkPoint[] bottomSide)
		{
			int num = Math.Max(topSide.Length, bottomSide.Length);
			for (int i = 0; i < num; i++)
			{
				int num2 = Math.Min(i, topSide.Length - 1);
				int num3 = Math.Min(i, bottomSide.Length - 1);
				topSide[num2].Down = bottomSide[num3].ID;
				bottomSide[num3].Up = topSide[num2].ID;
			}
		}
		#endregion

		public static void OnSetDefaultOptions(On_UIWorldCreation.orig_SetDefaultOptions orig, UIWorldCreation self)
		{
			orig(self);

			ModContent.GetInstance<ConfectionWorldGeneration>().SelectedHallowOption = HallowOptions.Random;
			foreach (GroupOptionButton<HallowOptions> hallowButton in HallowedButtons)
			{
				hallowButton.SetCurrentOption(HallowOptions.Random);
			}
		}

		public static void ILShowOptionDescription(ILContext il)
		{
			var c = new ILCursor(il);

			// Navigate to before final break
			c.Index = c.Instrs.Count - 1;
			c.GotoPrev(i => i.MatchBrfalse(out _));

			// Add description handling logic
			c.Emit(OpCodes.Pop);
			c.Emit(OpCodes.Ldloc_0); // localizedText
			c.Emit(OpCodes.Ldarg_2); // listeningElement
			c.EmitDelegate((LocalizedText localizedText, UIElement listeningElement) =>
				listeningElement is not GroupOptionButton<HallowOptions> hallowButton ? localizedText : hallowButton.Description);
			c.Emit(OpCodes.Stloc_0);
			c.Emit(OpCodes.Ldloc_0);
		}

		private static void AddHallowOptions(UIWorldCreation self, UIElement container, float accumulatedHeight,
												 UIElement.MouseEvent clickEvent, string tagGroup,
												 float usableWidthPercent)
		{
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
			for (int i = 0; i < HallowedButtons.Length; i++)
			{
				var groupOptionButton = new global::TheConfectionRebirth.GroupOptionButton<HallowOptions>(
					Enum.GetValues<HallowOptions>()[i],
					titles[i],
					descriptions[i],
					colors[i],
					icons[i],
					1f,
					1f,
					16f)
				{
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

		private static void ClickHallowedOption(UIMouseEvent evt, UIElement listeningElement)
		{
			var groupOptionButton = (GroupOptionButton<HallowOptions>)listeningElement;
			ModContent.GetInstance<ConfectionWorldGeneration>().SelectedHallowOption = groupOptionButton.OptionValue;

			foreach (GroupOptionButton<HallowOptions> hallowButton in HallowedButtons)
			{
				hallowButton.SetCurrentOption(groupOptionButton.OptionValue);
			}
		}
	}
}
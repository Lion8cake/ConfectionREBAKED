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
<<<<<<< HEAD
<<<<<<< Updated upstream
using TheConfectionRebirth.Items;
=======
using TheConfectionRebirth.Hooks;
>>>>>>> Stashed changes
=======
>>>>>>> 21d961fc5a6b7f1a1395f5a436ef383fb42b52eb
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

<<<<<<< HEAD
<<<<<<< Updated upstream
namespace TheConfectionRebirth
{
	public class TheConfectionRebirth : Mod
    {
        private struct DateTimeMatch
		{
            private readonly bool value;
=======
namespace TheConfectionRebirth {
	public class TheConfectionRebirth : Mod {

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


		private struct DateTimeMatch {
			private readonly bool value;
>>>>>>> Stashed changes
=======
namespace TheConfectionRebirth {
	public class TheConfectionRebirth : Mod {
		private struct DateTimeMatch {
			private readonly bool value;
>>>>>>> 21d961fc5a6b7f1a1395f5a436ef383fb42b52eb

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
		public class TileTest {
			public bool this[int tile1, int tile2] {
				get => Main.tileMerge[tile1][tile2] || Main.tileMerge[tile2][tile1];
				set => ConfectionUtils.Merge(tile1, tile2);
			}
		}

<<<<<<< HEAD
<<<<<<< Updated upstream
        private delegate void BackgroundChangeFlashInfo_UpdateVariation(BackgroundChangeFlashInfo self, int areaId, int newVariationValue);
        private static TileTest v = new();
        private static BackgroundChangeFlashInfo_UpdateVariation backgroundChangeFlashInfo_UpdateVariation;
        private static bool SecretChance => Main.rand.Next(100000000 + Main.rand.Next(5002254)) < 7752 * Main.rand.NextFloat(3f);
        public static bool OurFavoriteDay => new DateTimeMatch(DateTime.Now, new DateTime(2022, 12, 11), new DateTime(2022, 10, 2), new DateTime(2022, 5, 16)).ToBoolean();
        public static TileTest tileMerge => v;
=======
		private delegate void BackgroundChangeFlashInfo_UpdateVariation(BackgroundChangeFlashInfo self, int areaId, int newVariationValue);
		private static TileTest v = new();
		private static BackgroundChangeFlashInfo_UpdateVariation backgroundChangeFlashInfo_UpdateVariation;
		private static bool SecretChance => Main.rand.Next(100000000 + Main.rand.Next(5002254)) < 7752 * Main.rand.NextFloat(3f);
		public static bool OurFavoriteDay => new DateTimeMatch(DateTime.Now, new DateTime(2022, 12, 11), new DateTime(2022, 10, 2), new DateTime(2022, 5, 16)).ToBoolean();
		public static TileTest tileMerge => v;
>>>>>>> 21d961fc5a6b7f1a1395f5a436ef383fb42b52eb

		internal const int bgVarAmount = 4;

		private static MethodInfo Limits = null;

		private static event ILContext.Manipulator ModifyLimits {
			add => HookEndpointManager.Modify(Limits, value);
			remove => HookEndpointManager.Unmodify(Limits, value);
		}

		private static MethodInfo Limits2 = null;

		private static event ILContext.Manipulator ModifyLimits2 {
			add => HookEndpointManager.Modify(Limits2, value);
			remove => HookEndpointManager.Unmodify(Limits2, value);
		}

		private static MethodInfo Limits3 = null;

		private static event ILContext.Manipulator ModifyLimits3 {
			add => HookEndpointManager.Modify(Limits3, value);
			remove => HookEndpointManager.Unmodify(Limits3, value);
		}

		public override void Load() {
			backgroundChangeFlashInfo_UpdateVariation = typeof(BackgroundChangeFlashInfo).GetMethod("UpdateVariation", BindingFlags.NonPublic | BindingFlags.Instance).CreateDelegate<BackgroundChangeFlashInfo_UpdateVariation>();

			Fields = new dynamic[]
			{
				typeof(Main).GetField("bgLoops", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic),
				typeof(Main).GetField("ColorOfSurfaceBackgroundsModified", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static),
				typeof(Main).GetField("bgWidthScaled", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static),
				typeof(Main).GetField("scAdj", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance),
				typeof(Main).GetField("bgScale", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static),
				typeof(Main).Assembly.GetType("Terraria.ModLoader." + nameof(SurfaceBackgroundStylesLoader)),
				typeof(BackgroundChangeFlashInfo).GetField("_flashPower", BindingFlags.Instance | BindingFlags.NonPublic),
				typeof(BackgroundChangeFlashInfo).GetField("_variations", BindingFlags.Instance | BindingFlags.NonPublic),
			};

			if (ModLoader.TryGetMod("Wikithis", out Mod wikithis))
				wikithis.Call("AddModURL", this, "terrariamods.wiki.gg$Confection_Rebaked");

			Type UIMods = Fields[5];
			Limits = UIMods.GetMethod(nameof(SurfaceBackgroundStylesLoader.DrawMiddleTexture));
			ModifyLimits += TheConfectionRebirth_ModifyLimits;
			Limits2 = UIMods.GetMethod(nameof(SurfaceBackgroundStylesLoader.DrawFarTexture));
			ModifyLimits2 += TheConfectionRebirth_ModifyLimits2;
			Limits3 = UIMods.GetMethod(nameof(SurfaceBackgroundStylesLoader.DrawCloseBackground));
			ModifyLimits3 += TheConfectionRebirth_ModifyLimits3;

			float[] _flashPower = (float[])Fields[6].GetValue(WorldGen.BackgroundsCache);
			int[] _variations = (int[])Fields[7].GetValue(WorldGen.BackgroundsCache);
			float[] newFlashPower = new float[_flashPower.Length + bgVarAmount];
			int[] newVariations = new int[_variations.Length + bgVarAmount];
			_cacheIndexes = new int[bgVarAmount];
			for (int i = 0; i < newFlashPower.Length; i++) {
				if (i >= _flashPower.Length) {
					newFlashPower[i] = 0f;
					newVariations[i] = 0;
					_cacheIndexes[i - _flashPower.Length] = i;
					continue;
				}

				newFlashPower[i] = _flashPower[i];
				newVariations[i] = _variations[i];
			}
			Fields[6].SetValue(WorldGen.BackgroundsCache, newFlashPower);
			Fields[7].SetValue(WorldGen.BackgroundsCache, newVariations);

			On.Terraria.GameContent.BackgroundChangeFlashInfo.UpdateCache += BackgroundChangeFlashInfo_UpdateCache;
			On.Terraria.WorldGen.RandomizeBackgroundBasedOnPlayer += WorldGen_RandomizeBackgroundBasedOnPlayer;

			On.Terraria.Main.DrawSurfaceBG_BackMountainsStep1 += Main_DrawSurfaceBG_BackMountainsStep1;
		}

		private static double _backgroundTopMagicNumberCache;
		private static int _pushBGTopHackCache;
		private static void Main_DrawSurfaceBG_BackMountainsStep1(On.Terraria.Main.orig_DrawSurfaceBG_BackMountainsStep1 orig, Main self, double backgroundTopMagicNumber, float bgGlobalScaleMultiplier, int pushBGTopHack) {
			_backgroundTopMagicNumberCache = backgroundTopMagicNumber;
			_pushBGTopHackCache = pushBGTopHack;
			orig(self, backgroundTopMagicNumber, bgGlobalScaleMultiplier, pushBGTopHack);
		}

		private static int[] _cacheIndexes;
		private static void BackgroundChangeFlashInfo_UpdateCache(On.Terraria.GameContent.BackgroundChangeFlashInfo.orig_UpdateCache orig, BackgroundChangeFlashInfo self) {
			orig(self);

			for (int i = 0; i < bgVarAmount; i++)
				backgroundChangeFlashInfo_UpdateVariation(self, _cacheIndexes[i], ConfectionWorld.ConfectionSurfaceBG[i]);
		}

		private void WorldGen_RandomizeBackgroundBasedOnPlayer(On.Terraria.WorldGen.orig_RandomizeBackgroundBasedOnPlayer orig, UnifiedRandom random, Player player) {
			if (ModContent.GetInstance<ConfectionBiomeTileCount>().confectionBlockCount >= 120) {
				int[] rand = new int[4] { Main.rand.Next(bgVarAmount), Main.rand.Next(bgVarAmount), Main.rand.Next(bgVarAmount), Main.rand.Next(bgVarAmount) };
				for (int i = 0; i < bgVarAmount; i++) {
					while (ConfectionWorld.ConfectionSurfaceBG[i] == rand[i]) {
						rand[i] = Main.rand.Next(bgVarAmount);
						if (SecretChance)
							ConfectionWorld.Secret = true;
					}

					ConfectionWorld.ConfectionSurfaceBG[i] = rand[i];
					backgroundChangeFlashInfo_UpdateVariation(WorldGen.BackgroundsCache, _cacheIndexes[i], rand[i]);
				}

				if (Main.netMode == NetmodeID.MultiplayerClient)
					NetMessage.SendData(MessageID.WorldData);
			}

			orig(random, player);
		}

		public override void Unload() {
			if (Limits != null)
				ModifyLimits -= TheConfectionRebirth_ModifyLimits;
			Limits = null;
			if (Limits2 != null)
				ModifyLimits2 -= TheConfectionRebirth_ModifyLimits2;
			Limits2 = null;
			if (Limits3 != null)
				ModifyLimits3 -= TheConfectionRebirth_ModifyLimits3;
			Limits3 = null;

			_backgroundTopMagicNumberCache = 0;
			_pushBGTopHackCache = 0;
			_cacheIndexes = null;
			backgroundChangeFlashInfo_UpdateVariation = null;
			Fields = null;
			v = null;
		}

		public override void PostSetupContent() {
			SummonersShineThoughtBubble.PostSetupContent();
			StackableBuffData.PostSetupContent();
			ModSupport.ModSupportBaseClass.HookAll();
		}

		// middle
		private void TheConfectionRebirth_ModifyLimits(ILContext il) {
			ILCursor c = new(il);
			if (!c.TryGotoNext(i => i.MatchCallvirt(typeof(Asset<Texture2D>).GetMethod("get_Value"))))
				return;

			c.Index++;
			c.Emit(OpCodes.Ldloc, 1);
			c.EmitDelegate<Func<Texture2D, ModSurfaceBackgroundStyle, Texture2D>>((value, style) => {
				if (style is IBackground) {
					if (ConfectionWorld.ConfectionSurfaceBG[1] == -1) {
						ConfectionWorld.ConfectionSurfaceBG[1] = Main.rand.Next(bgVarAmount);
						if (SecretChance)
							ConfectionWorld.Secret = true;
						if (Main.netMode == NetmodeID.MultiplayerClient)
							NetMessage.SendData(MessageID.WorldData);
					}

					return (style as IBackground).GetMidTexture(ConfectionWorld.ConfectionSurfaceBG[1]).Value;
				}
				return value;
			});

			if (!c.TryGotoNext(MoveType.After,
				i => i.MatchLdsfld<Main>(nameof(Main.backgroundWidth)),
				i => i.MatchLdloc(4),
				i => i.MatchLdelemI4()))
				return;

			c.Emit(OpCodes.Ldloc, 1);
			c.EmitDelegate<Func<int, ModSurfaceBackgroundStyle, int>>((v, style) => {
				if (style is IBackground) {
					if (ConfectionWorld.ConfectionSurfaceBG[1] == -1) {
						ConfectionWorld.ConfectionSurfaceBG[1] = Main.rand.Next(bgVarAmount);
						if (SecretChance)
							ConfectionWorld.Secret = true;
						if (Main.netMode == NetmodeID.MultiplayerClient)
							NetMessage.SendData(MessageID.WorldData);
					}

					return (style as IBackground).GetFarTexture(ConfectionWorld.ConfectionSurfaceBG[1]).Value.Width;
				}
				return v;
			});

			if (!c.TryGotoNext(MoveType.After,
				i => i.MatchLdsfld<Main>(nameof(Main.backgroundHeight)),
				i => i.MatchLdloc(4),
				i => i.MatchLdelemI4()))
				return;

			c.Emit(OpCodes.Ldloc, 1);
			c.EmitDelegate<Func<int, ModSurfaceBackgroundStyle, int>>((v, style) => {
				if (style is IBackground) {
					if (ConfectionWorld.ConfectionSurfaceBG[1] == -1) {
						ConfectionWorld.ConfectionSurfaceBG[1] = Main.rand.Next(bgVarAmount);
						if (SecretChance)
							ConfectionWorld.Secret = true;
						if (Main.netMode == NetmodeID.MultiplayerClient)
							NetMessage.SendData(MessageID.WorldData);
					}

					return (style as IBackground).GetFarTexture(ConfectionWorld.ConfectionSurfaceBG[1]).Value.Height;
				}
				return v;
			});
		}

		private static dynamic[] Fields;
		// far and super far
		private void TheConfectionRebirth_ModifyLimits2(ILContext il) {
			ILCursor c = new(il);

			if (!c.TryGotoNext(i => i.MatchLdloc(3),
				i => i.MatchLdcR4(0)))
				return;

			c.Emit(OpCodes.Ldloc, 1);
			c.EmitDelegate<Action<ModSurfaceBackgroundStyle>>((style) => {
				if (style == null || style is not IBackground)
					return;

				int slot = style.Slot;
				float alpha = Main.bgAlphaFarBackLayer[slot];
				if (alpha > 0f) {
					for (int i = 0; i < (int)Fields[0].GetValue(Main.instance); i++) {
						if (ConfectionWorld.ConfectionSurfaceBG[3] == -1) {
							ConfectionWorld.ConfectionSurfaceBG[3] = Main.rand.Next(bgVarAmount);
							if (SecretChance)
								ConfectionWorld.Secret = true;
							if (Main.netMode == NetmodeID.MultiplayerClient)
								NetMessage.SendData(MessageID.WorldData);
						}

						float bgParallax = 0.1f;
						Texture2D texture = (style as IBackground).GetUltraFarTexture(ConfectionWorld.ConfectionSurfaceBG[3]).Value;
						if (texture is null)
							return;

						Color ColorOfSurfaceBackgroundsModified = (Color)Fields[1].GetValue(null);
						float scAdj = (float)Fields[3].GetValue(Main.instance);
						int bgWidthScaled = (int)Fields[2].GetValue(null);
						int bgTopY = !Main.gameMenu ? (int)(_backgroundTopMagicNumberCache * 1300.0 + 1005.0 + (int)scAdj + _pushBGTopHackCache + 40) : 75 + _pushBGTopHackCache;

						float bgScale = (float)Fields[4].GetValue(null);
						int bgStartX = (int)(0.0 - Math.IEEERemainder((double)Main.screenPosition.X * bgParallax, bgWidthScaled) - (bgWidthScaled / 2));

						Main.spriteBatch.Draw(texture,
							new Vector2(bgStartX + bgWidthScaled * i, bgTopY - 20),
							new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)),
							ColorOfSurfaceBackgroundsModified, 0f, default,
							bgScale, 0, 0f);
					}
				}
			});

			if (!c.TryGotoNext(i => i.MatchCallvirt(typeof(Asset<Texture2D>).GetMethod("get_Value"))))
				return;

			c.Index++;
			c.Emit(OpCodes.Ldloc, 1);
			c.EmitDelegate<Func<Texture2D, ModSurfaceBackgroundStyle, Texture2D>>((value, style) => {
				if (style is IBackground) {
					if (ConfectionWorld.ConfectionSurfaceBG[2] == -1) {
						ConfectionWorld.ConfectionSurfaceBG[2] = Main.rand.Next(bgVarAmount);
						if (SecretChance)
							ConfectionWorld.Secret = true;
						if (Main.netMode == NetmodeID.MultiplayerClient)
							NetMessage.SendData(MessageID.WorldData);
					}

					return (style as IBackground).GetFarTexture(ConfectionWorld.ConfectionSurfaceBG[2]).Value;
				}
				return value;
			});

			if (!c.TryGotoNext(MoveType.After,
				i => i.MatchLdsfld<Main>(nameof(Main.backgroundWidth)),
				i => i.MatchLdloc(4),
				i => i.MatchLdelemI4()))
				return;

			c.Emit(OpCodes.Ldloc, 1);
			c.EmitDelegate<Func<int, ModSurfaceBackgroundStyle, int>>((v, style) => {
				if (style is IBackground) {
					if (ConfectionWorld.ConfectionSurfaceBG[2] == -1) {
						ConfectionWorld.ConfectionSurfaceBG[2] = Main.rand.Next(bgVarAmount);
						if (SecretChance)
							ConfectionWorld.Secret = true;
						if (Main.netMode == NetmodeID.MultiplayerClient)
							NetMessage.SendData(MessageID.WorldData);
					}

					return (style as IBackground).GetFarTexture(ConfectionWorld.ConfectionSurfaceBG[2]).Value.Width;
				}
				return v;
			});

			if (!c.TryGotoNext(MoveType.After,
				i => i.MatchLdsfld<Main>(nameof(Main.backgroundHeight)),
				i => i.MatchLdloc(4),
				i => i.MatchLdelemI4()))
				return;

			c.Emit(OpCodes.Ldloc, 1);
			c.EmitDelegate<Func<int, ModSurfaceBackgroundStyle, int>>((v, style) => {
				if (style is IBackground) {
					if (ConfectionWorld.ConfectionSurfaceBG[2] == -1) {
						ConfectionWorld.ConfectionSurfaceBG[2] = Main.rand.Next(bgVarAmount);
						if (SecretChance)
							ConfectionWorld.Secret = true;
						if (Main.netMode == NetmodeID.MultiplayerClient)
							NetMessage.SendData(MessageID.WorldData);
					}

					return (style as IBackground).GetFarTexture(ConfectionWorld.ConfectionSurfaceBG[2]).Value.Height;
				}
				return v;
			});
		}

		// close
		private void TheConfectionRebirth_ModifyLimits3(ILContext il) {
			ILCursor c = new(il);
			if (!c.TryGotoNext(i => i.MatchCallvirt(typeof(Asset<Texture2D>).GetMethod("get_Value"))))
				return;

			c.Index++;
			c.Emit(OpCodes.Ldloc, 0);
			c.EmitDelegate<Func<Texture2D, ModSurfaceBackgroundStyle, Texture2D>>((value, style) => {
				if (style is IBackground) {
					if (ConfectionWorld.ConfectionSurfaceBG[0] == -1) {
						ConfectionWorld.ConfectionSurfaceBG[0] = Main.rand.Next(bgVarAmount);
						if (SecretChance)
							ConfectionWorld.Secret = true;
						if (Main.netMode == NetmodeID.MultiplayerClient)
							NetMessage.SendData(MessageID.WorldData);
					}
					return (style as IBackground).GetCloseTexture(ConfectionWorld.ConfectionSurfaceBG[0]).Value;
				}
				return value;
			});

			if (!c.TryGotoNext(MoveType.After,
				i => i.MatchLdsfld<Main>(nameof(Main.backgroundWidth)),
				i => i.MatchLdloc(3),
				i => i.MatchLdelemI4()))
				return;

			c.Emit(OpCodes.Ldloc, 0);
			c.EmitDelegate<Func<int, ModSurfaceBackgroundStyle, int>>((v, style) => {
				if (style is IBackground) {
					if (ConfectionWorld.ConfectionSurfaceBG[0] == -1) {
						ConfectionWorld.ConfectionSurfaceBG[0] = Main.rand.Next(bgVarAmount);
						if (SecretChance)
							ConfectionWorld.Secret = true;
						if (Main.netMode == NetmodeID.MultiplayerClient)
							NetMessage.SendData(MessageID.WorldData);
					}
					return (style as IBackground).GetFarTexture(ConfectionWorld.ConfectionSurfaceBG[0]).Value.Width;
				}
				return v;
			});

			if (!c.TryGotoNext(MoveType.After,
				i => i.MatchLdsfld<Main>(nameof(Main.backgroundHeight)),
				i => i.MatchLdloc(3),
				i => i.MatchLdelemI4()))
				return;

			c.Emit(OpCodes.Ldloc, 0);
			c.EmitDelegate<Func<int, ModSurfaceBackgroundStyle, int>>((v, style) => {
				if (style is IBackground) {
					if (ConfectionWorld.ConfectionSurfaceBG[0] == -1) {
						ConfectionWorld.ConfectionSurfaceBG[0] = Main.rand.Next(bgVarAmount);
						if (SecretChance)
							ConfectionWorld.Secret = true;
						if (Main.netMode == NetmodeID.MultiplayerClient)
							NetMessage.SendData(MessageID.WorldData);
					}
					Texture2D texture2 = TextureAssets.MagicPixel.Value;
					Color color = Color.Black * WorldGen.BackgroundsCache.GetFlashPower(_cacheIndexes[0]);
					Main.spriteBatch.Draw(texture2, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), color);

<<<<<<< HEAD
                    return (style as IBackground).GetFarTexture(ConfectionWorld.ConfectionSurfaceBG[0]).Value.Height;
                }
                return v;
            });
        }
    }
=======
		private delegate void BackgroundChangeFlashInfo_UpdateVariation(BackgroundChangeFlashInfo self, int areaId, int newVariationValue);
		private static TileTest v = new();
		public static bool OurFavoriteDay => new DateTimeMatch(DateTime.Now, new DateTime(2022, 12, 11), new DateTime(2022, 10, 2), new DateTime(2022, 5, 16)).ToBoolean();
		public static TileTest tileMerge => v;

		public override void PostSetupContent() {
			SummonersShineThoughtBubble.PostSetupContent();
			StackableBuffData.PostSetupContent();
			ModSupport.ModSupportBaseClass.HookAll();
		}

		public override void Load() {
			Terraria.On_Main.UpdateAudio_DecideOnTOWMusic += Main_UpdateAudio_DecideOnTOWMusic;

			Terraria.GameContent.UI.States.IL_UIWorldCreation.BuildPage += ConfectionSelectionMenu.ILBuildPage;
			Terraria.GameContent.UI.States.IL_UIWorldCreation.MakeInfoMenu += ConfectionSelectionMenu.ILMakeInfoMenu;
			Terraria.GameContent.UI.States.IL_UIWorldCreation.ShowOptionDescription +=
				ConfectionSelectionMenu.ILShowOptionDescription;

			Terraria.GameContent.UI.States.On_UIWorldSelect.UpdateWorldsList += On_UIWorldSelect_UpdateWorldsList;
			Terraria.On_Player.MowGrassTile += On_Player_MowGrassTile;
			Terraria.GameContent.ItemDropRules.On_ItemDropDatabase.RegisterBoss_Twins += On_ItemDropDatabase_RegisterBoss_Twins;
		}

		public override void Unload() {
			On_Main.UpdateAudio_DecideOnTOWMusic -= Main_UpdateAudio_DecideOnTOWMusic;
			Terraria.GameContent.UI.States.On_UIWorldSelect.UpdateWorldsList -= On_UIWorldSelect_UpdateWorldsList;
			Terraria.On_Player.MowGrassTile -= On_Player_MowGrassTile;
			Terraria.GameContent.ItemDropRules.On_ItemDropDatabase.RegisterBoss_Twins -= On_ItemDropDatabase_RegisterBoss_Twins;
		}

		#region TwinsDropDetour
		private void On_ItemDropDatabase_RegisterBoss_Twins(On_ItemDropDatabase.orig_RegisterBoss_Twins orig, ItemDropDatabase self) {
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
		private void On_Player_MowGrassTile(On_Player.orig_MowGrassTile orig, Player self, Vector2 thePos) {
			orig.Invoke(self, thePos);
			Point point = thePos.ToTileCoordinates();
			Tile tile = Main.tile[point.X, point.Y];
			if (tile == null || !WorldGen.CanKillTile(point.X, point.Y, WorldGen.SpecialKillTileContext.MowingTheGrass)) {
				return;
			}
			ushort num = 0;
			if (tile.TileType == ModContent.TileType<Tiles.CreamGrass>()) 
			{ 
				num = (ushort)ModContent.TileType<Tiles.CreamGrassMowed>();
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

		#region WorldUiOverlay
		private void On_UIWorldSelect_UpdateWorldsList(Terraria.GameContent.UI.States.On_UIWorldSelect.orig_UpdateWorldsList orig, Terraria.GameContent.UI.States.UIWorldSelect self) {
			orig(self);
			
			UIList WorldList = (UIList)typeof(UIWorldSelect).GetField("_worldList", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self);
			foreach (var item in WorldList) {
				if (item is UIWorldListItem) {
					UIElement _WorldIcon = (UIElement)typeof(UIWorldListItem).GetField("_worldIcon", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(item);
					//_WorldIcon = GetIconElement();

					UIElement WorldIcon = (UIElement)typeof(UIWorldListItem).GetField("_worldIcon", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(item);
					WorldFileData Data = (WorldFileData)typeof(AWorldListItem).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(item);

					var path = Path.ChangeExtension(Data.Path, ".twld");

					ConfectionConfig config = ModContent.GetInstance<ConfectionConfig>();
					Dictionary<string, ConfectionConfig.WorldDataValues> tempDict = config.GetWorldData();

					if (!tempDict.ContainsKey(path)) {
						byte[] buf = FileUtilities.ReadAllBytes(path, Data.IsCloudSave);
						var stream = new MemoryStream(buf);
						var tag = TagIO.FromStream(stream);
						bool containsMod = false;

						if (tag.ContainsKey("modData")) {
							foreach (TagCompound modDataTag in tag.GetList<TagCompound>("modData").Skip(2)) {
								if (modDataTag.Get<string>("mod") == ModContent.GetInstance<ConfectionConfig>().Mod.Name) {
									TagCompound dataTag = modDataTag.Get<TagCompound>("data");
									ConfectionConfig.WorldDataValues worldData;

									worldData.confection = dataTag.Get<bool>("TheConfectionRebirth:confectionorHallow");
									tempDict[path] = worldData;

									containsMod = true;

									break;
								}
							}

							if (!containsMod) {
								ConfectionConfig.WorldDataValues worldData;

								worldData.confection = false;
								tempDict[path] = worldData;
							}

							config.SetWorldData(tempDict);
							ConfectionConfig.Save(config);
						}
					}

					#region RegularSeedIcon
					if (tempDict[path].confection && !Data.RemixWorld && !Data.DrunkWorld && !Data.Anniversary && !Data.DontStarve && !Data.ForTheWorthy && !Data.ZenithWorld && !Data.NotTheBees && !Data.NoTrapsWorld && Data.IsHardMode) {
						UIElement worldIcon = WorldIcon;
						UIImage element = new UIImage(ModContent.Request<Texture2D>("TheConfectionRebirth/Assets/WorldIcons/ConfectionNormal")) {
							Top = new StyleDimension(0f, 0f),
							Left = new StyleDimension(1f, 0f),
							IgnoresMouseInteraction = true
						};
						worldIcon.Append(element);
					}
					#endregion

					#region AnniversarySeedIcon
					if (tempDict[path].confection && !Data.RemixWorld && !Data.DrunkWorld && Data.Anniversary && !Data.DontStarve && !Data.ForTheWorthy && !Data.ZenithWorld && !Data.NotTheBees && !Data.NoTrapsWorld && Data.IsHardMode) {
						UIElement worldIcon = WorldIcon;
						UIImage element = new UIImage(ModContent.Request<Texture2D>("TheConfectionRebirth/Assets/WorldIcons/ConfectionAnniversary")) {
							Top = new StyleDimension(0f, 0f),
							Left = new StyleDimension(0f, 0f),
							IgnoresMouseInteraction = true
						};
						worldIcon.Append(element);
					}
					#endregion

					#region DontStarveSeedIcon
					if (tempDict[path].confection && !Data.RemixWorld && !Data.DrunkWorld && !Data.Anniversary && Data.DontStarve && !Data.ForTheWorthy && !Data.ZenithWorld && !Data.NotTheBees && !Data.NoTrapsWorld && Data.IsHardMode) {
						UIElement worldIcon = WorldIcon;
						UIImage element = new UIImage(ModContent.Request<Texture2D>("TheConfectionRebirth/Assets/WorldIcons/ConfectionDontStarve")) {
							Top = new StyleDimension(0f, 0f),
							Left = new StyleDimension(0f, 0f),
							IgnoresMouseInteraction = true
						};
						worldIcon.Append(element);
					}
					#endregion

					#region DrunkSeedIcon
					if (tempDict[path].confection && !Data.RemixWorld && Data.DrunkWorld && !Data.Anniversary && !Data.DontStarve && !Data.ForTheWorthy && !Data.ZenithWorld && !Data.NotTheBees && !Data.NoTrapsWorld && Data.IsHardMode) {
						UIElement worldIcon = WorldIcon;
						UIImage element = new UIImage(ModContent.Request<Texture2D>("TheConfectionRebirth/Assets/WorldIcons/ConfectionDrunk")) {
							Top = new StyleDimension(0f, 0f),
							Left = new StyleDimension(1f, 0f),
							IgnoresMouseInteraction = true
						};
						worldIcon.Append(element);
					}
					#endregion

					#region FTWSeedIcon
					if (tempDict[path].confection && !Data.RemixWorld && !Data.DrunkWorld && !Data.Anniversary && !Data.DontStarve && Data.ForTheWorthy && !Data.ZenithWorld && !Data.NotTheBees && !Data.NoTrapsWorld && Data.IsHardMode) {
						UIElement worldIcon = WorldIcon;
						UIImage element = new UIImage(ModContent.Request<Texture2D>("TheConfectionRebirth/Assets/WorldIcons/ConfectionForTheWorthy")) {
							Top = new StyleDimension(0f, 0f),
							Left = new StyleDimension(0f, 0f),
							IgnoresMouseInteraction = true
						};
						worldIcon.Append(element);
					}
					#endregion

					#region NotTheBeesSeedIcon
					if (tempDict[path].confection && !Data.RemixWorld && !Data.DrunkWorld && !Data.Anniversary && !Data.DontStarve && !Data.ForTheWorthy && !Data.ZenithWorld && Data.NotTheBees && !Data.NoTrapsWorld && Data.IsHardMode) {
						UIElement worldIcon = WorldIcon;
						UIImage element = new UIImage(ModContent.Request<Texture2D>("TheConfectionRebirth/Assets/WorldIcons/ConfectionNotTheBees")) {
							Top = new StyleDimension(0f, 0f),
							Left = new StyleDimension(0f, 0f),
							IgnoresMouseInteraction = true
						};
						worldIcon.Append(element);
					}
					#endregion

					#region NoTrapsSeedIcon
					if (tempDict[path].confection && !Data.RemixWorld && !Data.DrunkWorld && !Data.Anniversary && !Data.DontStarve && !Data.ForTheWorthy && !Data.ZenithWorld && !Data.NotTheBees && Data.NoTrapsWorld && Data.IsHardMode) {
						UIElement worldIcon = WorldIcon;
						UIImage element = new UIImage(ModContent.Request<Texture2D>("TheConfectionRebirth/Assets/WorldIcons/ConfectionTrap")) {
							Top = new StyleDimension(0f, 0f),
							Left = new StyleDimension(1f, 0f),
							IgnoresMouseInteraction = true
						};
						worldIcon.Append(element);
					}
					#endregion

					#region RemixSeedIcon
					if (tempDict[path].confection && Data.RemixWorld && !Data.DrunkWorld && !Data.Anniversary && !Data.DontStarve && !Data.ForTheWorthy && !Data.ZenithWorld && !Data.NotTheBees && !Data.NoTrapsWorld && Data.IsHardMode) {
						UIElement worldIcon = WorldIcon;
						UIImage element = new UIImage(ModContent.Request<Texture2D>("TheConfectionRebirth/Assets/WorldIcons/ConfectionRemix")) {
							Top = new StyleDimension(0f, 0f),
							Left = new StyleDimension(1f, 0f),
							IgnoresMouseInteraction = true
						};
						worldIcon.Append(element);
					}
					#endregion

					#region RemixSeedIcon
					if (tempDict[path].confection && Data.RemixWorld && Data.DrunkWorld) {
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

		protected UIElement GetIconElement() {
			WorldFileData Data = (WorldFileData)typeof(AWorldListItem).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(null);
			if (Data.DrunkWorld && Data.RemixWorld) {
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

		private void UpdateGlitchAnimation(UIElement affectedElement) {
			_ = _glitchFrame;
			int minValue = 3;
			int num = 3;
			if (_glitchFrame == 0) {
				minValue = 15;
				num = 120;
			}
			if (++_glitchFrameCounter >= Main.rand.Next(minValue, num + 1)) {
				_glitchFrameCounter = 0;
				_glitchFrame = (_glitchFrame + 1) % 16;
				if ((_glitchFrame == 4 || _glitchFrame == 8 || _glitchFrame == 12) && Main.rand.Next(3) == 0) {
					_glitchVariation = Main.rand.Next(7);
				}
			}
			(affectedElement as UIImageFramed).SetFrame(7, 16, _glitchVariation, _glitchFrame, 0, 0);
		}
		#endregion

		#region OtherworldlyMusic
		private void Main_UpdateAudio_DecideOnTOWMusic(Terraria.On_Main.orig_UpdateAudio_DecideOnTOWMusic orig, Main self) {
			orig.Invoke(self);
			Player player = Main.CurrentPlayer;
			if (player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && !Main.LocalPlayer.ZoneRockLayerHeight) {
				Main.newMusic = 88; //Hallow Otherworldly
			}
			if (player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && Main.LocalPlayer.ZoneRockLayerHeight) {
				Main.newMusic = 78; //Underground Hallow Otherworldly
			}
		}
		#endregion
	}
>>>>>>> Stashed changes
=======
					return (style as IBackground).GetFarTexture(ConfectionWorld.ConfectionSurfaceBG[0]).Value.Height;
				}
				return v;
			});
		}
	}
>>>>>>> 21d961fc5a6b7f1a1395f5a436ef383fb42b52eb
}

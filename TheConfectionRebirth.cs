using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.ModSupport;
using System.Collections.Generic;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.IO;
using Terraria.GameContent.ItemDropRules;
using static TheConfectionRebirth.NPCs.BagDrops;
using TheConfectionRebirth.Hooks;
using Terraria.Localization;
using Terraria.Graphics;
using static Terraria.Graphics.FinalFractalHelper;
using TheConfectionRebirth.Items.Weapons;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Shaders;
using TheConfectionRebirth.Items.Placeable;
using TheConfectionRebirth.Dusts;
using static Terraria.Player;
using Terraria.GameContent;

namespace TheConfectionRebirth {
	public class TheConfectionRebirth : Mod
	{
		public static ShaderData GummyWyrmShaderData { get; private set; }

		internal static TheConfectionRebirth Instance;

		public static int[] confectBG = new int[3];

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
			Instance = this;

			if (!Main.dedServ) {
				GummyWyrmShaderData = new(new(Assets.Request<Effect>("Shaders/GummyWyrmShader", AssetRequestMode.ImmediateLoad).Value), "GummyWyrmPass");
			}

			var fractalProfiles = (Dictionary<int, FinalFractalProfile>)typeof(FinalFractalHelper).GetField("_fractalProfiles", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

			ConfectionWindUtilities.Load();

			fractalProfiles.Add(ModContent.ItemType<TrueSucrosa>(), new FinalFractalProfile(70f, new Color(224, 92, 165))); //Add the True Sucrosa with a pink trail
			fractalProfiles.Add(ModContent.ItemType<Sucrosa>(), new FinalFractalProfile(70f, new Color(224, 92, 165))); //Add the Sucrosa with a pink trail

			Terraria.GameContent.UI.States.IL_UIWorldCreation.BuildPage += ConfectionSelectionMenu.ILBuildPage;
			Terraria.GameContent.UI.States.IL_UIWorldCreation.MakeInfoMenu += ConfectionSelectionMenu.ILMakeInfoMenu;
			Terraria.GameContent.UI.States.IL_UIWorldCreation.ShowOptionDescription +=
				ConfectionSelectionMenu.ILShowOptionDescription;

			On_UIWorldListItem.DrawSelf += (orig, self, spriteBatch) => {
				orig(self, spriteBatch);
				DrawWorldSelectItemOverlay(self, spriteBatch);
			};

			Terraria.On_Player.MowGrassTile += On_Player_MowGrassTile;
			Terraria.GameContent.ItemDropRules.On_ItemDropDatabase.RegisterBoss_Twins += On_ItemDropDatabase_RegisterBoss_Twins;
			On_Lang.GetDryadWorldStatusDialog += On_Lang_GetDryadWorldStatusDialog;
			On_NPC.BigMimicSummonCheck += On_NPC_BigMimicSummonCheck;

			On_TileDrawing.DrawMultiTileVinesInWind += On_TileDrawing_DrawMultiTileVinesInWind;
			On_Main.DrawMapFullscreenBackground += On_Main_DrawMapFullscreenBackground;
			On_Player.PickAmmo_Item_refInt32_refSingle_refBoolean_refInt32_refSingle_refInt32_bool += On_Player_PickAmmo_Item_refInt32_refSingle_refBoolean_refInt32_refSingle_refInt32_bool;
			On_Player.ItemCheck_ApplyHoldStyle_Inner += On_Player_ItemCheck_ApplyHoldStyle_Inner;
		}

		public override void Unload()
		{
			Instance = null;
			var fractalProfiles = (Dictionary<int, FinalFractalProfile>)typeof(FinalFractalHelper).GetField("_fractalProfiles", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

			ConfectionWindUtilities.Unload();

			fractalProfiles.Remove(ModContent.ItemType<TrueSucrosa>());
			fractalProfiles.Remove(ModContent.ItemType<Sucrosa>());

			Terraria.On_Player.MowGrassTile -= On_Player_MowGrassTile;
			Terraria.GameContent.ItemDropRules.On_ItemDropDatabase.RegisterBoss_Twins -= On_ItemDropDatabase_RegisterBoss_Twins;
			On_Lang.GetDryadWorldStatusDialog -= On_Lang_GetDryadWorldStatusDialog;
			On_NPC.BigMimicSummonCheck -= On_NPC_BigMimicSummonCheck;

			On_TileDrawing.DrawMultiTileVinesInWind -= On_TileDrawing_DrawMultiTileVinesInWind;
			On_Main.DrawMapFullscreenBackground -= On_Main_DrawMapFullscreenBackground;
			On_Player.PickAmmo_Item_refInt32_refSingle_refBoolean_refInt32_refSingle_refInt32_bool -= On_Player_PickAmmo_Item_refInt32_refSingle_refBoolean_refInt32_refSingle_refInt32_bool;
			On_Player.ItemCheck_ApplyHoldStyle_Inner -= On_Player_ItemCheck_ApplyHoldStyle_Inner;
		}

		#region flareholditemdust
		private void On_Player_ItemCheck_ApplyHoldStyle_Inner(On_Player.orig_ItemCheck_ApplyHoldStyle_Inner orig, Player self, float mountOffset, Item sItem, Rectangle heldItemFrame) {
			orig.Invoke(self, mountOffset, sItem, heldItemFrame);
			if (sItem.holdStyle == 1 && !self.pulley) {
				if (Main.dedServ) {
					self.itemLocation.X = self.position.X + (float)self.width * 0.5f + 20f * (float)self.direction;
				}
				else if (sItem.type == ItemID.FlareGun) {
					self.itemLocation.X = self.position.X + (float)(self.width / 2) * 0.5f - 12f - (float)(2 * self.direction);
					float x = self.position.X + (float)(self.width / 2) + (float)(38 * self.direction);
					if (self.direction == 1) {
						x -= 10f;
					}
					float y = self.MountedCenter.Y - 4f * self.gravDir;
					if (self.gravDir == -1f) {
						y -= 8f;
					}
					self.RotateRelativePoint(ref x, ref y);
					int num3 = 0;
					for (int i = 54; i < 58; i++) {
						if (self.inventory[i].stack > 0 && self.inventory[i].ammo == 931) {
							num3 = self.inventory[i].type;
							break;
						}
					}
					if (num3 == 0) {
						for (int j = 0; j < 54; j++) {
							if (self.inventory[j].stack > 0 && self.inventory[j].ammo == 931) {
								num3 = self.inventory[j].type;
								break;
							}
						}
					}
					if (num3 == ModContent.ItemType<SherbetFlare>()) {
						int num4 = Dust.NewDust(new Vector2(x, y + self.gfxOffY), 6, 6, ModContent.DustType<SherbetDust>(), 0f, 0f, 100, default(Color), 1.6f);
						Main.dust[num4].noGravity = true;
						Main.dust[num4].velocity.Y -= 4f * self.gravDir;
						if (num3 == 66) {
							Main.dust[num4].color = Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.6f % 1f, 1f, 0.5f);
							Main.dust[num4].scale *= 0.5f;
							Dust obj = Main.dust[num4];
							obj.velocity *= 0.75f;
						}
					}
				}
			}
		}
		#endregion

		#region SandgunDetour
		private void On_Player_PickAmmo_Item_refInt32_refSingle_refBoolean_refInt32_refSingle_refInt32_bool(On_Player.orig_PickAmmo_Item_refInt32_refSingle_refBoolean_refInt32_refSingle_refInt32_bool orig, Player self, Item sItem, ref int projToShoot, ref float speed, ref bool canShoot, ref int totalDamage, ref float KnockBack, out int usedAmmoItemId, bool dontConsume) {
			orig.Invoke(self, sItem, ref projToShoot, ref speed, ref canShoot, ref totalDamage, ref KnockBack, out usedAmmoItemId, dontConsume);
			if (projToShoot == 42) {
				Item item = self.ChooseAmmo(sItem);
				if (item.type == ModContent.ItemType<Creamsand>()) {
					projToShoot = ModContent.ProjectileType<Projectiles.CreamsandSandgunProjectile>();
					totalDamage += 5;
				}
			}
		}
		#endregion

		#region MapBackgroundColorFixer
		private void On_Main_DrawMapFullscreenBackground(On_Main.orig_DrawMapFullscreenBackground orig, Vector2 screenPosition, int screenWidth, int screenHeight) {
			if (Main.LocalPlayer.InModBiome(ModContent.GetInstance<ConfectionBiome>())) 
			{
				Texture2D MapBGAsset = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Biomes/ConfectionBiomeMapBackground");
				Color color = Color.White;
				if ((double)screenPosition.Y > Main.worldSurface * 16.0) {
					MapBGAsset = Main.player[Main.myPlayer].ZoneDesert ? (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Biomes/ConfectionUndergroundDesertMapBackground") : ((!Main.player[Main.myPlayer].ZoneSnow) ? (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Biomes/ConfectionUndergroundMapBackground") : (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Biomes/ConfectionUndergroundIceMapBackground"));
				}
				else {
					color = Main.ColorOfTheSkies;
					MapBGAsset = ((!Main.player[Main.myPlayer].ZoneDesert) ? (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Biomes/ConfectionBiomeMapBackground") : ((Main.player[Main.myPlayer].ZoneSnow) ? (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Biomes/ConfectionIceBiomeMapBackground") : (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Biomes/ConfectionDesertBiomeMapBackground")));
				}
				Main.spriteBatch.Draw(MapBGAsset, new Rectangle(0, 0, screenWidth, screenHeight), color);
			}
			else {
				orig.Invoke(screenPosition, screenWidth, screenHeight);
			}
		}
		#endregion

		#region VineWindTileLength
		private void On_TileDrawing_DrawMultiTileVinesInWind(On_TileDrawing.orig_DrawMultiTileVinesInWind orig, TileDrawing self, Vector2 screenPosition, Vector2 offSet, int topLeftX, int topLeftY, int sizeX, int sizeY) {
			if (Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<Tiles.ConfectionBanners>()) {
				sizeY = 3;
			}
			else if (Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<Tiles.Furniture.CreamwoodChandelier>()) {
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
			}
			orig.Invoke(self, screenPosition, offSet, topLeftX, topLeftY, sizeX, sizeY);
		}
		#endregion

		#region NEWWorldIcondetour
		private void DrawWorldSelectItemOverlay(UIWorldListItem uiItem, SpriteBatch spriteBatch) {
			bool data = uiItem.Data.TryGetHeaderData(ModContent.GetInstance<ConfectionWorldGeneration>(), out var _data);
			UIElement WorldIcon = (UIElement)typeof(UIWorldListItem).GetField("_worldIcon", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(uiItem);
			WorldFileData Data = (WorldFileData)typeof(AWorldListItem).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(uiItem);

			if (data) {
				#region RegularSeedIcon
				if (_data.GetBool("HasConfection") && !Data.RemixWorld && !Data.DrunkWorld && !Data.Anniversary && !Data.DontStarve && !Data.ForTheWorthy && !Data.ZenithWorld && !Data.NotTheBees && !Data.NoTrapsWorld && Data.IsHardMode) {
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
				if (_data.GetBool("HasConfection") && !Data.RemixWorld && !Data.DrunkWorld && Data.Anniversary && !Data.DontStarve && !Data.ForTheWorthy && !Data.ZenithWorld && !Data.NotTheBees && !Data.NoTrapsWorld && Data.IsHardMode) {
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
				if (_data.GetBool("HasConfection") && !Data.RemixWorld && !Data.DrunkWorld && !Data.Anniversary && Data.DontStarve && !Data.ForTheWorthy && !Data.ZenithWorld && !Data.NotTheBees && !Data.NoTrapsWorld && Data.IsHardMode) {
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
				if (/*_data.GetBool("HasConfection") && */!Data.RemixWorld && Data.DrunkWorld && !Data.Anniversary && !Data.DontStarve && !Data.ForTheWorthy && !Data.ZenithWorld && !Data.NotTheBees && !Data.NoTrapsWorld && Data.IsHardMode) {
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
				if (_data.GetBool("HasConfection") && !Data.RemixWorld && !Data.DrunkWorld && !Data.Anniversary && !Data.DontStarve && Data.ForTheWorthy && !Data.ZenithWorld && !Data.NotTheBees && !Data.NoTrapsWorld && Data.IsHardMode) {
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
				if (_data.GetBool("HasConfection") && !Data.RemixWorld && !Data.DrunkWorld && !Data.Anniversary && !Data.DontStarve && !Data.ForTheWorthy && !Data.ZenithWorld && Data.NotTheBees && !Data.NoTrapsWorld && Data.IsHardMode) {
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
				if (_data.GetBool("HasConfection") && !Data.RemixWorld && !Data.DrunkWorld && !Data.Anniversary && !Data.DontStarve && !Data.ForTheWorthy && !Data.ZenithWorld && !Data.NotTheBees && Data.NoTrapsWorld && Data.IsHardMode) {
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
				if (_data.GetBool("HasConfection") && Data.RemixWorld && !Data.DrunkWorld && !Data.Anniversary && !Data.DontStarve && !Data.ForTheWorthy && !Data.ZenithWorld && !Data.NotTheBees && !Data.NoTrapsWorld && Data.IsHardMode) {
					UIElement worldIcon = WorldIcon;
					UIImage element = new UIImage(ModContent.Request<Texture2D>("TheConfectionRebirth/Assets/WorldIcons/ConfectionRemix")) {
						Top = new StyleDimension(0f, 0f),
						Left = new StyleDimension(1f, 0f),
						IgnoresMouseInteraction = true
					};
					worldIcon.Append(element);
				}
				#endregion

				#region ZenithSeedIcon
				if (_data.GetBool("HasConfection") && Data.RemixWorld && Data.DrunkWorld) {
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

		#region CorruptionMimic
		private bool On_NPC_BigMimicSummonCheck(On_NPC.orig_BigMimicSummonCheck orig, int x, int y, Player user) {
			if (user.width == -1) {
				orig.Invoke(x, y, user);
			}
			else {
				if (Main.netMode == NetmodeID.MultiplayerClient || !Main.hardMode) {
					return false;
				}
				int num = Chest.FindChest(x, y);
				if (num < 0) {
					return false;
				}
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				for (int i = 0; i < 40; i++) {
					ushort num5 = Main.tile[Main.chest[num].x, Main.chest[num].y].TileType;
					int num6 = Main.tile[Main.chest[num].x, Main.chest[num].y].TileFrameX / 36;
					if (TileID.Sets.BasicChest[num5] && (num5 != 21 || num6 < 5 || num6 > 6) && Main.chest[num].item[i] != null && Main.chest[num].item[i].type > 0) {
						if (Main.chest[num].item[i].type == ItemID.LightKey) {
							num2 += Main.chest[num].item[i].stack;
						}
						else if (Main.chest[num].item[i].type == ItemID.NightKey) {
							num3 += Main.chest[num].item[i].stack;
						}
						else {
							num4++;
						}
					}
				}
				if (num4 == 0 && num2 + num3 == 1) {
					if (num2 != 1) {
						_ = 1;
					}
					if (TileID.Sets.BasicChest[Main.tile[x, y].TileType]) {
						if (Main.tile[x, y].TileFrameX % 36 != 0) {
							x--;
						}
						if (Main.tile[x, y].TileFrameY % 36 != 0) {
							y--;
						}
						int number = Chest.FindChest(x, y);
						for (int j = 0; j < 40; j++) {
							Main.chest[num].item[j] = new Item();
						}
						Chest.DestroyChest(x, y);
						for (int k = x; k <= x + 1; k++) {
							for (int l = y; l <= y + 1; l++) {
								if (TileID.Sets.BasicChest[Main.tile[k, l].TileType]) {
									Main.tile[k, l].ClearTile();
								}
							}
						}
						int number2 = 1;
						if (Main.tile[x, y].TileType == 467) {
							number2 = 5;
						}
						NetMessage.SendData(MessageID.ChestUpdates, -1, -1, null, number2, x, y, 0f, number);
						NetMessage.SendTileSquare(-1, x, y, 3);
					}
					int num7 = 475;
					if (num3 == 1) {
						num7 = 473;
					}
					int num8 = NPC.NewNPC(NPC.GetSource_NaturalSpawn(), x * 16 + 16, y * 16 + 32, num7);
					Main.npc[num8].whoAmI = num8;
					NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num8);
					Main.npc[num8].BigMimicSpawnSmoke();
				}
				return false;
			}
			return false;
		}
		#endregion

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
				text = Language.GetTextValue("Mods.TheConfectionRebirth.DryadSpecialText.WorldStatusAll", Main.worldName, tGood, tCandy, tEvil, tBlood);
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
	}

	public static class ConfectionWindUtilities {
		public static void Load() {
			_addSpecialPointSpecialPositions = typeof(Terraria.GameContent.Drawing.TileDrawing).GetField("_specialPositions", BindingFlags.NonPublic | BindingFlags.Instance);
			_addSpecialPointSpecialsCount = typeof(Terraria.GameContent.Drawing.TileDrawing).GetField("_specialsCount", BindingFlags.NonPublic | BindingFlags.Instance);
			_addVineRootPositions = typeof(Terraria.GameContent.Drawing.TileDrawing).GetField("_vineRootsPositions", BindingFlags.NonPublic | BindingFlags.Instance);
		}

		public static void Unload() {
			_addSpecialPointSpecialPositions = null;
			_addSpecialPointSpecialsCount = null;
			_addVineRootPositions = null;
		}

		public static FieldInfo _addSpecialPointSpecialPositions;
		public static FieldInfo _addSpecialPointSpecialsCount;
		public static FieldInfo _addVineRootPositions;

		public static void AddSpecialPoint(this Terraria.GameContent.Drawing.TileDrawing tileDrawing, int x, int y, int type) {
			if (_addSpecialPointSpecialPositions.GetValue(tileDrawing) is Point[][] _specialPositions) {
				if (_addSpecialPointSpecialsCount.GetValue(tileDrawing) is int[] _specialsCount) {
					_specialPositions[type][_specialsCount[type]++] = new Point(x, y);
				}
			}
		}

		public static void CrawlToTopOfVineAndAddSpecialPoint(this Terraria.GameContent.Drawing.TileDrawing tileDrawing, int j, int i) {
			if (_addVineRootPositions.GetValue(tileDrawing) is List<Point> _vineRootsPositions) {
				int y = j;
				for (int num = j - 1; num > 0; num--) {
					Tile tile = Main.tile[i, num];
					if (WorldGen.SolidTile(i, num) || !tile.HasTile) {
						y = num + 1;
						break;
					}
				}
				Point item = new(i, y);
				if (!_vineRootsPositions.Contains(item)) {
					_vineRootsPositions.Add(item);
					Main.instance.TilesRenderer.AddSpecialPoint(i, y, 6);
				}
			}
		}
	}
}

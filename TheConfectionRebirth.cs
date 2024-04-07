using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Animations;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Events;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Skies.CreditsRoll;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Hooks;
using TheConfectionRebirth.Items.Placeable;
using TheConfectionRebirth.Items.Weapons;
using TheConfectionRebirth.ModSupport;
using static Terraria.Graphics.FinalFractalHelper;
using static TheConfectionRebirth.NPCs.BagDrops;

namespace TheConfectionRebirth {
	public class TheConfectionRebirth : Mod {
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

		public override void PostSetupContent() {
			SummonersShineThoughtBubble.PostSetupContent();
			StackableBuffData.PostSetupContent();
			ModSupport.ModSupportBaseClass.HookAll();
		}

		public override void Load() {
			Instance = this;

			if (!Main.dedServ) {
				GummyWyrmShaderData = new(ModContent.Request<Effect>("TheConfectionRebirth/Shaders/GummyWyrmShader", AssetRequestMode.ImmediateLoad), "GummyWyrmPass");
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

			//credits
			IL_CreditsRollComposer.FillSegments += FillCreditSegmentILEdit;
			IL_CreditsRollEvent.TryStartingCreditsRoll += CreditsRollIngameTimeDurationExtention;
			IL_CreditsRollEvent.UpdateTime += CreditsRollIngameTimeDurationExtention;
			IL_CreditsRollEvent.SetRemainingTimeDirect += CreditsRollIngameTimeDurationExtention;
		}

		public override void Unload() {
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

			//credits
			IL_CreditsRollComposer.FillSegments -= FillCreditSegmentILEdit;
			IL_CreditsRollEvent.TryStartingCreditsRoll -= CreditsRollIngameTimeDurationExtention;
			IL_CreditsRollEvent.UpdateTime -= CreditsRollIngameTimeDurationExtention;
			IL_CreditsRollEvent.SetRemainingTimeDirect -= CreditsRollIngameTimeDurationExtention;
		}

		public override object Call(params object[] args) {
			//For Content creators: Message me (Lion8cake) on discord if you have any mod call suggestions
			return args switch {
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

		#region credits
		private SegmentInforReport PlaySegment_ModdedTextRoll(CreditsRollComposer self, int startTime, string sourceCategory, Vector2 anchorOffset = default(Vector2)) {
			//We have our own text roll segment due to tmodloader using Hjson instead of json meaning that sometimes the order of names becomes backwards
			//if you want to use vanilla text for some reason i would recomend that you reflect CreditsRollComposer.PlaySegment_TextRoll
			List<IAnimationSegment> _segments = (List<IAnimationSegment>)typeof(CreditsRollComposer).GetField("_segments", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(self);
			anchorOffset.Y -= 40f;
			int num = 80;
			LocalizedText[] array = Language.FindAll(Lang.CreateDialogFilter(sourceCategory + ".", null));
			for (int i = 0; i < array.Length; i++) {
				_segments.Add(new Segments.LocalizedTextSegment(startTime + i * num, Language.GetText(sourceCategory + "." + (i + 1)), anchorOffset));
			}
			SegmentInforReport result = default(SegmentInforReport);
			result.totalTime = array.Length * num + num * -1;
			return result;
		}

		private SegmentInforReport PlaySegment_LionEightCake_HungryStyalist(CreditsRollComposer self, int startTime, Vector2 sceneAnchorPosition) {
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
			Segments.AnimationSegmentWithActions<NPC> hoardEnemy1 = new Segments.NPCSegment(duration, ModContent.NPCType<NPCs.SweetGummy>(), sceneAnchorPosition + new Vector2(250f, 0f), _originAtBottom).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51)).Then(new Actions.NPCs.LookAt(-1))
				.Then(new Actions.NPCs.Move(new(-1.7f, 0f), 130));
			hoardEnemy1.Then(new Actions.NPCs.Move(new(-0.6f, 0f), 51)).With(new Actions.NPCs.Fade(5, 51));
			duration += 7; //Spawn a sweet gummy and wait 7 frames
			Segments.AnimationSegmentWithActions<NPC> hoardEnemy2 = new Segments.NPCSegment(duration, ModContent.NPCType<NPCs.WildWilly>(), sceneAnchorPosition + new Vector2(250f, 0f), _originAtBottom).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51)).Then(new Actions.NPCs.LookAt(-1))
				.Then(new Actions.NPCs.Move(new(-1.7f, 0f), 123));
			hoardEnemy2.Then(new Actions.NPCs.Move(new(-0.6f, 0f), 51)).With(new Actions.NPCs.Fade(5, 51));
			duration += 22; //Spawn a Wild Willy and wait 22 frames
			Segments.AnimationSegmentWithActions<NPC> hoardEnemy3 = new Segments.NPCSegment(duration, ModContent.NPCType<NPCs.IcecreamGal>(), sceneAnchorPosition + new Vector2(250f, 0f), _originAtBottom).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51)).Then(new Actions.NPCs.LookAt(-1))
				.Then(new Actions.NPCs.Move(new(-1.7f, 0f), 101));
			hoardEnemy3.Then(new Actions.NPCs.Move(new(-0.6f, 0f), 51)).With(new Actions.NPCs.Fade(5, 51));
			duration += 20; //Spawn a Icecream Gal and wait 20 frames
			Segments.AnimationSegmentWithActions<NPC> hoardEnemy4 = new Segments.NPCSegment(duration, ModContent.NPCType<NPCs.CreamsandWitchPhase2>(), sceneAnchorPosition + new Vector2(250f, 0f), _originAtBottom).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51)).Then(new Actions.NPCs.LookAt(-1))
				.Then(new Actions.NPCs.Move(new(-1.7f, 0f), 81));
			hoardEnemy4.Then(new Actions.NPCs.Move(new(-0.6f, 0f), 51)).With(new Actions.NPCs.Fade(5, 51));
			duration += 24; //Spawn a Creamsand witch (standing npc) and wait 24 frames
			Asset<Texture2D> rollerxCookieTexture = ModContent.Request<Texture2D>("TheConfectionRebirth/NPCs/Rollercookie", AssetRequestMode.ImmediateLoad); //Make sure that its ImmediateLoad otherwise 90% of the time it wont load
			int FrameCountX = 9; //For less repeated code
			int FrameCountY = 2; //For less repeated code
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
			_segments.Add(hoardEnemy4);
			_segments.Add(hoardEnemy5);
			duration += 120; //Give a final duration time until the next part of the credits loads
			SegmentInforReport FinalDurationTime = default(SegmentInforReport);
			FinalDurationTime.totalTime = duration - startTime;
			return FinalDurationTime; //Return the duration of the animation so the next text or animation can play fluently and straight after
		}

		private void FillCreditSegmentILEdit(ILContext il) {
			ILCursor c = new ILCursor(il); //place a IL Cursor
			c.GotoNext(MoveType.Before, i => i.MatchLdloc0(), i => i.MatchLdarg0(), i => i.MatchLdloc0(), i => i.MatchLdstr("CreditsRollCategory_Creator"), i => i.MatchLdloc3());
			//make sure all instructions match, movetype will place our code before the first instruction once all instructions match
			c.EmitLdarg(0); //Emit ldarg_0 (self)
			c.EmitLdloca(0); //Emit ldloc_0 (num)
			c.EmitLdloca(2); //Emit ldloc_2 (num3)
			c.EmitLdloca(3); //Emit ldloc_3 (vector2 or val2)
			c.EmitDelegate((CreditsRollComposer self, ref int num, ref int num3, ref Vector2 vector2) => { //Get the needed variables and instance
				//Edit inside here for more text and animations, shown here is just how to add 1 text and 1 animation
				num += PlaySegment_ModdedTextRoll(self, num, "Mods.TheConfectionRebirth.CreditsRollCategory_ConfectionTeam", vector2).totalTime; //Play our credit text
				num += num3; //wait
				num += PlaySegment_LionEightCake_HungryStyalist(self, num, vector2).totalTime; //Play our custom animation
				num += num3; //wait
			});
		}

		private void CreditsRollIngameTimeDurationExtention(ILContext il) {
			ILCursor c = new ILCursor(il); //place a IL cursor
			c.GotoNext(MoveType.After, i => i.MatchLdcI4(28800)); //Look for a LDC I4 instruction with 28800 (all timers use this)
			c.EmitDelegate<Func<int, int>>(maxDuration => maxDuration + 60 * 35); //Adds ontop of the max duration to account for the custom credits
		}
		#endregion

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
			if (Main.LocalPlayer.InModBiome(ModContent.GetInstance<ConfectionBiome>())) {
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
		private void On_ItemDropDatabase_RegisterBoss_Twins(On_ItemDropDatabase.orig_RegisterBoss_Twins orig, ItemDropDatabase self) {
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
		private void On_Player_MowGrassTile(On_Player.orig_MowGrassTile orig, Player self, Vector2 thePos) {
			orig.Invoke(self, thePos);
			Point point = thePos.ToTileCoordinates();
			Tile tile = Main.tile[point.X, point.Y];
			if (tile == null || !WorldGen.CanKillTile(point.X, point.Y, WorldGen.SpecialKillTileContext.MowingTheGrass)) {
				return;
			}
			ushort num = 0;
			if (tile.TileType == ModContent.TileType<Tiles.CreamGrass>()) {
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

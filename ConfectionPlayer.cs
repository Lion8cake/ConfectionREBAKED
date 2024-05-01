using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Utilities;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Util;
using TheConfectionRebirth.Items.Weapons;
using System.Reflection.Metadata;
using TheConfectionRebirth.Projectiles;
using Terraria.Localization;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.ID;

namespace TheConfectionRebirth {
	public class ConfectionPlayer : ModPlayer {
		public bool RollerCookiePet;
		public bool CreamsandWitchPet;
		public bool minitureCookie;
		public bool littleMeawzer;
		public bool miniGastropod;
		public bool flyingGummyFish;
		public bool birdnanaLightPet;
		public bool MeawzerPet;
		public bool DudlingPet;
		public bool FoxPet;
		public bool NeapoliniteMagicSet;
		public bool NeapoliniteSummonerSet;
		public bool cookiePet;
		public bool CandySuffocation;

		public Projectile DimensionalWarp;
		public Projectile BananawarpPeelWarp;

		public float neapoliniteSummonTimer;

		public BinaryHeap<TimerData> Timer;
		public int VanillaValorDamageDealt;
		public int ManaConsumed;
		public bool StrawberryStrikeOnCooldown;

		public override void OnEnterWorld() {
			Timer = new(TimerData.Comparer);
			VanillaValorDamageDealt = 0;
			ManaConsumed = 0;
			StrawberryStrikeOnCooldown = false;
		}
		public override void ResetEffects() {
			RollerCookiePet = false;
			CreamsandWitchPet = false;
			minitureCookie = false;
			littleMeawzer = false;
			miniGastropod = false;
			flyingGummyFish = false;
			birdnanaLightPet = false;
			MeawzerPet = false;
			DudlingPet = false;
			FoxPet = false;
			CandySuffocation = false;
			NeapoliniteMagicSet = false;
			NeapoliniteSummonerSet = false;
			cookiePet = false;
		}

		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource) {
			if (damageSource.SourceCustomReason == "DimensionSplit") {
				WeightedRandom<string> deathmessage = new();
				deathmessage.Add(Language.GetTextValue("Mods.TheConfectionRebirth.PlayerDeathReason.DimensionSplit.0", Player.name));
				deathmessage.Add(Language.GetTextValue("Mods.TheConfectionRebirth.PlayerDeathReason.DimensionSplit.1", Player.name));
				deathmessage.Add(Language.GetTextValue("Mods.TheConfectionRebirth.PlayerDeathReason.DimensionSplit.2", Player.name));
				deathmessage.Add(Language.GetTextValue("Mods.TheConfectionRebirth.PlayerDeathReason.DimensionSplit.3", Player.name));
				damageSource = PlayerDeathReason.ByCustomReason(deathmessage);
				return true;
			}
			return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
		}

		public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition) {
			bool inWater = !attempt.inLava && !attempt.inHoney;
			bool inConfectionSurfaceBiome = Player.InModBiome(ModContent.GetInstance<ConfectionBiome>());

			if (inWater && inConfectionSurfaceBiome) {
				if (Player.ZoneDesert && Main.rand.NextBool(2)) {
					if (attempt.uncommon && attempt.questFish == 4393) {
						itemDrop = 4393;
					}
					else if (attempt.uncommon && attempt.questFish == 4394) {
						itemDrop = 4394;
					}
					else if (attempt.uncommon) {
						itemDrop = 4410;
					}
					else if (Main.rand.NextBool(3)) {
						itemDrop = 4402;
					}
					else {
						itemDrop = 4401;
					}
				}
				else if (attempt.legendary && Main.hardMode && Player.ZoneSnow && attempt.heightLevel == 3 && !Main.rand.NextBool(3)) {
					itemDrop = 2429;
				}
				else if (attempt.legendary && Main.hardMode && Main.rand.NextBool(2)) {
					itemDrop = ModContent.ItemType<Items.Weapons.Minions.DuchessPrincess.GummyStaff>();
				}
				else if (attempt.rare && attempt.crate) {
					itemDrop = (Main.hardMode ? ModContent.ItemType<Items.Placeable.ConfectionCrate>() : ModContent.ItemType<Items.Placeable.BananaSplitCrate>());
				}
				else if (attempt.legendary && Main.hardMode && !Main.rand.NextBool(3)) {
					itemDrop = ModContent.ItemType<Items.Placeable.SweetAndSavage>();
				}
				else if (attempt.heightLevel > 1 && attempt.veryrare) {
					itemDrop = ModContent.ItemType<Items.SugarFish>();
				}
				else if (attempt.heightLevel > 1 && attempt.uncommon && attempt.questFish == ModContent.ItemType<Items.SacchariteBatFish>()) {
					itemDrop = ModContent.ItemType<Items.SacchariteBatFish>();
				}
				else if (attempt.heightLevel < 2 && attempt.uncommon && attempt.questFish == ModContent.ItemType<Items.Sprinklefish>()) {
					itemDrop = ModContent.ItemType<Items.Sprinklefish>();
				}
				else if (attempt.rare) {
					itemDrop = ModContent.ItemType<Items.Cakekite>();
				}
				else if (attempt.uncommon && attempt.questFish == ModContent.ItemType<Items.CookieCutterShark>()) {
					itemDrop = ModContent.ItemType<Items.CookieCutterShark>();
				}
				else if (attempt.uncommon) {
					itemDrop = ModContent.ItemType<Items.CookieCarp>();
				}
			}
		}
		const int oneStageNeapolioniteSummoner = 8 * 60;
		public override void PostUpdate() {
			if (NeapoliniteSummonerSet) {
				neapoliniteSummonTimer++;
				float progress = neapoliniteSummonTimer / oneStageNeapolioniteSummoner;
				int rank = (int)progress;
				int timer = (int)(oneStageNeapolioniteSummoner - neapoliniteSummonTimer % oneStageNeapolioniteSummoner);
				StackableBuffData.SwirlySwarm.AscendBuff(Player, rank - 1, timer, rank == 5);
				if (neapoliniteSummonTimer >= 2400) {
					neapoliniteSummonTimer = 2400;
				}
				if (NeapoliniteSummonerSet == false) {
					neapoliniteSummonTimer = 0;
				}
			}

			Timer ??= new(TimerData.Comparer);
			while (Timer.items.Count > 0 && Timer.items[0].endTime == Main.GameUpdateCount) {
				TimerData top = Timer.Pop();
				switch (top.type) {
					case TimerDataType.MeleeDamage:
						VanillaValorDamageDealt -= top.value;
						break;
					case TimerDataType.MagicManaRegeneration:
						ManaConsumed -= top.value;
						break;
					case TimerDataType.StrawberryStrikeDelay:
						StrawberryStrikeOnCooldown = false;
						break;
				}
			}
			//Main.NewText(VanillaValorDamageDealt);
		}

		public override void GetDyeTraderReward(List<int> rewardPool) {
			rewardPool.Add(ModContent.ItemType<Items.CandyCornDye>());
			rewardPool.Add(ModContent.ItemType<Items.FoaminWispDye>());
			rewardPool.Add(ModContent.ItemType<Items.GummyWispDye>());
			rewardPool.Add(ModContent.ItemType<Items.SwirllingChocolateDye>());
		}

		public override void OnConsumeMana(Item item, int manaConsumed) {
			if (item.CountsAsClass(DamageClass.Magic)) {
				ManaConsumed += manaConsumed;
				Timer.Add(new(manaConsumed, 180, TimerDataType.MagicManaRegeneration));
			}
		}

		public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
			if (item.CountsAsClass(DamageClass.Melee))
				AddDamage(hit.Damage);
		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			if (proj.CountsAsClass(DamageClass.Melee))
				AddDamage(hit.Damage);
		}

		/*public override void OnHurt(Player.HurtInfo info) {
			//if (info.DamageSource.SourceItem.CountsAsClass(DamageClass.Melee)) {
				AddDamage(info.Damage);
			//}
			//Note due to HitInfo not having SourceProjectile melee projectiles may break
		}*/

		void AddDamage(int damage) {
			this.VanillaValorDamageDealt += damage;
			Timer.Add(new(damage, 300, TimerDataType.MeleeDamage));
		}

		public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
			StackableBuffData.SwirlySwarm.DeleteBuff(Player);
			neapoliniteSummonTimer = Math.Max(neapoliniteSummonTimer - (neapoliniteSummonTimer % oneStageNeapolioniteSummoner) - oneStageNeapolioniteSummoner * 2, 0);
		}
	}

	public class SpiteMimicSpawning : ModPlayer {
		public int LastChest;

		public override void PreUpdateBuffs() {
			if (Main.netMode != NetmodeID.MultiplayerClient) {
				if (Player.chest == -1 && LastChest >= 0 && Main.chest[LastChest] != null) {
					int x2 = Main.chest[LastChest].x;
					int y2 = Main.chest[LastChest].y;
					ChestItemSummonCheck(x2, y2, Mod);
				}
				LastChest = Player.chest;
			}
		}

		public override void UpdateAutopause() {
			LastChest = Player.chest;
		}

		public static bool ChestItemSummonCheck(int x, int y, Mod mod) {
			if (Main.netMode == NetmodeID.MultiplayerClient || !Main.hardMode) {
				return false;
			}
			int num = Chest.FindChest(x, y);
			if (num < 0) {
				return false;
			}
			int numberKeyofDelight = 0;
			int numberOtherItems = 0;
			ushort tileType = Main.tile[Main.chest[num].x, Main.chest[num].y].TileType;
			int tileStyle = (int)(Main.tile[Main.chest[num].x, Main.chest[num].y].TileFrameX / 36);
			if (TileID.Sets.BasicChest[tileType] && (tileStyle < 5 || tileStyle > 6)) {
				for (int i = 0; i < 40; i++) {
					if (Main.chest[num].item[i] != null && Main.chest[num].item[i].type > ItemID.None) {
						if (Main.chest[num].item[i].type == ModContent.ItemType<Items.KeyofSpite>()) {
							numberKeyofDelight += Main.chest[num].item[i].stack;
						}
						else {
							numberOtherItems++;
						}
					}
				}
			}
			if (numberOtherItems == 0 && numberKeyofDelight == 1) {
				if (TileID.Sets.BasicChest[Main.tile[x, y].TileType]) {
					if (Main.tile[x, y].TileFrameX % 36 != 0) {
						x--;
					}
					if (Main.tile[x, y].TileFrameY % 36 != 0) {
						y--;
					}
					int number = Chest.FindChest(x, y);
					for (int j = x; j <= x + 1; j++) {
						for (int k = y; k <= y + 1; k++) {
							if (TileID.Sets.BasicChest[Main.tile[j, k].TileType]) {
								Tile tile = Main.tile[j, k];
								tile.HasTile = false;
							}
						}
					}
					for (int l = 0; l < 40; l++) {
						Main.chest[num].item[l] = new Item();
					}
					Chest.DestroyChest(x, y);
					NetMessage.SendData(MessageID.ChestUpdates, -1, -1, null, 1, (float)x, (float)y, 0f, number, 0, 0);
					NetMessage.SendTileSquare(-1, x, y, 3);
				}
				int npcToSpawn = NPCID.BigMimicCrimson;
				int npcIndex = NPC.NewNPC(NPC.GetSource_NaturalSpawn(), x * 16 + 16, y * 16 + 32, npcToSpawn, 0, 0f, 0f, 0f, 0f, 255);
				Main.npc[npcIndex].whoAmI = npcIndex;
				NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npcIndex, 0f, 0f, 0f, 0, 0, 0);
				Main.npc[npcIndex].BigMimicSpawnSmoke();
			}
			return false;
		}
	}

	public abstract class NeapoliniteArmourDrawLayer : PlayerDrawLayer {
		public virtual int NeapoliniteHelmetNumber(ref PlayerDrawSet drawInfo) {
			Player drawPlayer = drawInfo.drawPlayer;
			int HelmetType = 0;
			if (drawPlayer.armor[10].type == ModContent.ItemType<Items.Armor.NeapoliniteHat>()) {
				HelmetType = 4;
			}
			else if (drawPlayer.armor[10].type == ModContent.ItemType<Items.Armor.NeapoliniteHeadgear>()) {
				HelmetType = 3;
			}
			else if (drawPlayer.armor[10].type == ModContent.ItemType<Items.Armor.NeapoliniteHelmet>()) {
				HelmetType = 2;
			}
			else if (drawPlayer.armor[10].type == ModContent.ItemType<Items.Armor.NeapoliniteMask>()) {
				HelmetType = 1;
			}
			else if (drawPlayer.armor[0].type == ModContent.ItemType<Items.Armor.NeapoliniteHat>()) {
				HelmetType = 4;
			}
			else if (drawPlayer.armor[0].type == ModContent.ItemType<Items.Armor.NeapoliniteHeadgear>()) {
				HelmetType = 3;
			}
			else if (drawPlayer.armor[0].type == ModContent.ItemType<Items.Armor.NeapoliniteHelmet>()) {
				HelmetType = 2;
			}
			else if (drawPlayer.armor[0].type == ModContent.ItemType<Items.Armor.NeapoliniteMask>()) {
				HelmetType = 1;
			}
			return HelmetType;
		}

		public override Position GetDefaultPosition() {
			return new AfterParent(PlayerDrawLayers.Torso);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo) {
		}
	}
	
	public class NeapoliniteConeMailDrawing : NeapoliniteArmourDrawLayer {
		public override Position GetDefaultPosition() {
			return new AfterParent(PlayerDrawLayers.Torso);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo) {
			Player drawPlayer = drawInfo.drawPlayer;
			int HelmetType = NeapoliniteHelmetNumber(ref drawInfo);
			if (drawInfo.drawPlayer.dead || HelmetType == 0) {
				return;
			}
			if ((drawPlayer.armor[11].type == 0 && drawPlayer.armor[1].type == ModContent.ItemType<Items.Armor.NeapoliniteConeMail>()) || drawPlayer.armor[11].type == ModContent.ItemType<Items.Armor.NeapoliniteConeMail>()) {
				Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Items/Armor/NeapoliniteConeMail_Body_" + HelmetType);

				float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2;
				float drawY = (int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
				Vector2 origin = drawInfo.bodyVect;
				Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
				Rectangle frame = new(0, 0, 40, 56);
				if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 7, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 8, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 9, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 14, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 15, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 16, 40, 56)) {
					frame = new(0, 2, 40, 56); //walking bop
				}
				if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 6, 40, 56)) {
					frame = new(40, 0, 40, 56); //jumping frame
				}
				if (!drawPlayer.Male) {
					frame = new(0, 112, 40, 56);
					if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 7, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 8, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 9, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 14, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 15, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 16, 40, 56)) {
						frame = new(0, 114, 40, 56); //walking bop
					}
					if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 6, 40, 56)) {
						frame = new(40, 112, 40, 56); //jumping frame
					}
				}
				float rotation = drawPlayer.bodyRotation;
				SpriteEffects spriteEffects = drawInfo.playerEffect;

				DrawData drawData = new(texture, position, frame, drawInfo.colorArmorBody, rotation, origin, 1f, spriteEffects, 0);
				drawData.shader = drawInfo.cBody;
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}
	public class NeapoliniteGlenohumeralJointDrawing : NeapoliniteArmourDrawLayer { //Shoulder Drawing
		public override Position GetDefaultPosition() {
			return new AfterParent(PlayerDrawLayers.ArmOverItem);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo) {
			Player drawPlayer = drawInfo.drawPlayer;
			int HelmetType = NeapoliniteHelmetNumber(ref drawInfo);
			if (drawInfo.drawPlayer.dead || HelmetType == 0) {
				return;
			}
			if ((drawPlayer.armor[11].type == 0 && drawPlayer.armor[1].type == ModContent.ItemType<Items.Armor.NeapoliniteConeMail>()) || drawPlayer.armor[11].type == ModContent.ItemType<Items.Armor.NeapoliniteConeMail>()) {
				Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Items/Armor/NeapoliniteConeMail_Body_" + HelmetType);

				float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2;
				float drawY = (int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
				Vector2 origin = drawInfo.bodyVect;
				Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
				Rectangle frame = new(0, 56, 40, 56);
				if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 7, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 8, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 9, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 14, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 15, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 16, 40, 56)) {
					frame = new(0, 58, 40, 56); //walking bop
				}
				if (!drawPlayer.Male) {
					frame = new(0, 168, 40, 56);
					if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 7, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 8, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 9, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 14, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 15, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 16, 40, 56)) {
						frame = new(0, 170, 40, 56); //walking bop
					}
				}
				float rotation = drawPlayer.bodyRotation;
				SpriteEffects spriteEffects = drawInfo.playerEffect;

				DrawData drawData = new(texture, position, frame, drawInfo.colorArmorBody, rotation, origin, 1f, spriteEffects, 0);
				drawData.shader = drawInfo.cBody;
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}

	public class NeapoliniteUpperLimbDrawing : NeapoliniteArmourDrawLayer { //Arm Drawing
		public override Position GetDefaultPosition() {
			return new AfterParent(PlayerDrawLayers.ArmOverItem);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo) {
			Player drawPlayer = drawInfo.drawPlayer;
			int HelmetType = NeapoliniteHelmetNumber(ref drawInfo);
			if (drawInfo.drawPlayer.dead || HelmetType == 0) {
				return;
			}
			if ((drawPlayer.armor[11].type == 0 && drawPlayer.armor[1].type == ModContent.ItemType<Items.Armor.NeapoliniteConeMail>()) || drawPlayer.armor[11].type == ModContent.ItemType<Items.Armor.NeapoliniteConeMail>()) {
				Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Items/Armor/NeapoliniteConeMail_Body_" + HelmetType);

				float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2;
				float drawY = (int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
				Vector2 origin = drawInfo.bodyVect;
				Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
				Rectangle frame;
				if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 5, 40, 56)) {
					frame = new(80, 56, 40, 56); //Jumping
				}
				else if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 1, 40, 56)) {
					frame = new(120, 0, 40, 56); //Use1
				}
				else if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 2, 40, 56)) {
					frame = new(160, 0, 40, 56); //Use2
				}
				else if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 3, 40, 56)) {
					frame = new(200, 0, 40, 56); //Use3
				}
				else if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 4, 40, 56)) {
					frame = new(240, 0, 40, 56); //Use4
				}
				else {
					frame = new(0, 0, 0, 0); //None
				}
				float rotation = drawPlayer.bodyRotation;
				SpriteEffects spriteEffects = drawInfo.playerEffect;

				DrawData drawData = new(texture, position, frame, drawInfo.colorArmorBody, rotation, origin, 1f, spriteEffects, 0);
				drawData.shader = drawInfo.cBody;
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}
	public class TopCakeCandlesDrawing : PlayerDrawLayer {
		public override Position GetDefaultPosition() {
			return new AfterParent(PlayerDrawLayers.Head);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo) {
			Player drawPlayer = drawInfo.drawPlayer;
			if (!drawPlayer.dead && ((drawPlayer.armor[10].type == 0 && drawPlayer.armor[0].type == ModContent.ItemType<Items.Armor.TopCake>()) || drawPlayer.armor[10].type == ModContent.ItemType<Items.Armor.TopCake>())) {
				Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Items/Armor/TopCake_Candles");

				float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2;
				float drawY = (int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 - 5.5f;
				Vector2 origin = drawInfo.headVect;
				Vector2 position = new Vector2(drawX, drawY) + drawPlayer.headPosition - Main.screenPosition;
				Rectangle frame = drawPlayer.bodyFrame;
				float rotation = drawPlayer.headRotation;
				SpriteEffects spriteEffects = drawInfo.playerEffect;

				DrawData drawData = new(texture, position, frame, drawInfo.colorArmorHead, rotation, origin, 1f, spriteEffects, 0);
				drawData.shader = drawInfo.cHead;
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}
	/*public class NeapoliniteCookieDrawing : PlayerDrawLayer {

		public static int CookieSpinTimer = 0;

		public static bool CookieReturn = false;

		public static int CookieTurnAdditive = 0;

		public static int CookieUpTimer = 0;

		public static bool CookieUpReturn = false;

		public override Position GetDefaultPosition() {
			return PlayerDrawLayers.AfterLastVanillaLayer;
		}

		protected override void Draw(ref PlayerDrawSet drawInfo) {

			if (drawInfo.shadow != 0f || drawInfo.drawPlayer.dead) {
				return;
			}

			Player drawPlayer = drawInfo.drawPlayer;

			if (drawPlayer.HasBuff(ModContent.BuffType<Buffs.NeapoliniteBuffs.ChocolateChargeI>())) {
				Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Assets/NeapoliniteCookies");

				if (CookieReturn == false) {
					CookieSpinTimer++;
				}	
				else if (CookieReturn == true) {
					CookieSpinTimer--;
				}

				if (CookieSpinTimer > 60) {
					CookieReturn = true;
				}
				else if (CookieSpinTimer < 0) {
					CookieReturn = false;
				}

				CookieSpinTimer = CookieSpinTimer * (CookieTurnAdditive / 10);

				if (CookieReturn == false) {
					if (CookieSpinTimer >= 0 && CookieSpinTimer <= 2) {
						CookieTurnAdditive++;
					}
					else if (CookieSpinTimer >= 58 && CookieSpinTimer <= 60) {
						CookieTurnAdditive--;
					}
				}
				else {
					if (CookieSpinTimer >= 0 && CookieSpinTimer <= 2) {
						CookieTurnAdditive--;
					}
					else if (CookieSpinTimer >= 58 && CookieSpinTimer <= 60) {
						CookieTurnAdditive++;
					}
				}
				if (CookieUpReturn == false) {
					CookieUpTimer++;
				}
				else if (CookieUpReturn == true) {
					CookieUpTimer--;
				}

				if (CookieUpTimer > 30) {
					CookieUpReturn = true;
				}
				else if (CookieUpTimer < -30) {
					CookieUpReturn = false;
				}

				Main.NewText(CookieSpinTimer);

				float drawX = ((int)drawInfo.Position.X + drawPlayer.width / 2) - 20;
				float drawY = ((int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f) + 32;
				Vector2 origin = drawInfo.bodyVect;
				Vector2 position = new Vector2((float)(drawX + (CookieSpinTimer)), drawY + (CookieUpTimer / 8)) + drawPlayer.bodyPosition - Main.screenPosition;
				Rectangle frame = new(0, 0, 14, 14);
				float rotation = drawPlayer.bodyRotation;
				SpriteEffects spriteEffects = drawInfo.playerEffect;

				float scale = 1f;

				if (CookieReturn == true) {
					scale = 0f;
				}

				DrawData drawData = new(texture, position, frame, drawInfo.colorArmorBody, rotation, origin, scale, spriteEffects, 0);
				drawData.shader = drawInfo.cBody;
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}

	public class NeapoliniteCookieBehindDrawing : PlayerDrawLayer {

		public override Position GetDefaultPosition() {
			return PlayerDrawLayers.BeforeFirstVanillaLayer;
		}

		protected override void Draw(ref PlayerDrawSet drawInfo) {

			if (drawInfo.shadow != 0f || drawInfo.drawPlayer.dead) {
				return;
			}

			Player drawPlayer = drawInfo.drawPlayer;

			if (drawPlayer.HasBuff(ModContent.BuffType<Buffs.NeapoliniteBuffs.ChocolateChargeI>())) {
				Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Assets/NeapoliniteCookies");

				float drawX = ((int)drawInfo.Position.X + drawPlayer.width / 2) - 20;
				float drawY = ((int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f) + 32;
				Vector2 origin = drawInfo.bodyVect;
				Vector2 position = new Vector2((float)(drawX + (NeapoliniteCookieDrawing.CookieSpinTimer)), drawY + (NeapoliniteCookieDrawing.CookieUpTimer / 8)) + drawPlayer.bodyPosition - Main.screenPosition;
				Rectangle frame = new(0, 0, 14, 14);
				float rotation = drawPlayer.bodyRotation;
				SpriteEffects spriteEffects = drawInfo.playerEffect;
				float scale = 0f;

				if (NeapoliniteCookieDrawing.CookieReturn == true) {
					scale = 1f;
				}

				DrawData drawData = new(texture, position, frame, drawInfo.colorArmorBody, rotation, origin, scale, spriteEffects, 0);
				drawData.shader = drawInfo.cBody;
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}*/
}
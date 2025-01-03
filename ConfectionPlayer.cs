using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steamworks;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Buffs;
using TheConfectionRebirth.Buffs.NeapoliniteBuffs;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Items.Accessories;
using TheConfectionRebirth.Items.Placeable;
using TheConfectionRebirth.Items.Weapons;
using TheConfectionRebirth.Mounts;
using TheConfectionRebirth.NPCs;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth
{
	public class ConfectionPlayer : ModPlayer
	{
		public bool cookiestPet;
		public bool lightnana;
		public bool rollerCookiePet;
		public bool creamsandWitchPet;
		public bool meawzerPet;
		public bool dudlingPet;
		public bool toothfairyMinion;
		public bool meawzerMinion;
		public bool gastropodMinion;

		public bool SacchariteLashed;
		public bool candleFire;
		public int candleFlameDelay = 0;
		public Projectile DimensionalWarp;
		public Projectile BananawarpPeelWarp;

		public bool neapoliniteMelee;
		public int meleeVanilla;
		public bool neapoliniteRanger;
		public bool neapoliniteMage;
		public int mageStrawberry;
		public bool neapoliniteSummoner;
		public int summonerCone;

		public int vanillaValorDamage;
		public int vanillaTimer;

		public int strawberryManaHealed;
		public int strawberryTimer;
		public int strawberryStartingMana;
		public int strawberrySpawnStrawTimer;

		public int coneTimer;
		public int coneSummonID;
		public static bool hasSwirlBuff(Player player) => player.HasBuff(ModContent.BuffType<SwirlySwarmI>()) || player.HasBuff(ModContent.BuffType<SwirlySwarmII>()) || player.HasBuff(ModContent.BuffType<SwirlySwarmIII>()) || player.HasBuff(ModContent.BuffType<SwirlySwarmIV>()) || player.HasBuff(ModContent.BuffType<SwirlySwarmV>());

		public int neapolinitePowerLevel;

		public int bakersDozenHitCount = 0;

		public int sweetToothCounter = 0;
		public int gummyWormWhipCounter = 0;

		public float snickerDevCookieRot = 0f;

		public int rollerCycleTimer;
		/// <summary>
		/// Used by the Mimic Chest Spawning to know what NPC to spawn when leaving the chest
		/// </summary>
		public int mimicSpawnKeyType = 0;

		public override void ResetEffects()
		{
			cookiestPet = false;
			lightnana = false;
			rollerCookiePet = false;
			creamsandWitchPet = false;
			meawzerPet = false;
			dudlingPet = false;
			toothfairyMinion = false;
			gastropodMinion = false;
			meawzerMinion = false;

			if (!candleFire)
			{
				candleFlameDelay = 0;
			}
			SacchariteLashed = false;
			candleFire = false;
			
			if (!neapoliniteMelee)
			{
				vanillaValorDamage = 0;
				vanillaTimer = 0;
				meleeVanilla = -1;
			}
			if (vanillaTimer <= 0)
			{
				vanillaValorDamage = 0;
			}
			if (!neapoliniteMage)
			{
				strawberryManaHealed = 0;
				strawberryTimer = 0;
				strawberryStartingMana = 0;
				strawberrySpawnStrawTimer = 0;
				mageStrawberry = -1;
			}
			if (strawberryTimer <= 0)
			{
				strawberryManaHealed = 0;
			}
			if (!neapoliniteSummoner)
			{
				coneTimer = 0;
				summonerCone = -1;
			}
			if (!hasSwirlBuff(Player))
			{
				if (coneSummonID > -1)
				{
					Main.projectile[coneSummonID].Kill();
					coneSummonID = -1;
				}
			}
			neapoliniteMelee = false;
			neapoliniteRanger = false;
			neapoliniteMage = false;
			neapoliniteSummoner = false;
			neapolinitePowerLevel = 0;

			mimicSpawnKeyType = 0;
		}

		public override void UpdateDead()
		{
			SacchariteLashed = false;
			candleFire = false;
		}

		public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{
			if (SacchariteLashed)
			{
				if (Main.rand.NextBool(4))
				{
					int dust = Dust.NewDust(Player.Center + new Vector2(Main.rand.NextFloat(-(Player.width / 2), Player.width / 2), Main.rand.NextFloat(-(Player.height / 2), Player.height / 2)), 10, 10, ModContent.DustType<SacchariteDust>());
					drawInfo.DustCache.Add(dust);
				}
			}
			if (candleFire && candleFlameDelay <= 0)
			{
				if (Main.rand.Next(4) < 3)
				{
					Dust dust = Dust.NewDustDirect(new Vector2(Player.position.X - 2f, Player.position.Y - 2f), Player.width + 4, Player.height + 4, DustID.Torch, Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 100, default(Color), 3.5f);
					dust.noGravity = true;
					dust.velocity *= 1.8f;
					dust.velocity.Y -= 0.5f;
					if (Main.rand.NextBool(4))
					{
						dust.noGravity = false;
						dust.scale *= 0.5f;
					}
					drawInfo.DustCache.Add(dust.dustIndex);
				}
				Lighting.AddLight((int)(Player.position.X / 16f), (int)(Player.position.Y / 16f + 1f), 1f, 0.3f, 0.1f);
			}
		}

		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if (candleFire)
			{
				WeightedRandom<string> deathmessage = new();
				deathmessage.Add(Language.GetTextValue("Mods.TheConfectionRebirth.PlayerDeathReason.CandleFire.0", Player.name));
				deathmessage.Add(Language.GetTextValue("Mods.TheConfectionRebirth.PlayerDeathReason.CandleFire.1", Player.name));
				deathmessage.Add(Language.GetTextValue("Mods.TheConfectionRebirth.PlayerDeathReason.CandleFire.2", Player.name));
				deathmessage.Add(Language.GetTextValue("Mods.TheConfectionRebirth.PlayerDeathReason.CandleFire.3", Player.name));
				deathmessage.Add(Language.GetTextValue("Mods.TheConfectionRebirth.PlayerDeathReason.CandleFire.4", Player.name));
				deathmessage.Add(Language.GetTextValue("Mods.TheConfectionRebirth.PlayerDeathReason.CandleFire.5", Player.name));
				deathmessage.Add(Language.GetTextValue("Mods.TheConfectionRebirth.PlayerDeathReason.CandleFire.6", Player.name));
				damageSource = PlayerDeathReason.ByCustomReason(deathmessage);
				return true;
			}
			if (damageSource.SourceCustomReason == "DimensionSplit")
			{
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

		public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
		{
			bool corrupt = Player.ZoneCorrupt;	
			bool crimson = Player.ZoneCrimson;
			bool snow = Player.ZoneSnow;
			bool dungeon = Player.ZoneDungeon;
			bool desert = Player.ZoneDesert;
			bool confection = Player.InModBiome<ConfectionBiome>();

			if (!attempt.inHoney && !attempt.inLava)
			{
				if (attempt.rolledEnemySpawn > 0)
				{
					return;
				}
				if (attempt.crate) //crates
				{
					bool hardMode = Main.hardMode;
					if (!dungeon && !(Player.ZoneBeach || (Main.remixWorld && attempt.heightLevel == 1 && (double)attempt.Y >= Main.rockLayer && Main.rand.NextBool(2))) && !corrupt && !crimson && attempt.rare && confection)
					{
						attempt.rolledItemDrop = (hardMode ? ModContent.ItemType<ConfectionCrate>() : ModContent.ItemType<BananaSplitCrate>());
					}
					return;
				}
				if (!dungeon)
				{
					if (!corrupt && !crimson && confection) //normal fishing items
					{
						if (desert && Main.rand.NextBool(2))
						{
							if (attempt.uncommon && attempt.questFish == ItemID.ScarabFish)
							{
								attempt.rolledItemDrop = ItemID.ScarabFish;
							}
							else if (attempt.uncommon && attempt.questFish == ItemID.ScorpioFish)
							{
								attempt.rolledItemDrop = ItemID.ScorpioFish;
							}
							else if (attempt.uncommon)
							{
								attempt.rolledItemDrop = ItemID.Oyster;
							}
							else if (Main.rand.NextBool(3))
							{
								attempt.rolledItemDrop = ItemID.RockLobster;
							}
							else
							{
								attempt.rolledItemDrop = ItemID.Flounder;
							}
						}
						else if (attempt.legendary && Main.hardMode && snow && attempt.heightLevel == 3 && !Main.rand.NextBool(3))
						{
							attempt.rolledItemDrop = ItemID.ScalyTruffle;
						}
						else if (attempt.legendary && Main.hardMode && Main.rand.NextBool(2))
						{
							attempt.rolledItemDrop = ModContent.ItemType<GummyStaff>(); //Biome fishing weapon
						}
						//else if (attempt.legendary && Main.hardMode && !Main.rand.NextBool(3)) //Confection nolonger has a fishing painting
						//{
						//	attempt.rolledItemDrop = ItemID.LadyOfTheLake;
						//}
						else if (attempt.heightLevel > 1 && attempt.veryrare)
						{
							attempt.rolledItemDrop = ModContent.ItemType<SugarFish>();
						}
						else if (attempt.heightLevel > 1 && attempt.uncommon && attempt.questFish == ModContent.ItemType<SacchariteBatFish>())
						{
							attempt.rolledItemDrop = ModContent.ItemType<SacchariteBatFish>();
						}
						else if (attempt.heightLevel < 2 && attempt.uncommon && attempt.questFish == ModContent.ItemType<Sprinklefish>())
						{
							attempt.rolledItemDrop = ModContent.ItemType<Sprinklefish>();
						}
						else if (attempt.rare)
						{
							attempt.rolledItemDrop = ModContent.ItemType<Cakekite>();
						}
						else if (attempt.uncommon && attempt.questFish == ModContent.ItemType<CookieCutterShark>())
						{
							attempt.rolledItemDrop = ModContent.ItemType<CookieCutterShark>();
						}
						else if (attempt.uncommon)
						{
							attempt.rolledItemDrop = ModContent.ItemType<CookieCarp>();
						}
					}
				}
			}
		}

		public override void UpdateBadLifeRegen()
		{
			if (candleFire && candleFlameDelay <= 0)
			{
				if (Player.lifeRegen > 0)
				{
					Player.lifeRegen = 0;
				}
				Player.lifeRegenTime = 0f;
				Player.lifeRegen -= 8;
			}
		}

		public override void PostUpdate()
		{
			if (neapoliniteMelee)
			{
				int rank = Math.Min(vanillaValorDamage, 1750) / 350 - 1;
				if (rank != meleeVanilla)
				{
					vanillaTimer = 300;
				}
				meleeVanilla = rank;

				IncrimentNeapoliniteBuffPower(Player, meleeVanilla);
				if (vanillaTimer > 0)
					vanillaTimer--;
			}
			if (neapoliniteRanger)
			{
				int rank;
				float vel = Player.velocity.Length();
				if (vel >= 11f)
					rank = 4;
				else
				{
					float speed = vel / 2.2f;
					rank = (int)(speed - 1);
				}
				IncrimentNeapoliniteBuffPower(Player, rank, 1);
			}
			if (neapoliniteMage)
			{
				int rank = (int)(strawberryManaHealed / 25);
				IncrimentNeapoliniteBuffPower(Player, rank - 1, 2);
				if (strawberryTimer > 0)
					strawberryTimer--;

				if (Player.statMana != Player.statManaMax2)
				{
					if (Player.manaRegenDelay == 0)
					{
						if (strawberryManaHealed <= 0 && strawberryTimer <= 0)
						{
							strawberryTimer = 180;
							strawberryStartingMana = Player.statMana;
						}
						if (strawberryTimer > 0)
						{
							strawberryManaHealed += (Player.statMana - strawberryStartingMana);
						}
						strawberryStartingMana = Player.statMana;
					}
				}

				if (strawberrySpawnStrawTimer > 60)
				{
					strawberrySpawnStrawTimer = 0;

					for (int i = 0; i <= mageStrawberry; i++)
					{
						int damage = 1;
						if (Player.HeldItem.DamageType == DamageClass.Magic)
						{
							damage = Player.HeldItem.damage / 2;
						}
						Vector2 pointPoisition = Player.RotatedRelativePoint(Player.MountedCenter);
						float mouseX = (float)Main.mouseX + Main.screenPosition.X - pointPoisition.X;
						float mouseY = (float)Main.mouseY + Main.screenPosition.Y - pointPoisition.Y;
						float f = Main.rand.NextFloat() * ((float)Math.PI * 2f);
						float width = 20f;
						float height = 60f;
						Vector2 pos2 = pointPoisition + f.ToRotationVector2() * MathHelper.Lerp(width, height, Main.rand.NextFloat());
						for (int avaliablePos = 0; avaliablePos < 50; avaliablePos++)
						{
							pos2 = pointPoisition + f.ToRotationVector2() * MathHelper.Lerp(width, height, Main.rand.NextFloat());
							if (Collision.CanHit(pointPoisition, 0, 0, pos2 + (pos2 - pointPoisition).SafeNormalize(Vector2.UnitX) * 8f, 0, 0))
							{
								break;
							}
							f = Main.rand.NextFloat() * ((float)Math.PI * 2f);
						}
						Vector2 velc = Main.MouseWorld - pos2;
						Vector2 pos = Utils.SafeNormalize(new Vector2(mouseX, mouseY), Vector2.UnitY) * 14f;
						velc = velc.SafeNormalize(pos) * 14f;
						velc = Vector2.Lerp(velc, pos, 0.25f);
						Projectile.NewProjectile(new EntitySource_Misc(""), pos2, velc, ModContent.ProjectileType<StrawberryStrike>(), damage, Player.HeldItem.knockBack, Player.whoAmI);
					}
				}
			}
			if (neapoliniteSummoner)
			{
				int rank = summonerCone;
				IncrimentNeapoliniteBuffPower(Player, rank, 3);
				coneTimer++;
				if (summonerCone < 4)
				{
					if (coneTimer >= 60 * 8)
					{
						summonerCone++;
						coneTimer = 0;
					}
				}
				if (coneTimer > 60 * 8)
				{
					coneTimer = 60 * 8;
				}
				if (summonerCone < -1)
					summonerCone = -1;
			}
			if (hasSwirlBuff(Player))
			{
				List<int> validSummonIDs = new List<int>();
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile proj = Main.projectile[i];
					if (proj.active && proj.minion && proj.owner == Player.whoAmI)
					{
						validSummonIDs.Add(i);
					}
				}
				if (coneSummonID <= -1)
				{
					if (validSummonIDs.Count != 0)
					{
						int num;
						if (validSummonIDs.Count > 1)
							num = Main.rand.Next(0, validSummonIDs.Count);
						else
							num = 0;
						Projectile ghost = Main.projectile[validSummonIDs[num]];
						int ghostProjID = Projectile.NewProjectile(new EntitySource_Misc(""), Player.position, Vector2.Zero, ghost.type, ghost.damage, ghost.knockBack, Player.whoAmI);
						Projectile ghostProj = Main.projectile[ghostProjID];
						ghostProj.minionSlots = 0;
						ghostProj.minion = false;
						ghostProj.minionPos = validSummonIDs.Count;
						coneSummonID = ghostProjID;
					}
					else
					{
						Player.GetDamage(DamageClass.Summon) += 0.1f;
					}
				}
				else
				{
					Projectile proj = Main.projectile[coneSummonID];
					if (!proj.active)
					{
						coneSummonID = -1;
					}
					else
					{
						proj.minionPos = validSummonIDs.Count;
					}
				}
			}
			if (neapolinitePowerLevel > 0)
			{
				for (int j = 0; j < neapolinitePowerLevel; j++)
				{
					bool alreadyExists = false;
					for (int i = 0; i < Main.maxProjectiles; i++)
					{
						Projectile proj = Main.projectile[i];
						if (proj.active && proj.type == ModContent.ProjectileType<NeapoliniteCookies>() && proj.ai[0] == j && proj.owner == Player.whoAmI)
						{
							alreadyExists = true;
						}
					}
					if (!alreadyExists)
					{
						Projectile.NewProjectile(new EntitySource_Misc(""), Player.Center, Vector2.Zero, ModContent.ProjectileType<NeapoliniteCookies>(), 0, 0, Player.whoAmI, j);
					}
				}
			}
			if (candleFire && Collision.WetCollision(Player.position, Player.width, Player.height))
			{
				Player.ClearBuff(ModContent.BuffType<HumanCandle>());
			}
			if (candleFire && Main.rand.NextBool(200))
			{
				candleFlameDelay = Main.rand.Next(40, 100);
			}
			if (candleFlameDelay > 0)
			{
				candleFlameDelay--;
			}
		}

		public override void PreUpdateMovement()
		{
			Player player = Player;
			if (player.wingsLogic == ModContent.GetInstance<WildAiryBlue>().Item.wingSlot && player.TryingToHoverDown && player.controlJump && player.wingTime > 0f && !player.merman)
			{
				float num82 = 0.9f;
				player.velocity.Y *= num82;
				if (player.velocity.Y > -2f && player.velocity.Y < 1f)
				{
					player.velocity.Y = 1E-05f;
				}
			}
		}

		public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (neapoliniteMelee)
			{
				if (item.CountsAsClass(DamageClass.Melee))
					OnHitValor(hit.Damage);
			}
		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (neapoliniteMelee)
			{
				if (proj.CountsAsClass(DamageClass.Melee))
					OnHitValor(hit.Damage);
			}
		}

		public void OnHitValor(int damage)
		{
			if (neapoliniteMelee)
			{
				if (vanillaValorDamage <= 0)
				{
					vanillaTimer = 300;
				}
				if (vanillaTimer > 0)
				{
					vanillaValorDamage += damage;
				}
			}
		}

		public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
		{
			if (neapoliniteSummoner)
			{
				coneTimer = 0;
				summonerCone -= 2;
			}
		}

		public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
		{
			if (neapoliniteSummoner)
			{
				coneTimer = 0;
				summonerCone -= 2;
			}
		}

		public override void GetDyeTraderReward(List<int> rewardPool)
		{
			rewardPool.Add(ModContent.ItemType<Items.CandyCornDye>());
			rewardPool.Add(ModContent.ItemType<Items.FoaminWispDye>());
			rewardPool.Add(ModContent.ItemType<Items.GummyWispDye>());
			rewardPool.Add(ModContent.ItemType<Items.SwirllingChocolateDye>());
		}

		public static void IncrimentNeapoliniteBuffPower(Player player, int Power, int BuffType = 0)
		{
			if (Power < 0 || Power > 4)
			{
				return; 
			}

			int time = 300;
			int type = 0;
			int[][] validTypes = new int[4][] {
				new int[5] { ModContent.BuffType<VanillaValorI>(), ModContent.BuffType<VanillaValorII>(), ModContent.BuffType<VanillaValorIII>(), ModContent.BuffType<VanillaValorIV>(), ModContent.BuffType<VanillaValorV>() },
				new int[5] { ModContent.BuffType<ChocolateChargeI>(), ModContent.BuffType<ChocolateChargeII>(), ModContent.BuffType<ChocolateChargeIII>(), ModContent.BuffType<ChocolateChargeIV>(), ModContent.BuffType<ChocolateChargeV>() },
				new int[5] { ModContent.BuffType<StrawberryStrikeI>(), ModContent.BuffType<StrawberryStrikeII>(), ModContent.BuffType<StrawberryStrikeIII>(), ModContent.BuffType<StrawberryStrikeIV>(), ModContent.BuffType<StrawberryStrikeV>() },
				new int[5] { ModContent.BuffType<SwirlySwarmI>(), ModContent.BuffType<SwirlySwarmII>(), ModContent.BuffType<SwirlySwarmIII>(), ModContent.BuffType<SwirlySwarmIV>(), ModContent.BuffType<SwirlySwarmV>() }
			};
			type = validTypes[BuffType][Power];

			for (int j = 0; j < Player.MaxBuffs; j++)
			{
				for (int k = 0; k < 5; k++)
				{
					if (player.buffType[j] == validTypes[BuffType][k])
					{
						if (k > Power)
						{
							return;
						}
					}
				}
			}

			if (type != 0)
			{
				if (Power > 0)
				{
					for (int i = 0; i < Power; i++)
					{
						if (player.HasBuff(validTypes[BuffType][i]))
							player.ClearBuff(validTypes[BuffType][i]);
					}
				}
				player.AddBuff(type, time);
			}
		}

		public static int NeapoliniteHelmetNumber(PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			int HelmetType = 0;
			if (drawPlayer.armor[10].type == ModContent.ItemType<Items.Armor.NeapoliniteSet.NeapoliniteHat>())
			{
				HelmetType = 4;
			}
			else if (drawPlayer.armor[10].type == ModContent.ItemType<Items.Armor.NeapoliniteSet.NeapoliniteHeadgear>())
			{
				HelmetType = 3;
			}
			else if (drawPlayer.armor[10].type == ModContent.ItemType<Items.Armor.NeapoliniteSet.NeapoliniteHelmet>())
			{
				HelmetType = 2;
			}
			else if (drawPlayer.armor[10].type == ModContent.ItemType<Items.Armor.NeapoliniteSet.NeapoliniteMask>())
			{
				HelmetType = 1;
			}
			else if (drawPlayer.armor[0].type == ModContent.ItemType<Items.Armor.NeapoliniteSet.NeapoliniteHat>())
			{
				HelmetType = 4;
			}
			else if (drawPlayer.armor[0].type == ModContent.ItemType<Items.Armor.NeapoliniteSet.NeapoliniteHeadgear>())
			{
				HelmetType = 3;
			}
			else if (drawPlayer.armor[0].type == ModContent.ItemType<Items.Armor.NeapoliniteSet.NeapoliniteHelmet>())
			{
				HelmetType = 2;
			}
			else if (drawPlayer.armor[0].type == ModContent.ItemType<Items.Armor.NeapoliniteSet.NeapoliniteMask>())
			{
				HelmetType = 1;
			}
			return HelmetType;
		}
	}
}

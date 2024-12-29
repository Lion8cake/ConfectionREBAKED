using System;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.GameContent.ItemDropRules.Chains;
using static Terraria.GameContent.ItemDropRules.Conditions;
using TheConfectionRebirth.ModSupport;
using TheConfectionRebirth.Items.Placeable;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Buffs.NeapoliniteBuffs;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Personalities;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Buffs;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.NPCs
{
	public class ConfectionGlobalNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;

		public bool SacchariteLashed;
		public bool candleFire;
		public int candleFlameDelay = 0;
		public bool sacchariteAmmoDebuff;

		public override void ResetEffects(NPC npc)
		{
			if (!candleFire)
			{
				candleFlameDelay = 0;
			}
			SacchariteLashed = false;
			candleFire = false;
			sacchariteAmmoDebuff = false;

			if (npc.noTileCollide)
			{
				if (candleFire && npc.boss && Main.netMode != NetmodeID.MultiplayerClient && Collision.WetCollision(npc.position, npc.width, npc.height))
				{
					for (int k = 0; k < NPC.maxBuffs; k++)
					{
						if (npc.buffType[k] == ModContent.BuffType<HumanCandle>())
						{
							npc.DelBuff(k);
						}
					}
				}
			}
			else
			{
				if (candleFire && Main.netMode != NetmodeID.MultiplayerClient && Collision.WetCollision(npc.position, npc.width, npc.height))
				{
					for (int k = 0; k < NPC.maxBuffs; k++)
					{
						if (npc.buffType[k] == ModContent.BuffType<HumanCandle>())
						{
							npc.DelBuff(k);
						}
					}
				}
			}
		}

		public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
		{
			if (SacchariteLashed)
			{
				modifiers.Defense -= 4;
			}
		}

		public override void DrawEffects(NPC npc, ref Color drawColor)
		{
			if (SacchariteLashed)
			{
				if (Main.rand.NextBool(4))
				{
					Dust.NewDust(npc.Center + new Vector2(Main.rand.NextFloat(-(npc.width / 2), npc.width / 2), Main.rand.NextFloat(-(npc.height / 2), npc.height / 2)), 10, 10, ModContent.DustType<SacchariteDust>());
				}
			}
			if (candleFire && candleFlameDelay <= 0)
			{
				if (Main.rand.Next(4) < 3)
				{
					Dust dust = Dust.NewDustDirect(new Vector2(npc.position.X - 2f, npc.position.Y - 2f), npc.width + 4, npc.height + 4, DustID.Torch, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 3.5f);
					dust.noGravity = true;
					dust.velocity *= 1.8f;
					dust.velocity.Y -= 0.5f;
					if (Main.rand.NextBool(4))
					{
						dust.noGravity = false;
						dust.scale *= 0.5f;
					}
				}
				Lighting.AddLight((int)(npc.position.X / 16f), (int)(npc.position.Y / 16f + 1f), 1f, 0.3f, 0.1f);
			}
		}

		public override void UpdateLifeRegen(NPC npc, ref int damage)
		{
			if (candleFire && candleFlameDelay <= 0)
			{
				if (npc.lifeRegen > 0)
				{
					npc.lifeRegen = 0;
				}
				npc.lifeRegen -= 8;
			}
			if (npc.oiled && candleFire && candleFlameDelay <= 0)
			{
				if (npc.lifeRegen > 0)
				{
					npc.lifeRegen = 0;
				}
				npc.lifeRegen -= 50;
				if (damage < 10)
				{
					damage = 10;
				}
			}
			if (sacchariteAmmoDebuff)
			{
				if (npc.lifeRegen > 0)
				{
					npc.lifeRegen = 0;
				}

				int count = 0;
				for (int i = 0; i < 1000; i++)
				{
					Projectile projectile = Main.projectile[i];
					if (projectile.active && (projectile.type == ModContent.ProjectileType<SacchariteArrow>() || projectile.type == ModContent.ProjectileType<SacchariteBullet>()) && projectile.ai[1] == npc.whoAmI)
					{
						count++;
					}
				}

				float count2 = count * 0.2f;
				npc.lifeRegen -= (int)(count2 * 4 * 20);
				if (damage < count2 * 20)
				{
					damage = (int)(count2 * 20);
				}
			}
		}

		public override bool PreAI(NPC npc)
		{
			if (candleFire && Main.rand.NextBool(200))
			{
				candleFlameDelay = Main.rand.Next(40, 100);
			}
			if (candleFlameDelay > 0)
			{
				candleFlameDelay--;
			}
			return true;
		}

		public override void SetStaticDefaults()
		{
			var nurseHappiness = NPCHappiness.Get(NPCID.Nurse);
			var wizardHappiness = NPCHappiness.Get(NPCID.Wizard);
			var partygirlHappiness = NPCHappiness.Get(NPCID.PartyGirl);
			var tavernkeepHappiness = NPCHappiness.Get(NPCID.DD2Bartender);

			var clothierHappiness = NPCHappiness.Get(NPCID.Clothier);
			var witchdoctorHappiness = NPCHappiness.Get(NPCID.WitchDoctor);
			var taxcollectorHappiness = NPCHappiness.Get(NPCID.TaxCollector);

			nurseHappiness.SetBiomeAffection<ConfectionBiome>(AffectionLevel.Like);
			wizardHappiness.SetBiomeAffection<ConfectionBiome>(AffectionLevel.Like);
			partygirlHappiness.SetBiomeAffection<ConfectionBiome>(AffectionLevel.Like);
			tavernkeepHappiness.SetBiomeAffection<ConfectionBiome>(AffectionLevel.Like);

			clothierHappiness.SetBiomeAffection<ConfectionBiome>(AffectionLevel.Dislike);
			witchdoctorHappiness.SetBiomeAffection<ConfectionBiome>(AffectionLevel.Dislike);
			taxcollectorHappiness.SetBiomeAffection<ConfectionBiome>(AffectionLevel.Dislike);
		}

		#region Soul drop conditions
	public class SoulOfDelight : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (Conditions.SoulOfWhateverConditionCanDrop(info))
				{
					return info.player.InModBiome(ModContent.GetInstance<ConfectionBiome>());
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				return false;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}

		public class SoulOfNightCorruption : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (Conditions.SoulOfWhateverConditionCanDrop(info))
				{
					return info.player.ZoneCorrupt;
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				return false;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}

		public class SoulOfSpite : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (Conditions.SoulOfWhateverConditionCanDrop(info))
				{
					return info.player.ZoneCrimson;
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				return false;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}
		#endregion

		#region ConfectionDropRule
		public class ConfectionDropRule : IItemDropRuleCondition
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return ConfectionWorldGeneration.confectionorHallow;
			}

			public bool CanShowItemDropInUI()
			{
				return false;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}
		#endregion

		#region HallowDropRule
		public class HallowDropRule : IItemDropRuleCondition
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return !ConfectionWorldGeneration.confectionorHallow;
			}

			public bool CanShowItemDropInUI()
			{
				return false;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}

		#endregion

		#region 50/50
		public class oneInTwo : IItemDropRuleCondition
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return Main.rand.NextBool(2);
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}
		#endregion

		#region notexpertordrunk
		public class NotDrunkandExpert : IItemDropRuleCondition
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return (!Main.drunkWorld || !ConfectionModCalling.FargoBoBW) && !Main.expertMode;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}
		#endregion

		#region drunkactive
		public class DrunkWorldIsActive : IItemDropRuleCondition
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				ConfectionModCalling.UpdateFargoBoBW();
				return (Main.drunkWorld || ConfectionModCalling.FargoBoBW);
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}
		#endregion

		#region NotDrunkActive
		public class DrunkWorldIsNotActive : IItemDropRuleCondition
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				ConfectionModCalling.UpdateFargoBoBW();
				return (!Main.drunkWorld || !ConfectionModCalling.FargoBoBW);
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}
		#endregion

		public override void ModifyGlobalLoot(GlobalLoot globalLoot)
		{
			globalLoot.Add(ItemDropRule.ByCondition(new SoulOfDelight(), ModContent.ItemType<SoulofDelight>(), 5, 1, 1));
			globalLoot.Add(ItemDropRule.ByCondition(new SoulOfSpite(), ModContent.ItemType<SoulofSpite>(), 5, 1, 1));
			globalLoot.Add(ItemDropRule.ByCondition(new SoulOfNightCorruption(), ItemID.SoulofNight, 5, 1, 1));

			globalLoot.RemoveWhere(
				rule => rule is ItemDropWithConditionRule drop
					&& drop.itemId == ItemID.SoulofNight
					&& drop.condition is Conditions.SoulOfNight
			);
		}

		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			static bool TwinsDrops(DropAttemptInfo info)
			{
				NPC npc = info.npc;
				if (npc is null)
				{
					return false;
				}
				if (npc.type == NPCID.Retinazer)
				{
					return !NPC.AnyNPCs(NPCID.Spazmatism);
				}
				else if (npc.type == NPCID.Spazmatism)
				{
					return !NPC.AnyNPCs(NPCID.Retinazer);
				}
				return false;
			}
			if (npc.type == NPCID.Retinazer || npc.type == NPCID.Spazmatism)
			{
				try
				{
					IItemDropRule obj9 = npcLoot.Get(false).Find(delegate (IItemDropRule rule) {
						LeadingConditionRule val19 = (LeadingConditionRule)(object)((rule is LeadingConditionRule) ? rule : null);
						return val19 != null && val19.condition is MissingTwin;
					});
					LeadingConditionRule LCR_LTS = (LeadingConditionRule)(object)((obj9 is LeadingConditionRule) ? obj9 : null);
					if (LCR_LTS != null)
					{
						IItemDropRule ruleToChain2 = LCR_LTS.ChainedRules.Find(delegate (IItemDropRuleChainAttempt chainAttempt) {
							TryIfSucceeded val17 = (TryIfSucceeded)(object)((chainAttempt is TryIfSucceeded) ? chainAttempt : null);
							if (val17 != null)
							{
								IItemDropRule ruleToChain7 = val17.RuleToChain;
								LeadingConditionRule val18 = (LeadingConditionRule)(object)((ruleToChain7 is LeadingConditionRule) ? ruleToChain7 : null);
								if (val18 != null)
								{
									return val18.condition is NotExpert;
								}
							}
							return false;
						}).RuleToChain;
						LeadingConditionRule LCR_NotExpert10 = (LeadingConditionRule)(object)((ruleToChain2 is LeadingConditionRule) ? ruleToChain2 : null);
						if (LCR_NotExpert10 != null)
						{
							LCR_NotExpert10.ChainedRules.RemoveAll(delegate (IItemDropRuleChainAttempt chainAttempt) {
								TryIfSucceeded val15 = (TryIfSucceeded)(object)((chainAttempt is TryIfSucceeded) ? chainAttempt : null);
								if (val15 != null)
								{
									IItemDropRule ruleToChain6 = val15.RuleToChain;
									CommonDrop val16 = (CommonDrop)(object)((ruleToChain6 is CommonDrop) ? ruleToChain6 : null);
									if (val16 != null)
									{
										return val16.itemId == ItemID.HallowedBar;
									}
								}
								return false;
							});
						}
					}
				}
				catch (ArgumentNullException)
				{
				}
			}

			if (npc.type == NPCID.Gastropod)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ShellBlock>(), 2, 15, 25));
			}

			if (npc.type == NPCID.BloodMummy)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CanofMeat>(), 10));
			}
			if (npc.type == NPCID.DesertGhoulCrimson)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CanofMeat>(), 15));
			}
			if (npc.type == NPCID.SandsharkCrimson)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CanofMeat>(), 25));
			}

			var entries = npcLoot.Get(false);
			if (npc.type == NPCID.WallofFlesh)
			{
				foreach (var entry in entries)
				{
					if (entry is ItemDropWithConditionRule rule && rule.itemId == ItemID.Pwnhammer)
					{
						npcLoot.Remove(rule);
						break;
					}
				}

				DrunkWorldIsNotActive NotDrunkWorld = new DrunkWorldIsNotActive();
				DrunkWorldIsActive DrunkWorld = new DrunkWorldIsActive();

				LeadingConditionRule ConfectionHammer = new LeadingConditionRule(new ConfectionDropRule());
				ConfectionHammer.OnSuccess(ItemDropRule.ByCondition(NotDrunkWorld, ModContent.ItemType<Items.Weapons.GrandSlammer>()));
				npcLoot.Add(ConfectionHammer);

				LeadingConditionRule HallowHammer = new LeadingConditionRule(new HallowDropRule());
				HallowHammer.OnSuccess(ItemDropRule.ByCondition(NotDrunkWorld, ItemID.Pwnhammer));
				npcLoot.Add(HallowHammer);

				LeadingConditionRule fiftyfifty = new LeadingConditionRule(new oneInTwo());
				fiftyfifty.OnSuccess(ItemDropRule.ByCondition(DrunkWorld, ModContent.ItemType<Items.Weapons.GrandSlammer>()));
				fiftyfifty.OnFailedConditions(ItemDropRule.ByCondition(DrunkWorld, ItemID.Pwnhammer));
				npcLoot.Add(fiftyfifty);

			}

			if (npc.type == NPCID.TheDestroyer || npc.type == NPCID.SkeletronPrime)
			{
				npcLoot.RemoveWhere(
				rule => rule is ItemDropWithConditionRule drop
					&& drop.itemId == ItemID.HallowedBar
				);
			}

			if (npc.type == NPCID.TheDestroyer || npc.type == NPCID.SkeletronPrime)
			{
				NotDrunkandExpert ExpertDrunkmode = new NotDrunkandExpert();
				NotExpert Expertmode = new NotExpert();

				LeadingConditionRule ConfectionCondition = new LeadingConditionRule(new ConfectionDropRule());
				ConfectionCondition.OnSuccess(ItemDropRule.ByCondition(ExpertDrunkmode, ModContent.ItemType<Items.Placeable.NeapoliniteOre>(), 1, 15 * 5, 30 * 5));
				npcLoot.Add(ConfectionCondition);

				LeadingConditionRule HallowCondition = new LeadingConditionRule(new HallowDropRule());
				HallowCondition.OnSuccess(ItemDropRule.ByCondition(ExpertDrunkmode, ModContent.ItemType<Items.Placeable.HallowedOre>(), 1, 15 * 5, 30 * 5));
				npcLoot.Add(HallowCondition);

				LeadingConditionRule DrunkCondition = new LeadingConditionRule(new DrunkWorldIsActive());
				DrunkCondition.OnSuccess(ItemDropRule.ByCondition(Expertmode, ModContent.ItemType<Items.Placeable.HallowedOre>(), 1, 8 * 5, 15 * 5));
				ConfectionCondition.OnSuccess(ItemDropRule.ByCondition(Expertmode, ModContent.ItemType<Items.Placeable.NeapoliniteOre>(), 1, 8 * 5, 15 * 5));
				npcLoot.Add(DrunkCondition);
			}
			if (npc.type == NPCID.BloodMummy || npc.type == NPCID.DesertGhoulCrimson || npc.type == NPCID.SandsharkCrimson)
			{
				npcLoot.Remove(FindDarkShard(npcLoot));
			}
		}

		private static IItemDropRule FindDarkShard(NPCLoot loot)
		{
			foreach (IItemDropRule item in loot.Get(false))
			{
				CommonDrop c = (CommonDrop)(object)((item is CommonDrop) ? item : null);
				if (c != null && c.itemId == ItemID.DarkShard)
				{
					return (IItemDropRule)(object)c;
				}
			}
			return null;
		}

		public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
		{
			if (player.HasBuff(ModContent.BuffType<VanillaValorV>()) && !ConfectionIDs.Sets.IsEnemyVanillaCritImmune[npc.type])
			{
				modifiers.HideCombatText();
				modifiers.Defense *= 0;
			}
		}
		public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
		{
			if (Main.player[projectile.owner].HasBuff(ModContent.BuffType<VanillaValorV>()) && !ConfectionIDs.Sets.IsEnemyVanillaCritImmune[npc.type])
			{
				modifiers.HideCombatText();
				modifiers.Defense *= 0;
			}

			if (!projectile.npcProj && !projectile.trap && projectile.IsMinionOrSentryRelated)
			{
				float projTagMultiplier = ProjectileID.Sets.SummonTagDamageMultiplier[projectile.type];
				if (npc.HasBuff<SacchariteLashTagDamage>())
				{
					modifiers.FlatBonusDamage += 10 * projTagMultiplier;
				}
				if (npc.HasBuff<GummyWormWhipTagDamage>())
				{
					modifiers.FlatBonusDamage += 8 * projTagMultiplier;
				}
			}
		}

		public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
		{
			if (!ConfectionIDs.Sets.IsEnemyVanillaCritImmune[npc.type] && player.HasBuff(ModContent.BuffType<VanillaValorV>()))
			{
				if (hit.Crit)
				{
					Color color3 = new(230, 196, 125);
					CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), color3, hit.Damage, true);
				}
				else
				{
					HitText(npc, hit);
				}
			}
		}

		public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
		{
			if (!ConfectionIDs.Sets.IsEnemyVanillaCritImmune[npc.type] && Main.player[projectile.owner].HasBuff(ModContent.BuffType<VanillaValorV>()))
			{
				if (hit.Crit)
				{
					Color color2 = new(230, 196, 125);
					CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), color2, hit.Damage, true);
				}
				else
				{
					HitText(npc, hit);
				}
			}
		}

		private static void HitText(NPC npc, NPC.HitInfo hit)
		{
			double num = hit.Damage;
			bool crit = hit.Crit;
			if (hit.InstantKill)
			{
				num = ((npc.realLife > 0) ? Main.npc[npc.realLife].life : npc.life);
			}
			if (!hit.InstantKill && npc.lifeMax > 1 && !npc.HideStrikeDamage)
			{
				if (npc.friendly)
				{
					Color color = (crit ? CombatText.DamagedFriendlyCrit : CombatText.DamagedFriendly);
					CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), color, (int)num, crit);
				}
				else
				{
					Color color2 = (crit ? CombatText.DamagedHostileCrit : CombatText.DamagedHostile);
					CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), color2, (int)num, crit);
				}
			}
		}

		public static Condition InConfection = new Condition("Mods.TheConfectionRebirth.InConfection", () => Main.LocalPlayer.InModBiome<ConfectionBiome>());
		public static Condition NotInConfection = new Condition("Mods.TheConfectionRebirth.NotInConfection", () => !Main.LocalPlayer.InModBiome<ConfectionBiome>());

		public static Condition confectionworld = new Condition("Mods.TheConfectionRebirth.TheConfection", () => ConfectionWorldGeneration.confectionorHallow);

		public static Condition hallowworld = new Condition("Mods.TheConfectionRebirth.TheHallow", () => !ConfectionWorldGeneration.confectionorHallow);

		public static Condition paintingNotCondition = new Condition("Mods.TheConfectionRebirth.NotInSeveralBiomes", () => (!Main.LocalPlayer.ZoneJungle && !Main.LocalPlayer.ZoneDesert && !Main.LocalPlayer.ZoneSnow && !Main.bloodMoon));

		public override void ModifyShop(NPCShop shop)
		{
			if (shop.NpcType == NPCID.Dryad)
			{
				shop.InsertAfter(ItemID.HallowedGrassEcho, ModContent.ItemType<CreamgrassWall>(), Condition.Hardmode, confectionworld);
				shop.InsertAfter(ItemID.HallowedSeeds, ModContent.ItemType<CreamBeans>(), Condition.Hardmode, confectionworld, Condition.NotInGraveyard);
				shop.InsertAfter(ItemID.HallowedSeeds, ModContent.ItemType<CreamBeans>(), Condition.Hardmode, hallowworld, Condition.InGraveyard);
				shop.InsertAfter(ItemID.HallowedSeeds, ItemID.HallowedSeeds, Condition.Hardmode, confectionworld, Condition.InGraveyard);

				shop.InsertAfter(ItemID.PottedHallowCedar, ModContent.ItemType<PottedConfectionCedar>(), Condition.Hardmode, confectionworld, Condition.MoonPhaseFull);
				shop.InsertAfter(ItemID.PottedHallowCedar, ModContent.ItemType<PottedConfectionCedar>(), Condition.Hardmode, confectionworld, Condition.MoonPhaseWaningGibbous);
				shop.InsertAfter(ItemID.PottedHallowTree, ModContent.ItemType<PottedConfectionTree>(), Condition.Hardmode, confectionworld, Condition.MoonPhaseThirdQuarter);
				shop.InsertAfter(ItemID.PottedHallowTree, ModContent.ItemType<PottedConfectionTree>(), Condition.Hardmode, confectionworld, Condition.MoonPhaseWaningCrescent);
				shop.InsertAfter(ItemID.PottedHallowPalm, ModContent.ItemType<PottedConfectionPalm>(), Condition.Hardmode, confectionworld, Condition.MoonPhaseNew);
				shop.InsertAfter(ItemID.PottedHallowPalm, ModContent.ItemType<PottedConfectionPalm>(), Condition.Hardmode, confectionworld, Condition.MoonPhaseWaxingCrescent);
				shop.InsertAfter(ItemID.PottedHallowBamboo, ModContent.ItemType<PottedConfectionBamboo>(), Condition.Hardmode, confectionworld, Condition.MoonPhaseFirstQuarter);
				shop.InsertAfter(ItemID.PottedHallowBamboo, ModContent.ItemType<PottedConfectionBamboo>(), Condition.Hardmode, confectionworld, Condition.MoonPhaseWaxingGibbous);

				if (shop.TryGetEntry(ItemID.HallowedGrassEcho, out NPCShop.Entry entry))
				{
					entry.AddCondition(hallowworld);
				}
				if (shop.TryGetEntry(ItemID.HallowedSeeds, out NPCShop.Entry entry2))
				{
					entry2.AddCondition(hallowworld);
				}

				if (shop.TryGetEntry(ItemID.PottedHallowCedar, out NPCShop.Entry entry4))
				{
					entry4.AddCondition(hallowworld);
				}
				if (shop.TryGetEntry(ItemID.PottedHallowTree, out NPCShop.Entry entry5))
				{
					entry5.AddCondition(hallowworld);
				}
				if (shop.TryGetEntry(ItemID.PottedHallowPalm, out NPCShop.Entry entry6))
				{
					entry6.AddCondition(hallowworld);
				}
				if (shop.TryGetEntry(ItemID.PottedHallowBamboo, out NPCShop.Entry entry7))
				{
					entry7.AddCondition(hallowworld);
				}
			}
			if (shop.NpcType == NPCID.Steampunker)
			{
				shop.InsertAfter(ItemID.BlueSolution, ModContent.ItemType<Items.CreamSolution>(), Condition.Hardmode, InConfection, Condition.NotInGraveyard);
				shop.InsertAfter(ItemID.BlueSolution, ModContent.ItemType<Items.CreamSolution>(), Condition.Hardmode, Condition.InHallow, Condition.InGraveyard);
				shop.InsertAfter(ItemID.BlueSolution, ItemID.BlueSolution, Condition.Hardmode, InConfection, Condition.InGraveyard);
				if (shop.TryGetEntry(ItemID.GreenSolution, out NPCShop.Entry entry8))
				{
					entry8.AddCondition(NotInConfection);
				}
			}
			if (shop.NpcType == NPCID.Wizard)
			{
				shop.InsertAfter(ItemID.Bell, ModContent.ItemType<Items.Kazoo>(), Condition.Hardmode);
			}
			if (shop.NpcType == NPCID.BestiaryGirl)
			{
				shop.InsertAfter(ItemID.WorldGlobe, new Item(ModContent.ItemType<Items.HallowedGlobe>())
				{
					shopCustomPrice = Item.buyPrice(0, 3, 0, 0)
				}, Condition.DownedPlantera);
			}
			if (shop.NpcType == NPCID.Painter && shop.Name == "Decor")
			{
				shop.InsertAfter(ItemID.Purity, new Item(ModContent.ItemType<ConfectionPainting>())
				{
					shopCustomPrice = Item.buyPrice(0, 1)
				}, InConfection, paintingNotCondition);

				if (shop.TryGetEntry(ItemID.TheLandofDeceivingLooks, out NPCShop.Entry entry))
				{
					entry.AddCondition(NotInConfection);
				}
			}
		}
	}
}

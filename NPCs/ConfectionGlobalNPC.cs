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

namespace TheConfectionRebirth.NPCs
{
	public class ConfectionGlobalNPC : GlobalNPC
	{
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


		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			bool TwinsDrops(DropAttemptInfo info)
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

		public static Condition InConfection = new Condition("Mods.TheConfectionRebirth.InConfection", () => true/*Main.LocalPlayer.InModBiome<ConfectionBiome>()*/);
		public static Condition NotInConfection = new Condition("Mods.TheConfectionRebirth.NotInConfection", () => false/*!Main.LocalPlayer.InModBiome<ConfectionBiome>()*/);

		public static Condition confectionworld = new Condition("Mods.TheConfectionRebirth.TheConfection", () => ConfectionWorldGeneration.confectionorHallow);

		public static Condition hallowworld = new Condition("Mods.TheConfectionRebirth.TheHallow", () => !ConfectionWorldGeneration.confectionorHallow);

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
		}

	}
}

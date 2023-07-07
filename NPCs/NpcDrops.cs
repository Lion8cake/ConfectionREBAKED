using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Buffs.NeapoliniteBuffs;
using static TheConfectionRebirth.NPCs.BagDrops;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using System;
using Microsoft.Xna.Framework;
using static Terraria.GameContent.ItemDropRules.Chains;
using static Terraria.GameContent.ItemDropRules.Conditions;

namespace TheConfectionRebirth.NPCs
{
    public class NpcDrops : GlobalNPC
    {
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
                return "";
            }
        }


        public override void ModifyGlobalLoot(GlobalLoot globalLoot)
        {
            globalLoot.Add(ItemDropRule.ByCondition(new SoulOfDelight(), ModContent.ItemType<SoulofDelight>(), 5, 1, 1));
		}

		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
			bool TwinsDrops(DropAttemptInfo info)
			{
				NPC npc = info.npc;
				if (npc is null) {
					return false;
				}
				if (npc.type == NPCID.Retinazer) {
					return !NPC.AnyNPCs(NPCID.Spazmatism);
				}
				else if (npc.type == NPCID.Spazmatism) {
					return !NPC.AnyNPCs(NPCID.Retinazer);
				}
				return false;
			}
			if (npc.type == NPCID.Retinazer || npc.type == NPCID.Spazmatism) {
				try {
					IItemDropRule obj9 = npcLoot.Get(false).Find(delegate (IItemDropRule rule) {
						LeadingConditionRule val19 = (LeadingConditionRule)(object)((rule is LeadingConditionRule) ? rule : null);
						return val19 != null && val19.condition is MissingTwin;
					});
					LeadingConditionRule LCR_LTS = (LeadingConditionRule)(object)((obj9 is LeadingConditionRule) ? obj9 : null);
					if (LCR_LTS != null) {
						IItemDropRule ruleToChain2 = LCR_LTS.ChainedRules.Find(delegate (IItemDropRuleChainAttempt chainAttempt) {
							TryIfSucceeded val17 = (TryIfSucceeded)(object)((chainAttempt is TryIfSucceeded) ? chainAttempt : null);
							if (val17 != null) {
								IItemDropRule ruleToChain7 = val17.RuleToChain;
								LeadingConditionRule val18 = (LeadingConditionRule)(object)((ruleToChain7 is LeadingConditionRule) ? ruleToChain7 : null);
								if (val18 != null) {
									return val18.condition is NotExpert;
								}
							}
							return false;
						}).RuleToChain;
						LeadingConditionRule LCR_NotExpert10 = (LeadingConditionRule)(object)((ruleToChain2 is LeadingConditionRule) ? ruleToChain2 : null);
						if (LCR_NotExpert10 != null) {
							LCR_NotExpert10.ChainedRules.RemoveAll(delegate (IItemDropRuleChainAttempt chainAttempt) {
								TryIfSucceeded val15 = (TryIfSucceeded)(object)((chainAttempt is TryIfSucceeded) ? chainAttempt : null);
								if (val15 != null) {
									IItemDropRule ruleToChain6 = val15.RuleToChain;
									CommonDrop val16 = (CommonDrop)(object)((ruleToChain6 is CommonDrop) ? ruleToChain6 : null);
									if (val16 != null) {
										return val16.itemId == 1225;
									}
								}
								return false;
							});
						}
					}
				}
				catch (ArgumentNullException) {
				}
			}

			if (npc.type == NPCID.Gastropod) {
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ShellBlock>(), 2, 15, 25));
			}

			var entries = npcLoot.Get(false);
			if (npc.type == NPCID.WallofFlesh) {
				foreach (var entry in entries) {
					if (entry is ItemDropWithConditionRule rule && rule.itemId == ItemID.Pwnhammer) {
						npcLoot.Remove(rule);
						break;
					}
				}

				LeadingConditionRule Expertmode = new LeadingConditionRule(new Conditions.NotExpert());
				LeadingConditionRule ConfectionHammer = new LeadingConditionRule(new ConfectionDropRule());
				Expertmode.OnSuccess(ConfectionHammer);
				ConfectionHammer.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.GrandSlammer>()));
				npcLoot.Add(ConfectionHammer);

				LeadingConditionRule HallowHammer = new LeadingConditionRule(new HallowDropRule());
				Expertmode.OnSuccess(HallowHammer);
				HallowHammer.OnSuccess(ItemDropRule.Common(ItemID.Pwnhammer));
				npcLoot.Add(HallowHammer);
			}

			if (npc.type == NPCID.TheDestroyer || npc.type == NPCID.SkeletronPrime) {
				npcLoot.RemoveWhere(
				rule => rule is ItemDropWithConditionRule drop
					&& drop.itemId == ItemID.HallowedBar
				);
			}

			if (npc.type == NPCID.TheDestroyer || npc.type == NPCID.SkeletronPrime) {
				LeadingConditionRule Expertmode = new LeadingConditionRule(new Conditions.NotExpert());
				LeadingConditionRule ConfectionCondition = new LeadingConditionRule(new ConfectionDropRule());
				Expertmode.OnSuccess(ConfectionCondition);
				ConfectionCondition.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.NeapoliniteOre>(), 1, 15 * 5, 30 * 5));
				npcLoot.Add(ConfectionCondition);

				LeadingConditionRule HallowCondition = new LeadingConditionRule(new HallowDropRule());
				Expertmode.OnSuccess(HallowCondition);
				HallowCondition.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.HallowedOre>(), 1, 15 * 5, 30 * 5));
				npcLoot.Add(HallowCondition);
			}
		}

		//Vanilla Valor Critical stike ignore defence code below

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
			if (player.HasBuff(ModContent.BuffType<VanillaValorV>()) && npc.type != NPCID.DungeonGuardian) {
				modifiers.HideCombatText();
			}
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
			if (Main.player[projectile.owner].HasBuff(ModContent.BuffType<VanillaValorV>()) && npc.type != NPCID.DungeonGuardian) {
				modifiers.HideCombatText();
			}
		}
		public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone) {
			if (player.HasBuff(ModContent.BuffType<VanillaValorV>()) && hit.Crit && npc.type != NPCID.DungeonGuardian) {
				hit.Damage = hit.Damage + (int)(npc.defense * 0.5f);
				Color color3 = new(230, 196, 125);
				CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), color3, hit.Damage, true);
			}
			else if (player.HasBuff(ModContent.BuffType<VanillaValorV>()) && !hit.Crit) {
				HitText(npc, hit);
			}
		}

		public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) {
			if (Main.player[projectile.owner].HasBuff(ModContent.BuffType<VanillaValorV>()) && hit.Crit && npc.type != NPCID.DungeonGuardian) {
				hit.Damage = hit.Damage + (int)(npc.defense * 0.5f);
				Color color2 = new(230, 196, 125);
				CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), color2, hit.Damage, true);
			}
			else if (Main.player[projectile.owner].HasBuff(ModContent.BuffType<VanillaValorV>()) && !hit.Crit) {
				HitText(npc, hit);
			}
		}

		private static void HitText(NPC npc, NPC.HitInfo hit) {
			double num = hit.Damage;
			bool crit = hit.Crit;
			if (hit.InstantKill) {
				num = ((npc.realLife > 0) ? Main.npc[npc.realLife].life : npc.life);
			}
			if (!hit.InstantKill && npc.lifeMax > 1 && !npc.HideStrikeDamage) {
				if (npc.friendly) {
					Color color = (crit ? CombatText.DamagedFriendlyCrit : CombatText.DamagedFriendly);
					CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), color, (int)num, crit);
				}
				else {
					Color color2 = (crit ? CombatText.DamagedHostileCrit : CombatText.DamagedHostile);
					/*if (Main.netMode == NetmodeID.SinglePlayer) {
						color2 = (crit ? CombatText.OthersDamagedHostileCrit : CombatText.OthersDamagedHostile);
					}*/
					CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), color2, (int)num, crit);
				}
			}
		}
	}

	public class BagDrops : GlobalItem {

		#region ConfectionDropRule
		public class ConfectionDropRule : IItemDropRuleCondition {
			public bool CanDrop(DropAttemptInfo info) {
				return ConfectionWorldGeneration.confectionorHallow;
			}

			public bool CanShowItemDropInUI() {
				return false;
			}

			public string GetConditionDescription() {
				return "";
			}
		}

		#endregion

		#region HallowDropRule
		public class HallowDropRule : IItemDropRuleCondition{
			public bool CanDrop(DropAttemptInfo info) {
				return !ConfectionWorldGeneration.confectionorHallow;
			}

			public bool CanShowItemDropInUI() {
				return false;
			}

			public string GetConditionDescription() {
				return "";
			}
		}

		#endregion
		public override void ModifyItemLoot(Item item, ItemLoot itemLoot) {
			if (item.type == ItemID.WallOfFleshBossBag) {
				NPCLoader.blockLoot.Add(ItemID.Pwnhammer);
				itemLoot.Remove(FindHammer(itemLoot));

				LeadingConditionRule ConfectionHammer = new LeadingConditionRule(new ConfectionDropRule());
				ConfectionHammer.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.GrandSlammer>()));
				itemLoot.Add(ConfectionHammer);

				LeadingConditionRule HallowHammer = new LeadingConditionRule(new HallowDropRule());
				HallowHammer.OnSuccess(ItemDropRule.Common(ItemID.Pwnhammer));
				itemLoot.Add(HallowHammer);
			}
			if (item.type == ItemID.TwinsBossBag || item.type == ItemID.DestroyerBossBag || item.type == ItemID.SkeletronPrimeBossBag) {
				NPCLoader.blockLoot.Add(ItemID.HallowedBar);
				itemLoot.Remove(FindHallowedBars(itemLoot));

				LeadingConditionRule ConfectionCondition = new LeadingConditionRule(new ConfectionDropRule());
				ConfectionCondition.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.NeapoliniteOre>(), 1, 15 * 5, 30 * 5));
				itemLoot.Add(ConfectionCondition);
				
				LeadingConditionRule HallowCondition = new LeadingConditionRule(new HallowDropRule());
				HallowCondition.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.HallowedOre>(), 1, 15 * 5, 30 * 5));
				itemLoot.Add(HallowCondition);
			}
		}

		private static IItemDropRule FindHallowedBars(ItemLoot loot) {
			foreach (IItemDropRule item in loot.Get(false)) {
				CommonDrop c = (CommonDrop)(object)((item is CommonDrop) ? item : null);
				if (c != null && c.itemId == 1225) {
					return (IItemDropRule)(object)c;
				}
			}
			return null;
		}

		private static IItemDropRule FindHammer(ItemLoot loot) {
			foreach (IItemDropRule item in loot.Get(false)) {
				CommonDrop c = (CommonDrop)(object)((item is CommonDrop) ? item : null);
				if (c != null && c.itemId == ItemID.Pwnhammer) {
					return (IItemDropRule)(object)c;
				}
			}
			return null;
		}
	}
}

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
			/*if (npc.type == NPCID.Spazmatism || npc.type == NPCID.Retinazer) { //npc loot it the bane of my existance
				npcLoot.RemoveWhere(rule => {
					// Loop through the topmost rules
					foreach (var rule2 in npcLoot.Get()) {
						// If the rule has any children (e.g. from OnSuccess and OnFailed), check them
						for (int i = 0; i < rule2.ChainedRules.Count; i++) {
							var chained = rule2.ChainedRules[i].RuleToChain;
							// ItemDropRule.Common returns a CommonDrop
							if (chained is CommonDrop common && common.itemId == ItemID.HallowedBar) {
								rule2.ChainedRules.RemoveAt(i);
								return true;
							}
						}
					}
					return false;
				});//shout out to aquaAqurian for the help on this, even if it doesn't work
			}*/

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

        //Putting this here because the confection doesn't need another global npc file cloging up space 
        //This is the code that bypasses the defence of every npc when the player has hit the max neapolinite set buff
        //most of the code goes to Ouroel and foxXD_ for helping make this
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (player.HasBuff(ModContent.BuffType<VanillaValorV>()) && npc.type != NPCID.DungeonGuardian)
            {
                modifiers.FinalDamage = modifiers.FinalDamage + (int)(npc.defense * 0.5f);
            }
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (Main.player[projectile.owner].HasBuff(ModContent.BuffType<VanillaValorV>()) && npc.type != NPCID.DungeonGuardian)
            {
				modifiers.FinalDamage = modifiers.FinalDamage + (int)(npc.defense * 0.5f);  
            }
            if (Main.player[projectile.owner].HasBuff(ModContent.BuffType<VanillaValorV>()) && npc.type == NPCID.DungeonGuardian)
            {
                modifiers.FinalDamage = modifiers.FinalDamage + (int)(npc.defense * 0.5f) + 3;
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

				LeadingConditionRule ConfectionHammer = new LeadingConditionRule(new ConfectionDropRule());
				ConfectionHammer.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.GrandSlammer>()));
				itemLoot.Add(ConfectionHammer);

				LeadingConditionRule HallowHammer = new LeadingConditionRule(new HallowDropRule());
				HallowHammer.OnSuccess(ItemDropRule.Common(ItemID.Pwnhammer));
				itemLoot.Add(HallowHammer);
			}
			if (item.type == ItemID.TwinsBossBag || item.type == ItemID.DestroyerBossBag || item.type == ItemID.SkeletronPrimeBossBag) {
				NPCLoader.blockLoot.Add(ItemID.HallowedBar);

				LeadingConditionRule ConfectionCondition = new LeadingConditionRule(new ConfectionDropRule());
				ConfectionCondition.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.NeapoliniteOre>(), 1, 15 * 5, 30 * 5));
				itemLoot.Add(ConfectionCondition);

				LeadingConditionRule HallowCondition = new LeadingConditionRule(new HallowDropRule());
				HallowCondition.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.HallowedOre>(), 1, 15 * 5, 30 * 5));
				itemLoot.Add(HallowCondition);
			}
		}
	}
}

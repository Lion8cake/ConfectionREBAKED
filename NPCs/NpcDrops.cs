using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items;

namespace TheConfectionRebirth.NPCs
{
    public class NpcDrops : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.Gastropod)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ShellBlock>(), 5, 15, 25));
            }
            if (npc.lifeMax >= 1)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new SoulOfDelight(), ModContent.ItemType<SoulofDelight>(), 5, 1, 1));
            }
            if (npc.lifeMax >= 1)
            {
                npcLoot.RemoveWhere(
                    rule => rule is ItemDropWithConditionRule drop
                        && drop.itemId == ItemID.SoulofNight
                        && drop.condition is Conditions.SoulOfNight
                );

                //npcLoot.Add(ItemDropRule.ByCondition(new SoulOfSpite(), ModContent.ItemType<SoulofSpite>(), 5, 1, 1));
                //npcLoot.Add(ItemDropRule.ByCondition(new SoulOfNightCorruption(), ItemID.SoulofNight, 5, 1, 1));
            }
        }
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
        return "Lion forgot to write something here, im sorry for the lack of brain cells he has";
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
        return "Lion forgot to write something here, im sorry for the lack of brain cells he has";
    }
}

public class SoulOfDelight : IItemDropRuleCondition, IProvideItemConditionDescription
{
    public bool CanDrop(DropAttemptInfo info)
    {
        if (Conditions.SoulOfWhateverConditionCanDrop(info))
        {
            return info.player.InModBiome(ModContent.GetInstance<ConfectionBiomeSurface>());
        }
        return false;
    }

    public bool CanShowItemDropInUI()
    {
        return false;
    }

    public string GetConditionDescription()
    {
        return "Lion forgot to write something here, im sorry for the lack of brain cells he has";
    }
}

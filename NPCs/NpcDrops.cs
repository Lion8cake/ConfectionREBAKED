using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Buffs.NeapoliniteBuffs;

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
                npcLoot.Add(ItemDropRule.ByCondition(new SoulOfSpite(), ModContent.ItemType<SoulofSpite>(), 5, 1, 1));
                npcLoot.Add(ItemDropRule.ByCondition(new SoulOfNightCorruption(), ItemID.SoulofNight, 5, 1, 1));
            }
        }

        public override void ModifyGlobalLoot(GlobalLoot globalLoot)
        {
            globalLoot.RemoveWhere(
                rule => rule is ItemDropWithConditionRule drop
                    && drop.itemId == ItemID.SoulofNight
                    && drop.condition is Conditions.SoulOfNight
            );
        }

        //Putting this here because the confection doesn't need another global npc file cloging up space 
        //This is the code that bypasses the defence of every npc when the player has hit the max neapolinite set buff
        //most of the code goes to Ouroel and foxXD_ for helping make this
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (player.HasBuff(ModContent.BuffType<VanillaValorV>()) && npc.type != NPCID.DungeonGuardian)
            {
                damage = damage + (int)(npc.defense * 0.5f);
            }
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Main.player[projectile.owner].HasBuff(ModContent.BuffType<VanillaValorV>()) && npc.type != NPCID.DungeonGuardian)
            {
                damage = damage + (int)(npc.defense * 0.5f);  
            }
            if (Main.player[projectile.owner].HasBuff(ModContent.BuffType<VanillaValorV>()) && npc.type == NPCID.DungeonGuardian)
            {
                damage = (int)(npc.defense * 0.5f) + 3;
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

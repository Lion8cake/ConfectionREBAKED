using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;

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
        }
    }
}

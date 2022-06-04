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
            if (Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneRockLayerHeight && Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].InModBiome(ModContent.GetInstance<ConfectionBiomeBiome>()))
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SoulofDelight>(), 5, 1, 1));
            }
        }
    }
}

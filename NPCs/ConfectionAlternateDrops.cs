using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;
using TheConfectionRebirth.Items.Weapons;

namespace TheConfectionRebirth.NPCs
{
    /*public class ConfectionAlternateDrops : GlobalNPC
    {
        public override bool PreKill(NPC npc)
        {
            if (npc.type == NPCID.SkeletronPrime || npc.type == NPCID.TheDestroyer || npc.type == NPCID.Retinazer || npc.type == NPCID.Spazmatism)
            {
                NPCLoader.blockLoot.Add(ItemID.HallowedBar);
            }
			if (npc.type == NPCID.WallofFlesh)
            {
                NPCLoader.blockLoot.Add(ItemID.Pwnhammer);
            }
            return base.PreKill(npc);
        }

        public override void OnKill(NPC npc)
        {
            if (!Main.expertMode)
            {
                if (ConfectionWorldGeneration.confectionorHallow)
                {
                    if (npc.type == NPCID.SkeletronPrime || npc.type == NPCID.TheDestroyer)
                    {
                        Item.NewItem(npc.getRect(), ModContent.ItemType<NeapoliniteOre>(), Main.rand.Next(75, 150));
                    }
                    int TS = NPC.CountNPCS(NPCID.Spazmatism);
                    int TR = NPC.CountNPCS(NPCID.Retinazer);
                    if (TS == 0 && TR == 1)
                    {
                        Item.NewItem(npc.getRect(), ModContent.ItemType<NeapoliniteOre>(), Main.rand.Next(75, 150));
                    }
                    if (TS == 1 && TR == 0)
                    {
                        Item.NewItem(npc.getRect(), ModContent.ItemType<NeapoliniteOre>(), Main.rand.Next(75, 150));
                    }
				    if (npc.type == NPCID.WallofFlesh)
                    {
                        Item.NewItem(npc.getRect(), ModContent.ItemType<GrandSlammer>(), Main.rand.Next(1, 1));
                    }
                }
                else
                {
                    if (npc.type == NPCID.SkeletronPrime || npc.type == NPCID.TheDestroyer)
                    {
                        Item.NewItem(npc.getRect(), ModContent.ItemType<HallowedOre>(), Main.rand.Next(75, 150));
                    }
                    int TS = NPC.CountNPCS(NPCID.Spazmatism);
                    int TR = NPC.CountNPCS(NPCID.Retinazer);
                    if (TS == 0 && TR == 1)
                    {
                        Item.NewItem(npc.getRect(), ModContent.ItemType<HallowedOre>(), Main.rand.Next(75, 150));
                    }
                    if (TS == 1 && TR == 0)
                    {
                        Item.NewItem(npc.getRect(), ModContent.ItemType<HallowedOre>(), Main.rand.Next(75, 150));
                    }
					if (npc.type == NPCID.WallofFlesh)
                    {
                        Item.NewItem(npc.getRect(), 367, Main.rand.Next(1, 1));
                    }
                }
            }
        }
    }
    public class neapolinitebardrop2 : GlobalItem
    {
        public override void OpenVanillaBag(string context, Player player, int arg)
        {
            if (context == "bossBag" && (arg == ItemID.SkeletronPrimeBossBag || arg == ItemID.DestroyerBossBag || arg == ItemID.TwinsBossBag))
            {
			    NPCLoader.blockLoot.Add(ItemID.HallowedBar);
                if (ConfectionWorldGeneration.confectionorHallow)
                {
                    player.QuickSpawnItem(ModContent.ItemType<NeapoliniteOre>(), Main.rand.Next(75, 150));
                }
                else
                {
                    player.QuickSpawnItem(ModContent.ItemType<HallowedOre>(), Main.rand.Next(75, 150));
                }
            }
			if (context == "bossBag" && (arg == ItemID.WallOfFleshBossBag))
            {
			    NPCLoader.blockLoot.Add(ItemID.Pwnhammer);
                if (ConfectionWorldGeneration.confectionorHallow)
                {
                    player.QuickSpawnItem(ModContent.ItemType<GrandSlammer>(), Main.rand.Next(1, 1));
                }
                else
                {
                    player.QuickSpawnItem(367, 1);
                }
            }
        }
    }*/
}